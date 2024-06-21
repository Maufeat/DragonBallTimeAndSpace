using System;
using copymap;
using Framework.Managers;
using msg;
using Net;

public class CopyNetWork : NetWorkBase
{
    public CopyManager MCopyManager
    {
        get
        {
            return ManagerCenter.Instance.GetManager<CopyManager>();
        }
    }

    public override void Initialize()
    {
        this.RegisterMsg();
    }

    public override void RegisterMsg()
    {
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_Ret_CopymapBossTempID_SC>(2132, new ProtoMsgCallback<MSG_Ret_CopymapBossTempID_SC>(this.OnRetCopymapBossTempID_SC));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_Ret_Find_Path_SC>(2314, new ProtoMsgCallback<MSG_Ret_Find_Path_SC>(this.On_MSG_Ret_Find_Path_SC));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_Ret_Rondom_Way_SC>(2315, new ProtoMsgCallback<MSG_Ret_Rondom_Way_SC>(this.On_MSG_Ret_Rondom_Way_SC));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_Ret_Find_Path_End_SC>(2316, new ProtoMsgCallback<MSG_Ret_Find_Path_End_SC>(this.On_MSG_Ret_Find_Path_End_SC));
    }

    public void On_MSG_Ret_Find_Path_SC(MSG_Ret_Find_Path_SC msg)
    {
        CopyWayCheckContoller controller = ControllerManager.Instance.GetController<CopyWayCheckContoller>();
        if (controller != null)
        {
            controller.OnCheckWayPointValid(msg);
        }
    }

    public void On_MSG_Ret_Rondom_Way_SC(MSG_Ret_Rondom_Way_SC msg)
    {
        CopyWayCheckContoller controller = ControllerManager.Instance.GetController<CopyWayCheckContoller>();
        if (controller != null)
        {
            controller.OnGetWayGrid(msg);
        }
    }

    public void ReqMSG_Show_Path_Way_End_CS()
    {
        MSG_Show_Path_Way_End_CS t = new MSG_Show_Path_Way_End_CS();
        base.SendMsg<MSG_Show_Path_Way_End_CS>(CommandID.MSG_Show_Path_Way_End_CS, t, false);
    }

    public void On_MSG_Ret_Find_Path_End_SC(MSG_Ret_Find_Path_End_SC msg)
    {
        CopyWayCheckContoller controller = ControllerManager.Instance.GetController<CopyWayCheckContoller>();
        if (controller != null)
        {
            controller.On_MSG_Ret_Find_Path_End_SC(msg);
        }
    }

    private void OnRetCopymapBossTempID_SC(MSG_Ret_CopymapBossTempID_SC mdata)
    {
        this.MCopyManager.SetBossTempid(mdata.tempid);
    }

    public void ReqCopymapLottery()
    {
    }

    public void OnCopymapOver(MSG_Ret_CopymapOver_SC mdata)
    {
        FFDebug.Log(this, FFLogType.Copy, "OnCopymapOver");
        this.MCopyManager.ShowCompleteCopyViewOver(mdata);
    }
}
