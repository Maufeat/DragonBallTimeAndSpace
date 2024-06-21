using System;
using Game.CutScene;
using UnityEngine;

public class DBTimelineEvent : DBTimelineBase
{
    public DBEventBase[] Events
    {
        get
        {
            this.cachedEvents = base.GetComponentsInChildren<DBEventBase>();
            return this.cachedEvents;
        }
    }

    public new void SetCurDBTrack()
    {
    }

    public override void StopTimeline()
    {
    }

    public override void StartTimeline()
    {
    }

    public override void EndTimeline()
    {
        for (int i = 0; i < this.Events.Length; i++)
        {
            this.Events[i].EndEvent();
        }
    }

    public override void PauseTimeline()
    {
    }

    public override void ResumeTimeline()
    {
    }

    public override void SkipTimelineTo(float time)
    {
    }

    public override void Process(float sequencerTime, float playbackRate)
    {
        for (int i = 0; i < this.Events.Length; i++)
        {
            this.Events[i].ProcessEvent(sequencerTime);
        }
    }

    public override void ManuallySetTime(float sequencerTime)
    {
    }

    public override void LateBindAffectedObjectInScene(Transform newAffectedObject)
    {
        this.affectObject = newAffectedObject;
    }

    public override void ResetCachedData()
    {
        base.ResetCachedData();
    }

    private Transform affectObject;

    private DBEventBase[] cachedEvents;
}
