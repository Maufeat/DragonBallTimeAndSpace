using System;
using System.Collections.Generic;
using Framework.Managers;
using Models;

public class YuanQiDanController : ControllerBase
{
    private UI_YuanQiDan UIYuanQiDan
    {
        get
        {
            return UIManager.GetUIObject<UI_YuanQiDan>();
        }
    }

    public override void Awake()
    {
        this.mNetwork = new YuanQiDanNetwork();
        this.mNetwork.Initialize();
        LuaProcess.RegisertFunction(new Action<List<VarType>>(this.OpenYuanQiDanPanel));
    }

    public void OpenYuanQiDanPanel(List<VarType> paras)
    {
        if (paras.Count == 0)
        {
            return;
        }
        this.SetMoveState(false);
        UIManager.Instance.ShowUI<UI_YuanQiDan>("UI_QTE_yqd", delegate ()
        {
            this.UIYuanQiDan.Start(paras[0]);
        }, UIManager.ParentType.CommonUI, false);
    }

    public void CloseUI()
    {
        ManagerCenter.Instance.GetManager<UIManager>().DeleteUI("UI_QTE_yqd");
    }

    private uint GetTaskType(int taskid)
    {
        uint result = 1U;
        if (taskid == 10318)
        {
            result = 2U;
        }
        return result;
    }

    public void CommitYQDData_CS(int step, bool succ, int taskid)
    {
        this.mNetwork.CommitYQDData_CS(step, succ, this.GetTaskType(taskid));
    }

    public void Req_PlayYQDRetry_CS(int taskid)
    {
        this.mNetwork.Req_PlayYQDRetry_CS(this.GetTaskType(taskid));
    }

    public void ReqGameExit()
    {
        this.mNetwork.ReqGameExit();
        this.SetMoveState(true);
    }

    public void SetMoveState(bool canMove)
    {
        SingletonForMono<InputController>.Instance.mScreenEventController.ActiveUpdateByMouse(canMove);
        GameSystemSettings.SetMouseClickMove(canMove);
        ControllerManager.Instance.GetController<ShortcutsConfigController>().ShortcutSwitch(canMove);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
    }

    public override string ControllerName
    {
        get
        {
            return "yuanqidan_controller";
        }
    }

    private YuanQiDanNetwork mNetwork;
}
