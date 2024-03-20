using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubmarineAudioManager : MonoBehaviour
{
    public AudioClip enterSubmarineClip;

    [Header("Sonar Ping Audio")]
    public AudioSource audioSource;
    public AudioClip[] sonarAudioClips;
    public float sonarPingVolume = 0.65f;

    [Header("Upgrade Console Audio")]
    public AudioSource upgradeConsoleAudioSource;
    public AudioClip upgradeUIHoverClip;
    public AudioClip upgradeUIClickClip;

    [Header("Movement Audio Source")]
    public AudioSource movementAudioSource;
    public AudioSource engineAudioSource;
    float maxMovementVolume;
    float minEngineVolume = 0.3f;
    float maxEngineVolume;

    [Header("Rigidbody")]
    public float maxRBSpeed = 12f;
    public Rigidbody rb;

    private void Awake()
    {
        maxMovementVolume = movementAudioSource.volume;
        movementAudioSource.volume = 0f;

        maxEngineVolume = engineAudioSource.volume;
        engineAudioSource.volume = minEngineVolume;
    }

    private void OnEnable()
    {
        PlayEnterSubmarine();
    }

    private void Update()
    {
        HandleMovementVolume();
    }

    void HandleMovementVolume()
    {
        float speed = Mathf.Clamp01(rb.velocity.magnitude / maxRBSpeed);
        movementAudioSource.volume = Mathf.Lerp(
            0f,
            maxMovementVolume,
            speed);

        engineAudioSource.volume = Mathf.Lerp(
            minEngineVolume,
            maxEngineVolume,
            speed);
    }

    public void PlayEnterSubmarine()
    {
        audioSource.PlayOneShot(enterSubmarineClip);
    }

    public void PlaySonarPing()
    {
        int i = Random.Range(0, sonarAudioClips.Length);
        audioSource.PlayOneShot(sonarAudioClips[i], sonarPingVolume);
    }

    public void PlayUpgradeUIHover()
    {
        upgradeConsoleAudioSource.pitch = Random.Range(0.95f, 1.05f);
        upgradeConsoleAudioSource.PlayOneShot(upgradeUIHoverClip);
    }

    public void PlayUpgradeUIClick()
    {
        upgradeConsoleAudioSource.pitch = Random.Range(0.95f, 1.05f);
        upgradeConsoleAudioSource.PlayOneShot(upgradeUIClickClip);
    }
}
