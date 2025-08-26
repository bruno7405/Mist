using UnityEngine;
using UnityEngine.AI;

public class Idle : State
{
    // Behavior Params
    private float aggroDistance;
    private float attackDistance;

    // General
    EnemyBase enemy;
    NavMeshAgent agent;
    GameObject player;

    // Animation
    Animator animator;

    // Next State
    [SerializeField] State followPlayer;
    [SerializeField] State meleeAttackState;

    private void Awake()
    {
        enemy = parent.GetComponent<EnemyBase>();
        agent = parent.GetComponent<NavMeshAgent>();
        animator = parent.GetComponent<Animator>();
        aggroDistance = enemy.aggroDistance;
        attackDistance = enemy.attackDistance;
    }

    public override void OnStart()
    {
        player = PlayerManager.instance.player;
        animator.SetTrigger("idle");
        agent.isStopped = true;
    }

    public override void OnUpdate()
    {
        var distanceFromPlayer = Vector3.Distance(enemy.transform.position, player.transform.position);
        if (distanceFromPlayer <= attackDistance) stateMachine.SetNewState(meleeAttackState);
        else if (distanceFromPlayer <= aggroDistance) stateMachine.SetNewState(followPlayer);
    }
}
