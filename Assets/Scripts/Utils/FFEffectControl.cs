using System;
using System.Collections.Generic;
using Framework.Managers;
using UnityEngine;

public class FFEffectControl : IFFComponent
{
    public GameObject Root
    {
        get
        {
            return this.FFCompMgr.Owner.ModelObj;
        }
    }

    public CompnentState State { get; set; }

    public FFBipBindMgr mBipBindMgr
    {
        get
        {
            return this.FFCompMgr.GetComponent<FFBipBindMgr>();
        }
    }

    public BipBindDataMgr mBipBindDataMgr
    {
        get
        {
            return ManagerCenter.Instance.GetManager<BipBindDataMgr>();
        }
    }

    public FFEffectManager EffectMgr
    {
        get
        {
            if (this._effectMgr == null)
            {
                this._effectMgr = ManagerCenter.Instance.GetManager<FFEffectManager>();
            }
            return this._effectMgr;
        }
    }

    private void AddEffect(FFeffect eff, Transform parentTran = null)
    {
        if (eff.Clip.IsCameraEffect && this.FFCompMgr.Owner != MainPlayer.Self)
        {
            eff.Despose();
            return;
        }
        eff.parentTran = parentTran;
        if (eff.effobj != null && parentTran != null)
        {
            eff.effobj.transform.SetParent(parentTran);
            if (eff.Clip.IsPointPosition)
            {
                eff.effobj.transform.Reset();
            }
            else
            {
                eff.effobj.transform.localPosition = eff.Clip.Position;
                eff.effobj.transform.localEulerAngles = eff.Clip.Rotation;
            }
        }
        eff.mState = FFeffect.State.Dalay;
        eff.RunningTime = 0f;
        this.FFeffectlist.Add(eff);
    }

    public FFeffect AddEffect(string effname, Transform parentTran = null, Action<GameObject> onCreateObj = null)
    {
        EffectClip clip = this.EffectMgr.GetClip(effname);
        if (!(clip != null))
        {
            return null;
        }
        if (clip.IsCameraEffect && this.FFCompMgr.Owner != MainPlayer.Self)
        {
            return null;
        }
        FFeffect @object = ClassPool.GetObject<FFeffect>();
        @object.Init(this, clip, delegate (GameObject go)
        {
            if (onCreateObj != null)
            {
                onCreateObj(go);
            }
        });
        this.AddEffect(@object, parentTran);
        return @object;
    }

    public FFeffect[] AddEffect(string[] EffectNameArray, Transform parentTran = null, Action<GameObject> onCreateObj = null, bool isFlyObj = false)
    {
        if (EffectNameArray == null)
        {
            return null;
        }
        this.TmpList.Clear();
        foreach (string effname in EffectNameArray)
        {
            FFeffect ffeffect = this.AddEffect(effname, parentTran, onCreateObj);
            if (ffeffect != null)
            {
                ffeffect.isFlyObj = isFlyObj;
                this.TmpList.Add(ffeffect);
            }
        }
        return this.TmpList.ToArray();
    }

    public FFeffect[] AddEffect(string[] EffectNameArray, Vector3 targetpos)
    {
        if (EffectNameArray == null)
        {
            return null;
        }
        this.TmpList.Clear();
        foreach (string effname in EffectNameArray)
        {
            FFeffect ffeffect = this.AddEffect(effname, null, null);
            ffeffect.SetTargetPos(targetpos);
            if (ffeffect != null)
            {
                this.TmpList.Add(ffeffect);
            }
        }
        return this.TmpList.ToArray();
    }

    public void AddEffectGroup(string groupName)
    {
        if (this.EffectGroupMap.ContainsKey(groupName))
        {
            FFDebug.LogWarning(this, "EffectGroupMap Has This  group :" + groupName);
            return;
        }
        FFDebug.Log(this, FFLogType.Effect, "AddEffectGroup :" + groupName);
        List<FFeffect> list = new List<FFeffect>();
        this.EffectGroupMap[groupName] = list;
        string[] group = this.EffectMgr.GetGroup(groupName);
        for (int i = 0; i < group.Length; i++)
        {
            EffectClip clip = this.EffectMgr.GetClip(group[i]);
            if (clip != null)
            {
                FFeffect ffeffect = this.AddEffect(group[i], null, null);
                if (ffeffect != null)
                {
                    list.Add(ffeffect);
                }
            }
        }
    }

    public void RotateEffectGroup(string groupname, Quaternion rotation)
    {
        if (!this.EffectGroupMap.ContainsKey(groupname))
        {
            return;
        }
        List<FFeffect> list = this.EffectGroupMap[groupname];
        for (int i = 0; i < list.Count; i++)
        {
            list[i].RotateEffect(rotation);
        }
    }

    public void AddEffectGroupOnce(string groupName)
    {
        List<FFeffect> list = new List<FFeffect>();
        string[] group = this.EffectMgr.GetGroup(groupName);
        for (int i = 0; i < group.Length; i++)
        {
            this.AddEffect(group[i], null, null);
        }
    }

    public void RemoveEffectGroup(string groupName)
    {
        if (!this.EffectGroupMap.ContainsKey(groupName))
        {
            return;
        }
        List<FFeffect> list = this.EffectGroupMap[groupName];
        for (int i = 0; i < list.Count; i++)
        {
            list[i].mState = FFeffect.State.Over;
        }
        list.Clear();
        this.EffectGroupMap.Remove(groupName);
    }

    public void SetGameCameraEnabled(bool enabled)
    {
        if (this.FFCompMgr.Owner != MainPlayer.Self)
        {
            return;
        }
        if (CameraController.Self == null)
        {
            return;
        }
        Camera component = CameraController.Self.GetComponent<Camera>();
        if (component != null)
        {
            component.enabled = enabled;
        }
    }

    public void SetEffectOver(string effname)
    {
        EffectClip clip = this.EffectMgr.GetClip(effname);
        if (clip != null)
        {
            if (clip.IsCameraEffect && this.FFCompMgr.Owner != MainPlayer.Self)
            {
                return;
            }
            FFeffect @object = ClassPool.GetObject<FFeffect>();
            for (int i = 0; i < this.FFeffectlist.Count; i++)
            {
                if (this.FFeffectlist[i].Clip.EffectName == effname)
                {
                    this.FFeffectlist[i].SetEffectOver();
                }
            }
        }
    }

    public void SetAllEffectOver()
    {
        if (this.FFCompMgr.Owner != MainPlayer.Self)
        {
            return;
        }
        for (int i = 0; i < this.FFeffectlist.Count; i++)
        {
            this.FFeffectlist[i].SetEffectOver();
        }
    }

    public void CompAwake(FFComponentMgr Mgr)
    {
        this.FFCompMgr = Mgr;
    }

    public void CompUpdate()
    {
        this.RemoveTmp.Clear();
        for (int i = 0; i < this.FFeffectlist.Count; i++)
        {
            FFeffect ffeffect = this.FFeffectlist[i];
            ffeffect.update();
            if (ffeffect.mState == FFeffect.State.Over)
            {
                this.RemoveTmp.Add(ffeffect);
            }
        }
        for (int j = 0; j < this.RemoveTmp.Count; j++)
        {
            FFeffect ffeffect2 = this.RemoveTmp[j];
            ffeffect2.Despose();
            this.FFeffectlist.Remove(ffeffect2);
        }
    }

    public FFeffect GetEeffectByName(string effectName = "")
    {
        if (!string.IsNullOrEmpty(effectName))
        {
            for (int i = 0; i < this.FFeffectlist.Count; i++)
            {
                if (this.FFeffectlist[i].Clip.ClipName.Equals(effectName))
                {
                    return this.FFeffectlist[i];
                }
            }
            return null;
        }
        if (this.FFeffectlist.Count > 0)
        {
            return this.FFeffectlist[this.FFeffectlist.Count - 1];
        }
        return null;
    }

    public void CompDispose()
    {
        for (int i = 0; i < this.FFeffectlist.Count; i++)
        {
            this.FFeffectlist[i].Despose();
        }
        this.FFeffectlist.Clear();
        this.EffectGroupMap.Clear();
        if (this.FFCompMgr != null && this.FFCompMgr.Owner != null && this.FFCompMgr.Owner.EID.Etype == CharactorType.NPC)
        {
            this.EffectMgr.RemoveObjectPoolUnit(this.FFCompMgr.Owner.EID.Id, string.Empty, RemovePoolUnitType.TEMPID, 0UL);
        }
    }

    public void PlayAudio(Transform tran, string BindName, float runningtime)
    {
    }

    public void SetTarget(string Tag, Vector3 pos, GameObject target)
    {
        for (int i = 0; i < this.FFeffectlist.Count; i++)
        {
            FFeffect ffeffect = this.FFeffectlist[i];
            ffeffect.SetTarget(Tag, pos, target);
        }
    }

    public void ResetComp()
    {
        for (int i = 0; i < this.FFeffectlist.Count; i++)
        {
            this.FFeffectlist[i].ResetEffectBindPoint();
        }
    }

    private FFEffectManager _effectMgr;

    private List<FFeffect> TmpList = new List<FFeffect>();

    private List<FFeffect> FFeffectlist = new List<FFeffect>();

    private Dictionary<string, List<FFeffect>> EffectGroupMap = new Dictionary<string, List<FFeffect>>();

    public FFComponentMgr FFCompMgr;

    private List<FFeffect> RemoveTmp = new List<FFeffect>();
}
