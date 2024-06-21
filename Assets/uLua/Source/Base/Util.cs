using UnityEngine;
using System;
using System.Text;
using System.Collections;
using System.Reflection;
using System.IO;
using LuaInterface;


#if UNITY_EDITOR
using UnityEditor;
#endif

public class Util {

    /// <summary>
    /// 取得Lua路径
    /// </summary>
    public static string LuaPath(string name)
    {
        string text = name.ToLower();
        if (text.EndsWith(".lua"))
        {
            int length = name.LastIndexOf('.');
            name = name.Substring(0, length);
        }
        name = name.Replace('.', '/');
        return Util.getScriptPath(name + ".lua");
    }

    public static string getDataPath(string name)
    {
        return LoadHelper.GetPath(name, false);
    }

    private static string getScriptPath(string name)
    {
        string text = string.Empty;
        for (int i = 0; i < Util.rootPaths.Length; i++)
        {
            text = LoadHelper.GetPath(Util.rootPaths[i] + name, false);
            if (File.Exists(text))
            {
                break;
            }
        }
        return text;
    }

    public static void Log(string str) {
        Debug.Log(str);
    }

    public static void LogWarning(string str) {
        Debug.LogWarning(str);
    }

    public static void LogError(string str) {
        Debug.LogError(str);
    }

    /// <summary>
    /// 清理内存
    /// </summary>
    public static void ClearMemory() {
        GC.Collect();
        Resources.UnloadUnusedAssets();
        LuaScriptMgr mgr = LuaScriptMgr.Instance;
        if (mgr != null && mgr.lua != null) mgr.LuaGC();
    }

    /// <summary>
    /// 防止初学者不按步骤来操作
    /// </summary>
    /// <returns></returns>
    static int CheckRuntimeFile() {
        if (!Application.isEditor) return 0;
        string sourceDir = AppConst.uLuaPath + "/Source/LuaWrap/";
        if (!Directory.Exists(sourceDir)) {
            return -2;
        } else {
            string[] files = Directory.GetFiles(sourceDir);
            if (files.Length == 0) return -2;
        }
        return 0;
    }

    /// <summary>
    /// 检查运行环境
    /// </summary>
    public static bool CheckEnvironment() {
        return true;
    }
    /// <summary>
    /// 是不是苹果平台
    /// </summary>
    /// <returns></returns>
    public static bool isApplePlatform {
        get {
            return Application.platform == RuntimePlatform.IPhonePlayer ||
                   Application.platform == RuntimePlatform.OSXEditor ||
                   Application.platform == RuntimePlatform.OSXPlayer;
        }
    }


    public static LuaTable GetLuaTable(string tableName)
    {
        if (LuaScriptMgr.Instance.lua == null)
        {
            FFDebug.LogError(string.Empty, "LuaScriptMgr.Instance.lua == null");
        }
        return LuaScriptMgr.Instance.lua[tableName] as LuaTable;
    }

    public static LuaTable GetCacheLuaTable(string tableName)
    {
        return LuaScriptMgr.Instance.GetLuaTable(tableName);
    }

    public static string LuaOppositePath(string name)
    {
        for (int i = 0; i < Util.rootPaths.Length; i++)
        {
            string path = LoadHelper.GetPath(Util.rootPaths[i] + name, false);
            if (File.Exists(path))
            {
                return Util.rootPaths[i] + name;
            }
        }
        return string.Empty;
    }

    public static string[] rootPaths = new string[]
    {
        "Lua/",
        "Lua/uLua/"
    };
}