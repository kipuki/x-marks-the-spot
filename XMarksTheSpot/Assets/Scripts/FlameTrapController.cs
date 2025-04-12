using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameTrapController : MonoBehaviour
{

    public float flameDuration = 4;
    public float flameCooldown = 4;

    public int damage = 10;
    public float damageCooldown = 1f;

    private bool canDamage = true;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("cycleFlames");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void turnOn()
    {
        Transform[] children = transform.GetComponentsInChildren<Transform>();
        foreach (Transform child in children)
        {
            ParticleSystem[] particles = transform.GetComponentsInChildren<ParticleSystem>();
            foreach (ParticleSystem particle in particles)
                particle.Play();
        }
        GetComponent<BoxCollider>().enabled = true;
    }

    public void turnOff()
    {
        GetComponent<BoxCollider>().enabled = false;
        Transform[] children = transform.GetComponentsInChildren<Transform>();
        foreach (Transform child in children)
        {
            ParticleSystem[] particles = transform.GetComponentsInChildren<ParticleSystem>();
            foreach (ParticleSystem particle in particles)
                particle.Stop();
        }
    }

    IEnumerator cycleFlames()
    {
        while (this.enabled)
        {
            yield return new WaitForSeconds(flameCooldown);
            turnOn();
            yield return new WaitForSeconds(flameDuration);
            turnOff();
        }
    }

    // void OnTriggerEnter(Collider col)
    // {
    //     if (col.gameObject.tag == "Player")
    //     {
    //         PlayerController.damagePlayer(damage);
    //     }
    // }

    void OnTriggerStay(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            if (canDamage)
                StartCoroutine("damagePlayer");
        }
    }

    IEnumerator damagePlayer()
    {
        canDamage = false;
        PlayerController.damagePlayer((int)(damage * UserSettings.getDifficultyMultiplier()));
        yield return new WaitForSeconds(damageCooldown);
        canDamage = true;
    }

}

