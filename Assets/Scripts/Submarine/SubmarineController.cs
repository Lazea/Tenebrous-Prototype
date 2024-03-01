using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubmarineController : MonoBehaviour
{
    [Header("Rotation Limits")]
    public float maxXRotation = 45f;
    public float maxYRotation = 45f;
    public float maxZRotation = 45f;

    [Header("Alignment Torque")]
    public float alignmentStrength = 10f;   // How strongly the sub realigns to original rotation

    // Components
    public Rigidbody rb;

    void FixedUpdate()
    {
        LimitRotation();
        ApplyAlignmentTorque();
    }

    void LimitRotation()
    {
        Vector3 currentRotation = transform.localEulerAngles;
        currentRotation.x = ClampAngle(currentRotation.x, maxXRotation);
        currentRotation.y = ClampAngle(currentRotation.y, maxYRotation);
        currentRotation.z = ClampAngle(currentRotation.z, maxZRotation);

        rb.MoveRotation(Quaternion.Euler(currentRotation));
    }

    float ClampAngle(float angle, float maxAngle)
    {
        angle = NormalizeAngle(angle);
        return Mathf.Clamp(angle, -maxAngle, maxAngle);
    }

    float NormalizeAngle(float angle)
    {
        while (angle > 180) angle -= 360;
        while (angle < -180) angle += 360;
        return angle;
    }

    void ApplyAlignmentTorque()
    {
        Vector3 desiredRotation = Vector3.zero; // The target rotation (0, 0, 0)
        Vector3 currentRotation = transform.localEulerAngles;
        currentRotation = new Vector3(NormalizeAngle(currentRotation.x), NormalizeAngle(currentRotation.y), NormalizeAngle(currentRotation.z));

        Vector3 rotationDifference = desiredRotation - currentRotation;
        Vector3 torque = rotationDifference * alignmentStrength;

        rb.AddTorque(torque);
    }
}
