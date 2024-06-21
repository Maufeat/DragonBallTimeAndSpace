using System;
using System.Collections.Generic;
using Engine;
using Framework.Managers;
using Game.Scene;
using LuaInterface;
using magic;
using msg;
using Pet;
using UnityEngine;

public class CharactorBase
{
    public CharactorBase()
    {
        this.CharEvtMgr = ClassPool.GetObject<CharacterEventMgr>();
        this.NextJumPos.pos = default(cs_FloatMovePos);
        this.NextJumPos.pos.fx = -1f;
        this.NextJumPos.pos.fy = -1f;
    }

    public CharactorState CharState
    {
        get
        {
            return this._CharState;
        }
        set
        {
            this._CharState = value;
        }
    }

    public void UpdateHatredList(List<ulong> hatreds)
    {
        if (this.hatredList == null)
        {
            return;
        }
        this.hatredList.Clear();
        this.hatredList.AddRange(hatreds);
        if (!(this is MainPlayer) && MainPlayer.Self != null)
        {
            MainPlayer.Self.OnNpcHatredListChange(this);
        }
    }

    public bool IsInHatredList(ulong charid)
    {
        return this.hatredList.Contains(charid);
    }

    public FFBehaviourControl Fbc
    {
        get
        {
            return this.fbc;
        }
        set
        {
            this.fbc = value;
        }
    }

    public PathFindComponent Pfc
    {
        get
        {
            return this.pfc;
        }
        set
        {
            this.pfc = value;
        }
    }

    public GameObject ModelObj
    {
        get
        {
            return this._modelObj;
        }
        set
        {
            this._modelObj = value;
            if (null != this._modelObj)
            {
                this.HightLightControl = this._modelObj.GetComponent<HighlighterController>();
            }
        }
    }

    ~CharactorBase()
    {
        this.CharEvtMgr.StoreToPool();
    }

    public string animatorControllerName
    {
        get
        {
            if (this.animator == null)
            {
                return "none";
            }
            if (this.animator.runtimeAnimatorController == null)
            {
                return "none";
            }
            return this.animator.runtimeAnimatorController.name;
        }
    }

    public CharabaseData BaseData
    {
        get
        {
            return this.GetComponent<CharabaseData>();
        }
    }

    public bool IsMoving
    {
        get
        {
            return this.isMoving;
        }
    }

    public bool IsCurFrameMoved
    {
        get
        {
            return this.moved;
        }
    }

    public bool IsBufferNoMove
    {
        get
        {
            PlayerBufferControl component = this.ComponentMgr.GetComponent<PlayerBufferControl>();
            return component != null && component.HasBeControlled(BufferState.ControlType.NoMove);
        }
    }

    public bool IsFly
    {
        get
        {
            PlayerBufferControl component = this.ComponentMgr.GetComponent<PlayerBufferControl>();
            return component != null && component.ContainsState(UserState.USTATE_FLY);
        }
    }

    public bool IsFallChecking
    {
        get
        {
            PlayerBufferControl component = this.ComponentMgr.GetComponent<PlayerBufferControl>();
            return component != null && component.ContainsState(UserState.USTATE_EVIL_TOILET);
        }
    }

    public bool IsBattleState
    {
        get
        {
            PlayerBufferControl component = this.ComponentMgr.GetComponent<PlayerBufferControl>();
            return component != null && component.ContainsState(UserState.USTATE_BATTLE);
        }
    }

    public Vector2 CurrentPosition2D
    {
        get
        {
            if (this.BaseData == null || this.BaseData.BaseData == null)
            {
                return Vector3.zero;
            }
            return new Vector2(this.BaseData.BaseData.pos.fx, this.BaseData.BaseData.pos.fy);
        }
    }

    public Vector2 NextPosition2D
    {
        get
        {
            return this.nextPosition2D;
        }
        set
        {
            this.nextPosition2D = value;
        }
    }

    public float moveSpeed
    {
        get
        {
            return this._moveSpeed * this.SpeedCut;
        }
        set
        {
            if (value == float.PositiveInfinity || value == float.NegativeInfinity || value == float.NaN)
            {
                FFDebug.LogError(this, "you want to set moveSpeed value == float.PositiveInfinity || value == float.NegativeInfinity || value == float.");
                return;
            }
            if (value < 0.01f)
            {
                this._moveSpeed = 0.01f;
            }
            else
            {
                this._moveSpeed = value;
            }
        }
    }

    public Vector3 TargetPos
    {
        get
        {
            return this.TargetPos_;
        }
        set
        {
            this.TargetPos_ = value;
        }
    }

    public uint FaceDir
    {
        get
        {
            return this._faceDir;
        }
        set
        {
            this._faceDir = value;
        }
    }

    public uint ServerDir
    {
        get
        {
            return this._serverDir;
        }
        set
        {
            this._serverDir = value;
            this._faceDir = value;
        }
    }

    public void AddComponent(IFFComponent IFFcomp)
    {
        if (this.ComponentMgr == null)
        {
            return;
        }
        this.ComponentMgr.AddComponent(IFFcomp);
    }

    public void AddComponentImmediate(IFFComponent IFFcomp)
    {
        if (this.ComponentMgr == null)
        {
            return;
        }
        this.ComponentMgr.AddComponentImmediate(IFFcomp);
    }

    public T GetComponent<T>() where T : IFFComponent
    {
        if (this.ComponentMgr == null)
        {
            return default(T);
        }
        return this.ComponentMgr.GetComponent<T>();
    }

    public void RemoveComponent(IFFComponent IFFcomp)
    {
        if (this.ComponentMgr == null)
        {
            return;
        }
        this.ComponentMgr.RemoveComponent(IFFcomp);
    }

    public void ResetAllCompment()
    {
        if (this.ComponentMgr == null)
        {
            return;
        }
        this.ComponentMgr.ResetAllCompment();
    }

    public virtual void InitComponent()
    {
    }

    public virtual void Init()
    {
        this.ComponentMgr = new FFComponentMgr(this);
    }

    public void SetPlayerPosition(cs_FloatMovePos pos, uint dir)
    {
        if (this.CharState != CharactorState.CreatComplete)
        {
            return;
        }
        this.ServerDir = dir;
        this._lastServerDir = dir;
        this.SetPlayerPosition(pos);
    }

    public void SetPlayerLookAt(Vector3 Target, bool immediately)
    {
        if (this.CharState != CharactorState.CreatComplete)
        {
            return;
        }
        Vector3 vector = Target - this.ModelObj.transform.position;
        Vector2 vdir = new Vector2(vector.x, vector.z);
        uint serverDirByClientDir = CommonTools.GetServerDirByClientDir(vdir);
        this.SetPlayerDirection(serverDirByClientDir, false);
        if (immediately)
        {
            this.ModelObj.transform.rotation = CommonTools.GetClientDirQuaternionByServerDir((int)this.ServerDir);
        }
    }

    public void SetPlayerDirection(uint dir, bool immediately = false)
    {
        if (this.CharState != CharactorState.CreatComplete)
        {
            return;
        }
        this.ServerDir = dir;
        if (immediately)
        {
            this.ModelObj.transform.rotation = CommonTools.GetClientDirQuaternionByServerDir((int)this.ServerDir);
        }
    }

    public void SetPlayerPosition(cs_FloatMovePos pos)
    {
        if (CharState != CharactorState.CreatComplete)
        {
            return;
        }
        if (this is MainPlayer)
        {
            if (AreaMusicTool.Instance == null)
            {
                FFDebug.LogError(this, "加载场景完成后未初始化AreaMusicTool");
            }
            else
            {
                AreaMusicTool.Instance.UpdateMusic(this.CurrentPosition2D);
            }
        }
        BaseData.RefreshCharaBasePosition(pos.fx, pos.fy);
        NextPosition2D = this.CurrentPosition2D;
        Vector3 worldPosByServerPos = GraphUtils.GetWorldPosByServerPos(CurrentPosition2D);
        SetPhysicsPos(worldPosByServerPos);
        TargetPos = worldPosByServerPos;
        if (this is MainPlayer)
        {
            ManagerCenter.Instance.GetManager<GameScene>().SetFogAndLight();
        }
        CharEvtMgr.SendEvent("CharEvt_MoveOneStep", new object[0]);
    }

    public void SetPlayerPosition(Vector2 pos, uint dir)
    {
        this.SetPlayerPosition(new cs_FloatMovePos
        {
            fx = pos.x,
            fy = pos.y
        }, dir);
    }

    public void setPetPostionAndDir(Vector2 pos, uint dir)
    {
        this.SetPlayerPosition(new cs_FloatMovePos
        {
            fx = pos.x,
            fy = pos.y
        });
        this.ModelObj.transform.rotation = new Quaternion(0f, dir * 2U, 0f, 0f);
    }

    public void SetPlayerLastDirection()
    {
        if (this.CharState != CharactorState.CreatComplete)
        {
            return;
        }
        this.SetPlayerDirection(this._lastServerDir, false);
        this.ModelObj.transform.rotation = CommonTools.GetClientDirQuaternionByServerDir((int)this.ServerDir);
    }

    public bool CharactorCheckMove(Vector2 serverpos)
    {
        if (Mathf.Abs(serverpos.x - this.CurrentPosition2D.x) > 18f || Mathf.Abs(serverpos.y - this.CurrentPosition2D.y) > 18f)
        {
            FFDebug.LogWarning(this, "Client Check CharBase Move Error!!");
            this.ForceSetCharBasePositionTo(serverpos);
            return true;
        }
        return false;
    }

    public void ForceSetCharBasePositionTo(Vector2 serverpos)
    {
        FFDebug.Log(this, FFLogType.Default, string.Concat(new object[]
        {
            "----------------------------------------->>Force Set CharBase position to X:",
            serverpos.x,
            " Y:",
            serverpos.y,
            "     ",
            " CurrentPosition2D (",
            this.CurrentPosition2D.x,
            ",",
            this.CurrentPosition2D.y,
            ")"
        }));
        this.BaseData.RefreshCharaBasePosition(serverpos);
        this.NextPosition2D = this.CurrentPosition2D;
        this.StopMoveImmediate(delegate
        {
            Vector3 worldPosByServerPos = GraphUtils.GetWorldPosByServerPos(serverpos);
            if (this.ModelObj != null)
            {
                this.SetPhysicsPos(worldPosByServerPos);
                this.TargetPos = this.ModelObj.transform.position;
            }
        });
    }

    public void SetPhysicsPos(Vector3 pos)
    {
        this.ModelObj.transform.position = this.GetCharactorY(pos);
    }

    public void SetPhysicsPos(Vector2 pos)
    {
        this.ModelObj.transform.position = this.GetCharactorY(new Vector3(pos.x, 0f, pos.y));
    }

    public void SetWorldPosition(Vector3 v3Pos)
    {
        if (null != this.ModelObj)
        {
            this.ModelObj.transform.position = v3Pos;
        }
    }

    public float GetCharactorY(Vector2 pos)
    {
        if (this.IsFly)
        {
            return MapHightDataHolder.FlyMapHeight;
        }
        if (this.IsFallChecking)
        {
            return this.FallCheckingHeight;
        }
        return MapHightDataHolder.GetMapHeight(pos.x, pos.y);
    }

    public Vector3 GetCharactorY(Vector3 pos)
    {
        Vector3 result = pos;
        if (this.IsFly)
        {
            result.y = MapHightDataHolder.FlyMapHeight + this.JumpHeight;
        }
        else if (this.IsFallChecking)
        {
            result.y = this.FallCheckingHeight;
        }
        else
        {
            result.y = MapHightDataHolder.GetMapHeight(result.x, result.z) + this.JumpHeight;
        }
        if (this.CustomPosY > 0f)
        {
            result.y = this.CustomPosY;
        }
        return result;
    }

    public void RefreshPhysicsPos()
    {
        Vector3 worldPosByServerPos = GraphUtils.GetWorldPosByServerPos(this.CurrentPosition2D);
        this.SetPhysicsPos(worldPosByServerPos);
    }

    public bool IsCanMove()
    {
        AbattoirMatchController controller = ControllerManager.Instance.GetController<AbattoirMatchController>();
        if (controller == null || controller.getStartRestTime > 0)
        {
            return false;
        }
        if (this.ComponentMgr != null)
        {
            FFBehaviourControl component = this.ComponentMgr.GetComponent<FFBehaviourControl>();
            PlayerBufferControl component2 = this.ComponentMgr.GetComponent<PlayerBufferControl>();
            if (component == null)
            {
                return false;
            }
            if (component2 != null && component2.HasBeControlled(BufferState.ControlType.NoMove))
            {
                return false;
            }
            FFBehaviourState_Skill ffbehaviourState_Skill = component.CurrStatebyType<FFBehaviourState_Skill>();
            if (ffbehaviourState_Skill != null)
            {
                return ffbehaviourState_Skill.CanMoveCancelSkil || ffbehaviourState_Skill.SpeedCut > 0f;
            }
        }
        return !this.OnFastMove && !this.OnRestTime;
    }

    public bool IsCanJump()
    {
        if (!(this is MainPlayer) || this.Fbc == null)
        {
            return false;
        }
        if (this.Fbc.CurrState is FFBehaviourState_Skill)
        {
            FFBehaviourState_Skill ffbehaviourState_Skill = this.Fbc.CurrState as FFBehaviourState_Skill;
            return ffbehaviourState_Skill.CurrSkillClip == null || ffbehaviourState_Skill.CurrSkillClip.AnimConfig == null || MainPlayerSkillHolder.Instance.Skillmgr.IsChantStage((ulong)ffbehaviourState_Skill.CurrSkillClip.AnimConfig.SkillStateId);
        }
        PlayerBufferControl component = this.ComponentMgr.GetComponent<PlayerBufferControl>();
        return component == null || !component.HasBeControlled(BufferState.ControlType.NoMove);
    }

    public bool IsCanRotate()
    {
        return true;
    }

    public bool InCastSkillState()
    {
        return this.fbc != null && this.fbc.CurrState is FFBehaviourState_Skill;
    }

    public cs_MoveData CurrMoveData
    {
        get
        {
            return this._currMoveData;
        }
    }

    public void RecodeSeverMoveData(cs_MoveData data)
    {
        this._currMoveData = data;
        if (this.OnMoveDataChange != null)
        {
            this.OnMoveDataChange(this);
        }
    }

    public void MoveDir(cs_MoveData data)
    {
        this.ServerDir = data.dir;
        if (this.CurrMoveData != null)
        {
            ControllerManager.Instance.GetController<UIMapController>().SetMyPos(this.CurrServerPos, this.ServerDir, false, true);
        }
    }

    public void MoveTo(cs_MoveData data)
    {
        this.MoveTo(new Vector2(data.pos.fx, data.pos.fy));
        this.ServerDir = data.dir;
    }

    public void MoveTo(Vector2 serverpos)
    {
        if (this.CharState != CharactorState.CreatComplete)
        {
            return;
        }
        this.NextPosition2D = serverpos;
        if (this is Npc)
        {
            Npc npc = this as Npc;
            npc.SetTargetPos(serverpos);
        }
        else if (this is MainPlayer)
        {
            if (AreaMusicTool.Instance == null)
            {
                FFDebug.LogError(this, "加载场景完成后未初始化AreaMusicTool");
            }
            else
            {
                AreaMusicTool.Instance.UpdateMusic(serverpos);
            }
        }
        this.TargetPos = GraphUtils.GetWorldPosByServerPos(this.NextPosition2D);
        this.moveDir = GraphUtils.GetWorldPosByServerPos(this.NextPosition2D) - this.ModelObj.transform.position;
        this.moveDir.y = 0f;
        this.moveDir.Normalize();
        this.isMoving = true;
    }

    public void Moving()
    {
        this.moved = false;
        if (this.animatorSpeed > 0f)
        {
            this.animatorSpeed -= Time.deltaTime * 80f;
        }
        this.UpdateFastMove();
        if (!this.IsCanMove())
        {
            this.SetFFBWalkorIdle(this.animatorSpeed);
            return;
        }
        if (this.ModelObj == null)
        {
            return;
        }
        if (this.isMoving)
        {
            this.moved = true;
            Vector3 vector = this.ModelObj.transform.position + this.moveDir * this.moveSpeed * Time.deltaTime;
            Vector3 a = this.TargetPos - this.ModelObj.transform.position;
            a.y = 0f;
            Vector3 a2 = vector - this.ModelObj.transform.position;
            a2.y = 0f;
            float num = Vector3.Magnitude(a);
            float num2 = Vector3.Magnitude(a2);
            float num3 = Vector3.Dot(a.normalized, a2.normalized);
            Vector3 physicsPos = Vector3.zero;
            this.animatorSpeed = this.moveSpeed;
            if (this is MainPlayer)
            {
                if (this.Pfc != null)
                {
                    Vector3? findPathEndPos = this.Pfc.findPathEndPos;
                    if (findPathEndPos != null && this.Pfc.isPathfinding)
                    {
                        Vector3 position = this.ModelObj.transform.position;
                        Vector3? findPathEndPos2 = this.Pfc.findPathEndPos;
                        Vector3 value = findPathEndPos2.Value;
                        position.y = 0f;
                        value.y = 0f;
                        float num4 = Vector3.Distance(position, value);
                        if (num4 < this.BaseData.GetCharaVolume() + MainPlayer.Self.pfc.visitNpcRadius)
                        {
                            this.Pfc.EndPathFind(PathFindState.Complete, true);
                            this.isMoving = false;
                            if (this.Pfc.OnPathFindEnd != null)
                            {
                                this.Pfc.findPathEndPos = null;
                            }
                        }
                    }
                }
            }
            else
            {
                Vector2 serverPosByWorldPos = GraphUtils.GetServerPosByWorldPos(vector, true);
                this.CheckNeedJump(serverPosByWorldPos);
            }
            physicsPos = vector;
            if (num2 >= num)
            {
                this.SetPhysicsPos(this.TargetPos);
                this.OnReachTarget();
            }
            else
            {
                this.SetPhysicsPos(physicsPos);
                if (num3 < 0f)
                {
                    FFDebug.LogWarning(this, "移动异常！!");
                    this.OnReachTarget();
                }
            }
        }
        if (this is MainPlayer)
        {
            this.SetFFBWalkorIdleWithInput(this.animatorSpeed, this.inputDir);
        }
        else
        {
            this.SetFFBWalkorIdle(this.animatorSpeed);
        }
        this.inputDir = -1;
    }

    public void Jump(bool needSync, bool isFall = false)
    {
        FFBehaviourControl component = this.GetComponent<FFBehaviourControl>();
        if (component == null)
        {
            return;
        }
        if (component.CurrState is FFBehaviourState_Jump && !(component.CurrState as FFBehaviourState_Jump).CanChange())
        {
            return;
        }
        if (component.CurrState is FFBehaviourState_Fall && !(component.CurrState as FFBehaviourState_Fall).CanChange())
        {
            return;
        }
        if (this.noJump)
        {
            return;
        }
        if (needSync)
        {
            ManagerCenter.Instance.GetManager<EntitiesManager>().PNetWork.ReqJump();
            if (this is MainPlayer)
            {
                MainPlayerSkillHolder.Instance.OnBreak(CSkillBreakType.Jump);
            }
        }
        if (isFall)
        {
            component.ChangeState(ClassPool.GetObject<FFBehaviourState_Fall>());
        }
        else
        {
            component.ChangeState(ClassPool.GetObject<FFBehaviourState_Jump>());
        }
    }

    public bool inJumpState()
    {
        return this.fbc != null && (this.fbc.CurrState is FFBehaviourState_Jump || this.fbc.CurrState is FFBehaviourState_Fall);
    }

    public bool inWalkState()
    {
        return this.fbc != null && this.fbc.CurrState is FFBehaviourState_Walk;
    }

    public bool canJumpStateChange()
    {
        return this.fbc != null && (this.fbc.CurrState as FFBehaviourState_Jump).CanChange();
    }

    public bool OnJumpLand()
    {
        FFBehaviourControl component = this.GetComponent<FFBehaviourControl>();
        if (component == null)
        {
            return false;
        }
        if (this.isMoving)
        {
            component.ChangeState(ClassPool.GetObject<FFBehaviourState_Walk>());
        }
        else
        {
            component.ChangeState(ClassPool.GetObject<FFBehaviourState_Idle>());
        }
        return true;
    }

    public void OnMainCameraChanged()
    {
        if (this.hpdata != null)
        {
            this.hpdata.OnMainCameraChange();
        }
    }

    public void Rotatint()
    {
        Quaternion clientDirQuaternionByServerDir = CommonTools.GetClientDirQuaternionByServerDir((int)this.FaceDir);
        if (this.ModelObj != null)
        {
            Quaternion rotation = Quaternion.Slerp(this.ModelObj.transform.rotation, clientDirQuaternionByServerDir, this.rotateSpeed * Time.deltaTime);
            if (this.IsCanRotate())
            {
                this.ModelObj.transform.rotation = rotation;
            }
        }
    }

    private void SetFFBWalkorIdle(float animatorSpeed)
    {
        FFBehaviourControl component = this.GetComponent<FFBehaviourControl>();
        this.SpeedCut = 1f;
        if (component == null)
        {
            return;
        }
        if (!(component.CurrState is FFBehaviourState_Walk) && animatorSpeed > 0f)
        {
            if (component.CurrState is FFBehaviourState_Skill)
            {
                if ((component.CurrState as FFBehaviourState_Skill).CanMoveCancelSkil)
                {
                    PlayerBufferControl component2 = this.GetComponent<PlayerBufferControl>();
                    if (component2 == null || !component2.ResetSetPlayerBuffActionBehaviour())
                    {
                        component.ChangeState(ClassPool.GetObject<FFBehaviourState_Idle>());
                    }
                    else if (this is MainPlayer)
                    {
                        MainPlayerSkillHolder.Instance.OnBreak(CSkillBreakType.Move);
                    }
                }
                else
                {
                    this.SpeedCut = (component.CurrState as FFBehaviourState_Skill).SpeedCut;
                }
            }
            else if (!(component.CurrState is FFBehaviourState_InBuffAction))
            {
                if (!(component.CurrState is FFBehaviourState_Jump))
                {
                    if (!(component.CurrState is FFBehaviourState_Fall))
                    {
                        component.ChangeState(ClassPool.GetObject<FFBehaviourState_Walk>());
                    }
                }
            }
        }
        else if (component.CurrState is FFBehaviourState_Walk && animatorSpeed <= 0f)
        {
            component.ChangeState(ClassPool.GetObject<FFBehaviourState_Idle>());
        }
    }

    private void SetFFBWalkorIdleWithInput(float animatorSpeed, int inputDir)
    {
        FFBehaviourControl component = this.GetComponent<FFBehaviourControl>();
        this.SpeedCut = 1f;
        if (component == null)
        {
            return;
        }
        if (!(component.CurrState is FFBehaviourState_Walk) && (animatorSpeed > 0f || inputDir > 0))
        {
            if (component.CurrState is FFBehaviourState_Skill)
            {
                if ((component.CurrState as FFBehaviourState_Skill).CanMoveCancelSkil)
                {
                    PlayerBufferControl component2 = this.GetComponent<PlayerBufferControl>();
                    if (component2 == null || !component2.ResetSetPlayerBuffActionBehaviour())
                    {
                        component.ChangeState(ClassPool.GetObject<FFBehaviourState_Idle>());
                    }
                    else if (this is MainPlayer)
                    {
                        MainPlayerSkillHolder.Instance.OnBreak(CSkillBreakType.Move);
                    }
                }
                else
                {
                    this.SpeedCut = (component.CurrState as FFBehaviourState_Skill).SpeedCut;
                }
            }
            else if (component.CurrState is FFBehaviourState_InBuffAction)
            {
                if (this.ComponentMgr.GetComponent<PlayerBufferControl>().ContainsState(UserState.USTATE_PRETEND_DEATH))
                {
                    component.ChangeState(ClassPool.GetObject<FFBehaviourState_Walk>());
                }
            }
            else if (!(component.CurrState is FFBehaviourState_Jump) || (component.CurrState as FFBehaviourState_Jump).CanChange())
            {
                if (!(component.CurrState is FFBehaviourState_Fall) || (component.CurrState as FFBehaviourState_Fall).CanChange())
                {
                    component.ChangeState(ClassPool.GetObject<FFBehaviourState_Walk>());
                }
            }
        }
        else if (component.CurrState is FFBehaviourState_Walk && animatorSpeed <= 0f && inputDir < 0)
        {
            component.ChangeState(ClassPool.GetObject<FFBehaviourState_Idle>());
        }
    }

    private void OnReachTarget()
    {
        this.BaseData.RefreshCharaBasePosition(this.NextPosition2D);
        if (this.RetMoveDataQueue.Count != 0)
        {
            cs_MoveData data = this.RetMoveDataQueue.Dequeue();
            this.MoveTo(data);
        }
        else
        {
            this.isMoving = false;
        }
        if (this is MainPlayer && this.Pfc.isPathfinding)
        {
            if (this.AStarPathDataQueue.Count == 0)
            {
                this.Pfc.EndPathFind(PathFindState.Complete, true);
                this.isMoving = false;
            }
            else
            {
                cs_MoveData cs_MoveData = this.AStarPathDataQueue.Dequeue();
                if (GraphUtils.IsBlockPointForMove(cs_MoveData.pos.fx, cs_MoveData.pos.fy))
                {
                    this.AStarPathDataQueue.Clear();
                    this.isMoving = false;
                    return;
                }
                if (this.OnAStarPathDataQueueDequeue != null)
                {
                    this.OnAStarPathDataQueueDequeue();
                }
                ManagerCenter.Instance.GetManager<EntitiesManager>().PNetWork.ReqMove(cs_MoveData, false, false);
                this.MoveTo(cs_MoveData);
            }
        }
        if (this.OnStopMoveHandle != null)
        {
            this.animatorSpeed = 0f;
            this.OnStopMoveHandle();
            this.OnStopMoveHandle = null;
            this.isMoving = false;
            if (this is MainPlayer && this.Pfc.isPathfinding)
            {
                this.Pfc.EndPathFind(PathFindState.Break, true);
            }
        }
        if (this is MainPlayer)
        {
            (this as MainPlayer).OnMoveOneStep();
        }
        this.CharEvtMgr.SendEvent("CharEvt_MoveOneStep", new object[0]);
    }

    protected void ReloadShader(GameObject go)
    {
        GameMain gameMain = MonobehaviourManager.Instance as GameMain;
        if (null == gameMain)
        {
            return;
        }
        Renderer[] componentsInChildren = go.GetComponentsInChildren<Renderer>();
        for (int i = 0; i < componentsInChildren.Length; i++)
        {
            for (int j = 0; j < componentsInChildren[i].sharedMaterials.Length; j++)
            {
                if (componentsInChildren[i].sharedMaterials[j])
                {
                    Shader shader = gameMain.GetShader(componentsInChildren[i].sharedMaterials[j].shader.name);
                    if (shader != null)
                    {
                        componentsInChildren[i].sharedMaterials[j].shader = shader;
                    }
                }
            }
        }
    }

    public void CheckNeedJump(Vector2 pos)
    {
        if ((int)this.NextJumPos.pos.fx != (int)pos.x || (int)this.NextJumPos.pos.fy != (int)pos.y)
        {
            return;
        }
        this.NextJumPos.pos.fx = -1f;
        this.NextJumPos.pos.fy = -1f;
        this.Jump(false, false);
    }

    public void StopMoving(Action callBack)
    {
        if (this.isMoving)
        {
            this.animatorSpeed = 0f;
            if (this is MainPlayer)
            {
            }
            this.OnStopMoveHandle = callBack;
        }
        else
        {
            callBack();
        }
    }

    public void StopMoveImmediate(Action callback)
    {
        FFDebug.Log(this, FFLogType.Default, "StopMoveImmediate " + this.CurrentPosition2D);
        this.SetPlayerPosition(this.CurrentPosition2D, this.ServerDir);
        this.isMoving = false;
        if (this is MainPlayer && this.Pfc != null)
        {
            this.Pfc.EndPathFind(PathFindState.Break, true);
        }
        this.CharEvtMgr.SendEvent("CharEvt_MoveOneStep", new object[0]);
        this.animatorSpeed = 0f;
        if (callback != null)
        {
            callback();
        }
    }

    public void StopMoveImmediateWithOutSetPos(Action callback)
    {
        this.isMoving = false;
        if (this is MainPlayer && this.Pfc != null)
        {
            this.Pfc.EndPathFind(PathFindState.Break, true);
        }
        this.CharEvtMgr.SendEvent("CharEvt_MoveOneStep", new object[0]);
        this.animatorSpeed = 0f;
        if (callback != null)
        {
            callback();
        }
    }

    public void OnMoveStateChange(bool needCheck)
    {
        if (needCheck)
        {
            ManagerCenter.Instance.GetManager<EntitiesManager>().PNetWork.CheckNeedReqMove();
        }
    }

    public void SetInputDir(int dir)
    {
        this.inputDir = dir;
    }

    public void FastMoveTo(Vector2 serverpos, float Duration, float mirrorTotalTime, float mirrorFadeSpeed)
    {
        if (serverpos == Vector2.zero)
        {
            return;
        }
        if (Duration <= 0f)
        {
            FFDebug.LogWarning(this, "FastMove Error: Duration <= 0");
            return;
        }
        FFDebug.Log(this, FFLogType.Player, string.Format("FastMoveTo--->{0}", serverpos));
        this.FastMoveRunningTime = Duration;
        this._currMoveData.pos.fx = serverpos.x;
        this._currMoveData.pos.fy = serverpos.y;
        this.fmtargetpos = GraphUtils.GetWorldPosByServerPos(serverpos);
        this.FMStep = (this.fmtargetpos - this.ModelObj.transform.position) / Duration;
        this.OnFastMove = true;
        if (mirrorTotalTime > 0f)
        {
            this.createMirror(mirrorTotalTime, mirrorFadeSpeed);
        }
        this.BaseData.RefreshCharaBasePosition(serverpos);
        this.NextPosition2D = serverpos;
    }

    public void UpdateFastMove()
    {
        if (!this.OnFastMove)
        {
            return;
        }
        this.FastMoveRunningTime -= Time.deltaTime;
        this.SetPhysicsPos(this.ModelObj.transform.position + this.FMStep * Time.deltaTime);
        if (this.FastMoveRunningTime <= 0f)
        {
            this.FastMoveOver();
        }
    }

    protected virtual void createMirror(float mirrorTotalTime, float mirrorFadeSpeed)
    {
    }

    protected void FastMoveOver()
    {
        this.OnFastMove = false;
        this.SetPlayerPosition(new cs_FloatMovePos
        {
            fx = this.CurrentPosition2D.x,
            fy = this.CurrentPosition2D.y
        });
        this.SetPhysicsPos(this.fmtargetpos);
        this.animatorSpeed = 0f;
    }

    public Vector2 CurrServerPos
    {
        get
        {
            return new Vector2(this.CurrMoveData.pos.fx, this.CurrMoveData.pos.fy);
        }
    }

    public Vector2 GetPosBySelf(float Angle, float dis)
    {
        Vector2 vector = new Vector2(this.CurrMoveData.pos.fx, this.CurrMoveData.pos.fy);
        float num = this.ServerDir * 2U;
        num += Angle;
        if (num > 360f)
        {
            num -= 360f;
        }
        Quaternion rotation = Quaternion.AngleAxis(num, Vector3.up);
        Vector3 vector2 = rotation * Vector3.forward;
        Vector2 vector3 = new Vector2(vector2.x, -vector2.z);
        Vector2 normalized = vector3.normalized;
        FFDebug.Log(this, FFLogType.Skill, string.Concat(new object[]
        {
            "GetPosBySelf from->",
            vector,
            "<--",
            normalized,
            "-> to",
            vector + normalized * dis
        }));
        return vector + normalized * dis;
    }

    public Vector2 GetPosByTarget(Vector2 Target, float dis)
    {
        Vector2 vector = new Vector2(this.CurrMoveData.pos.fx, this.CurrMoveData.pos.fy);
        Vector2 normalized = (Target - vector).normalized;
        FFDebug.Log(this, FFLogType.Default, string.Concat(new object[]
        {
            "from->",
            vector,
            "<--",
            normalized,
            "-> to",
            vector + normalized * dis
        }));
        return Target + normalized * dis;
    }

    public bool IsLive
    {
        get
        {
            PlayerBufferControl component = this.GetComponent<PlayerBufferControl>();
            return component == null || !component.ContainsState(UserState.USTATE_DEATH);
        }
    }

    public bool IsSoul
    {
        get
        {
            PlayerBufferControl component = this.GetComponent<PlayerBufferControl>();
            return component != null && component.ContainsState(UserState.USTATE_SOUL);
        }
    }

    protected void PlayAniByState(UserState state)
    {
        LuaTable bufferConfig = ManagerCenter.Instance.GetManager<BufferStateManager>().GetBufferConfig(state);
        uint field_Uint = bufferConfig.GetField_Uint("BuffAnim");
        FFActionClip ffactionClip = ManagerCenter.Instance.GetManager<FFActionClipManager>().GetFFActionClip(this.animatorControllerName, field_Uint, 0);
        if (ffactionClip == null || this.Fbc == null)
        {
            return;
        }
        this.Fbc.PlayAnim(ffactionClip.ClipName, 0f, 0f);
    }

    public virtual void Die()
    {
        this.StopMoveImmediate(null);
        FFEffectControl component = this.GetComponent<FFEffectControl>();
        if (component != null)
        {
            component.SetAllEffectOver();
        }
        PlayerBufferControl component2 = this.GetComponent<PlayerBufferControl>();
        if (component2 != null)
        {
            component2.AddStateByLocal(new StateItem
            {
                lasttime = 0UL,
                overtime = 0UL,
                uniqid = CommonTools.GenernateBuffHash(this.EID, 1UL, 0UL)
            });
        }
        if (this.hpdata != null)
        {
            this.hpdata.SetDeathStateColor();
        }
        MainPlayerTargetSelectMgr.SetTargetSelectEffect(this, false);
        this.CancelSelect();
    }

    public virtual void DelayRelive()
    {
        PlayerBufferControl component = this.GetComponent<PlayerBufferControl>();
        if (component != null)
        {
            component.RemoveState(UserState.USTATE_DEATH);
        }
    }

    public virtual void ClearEffect()
    {
        FFEffectControl component = this.GetComponent<FFEffectControl>();
        if (component != null)
        {
            component.SetAllEffectOver();
        }
    }

    public virtual void HandleHit(MSG_Ret_MagicAttack_SC mdata)
    {
    }

    public virtual void HitOther(MSG_Ret_MagicAttack_SC mdata, CharactorBase[] BeNHits)
    {
    }

    public virtual void RevertHpMp(MSG_Ret_HpMpPop_SC mdata)
    {
    }

    public virtual void DestroyThisInNineScreen()
    {
        this.CancelSelect();
        this.CharState = CharactorState.RemoveFromNineScreen;
        this.ComponentMgr.Dispose();
        if (this.OnDestroyThisInNineScreen != null)
        {
            this.OnDestroyThisInNineScreen(this);
        }
    }

    public virtual void TargetSelect()
    {
        this.IsSelect = true;
        this.InitTempSelectTrans();
    }

    public virtual void CancelSelect()
    {
        this.IsSelect = false;
    }

    public virtual void OnUpdateCharacterBuff(UserState newState)
    {
        this.CharEvtMgr.SendEvent("CharEvt_BuffUpdate", new object[]
        {
            newState
        });
    }

    public virtual void OnRemoveCharacterBuff(UserState oldState)
    {
        this.CharEvtMgr.SendEvent("CharEvt_BuffRemove", new object[]
        {
            oldState
        });
    }

    public virtual bool IsShowHitAnim(int hpChange)
    {
        return true;
    }

    private void InitTempSelectTrans()
    {
        FFBipBindMgr component = this.GetComponent<FFBipBindMgr>();
        if (component == null)
        {
            return;
        }
        Transform bindPoint = component.GetBindPoint("Top");
        Transform bindPoint2 = component.GetBindPoint("Feet");
        if (bindPoint == null || bindPoint2 == null)
        {
            return;
        }
        if (this.tempSelectTran == null)
        {
            this.tempSelectTran = new GameObject
            {
                name = "selectPos"
            }.transform;
        }
        if (this.tempSelectTran.parent != bindPoint.parent)
        {
            this.tempSelectTran.SetParent(bindPoint.parent);
            this.tempSelectTran.localScale = Vector3.one;
            this.tempSelectTran.rotation = bindPoint.rotation;
        }
        float y = (bindPoint.position.y + bindPoint2.position.y) / 2f;
        Vector3 position = new Vector3(bindPoint.position.x, y, bindPoint.position.z);
        this.tempSelectTran.position = position;
    }

    public Transform GetTempSelectTrans()
    {
        if (this.tempSelectTran == null)
        {
            this.InitTempSelectTrans();
            this.InitCameraFocusPos();
        }
        return this.tempSelectTran;
    }

    public Transform GetFeetSelectTrans()
    {
        FFBipBindMgr component = this.GetComponent<FFBipBindMgr>();
        if (component == null)
        {
            return null;
        }
        return component.GetBindPoint("Feet");
    }

    public virtual void OnRelationChange()
    {
    }

    public Transform TopPos
    {
        get
        {
            return this.m_topPos;
        }
    }

    public Transform FeetPos
    {
        get
        {
            return this.m_feetPos;
        }
    }

    public Transform CameraPos
    {
        get
        {
            return this.m_cameraPos;
        }
    }

    public void InitCameraFocusPos()
    {
        FFBipBindMgr component = this.GetComponent<FFBipBindMgr>();
        if (component != null)
        {
            this.m_cameraPos = component.GetBindPoint("camerapoint");
            this.m_topPos = component.GetBindPoint("top");
            this.m_feetPos = component.GetBindPoint("feet");
        }
    }

    public EntitiesID EID = default(EntitiesID);

    public uint petBarCount;

    public uint petBarUnlockcount;

    public PetBase FightPet;

    public PetBase AssistPet;

    public HpStruct hpdata;

    private CharactorState _CharState;

    public CharacterEventMgr CharEvtMgr;

    public HighlighterController HightLightControl;

    protected ulong lastCreateId = 1UL;

    public List<ulong> hatredList = new List<ulong>();

    private FFBehaviourControl fbc;

    private PathFindComponent pfc;

    private GameObject _modelObj;

    public RelationType rlationType;

    public Animator animator;

    public FFComponentMgr ComponentMgr;

    private bool isMoving;

    private bool moved;

    public bool isFalling;

    private Vector2 nextPosition2D = new Vector2(99f, 99f);

    public float JumpHeight;

    public float FallCheckingHeight;

    public float CustomPosY;

    public Queue<cs_MoveData> RetMoveDataQueue = new Queue<cs_MoveData>();

    public cs_MoveData NextJumPos = new cs_MoveData();

    public Queue<cs_MoveData> AStarPathDataQueue = new Queue<cs_MoveData>();

    public Action OnAStarPathDataQueueDequeue;

    private float _moveSpeed;

    private float SpeedCut = 1f;

    public float rotateSpeed = 10f;

    public Vector3 moveDir = default(Vector3);

    public Vector3 TargetPos_;

    public float MinimunMoveUnit = 1f;

    public Action OnStopMoveHandle;

    private float animatorSpeed;

    private uint _faceDir;

    private uint _serverDir;

    private uint _lastServerDir;

    protected int inputDir = -1;

    protected bool isMoveByInput;

    private cs_MoveData _currMoveData;

    public Action<CharactorBase> OnMoveDataChange;

    public bool noJump;

    public bool OnRestTime;

    public bool OnFastMove;

    private float FastMoveRunningTime;

    private Vector3 fmtargetpos;

    private Vector3 FMStep;

    public Action<CharactorBase> OnDestroyThisInNineScreen;

    protected bool IsSelect;

    private Transform tempSelectTran;

    private Transform m_topPos;

    private Transform m_feetPos;

    private Transform m_cameraPos;
}

public enum CharactorState
{
    None,
    CreatEntity,
    CreatModel,
    CreatComplete,
    RemoveFromNineScreen
}
public enum CharactorType
{
    Player,
    NPC
}
public enum RelationType
{
    None = 0,
    Self = 1,
    Friend = 2,
    Neutral = 4,
    Enemy = 8,
}