using System;
using System.Collections.Generic;
using Framework.Managers;
using UnityEngine;

public class FlyObjControl : IFFComponent
{
    public GameObject Root
    {
        get
        {
            return this.FFCompMgr.Owner.ModelObj;
        }
    }

    public FFBipBindMgr mBipBindMgr
    {
        get
        {
            return this.FFCompMgr.GetComponent<FFBipBindMgr>();
        }
    }

    public FFEffectControl mFFEffectCtrl
    {
        get
        {
            return this.FFCompMgr.GetComponent<FFEffectControl>();
        }
    }

    private FFEffectManager FFEfectMgr
    {
        get
        {
            return ManagerCenter.Instance.GetManager<FFEffectManager>();
        }
    }

    public void AddFlyObjArray(string[] names, Transform Target, FlyObjConfig.LaunchType CheckLaunchType)
    {
        if (names == null)
        {
            return;
        }
        for (int i = 0; i < names.Length; i++)
        {
            this.AddFlyObj(names[i], Target, CheckLaunchType);
        }
    }

    public FlyObjHold[] AddFlyObjArray(string[] names, Vector3 pos, FlyObjConfig.LaunchType CheckLaunchType)
    {
        if (names == null)
        {
            return null;
        }
        this.FlyObjHoldTmpList.Clear();
        for (int i = 0; i < names.Length; i++)
        {
            FlyObjHold flyObjHold = this.AddFlyObj(names[i], pos, CheckLaunchType);
            if (flyObjHold != null)
            {
                this.FlyObjHoldTmpList.Add(flyObjHold);
            }
        }
        return this.FlyObjHoldTmpList.ToArray();
    }

    public FlyObjHold AddFlyObj(string name, Transform Target, FlyObjConfig.LaunchType CheckLaunchType)
    {
        FlyObjConfig flyobjConfig = this.FFEfectMgr.GetFlyobjConfig(name);
        if (flyobjConfig == null)
        {
            return null;
        }
        if (flyobjConfig.mTargetType != FlyObjConfig.TargetType.TargetEntity)
        {
            return null;
        }
        if (flyobjConfig.mLaunchType != CheckLaunchType)
        {
            return null;
        }
        FlyObjHold flyObjHold = new FlyObjHold(flyobjConfig);
        flyObjHold.TTran = Target;
        this.AddFlyObj(flyObjHold);
        return flyObjHold;
    }

    public FlyObjHold AddFlyObj(string name, Vector3 pos, FlyObjConfig.LaunchType CheckLaunchType)
    {
        FFDebug.Log(this, FFLogType.Effect, "AddFlyObj-->" + name);
        FlyObjConfig flyobjConfig = this.FFEfectMgr.GetFlyobjConfig(name);
        if (flyobjConfig == null)
        {
            return null;
        }
        if (flyobjConfig.mTargetType != FlyObjConfig.TargetType.Position)
        {
            return null;
        }
        if (flyobjConfig.mLaunchType != CheckLaunchType)
        {
            return null;
        }
        FlyObjHold flyObjHold = new FlyObjHold(flyobjConfig);
        flyObjHold.TPos = pos;
        this.AddFlyObj(flyObjHold);
        return flyObjHold;
    }

    public void AddFlyObj(FlyObjHold hold)
    {
        hold.Control = this;
        hold.mState = FlyObjHold.State.Dalay;
        this.FlyobjHoldList.Add(hold);
    }

    public void DisposeFlyObjHold(FlyObjHold[] FlyObjArray)
    {
        if (FlyObjArray == null)
        {
            return;
        }
        for (int i = 0; i < FlyObjArray.Length; i++)
        {
            FlyObjArray[i].mState = FlyObjHold.State.Over;
        }
    }

    public CompnentState State { get; set; }

    public void CompAwake(FFComponentMgr Mgr)
    {
        this.FFCompMgr = Mgr;
    }

    public void CompUpdate()
    {
        this.Tmp.Clear();
        for (int i = 0; i < this.FlyobjHoldList.Count; i++)
        {
            FlyObjHold flyObjHold = this.FlyobjHoldList[i];
            flyObjHold.Updata();
            if (flyObjHold.mState == FlyObjHold.State.Over)
            {
                this.Tmp.Add(flyObjHold);
            }
        }
        for (int j = 0; j < this.Tmp.Count; j++)
        {
            FlyObjHold flyObjHold2 = this.Tmp[j];
            flyObjHold2.Despose();
            this.FlyobjHoldList.Remove(flyObjHold2);
        }
    }

    public void CompDispose()
    {
        for (int i = 0; i < this.FlyobjHoldList.Count; i++)
        {
            this.FlyobjHoldList[i].Despose();
        }
        this.FlyobjHoldList.Clear();
    }

    public void ResetComp()
    {
    }

    private List<FlyObjHold> FlyobjHoldList = new List<FlyObjHold>();

    private List<FlyObjHold> FlyObjHoldTmpList = new List<FlyObjHold>();

    private FFComponentMgr FFCompMgr;

    private List<FlyObjHold> Tmp = new List<FlyObjHold>();
}
