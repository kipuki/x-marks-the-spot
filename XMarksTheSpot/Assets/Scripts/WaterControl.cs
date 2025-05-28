using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterControl : MonoBehaviour
{
    void OnTriggerEnter(Collider col)
    {
        TouchesWater waterTouchInterface = col.gameObject.GetComponent<TouchesWater>();
        if (waterTouchInterface != null)
            waterTouchInterface.OnTouchedWater();
    }
}
