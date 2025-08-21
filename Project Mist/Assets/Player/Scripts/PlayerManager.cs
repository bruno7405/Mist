using System.Collections;
using Unity.Cinemachine;
using UnityEditor.Rendering;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;
    [SerializeField] PlayerInput playerInput;
    [SerializeField] PlayerMovement playerMovement;
    [SerializeField] PlayerLook playerLook;
    [SerializeField] CharacterController characterController;

    [SerializeField] Transform cameraAnchor;
    [SerializeField] Rigidbody cameraRb;
    [SerializeField] BoxCollider cameraCollider;

    [SerializeField] CinemachineImpulseSource jumpscareShake;

    public GameObject player;

    private int health = 100; 

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }

    }

    public void TakeDamage(int i)
    {
        health -= i;

        if (health <= 0) StartCoroutine(Death());
    }

    public IEnumerator Death()
    {
        // Stop player input and movement
        playerInput.active = false;
        playerMovement.enabled = false;
        characterController.enabled = false;

        // Reset camera position and rotation
        cameraRb.isKinematic = false;
        cameraCollider.isTrigger = false;

        // Fade to black
        yield return new WaitForSeconds(1f);    

        GameManager.OnLevelReset();

        // Enable player input and movement
        playerInput.active = true;
        playerMovement.enabled = true;
        characterController.enabled = true;

        // Reset camera position and rotation
        cameraRb.isKinematic = true;
        cameraCollider.isTrigger = true;
        cameraRb.transform.position = cameraAnchor.position;
        cameraRb.transform.rotation = Quaternion.identity;

    }

    public void TriggerJumpscare(Vector3 pos)
    {
        playerLook.LookAt(pos);
        jumpscareShake.GenerateImpulse();
        StartCoroutine(Death());
    }


}
