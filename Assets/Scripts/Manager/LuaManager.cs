using System;
using System.Collections.Generic;
using Framework.Base;
using Framework.Managers;

public class LuaManager : IManager
{
    public static LuaManager GetInstance()
    {
        if (LuaManager._instance == null)
        {
            LuaManager._instance = ManagerCenter.Instance.GetManager<LuaManager>();
        }
        return LuaManager._instance;
    }

    public string ManagerName
    {
        get
        {
            return "LuaManager";
        }
    }

    public void Init()
    {
        LuaProcess.RegisertFunction(new Action<List<VarType>>(this.Luafun_CallLuaFunc));
        this.luaMgr = new LuaScriptMgr();
        this.luaMgr.Start();
    }

    public void Luafun_CallLuaFunc(List<VarType> paras)
    {
        if (paras == null || paras.Count < 1)
        {
            FFDebug.LogWarning(this, "Luafun_CallLuaFunc require at least one params !!!");
            return;
        }
        switch (paras.Count)
        {
            case 1:
                LuaScriptMgr.Instance.CallLuaFunction(paras[0], new object[0]);
                break;
            case 2:
                LuaScriptMgr.Instance.CallLuaFunction(paras[0], new object[]
                {
                paras[1].ToString()
                });
                break;
            case 3:
                LuaScriptMgr.Instance.CallLuaFunction(paras[0], new object[]
                {
                paras[1].ToString(),
                paras[2].ToString()
                });
                break;
            case 4:
                LuaScriptMgr.Instance.CallLuaFunction(paras[0], new object[]
                {
                paras[1].ToString(),
                paras[2].ToString(),
                paras[3].ToString()
                });
                break;
            case 5:
                LuaScriptMgr.Instance.CallLuaFunction(paras[0], new object[]
                {
                paras[1].ToString(),
                paras[2].ToString(),
                paras[3].ToString(),
                paras[4].ToString()
                });
                break;
            default:
                FFDebug.LogWarning(this, "Luafun_CallLuaFunc require at least one params !!!");
                break;
        }
    }

    public void Destroy()
    {
        this.luaMgr.Destroy();
    }

    public void OnUpdate()
    {
    }

    public void OnReSet()
    {
        LuaProcess.RegisertFunction(new Action<List<VarType>>(this.Luafun_CallLuaFunc));
    }

    private LuaScriptMgr luaMgr;

    private static LuaManager _instance;
}
