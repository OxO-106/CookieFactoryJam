using UnityEngine;

public class SimplePlayerController : MonoBehaviour
{
    public CharacterController controller;
    public Transform cameraTransform;

    public float moveSpeed = 6f;
    public float jumpSpeed = 8f;
    public float fallSpeed = 16f;

    private Vector3 velocity;
    private bool isGrounded;

    void Update()
    {
        isGrounded = controller.isGrounded;

        // input
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;
        forward.y = 0f; right.y = 0f;
        forward.Normalize(); right.Normalize();

        Vector3 move = forward * z + right * x;
        controller.Move(move * moveSpeed * Time.deltaTime);

        if (move != Vector3.zero)
            transform.forward = move;

        // jump
        if (Input.GetButtonDown("Jump") && isGrounded)
            velocity.y = jumpSpeed;
        else if (isGrounded)
            velocity.y = -2f;
        else
            velocity.y -= fallSpeed * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }
}