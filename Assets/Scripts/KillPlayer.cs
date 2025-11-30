using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KillPlayer : MonoBehaviour
{
    public DamagePlayer health;
    public PlayerMovement pMove;
    public PlayerCam pCam;
    public WandAttackRaycast wandR;
    public Animator transitionAnim;
    
    void Update()
    {
        if (transform.position.y < -20)
        {
            health.currentHealth = 0;
            health.healthBar.SetHealth(health.currentHealth);
        }

        if (health.currentHealth <= 1)
        {
            pMove.enabled = false;
            pCam.enabled = false;
            wandR.enabled = false;

            StartCoroutine(Transition());
        }
    }

    IEnumerator Transition()
    {
        transitionAnim.SetTrigger("Dead");
        yield return new WaitForSeconds(0.95f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
