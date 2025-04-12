using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(AudioSource))]
public class Cannonball : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // void OnCollisionEnter(Collision theObject)
    // {
    //     if (theObject.collider.gameObject.name == "Water")
    //         return;
    //     // gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
    //         ParticleSystem particleSystem = gameObject.GetComponentInChildren<ParticleSystem>();
    //         // Set particle to root
    //         particleSystem.transform.parent = null;
    //         particleSystem.Play();
    //         // Cleanup particle object after completion
    //         Destroy(particleSystem, particleSystem.main.duration);
    //         Destroy(gameObject);
    // }

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
