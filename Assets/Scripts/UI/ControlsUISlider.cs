using UnityEngine;
using UnityEngine.UI;

public class ControlsUISlider : MonoBehaviour
{
    public enum ControlType
    {
        XSensitivity,
        YSensitivity
    }
    public ControlType controlType = ControlType.XSensitivity;

    public Slider volumeSlider;

    private void OnEnable()
    {
        SetSliderValue();
    }

    public void SetSliderValue()
    {
        switch (controlType)
        {
            case ControlType.XSensitivity:
                volumeSlider.value = GameSettingsManager.Instance.GetXSensitivity();
                break;
            case ControlType.YSensitivity:
                volumeSlider.value = GameSettingsManager.Instance.GetYSensitivity();
                break;
        }
    }

    public void SetControlSensitivity(float value)
    {
        switch (controlType)
        {
            case ControlType.XSensitivity:
                GameSettingsManager.Instance.SetXSensitivity(value);
                break;
            case ControlType.YSensitivity:
                GameSettingsManager.Instance.SetYSensitivity(value);
                break;
        }
    }
}
