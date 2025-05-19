using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.Events;

public class ActiveDeviceManager : MonoBehaviour
{
    private PlayerInput playerInput;
    public DeviceType controlScheme { get; private set; } = DeviceType.Unknown;
    public GamepadLayout gamepadLayout { get; private set; } = GamepadLayout.Unknown;
    public static ActiveDeviceManager Instance { get; private set; }
    public UnityEvent onDeviceChangedEvent;
    private static readonly List<Action> pendingSubscribers = new();

    public static DeviceType currentControlScheme =>
        Instance != null ? Instance.controlScheme : DeviceType.Unknown;
    public static GamepadLayout currentGamepadLayout =>
        Instance != null ? Instance.gamepadLayout : GamepadLayout.Unknown;
    public event Action onDeviceChanged;
    public static event Action onDeviceChangedStatic
    {
        add
        {
            if (Instance != null)
                Instance.onDeviceChanged += value;
            else
                pendingSubscribers.Add(value);
        }
        remove
        {
            if (Instance != null)
                Instance.onDeviceChanged -= value;
        }
    }


    public enum DeviceType
    {
        Unknown = 0,
        Keyboard,
        Gamepad
    }

    public enum GamepadLayout
    {
        Unknown = 0,
        Xbox,
        PlayStation,
        NintendoSwitch,
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        Instance = this;

        foreach (var sub in pendingSubscribers)
            onDeviceChanged += sub;
    }

    private void UpdateControlScheme()
    {
        DeviceType lastControlScheme = controlScheme;

        switch (playerInput?.currentControlScheme)
        {
            case "Keyboard&Mouse":
                controlScheme = DeviceType.Keyboard;
                break;

            case "Gamepad":
                controlScheme = DeviceType.Gamepad;
                break;

            default:
                controlScheme = DeviceType.Unknown;
                break;
        }

        if (lastControlScheme != controlScheme)
        {
            onDeviceChangedEvent?.Invoke();
            onDeviceChanged?.Invoke();

            gamepadLayout = GetControllerType();
        }
    }

    public static GamepadLayout GetControllerType()
    {
        Gamepad gamepad = Gamepad.current;
        if (gamepad == null)
            return GamepadLayout.Unknown;

        string name = gamepad.displayName.ToLower();

        if (name.Contains("xbox"))
            return GamepadLayout.Xbox;

        if (name.Contains("playstation") || name.Contains("dualshock") || name.Contains("dualsense"))
            return GamepadLayout.PlayStation;

        if (name.Contains("switch"))
            return GamepadLayout.NintendoSwitch;
        
        return GamepadLayout.Xbox; // Default to Xbox
    }

    private void OnControlsChanged(PlayerInput input)
    {
        UpdateControlScheme();
    }

    void OnEnable()
    {
        UpdateControlScheme();
        playerInput.onControlsChanged += OnControlsChanged;
    }

    void OnDisable()
    {
        playerInput.onControlsChanged -= OnControlsChanged;
    }
}
