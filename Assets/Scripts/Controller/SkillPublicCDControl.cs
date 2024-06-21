using System;
using System.Collections;
using System.Collections.Generic;
using LuaInterface;

public class SkillPublicCDControl : IFFComponent
{
    private void InitConfigData()
    {
        LuaTable cacheField_Table = LuaConfigManager.GetXmlConfigTable("careerskill").GetCacheField_Table("publicCD");
        IEnumerator enumerator = cacheField_Table.Values.GetEnumerator();
        enumerator.Reset();
        while (enumerator.MoveNext())
        {
            object obj = enumerator.Current;
            LuaTable luaTable = obj as LuaTable;
            uint field_Uint = luaTable.GetField_Uint("ID");
            uint field_Uint2 = luaTable.GetField_Uint("value");
            PublicCDData value = new PublicCDData(field_Uint, field_Uint2);
            this.dicPublicCD[field_Uint] = value;
        }
    }

    public void ActiveAllPublicCD()
    {
        this.dicPublicCD.BetterForeach(delegate (KeyValuePair<uint, PublicCDData> pair)
        {
            pair.Value.ActivateCD(0U);
        });
    }

    public PublicCDData GetPublicCDDataByGroup(uint group)
    {
        if (this.dicPublicCD.ContainsKey(group))
        {
            return this.dicPublicCD[group];
        }
        return null;
    }

    public CompnentState State { get; set; }

    public void CompAwake(FFComponentMgr Mgr)
    {
        this.InitConfigData();
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

    private BetterDictionary<uint, PublicCDData> dicPublicCD = new BetterDictionary<uint, PublicCDData>();
}
