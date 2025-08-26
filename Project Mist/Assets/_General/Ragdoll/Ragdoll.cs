using UnityEngine;
using UnityEngine.AI;

public class Ragdoll : MonoBehaviour
{

    [SerializeField] GameObject parent;
    Rigidbody[] rigidbodies;
    Collider[] colliders;

    private void Awake()
    {
        rigidbodies = GetComponentsInChildren<Rigidbody>();
        colliders = GetComponentsInChildren<Collider>();
        SetRigidbodies(false);
        SetColliders(false);
    }

    public void ActivateRagdoll(Vector3 hitPoint, float force)
    {
        DisableComponents();
        SetRigidbodies(true);
        SetColliders(true);
        Knockback(hitPoint, force);
    }

    private void DisableComponents()
    {
        if (parent.TryGetComponent(out Animator animator))
            animator.enabled = false;

        if (parent.TryGetComponent(out Rigidbody rb))
            rb.isKinematic = true;

        if (parent.TryGetComponent(out NavMeshAgent agent))
            agent.enabled = false;

        if (parent.TryGetComponent(out Collider collider))
            collider.enabled = false;
    }

    /// <summary>
    /// Turns ragdoll rb on when state is true, off when false
    /// </summary>
    /// <param name="state"></param>
    private void SetRigidbodies(bool state)
    {
        foreach (Rigidbody rb in rigidbodies)
        {
            rb.isKinematic = !state;
        }
    }

    private void SetColliders(bool state)
    {
        foreach (Collider c in colliders)
        {
            c.enabled = state;
        }
    }

    private void Knockback(Vector3 hitPoint, float force)
    {
        foreach (Rigidbody rb in rigidbodies)
        {
            rb.AddExplosionForce(force, hitPoint, 40f);
        }
    }
}
