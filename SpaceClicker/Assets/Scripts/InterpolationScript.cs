using UnityEngine;

public class InterpolationScript : MonoBehaviour
{
    public Transform target; // The target transform.
    public float positionSpeed = 5f; // Speed of position interpolation.
    public float rotationSpeed = 5f; // Speed of rotation interpolation.
    public bool atPosition = false; // Flag to indicate if at target position.
    public bool atRotation = false;
    public GameObject canvas;
    void Update()
    {
        // Check if the current position matches the target position.
        if (Vector3.Distance(transform.position, target.position) > 0.01f)
        {
            // Interpolate position.
            transform.position = Vector3.Lerp(transform.position, target.position, positionSpeed * Time.deltaTime);
        }
        else {
            atPosition = true;
        }
        // Check if the current rotation matches the target rotation.
        if (Quaternion.Angle(transform.rotation, target.rotation) > 1f)
        {
            // Interpolate rotation.
            transform.rotation = Quaternion.Lerp(transform.rotation, target.rotation, rotationSpeed * Time.deltaTime);
        }
        else 
        {
            atRotation = true;
        }
        if (atRotation && atPosition) 
        {
            enabled = false;
            canvas.SetActive(true);
        }
    }
}
