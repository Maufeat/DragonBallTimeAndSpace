using System;
using Framework.Managers;
using UnityEngine;

public class BufferStateHollowMan : BufferState
{
    public BufferStateHollowMan(UserState Flag) : base(Flag)
    {
    }

    public override void Enter(PlayerBufferControl PBControl)
    {
        base.Enter(PBControl);
        this.scc = ControllerManager.Instance.GetController<ShortcutsConfigController>();
        if (this.scc != null)
        {
            this.scc.SetShortcutKeyEnableState(false, false);
        }
        UI_HollowMan uiHollowMan = UIManager.GetUIObject<UI_HollowMan>();
        if (uiHollowMan == null)
        {
            ManagerCenter.Instance.GetManager<UIManager>().ShowUI<UI_HollowMan>("UI_HollowMan", delegate ()
            {
                uiHollowMan = UIManager.GetUIObject<UI_HollowMan>();
                if (uiHollowMan != null)
                {
                    uiHollowMan.CheckBombCountCloseUI();
                }
            }, UIManager.ParentType.CommonUI, false);
        }
        else
        {
            uiHollowMan.InitInfo();
        }
        Scheduler.Instance.AddUpdator(new Scheduler.OnScheduler(this.Update));
    }

    public void Update()
    {
        if (this.scc != null)
        {
            this.scc.SetShortcutKeyEnableState(false, false);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            UI_HollowMan uiobject = UIManager.GetUIObject<UI_HollowMan>();
            if (uiobject != null)
            {
                uiobject.UseBomb();
            }
        }
    }

    public override void Exit()
    {
        base.Exit();
        GlobalRegister.SetShortcutKeyEnableState(true);
        Scheduler.Instance.RemoveUpdator(new Scheduler.OnScheduler(this.Update));
        ManagerCenter.Instance.GetManager<UIManager>().DeleteUI("UI_HollowMan");
    }

    private ShortcutsConfigController scc;
}
