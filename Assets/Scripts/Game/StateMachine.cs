using System;

public class StateMachine
{
    public IState CurrState
    {
        get
        {
            return this._currState;
        }
    }

    public T CurrStatebyType<T>() where T : class, IState
    {
        if (this._currState is T)
        {
            return this._currState as T;
        }
        return (T)((object)null);
    }

    public virtual void Update()
    {
        if (this._currState != null)
        {
            this._currState.OnUpdate(this);
        }
    }

    public virtual void ChangeState(IState Next)
    {
        if (this._currState != null)
        {
            this._currState.OnExit(this);
        }
        this._currState = Next;
        if (this._currState != null)
        {
            this._currState.OnEnter(this);
        }
    }

    protected IState _currState;
}

public interface IState
{
    void OnEnter(StateMachine parent);

    void OnUpdate(StateMachine parent);

    void OnExit(StateMachine parent);

    int MoveDir();
}
