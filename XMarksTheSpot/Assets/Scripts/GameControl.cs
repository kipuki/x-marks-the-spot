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
    public TMPro.TextMeshProUGUI scoreText;
    public Image hudDisplay;
    private bool completedGame = false;
    PlayerControls controls;

    public void goToMainMenu()
    {
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
    }
}
