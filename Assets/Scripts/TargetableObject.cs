using UnityEngine;

public class TargetableObject : MonoBehaviour
{
    public bool isTargetable = true;

    public void SetTargetable(bool targetable)
    {
        isTargetable = targetable;
    }
}
