using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ScopeImageController : MonoBehaviour
{
    public Image tryScopeImage; // Référence à l'image "TryScope"
    public Image hitScopeImage; // Référence à l'image "HitScope"
    public Animator ScopeZoom;

    void Start()
    {
        // Assurez-vous que l'image "HitScope" est désactivée au démarrage
        hitScopeImage.gameObject.SetActive(false);
    }

    public void ShowTryScope()
    {
        // Affiche l'image "TryScope" pendant 1 seconde
        tryScopeImage.gameObject.SetActive(true);
        StartCoroutine(HideScope(tryScopeImage));
    }

    public void ShowHitScope()
    {
        // Affiche l'image "HitScope" pendant 1 seconde
        hitScopeImage.gameObject.SetActive(true);
        StartCoroutine(HideScope(hitScopeImage));
    }

    // Méthode pour déclencher l'animation "HitScopePop"
    public void PlayScopeZoom()
    {
        ScopeZoom.Play("HitScopePop");
    }


    IEnumerator HideScope(Image scopeImage)
    {
        // Attend 1 seconde puis désactive l'image
        yield return new WaitForSeconds(0.7f);
        scopeImage.gameObject.SetActive(false);
    }

}
