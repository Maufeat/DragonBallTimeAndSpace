using System;
using System.Collections.Generic;
using Framework.Managers;
using LuaInterface;

public class KeyWordFilter
{
    public static void InitFilter()
    {
        if (KeyWordFilter.filter == null)
        {
            KeyWordFilter.filter = new TrieFilter();
            KeyWordFilter.filter.Mask = '*';
            List<LuaTable> configTableList = LuaConfigManager.GetConfigTableList("illegalword");
            for (int i = 0; i < configTableList.Count; i++)
            {
                KeyWordFilter.filter.AddKey(configTableList[i].GetField_String("word"));
            }
        }
        if (KeyWordFilter.chatFilter == null)
        {
            KeyWordFilter.chatFilter = new TrieFilter();
            KeyWordFilter.chatFilter.Mask = '*';
            List<LuaTable> configTableList2 = LuaConfigManager.GetConfigTableList("illegalword");
            for (int j = 0; j < configTableList2.Count; j++)
            {
                LuaTable luaTable = configTableList2[j];
                if (luaTable.GetField_Uint("display") == 0U)
                {
                    KeyWordFilter.chatFilter.AddKey(luaTable.GetField_String("word"));
                }
            }
        }
    }

    public static string TextFilter(string str)
    {
        if (KeyWordFilter.filter == null)
        {
            return string.Empty;
        }
        return KeyWordFilter.filter.Replace(str);
    }

    public static string ChatFilter(string str)
    {
        if (KeyWordFilter.chatFilter == null)
        {
            return string.Empty;
        }
        return KeyWordFilter.chatFilter.Replace(str);
    }

    private static TrieFilter filter;

    private static TrieFilter chatFilter;
}
