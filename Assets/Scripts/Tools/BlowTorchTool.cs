using UnityEngine;
using SOGameEventSystem.Events;
using UnityEngine.Events;

public class BlowTorchTool : MonoBehaviour, ITool
{
    [Header("Stats")]
    public int repairAmount = 5;
    public float torchRange = 1f;
    public float torchRadius = 0.2f;
    public LayerMask mask;

    bool useTorch;

    public Submarine submarine;

    public ToolType ToolType { get { return ToolType.BlowTorch; } }

    [Header("Particle FX")]
    public ParticleSystem torchFixFX;

    [Header("Events")]
    public UnityEvent onRepair;

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
        submarine = null;
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetBool("BlowTorching", useTorch);
    }

    private void FixedUpdate()
    {
        bool playTorchFX = false;
        Submarine _submarine = null;
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
                if(hit.collider.gameObject != null)
                {
                    _submarine = hit.collider.gameObject.GetComponent<Submarine>();
                    if(_submarine != null)
                        playTorchFX = true;
                }

                torchFixFX.transform.position = hit.point;
                torchFixFX.transform.rotation = Quaternion.LookRotation(hit.normal, Vector3.up);
            }
        }

        if (playTorchFX && !torchFixFX.isPlaying)
            torchFixFX.Play();
        else if(!playTorchFX && torchFixFX.isPlaying)
            torchFixFX.Stop();

        submarine = _submarine;
    }

    public void Repair()
    {
        if (submarine == null)
            return;

        if (submarine.Health < submarine.MaxHealth)
        {
            submarine.Repair(repairAmount);
            onRepair.Invoke();
            Debug.LogFormat(
                "Repair [{0}] {1}/{2}",
                repairAmount,
                submarine.Health,
                submarine.MaxHealth);
        }
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
