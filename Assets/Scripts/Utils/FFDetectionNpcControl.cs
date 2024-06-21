using System;
using System.Collections.Generic;
using Framework.Managers;
using LuaInterface;
using UnityEngine;

public class FFDetectionNpcControl : IFFComponent
{
    private EntitiesManager entitiesManager
    {
        get
        {
            return ManagerCenter.Instance.GetManager<EntitiesManager>();
        }
    }

    public CompnentState State { get; set; }

    public ulong CurrentNearestNpcID
    {
        get
        {
            return this.currentNearestNpcID;
        }
        set
        {
            if (this.currentNearestNpcID != value)
            {
                this.currentNearestNpcID = value;
                if (FFDetectionNpcControl.OnCurrentNearestNpcChanged != null)
                {
                    FFDetectionNpcControl.OnCurrentNearestNpcChanged(this.currentNearestNpcID);
                }
            }
        }
    }

    public void UpDateOnMoveOneStep()
    {
        this.DetectVisitNpc();
    }

    public void DetectVisitNpc()
    {
        this.RefrashInRangeNpcTopButton();
        if (TaskController.CurrnetNpcDlg != null && TaskController.CurNpcDlgSource == 0U)
        {
            TaskController.CurrnetNpcDlg.ResetNPCDir();
            TaskController.CurrnetNpcDlg.UnInit();
            this.CurrentVisteNpcID = 0UL;
        }
    }

    public void RefrashInRangeNpcTopButton()
    {
        this.entitiesManager.NpcList.BetterForeach(delegate (KeyValuePair<ulong, Npc> pair)
        {
            Npc value = pair.Value;
            PlayerBufferControl component = value.GetComponent<PlayerBufferControl>();
            if (component != null && component.ContainsState(UserState.USTATE_TIPS))
            {
                float num = 0f;
                bool flag = this.IsInTriggerRange(this.Owner.NextPosition2D, value.NextPosition2D, out num);
                if (flag)
                {
                    bool flag2 = true;
                    if (value is Npc_TaskCollect && !(value as Npc_TaskCollect).CheckStateContainDoing())
                    {
                        flag2 = false;
                    }
                    if (!value.CheckIsShowByTask(false))
                    {
                        flag2 = false;
                    }
                    if (flag2)
                    {
                        value.ShowTopBtn();
                    }
                }
                else
                {
                    value.CloseTopBtn(true);
                }
            }
        });
    }

    public Npc GetNearestVisitNpc()
    {
        Npc result = null;
        if (this.Owner == null)
        {
            return null;
        }
        List<Npc> npcsWithTips = new List<Npc>();
        this.entitiesManager.NpcList.BetterForeach(delegate (KeyValuePair<ulong, Npc> pair)
        {
            Npc value = pair.Value;
            PlayerBufferControl component = value.GetComponent<PlayerBufferControl>();
            if (component != null && component.ContainsState(UserState.USTATE_TIPS))
            {
                float num3 = 0f;
                bool flag = this.IsInTriggerRange(this.Owner.NextPosition2D, value.NextPosition2D, out num3);
                if (flag)
                {
                    bool flag2 = true;
                    if (value is Npc_TaskCollect && !(value as Npc_TaskCollect).CheckStateContainDoing())
                    {
                        flag2 = false;
                    }
                    if (!value.CheckIsShowByTask(false))
                    {
                        flag2 = false;
                    }
                    if (flag2)
                    {
                        npcsWithTips.Add(value);
                    }
                }
            }
        });
        float num = (float)this.ShowTopBtnRange;
        for (int i = 0; i < npcsWithTips.Count; i++)
        {
            Npc npc = npcsWithTips[i];
            if ((ulong)npc.NpcData.MapNpcData.baseid == this.PriorityVisiteNPCID)
            {
                result = npc;
                break;
            }
            float num2 = Vector2.Distance(this.Owner.NextPosition2D, npc.NextPosition2D);
            if (num2 < num)
            {
                num = num2;
                result = npc;
            }
        }
        return result;
    }

    private bool IsInTriggerRange(Vector2 selfServerPos, Vector2 npcServerPos, out float distance)
    {
        bool result = false;
        distance = 0f;
        Vector3 worldPosByServerPos = GraphUtils.GetWorldPosByServerPos(selfServerPos);
        Vector3 worldPosByServerPos2 = GraphUtils.GetWorldPosByServerPos(npcServerPos);
        distance = Vector3.Distance(worldPosByServerPos, worldPosByServerPos2);
        if (distance <= (float)this.ShowTopBtnRange)
        {
            result = true;
        }
        return result;
    }

    public bool TryResetNPCDir()
    {
        bool result = false;
        ulong currentVisteNpcID = this.CurrentVisteNpcID;
        if (currentVisteNpcID == 0UL)
        {
            return result;
        }
        Npc npc = ManagerCenter.Instance.GetManager<EntitiesManager>().GetCharactorByID(currentVisteNpcID, CharactorType.NPC) as Npc;
        if (npc == null)
        {
            return result;
        }
        LuaTable configTable = LuaConfigManager.GetConfigTable("npc_data", (ulong)npc.NpcData.MapNpcData.baseid);
        if (configTable == null)
        {
            return result;
        }
        if (npc != null && configTable.GetField_Uint("kind") == 1U)
        {
            npc.SetPlayerLastDirection();
        }
        return true;
    }

    public bool IsNearestNpc(ulong npcid)
    {
        return npcid == this.CurrentNearestNpcID;
    }

    public bool IsInMainPlayerShowTopBtnRange(Npc npc)
    {
        CharactorBase self = MainPlayer.Self;
        if (this.inShowTopBtnRangeNpc == null)
        {
            this.inShowTopBtnRangeNpc = new List<ulong>();
        }
        float num;
        return this.IsInTriggerRange(self.NextPosition2D, npc.NextPosition2D, out num);
    }

    public void CompAwake(FFComponentMgr Mgr)
    {
        this.Owner = Mgr.Owner;
        this.ShowTopBtnRange = LuaConfigManager.GetXmlConfigTable("massiveConfig").GetCacheField_Table("npcVisiteShowDis").GetCacheField_Int("value");
    }

    public void CompUpdate()
    {
    }

    public void CompDispose()
    {
    }

    public void ResetComp()
    {
    }

    public CharactorBase Owner;

    public int ShowTopBtnRange;

    public ulong PriorityVisiteNPCID;

    public ulong CurrentVisteNpcID;

    private ulong currentNearestNpcID;

    public static Action<ulong> OnCurrentNearestNpcChanged;

    public ulong RecommendVisiteNpcID;

    private float lastGetTime;

    private float cacheInternal;

    private List<ulong> inShowTopBtnRangeNpc;
}
