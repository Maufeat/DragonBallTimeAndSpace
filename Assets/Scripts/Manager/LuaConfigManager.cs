using System;
using System.Collections;
using System.Collections.Generic;
using LuaInterface;

public class LuaConfigManager
{
    public static void StartLoadLuaConfig(Action callback)
    {
        LuaConfigManager.onComplete = callback;
        LuaScriptMgr.Instance.CallLuaFunction("ConfigManager.Init", new object[]
        {
            Util.GetLuaTable("ConfigManager")
        });
    }

    public static void LoadLuaConfigComplete()
    {
        if (LuaConfigManager.onComplete != null)
        {
            LuaConfigManager.onComplete();
        }
    }

    public static LuaTable GetConfig(string cfgname)
    {
        if (!LuaConfigManager.dic_ExcelConfig.ContainsKey(cfgname))
        {
            LuaTable luaTable = Util.GetLuaTable("ConfigManager.ConfigData." + cfgname);
            LuaConfigManager.dic_ExcelConfig.Add(cfgname, luaTable);
        }
        return LuaConfigManager.dic_ExcelConfig[cfgname];
    }

    public static void ForceLoadExcelConfig(string cfgname)
    {
        LuaConfigManager.dic_ExcelConfig[cfgname] = Util.GetLuaTable("ConfigManager.ConfigData." + cfgname);
    }

    public static LuaTable GetConfigTable(string cfgname, ulong id)
    {
        LuaTable config = LuaConfigManager.GetConfig(cfgname);
        if (config != null)
        {
            return config.GetCacheField_Table(id);
        }
        return null;
    }

    public static List<LuaTable> GetConfigTableList(string cfgname)
    {
        List<LuaTable> result;
        try
        {
            LuaTable config = LuaConfigManager.GetConfig(cfgname);
            if (config != null)
            {
                List<LuaTable> list = new List<LuaTable>();
                IEnumerator enumerator = config.Values.GetEnumerator();
                enumerator.Reset();
                while (enumerator.MoveNext())
                {
                    object obj = enumerator.Current;
                    list.Add(obj as LuaTable);
                }
                result = list;
            }
            else
            {
                result = null;
            }
        }
        catch (Exception ex)
        {
            FFDebug.LogError("GetConfigTableList:" + cfgname, ex.Message);
            result = null;
        }
        return result;
    }

    public static List<LuaTable> GetConfigTableList(LuaTable ltb, string cfgname = "")
    {
        if (ltb != null)
        {
            List<LuaTable> list = new List<LuaTable>();
            IEnumerator enumerator2;
            if (cfgname == string.Empty)
            {
                IEnumerator enumerator = ltb.Values.GetEnumerator();
                enumerator2 = enumerator;
            }
            else
            {
                enumerator2 = ltb.GetField_Table(cfgname).Values.GetEnumerator();
            }
            IEnumerator enumerator3 = enumerator2;
            enumerator3.Reset();
            while (enumerator3.MoveNext())
            {
                object obj = enumerator3.Current;
                list.Add(obj as LuaTable);
            }
            list.Sort(delegate (LuaTable x, LuaTable y)
            {
                int field_Int = x.GetField_Int("id");
                int field_Int2 = y.GetField_Int("id");
                if (field_Int > field_Int2)
                {
                    return 1;
                }
                if (field_Int == field_Int2)
                {
                    return 0;
                }
                return -1;
            });
            return list;
        }
        return null;
    }

    public static LuaTable GetXmlConfigTable(string cfgname)
    {
        if (!LuaConfigManager.dic_XmlConfig.ContainsKey(cfgname))
        {
            LuaTable luaTable = Util.GetLuaTable("XmlConfigManager." + cfgname);
            LuaConfigManager.dic_XmlConfig.Add(cfgname, luaTable);
        }
        return LuaConfigManager.dic_XmlConfig[cfgname];
    }

    private static Action onComplete = null;

    private static BetterDictionary<string, LuaTable> dic_ExcelConfig = new BetterDictionary<string, LuaTable>();

    private static BetterDictionary<string, LuaTable> dic_XmlConfig = new BetterDictionary<string, LuaTable>();
}
