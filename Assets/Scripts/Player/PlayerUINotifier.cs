using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUINotifier : MonoBehaviour
{
    public Interactable interactable;
    public ToolType currentToolType;

    public CircleFill fillCircle;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(interactable != null)
        {
            switch(interactable.interactableType)
            {
                case Interactable.InteractableType.Submarine:
                    HandleSubmarineNotification(interactable);
                    break;
            }
        }
        else
        {
            fillCircle.gameObject.SetActive(false);
        }
    }

    void HandleSubmarineNotification(Interactable interactable)
    {
        if (currentToolType == ToolType.BlowTorch)
        {
            var sub = interactable.GetComponent<Submarine>();
            fillCircle.gameObject.SetActive(true);
            fillCircle.SetFillValue(sub.Health / (float)sub.MaxHealth);
        }
        else
        {
            fillCircle.gameObject.SetActive(false);
        }
    }

    public void SetInRangeInteractable(Interactable interactable)
    {
        this.interactable = interactable;
    }

    public void SetPlayerSelectedTool(ITool tool)
    {
        currentToolType = tool.ToolType;
    }
}
