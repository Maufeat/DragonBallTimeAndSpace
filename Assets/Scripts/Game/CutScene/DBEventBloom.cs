using System;
using Framework.Managers;

namespace Game.CutScene
{
    [DBFriendlyName("Bloom")]
    [DBEvent("Bloom")]
    public class DBEventBloom : DBEventBase
    {
        private void Awake()
        {
            this.manager = ManagerCenter.Instance.GetManager<CutSceneManager>();
        }

        public override void Execute()
        {
            this.manager.ActiveCurrBlomm(true);
        }

        public override void ProcessEvent(float runningTime)
        {
            if (runningTime >= base.FireTime + base.Duration)
            {
                this.EndEvent();
            }
            else if (runningTime >= base.FireTime)
            {
                this.Execute();
            }
        }

        public override void SetDbBehacior()
        {
        }

        public override void EndEvent()
        {
            this.manager.ActiveCurrBlomm(false);
        }

        public override void StopEvent()
        {
            this.manager.ActiveCurrBlomm(false);
        }

        private CutSceneManager manager;

        private bool Playing;
    }
}
