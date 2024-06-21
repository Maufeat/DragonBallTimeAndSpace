using System;
using UnityEngine;

public class BufferStateAreaCircle : BufferState
{
    public BufferStateAreaCircle(UserState Flag) : base(Flag)
    {
        Scheduler.Instance.AddFrame(1U, true, new Scheduler.OnScheduler(this.MyUpdate));
    }

    private void MyUpdate()
    {
        if (this.FFeffectList.Count > 0)
        {
            FFeffect ffeffect = this.FFeffectList[0];
            if (ffeffect != null && ffeffect.effobj != null)
            {
                GameObject effobj = ffeffect.effobj;
                effobj.transform.GetComponentInChildren<Projector>().orthographicSize = 7.7f;
                MeshRenderer component = effobj.transform.GetChild(0).GetComponent<MeshRenderer>();
                if (null != component)
                {
                    effobj.transform.GetChild(0).GetComponent<MeshRenderer>().material.shader = Shader.Find("Unlit/SlashCustom");
                }
                Scheduler.Instance.RemoveFrame(new Scheduler.OnScheduler(this.MyUpdate));
            }
        }
    }

    public override void Enter(PlayerBufferControl PBControl)
    {
        base.Enter(PBControl);
    }

    public override void Exit()
    {
        base.Exit();
    }
}
