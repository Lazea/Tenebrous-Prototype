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
        ApplyAlignmentTorque();
        LimitRotation();
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
        currentRotation.z = ClampAngle(currentRotation.x, 5f);

        rb.MoveRotation(Quaternion.Euler(currentRotation));
    }

    float ClampAngle(float angle, float maxAngle)
    {
        angle = NormalizeAngle(angle);
        return Mathf.Clamp(angle, -maxAngle, maxAngle);
    }

    public Vector3 desiredRotation;
    public Vector3 currentRotation;
    public Vector3 rotationDifference;
    public Vector3 torque;

    void ApplyAlignmentTorque()
    {
        desiredRotation = Vector3.zero; // The target rotation (0, 0, 0)
        currentRotation = transform.localEulerAngles;
        desiredRotation.y = currentRotation.y;
        currentRotation = new Vector3(
            NormalizeAngle(currentRotation.x),
            currentRotation.y,
            NormalizeAngle(currentRotation.z));

        rotationDifference = desiredRotation - currentRotation;
        rotationDifference = Vector3.ClampMagnitude(
            rotationDifference / 10f, 1f);

        torque = rotationDifference * alignmentStrength;

        rb.AddRelativeTorque(torque);
    }

    float NormalizeAngle(float angle)
    {
        while (angle > 180) angle -= 360;
        while (angle < -180) angle += 360;
        return angle;
    }
}
