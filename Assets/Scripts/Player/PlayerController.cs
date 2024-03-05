using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Params")]
    public float speed = 3f;
    public float elevationSpeed = 3f;
    public float acceleration = 0.25f;
    public float deceleration = 0.25f;

    Vector3 velocity;

    public float animMoveSmooth = 0.2f;

    // Components
    Rigidbody rb;
    Controls.GameplayActions controls;
    Animator anim;

    private void Awake()
    {
        controls = new Controls().Gameplay;
        anim = GetComponent<Animator>();
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

        Vector3 animMove = new Vector2(
            anim.GetFloat("MoveX"),
            anim.GetFloat("MoveY"));
        animMove = Vector2.Lerp(
            animMove,
            movement,
            animMoveSmooth);
        anim.SetFloat("MoveX", animMove.x);
        anim.SetFloat("MoveY", animMove.y);

        float animElevation = Mathf.Lerp(
            anim.GetFloat("MoveZ"),
            elevation,
            animMoveSmooth);
        anim.SetFloat("MoveZ", animElevation);

        anim.SetFloat("Speed", animMove.magnitude);

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
