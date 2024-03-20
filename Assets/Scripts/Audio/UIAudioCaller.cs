using UnityEngine;

public class UIAudioCaller : MonoBehaviour
{
    public float quietVolume = 0.5f;

    public void PlayClip(AudioClip clip)
    {
        AudioManager.Instance.PlayUIAudioClip(clip, true);
    }

    public void PlayQuietClip(AudioClip clip)
    {
        AudioManager.Instance.PlayUIAudioClip(clip, quietVolume, true);
    }

    public void PlayClipNoOverlap(AudioClip clip)
    {
        AudioManager.Instance.PlayUIAudioClip(clip, false);
    }

    public void PlayQuietClipNoOverlap(AudioClip clip)
    {
        AudioManager.Instance.PlayUIAudioClip(clip, quietVolume, false);
    }
}
