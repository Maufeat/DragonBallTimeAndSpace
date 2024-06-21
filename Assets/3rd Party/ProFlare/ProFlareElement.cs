using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ProFlareElement
{
    public bool Editing;

    public bool Visible = true;

    public int elementTextureID;

    public string SpriteName;

    public ProFlare flare;

    public ProFlareAtlas flareAtlas;

    public float Brightness = 1f;

    public float Scale = 1f;

    public float ScaleRandom;

    public float ScaleFinal = 1f;

    public Vector4 RandomColorAmount = Vector4.zero;

    public float position;

    public bool useRangeOffset;

    public float SubElementPositionRange_Min = -1f;

    public float SubElementPositionRange_Max = 1f;

    public float SubElementAngleRange_Min = -180f;

    public float SubElementAngleRange_Max = 180f;

    public Vector3 OffsetPosition;

    public Vector3 Anamorphic = Vector3.zero;

    public Vector3 OffsetPostion = Vector3.zero;

    public float angle;

    public float FinalAngle;

    public bool useRandomAngle;

    public bool useStarRotation;

    public float AngleRandom_Min;

    public float AngleRandom_Max;

    public bool OrientToSource;

    public bool rotateToFlare;

    public float rotationSpeed;

    public float rotationOverTime;

    public bool useColorRange;

    public Color ElementFinalColor;

    public Color ElementTint = new Color(1f, 1f, 1f, 0.33f);

    public Color SubElementColor_Start = Color.white;

    public Color SubElementColor_End = Color.white;

    public bool useScaleCurve;

    public AnimationCurve ScaleCurve = new AnimationCurve(new Keyframe[]
    {
        new Keyframe(0f, 0.1f),
        new Keyframe(0.5f, 1f),
        new Keyframe(1f, 0.1f)
    });

    public bool OverrideDynamicEdgeBoost;

    public float DynamicEdgeBoostOverride = 1f;

    public bool OverrideDynamicCenterBoost;

    public float DynamicCenterBoostOverride = 1f;

    public bool OverrideDynamicEdgeBrightness;

    public float DynamicEdgeBrightnessOverride = 0.4f;

    public bool OverrideDynamicCenterBrightness;

    public float DynamicCenterBrightnessOverride = 0.4f;

    public List<SubElement> subElements = new List<SubElement>();

    public Vector2 size = Vector2.one;

    public ProFlareElement.Type type;

    public enum Type
    {
        Single,
        Multi
    }
}
