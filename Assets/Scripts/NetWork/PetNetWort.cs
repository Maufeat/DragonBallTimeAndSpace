using System;
using Framework.Managers;
using Net;
using Pet;

public class PetNetWort : NetWorkBase
{
    public override void Initialize()
    {
        this.RegisterMsg();
    }

    public override void RegisterMsg()
    {
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_RetSwitchPetState_SC>(2364, new ProtoMsgCallback<MSG_RetSwitchPetState_SC>(this.RetPetState));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_RetRefreshSummonPet_SC>(2365, new ProtoMsgCallback<MSG_RetRefreshSummonPet_SC>(this.RetPetList));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_RetUserPetInfo_SC>(2366, new ProtoMsgCallback<MSG_RetUserPetInfo_SC>(this.RetPetBarCount));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_NotifyPetQTESkill_SC>(2367, new ProtoMsgCallback<MSG_NotifyPetQTESkill_SC>(this.RetPetQTEState));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_RetUnlockPetNum_SC>(2369, new ProtoMsgCallback<MSG_RetUnlockPetNum_SC>(this.RetUnlockPetBar));
    }

    private PetController petController
    {
        get
        {
            return ControllerManager.Instance.GetController<PetController>();
        }
    }

    private MainUIController uimainController
    {
        get
        {
            return ControllerManager.Instance.GetController<MainUIController>();
        }
    }

    public void ReqSwitchPetState(ulong tmpID, PetState from, PetState to)
    {
        base.SendMsg<MSG_ReqSwitchPetState_CS>(CommandID.MSG_ReqSwitchPetState_CS, new MSG_ReqSwitchPetState_CS
        {
            tempid = tmpID,
            fromstate = from,
            tostate = to
        }, false);
    }

    public void ReqUnlockPetBar()
    {
        base.SendMsg<MSG_ReqUnlockPetNum_CS>(CommandID.MSG_ReqUnlockPetNum_CS, new MSG_ReqUnlockPetNum_CS
        {
            unlockcount = MainPlayer.Self.petBarUnlockcount
        }, false);
    }

    public void RetUnlockPetBar(MSG_RetUnlockPetNum_SC msgInfo)
    {
        if (msgInfo.retcode == 1U)
        {
            MainPlayer.Self.petBarCount += 1U;
            MainPlayer.Self.petBarUnlockcount += 1U;
            this.petController.UnLockPetBarSuccess();
        }
        else
        {
            FFDebug.LogWarning(this, "  Unlock   PetBar   fail ");
        }
    }

    public void RetPetState(MSG_RetSwitchPetState_SC msgInfo)
    {
        if (msgInfo.errcode == 1U)
        {
            FFDebug.LogWarning(this, "  Change   State  fail ");
        }
        else
        {
            this.petController.RetChangePetState(msgInfo);
        }
    }

    public void RetPetList(MSG_RetRefreshSummonPet_SC msgInfo)
    {
        this.petController.RefreshPetList(msgInfo.pet);
    }

    public void RetPetBarCount(MSG_RetUserPetInfo_SC msgInfo)
    {
        this.petController.InitPetListData(msgInfo.info);
    }

    public void RetPetQTEState(MSG_NotifyPetQTESkill_SC msgInfo)
    {
        if (msgInfo.onoff == this.QTEOFF)
        {
            this.uimainController.DisablePetQTE();
        }
        else
        {
            this.uimainController.EnablePetQTE(msgInfo);
        }
    }

    private uint QTEOFF = 2U;
}
