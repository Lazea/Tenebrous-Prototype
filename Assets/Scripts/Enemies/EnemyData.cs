using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyData
{
    public int health;
    [SerializeField]
    int maxHealth;
    public int MaxHealth { get { return maxHealth; } }
}
