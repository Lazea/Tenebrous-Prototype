using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class GameplayHUD : Singleton<GameplayHUD>
{
    [Header("Circle Fill")]
    [SerializeField]
    CircleFill fillCircle;
    public CircleFill FillCircle { get { return fillCircle; } }

    [Header("Interaction Context")]
    [SerializeField]
    TextMeshProUGUI interactionContext;
    public TextMeshProUGUI InteractionContext { get { return interactionContext; } }

    [Header("Health Bar")]
    [SerializeField]
    Image healthFillBar;
    public Image HealthFillBar { get { return healthFillBar; } }
    float targetHealthFill;

    [Header("O2 Bar")]
    [SerializeField]
    Image o2FillBar;
    public Image O2FillBar { get { return o2FillBar; } }
    float targetO2Fill;

    [Header("Bar Fill Smooth")]
    public float barFillSmooth;

    [Header("Tool Slos")]
    [SerializeField]
    ToolSlotData drillToolSlot;
    public ToolSlotData DrillToolSlot { get { return drillToolSlot; } }
    [SerializeField]
    ToolSlotData blowTorchToolSlot;
    public ToolSlotData BlowTorchToolSlot { get { return blowTorchToolSlot; } }
    [SerializeField]
    ToolSlotData harpoonGunToolSlot;
    public ToolSlotData HarpoonGunToolSlot { get { return harpoonGunToolSlot; } }
    [SerializeField]
    ToolSlotData explosiveToolSlot;
    public ToolSlotData ExplosiveToolSlot { get { return explosiveToolSlot; } }
    [System.Serializable]
    public struct ToolSlotData
    {
        public RectTransform toolSlotTransform;
        public Image toolSlotBorder;
        public ToolType toolType;
    }
    [SerializeField]
    ToolSlotData[] toolSlots;
    public ToolSlotData[] ToolSlots { get { return toolSlots; } }
    public Color normalToolSlotColor;
    public Color selectedToolSlotColor;

    [Header("Controls Text")]
    [SerializeField]
    GameObject playerControlsText;
    [SerializeField]
    GameObject submarineControlsText;

    // Components
    public Animator anim;

    private void Awake()
    {
        targetHealthFill = healthFillBar.fillAmount;
        targetO2Fill = o2FillBar.fillAmount;
    }

    private void OnDisable()
    {
        anim.ResetTrigger("ShowHealthBar");
        anim.ResetTrigger("ShowO2Bar");
        anim.ResetTrigger("ShowToolSlots");
    }

    // Update is called once per frame
    void Update()
    {
        healthFillBar.fillAmount = Mathf.Lerp(
            healthFillBar.fillAmount,
            targetHealthFill,
            barFillSmooth);
        o2FillBar.fillAmount = Mathf.Lerp(
            o2FillBar.fillAmount,
            targetO2Fill,
            barFillSmooth);
    }

    #region [Bars]
    [ContextMenu("Show Health Bar")]
    public void ShowHealthBar()
    {
        anim.SetTrigger("ShowHealthBar");
    }

    [ContextMenu("Show O2 Bar")]
    public void ShowO2Bar()
    {
        anim.SetTrigger("ShowO2Bar");
    }

    [ContextMenu("Show Tool Slots")]
    public void ShowToolSlotsBar()
    {
        anim.SetTrigger("ShowToolSlots");
    }

    [ContextMenu("Show Player Controls")]
    public void ShowPlayerContolsText()
    {
        Debug.Log("Show Player Controls");
        playerControlsText.SetActive(true);
        submarineControlsText.SetActive(false);
        anim.SetTrigger("ShowContolsText");
    }

    [ContextMenu("Show Submarine Controls")]
    public void ShowSubmarineControlsText()
    {
        Debug.Log("Show Submarine Controls");
        playerControlsText.SetActive(false);
        submarineControlsText.SetActive(true);
        anim.SetTrigger("ShowContolsText");
    }

    public void SetHealthbarValue(float value)
    {
        targetHealthFill = value;
    }

    public void SetO2BarValue(float value)
    {
        targetO2Fill = value;
    }
    #endregion

    #region[Tool Slots]
    public void UpdateToolSlots(ITool[] tools)
    {
        drillToolSlot.toolSlotTransform.gameObject.SetActive(false);
        blowTorchToolSlot.toolSlotTransform.gameObject.SetActive(false);
        harpoonGunToolSlot.toolSlotTransform.gameObject.SetActive(false);
        explosiveToolSlot.toolSlotTransform.gameObject.SetActive(false);
        toolSlots = new ToolSlotData[tools.Length];

        for (int i = 0; i < tools.Length; i++)
        {
            switch (tools[i].ToolType)
            {
                case ToolType.Drill:
                    drillToolSlot.toolSlotTransform.gameObject.SetActive(true);
                    drillToolSlot.toolSlotTransform.SetSiblingIndex(i);
                    toolSlots[i]= drillToolSlot;
                    break;
                case ToolType.BlowTorch:
                    blowTorchToolSlot.toolSlotTransform.gameObject.SetActive(true);
                    blowTorchToolSlot.toolSlotTransform.SetSiblingIndex(i);
                    toolSlots[i] = blowTorchToolSlot;
                    break;
                case ToolType.Harpoon:
                    harpoonGunToolSlot.toolSlotTransform.gameObject.SetActive(true);
                    harpoonGunToolSlot.toolSlotTransform.SetSiblingIndex(i);
                    toolSlots[i] = harpoonGunToolSlot;
                    break;
                case ToolType.Explosive:
                    explosiveToolSlot.toolSlotTransform.gameObject.SetActive(true);
                    explosiveToolSlot.toolSlotTransform.SetSiblingIndex(i);
                    toolSlots[i] = explosiveToolSlot;
                    break;
            }
        }
    }

    public void HandleToolSlotSwitch(ITool selectedTool)
    {
        foreach (var toolSlot in toolSlots)
        {
            toolSlot.toolSlotTransform.localScale = Vector3.one;
            toolSlot.toolSlotBorder.color = normalToolSlotColor;

            if (toolSlot.toolType == selectedTool.ToolType)
            {
                toolSlot.toolSlotTransform.localScale = Vector3.one * 1.2f;
                toolSlot.toolSlotBorder.color = selectedToolSlotColor;
            }
        }

        ShowToolSlotsBar();
    }

    public void HandleToolSlotSwitch(int i)
    {
        for(int j = 0; j < toolSlots.Length; j++)
        {
            if(i !=  j)
            {
                toolSlots[j].toolSlotTransform.localScale = Vector3.one;
                toolSlots[j].toolSlotBorder.color = normalToolSlotColor;
            }
            else
            {
                toolSlots[j].toolSlotTransform.localScale = Vector3.one * 1.2f;
                toolSlots[j].toolSlotBorder.color = selectedToolSlotColor;
            }
        }

        ShowToolSlotsBar();
    }
    #endregion
}
