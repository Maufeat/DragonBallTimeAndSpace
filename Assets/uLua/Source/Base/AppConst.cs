using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class AppConst {
    public const bool UsePbc = true;                           //PBC
    public const bool UseLpeg = true;                          //LPEG
    public const bool UsePbLua = false;                        //Protobuff-lua-gen
    public const bool UseCJson = false;                        //CJson
    public const bool UseSproto = true;                        //Sproto
    public const bool AutoWrapMode = true;                     //自动Wrap模式 

    public const bool DisableEncryption = true;
    public const bool EnablePacketDumping = false;
    public const bool UseAssetBundle = false;

    public static string uLuaPath
    {
        get
        {
            return LoadHelper.GetPath("uLua/", true);
        }
    }

    public static string dataPath
    {
        get
        {
            return LoadHelper.GetPath("/", false);
        }
    }

#if UNITY_EDITOR
    public static string uLuaInternalPath
    {
        get
        {
            return Application.dataPath + "/uLua/";
        }
    }
#endif

    public static string uLuaScriptPath
    {
        get
        {
            return LoadHelper.GetPath("Lua/uLua/", false);
        }
    }

    public static string GameScriptPath
    {
        get
        {
            return LoadHelper.GetPath("Lua/", false);
        }
    }
}
