using UnityEngine;

[CreateAssetMenu(
    fileName = "SO_Data_Submarine",
    menuName = "Scriptable Objects/Submarine Data")]
public class SubmarineData : ScriptableObject
{
    [Header("Hull")]
    public int hullHealth = 100;
    [SerializeField]
    int hullMaxHealth = 100;
    public int HullMaxHealth { get { return hullMaxHealth; } }

    public int hullLevel = 1;
    [SerializeField]
    int hullMaxLevel = 3;
    public int HullMaxLevel { get { return hullMaxLevel; } }

    [Header("Sonar")]
    public int sonarLevel = 1;
    [SerializeField]
    int sonarMaxLevel = 3;
    public int SonarMaxLevel { get { return sonarMaxLevel; } }

    public void ResetData()
    {
        hullHealth = hullMaxHealth;
        hullLevel = 1;
        sonarLevel = 1;
    }
}
