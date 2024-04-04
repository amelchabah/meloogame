using UnityEngine;
using UnityEngine.UI;

public class CrosshairController : MonoBehaviour
{
    public Image crosshairImage; 
    public float normalScale = 1f; 
    public float hitScaleMultiplier = 1.2f; 
    public Color hitColor = Color.red;

    private void Start()
    {
        crosshairImage.rectTransform.localScale = Vector3.one * normalScale;
    }

    public void UpdateCrosshair(bool targetHit)
    {
        if (targetHit)
        {
            crosshairImage.rectTransform.localScale = Vector3.one * normalScale * hitScaleMultiplier;
            crosshairImage.color = hitColor;
        }
        else
        {
            crosshairImage.rectTransform.localScale = Vector3.one * normalScale;
            crosshairImage.color = Color.white; 
        }
    }
}
