using System;
using Framework.Managers;

public class BufferStateGuildSheild : BufferState
{
    public BufferStateGuildSheild(UserState Flag) : base(Flag)
    {
    }

    public override void Enter(PlayerBufferControl PBControl)
    {
        base.Enter(PBControl);
        FFBehaviourControl component = this.BufferControl.Owner.GetComponent<FFBehaviourControl>();
        this.npcEffectCtrl = component.Owmner.GetComponent<FFEffectControl>();
        if (this.npcEffectCtrl != null && this.eventAddFlag == 0)
        {
            this.eventAddFlag = 1;
            Scheduler.Instance.AddUpdator(new Scheduler.OnScheduler(this.TryCheckEffectState));
        }
    }

    private void TryCheckEffectState()
    {
        if (this.npcEffectCtrl != null)
        {
            FFeffect eeffectByName = this.npcEffectCtrl.GetEeffectByName(string.Empty);
            if (eeffectByName != null && eeffectByName.effobj != null && this.eventAddFlag == 1)
            {
                FFBehaviourControl component = this.BufferControl.Owner.GetComponent<FFBehaviourControl>();
                RelationType relationType = ManagerCenter.Instance.GetManager<EntitiesManager>().CheckRelationBaseMainPlayer(this.BufferControl.Owner);
                this.eventAddFlag = 2;
                bool flag = relationType == RelationType.Friend;
                Scheduler.Instance.RemoveUpdator(new Scheduler.OnScheduler(this.TryCheckEffectState));
                if (eeffectByName.effobj.transform.GetChild(0) != null)
                {
                    eeffectByName.effobj.transform.GetChild(0).gameObject.SetActive(flag);
                }
                if (eeffectByName.effobj.transform.GetChild(1) != null)
                {
                    eeffectByName.effobj.transform.GetChild(1).gameObject.SetActive(!flag);
                }
            }
        }
    }

    public override void Exit()
    {
        Debugger.LogError("Exit BufferStateGuildSheild", new object[0]);
        base.Exit();
        if (this.eventAddFlag == 1)
        {
            Scheduler.Instance.RemoveUpdator(new Scheduler.OnScheduler(this.TryCheckEffectState));
        }
    }

    private byte eventAddFlag;

    private FFEffectControl npcEffectCtrl;
}
