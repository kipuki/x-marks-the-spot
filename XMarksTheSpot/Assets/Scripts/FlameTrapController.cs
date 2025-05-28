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
        StartCoroutine(CycleFlames());
    }

    public void TurnOn()
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

    public void TurnOff()
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

    IEnumerator CycleFlames()
    {
        while (this.enabled)
        {
            yield return new WaitForSeconds(flameCooldown);
            TurnOn();
            yield return new WaitForSeconds(flameDuration);
            TurnOff();
        }
    }

    void OnTriggerStay(Collider col)
    {
        if (col.gameObject.tag != "Player")
            return;
            
        if (canDamage)
            StartCoroutine(DamagePlayer());
    }

    IEnumerator DamagePlayer()
    {
        canDamage = false;
        PlayerController.DamagePlayer((int)(damage * UserSettings.GetDifficultyMultiplier()));
        yield return new WaitForSeconds(damageCooldown);
        canDamage = true;
    }

}

