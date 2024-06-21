using System;
using System.Collections.Generic;
using Framework.Managers;
using Game.Scene;
using hero;
using magic;
using msg;
using Net;
using npc;
using rankpk_msg;
using UnityEngine;

public class PlayerNetWork : NetWorkBase
{
    public override void Initialize()
    {
        this.RegisterMsg();
    }

    public override void RegisterMsg()
    {
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_Ret_Move_SC>(2280, new ProtoMsgCallback<MSG_Ret_Move_SC>(this.OnRetMove));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_Server_Force_Move_SC>(2282, new ProtoMsgCallback<MSG_Server_Force_Move_SC>(this.OnRetForceMove));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_OnUserJump_CSC>(2320, new ProtoMsgCallback<MSG_OnUserJump_CSC>(this.OnRetJump));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_Ret_MapScreenRefreshCharacter_SC>(2275, new ProtoMsgCallback<MSG_Ret_MapScreenRefreshCharacter_SC>(this.OnMapScreenRefreshCharacter));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_Ret_MapScreenRemoveCharacter_SC>(2276, new ProtoMsgCallback<MSG_Ret_MapScreenRemoveCharacter_SC>(this.OnMapScreenRemoveCharacter));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_Ret_MapScreenRemoveCharacter_SC>(2525, new ProtoMsgCallback<MSG_Ret_MapScreenRemoveCharacter_SC>(this.OnMapScreenRemoveCharacterShowCorpse));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_Ret_MapScreenBatchRemoveCharacter_SC>(2277, new ProtoMsgCallback<MSG_Ret_MapScreenBatchRemoveCharacter_SC>(this.OnMapScreenBatchRemoveCharacter));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_Ret_MapScreenBatchRefreshNpc_SC>(2286, new ProtoMsgCallback<MSG_Ret_MapScreenBatchRefreshNpc_SC>(this.OnMapScreenBatchRefreshNpc));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_Ret_MapScreenBatchRemoveNpc_SC>(2287, new ProtoMsgCallback<MSG_Ret_MapScreenBatchRemoveNpc_SC>(this.OnMapScreenBatchRemoveNpc));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_Ret_MapScreenRefreshNpc_SC>(2288, new ProtoMsgCallback<MSG_Ret_MapScreenRefreshNpc_SC>(this.OnMapScreenRefreshNpc));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_Ret_MapScreenRemoveNpc_SC>(2289, new ProtoMsgCallback<MSG_Ret_MapScreenRemoveNpc_SC>(this.OnMapScreenRemoveNpc));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_DataCharacterMain_SC>(2261, new ProtoMsgCallback<MSG_DataCharacterMain_SC>(this.OnDataCharacterMain));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_Ret_LoginOnReturnCharList_SC>(2308, new ProtoMsgCallback<MSG_Ret_LoginOnReturnCharList_SC>(this.OnDataCharacterList));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_Ret_NineScreenRefreshPlayer_SC>(2274, new ProtoMsgCallback<MSG_Ret_NineScreenRefreshPlayer_SC>(this.OnNineScreenRefreshPlayer));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_Ret_MainUserRelive_SC>(2214, new ProtoMsgCallback<MSG_Ret_MainUserRelive_SC>(this.OnUserRelive));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_Ret_MapScreenFuncNpc_SC>(2290, new ProtoMsgCallback<MSG_Ret_MapScreenFuncNpc_SC>(this.OnMapScreenFuncNpc));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_NPC_Death_SC>(2325, new ProtoMsgCallback<MSG_NPC_Death_SC>(this.OnNPCDeath));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_NPCHatredList_SC>(2321, new ProtoMsgCallback<MSG_NPCHatredList_SC>(this.OnNPCHatredList));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_RetNpcDir_SC>(2293, new ProtoMsgCallback<MSG_RetNpcDir_SC>(this.OnRetNpcDir));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_Ret_RetNpcMove_SC>(2291, new ProtoMsgCallback<MSG_Ret_RetNpcMove_SC>(this.OnRetNpcMove));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_Ret_Move_Failed_SC>(2283, new ProtoMsgCallback<MSG_Ret_Move_Failed_SC>(this.OnRetMoveFailed));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_Ret_setTimeState_SC>(2295, new ProtoMsgCallback<MSG_Ret_setTimeState_SC>(this.OnRetSetTimeState));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_Ret_VisibleNpcList_SC>(2297, new ProtoMsgCallback<MSG_Ret_VisibleNpcList_SC>(this.OnRetVisibleNpcList));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_RetNinePlayerLevelUp_SC>(2298, new ProtoMsgCallback<MSG_RetNinePlayerLevelUp_SC>(this.RetNinePlyaerLevelUp));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_UpdateExpLevel_SC>(2299, new ProtoMsgCallback<MSG_UpdateExpLevel_SC>(this.UpDateExpAndLevel));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_RetNpcWarpMove_SC>(2292, new ProtoMsgCallback<MSG_RetNpcWarpMove_SC>(this.RetNpcWarpMove));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_Ret_StateList_SC>(2278, new ProtoMsgCallback<MSG_Ret_StateList_SC>(this.OnRetStateList));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_RetEntrySelectState_SC>(2302, new ProtoMsgCallback<MSG_RetEntrySelectState_SC>(this.OnRetEntrySelectState));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_RetHpMpToSelects_SC>(2303, new ProtoMsgCallback<MSG_RetHpMpToSelects_SC>(this.OnRetHpMpToSelects));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_Req_Delete_Char_CSC>(2312, new ProtoMsgCallback<MSG_Req_Delete_Char_CSC>(this.OnRetDelSelectRole));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_RankPkReqPrepare_SC>(60090, new ProtoMsgCallback<MSG_RankPkReqPrepare_SC>(this.OnRetRankPkReqPrepare));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_NoticeClientAllLines_SC>(2318, new ProtoMsgCallback<MSG_NoticeClientAllLines_SC>(this.OnRet_NoticeClientAllLines_SC));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_Ret_UserDeath_SC>(2212, new ProtoMsgCallback<MSG_Ret_UserDeath_SC>(this.OnRet_UserDeath_SC));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_NEW_ROLE_CUTSCENE_SCS>(2324, new ProtoMsgCallback<MSG_NEW_ROLE_CUTSCENE_SCS>(this.OnRet_PlayStartCutScene_SC));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_START_CUTSCENE_SC>(2510, new ProtoMsgCallback<MSG_START_CUTSCENE_SC>(this.On_StartCutScene_SC));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_User_Drop_SCS>(2509, new ProtoMsgCallback<MSG_User_Drop_SCS>(this.OnRet_User_Drop_SCS));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_AttackTargetChange_SC>(2531, new ProtoMsgCallback<MSG_AttackTargetChange_SC>(this.MSG_AttackTargetChange_SC));
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_NotifyClientHeroScore_SC>(CommandID.MSG_NotifyClientHeroScore_SC, new ProtoMsgCallback<MSG_NotifyClientHeroScore_SC>(this.NotifyClientHeroScore_SC));
    }

    private MainUIController mainUIController
    {
        get
        {
            return ControllerManager.Instance.GetController<MainUIController>();
        }
    }

    private EntitiesManager entitiesManager
    {
        get
        {
            return ManagerCenter.Instance.GetManager<EntitiesManager>();
        }
    }

    public void RetNpcWarpMove(MSG_RetNpcWarpMove_SC data)
    {
        this.mainUIController.NpcWarpMove(data);
    }

    public void RetNinePlyaerLevelUp(MSG_RetNinePlayerLevelUp_SC data)
    {
        this.mainUIController.LevelUpEffect(data.target.id, (CharactorType)data.target.type);
    }

    public void UpDateExpAndLevel(MSG_UpdateExpLevel_SC data)
    {
        HeroHandbookController controller = ControllerManager.Instance.GetController<HeroHandbookController>();
        if (controller != null && this.mainUIController.mExpData != null)
        {
            bool flag = controller.IsMainHeroOnline();
            if (flag)
            {
                if (data.mainhero_lv > this.mainUIController.mExpData.mainhero_lv)
                {
                }
            }
            else if (data.curlevel > this.mainUIController.mExpData.curlevel)
            {
                Scheduler.Instance.AddFrame(1U, false, delegate
                {
                    LuaScriptMgr.Instance.CallLuaFunction("MainUICtrl.ShowTaskList", new object[]
                    {
                        Util.GetLuaTable("MainUICtrl")
                    });
                });
            }
        }
        LuaScriptMgr.Instance.CallLuaFunction("HerosCtrl.UpdateHeroLevelAndExp", new object[]
        {
            data.mainhero_thisid.ToString(),
            data.mainhero_lv,
            data.mainhero_exp
        });
        this.mainUIController.RefreshEXP(data);
        uint curLv = this.mainUIController.GetCurLv();
        uint num = (uint)this.mainUIController.GetCurExp();
        this.mainUIController.RefreshPlayerEXP(curLv, num);
        uint secondLv = this.mainUIController.GetSecondLv();
        uint exp = (uint)this.mainUIController.GetSecondExp();
        this.mainUIController.RefreshHeroEXP(secondLv, exp);
        UI_Character uiobject = UIManager.GetUIObject<UI_Character>();
        if (uiobject != null)
        {
            uiobject.RefreshExpPanel();
        }
        UI_HeroHandbook uiobject2 = UIManager.GetUIObject<UI_HeroHandbook>();
        if (uiobject2 != null)
        {
            uiobject2.SetupInfo();
        }
        if (MainPlayer.Self.MainPlayeData != null)
        {
            MainPlayer.Self.MainPlayeData.RefreshExp(num, curLv);
        }
        if (MainPlayer.Self.OtherPlayerData != null)
        {
            MainPlayer.Self.OtherPlayerData.RefreshPlayerLevel(curLv);
        }
    }

    private void NotifyClientHeroScore_SC(MSG_NotifyClientHeroScore_SC data)
    {
        if (data == null)
        {
            return;
        }
        for (int i = 0; i < data.scores.Count; i++)
        {
            LuaScriptMgr.Instance.CallLuaFunction("HerosCtrl.UpdateHeroFightValue", new object[]
            {
                data.scores[i].thisid.ToString(),
                data.scores[i].score
            });
        }
        UI_HeroHandbook uiobject = UIManager.GetUIObject<UI_HeroHandbook>();
        if (uiobject != null)
        {
            uiobject.SetupInfo();
        }
    }

    public void ClearMoveCachesData()
    {
        this._mvDataCache.ClearMoveCachesData();
    }

    public void CheckNeedReqMove()
    {
        cs_MoveData syncData = this._mvDataCache.getSyncData();
        if (syncData == null)
        {
            return;
        }
        MSG_Req_Move_CS msg_Req_Move_CS = new MSG_Req_Move_CS();
        MoveData moveData = new MoveData();
        moveData.pos = new FloatMovePos();
        moveData.pos.fx = syncData.pos.fx;
        moveData.pos.fy = syncData.pos.fy;
        moveData.dir = syncData.dir;
        msg_Req_Move_CS.movedata.Add(moveData);
        msg_Req_Move_CS.steplenth = (uint)syncData.step;
        msg_Req_Move_CS.charid = this.entitiesManager.MainPlayer.OtherPlayerData.MapUserData.charid;
        if (this.logMove)
        {
            for (int i = 0; i < msg_Req_Move_CS.movedata.Count; i++)
            {
                FFDebug.LogError("1111  CheckNeedReqMove", string.Format("MSG_Req_Move_CS data[{0}]: dir:{1}  pos:({2},{3})", new object[]
                {
                    i,
                    msg_Req_Move_CS.movedata[i].dir,
                    msg_Req_Move_CS.movedata[i].pos.fx,
                    msg_Req_Move_CS.movedata[i].pos.fy
                }));
            }
        }
        MainPlayer.Self.RecodeSeverMoveData(syncData);
        base.SendMsg<MSG_Req_Move_CS>(CommandID.MSG_Req_Move_CS, msg_Req_Move_CS, false);
    }

    public void ResetLastPos()
    {
        if (this.lastSendData != null)
        {
            this.lastSendData.dir = 0U;
            this.lastSendData.pos.fx = 0f;
            this.lastSendData.pos.fy = 0f;
        }
    }

    public void ReqMove(cs_MoveData data, bool istoself = false, bool now = false)
    {
        if (this.lastSendData == null || (double)(Math.Abs(data.pos.fx - this.lastSendData.pos.fx) + Math.Abs(data.pos.fy - this.lastSendData.pos.fy)) > 0.5 || Math.Abs((long)((ulong)(data.dir - this.lastSendData.dir))) > 10L)
        {
            if (this.lastSendData == null)
            {
                this.lastSendData = new cs_MoveData();
                this.lastSendData.pos = default(cs_FloatMovePos);
                this.lastSendData.step = 1;
            }
            this.lastSendData.dir = data.dir;
            this.lastSendData.pos.fx = data.pos.fx;
            this.lastSendData.pos.fy = data.pos.fy;
            MSG_Req_Move_CS msg_Req_Move_CS = new MSG_Req_Move_CS();
            if (this._md.pos == null)
            {
                this._md.pos = new FloatMovePos();
            }
            this._md.pos.fx = data.pos.fx;
            this._md.pos.fy = data.pos.fy;
            this._md.dir = data.dir;
            msg_Req_Move_CS.movedata.Add(this._md);
            msg_Req_Move_CS.steplenth = 1U;
            msg_Req_Move_CS.charid = this.entitiesManager.MainPlayer.OtherPlayerData.MapUserData.charid;
            if (this.logMove)
            {
                for (int i = 0; i < msg_Req_Move_CS.movedata.Count; i++)
                {
                    FFDebug.LogError("2222  ReqMove", string.Format("MSG_Req_Move_CS data[{0}]: dir:{1}  pos:({2},{3})", new object[]
                    {
                        i,
                        msg_Req_Move_CS.movedata[i].dir,
                        msg_Req_Move_CS.movedata[i].pos.fx,
                        msg_Req_Move_CS.movedata[i].pos.fy
                    }));
                }
            }
            MainPlayer.Self.RecodeSeverMoveData(data);
            base.SendMsg<MSG_Req_Move_CS>(CommandID.MSG_Req_Move_CS, msg_Req_Move_CS, istoself);
        }
    }

    public void OnRetForceMove(MSG_Server_Force_Move_SC data)
    {
        OtherPlayer otherPlayer = this.entitiesManager.GetPlayerByID(data.charid);
        if (otherPlayer == null)
        {
            FFDebug.LogWarning(this, "OnRetMove Does exist player with id " + data.charid);
            return;
        }
        if (otherPlayer.CharState != CharactorState.CreatComplete)
        {
            return;
        }
        otherPlayer.RecodeSeverMoveData(new cs_MoveData(data.movedata[data.movedata.Count - 1]));
        if (this.entitiesManager.IsMainPlayer(data.charid))
        {
            otherPlayer = this.entitiesManager.MainPlayer;
        }
        int count = data.movedata.Count;
        if (!otherPlayer.CharactorCheckMove(new Vector2(data.movedata[count - 1].pos.fx, data.movedata[count - 1].pos.fy)))
        {
            for (int i = 0; i < data.movedata.Count; i++)
            {
                cs_MoveData cs_MoveData = new cs_MoveData();
                cs_MoveData.dir = data.movedata[i].dir;
                cs_MoveData.pos = default(cs_FloatMovePos);
                cs_MoveData.pos.fx = data.movedata[i].pos.fx;
                cs_MoveData.pos.fy = data.movedata[i].pos.fy;
                otherPlayer.RetMoveDataQueue.Enqueue(cs_MoveData);
            }
            otherPlayer.BaseData.RefreshCharaBasePosition(otherPlayer.NextPosition2D);
            cs_MoveData data2 = otherPlayer.RetMoveDataQueue.Dequeue();
            otherPlayer.MoveTo(data2);
        }
    }

    public void OnRetMove(MSG_Ret_Move_SC data)
    {
        if (this.entitiesManager.IsMainPlayer(data.charid))
        {
            return;
        }
        OtherPlayer playerByID = this.entitiesManager.GetPlayerByID(data.charid);
        if (playerByID == null)
        {
            FFDebug.LogWarning(this, "OnRetMove Does exist player with id " + data.charid);
            return;
        }
        if (playerByID.CharState != CharactorState.CreatComplete)
        {
            return;
        }
        playerByID.RecodeSeverMoveData(new cs_MoveData(data.movedata[data.movedata.Count - 1]));
        int count = data.movedata.Count;
        if (!playerByID.CharactorCheckMove(new Vector2(data.movedata[count - 1].pos.fx, data.movedata[count - 1].pos.fy)))
        {
            for (int i = 0; i < data.movedata.Count; i++)
            {
                cs_MoveData cs_MoveData = new cs_MoveData();
                cs_MoveData.dir = data.movedata[i].dir;
                cs_MoveData.pos = default(cs_FloatMovePos);
                cs_MoveData.pos.fx = data.movedata[i].pos.fx;
                cs_MoveData.pos.fy = data.movedata[i].pos.fy;
                playerByID.RetMoveDataQueue.Enqueue(cs_MoveData);
            }
            playerByID.BaseData.RefreshCharaBasePosition(playerByID.NextPosition2D);
            cs_MoveData data2 = playerByID.RetMoveDataQueue.Dequeue();
            playerByID.MoveTo(data2);
        }
    }

    public void OnRetMoveFailed(MSG_Ret_Move_Failed_SC mdata)
    {
        OtherPlayer playerByID = this.entitiesManager.GetPlayerByID(mdata.charid);
        if (playerByID == null)
        {
            FFDebug.LogWarning(this, "Dosent containe player with id:" + mdata.charid);
            return;
        }
        if (playerByID.CharState != CharactorState.CreatComplete)
        {
            return;
        }
        playerByID.RecodeSeverMoveData(new cs_MoveData(mdata.movedata));
        if (this.entitiesManager.IsMainPlayer(mdata.charid))
        {
            (playerByID as MainPlayer).ForceSetPositionTo(new Vector2(mdata.movedata.pos.fx, mdata.movedata.pos.fy));
        }
    }

    public void ReqJump()
    {
        if (MainPlayer.Self == null)
        {
            return;
        }
        if (this.reqJump.data == null)
        {
            this.reqJump.data = new MoveData();
        }
        if (this.reqJump.data.pos == null)
        {
            this.reqJump.data.pos = new FloatMovePos();
        }
        this.reqJump.charid = MainPlayer.Self.OtherPlayerData.MapUserData.charid;
        this.reqJump.data.dir = MainPlayer.Self.ServerDir;
        this.reqJump.data.pos.fx = MainPlayer.Self.CurrentPosition2D.x;
        this.reqJump.data.pos.fy = MainPlayer.Self.CurrentPosition2D.y;
        base.SendMsg<MSG_OnUserJump_CSC>(CommandID.MSG_OnUserJump_CSC, this.reqJump, false);
    }

    public void OnRetJump(MSG_OnUserJump_CSC data)
    {
        OtherPlayer playerByID = this.entitiesManager.GetPlayerByID(data.charid);
        if (playerByID == null)
        {
            FFDebug.LogWarning(this, "OnRetMove Does exist player with id " + data.charid);
            return;
        }
        if (playerByID.CharState != CharactorState.CreatComplete)
        {
            return;
        }
        if (this.entitiesManager.IsMainPlayer(data.charid))
        {
            return;
        }
        if ((int)playerByID.CurrentPosition2D.x != (int)data.data.pos.fx || (int)playerByID.CurrentPosition2D.y != (int)data.data.pos.fy)
        {
            playerByID.NextJumPos.dir = data.data.dir;
            playerByID.NextJumPos.pos.fx = data.data.pos.fx;
            playerByID.NextJumPos.pos.fy = data.data.pos.fy;
        }
        else
        {
            playerByID.NextJumPos.pos.fx = -1f;
            playerByID.NextJumPos.pos.fy = -1f;
            playerByID.Jump(false, false);
        }
    }

    public void RegRole(string name, uint[] idDatas, SEX sex)
    {
        MSG_Create_Role_CS msg_Create_Role_CS = new MSG_Create_Role_CS();
        msg_Create_Role_CS.name = name;
        msg_Create_Role_CS.sex = sex;
        msg_Create_Role_CS.heroid = idDatas[0];
        msg_Create_Role_CS.haircolor = idDatas[1];
        msg_Create_Role_CS.hairstyle = idDatas[2];
        msg_Create_Role_CS.facestyle = idDatas[3];
        msg_Create_Role_CS.antenna = idDatas[4];
        FFDebug.LogWarning("Create Charactor", string.Concat(new object[]
        {
            " heroid = ",
            msg_Create_Role_CS.heroid,
            " heroname = ",
            msg_Create_Role_CS.name,
            " haircolor = ",
            msg_Create_Role_CS.haircolor,
            " hairstyle = ",
            msg_Create_Role_CS.hairstyle,
            " facestyle = ",
            msg_Create_Role_CS.facestyle
        }));
        base.SendMsg<MSG_Create_Role_CS>(CommandID.MSG_Create_Role_CS, msg_Create_Role_CS, false);
    }

    public void DelRole(ulong charid, uint opcode)
    {
        base.SendMsg<MSG_Req_Delete_Char_CSC>(CommandID.MSG_Req_Delete_Char_CSC, new MSG_Req_Delete_Char_CSC
        {
            charid = charid,
            opcode = opcode
        }, false);
    }

    private void CreateRole()
    {
        string account = ControllerManager.Instance.GetLoginController().loginModel.Account;
        ControllerManager.Instance.GetLoginController().RegisterPlayer(account, 1U);
    }

    public void OnDataCharacterList(MSG_Ret_LoginOnReturnCharList_SC dataLogin)
    {
        this.ClearData();
        SceneInfo sceneInfo = new SceneInfo();
        sceneInfo.mapid = 698U;
        sceneInfo.mapname = "创建角色选人场景";
        sceneInfo.lineid = 1U;
        sceneInfo.pos = new FloatMovePos();
        GameScene gameScene = ManagerCenter.Instance.GetManager<GameScene>();
        ControllerManager.Instance.GetLoginController().showStaticImg();
        ShortcutsConfigController controller = ControllerManager.Instance.GetController<ShortcutsConfigController>();
        if (controller != null)
        {
            controller.Reset();
        }
        UI_Loading.StartLoading(sceneInfo);
        if (ControllerManager.Instance.GetLoginController().uiLogin != null)
        {
            ControllerManager.Instance.GetLoginController().Close();
        }
        gameScene.CheckSameScene(sceneInfo);
        gameScene.ChangeScene(sceneInfo, delegate
        {
            gameScene.OnSceneLoadNotifyServer();
            UI_Loading.EndLoading();
            UIManager.Instance.ShowUI<UI_SelectRole>("UI_SelectRole", delegate ()
            {
                UIManager.GetUIObject<UI_SelectRole>().SetCharData(dataLogin);
                ControllerManager.Instance.GetController<LoadTipsController>().CloseReconnectTips();
                this.isLogin = true;
            }, UIManager.ParentType.CommonUI, false);
        }, delegate (float f)
        {
        });
    }

    public void OnDataCharacterMain(MSG_DataCharacterMain_SC dataLogin)
    {
        FFDebug.Log(this, FFLogType.Login, "OnDataCharacterMain");
        CharacterMainData data = dataLogin.data;
        ControllerManager.Instance.GetController<LoadTipsController>().CloseReconnectTips();
        if (ControllerManager.Instance.GetLoginController().uiLogin != null)
        {
            ControllerManager.Instance.GetLoginController().Close();
        }
        UI_SelectRole uiobject = UIManager.GetUIObject<UI_SelectRole>();
        if (uiobject != null)
        {
            UIManager.Instance.DeleteUI<UI_SelectRole>();
        }
        if (data == null)
        {
            return;
        }
        ControllerManager.Instance.GetController<ChooseRoleController>().Dispose();
        MainPlayer mainPlayer = this.entitiesManager.MainPlayer;
        if (mainPlayer != null)
        {
            bool flag;
            bool flag2;
            mainPlayer.OtherPlayerData.RefreshMapShow(new cs_MapUserData(dataLogin.data.mapdata), out flag, out flag2);
            if (flag || flag2)
            {
                ControllerManager.Instance.GetController<MainUIController>().RefreshHeadIcon();
            }
            if (!flag && mainPlayer.CharState == CharactorState.CreatComplete)
            {
                bool isNeedCloseLoading = LSingleton<NetWorkModule>.Instance.isNeedCloseLoading;
                if (isNeedCloseLoading)
                {
                    UI_Loading.EndLoading();
                }
            }
            mainPlayer.MainPlayeData.RefreshAttributeData(new cs_AttributeData(dataLogin.data.attridata));
            mainPlayer.MainPlayeData.RefreshCharacterBaseData(new cs_CharacterBaseData(dataLogin.data.basedata));
            mainPlayer.MainPlayeData.RefreshFightData(new cs_CharacterFightData(dataLogin.data.fightdata));
            mainPlayer.RefreshCurrency();
            if (mainPlayer.CharState == CharactorState.CreatComplete)
            {
                mainPlayer.ModelObj.SetActive(true);
                mainPlayer.InitCamera();
                if (CameraController.Self != null)
                {
                    CameraController.Self.AglinWithTarget();
                }
            }
            mainPlayer.OtherPlayerData.RefreshSpeedAndState(new cs_MapUserData(dataLogin.data.mapdata));
        }
        else
        {
            if (null != Fps.Instance)
            {
                Fps.Instance.StartNow = true;
            }
            mainPlayer = new MainPlayer();
            mainPlayer.MainPlayeData.RefreshAttributeData(new cs_AttributeData(dataLogin.data.attridata));
            mainPlayer.MainPlayeData.RefreshCharacterBaseData(new cs_CharacterBaseData(dataLogin.data.basedata));
            mainPlayer.OtherPlayerData.RefreshMapUserData(new cs_MapUserData(dataLogin.data.mapdata));
            mainPlayer.MainPlayeData.RefreshFightData(new cs_CharacterFightData(dataLogin.data.fightdata));
            this.entitiesManager.MainPlayer = mainPlayer;
            mainPlayer.CreatPlayerModel();
            ControllerManager.Instance.GetController<MainUIController>().RefreshHeadIcon();
            this.entitiesManager.AddCharacter(mainPlayer);
            ControllerManager.Instance.GetController<FriendControllerNew>().InitPrivateChatLocalData();
            ControllerManager.Instance.GetController<FriendControllerNew>().InitChatHistory();
            ControllerManager.Instance.GetController<FriendControllerNew>().InitRecentList();
            ControllerManager.Instance.GetController<FriendControllerNew>().mFriendNetWork.ReqBlackList();
            ControllerManager.Instance.GetController<MailControl>().ReqMailList();
        }
        GuildControllerNew controller = ControllerManager.Instance.GetController<GuildControllerNew>();
        if (controller.GetMyGuildMember() == null && controller.GetMyGuildId() != 0UL)
        {
            ControllerManager.Instance.GetController<GuildControllerNew>().GuildInfoSelf(null);
        }
    }

    private void ClearData()
    {
        UIManager instance = UIManager.Instance;
        if (instance != null)
        {
            instance.DeleteAllUIWithOutList(new List<string>
            {
                "ui_login",
                "UI_P2PLogin",
                "UI_LoadTips",
                "ui_loading"
            });
            instance.Init();
        }
        if (MainPlayer.Self != null)
        {
            MainPlayer.Self.TrueDestroy();
        }
        if (ManagerCenter.Instance.GetManager<EntitiesManager>() != null)
        {
            ManagerCenter.Instance.GetManager<EntitiesManager>().MainPlayer = null;
        }
        MainUIController controller = ControllerManager.Instance.GetController<MainUIController>();
        if (controller != null)
        {
            controller.isInitLuaPanel = false;
        }
        TeamController controller2 = ControllerManager.Instance.GetController<TeamController>();
        if (controller2 != null)
        {
            controller2.ClearTeamInfo();
        }
        LuaScriptMgr.Instance.CallLuaFunction("BagCtrl.GetOrderByType", new object[]
        {
            Util.GetLuaTable("BagCtrl"),
            0
        });
        LuaScriptMgr.Instance.CallLuaFunction("BagCtrl.Clear", new object[]
        {
            Util.GetLuaTable("BagCtrl")
        });
        ManagerCenter.Instance.GetManager<EntitiesManager>().UnLoadCharactors();
        ControllerManager.Instance.GetController<SecondPwdControl>().PlayerInputSecondPwd = string.Empty;
    }

    public void OnNineScreenRefreshPlayer(MSG_Ret_NineScreenRefreshPlayer_SC mdata)
    {
        for (int i = 0; i < mdata.data.Count; i++)
        {
            MapUserData mapUserData = mdata.data[i];
            FFDebug.Log(this, FFLogType.Player, string.Concat(new object[]
            {
                "OnNineScreenRefreshPlayer: ",
                mapUserData.charid,
                " Frame: ",
                Scheduler.Instance.FrameSinceStartup
            }));
            if (this.entitiesManager.GetPlayerByID(mapUserData.charid) == null)
            {
                if (!this.entitiesManager.IsMainPlayer(mapUserData.charid))
                {
                    OtherPlayer otherPlayer = new OtherPlayer();
                    otherPlayer.OtherPlayerData.RefreshMapUserData(new cs_MapUserData(mapUserData));
                    otherPlayer.CreatPlayerModel();
                    this.entitiesManager.AddCharacter(otherPlayer);
                }
            }
            else
            {
                OtherPlayer playerByID = this.entitiesManager.GetPlayerByID(mapUserData.charid);
                if (playerByID.CharState == CharactorState.RemoveFromNineScreen)
                {
                    playerByID.CharState = CharactorState.CreatEntity;
                }
                playerByID.OtherPlayerData.RefreshMapUserData(new cs_MapUserData(mapUserData));
            }
        }
    }

    public void OnMapScreenRefreshCharacter(MSG_Ret_MapScreenRefreshCharacter_SC mdata)
    {
        OtherPlayer otherPlayer = this.entitiesManager.GetPlayerByID(mdata.data.charid);
        if (otherPlayer == null)
        {
            if (this.entitiesManager.MainPlayer != null && mdata.data.charid == this.entitiesManager.MainPlayer.OtherPlayerData.MapUserData.charid)
            {
                return;
            }
            otherPlayer = new OtherPlayer();
            otherPlayer.OtherPlayerData.RefreshMapUserData(new cs_MapUserData(mdata.data));
            otherPlayer.CreatPlayerModel();
            this.entitiesManager.AddCharacter(otherPlayer);
        }
        else
        {
            if (otherPlayer.CharState == CharactorState.RemoveFromNineScreen)
            {
                otherPlayer.CharState = CharactorState.CreatEntity;
            }
            cs_MapUserData cs_MapUserData = new cs_MapUserData(mdata.data);
            if (otherPlayer is MainPlayer && !otherPlayer.IsFallChecking && otherPlayer.OtherPlayerData.MapUserData.mapdata.movespeed != cs_MapUserData.mapdata.movespeed && otherPlayer.OtherPlayerData.MapUserData != null && otherPlayer.OtherPlayerData.MapUserData.isSameExceptSpeetAndPosition(cs_MapUserData))
            {
                if (Mathf.Abs(otherPlayer.CurrentPosition2D.x - cs_MapUserData.mapdata.pos.fx) >= 7f || Mathf.Abs(otherPlayer.CurrentPosition2D.y - cs_MapUserData.mapdata.pos.fy) >= 7f)
                {
                    otherPlayer.OtherPlayerData.RefreshMapUserData(cs_MapUserData);
                }
                else
                {
                    otherPlayer.OtherPlayerData.RefreshSpeedAndState(cs_MapUserData);
                }
            }
            else
            {
                otherPlayer.OtherPlayerData.RefreshMapUserData(cs_MapUserData);
            }
        }
    }

    public void Notify_SceneLoaded_CS(ulong sceneID = 0UL)
    {
        base.SendMsg<MSG_Notify_SceneLoaded_CS>(CommandID.MSG_Notify_SceneLoaded_CS, new MSG_Notify_SceneLoaded_CS
        {
            sceneid = sceneID
        }, false);
    }

    public void OnMapScreenRemoveCharacterShowCorpse(MSG_Ret_MapScreenRemoveCharacter_SC mdata)
    {
        GameScene manager = ManagerCenter.Instance.GetManager<GameScene>();
        if (manager.isAbattoirScene)
        {
            this.OnMapScreenRemoveCharacter(mdata);
            return;
        }
        OtherPlayer playerByID = this.entitiesManager.GetPlayerByID(mdata.charid);
        if (playerByID == null)
        {
            FFDebug.Log(this, FFLogType.Network, "Doesnt exist player with id :" + mdata.charid);
            return;
        }
        FFDebug.Log(this, FFLogType.Player, "OnMapScreenRemoveCharacterShowCorpse  " + playerByID.OtherPlayerData.MapUserData.name);
        ManagerCenter.Instance.GetManager<ObjectPoolManager>().CloneCharactorCorpse(playerByID);
        this.OnMapScreenRemoveCharacter(mdata);
    }

    public void OnMapScreenRemoveCharacter(MSG_Ret_MapScreenRemoveCharacter_SC mdata)
    {
        OtherPlayer playerByID = this.entitiesManager.GetPlayerByID(mdata.charid);
        if (playerByID == null)
        {
            FFDebug.Log(this, FFLogType.Network, "Doesnt exist player with id :" + mdata.charid);
            return;
        }
        FFDebug.Log(this, FFLogType.Player, "OnMapScreenRemoveCharacter  " + playerByID.OtherPlayerData.MapUserData.name);
        if (playerByID is MainPlayer)
        {
            ((MainPlayer)playerByID).DestroyThisInNineScreen();
        }
        else
        {
            playerByID.DestroyThisInNineScreen();
        }
    }

    public void OnMapScreenBatchRemoveCharacter(MSG_Ret_MapScreenBatchRemoveCharacter_SC mdata)
    {
        FFDebug.Log(this, FFLogType.Player, "OnMapScreenBatchRemoveCharacter");
        for (int i = 0; i < mdata.charids.Count; i++)
        {
            OtherPlayer playerByID = this.entitiesManager.GetPlayerByID(mdata.charids[i]);
            if (playerByID == null)
            {
                FFDebug.Log(this, FFLogType.Network, "Doesnt exist player with id :" + mdata.charids[i]);
            }
            else if (playerByID is MainPlayer)
            {
                ((MainPlayer)playerByID).DestroyThisInNineScreen();
            }
            else
            {
                playerByID.DestroyThisInNineScreen();
            }
        }
    }

    public void OnNPCHatredList(MSG_NPCHatredList_SC data)
    {
        Npc npc = this.entitiesManager.GetNpc(data.npctempid);
        if (npc != null)
        {
            npc.UpdateHatredList(data.enemytempid);
            if (npc.hpdata != null)
            {
                npc.hpdata.RefreshModel();
            }
        }
    }

    public void OnNPCDeath(MSG_NPC_Death_SC data)
    {
        Npc npc = this.entitiesManager.GetNpc(data.tempid);
        FFDebug.Log(this, FFLogType.Network, string.Concat(new object[]
        {
            "OnNPCDeath ",
            data.tempid,
            "--null--",
            npc == null
        }));
        if (npc != null)
        {
            npc.Die();
        }
    }

    public void OnUserDeath(MSG_Ret_MainUserDeath_SC data)
    {
    }

    private void OpenReLiveDialog(MSG_Ret_MainUserDeath_SC data)
    {
        CopyManager manager = ManagerCenter.Instance.GetManager<CopyManager>();
        if (manager == null)
        {
            FFDebug.LogWarning(this, " CopyManager is null  ");
            return;
        }
        if (manager.InCompetitionCopy)
        {
            return;
        }
        if (this.ReLiveDialogOpen)
        {
            return;
        }
        ManagerCenter.Instance.GetManager<UIManager>().ShowUI<UI_Dialog_Revive>("ui_revive", delegate ()
        {
            this.ReLiveDialogOpen = true;
            if (UIManager.GetUIObject<UI_Dialog_Revive>() != null)
            {
                UIManager.GetUIObject<UI_Dialog_Revive>().Viewrevive(data);
                UIManager.GetUIObject<UI_Dialog_Revive>().OnClose = delegate ()
                {
                    this.ReLiveDialogOpen = false;
                };
            }
        }, UIManager.ParentType.CommonUI, false);
    }

    public void OnMapScreenBatchRefreshNpc(MSG_Ret_MapScreenBatchRefreshNpc_SC mdata)
    {
        PryController controller = ControllerManager.Instance.GetController<PryController>();
        int i = 0;
        while (i < mdata.data.Count)
        {
            MapNpcData mapNpcData = mdata.data[i];
            Npc npc = this.entitiesManager.GetNpc((ulong)((uint)mdata.data[i].tempid));
            if (npc != null)
            {
                npc.NpcData.RefreshMapNpcData(new cs_MapNpcData(mapNpcData));
                npc.RefreshNPC();
                goto IL_136;
            }
            if (!controller.IsInVisableSpecialNpc(mapNpcData.baseid))
            {
                FFDebug.Log(this, FFLogType.Npc, string.Concat(new object[]
                {
                    "OnMapScreenBatchRefreshNpc  ",
                    mapNpcData.baseid,
                    "  ",
                    mapNpcData.tempid
                }));
                npc = EntitesFactory.CreatNpc(new cs_MapNpcData(mapNpcData));
                if (npc != null)
                {
                    this.entitiesManager.AddNpc(npc);
                    if (npc is Npc_BuildingAirWall)
                    {
                        (npc as Npc_BuildingAirWall).CreatNpc();
                    }
                    else if (npc is Npc_Building)
                    {
                        (npc as Npc_Building).CreatNpc();
                    }
                    else
                    {
                        npc.CreatNpc();
                    }
                }
                goto IL_136;
            }
            FFDebug.Log(this, FFLogType.Player, "InVisableSpecialNpc " + mapNpcData.baseid);
        IL_158:
            i++;
            continue;
        IL_136:
            if (npc != null && mapNpcData.hatredlist != null)
            {
                npc.UpdateHatredList(mapNpcData.hatredlist.enemytempid);
                goto IL_158;
            }
            goto IL_158;
        }
    }

    public void OnMapScreenBatchRemoveNpc(MSG_Ret_MapScreenBatchRemoveNpc_SC mdata)
    {
        FFDebug.Log(this, FFLogType.Npc, "OnMapScreenBatchRemoveNpc");
        for (int i = 0; i < mdata.tempids.Count; i++)
        {
            Npc npc = this.entitiesManager.GetNpc((ulong)((uint)mdata.tempids[i]));
            if (npc == null)
            {
                FFDebug.Log(this, FFLogType.Npc, "Doesnt exist NPC with id :" + mdata.tempids[i]);
            }
            else
            {
                this.entitiesManager.RemoveNpc(npc);
                if (npc is Npc_Building)
                {
                    (npc as Npc_Building).DestroyThisInNineScreen();
                }
                else
                {
                    npc.DestroyThisInNineScreen();
                }
            }
        }
    }

    public void OnMapScreenRefreshNpc(MSG_Ret_MapScreenRefreshNpc_SC mdata)
    {
        FFDebug.Log(this, FFLogType.Npc, string.Format("OnMapScreenRefreshNpc  baseid: {0} tempid:{1}  movespeed: {2}", mdata.data.baseid, mdata.data.tempid, mdata.data.movespeed));
        Npc npc = this.entitiesManager.GetNpc((ulong)((uint)mdata.data.tempid));
        PryController controller = ControllerManager.Instance.GetController<PryController>();
        if (npc == null)
        {
            if (controller.IsInVisableSpecialNpc(mdata.data.baseid))
            {
                FFDebug.LogWarning(this, "InVisableSpecialNpc " + mdata.data.baseid);
            }
            else
            {
                npc = EntitesFactory.CreatNpc(new cs_MapNpcData(mdata.data));
                if (npc != null)
                {
                    this.entitiesManager.AddNpc(npc);
                    if (npc is Npc_BuildingAirWall)
                    {
                        (npc as Npc_BuildingAirWall).CreatNpc();
                    }
                    else if (npc is Npc_Building)
                    {
                        (npc as Npc_Building).CreatNpc();
                    }
                    else
                    {
                        npc.CreatNpc();
                    }
                }
            }
        }
        else
        {
            npc.NpcData.RefreshMapNpcData(new cs_MapNpcData(mdata.data));
            npc.RefreshNPC();
        }
        if (npc != null && mdata.data.hatredlist != null)
        {
            npc.UpdateHatredList(mdata.data.hatredlist.enemytempid);
        }
    }

    public void OnMapScreenRemoveNpc(MSG_Ret_MapScreenRemoveNpc_SC mdata)
    {
        Npc npc = this.entitiesManager.GetNpc((ulong)((uint)mdata.tempid));
        if (npc == null)
        {
            FFDebug.Log(this, FFLogType.Network, "Doesnt exist player with id :" + mdata.tempid);
            return;
        }
        FFDebug.Log(this, FFLogType.Npc, "OnMapScreenRemoveNpc..." + npc.NpcData.MapNpcData.name + npc.NpcData.MapNpcData.tempid);
        this.entitiesManager.RemoveNpc(npc);
        if (npc is Npc_Building)
        {
            (npc as Npc_Building).DestroyThisInNineScreen();
        }
        else
        {
            npc.DestroyThisInNineScreen();
        }
    }

    public void OnMapScreenFuncNpc(MSG_Ret_MapScreenFuncNpc_SC mdata)
    {
        this.entitiesManager.ClearFunNpc();
        for (int i = 0; i < mdata.data.Count; i++)
        {
            FuncNpcData funcNpcData = mdata.data[i];
            PryController controller = ControllerManager.Instance.GetController<PryController>();
            FFDebug.Log(this, FFLogType.Npc, string.Concat(new object[]
            {
                "OnMapScreenFuncNpc:  ",
                funcNpcData.baseid,
                " ",
                funcNpcData.tempid
            }));
            if (this.entitiesManager.GetNpc((ulong)((uint)funcNpcData.tempid)) == null)
            {
                if (!controller.IsInVisableSpecialNpc(funcNpcData.baseid))
                {
                    Npc npc = EntitesFactory.CreatNpc(new cs_MapNpcData
                    {
                        baseid = mdata.data[i].baseid,
                        tempid = mdata.data[i].tempid,
                        x = mdata.data[i].x,
                        y = mdata.data[i].y
                    });
                    if (npc != null)
                    {
                        this.entitiesManager.AddFunNpc(npc);
                        if (npc is Npc_BuildingAirWall)
                        {
                            (npc as Npc_BuildingAirWall).CreatNpc();
                        }
                        else if (npc is Npc_Building)
                        {
                            (npc as Npc_Building).CreatNpc();
                        }
                    }
                }
            }
        }
    }

    private void OnRetNpcDir(MSG_RetNpcDir_SC data)
    {
        Npc npc = this.entitiesManager.GetNpc((ulong)((uint)data.tempid));
        if (npc == null)
        {
            return;
        }
        npc.SetPlayerDirection(data.dir, false);
    }

    public void OnRetNpcMove(MSG_Ret_RetNpcMove_SC data)
    {
        Npc npc = this.entitiesManager.GetNpc((ulong)((uint)data.tempid));
        if (npc == null)
        {
            return;
        }
        int count = data.movedata.Count;
        if (!npc.CharactorCheckMove(new Vector2(data.movedata[count - 1].pos.fx, data.movedata[count - 1].pos.fy)))
        {
            for (int i = 0; i < data.movedata.Count; i++)
            {
                cs_MoveData cs_MoveData = new cs_MoveData();
                cs_MoveData.dir = data.movedata[i].dir;
                cs_MoveData.pos = default(cs_FloatMovePos);
                cs_MoveData.pos.fx = data.movedata[i].pos.fx;
                cs_MoveData.pos.fy = data.movedata[i].pos.fy;
                npc.RetMoveDataQueue.Enqueue(cs_MoveData);
            }
            npc.BaseData.RefreshCharaBasePosition(npc.NextPosition2D);
            cs_MoveData cs_MoveData2 = npc.RetMoveDataQueue.Dequeue();
            npc.FixMoveSpeed(new Vector2(cs_MoveData2.pos.fx, cs_MoveData2.pos.fy));
            npc.RecodeSeverMoveData(cs_MoveData2);
            npc.MoveTo(cs_MoveData2);
        }
    }

    public void OnUserRelive(MSG_Ret_MainUserRelive_SC data)
    {
        OtherPlayer playerByID = this.entitiesManager.GetPlayerByID(data.userid);
        if (playerByID != null)
        {
            playerByID.SetPlayerPosition(new Vector2(data.x, data.y), playerByID.ServerDir);
            playerByID.DelayRelive();
            if (playerByID.EID.Equals(MainPlayer.Self.EID))
            {
                CameraController.IsRelive = true;
                if (data.relivetype != ReliveType.RELIVE_ORIGIN)
                {
                    CameraController.IsReliveOrg = false;
                }
                else
                {
                    CameraController.IsReliveOrg = true;
                }
                GlobalRegister.MainUserDeathOrRelive(false);
            }
            if (MainPlayer.Self != null && playerByID.EID.Equals(MainPlayer.Self.EID))
            {
                ManagerCenter.Instance.GetManager<UIManager>().DeleteUI("ui_revive");
                CallLuaListener.SendLuaEvent("OnUserReliveLuaListener", true, new object[]
                {
                    playerByID
                });
            }
        }
    }

    public void ReqRelive(bool ReliveOrgin)
    {
        base.SendMsg<MSG_Req_MainUserRelive_CS>(CommandID.MSG_Req_MainUserRelive_CS, new MSG_Req_MainUserRelive_CS
        {
            relivetype = ((!ReliveOrgin) ? 1U : 2U)
        }, false);
    }

    public void ReqChangeLine(int lineid)
    {
        base.SendMsg<MSG_UserReqChangeLine_CS>(CommandID.MSG_UserReqChangeLine_CS, new MSG_UserReqChangeLine_CS
        {
            lineid = (uint)lineid
        }, false);
    }

    public void ReqSummonNpc(List<TempNpcInfo> list)
    {
        MSG_Req_Summon_Npc_CS msg_Req_Summon_Npc_CS = new MSG_Req_Summon_Npc_CS();
        for (int i = 0; i < list.Count; i++)
        {
            msg_Req_Summon_Npc_CS.npcs.Add(list[i]);
        }
        base.SendMsg<MSG_Req_Summon_Npc_CS>(CommandID.MSG_Req_Summon_Npc_CS, msg_Req_Summon_Npc_CS, false);
    }

    public void OnRetSetTimeState(MSG_Ret_setTimeState_SC data)
    {
        if (ControllerManager.Instance.GetController<MainUIController>().mainView != null)
        {
            ControllerManager.Instance.GetController<MainUIController>().mainView.ShowTimeBuffState(data);
        }
    }

    public void OnRetVisibleNpcList(MSG_Ret_VisibleNpcList_SC msgb)
    {
        PryController controller = ControllerManager.Instance.GetController<PryController>();
        if (controller != null)
        {
            controller.RefreshVisableNpc(msgb);
        }
    }

    public void OnRetStateList(MSG_Ret_StateList_SC msgb)
    {
        if (MainPlayer.Self != null)
        {
            MainPlayer.Self.OtherPlayerData.RefreshState(msgb.states);
        }
    }

    public void ReqEntrySelectState(EntitiesID oldEntry, EntitiesID newEntry, bool istoself = false)
    {
        MSG_ReqEntrySelectState_CS msg_ReqEntrySelectState_CS = new MSG_ReqEntrySelectState_CS();
        msg_ReqEntrySelectState_CS.newone = new EntryIDType();
        msg_ReqEntrySelectState_CS.newone.id = newEntry.Id;
        msg_ReqEntrySelectState_CS.newone.type = (uint)newEntry.Etype;
        msg_ReqEntrySelectState_CS.oldone = new EntryIDType();
        msg_ReqEntrySelectState_CS.oldone.id = oldEntry.Id;
        msg_ReqEntrySelectState_CS.oldone.type = (uint)oldEntry.Etype;
        FFDebug.Log(this, FFLogType.Buff, string.Format("ReqEntrySelectState: oldid = {0}; oldtype = {1}; newid = {2}; newtype = {3}", new object[]
        {
            msg_ReqEntrySelectState_CS.oldone.id,
            msg_ReqEntrySelectState_CS.oldone.type,
            msg_ReqEntrySelectState_CS.newone.id,
            msg_ReqEntrySelectState_CS.newone.type
        }));
        base.SendMsg<MSG_ReqEntrySelectState_CS>(CommandID.MSG_ReqEntrySelectState_CS, msg_ReqEntrySelectState_CS, istoself);
    }

    public void OnRetEntrySelectState(MSG_RetEntrySelectState_SC msgb)
    {
        UserState userState = UserState.USTATE_NOSTATE;
        if (msgb.states.Count > 0)
        {
            userState = CommonTools.GetUserStateFromHash(msgb.states[0].uniqid);
        }
        FFDebug.Log(this, FFLogType.TargetSelect, string.Format("OnRetEntrySelectState: choose id = {0} ; type = {1} ; states.Count = {2},state = {3}", new object[]
        {
            msgb.choosen.id,
            (CharactorType)msgb.choosen.type,
            msgb.states.Count,
            userState
        }));
        MainUIController controller = ControllerManager.Instance.GetController<MainUIController>();
        if (controller != null)
        {
            controller.UpdateTargetBuffIcon(msgb.choosen.id, (CharactorType)msgb.choosen.type, msgb.states);
        }
    }

    public void OnRetHpMpToSelects(MSG_RetHpMpToSelects_SC msgb)
    {
        FFDebug.Log(this, FFLogType.Buff, string.Format("OnRetHpMpToSelects: choose id = {0} type = {1}, curhp = {2}, maxhp = {3}, curmp = {4}, maxmp = {5} ", new object[]
        {
            msgb.choosen.id,
            msgb.choosen.type,
            msgb.curhp,
            msgb.maxhp,
            msgb.curmp,
            msgb.maxmp
        }));
        MainUIController controller = ControllerManager.Instance.GetController<MainUIController>();
        if (controller != null)
        {
            controller.UpdateTargetHP(msgb.choosen.id, msgb.curhp, msgb.maxhp);
        }
        if (this.entitiesManager.NpcList.ContainsKey(msgb.choosen.id))
        {
            FakeHitContorl component = this.entitiesManager.NpcList[msgb.choosen.id].GetComponent<FakeHitContorl>();
            if (component != null)
            {
                component.ResetHpByServerPush(msgb);
            }
        }
    }

    public void OnRetDelSelectRole(MSG_Req_Delete_Char_CSC msgb)
    {
        UI_SelectRole uiobject = UIManager.GetUIObject<UI_SelectRole>();
        if (null == uiobject)
        {
            return;
        }
        uiobject.handleDelCallBack(msgb);
    }

    public void OnRetRankPkReqPrepare(MSG_RankPkReqPrepare_SC msgb)
    {
        ControllerManager.Instance.GetController<PVPMatchController>().OpenMatchCountDown(msgb);
    }

    public void OnRet_NoticeClientAllLines_SC(MSG_NoticeClientAllLines_SC msgb)
    {
        UIMapController controller = ControllerManager.Instance.GetController<UIMapController>();
        controller.OnReceiveLinesData(msgb);
    }

    public void On_StartCutScene_SC(MSG_START_CUTSCENE_SC msgb)
    {
        TaskController tc = ControllerManager.Instance.GetController<TaskController>();
        if (tc != null && tc.CheckCanPlayCutScene(msgb.cutsceneid))
        {
            CutSceneManager manager = ManagerCenter.Instance.GetManager<CutSceneManager>();
            if (manager != null)
            {
            }
            tc.ShowCutSceneByID(msgb.cutsceneid, delegate
            {
                MainPlayer mainPlayer = this.entitiesManager.MainPlayer;
                if (mainPlayer != null)
                {
                    mainPlayer.InitCamera();
                }
                tc.OnCutSceneFinish(msgb.onfinish);
            });
        }
    }

    public void OnRet_PlayStartCutScene_SC(MSG_NEW_ROLE_CUTSCENE_SCS msgb)
    {
        TaskController controller = ControllerManager.Instance.GetController<TaskController>();
        if (controller != null && controller.CheckCanPlayCutScene(2U))
        {
            CutSceneManager manager = ManagerCenter.Instance.GetManager<CutSceneManager>();
            if (manager != null)
            {
                manager.SetMainCamera(false);
                manager.SetUICamera(false);
            }
            controller.ShowCutSceneByID(2U, delegate
            {
                this.ReqStartCutSceneFinish();
                MainPlayer mainPlayer = this.entitiesManager.MainPlayer;
                if (mainPlayer != null)
                {
                    mainPlayer.InitCamera();
                }
            });
        }
    }

    public void ReqStartCutSceneFinish()
    {
        if (MainPlayer.Self == null)
        {
            return;
        }
        MSG_NEW_ROLE_CUTSCENE_SCS t = new MSG_NEW_ROLE_CUTSCENE_SCS();
        base.SendMsg<MSG_NEW_ROLE_CUTSCENE_SCS>(CommandID.MSG_NEW_ROLE_CUTSCENE_SCS, t, false);
    }

    public void ReqBackToRoleList()
    {
        MSG_Req_Back_to_Select_CS t = new MSG_Req_Back_to_Select_CS();
        base.SendMsg<MSG_Req_Back_to_Select_CS>(CommandID.MSG_Req_Back_to_Select_CS, t, false);
    }

    public void OnRet_UserDeath_SC(MSG_Ret_UserDeath_SC msgb)
    {
        EntitiesManager manager = ManagerCenter.Instance.GetManager<EntitiesManager>();
        CharactorBase charactorFromCharid = manager.GetCharactorFromCharid(msgb.tempid);
        if (charactorFromCharid == null)
        {
            return;
        }
        CharactorBase charactorFromCharid2 = manager.GetCharactorFromCharid(msgb.attid);
        if (charactorFromCharid is MainPlayer)
        {
            MainUIController controller = ControllerManager.Instance.GetController<MainUIController>();
            controller.ShowBattleLog("你战败了", true);
            if (charactorFromCharid2 != null)
            {
                string nameByCharactorBase = ControllerManager.Instance.GetController<MainUIController>().GetNameByCharactorBase(charactorFromCharid2);
                controller.ShowBattleLog("打败你的是 " + nameByCharactorBase, true);
            }
        }
        else if (charactorFromCharid2 is MainPlayer)
        {
            string nameByCharactorBase2 = ControllerManager.Instance.GetController<MainUIController>().GetNameByCharactorBase(charactorFromCharid);
            this.mainUIController.ShowBattleLog(nameByCharactorBase2 + "被你击败了", true);
        }
    }

    public void OnRet_User_Drop_SCS(MSG_User_Drop_SCS msgb)
    {
        if (MainPlayer.Self == null)
        {
            return;
        }
        MainPlayer.Self.isFalling = true;
        if (MainPlayer.Self != null && MainPlayer.Self.IsCanJump() && !MainPlayer.Self.inJumpState())
        {
            MainPlayer.Self.Jump(true, true);
        }
    }

    public void MSG_AttackTargetChange_SC(MSG_AttackTargetChange_SC msg)
    {
        UI_MainView mainView = ControllerManager.Instance.GetController<MainUIController>().mainView;
        if (mainView != null)
        {
            mainView.ShowTargetOfTarget(msg);
        }
    }

    public void ReqOnTargetChange_SC(ulong id, ChooseTargetType ctt, MapDataType mdt)
    {
        base.SendMsg<MSG_SetChooseTarget_CS>(CommandID.MSG_SetChooseTarget_CS, new MSG_SetChooseTarget_CS
        {
            charid = id,
            choosetype = ctt,
            mapdatatype = mdt
        }, false);
    }

    public void ReqFallEnd()
    {
        if (MainPlayer.Self == null)
        {
            return;
        }
        MSG_User_Drop_SCS t = new MSG_User_Drop_SCS();
        base.SendMsg<MSG_User_Drop_SCS>(CommandID.MSG_User_Drop_SCS, t, false);
    }

    public void OnReLogin()
    {
        this.isLogin = false;
    }

    private MoveDataCache _mvDataCache = default(MoveDataCache);

    private MoveData _md = new MoveData();

    private cs_MoveData lastSendData;

    private MSG_OnUserJump_CSC reqJump = new MSG_OnUserJump_CSC();

    private bool isLogin;

    private bool ReLiveDialogOpen;

    private bool logMove;
}
