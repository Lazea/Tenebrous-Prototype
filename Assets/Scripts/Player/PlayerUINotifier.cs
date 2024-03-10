using UnityEngine;

public class PlayerUINotifier : MonoBehaviour
{
    public Interactable interactable;
    public ToolType currentToolType;

    // Start is called before the first frame update
    void Start()
    {
        ShowPlayerControlsText();
    }

    private void OnDisable()
    {
        GameplayHUD.Instance.FillCircle.gameObject.SetActive(false);
        GameplayHUD.Instance.InteractionContext.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        if(interactable != null)
        {
            switch(interactable.interactableType)
            {
                case Interactable.InteractableType.Mineral:
                    HandleMineralNotification(interactable);
                    break;
                case Interactable.InteractableType.Submarine:
                    HandleSubmarineNotification(interactable);
                    break;
            }
        }
        else
        {
            GameplayHUD.Instance.FillCircle.gameObject.SetActive(false);
        }
    }

    #region [Notification Context Handlers]
    void HandleMineralNotification(Interactable interactable)
    {
        if (currentToolType == ToolType.Drill)
        {
            var mineral = interactable.GetComponent<Mineral>();
            GameplayHUD.Instance.FillCircle.gameObject.SetActive(true);
            GameplayHUD.Instance.FillCircle.SetFillValue(
                mineral.Health / (float)mineral.MaxHealth);
        }
        else
        {
            GameplayHUD.Instance.FillCircle.gameObject.SetActive(false);
        }
    }

    void HandleSubmarineNotification(Interactable interactable)
    {
        if (currentToolType == ToolType.BlowTorch)
        {
            var submarine = interactable.GetComponent<Submarine>();
            GameplayHUD.Instance.FillCircle.gameObject.SetActive(true);
            GameplayHUD.Instance.FillCircle.SetFillValue(
                submarine.Health /
                (float)submarine.MaxHealth);
        }
        else
        {
            GameplayHUD.Instance.FillCircle.gameObject.SetActive(false);
        }
    }

    public void SetInteractableContext()
    {
        string contextMsg = "";
        if (interactable != null)
        {
            switch (interactable.interactableType)
            {
                case Interactable.InteractableType.Submarine:
                    contextMsg = "[E] Enter Submarine";
                    break;
                case Interactable.InteractableType.Mineral:
                    var mineral = interactable.GetComponent<Mineral>();
                    contextMsg = mineral.Type.ToString();
                    break;
            }
        }

        GameplayHUD.Instance.InteractionContext.text = contextMsg;
    }
    #endregion

    #region [Interactable Setters]
    public void SetInRangeInteractable(Interactable interactable)
    {
        this.interactable = interactable;
    }

    public void SetPlayerSelectedTool(ITool tool)
    {
        currentToolType = tool.ToolType;
    }
    #endregion

    #region [Tool Slots]
    public void ToolUnlocked(ITool[] tools)
    {
        GameplayHUD.Instance.UpdateToolSlots(tools);
    }

    public void ToolLocked(ITool[] tools)
    {
        GameplayHUD.Instance.UpdateToolSlots(tools);
    }

    public void ToolSwitched(ITool selectedTool)
    {
        GameplayHUD.Instance.HandleToolSlotSwitch(selectedTool);
    }
    #endregion

    #region [Player Health and Oxygen]
    public void PlayerHealthChange(float health)
    {
        GameplayHUD.Instance.ShowHealthBar();
        GameplayHUD.Instance.SetHealthbarValue(health);
    }

    public void PlayerOxygenChange(float oxygen)
    {
        GameplayHUD.Instance.SetO2BarValue(oxygen);
        if(oxygen <= 0.33f)
        {
            ShowOxygenMeter();
        }
    }

    public void ShowOxygenMeter()
    {
        GameplayHUD.Instance.ShowO2Bar();
    }
    #endregion

    public void ShowPlayerControlsText()
    {
        GameplayHUD.Instance.ShowPlayerContolsText();
    }

    public void ShowSubmarineControlsText()
    {
        GameplayHUD.Instance.ShowSubmarineControlsText();
    }
}
