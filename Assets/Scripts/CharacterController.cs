using System.Collections;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterController : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float jumpForce = 5f;
    public float gravityScale = 1f;
    public Transform hologram;

    private Rigidbody rb;
    private bool isGrounded;
    private Vector3 moveDirection;
    private Vector3 gravityDirection = Vector3.down;
    private bool isFalling;
    private Vector3 lastPosition;
    Animator anim;
    public GameObject gameOverPanel;

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
        CheckFalling();
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

        // Change direction based on movement
        if (movement.magnitude > 0.1f)
        {
            Quaternion newRotation = Quaternion.LookRotation(movement);
            rb.MoveRotation(newRotation);
        }
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

    void CheckFalling()
    {
        if (!isGrounded && rb.velocity.magnitude > 10f)
        {
            isFalling = true;
            anim.SetBool("IsFalling", true);
        }
        else
        {
            isFalling = false;
            anim.SetBool("IsFalling", false);
            //Debug.Log("grounded!");
        }

        if (isFalling && transform.position.y < lastPosition.y)
        {
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
        StartCoroutine("Reset");
        // Implement your game over logic here
        // For example, stop the game or reload the scene
        // You can use UnityEngine.SceneManagement to reload the scene
    }

    IEnumerator Reset()
    {
        yield return new WaitForSeconds(4f);
        gameOverPanel.SetActive(true);
        Time.timeScale = 0f;
        Debug.Log("Game Over!");
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            isFalling = false ;
        }

        if(collision.gameObject.tag == "goal")
        {
            Debug.Log("collected");
            collision.gameObject.SetActive(false);
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
