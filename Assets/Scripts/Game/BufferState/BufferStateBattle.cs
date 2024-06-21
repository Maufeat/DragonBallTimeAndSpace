using System;
using Framework.Managers;

public class BufferStateBattle : BufferState
{
    public BufferStateBattle(UserState Flag) : base(Flag)
    {
    }

    public override void Enter(PlayerBufferControl PBControl)
    {
        this.BufferControl = PBControl;
        if (this.BufferControl.Owner == MainPlayer.Self)
        {
            TipsWindow.ShowWindow(TipsType.IN_FIGHT, null);
        }
        FFWeaponHold component = this.BufferControl.Owner.GetComponent<FFWeaponHold>();
        if (component != null)
        {
            component.ChangeState(true);
        }
        if (this.BufferControl.Owner is Npc)
        {
            Npc npc = this.BufferControl.Owner as Npc;
            if (npc.hpdata != null)
            {
                npc.hpdata.RefreshModel();
                if (!(this.BufferControl.Owner is Npc_Pet))
                {
                    npc.hpdata.PlayUIEffect(base.CurrBuffConfig.GetField_String("BuffEffect"));
                }
                npc.OnRelationChange();
            }
        }
        FFBehaviourControl component2 = this.BufferControl.Owner.GetComponent<FFBehaviourControl>();
        if (component2 != null)
        {
            FFBehaviourState_Idle ffbehaviourState_Idle = component2.CurrStatebyType<FFBehaviourState_Idle>();
            if (ffbehaviourState_Idle != null)
            {
                ffbehaviourState_Idle.playIdle(false);
            }
        }
        if (this.BufferControl.Owner == MainPlayer.Self)
        {
            try
            {
                if (UIManager.GetUIObject<UI_MainView>() != null)
                {
                    LuaScriptMgr.Instance.CallLuaFunction("MainUICtrl.ShowFightUI", new object[]
                    {
                        Util.GetLuaTable("MainUICtrl")
                    });
                    GameSystemSettings.SetMainPlayerInBattleState(true);
                    ControllerManager.Instance.GetController<MainUIController>().SetBattleEffectVisibility(true);
                }
                else
                {
                    ControllerManager.Instance.GetController<MainUIController>().isGoFightUI = true;
                }
            }
            catch (Exception arg)
            {
                FFDebug.LogError(this, "callluafunction error: " + arg);
            }
        }
    }

    public override void Exit()
    {
        base.Exit();
        if (this.BufferControl.Owner is Npc)
        {
            Npc npc = this.BufferControl.Owner as Npc;
            if (npc.hpdata != null)
            {
                npc.hpdata.RefreshModel();
            }
            npc.OnRelationChange();
        }
        if (this.BufferControl.Owner == MainPlayer.Self)
        {
            TipsWindow.ShowWindow(TipsType.OUT_FIGHT, null);
        }
        FFBehaviourControl component = this.BufferControl.Owner.GetComponent<FFBehaviourControl>();
        if (component != null)
        {
            FFBehaviourState_Idle ffbehaviourState_Idle = component.CurrStatebyType<FFBehaviourState_Idle>();
            if (ffbehaviourState_Idle != null)
            {
                ffbehaviourState_Idle.playIdle(false);
            }
        }
        FFWeaponHold component2 = this.BufferControl.Owner.GetComponent<FFWeaponHold>();
        if (component2 != null)
        {
            component2.ChangeState(false);
        }
        if (this.BufferControl.Owner == MainPlayer.Self)
        {
            if (UIManager.GetUIObject<UI_MainView>() != null)
            {
                LuaScriptMgr.Instance.CallLuaFunction("MainUICtrl.ShowFightUI", new object[]
                {
                    Util.GetLuaTable("MainUICtrl")
                });
                GameSystemSettings.SetMainPlayerInBattleState(false);
                ControllerManager.Instance.GetController<MainUIController>().SetBattleEffectVisibility(false);
            }
            else
            {
                ControllerManager.Instance.GetController<MainUIController>().isGoFightUI = true;
            }
        }
    }
}
