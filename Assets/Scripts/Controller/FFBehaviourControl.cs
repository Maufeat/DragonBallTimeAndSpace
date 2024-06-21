using System;
using System.Collections.Generic;
using Framework.Managers;
using UnityEngine;

public class FFBehaviourControl : StateMachine, IFFComponent
{
    public Animator m_animator
    {
        get
        {
            return this.Owmner.animator;
        }
    }

    public CompnentState State { get; set; }

    public string CurrClipName
    {
        get
        {
            return this._currClipName;
        }
    }

    public bool LockState { get; set; }

    public void PlayAnim(string clipName, float transitionDuration = 0f, float normalizedTime = 0f)
    {
        this._currClipName = clipName;
        if (this.m_animator != null)
        {
            try
            {
                if (this.allAnimationClip.ContainsKey(this._currClipName))
                {
                    float animationClipTime = ManagerCenter.Instance.GetManager<AnimatorControllerMgr>().GetAnimationClipTime(this.Owmner.animatorControllerName, this._currClipName);
                    this.m_animator.CrossFade(this._currClipName, transitionDuration, 0, normalizedTime / animationClipTime);
                }
                else
                {
                    FFDebug.LogWarning(this, "Get animationClip error!!! clipName : " + this._currClipName + " animatorControllerName : " + this.Owmner.animatorControllerName);
                }
            }
            catch (Exception ex)
            {
                FFDebug.LogError(this, string.Concat(new object[]
                {
                    this.Owmner.EID.ToString(),
                    "PlayAnim :",
                    clipName,
                    " Error :",
                    ex
                }));
            }
        }
    }

    public void GetHit(CharactorBase att)
    {
        if (base.CurrState is HandleNormalHit && att != null)
        {
            HandleNormalHit handleNormalHit = base.CurrState as HandleNormalHit;
            handleNormalHit.HandleNormalHit(att);
        }
    }

    public void EnterRun()
    {
        if (base.CurrState is FFBehaviourState_Walk)
        {
            (base.CurrState as FFBehaviourState_Walk).EnterRun();
        }
    }

    public void ExitRun()
    {
        if (base.CurrState is FFBehaviourState_Walk)
        {
            (base.CurrState as FFBehaviourState_Walk).ExitRun();
        }
    }

    public float PlayNormalAction(uint actionid, bool force = false, float transitionDuration = 0.1f)
    {
        int selectindex = (!this.Owmner.IsFly) ? 0 : 1;
        FFActionClip ffactionClip = ManagerCenter.Instance.GetManager<FFActionClipManager>().GetFFActionClip(this.Owmner.animatorControllerName, actionid, selectindex);
        if (ffactionClip != null)
        {
            return this.PlayNormalActionAndEffect(actionid, ffactionClip, force, transitionDuration);
        }
        return -1f;
    }

    public float GetActionTime(uint actionid)
    {
        int selectindex = (!this.Owmner.IsFly) ? 0 : 1;
        FFActionClip ffactionClip = ManagerCenter.Instance.GetManager<FFActionClipManager>().GetFFActionClip(this.Owmner.animatorControllerName, actionid, selectindex);
        if (ffactionClip != null)
        {
            return ManagerCenter.Instance.GetManager<AnimatorControllerMgr>().GetAnimationClipTime(ffactionClip.ACName, ffactionClip.ClipName);
        }
        return -1f;
    }

    private float PlayNormalActionAndEffect(uint actionid, FFActionClip clip, bool force = false, float transitionDuration = 0.1f)
    {
        float result = -1f;
        if (clip.ACName != "none" && this.m_animator == null)
        {
            return result;
        }
        if (clip != null)
        {
            if (this.CurrClipName != clip.ClipName || force)
            {
                this.PlayAnim(clip.ClipName, transitionDuration, 0f);
                result = ManagerCenter.Instance.GetManager<AnimatorControllerMgr>().GetAnimationClipTime(clip.ACName, clip.ClipName);
                this.playEffect(actionid, clip);
            }
        }
        else
        {
            result = this.PlayNormalAction(1U, false, 0.1f);
        }
        return result;
    }

    private void playEffect(uint actionid, FFActionClip clip)
    {
        FFEffectControl component = this.Owmner.ComponentMgr.GetComponent<FFEffectControl>();
        if (component == null)
        {
            return;
        }
        FFeffect[] array = component.AddEffect(clip.GetEffectsByGroupID(FFActionClip.EffectType.Type_Skill, 1U), null, null, false);
        if (array == null)
        {
            return;
        }
        if (this.ClipAndFFeffectlist.ContainsKey(actionid))
        {
            return;
        }
        if (!this.ClipAndFFeffectlist.ContainsKey(actionid))
        {
            this.ClipAndFFeffectlist[actionid] = new List<FFeffect>();
        }
        if (this.ClipAndFFeffectlist[actionid] == null)
        {
            this.ClipAndFFeffectlist[actionid] = new List<FFeffect>();
        }
        this.ClipAndFFeffectlist[actionid].AddRange(array);
    }

    public string[] GetCurrentAllEffectName()
    {
        List<string> lstResult = new List<string>();
        this.ClipAndFFeffectlist.BetterForeach(delegate (KeyValuePair<uint, List<FFeffect>> item)
        {
            for (int i = 0; i < item.Value.Count; i++)
            {
                lstResult.Add(item.Value[i].Clip.EffectName);
            }
        });
        return lstResult.ToArray();
    }

    public void DisPoseEffect(uint actionid)
    {
        if (!this.ClipAndFFeffectlist.ContainsKey(actionid))
        {
            return;
        }
        for (int i = 0; i < this.ClipAndFFeffectlist[actionid].Count; i++)
        {
            FFeffect ffeffect = this.ClipAndFFeffectlist[actionid][i];
            if (!ffeffect.IsDirty)
            {
                if (ffeffect.mState != FFeffect.State.Play || ffeffect.Clip.IsInfinite)
                {
                    ffeffect.mState = FFeffect.State.Over;
                }
            }
        }
        if (this.ClipAndFFeffectlist.ContainsKey(actionid))
        {
            this.ClipAndFFeffectlist.Remove(actionid);
        }
    }

    public float PlayRandomAction(uint actionid)
    {
        float result = 0f;
        FFActionClip[] ffactionClipArr = ManagerCenter.Instance.GetManager<FFActionClipManager>().GetFFActionClipArr(this.Owmner.animatorControllerName, actionid);
        if (ffactionClipArr != null && ffactionClipArr.Length > 0)
        {
            result = this.PlayNormalActionAndEffect(actionid, ffactionClipArr.RandomGetOne<FFActionClip>(), false, 0.1f);
        }
        return result;
    }

    public void CompAwake(FFComponentMgr Mgr)
    {
        this.Owmner = Mgr.Owner;
        if (this.m_animator != null && this.m_animator.runtimeAnimatorController != null)
        {
            AnimationClip[] animationClips = this.m_animator.runtimeAnimatorController.animationClips;
            for (int i = 0; i < animationClips.Length; i++)
            {
                this.allAnimationClip[animationClips[i].name] = animationClips[i];
            }
        }
    }

    public void ResetComp()
    {
        if (this.m_animator != null && this.m_animator.runtimeAnimatorController != null)
        {
            AnimationClip[] animationClips = this.m_animator.runtimeAnimatorController.animationClips;
            for (int i = 0; i < animationClips.Length; i++)
            {
                this.allAnimationClip[animationClips[i].name] = animationClips[i];
            }
        }
        if (base.CurrState is FFBehaviourState_Idle)
        {
            base.CurrStatebyType<FFBehaviourState_Idle>().playIdle(true);
        }
        else if (base.CurrState is FFBehaviourState_Walk)
        {
            base.CurrStatebyType<FFBehaviourState_Walk>().PlayRunAnim();
        }
    }

    public void CompUpdate()
    {
        this.Update();
    }

    public void CompDispose()
    {
        this.ChangeState(null);
    }

    public CharactorBase Owmner;

    public Dictionary<string, AnimationClip> allAnimationClip = new Dictionary<string, AnimationClip>();

    private string _currClipName;

    private BetterDictionary<uint, List<FFeffect>> ClipAndFFeffectlist = new BetterDictionary<uint, List<FFeffect>>();
}

public interface HandleNormalHit
{
    void HandleNormalHit(CharactorBase From);
}
