using TMPro;
using UnityEngine;
using UnityEngine.UI;
using SOGameEventSystem;
using UnityEngine.EventSystems;

public class UpgradeSystem : MonoBehaviour
{
    public PlayerInventoryData playerInventoryData;
    public SubmarineData submarineData;

    [System.Serializable]
    public enum UpgradeType
    {
        SubmarineHull,
        SubmarineSonar,
        Drillbit,
        HarpoonGun,
        Explosive
    }
    [SerializeField]
    UpgradeType selectedUpgradeType;

    [Header("Upgrade Costs")]
    public UpgradeCost[] submarineHullLevelCost;
    public UpgradeCost[] submarineSonarLevlCost;
    public UpgradeCost[] drillbitLevelCost;
    public UpgradeCost harpoonGunCost;
    public UpgradeCost explosiveCost;

    [Header("Upgrade Items UI")]
    public UpgradeSlotUI submarineHullUpgradeSlot;
    public UpgradeSlotUI submarineSonarUpgradeSlot;
    public UpgradeSlotUI drillbitUpgradeSlot;
    public UpgradeSlotUI harpoonGunUpgradeSlot;
    public UpgradeSlotUI explosiveUpgradeSlot;
    [System.Serializable]
    public struct UpgradeSlotUI
    {
        public Button upgradeSlotButton;
        public TextMeshProUGUI upgradeSlotText;
    }

    [Header("Resources UI")]
    public ResourceCostUIElement ironResourceUIElement;
    public ResourceCostUIElement quartzResourceUIElement;
    public ResourceCostUIElement goldResourceUIElement;
    public ResourceCostUIElement diamondResourceUIElement;

    [Header("Upgrade Info UI")]
    public Button buyButton;
    public TextMeshProUGUI upgradeInfoText;
    public RectTransform upgradeResourceCostPanel;
    public Color costMetColor;
    public Color costUnmetColor;

    [Header("Upgrade Cost UI")]
    public ResourceCostUIElement ironCostUIElement;
    public ResourceCostUIElement quartzCostUIElement;
    public ResourceCostUIElement goldCostUIElement;
    public ResourceCostUIElement diamondCostUIElement;
    [System.Serializable]
    public struct ResourceCostUIElement
    {
        public GameObject uiElement;
        public TextMeshProUGUI uiElementCostText;
    }

    [Header("Events")]
    public BaseGameEvent submarineHullUpgraded;
    public BaseGameEvent submarineSonarUpgraded;
    public BaseGameEvent drillbitUpgraded;
    public BaseGameEvent harpoonGunUnlocked;
    public BaseGameEvent explosiveUnlocked;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnEnable()
    {
        UpdateResourceInventory();
        UpdateUpgradeSlotsUI();

        EventSystem.current.SetSelectedGameObject(null);
        ClearUpgradeInfo();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateUpgradeSlotsUI()
    {
        bool hullMaxLevel = submarineData.hullLevel >= submarineData.HullMaxLevel;
        submarineHullUpgradeSlot.upgradeSlotButton.interactable = !hullMaxLevel;
        submarineHullUpgradeSlot.upgradeSlotText.text = string.Format(
            "{0}Hull Strength{1} [{2} LVL {3}]",
            (hullMaxLevel) ? "<s>" : "",
            (hullMaxLevel) ? "</s>" : "",
            (hullMaxLevel) ? "Max" : "Curr",
            submarineData.hullLevel);

        bool sonarMaxLevel = submarineData.sonarLevel >= submarineData.SonarMaxLevel;
        submarineSonarUpgradeSlot.upgradeSlotButton.interactable = !sonarMaxLevel;
        submarineSonarUpgradeSlot.upgradeSlotText.text = string.Format(
            "{0}Sonar{1} [{2} LVL {3}]",
            (sonarMaxLevel) ? "<s>" : "",
            (sonarMaxLevel) ? "</s>" : "",
            (sonarMaxLevel) ? "Max" : "Curr",
            submarineData.sonarLevel);

        bool drillbitMaxLevel = playerInventoryData.drillbitLevel >= playerInventoryData.DrillbitMaxLevel;
        drillbitUpgradeSlot.upgradeSlotButton.interactable = !drillbitMaxLevel;
        drillbitUpgradeSlot.upgradeSlotText.text = string.Format(
            "{0}Drillbit Strength{1} [{2} LVL {3}]",
            (drillbitMaxLevel) ? "<s>" : "",
            (drillbitMaxLevel) ? "</s>" : "",
            (drillbitMaxLevel) ? "Max" : "Curr",
            playerInventoryData.drillbitLevel);

        harpoonGunUpgradeSlot.upgradeSlotButton.interactable = !playerInventoryData.harpoonGunUnlocked;
        harpoonGunUpgradeSlot.upgradeSlotText.text = string.Format(
            "{0}Harpoon Gun{1}{2}",
            (playerInventoryData.harpoonGunUnlocked) ? "<s>" : "",
            (playerInventoryData.harpoonGunUnlocked) ? "</s>" : "",
            (playerInventoryData.harpoonGunUnlocked) ? " [Owned]" : "");

        explosiveUpgradeSlot.upgradeSlotButton.interactable = !playerInventoryData.explosiveUnlocked;
        explosiveUpgradeSlot.upgradeSlotText.text = string.Format(
            "{0}Explosives{1}{2}",
            (playerInventoryData.explosiveUnlocked) ? "<s>" : "",
            (playerInventoryData.explosiveUnlocked) ? "</s>" : "",
            (playerInventoryData.explosiveUnlocked) ? " [Owned]" : "");
    }

    public void UpdateResourceInventory()
    {
        UpdateResourceUIElement(
            ironResourceUIElement,
            true,
            playerInventoryData.ironResourceCount,
            costMetColor);
        UpdateResourceUIElement(
            quartzResourceUIElement,
            true,
            playerInventoryData.quartzResourceCount,
            costMetColor);
        UpdateResourceUIElement(
            goldResourceUIElement,
            true,
            playerInventoryData.goldResourceCount,
            costMetColor);
        UpdateResourceUIElement(
            diamondResourceUIElement,
            true,
            playerInventoryData.diamondResourceCount,
            costMetColor);
    }

    public void SelectUpgradeType(int upgradeType)
    {
        selectedUpgradeType = (UpgradeType)upgradeType;
        UpdateUpgradeInfo();
    }

    public void ClearUpgradeInfo()
    {
        UpdateResourceUIElement(ironCostUIElement, false, 0, costMetColor);
        UpdateResourceUIElement(quartzCostUIElement, false, 0, costMetColor);
        UpdateResourceUIElement(goldCostUIElement, false, 0, costMetColor);
        UpdateResourceUIElement(diamondCostUIElement, false, 0, costMetColor);

        buyButton.gameObject.SetActive(false);

        upgradeInfoText.text = "Please select an Upgrade for more info";
    }

    public void UpdateUpgradeInfo()
    {
        UpdateResourceUIElement(ironCostUIElement, false, 0, costMetColor);
        UpdateResourceUIElement(quartzCostUIElement, false, 0, costMetColor);
        UpdateResourceUIElement(goldCostUIElement, false, 0, costMetColor);
        UpdateResourceUIElement(diamondCostUIElement, false, 0, costMetColor);

        bool maxedOutUpgrade = UpgradeMaxedOut();
        bool canAfford = false;
        if (!maxedOutUpgrade)
        {
            canAfford = CanAffordUpgrade();
            buyButton.interactable = canAfford;
        }
        buyButton.gameObject.SetActive(!maxedOutUpgrade);
        Debug.LogFormat(
            "Updating Upgrade Info:\nUpgrade maxed out ? {0}\nCan afford cost ? {1}",
            maxedOutUpgrade,
            canAfford);

        string upgradeInfoMSG = "";
        switch(selectedUpgradeType)
        {
            case UpgradeType.SubmarineHull:
                int newSubmarineHullLevel = submarineData.hullLevel + 1;
                upgradeInfoMSG = "Upgrade Submarine Hull to ";
                upgradeInfoMSG += "<color=\"green\">LVL {0}</color>\n";
                upgradeInfoMSG += "<size=75%>Reinforced hull structure allows for dives up to <color=\"green\">{1} Fathoms</color></size>";
                upgradeInfoMSG = string.Format(
                    upgradeInfoMSG,
                    newSubmarineHullLevel,
                    0);
                Debug.Log(
                    "Updating Upgrade Info Cost for sub hull");
                HandleSelectedUpgradeResourceCostUI(
                    submarineHullLevelCost[submarineData.hullLevel - 1]);
                break;
            case UpgradeType.SubmarineSonar:
                upgradeInfoMSG = "Upgrade Submarine Sonar to ";
                upgradeInfoMSG += "<color=\"green\">LVL {0}</color>\n";
                upgradeInfoMSG += "<size=75%>Quartz enhanced sonar system allows for detection of\n<color=\"green\">{1}</color></size>";

                string detectables = "[Organics, Iron";
                int newSonarLevel = submarineData.sonarLevel + 1;
                if (newSonarLevel >= 2)
                    detectables += ", Quartz";
                if (newSonarLevel >= 3)
                    detectables += ", Gold, Diamond";
                detectables += "]";

                upgradeInfoMSG = string.Format(
                    upgradeInfoMSG,
                    newSonarLevel,
                    detectables);
                Debug.Log(
                    "Updating Upgrade Info Cost for sub sonar");
                HandleSelectedUpgradeResourceCostUI(
                    submarineSonarLevlCost[submarineData.sonarLevel - 1]);
                break;
            case UpgradeType.Drillbit:
                upgradeInfoMSG = "Upgrade Drillbit to ";
                upgradeInfoMSG += "<color=\"green\">LVL {0}</color>\n";
                upgradeInfoMSG += "<size=75%>Diamond reinforced drillbit results in faster mining speeds</size>";
                int newDrillbitLevel = playerInventoryData.drillbitLevel + 1;
                upgradeInfoMSG = string.Format(
                    upgradeInfoMSG,
                    newDrillbitLevel);
                Debug.Log(
                    "Updating Upgrade Info Cost for drillbit");
                HandleSelectedUpgradeResourceCostUI(
                    drillbitLevelCost[playerInventoryData.drillbitLevel - 1]);
                break;
            case UpgradeType.HarpoonGun:
                upgradeInfoMSG = "Unlock the Harpoon Gun\n";
                upgradeInfoMSG += "<size=75%>High-velocity precision weapon for underwater targeting</size>";
                Debug.Log(
                    "Updating Upgrade Info Cost for harpoon gun");
                HandleSelectedUpgradeResourceCostUI(harpoonGunCost);
                break;
            case UpgradeType.Explosive:
                upgradeInfoMSG = "Unlock Explosives\n";
                upgradeInfoMSG += "<size=75%>Timed explosive designed for submerged detonation</size>";
                Debug.Log(
                    "Updating Upgrade Info Cost for explosive");
                HandleSelectedUpgradeResourceCostUI(explosiveCost);
                break;
        }

        upgradeInfoText.text = upgradeInfoMSG;
    }

    void HandleSelectedUpgradeResourceCostUI(UpgradeCost upgradeCost)
    {
        Debug.LogFormat(
            "Updating Upgrade Info Resource Costs:\nSetting {0} costs",
            upgradeCost.resourceCosts.Length);
        foreach (var resourceCost in upgradeCost.resourceCosts)
        {
            switch (resourceCost.mineralType)
            {
                case Mineral.MineralType.Iron:
                    bool costMet = resourceCost.resourceCost <= playerInventoryData.ironResourceCount;
                    Color costTextColor = costMet ? costMetColor : costUnmetColor;

                    UpdateResourceUIElement(
                        ironCostUIElement,
                        true,
                        resourceCost.resourceCost,
                        costTextColor);
                    break;
                case Mineral.MineralType.Quartz:
                    costMet = resourceCost.resourceCost <= playerInventoryData.quartzResourceCount;
                    costTextColor = costMet ? costMetColor : costUnmetColor;

                    UpdateResourceUIElement(
                        quartzCostUIElement,
                        true,
                        resourceCost.resourceCost,
                        costTextColor);
                    break;
                case Mineral.MineralType.Gold:
                    costMet = resourceCost.resourceCost <= playerInventoryData.goldResourceCount;
                    costTextColor = costMet ? costMetColor : costUnmetColor;

                    UpdateResourceUIElement(
                        goldCostUIElement,
                        true,
                        resourceCost.resourceCost,
                        costTextColor);
                    break;
                case Mineral.MineralType.Diamond:
                    costMet = resourceCost.resourceCost <= playerInventoryData.diamondResourceCount;
                    costTextColor = costMet ? costMetColor : costUnmetColor;

                    UpdateResourceUIElement(
                        diamondCostUIElement,
                        true,
                        resourceCost.resourceCost,
                        costTextColor);
                    break;
            }
        }
    }

    void UpdateResourceUIElement(
        ResourceCostUIElement resourceCostUIElement,
        bool activate,
        int resourceCost,
        Color costTextColor)
    {
        resourceCostUIElement.uiElement.SetActive(activate);
        resourceCostUIElement.uiElementCostText.text = string.Format(
            ":<color=#{0}>{1}",
            ColorUtility.ToHtmlStringRGB(costTextColor),
            resourceCost);
    }

    public bool UpgradeMaxedOut()
    {
        bool maxedOut = false;
        switch (selectedUpgradeType)
        {
            case UpgradeType.SubmarineHull:
                maxedOut = submarineData.hullLevel == submarineData.HullMaxLevel;
                break;
            case UpgradeType.SubmarineSonar:
                maxedOut = submarineData.sonarLevel == submarineData.SonarMaxLevel;
                break;
            case UpgradeType.Drillbit:
                maxedOut = playerInventoryData.drillbitLevel == playerInventoryData.DrillbitMaxLevel;
                break;
            case UpgradeType.HarpoonGun:
                maxedOut = playerInventoryData.harpoonGunUnlocked;
                break;
            case UpgradeType.Explosive:
                maxedOut = playerInventoryData.explosiveUnlocked;
                break;
        }

        return maxedOut;
    }

    public bool CanAffordUpgrade()
    {
        bool canAfford = true;
        switch(selectedUpgradeType)
        {
            case UpgradeType.SubmarineHull:
                int i = submarineData.hullLevel - 1;
                canAfford = CheckResourceCosts(submarineHullLevelCost[i]);
                break;
            case UpgradeType.SubmarineSonar:
                i = submarineData.sonarLevel - 1;
                canAfford = CheckResourceCosts(submarineSonarLevlCost[i]);
                break;
            case UpgradeType.Drillbit:
                i = playerInventoryData.drillbitLevel - 1;
                canAfford = CheckResourceCosts(drillbitLevelCost[i]);
                break;
            case UpgradeType.HarpoonGun:
                canAfford = CheckResourceCosts(harpoonGunCost);
                break;
            case UpgradeType.Explosive:
                canAfford = CheckResourceCosts(explosiveCost);
                break;
        }

        return canAfford;
    }

    bool CheckResourceCosts(UpgradeCost upgradeCost)
    {
        bool canAfford = false;
        foreach (var cost in upgradeCost.resourceCosts)
        {
            switch (cost.mineralType)
            {
                case Mineral.MineralType.Iron:
                    canAfford = cost.resourceCost <= playerInventoryData.ironResourceCount;
                    break;
                case Mineral.MineralType.Quartz:
                    canAfford = cost.resourceCost <= playerInventoryData.quartzResourceCount;
                    break;
                case Mineral.MineralType.Gold:
                    canAfford = cost.resourceCost <= playerInventoryData.goldResourceCount;
                    break;
                case Mineral.MineralType.Diamond:
                    canAfford = cost.resourceCost <= playerInventoryData.diamondResourceCount;
                    break;
            }

            if (!canAfford)
                break;
        }

        return canAfford;
    }

    #region [Upgrades and Unlocks]
    public void BuyUpgrade()
    {
        Debug.LogFormat("Buy upgrade {0}", selectedUpgradeType.ToString());
        switch (selectedUpgradeType)
        {
            case UpgradeType.SubmarineHull:
                UpgradeSubmarineHull();
                break;
            case UpgradeType.SubmarineSonar:
                UpgradeSubmarineSonar();
                break;
            case UpgradeType.Drillbit:
                UpgradeDrillbit();
                break;
            case UpgradeType.HarpoonGun:
                UnlockHarpoonGun();
                break;
            case UpgradeType.Explosive:
                UnlockExplosive();
                break;
        }

        UpdateResourceInventory();
        UpdateUpgradeSlotsUI();
        UpdateUpgradeInfo();
        EventSystem.current.SetSelectedGameObject(null);
    }

    [ContextMenu("Upgrade Submarine Hull Level")]
    public void UpgradeSubmarineHull()
    {
        submarineData.hullLevel = Mathf.Min(
            submarineData.hullLevel + 1,
            submarineData.HullMaxLevel);

        SpendResource(submarineHullLevelCost[submarineData.hullLevel - 1].resourceCosts);
        submarineHullUpgraded.Raise();
    }

    [ContextMenu("Upgrade Submarine Sonar Level")]
    public void UpgradeSubmarineSonar()
    {
        submarineData.sonarLevel = Mathf.Min(
            submarineData.sonarLevel + 1,
            submarineData.SonarMaxLevel);

        SpendResource(submarineSonarLevlCost[submarineData.sonarLevel - 1].resourceCosts);
        submarineSonarUpgraded.Raise();
    }

    [ContextMenu("Upgrade Drillbit Level")]
    public void UpgradeDrillbit()
    {
        playerInventoryData.drillbitLevel = Mathf.Min(
            playerInventoryData.drillbitLevel + 1,
            playerInventoryData.DrillbitMaxLevel);

        SpendResource(drillbitLevelCost[playerInventoryData.drillbitLevel - 1].resourceCosts);
        drillbitUpgraded.Raise();
    }

    [ContextMenu("Unlock Harpoon Gun")]
    public void UnlockHarpoonGun()
    {
        playerInventoryData.harpoonGunUnlocked = true;

        SpendResource(harpoonGunCost.resourceCosts);
        harpoonGunUnlocked.Raise();
    }

    [ContextMenu("Unlock Harpoon Gun")]
    public void UnlockExplosive()
    {
        playerInventoryData.explosiveUnlocked = true;

        SpendResource(explosiveCost.resourceCosts);
        explosiveUnlocked.Raise();
    }

    void SpendResource(ResourceCost[] resourceCost)
    {
        foreach (var cost in resourceCost)
        {
            switch (cost.mineralType)
            {
                case Mineral.MineralType.Iron:
                    playerInventoryData.ironResourceCount = Mathf.Max(
                        playerInventoryData.ironResourceCount - cost.resourceCost,
                        0);
                    break;
                case Mineral.MineralType.Quartz:
                    playerInventoryData.quartzResourceCount = Mathf.Max(
                        playerInventoryData.quartzResourceCount - cost.resourceCost,
                        0);
                    break;
                case Mineral.MineralType.Gold:
                    playerInventoryData.goldResourceCount = Mathf.Max(
                        playerInventoryData.goldResourceCount - cost.resourceCost,
                        0);
                    break;
                case Mineral.MineralType.Diamond:
                    playerInventoryData.diamondResourceCount = Mathf.Max(
                        playerInventoryData.diamondResourceCount - cost.resourceCost,
                        0);
                    break;
            }
        }
    }
    #endregion
}
