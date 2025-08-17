using UnityEngine;

public class Jumpscare : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        var playerManager = other.GetComponent<PlayerManager>();
        if (playerManager == null) return;

        playerManager.TriggerJumpscare(transform.position);
    }
}
