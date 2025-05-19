using System;
using UnityEngine;
using UnityEngine.InputSystem;

[Serializable]
public class PlayerLook
{
    public bool clampVerticalRotation = true;
    public float MinimumX = -90F;
    public float MaximumX = 90F;
    public bool smooth;
    public float smoothTime = 5f;
    public bool lockCursor = true;

    private Transform m_CharacterTransform;
    private Transform m_CameraTransform;
    private Quaternion m_CharacterTargetRot;
    private Quaternion m_CameraTargetRot;
    private PlayerControls m_PlayerControls;
    private bool m_cursorIsLocked = true;
    private bool shouldRotate = false;
    private float yRot = 0;
    private float xRot = 0;
    private InputAction lookUnlockAction;
    private InputAction lookLockAction;

    private System.Action<InputAction.CallbackContext> m_OnLookPerformed;
    private System.Action<InputAction.CallbackContext> m_OnLookCanceled;

    public void Init(PlayerControls controls, Transform character, Transform camera)
    {
        DisconnectEvents();
        m_PlayerControls = controls;
        m_CharacterTransform = character;
        m_CameraTransform = camera;
        m_CharacterTargetRot = character.localRotation;
        m_CameraTargetRot = camera.localRotation;

        lookUnlockAction = m_PlayerControls.Player.UnlockCursor;
        lookLockAction = m_PlayerControls.Player.LockCursor;

        m_OnLookPerformed = ctx => shouldRotate = true;
        m_OnLookCanceled = ctx => shouldRotate = false;
    }

    private void ConnectEvents()
    {
        if (m_PlayerControls == null)
            return;

        m_PlayerControls.Player.Look.performed += m_OnLookPerformed;
        m_PlayerControls.Player.Look.canceled += m_OnLookCanceled;

        lookUnlockAction.performed += ctx => m_cursorIsLocked = false;
        lookLockAction.performed += ctx => m_cursorIsLocked = true;
    }

    private void DisconnectEvents()
    {
        if (m_PlayerControls == null)
            return;

        lookUnlockAction.performed -= ctx => m_cursorIsLocked = false;
        lookLockAction.performed -= ctx => m_cursorIsLocked = true;

        m_PlayerControls.Player.Look.performed -= m_OnLookPerformed;
        m_PlayerControls.Player.Look.canceled -= m_OnLookCanceled;
    }

    public void Enable()
    {
        ConnectEvents();
    }

    public void Disable()
    {
        DisconnectEvents();
    }

    public void Update()
    {
        if (m_PlayerControls == null || !shouldRotate)
            return;

        LookRotation(m_PlayerControls.Player.Look.ReadValue<Vector2>());
    }

    public void LookRotation(Vector2 lookDelta)
    {
        yRot = lookDelta.x;
        xRot = lookDelta.y;

        m_CharacterTargetRot = m_CharacterTransform.localRotation * Quaternion.Euler(0f, yRot, 0f);
        m_CameraTargetRot = m_CameraTransform.localRotation * Quaternion.Euler(-xRot, 0f, 0f);

        if (clampVerticalRotation)
            m_CameraTargetRot = ClampRotationAroundXAxis(m_CameraTargetRot);

        if (smooth)
        {
            m_CharacterTransform.localRotation = Quaternion.Slerp(m_CharacterTransform.localRotation, m_CharacterTargetRot,
                smoothTime * Time.deltaTime);
            m_CameraTransform.localRotation = Quaternion.Slerp(m_CameraTransform.localRotation, m_CameraTargetRot,
                smoothTime * Time.deltaTime);
        }
        else
        {
            m_CharacterTransform.localRotation = m_CharacterTargetRot;
            m_CameraTransform.localRotation = m_CameraTargetRot;
        }

        UpdateCursorLock();
    }

    public void SetCursorLock(bool value)
    {
        lockCursor = value;
        if(!lockCursor)
        {//we force unlock the cursor if the user disable the cursor locking helper
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void UpdateCursorLock()
    {
        //if the user set "lockCursor" we check & properly lock the cursos
        if (lockCursor)
            InternalLockUpdate();
    }

    private void InternalLockUpdate()
    {
        if (m_cursorIsLocked)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    Quaternion ClampRotationAroundXAxis(Quaternion q)
    {
        q.x /= q.w;
        q.y /= q.w;
        q.z /= q.w;
        q.w = 1.0f;

        float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan (q.x);

        angleX = Mathf.Clamp (angleX, MinimumX, MaximumX);

        q.x = Mathf.Tan (0.5f * Mathf.Deg2Rad * angleX);

        return q;
    }

}