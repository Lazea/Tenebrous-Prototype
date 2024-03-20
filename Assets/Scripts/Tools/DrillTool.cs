using UnityEngine;

public class DrillTool : MonoBehaviour, ITool
{
    [Header("Stats")]
    public int damage = 5;
    public float drillRange = 1f;
    public float drillRadius = 0.2f;
    public LayerMask mask;

    bool useDrill;
    GameObject objectDrilled;

    public ToolType ToolType { get { return ToolType.Drill; } }

    [Header("Particle FX")]
    public ParticleSystem drillFX;

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

        controls.Action.started += ctx => useDrill = true;
        controls.Action.canceled += ctx => useDrill = false;
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
        useDrill = false;
        anim.SetBool("Drilling", false);
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetBool("Drilling", useDrill);
    }

    private void FixedUpdate()
    {
        bool playFX = false;
        GameObject _objectDrilled = null;
        if (animStateInfo.IsName("Drilling"))
        {
            Ray ray = new Ray(
            Camera.main.transform.position,
            Camera.main.transform.forward);
            RaycastHit hit;
            if(Physics.SphereCast(
                ray,
                drillRadius,
                out hit,
                drillRange,
                mask))
            {
                _objectDrilled = hit.collider.gameObject;
                playFX = true;

                drillFX.transform.position = hit.point;
                drillFX.transform.rotation = Quaternion.LookRotation(hit.normal, Vector3.up);
            }
        }

        if (playFX && !drillFX.isPlaying)
            drillFX.Play();
        else if (!playFX && drillFX.isPlaying)
            drillFX.Stop();

        objectDrilled = _objectDrilled;
    }

    public void Drill()
    {
        if (objectDrilled == null)
            return;

        var damageable = objectDrilled.GetComponent<IDamageable>();
        damageable?.DealDamage(transform.gameObject, damage);
        Debug.LogFormat("Drilling {0}", objectDrilled.name);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector3 startPoint = Camera.main.transform.position;
        Vector3 endPoint = Camera.main.transform.forward * drillRange;
        endPoint += startPoint;
        Gizmos.DrawLine(startPoint, endPoint);
        Gizmos.DrawWireSphere(endPoint, drillRadius);
    }
}
