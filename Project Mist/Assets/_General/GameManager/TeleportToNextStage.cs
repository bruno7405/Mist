using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class TeleportToNextStage : MonoBehaviour
{

    // Cabin Reference
    [SerializeField] private Transform cabin;
    [SerializeField] private Transform nextLevelCabin;

    // Player Reference
    private PlayerManager player;
    private PlayerInput playerInput;
    private PlayerMovement playerMovement;
    private CharacterController playerController;


    private void Awake()
    {
        cabin = transform;
    }
    private void Start()
    {
        player = PlayerManager.instance;
        playerInput = player.GetComponent<PlayerInput>();
        playerMovement = player.GetComponent<PlayerMovement>();
        playerController = player.GetComponent<CharacterController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Play Sound

            StartCoroutine(Teleport());
        }
    }

    private IEnumerator Teleport()
    {
        playerInput.active = false;
        playerMovement.enabled = false;

        // Get player position and rotation relative to the cabin
        Vector3 relativePos = cabin.InverseTransformPoint(player.transform.position);
        Quaternion relativeRot = Quaternion.Inverse(cabin.rotation) * player.transform.rotation;

        // Teleport player at the exact postion and rotation of the next level spawn
        playerController.enabled = false;
        player.transform.position = nextLevelCabin.position + relativePos;
        player.transform.rotation = nextLevelCabin.rotation * relativeRot;
        playerController.enabled = true;

        yield return new WaitForSeconds(1);

        playerInput.active = true;
        playerMovement.enabled = true;
        GameManager.OnLevelProgressed();
    }
}
