using UnityEngine;

public class BlowTorchTool : MonoBehaviour
{
    [Header("Stats")]
    public float torchRange = 1f;
    public float torchRadius = 0.2f;
    public LayerMask mask;

    bool useTorch;

    GameObject objectTorched;

    // Components
    Controls.GameplayActions controls;
    Animator anim;
    AnimatorStateInfo animStateInfo
    {
        get { return anim.GetCurrentAnimatorStateInfo(0); }
    }

    void Awake()
    {
        anim = GetComponentInParent<Animator>();

        controls = new Controls().Gameplay;

        controls.Action.started += ctx => useTorch = true;
        controls.Action.canceled += ctx => useTorch = false;
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
        useTorch = false;
        anim.SetBool("BlowTorching", false);
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetBool("BlowTorching", useTorch);
    }

    private void FixedUpdate()
    {
        GameObject _objectTorched = null;
        if (animStateInfo.IsName("BlowTorching"))
        {
            Ray ray = new Ray(
            Camera.main.transform.position,
            Camera.main.transform.forward);
            RaycastHit hit;
            if (Physics.SphereCast(
                ray,
                torchRadius,
                out hit,
                torchRange,
                mask))
            {
                _objectTorched = hit.collider.gameObject;
                // TODO: Play a torch particle effect on submarine if hit
            }
        }

        objectTorched = _objectTorched;
    }

    public void Repair()
    {
        if (objectTorched == null)
            return;

        // TODO: Perform a repair if objectTorched is the submarine
        Debug.Log("Repair");
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector3 startPoint = Camera.main.transform.position;
        Vector3 endPoint = Camera.main.transform.forward * torchRange;
        endPoint += startPoint;
        Gizmos.DrawLine(startPoint, endPoint);
        Gizmos.DrawWireSphere(endPoint, torchRadius);
    }
}
