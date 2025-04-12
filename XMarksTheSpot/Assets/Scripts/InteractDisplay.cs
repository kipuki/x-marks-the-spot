using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractDisplay : MonoBehaviour
{
    private static TMPro.TextMeshProUGUI textDisplay;


	// Use this for initialization
	void Awake () {
        textDisplay = gameObject.GetComponent<TMPro.TextMeshProUGUI>();
	}

    // Update is called once per frame
    void Update()
    {
        
    }

    private static void waitForInitialization()
    {
        while (textDisplay == null);
    }

    public static void disableInteract()
    {
        textDisplay.enabled = false;
    }

    public static void enableInteract(string action)
    {
        textDisplay.text = "Press <b>E</b> to "+action;
        textDisplay.enabled = true;
    }

}
