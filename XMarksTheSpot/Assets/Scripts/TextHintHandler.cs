using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Modified version of TextHints
// This version takes advantage of priorities and duration as a feature of text hints
public class TextHintHandler : MonoBehaviour {

    // float timer = 0.0f;
    private static TextHint currentHint;
    private static IEnumerator currentCoroutine;


    public static TextHintHandler mainTextHintHandler;
    private static TMPro.TextMeshProUGUI textDisplay;


	// Use this for initialization
	void Awake () {
        textDisplay = gameObject.GetComponent<TMPro.TextMeshProUGUI>();
        mainTextHintHandler = this;
	}
	
	void Update () {
        // As taught in lectures, best to use coroutine! :)
        // if (gameObject.GetComponent<Text>().enabled)
        // {
        //     timer += Time.deltaTime;
        //     if(timer >= 4)
        //     {
        //         gameObject.GetComponent<Text>().enabled = false;
        //         timer = 0.0f;
        //     }
        // }
	}

    public void setHint(string message)
    {
        TextHintHandler.showHint(message);
    }

    public void setHint(TextHint hintData)
    {
        TextHintHandler.showHint(hintData);
    }

    private static void waitForInitialization()
    {
        while (mainTextHintHandler == null);
    }

    public static void showHint(string message)
    {
        showHint(new TextHint(message));
    }

    public static void showHint(TextHint hintData)
    {
        waitForInitialization();
        if (currentHint == null || hintData.getPriority() >= currentHint.getPriority())
        {
            cancelCoroutine();
            currentCoroutine = playHint(hintData);
            mainTextHintHandler.StartCoroutine(currentCoroutine);
        }
    }

    public static void cancelHint()
    {
        waitForInitialization();
        if (textDisplay.enabled)
            textDisplay.enabled = false;        
    }

    static private void cancelCoroutine()
    {
        waitForInitialization();
        if (currentCoroutine != null)
        {
            mainTextHintHandler.StopCoroutine(currentCoroutine);
            currentCoroutine = null;
        }
        currentHint = null;
    }

    static private IEnumerator playHint(TextHint hintData)
    {
        currentHint = hintData;
        textDisplay.text = hintData.getMessage();
        textDisplay.enabled = true;
        yield return new WaitForSeconds(hintData.getDuration());
        textDisplay.enabled = false;
        cancelCoroutine();
    }
}


public class TextHint {
        private string message;
        private int priority;
        private float duration;


        public TextHint(string message, int priority, float duration)
        {
            this.message = message;
            this.priority = priority;
            this.duration = duration;
        }

        public TextHint(string message, int priority) : this(message, priority, 4) {}

        public TextHint(string message) : this(message, 1, 4) {}
        
        public string getMessage()
        {
            return message;
        }

        public int getPriority()
        {
            return priority;
        }

        public float getDuration()
        {
            return duration;
        }
    }