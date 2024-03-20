using UnityEngine;

public class ToolAudioManager : MonoBehaviour
{
    public AudioSource soundFXAudioSource;
    public AudioClip enterWaterClip;

    [Header("Tool Switch")]
    public AudioSource toolSoundFXAudioSource;
    public AudioClip[] toolSwitchClips;

    [Header("Drill Tool")]
    public AudioSource drillingAudioSource;
    public AudioClip startDrillClip;
    public AudioClip endDrillClip;

    [Header("Blow Torch Tool")]
    public AudioSource blowTorchAudioSource;
    public AudioClip startBlowTorchClip;
    public AudioClip endBlowTorchClip;

    [Header("Harpoon Gun")]
    public AudioClip harpoonShotClip;

    [Header("Movement Audio Source")]
    public AudioSource movementAudioSource;
    float maxMovementVolume;

    [Header("Rigidbody")]
    public float maxRBSpeed = 4f;
    public Rigidbody rb;

    private void Awake()
    {
        maxMovementVolume = movementAudioSource.volume;
        movementAudioSource.volume = 0f;
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
    }

    public void PlayEnterWater()
    {
        soundFXAudioSource.PlayOneShot(enterWaterClip);
    }

    #region [Tools]
    public void PlayToolSwitch()
    {
        if (toolSoundFXAudioSource.isPlaying)
            toolSoundFXAudioSource.Stop();

        int i = Random.Range(0, toolSwitchClips.Length);
        toolSoundFXAudioSource.pitch = Random.Range(0.95f, 1.05f);
        toolSoundFXAudioSource.PlayOneShot(toolSwitchClips[i]);
    }

    #region [Drill]
    public void PlayDrilling()
    {
        if (!drillingAudioSource.isPlaying)
        {
            drillingAudioSource.Play();
        }
    }

    public void StartDrilling()
    {
        if (drillingAudioSource.isPlaying)
        {
            drillingAudioSource.Stop();
        }

        if (toolSoundFXAudioSource.isPlaying)
            toolSoundFXAudioSource.Stop();

        toolSoundFXAudioSource.pitch = 1f;
        toolSoundFXAudioSource.PlayOneShot(startDrillClip);
    }

    public void EndDrilling()
    {
        if (drillingAudioSource.isPlaying)
        {
            drillingAudioSource.Stop();
        }

        if (toolSoundFXAudioSource.isPlaying)
            toolSoundFXAudioSource.Stop();

        toolSoundFXAudioSource.pitch = 1f;
        toolSoundFXAudioSource.PlayOneShot(endDrillClip);
    }
    #endregion

    #region [BlowTorch]
    public void PlayBlowTorch()
    {
        if (!blowTorchAudioSource.isPlaying)
        {
            blowTorchAudioSource.Play();
        }
    }

    public void StartBlowTorch()
    {
        if (blowTorchAudioSource.isPlaying)
        {
            blowTorchAudioSource.Stop();
        }

        if (toolSoundFXAudioSource.isPlaying)
            toolSoundFXAudioSource.Stop();

        toolSoundFXAudioSource.pitch = 1f;
        toolSoundFXAudioSource.PlayOneShot(startBlowTorchClip);
    }

    public void EndBlowTorch()
    {
        if (blowTorchAudioSource.isPlaying)
        {
            blowTorchAudioSource.Stop();
        }

        if (toolSoundFXAudioSource.isPlaying)
            toolSoundFXAudioSource.Stop();

        toolSoundFXAudioSource.pitch = 1f;
        toolSoundFXAudioSource.PlayOneShot(endBlowTorchClip);
    }
    #endregion

    public void PlayHarpoonGunShot()
    {
        if (toolSoundFXAudioSource.isPlaying)
            toolSoundFXAudioSource.Stop();

        toolSoundFXAudioSource.pitch = Random.Range(0.95f, 1.05f);
        toolSoundFXAudioSource.PlayOneShot(harpoonShotClip);
    }
    #endregion
}
