using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DigTreasure : MonoBehaviour
{

    private int timesDug = 0;
    public int digGoal = 5;
    public Vector3 targetVector = Vector3.zero;
    // private Vector3 startPos;
    private Vector3 deltaVector;
    // Start is called before the first frame update
    void Start()
    {
        deltaVector = targetVector - transform.localPosition;
    }

    public void DigOut()
    {
        transform.parent.name = "treasureSpot";
        timesDug++;
        transform.localPosition = Vector3.MoveTowards(transform.localPosition, targetVector, deltaVector.magnitude/digGoal);
        if (transform.localPosition.Equals(targetVector))
        {
            GetComponentInParent<ProximityPrompt>().disableAfterUse = true;
            GetComponentInChildren<CapsuleCollider>().enabled = true;
            TextHintHandler.ShowHint(new TextHint("At last I have found the treasure! Lets open it.",2,5));
        }
    }
}
