using System;
using System.Collections.Generic;
using Chat;
using LuaInterface;

public class ConvenientProcess
{
    public static void RegisertConvenientFunction(uint id, Action<List<VarType>> callback)
    {
        ConvenientProcess.funMap[id] = callback;
    }

    public static void RegisertConvenientFunctionLua(uint id, LuaFunction callback)
    {
        ConvenientProcess.lunFunMap[id] = callback;
    }

    public static void ProcessConvenient(ChatLink link)
    {
        if (link == null)
        {
            FFDebug.LogWarning("ConvenientProcess", "link == null");
            return;
        }
        ConvenientProcess.ProcessConvenient(link.linktype, link.data_args);
    }

    public static void ProcessConvenient(uint type, List<string> args)
    {
        if (!ConvenientProcess.funMap.ContainsKey(type) && !ConvenientProcess.lunFunMap.ContainsKey(type))
        {
            FFDebug.LogWarning("ConvenientProcess", "Dosent exit convenient id: " + type);
            return;
        }
        if (ConvenientProcess.lunFunMap.ContainsKey(type))
        {
            ConvenientProcess.lunFunMap[type].Call(new object[]
            {
                args
            });
            return;
        }
        List<VarType> list = new List<VarType>();
        for (int i = 0; i < args.Count; i++)
        {
            list.Add(new VarType(args[i]));
        }
        ConvenientProcess.funMap[type](list);
    }

    private static BetterDictionary<uint, Action<List<VarType>>> funMap = new BetterDictionary<uint, Action<List<VarType>>>();

    private static BetterDictionary<uint, LuaFunction> lunFunMap = new BetterDictionary<uint, LuaFunction>();
}
