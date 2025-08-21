using UnityEngine;
using UnityEngine.AI;

public class MeleeAttack : State
{

    // Behavior Params
    [Range(0, 100)]
    [SerializeField] private int damage = 50;
    [SerializeField] Transform center;
    [SerializeField] float radius;

    // General
    EnemyBase enemy;
    NavMeshAgent agent;
    GameObject player;
    Vector3 playerPos;
    Quaternion lookRotation;

    // Animation
    Animator animator;
    [SerializeField] AnimationClip attackAnimation;

    // Next State
    [SerializeField] State idleState;

    private void Awake()
    {
        enemy = parent.GetComponent<EnemyBase>();
        agent = parent.GetComponent<NavMeshAgent>();
        animator = parent.GetComponent<Animator>();
    }

    public override void OnStart()
    {
        player = PlayerManager.instance.player;

        agent.isStopped = true; // stop moving
        animator.SetTrigger("attack"); // swing animation
        Invoke(nameof(DamageTick), attackAnimation.length / 2); // damage after half the time
        Invoke(nameof(TransitionToIdleState), attackAnimation.length); // switch states after animation ends
    }

    public override void OnUpdate() 
    {
        playerPos = player.transform.position;
        Vector3 dir = (playerPos - enemy.transform.position).normalized;
        dir.y = 0;
        Quaternion targetRot = Quaternion.LookRotation(dir);
        enemy.transform.rotation = Quaternion.RotateTowards(enemy.transform.rotation, targetRot, Time.deltaTime * (360f / 1f));
    }

    private void DamageTick()
    {
        Collider[] players = Physics.OverlapSphere(center.position, radius, LayerMask.GetMask("Player")); // Layer Mask 3 = player
        foreach (var player in players)
        {
            if (player.TryGetComponent<PlayerManager>(out PlayerManager playerManager))
                playerManager.TakeDamage(damage);
        }
    }

    private void TransitionToIdleState()
    {
        stateMachine.SetNewState(idleState);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(center.position, radius);
    }


}
