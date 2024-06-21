using System;
using System.Collections.Generic;
using Framework.Base;

public class BipBindDataMgr : IManager
{
    public void Init(BipBindData[] BipBindDatalist)
    {
        foreach (BipBindData bipBindData in BipBindDatalist)
        {
            this.BipBindDataMap[bipBindData.ModelName] = bipBindData;
        }
    }

    public void LoadFromAB(Action callback)
    {
        FFAssetBundleRequest.Request("Config", "bipbinddatas", delegate (FFAssetBundle ab)
        {
            BipBindData[] allAsset = ab.GetAllAsset<BipBindData>();
            this.Init(allAsset);
            if (callback != null)
            {
                callback();
            }
        }, true);
    }

    public void LoadFromProto(Action callback)
    {
        ScriptableToProto.Read<BipBindData>("config/bipbinddatas.bytes", delegate (BipBindData config)
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

    public BipBindData GetClip(string Key)
    {
        if (!this.BipBindDataMap.ContainsKey(Key))
        {
            FFDebug.Log(this, FFLogType.Default, "No BipBindData  :" + Key);
            return null;
        }
        return this.BipBindDataMap[Key];
    }

    public List<string> GetAllBipBindDataMap()
    {
        return new List<string>(this.BipBindDataMap.Keys);
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

    private Dictionary<string, BipBindData> BipBindDataMap = new Dictionary<string, BipBindData>();
}
