using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour, TouchesWater
{

    public static PlayerController mainController;

    public Vector3 spawnPosition;
    public Quaternion spawnRotation;
    private MonoBehaviour fpsController;
    private CharacterController charController;

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

    public static void DamagePlayer(int damage)
    {
        mainController.HUDon();
        Debug.Log("Took damage: "+ damage);
        health -= damage;
        if (health <= 0)
            mainController.Respawn();
        else
            mainController.healthBar.value = health;
    }

    public static void SpeedDebuff(float strength, float duration)
    {
        if (!isDebuffed)
            mainController.StartCoroutine(mainController.SlowDown(strength, duration));
    }

    IEnumerator SlowDown(float strength, float duration)
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
    }


    // Start is called before the first frame update
    void Start()
    {
        healthBar.value = health;
        spawnPosition = transform.position;
        spawnRotation = transform.rotation;
        fpsController = (gameObject.GetComponent("FirstPersonController") as MonoBehaviour);
        charController = transform.GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {


        RaycastHit hit;

        if(Physics.Raycast (transform.GetChild(0).position, transform.GetChild(0).forward, out hit, 100, layerMask))
        {
            switch(hit.collider.gameObject.name)
            {
                case "Boat":
                    // if (!usedBoat)
                    if (hasBoatKey)
                        TextHintHandler.ShowHint("I should use the boat to get to the mountain...");
                    else
                        TextHintHandler.ShowHint("I should use the boat to get to the mountain... But where did I put the keys?");
                    break;
                case "backWall":
                    TextHintHandler.ShowHint("Seems the entrance is blocked off. Maybe if I use that cannon over there...");
                    break;
                case "boatKey":
                    // No need to check if hasBoatKey since the key destroys itself and it wouldnt be possible to get here.
                    TextHintHandler.ShowHint("I see a key over there!");
                    // TextHintHandler.testHint();
                    break;
                case "untouchedTreasureSpot":
                    TextHintHandler.ShowHint("X marks the spot. The treasure should be under that X!");
                    break;
            }

        }
    }

    public Camera GetCamera()
    {
        return transform.GetChild(0).gameObject.GetComponent<Camera>();
    }

    public GameObject GetPlayer()
    {
        return gameObject;
    }

    public bool CheckIfFacing(GameObject targetObject)
    {
        RaycastHit hit;

        if(Physics.Raycast (transform.GetChild(0).position, transform.GetChild(0).forward, out hit, 100))
            return hit.collider.gameObject == targetObject;
        return false;
    }

    void FoundKey()
    {
        hasBoatKey = true;
        TextHintHandler.ShowHint("Great! Now I can use my boat.");
    }

    void AddPoints(int increment)
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

    public void EnableCharacter()
    {
        charController.enabled = true;
        fpsController.enabled = true;
    }

    public void DisableCharacter()
    {
        fpsController.enabled = false;
        charController.enabled = false;
    }

    void Respawn()
    {
        DisableCharacter();
        health = 100;
        transform.position = spawnPosition;
        transform.rotation = spawnRotation;
        fpsController.enabled = true;
        (fpsController as FirstPersonController).m_MoveDir = Vector2.zero;
        EnableCharacter();
        healthBar.value = health;
    }

    void SetSpawnPosition(Vector3 newSpawnPosition)
    {
        spawnPosition = newSpawnPosition;
    }

    void SetSpawnRotation(Quaternion newSpawnRotation)
    {
        spawnRotation = newSpawnRotation;
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.tag == "Respawn")
        {
            Respawn();
            Debug.Log("Respawned.");
            TextHintHandler.ShowHint("Lets be more careful this time...");
        }
    }

    public void OnTouchedWater()
    {
        Respawn();
        TextHintHandler.ShowHint("Lets not fall in the water again...");
    }
}
