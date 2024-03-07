using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubmarineController : MonoBehaviour
{
    [Header("Movement")]
    public float thrustForce = 350f;
    public float elevationThrustForce = 225f;
    public float turnTorque = 100f;
    public bool controlsEnabled;

    [Header("Rotation Limits")]
    public float maxXRotation = 25f;

    [Header("Alignment Torque")]
    public float alignmentStrength = 10f;   // How strongly the sub realigns to original rotation

    // Components
    Rigidbody rb;
    Controls.SubGameplayActions controls;

    private void Awake()
    {
        controls = new Controls().SubGameplay;
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        if (controlsEnabled)
            controls.Enable();
    }

    private void OnEnable()
    {
        if(controlsEnabled)
            controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    public void EnableControls(bool enable)
    {
        controlsEnabled = enable;
        if (enable)
        {
            controls.Enable();
        }
        else
        {
            controls.Disable();
        }
    }

    void FixedUpdate()
    {
        LimitRotation();
        ApplyAlignmentTorque();
        HandleMovement();
    }

    void HandleMovement()
    {
        Vector2 movement = controls.Movement.ReadValue<Vector2>();
        float elevation = controls.Elevation.ReadValue<float>();

        Vector3 forward = transform.forward;
        forward.y = Mathf.Clamp(forward.y, -0.2f, 0.2f);
        forward.Normalize();

        rb.AddForce(forward * movement.y * thrustForce);
        rb.AddForce(Vector3.up * elevation * elevationThrustForce);

        rb.AddTorque(transform.right * -elevation * 0.1f * turnTorque);
        rb.AddTorque(Vector3.up * movement.x * turnTorque);
    }

    void LimitRotation()
    {
        Vector3 currentRotation = transform.localEulerAngles;
        currentRotation.x = ClampAngle(currentRotation.x, maxXRotation);

        rb.MoveRotation(Quaternion.Euler(currentRotation));
    }

    float ClampAngle(float angle, float maxAngle)
    {
        angle = NormalizeAngle(angle);
        return Mathf.Clamp(angle, -maxAngle, maxAngle);
    }

    void ApplyAlignmentTorque()
    {
        Vector3 desiredRotation = Vector3.zero; // The target rotation (0, 0, 0)
        Vector3 currentRotation = transform.localEulerAngles;
        currentRotation = new Vector3(
            NormalizeAngle(currentRotation.x),
            0f,
            0f);

        Vector3 rotationDifference = desiredRotation - currentRotation;
        Vector3 torque = rotationDifference * alignmentStrength;

        rb.AddTorque(torque);
    }

    float NormalizeAngle(float angle)
    {
        while (angle > 180) angle -= 360;
        while (angle < -180) angle += 360;
        return angle;
    }
}
