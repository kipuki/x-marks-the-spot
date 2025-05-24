using UnityEngine;
using UnityEngine.Events;

public class LoopingCamera : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float loopDistance = 50f;
    public UnityEvent OnStart;
    public UnityEvent OnUpdate;

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
        OnStart?.Invoke();
    }

    void Update()
    {
        OnUpdate?.Invoke();

        // Move camera forward
        transform.position += transform.forward * moveSpeed * Time.deltaTime;

        // Check distance traveled
        float traveled = Vector3.Distance(startPosition, transform.position);
        if (traveled >= loopDistance)
        {
            transform.position = startPosition;
        }
    }
}
