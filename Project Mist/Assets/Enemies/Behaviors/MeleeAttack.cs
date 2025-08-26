using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class MeleeAttack : State
{

    // Behavior Params
    [Range(0, 100)]
    [SerializeField] private int damage = 50;
    [SerializeField] Transform center;
    [SerializeField] float radius;
    private float aggroDistance;
    private float attackDistance;

    // General
    EnemyBase enemy;
    NavMeshAgent agent;
    GameObject player;
    Vector3 playerPos;

    // Animation
    Animator animator;
    [SerializeField] AnimationClip attackAnimation;

    // Next State
    [SerializeField] State chaseState;

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

        agent.isStopped = true; // stop moving
        animator.SetTrigger("attack"); // swing animation
        StartCoroutine(DamageTick());
    }

    public override void OnUpdate() 
    {
        playerPos = player.transform.position;
        Vector3 dir = (playerPos - enemy.transform.position).normalized;
        dir.y = 0;
        Quaternion targetRot = Quaternion.LookRotation(dir);
        enemy.transform.rotation = Quaternion.RotateTowards(enemy.transform.rotation, targetRot, Time.deltaTime * (360f / 1f));
    }

    private IEnumerator DamageTick()
    {

        yield return new WaitForSeconds(attackAnimation.length / 2);

        // Damage player
        Collider[] players = Physics.OverlapSphere(center.position, radius, LayerMask.GetMask("Player")); // Layer Mask 3 = player
        foreach (var player in players)
        {
            if (player.TryGetComponent<PlayerManager>(out PlayerManager playerManager))
                playerManager.TakeDamage(damage);
        }

        yield return new WaitForSeconds(attackAnimation.length / 2);

        TransitionToNextState();
    }

    private void TransitionToNextState()
    {
        var distanceFromPlayer = Vector3.Distance(enemy.transform.position, player.transform.position);

        // Attack player when close
        if (distanceFromPlayer <= attackDistance)
        {
            OnStart(); // attack again
        }

        // Idle when too far away
        else if (distanceFromPlayer > aggroDistance)
        {
            stateMachine.SetNewState(chaseState);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(center.position, radius);
    }


}
