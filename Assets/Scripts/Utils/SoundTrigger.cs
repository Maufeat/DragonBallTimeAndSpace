using System;
using UnityEngine;

public class SoundTrigger : MonoBehaviour
{
    public int getSoundIndex
    {
        get
        {
            return this.soundIndex;
        }
    }

    public int getTriggerIndex
    {
        get
        {
            return this.triggerIndex;
        }
    }

    public void SetData(int soundIndex, int triggerIndex, Action<SoundTrigger> onTriggerEnter, Action<SoundTrigger> onTriggerExit)
    {
        this.soundIndex = soundIndex;
        this.triggerIndex = triggerIndex;
        this.onEnter = onTriggerEnter;
        this.onExit = onTriggerExit;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (MainPlayer.Self == null || MainPlayer.Self.ModelObj == null || other.gameObject != MainPlayer.Self.ModelObj)
        {
            return;
        }
        if (this.onEnter != null)
        {
            this.onEnter(this);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (MainPlayer.Self == null || MainPlayer.Self.ModelObj == null || other.gameObject != MainPlayer.Self.ModelObj)
        {
            return;
        }
        if (this.onExit != null)
        {
            this.onExit(this);
        }
    }

    private int soundIndex;

    private int triggerIndex;

    private Action<SoundTrigger> onEnter;

    private Action<SoundTrigger> onExit;
}
