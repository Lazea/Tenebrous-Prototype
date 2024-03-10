[System.Serializable]
public struct UpgradeCost
{
    public ResourceCost[] resourceCosts;
}

[System.Serializable]
public struct ResourceCost
{
    public Mineral.MineralType mineralType;
    public int resourceCost;
}
