using System;
using UnityEngine;

namespace Game.CutScene
{
    [ExecuteInEditMode]
    [Serializable]
    public abstract class DBEventBase : MonoBehaviour
    {
        public float FireTime
        {
            get
            {
                return this.firetime;
            }
            set
            {
                this.firetime = value;
                if (this.firetime < 0f)
                {
                    this.firetime = 0f;
                }
                if (this.firetime > this.TimeLineDB.TimelineContainer.CutSceneContent.Duration)
                {
                    this.firetime = this.TimeLineDB.TimelineContainer.CutSceneContent.Duration;
                }
            }
        }

        public float Duration
        {
            get
            {
                return this.duration;
            }
            set
            {
                this.duration = value;
            }
        }

        public DBTimelineBase TimeLineDB
        {
            get
            {
                if (!base.transform.parent)
                {
                    return null;
                }
                DBTimelineBase component = base.transform.parent.GetComponent<DBTimelineBase>();
                if (!component)
                {
                }
                return component;
            }
        }

        public DBTrackContainer TDBimelineContainer
        {
            get
            {
                return this.TimeLineDB ? this.TimeLineDB.TimelineContainer : null;
            }
        }

        public GameObject AffectedObject
        {
            get
            {
                if (this.TDBimelineContainer == null)
                {
                    return null;
                }
                return this.TDBimelineContainer.AffectedObject ? this.TDBimelineContainer.AffectedObject.gameObject : null;
            }
        }

        public bool IsFireAndForgetEvent
        {
            get
            {
                return this.Duration < 0f;
            }
        }

        public virtual void SetDbBehacior()
        {
        }

        public abstract void Execute();

        public abstract void ProcessEvent(float runningTime);

        public virtual void PauseEvent()
        {
        }

        public virtual void ResumeEvent()
        {
        }

        public virtual void StopEvent()
        {
        }

        public virtual void EndEvent()
        {
        }

        public virtual void UndoEvent()
        {
        }

        [SerializeField]
        private float firetime = -1f;

        [SerializeField]
        private float duration = -1f;
    }
}
