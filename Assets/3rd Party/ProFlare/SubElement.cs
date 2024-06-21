using System;
using UnityEngine;

[Serializable]
public class SubElement
{
    public Color color = Color.white;

    public Color colorFinal = Color.white;

    public float position;

    public Vector3 offset = Vector2.zero;

    public float angle;

    public float scale;

    public float random = 0.5f;

    public float random2 = 0.5f;

    public float RandomScaleSeed = 0.5f;

    public float RandomColorSeedR = 0.5f;

    public float RandomColorSeedG = 0.5f;

    public float RandomColorSeedB = 0.5f;

    public float RandomColorSeedA = 0.5f;
}
