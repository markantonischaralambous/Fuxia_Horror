using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public float speed = 5f;
    public float mouseSensitivity = 100f;

    private float xRotation = 0f;
    private Transform cameraTransform;

    // Gravity variables
    private float gravity = -9.81f;
    private Vector3 velocity;

    void Start()
    {
        // Automatically finds the camera inside the Player object
        cameraTransform = GetComponentInChildren<Camera>().transform;

        // Locks the cursor so you can look around like a real horror game
        Cursor.lockState = CursorLockMode.Locked;

        // Safety check: ensure the controller is assigned
        if (controller == null)
            controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        // 1. MOUSE LOOK LOGIC
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // Stops you from looking behind your own neck

        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);

        // 2. MOVEMENT LOGIC
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // Move relative to where the player is facing
        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * speed * Time.deltaTime);

        // 3. GRAVITY LOGIC
        // If we are on the ground, reset the falling speed
        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        // 4. UTILITY
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None; // Unlocks mouse so you can stop the game
        }
    }
}