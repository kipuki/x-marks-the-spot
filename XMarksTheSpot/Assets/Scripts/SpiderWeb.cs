using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderWeb : MonoBehaviour
{
    public int damage = 20;
    public float speedDebuff = 3;

    void OnTriggerEnter(Collider col)
    {
        GetComponent<MeshRenderer>().enabled = false;
        if (col.gameObject.tag == "Player")
        {
            PlayerController.DamagePlayer(damage);
            PlayerController.SpeedDebuff(speedDebuff, 4);
            Destroy(gameObject);
        } else {
            Destroy(gameObject, 0.5f);
        }
    }
}
