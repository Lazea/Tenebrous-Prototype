using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using SOGameEventSystem;

public class Player : MonoBehaviour, IDamageable
{
    [SerializeField]
    PlayerData data;

    public int Health => data.health;
    public int MaxHealth => data.maxHealth;

    public int Oxygen => data.oxygen;
    public int MaxOxygen => data.maxOxygen;

    [Header("Events")]
    public UnityEvent<float> onHealthChange = new UnityEvent<float>();
    public UnityEvent<float> onOxygenChange = new UnityEvent<float>();
    public UnityEvent onOxygenRefill;
    public UnityEvent onPlayerDeath;
    public BaseGameEvent PlayerDeath;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region [Health]
    public void DealDamage(GameObject damageSource, int damage)
    {
        data.health = Mathf.Max(data.health - damage, 0);
        onHealthChange.Invoke(data.health / (float)data.maxHealth);
    }

    public void Heal(int healAmount)
    {
        data.health = Mathf.Min(data.maxHealth, data.health + healAmount);
        onHealthChange.Invoke(data.health / (float)data.maxHealth);
    }

    public void FullHeal()
    {
        Heal(data.maxHealth);
    }

    public void Kill()
    {
        onPlayerDeath.Invoke();
        PlayerDeath.Raise();
        Debug.Log("Player Dead");
    }
    #endregion

    #region [Oxygen]
    public void SetOxygen(int oxygenAmount)
    {
        data.oxygen = Mathf.Clamp(oxygenAmount, 0, data.maxOxygen);
        onOxygenChange.Invoke(data.oxygen / (float)data.maxOxygen);
    }

    public void RefillOxygen()
    {
        SetOxygen(data.maxOxygen);
        onOxygenRefill.Invoke();
    }
    #endregion
}
