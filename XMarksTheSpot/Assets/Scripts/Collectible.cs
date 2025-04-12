using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(SphereCollider))]
public class Collectible : MonoBehaviour
{

    public int value;
    public GameObject collectEffect;

    public float rotationSpeed = 100.0f;
    
    // Start is called before the first frame update
    void Start()
    {
        SphereCollider collider = gameObject.GetComponent<SphereCollider>();
        // if (!collider)
        //     collider = transform.parent.gameObject.AddComponent<SphereCollider>();
        collider.radius = 0.8f;
        collider.isTrigger = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (rotationSpeed != 0f)
            transform.Rotate(new Vector3(0, rotationSpeed * Time.deltaTime, 0));
            
        // Removed to replace with an actual animation :)
        //transform.Translate(0,0.5f,0);
        // if (idleFrameDuration <= 0)
        // {
        //     if (transform.position.y > translationHeight + originalHeight)
        //     {
        //         directionMultiplier = -1;
        //         idleFrameDuration = stopDuration;
        //     } else if (transform.position.y < originalHeight)
        //     {
        //         directionMultiplier = 1;
        //         idleFrameDuration = stopDuration;
        //         gameObject.GetComponentInChildren<ParticleSystem>().Play();
        //     }
        // }
        // else {
        //     idleFrameDuration--;
        // }

        // if (idleFrameDuration <= 0)
        //     transform.Translate(0,directionMultiplier * Time.deltaTime * translationSpeed,0);
        
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player") {
            col.SendMessage("addPoints", value);
            if(collectEffect)
			    Instantiate(collectEffect, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

}
