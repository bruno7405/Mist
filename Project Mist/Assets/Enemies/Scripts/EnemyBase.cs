using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    int currentHealth;
    public int maxHealth;
    private bool isDead = false;

    [SerializeField] StateMachineManager stateMachine;
    [SerializeField] State hitState;
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
            stateMachine.SetNewState(hitState);
        }
    }

}
