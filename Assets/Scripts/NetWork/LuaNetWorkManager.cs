using System;
using System.Collections.Generic;
using System.Text;
using Framework.Base;
using Framework.Managers;
using LuaInterface;
using Net;

public class LuaNetWorkManager : NetWorkBase, IManager
{
    public static LuaNetWorkManager Instance
    {
        get
        {
            return ManagerCenter.Instance.GetManager<LuaNetWorkManager>();
        }
    }

    public string ManagerName
    {
        get
        {
            return base.GetType().ToString();
        }
    }

    public override void Initialize()
    {
        base.Initialize();
    }

    public void ResetCache()
    {
    }

    public void RegisterMsg(ushort cmid, LuaFunction Callback)
    {
        base.RegisterMsg();
        if (!this.MsgLuaFunctions.ContainsKey(cmid))
        {
            this.MsgLuaFunctions[cmid] = Callback;
        }
    }

    public void UnRegisterMsg(ushort cmid, LuaFunction Callback)
    {
        if (this.MsgLuaFunctions.ContainsKey(cmid))
        {
            this.MsgLuaFunctions.Remove(cmid);
        }
    }

    public bool IsRegisterInLua(ushort msgid)
    {
        return this.MsgLuaFunctions.ContainsKey(msgid);
    }

    public void OnMessage(NullCmd cmd)
    {
        this.MsgLuaFunctions.RemoveAll((KeyValuePair<ushort, LuaFunction> x) => x.Value == null);
        LuaFunction luaFunction;
        if (this.MsgLuaFunctions.TryGetValue(cmd.Msgid, out luaFunction) && luaFunction != null)
        {
            string @string = Encoding.UTF8.GetString(cmd.BufferData);
            luaFunction.Call(new object[]
            {
                cmd.Msgid,
                new LuaStringBuffer(cmd.BufferData)
            });
        }
    }

    public void SendMsg(ushort cmid, LuaStringBuffer stringBuffer)
    {
        base.SendMsg(cmid, stringBuffer.buffer);
    }

    public override void Uninitialize()
    {
        this.MsgLuaFunctions.Clear();
        base.Uninitialize();
    }

    public void OnUpdate()
    {
    }

    public void OnReSet()
    {
    }

    private Dictionary<ushort, LuaFunction> MsgLuaFunctions = new Dictionary<ushort, LuaFunction>();
}
