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
        Move();
        Jump();
        GravityManipulation();
        ApplyGravity();
        FaceMovementDirection();
        AlignWithGravity();
    }

    void Move()
    {
        float moveHorizontal = Input.GetAxis("Horizontal"); // A, D keys
        float moveVertical = Input.GetAxis("Vertical"); // W, S keys

        Vector3 movement = new Vector3(moveHorizontal, 0, moveVertical).normalized;

        // Move the player
        rb.MovePosition(transform.position + movement * moveSpeed * Time.deltaTime);

        //// Change direction based on movement
        //if (movement.magnitude > 0.1f)
        //{
        //    Quaternion newRotation = Quaternion.LookRotation(movement);
        //    transform.rotation = newRotation;
        //}
    }

    void Jump()
    {
        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    void GravityManipulation()
    {
        Vector3 newGravityDirection = gravityDirection;

        if (Input.GetKey(KeyCode.UpArrow)) newGravityDirection = Vector3.up;
        if (Input.GetKey(KeyCode.DownArrow)) newGravityDirection = Vector3.back;
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

    void FaceMovementDirection()
    {
        Vector3 movementDirection = rb.velocity;

        if (movementDirection.magnitude > 0.1f) // Only rotate if there is movement
        {
            Quaternion newRotation = Quaternion.LookRotation(movementDirection);
            rb.MoveRotation(newRotation);
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
