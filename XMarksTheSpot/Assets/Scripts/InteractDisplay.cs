using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractDisplay : MonoBehaviour
{
    private static TMPro.TextMeshProUGUI textDisplay;
    private static string currentAction = "";

    private static Dictionary<ActiveDeviceManager.GamepadLayout, string> gamepadToButtonHint = new Dictionary<ActiveDeviceManager.GamepadLayout, string>()
    {
        { ActiveDeviceManager.GamepadLayout.Xbox, "<sprite name=\"gamepad-x-colored\">" },
        { ActiveDeviceManager.GamepadLayout.PlayStation, "<sprite name=\"gamepad-square-colored\">" },
        { ActiveDeviceManager.GamepadLayout.NintendoSwitch, "<sprite name=\"gamepad-y-colored\">" },
    };

    private static string GetRelevantHintSprite()
    {
        if (ActiveDeviceManager.currentControlScheme == ActiveDeviceManager.DeviceType.Keyboard)
            return "<sprite name=\"keyboard-E\">";
        

        if (gamepadToButtonHint.TryGetValue(ActiveDeviceManager.currentGamepadLayout, out string hint))
                return hint;

        return "Unknown";
    }

    // Use this for initialization
    void Awake () {
        textDisplay = gameObject.GetComponent<TMPro.TextMeshProUGUI>();
	}

    public static void disableInteract()
    {
        textDisplay.enabled = false;
        ActiveDeviceManager.onDeviceChangedStatic -= updateText;
    }

    private static void updateText()
    {
        string hint = GetRelevantHintSprite();
        textDisplay.text = $"Press {hint} to {currentAction}";
    }

    public static void enableInteract(string action)
    {
        currentAction = action;
        updateText();
        textDisplay.enabled = true;
        ActiveDeviceManager.onDeviceChangedStatic += updateText;
    }

}
