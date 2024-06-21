using System;
using System.Collections.Generic;
using Framework.Base;
using LuaInterface;
using UnityEngine;

public class FFActionClipManager : IManager
{
    public void Init(FFActionClip[] ClipList)
    {
        foreach (FFActionClip ffactionClip in ClipList)
        {
            this.FFActionClipMap[ffactionClip.name] = ffactionClip;
        }
        List<LuaTable> configTableList = LuaConfigManager.GetConfigTableList("allaction");
        this.AllActionMap.Clear();
        for (int j = 0; j < configTableList.Count; j++)
        {
            LuaTable luaTable = configTableList[j];
            if (!this.AllActionMap.ContainsKey(luaTable.GetField_String("ACName").ToLower()))
            {
                this.AllActionMap.Add(luaTable.GetField_String("ACName").ToLower(), new BetterDictionary<uint, string[]>());
            }
            BetterDictionary<uint, string[]> betterDictionary = this.AllActionMap[luaTable.GetField_String("ACName").ToLower()];
            string[] array = luaTable.GetField_String("ActionList").Split(new char[]
            {
                '.'
            }, StringSplitOptions.RemoveEmptyEntries);
            if (array != null)
            {
                if (!betterDictionary.ContainsKey(luaTable.GetField_Uint("ActionId")))
                {
                    betterDictionary.Add(luaTable.GetField_Uint("ActionId"), array);
                }
                else
                {
                    FFDebug.LogWarning(this, "same key already exits:" + luaTable.GetField_Uint("ActionId"));
                }
            }
        }
    }

    public void LoadFromAB(Action callback)
    {
        FFAssetBundleRequest.Request("Config", "ActionClip", delegate (FFAssetBundle ab)
        {
            FFActionClip[] allAsset = ab.GetAllAsset<FFActionClip>();
            this.Init(allAsset);
            if (callback != null)
            {
                callback();
            }
        }, true);
    }

    public void LoadFromProto(Action callback)
    {
        ScriptableToProto.Read<FFActionClip>("config/actionclip.bytes", delegate (FFActionClip config)
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

    public string Generatekey(string ACName, string ClipName)
    {
        return ACName + "&" + ClipName;
    }

    private FFActionClip[] zeroActionClipArr
    {
        get
        {
            if (this._zeroActionClip == null)
            {
                FFActionClip ffactionClip = ScriptableObject.CreateInstance<FFActionClip>();
                ffactionClip.ACName = "none";
                ffactionClip.ClipName = "none";
                ffactionClip.SkillEffects = new string[0];
                ffactionClip.HitEffects = new string[0];
                ffactionClip.SkillMaterialEffects = new string[0];
                ffactionClip.HitMaterialEffects = new string[0];
                ffactionClip.ApplyFeetIK = true;
                ffactionClip.ApplyAniMotion = false;
                ffactionClip.CloseFistTimeF = 0U;
                ffactionClip.FakeAttackTimeFs = new uint[0];
                ffactionClip.SkillEffectList = new List<FFActionClipEffects>();
                ffactionClip.HitEffectList = new List<FFActionClipEffects>();
                ffactionClip.FlyEffectList = new List<FFActionClipEffects>();
                this._zeroActionClip = new FFActionClip[]
                {
                    ffactionClip
                };
            }
            return this._zeroActionClip;
        }
    }

    public FFActionClip[] GetFFActionClipArr(string ACName, uint ActionId)
    {
        if (ActionId == 0U)
        {
            return this.zeroActionClipArr;
        }
        this.tmpAnimClipList.Clear();
        string[] actionNameArr = this.GetActionNameArr(ACName, ActionId);
        for (int i = 0; i < actionNameArr.Length; i++)
        {
            FFActionClip ffactionClip = this.GetFFActionClip(actionNameArr[i]);
            if (ffactionClip != null)
            {
                this.tmpAnimClipList.Add(ffactionClip);
            }
        }
        return this.tmpAnimClipList.ToArray();
    }

    public FFActionClip GetFFActionClip(string ACName, uint ActionId, int Selectindex = 0)
    {
        FFActionClip[] ffactionClipArr = this.GetFFActionClipArr(ACName, ActionId);
        if (ffactionClipArr.Length == 0 && ACName != "none")
        {
            ffactionClipArr = this.GetFFActionClipArr("none", ActionId);
        }
        if (ffactionClipArr.Length == 0)
        {
            return null;
        }
        int num = (Selectindex >= ffactionClipArr.Length) ? 0 : Selectindex;
        return ffactionClipArr[num];
    }

    private FFActionClip GetFFActionClip(string key)
    {
        if (this.FFActionClipMap.ContainsKey(key))
        {
            return this.FFActionClipMap[key];
        }
        FFDebug.LogWarning(this, "not find  key: [" + key + "] in FFSkillAnimClipMap");
        return null;
    }

    private string[] GetActionNameArr(string AcName, uint ActionID)
    {
        if (this.AllActionMap.ContainsKey(AcName.ToLower()))
        {
            BetterDictionary<uint, string[]> betterDictionary = this.AllActionMap[AcName.ToLower()];
            if (betterDictionary.ContainsKey(ActionID))
            {
                return betterDictionary[ActionID];
            }
        }
        return this.nonearr;
    }

    private string[] AllActionNameArr()
    {
        List<string> TmpList = new List<string>();
        this.AllActionMap.BetterForeach(delegate (KeyValuePair<string, BetterDictionary<uint, string[]>> item)
        {
            item.Value.BetterForeach(delegate (KeyValuePair<uint, string[]> config)
            {
                for (int i = 0; i < config.Value.Length; i++)
                {
                    string itemC = config.Value[0];
                    if (!TmpList.Contains(itemC))
                    {
                        TmpList.Add(itemC);
                    }
                }
            });
        });
        return TmpList.ToArray();
    }

    private string GetIdleAction(string AcName)
    {
        return this.GetActionNameArr(AcName, 1U)[0];
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

    private BetterDictionary<string, BetterDictionary<uint, string[]>> AllActionMap = new BetterDictionary<string, BetterDictionary<uint, string[]>>();

    private BetterDictionary<string, FFActionClip> FFActionClipMap = new BetterDictionary<string, FFActionClip>();

    private List<FFActionClip> tmpAnimClipList = new List<FFActionClip>();

    private FFActionClip[] _zeroActionClip;

    private string[] nonearr = new string[0];
}
