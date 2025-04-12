using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractTrigger : MonoBehaviour
{
    public string interactionMessage;
    public bool disableAfterUse;
    public string interactButton = "Interact";
    private bool active = false;
    public bool hasToFaceObject = false;
    // private bool isInRange = false;

    public UnityEvent onInteract;

    // private Coroutine facingCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        // facingCoroutine = checkIfFacing();        
    }

    // Update is called once per frame
    void Update()
    {
        if (active && Input.GetButtonDown(interactButton))
            action();

    }

    void action()
    {

        onInteract.Invoke();
        if (disableAfterUse)
        {
            // if (facingCoroutine != null)
            //     StopCoroutine(facingCoroutine);
            this.enabled = false;
            disable();
        }
    }

    void enable()
    {
        InteractDisplay.enableInteract(interactionMessage);
        active = true;
    }

    void disable()
    {
        InteractDisplay.disableInteract();
        active = false;
    }


    void OnTriggerEnter(Collider col)
    {
        if (this.enabled && col.gameObject.tag == "Player")
        {
            // isInRange = true;
            // if (hasToFaceObject)
            // {
            //     if (facingCoroutine != null)
            //         StopCoroutine(facingCoroutine);
            //     facingCoroutine = StartCoroutine(checkIfFacing());
            // } else
            //     enable();
            if (!hasToFaceObject)
                enable();
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "Player")
            disable();
    }

    void OnTriggerStay(Collider col)
    {
        if (this.enabled && hasToFaceObject && col.gameObject.tag == "Player")
        {
            if (PlayerController.mainController.checkIfFacing(gameObject))
                enable();
            else
                disable();
        }
    }

    // Not needed anymore due to OnTriggerStay
    // IEnumerator checkIfFacing()
    // {
    //     // yield return null;
    //     while (isInRange && this.enabled)
    //     {
    //         if (PlayerController.mainController.checkIfFacing(gameObject))
    //             enable();
    //         else
    //             disable();
    //         yield return new WaitForEndOfFrame();
    //     }
    //     facingCoroutine = null;
    // }
}
