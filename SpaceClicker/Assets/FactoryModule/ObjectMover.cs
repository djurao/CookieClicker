using UnityEngine;

public class ObjectMover : MonoBehaviour
{
    public Transform pointA; // Anchor point A
    public Transform pointB; // Anchor point B
    public float speed = 1.0f; // Movement speed

    private bool movingToB = true; // Flag to determine direction
    private float time = 0f; // Time accumulator for Lerp
    private void Start()
    {
        transform.LookAt(pointB.position);

    }
    void Update()
    {
        // Update the time based on speed
        time += Time.deltaTime * speed;

        Vector3 targetPosition;

        if (movingToB)
        {
            targetPosition = Vector3.Lerp(pointA.position, pointB.position, time);

            // Once the object reaches B, change direction
            if (time >= 1f)
            {
                movingToB = false;
                time = 0f; // Reset time for reverse trip

                // Instantly adjust rotation to face the next target
                transform.LookAt(pointA.position);
            }
        }
        else
        {
            targetPosition = Vector3.Lerp(pointB.position, pointA.position, time);

            // Once the object reaches A, change direction
            if (time >= 1f)
            {
                movingToB = true;
                time = 0f; // Reset time for forward trip

                // Instantly adjust rotation to face the next target
                transform.LookAt(pointB.position);
            }
        }

        // Move the object to the target position
        transform.position = targetPosition;
    }
}
