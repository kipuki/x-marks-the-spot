using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoatControl : MonoBehaviour
{

    private bool used = false;
    private Camera playerCamera;
    private Camera boatCamera;

    public Vector3 destination;

    private GameObject playerObject;

    // public TMPro.TextMeshProUGUI textHints;
    private bool moveBoat = false;

    // Start is called before the first frame update
    void Start()
    {
        boatCamera = gameObject.transform.GetChild(0).GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (moveBoat)
        {
            if (!transform.position.Equals(destination)) {
                // transform.position += -transform.right * 10f * Time.deltaTime;
                // moveBoat = false;

                transform.position = Vector3.MoveTowards(transform.position, destination, 10f * Time.deltaTime);
            }
            else {
                moveBoat = false;
                playerCamera.enabled = true;
                boatCamera.enabled = false;
                GameObject player = playerCamera.transform.parent.gameObject;
                // playerCamera.gameObject.transform.parent.transform.position = boatCamera.gameObject.transform.position;
                player.SendMessage("setSpawn", boatCamera.gameObject.transform.position);
                player.SendMessage("respawn");
                this.enabled = false;
            }
        }
    }

    private void startMoving()
    {
        // yield return new WaitForSeconds(1.5f);
        // transform.rotation = Quaternion.FromToRotation(transform.position, destination);
        // Quaternion.LookRotation()
        gameObject.name = "usedBoat";
        moveBoat = true;
    }

    public void rideBoat()
    {
        used = true;
        playerCamera = playerObject.transform.GetChild(0).GetComponent<Camera>();
        boatCamera.enabled = true;
        playerCamera.enabled = false;
        playerObject.SendMessage("disableCharacter");
        TextHintHandler.showHint(new TextHint("The key worked! Onto the other island.", 2));
        // StartCoroutine("startMoving");
        // To disable the trigger.
        gameObject.GetComponent<BoxCollider>().enabled = false;
        Invoke("startMoving", 1.5f);
    }

    void OnTriggerEnter(Collider col)
    {
        if(!used && col.gameObject.tag == "Player")
        {
            if (PlayerController.hasBoatKey)
            {
                playerObject = col.gameObject;
                (gameObject.GetComponent("InteractTrigger") as MonoBehaviour).enabled = true;
            } else
                TextHintHandler.showHint(new TextHint("I need a key to start the engine.", 1, 7));
        }
    }
}

