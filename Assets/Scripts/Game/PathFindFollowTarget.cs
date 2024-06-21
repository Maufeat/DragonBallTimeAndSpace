using System;
using Framework.Managers;
using LuaInterface;
using UnityEngine;

public class PathFindFollowTarget : IFFComponent
{
    private CharactorBase Target
    {
        get
        {
            if (this._target == null)
            {
                return null;
            }
            if (this._target.CharState != CharactorState.CreatComplete)
            {
                return null;
            }
            return this._target;
        }
    }

    private float NextthinkTime
    {
        get
        {
            return this.NextthinkTime_;
        }
        set
        {
            this.NextthinkTime_ = value;
        }
    }

    private float AttackRange
    {
        get
        {
            SkillViewControll controller = ControllerManager.Instance.GetController<SkillViewControll>();
            if (controller != null)
            {
                string field = string.Empty + (int)controller.curCareer;
                LuaTable cacheField_Table = LuaConfigManager.GetXmlConfigTable("autoattack").GetCacheField_Table("careerAI").GetCacheField_Table(field);
                if (cacheField_Table != null)
                {
                    return cacheField_Table.GetCacheField_Float("AttackRange");
                }
            }
            return -1f;
        }
    }

    private float AttackAngle
    {
        get
        {
            SkillViewControll controller = ControllerManager.Instance.GetController<SkillViewControll>();
            if (controller != null)
            {
                string field = string.Empty + (int)controller.curCareer;
                LuaTable cacheField_Table = LuaConfigManager.GetXmlConfigTable("autoattack").GetCacheField_Table("careerAI").GetCacheField_Table(field);
                if (cacheField_Table != null)
                {
                    return cacheField_Table.GetCacheField_Float("AttackAngle");
                }
            }
            return -1f;
        }
    }

    private float VisiteRange
    {
        get
        {
            return Const.DistNpcVisitResponse;
        }
    }

    private float DrinkBloodRange
    {
        get
        {
            uint occupation = MainPlayer.Self.OtherPlayerData.MapUserData.mapshow.occupation;
            return LuaConfigManager.GetXmlConfigTable("careerskill").GetCacheField_Table("drinkBloodDistance").GetCacheField_Table(occupation.ToString()).GetCacheField_Float("value") - 0.5f;
        }
    }

    private float NormalAttackRange
    {
        get
        {
            float result = 0f;
            uint attackSkillIDbystor = MainPlayer.Self.GetComponent<MainPlayerSkillHolder>().GetAttackSkillIDbystor(1U);
            if (attackSkillIDbystor != 0U)
            {
                string cacheField_String = MainPlayer.Self.GetComponent<MainPlayerSkillHolder>().MainPlayerSkillList[attackSkillIDbystor].SkillConfig.GetCacheField_String("SkillRange");
                string[] array = cacheField_String.Split(new char[]
                {
                    ':'
                });
                if (array.Length > 1)
                {
                    result = float.Parse(array[1]);
                }
            }
            return result;
        }
    }

    public CompnentState State { get; set; }

    public void CompAwake(FFComponentMgr Mgr)
    {
        this.SetConfig();
    }

    public void CompDispose()
    {
        this.ResetFollowTarget();
    }

    public void PrintParm()
    {
    }

    public void CompUpdate()
    {
        this.CheckMoveToMoveingTarget();
        if (this.PathFindFollowType == PathFindFollowTarget.FollowType.None)
        {
            return;
        }
        if (this.IsOnPlayerControl())
        {
            if (this.OnReachTarget != null)
            {
                this.OnReachTarget(this.Target, false);
            }
            MainPlayer.Self.Pfc.EndPathFind(PathFindState.Break, true);
            this.ResetFollowTarget();
            return;
        }
        this.runningtime += Time.deltaTime;
        if (this.runningtime < this.NextthinkTime)
        {
            return;
        }
        this.runningtime = 0f;
        if (this.IsAttackComplete())
        {
            return;
        }
        this.MakeDecide();
    }

    private bool IsOnPlayerControl()
    {
        return SingletonForMono<InputController>.Instance.InputDir != -1 || (MainPlayer.Self.Pfc.isPathfinding && MainPlayer.Self.Pfc.CurrAutoMoveState > PathFindComponent.AutoMoveState.MoveToFollowTarget);
    }

    private bool IsAttackComplete()
    {
        FFBehaviourControl component = MainPlayer.Self.GetComponent<FFBehaviourControl>();
        FFBehaviourState_Skill ffbehaviourState_Skill = null;
        if (component != null)
        {
            ffbehaviourState_Skill = component.CurrStatebyType<FFBehaviourState_Skill>();
        }
        if (ffbehaviourState_Skill != null && ffbehaviourState_Skill.CurrSkillClipState < SkillClip.State.Attack)
        {
            this.NextthinkTime = 0.05f;
            return true;
        }
        return false;
    }

    public void MakeDecide()
    {
        this.RefreshDisToTarget();
        if (this.CheckReachTarget())
        {
            FFDebug.Log(this, FFLogType.FollowTarget, "ReachTarget");
            MainPlayer.Self.Pfc.EndPathFind(PathFindState.Complete, true);
            if (this.OnReachTarget != null)
            {
                this.OnReachTarget(this.Target, true);
            }
            this.ResetFollowTarget();
        }
    }

    public void ResetFollowTarget()
    {
        this.NextthinkTime = 0f;
        this._target = null;
        this.OnReachTarget = null;
        this.PathFindFollowType = PathFindFollowTarget.FollowType.None;
        this.NextPosition2DOfTargetFinder = Vector2.zero;
    }

    public void SetConfig()
    {
    }

    public void StartFwllowTarget(CharactorBase charactor, PathFindFollowTarget.FollowType type, Action<CharactorBase, bool> onreachtarget)
    {
        if (this.OnReachTarget != null)
        {
            this.OnReachTarget(this.Target, false);
            MainPlayer.Self.Pfc.EndPathFind(PathFindState.Break, true);
            this.ResetFollowTarget();
        }
        this._target = charactor;
        this.PathFindFollowType = type;
        this.OnReachTarget = onreachtarget;
    }

    private void RefreshDisToTarget()
    {
        if (this.Target == null)
        {
            return;
        }
        this.Angle = Vector3.Angle((CommonTools.DismissYSize(this.Target.ModelObj.transform.position) - CommonTools.DismissYSize(MainPlayer.Self.ModelObj.transform.position)).normalized, CommonTools.DismissYSize(MainPlayer.Self.ModelObj.transform.forward));
        this.Distance = Vector2.Distance(this.Target.NextPosition2D, MainPlayer.Self.NextPosition2D);
        MainPlayer.Self.Pfc.findPathEndPos = new Vector3?(GraphUtils.GetWorldPosByServerPos(this.Target.NextPosition2D));
    }

    private void PathFindEndFollow(bool showtips = true)
    {
        bool arg = MainPlayer.Self.Pfc.FindState == PathFindState.Complete;
        if (this.OnReachTarget != null)
        {
            this.OnReachTarget(this.Target, arg);
        }
        this.ResetFollowTarget();
        if (showtips && MainPlayer.Self.Pfc.FindState == PathFindState.NotFind)
        {
            FFDebug.LogError(this, "PathFindEndFollow  PathFindState.NotFind");
            TipsWindow.ShowWindow(TipsType.CANNOT_FIND_PATH_TO, null);
        }
    }

    private bool CanPathFindTarget()
    {
        if (MainPlayer.Self.Pfc.CastAstarPath(this.Target.NextPosition2D) == null)
        {
            if (this.OnReachTarget != null)
            {
                this.OnReachTarget(this.Target, false);
            }
            this.ResetFollowTarget();
            TipsWindow.ShowWindow(TipsType.CANNOT_FIND_PATH_TO, null);
            return false;
        }
        return true;
    }

    public void StartFollowMoveingTarget(CharactorBase charBase, PathFindFollowTarget.FollowType ft = PathFindFollowTarget.FollowType.FollowToVisite, Action<CharactorBase, bool> onReach = null)
    {
        this.PathFindFollowType = PathFindFollowTarget.FollowType.FollowToVisite;
        if (this.onReachMoveingTarget != null && this.charMoveingNpc != null)
        {
            this.onReachMoveingTarget(this.charMoveingNpc, false);
        }
        this.onReachMoveingTarget = onReach;
        this.charMoveingNpc = charBase;
        this.lastNpcWorldStayPos = new Vector2?(charBase.ModelObj.transform.position);
        this.MoveOneStepToMoveingTarget();
    }

    private void CheckMoveToMoveingTarget()
    {
        if (this.charMoveingNpc != null)
        {
            Vector2? vector = this.lastNpcWorldStayPos;
            if (vector != null)
            {
                if (this.IsOnPlayerControl())
                {
                    if (this.charMoveingNpc != null)
                    {
                        this.onReachMoveingTarget(this.charMoveingNpc, false);
                    }
                    this.charMoveingNpc = null;
                    this.lastNpcWorldStayPos = null;
                    return;
                }
                float num = Vector2.Distance(MainPlayer.Self.NextPosition2D, this.charMoveingNpc.NextPosition2D);
                if (num < Const.DistMovingNpcVisitResponse)
                {
                    if (this.onReachMoveingTarget != null)
                    {
                        this.onReachMoveingTarget(this.charMoveingNpc, true);
                    }
                    this.onReachMoveingTarget = null;
                    this.lastNpcWorldStayPos = null;
                    this.charMoveingNpc = null;
                }
                else
                {
                    this.MoveInClient();
                }
            }
        }
        else if (this.onReachMoveingTarget != null)
        {
            this.onReachMoveingTarget(this.charMoveingNpc, true);
            this.onReachMoveingTarget = null;
            this.lastNpcWorldStayPos = null;
            this.charMoveingNpc = null;
        }
    }

    public void OnReachMoveingDestPos()
    {
    }

    private void MoveOneStepToMoveingTarget()
    {
        if (this.charMoveingNpc == null)
        {
            return;
        }
        EntitiesManager manager = ManagerCenter.Instance.GetManager<EntitiesManager>();
        this.lastNpcWorldStayPos = new Vector2?(this.charMoveingNpc.ModelObj.transform.position);
        Vector2 nextPosition2D = MainPlayer.Self.NextPosition2D;
        Vector2 vector = nextPosition2D + (this.charMoveingNpc.NextPosition2D - MainPlayer.Self.NextPosition2D).normalized;
        cs_MoveData cs_MoveData = new cs_MoveData();
        cs_MoveData.pos = default(cs_FloatMovePos);
        cs_MoveData.pos.fx = vector.x;
        cs_MoveData.pos.fy = vector.y;
        cs_MoveData.step = 1;
        manager.PNetWork.ReqMove(cs_MoveData, false, false);
    }

    private void MoveInClient()
    {
        Vector3 position = MainPlayer.Self.ModelObj.transform.position;
        Vector2? vector = this.lastNpcWorldStayPos;
        Vector3 a = vector.Value;
        a = this.charMoveingNpc.ModelObj.transform.position;
        position.y = 0f;
        a.y = 0f;
        Vector3 normalized = (a - position).normalized;
        float num = Vector3.Angle(Vector3.forward, normalized);
        float num2 = Vector3.Dot(normalized, Vector3.right);
        float value;
        if (num2 > 0f)
        {
            value = num / 2f;
        }
        else
        {
            value = (360f - num) / 2f;
        }
        MainPlayer.Self.MoveToByDir(value.ToInt(), false);
    }

    private bool CheckReachTarget()
    {
        if (this.Target == null)
        {
            return true;
        }
        float num = 0f;
        if (this.Target is Npc && this.Target is Npc)
        {
            num = LuaConfigManager.GetConfigTable("npc_data", (ulong)(this.Target as Npc).NpcData.MapNpcData.baseid).GetField_Uint("volume");
        }
        if (this.PathFindFollowType == PathFindFollowTarget.FollowType.FollowToAttack)
        {
            if (this.Distance > this.AttackRange + num)
            {
                if (MainPlayer.Self.Pfc.CurrAutoMoveState != PathFindComponent.AutoMoveState.Off && Vector2.Distance(this.NextPosition2DOfTargetFinder, this.Target.CurrentPosition2D) < 12f)
                {
                    this.NextthinkTime = (this.Distance - num - this.AttackRange) / 8f;
                    return false;
                }
                MainPlayer.Self.Pfc.BeginFindPath(this.Target.CurrentPosition2D, PathFindComponent.AutoMoveState.MoveToFollowTarget, this.AttackRange, delegate ()
                {
                    this.PathFindEndFollow(true);
                }, null, 0f);
                this.NextthinkTime = (this.Distance - num - this.AttackRange) / 8f;
                if (this.Target != null)
                {
                    this.NextPosition2DOfTargetFinder = this.Target.CurrentPosition2D;
                }
                return false;
            }
            else
            {
                if (!this.CanPathFindTarget())
                {
                    return false;
                }
                if (this.Angle > this.AttackAngle)
                {
                    MainPlayer.Self.SetPlayerLookAt(this.Target.ModelObj.transform.position, true);
                }
                return true;
            }
        }
        else if (this.PathFindFollowType == PathFindFollowTarget.FollowType.FollowToVisite)
        {
            if (num < this.VisiteRange)
            {
                num = this.VisiteRange;
            }
            if (this.Distance <= num)
            {
                return true;
            }
            if (MainPlayer.Self.Pfc.CurrAutoMoveState != PathFindComponent.AutoMoveState.Off && Vector2.Distance(this.NextPosition2DOfTargetFinder, this.Target.CurrentPosition2D) < 12f)
            {
                this.NextthinkTime = (this.Distance - num) / 8f;
                return false;
            }
            float npcRadius = 0f;
            if (this.Target is Npc)
            {
                LuaTable npcConfig = (this.Target as Npc).GetNpcConfig();
                string field_String = npcConfig.GetField_String("CapsuleCollider");
                if (!string.IsNullOrEmpty(field_String))
                {
                    string[] array = field_String.Split(new string[]
                    {
                        "|"
                    }, StringSplitOptions.RemoveEmptyEntries);
                    if (array.Length > 3)
                    {
                        string s = array[3];
                        float.TryParse(s, out npcRadius);
                    }
                }
            }
            MainPlayer.Self.Pfc.BeginFindPath(this.Target.CurrentPosition2D, PathFindComponent.AutoMoveState.MoveToFollowTarget, this.VisiteRange, delegate ()
            {
                this.PathFindEndFollow(true);
            }, null, npcRadius);
            if (this.Target == null)
            {
                return true;
            }
            this.NextthinkTime = (this.Distance - num) / 8f;
            this.NextPosition2DOfTargetFinder = this.Target.CurrentPosition2D;
            return false;
        }
        else if (this.PathFindFollowType == PathFindFollowTarget.FollowType.FollowToDrinkBlood)
        {
            if (this.Distance <= this.DrinkBloodRange + num)
            {
                return true;
            }
            if (MainPlayer.Self.Pfc.CurrAutoMoveState != PathFindComponent.AutoMoveState.Off && Vector2.Distance(this.NextPosition2DOfTargetFinder, this.Target.CurrentPosition2D) < 12f)
            {
                this.NextthinkTime = (this.Distance - this.DrinkBloodRange - num) / 8f;
                return false;
            }
            MainPlayer.Self.Pfc.BeginFindPath(this.Target.CurrentPosition2D, PathFindComponent.AutoMoveState.MoveToFollowTarget, this.DrinkBloodRange, delegate ()
            {
                this.PathFindEndFollow(false);
            }, null, 0f);
            this.NextthinkTime = (this.Distance - this.DrinkBloodRange - num) / 8f;
            if (this.Target == null)
            {
                return true;
            }
            this.NextPosition2DOfTargetFinder = this.Target.CurrentPosition2D;
            return false;
        }
        else
        {
            if (this.PathFindFollowType != PathFindFollowTarget.FollowType.FollowToNormalAttack)
            {
                return false;
            }
            if (this.Distance <= this.NormalAttackRange + num)
            {
                return this.CanPathFindTarget();
            }
            if (MainPlayer.Self.Pfc.CurrAutoMoveState != PathFindComponent.AutoMoveState.Off && Vector2.Distance(this.NextPosition2DOfTargetFinder, this.Target.CurrentPosition2D) < 12f)
            {
                this.NextthinkTime = (this.Distance - this.NormalAttackRange - num) / 8f;
                return false;
            }
            MainPlayer.Self.Pfc.BeginFindPath(this.Target.CurrentPosition2D, PathFindComponent.AutoMoveState.MoveToAttackNpc, this.NormalAttackRange, delegate ()
            {
                this.PathFindEndFollow(true);
            }, null, 0f);
            this.NextthinkTime = (this.Distance - this.NormalAttackRange - num) / 8f;
            this.NextPosition2DOfTargetFinder = this.Target.CurrentPosition2D;
            return false;
        }
    }

    public void ResetComp()
    {
    }

    public PathFindFollowTarget.FollowType PathFindFollowType;

    private CharactorBase _target;

    public Action<CharactorBase, bool> OnReachTarget;

    private float NextthinkTime_;

    private Vector2 DisV;

    private Vector2 NextPosition2DOfTargetFinder;

    private float Distance;

    private float Angle;

    private CharactorBase charMoveingNpc;

    private Action<CharactorBase, bool> onReachMoveingTarget;

    private Vector2? lastNpcWorldStayPos;

    private float runningtime;

    public enum FollowType
    {
        None,
        FollowToAttack,
        FollowToVisite,
        FollowToDrinkBlood,
        FollowToNormalAttack
    }
}
