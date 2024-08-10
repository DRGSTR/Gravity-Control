using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform target; // The target the camera will follow (usually the player)
 
    public float rotationSpeed = 5.0f; // Speed of camera rotation

    private float currentX = 0.0f;
    private float currentY = 0.0f;
    public float sensitivityX = 4.0f;
    public float sensitivityY = 2.0f;

    public float minYAngle = -20f;
    public float maxYAngle = 60f;

    void Update()
    {
        // Get mouse input for camera rotation
        currentX += Input.GetAxis("Mouse X") * sensitivityX;
        currentY -= Input.GetAxis("Mouse Y") * sensitivityY;

        // Clamp the vertical rotation
        currentY = Mathf.Clamp(currentY, minYAngle, maxYAngle);
    }

    void LateUpdate()
    {
        if (target != null)
        {
            // Calculate the desired camera position
            Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
        }
    }
} // class
