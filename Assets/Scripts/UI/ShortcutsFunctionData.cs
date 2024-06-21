using System;
using System.Collections;
using System.Collections.Generic;
using LuaInterface;
using UnityEngine;

public class ShortcutsFunctionData
{
    public ShortcutsFunctionData(ShortcutkeyFunctionType sType, Action call, Dictionary<string, DefaulSetKeyData> dskd, List<KeyCode> k = null)
    {
        this.sft = sType;
        this.callBack = call;
        this.keys = k;
        this.FillDefaultData(sType, dskd);
    }

    public string keyShowNameInSetting
    {
        get
        {
            return this.GetKeyName(true);
        }
    }

    public string keyShowNameInMainUI
    {
        get
        {
            return this.GetKeyName(false);
        }
    }

    public void FillDefaultData(ShortcutkeyFunctionType sfd, Dictionary<string, DefaulSetKeyData> dskdDic)
    {
        if (dskdDic != null)
        {
            DefaulSetKeyData defaulSetKeyData = null;
            if (dskdDic.TryGetValue(sfd.ToString(), out defaulSetKeyData))
            {
                this.functionName = defaulSetKeyData.cname;
                this.abt = defaulSetKeyData.tapType;
                this.ktt = defaulSetKeyData.ktt;
            }
        }
    }

    private string GetKeyName(bool isInSetting)
    {
        if (this.keys == null || this.keys.Count == 0)
        {
            return string.Empty;
        }
        string text = string.Empty;
        foreach (KeyCode keyCode in this.keys)
        {
            if (ShortcutsFunctionData.keyNameToShowDic.ContainsKey(keyCode.ToString()))
            {
                text = text + ShortcutsFunctionData.keyNameToShowDic[keyCode.ToString()] + "+";
            }
            else
            {
                text = text + keyCode.ToString() + "+";
            }
        }
        if (text.EndsWith("+"))
        {
            text = text.Substring(0, text.Length - 1);
        }
        return text;
    }

    private static Dictionary<string, string> keyNameToShowDic
    {
        get
        {
            if (ShortcutsFunctionData.keyNameToShowDic_ == null)
            {
                ShortcutsFunctionData.keyNameToShowDic_ = ShortcutsFunctionData.InitDefaultKeyToShowName();
            }
            return ShortcutsFunctionData.keyNameToShowDic_;
        }
    }

    private static Dictionary<string, string> InitDefaultKeyToShowName()
    {
        Dictionary<string, string> dictionary = new Dictionary<string, string>();
        LuaTable cacheField_Table = LuaConfigManager.GetXmlConfigTable("other").GetCacheField_Table("keycodenames");
        IEnumerator enumerator = cacheField_Table.Values.GetEnumerator();
        enumerator.Reset();
        while (enumerator.MoveNext())
        {
            object obj = enumerator.Current;
            LuaTable luaTable = obj as LuaTable;
            dictionary[luaTable.GetField_String("keycodename")] = luaTable.GetField_String("showname");
        }
        return dictionary;
    }

    public ShortcutkeyFunctionType sft;

    public Action callBack;

    public List<KeyCode> keys;

    public string functionName;

    public KeyTriggerType ktt;

    public KeyConfigTapType abt = KeyConfigTapType.None;

    private static Dictionary<string, string> keyNameToShowDic_;
}
