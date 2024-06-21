using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ObjInfo
{
    public string _strObjName;

    public string _strFather;

    public Vector3 _v3Position;

    public Vector3 _v3Rotation;

    public Vector3 _v3Scale;

    public string _strPrefabName;

    public string _strPrefabPath;

    public int _nPriority;

    public int _nZoneID;

    public int _nStaticBatchID;

    public int _nObjType;

    public bool _bStatic;

    public bool _bCommon;

    public int _nLightmapIndex;

    public Vector4 _v4LightmapScaleOffset;

    public bool _bIsLight;

    public string _strIndexXY;

    public List<ChildLightmapInfo> _lstCLInfo;

    public int _InstanceId;
}
