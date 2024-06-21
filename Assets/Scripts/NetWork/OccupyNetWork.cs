using System;
using Framework.Managers;
using Net;
using npc;

public class OccupyNetWork : NetWorkBase
{
    private OccupyController occupyController
    {
        get
        {
            return ControllerManager.Instance.GetController<OccupyController>();
        }
    }

    public override void Initialize()
    {
        this.RegisterMsg();
        this.OnHoldOnFinish = delegate (bool b)
        {
            if (MainPlayer.Self.GetComponent<AutoAttack>().AutoAttackOn)
            {
                MainPlayer.Self.StopMoveImmediate(null);
                MainPlayer.Self.Pfc.EndPathFind(PathFindState.Break, true);
                MainPlayer.Self.GetComponent<FFDetectionNpcControl>().CurrentVisteNpcID = 0UL;
                MainPlayer.Self.GetComponent<AutoAttack>().ThinkDelay(2f);
            }
            if (MainPlayer.Self.GetComponent<AttactFollowTeamLeader>().AutoAttackOn)
            {
                MainPlayer.Self.StopMoveImmediate(null);
                MainPlayer.Self.Pfc.EndPathFind(PathFindState.Break, true);
                MainPlayer.Self.GetComponent<FFDetectionNpcControl>().CurrentVisteNpcID = 0UL;
                MainPlayer.Self.GetComponent<AttactFollowTeamLeader>().ThinkDelay(2f);
            }
        };
    }

    public override void RegisterMsg()
    {
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_Ret_Holdon_SC>(2327, new ProtoMsgCallback<MSG_Ret_Holdon_SC>(this.OnReturnHoldOn));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_Ret_Holdon_Interrupt_SC>(2328, new ProtoMsgCallback<MSG_Ret_Holdon_Interrupt_SC>(this.OnReturnHoldOnInterupt));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_Ret_AddHoldNpcData_SC>(2329, new ProtoMsgCallback<MSG_Ret_AddHoldNpcData_SC>(this.OnRetAddHoldNpcData));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_Ret_BatchAddHoldNpcData_SC>(2330, new ProtoMsgCallback<MSG_Ret_BatchAddHoldNpcData_SC>(this.OnRetBatchAddHoldNpcData));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_Ret_RemoveHoldNpcData_SC>(2331, new ProtoMsgCallback<MSG_Ret_RemoveHoldNpcData_SC>(this.OnRemoveHoldNpcData));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_Ret_HoldonSuccess>(2332, new ProtoMsgCallback<MSG_Ret_HoldonSuccess>(this.OnReturnHoldonSuccessc));
    }

    public void ReqHoldon(ulong holdid, uint type, uint npcType)
    {
        ManagerCenter.Instance.GetManager<EntitiesManager>().PNetWork.ClearMoveCachesData();
        MSG_Req_Holdon_CS msg_Req_Holdon_CS = new MSG_Req_Holdon_CS();
        msg_Req_Holdon_CS.thisid = holdid;
        msg_Req_Holdon_CS.type = type;
        msg_Req_Holdon_CS.npctype = npcType;
        FFDebug.Log(this, FFLogType.HoldOn, string.Concat(new object[]
        {
            "Send ReqHoldon ",
            holdid,
            "  ",
            type,
            " ",
            npcType
        }));
        base.SendMsg<MSG_Req_Holdon_CS>(CommandID.MSG_Req_Holdon_CS, msg_Req_Holdon_CS, false);
    }

    private void OnReturnHoldOn(MSG_Ret_Holdon_SC data)
    {
        FFDebug.Log(this, FFLogType.HoldOn, string.Format("OnReturnHoldOn ............ data.retcod = {0}; type = {1}", data.retcode, data.type));
        if (data.npctype == 19U)
        {
            this.occupyController.HandlePickUpRet(data);
        }
        else if (data.type == 1U)
        {
            if (data.retcode == 1U)
            {
                if (this.OnHoldOnFinish != null)
                {
                    this.OnHoldOnFinish(false);
                }
                return;
            }
            if (data.retcode == 0U)
            {
                this.occupyController.HoldOn(data);
            }
        }
        else
        {
            this.occupyController.HoldOnComplete(data.retcode);
        }
    }

    private void OnReturnHoldOnInterupt(MSG_Ret_Holdon_Interrupt_SC data)
    {
        this.occupyController.BreakHoldOn(data.baseid);
        if (this.OnHoldOnFinish != null)
        {
            this.OnHoldOnFinish(false);
        }
    }

    private void OnRetAddHoldNpcData(MSG_Ret_AddHoldNpcData_SC data)
    {
        this.occupyController.AddHoldNpcData(data.data);
    }

    private void OnRetBatchAddHoldNpcData(MSG_Ret_BatchAddHoldNpcData_SC data)
    {
        for (int i = 0; i < data.datas.Count; i++)
        {
            this.occupyController.AddHoldNpcData(data.datas[i]);
        }
    }

    private void OnRemoveHoldNpcData(MSG_Ret_RemoveHoldNpcData_SC data)
    {
        this.occupyController.RemoveHoldNpcData(data.npc_tempid);
    }

    private void OnReturnHoldonSuccessc(MSG_Ret_HoldonSuccess data)
    {
        if (data.type == 1U)
        {
            ControllerManager.Instance.GetController<OccupyController>().ShowHoldSuccessEffect(data.npc_tempid);
            if (this.OnHoldOnFinish != null)
            {
                this.OnHoldOnFinish(true);
            }
        }
        else if (data.type == 2U)
        {
            ControllerManager.Instance.GetController<OccupyController>().ShowTransformSuccessEffect(data.npc_tempid);
        }
    }

    public Action<bool> OnHoldOnFinish;
}
