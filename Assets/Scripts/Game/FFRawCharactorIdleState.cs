using System;
using System.Collections.Generic;
using Framework.Managers;

public class FFRawCharactorIdleState : IFFComponent
{
    public CompnentState State { get; set; }

    private FFActionClipManager clipMgr
    {
        get
        {
            return ManagerCenter.Instance.GetManager<FFActionClipManager>();
        }
    }

    private AnimatorControllerMgr animCtrlMgr
    {
        get
        {
            return ManagerCenter.Instance.GetManager<AnimatorControllerMgr>();
        }
    }

    public bool Setup(string modlAct)
    {
        this.firstinDelayTime = 1f;
        this.listIndex = 0;
        this.ListAttackClips.Clear();
        this.NormalIdleClip = this.clipMgr.GetFFActionClip(this.Owner.animatorControllerName, 1U, 0);
        string[] array = modlAct.Split(new char[]
        {
            '|'
        }, StringSplitOptions.RemoveEmptyEntries);
        for (int i = 0; i < array.Length; i++)
        {
            uint actionId = uint.Parse(array[i]);
            FFActionClip ffactionClip = this.clipMgr.GetFFActionClip(this.Owner.animatorControllerName, actionId, 0);
            if (ffactionClip != null)
            {
                this.ListAttackClips.Add(ffactionClip);
            }
        }
        this.playState = FFRawCharactorIdleState.PlayAnimState.Normal;
        this.PlayAnim(this.NormalIdleClip, 0f, 0f);
        if (this.NormalIdleClip == null)
        {
            this.Owner.ComponentMgr.RemoveComponent(this);
            return false;
        }
        return true;
    }

    public void PlayNormalIdle()
    {
        if (this.playState != FFRawCharactorIdleState.PlayAnimState.Normal)
        {
            this.PlayAnim(this.NormalIdleClip, 0f, 0f);
            this.playState = FFRawCharactorIdleState.PlayAnimState.Normal;
        }
    }

    public void PlayAttackClips()
    {
        if (this.ListAttackClips.Count == this.listIndex)
        {
            this.PlayNormalIdle();
            this.listIndex = 0;
            return;
        }
        this.playState = FFRawCharactorIdleState.PlayAnimState.Attack;
        this.fOnceAniTime = this.PlayAnim(this.ListAttackClips[this.listIndex++], 0f, 0f);
    }

    private float PlayAnim(FFActionClip clip, float transitionDuration = 0f, float normalizedTime = 0f)
    {
        float result = 0f;
        try
        {
            if (clip != null)
            {
                this.Owner.animator.CrossFade(clip.ClipName, transitionDuration, 0, normalizedTime);
                result = this.animCtrlMgr.GetAnimationClipTime(clip.ACName, clip.ClipName);
            }
        }
        catch (Exception ex)
        {
            FFDebug.LogError(this, string.Concat(new object[]
            {
                this.Owner.EID.ToString(),
                "PlayAnim :",
                clip.ClipName,
                " Error :",
                ex
            }));
        }
        return result;
    }

    public void CompAwake(FFComponentMgr Mgr)
    {
        this.Owner = Mgr.Owner;
        this.fDeltaTime = LuaConfigManager.GetXmlConfigTable("massiveConfig").GetCacheField_Table("ModelActInterval").GetCacheField_Float("value");
    }

    public void CompUpdate()
    {
        if (this.playState == FFRawCharactorIdleState.PlayAnimState.Normal)
        {
            this.firstinDelayTime -= Scheduler.Instance.realDeltaTime;
            if (this.firstinDelayTime <= 0f)
            {
                this.PlayAttackClips();
                this.firstinDelayTime = this.fDeltaTime;
            }
        }
        if (this.playState == FFRawCharactorIdleState.PlayAnimState.Attack)
        {
            this.fOnceAniTime -= Scheduler.Instance.realDeltaTime;
            if (this.fOnceAniTime <= 0f)
            {
                this.PlayAttackClips();
            }
        }
    }

    public void CompDispose()
    {
    }

    public void ResetComp()
    {
    }

    private FFActionClip NormalIdleClip;

    private List<FFActionClip> ListAttackClips = new List<FFActionClip>();

    private int listIndex;

    private FFRawCharactorIdleState.PlayAnimState playState;

    private float firstinDelayTime = 1f;

    private float fDeltaTime;

    private float fOnceAniTime;

    public CharactorBase Owner;

    public enum PlayAnimState
    {
        Normal,
        Attack
    }
}
