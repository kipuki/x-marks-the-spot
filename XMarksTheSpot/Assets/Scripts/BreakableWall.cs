using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent (typeof(AudioSource))]
public class BreakableWall : MonoBehaviour
{

    public UnityEvent onWallBreak;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision theObject) 
    {
        if (theObject.gameObject.name == "cannonball")
        {
            gameObject.name = "brokenWall";
            gameObject.GetComponent<MeshRenderer>().enabled = false;
            gameObject.GetComponent<BoxCollider>().enabled = false;

            Rigidbody[] children = transform.GetComponentsInChildren<Rigidbody>();
            foreach(Rigidbody child in children)
            {
                child.transform.parent = transform.parent;
                child.gameObject.GetComponent<BoxCollider>().enabled = true;
                child.isKinematic = false;
                child.GetComponent<TidyObject>().enabled = true;
            }
            onWallBreak.Invoke();
            gameObject.GetComponent<AudioSource>().Play();
            TextHintHandler.showHint(new TextHint("What a bang! That seems to have done it. Into the tunnel I go!", 1, 8));
            // (transform.parent.GetComponent("TidyObject") as MonoBehaviour).enabled = true;
        }
    }
}
