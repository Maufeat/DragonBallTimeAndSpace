using System;
using System.Collections.Generic;
using Framework.Base;
using Framework.Managers;
using UnityEngine;

public class WeaponResourcesMgr : IManager
{
    public void Init(WeaponArtConfig[] Assetslist)
    {
        this.WeaponArtConfigMap.Clear();
        this.WeaponObjMap.Clear();
        foreach (WeaponArtConfig weaponArtConfig in Assetslist)
        {
            this.WeaponArtConfigMap[weaponArtConfig.WeaponName] = weaponArtConfig;
        }
    }

    public void LoadFromAB(Action callback)
    {
        FFAssetBundleRequest.Request("Config", "weaponartconfig.u", delegate (FFAssetBundle ab)
        {
            WeaponArtConfig[] allAsset = ab.GetAllAsset<WeaponArtConfig>();
            this.Init(allAsset);
            if (callback != null)
            {
                callback();
            }
        }, true);
    }

    public void LoadWeaponobj(string Key, Action callback)
    {
        if (this.WeaponObjMap.ContainsKey(Key))
        {
            callback();
        }
        else
        {
            FFAssetBundleRequest.CleverRequest(CharactorAndEffectBundleType.Weapon, "weaponprefab/" + Key.ToLower() + ".u", delegate (FFAssetBundle ab)
            {
                this.WeaponObjMap[Key] = new GameObjectInAB(ab);
                callback();
            });
        }
    }

    public ObjectPool<ModleObjInPool> GetWeaponobj(string Key)
    {
        if (!this.WeaponObjMap.ContainsKey(Key))
        {
            FFDebug.LogError(this, "No Weapon obj :" + Key);
            return null;
        }
        ObjectPool<ModleObjInPool> objectPool = null;
        if (this.WeaponObjMap[Key] != null)
        {
            objectPool = ManagerCenter.Instance.GetManager<ObjectPoolManager>().GetObjectPool<ModleObjInPool>(Key, true);
            if (objectPool == null && this.WeaponObjMap[Key].Gobj != null)
            {
                GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.WeaponObjMap[Key].Gobj);
                gameObject.name = this.WeaponObjMap[Key].Gobj.name;
                objectPool = ManagerCenter.Instance.GetManager<ObjectPoolManager>().CreatPool<ModleObjInPool>(this.WeaponObjMap[Key].Gobj, null, null, true, string.Empty);
            }
        }
        else
        {
            FFDebug.LogError(this, " Effect obj null:" + Key);
        }
        return objectPool;
    }

    public WeaponArtConfig GetWeaponAssets(string Key)
    {
        if (string.IsNullOrEmpty(Key))
        {
            return null;
        }
        if (!this.WeaponArtConfigMap.ContainsKey(Key))
        {
            FFDebug.LogError(this, "can not find Weapon:" + Key);
            return null;
        }
        return this.WeaponArtConfigMap[Key];
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

    public void AddUnloadbesideByMainplayer()
    {
        if (MainPlayer.Self != null)
        {
            FFWeaponHold component = MainPlayer.Self.GetComponent<FFWeaponHold>();
            if (component != null)
            {
                component.WeaponObjMap.BetterForeach(delegate (KeyValuePair<string, FFWeaponHold.FFWeaponObj> item)
                {
                    this.BesideTmp.Add(item.Value.CurrWeapon.WeaponModel);
                });
            }
        }
    }

    public void UnloadBeside()
    {
        this.WeaponObjMap.BetterForeach(delegate (KeyValuePair<string, GameObjectInAB> Item)
        {
            if (!this.BesideTmp.Contains(Item.Key))
            {
                Item.Value.Unload();
                ManagerCenter.Instance.GetManager<ObjectPoolManager>().RemoveObjectPool(Item.Key, true);
                this.WeaponObjMap.Remove(Item.Key);
            }
        });
        this.BesideTmp.Clear();
    }

    public void OnReSet()
    {
        this.UnloadBeside();
    }

    private string Weaponobjpath = "Weapon/Prefabs/";

    private string WeaponArtConfigpath = "Weapon/WeaponAssets/";

    public BetterDictionary<string, GameObjectInAB> WeaponObjMap = new BetterDictionary<string, GameObjectInAB>();

    public Dictionary<string, WeaponArtConfig> WeaponArtConfigMap = new Dictionary<string, WeaponArtConfig>();

    private List<string> BesideTmp = new List<string>();
}
