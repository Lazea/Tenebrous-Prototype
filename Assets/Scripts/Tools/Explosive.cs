using System.Collections.Generic;
using UnityEditor.Profiling;
using UnityEngine;

public class Explosive : MonoBehaviour
{
    [Header("Damage")]
    public int damage = 100;
    public AnimationCurve damageFalloff;

    [Header("Blast")]
    public float fuse = 1f;
    public float blastRadius = 4f;
    public float blastForce = 25f;
    public LayerMask coverMask;
    public LayerMask mask;

    [Header("FX")]
    public ParticleSystem explosionFX; 

    // Start is called before the first frame update
    void Start()
    {
        Invoke("Explode", fuse);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [ContextMenu("Explode")]
    public void Explode()
    {
        if(explosionFX != null)
        {
            explosionFX.transform.parent = null;
            explosionFX.Play();
        }

        var hitObject = Physics.OverlapSphere(
            transform.position,
            blastRadius,
            mask);
        HashSet<GameObject> uniqueHitObjs = new HashSet<GameObject>();

        foreach(var hitObj in hitObject)
        {
            Vector3 disp = hitObj.transform.position - transform.position;
            float dist = disp.magnitude;
            Ray ray = new Ray(
                transform.position,
                disp.normalized);
            if(!Physics.Raycast(
                ray,
                dist,
                coverMask))
            {
                if (uniqueHitObjs.Contains(hitObj.gameObject))
                    continue;
                uniqueHitObjs.Add(hitObj.gameObject);

                if (hitObj.attachedRigidbody != null)
                {
                    hitObj.attachedRigidbody.AddExplosionForce(
                        blastForce,
                        transform.position,
                        blastRadius);
                }

                int hitDamage = 0;
                var damageable = hitObj.GetComponentInParent<IDamageable>();
                if(damageable != null)
                {
                    float t = Mathf.Clamp(dist / blastRadius, 0.25f, 1f);
                    hitDamage = Mathf.RoundToInt(damageFalloff.Evaluate(t) * damage);
                    damageable.DealDamage(null, hitDamage);
                }

                Debug.LogFormat(
                    "Explosive {0} hit {1}; Dealt {2} damage and applied {3} force",
                    gameObject.name,
                    hitObj.name,
                    hitDamage,
                    blastForce);
            }
        }

        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, blastRadius);
    }
}
