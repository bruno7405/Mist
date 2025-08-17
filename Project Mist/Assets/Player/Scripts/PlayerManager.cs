using Unity.Cinemachine;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;
    [SerializeField] PlayerInput playerInput;
    [SerializeField] PlayerMovement playerMovement;
    [SerializeField] PlayerLook playerLook;
    
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

        if (health <= 0) Death();
    }

    public void Death()
    {

    }

    public void TriggerJumpscare(Vector3 pos)
    {
        playerLook.LookAt(pos);
        PlayerInput.active = false;
        jumpscareShake.GenerateImpulse();
    }


}
