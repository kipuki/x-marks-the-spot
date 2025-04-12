using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TidyProjectile : MonoBehaviour, TouchesWater
{

    private bool touchedWater = false;
    private bool invisible = false;

    public bool hasToTouchWater = false;

    public bool destroyOnInvisible = true;

    public bool destroyOnCollision = false;

    public UnityEvent onCollide;

    private GameObject collidedObject;
    public float maximumLifeTime = 30;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, maximumLifeTime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void checkDespawnConditions()
    {
        // Debug.Log("Invisible: "+ invisible);
        // Debug.Log("Touched Water: "+ touchedWater);
        if (invisible && (touchedWater || !hasToTouchWater))
            Destroy(gameObject);
    }

    public void OnTouchedWater()
    {
        // Debug.Log("Touched water");
        touchedWater = true;
        checkDespawnConditions();
    }

    void OnBecameInvisible()
    {
        invisible = true;
        checkDespawnConditions();
    }

    void OnBecameVisible()
    {
        invisible = false;
    }

    public GameObject getCollidedObject()
    {
        return collidedObject;
    }

    void OnCollisionEnter(Collision theObject)
    {
        if (destroyOnCollision)
            Destroy(gameObject);
        onCollide.Invoke();
    }

}
