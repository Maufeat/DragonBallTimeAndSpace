using System;
using Framework.Managers;
using LuaInterface;

public class CopyGuide : ICopy
{
    public uint CopyID
    {
        get
        {
            return this.CurCopyID;
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
        return this.MCopyManager.currentCopyConfig != null && this.MCopyManager.currentCopyConfig.GetField_Uint("type") == 2U;
    }

    public void InitCopy()
    {
        this.MCopyConfig = this.MCopyManager.currentCopyConfig;
        this.CurCopyID = this.MCopyManager.MCurrentCopyID;
    }

    public void EnterCopy()
    {
        FFDebug.Log(this, FFLogType.Copy, "Enter Copy " + this);
        UI_MainView mainView = ControllerManager.Instance.GetController<MainUIController>().mainView;
        if (mainView != null)
        {
            mainView.CheckExitCopyBtn();
        }
        ControllerManager.Instance.GetController<MainUIController>().CopyGuideSpecialHandling(true);
    }

    public void ExitCopy()
    {
        ControllerManager.Instance.GetController<MainUIController>().CopyGuideSpecialHandling(false);
        FFDebug.Log(this, FFLogType.Copy, "Exit Copy " + this);
    }

    public void Update()
    {
    }

    public void OnComplete(float time)
    {
        this.ExitCopy();
    }

    public void OnOver(uint countdowm)
    {
        this.ExitCopy();
    }

    private uint CurCopyID;

    public LuaTable MCopyConfig;
}
