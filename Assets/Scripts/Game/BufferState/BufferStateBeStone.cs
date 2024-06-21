using System;
using UnityEngine;

public class BufferStateBeStone : BufferState
{
    public BufferStateBeStone(UserState Flag) : base(Flag)
    {
    }

    public override void Enter(PlayerBufferControl PBControl)
    {
        base.Enter(PBControl);
        this.HidePlayerSwitch(false);
        Scheduler.Instance.AddUpdator(new Scheduler.OnScheduler(this.Update));
    }

    public override void Exit()
    {
        base.Exit();
        this.HidePlayerSwitch(true);
        Scheduler.Instance.RemoveUpdator(new Scheduler.OnScheduler(this.Update));
        if (this.BufferControl.Owner != null && this.BufferControl.Owner.ModelObj != null)
        {
            CapsuleCollider component = this.BufferControl.Owner.ModelObj.GetComponent<CapsuleCollider>();
            if (component != null)
            {
                component.enabled = true;
            }
        }
    }

    private void HidePlayerSwitch(bool b)
    {
        if (this.BufferControl.Owner != null && this.BufferControl.Owner.ModelObj != null)
        {
            SkinnedMeshRenderer[] componentsInChildren = this.BufferControl.Owner.ModelObj.GetComponentsInChildren<SkinnedMeshRenderer>(true);
            if (componentsInChildren != null)
            {
                for (int i = 0; i < componentsInChildren.Length; i++)
                {
                    componentsInChildren[i].gameObject.SetActive(b);
                }
            }
        }
    }

    private void Update()
    {
        if (this.BufferControl.Owner != null && this.BufferControl.Owner.ModelObj != null)
        {
            CapsuleCollider component = this.BufferControl.Owner.ModelObj.GetComponent<CapsuleCollider>();
            if (component != null)
            {
                component.enabled = false;
            }
        }
    }

    public string effectName;

    public FFeffect ffectct;
}
