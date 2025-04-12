using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderWeb : MonoBehaviour
{
    public int damage = 20;
    public float speedDebuff = 3;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // GetComponent<Rigidbody>().velocity += transform.forward;
    }

    void OnTriggerEnter(Collider col)
    {
        GetComponent<MeshRenderer>().enabled = false;
        if (col.gameObject.tag == "Player")
        {
            PlayerController.damagePlayer(damage);
            PlayerController.speedDebuff(speedDebuff, 4);
            Destroy(gameObject);
        } else {
            Destroy(gameObject, 0.5f);
        }
    }
}
