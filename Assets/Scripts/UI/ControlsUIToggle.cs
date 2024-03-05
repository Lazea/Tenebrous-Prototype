using UnityEngine;
using UnityEngine.UI;

public class ControlsUIToggle : MonoBehaviour
{
    public Toggle toggle;

    private void OnEnable()
    {
        SetToggleValue();
    }

    public void SetToggleValue()
    {
        toggle.isOn = GameSettingsManager.Instance.GetYInverted();
    }

    public void SetControlToggle(bool isOn)
    {
        GameSettingsManager.Instance.SetYInverted(isOn);
    }
}
