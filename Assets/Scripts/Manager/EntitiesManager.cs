using System;
using System.Collections.Generic;
using Framework.Base;
using Framework.Managers;
using Game.Scene;
using LuaInterface;
using massive;
using msg;
using Net;
using UnityEngine;

public class EntitiesManager : IManager
{
    public void DoSendOrWaitSendWeGameRevenue(uint ratio)
    {
        if (this.PNetWork == null)
        {
            this.onPlayerNetWork = delegate ()
            {
                MSG_Ret_Wegame_Fcm_Info msg_Ret_Wegame_Fcm_Info2 = new MSG_Ret_Wegame_Fcm_Info();
                msg_Ret_Wegame_Fcm_Info2.ratio = ratio;
                this.PNetWork.SendMsg<MSG_Ret_Wegame_Fcm_Info>(CommandID.MSG_Ret_Wegame_Fcm_Info, msg_Ret_Wegame_Fcm_Info2, false);
                if (ManagerCenter.Instance.GetManager<EntitiesManager>().wegametest)
                {
                    FFDebug.LogError(this, "onPlayerNetWork PNetWork.SendMsg ratio:" + ratio);
                }
            };
        }
        else
        {
            MSG_Ret_Wegame_Fcm_Info msg_Ret_Wegame_Fcm_Info = new MSG_Ret_Wegame_Fcm_Info();
            msg_Ret_Wegame_Fcm_Info.ratio = ratio;
            this.PNetWork.SendMsg<MSG_Ret_Wegame_Fcm_Info>(CommandID.MSG_Ret_Wegame_Fcm_Info, msg_Ret_Wegame_Fcm_Info, false);
            if (ManagerCenter.Instance.GetManager<EntitiesManager>().wegametest)
            {
                FFDebug.LogError(this, "PNetWork.SendMsg ratio:" + ratio);
            }
        }
    }

    public MainPlayer MainPlayer
    {
        get
        {
            return this._MainPlayer;
        }
        set
        {
            if (this._MainPlayer == null && value != null)
            {
                this._MainPlayer = value;
                if (this.onMainPlayer != null)
                {
                    this.onMainPlayer();
                    this.onMainPlayer = null;
                }
            }
            else
            {
                this._MainPlayer = value;
            }
        }
    }

    public CharactorBase SelectTarget
    {
        get
        {
            return this.m_selectTarget;
        }
        set
        {
            this.m_selectTarget = value;
        }
    }

    private GameScene gs
    {
        get
        {
            if (this.gs_ == null)
            {
                this.gs_ = ManagerCenter.Instance.GetManager<GameScene>();
            }
            return this.gs_;
        }
    }

    private UIMapController MapController
    {
        get
        {
            return ControllerManager.Instance.GetController<UIMapController>();
        }
    }

    public void Init()
    {
        if (this.PNetWork == null)
        {
            this.PNetWork = new PlayerNetWork();
            this.PNetWork.Initialize();
            if (this.onPlayerNetWork != null)
            {
                this.onPlayerNetWork();
                this.onPlayerNetWork = null;
            }
        }
    }

    public PlayerShowState PlayerStateInCompetition
    {
        get
        {
            return this.playershowState;
        }
    }

    public void setPlayerShowState(PlayerShowState bshow)
    {
        this.playershowState = bshow;
    }

    private Vector3 DismissYSize(Vector3 old)
    {
        return new Vector3(old.x, 0f, old.z);
    }

    public EntitiesManager.SeachResult[] SectorSeachBaseMainPlayer(float radius, float AngleSize)
    {
        this.SeachResultList.Clear();
        if (this.MainPlayer == null)
        {
            this.SeachResultList.ToArray();
        }
        if (this.MainPlayer.ModelObj == null)
        {
            this.SeachResultList.ToArray();
        }
        Vector3 vector = this.DismissYSize(this.MainPlayer.ModelObj.transform.position);
        Vector3 normalized = this.DismissYSize(this.MainPlayer.ModelObj.transform.forward).normalized;
        this.ALLCharactor.Clear();
        this.CurrentNineScreenPlayers.BetterForeach(delegate (KeyValuePair<ulong, OtherPlayer> pair)
        {
            this.ALLCharactor.Add(pair.Value);
        });
        this.NpcList.BetterForeach(delegate (KeyValuePair<ulong, Npc> pair)
        {
            this.ALLCharactor.Add(pair.Value);
        });
        for (int i = 0; i < this.ALLCharactor.Count; i++)
        {
            CharactorBase charactorBase = this.ALLCharactor[i];
            if (charactorBase != MainPlayer.Self)
            {
                if (charactorBase.CharState == CharactorState.CreatComplete)
                {
                    float num = Vector3.Distance(vector, this.DismissYSize(charactorBase.ModelObj.transform.position));
                    if (num <= radius)
                    {
                        Vector3 to = this.DismissYSize(charactorBase.ModelObj.transform.position) - vector;
                        float num2 = Vector3.Angle(normalized, to);
                        if (num2 <= AngleSize)
                        {
                            EntitiesManager.SeachResult seachResult = new EntitiesManager.SeachResult();
                            seachResult.Char = charactorBase;
                            seachResult.Distance = num;
                            seachResult.Angle = num2;
                            this.SeachResultList.Add(seachResult);
                        }
                    }
                }
            }
        }
        return this.SeachResultList.ToArray();
    }

    public EntitiesManager.SeachResult[] RectangleSeachBaseMainPlayer(float length, float width)
    {
        this.SeachResultList.Clear();
        if (this.MainPlayer == null)
        {
            this.SeachResultList.ToArray();
        }
        if (this.MainPlayer.ModelObj == null)
        {
            this.SeachResultList.ToArray();
        }
        Vector3 vector = this.DismissYSize(this.MainPlayer.ModelObj.transform.position);
        Vector3 normalized = this.DismissYSize(this.MainPlayer.ModelObj.transform.forward).normalized;
        this.ALLCharactor.Clear();
        this.CurrentNineScreenPlayers.BetterForeach(delegate (KeyValuePair<ulong, OtherPlayer> pair)
        {
            this.ALLCharactor.Add(pair.Value);
        });
        this.NpcList.BetterForeach(delegate (KeyValuePair<ulong, Npc> pair)
        {
            this.ALLCharactor.Add(pair.Value);
        });
        for (int i = 0; i < this.ALLCharactor.Count; i++)
        {
            CharactorBase charactorBase = this.ALLCharactor[i];
            if (charactorBase != MainPlayer.Self)
            {
                if (charactorBase.CharState == CharactorState.CreatComplete)
                {
                    float num = Vector3.Distance(vector, this.DismissYSize(charactorBase.ModelObj.transform.position));
                    Vector3 to = this.DismissYSize(charactorBase.ModelObj.transform.position) - vector;
                    float num2 = Vector3.Angle(normalized, to);
                    if (num2 <= 90f)
                    {
                        float num3 = num * Mathf.Cos(num2 * 0.0174532924f);
                        float num4 = num * Mathf.Sin(num2 * 0.0174532924f);
                        if (num3 <= length)
                        {
                            if (num4 <= width)
                            {
                                EntitiesManager.SeachResult seachResult = new EntitiesManager.SeachResult();
                                seachResult.Char = charactorBase;
                                seachResult.Distance = num;
                                seachResult.Angle = num2;
                                seachResult.length = num3;
                                seachResult.width = num4;
                                this.SeachResultList.Add(seachResult);
                            }
                        }
                    }
                }
            }
        }
        return this.SeachResultList.ToArray();
    }

    public Npc SearchNearNPCById(uint npcId)
    {
        return this.SearchNearNPCById(npcId, Const.DistNpcVisitResponse + 5f);
    }

    public Npc SearchNearNPCById(uint npcId, float maxdistance)
    {
        Npc n = null;
        float distance = 9999f;
        this.NpcList.BetterForeach(delegate (KeyValuePair<ulong, Npc> pair)
        {
            if (pair.Value.CharState == CharactorState.CreatComplete)
            {
                cs_MapNpcData mapNpcData = pair.Value.NpcData.MapNpcData;
                if (mapNpcData.baseid == npcId)
                {
                    Vector2 serverPosByWorldPos = GraphUtils.GetServerPosByWorldPos(MainPlayer.Self.ModelObj.transform.position, true);
                    Vector2 serverPosByWorldPos2 = GraphUtils.GetServerPosByWorldPos(pair.Value.ModelObj.transform.position, true);
                    float num = Vector2.Distance(serverPosByWorldPos, serverPosByWorldPos2);
                    if (num < distance && (num < maxdistance || maxdistance == -1f))
                    {
                        distance = num;
                        n = pair.Value;
                    }
                }
            }
        });
        return n;
    }

    public RelationType CheckRelationBaseMainPlayer(CharactorBase Char)
    {
        RelationType relationType = RelationType.None;
        if (Char == null)
        {
            return relationType;
        }
        if (Char == this.MainPlayer)
        {
            Char.rlationType = RelationType.Self;
            return RelationType.Self;
        }
        if (Char.CharState != CharactorState.CreatComplete)
        {
            Char.rlationType = relationType;
            return relationType;
        }
        if (Char.GetComponent<PlayerBufferControl>() != null && Char.GetComponent<PlayerBufferControl>().ContainsState(UserState.USTATE_PEACE))
        {
            Char.rlationType = RelationType.Friend;
            return RelationType.Friend;
        }
        if (Char is OtherPlayer)
        {
            relationType = this.GetOtherPlayerRelationType(Char as OtherPlayer);
        }
        if (Char is Npc)
        {
            relationType = this.GetNpcRelationType(Char as Npc);
        }
        Char.rlationType = relationType;
        return relationType;
    }

    public RelationType GetNpcRelationType(Npc npc)
    {
        if (this.gs.isAbattoirScene)
        {
            return ControllerManager.Instance.GetController<AbattoirMatchController>().GetNpcRelationTypeToMainPlayer(npc);
        }
        RelationType result = RelationType.None;
        if (npc == null)
        {
            return result;
        }
        if (npc.CharState != CharactorState.CreatComplete)
        {
            return result;
        }
        NpcData npcData = npc.NpcData;
        LuaTable configTable = LuaConfigManager.GetConfigTable("npc_data", (ulong)npcData.MapNpcData.baseid);
        if (configTable == null)
        {
            return result;
        }
        switch (configTable.GetCacheField_Uint("kind"))
        {
            case 1U:
            case 11U:
            case 20U:
            case 21U:
                result = RelationType.Friend;
                break;
            case 2U:
            case 22U:
                if (npc.GetComponent<PlayerBufferControl>().ContainsState(UserState.USTATE_BATTLE))
                {
                    if (MainPlayer.Self != null && npc.IsInHatredList(MainPlayer.Self.OtherPlayerData.MapUserData.charid))
                    {
                        result = RelationType.Enemy;
                    }
                    else
                    {
                        result = RelationType.Neutral;
                    }
                }
                else
                {
                    if (MainPlayer.Self != null && npc.IsInHatredList(MainPlayer.Self.OtherPlayerData.MapUserData.charid))
                    {
                        return RelationType.Enemy;
                    }
                    result = RelationType.Neutral;
                }
                break;
            case 3U:
            case 4U:
            case 5U:
                result = RelationType.Enemy;
                break;
            case 6U:
                if (!string.IsNullOrEmpty(npcData.MapNpcData.Owner))
                {
                    if (npcData.MapNpcData.Owner.Equals(MainPlayer.Self.OtherPlayerData.MapUserData.name))
                    {
                        result = RelationType.Friend;
                    }
                    else
                    {
                        result = RelationType.Enemy;
                    }
                }
                break;
            case 9U:
                if (npc.NpcData.MapNpcData.MasterData != null)
                {
                    if (MainPlayer.Self.OtherPlayerData.MapUserData.charid == npc.NpcData.MapNpcData.MasterData.Eid.Id)
                    {
                        result = RelationType.Friend;
                    }
                    else
                    {
                        OtherPlayer otherPlayer = this.GetCharactorByID(npc.NpcData.MapNpcData.MasterData.Eid.Id, CharactorType.Player) as OtherPlayer;
                        if (otherPlayer != null)
                        {
                            result = this.GetOtherPlayerRelationType(otherPlayer);
                        }
                    }
                }
                break;
            case 10U:
            case 17U:
            case 25U:
                if (npc.NpcData.MapNpcData.MasterData != null)
                {
                    OtherPlayer otherPlayer2 = this.GetCharactorByID(npc.NpcData.MapNpcData.MasterData.Eid.Id, CharactorType.Player) as OtherPlayer;
                    if (otherPlayer2 is MainPlayer)
                    {
                        result = RelationType.Friend;
                    }
                    else if (otherPlayer2 != null)
                    {
                        result = this.GetOtherPlayerRelationType(otherPlayer2);
                    }
                    else
                    {
                        result = RelationType.Self;
                    }
                }
                break;
        }
        return result;
    }

    public RelationType GetOtherPlayerRelationType(OtherPlayer Char)
    {
        if (this.gs.isAbattoirScene)
        {
            return ControllerManager.Instance.GetController<AbattoirMatchController>().GetOtherPlayerRelationTypeToMainPlayer(Char);
        }
        cs_CharacterMapData mapdata = Char.OtherPlayerData.MapUserData.mapdata;
        return this.GetOtherPlayerRelationType(mapdata);
    }

    public RelationType GetOtherPlayerRelationType(cs_CharacterMapData mapdata)
    {
        RelationType result = RelationType.None;
        if (MainPlayer.Self == null)
        {
            return result;
        }
        switch (MainPlayer.Self.MainPlayeData.CharacterBaseData.pkmode)
        {
            case 0U:
                break;
            case 1U:
                result = RelationType.Friend;
                break;
            case 2U:
                result = RelationType.Enemy;
                break;
            case 3U:
                if (mapdata.guildid == 0UL || MainPlayer.Self.OtherPlayerData.MapUserData.mapdata.guildid == 0UL)
                {
                    result = RelationType.Enemy;
                }
                else if (mapdata.guildid != MainPlayer.Self.OtherPlayerData.MapUserData.mapdata.guildid)
                {
                    result = RelationType.Enemy;
                }
                else
                {
                    result = RelationType.Friend;
                }
                break;
            case 4U:
                if (mapdata.teamid == 0U || MainPlayer.Self.OtherPlayerData.MapUserData.mapdata.teamid == 0U)
                {
                    result = RelationType.Enemy;
                }
                else if (mapdata.teamid != MainPlayer.Self.OtherPlayerData.MapUserData.mapdata.teamid)
                {
                    result = RelationType.Enemy;
                }
                else
                {
                    result = RelationType.Friend;
                }
                break;
            case 5U:
                if (MainPlayer.Self.OtherPlayerData.MapUserData.mapdata.guildid != 0UL && mapdata.guildid == MainPlayer.Self.OtherPlayerData.MapUserData.mapdata.guildid)
                {
                    result = RelationType.Friend;
                }
                else if (mapdata.teamid != 0U && mapdata.teamid == MainPlayer.Self.OtherPlayerData.MapUserData.mapdata.teamid)
                {
                    result = RelationType.Friend;
                }
                else if (MainPlayer.Self.OtherPlayerData.MapUserData.mapdata.country == Const.CampNeutralID || mapdata.country == Const.CampNeutralID)
                {
                    result = RelationType.Neutral;
                }
                else if (MainPlayer.Self.OtherPlayerData.MapUserData.mapdata.country != Const.CampNeutralID && MainPlayer.Self.OtherPlayerData.MapUserData.mapdata.country == mapdata.country)
                {
                    result = RelationType.Neutral;
                }
                else if (MainPlayer.Self.OtherPlayerData.MapUserData.mapdata.country != Const.CampNeutralID && mapdata.country == Const.CampNeutralID)
                {
                    result = RelationType.Neutral;
                }
                else if (MainPlayer.Self.OtherPlayerData.MapUserData.mapdata.country != Const.CampNeutralID && MainPlayer.Self.OtherPlayerData.MapUserData.mapdata.country != mapdata.country)
                {
                    result = RelationType.Enemy;
                }
                else
                {
                    result = RelationType.Neutral;
                }
                break;
            default:
                result = RelationType.Neutral;
                break;
        }
        return result;
    }

    public float GetDistance(CharactorBase A, CharactorBase B)
    {
        return Vector3.Distance(this.DismissYSize(A.ModelObj.transform.position), this.DismissYSize(B.ModelObj.transform.position));
    }

    public void OnMainCameraChange()
    {
        this.CurrentNineScreenPlayers.BetterForeach(delegate (KeyValuePair<ulong, OtherPlayer> pair)
        {
            pair.Value.OnMainCameraChanged();
        });
        this.NpcList.BetterForeach(delegate (KeyValuePair<ulong, Npc> pair)
        {
            pair.Value.OnMainCameraChanged();
        });
    }

    public void SummonNpc(uint id, uint num, uint script)
    {
        List<TempNpcInfo> list = new List<TempNpcInfo>();
        list.Add(new TempNpcInfo
        {
            npcid = id,
            num = num,
            script = script
        });
        this.PNetWork.ReqSummonNpc(list);
    }

    public void AddCharacter(OtherPlayer player)
    {
        FFDebug.Log(this, FFLogType.Player, "AddPlayer: " + player.EID);
        if (!this.CurrentNineScreenPlayers.ContainsKey(player.EID.Id))
        {
            this.CurrentNineScreenPlayers[player.EID.Id] = player;
            bool flag = EntitiesManager.RefreshTeamMap(player);
            if (player != MainPlayer.Self && !flag)
            {
                RelationType rt = this.CheckRelationBaseMainPlayer(player);
                GameMap.ItemType it = this.MapController.GetItemTypeByRelationType(rt);
                this.MapController.SetItemIconInfoByItemPlayerAndItemType(player, it);
                player.OnMoveDataChange = null;
                OtherPlayer player2 = player;
                player2.OnMoveDataChange = (Action<CharactorBase>)Delegate.Combine(player2.OnMoveDataChange, new Action<CharactorBase>(delegate (CharactorBase cb)
                {
                    this.MapController.SetItemIconInfoByItemPlayerAndItemType(cb, it);
                }));
                OtherPlayer player3 = player;
                player3.OnDestroyThisInNineScreen = (Action<CharactorBase>)Delegate.Combine(player3.OnDestroyThisInNineScreen, new Action<CharactorBase>(delegate (CharactorBase cb)
                {
                    this.MapController.DeleteIcon(cb, it);
                }));
            }
            this.DoEentityActionOrCache(player.EID, delegate (CharactorBase basePlayer)
            {
                if (this.bdicCurrentHidePlayer.ContainsKey(player.EID.Id))
                {
                    this.bdicCurrentHidePlayer.Remove(player.EID.Id);
                }
                if (!this.bdicCurrentVisiblePlayer.ContainsKey(player.EID.Id))
                {
                    this.bdicCurrentVisiblePlayer.Add(player.EID.Id, player);
                }
                if (!this.bEnablePlayerLimit || this.IsAlwaysShowPlayer(player) || this.bdicCurrentVisiblePlayer.Count < this.nCurrentLimitCount)
                {
                    this.ShowPlayer(player);
                }
                else
                {
                    this.HidePlayer(player);
                }
            });
            this.StartCacheEentityAction(player.EID);
        }
    }

    private static bool RefreshTeamMap(OtherPlayer player)
    {
        GameScene manager = ManagerCenter.Instance.GetManager<GameScene>();
        ITeamController controller;
        if (manager.isAbattoirScene)
        {
            controller = ControllerManager.Instance.GetController<AbattoirMatchController>();
        }
        else
        {
            controller = ControllerManager.Instance.GetController<TeamController>();
        }
        bool flag = controller.IsMyTeamNember(player.EID);
        if (flag)
        {
            controller.RefreshTeamNemberInMap();
        }
        return flag;
    }

    public void RemoveCharacter(OtherPlayer player)
    {
        if (this.CurrentNineScreenPlayers.ContainsKey(player.EID.Id))
        {
            this.CurrentNineScreenPlayers.Remove(player.EID.Id);
            EntitiesManager.RefreshTeamMap(player);
        }
        if (player.OtherPlayerData != null && player.OtherPlayerData.MapUserData != null)
        {
            RelationType otherPlayerRelationType = this.GetOtherPlayerRelationType(player);
            RelationType relationType = otherPlayerRelationType;
            if (relationType != RelationType.Friend)
            {
                if (relationType == RelationType.Enemy)
                {
                    this.MapController.DeleteIcon(player, GameMap.ItemType.Enemy);
                }
            }
            else
            {
                this.MapController.DeleteIcon(player, GameMap.ItemType.Friend);
            }
        }
        if (this.bdicCurrentHidePlayer.ContainsKey(player.EID.Id))
        {
            this.bdicCurrentHidePlayer.Remove(player.EID.Id);
        }
        if (this.bdicCurrentVisiblePlayer.ContainsKey(player.EID.Id))
        {
            this.bdicCurrentVisiblePlayer.Remove(player.EID.Id);
            this.SetVisiblePlayerLimit(this.nCurrentLimitCount);
        }
        this.ClearEentityActionCache(player.EID);
    }

    public void SetVisiblePlayerLimit(int nPlayerLimit)
    {
        if (this.nMaxPlayerLimitCount < nPlayerLimit)
        {
            nPlayerLimit = this.nMaxPlayerLimitCount;
        }
        else if (this.nMinPlayerLimitCount > nPlayerLimit)
        {
            nPlayerLimit = this.nMinPlayerLimitCount;
        }
        this.nCurrentLimitCount = nPlayerLimit;
        this.bdicCurrentHidePlayer.BetterForeach(delegate (KeyValuePair<ulong, OtherPlayer> item)
        {
            if (!this.bEnablePlayerLimit || this.IsAlwaysShowPlayer(item.Value) || this.bdicCurrentVisiblePlayer.Count < this.nCurrentLimitCount)
            {
                this.ShowPlayer(item.Value);
            }
        });
        this.bdicCurrentVisiblePlayer.BetterForeach(delegate (KeyValuePair<ulong, OtherPlayer> item)
        {
            if (this.bEnablePlayerLimit && this.bdicCurrentVisiblePlayer.Count > this.nCurrentLimitCount && !this.IsAlwaysShowPlayer(item.Value))
            {
                this.HidePlayer(item.Value);
            }
        });
    }

    public void ShowPlayer(OtherPlayer player)
    {
        if (player == null)
        {
            return;
        }
        if (this.bdicCurrentHidePlayer.ContainsKey(player.EID.Id))
        {
            this.bdicCurrentHidePlayer.Remove(player.EID.Id);
        }
        if (!this.bdicCurrentVisiblePlayer.ContainsKey(player.EID.Id))
        {
            this.bdicCurrentVisiblePlayer.Add(player.EID.Id, player);
        }
        PlayerClientStateControl component = player.GetComponent<PlayerClientStateControl>();
        if (component != null)
        {
            component.SetPlayerState(PlayerShowState.show);
        }
        int num = this.bdicCurrentVisiblePlayer.Count - this.nCurrentLimitCount;
        int count = this.lstCurVisNotAlwaysPlayerID.Count;
        for (int i = 0; i < num; i++)
        {
            for (int j = 0; j < count; j++)
            {
                if (i < j)
                {
                    return;
                }
                OtherPlayer player2 = null;
                if (this.bdicCurrentVisiblePlayer.TryGetValue(this.lstCurVisNotAlwaysPlayerID[j], out player2))
                {
                    this.HidePlayer(player2);
                    this.lstCurVisNotAlwaysPlayerID.Remove(this.lstCurVisNotAlwaysPlayerID[j]);
                }
            }
        }
    }

    public void HidePlayer(OtherPlayer player)
    {
        if (player == null)
        {
            return;
        }
        PlayerClientStateControl component = player.GetComponent<PlayerClientStateControl>();
        if (component == null)
        {
            return;
        }
        if (this.bdicCurrentVisiblePlayer.ContainsKey(player.EID.Id))
        {
            this.bdicCurrentVisiblePlayer.Remove(player.EID.Id);
        }
        if (!this.bdicCurrentHidePlayer.ContainsKey(player.EID.Id))
        {
            this.bdicCurrentHidePlayer.Add(player.EID.Id, player);
        }
        component.SetPlayerState(PlayerShowState.hide);
        MainPlayerTargetSelectMgr component2 = MainPlayer.Self.GetComponent<MainPlayerTargetSelectMgr>();
        if (component2 != null && component2.manualSelect.Target == player)
        {
            component2.SetTargetNull();
        }
    }

    private bool IsAlwaysShowPlayer(OtherPlayer player)
    {
        bool flag = false;
        if (player == null)
        {
            return flag;
        }
        flag = (ControllerManager.Instance.GetController<TeamController>().IsMyTeamNember(player.EID) || this.CheckRelationBaseMainPlayer(player) == RelationType.Enemy || this.IsMainPlayer(player.EID.Id) || EntitiesManager.IsMasterOrApprentice(player.EID.Id));
        if (!flag && !this.lstCurVisNotAlwaysPlayerID.Contains(player.EID.Id))
        {
            this.lstCurVisNotAlwaysPlayerID.Add(player.EID.Id);
        }
        if (flag && this.lstCurVisNotAlwaysPlayerID.Contains(player.EID.Id))
        {
            this.lstCurVisNotAlwaysPlayerID.Remove(player.EID.Id);
        }
        return flag;
    }

    public static bool IsMasterOrApprentice(ulong uID)
    {
        object[] array = LuaScriptMgr.Instance.CallLuaFunction("MentoringCtrl.IsMasterOrApprentice", new object[]
        {
            Util.GetLuaTable("MentoringCtrl"),
            uID.ToString()
        });
        if (array != null && array.Length != 0)
        {
            int num = int.Parse(array[0].ToString());
            return num == 1 || num == 2;
        }
        return false;
    }

    public void SetCurrentMapEnablePlayerLimit(uint nMapID)
    {
        int cacheField_Int = LuaConfigManager.GetXmlConfigTable("scenesinfo").GetCacheField_Table("mapinfo").GetCacheField_Table(nMapID.ToString()).GetCacheField_Int("enablePlayerLimit");
        if (0 >= cacheField_Int)
        {
            this.bEnablePlayerLimit = false;
        }
        else
        {
            this.bEnablePlayerLimit = true;
        }
    }

    public void SetPlayerCountLimit(int nMin, int nMax)
    {
        this.nMinPlayerLimitCount = nMin;
        this.nMaxPlayerLimitCount = nMax;
    }

    public void SetPlayerNameActive(bool bActive)
    {
        this.CurrentNineScreenPlayers.BetterForeach(delegate (KeyValuePair<ulong, OtherPlayer> item)
        {
            PlayerClientStateControl component = item.Value.GetComponent<PlayerClientStateControl>();
            if (component != null)
            {
                component.ActiveShowPlayerUIInfo(bActive, false);
            }
        });
    }

    public void SetNPCActive(bool bActive)
    {
        this.NpcList.BetterForeach(delegate (KeyValuePair<ulong, Npc> item)
        {
            PlayerClientStateControl component = item.Value.GetComponent<PlayerClientStateControl>();
            if (component != null)
            {
                if (!bActive && !GameSystemSettings.IsMainPlayerInBattleState())
                {
                    component.SetPlayerState(PlayerShowState.hide);
                }
                else
                {
                    component.SetPlayerState(PlayerShowState.show);
                }
            }
        });
    }

    public OtherPlayer GetPlayerByID(ulong charid)
    {
        OtherPlayer otherPlayer = null;
        if (this.CurrentNineScreenPlayers.ContainsKey(charid))
        {
            otherPlayer = this.CurrentNineScreenPlayers[charid];
        }
        if (otherPlayer == null)
        {
        }
        return otherPlayer;
    }

    public CharactorBase GetCharactorByID(ulong charid, CharactorType chartype)
    {
        if (chartype == CharactorType.Player)
        {
            return this.GetPlayerByID(charid);
        }
        if (chartype == CharactorType.NPC)
        {
            return this.GetNpc(charid);
        }
        FFDebug.LogWarning(this, string.Concat(new object[]
        {
            "Cont Find :",
            charid,
            "--chartype:",
            chartype
        }));
        return null;
    }

    public CharactorBase GetCharactorByID(EntryIDType Entryident)
    {
        return this.GetCharactorByID(Entryident.id, (CharactorType)Entryident.type);
    }

    public CharactorBase GetCharactorByID(EntitiesID Entryident)
    {
        return this.GetCharactorByID(Entryident.Id, Entryident.Etype);
    }

    public Npc[] GetNpcsByBaseidInFun(uint baseid)
    {
        this.Tmplist.Clear();
        this.FuncNpcMap.BetterForeach(delegate (KeyValuePair<ulong, Npc> item)
        {
            if (baseid == item.Value.NpcData.MapNpcData.baseid)
            {
                this.Tmplist.Add(item.Value);
            }
        });
        return this.Tmplist.ToArray();
    }

    public void AddNpc(Npc npc)
    {
        if (!this.NpcList.ContainsValue(npc))
        {
            this.NpcList[npc.EID.Id] = npc;
            if (!this.IsFunNpc(npc))
            {
                Npc npc2 = npc;
                npc2.OnMoveDataChange = (Action<CharactorBase>)Delegate.Combine(npc2.OnMoveDataChange, new Action<CharactorBase>(delegate (CharactorBase CB)
                {
                    this.MapController.SetNpcIconInfo(CB as Npc);
                }));
            }
            if (npc.NpcData.GetAppearanceid() == 12100U)
            {
                this.MapController.SetNpcIconInfo_Capsule(npc);
            }
            if (npc.GetNpcConfig().GetField_Uint("kind") == 1U)
            {
                this.bdicNpcKindOne.Add(npc.EID.Id, npc);
                this.DoEentityActionOrCache(npc.EID, delegate (CharactorBase basePlayer)
                {
                    PlayerClientStateControl component = npc.GetComponent<PlayerClientStateControl>();
                    if (component != null)
                    {
                        if (GameSystemSettings.IsHideNpc() && !GameSystemSettings.IsMainPlayerInBattleState())
                        {
                            component.SetPlayerState(PlayerShowState.hide);
                        }
                        else
                        {
                            component.SetPlayerState(PlayerShowState.show);
                        }
                    }
                });
            }
            this.DoEentityActionOrCache(npc.EID, delegate (CharactorBase basePlayer)
            {
                npc.InitShowState();
            });
            this.StartCacheEentityAction(npc.EID);
        }
    }

    public void AddFunNpc(Npc npc)
    {
        if (!this.FuncNpcMap.ContainsKey(npc.EID.Id))
        {
            this.FuncNpcMap.Add(npc.EID.Id, npc);
            this.MapController.SetNpcIconInfo(npc);
        }
    }

    public void ClearFunNpc()
    {
        this.FuncNpcMap.Clear();
    }

    public bool IsFunNpc(Npc npc)
    {
        return this.FuncNpcMap.ContainsKey(npc.EID.Id);
    }

    public void RemoveNpc(Npc npc)
    {
        if (this.NpcList.ContainsKey(npc.EID.Id))
        {
            this.NpcList.Remove(npc.EID.Id);
            if (this.bdicNpcKindOne.ContainsKey(npc.EID.Id))
            {
                this.bdicNpcKindOne.Remove(npc.EID.Id);
            }
            if (!this.IsFunNpc(npc))
            {
                this.MapController.DeleteIcon(npc, GameMap.ItemType.NPC);
            }
            if (npc.NpcData.GetAppearanceid() == 12100U)
            {
                this.MapController.DeleteNpcIconInfo_Capsule(npc.EID.Id.ToString());
            }
        }
        this.ClearEentityActionCache(npc.EID);
        if (MainPlayer.Self != null && MainPlayer.Self.GetComponent<FFDetectionNpcControl>() != null)
        {
            MainPlayer.Self.GetComponent<FFDetectionNpcControl>().DetectVisitNpc();
        }
    }

    public string LogOutALLFuncNpcMap()
    {
        string text = "FuncNpcMap------Count->" + this.FuncNpcMap.Count;
        foreach (KeyValuePair<ulong, Npc> keyValuePair in this.FuncNpcMap)
        {
            text = text + "\n" + keyValuePair.Value.EID.ToString();
        }
        return text;
    }

    public void RefreshFuncNpcList(List<Npc> funcNpcs)
    {
    }

    public Npc GetNpc(ulong id)
    {
        Npc result = null;
        if (this.NpcList.ContainsKey(id))
        {
            result = this.NpcList[id];
        }
        return result;
    }

    public CharactorBase GetCharactorFromGameObject(GameObject obj)
    {
        CharactorBase cb = null;
        this.CurrentNineScreenPlayers.BetterForeach(delegate (KeyValuePair<ulong, OtherPlayer> pair)
        {
            if (pair.Value.CharState == CharactorState.CreatComplete && pair.Value.ModelObj == obj)
            {
                cb = pair.Value;
            }
        });
        this.NpcList.BetterForeach(delegate (KeyValuePair<ulong, Npc> pair)
        {
            Npc value = pair.Value;
            if (value.CharState == CharactorState.CreatComplete && value.ModelObj == obj)
            {
                cb = value;
            }
        });
        return cb;
    }

    public CharactorBase GetCharactorFromCharid(ulong charid)
    {
        CharactorBase result = null;
        if (this.CurrentNineScreenPlayers.ContainsKey(charid))
        {
            result = this.CurrentNineScreenPlayers[charid];
        }
        if (this.NpcList.ContainsKey(charid))
        {
            result = this.NpcList[charid];
        }
        return result;
    }

    public void UnLoadCharactors()
    {
        if (this.CurrentNineScreenPlayers == null)
        {
            return;
        }
        this.CurrentNineScreenPlayers.BetterForeach(delegate (KeyValuePair<ulong, OtherPlayer> pair)
        {
            OtherPlayer value = pair.Value;
            if (this.IsMainPlayer(value.GetCharID()))
            {
                value.StopMoveImmediate(null);
            }
            else
            {
                value.DestroyThisInNineScreen();
            }
        });
        this.NpcList.BetterForeach(delegate (KeyValuePair<ulong, Npc> pair)
        {
            pair.Value.DestroyThisInNineScreen();
        });
        this.FuncNpcMap.BetterForeach(delegate (KeyValuePair<ulong, Npc> item)
        {
            item.Value.DestroyThisInNineScreen();
        });
        this.NpcList.Clear();
        this.FuncNpcMap.Clear();
        this.bdicCurrentVisiblePlayer.Clear();
        this.bdicCurrentHidePlayer.Clear();
        this.bdicNpcKindOne.Clear();
        this.lstCurVisNotAlwaysPlayerID.Clear();
        if (MainPlayer.Self != null && MainPlayer.Self.GetComponent<FFDetectionNpcControl>() != null)
        {
            MainPlayer.Self.GetComponent<FFDetectionNpcControl>().DetectVisitNpc();
        }
        ManagerCenter.Instance.GetManager<ObjectPoolManager>().ClearCorpse();
    }

    public bool IsMainPlayer(ulong charid)
    {
        return this.MainPlayer != null && charid == this.MainPlayer.GetCharID();
    }

    public void UnInitialize()
    {
        this.CurrentNineScreenPlayers.Clear();
        this.CurrentNineScreenPlayers = null;
        this.NpcList.Clear();
        this.NpcList = null;
        this.FuncNpcMap.Clear();
        this.FuncNpcMap = null;
        this.bdicCurrentVisiblePlayer.Clear();
        this.bdicCurrentVisiblePlayer = null;
        this.bdicCurrentHidePlayer.Clear();
        this.bdicCurrentHidePlayer = null;
        this.bdicNpcKindOne.Clear();
        this.bdicNpcKindOne = null;
        this.lstCurVisNotAlwaysPlayerID.Clear();
        if (MainPlayer.Self != null && MainPlayer.Self.GetComponent<FFDetectionNpcControl>() != null)
        {
            MainPlayer.Self.GetComponent<FFDetectionNpcControl>().DetectVisitNpc();
        }
    }

    public string ManagerName
    {
        get
        {
            return base.GetType().ToString();
        }
    }

    public void OnUpdate()
    {
    }

    public void OnReSet()
    {
        if (this.MainPlayer != null)
        {
            this.MainPlayer.TrueDestroy();
        }
        this.MainPlayer = null;
        this.PNetWork.OnReLogin();
        this.UnLoadCharactors();
        this.ClearAllEentityActionCache();
    }

    public void StartCacheEentityAction(EntitiesID EID)
    {
        if (!EntitiesManager.EentityActionPoolMap.ContainsKey(EID.Id))
        {
            EntitiesManager.EentityActionPoolMap[EID.Id] = ClassPool.GetObject<EentityActionPool>();
            EntitiesManager.EentityActionPoolMap[EID.Id].Eid = EID;
        }
    }

    public void RunAllEentityActionCacheAndClear(EntitiesID EID)
    {
        if (EntitiesManager.EentityActionPoolMap.ContainsKey(EID.Id))
        {
            CharactorBase charactorByID = this.GetCharactorByID(EID);
            if (charactorByID != null && charactorByID.CharState == CharactorState.CreatComplete)
            {
                EntitiesManager.EentityActionPoolMap[EID.Id].RunAllAction(charactorByID);
            }
            EntitiesManager.EentityActionPoolMap[EID.Id].Dispose();
            EntitiesManager.EentityActionPoolMap.Remove(EID.Id);
            if (EID.Id == 119818UL)
            {
                Debug.LogError("================================= RunAllEentityActionCacheAndClear remove otherplayer");
            }
        }
    }

    public bool IsAbattoirSceneRelive(string idstr, int charactorType, Action<bool> action)
    {
        if (this.gs.isAbattoirScene)
        {
            bool obj = MainPlayer.Self.GetComponent<PlayerBufferControl>().ContainsState(1351);
            action(obj);
            return true;
        }
        return false;
    }

    public void DoEentityActionOrCacheForLua(string idstr, int charactorType, Action<CharactorBase> action)
    {
        if (this.gs.isAbattoirScene)
        {
            if (MainPlayer.Self == null)
            {
                return;
            }
            PlayerBufferControl component = MainPlayer.Self.ComponentMgr.GetComponent<PlayerBufferControl>();
            if (component.ContainsState(1362))
            {
                return;
            }
            AbattoirMatchController controller = ControllerManager.Instance.GetController<AbattoirMatchController>();
            if (controller.getFightState == AbattoirFightState.USTATE_REWARD)
            {
                return;
            }
        }
        if (this.MainPlayer != null)
        {
            action(this.MainPlayer);
            return;
        }
        ulong num = 0UL;
        if (ulong.TryParse(idstr, out num) && num != 0UL)
        {
            this.DoEentityActionOrCache(new EntitiesID(num, (CharactorType)charactorType), action);
        }
    }

    public void DoEentityActionOrCache(EntitiesID EID, Action<CharactorBase> action)
    {
        CharactorBase charactorByID = this.GetCharactorByID(EID);
        if (charactorByID != null && charactorByID.CharState == CharactorState.CreatComplete)
        {
            action(charactorByID);
        }
        else
        {
            if (!EntitiesManager.EentityActionPoolMap.ContainsKey(EID.Id))
            {
                this.StartCacheEentityAction(EID);
            }
            EntitiesManager.EentityActionPoolMap[EID.Id].AddAction(action);
        }
    }

    public void ClearEentityActionCache(EntitiesID EID)
    {
        if (EntitiesManager.EentityActionPoolMap.ContainsKey(EID.Id))
        {
            EntitiesManager.EentityActionPoolMap[EID.Id].Dispose();
            EntitiesManager.EentityActionPoolMap.Remove(EID.Id);
        }
    }

    public void ClearAllEentityActionCache()
    {
        EntitiesManager.EentityActionPoolMap.BetterForeach(delegate (KeyValuePair<ulong, EentityActionPool> item)
        {
            item.Value.Dispose();
        });
        EntitiesManager.EentityActionPoolMap.Clear();
    }

    public void RefreshNPCShowState()
    {
        if (this.NpcList == null)
        {
            return;
        }
        this.NpcList.BetterForeach(delegate (KeyValuePair<ulong, Npc> pair)
        {
            pair.Value.InitShowState();
            if (this.MapController != null && !this.IsFunNpc(pair.Value))
            {
                this.MapController.SetNpcIconInfo(pair.Value);
            }
        });
        if (MainPlayer.Self != null && MainPlayer.Self.GetComponent<FFDetectionNpcControl>() != null)
        {
            MainPlayer.Self.GetComponent<FFDetectionNpcControl>().DetectVisitNpc();
        }
        this.FuncNpcMap.BetterForeach(delegate (KeyValuePair<ulong, Npc> item)
        {
            if (this.MapController != null)
            {
                this.MapController.SetNpcIconInfo(item.Value);
            }
        });
    }

    public bool wegame;

    public bool wegametest;

    private Action onPlayerNetWork;

    public PlayerNetWork PNetWork;

    public int nMaxPlayerLimitCount = 50;

    public int nMinPlayerLimitCount;

    private int nCurrentLimitCount = 50;

    private bool bEnablePlayerLimit = true;

    public BetterDictionary<ulong, OtherPlayer> CurrentNineScreenPlayers = new BetterDictionary<ulong, OtherPlayer>();

    public BetterDictionary<ulong, OtherPlayer> bdicCurrentVisiblePlayer = new BetterDictionary<ulong, OtherPlayer>();

    public BetterDictionary<ulong, OtherPlayer> bdicCurrentHidePlayer = new BetterDictionary<ulong, OtherPlayer>();

    private List<ulong> lstCurVisNotAlwaysPlayerID = new List<ulong>();

    public BetterDictionary<ulong, Npc> NpcList = new BetterDictionary<ulong, Npc>();

    public BetterDictionary<ulong, Npc> FuncNpcMap = new BetterDictionary<ulong, Npc>();

    public BetterDictionary<ulong, Npc> bdicNpcKindOne = new BetterDictionary<ulong, Npc>();

    public Action onMainPlayer;

    private MainPlayer _MainPlayer;

    private CharactorBase m_selectTarget;

    private GameScene gs_;

    private PlayerShowState playershowState = PlayerShowState.show;

    private List<EntitiesManager.SeachResult> SeachResultList = new List<EntitiesManager.SeachResult>();

    private List<CharactorBase> ALLCharactor = new List<CharactorBase>();

    private List<Npc> Tmplist = new List<Npc>();

    private static BetterDictionary<ulong, EentityActionPool> EentityActionPoolMap = new BetterDictionary<ulong, EentityActionPool>();

    public class SeachResult
    {
        public CharactorBase Char;

        public float Distance;

        public float Angle;

        public float length;

        public float width;
    }
}
