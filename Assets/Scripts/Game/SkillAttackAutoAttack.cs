using System;
using Framework.Managers;
using Game.Scene;
using LuaInterface;
using map;
using UnityEngine;

public class SkillAttackAutoAttack
{
    private float AttackRange
    {
        get
        {
            if (this.CurrentSkillBase != null)
            {
                return this.CurrentSkillBase.AttackRange;
            }
            return -1f;
        }
    }

    private bool canAttackAngle
    {
        get
        {
            return this.Angle <= this.AttackAngle;
        }
    }

    private MainPlayerSkillBase CurrentSkillBase
    {
        get
        {
            if (this.currskill == null && this.SkillHolder != null && this.SkillID != 0U)
            {
                this.currskill = this.SkillHolder.MainPlayerSkillList[this.SkillID];
                string field_String = this.currskill.SkillConfig.GetField_String("SkillRange");
                string[] array = field_String.Split(new char[]
                {
                    ':'
                });
                if (array.Length > 1)
                {
                    this.autoAttack.AttackRange = float.Parse(array[1]);
                }
            }
            return this.currskill;
        }
    }

    private MainPlayerSkillHolder SkillHolder
    {
        get
        {
            return MainPlayer.Self.GetComponent<MainPlayerSkillHolder>();
        }
    }

    private AutoAttack autoAttack
    {
        get
        {
            return MainPlayer.Self.GetComponent<AutoAttack>();
        }
    }

    public uint SkillID
    {
        get
        {
            if (this.m_skillId == 0U)
            {
                this.m_skillId = this.SkillHolder.GetNormalSkillID();
            }
            return this.m_skillId;
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

    public CharactorBase AttackTarget
    {
        get
        {
            return this.SelectMgr.TargetCharactor;
        }
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

    public void Init()
    {
        Scheduler.Instance.AddUpdator(new Scheduler.OnScheduler(this.Update));
    }

    public void Dispose()
    {
        Scheduler.Instance.RemoveUpdator(new Scheduler.OnScheduler(this.Update));
    }

    public bool StartAttack(uint skillid, bool bclick = false)
    {
        if (MainPlayer.Self.IsSoul)
        {
            return false;
        }
        if (ManagerCenter.Instance.GetManager<GameScene>().isAbattoirScene)
        {
            AbattoirMatchController controller = ControllerManager.Instance.GetController<AbattoirMatchController>();
            if (!controller.CheckCanEnter(skillid))
            {
                return false;
            }
        }
        this.Clean();
        this.bClickSkillBtn = bclick;
        this.m_skillId = skillid;
        if (this.CurrentSkillBase == null)
        {
            return false;
        }
        if (this.CurrentSkillBase.CurrState == MainPlayerSkillBase.state.CD && this.CurrentSkillBase.IStorageTimes <= 0U)
        {
            return false;
        }
        if (!this.CurrentSkillBase.SkillConfig.GetField_Bool("NeedTarget"))
        {
            this.ActiveSelf = true;
            this.MakeAttack();
            return true;
        }
        CharactorBase target = this.CurrentSkillBase.GetTarget(true, false);
        if (target == null)
        {
            TipsWindow.ShowWindow(TipsType.NEED_A_TARGET, null);
            return false;
        }
        float num = Vector2.Distance(target.CurrentPosition2D, MainPlayer.Self.CurrentPosition2D);
        if (target is Npc)
        {
            uint baseid = (target as Npc).NpcData.MapNpcData.baseid;
            LuaTable configTable = LuaConfigManager.GetConfigTable("npc_data", (ulong)baseid);
            if (configTable != null)
            {
                num -= configTable.GetField_Uint("volume");
            }
        }
        if (this.AttackRange >= num)
        {
            this.ActiveSelf = true;
            this.MakeAttack();
            return true;
        }
        float range;
        if (this.CurrentSkillBase.SkillConfig.GetField_Uint("SearchRange") == 0U)
        {
            range = this.AttackRange;
        }
        else
        {
            range = this.CurrentSkillBase.SkillConfig.GetField_Uint("SearchRange");
        }
        if (this.CurrentSkillBase.SkillConfig.GetField_Uint("CastBeyondType") == 3U)
        {
            CharactorBase charactorBase = MainPlayer.Self.GetComponent<MainPlayerTargetSelectMgr>().skillattackSelect.SearchSkillAttackTarget(range);
            charactorBase = null;
            if (charactorBase == null)
            {
                TipsWindow.ShowWindow(TipsType.TARGET_TOO_FAR, null);
                return false;
            }
            MainPlayer.Self.GetComponent<MainPlayerTargetSelectMgr>().skillattackSelect.SetTarget(charactorBase, false, true);
            this.ActiveSelf = true;
            return true;
        }
        else if (this.CurrentSkillBase.SkillConfig.GetField_Uint("CastBeyondType") == 4U)
        {
            CharactorBase charactorBase2 = MainPlayer.Self.GetComponent<MainPlayerTargetSelectMgr>().skillattackSelect.SearchSkillAttackTarget(range);
            charactorBase2 = null;
            if (charactorBase2 == null || charactorBase2 == target)
            {
                this.ActiveSelf = true;
                return false;
            }
            MainPlayer.Self.GetComponent<MainPlayerTargetSelectMgr>().skillattackSelect.SetTarget(charactorBase2, false, true);
            this.ActiveSelf = true;
            return true;
        }
        else if (this.CurrentSkillBase.SkillConfig.GetField_Uint("CastBeyondType") == 5U)
        {
            CharactorBase charactorBase3 = MainPlayer.Self.GetComponent<MainPlayerTargetSelectMgr>().skillattackSelect.SearchSkillAttackTarget(range);
            charactorBase3 = null;
            if (charactorBase3 == null)
            {
                this.ActiveSelf = true;
                this.MakeAttack();
                return false;
            }
            MainPlayer.Self.GetComponent<MainPlayerTargetSelectMgr>().skillattackSelect.SetTarget(charactorBase3, false, true);
            this.ActiveSelf = true;
            return true;
        }
        else
        {
            if (this.CurrentSkillBase.SkillConfig.GetField_Uint("CastBeyondType") == 2U && this.SelectMgr.CanSelect(target))
            {
                this.ActiveSelf = true;
                return true;
            }
            return false;
        }
    }

    public void Clean()
    {
        this.currskill = null;
        this.m_skillId = 0U;
        this.ActiveSelf = false;
        this.NextPosition2DOfTargetFinder = Vector2.zero;
    }

    public void MakeDecide()
    {
        if (this.CheckSkill())
        {
            return;
        }
        this.CheckBreak();
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
                return false;
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
        if (this.SkillHolder.FFBC != null)
        {
            ffbehaviourState_Skill = this.SkillHolder.FFBC.CurrStatebyType<FFBehaviourState_Skill>();
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
        if (this.SkillID != 0U)
        {
            this.casting = this.SkillHolder.ClickSkillEvent(this.SkillID);
            MainPlayer.Self.Pfc.EndPathFind(PathFindState.Complete, false);
        }
        this.HasMakeAttack = true;
        this.Clean();
    }

    public void ChangeNormalAttack()
    {
        if (!this.AutoAttackState)
        {
            return;
        }
        if (this.SelectMgr.TargetCharactor != null)
        {
            uint normalSkillID = this.SkillHolder.GetNormalSkillID();
            if (this.CurrentSkillBase.CheckLegal(this.AttackTarget) && MyPlayerPrefs.GetInt("AutoAttackClosed") == 0)
            {
                this.StartAttack(normalSkillID, false);
            }
        }
    }

    private void NotBreakoffAttackAndCloseToTarget()
    {
        if (this.AttackTarget == null)
        {
            return;
        }
        if (!MainPlayer.Self.GetComponent<PlayerBufferControl>().ContainsState(UserState.USTATE_BATTLE))
        {
            return;
        }
        if (!this.IsNormalSkillRange())
        {
            return;
        }
        if (this.casting)
        {
            return;
        }
        this.ChangeNormalAttack();
    }

    private void RefreshDisToTarget()
    {
        this.Angle = Vector3.Angle((CommonTools.DismissYSize(this.AttackTarget.ModelObj.transform.position) - CommonTools.DismissYSize(MainPlayer.Self.ModelObj.transform.position)).normalized, CommonTools.DismissYSize(MainPlayer.Self.ModelObj.transform.forward));
        this.Distance = Vector2.Distance(this.AttackTarget.NextPosition2D, MainPlayer.Self.CurrentPosition2D);
    }

    public bool IsNormalSkillRange()
    {
        if (this.AttackTarget == null)
        {
            return false;
        }
        this.RefreshDisToTarget();
        float num = 0f;
        if (this.AttackTarget is Npc && this.AttackTarget is Npc)
        {
            num = LuaConfigManager.GetConfigTable("npc_data", (ulong)(this.AttackTarget as Npc).NpcData.MapNpcData.baseid).GetField_Uint("volume");
        }
        return this.Distance <= this.AttackRange + num;
    }

    private bool CheckAttackAngle()
    {
        return this.bClickSkillBtn || (!this.bClickSkillBtn && this.canAttackAngle);
    }

    private bool CheckMoveToTarget()
    {
        float num = 0f;
        if (this.AttackTarget is Npc)
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
                if (MainPlayer.Self.Pfc.FindState != PathFindState.Complete)
                {
                }
                this.NextPosition2DOfTargetFinder = Vector2.zero;
            }, null, num * GraphUtils.GetRealDistanceRate());
            this.NextPosition2DOfTargetFinder = this.AttackTarget.NextPosition2D;
            return true;
        }
        else
        {
            if (this.CurrentSkillBase == null || !this.CheckSkillRushBlock(this.CurrentSkillBase.FirstStage, this.AttackTarget.CurrServerPos))
            {
                return false;
            }
            this.NextthinkTime = 0.1f;
            if (MainPlayer.Self.Pfc.CurrAutoMoveState != PathFindComponent.AutoMoveState.Off && Vector2.Distance(this.NextPosition2DOfTargetFinder, this.AttackTarget.NextPosition2D) < 12f)
            {
                return true;
            }
            MainPlayer.Self.Pfc.BeginFindPath(this.AttackTarget.NextPosition2D, PathFindComponent.AutoMoveState.MoveToAttackNpc, 0f, delegate ()
            {
                if (MainPlayer.Self.Pfc.FindState != PathFindState.Complete)
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
            if (this.SkillID != 0U)
            {
                this.SkillHolder.CancelCacheSkill();
            }
            MainPlayer.Self.Pfc.EndPathFind(PathFindState.Break, true);
            this.HasMakeAttack = false;
        }
    }

    public void Update()
    {
        if (this.AttackTarget == null)
        {
            this.ActiveSelf = false;
            return;
        }
        if (!this.ActiveSelf)
        {
            this.NotBreakoffAttackAndCloseToTarget();
            return;
        }
        if (this.IsOnPlayerControl())
        {
            return;
        }
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
        uint attackSkillIDbystor = this.SkillHolder.GetAttackSkillIDbystor(1U);
        if (attackSkillIDbystor != 0U)
        {
            this.SkillHolder.CancelCacheSkill();
        }
        MainPlayer.Self.Pfc.EndPathFind(PathFindState.Break, true);
        this.NextthinkTime = 2f;
    }

    public void ThinkImmediately()
    {
        this.NextthinkTime = 0f;
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

    public void OnBreak()
    {
        this.casting = false;
    }

    private float AttackAngle = 45f;

    private bool casting;

    private float Distance;

    private float Angle;

    private float NextthinkTime = 0.8f;

    private Vector2 NextPosition2DOfTargetFinder;

    private bool bClickSkillBtn;

    private MainPlayerSkillBase currskill;

    private uint m_skillId;

    private BreakAutoattackUIMgr breakAutoattack;

    private PathFindComponent pathFind;

    private MainPlayerTargetSelectMgr selectMgr;

    private bool activeself;

    private bool HasMakeAttack;

    public bool AutoAttackState;

    private float runningtime;
}
