using System;
using Models;

public class PreLoading : ControllerBase
{
    public override string ControllerName
    {
        get
        {
            return base.GetType().ToString();
        }
    }

    public override void Awake()
    {
    }

    public override void OnUpdate()
    {
    }

    public void InitLuaManager()
    {
        LuaScriptMgr instance = LuaScriptMgr.Instance;
        if (instance == null)
        {
            FFDebug.LogError(this, "Get LuaScriptMgr is null!");
            return;
        }
        instance.Start();
        instance.DoFile("Logic/GameManager");
        object[] array = instance.CallLuaFunction("GameManager.GetManagedPanel", new object[0]);
        if (array != null && array.Length > 0)
        {
            for (int i = 0; i < array.Length; i++)
            {
                string str = array[i].ToString();
                string str2 = str + ".lua";
                instance.DoFile("View/" + str2);
            }
        }
        instance.CallLuaFunction("GameManager.InitOK", new object[0]);
    }
}
