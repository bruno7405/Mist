using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] CharacterController controller;

    [Header("Movement values")]
    [SerializeField] private float walkSpeed;
    [SerializeField] private float jumpVelocity;
    [SerializeField] private float sprintMultiplier;
    [SerializeField] private bool isWalking = false;
    [SerializeField] private bool isSprinting = false;


    [Header("Jumping values")]
    public bool isGrounded = false;
    private bool canJump;
    private const float GRAVITY = -10f;
    [SerializeField] float gravityMultiplier;
    [SerializeField] private float jumpOffset; // for smooth jumping
    [SerializeField] private LayerMask groundMask; // layer for "ground" gameobjects

    private Vector3 verticalVelocity; // current vertical velocity of player

    private float sphereCastVerticalOffset;
    private Vector3 castOrigin;

    Vector3 moveVector;

    public static PlayerMovement Instance { get; private set; }

    void Awake()
    {
        controller = this.GetComponent<CharacterController>();
        Instance = this;
    }

    void Update()
    {
        sphereCastVerticalOffset = controller.height / 2f - controller.radius;
        castOrigin = transform.position + Vector3.down * sphereCastVerticalOffset;

        Vertical();
        controller.Move(moveVector * Time.deltaTime);
    }

    public void HandleMovement(Vector2 input, InputAction sprintAction)
    {
        if (input.magnitude <= 0)
        {
            isWalking = false;
            moveVector.x = 0;
            moveVector.z = 0;
            return;
        }
        else isWalking = true;

        float speedMultiplier = 1;
        if (sprintAction.ReadValue<float>() <= 0)
        {
            isSprinting = false;
        }
        else
        {
            isSprinting = true;
            speedMultiplier = sprintMultiplier;
        }

        float verticalSpeed = input.y * walkSpeed * speedMultiplier;
        float horizonalSpeed = input.x * walkSpeed * speedMultiplier;
        moveVector.x = (transform.right * horizonalSpeed).x + (transform.forward * verticalSpeed).x;
        moveVector.z = (transform.right * horizonalSpeed).z + (transform.forward * verticalSpeed).z;
    }

    /// <summary>
    /// Jumping Logic
    /// canJump is slightly earlier (more range) than isGrounded for easy jumps
    /// </summary>
    public void HandleJump(InputAction jumpAction)
    {
        canJump = Physics.CheckSphere(castOrigin, controller.radius + jumpOffset, groundMask);
        if (jumpAction.triggered && canJump)
        {
            moveVector.y = jumpVelocity; // set initial velocity (pos)
        }
    }

    /// <summary>
    /// Updates vertical movement of player
    /// Gravity logic
    /// </summary>
    private void Vertical()
    {
        isGrounded = Physics.CheckSphere(castOrigin, controller.radius + 0.02f, groundMask);
        if (!isGrounded) moveVector.y += GRAVITY * gravityMultiplier * Time.deltaTime;
    }

    public bool GetIsWalking()
    {
        return isWalking && controller.velocity.magnitude > 0.05f;
    }

    public bool GetIsSprinting()
    {
        return isSprinting;
    }

    public float GetSprintMultiplier()
    {
        return sprintMultiplier;
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(castOrigin, controller.radius + 0.02f);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(castOrigin, controller.radius + jumpOffset);
    }
}
