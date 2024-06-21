using UnityEngine;

public class UIFocus : MonoBehaviour, ICanvasRaycastFilter
{
    public bool IsRaycastLocationValid(Vector2 sp, Camera eventCamera)
    {
        return this.IsFocus;
    }

    public bool IsFocus;
}
