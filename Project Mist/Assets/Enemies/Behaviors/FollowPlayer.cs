using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Audio;

public class FollowPlayer : State
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

    // Next state
    [SerializeField] State idleState;
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
        agent.isStopped = false; // allow movement
        animator.SetTrigger("run");
    }

    public override void OnUpdate()
    {
        agent.SetDestination(player.transform.position); // track and go towards player

        var distanceFromPlayer = Vector3.Distance(enemy.transform.position, player.transform.position);

        // Attack player when close
        if (distanceFromPlayer <= attackDistance)
        {
            stateMachine.SetNewState(meleeAttackState);
        }

        // Idle when too far away
        else if (distanceFromPlayer > aggroDistance)
        {
            stateMachine.SetNewState(idleState);
        }


    }
}
