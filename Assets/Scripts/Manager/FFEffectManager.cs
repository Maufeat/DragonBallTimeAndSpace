using System;
using System.Collections.Generic;
using Framework.Base;
using Framework.Managers;
using LuaInterface;
using UnityEngine;

public class FFEffectManager : IManager
{
    public ulong GetPoolIndex()
    {
        this._poolIndex += 1UL;
        if (this._poolIndex >= 18446744073709551615UL)
        {
            this._poolIndex = 0UL;
        }
        return this._poolIndex;
    }

    public void SetObjectPoolUnit(ulong tempid, string effname, ObjectInPoolBase pool)
    {
        this.SetObjectPoolUnit(this.GetPoolIndex(), tempid, effname, pool);
    }

    public void SetObjectPoolUnit(ulong poolidx, ulong tempid, string effname, ObjectInPoolBase pool)
    {
        ObjectInPoolUnit item = new ObjectInPoolUnit(poolidx, tempid, effname, pool);
        this.unitPoolList.Add(item);
    }

    public void RemoveObjectPoolUnit(ulong tempid, string effname, RemovePoolUnitType rmtype, ulong poolidx = 0UL)
    {
        List<ObjectInPoolUnit> list = new List<ObjectInPoolUnit>();
        for (int i = 0; i < this.unitPoolList.Count; i++)
        {
            bool flag = false;
            ObjectInPoolUnit objectInPoolUnit = this.unitPoolList[i];
            switch (rmtype)
            {
                case RemovePoolUnitType.TEMPID:
                    if (objectInPoolUnit.tempid == tempid)
                    {
                        flag = true;
                    }
                    break;
                case RemovePoolUnitType.EFFNAME:
                    if (objectInPoolUnit.effName == effname)
                    {
                        flag = true;
                    }
                    break;
                case RemovePoolUnitType.IDANDNAME:
                    if (objectInPoolUnit.tempid == tempid && objectInPoolUnit.effName == effname && objectInPoolUnit.storeID == poolidx)
                    {
                        flag = true;
                    }
                    break;
                case RemovePoolUnitType.ALL:
                    flag = true;
                    break;
            }
            if (flag)
            {
                if (objectInPoolUnit.objInPool != null && !objectInPoolUnit.objInPool.DisableAndBackToPool(true))
                {
                    UnityEngine.Object.Destroy(objectInPoolUnit.objInPool.ItemObj);
                }
                list.Add(objectInPoolUnit);
            }
        }
        for (int j = 0; j < list.Count; j++)
        {
            if (this.unitPoolList.Contains(list[j]))
            {
                this.unitPoolList.Remove(list[j]);
            }
        }
    }

    public void Init(EffectClip[] Cliplist, EffectGroup[] Grouplist)
    {
        if (Cliplist == null)
        {
            FFDebug.LogWarning(this, "Cliplist null");
        }
        if (Grouplist == null)
        {
            FFDebug.LogWarning(this, "Grouplist null");
        }
        foreach (EffectClip effectClip in Cliplist)
        {
            if (!string.IsNullOrEmpty(effectClip.ClipName))
            {
                this.EffectClipMap[effectClip.ClipName] = effectClip;
            }
        }
        foreach (EffectGroup effectGroup in Grouplist)
        {
            FFDebug.Log(this, FFLogType.Effect, effectGroup.GroupName);
            if (!string.IsNullOrEmpty(effectGroup.GroupName))
            {
                this.EffectGroupMap[effectGroup.GroupName] = effectGroup;
            }
        }
    }

    public void OnEffectUserd(GameObject go, string name)
    {
        BundleDetective bundleDetective = go.GetComponent<BundleDetective>();
        if (null == bundleDetective)
        {
            bundleDetective = go.AddComponent<BundleDetective>();
            bundleDetective.BundleOnAddCallBack = new Action<string>(this.OnEffectAddUse);
            bundleDetective.BundleOnDestryCallBack = new Action<string>(this.OnEffectReduceUse);
        }
        bundleDetective.AniBundleName = name;
    }

    public void OnEffectAddUse(string key)
    {
        if (string.IsNullOrEmpty(key))
        {
            return;
        }
        if (this.EffectObjMap.ContainsKey(key))
        {
            this.EffectObjMap[key].RefCount++;
        }
    }

    public void OnEffectReduceUse(string key)
    {
        if (string.IsNullOrEmpty(key))
        {
            return;
        }
        if (this.EffectObjMap.ContainsKey(key))
        {
            this.EffectObjMap[key].RefCount--;
            int refCount = this.EffectObjMap[key].RefCount;
            if (this.EffectObjMap[key].RefCount <= 0)
            {
                this.EffectObjMap[key].Unload();
                this.EffectObjMap.Remove(key);
            }
        }
    }

    public void LoadFromProto(Action callback)
    {
        SimpleTaskQueue simpleTaskQueue = new SimpleTaskQueue();
        simpleTaskQueue.Finish = delegate ()
        {
            callback();
        };
        simpleTaskQueue.AddTask(new Action<Action>(this.LoadEffectClip));
        simpleTaskQueue.AddTask(new Action<Action>(this.LoadEffectGroup));
        simpleTaskQueue.AddTask(new Action<Action>(this.LoadFlyObj));
        simpleTaskQueue.Start();
    }

    private void LoadEffectClipold(Action callback)
    {
        FFAssetBundleRequest.Request("Config", "effectassets", delegate (FFAssetBundle ab)
        {
            foreach (EffectClip effectClip in ab.GetAllAsset<EffectClip>())
            {
                if (!string.IsNullOrEmpty(effectClip.ClipName))
                {
                    this.EffectClipMap[effectClip.ClipName] = effectClip;
                }
            }
            FFDebug.Log(this, FFLogType.Effect, "LoadEffectAssets over" + this.EffectClipMap.Count);
            callback();
        }, true);
    }

    public void LoadEffectClip(Action callback)
    {
        ScriptableToProto.Read<EffectClip>("config/effectassets.bytes", delegate (EffectClip config)
        {
            if (config != null && config.ProtoList != null)
            {
                for (int i = 0; i < config.ProtoList.Length; i++)
                {
                    EffectClip effectClip = config.ProtoList[i];
                    if (!string.IsNullOrEmpty(effectClip.ClipName))
                    {
                        this.EffectClipMap[effectClip.ClipName] = effectClip;
                    }
                }
            }
            if (callback != null)
            {
                callback();
            }
        });
    }

    private void LoadEffectGroupold(Action callback)
    {
        FFAssetBundleRequest.Request("Config", "effectgroup", delegate (FFAssetBundle ab)
        {
            foreach (EffectGroup effectGroup in ab.GetAllAsset<EffectGroup>())
            {
                FFDebug.Log(this, FFLogType.Effect, effectGroup.GroupName);
                if (!string.IsNullOrEmpty(effectGroup.GroupName))
                {
                    this.EffectGroupMap[effectGroup.GroupName] = effectGroup;
                }
            }
            FFDebug.Log(this, FFLogType.Effect, "LoadEffectGroup over :" + this.EffectGroupMap.Count);
            callback();
        }, true);
    }

    public void LoadEffectGroup(Action callback)
    {
        ScriptableToProto.Read<EffectGroup>("config/effectgroup.bytes", delegate (EffectGroup config)
        {
            if (config != null && config.ProtoList != null)
            {
                for (int i = 0; i < config.ProtoList.Length; i++)
                {
                    EffectGroup effectGroup = config.ProtoList[i];
                    FFDebug.Log(this, FFLogType.Effect, effectGroup.GroupName);
                    if (!string.IsNullOrEmpty(effectGroup.GroupName))
                    {
                        this.EffectGroupMap[effectGroup.GroupName] = effectGroup;
                    }
                }
            }
            if (callback != null)
            {
                callback();
            }
        });
    }

    private void LoadFlyObjold(Action callback)
    {
        FFAssetBundleRequest.Request("Config", "flyobj", delegate (FFAssetBundle ab)
        {
            foreach (FlyObjConfig flyObjConfig in ab.GetAllAsset<FlyObjConfig>())
            {
                FFDebug.Log(this, FFLogType.Effect, flyObjConfig.FlyobjNmae);
                if (!string.IsNullOrEmpty(flyObjConfig.FlyobjNmae))
                {
                    this.FlyObjConfigMap[flyObjConfig.FlyobjNmae] = flyObjConfig;
                }
            }
            FFDebug.Log(this, FFLogType.Effect, "LoadFlyObj over" + this.FlyObjConfigMap.Count);
            callback();
        }, true);
    }

    public void LoadFlyObj(Action callback)
    {
        ScriptableToProto.Read<FlyObjConfig>("config/flyobj.bytes", delegate (FlyObjConfig config)
        {
            if (config != null && config.ProtoList != null)
            {
                for (int i = 0; i < config.ProtoList.Length; i++)
                {
                    FlyObjConfig flyObjConfig = config.ProtoList[i];
                    if (!string.IsNullOrEmpty(flyObjConfig.FlyobjNmae))
                    {
                        this.FlyObjConfigMap[flyObjConfig.FlyobjNmae] = flyObjConfig;
                    }
                }
            }
            if (callback != null)
            {
                callback();
            }
        });
    }

    public void PreLoadEffectGroup(string GroupName, Action callback)
    {
        string[] effects = this.GetGroup(GroupName);
        if (effects.Length == 0 && callback != null)
        {
            callback();
        }
        int index = 0;
        for (int i = 0; i < effects.Length; i++)
        {
            EffectClip clip = this.GetClip(effects[i]);
            if (clip == null)
            {
                index++;
                if (index == effects.Length && callback != null)
                {
                    callback();
                }
            }
            else
            {
                string effectName = clip.EffectName;
                this.LoadEffobj(effectName, delegate
                {
                    index++;
                    if (index == effects.Length && callback != null)
                    {
                        callback();
                    }
                });
            }
        }
    }

    public void LoadEffobj(string key, Action callback)
    {
        if (this.EffectObjMap.ContainsKey(key))
        {
            callback();
        }
        else
        {
            FFAssetBundleRequest.CleverRequest(CharactorAndEffectBundleType.Effect, key, delegate (FFAssetBundle ab)
            {
                this.EffectObjMap[key] = new GameObjectInAB(ab);
                callback();
            });
        }
    }

    public void LoadUIEffobj(string key, Action callback)
    {
        if (this.EffectObjMap.ContainsKey(key))
        {
            callback();
        }
        else
        {
            FFAssetBundleRequest.CleverRequest(CharactorAndEffectBundleType.UIEffect, key, delegate (FFAssetBundle ab)
            {
                this.EffectObjMap[key] = new GameObjectInAB(ab);
                this.EffectObjMap[key].RemoveByRef = true;
                callback();
            });
        }
    }

    public ObjectPool<EffectObjInPool> GetEffobj(string Key)
    {
        if (!this.EffectObjMap.ContainsKey(Key))
        {
            return null;
        }
        ObjectPool<EffectObjInPool> objectPool = null;
        if (this.EffectObjMap[Key] != null)
        {
            objectPool = ManagerCenter.Instance.GetManager<ObjectPoolManager>().GetObjectPool<EffectObjInPool>(Key, true);
            if (objectPool == null)
            {
                if (this.EffectObjMap[Key].Gobj != null)
                {
                    GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.EffectObjMap[Key].Gobj);
                    gameObject.name = this.EffectObjMap[Key].Gobj.name;
                    objectPool = ManagerCenter.Instance.GetManager<ObjectPoolManager>().CreatPool<EffectObjInPool>(gameObject, null, null, true, string.Empty);
                }
                else
                {
                    FFDebug.LogWarning(this, " EffectObjMap Gobj null:" + Key);
                }
            }
        }
        else
        {
            FFDebug.LogWarning(this, " Effect obj null:" + Key);
        }
        return objectPool;
    }

    public EffectClip GetClip(string Key)
    {
        if (!this.EffectClipMap.ContainsKey(Key))
        {
            FFDebug.LogWarning(this, "No Effect Clip :" + Key);
            return null;
        }
        return this.EffectClipMap[Key];
    }

    public FlyObjConfig GetFlyobjConfig(string Key)
    {
        if (!this.FlyObjConfigMap.ContainsKey(Key))
        {
            FFDebug.LogWarning(this, "No FlyobjConfig :" + Key);
            return null;
        }
        return this.FlyObjConfigMap[Key];
    }

    public string[] GetGroup(string Key)
    {
        if (!this.EffectGroupMap.ContainsKey(Key))
        {
            return new string[0];
        }
        return this.EffectGroupMap[Key].Groups;
    }

    public void AddUnLoadBesideTmpList(string[] effarray)
    {
        foreach (string eff in effarray)
        {
            this.AddUnLoadBesideTmpList(eff);
        }
    }

    public void AddUnLoadBesideTmpList(string eff)
    {
        if (!this.UnloadBesideTmp.Contains(eff))
        {
            this.UnloadBesideTmp.Add(eff);
        }
    }

    public void UnloadAllEffectWithBeside()
    {
        this.EffectObjMap.BetterForeach(delegate (KeyValuePair<string, GameObjectInAB> Item)
        {
            if (!this.UnloadBesideTmp.Contains(Item.Key))
            {
                this.RemoveObjectPoolUnit(0UL, Item.Key, RemovePoolUnitType.EFFNAME, 0UL);
            }
        });
        this.EffectObjMap.BetterForeach(delegate (KeyValuePair<string, GameObjectInAB> Item)
        {
            if (!this.UnloadBesideTmp.Contains(Item.Key) && !Item.Value.RemoveByRef)
            {
                Item.Value.Unload();
                this.EffectObjMap.Remove(Item.Key);
                ManagerCenter.Instance.GetManager<ObjectPoolManager>().RemoveObjectPool(Item.Key, true);
            }
        });
        this.UnloadBesideTmp.Clear();
    }

    public void UnloadAllEffect()
    {
        this.EffectObjMap.BetterForeach(delegate (KeyValuePair<string, GameObjectInAB> Item)
        {
            this.RemoveObjectPoolUnit(0UL, Item.Key, RemovePoolUnitType.EFFNAME, 0UL);
        });
        this.EffectObjMap.BetterForeach(delegate (KeyValuePair<string, GameObjectInAB> Item)
        {
            if (!Item.Value.RemoveByRef)
            {
                Item.Value.Unload();
                this.EffectObjMap.Remove(Item.Key);
                ManagerCenter.Instance.GetManager<ObjectPoolManager>().RemoveObjectPool(Item.Key, true);
            }
        });
    }

    public void AddUnloadDirectionEffectByAction()
    {
        if (MainPlayer.Self == null)
        {
            return;
        }
        FindDirections component = MainPlayer.Self.GetComponent<FindDirections>();
        if (component == null)
        {
            return;
        }
        if (!component.HaveDirectEffect)
        {
            return;
        }
        string[] group = this.GetGroup(component.directionEffectName);
        if (group != null)
        {
            this.AddUnLoadBesideTmpList(group);
        }
    }

    public void AddUnloadBesideEffectByAction(FFActionClip clip)
    {
        if (clip == null)
        {
            return;
        }
        this.AddUnLoadBesideTmpList(clip.GetEffectsByGroupID(FFActionClip.EffectType.Type_Skill, 1U));
        this.AddUnLoadBesideTmpList(clip.GetEffectsByGroupID(FFActionClip.EffectType.Type_Hit, 1U));
        string[] effectsByGroupID = clip.GetEffectsByGroupID(FFActionClip.EffectType.Type_Fly, 1U);
        if (effectsByGroupID.Length > 0)
        {
            for (int i = 0; i < effectsByGroupID.Length; i++)
            {
                FlyObjConfig flyobjConfig = this.GetFlyobjConfig(effectsByGroupID[i]);
                if (flyobjConfig != null && flyobjConfig.EffectList != null)
                {
                    this.AddUnLoadBesideTmpList(flyobjConfig.EffectList);
                }
            }
        }
    }

    public void AddUnloadBesideEffectBySkill(string ACName, uint ActionId)
    {
        FFActionClip[] ffactionClipArr = ManagerCenter.Instance.GetManager<FFActionClipManager>().GetFFActionClipArr(ACName, ActionId);
        if (ffactionClipArr != null)
        {
            for (int i = 0; i < ffactionClipArr.Length; i++)
            {
                this.AddUnloadBesideEffectByAction(ffactionClipArr[i]);
            }
        }
    }

    public void AddUnloadBesideEffectByNpc(uint BaseId)
    {
        LuaTable configTable = LuaConfigManager.GetConfigTable("npc_data", (ulong)BaseId);
        this.AddUnloadBesideEffectBySkill(configTable.GetField_String("animatorcontroller"), 1U);
        this.AddUnloadBesideEffectBySkill(configTable.GetField_String("animatorcontroller"), 2U);
        this.AddUnloadBesideEffectBySkill(configTable.GetField_String("animatorcontroller"), 3U);
        this.AddUnloadBesideEffectBySkill(configTable.GetField_String("animatorcontroller"), 4U);
        this.AddUnloadBesideEffectBySkill(configTable.GetField_String("animatorcontroller"), 5U);
        this.AddUnloadBesideEffectBySkill(configTable.GetField_String("animatorcontroller"), 6U);
        this.AddUnloadBesideEffectBySkill(configTable.GetField_String("animatorcontroller"), 7U);
        if (configTable.GetField_Uint("bornaction") != 0U)
        {
            this.AddUnloadBesideEffectBySkill(configTable.GetField_String("animatorcontroller"), configTable.GetField_Uint("bornaction"));
        }
    }

    public void AddUnloadBesideEffectByMainPlaySkill()
    {
        if (MainPlayerSkillHolder.Instance == null)
        {
            return;
        }
        if (MainPlayer.Self == null)
        {
            return;
        }
        LuaTable[] allmainPlayerSkillStageArray = MainPlayerSkillHolder.Instance.GetALLMainPlayerSkillStageArray();
        if (allmainPlayerSkillStageArray == null)
        {
            return;
        }
        foreach (LuaTable luaTable in allmainPlayerSkillStageArray)
        {
            if (luaTable != null)
            {
                FFActionClip[] ffactionClipArr = ManagerCenter.Instance.GetManager<FFActionClipManager>().GetFFActionClipArr(MainPlayer.Self.animatorControllerName, luaTable.GetField_Uint("ActionID"));
                for (int j = 0; j < ffactionClipArr.Length; j++)
                {
                    this.AddUnloadBesideEffectByAction(ffactionClipArr[j]);
                }
            }
        }
    }

    public void AddUnloadBesideEffectByMainPlayNormalAction()
    {
        if (MainPlayer.Self != null)
        {
            FFBehaviourControl component = MainPlayer.Self.GetComponent<FFBehaviourControl>();
            if (component != null)
            {
                this.AddUnLoadBesideTmpList(component.GetCurrentAllEffectName());
            }
        }
    }

    public void AddUnloadBesideEffectByBuffEffect()
    {
        if (MainPlayer.Self == null)
        {
            return;
        }
        PlayerBufferControl component = MainPlayer.Self.GetComponent<PlayerBufferControl>();
        if (component == null)
        {
            return;
        }
        component.AllBufferMap().BetterForeach(delegate (KeyValuePair<int, BufferState> item)
        {
            this.AddUnLoadBesideTmpList(item.Value.GetActiveEffect());
        });
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
        this.UnloadAllEffect();
    }

    private Dictionary<string, EffectGroup> EffectGroupMap = new Dictionary<string, EffectGroup>();

    private Dictionary<string, EffectClip> EffectClipMap = new Dictionary<string, EffectClip>();

    private Dictionary<string, FlyObjConfig> FlyObjConfigMap = new Dictionary<string, FlyObjConfig>();

    private BetterDictionary<string, GameObjectInAB> EffectObjMap = new BetterDictionary<string, GameObjectInAB>();

    private List<ObjectInPoolUnit> unitPoolList = new List<ObjectInPoolUnit>();

    private ulong _poolIndex;

    private List<string> UnloadBesideTmp = new List<string>();
}
