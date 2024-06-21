using System;
using System.Collections;
using System.Collections.Generic;
using battle;
using Framework.Managers;
using Game.Scene;
using LuaInterface;
using Models;
using msg;
using UnityEngine;
using UnityEngine.UI;

internal class UIMapController : ControllerBase
{
    public UI_Map MapUI
    {
        get
        {
            return UIManager.GetUIObject<UI_Map>();
        }
    }

    public uint GetPathWayId(ulong mapId, uint npcId)
    {
        string key = mapId + "_" + npcId;
        if (this.mPathWayIdDic.ContainsKey(key))
        {
            return this.mPathWayIdDic[key];
        }
        return 0U;
    }

    public override string ControllerName
    {
        get
        {
            return base.GetType().ToString();
        }
    }

    private void EscCloseMapPanel()
    {
        if (this.MapUI != null)
        {
            this.MapUI.CloseBigMap(null);
        }
    }

    public void LoadView(Action Loadover)
    {
        if (this.MapUI != null)
        {
            this.InitMap();
            if (Loadover != null)
            {
                Loadover();
            }
        }
        else
        {
            ManagerCenter.Instance.GetManager<EscManager>().RegisterEscPanelCb("UI_Map", new EscPanelCb(this.EscCloseMapPanel));
            ManagerCenter.Instance.GetManager<EscManager>().RegisterHidePanel("UI_Map");
            ManagerCenter.Instance.GetManager<UIManager>().ShowUI<UI_Map>("UI_Map", delegate ()
            {
                this.InitPathConfig();
                this.InitMap();
                if (Loadover != null)
                {
                    Loadover();
                }
                Action action = this.onMapShow;
                if (action != null)
                {
                    action();
                    this.onMapShow = null;
                }
            }, UIManager.ParentType.Map, false);
        }
    }

    private void InitPathConfig()
    {
        if (this.mPathWayIdDic.Count == 0)
        {
            List<LuaTable> configTableList = LuaConfigManager.GetConfigTableList("pathway");
            for (int i = 0; i < configTableList.Count; i++)
            {
                string key = configTableList[i].GetField_Uint("mapid") + "_" + configTableList[i].GetField_Uint("npcid");
                uint field_Uint = configTableList[i].GetField_Uint("pathwayid");
                if (!this.mPathWayIdDic.ContainsKey(key))
                {
                    this.mPathWayIdDic.Add(key, field_Uint);
                }
            }
        }
    }

    public void InitMap()
    {
        if (this.MapUI != null)
        {
            this.MapUI.InitmapView();
            this.MapUI.InitPanelmapInUI();
            CopyManager manager = ManagerCenter.Instance.GetManager<CopyManager>();
            if (manager != null)
            {
                this.MapUI.minmaipTop.gameObject.SetActive(!manager.InCopy);
            }
        }
        Scheduler.Instance.AddFrame(10U, false, delegate
        {
            this.SetupCharactorPos(null);
        });
    }

    public int CompreName(string name1, string name2)
    {
        if (name1.Contains(name2))
        {
            return 1;
        }
        if (name1.Contains(name2))
        {
            return -1;
        }
        int length = name1.Length;
        if (length > name2.Length)
        {
            length = name2.Length;
        }
        for (int i = 0; i < length; i++)
        {
            if (name1[i] > name2[i])
            {
                return 1;
            }
            if (name1[i] < name2[i])
            {
                return -1;
            }
        }
        return 0;
    }

    public void ActiveNewMailTips(int status)
    {
        if (null == this.MapUI)
        {
            return;
        }
        this.MapUI.ActiveNewMailTips(1 == status);
    }

    public void SetMyPos(Vector2 pos, uint dir, bool isSetMeToCenterAreamap, bool isSetMeToCenterMinimap)
    {
        if (this.MapUI == null)
        {
            return;
        }
        if (this.AreamapMgr != null && this.MapUI.IsOnCurrAreamap)
        {
            this.AreamapMgr.SetIconMy(pos, dir);
            if (isSetMeToCenterAreamap)
            {
                this.AreamapMgr.SetMeToCenter();
            }
        }
        if (this.PanelmapMgr != null)
        {
            this.PanelmapMgr.SetIconMy(pos, dir);
            if (isSetMeToCenterMinimap)
            {
                this.PanelmapMgr.SetMeToCenter();
            }
        }
    }

    public void SetupCharactorPos(Action callback)
    {
        EntitiesManager EntitiesMgr = ManagerCenter.Instance.GetManager<EntitiesManager>();
        if (EntitiesMgr.MainPlayer == null)
        {
            return;
        }
        this.SetMyPos(EntitiesMgr.MainPlayer.CurrServerPos, EntitiesMgr.MainPlayer.ServerDir, true, true);
        EntitiesMgr.NpcList.BetterForeach(delegate (KeyValuePair<ulong, Npc> pair)
        {
            if (!EntitiesMgr.IsFunNpc(pair.Value))
            {
                this.SetNpcIconInfo(pair.Value);
            }
            else if (pair.Value.NpcData.GetAppearanceid() == 12100U)
            {
                this.SetNpcIconInfo_Capsule(pair.Value);
            }
        });
        EntitiesMgr.FuncNpcMap.BetterForeach(delegate (KeyValuePair<ulong, Npc> item)
        {
            this.SetNpcIconInfo(item.Value);
        });
        GameScene manager = ManagerCenter.Instance.GetManager<GameScene>();
        ITeamController teamController = null;
        if (manager.isAbattoirScene)
        {
            teamController = ControllerManager.Instance.GetController<AbattoirMatchController>();
            teamController.RefreshTeamNemberInMap();
        }
        else
        {
            teamController = ControllerManager.Instance.GetController<TeamController>();
            teamController.RefreshTeamNemberInMap();
        }
        EntitiesMgr.CurrentNineScreenPlayers.BetterForeach(delegate (KeyValuePair<ulong, OtherPlayer> pair)
        {
            OtherPlayer value = pair.Value;
            bool flag = teamController.IsMyTeamNember(value.EID);
            if (value != MainPlayer.Self && !flag)
            {
                RelationType rt = EntitiesMgr.CheckRelationBaseMainPlayer(value);
                GameMap.ItemType it = this.GetItemTypeByRelationType(rt);
                this.SetItemIconInfoByItemPlayerAndItemType(value, it);
                value.OnMoveDataChange = null;
                OtherPlayer otherPlayer = value;
                otherPlayer.OnMoveDataChange = (Action<CharactorBase>)Delegate.Combine(otherPlayer.OnMoveDataChange, new Action<CharactorBase>(delegate (CharactorBase cb)
                {
                    this.SetItemIconInfoByItemPlayerAndItemType(cb, it);
                }));
                OtherPlayer otherPlayer2 = value;
                otherPlayer2.OnDestroyThisInNineScreen = (Action<CharactorBase>)Delegate.Combine(otherPlayer2.OnDestroyThisInNineScreen, new Action<CharactorBase>(delegate (CharactorBase cb)
                {
                    this.DeleteIcon(cb, it);
                }));
            }
        });
        ControllerManager.Instance.GetController<TaskController>().ReqMapQuestInfo(callback);
    }

    public GameMap.ItemType GetItemTypeByRelationType(RelationType rt)
    {
        if (rt == RelationType.Friend)
        {
            return GameMap.ItemType.Friend;
        }
        if (rt != RelationType.Enemy)
        {
            return GameMap.ItemType.None;
        }
        return GameMap.ItemType.Enemy;
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if (this.MapUI == null)
        {
            return;
        }
        if (this.MapUI.IsAreamapShow)
        {
            this.RunningTime += Time.deltaTime;
            if (this.RunningTime > this.GetTeamPosInterval)
            {
                ControllerManager.Instance.GetController<TeamController>().ReqTeamMemberPos();
                this.RunningTime = 0f;
            }
        }
    }

    public void OnReceiveLinesData(MSG_NoticeClientAllLines_SC data)
    {
        this._lineData = data;
        this.RefreshLinesData();
    }

    public void RefreshLinesData()
    {
        if (null == this.MapUI || this._lineData == null)
        {
            return;
        }
        LuaTable cacheField_Table = LuaConfigManager.GetXmlConfigTable("System").GetCacheField_Table("line");
        IEnumerator enumerator = cacheField_Table.Values.GetEnumerator();
        this.MapUI.ResetLines();
        this._lineDic.Clear();
        this.currLineIndex = 0;
        this.lines = new List<Dropdown.OptionData>();
        for (int i = 0; i < this._lineData.lines.Count; i++)
        {
            Dropdown.OptionData line = new Dropdown.OptionData();
            enumerator.Reset();
            string format = string.Empty;
            int num = 1;
            while (enumerator.MoveNext())
            {
                object obj = enumerator.Current;
                LuaTable luaTable = obj as LuaTable;
                int field_Int = luaTable.GetField_Int("num");
                format = luaTable.GetField_String("des");
                num = field_Int;
                if ((ulong)this._lineData.lines[i].user_num <= (ulong)((long)field_Int))
                {
                    break;
                }
            }
            if (this._lineData.lines[i].index == this._lineData.your_line)
            {
                this.currLineIndex = i;
            }
            this._lineDic[i] = (int)this._lineData.lines[i].index;
            GameScene manager = ManagerCenter.Instance.GetManager<GameScene>();
            string str = string.Empty;
            if (manager != null)
            {
                str = manager.CurrentSceneData.showName();
            }
            line.text = string.Format(format, this._lineData.lines[i].index) + "  " + str;
            ManagerCenter.Instance.GetManager<UITextureMgr>().GetSpriteFromAtlas("frame", "linestate" + num, delegate (Sprite sprite)
            {
                if (sprite != null)
                {
                    line.image = sprite;
                }
            });
            this.lines.Add(line);
        }
        if (this.lines[this.currLineIndex].image != null)
        {
            this.MapUI.AddLine(this.lines, this.currLineIndex);
        }
        else
        {
            Scheduler.Instance.AddFrame(3U, true, new Scheduler.OnScheduler(this.WaitImgLoadFinish));
        }
    }

    private void WaitImgLoadFinish()
    {
        if (this.lines[this.currLineIndex].image != null)
        {
            this.MapUI.AddLine(this.lines, this.currLineIndex);
            Scheduler.Instance.RemoveFrame(new Scheduler.OnScheduler(this.WaitImgLoadFinish));
        }
    }

    public void OnSelectLine(int index)
    {
        if (!this._lineDic.ContainsKey(index))
        {
            return;
        }
        int num = this._lineDic[index];
        if ((long)num == (long)((ulong)this._lineData.your_line))
        {
            return;
        }
        this.MapUI.ResetBack((int)(this._lineData.your_line - 1U));
        ManagerCenter.Instance.GetManager<EntitiesManager>().PNetWork.ReqChangeLine(num);
    }

    public void SetMapText(uint Mapid)
    {
        LuaTable cacheField_Table = LuaConfigManager.GetXmlConfigTable("scenesinfo").GetCacheField_Table("maplabel");
        IEnumerator enumerator = cacheField_Table.Values.GetEnumerator();
        enumerator.Reset();
        while (enumerator.MoveNext())
        {
            object obj = enumerator.Current;
            LuaTable luaTable = obj as LuaTable;
            if (luaTable.GetField_Uint("mapID") == Mapid)
            {
                this.SetLabelIconInfo(luaTable);
            }
        }
    }

    public LuaTable GetNpcIconuimapinfo(Npc npc)
    {
        uint baseid = npc.NpcData.MapNpcData.baseid;
        uint field_Uint = LuaConfigManager.GetConfigTable("npc_data", (ulong)baseid).GetField_Uint("mapinfo");
        return LuaConfigManager.GetConfigTable("uimapinfo", (ulong)field_Uint);
    }

    public bool NpcHasIcon(uint baseid)
    {
        return LuaConfigManager.GetConfigTable("npc_data", (ulong)baseid).GetField_Bool("mapinfo");
    }

    public void SetDraginBallIconInfo(string uid, uint num, uint x, uint y)
    {
        if (this.MapUI == null)
        {
            return;
        }
        if (this.AreamapMgr != null && this.MapUI.IsOnCurrAreamap)
        {
            this.AreamapMgr.SetIconInfo(uid + ":" + num, GameMap.ItemType.DraginBall, "img_draginball/" + num, new Vector2(x, y), 0, false, false, null, default(Vector2));
        }
        if (this.PanelmapMgr != null)
        {
            this.PanelmapMgr.SetIconInfo(uid + ":" + num, GameMap.ItemType.DraginBall, "img_draginball/" + num, new Vector2(x, y), 0, false, false, null, default(Vector2));
        }
    }

    public void DeleteDraginBallIcon(string uid, uint num)
    {
        if (this.MapUI == null)
        {
            return;
        }
        if (this.AreamapMgr != null && this.MapUI.IsOnCurrAreamap)
        {
            this.AreamapMgr.DeleteIcon(uid + ":" + num, GameMap.ItemType.DraginBall);
        }
        if (this.PanelmapMgr != null)
        {
            this.PanelmapMgr.DeleteIcon(uid + ":" + num, GameMap.ItemType.DraginBall);
        }
    }

    public void SetNpcIconInfo(Npc npc)
    {
        if (!npc.IsMapShowNpc())
        {
            return;
        }
        if (!this.NpcHasIcon(npc.NpcData.MapNpcData.baseid))
        {
            return;
        }
        if (this.MapUI == null)
        {
            return;
        }
        LuaTable npcIconuimapinfo = this.GetNpcIconuimapinfo(npc);
        if (npcIconuimapinfo == null)
        {
            return;
        }
        string field_String = npcIconuimapinfo.GetField_String("IconName");
        if (string.IsNullOrEmpty(field_String))
        {
            return;
        }
        if (this.AreamapMgr != null && this.MapUI.IsOnCurrAreamap)
        {
            this.AreamapMgr.SetNPCIconInfo(npc, GameMap.ItemType.NPC, field_String, npc.CurrServerPos, 0, false, false);
        }
        if (this.PanelmapMgr != null)
        {
            this.PanelmapMgr.SetNPCIconInfo(npc, GameMap.ItemType.NPC, field_String, npc.CurrServerPos, 0, false, false);
        }
    }

    public void SetNpcIconInfo_Capsule(Npc npc)
    {
        if (this.MapUI == null)
        {
            return;
        }
        if (this.AreamapMgr != null && this.MapUI.IsOnCurrAreamap)
        {
            this.AreamapMgr.SetIconInfo(npc.EID.IdStr, GameMap.ItemType.Capsule, "img_capsule", npc.CurrServerPos, 0, false, false, null, default(Vector2));
        }
        if (this.PanelmapMgr != null)
        {
            this.PanelmapMgr.SetIconInfo(npc.EID.IdStr, GameMap.ItemType.Capsule, "img_capsule", npc.CurrServerPos, 0, false, false, null, default(Vector2));
        }
    }

    public void DeleteNpcIconInfo_Capsule(string uid)
    {
        if (this.MapUI == null)
        {
            return;
        }
        if (this.AreamapMgr != null && this.MapUI.IsOnCurrAreamap)
        {
            this.AreamapMgr.DeleteIcon(uid, GameMap.ItemType.Capsule);
        }
        if (this.PanelmapMgr != null)
        {
            this.PanelmapMgr.DeleteIcon(uid, GameMap.ItemType.Capsule);
        }
    }

    public void SetNpcIconInfoByTask(Npc npc, TaskInfo Tinfo)
    {
        if (this.npcShowFirstTaskDic.ContainsKey(npc.EID.Id))
        {
            this.npcShowFirstTaskDic[npc.EID.Id] = Tinfo;
        }
        else
        {
            this.npcShowFirstTaskDic.Add(npc.EID.Id, Tinfo);
        }
        string text = Tinfo.GetIconType();
        if (string.IsNullOrEmpty(text))
        {
            LuaTable npcIconuimapinfo = this.GetNpcIconuimapinfo(npc);
            if (npcIconuimapinfo == null)
            {
                return;
            }
            text = npcIconuimapinfo.GetField_String("IconName");
            if (string.IsNullOrEmpty(text))
            {
                return;
            }
        }
        if (this.AreamapMgr != null)
        {
            this.AreamapMgr.SetNPCIconInfo(npc, GameMap.ItemType.NPC, text, npc.CurrServerPos, Tinfo.ShowPriority, Tinfo.ShowPriority > 0, true);
        }
    }

    public void SetPanelNPCIcon(Npc npc, int showPriority)
    {
        string text = this.GetPanelIconNameByPriority(showPriority);
        if (string.IsNullOrEmpty(text))
        {
            LuaTable npcIconuimapinfo = this.GetNpcIconuimapinfo(npc);
            if (npcIconuimapinfo == null)
            {
                return;
            }
            text = npcIconuimapinfo.GetField_String("IconName");
            if (string.IsNullOrEmpty(text))
            {
                return;
            }
        }
        if (this.PanelmapMgr != null)
        {
            this.PanelmapMgr.SetNPCIconInfo(npc, GameMap.ItemType.NPC, text, npc.CurrServerPos, showPriority, showPriority > 0, true);
        }
    }

    private string GetPanelIconNameByPriority(int priority)
    {
        string result = string.Empty;
        switch (priority)
        {
            case 1:
                result = "icontask/img_no";
                break;
            case 2:
                result = "icontask/img_in";
                break;
            case 3:
                result = "icontask/img_ok";
                break;
            case 4:
                result = "icontask/img_dialog";
                break;
        }
        return result;
    }

    public void DeletePanelNPCIcon(Npc npc)
    {
        if (this.PanelmapMgr != null)
        {
            this.PanelmapMgr.DeleteIcon(npc.EID.ToString(), GameMap.ItemType.NPC);
        }
    }

    public string GetTeamIconName(int mum)
    {
        string result = "iconteam/img_team1";
        switch (mum)
        {
            case 0:
                result = "iconteam/img_team1";
                break;
            case 1:
                result = "iconteam/img_team2";
                break;
            case 2:
                result = "iconteam/img_team3";
                break;
        }
        return result;
    }

    public void SetTeamIconInfo(EntitiesID EID, Vector2 ServerPos, int mum)
    {
        if (this.MapUI == null)
        {
            return;
        }
        string teamIconName = this.GetTeamIconName(mum);
        if (this.AreamapMgr != null && this.MapUI.IsOnCurrAreamap)
        {
            this.AreamapMgr.SetIconInfo(EID.IDTypeStr, GameMap.ItemType.Team, teamIconName, ServerPos, 0, false, false, null, default(Vector2));
        }
        if (this.PanelmapMgr != null)
        {
            this.PanelmapMgr.SetIconInfo(EID.IDTypeStr, GameMap.ItemType.Team, teamIconName, ServerPos, 0, false, false, null, default(Vector2));
        }
    }

    public void DeleteIcon(CharactorBase player, GameMap.ItemType type)
    {
        if (this.MapUI == null)
        {
            return;
        }
        if (this.AreamapMgr != null && this.MapUI.IsOnCurrAreamap)
        {
            this.AreamapMgr.DeleteIcon(player.EID.IDTypeStr, type);
        }
        if (this.PanelmapMgr != null)
        {
            this.PanelmapMgr.DeleteIcon(player.EID.IDTypeStr, type);
        }
    }

    public void DeleteIcon(EntitiesID EID, GameMap.ItemType type)
    {
        if (this.MapUI == null)
        {
            return;
        }
        if (this.AreamapMgr != null && this.MapUI.IsOnCurrAreamap)
        {
            this.AreamapMgr.DeleteIcon(EID.IDTypeStr, type);
        }
        if (this.PanelmapMgr != null)
        {
            this.PanelmapMgr.DeleteIcon(EID.IDTypeStr, type);
        }
    }

    public void DeleteIconbyType(GameMap.ItemType type)
    {
        if (this.MapUI == null)
        {
            return;
        }
        if (this.AreamapMgr != null && this.MapUI.IsOnCurrAreamap)
        {
            this.AreamapMgr.DeleteIconbyType(type);
        }
        if (this.PanelmapMgr != null)
        {
            this.PanelmapMgr.DeleteIconbyType(type);
        }
    }

    public void HideNpc(List<string> typeList)
    {
        if (this.MapUI == null)
        {
            return;
        }
        if (this.AreamapMgr != null && this.MapUI.IsOnCurrAreamap)
        {
            this.AreamapMgr.HideNpcIcon(typeList);
        }
        if (this.PanelmapMgr != null)
        {
            this.PanelmapMgr.HideNpcIcon(typeList);
        }
    }

    public void SetItemIconInfoByItemPlayerAndItemType(CharactorBase player, GameMap.ItemType git)
    {
        if (player.EID.Etype != CharactorType.Player)
        {
            return;
        }
        string icontype = string.Empty;
        if (git != GameMap.ItemType.Enemy)
        {
            if (git == GameMap.ItemType.Friend)
            {
                icontype = "img_friend";
            }
        }
        else
        {
            icontype = "iconenemy/img_player";
        }
        DuoQiController controller = ControllerManager.Instance.GetController<DuoQiController>();
        if (controller.InBattleState() && git == GameMap.ItemType.Enemy && controller.GetingBallEmemyId() != player.EID.Id)
        {
            return;
        }
        if (this.PanelmapMgr != null)
        {
            this.PanelmapMgr.SetIconInfo(player.EID.IDTypeStr, git, icontype, player.CurrServerPos, 0, false, false, player, default(Vector2));
        }
        if (this.MapUI == null)
        {
            return;
        }
        if (this.AreamapMgr != null && this.MapUI.IsOnCurrAreamap)
        {
            this.AreamapMgr.SetIconInfo(player.EID.IDTypeStr, git, icontype, player.CurrServerPos, 0, false, false, player, default(Vector2));
        }
    }

    private void SetLabelIconInfo(LuaTable config)
    {
        if (this.AreamapMgr != null)
        {
            this.AreamapMgr.SetLabelInfo(config.GetField_String("id"), GameMap.ItemType.Text, config.GetField_String("text"), new Vector2(config.GetField_Float("x"), config.GetField_Float("y")));
        }
    }

    public void SetWayPath(List<Vector2> path)
    {
        if (this.MapUI == null)
        {
            return;
        }
        if (this.AreamapMgr != null && this.MapUI.IsOnCurrAreamap)
        {
            this.AreamapMgr.SetPathIconList(path);
        }
        if (this.PanelmapMgr != null)
        {
            this.PanelmapMgr.SetPathIconList(path);
        }
    }

    public void RetBattleMatch(uint avagetime)
    {
        Scheduler.Instance.RemoveTimer(new Scheduler.OnScheduler(this.DuoqiBtnTipUpdate));
        this.mAvageTime = avagetime;
        this.mTmpTime = 0U;
        Scheduler.Instance.AddTimer(1f, true, new Scheduler.OnScheduler(this.DuoqiBtnTipUpdate));
        if (this.MapUI == null)
        {
            return;
        }
        this.MapUI.SetDuoqiMatchTipText(0U);
        this.MapUI.ShowHideDuoqiBtn(true);
    }

    public void RetCancelBattleMatch()
    {
        Scheduler.Instance.RemoveTimer(new Scheduler.OnScheduler(this.DuoqiBtnTipUpdate));
        this.mAvageTime = 0U;
        this.mTmpTime = 0U;
        if (this.MapUI == null)
        {
            return;
        }
        this.MapUI.ShowHideDuoqiBtn(false);
    }

    public void CloseDuoqiBtnTip()
    {
        Scheduler.Instance.RemoveTimer(new Scheduler.OnScheduler(this.DuoqiBtnTipUpdate));
        if (this.MapUI != null)
        {
            this.MapUI.DoSetDuoqiMatchTipText(string.Empty);
        }
    }

    private void DuoqiBtnTipUpdate()
    {
        if (this.MapUI != null)
        {
            this.MapUI.SetDuoqiMatchTipText(this.mTmpTime);
        }
        this.mTmpTime += 1U;
    }

    public void RetHoldFlagTeamScore(List<HoldFlagCampScore> scores)
    {
        if (this.MapUI == null)
        {
            return;
        }
        this.MapUI.RetHoldFlagTeamScore(scores);
    }

    public void RetHoldFlagDBState(List<HoldFlagDBState> states)
    {
        if (this.MapUI != null)
        {
            this.MapUI.RetHoldFlagDBState(states);
        }
        DuoQiController controller = ControllerManager.Instance.GetController<DuoQiController>();
        controller.RefreshEmemyIdWhoGetingBall(states);
        controller.RefreshBallState(states);
    }

    public void RetHoldFlagCountDown(uint endtime)
    {
        if (this.MapUI != null)
        {
            uint second = endtime - SingletonForMono<GameTime>.Instance.GetCurrServerTimeBySecond();
            this.MapUI.RetHoldFlagCountDown(second);
        }
        else
        {
            this.onMapShow = (Action)Delegate.Combine(this.onMapShow, new Action(delegate ()
            {
                this.RetHoldFlagCountDown(endtime);
            }));
        }
    }

    public void ShowBattlePanel()
    {
        if (this.MapUI != null)
        {
            this.MapUI.ShowBattlePanel();
        }
        else
        {
            this.onMapShow = (Action)Delegate.Combine(this.onMapShow, new Action(this.ShowBattlePanel));
        }
    }

    public void HideBattlePanel()
    {
        if (this.MapUI != null)
        {
            this.MapUI.HideBattlePanel();
        }
        else
        {
            this.onMapShow = null;
        }
    }

    public bool isShowAreaTipReaded = true;

    public GameMap PanelmapMgr;

    public GameMap AreamapMgr;

    public Action onMapShow;

    private MSG_NoticeClientAllLines_SC _lineData;

    private Dictionary<int, int> _lineDic = new Dictionary<int, int>();

    private Dictionary<string, uint> mPathWayIdDic = new Dictionary<string, uint>();

    private float GetTeamPosInterval = 2f;

    private float RunningTime;

    private List<Dropdown.OptionData> lines;

    private int currLineIndex;

    public Dictionary<ulong, TaskInfo> npcShowFirstTaskDic = new Dictionary<ulong, TaskInfo>();

    public uint mAvageTime;

    private uint mTmpTime;
}
