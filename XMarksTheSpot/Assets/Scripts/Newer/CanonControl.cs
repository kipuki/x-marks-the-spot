using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(CapsuleCollider))]
[RequireComponent (typeof(AudioSource))]
public class CanonControl : MonoBehaviour
{

    public Camera canonCamera;
    private Camera playerCamera;
    public Rigidbody cannonballPrefab;

    public Rigidbody horizontalRigid;

    public Rigidbody verticalRigid;

    public SpriteRenderer crosshair;

    public GameObject launcher;

    // public AudioClip fireSound;

    public float cannonBallVelocity;

    private float initialX;

    private int rotationSpeed = 60;

    private PlayerController occupyingPlayer;

    private bool canFire = true;

    // Start is called before the first frame update
    void Start()
    {
        canonCamera.enabled = false;
        if (verticalRigid)
            initialX = verticalRigid.transform.eulerAngles.x;
    }

    // Update is called once per frame
    void Update()
    {
        if (occupyingPlayer)
        {


            // Get button better than GetButtonDown because it always return true while rotated.
            if (Input.GetButton("Horizontal"))
                horizontalRigid.transform.Rotate(0,Input.GetAxis("Horizontal")*rotationSpeed * Time.deltaTime,0, Space.World);
            if (verticalRigid && Input.GetButton("Vertical"))
            {
                float angleX = verticalRigid.transform.eulerAngles.x;
                float verticalInputAxis = Input.GetAxis("Vertical");
                if (angleX > 180)
                    angleX -= 360;


                // Debug.Log(angleX);

                float newRotationX = (verticalInputAxis*rotationSpeed * Time.deltaTime) + angleX;

                if (newRotationX <= 0 && newRotationX >= -40)
                    newRotationX -= angleX;
                else if ((newRotationX >= 0 && verticalInputAxis > 0) || (newRotationX <= -40 && verticalInputAxis < 0))
                    newRotationX = 0;

                if (newRotationX != 0)
                    verticalRigid.transform.Rotate(newRotationX,0,0, Space.Self);
                
                
            } 
            if (canFire && Input.GetButtonDown("Jump"))
                StartCoroutine("fireCannon");
        }
    }

    // void OnTriggerEnter(Collider col)
    // {
    //     if (col.gameObject.tag == "Player")
    //     {
    //         canonCamera.enabled = true;
    //         playerCamera = PlayerController.mainController.getCamera();
    //         playerCamera.enabled = false;

    //         occupyingPlayer = PlayerController.mainController.getPlayer().GetComponent("FirstPersonController") as MonoBehaviour;

    //         // Disable character controller.
    //         occupyingPlayer.enabled = false;
    //         crosshair.enabled = true;
    //         Debug.Log("Entered Cannon");
    //     }
    // }

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
        occupyingPlayer = null;
        
        // (gameObject.GetComponent<InteractTrigger>() as MonoBehaviour).enabled = true;
        // gameObject.GetComponent<CapsuleCollider>().enabled = true;
    }


    public void controlCanon()
    {
        // Set occupying player
        occupyingPlayer = PlayerController.mainController;

        gameObject.GetComponent<CapsuleCollider>().enabled = false;
        canonCamera.enabled = true;
        
        playerCamera = occupyingPlayer.getCamera();
        playerCamera.enabled = false;

        // Disable character controller.
        occupyingPlayer.disableCharacter();
    
        crosshair.enabled = true;
        occupyingPlayer.transform.parent = gameObject.transform;
        // Debug.Log("Entered Cannon");
    }

    IEnumerator fireCannon()
    {
        if (canFire)
        {
            gameObject.GetComponent<AudioSource>().Play();
            Rigidbody cannonball = Instantiate(cannonballPrefab, launcher.transform.position, launcher.transform.rotation) as Rigidbody;
            // Debug.Log(launcher.transform.rotation);
            cannonball.name = "cannonball";
            cannonball.useGravity = true;
            cannonball.velocity = launcher.transform.forward  * cannonBallVelocity;
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
