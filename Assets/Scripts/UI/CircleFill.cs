using UnityEngine;
using UnityEngine.UI;

public class CircleFill : MonoBehaviour
{
    public Image circleImage;
    public Color emptyColor;
    public Color fullColor;

    float targetFill;
    public float fillSmooth;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        circleImage.fillAmount = Mathf.Lerp(
            circleImage.fillAmount,
            targetFill,
            fillSmooth);
        circleImage.color = Color.Lerp(
            emptyColor,
            fullColor,
            circleImage.fillAmount);
    }

    public void SetFillValue(float value)
    {
        targetFill = value;
    }

    [ContextMenu("Set Fill To Empty")]
    void SetFillToEmpty()
    {
        SetFillValue(0f);
    }

    [ContextMenu("Set Fill To Half")]
    void SetFillToHalf()
    {
        SetFillValue(0.5f);
    }

    [ContextMenu("Set Fill To Full")]
    void SetFillToFull()
    {
        SetFillValue(1f);
    }
}
