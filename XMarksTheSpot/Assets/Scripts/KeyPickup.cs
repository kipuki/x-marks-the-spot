using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;


public class KeyPickup : MonoBehaviour
{

    // public TMPro.TextMeshProUGUI textHints;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void keyPickup()
    {
        PlayerController.hasBoatKey = true;
        // TextHint hint = new TextHint("I can use this key to start the boat...", 1, 6);
        // textHints.SendMessage("showHint", hint);
        TextHintHandler.showHint(new TextHint("I can use this key to start the boat...", 1, 6));
        Destroy(gameObject);
    }

    // void OnTriggerEnter(Collider col)
    // {
    //     if (col.gameObject.tag == "Player") {
    //         PlayerController.hasBoatKey = true;
    //         // TextHint hint = new TextHint("I can use this key to start the boat...", 1, 6);
    //         // textHints.SendMessage("showHint", hint);
    //         TextHintHandler.showHint(new TextHint("I can use this key to start the boat...", 1, 6));
    //         Destroy(gameObject);
    //     }
    // }
}
