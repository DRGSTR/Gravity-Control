using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 7f;
    public float gravityScale = 1f;
    public GameObject referenceHologram;

    private Rigidbody rb;
    private Vector3 gravityDirection = Vector3.down;
    private bool isGrounded;
    private bool isFalling;
    private Vector3 lastPosition;
    Animator anim;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Physics.gravity = gravityDirection * gravityScale;
        lastPosition = transform.position;
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        HandleMovement();
        HandleJumping();
        HandleGravityManipulation();
        HandleHologram();
        ApplyGravity();
        CheckFalling();
        FaceMovementDirection();
    }

    void HandleMovement()
    {
        float moveHorizontal = Input.GetAxis("Horizontal"); // A, D keys
        float moveVertical = Input.GetAxis("Vertical"); // W, S keys

        Vector3 movement = new Vector3(moveHorizontal, 0, moveVertical).normalized;

        // Move the player
        rb.MovePosition(transform.position + movement * moveSpeed * Time.deltaTime);

        // Change direction based on movement
        if (movement.magnitude > 0.1f)
        {
            Quaternion newRotation = Quaternion.LookRotation(movement);
            rb.MoveRotation(newRotation);
        }
    }

    void HandleJumping()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    void HandleGravityManipulation()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            gravityDirection = Vector3.forward;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            gravityDirection = Vector3.back;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            gravityDirection = Vector3.left;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            gravityDirection = Vector3.right;
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            Physics.gravity = gravityDirection * gravityScale;
        }
    }

    void HandleHologram()
    {
        if (referenceHologram != null)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow) ||
                Input.GetKeyDown(KeyCode.DownArrow) ||
                Input.GetKeyDown(KeyCode.LeftArrow) ||
                Input.GetKeyDown(KeyCode.RightArrow))
            {
                referenceHologram.SetActive(true);
                referenceHologram.transform.position = transform.position + gravityDirection * 2;
            }
            else if (Input.GetKeyUp(KeyCode.UpArrow) ||
                     Input.GetKeyUp(KeyCode.DownArrow) ||
                     Input.GetKeyUp(KeyCode.LeftArrow) ||
                     Input.GetKeyUp(KeyCode.RightArrow))
            {
                referenceHologram.SetActive(false);
            }
        }
    }

    void ApplyGravity()
    {
        rb.AddForce(gravityDirection * gravityScale * Physics.gravity.magnitude, ForceMode.Acceleration);
    }

    void CheckFalling()
    {
        if (!isGrounded && rb.velocity.magnitude > 1f)
        {
            isFalling = true;
        }
        else
        {
            isFalling = false;
        }

        if (isFalling && transform.position.y < lastPosition.y)
        {
            anim.SetBool("IsFalling", true);
            // Player is falling and no longer in contact with the ground
            GameOver();
        }

        lastPosition = transform.position;
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

    void GameOver()
    {
        Debug.Log("Game Over!");
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);  
        // Implement your game over logic here
        // For example, stop the game or reload the scene
        // You can use UnityEngine.SceneManagement to reload the scene
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
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
}
