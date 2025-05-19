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
        collider.radius = 0.8f;
        collider.isTrigger = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (rotationSpeed != 0f)
            transform.Rotate(new Vector3(0, rotationSpeed * Time.deltaTime, 0));
        
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
