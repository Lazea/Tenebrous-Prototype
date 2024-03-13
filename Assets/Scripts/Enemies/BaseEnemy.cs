using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemy : MonoBehaviour, IDamageable
{
    [SerializeField]
    EnemyData data;

    public int Health => data.health;

    public int MaxHealth => data.MaxHealth;


    private void Awake()
    {
        data.health = data.MaxHealth;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DealDamage(GameObject damageSource, int damage)
    {
        data.health = Mathf.Max(data.health - damage, 0);
        if(data.health <= 0)
        {
            Kill();
        }
    }

    [ContextMenu("Kill")]
    public void Kill()
    {
        Destroy(gameObject);
    }
}
