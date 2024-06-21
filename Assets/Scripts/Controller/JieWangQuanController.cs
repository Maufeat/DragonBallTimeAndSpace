using System;
using System.Collections.Generic;
using Framework.Managers;
using Models;

public class JieWangQuanController : ControllerBase
{
    private UI_JieWangQuan UIJieWangQuan
    {
        get
        {
            return UIManager.GetUIObject<UI_JieWangQuan>();
        }
    }

    public override void Awake()
    {
        this.mNetwork = new JieWangQuanNetworker();
        this.mNetwork.Initialize();
        LuaProcess.RegisertFunction(new Action<List<VarType>>(this.OpenJieWangQuanPanel));
        LuaProcess.RegisertFunction(new Action<List<VarType>>(this.StartJieWangQuanGame));
    }

    public void OpenJieWangQuanPanel(List<VarType> paras)
    {
        if (paras.Count == 0)
        {
            return;
        }
        this.SetMoveState(false);
        UIManager.Instance.ShowUI<UI_JieWangQuan>("UI_QTE_jwq", delegate ()
        {
            this.UIJieWangQuan.Start(paras[0]);
        }, UIManager.ParentType.CommonUI, false);
    }

    private void StartJieWangQuanGame(List<VarType> paras)
    {
        this.UIJieWangQuan.StartGame();
    }

    private uint GetTaskType(int taskid)
    {
        uint result = 1U;
        if (taskid == 10313)
        {
            result = 2U;
        }
        return result;
    }

    public void ReqOperateData(uint step, bool succ, int taskid)
    {
        this.mNetwork.ReqOperateData(step, succ, this.GetTaskType(taskid));
    }

    public void ReqGameRetry(int taskid)
    {
        this.mNetwork.ReqGameRetry(this.GetTaskType(taskid));
    }

    public void ReqGameExit()
    {
        this.mNetwork.ReqGameExit();
        this.SetMoveState(true);
    }

    public void SetMoveState(bool canMove)
    {
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
            return "jiewangquan_controller";
        }
    }

    private JieWangQuanNetworker mNetwork;
}
