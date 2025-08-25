using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Hit : State
{
    EnemyBase enemy;
    Animator animator;
    NavMeshAgent agent;
    [SerializeField] State chaseState;



    private void Awake()
    {
        enemy = parent.GetComponent<EnemyBase>();
        animator = enemy.GetComponent<Animator>();
        agent = enemy.GetComponent<NavMeshAgent>();
    }

    public override void OnStart()
    {
        StopCoroutine(ChaseState());
        agent.isStopped = true;
        animator.SetTrigger("hit");
        StartCoroutine(ChaseState());
    }

    public override void OnUpdate()
    {

    }

    IEnumerator ChaseState()
    {
        yield return new WaitForSeconds(0.25f);
        stateMachine.SetNewState(chaseState);
    }

}
