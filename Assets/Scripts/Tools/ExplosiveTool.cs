using UnityEngine;

public class ExplosiveTool : MonoBehaviour, ITool
{
    public GameObject explosivePrefab;
    public float spawnRange = 0.4f;
    public float throwSpeed = 1f;
    public float maxAngularVelocity = 5f;

    public ToolType ToolType { get { return ToolType.Explosive; } }

    // Components
    Controls.GameplayActions controls;
    Animator anim;

    void Awake()
    {
        anim = GetComponentInParent<Animator>();

        controls = new Controls().Gameplay;

        controls.Action.performed += ctx => StartThrowExplosive();
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
        anim.ResetTrigger("ExplosiveThrow");
    }

    void StartThrowExplosive()
    {
        anim.SetTrigger("ExplosiveThrow");
    }

    public void ThrowExplosive()
    {
        Debug.Log("Throw Explosive");

        Transform camTransform = Camera.main.transform;
        Rigidbody explosive = Instantiate(
            explosivePrefab,
            camTransform.position + camTransform.forward * spawnRange,
            camTransform.rotation,
            null).GetComponent<Rigidbody>();

        Vector3 throwDir = camTransform.forward + Vector3.up * 0.1f;
        explosive.velocity = throwDir.normalized * throwSpeed;

        Vector3 angularVelocity = Random.insideUnitSphere;
        angularVelocity += camTransform.up * 0.5f;
        explosive.angularVelocity = angularVelocity * maxAngularVelocity;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector3 startPoint = Camera.main.transform.position;
        Vector3 endPoint = Camera.main.transform.forward * spawnRange;
        endPoint += startPoint;
        Gizmos.DrawLine(startPoint, endPoint);
        Gizmos.DrawWireSphere(endPoint, 0.1f);
    }
}
