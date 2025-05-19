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
        StartCoroutine(exitCannon(6));
    }

    IEnumerator exitCannon(float delay)
    {
        yield return new WaitForSeconds(delay);
        crosshair.enabled = false;
        canonCamera.enabled = false;
        playerCamera.enabled = true;

        if (verticalRigid)
        {
            Vector3 euler = verticalRigid.transform.eulerAngles;
            verticalRigid.transform.eulerAngles = new Vector3(initialX, euler.y, euler.z);
        }
        // Enable character controller.
        occupyingPlayer.transform.parent = null;
        occupyingPlayer.enableCharacter();
        occupyingPlayer.SendMessage("setSpawn", checkpointSpawnPosition);
        occupyingPlayer = null;
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
    }

    IEnumerator fireCannon()
    {
        if (canFire)
        {
            gameObject.GetComponent<AudioSource>().Play();
            Rigidbody cannonball = Instantiate(cannonballPrefab, launcher.transform.position, launcher.transform.rotation) as Rigidbody;
            cannonball.name = "cannonball";
            cannonball.useGravity = true;
            cannonball.linearVelocity = launcher.transform.forward  * cannonBallVelocity;
            // Disable collision with FPS controller because FPS controller could be in the way.
            Physics.IgnoreCollision(occupyingPlayer.GetComponent<Collider>(), cannonball.GetComponent<Collider>(), true);
            Physics.IgnoreCollision(gameObject.GetComponent<BoxCollider>(), cannonball.GetComponent<Collider>(), true);
            launcher.GetComponent<ParticleSystem>().Play();
            canFire = false;
            yield return new WaitForSeconds(3);
            canFire = true;
        }
    }

}
