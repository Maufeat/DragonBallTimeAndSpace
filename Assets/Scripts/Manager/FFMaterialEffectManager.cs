using System;
using System.Collections.Generic;
using Framework.Base;

public class FFMaterialEffectManager : IManager
{
    public void Init(FFMaterialAnimClip[] Cliplist)
    {
        foreach (FFMaterialAnimClip ffmaterialAnimClip in Cliplist)
        {
            if (!string.IsNullOrEmpty(ffmaterialAnimClip.AnimName))
            {
                this.MaterialEffectMap[ffmaterialAnimClip.AnimName] = ffmaterialAnimClip;
            }
        }
    }

    public void LoadFromAB(Action callback)
    {
        FFMaterialAnimClip[] Cliplist = null;
        FFAssetBundleRequest.Request("Config", "materialanim", delegate (FFAssetBundle ab)
        {
            Cliplist = ab.GetAllAsset<FFMaterialAnimClip>();
            this.Init(Cliplist);
            if (callback != null)
            {
                callback();
            }
        }, true);
    }

    public void LoadFromProto(Action callback)
    {
        ScriptableToProto.Read<FFMaterialAnimClip>("config/materialanim.bytes", delegate (FFMaterialAnimClip config)
        {
            if (config != null)
            {
                this.Init(config.ProtoList);
            }
            if (callback != null)
            {
                callback();
            }
        });
    }

    public FFMaterialAnimClip GetClip(string Key)
    {
        if (!this.MaterialEffectMap.ContainsKey(Key))
        {
            FFDebug.LogError(this, "No MaterialEffect Clip :" + Key);
            return null;
        }
        return this.MaterialEffectMap[Key];
    }

    public string ManagerName
    {
        get
        {
            return base.GetType().ToString();
        }
    }

    public void OnUpdate()
    {
    }

    public void OnReSet()
    {
    }

    private Dictionary<string, FFMaterialAnimClip> MaterialEffectMap = new Dictionary<string, FFMaterialAnimClip>();
}
