using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameControl : MonoBehaviour
{




    public RectTransform finishPanel;

    public TMPro.TextMeshProUGUI finishText;
    public TMPro.TextMeshProUGUI scoreText;
    public Image hudDisplay;
    private bool gameOver = false;

    public void Update()
    {
        if (gameOver && Input.GetButtonUp("Cancel"))
            goToMainMenu();
    }

    public void goToMainMenu()
    {
        Debug.Log("Going to main menu");
        SceneSwitcher.loadScene("mainMenu");
    }

    public void gameFinish()
    {
        gameOver = true;
        if (UserSettings.getDifficultyMultiplier() < 1.5f)
            finishText.text = "Great Job! You recovered the treasure. That was a good haul. Try again with higher difficulty!";
        scoreText.text = "Score: "+PlayerController.points;
        hudDisplay.gameObject.SetActive(false);
        finishPanel.gameObject.SetActive(true);
        PlayerController.mainController.getCamera().transform.parent = transform.parent;
        PlayerController.mainController.getPlayer().SetActive(false);
        TextHintHandler.cancelHint();
        // Time.timeScale = 0.1f;
    }
}
