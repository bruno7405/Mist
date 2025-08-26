using System.Collections;
using UnityEngine;

public class DeathState : State
{
    [SerializeField] Ragdoll ragdoll;
    [SerializeField] ParticleSystem deathParticles;

    private Vector3 hitPoint;
    private float force;


    public override void OnStart()
    {
        Debug.Log("Enemy Died");
        ragdoll.ActivateRagdoll(hitPoint, force);
        StartCoroutine(DeathEffects());
    }

    public override void OnUpdate()
    {
    }

    IEnumerator DeathEffects()
    {
        deathParticles.Play();
        yield return new WaitForSeconds(deathParticles.main.duration * 2);

        //Destroy();
    }


    public void SetHitPoint(Vector3 point)
    {
        hitPoint = point;
    }

    public void SetHitForce(float f)
    {
        force = f;
    }
}
