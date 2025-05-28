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

    private void CheckDespawnConditions()
    {
        if (invisible && (touchedWater || !hasToTouchWater))
            Destroy(gameObject);
    }

    public void OnTouchedWater()
    {
        touchedWater = true;
        CheckDespawnConditions();
    }

    void OnBecameInvisible()
    {
        invisible = true;
        CheckDespawnConditions();
    }

    void OnBecameVisible()
    {
        invisible = false;
    }

    public GameObject GetCollidedObject()
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
