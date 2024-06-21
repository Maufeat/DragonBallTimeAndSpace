using System;
using LuaInterface;
using ResoureManager;
using UnityEngine;

public class CommonUtil
{
    public static string GetResouresPath(string name, ResouresType type)
    {
        string result = string.Empty;
        if (type == ResouresType.UI)
        {
            result = string.Format("{0}/{1}", "UI", name);
        }
        return result;
    }

    public static string GetText(uint textid)
    {
        LuaTable configTable = LuaConfigManager.GetConfigTable("dynamictext", (ulong)textid);
        if (configTable != null)
        {
            return configTable.GetField_String("content");
        }
        FFDebug.LogWarning("GetText", "can't GetText :" + textid);
        return textid.ToString();
    }

    public static string GetText(dynamic_textid.BaseIDs textid)
    {
        return CommonUtil.GetText((uint)textid);
    }

    public static string GetText(dynamic_textid.ServerIDs textid)
    {
        return CommonUtil.GetText((uint)textid);
    }

    public static string GetText(dynamic_textid.IDs textid)
    {
        return CommonUtil.GetText((uint)textid);
    }

    public static void SetActive(GameObject _go, bool _active)
    {
        if (_go != null)
        {
            _go.SetActive(_active);
        }
    }

    public static void SetTransfromScale(GameObject go, float scale)
    {
        if (go == null)
        {
            return;
        }
        go.transform.localScale = Vector3.one * scale;
    }
}
