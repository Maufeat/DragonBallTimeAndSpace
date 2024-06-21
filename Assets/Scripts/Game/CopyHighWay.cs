using System;
using Framework.Managers;
using LuaInterface;

public class CopyHighWay : ICopy
{
    public uint CopyID
    {
        get
        {
            return 2U;
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
        return copymapid == this.CopyID && this.MCopyManager.currentCopyConfig.GetField_Uint("type") == 1U;
    }

    public void InitCopy()
    {
        this.MCopyConfig = LuaConfigManager.GetConfigTable("copymapmaster", (ulong)this.CopyID);
    }

    public void EnterCopy()
    {
        FFDebug.Log(this, FFLogType.Copy, "Enter Copy " + this);
        UI_MainView mainView = ControllerManager.Instance.GetController<MainUIController>().mainView;
        if (mainView != null)
        {
            mainView.CheckExitCopyBtn();
        }
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
