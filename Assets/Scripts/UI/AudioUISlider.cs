using UnityEngine;
using UnityEngine.UI;

public class AudioUISlider : MonoBehaviour
{
    public enum VolumeType
    {
        Master,
        Music,
        Effects
    }
    public VolumeType volumeType = VolumeType.Master;

    public Slider volumeSlider;

    private void OnEnable()
    {
        SetSliderValue();
    }

    public void SetSliderValue()
    {
        switch (volumeType)
        {
            case VolumeType.Master:
                volumeSlider.value = GameSettingsManager.Instance.GetMasterVolume();
                break;
            case VolumeType.Music:
                volumeSlider.value = GameSettingsManager.Instance.GetMusicVolume();
                break;
            case VolumeType.Effects:
                volumeSlider.value = GameSettingsManager.Instance.GetEffectsVolume();
                break;
        }
    }

    public void SetVolume(float volume)
    {
        switch(volumeType)
        {
            case VolumeType.Master:
                GameSettingsManager.Instance.SetMasterVolume(volume);
                break;
            case VolumeType.Music:
                GameSettingsManager.Instance.SetMusicVolume(volume);
                break;
            case VolumeType.Effects:
                GameSettingsManager.Instance.SetEffectsVolume(volume);
                break;
        }
    }
}
