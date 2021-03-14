using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public GameObject gameControl;

    public float mouseSensitivity = 100f;

    public Transform playerBody;

    private float xRotation = 0f;

    public Transform pickupCenter;

    public float mouseHSpeed;
    public float mouseVSpeed;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameControl.GetComponent<GameControl>().paused && playerBody.GetComponent<PlayerMovement>().alive)
        {

            Cursor.lockState = CursorLockMode.Locked;
            float mouseX = Input.GetAxis("Mouse X") * mouseHSpeed * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseVSpeed * Time.deltaTime;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90);

            transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            
            playerBody.Rotate(Vector3.up * mouseX);
            
            pickupCenter.localRotation = Quaternion.Euler(xRotation, playerBody.transform.rotation.y, playerBody.transform.rotation.z);
        } else
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }
}
