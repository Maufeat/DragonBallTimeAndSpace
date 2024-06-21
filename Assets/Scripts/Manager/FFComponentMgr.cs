using System;
using System.Collections.Generic;

public class FFComponentMgr
{
    public FFComponentMgr(CharactorBase Char)
    {
        this.Owner = Char;
    }

    public void AddComponentImmediate(IFFComponent IFFcomp)
    {
        if (!this.IFFComponentList.Contains(IFFcomp))
        {
            IFFcomp.State = CompnentState.Running;
            this.IFFComponentList.Add(IFFcomp);
            IFFcomp.CompAwake(this);
        }
    }

    public void AddComponent(IFFComponent IFFcomp)
    {
        if (!this.IFFComponentList.Contains(IFFcomp))
        {
            this.IFFComponentList.Add(IFFcomp);
            IFFcomp.State = CompnentState.Standby;
            if (this.hasInit)
            {
                IFFcomp.CompAwake(this);
            }
        }
    }

    public T GetComponent<T>() where T : IFFComponent
    {
        for (int i = 0; i < this.IFFComponentList.Count; i++)
        {
            IFFComponent iffcomponent = this.IFFComponentList[i];
            if (iffcomponent is T)
            {
                return (T)((object)iffcomponent);
            }
        }
        return default(T);
    }

    public void RemoveComponent(IFFComponent IFFcomp)
    {
        if (this.IFFComponentList.Contains(IFFcomp))
        {
            this.IFFComponentList.Remove(IFFcomp);
            IFFcomp.CompDispose();
        }
    }

    public void Update()
    {
        if (!this.hasInit)
        {
            return;
        }
        for (int i = 0; i < this.IFFComponentList.Count; i++)
        {
            this.IFFComponentList[i].CompUpdate();
        }
    }

    public void InitOver()
    {
        if (this.hasInit)
        {
            return;
        }
        this.hasInit = true;
        for (int i = 0; i < this.IFFComponentList.Count; i++)
        {
            if (this.IFFComponentList[i].State == CompnentState.Standby)
            {
                this.IFFComponentList[i].CompAwake(this);
                this.IFFComponentList[i].State = CompnentState.Running;
            }
        }
    }

    public void ResetAllCompment()
    {
        if (!this.hasInit)
        {
            return;
        }
        for (int i = 0; i < this.IFFComponentList.Count; i++)
        {
            this.IFFComponentList[i].ResetComp();
        }
    }

    public void RefreshIFFComponentList()
    {
        for (int i = 0; i < this.IFFComponentList.Count; i++)
        {
            this.IFFComponentList[i].CompAwake(this);
        }
    }

    public void Dispose()
    {
        for (int i = 0; i < this.IFFComponentList.Count; i++)
        {
            this.IFFComponentList[i].State = CompnentState.Disposed;
            this.IFFComponentList[i].CompDispose();
        }
        this.IFFComponentList.Clear();
    }

    public CharactorBase Owner;

    private List<IFFComponent> IFFComponentList = new List<IFFComponent>();

    private bool hasInit;
}
