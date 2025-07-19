using System.Collections.Generic;
using UnityEngine;

public class GhostManager : MonoBehaviour
{
    public GameObject dummyObject;
    public int maxBufferSize = 30;
    public float lerpSpeed = 10f;

    private Queue<MovementSnapshot> movementQueue = new Queue<MovementSnapshot>();
    private MovementSnapshot currentSnapshot;

    void Start()
    {
        currentSnapshot = new MovementSnapshot(dummyObject.transform.position, dummyObject.transform.rotation, dummyObject.transform.localScale);
    }

    public void ReceiveSnapshot(MovementSnapshot snapshot)
    {
        movementQueue.Enqueue(snapshot);

        if (movementQueue.Count > maxBufferSize)
        {
            movementQueue.Dequeue();
        }
    }

    void FixedUpdate()
    {
        if (movementQueue.Count > 0)
        {
            MovementSnapshot target = movementQueue.Peek();

            Vector3 offsetPosition = target.position + new Vector3(-2f, 0f, 0f); 

            dummyObject.transform.position = Vector3.Lerp(dummyObject.transform.position, offsetPosition, Time.deltaTime * lerpSpeed);

            dummyObject.transform.rotation = Quaternion.Slerp(dummyObject.transform.rotation, target.rotation, Time.deltaTime * lerpSpeed);

            dummyObject.transform.localScale = Vector3.Lerp(dummyObject.transform.localScale, target.scale, Time.deltaTime * lerpSpeed);

            if (Vector3.Distance(dummyObject.transform.position, offsetPosition) < 0.01f)
            {
                currentSnapshot = movementQueue.Dequeue();
            }
        }
    }


}
