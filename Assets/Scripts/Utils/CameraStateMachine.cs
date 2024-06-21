using System;
using UnityEngine;

public class CameraStateMachine : MonoBehaviour
{
    public ICameraState CurrState
    {
        get
        {
            return this._currState;
        }
    }

    private void OnDestroy()
    {
        Scheduler.Instance.RemoveUpdator(new Scheduler.OnScheduler(this.CameraUpdate));
    }

    public T CurrStatebyType<T>() where T : class, ICameraState
    {
        if (this._currState is T)
        {
            return this._currState as T;
        }
        return (T)((object)null);
    }

    public virtual void CameraUpdate()
    {
        if (this._currState != null)
        {
            this._currState.OnUpdate(this);
        }
    }

    public virtual void ChangeState(ICameraState Next)
    {
        if (this._currState != null)
        {
            this._currState.OnExit(this);
        }
        this._currState = Next;
        if (Next is CameraFollowTarget2D)
        {
            GameSystemSettings.SetCameraState(CameraState.CameraFollowTarget2D);
        }
        else if (Next is CameraFollowTarget4)
        {
            GameSystemSettings.SetCameraState(CameraState.CameraFollowTarget4);
        }
        else if (Next is CameraFollowTargetPrepare)
        {
            GameSystemSettings.SetCameraState(CameraState.CameraFollowPrepare);
        }
        if (this._currState != null)
        {
            this._currState.OnEnter(this);
            this._currState.ChangeState();
        }
    }

    public virtual void EnterState(ICameraState Next)
    {
        if (this._currState != null)
        {
            this._currState.OnExit(this);
        }
        this._currState = Next;
        if (Next is CameraFollowTarget2D)
        {
            GameSystemSettings.SetCameraState(CameraState.CameraFollowTarget2D);
        }
        else if (Next is CameraFollowTarget4)
        {
            GameSystemSettings.SetCameraState(CameraState.CameraFollowTarget4);
        }
        else if (Next is CameraFollowTargetPrepare)
        {
            GameSystemSettings.SetCameraState(CameraState.CameraFollowPrepare);
        }
        if (this._currState != null)
        {
            this._currState.OnEnter(this);
        }
    }

    protected ICameraState _currState;
}

public interface ICameraState
{
    void OnEnter(CameraStateMachine CameraCtrl);

    void ChangeState();

    void OnUpdate(CameraStateMachine CameraCtrl);

    void OnExit(CameraStateMachine CameraCtrl);

    void OnGUI();

    Vector3 TargetPos();

    Vector3 TopPos();

    Vector3 FeetPos();
}