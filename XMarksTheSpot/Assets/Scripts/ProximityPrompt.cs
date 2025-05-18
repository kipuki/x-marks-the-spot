using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class ProximityPrompt : MonoBehaviour
{
    private PlayerControls controls;
    public string interactionMessage;
    public bool disableAfterUse;
    public bool isShowingMessage = false;
    private InputAction interactAction;
    public bool hasToFaceObject = false;
    public UnityEvent onInteract;
    private System.Action<InputAction.CallbackContext> m_OnPromptTriggered;

    void Awake()
    {
        controls = new PlayerControls();
        m_OnPromptTriggered = ctx => action();
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    void OnDisable()
    {
        hideMessage();
        controls.Disable();
    }

    void action()
    {
        onInteract.Invoke();

        if (!disableAfterUse)
            return;

        hideMessage();
        this.enabled = false;
    }

    void showMessage()
    {
        if (isShowingMessage)
            return;

        InteractDisplay.enableInteract(interactionMessage);
        isShowingMessage = true;

        controls.Player.Interact.started += m_OnPromptTriggered;
    }

    void hideMessage()
    {
        if (!isShowingMessage)
            return;

        InteractDisplay.disableInteract();
        isShowingMessage = false;

        controls.Player.Interact.started -= m_OnPromptTriggered;
    }


    void OnTriggerEnter(Collider col)
    {
        if (this.enabled && col.gameObject.tag == "Player" && !hasToFaceObject)
            showMessage();
    }

    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "Player")
            hideMessage();
    }

    void OnTriggerStay(Collider col)
    {
        if (this.enabled && hasToFaceObject && col.gameObject.tag == "Player")
        {
            if (PlayerController.mainController.checkIfFacing(gameObject))
                showMessage();
            else
                hideMessage();
        }
    }
}
