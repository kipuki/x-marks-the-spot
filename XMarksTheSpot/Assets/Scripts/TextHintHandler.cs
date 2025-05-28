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

    public void SetHint(string message)
    {
        TextHintHandler.ShowHint(message);
    }

    public void SetHint(TextHint hintData)
    {
        TextHintHandler.ShowHint(hintData);
    }

    public static void ShowHint(string message)
    {
        ShowHint(new TextHint(message));
    }

    public static void ShowHint(TextHint hintData)
    {
        if (mainTextHintHandler == null)
            return;

        if (currentHint == null || hintData.GetPriority() >= currentHint.GetPriority())
        {
            CancelCoroutine();
            currentCoroutine = PlayHint(hintData);
            mainTextHintHandler.StartCoroutine(currentCoroutine);
        }
    }

    public static void CancelHint()
    {
        if (mainTextHintHandler == null)
            return;
        
        if (textDisplay.enabled)
            textDisplay.enabled = false;        
    }

    static private void CancelCoroutine()
    {
        if (mainTextHintHandler == null)
            return;
        
        if (currentCoroutine != null)
        {
            mainTextHintHandler.StopCoroutine(currentCoroutine);
            currentCoroutine = null;
        }
        currentHint = null;
    }

    static private IEnumerator PlayHint(TextHint hintData)
    {
        currentHint = hintData;
        textDisplay.text = hintData.GetMessage();
        textDisplay.enabled = true;

        float? duration = hintData.GetDuration();
        if (duration != null)
        {
            yield return new WaitForSeconds(duration.Value);
            textDisplay.enabled = false;
            CancelCoroutine();
        }
        
        yield return null;
    }
}


public class TextHint {
        private string message;
        private int priority;
        private float? duration;


        public TextHint(string message, int priority, float? duration)
        {
            this.message = message;
            this.priority = priority;
            this.duration = duration;
        }

        public TextHint(string message, int priority) : this(message, priority, 4) {}

        public TextHint(string message) : this(message, 1, 4) {}
        
        public string GetMessage()
        {
            return message;
        }

        public int GetPriority()
        {
            return priority;
        }

        public float? GetDuration()
        {
            return duration;
        }
    }