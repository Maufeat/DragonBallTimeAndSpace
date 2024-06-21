using System;
using UnityEngine;

[Serializable]
public class FogAndLightConfig
{
    public string areaName = "A";

    public Color fogColor = Color.white;

    public FogMode fogMode = FogMode.Exponential;

    public float fogDensity = 0.01f;

    public float fogStart;

    public float fogEnd = 300f;

    public Color lightColor = Color.white;

    public float lightIntensity = 1f;

    public float skyboxAlpha;
}
