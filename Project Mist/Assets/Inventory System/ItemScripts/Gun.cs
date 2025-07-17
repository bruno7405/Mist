using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class Gun : MonoBehaviour, IEquippable
{
    [SerializeField] enum FireMode { SemiAuto, Automatic, Burst }
    [SerializeField] FireMode fireMode = FireMode.SemiAuto;

    [SerializeField] Transform firePoint;
    [SerializeField] LayerMask mask;

    [Header("Fire Settings")]
    [SerializeField] float fireRate = 0.1f; // Time between shots
    [SerializeField] int burstCount = 3;
    [SerializeField] float burstDelay = 0.1f;

    [Header("Ammo Settings")]
    [SerializeField] int magazineSize = 30;
    [SerializeField] int totalAmmo = 90;
    [SerializeField] float reloadTime = 2f;

    [Header("Effects")]
    [SerializeField] Animator animator;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip fireSfx;
    [SerializeField] AudioClip reloadSfx;
    [SerializeField] float firePitchRandomization;   
    [SerializeField] MuzzleFlash muzzleFlashLight;
    [SerializeField] ParticleSystem muzzleFlashParticles;
    [SerializeField] TrailRenderer bulletTrail;

    private int currentAmmo;
    private bool isFiring = false;
    private bool isReloading = false;
    private float lastFireTime;
    private Coroutine burstCoroutine;

    private PlayerManager playerManager;

    void Start()
    {
        currentAmmo = magazineSize;
    }

    public void UseItem()
    {
        if (isReloading) return;

        switch (fireMode)
        {
            case FireMode.SemiAuto:
                TryFire();
                break;

            case FireMode.Automatic:
                isFiring = true;
                break;

            case FireMode.Burst:
                if (burstCoroutine == null)
                    burstCoroutine = StartCoroutine(BurstFire());
                break;
        }
    }

    public void StopUseItem()
    {
        isFiring = false;

        if (burstCoroutine != null)
        {
            StopCoroutine(burstCoroutine);
            burstCoroutine = null;
        }
    }

    void Update()
    {
        if (fireMode == FireMode.Automatic && isFiring && !isReloading)
        {
            if (Time.time >= lastFireTime + fireRate)
            {
                TryFire();
            }
        }


    }

    void TryFire()
    {
        if (currentAmmo <= 0)
        {
            Reload();
            return;
        }

        Fire();
    }

    /// <summary>
    /// Fires a single bullet
    /// </summary>
    void Fire()
    {
        lastFireTime = Time.time;
        currentAmmo--;

        // raycast and damage
        Vector3 direction = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f)).direction;

        if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f)), out RaycastHit hit, 500, mask))
        {
            Vector3 dir = hit.point - firePoint.position;

            if (hit.collider.gameObject.TryGetComponent<EnemyBase>(out EnemyBase enemy))
            {
                Debug.Log("enemy hit");
            }            
        }

        /*
        var trail = Instantiate(bulletTrail, firePoint.position, Quaternion.LookRotation(hit.normal));
        StartCoroutine(SpawnTrail(trail, hit));
        */

        // Sound Effects
        animator.SetTrigger("shoot");
        audioSource.pitch = Random.Range(1 - firePitchRandomization, 1 + firePitchRandomization);
        audioSource.PlayOneShot(fireSfx);

        // Visual Effects
        StartCoroutine(muzzleFlashLight.Flash());
        muzzleFlashParticles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear); 
        muzzleFlashParticles.Play();
    }

    IEnumerator BurstFire()
    {
        int shotsFired = 0;

        while (shotsFired < burstCount && currentAmmo > 0 && !isReloading)
        {
            if (Time.time >= lastFireTime + fireRate)
            {
                Fire();
                shotsFired++;
                yield return new WaitForSeconds(burstDelay);
            }
            else
            {
                yield return null;
            }
        }

        burstCoroutine = null;
    }

    public void Reload()
    {
        if (isReloading || currentAmmo == magazineSize || totalAmmo <= 0)
            return;

        StartCoroutine(ReloadRoutine());
    }

    IEnumerator ReloadRoutine()
    {
        isReloading = true;
        Debug.Log("Reloading...");

        yield return new WaitForSeconds(reloadTime);

        int neededAmmo = magazineSize - currentAmmo;
        int ammoToReload = Mathf.Min(neededAmmo, totalAmmo);

        currentAmmo += ammoToReload;
        totalAmmo -= ammoToReload;

        isReloading = false;
    }

    private IEnumerator SpawnTrail(TrailRenderer trail, RaycastHit hit)
    {
        float time = 0;
        Vector3 startPosition = trail.transform.position;

        while (time < 0.1f)
        {
            trail.transform.position = Vector3.Lerp(startPosition, hit.point, time);
            time += Time.deltaTime;
            yield return null;
        }

        Destroy(trail.gameObject, trail.time);

    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(firePoint.position, 0.015f);
    }
}
