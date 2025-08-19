using UnityEngine;

public class PlayerHeadBob : MonoBehaviour
{
    [SerializeField, Range(0, 0.01f)] private float camYAmplitude = 0.015f;
    [SerializeField, Range(0, 0.01f)] private float camXAmplitude = 0.015f;
    [SerializeField, Range(0, 30)] private float frequency = 10f;
    private float sprintMulitplier = 1.5f;

    [SerializeField] Transform cam = null;
    [SerializeField] Transform cameraHolder = null;

    private Vector3 startPos;
    private PlayerMovement playerMovement;

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        startPos = cam.localPosition;

        sprintMulitplier = playerMovement.GetSprintMultiplier();
    }

    private void Update()
    {
        CheckMotion();
        ResetCamPos();

        cam.LookAt(FocusTarget());
    }

    private void HeadBob(float mulitplier)
    {
        Vector3 difference = Vector3.zero;
        difference.y += Mathf.Sin(Time.time * frequency * mulitplier) * camYAmplitude;
        difference.x += Mathf.Cos(Time.time * frequency * mulitplier / 2) * camXAmplitude;

        cam.localPosition += difference;
    }

    /// <summary>
    /// Checks if player is moving on the ground
    /// </summary>
    private void CheckMotion()
    {
        // Movement and grounded checks
        if (!playerMovement.isGrounded) return; 
        if (!playerMovement.GetIsWalking()) return;

        // Walking
        if (!playerMovement.GetIsSprinting()) HeadBob(1f);

        // Sprinting
        else HeadBob(sprintMulitplier);

    }

    private void ResetCamPos()
    {
        if (cam.localPosition == startPos) return;
        cam.localPosition = Vector3.Lerp(cam.localPosition, startPos, 1 * Time.deltaTime);
    }

    private Vector3 FocusTarget()
    {
        Vector3 pos = new Vector3(transform.position.x, transform.position.y + cameraHolder.localPosition.y, transform.position.z);
        pos += cameraHolder.forward * 15f;
        return pos;
    }
}
