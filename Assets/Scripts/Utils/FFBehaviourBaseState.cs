using System;

public class FFBehaviourBaseState : IState
{
    public virtual void OnEnter(StateMachine parent)
    {
        this.Parent = (parent as FFBehaviourControl);
        this.RunningTime = 0f;
        this.lastmillsecond = DateTime.Now.Ticks;
    }

    public float RunningTime
    {
        get
        {
            return this.runningTime;
        }
        set
        {
            this.lastmillsecond = DateTime.Now.Ticks;
            this.runningTime = value;
        }
    }

    public int MoveDir()
    {
        return this.CurrMoveDir;
    }

    public virtual void OnUpdate(StateMachine parent)
    {
        long ticks = DateTime.Now.Ticks;
        this.RunningTime += (float)(ticks - this.lastmillsecond) / 1E+07f;
        this.lastmillsecond = ticks;
    }

    public virtual void OnExit(StateMachine parent)
    {
    }

    public FFBehaviourControl Parent;

    private float runningTime;

    public int CurrMoveDir;

    public long lastmillsecond = DateTime.Now.Ticks;
}
