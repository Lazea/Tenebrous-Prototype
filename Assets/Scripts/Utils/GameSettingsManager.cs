using UnityEngine;
using UnityEngine.Audio;

public class GameSettingsManager : Singleton<GameSettingsManager>
{
    [SerializeField]
    private AudioMixer audioMixer;

    private void Start()
    {
        GameSettings.audioMixer = audioMixer;
    }

    #region [Control Settings]
    #region [Controls Setters]
    public void SetXSensitivity(float value)
    {
        GameSettings.xSensitivity = value;
    }

    public void SetYSensitivity(float value)
    {
        GameSettings.ySensitivity = value;
    }

    public void SetYInverted(bool enable)
    {
        GameSettings.yInverted = enable;
    }
    #endregion

    #region [Controls Getters]
    public float GetXSensitivity()
    {
        return GameSettings.xSensitivity;
    }

    public float GetYSensitivity()
    {
        return GameSettings.ySensitivity;
    }

    public bool GetYInverted()
    {
        return GameSettings.yInverted;
    }
    #endregion
    #endregion

    #region [Audio Settings]
    #region [Volume Conversion]
    public static float NormalizedToDecibel(float value)
    {
        if (value <= 0.0f)
            return -80.0f;  // Essentially silence
        return 20.0f * Mathf.Log10(value);
    }

    public static float DecibelToNormalized(float db)
    {
        return Mathf.Pow(10.0f, db / 20.0f);
    }
    #endregion

    #region[Volume Setters]
    public void SetMixerGroupParam(string paramName, float value)
    {
        GameSettings.audioMixer.SetFloat(paramName, NormalizedToDecibel(value));
    }

    public void SetMasterVolume(float value)
    {
        SetMixerGroupParam("MasterVolume", value);
    }

    public void SetMusicVolume(float value)
    {
        SetMixerGroupParam("MusicVolume", value);
    }

    public void SetEffectsVolume(float value)
    {
        SetMixerGroupParam("EffectsVolume", value);
    }
    #endregion

    #region[Volume Getters]
    public float GetMixerGroupParamValue(string paramName)
    {
        float db;
        GameSettings.audioMixer.GetFloat(paramName, out db);
        return DecibelToNormalized(db);
    }

    public float GetMasterVolume()
    {
        return GetMixerGroupParamValue("MasterVolume");
    }

    public float GetMusicVolume()
    {
        return GetMixerGroupParamValue("MusicVolume");
    }

    public float GetEffectsVolume()
    {
        return GetMixerGroupParamValue("EffectsVolume");
    }
    #endregion
    #endregion

    [ContextMenu("Log Game Settings")]
    public void LogGameSettings()
    {
        string msg = string.Format(
            "Control Settings\n" +
            "X - Sensitivity: {0}\n" +
            "Y - Sensitivity: {1}\n" +
            "Y - Inverted: {2}\n" +
            "Audio Settings\n" +
            "Master Volume: {3}\n" +
            "Music Volume: {4}\n" +
            "Effects Volume: {5}",
            GameSettings.xSensitivity,
            GameSettings.ySensitivity,
            GameSettings.yInverted,
            GetMasterVolume(),
            GetMusicVolume(),
            GetEffectsVolume()
        );
        Debug.Log(msg);
    }
}
