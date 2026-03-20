using UnityEngine;

public class FPSController : MonoBehaviour
{
    public float speed = 5f;
    public float mouseSensitivity = 100f;
    public Transform playerCamera;

    public float gravity = -9.81f;
    public float jumpHeight = 1.5f;

    float xRotation = 0f;
    float yVelocity;
    CharacterController controller;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        Move();
        Look();
    }

    void Move()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        if (controller.isGrounded && yVelocity < 0)
        {
            yVelocity = -2f; 
        }

        if (Input.GetButtonDown("Jump") && controller.isGrounded)
        {
            yVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        yVelocity += gravity * Time.deltaTime;

        Vector3 move = transform.right * x + transform.forward * z;
        move.y = yVelocity;

        controller.Move(move * speed * Time.deltaTime);
    }

    void Look()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }
}