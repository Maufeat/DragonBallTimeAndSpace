using System;
using System.Collections.Generic;
using Framework.Base;
using Framework.Managers;
using UnityEngine;

public class CharacterModelMgr : IManager
{
    private void Init(AvatarDatas[] Cliplist)
    {
        foreach (AvatarDatas avatarDatas in Cliplist)
        {
            this.CombineCharacterConfig[avatarDatas.Character] = avatarDatas;
        }
    }

    public void LoadFromAB(Action callback)
    {
        AvatarDatas[] Cliplist;
        FFAssetBundleRequest.Request("Config", "charactermodelconfig.u", delegate (FFAssetBundle ab)
        {
            Cliplist = ab.GetAllAsset<AvatarDatas>();
            this.Init(Cliplist);
            if (callback != null)
            {
                callback();
            }
        }, true);
        FFAssetBundleRequest.Request("Characters", "Characters", delegate (FFAssetBundle ab)
        {
        }, true);
    }

    public void LoadFromProto(Action callback)
    {
        ScriptableToProto.Read<AvatarDatas>("config/avatardatas.bytes", delegate (AvatarDatas config)
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

    public void LoadCharacterElement(string key, Action callback)
    {
        string Key = key.ToLower();
        if (FFCharacterElement.FFCharacterElementMap.ContainsKey(Key))
        {
            if (callback != null)
            {
                callback();
            }
        }
        else
        {
            FFAssetBundleRequest.CleverRequest(CharactorAndEffectBundleType.Charactor, "CombineCharacter/" + Key + ".u", delegate (FFAssetBundle ab)
            {
                FFCharacterElement value = new FFCharacterElement(ab, Key);
                FFCharacterElement.FFCharacterElementMap[Key] = value;
                if (callback != null)
                {
                    callback();
                }
            });
        }
    }

    public void LoadAvatarPartObj(string respath, CharactorAndEffectBundleType btype, string key, string partName, Action<GameObjectInAB> callback)
    {
        string _Key = key.ToLower() + "|" + partName.ToLower();
        if (this.GameABObjMap.ContainsKey(_Key) && callback != null)
        {
            callback(this.GameABObjMap[_Key]);
            return;
        }
        FFAssetBundleRequest.CleverRequest(btype, respath + "/" + partName, delegate (FFAssetBundle ab)
        {
            this.GameABObjMap[_Key] = new GameObjectInAB(ab);
            if (callback != null)
            {
                callback(this.GameABObjMap[_Key]);
            }
        });
    }

    public void LoadCharacterElementArray(string[] RendererObjs, Action callback)
    {
        int loadCount = 0;
        for (int i = 0; i < RendererObjs.Length; i++)
        {
            this.LoadCharacterElement(RendererObjs[i], delegate
            {
                loadCount++;
                if (loadCount == RendererObjs.Length && callback != null)
                {
                    callback();
                }
            });
        }
    }

    public void LoadSimpleCharacterObj(string key, Action callback)
    {
        if (string.IsNullOrEmpty(key))
        {
            FFDebug.LogWarning(this, "LoadSimpleCharacterObj key null");
            if (callback != null)
            {
                callback();
            }
            return;
        }
        string Key = key.ToLower();
        if (this.GameABObjMap.ContainsKey(Key))
        {
            if (callback != null)
            {
                callback();
            }
        }
        else
        {
            FFAssetBundleRequest.CleverRequest(CharactorAndEffectBundleType.Charactor, Key, delegate (FFAssetBundle ab)
            {
                GameObjectInAB gameObjectInAB = new GameObjectInAB(ab);
                if (gameObjectInAB.Gobj == null)
                {
                    Debug.LogError("Character/" + Key + ".u not exits.");
                }
                this.GameABObjMap[Key] = gameObjectInAB;
                if (callback != null)
                {
                    callback();
                }
            });
        }
    }

    public void UnLoadSimpleCharacterObj(string key)
    {
        string key2 = key.ToLower();
        if (this.GameABObjMap.ContainsKey(key2))
        {
            this.GameABObjMap[key2].Unload();
            this.GameABObjMap.Remove(key2);
        }
        else
        {
            FFDebug.LogWarning(this, "cant find  key : " + key + "  in  SimpleCharacterObjMap");
        }
    }

    public AvatarDatas GetCombineCharacterConfig(string Key)
    {
        string text = Key.ToLower();
        if (this.CombineCharacterConfig.ContainsKey(text))
        {
            return this.CombineCharacterConfig[text];
        }
        FFDebug.LogWarning(this, "cant find  key : " + text + "  in  CombineCharacterConfig");
        return null;
    }

    public AvatarDatas GetCombineCharacterConfig(string Character, string Model)
    {
        return this.GetCombineCharacterConfig(Character + "@" + Model);
    }

    public GameObject GetCombineCharacterbaseObj(string Key)
    {
        string text = Key.ToLower();
        if (this.CombineCharacterbaseObjMap.ContainsKey(text))
        {
            return this.CombineCharacterbaseObjMap[text].Gobj;
        }
        FFDebug.LogWarning(this, "cant find  key : " + text + "  in  CombineCharacterbaseObjMap");
        return null;
    }

    public GameObject GetAvatarPartObj(string key, string partName)
    {
        string text = key.ToLower() + "|" + partName.ToLower();
        if (this.GameABObjMap.ContainsKey(text))
        {
            return this.GameABObjMap[text].Gobj;
        }
        FFDebug.LogWarning(this, "cant find  key : " + text + "  in  CombineCharacterbaseObjMap");
        return null;
    }

    public GameObject GetSimpleCharacterObj(string Key)
    {
        string text = Key.ToLower();
        if (this.GameABObjMap.ContainsKey(text))
        {
            return this.GameABObjMap[text].Gobj;
        }
        FFDebug.LogWarning(this, "cant find  key : " + text + "  in  CombineCharacterbaseObjMap");
        return null;
    }

    public void SetMeshAndMaterialSmr(string Key, SkinnedMeshRenderer Smr)
    {
        GameObject simpleCharacterObj = this.GetSimpleCharacterObj(Key);
        if (simpleCharacterObj != null)
        {
            SkinnedMeshRenderer componentInChildren = simpleCharacterObj.GetComponentInChildren<SkinnedMeshRenderer>();
            if (componentInChildren != null)
            {
                Smr.sharedMesh = componentInChildren.sharedMesh;
                Smr.sharedMaterials = componentInChildren.sharedMaterials;
            }
        }
    }

    public void UnloadAllModel()
    {
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
        this.UnloadAll();
        this.CombineCharacterConfig.Clear();
    }

    public void UnloadAll()
    {
        FFCharacterModelHold.UnloadAllbeside(new List<string>());
        this.CombineCharacterbaseObjMap.BetterForeach(delegate (KeyValuePair<string, GameObjectInAB> item)
        {
            item.Value.Unload();
        });
        this.GameABObjMap.BetterForeach(delegate (KeyValuePair<string, GameObjectInAB> item)
        {
            item.Value.Unload();
        });
        this.GameABObjMap.Clear();
        this.CombineCharacterbaseObjMap.Clear();
    }

    public void AddUnloadbesideByMainplayer()
    {
        if (MainPlayer.Self != null && MainPlayer.Self.ModelHoldHelper != null)
        {
            string text = MainPlayer.Self.ModelHoldHelper.rootKey.ToString() + "|" + MainPlayer.Self.ModelHoldHelper.bodyName.ToString();
            this.BesideSkinName.Add(text.Trim());
            string text2 = MainPlayer.Self.ModelHoldHelper.rootKey.ToString() + "|" + MainPlayer.Self.ModelHoldHelper.headName.ToString();
            this.BesideSkinName.Add(text2.Trim());
            string text3 = MainPlayer.Self.ModelHoldHelper.rootKey.ToString() + "|" + MainPlayer.Self.ModelHoldHelper.hairName.ToString();
            this.BesideSkinName.Add(text3.Trim());
            string item = MainPlayer.Self.ModelHoldHelper.rootKey.ToString() + "|" + MainPlayer.Self.ModelHoldHelper.antennaName.ToString();
            this.BesideSkinName.Add(item);
        }
    }

    public void Unloadbeside()
    {
        this.RmoveList.Clear();
        this.GameABObjMap.BetterForeach(delegate (KeyValuePair<string, GameObjectInAB> item)
        {
            if (!this.BesideSkinName.Contains(item.Key.Trim()))
            {
                this.RmoveList.Add(item.Key.Trim());
            }
        });
        for (int i = 0; i < this.RmoveList.Count; i++)
        {
            ManagerCenter.Instance.GetManager<CharacterModelMgr>().UnLoadSimpleCharacterObj(this.RmoveList[i]);
        }
        this.BesideSkinName.Clear();
    }

    private Dictionary<string, AvatarDatas> CombineCharacterConfig = new Dictionary<string, AvatarDatas>();

    private BetterDictionary<string, GameObjectInAB> CombineCharacterbaseObjMap = new BetterDictionary<string, GameObjectInAB>();

    private BetterDictionary<string, GameObjectInAB> GameABObjMap = new BetterDictionary<string, GameObjectInAB>();

    private List<string> BesideSkinName = new List<string>();

    private List<string> RmoveList = new List<string>();
}
