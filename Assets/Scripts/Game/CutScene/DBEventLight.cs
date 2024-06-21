using System;
using Framework.Managers;

namespace Game.CutScene
{
    [DBFriendlyName("Light")]
    [DBEvent("Light")]
    public class DBEventLight : DBEventBase
    {
        private void Awake()
        {
        }

        public override void Execute()
        {
            CutSceneContent curSceneContent = ManagerCenter.Instance.GetManager<CutSceneManager>().GetCurSceneContent();
            if (null != curSceneContent)
            {
                curSceneContent.UpdateCharactorLighInfo(this.LightName);
            }
        }

        public override void ProcessEvent(float runningTime)
        {
            if (runningTime >= base.FireTime + base.Duration)
            {
                this.EndEvent();
            }
            else if (runningTime >= base.FireTime)
            {
                if (this.Playing)
                {
                    return;
                }
                this.Playing = true;
                this.Execute();
            }
        }

        public override void SetDbBehacior()
        {
        }

        public override void EndEvent()
        {
        }

        public override void StopEvent()
        {
        }

        public string LightName = string.Empty;

        private bool Playing;
    }
}
