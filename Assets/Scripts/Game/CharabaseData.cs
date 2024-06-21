using System;
using LuaInterface;
using UnityEngine;

public class CharabaseData : IFFComponent
{
    public CompnentState State { get; set; }

    public void RefreshBaseData(cs_BaseData baseData)
    {
        this.BaseData = baseData;
        this.RefreshCharaBasePosition(baseData.pos.fx, baseData.pos.fy);
    }

    public void RefreshCharaBasePosition(float px, float py)
    {
        if (this.BaseData == null)
        {
            return;
        }
        this.BaseData.pos = new cs_FloatMovePos(px, py);
    }

    public void RefreshCharaBasePosition(Vector2 pos)
    {
        if (this.BaseData == null)
        {
            return;
        }
        this.BaseData.pos = new cs_FloatMovePos(pos.x, pos.y);
    }

    public virtual Vector3 GetModelSize()
    {
        this._modelsize = Vector3.one;
        try
        {
            LuaTable appearancenpcConfig = this.GetAppearancenpcConfig();
            if (appearancenpcConfig != null)
            {
                LuaTable cacheField_Table = appearancenpcConfig.GetCacheField_Table("PreWorkdata");
                if (cacheField_Table != null)
                {
                    this._modelsize.x = cacheField_Table.GetCacheField_Float("modelsize_x");
                    this._modelsize.y = cacheField_Table.GetCacheField_Float("modelsize_y");
                    this._modelsize.z = cacheField_Table.GetCacheField_Float("modelsize_z");
                }
            }
        }
        catch (Exception arg)
        {
            FFDebug.LogError(this, "SetModelSize :" + arg);
        }
        return this._modelsize;
    }

    public virtual string GetModelName()
    {
        LuaTable appearancenpcConfig = this.GetAppearancenpcConfig();
        if (appearancenpcConfig != null)
        {
            string cacheField_String = appearancenpcConfig.GetCacheField_String("model");
            if (!string.IsNullOrEmpty(cacheField_String))
            {
                return cacheField_String;
            }
        }
        return "none";
    }

    public virtual string GetACname()
    {
        LuaTable appearancenpcConfig = this.GetAppearancenpcConfig();
        if (appearancenpcConfig != null)
        {
            string field_String = appearancenpcConfig.GetField_String("animatorcontroller");
            if (!string.IsNullOrEmpty(field_String))
            {
                return field_String;
            }
        }
        return "none";
    }

    public virtual uint GetAppearanceid()
    {
        return 0U;
    }

    public virtual uint GetBaseOrHeroId()
    {
        return 0U;
    }

    public virtual LuaTable GetAppearancenpcConfig()
    {
        ulong num = (ulong)this.GetAppearanceid();
        if (num != 0UL)
        {
            LuaTable cacheField_Table = LuaConfigManager.GetConfig("npc_data").GetCacheField_Table(num);
            if (cacheField_Table != null)
            {
                return cacheField_Table;
            }
        }
        return null;
    }

    public virtual LuaTable GetCharabaseConfig()
    {
        uint baseOrHeroId = this.GetBaseOrHeroId();
        if (baseOrHeroId != 0U)
        {
            LuaTable cacheField_Table = LuaConfigManager.GetConfig("npc_data").GetCacheField_Table(baseOrHeroId);
            if (cacheField_Table != null)
            {
                return cacheField_Table;
            }
        }
        return null;
    }

    public virtual float GetCharaVolume()
    {
        if (this.mVolume > 0.1f)
        {
            return this.mVolume;
        }
        this.mVolume = 1.5f;
        LuaTable charabaseConfig = this.GetCharabaseConfig();
        if (charabaseConfig != null)
        {
            this.mVolume = (float)charabaseConfig.GetCacheField_Int("volume");
            this.mVolume -= 0.5f;
        }
        return this.mVolume;
    }

    public virtual void SetCharaVolume(float value)
    {
        this.mVolume = value;
    }

    public virtual void SetHp(uint hp)
    {
    }

    public void CompAwake(FFComponentMgr Mgr)
    {
        this.Owner = Mgr.Owner;
    }

    public void CompUpdate()
    {
    }

    public void CompDispose()
    {
    }

    public void ResetComp()
    {
    }

    public CharactorBase Owner;

    public cs_BaseData BaseData;

    private Vector3 _modelsize;

    private float mVolume;
}
