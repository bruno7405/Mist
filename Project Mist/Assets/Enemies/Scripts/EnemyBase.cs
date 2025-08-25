using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    int currentHealth;
    public int maxHealth;

    [SerializeField] StateMachineManager stateMachine;
    [SerializeField] State hitState;

    private void Awake()    
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int amt)
    {
        currentHealth -= amt;

        stateMachine.SetNewState(hitState);

        if (currentHealth <= 0) Death();
    }

    private void Death() 
    {
        Debug.Log("Dead");
        Destroy(gameObject);
    }
}
