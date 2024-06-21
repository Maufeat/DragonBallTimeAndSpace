using System;
using System.Collections.Generic;
using AudioStudio;
using map;
using UnityEngine;

public class FFBehaviourState_Walk : FFBehaviourBaseState, IstorebAble, HandleNormalHit
{
    public FFBehaviourState_Walk()
    {
        if (FFBehaviourState_Walk.SoundFlagList == null)
        {
            FFBehaviourState_Walk.SoundFlagList = new List<string>();
            Array values = Enum.GetValues(typeof(SoundFlag));
            for (int i = 0; i < values.Length; i++)
            {
                FFBehaviourState_Walk.SoundFlagList.Add(values.GetValue(i).ToString());
            }
        }
    }

    public override void OnEnter(StateMachine parent)
    {
        base.OnEnter(parent);
        if (this.Parent.Owmner is MainPlayer)
        {
            MainPlayerSkillHolder.Instance.OnBreak(CSkillBreakType.Move);
        }
        SingletonForMono<InputController>.Instance.ReSet();
        this.PlayRunAnim();
        this.onWalkAnim = true;
    }

    public float PlayRunAnim()
    {
        uint num = this.Parent.Owmner.GetComponent<PlayerBufferControl>().getBufferAnimActionID(BufferAnimtype.BufferAnimtype_WalkOrRun);
        if (num == 0U)
        {
            num = this.Parent.Owmner.GetComponent<PlayerBufferControl>().getBufferAnimActionID(BufferAnimtype.BufferAnimtype_WalkOrRun2);
        }
        if (num == 0U)
        {
            num = 4U;
        }
        float actionTime = this.Parent.GetActionTime(num);
        if (actionTime < 0f)
        {
            num = 4U;
        }
        this.walkAnimLength = actionTime / 2f;
        this.walkAnimTime = 0f;
        return this.Parent.PlayNormalAction(num, true, 0.1f);
    }

    public void EnterRun()
    {
        this.PlayRunAnim();
    }

    public void ExitRun()
    {
        this.Parent.PlayNormalAction(4U, false, 0.1f);
        this.onWalkAnim = false;
    }

    public override void OnUpdate(StateMachine parent)
    {
        base.OnUpdate(parent);
        this.UpdateNormalHit();
        this.UpdateWalkSound();
    }

    public void UpdateWalkSound()
    {
        if (!this.onWalkAnim)
        {
            return;
        }
        this.walkAnimTime += Time.deltaTime;
        if (this.walkAnimTime >= this.walkAnimLength)
        {
            this.walkAnimTime = 0f;
            if (null == this.animSound)
            {
                this.animSound = this.Parent.Owmner.ModelObj.GetComponent<AnimationSound>();
            }
            if (null != this.animSound)
            {
                Vector2 serverPosByWorldPos_new = GraphUtils.GetServerPosByWorldPos_new(this.Parent.Owmner.ModelObj.transform.position);
                byte b;
                if (MapLoader.soundInfo == null)
                {
                    b = 0;
                }
                else if (MapLoader.soundInfo.isSame)
                {
                    b = MapLoader.soundInfo.nodes[0];
                }
                else
                {
                    float f = serverPosByWorldPos_new.x / 3f;
                    float f2 = serverPosByWorldPos_new.y / 3f;
                    int w = Mathf.FloorToInt(f);
                    int h = Mathf.FloorToInt(f2);
                    b = MapLoader.soundInfo.GetValueByIndex(w, h);
                }
                if (FFBehaviourState_Walk.SoundFlagList.Count > (int)b)
                {
                    this.animSound.PostStep(FFBehaviourState_Walk.SoundFlagList[(int)b], "Footsteps");
                }
            }
        }
    }

    public override void OnExit(StateMachine parent)
    {
        SingletonForMono<InputController>.Instance.ReSet();
        base.OnExit(parent);
        this.Parent.DisPoseEffect(4U);
        this.Parent.DisPoseEffect(6U);
        this.StoreToPool();
    }

    public void HandleNormalHit(CharactorBase From)
    {
        this.onHitAnim = true;
        this.HitAnimLength = this.Parent.PlayNormalAction(7U, true, 0.1f);
        if (this.HitAnimLength <= 0f)
        {
            this.HitAnimLength = this.Parent.PlayNormalAction(4U, true, 0.1f);
        }
        this.HitAnimTime = 0f;
    }

    private void UpdateNormalHit()
    {
        if (!this.onHitAnim)
        {
            return;
        }
        this.HitAnimTime += Time.deltaTime;
        if (this.HitAnimTime >= this.HitAnimLength)
        {
            this.PlayRunAnim();
            this.onHitAnim = false;
            this.HitAnimTime = 0f;
        }
    }

    public bool IsDirty { get; set; }

    public void RestThisObject()
    {
        this.onHitAnim = false;
        this.HitAnimTime = 0f;
        this.HitAnimLength = 0f;
        base.RunningTime = 0f;
        this.animSound = null;
        this.Parent = null;
    }

    public void StoreToPool()
    {
        ClassPool.Store<FFBehaviourState_Walk>(this, 60);
    }

    private static List<string> SoundFlagList;

    private bool onHitAnim;

    private float HitAnimTime;

    private float HitAnimLength;

    private bool onWalkAnim;

    private float walkAnimTime;

    private float walkAnimLength;

    private AnimationSound animSound;
}
