using System;
using UnityEngine;

namespace Game.CutScene
{
    [DBEvent("ShowMask")]
    [DBFriendlyName("ShowMask")]
    public class DBEventShowMaskFade : DBEventBase
    {
        private void Awake()
        {
        }

        public override void Execute()
        {
            if (this.HasPlay)
            {
                return;
            }
            this.HasPlay = true;
            this.HasFinished = false;
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
                float deltaTime = runningTime - base.FireTime;
                this.ProcessFadeEvent(deltaTime);
            }
            else
            {
                this.EndEvent();
            }
        }

        public override void SetDbBehacior()
        {
        }

        public void ProcessFadeEvent(float deltaTime)
        {
            this.currentCurveSampleTime = deltaTime;
            if (!DBEventShowMaskFade.texture)
            {
                DBEventShowMaskFade.texture = new Texture2D(1, 1, TextureFormat.ARGB32, false);
            }
            float num = this.fadeCurve.Evaluate(this.currentCurveSampleTime);
            num = Mathf.Min(Mathf.Max(0f, num), 1f);
            DBEventShowMaskFade.texture.SetPixel(0, 0, new Color(this.fadeColour.r, this.fadeColour.g, this.fadeColour.b, num));
            DBEventShowMaskFade.texture.Apply();
        }

        public override void EndEvent()
        {
            if (this.HasFinished)
            {
                return;
            }
            if (!DBEventShowMaskFade.texture)
            {
                DBEventShowMaskFade.texture = new Texture2D(1, 1, TextureFormat.ARGB32, false);
            }
            float num = this.fadeCurve.Evaluate(this.fadeCurve.keys[this.fadeCurve.length - 1].time);
            num = Mathf.Min(Mathf.Max(0f, num), 1f);
            DBEventShowMaskFade.texture.SetPixel(0, 0, new Color(this.fadeColour.r, this.fadeColour.g, this.fadeColour.b, num));
            DBEventShowMaskFade.texture.Apply();
            this.StopEvent();
        }

        public override void StopEvent()
        {
            this.UndoEvent();
            this.HasPlay = false;
            this.HasFinished = true;
        }

        public override void UndoEvent()
        {
            this.currentCurveSampleTime = 0f;
            if (!DBEventShowMaskFade.texture)
            {
                DBEventShowMaskFade.texture = new Texture2D(1, 1, TextureFormat.ARGB32, false);
            }
            DBEventShowMaskFade.texture.SetPixel(0, 0, new Color(this.fadeColour.r, this.fadeColour.g, this.fadeColour.b, 0f));
            DBEventShowMaskFade.texture.Apply();
        }

        private void OnGUI()
        {
            if (!this.HasPlay)
            {
                return;
            }
            float num = 0f;
            foreach (Keyframe keyframe in this.fadeCurve.keys)
            {
                if (keyframe.time > num)
                {
                    num = keyframe.time;
                }
            }
            base.Duration = num;
            if (!DBEventShowMaskFade.texture)
            {
                return;
            }
            int depth = GUI.depth;
            GUI.depth = 0;
            GUI.DrawTexture(new Rect(0f, 0f, (float)Screen.width, (float)Screen.height), DBEventShowMaskFade.texture);
            GUI.depth = depth;
        }

        private void OnEnable()
        {
            if (DBEventShowMaskFade.texture == null)
            {
                DBEventShowMaskFade.texture = new Texture2D(1, 1, TextureFormat.ARGB32, false);
            }
            DBEventShowMaskFade.texture.SetPixel(0, 0, new Color(this.fadeColour.r, this.fadeColour.g, this.fadeColour.b, 0f));
            DBEventShowMaskFade.texture.Apply();
        }

        private bool HasPlay;

        private bool HasFinished;

        private bool Playing;

        public AnimationCurve fadeCurve = new AnimationCurve(new Keyframe[]
        {
            new Keyframe(0f, 0f),
            new Keyframe(1f, 1f),
            new Keyframe(3f, 1f),
            new Keyframe(4f, 0f)
        });

        public Color fadeColour = Color.black;

        private float currentCurveSampleTime;

        public static Texture2D texture;
    }
}
