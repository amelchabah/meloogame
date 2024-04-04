using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ScopeImageController : MonoBehaviour
{
    public Image TryScopeImage; // Référence à l'image "TryScope"
    public Image HitScopeImage; // Référence à l'image "HitScope"
    public Animator ScopeZoom;
    public Animator ScopeFade;

    void Start()
    {
        // Assurez-vous que l'image "HitScope" est désactivée au démarrage
        HitScopeImage.gameObject.SetActive(false);
    }

    public void ShowTryScope()
    {
        // Affiche l'image "TryScope" pendant 1 seconde
        TryScopeImage.gameObject.SetActive(true);
        StartCoroutine(HideScope(TryScopeImage));
    }

    public void ShowHitScope()
    {
        // Affiche l'image "HitScope" pendant 1 seconde
        HitScopeImage.gameObject.SetActive(true);
        StartCoroutine(HideScope(HitScopeImage));
    }

    // Méthode pour déclencher l'animation "HitScopePop"
    public void PlayScopeZoom()
    {
        ScopeZoom.Play("HitScopePop");
    }

    public void PlayScopeFade()
    {
        ScopeFade.Play("TryScopeFade");
    }


    IEnumerator HideScope(Image ScopeImage)
    {
        // Attend 1 seconde puis désactive l'image
        yield return new WaitForSeconds(1f);
        ScopeImage.gameObject.SetActive(false);
    }

}
