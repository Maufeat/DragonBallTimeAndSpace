using System;
using UnityEngine;

public class UIIdentifier : MonoBehaviour
{
    [SerializeField]
    public UIIdentifier.IdentifierType _type;

    public enum IdentifierType
    {
        UIRoot,
        UITransform,
        UIEffect,
        UIText,
        UIImage,
        UIRawImage,
        UIButton,
        UIToggle,
        UISlider,
        UIInputField,
        UIScrollBar,
        UIScrollRect,
        UINumberInput,
        UIBlurScreenShot
    }
}
