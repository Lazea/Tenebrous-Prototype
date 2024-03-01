using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Params")]
    public float speed = 3f;
    public float elevationSpeed = 3f;
    public float acceleration = 0.25f;
    public float deceleration = 0.25f;

    Vector3 velocity;

    Rigidbody rb;
    Controls.GameplayActions controls;

    private void Awake()
    {
        controls = new Controls().Gameplay;
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 movement = controls.Movement.ReadValue<Vector2>();
        float elevation = controls.Elevation.ReadValue<float>();

        Vector3 _velocity = new Vector3(movement.x, 0f, movement.y);
        _velocity = Camera.main.transform.TransformDirection(_velocity) * speed;

        float t = (movement.magnitude > 0f) ? acceleration : deceleration;
        velocity = Vector3.Lerp(
            rb.velocity,
            _velocity,
            t);

        t = (Mathf.Abs(elevation) > 0f) ? acceleration : deceleration;
        velocity.y = Mathf.Lerp(
            velocity.y,
            elevation * elevationSpeed,
            t);

        rb.velocity = velocity;
    }
}
