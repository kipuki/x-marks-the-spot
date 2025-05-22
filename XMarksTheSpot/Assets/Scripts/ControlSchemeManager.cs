using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.Events;

public class ControlSchemeManager : MonoBehaviour
{
    private PlayerInput playerInput;
    public ControlScheme controlScheme { get; private set; } = ControlScheme.Unknown;
    private ControlScheme lastControlScheme = ControlScheme.Unknown;
    private Gamepad currentGamepad;
    private static ControlSchemeManager Instance;

    // Dictionary to map gamepad types to control schemes
    private static readonly List<(string keyword, ControlScheme layout)> gamepadTypeKeywords = new()
    {
        ("xbox", ControlScheme.Xbox),
        ("playstation", ControlScheme.PlayStation),
        ("dualshock", ControlScheme.PlayStation),
        ("dualsense", ControlScheme.PlayStation),
        ("switch", ControlScheme.NintendoSwitch)
    };

    public UnityEvent onControlChangedEvent;
    public static ControlScheme currentControlScheme =>
        Instance != null ? Instance.controlScheme : ControlScheme.Unknown;
    public static event Action onControlSchemeChanged;

    public enum ControlScheme
    {
        Unknown = 0,
        KeyboardMouse,
        Xbox,
        PlayStation,
        NintendoSwitch
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if (Instance != null && Instance.enabled) // Enabled check allows instances across scenes
        {
            Debug.LogError("Multiple active instances of ControlSchemeManager detected. Please only use one instance.");
            enabled = false;
            return;
        }

        playerInput = GetComponent<PlayerInput>();
        if (playerInput == null)
        {
            Debug.LogError("PlayerInput component not found. Please attach it to the GameObject.");
            return;
        }

        Instance = this;
    }

    void Update()
    {
        // Added as a measure for gamepads that dont fire the OnControlsChanged event
        if (currentGamepad == null)
            return;

        if (currentGamepad != Gamepad.current)
            UpdateControlScheme();
    }

    void OnEnable()
    {
        // To prevent PlayerInput's OnEnable from being called before this script's Awake
        if (playerInput == null)
            return;

        UpdateControlScheme();
    }

    void OnDestroy()
    {
        Instance = null;
    }

    private void UpdateControlScheme()
    {
        lastControlScheme = controlScheme;

        bool isGamepad = false;
        switch (playerInput?.currentControlScheme)
        {
            case "Keyboard&Mouse":
                controlScheme = ControlScheme.KeyboardMouse;
                break;

            case "Gamepad":
                isGamepad = true;
                controlScheme = getGamepadControlScheme();
                break;

            default:
                controlScheme = ControlScheme.Unknown;
                break;
        }

        if (!isGamepad)
            currentGamepad = null;

        // If the control scheme is identical to the last one, do not invoke the event. (i.e. DualShock -> DualSense)
        if (lastControlScheme == controlScheme)
            return;

        Debug.Log($"Control scheme changed from {lastControlScheme} to {controlScheme}");

        onControlChangedEvent.Invoke();
        onControlSchemeChanged?.Invoke();
    }

    private ControlScheme getGamepadControlScheme()
    {
        Gamepad gamepad = Gamepad.current;
        if (gamepad == null)
        {
            currentGamepad = null;
            return ControlScheme.Unknown;
        }

        // If the gamepad is identical, return the current control scheme.
        if (gamepad == currentGamepad)
            return controlScheme;

        string name = gamepad.displayName.ToLower();
        currentGamepad = gamepad;

        foreach ((string keyword, ControlScheme layout) in gamepadTypeKeywords)
            if (name.Contains(keyword))
                return layout;

        return ControlScheme.Xbox; // Fallback to Xbox
    }

    private void OnControlsChanged(PlayerInput input)
    {
        UpdateControlScheme();
    }
}
