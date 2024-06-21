using System;
using Framework.Managers;
using hero;
using Net;

public class CharacterNetwork : NetWorkBase
{
    public override void Initialize()
    {
        this.RegisterMsg();
    }

    public override void RegisterMsg()
    {
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_SetMainHero_CSC>(2203, new ProtoMsgCallback<MSG_SetMainHero_CSC>(this.OnRetSetMainHero));
    }

    public void ReqSetMainHero(ulong herothisid)
    {
        base.SendMsg<MSG_SetMainHero_CSC>(CommandID.MSG_SetMainHero_CSC, new MSG_SetMainHero_CSC
        {
            herothisid = herothisid,
            opcode = 0U
        }, false);
    }

    public void ReqGetMainHero()
    {
        base.SendMsg<MSG_SetMainHero_CSC>(CommandID.MSG_SetMainHero_CSC, new MSG_SetMainHero_CSC
        {
            opcode = 1U
        }, false);
    }

    private void OnRetSetMainHero(MSG_SetMainHero_CSC data)
    {
        if (data.errorcode != 0U)
        {
            return;
        }
        MainUIController controller = ControllerManager.Instance.GetController<MainUIController>();
        controller.CheckSkillNewIconShow();
        HeroHandbookController controller2 = ControllerManager.Instance.GetController<HeroHandbookController>();
        if (controller2 != null)
        {
            controller2.SetupMainHero(data.herothisid);
        }
    }

    public override void Uninitialize()
    {
        base.Uninitialize();
    }

    private static bool IsGetMainHeroIdYet;
}
