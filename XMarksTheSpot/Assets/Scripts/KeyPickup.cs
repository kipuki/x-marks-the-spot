using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;


public class KeyPickup : MonoBehaviour
{
    public void Pickup()
    {
        PlayerController.hasBoatKey = true;
        TextHintHandler.ShowHint(new TextHint("I can use this key to start the boat...", 1, 6));
        Destroy(gameObject);
    }
}
