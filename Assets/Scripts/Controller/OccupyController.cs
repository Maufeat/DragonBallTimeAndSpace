using System;
using System.Collections.Generic;
using Framework.Managers;
using LuaInterface;
using Models;
using msg;
using npc;
using Team;
using UnityEngine;

public class OccupyController : ControllerBase
{
    public override void Awake()
    {
        LuaProcess.RegisertFunction(new Action<List<VarType>>(this.ReqHoldon));
        this.occupyNetWork = new OccupyNetWork();
        this.occupyNetWork.Initialize();
    }

    public override string ControllerName
    {
        get
        {
            return "occupy";
        }
    }

    public void ReqHoldon(List<VarType> paras)
    {
        if (paras.Count != 0)
        {
            FFDebug.LogWarning(this, "Invalid arguments to method,count: " + paras.Count);
            return;
        }
        this.ReqHoldon(0UL, 1U);
    }

    public void ReqHoldon(ulong npcid, uint holdtype)
    {
        if (MainPlayer.Self.GetComponent<PlayerBufferControl>().ContainsState(UserState.USTATE_GATHER) || MainPlayer.Self.GetComponent<PlayerBufferControl>().ContainsState(UserState.USTATE_HOLD_NPC))
        {
            return;
        }
        this.occupyNetWork.ReqHoldon(npcid, holdtype, 8U);
    }

    public void ReqHoldon(ulong npcid, uint holdtype, uint npctype)
    {
        if (MainPlayer.Self.GetComponent<PlayerBufferControl>().ContainsState(UserState.USTATE_GATHER) || MainPlayer.Self.GetComponent<PlayerBufferControl>().ContainsState(UserState.USTATE_HOLD_NPC))
        {
            return;
        }
        this.occupyNetWork.ReqHoldon(npcid, holdtype, npctype);
    }

    public void HoldOn(MSG_Ret_Holdon_SC data)
    {
        if (data.needtime == 0U)
        {
            this.occupyNetWork.ReqHoldon(data.thisid, 2U, data.npctype);
            return;
        }
        ControllerManager.Instance.GetController<ProgressUIController>().ShowProgressBar(data.needtime, delegate ()
        {
            this.occupyNetWork.ReqHoldon(data.thisid, 2U, data.npctype);
        });
    }

    public void HoldOnComplete(uint retCode)
    {
        bool obj = retCode == 0U;
        if (this.OnHoldOnComplete != null)
        {
            this.OnHoldOnComplete(obj);
        }
    }

    public void BreakHoldOn(uint holdid)
    {
        if (this.OnHoldOnComplete != null)
        {
            this.OnHoldOnComplete(false);
        }
        ControllerManager.Instance.GetController<ProgressUIController>().BreakProgressBar();
    }

    public void AddHoldNpcData(HoldNpcData data)
    {
        FFDebug.Log(this, FFLogType.HoldOn, string.Concat(new object[]
        {
            "AddHoldTransform  ",
            data.npc_tempid,
            "  time:",
            data.resttime
        }));
        if (this.HoldNpcMap.ContainsKey(data.npc_tempid))
        {
            this.HoldNpcMap[data.npc_tempid].RefreshData(data);
        }
        else
        {
            TransformData value = new TransformData(data, delegate (TransformData transformData)
            {
                transformData.StopCoutDown();
            });
            this.HoldNpcMap[data.npc_tempid] = value;
        }
    }

    public void RemoveHoldNpcData(ulong tempid)
    {
        FFDebug.Log(this, FFLogType.HoldOn, "RemoveHoldTransform  " + tempid);
        if (this.HoldNpcMap.ContainsKey(tempid))
        {
            this.HoldNpcMap[tempid].Dispose();
            this.HoldNpcMap.Remove(tempid);
        }
    }

    public void RemoveAllHoldTransform()
    {
        this.HoldNpcMap.BetterForeach(delegate (KeyValuePair<ulong, TransformData> item)
        {
            item.Value.Dispose();
        });
        this.HoldNpcMap.Clear();
    }

    public void PlayEffectOnCharactor(EntryIDType target, string effect)
    {
        CharactorBase charactorByID = ManagerCenter.Instance.GetManager<EntitiesManager>().GetCharactorByID(target);
        if (charactorByID == null)
        {
            return;
        }
        FFEffectControl component = charactorByID.GetComponent<FFEffectControl>();
        FFEffectManager manager = ManagerCenter.Instance.GetManager<FFEffectManager>();
        if (component == null || manager == null)
        {
            FFDebug.LogWarning(this, "FFEffectControl is null");
            return;
        }
        string[] group = manager.GetGroup(effect);
        for (int i = 0; i < group.Length; i++)
        {
            component.AddEffect(group[i], null, null);
        }
    }

    public void ShowHoldSuccessEffect(ulong ncpid)
    {
        EntryIDType entryIDType = new EntryIDType();
        entryIDType.id = ncpid;
        entryIDType.type = 1U;
        if (this.GetRelationship(ncpid) == 0)
        {
            this.PlayEffectOnCharactor(entryIDType, "zhanling1");
        }
        else
        {
            this.PlayEffectOnCharactor(entryIDType, "zhanling2");
        }
    }

    public void ShowTransformSuccessEffect(ulong ncpid)
    {
        EntryIDType entryIDType = new EntryIDType();
        entryIDType.id = ncpid;
        entryIDType.type = 1U;
        if (this.GetRelationship(ncpid) == 0)
        {
            this.PlayEffectOnCharactor(entryIDType, "zhanling1");
        }
        else
        {
            this.PlayEffectOnCharactor(entryIDType, "zhanling2");
        }
    }

    public void RefreshOccupyNpcShow()
    {
        this.HoldNpcMap.BetterForeach(delegate (KeyValuePair<ulong, TransformData> item)
        {
            Npc npc = ManagerCenter.Instance.GetManager<EntitiesManager>().GetNpc(item.Value.Data.npc_tempid);
            if (npc != null)
            {
                PlayerBufferControl component = npc.GetComponent<PlayerBufferControl>();
                if (component == null)
                {
                    return;
                }
                if (component.ContainsState(UserState.USTATE_HOLD_DONE))
                {
                    component.GetUserState<BufferStateOccupy>(UserState.USTATE_HOLD_DONE).RefreshState();
                }
                if (component.ContainsState(UserState.USTATE_HOLD_TRANSFORM))
                {
                    component.GetUserState<BufferStateOccupy>(UserState.USTATE_HOLD_TRANSFORM).RefreshState();
                }
                if (npc.hpdata != null)
                {
                    npc.hpdata.ResetData(npc.NpcData.MapNpcData);
                }
            }
        });
    }

    private UI_MainView mainView
    {
        get
        {
            return ControllerManager.Instance.GetController<MainUIController>().mainView;
        }
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if (this.HoldNpcMap.Count == 0)
        {
            return;
        }
        TransformData transformData = null;
        this.npcInrange.Clear();
        this.HoldNpcMap.BetterForeach(delegate (int index, KeyValuePair<ulong, TransformData> item)
        {
            bool result;
            try
            {
                item.Value.Update();
                Npc npc2 = ManagerCenter.Instance.GetManager<EntitiesManager>().GetNpc(item.Value.Data.npc_tempid);
                if (npc2 != null && item.Value.CurrentTime > 0f && CommonTools.IsInRange(MainPlayer.Self.CurrentPosition2D.x, MainPlayer.Self.CurrentPosition2D.y, npc2.CurrentPosition2D.x, npc2.CurrentPosition2D.y, item.Value.Data.distance))
                {
                    this.npcInrange.Add(item.Value);
                }
                result = true;
            }
            catch (Exception)
            {
                result = false;
            }
            return result;
        });
        float num = 9999f;
        for (int i = 0; i < this.npcInrange.Count; i++)
        {
            Npc npc = ManagerCenter.Instance.GetManager<EntitiesManager>().GetNpc(this.npcInrange[i].Data.npc_tempid);
            if (npc != null)
            {
                float num2 = Vector2.Distance(MainPlayer.Self.CurrentPosition2D, npc.CurrentPosition2D);
                if (num2 < num)
                {
                    num = num2;
                    transformData = this.npcInrange[i];
                }
            }
        }
        if (transformData == null)
        {
            if (this.mainView != null)
            {
                this.mainView.CloseHoldOnCountDownTip();
            }
        }
        else if (this.mainView != null)
        {
            this.mainView.ShowHoldOnCountDownTip(transformData);
        }
    }

    public HoldNpcData GetTransformDatas(ulong tempid)
    {
        if (this.HoldNpcMap.ContainsKey(tempid))
        {
            return this.HoldNpcMap[tempid].Data;
        }
        return null;
    }

    public int GetRelationship(ulong npcid)
    {
        Npc npc = ManagerCenter.Instance.GetManager<EntitiesManager>().GetNpc(npcid);
        if (npc == null)
        {
            FFDebug.LogWarning(this, "n==null");
            return 2;
        }
        HoldNpcData transformDatas = this.GetTransformDatas(npcid);
        if (transformDatas == null)
        {
            return 2;
        }
        LuaTable cacheField_Table = LuaConfigManager.GetXmlConfigTable("holdnpc").GetCacheField_Table("holddata").GetCacheField_Table(npc.NpcData.MapNpcData.baseid.ToString());
        int result = 0;
        uint cacheField_Uint = cacheField_Table.GetCacheField_Table("hold").GetCacheField_Uint("holdtype");
        if (cacheField_Uint == 1U)
        {
            result = this.GetOccupyRelationshipWithMe(transformDatas.holduser, transformDatas.holdteam, transformDatas.holdguild, 1);
        }
        else if (cacheField_Uint == 2U)
        {
            result = this.GetOccupyRelationshipWithMe(transformDatas.holduser, transformDatas.holdteam, transformDatas.holdguild, 2);
        }
        else if (cacheField_Uint == 3U)
        {
            result = this.GetOccupyRelationshipWithMe(transformDatas.holduser, transformDatas.holdteam, transformDatas.holdguild, 3);
        }
        return result;
    }

    public int GetOccupyRelationshipWithMe(ulong playerid, ulong teamid, ulong guildid, int type)
    {
        switch (type)
        {
            case 1:
                if (playerid == MainPlayer.Self.OtherPlayerData.MapUserData.charid)
                {
                    return 0;
                }
                return 1;
            case 2:
                {
                    MSG_TeamMemeberList_SC myTeamInfo = ControllerManager.Instance.GetController<TeamController>().myTeamInfo;
                    if (myTeamInfo != null && myTeamInfo.mem.Count > 0)
                    {
                        if (teamid == (ulong)myTeamInfo.id)
                        {
                            return 0;
                        }
                        return 1;
                    }
                    else
                    {
                        if (teamid != 0UL)
                        {
                            return 1;
                        }
                        if (playerid != MainPlayer.Self.OtherPlayerData.MapUserData.charid)
                        {
                            return 1;
                        }
                        return 0;
                    }
                }
            case 3:
                {
                    ulong guildid2 = MainPlayer.Self.OtherPlayerData.MapUserData.mapdata.guildid;
                    if (guildid2 == 0UL)
                    {
                        if (playerid == MainPlayer.Self.BaseData.BaseData.id)
                        {
                            FFDebug.Log(this, FFLogType.Default, "GetOccupyRelationshipWithMe");
                            return 0;
                        }
                        FFDebug.Log(this, FFLogType.Default, "GetOccupyRelationshipWithMe");
                        return 1;
                    }
                    else
                    {
                        if (guildid == guildid2)
                        {
                            FFDebug.Log(this, FFLogType.Default, "GetOccupyRelationshipWithMe");
                            return 0;
                        }
                        FFDebug.Log(this, FFLogType.Default, "GetOccupyRelationshipWithMe");
                        return 1;
                    }
                }
            default:
                return 1;
        }
    }

    public void ReqPickUp(ulong thisid)
    {
        this.ReqHoldon(thisid, 1U, 19U);
    }

    public void HandlePickUpRet(MSG_Ret_Holdon_SC data)
    {
        EntitiesManager manager = ManagerCenter.Instance.GetManager<EntitiesManager>();
        Npc npc = manager.GetNpc(data.thisid);
        if (npc != null)
        {
            NpcPickCtrl component = npc.GetComponent<NpcPickCtrl>();
            component.FinishPick(data.retcode == 0U);
        }
    }

    public OccupyNetWork occupyNetWork;

    public Action<bool> OnHoldOnComplete;

    public BetterDictionary<ulong, TransformData> HoldNpcMap = new BetterDictionary<ulong, TransformData>();

    private List<TransformData> npcInrange = new List<TransformData>();
}
