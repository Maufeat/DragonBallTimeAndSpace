using System;
using Framework.Managers;
using UnityEngine;

namespace Game.CutScene
{
    [DBEvent("Effect/Play Effect")]
    [DBFriendlyName("Play Effect")]
    public class DBEventEffectBehavior : DBEventBase
    {
        private void Awake()
        {
            this.manager = ManagerCenter.Instance.GetManager<CutSceneManager>();
            EventTranslate x = base.gameObject.GetComponent<EventTranslate>();
            if (x == null)
            {
                x = base.gameObject.AddComponent<EventTranslate>();
            }
        }

        public override void Execute()
        {
            if (this.Playing)
            {
                return;
            }
            this.Playing = true;
            EventTranslate eventTranslate = base.gameObject.GetComponent<EventTranslate>();
            if (eventTranslate == null)
            {
                eventTranslate = base.gameObject.AddComponent<EventTranslate>();
            }
            if (eventTranslate != null)
            {
                Transform transform = null;
                if (this.ParentId >= 0)
                {
                    Transform effectParentByIndex = this.manager.GetEffectParentByIndex(this.ParentId);
                    if (null != effectParentByIndex && !string.IsNullOrEmpty(this.ChildName))
                    {
                        transform = effectParentByIndex.Find(this.ChildName);
                    }
                }
                else
                {
                    transform = this.parent;
                }
                eventTranslate.PlayEffect(this.effectName, transform);
            }
        }

        public override void ProcessEvent(float runningTime)
        {
            if (runningTime >= base.FireTime)
            {
                this.Execute();
            }
            if (runningTime >= base.FireTime + base.Duration)
            {
                this.EndEvent();
            }
        }

        public override void StopEvent()
        {
        }

        public override void EndEvent()
        {
            EventTranslate eventTranslate = base.gameObject.GetComponent<EventTranslate>();
            if (eventTranslate == null)
            {
                eventTranslate = base.gameObject.AddComponent<EventTranslate>();
            }
            if (eventTranslate != null)
            {
                eventTranslate.RemoveEffectObj(this.effectName);
            }
        }

        public override void SetDbBehacior()
        {
        }

        public void SetEffect(string effect)
        {
            this.effectName = effect;
        }

        private CutSceneManager manager;

        public string effectName = string.Empty;

        private bool Playing;

        public Transform parent;

        public int ParentId = -1;

        public string ChildName = string.Empty;
    }
}
