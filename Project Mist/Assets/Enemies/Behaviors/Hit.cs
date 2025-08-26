using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Hit : State
{
    EnemyBase enemy;
    Animator animator;
    NavMeshAgent agent;
    [SerializeField] State chaseState;
    [SerializeField] AnimationClip hurtAnimClip;

    private float clipLength;
    private float timer = 0;

    private void Awake()
    {
        enemy = parent.GetComponent<EnemyBase>();
        animator = enemy.GetComponent<Animator>();
        agent = enemy.GetComponent<NavMeshAgent>();
        clipLength = hurtAnimClip.length;
    }

    public override void OnStart()
    {
        timer = 0;
        agent.isStopped = true;
        animator.SetTrigger("hit");
    }

    public override void OnUpdate()
    {
        timer += Time.deltaTime;
        if (timer >= clipLength)
        {
            timer = 0;
            stateMachine.SetNewState(chaseState);
        }
    }
}
