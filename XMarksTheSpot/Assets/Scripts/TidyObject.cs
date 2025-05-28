using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TidyObject : MonoBehaviour {

    public float removeTime = 3.0f;
	public bool shrinkBeforeRemove = false;
	public float shrinkRate = 0.5f;
	private bool shrink;

	public Vector3 shrinkLimit = new Vector3(0,0,0);

	// Use this for initialization
	void Start ()
	{
		if (shrinkBeforeRemove)
			StartCoroutine(ShrinkAfterRemoveTime());
		else
			Destroy(gameObject, removeTime);
	}

	void Update()
	{
		if (!shrink)
			return;
		
		if (!transform.localScale.Equals(shrinkLimit))
			transform.localScale = Vector3.MoveTowards(transform.localScale, shrinkLimit, shrinkRate * Time.deltaTime);
		else
			Destroy(gameObject);
	}

	IEnumerator ShrinkAfterRemoveTime()
	{
		yield return new WaitForSeconds(removeTime);
		shrink = true;
	}
}
