using UnityEngine;
using UnityEngine.Events;
using SOGameEventSystem;

public class Player : MonoBehaviour, IDamageable
{
    [SerializeField]
    PlayerData data;

    public int Health => data.health;
    public int MaxHealth => data.MaxHealth;
    public int Oxygen => data.oxygen;
    public int MaxOxygen => data.MaxOxygen;

    [Header("Events")]
    public UnityEvent<float> onHealthChange = new UnityEvent<float>();
    public UnityEvent<float> onOxygenChange = new UnityEvent<float>();
    public UnityEvent onOxygenRefill;
    public UnityEvent onPlayerDeath;
    public BaseGameEvent PlayerDeath;

    private void Awake()
    {
        data.health = data.MaxHealth;
        data.oxygen = data.MaxOxygen;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnEnable()
    {
        InvokeRepeating("ConsumeOxygen", 1f, 1f);
    }

    private void OnDisable()
    {
        CancelInvoke("ConsumeOxygen");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region [Health]
    public void DealDamage(GameObject damageSource, int damage)
    {
        if (data.health <= 0)
            return;

        data.health = Mathf.Max(data.health - damage, 0);
        onHealthChange.Invoke(data.health / (float)data.MaxHealth);

        if(data.health <= 0)
        {
            Kill();
        }
    }

    public void Heal(int healAmount)
    {
        data.health = Mathf.Min(data.MaxHealth, data.health + healAmount);
        onHealthChange.Invoke(data.health / (float)data.MaxHealth);
    }

    [ContextMenu("Full Heal Player")]
    public void FullHeal()
    {
        Heal(data.MaxHealth);
    }

    [ContextMenu("Kill Player")]
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
        data.oxygen = Mathf.Clamp(oxygenAmount, 0, data.MaxOxygen);
        onOxygenChange.Invoke(data.oxygen / (float)data.MaxOxygen);
    }

    [ContextMenu("Refill Player Oxygen")]
    public void RefillOxygen()
    {
        SetOxygen(data.MaxOxygen);
        onOxygenRefill.Invoke();
    }

    public void ConsumeOxygen()
    {
        SetOxygen(data.oxygen - data.oxygenConsumptionRate);

        if(data.oxygen <= 0)
        {
            DealDamage(null, data.lowOxygenHealthDamage);
        }
    }
    #endregion
}
