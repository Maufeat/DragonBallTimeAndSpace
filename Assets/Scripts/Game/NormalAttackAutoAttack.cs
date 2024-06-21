using System;
using Framework.Managers;
using LuaInterface;
using map;
using UnityEngine;

public class NormalAttackAutoAttack
{
    private float AttackRange
    {
        get
        {
            if (this.range >= 0f)
            {
                return this.range;
            }
            if (this.Skillid != 0U && this.NormalAttackSkillBase != null)
            {
                string field_String = this.NormalAttackSkillBase.SkillConfig.GetField_String("SkillRange");
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
    }

    private float AttackAngle
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
                string field = string.Empty + (int)controller.curCareer;
                LuaTable cacheField_Table = LuaConfigManager.GetXmlConfigTable("autoattack").GetCacheField_Table("careerAI").GetCacheField_Table(field);
                if (cacheField_Table != null)
                {
                    this.angle = cacheField_Table.GetCacheField_Float("AttackAngle");
                }
            }
            return this.angle;
        }
    }

    private MainPlayerSkillBase NormalAttackSkillBase
    {
        get
        {
            if (this.normalskill == null && this.SkillHolder != null && this.Skillid != 0U)
            {
                this.normalskill = this.SkillHolder.MainPlayerSkillList[this.Skillid];
            }
            return this.normalskill;
        }
    }

    private MainPlayerSkillHolder SkillHolder
    {
        get
        {
            return MainPlayer.Self.GetComponent<MainPlayerSkillHolder>();
        }
    }

    public uint Skillid
    {
        get
        {
            if (this.skillid == 0U && this.SkillHolder != null)
            {
                this.skillid = this.SkillHolder.GetAttackSkillIDbystor(1U);
                return this.skillid;
            }
            return this.skillid;
        }
    }

    private BreakAutoattackUIMgr BreakAutoattackUimgr
    {
        get
        {
            if (this.breakAutoattack == null)
            {
                this.breakAutoattack = MainPlayer.Self.GetComponent<BreakAutoattackUIMgr>();
            }
            return this.breakAutoattack;
        }
    }

    private PathFindComponent PathFindComp
    {
        get
        {
            if (this.pathFind == null)
            {
                this.pathFind = MainPlayer.Self.Pfc;
            }
            return this.pathFind;
        }
    }

    private MainPlayerTargetSelectMgr SelectMgr
    {
        get
        {
            if (this.selectMgr == null)
            {
                this.selectMgr = MainPlayer.Self.GetComponent<MainPlayerTargetSelectMgr>();
            }
            return this.selectMgr;
        }
    }

    public void Init()
    {
        Scheduler.Instance.AddUpdator(new Scheduler.OnScheduler(this.Update));
    }

    public void Dispose()
    {
        Scheduler.Instance.RemoveUpdator(new Scheduler.OnScheduler(this.Update));
    }

    public bool ActiveSelf
    {
        get
        {
            return this.activeself;
        }
        set
        {
            if (this.activeself != value)
            {
                this.activeself = value;
                if (this.activeself)
                {
                    this.ThinkImmediately();
                }
            }
        }
    }

    public void StartAttack(CharactorBase cb)
    {
        if (cb == null)
        {
            return;
        }
        if (cb == this.AttackTarget)
        {
            return;
        }
        if (!this.SelectMgr.CanAttack(cb))
        {
            return;
        }
        MainPlayer.Self.Pfc.EndPathFind(PathFindState.Break, true);
        this.ActiveSelf = true;
        this.NextPosition2DOfTargetFinder = Vector2.zero;
    }

    public CharactorBase AttackTarget
    {
        get
        {
            return this.SelectMgr.TargetCharactor;
        }
    }

    public void MakeDecide()
    {
        if (this.CheckSkill())
        {
            return;
        }
        this.RefreshDisToTarget();
        if (this.CheckMoveToTarget())
        {
            return;
        }
        this.CheckMakeAttack();
    }

    private bool IsOnPlayerControl()
    {
        if (this.PathFindComp != null)
        {
            if (this.BreakAutoattackUimgr != null && this.BreakAutoattackUimgr.IsBreakAutoAttack())
            {
                MainPlayer.Self.Pfc.EndPathFind(PathFindState.Break, true);
                this.ActiveSelf = false;
                return true;
            }
            if (SingletonForMono<InputController>.Instance.InputDir != -1)
            {
                this.PathFindComp.EndPathFind(PathFindState.Break, true);
                this.ActiveSelf = false;
                return true;
            }
            if (this.PathFindComp.isPathfinding && this.PathFindComp.CurrAutoMoveState > PathFindComponent.AutoMoveState.MoveToAttackNpc)
            {
                this.ActiveSelf = false;
                return true;
            }
        }
        return false;
    }

    private bool CheckSkill()
    {
        FFBehaviourState_Skill ffbehaviourState_Skill = null;
        if (this.FFBC == null)
        {
            this.FFBC = MainPlayer.Self.GetComponent<FFBehaviourControl>();
        }
        if (this.FFBC != null)
        {
            ffbehaviourState_Skill = this.FFBC.CurrStatebyType<FFBehaviourState_Skill>();
        }
        if (ffbehaviourState_Skill != null && ffbehaviourState_Skill.CurrSkillClipState < SkillClip.State.Attack)
        {
            this.NextthinkTime = 0.05f;
            return true;
        }
        return false;
    }

    private void CheckMakeAttack()
    {
        this.MakeAttack();
        this.NextthinkTime = 0.01f;
    }

    private void MakeAttack()
    {
        uint attackSkillIDbystor = MainPlayer.Self.GetComponent<MainPlayerSkillHolder>().GetAttackSkillIDbystor(1U);
        if (attackSkillIDbystor != 0U)
        {
            MainPlayer.Self.GetComponent<MainPlayerSkillHolder>().ClickSkillEvent(attackSkillIDbystor);
            MainPlayer.Self.Pfc.EndPathFind(PathFindState.Complete, true);
        }
        this.HasMakeAttack = true;
    }

    private void RefreshDisToTarget()
    {
        this.Angle = Vector3.Angle((CommonTools.DismissYSize(this.AttackTarget.ModelObj.transform.position) - CommonTools.DismissYSize(MainPlayer.Self.ModelObj.transform.position)).normalized, CommonTools.DismissYSize(MainPlayer.Self.ModelObj.transform.forward));
        this.Distance = Vector2.Distance(this.AttackTarget.NextPosition2D, MainPlayer.Self.CurrentPosition2D);
    }

    private bool CheckMoveToTarget()
    {
        float num = 0f;
        if (this.AttackTarget is Npc && this.AttackTarget is Npc)
        {
            num = LuaConfigManager.GetConfigTable("npc_data", (ulong)(this.AttackTarget as Npc).NpcData.MapNpcData.baseid).GetField_Uint("volume");
        }
        if (this.Distance > this.AttackRange + num)
        {
            this.NextthinkTime = 0.1f;
            if (MainPlayer.Self.Pfc.CurrAutoMoveState != PathFindComponent.AutoMoveState.Off && Vector2.Distance(this.NextPosition2DOfTargetFinder, this.AttackTarget.NextPosition2D) < 12f)
            {
                return true;
            }
            MainPlayer.Self.Pfc.BeginFindPath(this.AttackTarget.NextPosition2D, PathFindComponent.AutoMoveState.MoveToAttackNpc, this.AttackRange, delegate ()
            {
                if (MainPlayer.Self.Pfc.FindState == PathFindState.NotFind)
                {
                    this.ActiveSelf = false;
                }
                this.NextPosition2DOfTargetFinder = Vector2.zero;
            }, null, 0f);
            this.NextPosition2DOfTargetFinder = this.AttackTarget.NextPosition2D;
            return true;
        }
        else
        {
            if (this.NormalAttackSkillBase == null || !this.CheckSkillRushBlock(this.NormalAttackSkillBase.FirstStage, this.AttackTarget.CurrServerPos))
            {
                if (this.Angle > this.AttackAngle && MainPlayer.Self.Pfc.CurrAutoMoveState == PathFindComponent.AutoMoveState.Off)
                {
                    MainPlayer.Self.SetPlayerLookAt(this.AttackTarget.ModelObj.transform.position, true);
                }
                return false;
            }
            this.NextthinkTime = 0.1f;
            if (MainPlayer.Self.Pfc.CurrAutoMoveState != PathFindComponent.AutoMoveState.Off && Vector2.Distance(this.NextPosition2DOfTargetFinder, this.AttackTarget.NextPosition2D) < 12f)
            {
                return true;
            }
            MainPlayer.Self.Pfc.BeginFindPath(this.AttackTarget.NextPosition2D, PathFindComponent.AutoMoveState.MoveToAttackNpc, 0f, delegate ()
            {
                if (MainPlayer.Self.Pfc.FindState == PathFindState.NotFind)
                {
                    this.ActiveSelf = false;
                }
                this.NextPosition2DOfTargetFinder = Vector2.zero;
            }, null, 0f);
            this.NextPosition2DOfTargetFinder = this.AttackTarget.NextPosition2D;
            return true;
        }
    }

    private void CheckBreak()
    {
        if (SingletonForMono<InputController>.Instance.InputDir != -1 && this.HasMakeAttack)
        {
            uint attackSkillIDbystor = MainPlayer.Self.GetComponent<MainPlayerSkillHolder>().GetAttackSkillIDbystor(1U);
            if (attackSkillIDbystor != 0U)
            {
                MainPlayer.Self.GetComponent<MainPlayerSkillHolder>().CancelCacheSkill();
            }
            MainPlayer.Self.Pfc.EndPathFind(PathFindState.Break, true);
            this.HasMakeAttack = false;
        }
    }

    public void Update()
    {
        if (!this.ActiveSelf)
        {
            return;
        }
        if (this.AttackTarget == null && this.ActiveSelf)
        {
            this.StartAttack(MainPlayer.Self.GetComponent<MainPlayerTargetSelectMgr>().TargetCharactor);
        }
        if (this.AttackTarget == null)
        {
            this.ActiveSelf = false;
            return;
        }
        if (this.IsOnPlayerControl())
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

    public void PlayerSkill()
    {
        uint attackSkillIDbystor = MainPlayer.Self.GetComponent<MainPlayerSkillHolder>().GetAttackSkillIDbystor(1U);
        if (attackSkillIDbystor != 0U)
        {
            MainPlayer.Self.GetComponent<MainPlayerSkillHolder>().CancelCacheSkill();
        }
        MainPlayer.Self.Pfc.EndPathFind(PathFindState.Break, true);
        this.NextthinkTime = 2f;
    }

    public void ThinkImmediately()
    {
        this.NextthinkTime = 0f;
        this.Update();
    }

    public bool HasInit
    {
        get
        {
            return this.AttackRange > 0f;
        }
    }

    public void OnTargetChange(CharactorBase cb)
    {
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

    private float range = -1f;

    private float angle = -1f;

    private MainPlayerSkillBase normalskill;

    private uint skillid;

    private BreakAutoattackUIMgr breakAutoattack;

    private PathFindComponent pathFind;

    private MainPlayerTargetSelectMgr selectMgr;

    private bool activeself;

    private CharactorBase attacktarget;

    private float NextthinkTime = 0.8f;

    private Vector2 DisV;

    private float Distance;

    private Vector2 NextPosition2DOfTargetFinder;

    private float Angle;

    private FFBehaviourControl FFBC;

    private bool HasMakeAttack;

    private float runningtime;
}
