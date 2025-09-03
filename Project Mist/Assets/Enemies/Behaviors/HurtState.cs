using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class HurtState : State
{
    EnemyBase enemy;
    Animator animator;
    NavMeshAgent agent;

    [SerializeField] State chaseState;

    [Header("Effects")]
    [SerializeField] AnimationClip hurtAnimClip;
    [SerializeField] int numOfAnimations; // for random animations
    [SerializeField] ParticleSystem bloodParticles;
    private float clipLength;
    private float timer = 0;

    public Vector3 hitPoint;


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
        Instantiate(bloodParticles, hitPoint, Quaternion.identity);
        animator.SetInteger("hurtIndex", Random.Range(0, numOfAnimations));
        animator.SetTrigger("hurt");

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
