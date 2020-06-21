using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float moveSpeed = 6f;
    public float jumpSpeed = 8f;
    public float jumpVelocity = 0f;
    public float gravity = 20f;
    public float lookSensitivity = 100f;
    public float xClampRotation = 80f;
    public Transform camera;

    private float rotationY = 0f;
    private float rotationX = 0f;
    private float xAxis;
    private float yAxis;
    private Vector3 moveDirection = Vector3.zero;
    private CharacterController characterController;
    private Quaternion localRotation;

    // Start is called before the first frame update
    void Start()
    {
        Vector3 _rot = transform.localRotation.eulerAngles;
        Vector3 _cameraRotation = camera.localRotation.eulerAngles;

        rotationY = _rot.y;
        rotationX = _cameraRotation.x;
        characterController = GetComponent<CharacterController>();

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        // Look
        xAxis = Input.GetAxis("Mouse X");
        yAxis = -Input.GetAxis("Mouse Y");

        rotationY += xAxis * lookSensitivity * Time.deltaTime;
        rotationX += yAxis * lookSensitivity * Time.deltaTime;
        rotationX = Mathf.Clamp(rotationX, -xClampRotation, xClampRotation);
        
        transform.rotation = Quaternion.Euler(0f, rotationY, 0f);
        camera.localRotation = Quaternion.Euler(rotationX, 0f, 0f);
        
        moveDirection = transform.right * Input.GetAxis("Horizontal") + transform.forward * Input.GetAxis("Vertical"); 
        moveDirection *= moveSpeed;
        moveDirection += transform.up * jumpVelocity;
        
        if (characterController.isGrounded) {
            if (jumpVelocity < 0f) {
                jumpVelocity = -2f;
            }

            if (Input.GetButtonDown("Jump")) {
                jumpVelocity = Mathf.Sqrt(jumpSpeed * 2f * gravity);
            }
        }

        jumpVelocity -= gravity * Time.deltaTime;
        characterController.Move(moveDirection * Time.deltaTime);
    }
}
