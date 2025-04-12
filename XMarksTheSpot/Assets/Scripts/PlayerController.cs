using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;

public class PlayerController : MonoBehaviour, TouchesWater
{

    public static PlayerController mainController;

    public Vector3 spawnLocation;
    private MonoBehaviour fpsController;
    private CharacterController charController;
    // public TMPro.TextMeshProUGUI textHints;

    public Slider healthBar;
    public TMPro.TextMeshProUGUI scoreText;
    public static int points = 0;
    public static int health = 100;
    public AudioClip collectSound;

    public LayerMask layerMask;

    public Image hudDisplay;

    // private bool usedBoat = false;

    public static bool hasBoatKey = false;
    private static bool isDebuffed = false;

    public static void damagePlayer(int damage)
    {
        mainController.HUDon();
        Debug.Log("Took damage: "+ damage);
        health -= damage;
        if (health <= 0)
            mainController.respawn();
        else
            mainController.healthBar.value = health;
    }

    public static void speedDebuff(float strength, float duration)
    {
        if (!isDebuffed)
            mainController.StartCoroutine(mainController.slowDown(strength, duration));
    }

    IEnumerator slowDown(float strength, float duration)
    {
        isDebuffed = true;
        FirstPersonController fpsController = GetComponent<FirstPersonController>();
        float initialWalkspeed = fpsController.m_WalkSpeed;
        float initialRunSpeed = fpsController.m_RunSpeed;

        fpsController.m_RunSpeed /= strength;
        fpsController.m_WalkSpeed /= strength;

        yield return new WaitForSeconds(duration);

        fpsController.m_WalkSpeed = initialWalkspeed;
        fpsController.m_RunSpeed = initialRunSpeed;
        isDebuffed = false;
    }

    void Awake()
    {
        mainController = this;
        health = 100;
        points = 0;
        hasBoatKey = false;
        isDebuffed = false;
        // AudioSource backgroundMusic = GetComponentInChildren<AudioSource>();
        // backgroundMusic.volume = UserSettings.getMusicVolume();
        // AudioListener.volume = UserSettings.getMasterVolume();
        // AudioSource[] components = GameObject.FindObjectsOfType<AudioSource>();
        // foreach (AudioSource component in components)
        //     if (component != backgroundMusic)
        //         component.volume = UserSettings.getSFXVolume();
    }


    // Start is called before the first frame update
    void Start()
    {
        healthBar.value = health;
        spawnLocation = transform.position;
        fpsController = (gameObject.GetComponent("FirstPersonController") as MonoBehaviour);
        charController = transform.GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {

        // RaycastHit[] hits = Physics.RaycastAll(transform.position, transform.forward, 150.0f);
        // for (int i = 0; i < hits.Length; i++)
        // {
        //     RaycastHit hit = hits[i];
        //     Debug.Log(hit.collider.gameObject.name);
        //     if (hit.collider.gameObject.tag == "Boat")
        //     {
        //         if (hasBoatKey)
        //             textHints.SendMessage("showHint", "I should use the boat to get to the mountain...");
        //         else
        //             textHints.SendMessage("showHint", "I should use the boat to get to the mountain... But where did I put the keys?");
        //         break;
        //     } else if (hit.collider.gameObject.name=="blockade")
        //     {
        //         textHints.SendMessage("showHint", "Seems the entrance was blocked off. Maybe if I use that cannon over there...");
        //     }
        // }

        RaycastHit hit;
        // bool ray = Physics.Raycast (transform.GetChild(0).position, transform.GetChild(0).forward, out hit, 100);
        // bool ray = Physics.Raycast (transform.GetChild(0).GetComponent<Camera>().ViewportPointToRay(new Vector3(0.5f,0.5f,0)), out hit);

        if(Physics.Raycast (transform.GetChild(0).position, transform.GetChild(0).forward, out hit, 100, layerMask))
        {
            // if (hit.collider.gameObject.tag == "Boat")
            // {
            //     if (hasBoatKey)
            //         textHints.SendMessage("showHint", "I should use the boat to get to the mountain...");
            //     else
            //         textHints.SendMessage("showHint", "I should use the boat to get to the mountain... But where did I put the keys?");

            // } else if (hit.collider.gameObject.name=="blockade")
            // {
            //     textHints.SendMessage("showHint", "Seems the entrance was blocked off. Maybe if I use that cannon over there...");
            // }
            // Debug.Log(hit.collider.gameObject.name);
            switch(hit.collider.gameObject.name)
            {
                case "Boat":
                    // if (!usedBoat)
                    if (hasBoatKey)
                        TextHintHandler.showHint("I should use the boat to get to the mountain...");
                    else
                        TextHintHandler.showHint("I should use the boat to get to the mountain... But where did I put the keys?");
                    break;
                case "backWall":
                    TextHintHandler.showHint("Seems the entrance is blocked off. Maybe if I use that cannon over there...");
                    break;
                case "boatKey":
                    // No need to check if hasBoatKey since the key destroys itself and it wouldnt be possible to get here.
                    TextHintHandler.showHint("I see a key over there!");
                    // TextHintHandler.testHint();
                    break;
                case "untouchedTreasureSpot":
                    TextHintHandler.showHint("X marks the spot. The treasure should be under that X!");
                    break;
            }

        }
    }

    public Camera getCamera()
    {
        return transform.GetChild(0).gameObject.GetComponent<Camera>();
    }

    public GameObject getPlayer()
    {
        return gameObject;
    }

    public bool checkIfFacing(GameObject targetObject)
    {
        RaycastHit hit;

        if(Physics.Raycast (transform.GetChild(0).position, transform.GetChild(0).forward, out hit, 100))
            return hit.collider.gameObject == targetObject;
        return false;
    }

    void foundKey()
    {
        hasBoatKey = true;
        TextHintHandler.showHint("Great! Now I can use my boat.");
    }

    void addPoints(int increment)
    {
        HUDon();
        points += increment;
        scoreText.text = "Gems: "+ points;
        Debug.Log(points);
        AudioSource.PlayClipAtPoint(collectSound, transform.position);
    }

    void HUDon()
    {
        if (!hudDisplay.gameObject.activeSelf)
        {
            hudDisplay.gameObject.SetActive(true);
        }
    }

    public void enableCharacter()
    {
        charController.enabled = true;
        fpsController.enabled = true;
    }

    public void disableCharacter()
    {
        fpsController.enabled = false;
        charController.enabled = false;
    }

    void respawn()
    {
        disableCharacter();
        health = 100;
        transform.position = spawnLocation;
        fpsController.enabled = true;
        (fpsController as FirstPersonController).m_MoveDir = Vector2.zero;
        enableCharacter();
        healthBar.value = health;
    }

    void setSpawn(Vector3 newSpawn)
    {
        spawnLocation = newSpawn;
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.tag == "Respawn")
        {
            Debug.Log("Respawned.");
            respawn();
            TextHintHandler.showHint("Lets be more careful this time...");
        }
    }

    public void OnTouchedWater()
    {
        respawn();
        TextHintHandler.showHint("Lets not fall in the water again...");
    }
}
