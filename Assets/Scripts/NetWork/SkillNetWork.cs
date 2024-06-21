using System;
using career;
using Framework.Managers;
using magic;
using msg;
using Net;
using UnityEngine;

public class SkillNetWork : NetWorkBase
{
    public override void Initialize()
    {
        base.Initialize();
    }

    public override void RegisterMsg()
    {
        base.RegisterMsg();
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_Ret_StartMagicAttack_SC>(CommandID.MSG_Ret_StartMagicAttack_SC, new ProtoMsgCallback<MSG_Ret_StartMagicAttack_SC>(this.HandleStartDisplaySkill));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_Ret_SyncSkillStage_SC>(CommandID.MSG_Ret_SyncSkillStage_SC, new ProtoMsgCallback<MSG_Ret_SyncSkillStage_SC>(this.HandleSyncSkillStage));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_Ret_MagicAttack_SC>(CommandID.MSG_Ret_MagicAttack_SC, new ProtoMsgCallback<MSG_Ret_MagicAttack_SC>(this.HandleMagicAttackBack));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_Ret_InterruptSkill_SC>(CommandID.MSG_Ret_InterruptSkill_SC, new ProtoMsgCallback<MSG_Ret_InterruptSkill_SC>(this.HandleBreakSkill));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_Ret_HpMpPop_SC>(CommandID.MSG_Ret_HpMpPop_SC, new ProtoMsgCallback<MSG_Ret_HpMpPop_SC>(this.HandleHpMpPop));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_RetRefreshSkill_SC>(CommandID.MSG_RetRefreshSkill_SC, new ProtoMsgCallback<MSG_RetRefreshSkill_SC>(this.HandleRefreshSkill));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_Ret_AttWarning_SC>(CommandID.MSG_Ret_AttWarning_SC, new ProtoMsgCallback<MSG_Ret_AttWarning_SC>(this.HandleAttWarning));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_RetTriggerQTESkill_SC>(CommandID.MSG_RetTriggerQTESkill_SC, new ProtoMsgCallback<MSG_RetTriggerQTESkill_SC>(this.RetQTESkill));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_RetDrinkBloodSkill_SC>(CommandID.MSG_RetDrinkBloodSkill_SC, new ProtoMsgCallback<MSG_RetDrinkBloodSkill_SC>(this.RetDrinkBloodSkill));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_NoneChantSkill_SC>(CommandID.MSG_NoneChantSkill_SC, new ProtoMsgCallback<MSG_NoneChantSkill_SC>(this.OnNoneChantSkill));
    }

    private void OnNoneChantSkill(MSG_NoneChantSkill_SC msg)
    {
        ManagerCenter.Instance.GetManager<SkillManager>().RefreahChantSkillLimit(msg.arrskill);
    }

    private void RetDrinkBloodSkill(MSG_RetDrinkBloodSkill_SC msg)
    {
        if (msg.errcode == 1U)
        {
            FFDebug.LogError(this, "释放饮血技失败");
        }
        else
        {
            ManagerCenter.Instance.GetManager<SkillManager>().SendDrinkSkill();
        }
    }

    private void RetQTESkill(MSG_RetTriggerQTESkill_SC msg)
    {
        if (msg.errcode == 1U)
        {
            FFDebug.LogError(this, "释放QTE技能失败");
        }
        else
        {
            ControllerManager.Instance.GetController<MainUIController>().DisablePetQTE();
        }
    }

    private void HandleHpMpPop(MSG_Ret_HpMpPop_SC msg)
    {
        ManagerCenter.Instance.GetManager<SkillManager>().HandleMpHpChange(msg);
        ControllerManager.Instance.GetController<PetController>().RetRefreshPetHp(msg);
    }

    private void HandleBreakSkill(MSG_Ret_InterruptSkill_SC msg)
    {
        ManagerCenter.Instance.GetManager<SkillManager>().HandleBreakSkill(msg);
    }

    private void HandleMagicAttackBack(MSG_Ret_MagicAttack_SC msg)
    {
        ManagerCenter.Instance.GetManager<SkillManager>().GetSkillHit(msg);
    }

    private void HandleStartDisplaySkill(MSG_Ret_StartMagicAttack_SC msg)
    {
        ManagerCenter.Instance.GetManager<SkillManager>().StartDisplaySkill(msg);
    }

    private void HandleSyncSkillStage(MSG_Ret_SyncSkillStage_SC msg)
    {
        ManagerCenter.Instance.GetManager<SkillManager>().DisplaySkillStage(msg);
    }

    private void HandleRefreshSkill(MSG_RetRefreshSkill_SC msg)
    {
        ManagerCenter.Instance.GetManager<SkillManager>().RefreshSkillServerData(msg.skills);
    }

    private void HandleAttWarning(MSG_Ret_AttWarning_SC msg)
    {
        ManagerCenter.Instance.GetManager<SkillManager>().SetAttackWarningEffect(msg);
    }

    public void ReqRelaseDrinkBloodSkill(ulong npcID, uint skillID)
    {
        base.SendMsg<MSG_ReqDrinkBloodSkill_CS>(CommandID.MSG_ReqDrinkBloodSkill_CS, new MSG_ReqDrinkBloodSkill_CS
        {
            npctempid = npcID
        }, false);
    }

    public void ReqRelasePetQTESkill(ulong bossID, uint dir, Vector2 pos)
    {
        base.SendMsg<MSG_ReqTriggerQTESkill_CS>(CommandID.MSG_ReqTriggerQTESkill_CS, new MSG_ReqTriggerQTESkill_CS
        {
            bosstempid = bossID,
            dir = dir,
            warppos = new Position
            {
                x = pos.x,
                y = pos.y
            }
        }, false);
    }

    public void TurnOffSkill(uint Skillid)
    {
        base.SendMsg<MSG_Req_OffSkill_CS>(CommandID.MSG_Req_OffSkill_CS, new MSG_Req_OffSkill_CS
        {
            skillid = Skillid
        }, false);
    }

    public void SendStartSkill(uint Skillid, EntryIDType Target)
    {
        MSG_Req_MagicAttack_CS msg_Req_MagicAttack_CS = new MSG_Req_MagicAttack_CS();
        msg_Req_MagicAttack_CS.magictype = Skillid;
        if (Target != null)
        {
            msg_Req_MagicAttack_CS.target = Target;
        }
        base.SendMsg<MSG_Req_MagicAttack_CS>(CommandID.MSG_Req_MagicAttack_CS, msg_Req_MagicAttack_CS, false);
    }

    public void SendSkillStage(ulong SkillStageid, uint Type, Vector2 Pos, uint UseDir, EntryIDType Target)
    {
        MSG_Req_SyncSkillStage_CS msg_Req_SyncSkillStage_CS = new MSG_Req_SyncSkillStage_CS();
        msg_Req_SyncSkillStage_CS.skillstage = SkillStageid;
        msg_Req_SyncSkillStage_CS.stagetype = Type;
        msg_Req_SyncSkillStage_CS.userdir = UseDir;
        msg_Req_SyncSkillStage_CS.desx = Pos.x;
        msg_Req_SyncSkillStage_CS.desy = Pos.y;
        if (Target != null)
        {
            msg_Req_SyncSkillStage_CS.target = Target;
        }
        base.SendMsg<MSG_Req_SyncSkillStage_CS>(CommandID.MSG_Req_SyncSkillStage_CS, msg_Req_SyncSkillStage_CS, false);
    }

    public void FakeSkillStage(uint SkillStageid)
    {
    }

    public void FakeSkillProcessBar(ulong player, uint SkillStageid)
    {
    }
}
