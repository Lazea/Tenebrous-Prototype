using UnityEngine;
using UnityEngine.Animations;

public class OceanParticleField : MonoBehaviour
{
    [Header("Particles")]
    public ParentConstraint parentConstraint;

    [Header("Render Settings")]
    public Color shallowFogColor;
    public float shallowFogStartDistance;
    public float shallowFogEndDistance;
    public Color deepFogColor;
    public float deepFogStartDistance;
    public float deepFogEndDistance;
    float depth;

    [Header("Cameras")]
    public Camera[] cams;

    [Header("Directional Light")]
    public Light light;
    public float shallowLightIntensity;
    public float deepLightIntensity;

    private void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        SetPlayerAsTarget();
    }

    private void Update()
    {
        depth = Ocean.GetSamplePointDepth(transform.position);
        float t = Mathf.Clamp(depth / (Ocean.depth - 150f), 0f, 1f);

        light.intensity = Mathf.Lerp(
            shallowLightIntensity,
            deepLightIntensity,
            t);

        RenderSettings.fogStartDistance = Mathf.Lerp(
            shallowFogStartDistance,
            deepFogStartDistance,
            t);
        RenderSettings.fogEndDistance = Mathf.Lerp(
            shallowFogEndDistance,
            deepFogEndDistance,
            t);
        Color targetColor = Color.Lerp(
            shallowFogColor,
            deepFogColor,
            t);
        RenderSettings.fogColor = targetColor;
        foreach(var cam in cams)
        {
            cam.backgroundColor = targetColor;
        }
    }

    [ContextMenu("Set Player As Target")]
    public void SetPlayerAsTarget()
    {
        var playerSrc = parentConstraint.GetSource(0);
        playerSrc.weight = 1f;
        parentConstraint.SetSource(0, playerSrc);

        var subSrc = parentConstraint.GetSource(1);
        subSrc.weight = 0f;
        parentConstraint.SetSource(1, subSrc);
    }

    [ContextMenu("Set Player As Target")]
    public void SetSubmarineAsTarget()
    {
        var playerSrc = parentConstraint.GetSource(0);
        playerSrc.weight = 0f;
        parentConstraint.SetSource(0, playerSrc);

        var subSrc = parentConstraint.GetSource(1);
        subSrc.weight = 1f;
        parentConstraint.SetSource(1, subSrc);
    }
}
