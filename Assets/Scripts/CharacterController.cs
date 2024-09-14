using System.Collections;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterController : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float jumpForce = 7f;
    public float gravityScale = 1f;
    public Transform hologram;
    public LayerMask groundMask; // LayerMask to detect ground

    private Rigidbody rb;
    private bool isGrounded;
    private Vector3 moveDirection;
    private Vector3 gravityDirection = Vector3.down;
    private Vector3 lastPosition;
    Animator anim;

    private int count = 0;
    
    public GameObject winPanel;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        lastPosition = transform.position;
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        // Movement
        float moveX = Input.GetAxis("Horizontal"); // A and D keys
        float moveZ = Input.GetAxis("Vertical");   // W and S keys

        Vector3 moveDirection = transform.right * moveX + transform.forward * moveZ;
        rb.MovePosition(transform.position + moveDirection * moveSpeed * Time.deltaTime);

        // Change direction based on movement
        if (moveDirection.magnitude > 0.1f)
        {
            Quaternion newRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = newRotation;
            CheckGrounded();
        }  //causes issues with player falling through the ground even after checking for ground.

        // Jumping
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
        
        GravityManipulation();
        ApplyGravity();
        AlignWithGravity();
    }

    void GravityManipulation()
    {
        Vector3 newGravityDirection = gravityDirection;

        if (Input.GetKey(KeyCode.UpArrow)) newGravityDirection = Vector3.up;
        if (Input.GetKey(KeyCode.DownArrow)) newGravityDirection = Vector3.down;
        if (Input.GetKey(KeyCode.LeftArrow)) newGravityDirection = Vector3.left;
        if (Input.GetKey(KeyCode.RightArrow)) newGravityDirection = Vector3.right;

        // Display hologram in direction
        hologram.position = transform.position + newGravityDirection * 2;
        hologram.rotation = Quaternion.LookRotation(newGravityDirection);

        if (Input.GetKeyDown(KeyCode.Return))
        {
            gravityDirection = newGravityDirection;
        }

        rb.AddForce(gravityDirection * gravityScale, ForceMode.Acceleration);
    }

    void AlignWithGravity()
    {
        Quaternion targetRotation = Quaternion.FromToRotation(transform.up, -gravityDirection) * transform.rotation;
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
    }

    void ApplyGravity()
    {
        rb.AddForce(gravityDirection * gravityScale * Physics.gravity.magnitude, ForceMode.Acceleration);
    }

    void CheckGrounded()
    {
        // Check if the character is grounded
        Ray ray = new Ray(transform.position + Vector3.up * 0.1f, Vector3.down);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 0.2f, groundMask))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("goal"))
        {
            Debug.Log("Collected");
            count += 1;
            Debug.Log(count);
            collision.gameObject.SetActive(false);

            if(count == 5)
            {
                winPanel.SetActive(true);
                Time.timeScale = 0f;
            }
        }

        if(collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
} //class
