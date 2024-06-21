using System;
using evolution;
using Framework.Managers;
using Net;

public class HeroAwakeNetWorker : NetWorkBase
{
    private HeroAwakeController heroawakeController
    {
        get
        {
            return ControllerManager.Instance.GetController<HeroAwakeController>();
        }
    }

    public override void Initialize()
    {
        this.RegisterMsg();
    }

    public override void RegisterMsg()
    {
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_RetUserEvolution_SC>(CommandID.MSG_RetUserEvolution_SC, new ProtoMsgCallback<MSG_RetUserEvolution_SC>(this.OnRetAwakeHeroInfo));
    }

    public override void UnRegisterMsg()
    {
        base.UnRegisterMsg();
        LSingleton<NetWorkModule>.Instance.DeRegisterMsg(2535);
    }

    public void OnRetAwakeHeroInfo(MSG_RetUserEvolution_SC msg)
    {
        this.heroawakeController.AwakeHeroInfo = msg;
        MainUIController controller = ControllerManager.Instance.GetController<MainUIController>();
        controller.CheckSkillNewIconShow();
        HeroAwakeController controller2 = ControllerManager.Instance.GetController<HeroAwakeController>();
        controller2.PlayEvolutionEffect();
    }

    public void ReqHeroAwake(uint id)
    {
        base.SendMsg<MSG_ReqUserEvolution_CS>(CommandID.MSG_ReqUserEvolution_CS, new MSG_ReqUserEvolution_CS
        {
            evolutionId = id
        }, false);
    }

    public override void Uninitialize()
    {
        base.Uninitialize();
    }
}
