using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SnowballAttack : MonoBehaviour
{
    public DamagePlayer playerHealth;
    public int damage;

    private void Start()
    {
        playerHealth = GameObject.Find("HealthBar").GetComponent<DamagePlayer>();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.name == "Player")
        {
            playerHealth.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
