using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallDamage : MonoBehaviour
{
    public GameObject gameOverPanel;

    private bool wasFalling;
    private bool wasGrounded;
    private float startOfFall;
    private bool _grounded;
    private Rigidbody rb;
    private float minimumFall = 10f;

    private void Update()
    {
        rb = GetComponent<Rigidbody>(); 
    }

    private void FixedUpdate()
    {
        CheckGround();

        if (!wasGrounded && _grounded) GameOver();
        if (!wasFalling && isFalling) startOfFall = transform.position.y;

        wasGrounded = _grounded;
        wasFalling = isFalling;
    }

    void CheckGround()
    {
        _grounded = Physics.Raycast(transform.position + Vector3.up, -Vector3.up, 1.01f);
    }

    bool isFalling { get { return (!_grounded && rb.velocity.y < 0f); } }

    void GameOver()
    {
        float fallDistance = startOfFall - transform.position.y;

        if (fallDistance > minimumFall)
        {
            StartCoroutine("Reset");
            Debug.Log(fallDistance);
        }
    }

    IEnumerator Reset()
    {
        yield return new WaitForSeconds(1.5f);
        gameOverPanel.SetActive(true);
        Time.timeScale = 0f;
        Debug.Log("Game Over!");
    }
} // class
