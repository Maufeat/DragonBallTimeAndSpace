using System;
using System.Collections.Generic;
using LuaInterface;

public class PackPropComparer : IComparer<PropsBase>
{
    public int Compare(PropsBase a, PropsBase b)
    {
        if (a.orderIndex == -1 && b.orderIndex != -1)
        {
            return 1;
        }
        if (a.orderIndex != -1 && b.orderIndex == -1)
        {
            return -1;
        }
        if (a.orderIndex != -1 && b.orderIndex != -1)
        {
            if (a.orderIndex < b.orderIndex)
            {
                return -1;
            }
            if (a.orderIndex > b.orderIndex)
            {
                return 1;
            }
            return this.CommonCompare(a, b);
        }
        else
        {
            if (a.config.GetField_Uint("type") < b.config.GetField_Uint("type"))
            {
                return -1;
            }
            if (a.config.GetField_Uint("type") > b.config.GetField_Uint("type"))
            {
                return 1;
            }
            return this.CommonCompare(a, b);
        }
    }

    private int CardCompare(PropsBase a, PropsBase b)
    {
        LuaTable configTable = LuaConfigManager.GetConfigTable("carddata_config", (ulong)a.config.GetField_Uint("id"));
        LuaTable configTable2 = LuaConfigManager.GetConfigTable("carddata_config", (ulong)b.config.GetField_Uint("id"));
        if (configTable.GetField_Uint("cardtype") != configTable2.GetField_Uint("cardtype"))
        {
            return configTable.GetField_Uint("cardtype").CompareTo(configTable2.GetField_Uint("cardtype"));
        }
        if (configTable.GetField_Uint("cardstar") != configTable2.GetField_Uint("cardstar"))
        {
            return configTable.GetField_Uint("cardstar").CompareTo(configTable2.GetField_Uint("cardstar"));
        }
        return 0;
    }

    private int CommonCompare(PropsBase a, PropsBase b)
    {
        if (a.config.GetField_Uint("type") == 53U)
        {
            int num = this.CardCompare(a, b);
            if (num != 0)
            {
                return num;
            }
        }
        if (a.config.GetField_Uint("quality") > b.config.GetField_Uint("quality"))
        {
            return -1;
        }
        if (a.config.GetField_Uint("quality") < b.config.GetField_Uint("quality"))
        {
            return 1;
        }
        if (a.config.GetField_Uint("id") < b.config.GetField_Uint("id"))
        {
            return -1;
        }
        if (a.config.GetField_Uint("id") > b.config.GetField_Uint("id"))
        {
            return 1;
        }
        if (a._obj.bind < b._obj.bind)
        {
            return -1;
        }
        if (a._obj.bind > b._obj.bind)
        {
            return 1;
        }
        if (a._obj.num > b._obj.num)
        {
            return -1;
        }
        if (a._obj.num < b._obj.num)
        {
            return 1;
        }
        if (ulong.Parse(a._obj.thisid) > ulong.Parse(b._obj.thisid))
        {
            return -1;
        }
        if (ulong.Parse(a._obj.thisid) < ulong.Parse(b._obj.thisid))
        {
            return 1;
        }
        return 0;
    }
}
