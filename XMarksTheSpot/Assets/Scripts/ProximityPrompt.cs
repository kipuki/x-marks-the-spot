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
        m_OnPromptTriggered = ctx => Action();
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    void OnDisable()
    {
        HideMessage();
        controls.Disable();
    }

    void Action()
    {
        onInteract.Invoke();

        if (!disableAfterUse)
            return;

        HideMessage();
        this.enabled = false;
    }

    void ShowMessage()
    {
        if (isShowingMessage)
            return;

        InteractDisplay.EnableInteract(interactionMessage);
        isShowingMessage = true;

        controls.Player.Interact.started += m_OnPromptTriggered;
    }

    void HideMessage()
    {
        if (!isShowingMessage)
            return;

        InteractDisplay.DisableInteract();
        isShowingMessage = false;

        controls.Player.Interact.started -= m_OnPromptTriggered;
    }


    void OnTriggerEnter(Collider col)
    {
        if (this.enabled && col.gameObject.tag == "Player" && !hasToFaceObject)
            ShowMessage();
    }

    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "Player")
            HideMessage();
    }

    void OnTriggerStay(Collider col)
    {
        if (this.enabled && hasToFaceObject && col.gameObject.tag == "Player")
        {
            if (PlayerController.mainController.CheckIfFacing(gameObject))
                ShowMessage();
            else
                HideMessage();
        }
    }
}
