using UnityEngine;

public class PlayerCameraLook : MonoBehaviour
{
    [Header("Transforms")]
    public Transform body;
    public Transform camera;

    [Header("Params")]
    public float minYRotation = -90f;   // Minimum camera rotation
    public float maxYRotation = 90f;    // Maximum camera rotation
    public bool clampXRotation = false; // Toggle body clamping 
    public float xRotationClamp = 90f;  // Body clamp rotation value
    public float XSensitivity
    {
        get { return GameSettings.xSensitivity * 400f; }
    }
    public float YSensitivity
    {
        get { return GameSettings.ySensitivity * 400f; }
    }
    public bool YInverted { get { return GameSettings.yInverted; } }

    private float xRotation = 0f; // Initial rotation around the X axis
    private float yRotation = 0f; // Initial rotation around the Y axis

    public float animSwaySmooth = 0.1f;
    Vector3 camRotDelta;
    Vector3 prevCameraWorldRotation;

    // Components
    Controls.GameplayActions controls;
    Animator anim;

    private void Awake()
    {
        if (camera == null)
            camera = transform;

        controls = new Controls().Gameplay;

        yRotation = body.eulerAngles.y;

        anim = GetComponentInParent<Animator>();

        prevCameraWorldRotation = camera.rotation.eulerAngles;
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
        UpdateAniamtor();
    }

    void Look(Vector2 lookDelta)
    {
        float mouseX = lookDelta.x * XSensitivity * Time.deltaTime;
        float mouseY = lookDelta.y * YSensitivity * Time.deltaTime;

        // Invert mouseY if invertY is true
        if (YInverted)
            mouseY *= -1;

        if(clampXRotation)
        {
            // Calculate new Y rotation, clamped between -xRotationClamp and xRotationClamp
            yRotation += mouseX;
            yRotation = Mathf.Clamp(yRotation, -xRotationClamp, xRotationClamp);

            // Apply clamped rotation to body around the Y axis
            body.localRotation = Quaternion.Euler(0f, yRotation, 0f);
        }
        else
        {
            // Apply rotation to body around the Y axis
            body.Rotate(Vector3.up * mouseX);
        }

        // Calculate new X rotation, clamped between minYRotation and maxYRotation
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, minYRotation, maxYRotation);

        // Apply rotation to camera around the X axis
        camera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }

    void UpdateAniamtor()
    {
        camRotDelta = prevCameraWorldRotation - camera.rotation.eulerAngles;
        camRotDelta *= Time.deltaTime;

        Vector2 animLook = new Vector2(anim.GetFloat("LookX"), anim.GetFloat("LookY"));
        animLook = Vector2.Lerp(
            animLook,
            new Vector2(camRotDelta.y, camRotDelta.x).normalized,
            animSwaySmooth);
        anim.SetFloat("LookX", animLook.x);
        anim.SetFloat("LookY", animLook.y);

        prevCameraWorldRotation = camera.rotation.eulerAngles;
    }
}

