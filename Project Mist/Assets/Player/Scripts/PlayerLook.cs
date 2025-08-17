using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    private GameObject player;
    [SerializeField] private GameObject cameraHolder;
    [SerializeField] private float sensitivity;

    private bool canLookAround = true;

    float xRotation;
    float yRotation;


    void Start()
    {
        player = this.gameObject;

        Cursor.lockState = CursorLockMode.Locked;
    }


    public void HandleRotation(Vector2 lookInput)
    {
        if (!canLookAround) return;

        // Look left and right
        xRotation = lookInput.x * sensitivity;
        player.transform.Rotate(0, xRotation, 0);

        // Look up and down
        yRotation -= lookInput.y * sensitivity;
        yRotation = Mathf.Clamp(yRotation, -80f, 85f);
        cameraHolder.transform.localRotation = Quaternion.Euler(yRotation, 0, 0);
    }

    public void ChangeActiveState()
    {
        if (canLookAround) canLookAround = false;
        else canLookAround = true;
    }

    public void LookAt(Vector3 lookPoint)
    {
        Debug.Log("BOO");
        Vector3 camPos = cameraHolder.transform.position;

        cameraHolder.transform.LookAt(lookPoint);
    }

    public void DisableLook()
    {
        canLookAround = false;
    }
}
