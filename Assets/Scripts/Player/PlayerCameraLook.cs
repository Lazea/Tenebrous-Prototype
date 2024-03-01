using UnityEngine;

public class PlayerCameraLook : MonoBehaviour
{
    [Header("Transforms")]
    public Transform body;
    public Transform camera;

    [Header("Params")]
    public float xSensitivity = 100f;
    public float ySensitivity = 100f;
    public float minYRotation = -90f; // Minimum camera rotation
    public float maxYRotation = 90f; // Maximum camera rotation
    public bool invertY = false; // Invert Y axis

    private float xRotation = 0f; // Initial rotation around the X axis

    Controls.GameplayActions controls;

    private void Awake()
    {
        if (camera == null)
            camera = transform;

        controls = new Controls().Gameplay;
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    private void Update()
    {
        Look(controls.Look.ReadValue<Vector2>());
    }

    void Look(Vector2 lookDelta)
    {
        float mouseX = lookDelta.x * xSensitivity * Time.deltaTime;
        float mouseY = lookDelta.y * ySensitivity * Time.deltaTime;

        // Invert mouseY if invertY is true
        if (invertY)
            mouseY *= -1;

        // Apply rotation to body around the Y axis
        body.Rotate(Vector3.up * mouseX);

        // Calculate new X rotation, clamped between minYRotation and maxYRotation
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, minYRotation, maxYRotation);

        // Apply rotation to camera around the X axis
        camera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }
}

