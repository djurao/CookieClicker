using UnityEngine;

public class RandomRotation : MonoBehaviour
{
    // Minimum and maximum speeds for rotation on each axis
    public Vector3 minRotationSpeed = new Vector3(10f, 10f, 10f);
    public Vector3 maxRotationSpeed = new Vector3(100f, 100f, 100f);

    // Current rotation speed
    private Vector3 rotationSpeed;

    void Start()
    {
        // Initialize the rotation speed with a random value within the specified range
        rotationSpeed = new Vector3(
            Random.Range(minRotationSpeed.x, maxRotationSpeed.x),
            Random.Range(minRotationSpeed.y, maxRotationSpeed.y),
            Random.Range(minRotationSpeed.z, maxRotationSpeed.z));
    }

    void Update()
    {
        // Rotate the object around its own axes based on the rotation speed and time
        transform.Rotate(rotationSpeed * Time.deltaTime);
    }

    // Method to tweak rotation speed at runtime via script or editor
    public void SetRotationSpeed(Vector3 newSpeed)
    {
        rotationSpeed = newSpeed;
    }
}
