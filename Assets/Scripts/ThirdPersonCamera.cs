using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;         // Reference to the player character
    public Vector3 offset;           // Offset from the player character
    public float mouseSensitivity = 10f; // Sensitivity of the mouse movement
    public float smoothSpeed = 0.125f;   // Smoothness of the camera movement

    private float pitch = 0f;        // Vertical rotation angle
    private float yaw = 0f;          // Horizontal rotation angle

    void Start()
    {
        // Initialize the camera's position relative to the player
        transform.position = player.position + offset;
    }

    void LateUpdate()
    {
        // Get mouse input
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // Update yaw and pitch based on mouse input
        yaw += mouseX;
        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, -89f, 89f); // Clamp pitch to prevent flipping

        // Calculate the new rotation
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0f);

        // Apply the rotation to the camera
        transform.rotation = rotation;

        // Update the camera's position
        Vector3 desiredPosition = player.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }
} // class
