using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent (typeof(CapsuleCollider))]
[RequireComponent (typeof(AudioSource))]
public class CanonControl : MonoBehaviour
{
    private PlayerControls inputController;
    public Camera canonCamera;
    private Camera playerCamera;
    public Rigidbody cannonballPrefab;

    public Rigidbody horizontalRigid;

    public Rigidbody verticalRigid;

    public SpriteRenderer crosshair;

    public GameObject launcher;

    public float cannonBallVelocity;

    private float initialX;

    private int rotationSpeed = 60;

    private PlayerController occupyingPlayer;

    private bool canFire = false;

    private bool shouldFireCannon = false;
    private bool shouldRotate = false;

    public Vector3 checkpointSpawnPosition;

    private Vector2 rotationInput;

    private Dictionary<ControlSchemeManager.ControlScheme, (string, string)> controlRotateToButtonHint = new Dictionary<ControlSchemeManager.ControlScheme, (string, string)>()
    {
        { ControlSchemeManager.ControlScheme.Unknown, ("unknown", "unknown") },
        { ControlSchemeManager.ControlScheme.KeyboardMouse, ("keyboard-wasd", "keyboard-arrow-keys") },
        { ControlSchemeManager.ControlScheme.Xbox, ("xbox-left-stick", "xbox-dpad") },
        { ControlSchemeManager.ControlScheme.PlayStation, ("playstation-left-stick", "playstation-dpad") },
        { ControlSchemeManager.ControlScheme.NintendoSwitch, ("xbox-left-stick", "xbox-dpad") },
    };


    private Dictionary<ControlSchemeManager.ControlScheme, string> controlFireToButtonHint = new Dictionary<ControlSchemeManager.ControlScheme, string>()
    {
        { ControlSchemeManager.ControlScheme.Unknown, "unknown" },
        { ControlSchemeManager.ControlScheme.KeyboardMouse, "keyboard-Space" },
        { ControlSchemeManager.ControlScheme.Xbox, "gamepad-a-colored" },
        { ControlSchemeManager.ControlScheme.PlayStation, "gamepad-cross-colored" },
        { ControlSchemeManager.ControlScheme.NintendoSwitch, "gamepad-b-colored" },
    };

    private string GetRotateHintSprite()
    {
        if (controlRotateToButtonHint.TryGetValue(ControlSchemeManager.currentControlScheme, out (string spriteName1, string spriteName2) hintTuple))
        {
            (string spriteName1, string spriteName2) = hintTuple;
            return $"<sprite name=\"{spriteName1}\"> <sprite name=\"{spriteName2}\">";
        }

        return "Unknown";
    }

    private string GetFireHintSprite()
    {
        if (controlFireToButtonHint.TryGetValue(ControlSchemeManager.currentControlScheme, out string hint))
            return $"<sprite name=\"{hint}\">";

        return "Unknown";
    }

    void Awake()
    {
        inputController = new PlayerControls();

        inputController.Cannon.Rotate.performed += ctx => shouldRotate = true;
        inputController.Cannon.Rotate.canceled += ctx => shouldRotate = false;

        inputController.Cannon.Fire.performed += ctx =>
        {
            if (canFire)
                shouldFireCannon = true;
        };
    }

    private void OnEnable()
    {
        inputController.Enable();
    }

    void OnDisable()
    {
        inputController.Disable();
    }

    // Start is called before the first frame update
    void Start()
    {
        canonCamera.enabled = false;
        if (verticalRigid)
            initialX = verticalRigid.transform.eulerAngles.x;
    }

    void FixedUpdate()
    {
        if (occupyingPlayer == null)
            return;

        rotationInput = inputController.Cannon.Rotate.ReadValue<Vector2>();

        if (shouldRotate)
        {
            if (rotationInput.x != 0)
                horizontalRigid.transform.Rotate(0, rotationInput.x * rotationSpeed * Time.deltaTime, 0, Space.World);
            
            if (verticalRigid && rotationInput.y != 0)
            {
                float angleX = verticalRigid.transform.eulerAngles.x;
                float verticalInputAxis = rotationInput.y;
                if (angleX > 180)
                    angleX -= 360;

                float newRotationX = (verticalInputAxis * rotationSpeed * Time.deltaTime) + angleX;

                if (newRotationX <= 0 && newRotationX >= -40)
                    newRotationX -= angleX;
                else if ((newRotationX >= 0 && verticalInputAxis > 0) || (newRotationX <= -40 && verticalInputAxis < 0))
                    newRotationX = 0;

                if (newRotationX != 0)
                    verticalRigid.transform.Rotate(newRotationX, 0, 0, Space.Self);
            }
        }

        if (shouldFireCannon)
        {
            StartCoroutine("fireCannon");
            shouldFireCannon = false;
        }
    }

    public void setRotationSpeed(int newRotationSpeed)
    {
        rotationSpeed = newRotationSpeed;
    }

    public void exitCannon()
    {
        ControlSchemeManager.onControlSchemeChanged -= showInstructionText;
        TextHintHandler.showHint(new TextHint("What a bang! That seems to have done it. Into the tunnel I go!", 1, 8));
        StartCoroutine(exitCannon(6));
    }

    IEnumerator exitCannon(float delay)
    {
        yield return new WaitForSeconds(delay);
        crosshair.enabled = false;
        canonCamera.enabled = false;
        
        if (playerCamera != null)
            playerCamera.enabled = true;

        if (verticalRigid)
        {
            Vector3 euler = verticalRigid.transform.eulerAngles;
            verticalRigid.transform.eulerAngles = new Vector3(initialX, euler.y, euler.z);
        }
        // Enable character controller.
        if (occupyingPlayer != null)
        {
            occupyingPlayer.transform.parent = null;
            occupyingPlayer.enableCharacter();
            occupyingPlayer.SendMessage("setSpawnPosition", checkpointSpawnPosition);
            occupyingPlayer = null;
        }
    }

    private void showInstructionText()
    {
        string rotateHint = GetRotateHintSprite();
        string fireHint = GetFireHintSprite();

        TextHintHandler.showHint(new TextHint($"Rotate the cannon with {rotateHint} then press {fireHint} to fire", 1, null));
    }

    public void controlCanon()
    {
        // Set occupying player
        occupyingPlayer = PlayerController.mainController;
        canFire = true;

        gameObject.GetComponent<CapsuleCollider>().enabled = false;
        canonCamera.enabled = true;

        playerCamera = occupyingPlayer.getCamera();
        playerCamera.enabled = false;

        // Disable character controller.
        occupyingPlayer.disableCharacter();

        crosshair.enabled = true;
        occupyingPlayer.transform.parent = gameObject.transform;

        ControlSchemeManager.onControlSchemeChanged += showInstructionText;
        showInstructionText();
    }

    // Helps automatically align the cannon to face a wall. (Used for camera shots)
    public void FaceObject(Transform wallTransform)
    {
        if (wallTransform == null || horizontalRigid == null || verticalRigid == null)
            return;

        Vector3 cannonPos = horizontalRigid.transform.position;
        Vector3 targetPos = wallTransform.position;

        Vector3 dirToWall = targetPos - cannonPos;
        dirToWall.y = 0;
        if (dirToWall.sqrMagnitude < 0.0001f)
            return;

        Quaternion targetYaw = Quaternion.LookRotation(dirToWall, Vector3.up);
        horizontalRigid.transform.rotation = targetYaw;

        float horizontalDistance = dirToWall.magnitude;
        float heightDifference = targetPos.y - cannonPos.y;

        float gravity = Physics.gravity.magnitude;
        float pitchDeg = CalculatePitch(cannonBallVelocity, gravity, horizontalDistance, heightDifference);

        Vector3 currentEuler = verticalRigid.transform.localEulerAngles;
        verticalRigid.transform.localRotation = Quaternion.Euler(pitchDeg, currentEuler.y, currentEuler.z);
    }

    private float CalculatePitch(float initialSpeed, float gravity, float horizontalDistance, float heightDifference)
    {
        float speedSquared = initialSpeed * initialSpeed;
        float discriminant = speedSquared * speedSquared - gravity * (gravity * horizontalDistance * horizontalDistance + 2 * heightDifference * speedSquared);

        if (discriminant < 0)
            return -40f; // no valid solution, use max downward pitch

        float sqrtDiscriminant = Mathf.Sqrt(discriminant);
        float angle1 = Mathf.Atan((speedSquared + sqrtDiscriminant) / (gravity * horizontalDistance));
        float angle2 = Mathf.Atan((speedSquared - sqrtDiscriminant) / (gravity * horizontalDistance));

        float pitchRad = Mathf.Min(angle1, angle2);
        float pitchDeg = pitchRad * Mathf.Rad2Deg;

        return Mathf.Clamp(-pitchDeg, -40f, 0f);
    }


    public void SpawnCannonball(Vector3 spawnPosition, Quaternion spawnRotation, Vector3 linearVelocity)
    {
        Rigidbody cannonball = Instantiate(cannonballPrefab, spawnPosition, spawnRotation) as Rigidbody;
        cannonball.name = "cannonball";
        cannonball.useGravity = true;
        cannonball.linearVelocity = linearVelocity;
        // Disable collision with FPS controller because FPS controller could be in the way.
        if (occupyingPlayer != null)
            Physics.IgnoreCollision(occupyingPlayer.GetComponent<Collider>(), cannonball.GetComponent<Collider>(), true);
        Physics.IgnoreCollision(gameObject.GetComponent<BoxCollider>(), cannonball.GetComponent<Collider>(), true);
        launcher.GetComponent<ParticleSystem>().Play();
    }

    public void FireCannonball()
    {
        gameObject.GetComponent<AudioSource>().Play();
        SpawnCannonball(launcher.transform.position, launcher.transform.rotation, launcher.transform.forward * cannonBallVelocity);
    }

    IEnumerator fireCannon()
    {
        if (canFire)
        {
            // Disable cannon
            canFire = false;
            FireCannonball();
            // Re-enable cannon in 3 seconds
            yield return new WaitForSeconds(3);
            canFire = true;
        }
    }

}
