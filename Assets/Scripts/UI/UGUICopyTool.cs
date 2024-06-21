using System;
using UnityEngine;
using UnityEngine.UI;

public class UGUICopyTool
{
    public static Image ImageCopy(GameObject targetroot, GameObject orgroot, string path, Sprite Msprite = null)
    {
        if (targetroot == null || orgroot == null)
        {
            return null;
        }
        Transform transform;
        if (string.IsNullOrEmpty(path))
        {
            transform = orgroot.transform;
        }
        else
        {
            transform = orgroot.transform.FindChild(path);
        }
        if (transform != null)
        {
            Image component = transform.gameObject.GetComponent<Image>();
            if (component != null)
            {
                Transform transform2 = null;
                if (string.IsNullOrEmpty(path))
                {
                    transform = targetroot.transform;
                }
                else
                {
                    transform2 = targetroot.transform.FindChild(path);
                }
                if (transform2 != null)
                {
                    return UGUICopyTool.ImageCopy(transform2.gameObject, component, Msprite);
                }
            }
        }
        return null;
    }

    public static Image ImageCopy(GameObject target, Image org, Sprite Msprite = null)
    {
        Image image = null;
        if (org != null)
        {
            if (target == null)
            {
                target = UnityEngine.Object.Instantiate<GameObject>(org.gameObject);
            }
            image = target.GetComponent<Image>();
            if (image == null)
            {
                image = target.AddComponent<Image>();
                Sprite sprite = null;
                if (Msprite != null)
                {
                    sprite = org.sprite;
                    org.sprite = Msprite;
                }
                image.eventAlphaThreshold = org.eventAlphaThreshold;
                image.fillAmount = org.fillAmount;
                image.fillCenter = org.fillCenter;
                image.fillClockwise = org.fillClockwise;
                image.fillMethod = org.fillMethod;
                image.fillOrigin = org.fillOrigin;
                image.overrideSprite = org.overrideSprite;
                image.preserveAspect = org.preserveAspect;
                image.sprite = org.sprite;
                image.type = org.type;
                if (Msprite != null)
                {
                    org.sprite = sprite;
                }
            }
        }
        return image;
    }

    public static Text TextCopy(GameObject targetroot, GameObject orgroot, string path)
    {
        if (targetroot == null || orgroot == null)
        {
            return null;
        }
        Transform transform;
        if (string.IsNullOrEmpty(path))
        {
            transform = orgroot.transform;
        }
        else
        {
            transform = orgroot.transform.FindChild(path);
        }
        if (transform != null)
        {
            Text component = transform.gameObject.GetComponent<Text>();
            if (component != null)
            {
                Transform transform2 = null;
                if (string.IsNullOrEmpty(path))
                {
                    transform = targetroot.transform;
                }
                else
                {
                    transform2 = targetroot.transform.FindChild(path);
                }
                if (transform2 != null)
                {
                    return UGUICopyTool.TextCopy(transform2.gameObject, component);
                }
            }
        }
        return null;
    }

    public static Text TextCopy(GameObject target, Text org)
    {
        Text text = null;
        if (org != null)
        {
            if (target == null)
            {
                target = UnityEngine.Object.Instantiate<GameObject>(org.gameObject);
            }
            text = target.GetComponent<Text>();
            if (text == null)
            {
                text = target.AddComponent<Text>();
                text.font = org.font;
                text.fontStyle = org.fontStyle;
                text.fontSize = org.fontSize;
                text.lineSpacing = org.lineSpacing;
                text.alignment = org.alignment;
                text.horizontalOverflow = org.horizontalOverflow;
                text.color = org.color;
            }
        }
        return text;
    }
}
