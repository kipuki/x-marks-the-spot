using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class GameControl : MonoBehaviour
{
    public RectTransform finishPanel;

    public TMPro.TextMeshProUGUI finishText;
    public TMPro.TextMeshProUGUI exitText;
    public TMPro.TextMeshProUGUI scoreText;
    public Image hudDisplay;
    private bool completedGame = false;
    PlayerControls controls;

    private Dictionary<ControlSchemeManager.ControlScheme, string> gamepadToButtonHint = new Dictionary<ControlSchemeManager.ControlScheme, string>()
    {
        { ControlSchemeManager.ControlScheme.Unknown, "unknown" },
        { ControlSchemeManager.ControlScheme.KeyboardMouse, "keyboard-ESC" },
        { ControlSchemeManager.ControlScheme.Xbox, "xbox-start" },
        { ControlSchemeManager.ControlScheme.PlayStation, "playstation-start" },
        { ControlSchemeManager.ControlScheme.NintendoSwitch, "xbox-start" },
    };

    private string GetRelevantHintSprite()
    {
        if (gamepadToButtonHint.TryGetValue(ControlSchemeManager.currentControlScheme, out string spriteName))
                return $"<sprite name=\"{spriteName}\">";

        return "Unknown";
    }

    private void updateExitText()
    {
        string hint = GetRelevantHintSprite();
        exitText.text = $"Press {hint} to return to Main Menu";
    }

    public void goToMainMenu()
    {
        ControlSchemeManager.onControlSchemeChanged -= updateExitText;
        Debug.Log("Going to main menu");
        SceneSwitcher.loadScene("mainMenu");
    }

    private void Awake()
    {
        controls = new PlayerControls();
        controls.VictoryScreen.Quit.started += ctx =>
        {
            if (completedGame)
                goToMainMenu();
        };
    }

    public void gameFinish()
    {
        if (completedGame)
            return;

        completedGame = true;
        if (UserSettings.getDifficultyMultiplier() < 1.5f)
            finishText.text = "Great Job! You recovered the treasure. That was a good haul. Try again with higher difficulty!";
        scoreText.text = "Score: " + PlayerController.points;
        hudDisplay.gameObject.SetActive(false);
        finishPanel.gameObject.SetActive(true);
        PlayerController.mainController.getCamera().transform.parent = transform.parent;
        PlayerController.mainController.getPlayer().SetActive(false);
        TextHintHandler.cancelHint();
        controls.Enable();
        ControlSchemeManager.onControlSchemeChanged += updateExitText;
        updateExitText();
    }
}
