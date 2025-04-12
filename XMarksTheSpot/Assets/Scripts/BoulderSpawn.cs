using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoulderSpawn : MonoBehaviour
{

    public Rigidbody boulderPrefab;
    public int spawnRate = 5;
    public int count = 1;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("spawnBoulders");
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator spawnBoulders()
    {
        while (spawnRate > 0)
        {
            for (int i = 0; i < count; i++)
            {
                Rigidbody newBoulder = Instantiate(boulderPrefab, transform.Find("rockSpawn").position, transform.rotation) as Rigidbody;
                newBoulder.name = "boulder";
                newBoulder.mass *= Random.Range(0.2f,2);
                // Vector3 boulderSize = newBoulder.transform.localScale;
                // newBoulder.centerOfMass = new Vector3(Random.Range(-boulderSize.x/2, boulderSize.x/2), Random.Range(-boulderSize.y/2, boulderSize.y/2), Random.Range(-boulderSize.z/2, boulderSize.z/2));
            }
            yield return new WaitForSeconds(spawnRate);
        }
    }
}
