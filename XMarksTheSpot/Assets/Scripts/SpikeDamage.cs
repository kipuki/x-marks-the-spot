using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeDamage : MonoBehaviour
{
    public int damage = 100;

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            PlayerController.DamagePlayer((int)(damage*UserSettings.GetDifficultyMultiplier()));
        }
    }
}
