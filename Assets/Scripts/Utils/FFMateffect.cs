using System;
using System.Collections.Generic;
using UnityEngine;

public class FFMateffect
{
    public FFMateffect(FFMaterialAnimClip Clip)
    {
        this.MAClip = Clip;
        this.StartTime = this.FrameToTime(this.MAClip.StartDalay);
        this.EndTime = this.StartTime + this.FrameToTime(this.MAClip.Duration);
    }

    public void update()
    {
        this.RunningTime += Time.deltaTime;
        if (this.RunningTime >= this.EndTime)
        {
            if (this.MAClip.Isloop)
            {
                this.RunningTime = 0f;
            }
            else
            {
                this.mState = FFMateffect.State.Over;
            }
        }
        if (this.mState == FFMateffect.State.Dalay && this.RunningTime >= this.StartTime)
        {
            this.mState = FFMateffect.State.Play;
        }
        if (this.mState == FFMateffect.State.Play && this.CanPlayer)
        {
            this.PlayAnim(this.RunningTime);
        }
    }

    private void PlayAnim(float mRTime)
    {
        BetterDictionary<string, Color> betterDictionary = new BetterDictionary<string, Color>();
        for (int i = 0; i < this.MAClip.AllCurveData.Length; i++)
        {
            FFCurveData ffcurveData = this.MAClip.AllCurveData[i];
            if (!this.GetColor(ffcurveData.propertyName, ffcurveData.curve, mRTime, betterDictionary))
            {
                this.Control.SetRoleFloat(this.GetpropertyName(ffcurveData.propertyName), ffcurveData.curve.Evaluate(mRTime));
            }
        }
        betterDictionary.BetterForeach(delegate (KeyValuePair<string, Color> item)
        {
            KeyValuePair<string, Color> keyValuePair = item;
            this.Control.SetRoleColor(keyValuePair.Key, keyValuePair.Value);
        });
    }

    private bool GetColor(string Param, AnimationCurve curve, float time, Dictionary<string, Color> ColorTmpMap)
    {
        string[] array = Param.Split(new char[]
        {
            '.'
        });
        if (array.Length != 2)
        {
            return false;
        }
        if (array[1] != "r" && array[1] != "g" && array[1] != "b" && array[1] != "a")
        {
            return false;
        }
        string key = array[0];
        string a = array[1];
        if (!ColorTmpMap.ContainsKey(key))
        {
            ColorTmpMap[key] = new Color(0f, 0f, 0f);
        }
        Color value = ColorTmpMap[key];
        if (a == "r")
        {
            value.r = curve.Evaluate(time);
        }
        else if (a == "g")
        {
            value.g = curve.Evaluate(time);
        }
        else if (a == "b")
        {
            value.b = curve.Evaluate(time);
        }
        else if (a == "a")
        {
            value.a = curve.Evaluate(time);
        }
        ColorTmpMap[key] = value;
        return true;
    }

    public void DisPose()
    {
        BetterDictionary<string, Color> betterDictionary = new BetterDictionary<string, Color>();
        for (int i = 0; i < this.MAClip.AllCurveData.Length; i++)
        {
            FFCurveData ffcurveData = this.MAClip.AllCurveData[i];
            if (!this.GetColor(ffcurveData.propertyName, ffcurveData.curve, 0f, betterDictionary))
            {
                this.Control.SetRoleDefaultFloat(this.GetpropertyName(ffcurveData.propertyName));
            }
        }
        betterDictionary.BetterForeach(delegate (KeyValuePair<string, Color> item)
        {
            KeyValuePair<string, Color> keyValuePair = item;
            this.Control.SetRoleDefaultColor(keyValuePair.Key);
        });
    }

    private string GetpropertyName(string str)
    {
        if (str.StartsWith("_"))
        {
            return str;
        }
        return "_" + str;
    }

    private float FrameToTime(uint frame)
    {
        return frame / 30f;
    }

    public FFMaterialAnimClip MAClip;

    public float StartTime;

    public float EndTime;

    private float RunningTime;

    public FFMaterialEffectControl Control;

    public bool CanPlayer;

    public FFMateffect.State mState;

    public enum State
    {
        none,
        Dalay,
        Play,
        Over
    }
}
