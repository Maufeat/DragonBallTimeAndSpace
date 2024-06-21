using System;
using System.Collections.Generic;
using Framework.Managers;
using LuaInterface;
using UnityEngine;

public class FFWeaponHold : IFFComponent
{
    public CompnentState State { get; set; }

    private WeaponResourcesMgr mWeaponResourcesMgr
    {
        get
        {
            return ManagerCenter.Instance.GetManager<WeaponResourcesMgr>();
        }
    }

    private void AddFFWeaponObj(WeaponArtConfig config)
    {
        if (!this.WeaponObjMap.ContainsKey(config.WeaponName))
        {
            FFWeaponHold.FFWeaponObj ffweaponObj = new FFWeaponHold.FFWeaponObj(config, this);
            this.WeaponObjMap.Add(config.WeaponName, ffweaponObj);
            ffweaponObj.Init();
        }
    }

    public void ChangeWeapon(uint Weaponid)
    {
        if (Weaponid == this.CurrWeaponid)
        {
            return;
        }
        FFDebug.Log(this, FFLogType.Config, "owner->" + Weaponid);
        this.ClearWeapon();
        if (Weaponid == 0U)
        {
            return;
        }
        this.CurrWeaponid = Weaponid;
        LuaTable configTable = LuaConfigManager.GetConfigTable("objects", (ulong)Weaponid);
        try
        {
            foreach (string key in configTable.GetField_String("appearance").Split(new char[]
            {
                ','
            }, StringSplitOptions.RemoveEmptyEntries))
            {
                WeaponArtConfig weaponAssets = ManagerCenter.Instance.GetManager<WeaponResourcesMgr>().GetWeaponAssets(key);
                FFDebug.Log(this, FFLogType.Config, "appearance-->" + configTable.GetField_String("appearance"));
                this.AddFFWeaponObj(weaponAssets);
            }
        }
        catch (Exception arg)
        {
            FFDebug.LogError(this, string.Format("ChangeWeapon Error : {0}", arg));
        }
    }

    public void OnWeaponModleOver(GameObject obj)
    {
        bool HasAllLoadOver = true;
        this.WeaponObjMap.BetterForeach(delegate (KeyValuePair<string, FFWeaponHold.FFWeaponObj> item)
        {
            if (!item.Value.HadLoadOver)
            {
                HasAllLoadOver = false;
            }
        });
        if (HasAllLoadOver)
        {
            FFMaterialEffectControl component = this.Owner.GetComponent<FFMaterialEffectControl>();
            if (component != null)
            {
                component.ResetWeaponMaterial();
            }
        }
    }

    public GameObject[] GetAllWeaponObjs()
    {
        this.TmpList.Clear();
        this.WeaponObjMap.BetterForeach(delegate (KeyValuePair<string, FFWeaponHold.FFWeaponObj> item)
        {
            this.TmpList.Add(item.Value.CurrWeaponobj);
        });
        return this.TmpList.ToArray();
    }

    public void ChangeState(bool isInFight)
    {
    }

    private void ChangeWeapon(WeaponArtConfig Weapon)
    {
        if (this.CurrWeapon != null)
        {
            FFDebug.Log(this, FFLogType.Config, "CurrWeapon: " + this.CurrWeapon.WeaponName);
            FFDebug.Log(this, FFLogType.Config, "NewWeapon: " + Weapon.WeaponName);
        }
        if (this.CurrWeapon == Weapon)
        {
            return;
        }
        this.CurrWeapon = Weapon;
        if (this.CurrWeaponobj != null)
        {
            UnityEngine.Object.Destroy(this.CurrWeaponobj);
        }
    }

    private void seteffect()
    {
        if (this.CurrWeapon == null)
        {
            return;
        }
        FFEffectControl component = this.Owner.GetComponent<FFEffectControl>();
        if (component == null)
        {
            return;
        }
        FFEffectManager manager = ManagerCenter.Instance.GetManager<FFEffectManager>();
        if (component != null)
        {
            this.ClearEffect();
            for (int i = 0; i < this.CurrWeapon.WeaponEffects.Length; i++)
            {
                EffectClip clip = manager.GetClip(this.CurrWeapon.WeaponEffects[i]);
                if (clip != null)
                {
                }
            }
        }
    }

    private void ClearEffect()
    {
        for (int i = 0; i < this.FFeffectList.Count; i++)
        {
            this.FFeffectList[i].mState = FFeffect.State.Over;
        }
        this.FFeffectList.Clear();
    }

    private void SetWeaponPos()
    {
        if (this.CurrWeaponobj == null)
        {
            return;
        }
        if (this.CurrWeapon == null)
        {
            return;
        }
        FFBipBindMgr component = this.Owner.GetComponent<FFBipBindMgr>();
        if (component == null)
        {
            this.CurrWeaponobj.SetActive(false);
            return;
        }
        if (this.IsInFight)
        {
            this.CurrWeaponobj.transform.SetParent(component.GetBindPoint(this.CurrWeapon.FightBindPointName));
            this.CurrWeaponobj.transform.localPosition = this.CurrWeapon.FightPosition;
            this.CurrWeaponobj.transform.localScale = this.CurrWeapon.FightScale;
            this.CurrWeaponobj.transform.localEulerAngles = this.CurrWeapon.FightlRotation;
        }
        else
        {
            this.CurrWeaponobj.transform.SetParent(component.GetBindPoint(this.CurrWeapon.NormalBindPointName));
            this.CurrWeaponobj.transform.localPosition = this.CurrWeapon.NormalPosition;
            this.CurrWeaponobj.transform.localScale = this.CurrWeapon.NormalScale;
            this.CurrWeaponobj.transform.localEulerAngles = this.CurrWeapon.NormalRotation;
        }
        if (this.CurrWeaponobj.transform.parent == null)
        {
            FFDebug.LogError(this, "Weapon Get parent Error: " + this.CurrWeapon.WeaponName);
            this.ClearWeapon();
        }
        else
        {
            this.CurrWeaponobj.SetActive(true);
        }
    }

    private void ClearWeapon()
    {
        this.CurrWeaponid = 0U;
        FFMaterialEffectControl component = this.Owner.GetComponent<FFMaterialEffectControl>();
        if (component != null)
        {
            component.ClearWeaponMaterial();
        }
        this.WeaponObjMap.BetterForeach(delegate (KeyValuePair<string, FFWeaponHold.FFWeaponObj> item)
        {
            item.Value.Dispsose();
        });
        this.WeaponObjMap.Clear();
    }

    public void CompAwake(FFComponentMgr Mgr)
    {
        this.Owner = Mgr.Owner;
        if (this.Owner is OtherPlayer)
        {
            OtherPlayer otherPlayer = this.Owner as OtherPlayer;
            if (otherPlayer.OtherPlayerData.MapUserData != null)
            {
                this.ChangeWeapon(otherPlayer.OtherPlayerData.MapUserData.mapshow.weapon);
            }
        }
    }

    public void CompUpdate()
    {
    }

    public void CompDispose()
    {
        this.ClearWeapon();
    }

    public void ResetComp()
    {
    }

    public BetterDictionary<string, FFWeaponHold.FFWeaponObj> WeaponObjMap = new BetterDictionary<string, FFWeaponHold.FFWeaponObj>();

    private uint CurrWeaponid;

    private List<GameObject> TmpList = new List<GameObject>();

    public bool IsInFight;

    private List<FFeffect> FFeffectList = new List<FFeffect>();

    private WeaponArtConfig CurrWeapon;

    private GameObject CurrWeaponobj;

    public CharactorBase Owner;

    public class FFWeaponObj
    {
        public FFWeaponObj(WeaponArtConfig config, FFWeaponHold Hold)
        {
            this.CurrWeapon = config;
            this.FWHold = Hold;
        }

        public void Init()
        {
            this.LoadWeapon();
        }

        private WeaponResourcesMgr mWeaponResourcesMgr
        {
            get
            {
                return ManagerCenter.Instance.GetManager<WeaponResourcesMgr>();
            }
        }

        private void LoadWeapon()
        {
            if (this.CurrWeapon == null)
            {
                this.HadLoadOver = true;
                this.ClearWeapon();
                return;
            }
            this.mWeaponResourcesMgr.LoadWeaponobj(this.CurrWeapon.WeaponModel, delegate
            {
                ObjectPool<ModleObjInPool> weaponobj = this.mWeaponResourcesMgr.GetWeaponobj(this.CurrWeapon.WeaponModel);
                if (weaponobj != null)
                {
                    weaponobj.GetItemFromPool(delegate (ModleObjInPool OIP)
                    {
                        this.ObjInPool = OIP;
                        this.CurrWeaponobj = this.ObjInPool.ItemObj;
                        this.SetWeaponPos();
                        this.HadLoadOver = true;
                        this.FWHold.OnWeaponModleOver(this.CurrWeaponobj);
                    });
                }
                else
                {
                    this.HadLoadOver = true;
                    this.ClearWeapon();
                }
            });
        }

        private void SetWeaponPos()
        {
            if (this.CurrWeaponobj == null)
            {
                return;
            }
            if (this.CurrWeapon == null)
            {
                return;
            }
            FFBipBindMgr component = this.FWHold.Owner.GetComponent<FFBipBindMgr>();
            if (component == null)
            {
                this.CurrWeaponobj.SetActive(false);
                return;
            }
            this.CurrWeaponobj.transform.SetParent(component.GetBindPoint(this.CurrWeapon.NormalBindPointName));
            this.CurrWeaponobj.transform.localPosition = this.CurrWeapon.NormalPosition;
            this.CurrWeaponobj.transform.localScale = this.CurrWeapon.NormalScale;
            this.CurrWeaponobj.transform.localEulerAngles = this.CurrWeapon.NormalRotation;
            if (this.CurrWeaponobj.transform.parent == null)
            {
                FFDebug.LogError(this, "Weapon Get parent Error: " + this.CurrWeapon.WeaponName);
                this.ClearWeapon();
            }
            else
            {
                this.CurrWeaponobj.SetActive(true);
            }
        }

        private void ClearWeapon()
        {
            if (this.ObjInPool != null)
            {
                this.ObjInPool.DisableAndBackToPool(true);
            }
            if (null != this.CurrWeapon && this.FWHold.WeaponObjMap.ContainsKey(this.CurrWeapon.WeaponName))
            {
                this.FWHold.WeaponObjMap.Remove(this.CurrWeapon.WeaponName);
            }
            this.CurrWeapon = null;
        }

        public void Dispsose()
        {
            if (this.ObjInPool != null)
            {
                this.ObjInPool.DisableAndBackToPool(true);
            }
            this.CurrWeapon = null;
        }

        public WeaponArtConfig CurrWeapon;

        public GameObject CurrWeaponobj;

        public ModleObjInPool ObjInPool;

        private FFWeaponHold FWHold;

        public bool HadLoadOver;
    }
}
