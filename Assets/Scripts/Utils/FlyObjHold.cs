using System;
using UnityEngine;

public class FlyObjHold
{
    public FlyObjHold(FlyObjConfig config)
    {
        this.Config = config;
        this.StartTime = this.Config.LaunchDalayF / 30f;
        this.EndTime = this.Config.TotalLength / 30f;
        this.flyTime = this.Config.FlyLength / 30f;
        this.Fobj = new GameObject();
        this.Fobj.name = config.FlyobjNmae;
        this.Fobj.transform.position = Vector3.zero;
        this.Fobj.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        if (this.Config.mFlyTrackType == FlyObjConfig.FlyTrackType.Curve && this.Config.CurveSamlpeList.Length > 0)
        {
            this.InitSamlpe();
        }
    }

    private Vector3 OrgTargetPos
    {
        get
        {
            if (this.Config.mTargetType == FlyObjConfig.TargetType.TargetEntity)
            {
                return this.TTran.position;
            }
            return this.TPos;
        }
    }

    private Vector3 TrueTargetPos
    {
        get
        {
            return this.OrgTargetPos + this.Config.TargetRelativePos;
        }
    }

    public void Updata()
    {
        this.RunningTime += Time.deltaTime;
        if (this.mState == FlyObjHold.State.Dalay)
        {
            this.OnDalay();
        }
        else if (this.mState == FlyObjHold.State.Play)
        {
            this.OnPlay();
        }
    }

    private void UpdataAudio()
    {
    }

    private void PlayLoopAudio(bool startorEnd)
    {
    }

    private void OnDalay()
    {
        if (this.RunningTime >= this.StartTime)
        {
            this.mState = FlyObjHold.State.Play;
            this.Initflyobj();
            this.RunningTime = 0f;
            this.Updatafly(true);
        }
    }

    private void OnPlay()
    {
        if (this.RunningTime >= this.EndTime)
        {
            if (this.Fobj != null)
            {
                this.Fobj.SetActive(false);
            }
            this.mState = FlyObjHold.State.Over;
        }
        this.Updatafly(false);
    }

    private void Initflyobj()
    {
        Transform transform = this.Control.Root.transform;
        Transform transform2;
        if (this.Control.mBipBindMgr != null)
        {
            transform2 = this.Control.mBipBindMgr.GetBindPoint(this.Config.LaunchBindPoint);
        }
        else
        {
            transform2 = transform;
        }
        if (transform2 == null || transform == null)
        {
            this.mState = FlyObjHold.State.Over;
            return;
        }
        Quaternion rotation = Quaternion.LookRotation(transform.forward, Vector3.up);
        this.LaunchPos = transform2.position + rotation * this.Config.LaunchRelativePos;
        if (this.Control.mFFEffectCtrl != null)
        {
            this.FFeffectArr = this.Control.mFFEffectCtrl.AddEffect(this.Config.EffectList, this.Fobj.transform, delegate (GameObject obj)
            {
                if (obj != null)
                {
                    obj.SetActive(true);
                }
            }, true);
        }
    }

    private void Updatafly(bool first = false)
    {
        float num = this.RunningTime / this.flyTime;
        if (this.Config.mFlyTrackType == FlyObjConfig.FlyTrackType.Curve && this.Config.CurveSamlpeList.Length > 0)
        {
            Vector3 orgPos = this.mCurveFlyCtrl.Apply(num);
            Vector3 orgPos2 = this.mCurveFlyCtrl.TargetPos();
            if (this.Fobj != null)
            {
                this.Fobj.transform.position = this.ToTruePos(orgPos, this.LaunchPos, this.TrueTargetPos);
                this.Fobj.transform.LookAt(this.ToTruePos(orgPos2, this.LaunchPos, this.TrueTargetPos));
            }
        }
        else if (this.Fobj != null)
        {
            this.Fobj.transform.position = Vector3.Lerp(this.LaunchPos, this.TrueTargetPos, num);
            this.Fobj.transform.LookAt(this.TrueTargetPos);
        }
        if (first)
        {
            TrailRenderer[] componentsInChildren = this.Fobj.transform.GetComponentsInChildren<TrailRenderer>(true);
            for (int i = 0; i < componentsInChildren.Length; i++)
            {
                componentsInChildren[i].Clear();
            }
        }
    }

    private void InitSamlpe()
    {
        this.mCurveFlyCtrl = new CurveFlyCtrl();
        this.mCurveFlyCtrl.SetCurveSamlpe(this.Config.CurveSamlpeList);
    }

    public Vector3 ToTruePos(Vector3 OrgPos, Vector3 zeropos, Vector3 onepos)
    {
        Quaternion rotation = Quaternion.FromToRotation(Vector3.forward, (onepos - zeropos).normalized);
        float d = Vector3.Distance(zeropos, onepos);
        Vector3 point = OrgPos * d;
        return rotation * point + zeropos;
    }

    public void Despose()
    {
        if (this.FFeffectArr != null)
        {
            for (int i = 0; i < this.FFeffectArr.Length; i++)
            {
                this.FFeffectArr[i].mState = FFeffect.State.Over;
            }
        }
        if (this.Control.mFFEffectCtrl != null)
        {
            this.Control.mFFEffectCtrl.CompUpdate();
        }
        UnityEngine.Object.Destroy(this.Fobj);
    }

    private GameObject Fobj;

    public Vector3 TPos;

    public Transform TTran;

    public float StartTime;

    public float EndTime;

    public float flyTime;

    private Vector3 LaunchPos;

    public FlyObjConfig Config;

    public FlyObjControl Control;

    private float RunningTime;

    public FlyObjHold.State mState;

    private FFeffect[] FFeffectArr;

    private CurveFlyCtrl mCurveFlyCtrl;

    public enum State
    {
        none,
        Dalay,
        Play,
        Over
    }
}
