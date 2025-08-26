using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MeleeWeapon : MonoBehaviour, IEquippable
{
    private Camera cam;
    private AudioSource audioSource;

    [Header("Attacking")]
    [SerializeField] private float attackDistance;
    [SerializeField] private float attackDelay; // time after swing before hit registers
    [SerializeField] private float attackSpeed; // time before next attack
    [SerializeField] private int attackDamage;
    [SerializeField] private LayerMask attackLayer;

    [SerializeField] private GameObject hitEffect;
    [SerializeField] private AudioClip swordSwing;
    [SerializeField] private AudioClip hitSound;

    bool isAttacking = false;
    bool readyToAttack = true;
    int attackCount = 0;

    private void Start()
    {
        cam = FirstPersonCamera.Instance.GetCam();

        audioSource = GetComponent<AudioSource>();
    }

    public void UseItem()
    {
        // Animate Swing
        Debug.Log("Swing");
        Attack();
    }

    public void StopUseItem()
    {
        Debug.Log("Stop swinging");
    }

    private void Attack()
    {
        if (!readyToAttack || isAttacking) return;

        readyToAttack = false;
        isAttacking = true;

        Invoke(nameof(ResetAttack), attackSpeed); // cooldown for next attack
        Invoke(nameof(AttackRaycast), attackDelay); // swing weapon (damage will happen after delay)

        // audioSource.pitch = RandomRange(0.9f, 1.1f);
        // audioSource.PlayOneShot(swordSwing);
    }

    private void AttackRaycast()
    {
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out RaycastHit hit, attackDistance, attackLayer))
        {
            HitTarget(hit.point);

            if (hit.transform.TryGetComponent<EnemyBase>(out EnemyBase enemy))
            {
                //enemy.TakeDamage(attackDamage);
            }
        }
    }

    private void HitTarget(Vector3 pos)
    {
        // audioSource.pitch = 1;
        // audioSource.PlayOneShot(hitSound);

        // GameObject hitEffect = Instantiate(this.hitEffect, pos, Quaternion.identity);
        // Destroy(hitEffect, 20);
    }

    private void ResetAttack()
    {
        readyToAttack = true;
        isAttacking = false;
    }

}
