using System;
using Framework.Managers;
using LuaInterface;

public class CopyDefault : ICopy
{
    public uint CopyID
    {
        get
        {
            return 0U;
        }
    }

    public CopyManager MCopyManager
    {
        get
        {
            return ManagerCenter.Instance.GetManager<CopyManager>();
        }
    }

    public bool Try(uint copymapid)
    {
        return true;
    }

    public void InitCopy()
    {
        this.MCopyConfig = LuaConfigManager.GetConfigTable("copymapmaster", (ulong)this.CopyID);
    }

    public void EnterCopy()
    {
        FFDebug.Log(this, FFLogType.Copy, "Enter Copy " + this);
        UI_MainView mainView = ControllerManager.Instance.GetController<MainUIController>().mainView;
        if (mainView == null)
        {
            return;
        }
        mainView.CheckExitCopyBtn();
    }

    public void ExitCopy()
    {
        FFDebug.Log(this, FFLogType.Copy, "Exit Copy " + this);
    }

    public void Update()
    {
    }

    public void OnComplete(float time)
    {
    }

    public void OnOver(uint countdowm)
    {
    }

    public LuaTable MCopyConfig;
}
