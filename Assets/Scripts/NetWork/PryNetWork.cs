using System;
using Framework.Managers;
using guild;
using Net;

public class PryNetWork : NetWorkBase
{
    public override void Initialize()
    {
        this.RegisterMsg();
    }

    private PryController pryController
    {
        get
        {
            return ControllerManager.Instance.GetController<PryController>();
        }
    }

    public override void RegisterMsg()
    {
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_Ret_CiTanEnemyGuildList_SC>(60064, new ProtoMsgCallback<MSG_Ret_CiTanEnemyGuildList_SC>(this.OnRetPryGuildList));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_Ret_ChoosedCiTanEnemyGuild_SC>(60067, new ProtoMsgCallback<MSG_Ret_ChoosedCiTanEnemyGuild_SC>(this.OnRetChoosePryEnemyGuild));
    }

    public void ReqTargetPryEnemyGuild()
    {
        base.SendMsg<MSG_Req_TargetCiTanEnemyGuild_CS>(CommandID.MSG_Req_TargetCiTanEnemyGuild_CS, new MSG_Req_TargetCiTanEnemyGuild_CS(), false);
    }

    public void ReqPryGuildList()
    {
        base.SendMsg<MSG_Req_CiTanEnemyGuildList_CS>(CommandID.MSG_Req_CiTanEnemyGuildList_CS, new MSG_Req_CiTanEnemyGuildList_CS(), false);
    }

    public void OnRetPryGuildList(MSG_Ret_CiTanEnemyGuildList_SC msgInfo)
    {
        if (msgInfo != null && this.pryController.uiPry != null)
        {
            this.pryController.uiPry.ShowPryEnemyGuildsList(msgInfo);
        }
    }

    public void ReqChoosePryEnemyGuild(ulong guildid)
    {
        base.SendMsg<MSG_Req_ChooseCiTanEnemyGuild_CS>(CommandID.MSG_Req_ChooseCiTanEnemyGuild_CS, new MSG_Req_ChooseCiTanEnemyGuild_CS
        {
            guildid = guildid
        }, false);
    }

    public void OnRetChoosePryEnemyGuild(MSG_Ret_ChoosedCiTanEnemyGuild_SC msgInfo)
    {
        this.pryController.currentPryGuild = msgInfo.guild;
        if (this.pryController.uiPry != null)
        {
            this.pryController.uiPry.RefreshData();
        }
    }

    public void ReqIntoPryEnemyGuild()
    {
        base.SendMsg<MSG_Req_IntoCiTanEnemyGuild_CS>(CommandID.MSG_Req_IntoCiTanEnemyGuild_CS, new MSG_Req_IntoCiTanEnemyGuild_CS(), false);
    }

    public void ReqBackToMyGuild()
    {
        base.SendMsg<MSG_Req_CiTanBacktoMyGuild_CS>(CommandID.MSG_Req_CiTanBacktoMyGuild_CS, new MSG_Req_CiTanBacktoMyGuild_CS(), false);
    }
}
