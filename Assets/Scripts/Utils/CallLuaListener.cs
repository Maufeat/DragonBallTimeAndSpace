using System;
using System.Collections.Generic;
using LuaInterface;

public class CallLuaListener
{
    public static void SendLuaEvent(string ListenerName, bool Once, params object[] args)
    {
        if (string.IsNullOrEmpty(ListenerName))
        {
            return;
        }
        LuaTable luaTable = Util.GetLuaTable(ListenerName);
        if (luaTable != null)
        {
            if (args.Length == 0)
            {
                CallLuaListener.CallLuaFunction(ListenerName + ".CallListener", Once, new object[]
                {
                    luaTable
                });
            }
            else
            {
                Array.Resize<object>(ref args, args.Length + 1);
                for (int i = args.Length - 1; i >= 0; i--)
                {
                    if (i == 0)
                    {
                        args[i] = luaTable;
                    }
                    else
                    {
                        args[i] = args[i - 1];
                    }
                }
                if ((bool)CallLuaListener.CallLuaFunction(ListenerName + ".HasListener", Once, new object[]
                {
                    luaTable
                }))
                {
                    CallLuaListener.CallLuaFunction(ListenerName + ".CallListener", Once, args);
                }
            }
        }
    }

    private static object CallLuaFunction(string functionname, bool Once, params object[] args)
    {
        try
        {
            object[] array;
            if (CallLuaListener.luaFunctionMap.ContainsKey(functionname))
            {
                array = CallLuaListener.luaFunctionMap[functionname].Call(args);
            }
            else if (Once)
            {
                array = LuaScriptMgr.Instance.CallLuaFunction(functionname, args);
            }
            else
            {
                LuaFunction luaFunction = LuaScriptMgr.Instance.GetLuaFunction(functionname);
                CallLuaListener.luaFunctionMap.Add(functionname, luaFunction);
                array = luaFunction.Call(args);
            }
            if (array != null && array.Length > 0)
            {
                return array[0];
            }
        }
        catch (Exception arg)
        {
            FFDebug.LogError("CallLuaFunction", "error: " + arg);
        }
        return false;
    }

    public void ClearCacheLuaFunction()
    {
        CallLuaListener.luaFunctionMap.BetterForeach(delegate (KeyValuePair<string, LuaFunction> item)
        {
            item.Value.Release();
        });
        CallLuaListener.luaFunctionMap.Clear();
    }

    private static BetterDictionary<string, LuaFunction> luaFunctionMap = new BetterDictionary<string, LuaFunction>();
}
