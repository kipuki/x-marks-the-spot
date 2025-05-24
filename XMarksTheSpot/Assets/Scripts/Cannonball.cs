using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(AudioSource))]
public class Cannonball : MonoBehaviour
{
    // Start is called before the first frame update
    public void destroyCannonball()
    {
        ParticleSystem particleSystem = gameObject.GetComponentInChildren<ParticleSystem>();
        // Set particle to root
        particleSystem.transform.parent = null;
        particleSystem.Play();
        // Cleanup particle object after completion
        Destroy(particleSystem, particleSystem.main.duration);
        Destroy(gameObject);
    }
}
