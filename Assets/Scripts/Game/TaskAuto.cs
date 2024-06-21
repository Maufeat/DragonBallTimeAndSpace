using System;
using Framework.Managers;
using Game.Scene;
using LuaInterface;
using UnityEngine;

public class TaskAuto
{
    private uint CurVisiteNpcId
    {
        get
        {
            if (this.CurPathwayid != 0U)
            {
                LuaTable configTable = LuaConfigManager.GetConfigTable("pathway", (ulong)this.CurPathwayid);
                if (configTable != null)
                {
                    return configTable.GetField_Uint("npcid");
                }
            }
            return 0U;
        }
    }

    private LuaTable CurVisiteNpcCfg
    {
        get
        {
            if (this.CurVisiteNpc != null)
            {
                return LuaConfigManager.GetConfigTable("npc_data", (ulong)this.CurVisiteNpc.NpcData.MapNpcData.baseid);
            }
            return null;
        }
    }

    private void PathFinderMoveStateChange()
    {
        if (this.ProtectTime <= 0f && this.ActiveSelf && MainPlayer.Self.Pfc.isPathfinding)
        {
            this.ActiveSelf = false;
        }
    }

    public void Initialize()
    {
        if (this.isInitialize)
        {
            return;
        }
        if (MainPlayer.Self != null && MainPlayer.Self.Pfc != null)
        {
            PathFindComponent pfc = MainPlayer.Self.Pfc;
            pfc.MoveStateChangedEvent = (Action)Delegate.Combine(pfc.MoveStateChangedEvent, new Action(this.PathFinderMoveStateChange));
            this.isInitialize = true;
        }
    }

    public void Start(uint taskid, uint pathwayid)
    {
        this.Initialize();
        this.ActiveSelf = true;
        this.CurTaskId = taskid;
        this.CurPathwayid = pathwayid;
        this.ProtectTime = 5f;
        this.State = TaskAuto.E_Type.NON;
    }

    public void EnterState(uint taskid, TaskAuto.E_Type state)
    {
        if (this.CurTaskId == taskid && this.CurTaskId != 0U)
        {
            this.State = state;
            if (this.State == TaskAuto.E_Type.Seek)
            {
                this.CurVisiteNpc = this.GetVisiteNpc(this.CurVisiteNpcId);
            }
            this.thinkTime = 0f;
            this.ProtectTime = 5f;
        }
    }

    public void Stop()
    {
        this.ActiveSelf = false;
    }

    public void Update()
    {
        if (!this.ActiveSelf)
        {
            return;
        }
        if (this.CheckBreak())
        {
            this.ActiveSelf = false;
            return;
        }
        if (!this.NpcIsValid(this.CurVisiteNpc))
        {
            this.thinkTime = 0.5f;
        }
        this.runTime += Time.deltaTime;
        this.ProtectTime -= Time.deltaTime;
        if (this.runTime < this.thinkTime)
        {
            return;
        }
        this.runTime = 0f;
        this.thinkTime = 5f;
        if (this.State == TaskAuto.E_Type.Seek && this.NextSeek())
        {
            this.Tick();
        }
    }

    public bool NextSeek()
    {
        if (!this.NpcIsValid(this.CurVisiteNpc))
        {
            this.CurVisiteNpc = this.GetVisiteNpc(this.CurVisiteNpcId);
            return this.NpcIsValid(this.CurVisiteNpc);
        }
        return true;
    }

    public bool NpcIsValid(Npc npc)
    {
        return npc != null && npc.IsLive && npc.CharState == CharactorState.CreatComplete;
    }

    public void Tick()
    {
        if (!this.NpcIsValid(this.CurVisiteNpc))
        {
            return;
        }
        if (MainPlayer.Self.Pfc.isPathfinding)
        {
            return;
        }
        NpcType field_Uint = (NpcType)this.CurVisiteNpcCfg.GetField_Uint("kind");
        switch (field_Uint)
        {
            case NpcType.NPC_TYPE_VISIT:
                return;
            case NpcType.NPC_TYPE_NOACTIVE:
            case NpcType.NPC_TYPE_ACTIVE:
            case NpcType.NPC_TYPE_MONSTER:
            case NpcType.NPC_TYPE_BOSS:
                break;
            case NpcType.NPC_TYPE_GATHER:
            case NpcType.NPC_TYPE_QUESTGATHER:
                this.ProtectTime = 5f;
                this.CurVisiteNpc.hpdata.ClickTopButton();
                return;
            default:
                if (field_Uint != NpcType.NPC_TYPE_NOACTSKILL)
                {
                    return;
                }
                break;
        }
        this.ProtectTime = 5f;
        MainPlayer.Self.GetComponent<MainPlayerSkillHolder>().normalAttackAutoAttack.StartAttack(this.CurVisiteNpc);
        MainPlayer.Self.GetComponent<MainPlayerSkillHolder>().normalAttackAutoAttack.ThinkImmediately();
    }

    public void Dispose()
    {
    }

    private bool CheckBreak()
    {
        return this.IsMapBreak() || this.IsStateBreak() || this.IsOnPlayerControl();
    }

    private bool IsMapBreak()
    {
        if (this.CurPathwayid != 0U)
        {
            LuaTable configTable = LuaConfigManager.GetConfigTable("pathway", (ulong)this.CurPathwayid);
            if (configTable != null && configTable.GetField_Uint("mapid") == ManagerCenter.Instance.GetManager<GameScene>().CurrentSceneData.mapID())
            {
                return false;
            }
        }
        return true;
    }

    private bool IsStateBreak()
    {
        return this.State == TaskAuto.E_Type.Complete;
    }

    private bool IsOnPlayerControl()
    {
        return SingletonForMono<InputController>.Instance.InputDir != -1;
    }

    private Npc GetVisiteNpc(uint npcid)
    {
        if (npcid == 0U)
        {
            return null;
        }
        return ManagerCenter.Instance.GetManager<EntitiesManager>().SearchNearNPCById(npcid, -1f);
    }

    private bool isInitialize;

    public bool ActiveSelf = true;

    public uint CurTaskId;

    public uint CurTaskState;

    public uint CurPathwayid;

    public float ProtectTime = 5f;

    public TaskAuto.E_Type State;

    private Npc CurVisiteNpc;

    private float runTime;

    private float thinkTime = 5f;

    public enum E_Type
    {
        NON,
        Seek,
        Complete
    }
}
