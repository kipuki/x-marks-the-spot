using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractDisplay : MonoBehaviour
{
    private static TMPro.TextMeshProUGUI textDisplay;
    private static string currentAction = "";

    private static Dictionary<ControlSchemeManager.ControlScheme, string> gamepadToButtonHint = new Dictionary<ControlSchemeManager.ControlScheme, string>()
    {
        { ControlSchemeManager.ControlScheme.Unknown, "unknown" },
        { ControlSchemeManager.ControlScheme.KeyboardMouse, "keyboard-E" },
        { ControlSchemeManager.ControlScheme.Xbox, "gamepad-x-colored" },
        { ControlSchemeManager.ControlScheme.PlayStation, "gamepad-square-colored" },
        { ControlSchemeManager.ControlScheme.NintendoSwitch, "gamepad-y-colored" },
    };


    private static string GetRelevantHintSprite()
    {
        if (gamepadToButtonHint.TryGetValue(ControlSchemeManager.currentControlScheme, out string spriteName))
            return $"<sprite name=\"{spriteName}\">";

        return "Unknown";
    }

    // Use this for initialization
    void Awake () {
        textDisplay = gameObject.GetComponent<TMPro.TextMeshProUGUI>();
	}

    public static void disableInteract()
    {
        textDisplay.enabled = false;
        ControlSchemeManager.onControlSchemeChanged -= updateText;
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
        ControlSchemeManager.onControlSchemeChanged += updateText;
    }

}
