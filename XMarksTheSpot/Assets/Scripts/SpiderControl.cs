using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderControl : MonoBehaviour
{
    public Rigidbody webPrefab;
    private bool canFire = true;
    public float webSpeed = 10;
    public float webSize = 1;
    public Vector3 webAngularVelocity = new Vector3(0f, 0f, 0f);

    public float webCooldown = 3;
    public int webDamage = 20;
    public float speedDebuff = 3f;
    public bool rotates = true;
    public Vector3 rotationOffset;
    public float attackRange = 30f;
    private Transform targetTransform;
    public GameObject target;
    private Animation spiderAnimation;

    public Transform launcher;

    // Start is called before the first frame update
    void Start()
    {
        spiderAnimation = gameObject.GetComponent<Animation>();
        targetTransform = target.transform;
    }

    public void SetTarget(GameObject newTarget)
    {
        target = newTarget;
        targetTransform = target.transform;
    }

    public void FireAtTargetIfPossible()
    {
        if (Vector3.Distance(transform.position, targetTransform.position) > attackRange)
            return;

        if (!canFire)
            return;

        if (!spiderAnimation.isPlaying)
            spiderAnimation.Play("Idle");

        if (rotates)
        {
            transform.LookAt(new Vector3(targetTransform.position.x, transform.position.y, targetTransform.position.z));
            transform.Rotate(rotationOffset);
        }

        RaycastHit hit;
        if (Physics.Raycast(launcher.position, (targetTransform.position - launcher.position).normalized, out hit, attackRange) && (hit.collider.gameObject.tag == "Player" || hit.collider.gameObject == target))
            StartCoroutine(FireWeb());
    }

    // Update is called once per frame
    void Update()
    {
        FireAtTargetIfPossible();
    }

    IEnumerator FireWeb()
    {
        canFire = false;
        spiderAnimation.Stop();
        spiderAnimation.Play("Attack1");
        yield return new WaitForSeconds(0.6f);
        Rigidbody web = Instantiate(webPrefab, launcher.position, transform.rotation) as Rigidbody;
        web.transform.localScale *= webSize;
        web.transform.LookAt(targetTransform.position);
        web.name = "web";
        web.linearVelocity = web.transform.forward  * webSpeed * UserSettings.GetDifficultyMultiplier();
        web.angularVelocity = webAngularVelocity;
        TrailRenderer trail = web.GetComponent<TrailRenderer>();
        trail.widthMultiplier = web.transform.localScale.magnitude / 2;
        trail.time = webSize;
        // Setting damage at the end as last priority
        web.GetComponent<SpiderWeb>().damage = webDamage;
        web.GetComponent<SpiderWeb>().speedDebuff = speedDebuff;
        yield return new WaitForSeconds(webCooldown);
        canFire = true;
    }
}
