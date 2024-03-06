using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float lifetime = 4f;
    public float impactLifeTime = 35f;
    public float launchSpeed = 12f;
    public float radius;
    Vector3 previousPosition;
    public LayerMask mask;

    // Components
    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * launchSpeed;
        previousPosition = transform.position;

        Invoke("DestroyMe", lifetime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        Vector3 disp = transform.position - previousPosition;
        Ray ray = new Ray(previousPosition, disp.normalized);
        RaycastHit hit;
        if(Physics.SphereCast(
            ray,
            radius,
            out hit,
            disp.magnitude,
            mask))
        {
            Hit(hit);
        }

        Debug.DrawLine(previousPosition, transform.position);

        if(rb.velocity != Vector3.zero)
        {
            Quaternion targetRot = Quaternion.LookRotation(rb.velocity);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRot,
                4f * Time.deltaTime);
        }

        previousPosition = transform.position;
    }

    void Hit(RaycastHit hit)
    {
        Debug.LogFormat(
            "Projectile {0} hit {1}",
            gameObject.name,
            hit.collider.name);
        // TODO: Deal damage to hit

        CancelInvoke("DestroyMe");
        Destroy(gameObject, impactLifeTime);

        transform.position = hit.point;
        Destroy(rb);
        Destroy(this);
    }    

    void DestroyMe()
    {
        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
