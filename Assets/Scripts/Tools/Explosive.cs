using UnityEngine;

public class Explosive : MonoBehaviour
{
    public float fuse = 1f;
    public float blastRadius = 4f;
    public LayerMask coverMask;
    public LayerMask mask;

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

        // TODO: Sphere overlap for the explosion

        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, blastRadius);
    }
}
