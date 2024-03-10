using UnityEngine;

[System.Serializable]
public class PlayerData
{
    [Header("Health")]
    public int health = 100;
    [SerializeField]
    int maxHealth = 100;
    public int MaxHealth { get { return maxHealth; } }

    [Header("Oxygen")]
    public int oxygen = 100;
    [SerializeField]
    int maxOxygen = 120;
    public int MaxOxygen { get { return maxOxygen; } }
    public int oxygenConsumptionRate = 1;
    public int lowOxygenHealthDamage = 5;
}
