using System;

public class FFBehaviourState_CopyAction : FFBehaviourBaseState, IstorebAble
{
    public override void OnEnter(StateMachine parent)
    {
        base.OnEnter(parent);
        this.PlayAnim();
    }

    public override void OnExit(StateMachine parent)
    {
        Scheduler.Instance.RemoveTimer(new Scheduler.OnScheduler(this.PlayAction));
        this.Parent.DisPoseEffect((uint)this._actionStartId);
        this.Parent.DisPoseEffect((uint)this._actionId);
    }

    public void Init(int startId, int actionId)
    {
        this._actionStartId = startId;
        this._actionId = actionId;
    }

    public void PlayAnim()
    {
        float time = this.Parent.PlayNormalAction((uint)this._actionStartId, false, 0.1f);
        Scheduler.Instance.AddTimer(time, false, new Scheduler.OnScheduler(this.PlayAction));
    }

    private void PlayAction()
    {
        this.Parent.DisPoseEffect((uint)this._actionStartId);
        this.Parent.PlayNormalAction((uint)this._actionId, false, 0.1f);
    }

    public bool IsDirty { get; set; }

    public void RestThisObject()
    {
        base.RunningTime = 0f;
        this.Parent = null;
    }

    public void StoreToPool()
    {
        ClassPool.Store<FFBehaviourState_CopyAction>(this, 60);
    }

    private int _actionStartId;

    private int _actionId;
}
