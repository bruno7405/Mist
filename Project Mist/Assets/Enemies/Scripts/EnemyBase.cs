using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] int currentHealth;
    [SerializeField] int maxHealth;
    public float aggroDistance = 15;
    public float attackDistance = 2f;
    private bool isDead = false;

    [SerializeField] StateMachineManager stateMachine;
    [SerializeField] HurtState hitState;
    [SerializeField] DeathState deathState;

    private void Awake()    
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int amt, Vector3 hitPoint, float knockbackForce)
    {
        if (isDead) return;

        currentHealth -= amt;

        if (currentHealth <= 0) // death
        {
            isDead = true;
            deathState.SetHitPoint(hitPoint);
            deathState.SetHitForce(knockbackForce);
            stateMachine.SetNewState(deathState);
            
        }
        else // hurt
        {
            hitState.hitPoint = hitPoint;
            stateMachine.SetNewState(hitState);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, aggroDistance);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackDistance);
    }
}
