using UnityEngine;
using UnityEngine.UI;

public class CrosshairController : MonoBehaviour
{
    public Image crosshairImage; // Référence à l'image du point de visée
    public float normalScale = 1f; // Échelle normale du point de visée
    public float hitScaleMultiplier = 1.2f; // Multiplicateur d'échelle lorsque la cible est touchée
    public Color hitColor = Color.red; // Couleur du point de visée lorsque la cible est touchée

    private void Start()
    {
        // Assurez-vous que l'échelle du point de visée est initialement normale
        crosshairImage.rectTransform.localScale = Vector3.one * normalScale;
    }

    public void UpdateCrosshair(bool targetHit)
    {
        // Met à jour le point de visée en fonction de si la cible a été touchée ou non
        if (targetHit)
        {
            // Si la cible est touchée, grossissez le point de visée et changez sa couleur en rouge
            crosshairImage.rectTransform.localScale = Vector3.one * normalScale * hitScaleMultiplier;
            crosshairImage.color = hitColor;
        }
        else
        {
            // Si la cible n'est pas touchée, rétablissez l'échelle et la couleur normales du point de visée
            crosshairImage.rectTransform.localScale = Vector3.one * normalScale;
            crosshairImage.color = Color.white; // ou la couleur par défaut que vous souhaitez
        }
    }
}
