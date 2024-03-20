using System;
using System.Collections;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    [Header("Music Audio Source")]
    public AudioSource musicAudioSource;
    public float mainMusicVolume = 1f;
    public float mainMusicPauseVolume = 0.75f;
    public float musicFadeRate;

    [Header("Ambient Audio Source")]
    public AudioSource ambientAudioSource;

    [Header("UI Audio Source")]
    public AudioSource uiAudioSource;

    Coroutine musicFadeCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        musicAudioSource.volume = 0f;
        FadeInMusic();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayAudioClip(
        AudioSource audioSource,
        AudioClip clip,
        float volume,
        bool whileIsPlaying)
    {
        if (!whileIsPlaying && audioSource.isPlaying)
            return;

        audioSource.pitch = 1f;
        audioSource.PlayOneShot(clip, volume);
    }

    public void PlayAudioClip(
        AudioSource audioSource,
        AudioClip clip,
        float volume,
        float minPitch,
        float maxPitch,
        bool whileIsPlaying)
    {
        if (!whileIsPlaying && audioSource.isPlaying)
            return;

        audioSource.pitch = UnityEngine.Random.Range(minPitch, maxPitch);
        audioSource.PlayOneShot(clip, volume);
    }

    IEnumerator FadeAudio(
        AudioSource audioSource,
        float targetVolume,
        float fadeRate,
        Action callback)
    {
        float startVolume = audioSource.volume;
        float duration = Mathf.Abs(targetVolume - startVolume) / fadeRate;
        float t = 0f;
        while(t < duration && Mathf.Abs(targetVolume - audioSource.volume) > 0.01f)
        {
            t += Time.unscaledDeltaTime;

            audioSource.volume = Mathf.Lerp(
                audioSource.volume,
                targetVolume,
                t / duration);

            yield return new WaitForEndOfFrame();
        }

        audioSource.volume = targetVolume;
        callback();
    }

    #region [Music]
    public void FadeInMusic()
    {
        if(musicFadeCoroutine != null)
        {
            StopCoroutine(musicFadeCoroutine);
        }
        musicFadeCoroutine = StartCoroutine(FadeAudio(
            musicAudioSource,
            mainMusicVolume,
            musicFadeRate,
            () => { musicFadeCoroutine = null; }));
    }

    public void FadeOutMusic()
    {
        if (musicFadeCoroutine != null)
        {
            StopCoroutine(musicFadeCoroutine);
        }
        musicFadeCoroutine = StartCoroutine(FadeAudio(
            musicAudioSource,
            0f,
            musicFadeRate,
            () => { musicFadeCoroutine = null; }));
    }

    public void FadeInPauseMusic()
    {
        if (musicFadeCoroutine != null)
        {
            StopCoroutine(musicFadeCoroutine);
        }
        musicFadeCoroutine = StartCoroutine(FadeAudio(
            musicAudioSource,
            mainMusicPauseVolume,
            musicFadeRate,
            () => { musicFadeCoroutine = null; }));
    }
    #endregion

    #region [UI Audio]
    public void PlayUIAudioClip(AudioClip clip, bool whileIsPlaying)
    {
        PlayAudioClip(uiAudioSource, clip, 1f, 0.95f, 1.05f, whileIsPlaying);
    }

    public void PlayUIAudioClip(AudioClip clip, float volume, bool whileIsPlaying)
    {
        PlayAudioClip(uiAudioSource, clip, volume, 0.95f, 1.05f, whileIsPlaying);
    }
    #endregion
}
