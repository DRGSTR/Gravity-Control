using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform player; // Reference to the player's transform
    public Vector3 offset; // Offset relative to the player
    private Vector3 gravityDirection = Vector3.up;

    void Start()
    {
        // Initialize offset if not set
        if (offset == Vector3.zero)
        {
            offset = transform.position - player.position;
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.UpArrow)) AlignWithGravity();
    }

    void LateUpdate()
    {
        // Update the camera's position relative to the player
        transform.position = player.position + offset;

        // Make the camera look at the player
        //transform.LookAt(player.position + Vector3.up * 1.5f); // Adjust the height offset as needed

    }

    void AlignWithGravity()
    {
        Quaternion targetRotation = Quaternion.FromToRotation(transform.up, -gravityDirection) * transform.rotation;
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
    }
}
