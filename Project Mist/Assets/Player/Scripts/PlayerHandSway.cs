using UnityEngine;

public class PlayerHandSway : MonoBehaviour
{
    [Header("Sway Settings")]
    public float swayAmount = 0.02f;       // How far the hand moves
    public float maxSwayAmount = 0.06f;    // Limit of movement
    public float smoothAmount = 6f;        // How quickly it returns to center

    [Header("Rotation Sway Settings")]
    public float rotationSway = 2f;        // Tilt amount
    public float maxRotationSway = 5f;     // Tilt limit
    public float smoothRotation = 6f;      // How quickly it stabilizes

    private Vector3 initialPosition;
    private Quaternion initialRotation;

    void Start()
    {
        initialPosition = transform.localPosition;
        initialRotation = transform.localRotation;
    }

    void Update()
    {
        // Get mouse input
        float moveX = -Input.GetAxis("Mouse X");
        float moveY = -Input.GetAxis("Mouse Y");

        // --- Position sway ---
        Vector3 finalPosition = new Vector3(
            Mathf.Clamp(moveX * swayAmount, -maxSwayAmount, maxSwayAmount),
            Mathf.Clamp(moveY * swayAmount, -maxSwayAmount, maxSwayAmount),
            0
        );

        transform.localPosition = Vector3.Lerp(
            transform.localPosition,
            initialPosition + finalPosition,
            Time.deltaTime * smoothAmount
        );

        // --- Rotation sway ---
        Quaternion finalRotation = Quaternion.Euler(
            Mathf.Clamp(moveY * rotationSway, -maxRotationSway, maxRotationSway),
            Mathf.Clamp(-moveX * rotationSway, -maxRotationSway, maxRotationSway),
            0
        );

        transform.localRotation = Quaternion.Slerp(
            transform.localRotation,
            initialRotation * finalRotation,
            Time.deltaTime * smoothRotation
        );
    }
}
