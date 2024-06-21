using System;
using UnityEngine;

namespace Game.CutScene
{
    [DBFriendlyName("Play Audio")]
    [DBEvent("Audio/Play Audio")]
    public class DBEventAudioBehavior : DBEventBase
    {
        public override void Execute()
        {
        }

        public override void ProcessEvent(float runningTime)
        {
        }

        public void InitDBBehavior(GameObject obj)
        {
        }

        public string audionName = string.Empty;
    }
}
