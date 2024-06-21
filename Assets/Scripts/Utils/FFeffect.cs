using System;
using System.Collections.Generic;
using AudioStudio;
using Framework.Managers;
using UnityEngine;

public class FFeffect : IstorebAble
{
    public GameObject effobj
    {
        get
        {
            return this.effobj_;
        }
        set
        {
            this.effobj_ = value;
        }
    }

    public FFeffect.State mState
    {
        get
        {
            return this.mState_;
        }
        set
        {
            this.mState_ = value;
        }
    }

    public bool EffObjHasLoad
    {
        get
        {
            return this.effectObjHasLoad;
        }
    }

    private FFEffectManager effMgr
    {
        get
        {
            return ManagerCenter.Instance.GetManager<FFEffectManager>();
        }
    }

    private void Loadeffobj(Action<GameObject> onCreateObj)
    {
        this.effMgr.LoadEffobj(this.Clip.EffectName, delegate
        {
            ObjectPool<EffectObjInPool> effobj = this.effMgr.GetEffobj(this.Clip.EffectName);
            if (effobj != null)
            {
                effobj.GetItemFromPool(delegate (EffectObjInPool OIP)
                {
                    ulong tempid = 0UL;
                    if (this.Control != null && this.Control.FFCompMgr != null && this.Control.FFCompMgr.Owner != null)
                    {
                        tempid = this.Control.FFCompMgr.Owner.EID.Id;
                    }
                    this.effMgr.SetObjectPoolUnit(this.poolIndex, tempid, this.Clip.EffectName, OIP);
                    this.ObjInPool = OIP;
                    if (this.ObjInPool == null || this.ObjInPool.ItemObj == null)
                    {
                        return;
                    }
                    this.effobj = this.ObjInPool.ItemObj;
                    this.effobj.gameObject.SetActive(false);
                    EffectSound component = this.effobj.GetComponent<EffectSound>();
                    if (component != null && this.Control != null && this.Control.FFCompMgr != null)
                    {
                        component.SetEffectData(this.Control.FFCompMgr.Owner);
                    }
                    this.effobj.SetLayer((!this.Clip.IsCameraEffect) ? Const.Layer.Effect : Const.Layer.CameraEffect, true);
                    if (this.parentTran != null)
                    {
                        this.effobj.transform.SetParent(this.parentTran);
                        if (this.Clip.IsPointPosition)
                        {
                            this.effobj.transform.Reset();
                        }
                        else
                        {
                            this.effobj.transform.localPosition = this.Clip.Position;
                        }
                    }
                    this.effectObjHasLoad = true;
                    this.RunningTime = 0f;
                    if (onCreateObj != null)
                    {
                        onCreateObj(this.effobj);
                    }
                });
            }
            else
            {
                this.mState = FFeffect.State.Over;
            }
        });
    }

    private void getDlr()
    {
        if (this.effobj == null)
        {
            return;
        }
        DynamicLineRenderer[] componentsInChildren = this.effobj.GetComponentsInChildren<DynamicLineRenderer>(true);
        if (componentsInChildren != null && componentsInChildren.Length > 0)
        {
            this.DlrList = new List<DynamicLineRenderer>();
            foreach (DynamicLineRenderer dynamicLineRenderer in componentsInChildren)
            {
                if (!(dynamicLineRenderer == null))
                {
                    dynamicLineRenderer.gameObject.SetActive(false);
                    if (!this.DlrList.Contains(dynamicLineRenderer))
                    {
                        dynamicLineRenderer.Reset();
                        this.DlrList.Add(dynamicLineRenderer);
                    }
                }
            }
        }
    }

    private void GetHgt()
    {
        if (this.effobj == null)
        {
            return;
        }
        HighlighterEffectRenderer[] componentsInChildren = this.effobj.GetComponentsInChildren<HighlighterEffectRenderer>(true);
        if (componentsInChildren != null && componentsInChildren.Length > 0)
        {
            this.HgtList = new List<HighlighterEffectRenderer>();
            foreach (HighlighterEffectRenderer highlighterEffectRenderer in componentsInChildren)
            {
                if (!(highlighterEffectRenderer == null))
                {
                    if (!this.HgtList.Contains(highlighterEffectRenderer))
                    {
                        this.HgtList.Add(highlighterEffectRenderer);
                    }
                }
            }
        }
    }

    public void SetTargetPos(Vector3 _targetPos)
    {
        if (this.Clip.IsPointPosition)
        {
            this.targetPos = _targetPos;
        }
    }

    public void SetTarget(string Tag, Vector3 pos, GameObject target)
    {
        if (this.Tag != Tag)
        {
            return;
        }
        if (this.mState >= FFeffect.State.Play)
        {
            this.SetTargetafterInit(Tag, pos, target);
        }
        else
        {
            this.afterIniteffobj = delegate ()
            {
                this.SetTargetafterInit(Tag, pos, target);
            };
        }
    }

    private void SetTargetafterInit(string Tag, Vector3 pos, GameObject target)
    {
        if (this.Tag != Tag)
        {
            return;
        }
        if (this.DlrList != null && this.DlrList.Count != 0)
        {
            for (int i = 0; i < this.DlrList.Count; i++)
            {
                DynamicLineRenderer dynamicLineRenderer = this.DlrList[i];
                if (dynamicLineRenderer.targetType == FlyObjConfig.TargetType.Position)
                {
                    dynamicLineRenderer.gameObject.SetActive(true);
                    dynamicLineRenderer.StartPlay(pos);
                }
                else
                {
                    this.targetObj = target;
                }
            }
        }
        if (this.HgtList != null && this.HgtList.Count != 0)
        {
            List<string> allBipBindDataMap = this.Control.mBipBindDataMgr.GetAllBipBindDataMap();
            for (int j = 0; j < this.HgtList.Count; j++)
            {
                HighlighterEffectRenderer highlighterEffectRenderer = this.HgtList[j];
                if (highlighterEffectRenderer.m_targetType == HighlighterEffectRenderer.TargetType.Caster)
                {
                    highlighterEffectRenderer.SetHighterlighterState(MainPlayer.Self.ModelObj, allBipBindDataMap);
                }
                else if (highlighterEffectRenderer.m_targetType == HighlighterEffectRenderer.TargetType.BeHit)
                {
                    highlighterEffectRenderer.SetHighterlighterState(target, allBipBindDataMap);
                }
                else if (highlighterEffectRenderer.m_targetType == HighlighterEffectRenderer.TargetType.Itself)
                {
                    highlighterEffectRenderer.SetHighterlighterState(highlighterEffectRenderer.gameObject, true);
                }
                highlighterEffectRenderer.OnPlay();
            }
        }
    }

    public void SetTargetOnUpdate()
    {
        if (this.targetObj == null)
        {
            return;
        }
        if (this.DlrList != null && this.DlrList.Count != 0)
        {
            for (int i = 0; i < this.DlrList.Count; i++)
            {
                DynamicLineRenderer dynamicLineRenderer = this.DlrList[i];
                if (dynamicLineRenderer.targetType == FlyObjConfig.TargetType.TargetEntity)
                {
                    dynamicLineRenderer.gameObject.SetActive(true);
                    this.effobj.transform.LookAt(this.targetObj.transform);
                    dynamicLineRenderer.StartPlay(this.targetObj.transform.position);
                }
            }
        }
    }

    public void RotateEffect(Quaternion rotation)
    {
        if (this.effobj != null)
        {
            this.effobj.transform.rotation = rotation;
        }
    }

    public void update()
    {
        this.RunningTime += Time.deltaTime;
        if (this.mState == FFeffect.State.Dalay)
        {
            this.OnDalay();
        }
        else if (this.mState == FFeffect.State.Play)
        {
            this.OnPlay();
        }
        if (this.effobj != null)
        {
            if (this.mState == FFeffect.State.Dalay)
            {
                float num = this.StartTime - this.RunningTime;
                num = ((num >= 0f) ? 0f : num);
                this.Control.PlayAudio(this.effobj.transform, this.Clip.ClipName, num);
            }
            else
            {
                this.Control.PlayAudio(this.effobj.transform, this.Clip.ClipName, this.RunningTime);
            }
        }
    }

    public void SetEffectOver()
    {
        this.mState = FFeffect.State.Over;
        this.targetObj = null;
    }

    private void OnDalay()
    {
        if (this.RunningTime >= this.StartTime && this.effectObjHasLoad)
        {
            this.mState = FFeffect.State.Play;
            this.Initeffobj();
            this.RunningTime = 0f;
        }
    }

    private void OnPlay()
    {
        if (!this.isFlyObj)
        {
            this.SetTargetOnUpdate();
        }
        if (this.RunningTime >= this.EndTime && !this.Clip.IsInfinite)
        {
            if (this.effobj != null)
            {
                this.effobj.SetActive(false);
            }
            this.mState = FFeffect.State.Over;
        }
    }

    public void ResetEffectBindPoint()
    {
        Transform transform = this.parentTran;
        if (transform == null && this.Control != null)
        {
            if (this.Control.mBipBindMgr != null)
            {
                transform = this.Control.mBipBindMgr.GetBindPoint(this.Clip.BindPointName);
            }
            else
            {
                transform = this.Control.Root.transform;
            }
        }
        if (!(transform == null) && !(this.effobj == null))
        {
            Vector3 localPosition = this.effobj.transform.localPosition;
            Vector3 localEulerAngles = this.effobj.transform.localEulerAngles;
            Vector3 localScale = this.effobj.transform.localScale;
            this.effobj.transform.SetParent(transform);
            this.effobj.transform.localPosition = localPosition;
            this.effobj.transform.localEulerAngles = localEulerAngles;
            this.effobj.transform.localScale = localScale;
            return;
        }
        if (this.mState == FFeffect.State.Dalay)
        {
            Scheduler.Instance.AddFrame(1U, false, new Scheduler.OnScheduler(this.ResetEffectBindPoint));
            return;
        }
        this.mState = FFeffect.State.Over;
    }

    private void Initeffobj()
    {
        Transform transform = this.parentTran;
        if (transform == null)
        {
            if (this.Control.mBipBindMgr != null)
            {
                transform = this.Control.mBipBindMgr.GetBindPoint(this.Clip.BindPointName);
                if (transform == null)
                {
                }
            }
            else
            {
                transform = this.Control.Root.transform;
            }
        }
        if (transform == null || this.effobj == null)
        {
            this.mState = FFeffect.State.Over;
            return;
        }
        if (!this.isFlyObj)
        {
            if (this.Clip.IsPointPosition)
            {
                this.effobj.transform.SetParent(null);
                this.effobj.transform.position = this.targetPos;
            }
            else if (!this.Clip.IsCameraEffect)
            {
                this.effobj.transform.SetParent(transform);
                this.effobj.transform.localPosition = this.Clip.Position;
            }
        }
        this.effobj.transform.localEulerAngles = this.Clip.Rotation;
        this.effobj.transform.localScale = this.Clip.Scale;
        if (!this.Clip.IsBind)
        {
            this.effobj.transform.SetParent(null);
        }
        if (this.Clip.IsInfinite)
        {
            Animation[] componentsInChildren = this.effobj.transform.GetComponentsInChildren<Animation>();
            for (int i = 0; i < componentsInChildren.Length; i++)
            {
                componentsInChildren[i].wrapMode = WrapMode.Loop;
            }
        }
        this.effobj.SetActive(true);
        this.getDlr();
        this.GetHgt();
        if (this.afterIniteffobj != null)
        {
            this.afterIniteffobj();
            this.afterIniteffobj = null;
        }
        TrailRenderer[] componentsInChildren2 = this.effobj.transform.GetComponentsInChildren<TrailRenderer>(true);
        for (int j = 0; j < componentsInChildren2.Length; j++)
        {
            componentsInChildren2[j].Clear();
        }
    }

    public void Despose()
    {
        if (this.effobj != null)
        {
            if (this.effMgr != null)
            {
                this.effMgr.RemoveObjectPoolUnit(this.Control.FFCompMgr.Owner.EID.Id, this.Clip.EffectName, RemovePoolUnitType.IDANDNAME, this.poolIndex);
            }
        }
        else if (this.ObjInPool != null)
        {
            this.ObjInPool.DestroyThis();
        }
        else
        {
            UnityEngine.Object.Destroy(this.effobj);
        }
        this.effobj = null;
        this.ObjInPool = null;
        this.Control = null;
        this.parentTran = null;
        this.effectObjHasLoad = false;
        this.isFlyObj = false;
        this.StoreToPool();
    }

    public void RestThisObject()
    {
        this.Clip = null;
        this.StartTime = 0.1f;
        this.EndTime = 0.1f;
        this.parentTran = null;
        this.mState = FFeffect.State.none;
        this.effectObjHasLoad = false;
        this.RunningTime = 0f;
        this.isFlyObj = false;
    }

    public void Init(FFEffectControl ctrl, EffectClip clip, Action<GameObject> onCreateObj)
    {
        this.Control = ctrl;
        this.Clip = clip;
        this.StartTime = this.Clip.StartDalay / 30f;
        this.EndTime = this.Clip.Duration / 30f;
        if (this.Clip.IsInfinite)
        {
            this.EndTime = 999f;
        }
        this.parentTran = null;
        this.mState = FFeffect.State.none;
        this.effectObjHasLoad = false;
        this.poolIndex = this.effMgr.GetPoolIndex();
        this.Loadeffobj(onCreateObj);
    }

    public bool IsDirty { get; set; }

    public void StoreToPool()
    {
        ClassPool.Store<FFeffect>(this, 0);
    }

    public EffectClip Clip;

    public float StartTime;

    public float EndTime;

    public GameObject effobj_;

    public ulong poolIndex;

    public bool isFlyObj;

    public ObjectInPoolBase ObjInPool;

    public FFEffectControl Control;

    public Transform parentTran;

    private FFeffect.State mState_;

    private bool effectObjHasLoad;

    public float RunningTime;

    private List<DynamicLineRenderer> DlrList;

    private List<HighlighterEffectRenderer> HgtList;

    public string Tag = string.Empty;

    public GameObject targetObj;

    private Vector3 targetPos;

    private Action afterIniteffobj;

    public enum State
    {
        none,
        Dalay,
        Play,
        Over
    }
}
