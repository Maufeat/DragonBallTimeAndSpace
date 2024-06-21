using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class LightmapInfo : ScriptableObject
{
    public Material _matSkybox;

    public AmbientMode _ambientMode;

    public float _fAmbientIntensity;

    public Color _colorAmbientSky;

    public Color _colorAmbientEquator;

    public Color _colorAmbientGround;

    public Cubemap _customReflection;

    public DefaultReflectionMode _reflectionMode;

    public float _fReflectionIntensity;

    public int _nReflectionBounces;

    public int _nReflectionResolution;

    public bool _bFog;

    public Color _colorFlog;

    public FogMode _nFogMode;

    public float _fDensity;

    public float _fStar;

    public float _fEnd;

    public Vector3 _lightDir;

    public Vector3 _lightRot;

    public Color _lightColor;

    public float _intensity;

    public bool _isDynLight;

    public int _nLightmapsCount;

    public LightmapsMode _lightmapsMode;

    public List<Texture2D> _lstTex;

    public float shadowDistance;

    public Vector3 _rtLightDir;
}
