using UnityEngine;

public class BaseDestructable : MonoBehaviour, IDamageable
{
    [SerializeField]
    int health = 100;
    public int Health
    {
        get => health;
    }

    [SerializeField]
    int maxHealth = 100;
    public int MaxHealth
    {
        get => maxHealth;
    }

    public float chunkLaunchExplosionForce = 1f;
    public float chunkLaunchExplosionRadius = 1f;
    public float chunkLaunchMaxAngularSpeed = 1f;
    Rigidbody[] chunks;

    [Header("Particle FX")]
    public ParticleSystem destroyFX;

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
    }

    #region [Damage]
    public void DealDamage(GameObject damageSource, int damage)
    {
        health = Mathf.Max(
            0,
            health - damage);

        if (health <= 0)
        {
            Kill();
        }
    }

    void ReleaseChunks()
    {
        foreach (var chunk in chunks)
        {
            chunk.transform.parent = null;
            chunk.gameObject.SetActive(true);
            chunk.AddExplosionForce(
                chunkLaunchExplosionForce,
                transform.position,
                chunkLaunchExplosionRadius);
            chunk.angularVelocity = Random.insideUnitSphere * chunkLaunchMaxAngularSpeed;
        }
    }

    [ContextMenu("Break Mineral")]
    public void Kill()
    {
        GetComponent<Collider>().enabled = false;

        if (chunks?.Length > 0)
        {
            ReleaseChunks();
        }

        if (destroyFX != null)
        {
            destroyFX.transform.parent = null;
            destroyFX.gameObject.SetActive(true);
        }

        Destroy(gameObject);
    }
    #endregion
}
