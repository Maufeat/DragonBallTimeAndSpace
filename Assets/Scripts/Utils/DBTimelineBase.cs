using System;
using UnityEngine;

public class DBTimelineBase : MonoBehaviour
{
    public Transform AffectedObject
    {
        get
        {
            return this.TimelineContainer.AffectedObject;
        }
    }

    public void SetCurDBTrack()
    {
    }

    public DBTrackContainer TimelineContainer
    {
        get
        {
            if (this.timelineContainer)
            {
                return this.timelineContainer;
            }
            this.timelineContainer = base.transform.parent.GetComponent<DBTrackContainer>();
            return this.timelineContainer;
        }
    }

    public bool ShouldRenderGizmos { get; set; }

    public virtual void StopTimeline()
    {
    }

    public virtual void StartTimeline()
    {
    }

    public virtual void EndTimeline()
    {
    }

    public virtual void PauseTimeline()
    {
    }

    public virtual void ResumeTimeline()
    {
    }

    public virtual void SkipTimelineTo(float time)
    {
    }

    public virtual void Process(float sequencerTime, float playbackRate)
    {
    }

    public virtual void ManuallySetTime(float sequencerTime)
    {
    }

    public virtual void LateBindAffectedObjectInScene(Transform newAffectedObject)
    {
    }

    public virtual void ResetCachedData()
    {
    }

    private DBTrackContainer timelineContainer;
}
