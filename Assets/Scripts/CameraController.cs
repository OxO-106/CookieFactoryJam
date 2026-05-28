using UnityEngine;

public class MouseCamera : MonoBehaviour
{
    public Transform target;

    public Vector3 offset = new Vector3(0, 3, -5);

    public float sensitivity = 3f;

    float rotationX = 0f;
    float rotationY = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void LateUpdate()
    {
        rotationX += Input.GetAxis("Mouse X") * sensitivity;
        rotationY -= Input.GetAxis("Mouse Y") * sensitivity;

        rotationY = Mathf.Clamp(rotationY, -30f, 60f);

        Quaternion rotation = Quaternion.Euler(rotationY, rotationX, 0);

        transform.position = target.position + rotation * offset;
        transform.LookAt(target.position + Vector3.up * 1.5f);
    }
}