using UnityEngine;

public class Mineral : Interactable, IDamageable
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

    [Header("Mineral Chunks")]
    [SerializeField]
    MineralType mineralType;
    public MineralType Type { get { return mineralType; } }
    public enum MineralType
    {
        Iron,
        Gold,
        Diamond,
        Quartz,
    }
    public GameObject mineralChunkPrefab;
    public int mineralChunkCount = 4;
    public Transform mineralChunkSpawnPoint;
    public float mineralChunkSpawnRadius = 0.25f;
    public float mineralChunkLaunchExplosionForce = 1f;
    public float mineralChunkLaunchExplosionRadius = 1f;
    public float mineralChunkLaunchMaxAngularSpeed = 1f;

    private void Awake()
    {
        interactableType = InteractableType.Mineral;
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

    [ContextMenu("Deal 10 Damage")]
    void Deal10Damage()
    {
        DealDamage(null, 10);
    }

    [ContextMenu("Deal 50 Damage")]
    void Deal50Damage()
    {
        DealDamage(null, 50);
    }

    void SpawnMineralChunk()
    {
        Vector3 spawnPoint = Random.insideUnitSphere * mineralChunkSpawnRadius;
        if(mineralChunkSpawnPoint == null)
            spawnPoint += transform.position;
        else
            spawnPoint += mineralChunkSpawnPoint.position;

        var chunk = Instantiate(
            mineralChunkPrefab,
            spawnPoint,
            Random.rotation).GetComponent<Rigidbody>();

        chunk.AddExplosionForce(
            mineralChunkLaunchExplosionForce,
            transform.position,
            mineralChunkLaunchExplosionRadius);
        chunk.angularVelocity = Random.insideUnitSphere * mineralChunkLaunchMaxAngularSpeed;
    }

    [ContextMenu("Break Mineral")]
    public void Kill()
    {
        GetComponent<Collider>().enabled = false;

        for(int i = 0; i < mineralChunkCount; i++)
        {
            SpawnMineralChunk();
        }

        Destroy(gameObject);
    }
    #endregion

    private void OnDrawGizmos()
    {
        Vector3 spawnPoint = transform.position;
        if (mineralChunkSpawnPoint != null)
            spawnPoint = mineralChunkSpawnPoint.position;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(spawnPoint, mineralChunkSpawnRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, mineralChunkLaunchExplosionRadius);

    }
}
