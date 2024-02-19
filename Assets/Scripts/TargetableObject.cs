using UnityEngine;

public class TargetableObject : MonoBehaviour
{
    public bool isTargetable = true;

    // Méthode pour définir si l'objet est ciblable ou non
    public void SetTargetable(bool targetable)
    {
        isTargetable = targetable;
    }
}