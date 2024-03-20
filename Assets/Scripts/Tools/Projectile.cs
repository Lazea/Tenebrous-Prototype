using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Damage")]
    public int damage = 15;
    public float impactForce = 8f;

    [Header("Lifetime")]
    public float lifetime = 4f;
    public float impactLifeTime = 35f;
    public float launchSpeed = 12f;
    public float radius;
    Vector3 previousPosition;
    public LayerMask mask;

    [Header("Particcle FX")]
    public ParticleSystem particlesTrail;
    public ParticleSystem particlesOnHit;
    public ParticleSystem particlesOnHitFlesh;

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
        float force = 0f;
        float hitSpeed = rb.velocity.magnitude / launchSpeed;
        if (hit.collider.attachedRigidbody != null)
        {
            force = Mathf.Clamp(
                hitSpeed,
                0.2f, 1f);
            force *= impactForce;
            hit.collider.attachedRigidbody.AddForceAtPosition(
                transform.forward * force,
                hit.point);
        }

        int hitDamage = 0;
        var damageable = hit.collider.GetComponentInParent<IDamageable>();
        if (damageable != null)
        {
            hitDamage = Mathf.RoundToInt(Mathf.Clamp(
                hitSpeed,
                0.5f, 1f) * damage);
            damageable.DealDamage(null, hitDamage);
        }

        Debug.LogFormat(
            "Projectile {0} hit {1}; Dealt {2} damage and applied {3} force",
            gameObject.name,
            hit.collider.name,
            hitDamage,
            force);

        CancelInvoke("DestroyMe");
        Destroy(gameObject, impactLifeTime);

        transform.position = hit.point;
        transform.parent = hit.transform;

        if(particlesOnHit != null)
        {
            particlesOnHit.gameObject.SetActive(true);
        }

        Destroy(rb);
        Destroy(this);
    }    

    void DestroyMe()
    {
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        if(particlesTrail != null)
        {
            particlesTrail.transform.parent = null;
            var psMain = particlesTrail.main;
            psMain.loop = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
