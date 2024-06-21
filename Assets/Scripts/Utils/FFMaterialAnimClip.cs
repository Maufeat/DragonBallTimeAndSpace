using System;
using System.Collections.Generic;
using ProtoBuf;
using UnityEngine;

[ProtoContract]
public class FFMaterialAnimClip : ScriptableObject
{
    [ProtoMember(126)]
    public string UObjname
    {
        get
        {
            return base.name;
        }
        set
        {
            base.name = value;
        }
    }

    public bool CheckContains(string key)
    {
        string text = key;
        if (!text.StartsWith("_"))
        {
            text = "_" + text;
        }
        if (this.AllCurveData == null)
        {
            this.AllCurveData = new FFCurveData[0];
        }
        for (int i = 0; i < this.AllCurveData.Length; i++)
        {
            if (this.AllCurveData[i].propertyName == text)
            {
                return true;
            }
        }
        return false;
    }

    public void Add(FFCurveData data)
    {
        if (data == null)
        {
            return;
        }
        if (data.curve == null)
        {
            return;
        }
        if (string.IsNullOrEmpty(data.propertyName))
        {
            return;
        }
        if (this.CheckContains(data.propertyName))
        {
            return;
        }
        if (this.AllCurveData == null)
        {
            this.AllCurveData = new FFCurveData[0];
        }
        List<FFCurveData> list = new List<FFCurveData>();
        list.AddRange(this.AllCurveData);
        list.Add(data);
        this.AllCurveData = list.ToArray();
    }

    public void Remove(string key)
    {
        if (string.IsNullOrEmpty(key))
        {
            return;
        }
        if (this.AllCurveData == null)
        {
            this.AllCurveData = new FFCurveData[0];
        }
        string text = key;
        if (!text.StartsWith("_"))
        {
            text = "_" + text;
        }
        List<FFCurveData> list = new List<FFCurveData>();
        list.AddRange(this.AllCurveData);
        FFCurveData ffcurveData = null;
        for (int i = 0; i < this.AllCurveData.Length; i++)
        {
            if (this.AllCurveData[i].propertyName == text)
            {
                ffcurveData = this.AllCurveData[i];
                break;
            }
        }
        if (ffcurveData != null)
        {
            list.Remove(ffcurveData);
            this.AllCurveData = list.ToArray();
        }
    }

    [ProtoMember(127)]
    [NonSerialized]
    public FFMaterialAnimClip[] ProtoList;

    [ProtoMember(1)]
    public string AnimName;

    [ProtoMember(2)]
    public bool Isloop;

    [ProtoMember(3)]
    public uint Duration;

    [ProtoMember(4)]
    public uint StartDalay;

    [ProtoMember(5)]
    public FFCurveData[] AllCurveData;
}
