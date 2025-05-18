using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace UnityStandardAssets.Characters.FirstPerson
{
    [Serializable]
    public class MouseLook
    {
        public Vector2 sensitivity = new Vector2(0.1f, 0.1f);
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

            m_OnLookPerformed = ctx => LookRotation(ctx.ReadValue<Vector2>());
            m_OnLookCanceled = ctx => LookRotation(ctx.ReadValue<Vector2>());
        }

        private void ConnectEvents()
        {
            if (m_PlayerControls == null)
                return;
            m_PlayerControls.Player.Look.performed += m_OnLookPerformed;
            m_PlayerControls.Player.Look.canceled += m_OnLookCanceled;
        }

        private void DisconnectEvents()
        {
            if (m_PlayerControls == null)
                return;
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


        public void LookRotation(Vector2 lookDelta)
        {
            lookDelta *= sensitivity;

            float yRot = lookDelta.x;
            float xRot = lookDelta.y;

            m_CharacterTargetRot *= Quaternion.Euler(0f, yRot, 0f);
            m_CameraTargetRot *= Quaternion.Euler(-xRot, 0f, 0f);

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
            if(Input.GetKeyUp(KeyCode.Escape))
            {
                m_cursorIsLocked = false;
            }
            else if(Input.GetMouseButtonUp(0))
            {
                m_cursorIsLocked = true;
            }

            if (m_cursorIsLocked)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            else if (!m_cursorIsLocked)
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
}
