using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WandAttackRaycast : MonoBehaviour
{
    public Transform attackPoint;
    public GameObject hitEffect;
    public UseMana mana;
    public LayerMask whatIsEnemy;
    private bool canAttack;

    private void Start()
    {
        canAttack = true;
    }
    private void Update()
    {
        Firing();
    }

    public void Firing()
    {
        if (Input.GetMouseButtonDown(0) && canAttack && mana.currentMana > 0)
        {
            RaycastHit hit;

            if(Physics.Raycast(attackPoint.position, transform.TransformDirection(Vector3.forward), out hit, 100))
            {
                Debug.DrawRay(attackPoint.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.red);

                GameObject hitPoint = Instantiate(hitEffect, hit.point, Quaternion.identity);

                Destroy(hitPoint, 1);

                EnemyAI enemy = hit.transform.parent.parent.GetComponent<EnemyAI>();
                if (enemy != null)
                {
                    enemy.TakeDamage(10);
                }

                mana.UsingMana(10);
            }

            canAttack = false;
            StartCoroutine(AttackCooldown());
        }
    }

    IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(0.6f);
        canAttack = true;
    }
}
