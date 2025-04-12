using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterControl : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider col)
    {
        TouchesWater waterTouchInterface = col.gameObject.GetComponent<TouchesWater>();
        if (waterTouchInterface != null)
            waterTouchInterface.OnTouchedWater();
        // col.gameObject.SendMessage("OnTouchedWater");
    }
}
