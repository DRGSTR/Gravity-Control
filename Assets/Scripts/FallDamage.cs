using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallDamage : MonoBehaviour
{
    public GameObject gameOverPanel;
    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();    
    }

    private void OnTriggerEnter(Collider target)
    {
        if(target.gameObject.tag == "Respawn")
        {
            anim.SetBool("IsFalling", true);
            StartCoroutine("Reset");
        }
    }

    IEnumerator Reset()
    {
        yield return new WaitForSeconds(2f);
        gameOverPanel.SetActive(true);
        Time.timeScale = 0f;
        Debug.Log("Game Over!");
    }
} // class
