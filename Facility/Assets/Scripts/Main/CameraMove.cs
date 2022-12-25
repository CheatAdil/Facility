using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public Transform playerBody;
    public float Sensitivity = 100f;
    private float mouseX;
    private float mouseY;
    private float xRotation = 0f;
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void Update()
    {
        mouseX = Input.GetAxis("Mouse X") * Sensitivity * Time.deltaTime;
        mouseY = Input.GetAxis("Mouse Y") * Sensitivity * Time.deltaTime;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);
    }
}