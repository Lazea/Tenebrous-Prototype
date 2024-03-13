using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SubmarineGauges : MonoBehaviour
{
    [SerializeField]
    SubmarineData data;

    [Header("Hull Gauge")]
    public Transform hullStrengthGaugeArm;
    float hullGaugeStart = 0f;
    float hullGaugeEnd;
    public float hullGaugeSmooth = 0.05f;
    float hullHealth;

    [Header("Hull Gauge Warning Light")]
    public MeshRenderer hullLightMeshRenderer;
    [ColorUsageAttribute(false, true)]
    public Color hullIdleColor;
    [ColorUsageAttribute(false, true)]
    public Color hullAlertColor;
    public Light hullLight;
    public float hullLightIntensity;
    public float hullAlertLightPulseFrequency;

    [Header("Depth Gauge")]
    public Transform depthGaugeArm;
    float depthGaugeStart = 0f;
    float depthGaugeEnd;
    public float depthGaugeSmooth = 0.05f;
    public TextMeshProUGUI depthText;
    public Image maxDepthFillBar;
    float depth;

    [Header("Depth Gauge Warning Light")]
    public MeshRenderer depthLightMeshRenderer;
    [ColorUsageAttribute(false, true)]
    public Color depthIdleColor;
    [ColorUsageAttribute(false, true)]
    public Color depthAlertColor;
    public Light depthLight;
    public float depthLightIntensity;
    public float depthAlertLightPulseFrequency;

    // Start is called before the first frame update
    void Start()
    {
        hullGaugeEnd = hullStrengthGaugeArm.localRotation.eulerAngles.z;
        depthGaugeEnd = depthGaugeArm.localPosition.y;
    }

    // Update is called once per frame
    void Update()
    {
        HandleHullGauge();
        HandleHullWarningLight();

        HandleDepthGauge();
        HandleDepthWarningLight();
    }

    void HandleHullGauge()
    {
        hullHealth = Mathf.Clamp01(data.hullHealth / (float)data.HullMaxHealth);
        float hullHealthRotation = -Mathf.Lerp(
            hullGaugeStart,
            hullGaugeEnd,
            hullHealth);
        hullStrengthGaugeArm.localRotation = Quaternion.Lerp(
            hullStrengthGaugeArm.localRotation,
            Quaternion.Euler(Vector3.forward * hullHealthRotation),
            hullGaugeSmooth);
    }

    void HandleHullWarningLight()
    {
        if (hullHealth <= 0.2f)
        {
            HandlePulseWarningLight(
                hullLightMeshRenderer,
                hullIdleColor,
                hullAlertColor,
                hullLight,
                hullLightIntensity,
                hullAlertLightPulseFrequency);
        }
        else
        {
            hullLightMeshRenderer.material.SetColor("_Emissive_Color", hullIdleColor);
            hullLight.intensity = 0f;
        }
    }

    void HandleDepthGauge()
    {
        depth = Ocean.GetSamplePointDepth(transform.position);
        float _depth = Mathf.Clamp01(depth / Ocean.depth);
        float depthGaugePosition = Mathf.Lerp(
            depthGaugeStart,
            depthGaugeEnd,
            _depth);
        depthGaugeArm.localPosition = new Vector3(
            depthGaugeArm.localPosition.x,
            Mathf.Lerp(
                depthGaugeArm.localPosition.y,
                depthGaugePosition,
                depthGaugeSmooth),
            depthGaugeArm.localPosition.z);

        depthText.text = string.Format(
            "<size=60%>Fathoms</size>\n{0}",
            Mathf.RoundToInt(depth));
        float a = data.hullDepths[data.hullLevel - 1].maxHullDepth / Ocean.depth;
        maxDepthFillBar.fillAmount = 1f - a;
    }

    void HandleDepthWarningLight()
    {
        if (depth >= data.hullDepths[data.hullLevel - 1].maxHullDepth)
        {
            HandlePulseWarningLight(
                depthLightMeshRenderer,
                depthIdleColor,
                depthAlertColor,
                depthLight,
                depthLightIntensity,
                depthAlertLightPulseFrequency);
        }
        else
        {
            depthLightMeshRenderer.material.SetColor("_Emissive_Color", depthIdleColor);
            depthLight.intensity = 0f;
        }
    }

    void HandlePulseWarningLight(
        MeshRenderer rend,
        Color colorA,
        Color colorB,
        Light light,
        float lightIntensity,
        float freq)
    {
        float t = Mathf.Sin(Time.time * freq);
        t = (t + 1) / 2;
        rend.material.SetColor(
            "_Emissive_Color",
            Color.Lerp(
                colorA,
                colorB,
                t));
        light.intensity = Mathf.Lerp(0f, lightIntensity, t);
    }
}
