using System;
using Framework.Managers;
using LuaInterface;
using map;
using UnityEngine;

public class AutoAttack : IFFComponent
{
    public float AttackRange
    {
        get
        {
            if (this.range > 0f)
            {
                return this.range;
            }
            if (this.CurAttackSkillBase != null)
            {
                string field_String = this.CurAttackSkillBase.SkillConfig.GetField_String("SkillRange");
                string[] array = field_String.Split(new char[]
                {
                    ':'
                });
                if (array.Length > 1)
                {
                    this.range = float.Parse(array[1]);
                }
            }
            return this.range;
        }
        set
        {
            this.range = value;
        }
    }

    public float AttackAngle
    {
        get
        {
            if (this.angle >= 0f)
            {
                return this.angle;
            }
            SkillViewControll controller = ControllerManager.Instance.GetController<SkillViewControll>();
            if (controller != null)
            {
                string field = string.Empty + MainPlayer.Self.GetOccupation();
                LuaTable cacheField_Table = LuaConfigManager.GetXmlConfigTable("autoattack").GetCacheField_Table("careerAI").GetCacheField_Table(field);
                if (cacheField_Table != null)
                {
                    this.angle = cacheField_Table.GetCacheField_Float("AttackAngle");
                }
            }
            return this.angle;
        }
    }

    public MainPlayerSkillBase CurAttackSkillBase
    {
        get
        {
            return this.curskill;
        }
    }

    public MainPlayerSkillHolder skillHolder
    {
        get
        {
            return MainPlayer.Self.GetComponent<MainPlayerSkillHolder>();
        }
    }

    private MainPlayerSkillBase GetCurAvailableSkill()
    {
        return this.skillHolder.GetCurAvailableSkill();
    }

    public virtual bool AutoAttackOn
    {
        get
        {
            return this._AutoAttackOn;
        }
        set
        {
            if (this._AutoAttackOn != value)
            {
                this._AutoAttackOn = value;
                if (this._AutoAttackOn)
                {
                    if (MainPlayer.Self.GetComponent<MainPlayerTargetSelectMgr>() != null)
                    {
                        MainPlayer.Self.GetComponent<MainPlayerTargetSelectMgr>().autoattackSelect.ReqTarget();
                    }
                }
                else
                {
                    MainPlayer.Self.StopMoveImmediate(null);
                }
            }
        }
    }

    public virtual void SwitchModle(bool isAutoBattle)
    {
        if (this.AutoAttackOn == isAutoBattle)
        {
            return;
        }
        this.AutoAttackOn = isAutoBattle;
        this.SetUI();
    }

    public void SetUI()
    {
        LuaScriptMgr.Instance.CallLuaFunction("UIMapCtrl.SetAutoBattleUI", new object[]
        {
            "UIMapCtrl",
            this.AutoAttackOn
        });
    }

    public CharactorBase Target
    {
        get
        {
            return MainPlayer.Self.GetComponent<MainPlayerTargetSelectMgr>().TargetCharactor;
        }
    }

    public virtual void MakeDecide()
    {
        if (this.IsOnPlayerControl())
        {
            return;
        }
        if (!MainPlayer.Self.GetComponent<MainPlayerTargetSelectMgr>().autoattackSelect.CheckLegal(this.Target, false))
        {
            return;
        }
        this.RefreshDisToTarget();
        this.RefreshCurSkill();
        if (this.CheckMoveToTarget())
        {
            return;
        }
        this.CheckMakeAttack();
    }

    public void RefreshCurSkill()
    {
        if (MainPlayer.Self.GetComponent<FFBehaviourControl>().CurrState is FFBehaviourState_Skill)
        {
            return;
        }
        this.curskill = null;
    }

    public bool IsOnPlayerControl()
    {
        FFBehaviourControl component = MainPlayer.Self.GetComponent<FFBehaviourControl>();
        FFBehaviourState_Skill ffbehaviourState_Skill = null;
        if (component != null)
        {
            ffbehaviourState_Skill = component.CurrStatebyType<FFBehaviourState_Skill>();
        }
        BreakAutoattackUIMgr component2 = MainPlayer.Self.GetComponent<BreakAutoattackUIMgr>();
        if (component2 != null && component2.IsBreakAutoAttack())
        {
            MainPlayer.Self.Pfc.EndPathFind(PathFindState.Break, true);
            this.NextthinkTime = 2f;
            return true;
        }
        if (SingletonForMono<InputController>.Instance.InputDir != -1)
        {
            MainPlayer.Self.Pfc.EndPathFind(PathFindState.Break, true);
            this.NextthinkTime = 0.4f;
            return true;
        }
        if (MainPlayer.Self.Pfc.isPathfinding)
        {
            if (MainPlayer.Self.Pfc.CurrAutoMoveState > PathFindComponent.AutoMoveState.MoveToAttackNpc)
            {
                this.NextthinkTime = 2f;
                return true;
            }
        }
        else if (ffbehaviourState_Skill != null && ffbehaviourState_Skill.CurrSkillClipState < SkillClip.State.Attack)
        {
            this.NextthinkTime = 0.05f;
            return true;
        }
        return false;
    }

    public void CheckMakeAttack()
    {
        this.MakeAttack();
        this.NextthinkTime = 0.1f;
    }

    private void MakeAttack()
    {
        if (MainPlayer.Self.GetComponent<FFBehaviourControl>().CurrState is FFBehaviourState_Skill)
        {
            return;
        }
        if (this.CurAttackSkillBase == null)
        {
            return;
        }
        if (this.CurAttackSkillBase.Skillid != 0U)
        {
            this.AutoClickSkill(this.CurAttackSkillBase);
            MainPlayer.Self.Pfc.EndPathFind(PathFindState.Complete, true);
        }
        this.HasMakeAttack = true;
    }

    public void AutoClickSkill(MainPlayerSkillBase skillData)
    {
        if (UIManager.GetUIObject<UI_CompleteCopy>())
        {
            return;
        }
        if (MainPlayer.Self == null)
        {
            FFDebug.LogWarning(this, "MainPlayer.Self  NULL");
            return;
        }
        if (skillData == null)
        {
            FFDebug.LogWarning(this, "SkillBTN  SkillConfig  NULL");
            return;
        }
        SkillSelectRangeEffect component = MainPlayer.Self.GetComponent<SkillSelectRangeEffect>();
        if (component == null)
        {
            FFDebug.LogError(this, "SkillBTN  SkillSelectRangeEffect  NULL");
            return;
        }
        Vector3 vector = new Vector3(0f, -50f, 0f);
        if (skillData.mSightType == MainPlayerSkillBase.SightType.RotateCamera || skillData.mSightType == MainPlayerSkillBase.SightType.Click)
        {
            MainPlayerSkillHolder component2 = MainPlayer.Self.GetComponent<MainPlayerSkillHolder>();
            if (component2.mainPlayerEquipSkill.Count > 0 && skillData.Skillid == component2.mainPlayerEquipSkill[1U])
            {
                component2.ClickNormalAttack(skillData.Skillid, false);
            }
            else
            {
                component2.ClickSkillAttack(skillData.Skillid, false);
            }
        }
        else if (skillData.mSightType == MainPlayerSkillBase.SightType.Sector)
        {
            try
            {
                if (MainPlayer.Self != null && MainPlayer.Self.ModelObj != null)
                {
                    vector = MainPlayer.Self.ModelObj.transform.forward * 0.19f;
                    this.GetDireByTargeet(skillData.Sightradius, ref vector);
                }
                component.MoveSector(vector, skillData.Sightradius, skillData.Sightangle, skillData.Sighttex1name, skillData.Sighttex2name, skillData.Sighttex3name);
                component.CompUpdate();
            }
            catch (Exception arg)
            {
                FFDebug.LogError(this, "MoveSector error:" + arg);
            }
            MainPlayer.Self.GetComponent<MainPlayerSkillHolder>().ClickSkillEvent(skillData.Skillid);
        }
        else if (skillData.mSightType == MainPlayerSkillBase.SightType.Circle)
        {
            try
            {
                vector = MainPlayer.Self.ModelObj.transform.forward * skillData.Sightsize;
                this.GetDireByTargeet(skillData.Sightsize, ref vector);
                Vector3 pos = MainPlayer.Self.ModelObj.transform.position + vector;
                component.MoveCircle(pos, skillData.Sightradius, skillData.Sightsize, skillData.Sighttex1name, skillData.Sighttex2name);
                component.CompUpdate();
            }
            catch (Exception arg2)
            {
                FFDebug.LogError(this, "MoveCircle error:" + arg2);
            }
            MainPlayer.Self.GetComponent<MainPlayerSkillHolder>().ClickSkillEvent(skillData.Skillid);
        }
        else if (skillData.mSightType == MainPlayerSkillBase.SightType.Rectangle)
        {
            try
            {
                if (MainPlayer.Self != null && MainPlayer.Self.ModelObj != null)
                {
                    vector = MainPlayer.Self.ModelObj.transform.forward * 0.19f;
                    this.GetDireByTargeet(skillData.Sightsize, ref vector);
                }
                component.MoveRectangle(vector, skillData.Sightsize, skillData.Sightradius, skillData.Sighttex1name);
                component.CompUpdate();
            }
            catch (Exception arg3)
            {
                FFDebug.LogError(this, "MoveSector error:" + arg3);
            }
            MainPlayer.Self.GetComponent<MainPlayerSkillHolder>().ClickSkillEvent(skillData.Skillid);
        }
        component.HidleALL();
    }

    private void GetDireByTargeet(float range, ref Vector3 Dire)
    {
        if (MainPlayer.Self != null)
        {
            MainPlayerTargetSelectMgr component = MainPlayer.Self.GetComponent<MainPlayerTargetSelectMgr>();
            if (component != null && component.TargetCharactor != null && component.TargetCharactor.ModelObj != null)
            {
                Vector3 vector = CommonTools.DismissYSize(component.TargetCharactor.ModelObj.transform.position);
                Vector3 vector2 = CommonTools.DismissYSize(MainPlayer.Self.ModelObj.transform.position);
                float num = Vector3.Distance(vector2, vector);
                if (num < range)
                {
                    Dire = vector - vector2;
                }
            }
        }
    }

    public void RefreshDisToTarget()
    {
        this.Angle = Vector3.Angle((CommonTools.DismissYSize(this.Target.ModelObj.transform.position) - CommonTools.DismissYSize(MainPlayer.Self.ModelObj.transform.position)).normalized, CommonTools.DismissYSize(MainPlayer.Self.ModelObj.transform.forward));
        this.Distance = Vector2.Distance(this.Target.NextPosition2D, MainPlayer.Self.CurrentPosition2D);
    }

    public bool CheckMoveToTarget()
    {
        float num = 0f;
        if (this.Target is Npc && this.Target is Npc)
        {
            num = LuaConfigManager.GetConfigTable("npc_data", (ulong)(this.Target as Npc).NpcData.MapNpcData.baseid).GetField_Uint("volume");
        }
        if (this.AttackRange > 0f && this.Distance >= this.AttackRange + num)
        {
            if (MainPlayer.Self.Pfc.CurrAutoMoveState != PathFindComponent.AutoMoveState.Off && Vector2.Distance(this.NextPosition2DOfTargetFinder, this.Target.NextPosition2D) < 12f)
            {
                this.NextthinkTime = 0.1f;
                return true;
            }
            this.NextthinkTime = 0.1f;
            MainPlayer.Self.Pfc.BeginFindPath(this.Target.NextPosition2D, PathFindComponent.AutoMoveState.MoveToAttackNpc, this.AttackRange, delegate ()
            {
                if (MainPlayer.Self.Pfc.FindState != PathFindState.Complete)
                {
                    this.NextthinkTime = 5f;
                }
                this.NextPosition2DOfTargetFinder = Vector2.zero;
            }, null, 0f);
            this.NextPosition2DOfTargetFinder = this.Target.NextPosition2D;
            return true;
        }
        else
        {
            if (this.CurAttackSkillBase == null || !this.CheckSkillRushBlock(this.CurAttackSkillBase.FirstStage, this.Target.CurrServerPos))
            {
                if (this.Angle > this.AttackAngle && MainPlayer.Self.Pfc.CurrAutoMoveState == PathFindComponent.AutoMoveState.Off)
                {
                    MainPlayer.Self.SetPlayerLookAt(this.Target.ModelObj.transform.position, true);
                }
                return false;
            }
            if (MainPlayer.Self.Pfc.CurrAutoMoveState != PathFindComponent.AutoMoveState.Off && Vector2.Distance(this.NextPosition2DOfTargetFinder, this.Target.NextPosition2D) < 12f)
            {
                return true;
            }
            this.NextthinkTime = 0.1f;
            MainPlayer.Self.Pfc.BeginFindPath(this.Target.NextPosition2D, PathFindComponent.AutoMoveState.MoveToAttackNpc, 0f, delegate ()
            {
                if (MainPlayer.Self.Pfc.FindState != PathFindState.Complete)
                {
                    this.NextthinkTime = 5f;
                }
                this.NextPosition2DOfTargetFinder = Vector2.zero;
            }, null, 0f);
            this.NextPosition2DOfTargetFinder = this.Target.NextPosition2D;
            return true;
        }
    }

    public CompnentState State { get; set; }

    public void CompAwake(FFComponentMgr Mgr)
    {
        this.SetUI();
    }

    public void SetAutoAttackConfig()
    {
        this.SetUI();
    }

    private void CheckBreak()
    {
        if (SingletonForMono<InputController>.Instance.InputDir != -1 && this.HasMakeAttack)
        {
            if (this.CurAttackSkillBase != null && this.skillHolder != null)
            {
                this.skillHolder.CancelCacheSkill();
            }
            MainPlayer.Self.Pfc.EndPathFind(PathFindState.Break, true);
            this.HasMakeAttack = false;
        }
    }

    public bool HasInit
    {
        get
        {
            return this.AttackRange > 0f && this.AttackAngle > 0f;
        }
    }

    public void CompUpdate()
    {
        if (!this.AutoAttackOn)
        {
            return;
        }
        this.CheckBreak();
        this.runningtime += Time.deltaTime;
        if (this.runningtime < this.NextthinkTime)
        {
            return;
        }
        this.runningtime = 0f;
        this.MakeDecide();
    }

    public void CompDispose()
    {
    }

    public void PlayerSkill()
    {
        uint attackSkillIDbystor = MainPlayer.Self.GetComponent<MainPlayerSkillHolder>().GetAttackSkillIDbystor(1U);
        if (attackSkillIDbystor != 0U)
        {
            MainPlayer.Self.GetComponent<MainPlayerSkillHolder>().CancelCacheSkill();
        }
        MainPlayer.Self.Pfc.EndPathFind(PathFindState.Break, true);
        this.NextthinkTime = 1.2f;
    }

    public void ThinkImmediately()
    {
        this.NextthinkTime = 0f;
        this.MakeDecide();
    }

    public void ThinkDelay(float f)
    {
        this.runningtime = 0f;
        this.NextthinkTime = f;
    }

    public bool CheckRushBlock(Vector2 targetpos)
    {
        Vector2 b = Vector3.zero;
        GraphUtils.FindPosForRush(MainPlayer.Self.CurrServerPos, targetpos, out b, (TileFlag)11, (TileFlag)11);
        Vector3 vector = targetpos - b;
        return Mathf.Abs(vector.x) >= 0.1f || Mathf.Abs(vector.y) >= 0.1f;
    }

    public bool CheckSkillRushBlock(LuaTable SkillStage, Vector2 targetpos)
    {
        Vector2 targetpos2 = Vector2.zero;
        int[] skillMoveParam = this.GetSkillMoveParam(SkillStage.GetField_String("MoveDis"));
        if (skillMoveParam[0] == 0 && skillMoveParam[2] > 0)
        {
            targetpos2 = MainPlayer.Self.GetPosBySelf((float)skillMoveParam[1], (float)skillMoveParam[2]);
            return this.CheckRushBlock(targetpos2);
        }
        if (skillMoveParam[0] == 1 || skillMoveParam[0] == 3)
        {
            targetpos2 = MainPlayer.Self.GetPosByTarget(targetpos, (float)skillMoveParam[2]);
            return this.CheckRushBlock(targetpos2);
        }
        return false;
    }

    private int[] GetSkillMoveParam(string ParamStr)
    {
        string[] array = ParamStr.Split(new char[]
        {
            ':'
        });
        return new int[]
        {
            int.Parse(array[0]),
            int.Parse(array[1]),
            int.Parse(array[2])
        };
    }

    public void ResetComp()
    {
    }

    private float range = -1f;

    private float angle = -1f;

    private MainPlayerSkillBase curskill;

    private bool _AutoAttackOn;

    public float NextthinkTime = 0.8f;

    private Vector2 DisV;

    public Vector2 NextPosition2DOfTargetFinder;

    private float Distance;

    private float Angle;

    private bool HasMakeAttack;

    private float runningtime;
}
