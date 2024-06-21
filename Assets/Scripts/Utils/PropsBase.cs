using System;
using System.Collections.Generic;
using LuaInterface;
using Obj;

public class PropsBase
{
    public PropsBase()
    {
        this._obj = new t_Object();
    }

    public PropsBase(uint id, uint num)
    {
        this.config = LuaConfigManager.GetConfigTable("objects", (ulong)id);
        if (this.config == null)
        {
            return;
        }
        this._obj = new t_Object();
        this._obj.baseid = id;
        this._obj.bind = 0U;
        this._obj.num = num;
        this._obj.type = (ObjectType)this.config.GetField_Uint("type");
        this._obj.timer = 0U;
        this._obj.name = this.config.GetField_String("name");
        this._obj.quality = this.config.GetField_Uint("quality");
        this._obj.level = this.config.GetField_Uint("level");
        this._obj.nextusetime = 0U;
        this._obj.tradetime = 0U;
        this.RefrshFightValue();
        this.orderIndex = Convert.ToInt32(LuaScriptMgr.Instance.CallLuaFunction("BagCtrl.GetOrderByType", new object[]
        {
            Util.GetLuaTable("BagCtrl"),
            this.config.GetField_Uint("type")
        })[0]);
        this.ThisidStr = this._obj.thisid.ToString();
    }

    public PropsBase(t_Object obj)
    {
        this.config = LuaConfigManager.GetConfigTable("objects", (ulong)obj.baseid);
        this._obj = obj;
        this.RefrshFightValue();
        this.orderIndex = Convert.ToInt32(LuaScriptMgr.Instance.CallLuaFunction("BagCtrl.GetOrderByType", new object[]
        {
            Util.GetLuaTable("BagCtrl"),
            this.config.GetField_Uint("type")
        })[0]);
        this.ThisidStr = this._obj.thisid.ToString();
    }

    public PropsBase(LuaTable LuaTobj)
    {
        this._obj = new t_Object();
        if (LuaTobj == null)
        {
            return;
        }
        this._obj.baseid = LuaTobj.GetField_Uint("baseid");
        this.config = LuaConfigManager.GetConfigTable("objects", (ulong)this._obj.baseid);
        this._obj.thisid = LuaTobj.GetField_String("thisid");
        this._obj.bind = LuaTobj.GetField_Uint("bind");
        this._obj.num = LuaTobj.GetField_Uint("num");
        this._obj.type = (ObjectType)((int)Enum.Parse(typeof(ObjectType), LuaTobj.GetField_String("type")));
        this._obj.equiprand.Clear();
        this._obj.grid_x = LuaTobj.GetField_Uint("grid_x");
        this._obj.grid_y = LuaTobj.GetField_Uint("grid_y");
        this._obj.level = LuaTobj.GetField_Uint("level");
        this._obj.name = LuaTobj.GetField_String("name");
        this._obj.packtype = (PackType)((int)Enum.Parse(typeof(PackType), LuaTobj.GetField_String("packtype")));
        this._obj.timer = LuaTobj.GetField_Uint("timer");
        this._obj.quality = LuaTobj.GetField_Uint("quality");
        this._obj.nextusetime = 0U;
        this.RefrshFightValue();
        this.orderIndex = Convert.ToInt32(LuaScriptMgr.Instance.CallLuaFunction("BagCtrl.GetOrderByType", new object[]
        {
            Util.GetLuaTable("BagCtrl"),
            this.config.GetField_Uint("type")
        })[0]);
        this.ThisidStr = this._obj.thisid.ToString();
    }

    public bool IsNew
    {
        get
        {
            return this.isnew;
        }
    }

    public uint FightValue
    {
        get
        {
            return this.fightvalue;
        }
    }

    public uint Count
    {
        get
        {
            return this._obj.num;
        }
    }

    public void ResetData(t_Object obj)
    {
        this.config = LuaConfigManager.GetConfigTable("objects", (ulong)obj.baseid);
        this._obj.thisid = obj.thisid;
        this._obj.baseid = obj.baseid;
        this._obj.bind = obj.bind;
        this._obj.num = obj.num;
        this._obj.type = obj.type;
        this._obj.equipprop = obj.equipprop;
        this._obj.equiprand.Clear();
        this._obj.grid_x = obj.grid_x;
        this._obj.grid_y = obj.grid_y;
        this._obj.level = obj.level;
        this._obj.name = obj.name;
        this._obj.packtype = obj.packtype;
        this._obj.timer = obj.timer;
        this._obj.quality = obj.quality;
        this._obj.nextusetime = obj.nextusetime;
        this.RefrshFightValue();
        for (int i = 0; i < obj.equiprand.Count; i++)
        {
            this._obj.equiprand.Add(obj.equiprand[i]);
        }
        PropsAttributComparer comparer = new PropsAttributComparer();
        this._obj.equiprand.Sort(comparer);
        this.orderIndex = Convert.ToInt32(LuaScriptMgr.Instance.CallLuaFunction("BagCtrl.GetOrderByType", new object[]
        {
            Util.GetLuaTable("BagCtrl"),
            this.config.GetField_Uint("type")
        })[0]);
        this.ThisidStr = this._obj.thisid.ToString();
    }

    public void ResetData_Id(uint id)
    {
        this.config = LuaConfigManager.GetConfigTable("objects", (ulong)id);
        this.RefrshFightValue();
        this.orderIndex = Convert.ToInt32(LuaScriptMgr.Instance.CallLuaFunction("BagCtrl.GetOrderByType", new object[]
        {
            Util.GetLuaTable("BagCtrl"),
            this.config.GetField_Uint("type")
        })[0]);
        this.ThisidStr = this._obj.thisid.ToString();
    }

    private void RefrshFightValue()
    {
        EquipData equipprop = this._obj.equipprop;
        if (equipprop == null)
        {
            this.fightvalue = 0U;
            return;
        }
        LuaTable xmlConfigTable = LuaConfigManager.GetXmlConfigTable("FightCoefficient");
        this.fightvalue = equipprop.weaponatt * xmlConfigTable.GetCacheField_Table("weaponatt").GetCacheField_Uint("value") + equipprop.pdam * xmlConfigTable.GetCacheField_Table("pdam").GetCacheField_Uint("value") + equipprop.mdam * xmlConfigTable.GetCacheField_Table("mdam").GetCacheField_Uint("value") + equipprop.pdef * xmlConfigTable.GetCacheField_Table("pdef").GetCacheField_Uint("value") + equipprop.mdef * xmlConfigTable.GetCacheField_Table("mdef").GetCacheField_Uint("value") + equipprop.maxhp * xmlConfigTable.GetCacheField_Table("maxhp").GetCacheField_Uint("value") + equipprop.str * xmlConfigTable.GetCacheField_Table("str").GetCacheField_Uint("value") + equipprop.dex * xmlConfigTable.GetCacheField_Table("dex").GetCacheField_Uint("value") + equipprop.intel * xmlConfigTable.GetCacheField_Table("intel").GetCacheField_Uint("value") + equipprop.phy * xmlConfigTable.GetCacheField_Table("phy").GetCacheField_Uint("value") + equipprop.bang * xmlConfigTable.GetCacheField_Table("bang").GetCacheField_Uint("value") + equipprop.toughness * xmlConfigTable.GetCacheField_Table("toughness").GetCacheField_Uint("value") + equipprop.block * xmlConfigTable.GetCacheField_Table("block").GetCacheField_Uint("value") + equipprop.penetrate * xmlConfigTable.GetCacheField_Table("penerate").GetCacheField_Uint("value") + equipprop.accurate * xmlConfigTable.GetCacheField_Table("accurate").GetCacheField_Uint("value") + equipprop.hold * xmlConfigTable.GetCacheField_Table("hold").GetCacheField_Uint("value") + equipprop.deflect * xmlConfigTable.GetCacheField_Table("deflect").GetCacheField_Uint("value") + equipprop.bangextradamage * xmlConfigTable.GetCacheField_Table("bangextradamage").GetCacheField_Uint("value") + equipprop.toughnessreducedamage * xmlConfigTable.GetCacheField_Table("toughnessreducedamage").GetCacheField_Uint("value") + equipprop.blockreducedamage * xmlConfigTable.GetCacheField_Table("blockreducedamage").GetCacheField_Uint("value") + equipprop.penetrateextradamage * xmlConfigTable.GetCacheField_Table("penetrateextradamage").GetCacheField_Uint("value") + equipprop.accurateextradamage * xmlConfigTable.GetCacheField_Table("accurateextradamage").GetCacheField_Uint("value") + equipprop.holdreducedamage * xmlConfigTable.GetCacheField_Table("holdreducedamage").GetCacheField_Uint("value") + equipprop.deflectreducedamage * xmlConfigTable.GetCacheField_Table("deflectreducedamage").GetCacheField_Uint("value") + equipprop.maxmp * xmlConfigTable.GetCacheField_Table("maxmp").GetCacheField_Uint("value") + equipprop.miss * xmlConfigTable.GetCacheField_Table("miss").GetCacheField_Uint("value") + equipprop.hit * xmlConfigTable.GetCacheField_Table("hit").GetCacheField_Uint("value");
    }

    public void SetNewState(bool state)
    {
        this.isnew = state;
    }

    public void DeleteFromPackage()
    {
        switch (this._obj.packtype)
        {
            case PackType.PACK_TYPE_MAIN:
                if (this._obj.type == ObjectType.OBJTYPE_RES)
                {
                    LuaScriptMgr.Instance.CallLuaFunction("BagCtrl.ResourcePackageDicRemove", new object[]
                    {
                    Util.GetLuaTable("BagCtrl"),
                    this._obj.thisid
                    });
                    LuaScriptMgr.Instance.CallLuaFunction("BagCtrl.lisResourcePackageRemove", new object[]
                    {
                    Util.GetLuaTable("BagCtrl"),
                    this
                    });
                }
                else
                {
                    LuaScriptMgr.Instance.CallLuaFunction("BagCtrl.MainPackageDicRemove", new object[]
                    {
                    Util.GetLuaTable("BagCtrl"),
                    this._obj.thisid
                    });
                    LuaScriptMgr.Instance.CallLuaFunction("BagCtrl.lisMainPackageRemove", new object[]
                    {
                    Util.GetLuaTable("BagCtrl"),
                    this
                    });
                }
                break;
            case PackType.PACK_TYPE_EQUIP:
                LuaScriptMgr.Instance.CallLuaFunction("BagCtrl.EquipPackageDicRemove", new object[]
                {
                Util.GetLuaTable("BagCtrl"),
                this._obj.thisid
                });
                LuaScriptMgr.Instance.CallLuaFunction("BagCtrl.lisEquipPackageRemove", new object[]
                {
                Util.GetLuaTable("BagCtrl"),
                this
                });
                break;
            case PackType.PACK_TYPE_QUEST:
                LuaScriptMgr.Instance.CallLuaFunction("BagCtrl.TaskPackageDicRemove", new object[]
                {
                Util.GetLuaTable("BagCtrl"),
                this._obj.thisid
                });
                LuaScriptMgr.Instance.CallLuaFunction("BagCtrl.lisTaskPackageRemove", new object[]
                {
                Util.GetLuaTable("BagCtrl"),
                this
                });
                break;
        }
    }

    public void CopyData(PropsBase source)
    {
        if (source == null)
        {
            return;
        }
        this.config = source.config;
        this._obj.thisid = source._obj.thisid;
        this._obj.baseid = source._obj.baseid;
        this._obj.packtype = source._obj.packtype;
        this._obj.type = source._obj.type;
        this._obj.name = source._obj.name;
        this._obj.num = source._obj.num;
        this._obj.bind = source._obj.bind;
        this._obj.grid_x = source._obj.grid_x;
        this._obj.grid_y = source._obj.grid_y;
        this._obj.quality = source._obj.quality;
        this._obj.level = source._obj.level;
        this._obj.timer = source._obj.timer;
        this._obj.nextusetime = source._obj.nextusetime;
        this._obj.equipprop = source._obj.equipprop;
        this._obj.equiprand.Clear();
        for (int i = 0; i < source._obj.equiprand.Count; i++)
        {
            EquipRandInfo equipRandInfo = source._obj.equiprand[i];
            EquipRandInfo equipRandInfo2 = new EquipRandInfo();
            equipRandInfo2.id = equipRandInfo.id;
            equipRandInfo2.type = equipRandInfo.type;
            equipRandInfo2.value = equipRandInfo.value;
            this._obj.equiprand.Add(equipRandInfo2);
        }
        PropsAttributComparer comparer = new PropsAttributComparer();
        this._obj.equiprand.Sort(comparer);
        this.RefrshFightValue();
        this.orderIndex = Convert.ToInt32(LuaScriptMgr.Instance.CallLuaFunction("BagCtrl.GetOrderByType", new object[]
        {
            Util.GetLuaTable("BagCtrl"),
            this.config.GetField_Uint("type")
        })[0]);
        this.ThisidStr = this._obj.thisid.ToString();
    }

    public t_Object _obj;

    public LuaTable config;

    public int orderIndex = -1;

    private bool isnew = true;

    public string ThisidStr;

    private uint fightvalue;
}

public class PropsAttributComparer : IComparer<EquipRandInfo>
{
    public int Compare(EquipRandInfo a, EquipRandInfo b)
    {
        if (a.type < b.type)
        {
            return -1;
        }
        if (a.type > b.type)
        {
            return 1;
        }
        return 0;
    }
}
