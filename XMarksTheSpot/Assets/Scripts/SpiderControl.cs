using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderControl : MonoBehaviour
{
    public Rigidbody webPrefab;
    private bool canFire = true;
    public float webSpeed = 10;
    public float webSize = 1;

    public float webCooldown = 3;
    public int webDamage = 20;
    public float speedDebuff = 3f;
    public bool rotates = true;
    public Vector3 rotationOffset;
    public float attackRange = 30f;
    private Transform playerTransform;
    private Animation spiderAnimation;

    public Transform launcher;

    // Start is called before the first frame update
    void Start()
    {
        spiderAnimation = gameObject.GetComponent<Animation>();
        // spiderAnimation.Play("Idle");
        playerTransform = PlayerController.mainController.getPlayer().transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, playerTransform.position) < attackRange) {
            // transform.rotation = Quaternion.RotateTowards();
            if (!spiderAnimation.isPlaying)
                spiderAnimation.Play("Idle");
            if (rotates)
            {
                transform.LookAt(new Vector3(playerTransform.position.x, transform.position.y, playerTransform.position.z));
                transform.Rotate(rotationOffset);
            }
            // // transform.localRotation
            
            if (canFire)
            {
                // Raycast to make sure the spider can hit the player.
                RaycastHit hit;
                if (Physics.Raycast (launcher.position, (playerTransform.position - launcher.position ).normalized, out hit, attackRange) && hit.collider.gameObject.tag == "Player")
                    StartCoroutine("fireWeb");
        
            }
        }
    }

    IEnumerator fireWeb()
    {
        canFire = false;
        spiderAnimation.Stop();
        spiderAnimation.Play("Attack1");
        yield return new WaitForSeconds(0.6f);
        Rigidbody web = Instantiate(webPrefab, launcher.position, transform.rotation) as Rigidbody;
        web.transform.localScale *= webSize;
        web.transform.LookAt(playerTransform.position);
        web.name = "web";
        web.velocity = web.transform.forward  * webSpeed * UserSettings.getDifficultyMultiplier();
        // Setting damage at the end as last priority
        web.GetComponent<SpiderWeb>().damage = webDamage;
        web.GetComponent<SpiderWeb>().speedDebuff = speedDebuff;
        yield return new WaitForSeconds(webCooldown);
        canFire = true;
    }
}
