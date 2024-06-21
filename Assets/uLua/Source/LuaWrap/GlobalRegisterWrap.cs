using System;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using LuaInterface;

public class GlobalRegisterWrap
{
	public static void Register(IntPtr L)
	{
		LuaMethod[] regs = new LuaMethod[]
		{
			new LuaMethod("BeginSample", BeginSample),
			new LuaMethod("EndSample", EndSample),
			new LuaMethod("OpenUICustom", OpenUICustom),
			new LuaMethod("ClearChildrens", ClearChildrens),
			new LuaMethod("GetMCurrentCopyID", GetMCurrentCopyID),
			new LuaMethod("SetNpcDlgShowState", SetNpcDlgShowState),
			new LuaMethod("GetMainPlayerHeroId", GetMainPlayerHeroId),
			new LuaMethod("TryToShowCompleteCopyView", TryToShowCompleteCopyView),
			new LuaMethod("GetPathwayID", GetPathwayID),
			new LuaMethod("GetLineId", GetLineId),
			new LuaMethod("GetResourcePropsBaseBy", GetResourcePropsBaseBy),
			new LuaMethod("GetResourcePropsBaseBy_Obj", GetResourcePropsBaseBy_Obj),
			new LuaMethod("GetCurrenyStr", GetCurrenyStr),
			new LuaMethod("SetItemTextName", SetItemTextName),
			new LuaMethod("GetMainPlayerAttributeData", GetMainPlayerAttributeData),
			new LuaMethod("GetCharacterMapData", GetCharacterMapData),
			new LuaMethod("GetSelfFightData", GetSelfFightData),
			new LuaMethod("GetByteCount", GetByteCount),
			new LuaMethod("GetGameTime", GetGameTime),
			new LuaMethod("get_flag", get_flag),
			new LuaMethod("bor_flag", bor_flag),
			new LuaMethod("bxor_flag", bxor_flag),
			new LuaMethod("utf8to", utf8to),
			new LuaMethod("OpenMouseState", OpenMouseState),
			new LuaMethod("AddTextTipComponent", AddTextTipComponent),
			new LuaMethod("AddTextTipComponentString", AddTextTipComponentString),
			new LuaMethod("AddTextTipTimerComponent", AddTextTipTimerComponent),
			new LuaMethod("AddTextTipUpdateComponent", AddTextTipUpdateComponent),
			new LuaMethod("ReqSplitOperate", ReqSplitOperate),
			new LuaMethod("GetColorByName", GetColorByName),
			new LuaMethod("UpdateLastOutlineColor", UpdateLastOutlineColor),
			new LuaMethod("UpdateTextrue", UpdateTextrue),
			new LuaMethod("UpdateIconBG", UpdateIconBG),
			new LuaMethod("MainUIAddMesssage", MainUIAddMesssage),
			new LuaMethod("MainUIReadMesssage", MainUIReadMesssage),
			new LuaMethod("RegisertConvenientFunction", RegisertConvenientFunction),
			new LuaMethod("ShowMsgBoxToFriend", ShowMsgBoxToFriend),
			new LuaMethod("ShowMsgBoxUnpublishMasterApprentice", ShowMsgBoxUnpublishMasterApprentice),
			new LuaMethod("ShowMsgBoxTryQuitApprentice", ShowMsgBoxTryQuitApprentice),
			new LuaMethod("ShowEnsureMsgBox", ShowEnsureMsgBox),
			new LuaMethod("ShowMsgBoxPreQuitApprentice", ShowMsgBoxPreQuitApprentice),
			new LuaMethod("ShowPrivateChat", ShowPrivateChat),
			new LuaMethod("InviteToTeam", InviteToTeam),
			new LuaMethod("ApplyTeam", ApplyTeam),
			new LuaMethod("ActiveNewMailTips", ActiveNewMailTips),
			new LuaMethod("SetTextTimer", SetTextTimer),
			new LuaMethod("ShowMasterOrApprenticeListIsFull", ShowMasterOrApprenticeListIsFull),
			new LuaMethod("GetTimeInHours", GetTimeInHours),
			new LuaMethod("GetTimeInMinutes", GetTimeInMinutes),
			new LuaMethod("GetTimeInDays", GetTimeInDays),
			new LuaMethod("CopyToSameParent", CopyToSameParent),
			new LuaMethod("AddCountDownComponent", AddCountDownComponent),
			new LuaMethod("SetMainPlayerName", SetMainPlayerName),
			new LuaMethod("ShowMsgBox", ShowMsgBox),
			new LuaMethod("ShowTwoBtnMsgBox", ShowTwoBtnMsgBox),
			new LuaMethod("ShowTips", ShowTips),
			new LuaMethod("SetDragDropButtonDataNull", SetDragDropButtonDataNull),
			new LuaMethod("ShowLoadTip", ShowLoadTip),
			new LuaMethod("CloseLoadTip", CloseLoadTip),
			new LuaMethod("ShowCommonItemDesc", ShowCommonItemDesc),
			new LuaMethod("ShowRaneItemDesc", ShowRaneItemDesc),
			new LuaMethod("CheckNameFormat", CheckNameFormat),
			new LuaMethod("LoadByteData", LoadByteData),
			new LuaMethod("EnterCompetition", EnterCompetition),
			new LuaMethod("ExitCompetition", ExitCompetition),
			new LuaMethod("GetTeamList", GetTeamList),
			new LuaMethod("GetTeamInfo", GetTeamInfo),
			new LuaMethod("ChangeMainPanelMenu", ChangeMainPanelMenu),
			new LuaMethod("ShowChatUI", ShowChatUI),
			new LuaMethod("SetChatUIState", SetChatUIState),
			new LuaMethod("SetBagUICloseState", SetBagUICloseState),
			new LuaMethod("SetTaskUICloseState", SetTaskUICloseState),
			new LuaMethod("SetHeroUICloseState", SetHeroUICloseState),
			new LuaMethod("SetCharacterUIAttr", SetCharacterUIAttr),
			new LuaMethod("SetGeneUICloseState", SetGeneUICloseState),
			new LuaMethod("SetInstanceUICloseState", SetInstanceUICloseState),
			new LuaMethod("SetMentoringUICloseState", SetMentoringUICloseState),
			new LuaMethod("SetSysSettingUICloseState", SetSysSettingUICloseState),
			new LuaMethod("OpenShortcutsConfig", OpenShortcutsConfig),
			new LuaMethod("OpenResetSecondPwd", OpenResetSecondPwd),
			new LuaMethod("GetConstStoneID", GetConstStoneID),
			new LuaMethod("GetConstMoneyID", GetConstMoneyID),
			new LuaMethod("GetConstWelPointID", GetConstWelPointID),
			new LuaMethod("ShortCutUseEquipController_Reset", ShortCutUseEquipController_Reset),
			new LuaMethod("MainUIController_AddMessage", MainUIController_AddMessage),
			new LuaMethod("MainUIController_ReadMessage", MainUIController_ReadMessage),
			new LuaMethod("InitBagManager", InitBagManager),
			new LuaMethod("OpenInfo", OpenInfo),
			new LuaMethod("ActivateInputField", ActivateInputField),
			new LuaMethod("SetImageGrey", SetImageGrey),
			new LuaMethod("OpenInfoCommon", OpenInfoCommon),
			new LuaMethod("OpenInfoRune", OpenInfoRune),
			new LuaMethod("OpenInfoShortcutUse", OpenInfoShortcutUse),
			new LuaMethod("CloseInfo", CloseInfo),
			new LuaMethod("OnPlayerCuccencyUpdateEvent_Add", OnPlayerCuccencyUpdateEvent_Add),
			new LuaMethod("OnPlayerCuccencyUpdateEvent_Delete", OnPlayerCuccencyUpdateEvent_Delete),
			new LuaMethod("AddCurrencyItem", AddCurrencyItem),
			new LuaMethod("AddShortcutItem", AddShortcutItem),
			new LuaMethod("OnPlayerCuccencyUpdateEvent", OnPlayerCuccencyUpdateEvent),
			new LuaMethod("IntToEnum", IntToEnum),
			new LuaMethod("GetTidyPackCD", GetTidyPackCD),
			new LuaMethod("GetTidyPackInfo", GetTidyPackInfo),
			new LuaMethod("GetServerDateTimeByTimeStamp", GetServerDateTimeByTimeStamp),
			new LuaMethod("GetServerDateTimeByTimeStampWithAllTime", GetServerDateTimeByTimeStampWithAllTime),
			new LuaMethod("ShowMsgBoxDisengageApprentice", ShowMsgBoxDisengageApprentice),
			new LuaMethod("GetPropsInPackage", GetPropsInPackage),
			new LuaMethod("GetResourceProps", GetResourceProps),
			new LuaMethod("GetMainPlayerMapUserData", GetMainPlayerMapUserData),
			new LuaMethod("ReturnRenderTexture", ReturnRenderTexture),
			new LuaMethod("GetRawCharatorMgr", GetRawCharatorMgr),
			new LuaMethod("GetNpcTalkRawCharatorMgr", GetNpcTalkRawCharatorMgr),
			new LuaMethod("SwitchHeroCb", SwitchHeroCb),
			new LuaMethod("InitFiveanglerObj", InitFiveanglerObj),
			new LuaMethod("DeleteObjMaterialImmediate", DeleteObjMaterialImmediate),
			new LuaMethod("UpdateInputNumPos", UpdateInputNumPos),
			new LuaMethod("GetCurSubCopyMapID", GetCurSubCopyMapID),
			new LuaMethod("TryEnterAutoTaskState", TryEnterAutoTaskState),
			new LuaMethod("RefreshTaskInfoInMainUI", RefreshTaskInfoInMainUI),
			new LuaMethod("PathFindByIDAndState", PathFindByIDAndState),
			new LuaMethod("RefreshPKMode", RefreshPKMode),
			new LuaMethod("ReqExecuteQuest", ReqExecuteQuest),
			new LuaMethod("ReqExecuteQuest2ChangeBuf", ReqExecuteQuest2ChangeBuf),
			new LuaMethod("ChangeTextModel", ChangeTextModel),
			new LuaMethod("ShowMsgBoxAbandonQuest", ShowMsgBoxAbandonQuest),
			new LuaMethod("ShowMsgBoxAllUnload", ShowMsgBoxAllUnload),
			new LuaMethod("ShowMsgBoxActiveRune", ShowMsgBoxActiveRune),
			new LuaMethod("ShowMsgBoxSelectActivateRune", ShowMsgBoxSelectActivateRune),
			new LuaMethod("GetModelColor", GetModelColor),
			new LuaMethod("RefreshTaskState", RefreshTaskState),
			new LuaMethod("ShowAllNPCInMap", ShowAllNPCInMap),
			new LuaMethod("SetCameraDistance", SetCameraDistance),
			new LuaMethod("SetCameraTrack", SetCameraTrack),
			new LuaMethod("SetCameraSpeed", SetCameraSpeed),
			new LuaMethod("SetScreenFloor", SetScreenFloor),
			new LuaMethod("SetScreenShadow", SetScreenShadow),
			new LuaMethod("SetScreenDepthOfField", SetScreenDepthOfField),
			new LuaMethod("SetScreenAntialiasing", SetScreenAntialiasing),
			new LuaMethod("SetScreenBloomOptimized", SetScreenBloomOptimized),
			new LuaMethod("SetLoadLowPriorityObject", SetLoadLowPriorityObject),
			new LuaMethod("SetScreenResolution", SetScreenResolution),
			new LuaMethod("LogoutGame", LogoutGame),
			new LuaMethod("GetServerName", GetServerName),
			new LuaMethod("SetTeamIconInfo", SetTeamIconInfo),
			new LuaMethod("RetNewApply", RetNewApply),
			new LuaMethod("TeamActivityChanged", TeamActivityChanged),
			new LuaMethod("IsMainPlayerTeamLeader", IsMainPlayerTeamLeader),
			new LuaMethod("IsMainPlayerHasTeam", IsMainPlayerHasTeam),
			new LuaMethod("GetMyTeamMemCount", GetMyTeamMemCount),
			new LuaMethod("GetMyTeamMemMaxCount", GetMyTeamMemMaxCount),
			new LuaMethod("CheckIfTeamFull", CheckIfTeamFull),
			new LuaMethod("GetTeamMemberName", GetTeamMemberName),
			new LuaMethod("ShowBoxWhenEnterAdventure", ShowBoxWhenEnterAdventure),
			new LuaMethod("ShowMsgBoxWhenEnterAdventure", ShowMsgBoxWhenEnterAdventure),
			new LuaMethod("SetIsReqNearBy", SetIsReqNearBy),
			new LuaMethod("RefreshOccupyNpcShow", RefreshOccupyNpcShow),
			new LuaMethod("GetMainPlayerIdStr", GetMainPlayerIdStr),
			new LuaMethod("CheckCloseAllTeam", CheckCloseAllTeam),
			new LuaMethod("PathFindWithPathWayId", PathFindWithPathWayId),
			new LuaMethod("GetCopyInfiniteNum", GetCopyInfiniteNum),
			new LuaMethod("GetTeamLeaderID", GetTeamLeaderID),
			new LuaMethod("GetOpenChatType", GetOpenChatType),
			new LuaMethod("SaveOpenChatType", SaveOpenChatType),
			new LuaMethod("SetCurOpenChatType", SetCurOpenChatType),
			new LuaMethod("AddPrivateChatData", AddPrivateChatData),
			new LuaMethod("OnReceiveFriendChatData", OnReceiveFriendChatData),
			new LuaMethod("OnReceiveChatData", OnReceiveChatData),
			new LuaMethod("RegistGMInputField", RegistGMInputField),
			new LuaMethod("RemoveGMInputField", RemoveGMInputField),
			new LuaMethod("IsInCompleteCopy", IsInCompleteCopy),
			new LuaMethod("ShowLittleChat", ShowLittleChat),
			new LuaMethod("ShowMsgBoxByChat", ShowMsgBoxByChat),
			new LuaMethod("ShowNotice", ShowNotice),
			new LuaMethod("RemoveTextModel", RemoveTextModel),
			new LuaMethod("ShowWindow", ShowWindow),
			new LuaMethod("AddChatItem", AddChatItem),
			new LuaMethod("AddChatItem_Tips", AddChatItem_Tips),
			new LuaMethod("GetCampNeutralID", GetCampNeutralID),
			new LuaMethod("GetModelColorStr", GetModelColorStr),
			new LuaMethod("ProcessGMWithSpace", ProcessGMWithSpace),
			new LuaMethod("IsLocalGmText", IsLocalGmText),
			new LuaMethod("ShowMsgBoxCreateTeam", ShowMsgBoxCreateTeam),
			new LuaMethod("AddTextModel", AddTextModel),
			new LuaMethod("ForceRebuild", ForceRebuild),
			new LuaMethod("NewRichText", NewRichText),
			new LuaMethod("IsteamMember", IsteamMember),
			new LuaMethod("IsActiveAutoButton", IsActiveAutoButton),
			new LuaMethod("SetAutoAttack", SetAutoAttack),
			new LuaMethod("SetAttactFollowTeamLeader", SetAttactFollowTeamLeader),
			new LuaMethod("GetUIWorldToScreenPoint", GetUIWorldToScreenPoint),
			new LuaMethod("GetUIScreenPointToLocalPointInRectangle", GetUIScreenPointToLocalPointInRectangle),
			new LuaMethod("GetPlayerHeroID", GetPlayerHeroID),
			new LuaMethod("GetPlayerTeamID", GetPlayerTeamID),
			new LuaMethod("GetPlayerGuildID", GetPlayerGuildID),
			new LuaMethod("GetMainPlayerGuildID", GetMainPlayerGuildID),
			new LuaMethod("GetPlayerGuildName", GetPlayerGuildName),
			new LuaMethod("ProcessLuaCallback", ProcessLuaCallback),
			new LuaMethod("ResetNPCDir", ResetNPCDir),
			new LuaMethod("GetNpcTalkName", GetNpcTalkName),
			new LuaMethod("GetTalkNpcGameObject", GetTalkNpcGameObject),
			new LuaMethod("ViewMask", ViewMask),
			new LuaMethod("ProcessNpcdlgBreakAutoattack", ProcessNpcdlgBreakAutoattack),
			new LuaMethod("TextureasseetToSprite", TextureasseetToSprite),
			new LuaMethod("PlayDramaActionShake", PlayDramaActionShake),
			new LuaMethod("CheckIfInTeam", CheckIfInTeam),
			new LuaMethod("ShowMsgBoxCommon", ShowMsgBoxCommon),
			new LuaMethod("GetCurrentCopyID", GetCurrentCopyID),
			new LuaMethod("GetClientDirVector2ByServerDir", GetClientDirVector2ByServerDir),
			new LuaMethod("GetServerPosByWorldPos", GetServerPosByWorldPos),
			new LuaMethod("GetWorldPosByServerPos", GetWorldPosByServerPos),
			new LuaMethod("GetServerDirByClientDir", GetServerDirByClientDir),
			new LuaMethod("GetCurrencyByID", GetCurrencyByID),
			new LuaMethod("GetBluePointToday", GetBluePointToday),
			new LuaMethod("GetMainCameraTrans", GetMainCameraTrans),
			new LuaMethod("GetMainCameraAngle", GetMainCameraAngle),
			new LuaMethod("GetMainPlayerTrans", GetMainPlayerTrans),
			new LuaMethod("GetMapID", GetMapID),
			new LuaMethod("BeginFindPath", BeginFindPath),
			new LuaMethod("ReSetAndPlayTweens", ReSetAndPlayTweens),
			new LuaMethod("RefreshCopyTask", RefreshCopyTask),
			new LuaMethod("ShowMsgBoxUseVitPill", ShowMsgBoxUseVitPill),
			new LuaMethod("ShowAddNewItem", ShowAddNewItem),
			new LuaMethod("RefreshBagItem", RefreshBagItem),
			new LuaMethod("RefreshTreasureHuntInfo", RefreshTreasureHuntInfo),
			new LuaMethod("CloseTreasureHunt", CloseTreasureHunt),
			new LuaMethod("SetMapNameAction", SetMapNameAction),
			new LuaMethod("SetStoryNameAction", SetStoryNameAction),
			new LuaMethod("SetImage", SetImage),
			new LuaMethod("GetSpriteFromAtlas", GetSpriteFromAtlas),
			new LuaMethod("GetCanvasRenderer", GetCanvasRenderer),
			new LuaMethod("OnTaskStateChange", OnTaskStateChange),
			new LuaMethod("SetImageBlur", SetImageBlur),
			new LuaMethod("SetRawImageBlur", SetRawImageBlur),
			new LuaMethod("ActiveAllPublicCD", ActiveAllPublicCD),
			new LuaMethod("GetCurrentBattleHeroThisid", GetCurrentBattleHeroThisid),
			new LuaMethod("IsPointerEnterUI", IsPointerEnterUI),
			new LuaMethod("SetUIInfoPos", SetUIInfoPos),
			new LuaMethod("ShowMsgBoxHeroUpgradeBind", ShowMsgBoxHeroUpgradeBind),
			new LuaMethod("SetHeroSimpleAttrPos", SetHeroSimpleAttrPos),
			new LuaMethod("RefrashShortcutItemCount", RefrashShortcutItemCount),
			new LuaMethod("LogMessage", LogMessage),
			new LuaMethod("SetShortcutKeyEnableState", SetShortcutKeyEnableState),
			new LuaMethod("ConfigColorToRichTextFormat", ConfigColorToRichTextFormat),
			new LuaMethod("StripColorText", StripColorText),
			new LuaMethod("GetWayFindItemByQuestID", GetWayFindItemByQuestID),
			new LuaMethod("GetTaskGoalStr", GetTaskGoalStr),
			new LuaMethod("GetWayFindItemByStr", GetWayFindItemByStr),
			new LuaMethod("VisitLastVisitNpc", VisitLastVisitNpc),
			new LuaMethod("OnCurActiveQuestFrash", OnCurActiveQuestFrash),
			new LuaMethod("GetLastVisitNpcTmpId", GetLastVisitNpcTmpId),
			new LuaMethod("GetLastVisitNpcId", GetLastVisitNpcId),
			new LuaMethod("Clear3DIconData", Clear3DIconData),
			new LuaMethod("ShowMainPlayerRTT", ShowMainPlayerRTT),
			new LuaMethod("ShowNpcOrPlayerRTT", ShowNpcOrPlayerRTT),
			new LuaMethod("OpenMailUI", OpenMailUI),
			new LuaMethod("UpdateBubbleOfMail", UpdateBubbleOfMail),
			new LuaMethod("ShowPlayerRTT", ShowPlayerRTT),
			new LuaMethod("SetRTTInfo", SetRTTInfo),
			new LuaMethod("RemoveEffectLayerObject", RemoveEffectLayerObject),
			new LuaMethod("OpenGeneUI", OpenGeneUI),
			new LuaMethod("OpenUnLockSkillsUI", OpenUnLockSkillsUI),
			new LuaMethod("OnRetLevelupHeroSkill", OnRetLevelupHeroSkill),
			new LuaMethod("DelayCloseUI", DelayCloseUI),
			new LuaMethod("ItemSplitNumAddBtnDownUpListen", ItemSplitNumAddBtnDownUpListen),
			new LuaMethod("ItemSplitNumMinusBtnDownUpListen", ItemSplitNumMinusBtnDownUpListen),
			new LuaMethod("GetMainPlayerCharID", GetMainPlayerCharID),
			new LuaMethod("GetCurrentPriChatFileName", GetCurrentPriChatFileName),
			new LuaMethod("SaveChatDataAsFile", SaveChatDataAsFile),
			new LuaMethod("ProcessChatTime", ProcessChatTime),
			new LuaMethod("ProcessMailRemainTime", ProcessMailRemainTime),
			new LuaMethod("GetPropsBaseByID", GetPropsBaseByID),
			new LuaMethod("CompareLong", CompareLong),
			new LuaMethod("GetSelectTargetID", GetSelectTargetID),
			new LuaMethod("IsBitTrue", IsBitTrue),
			new LuaMethod("MainUserDeathOrRelive", MainUserDeathOrRelive),
			new LuaMethod("InitTaskTrackConcurrentItem", InitTaskTrackConcurrentItem),
			new LuaMethod("ClearSelectNpcState", ClearSelectNpcState),
			new LuaMethod("PlayProgressBarAndAnimationInLocal", PlayProgressBarAndAnimationInLocal),
			new LuaMethod("OpenShopUI", OpenShopUI),
			new LuaMethod("OpenExchange", OpenExchange),
			new LuaMethod("UIPostEvent", UIPostEvent),
			new LuaMethod("AddUISoundListener", AddUISoundListener),
			new LuaMethod("AddItemTip", AddItemTip),
			new LuaMethod("CheckTalkCloseGuideNext", CheckTalkCloseGuideNext),
			new LuaMethod("RefreshShortcutCD", RefreshShortcutCD),
			new LuaMethod("RefreshBagNewTip", RefreshBagNewTip),
			new LuaMethod("New", _CreateGlobalRegister),
			new LuaMethod("GetClassType", GetClassType),
		};

		LuaField[] fields = new LuaField[]
		{
			new LuaField("isMainUserDeathing", get_isMainUserDeathing, set_isMainUserDeathing),
			new LuaField("rtObjRoot", get_rtObjRoot, null),
		};

		LuaScriptMgr.RegisterLib(L, "GlobalRegister", typeof(GlobalRegister), regs, fields, null);
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateGlobalRegister(IntPtr L)
	{
		LuaDLL.luaL_error(L, "GlobalRegister class does not have a constructor function");
		return 0;
	}

	static Type classType = typeof(GlobalRegister);

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetClassType(IntPtr L)
	{
		LuaScriptMgr.Push(L, classType);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_isMainUserDeathing(IntPtr L)
	{
		LuaScriptMgr.Push(L, GlobalRegister.isMainUserDeathing);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_rtObjRoot(IntPtr L)
	{
		LuaScriptMgr.Push(L, GlobalRegister.rtObjRoot);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_isMainUserDeathing(IntPtr L)
	{
		GlobalRegister.isMainUserDeathing = LuaScriptMgr.GetBoolean(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int BeginSample(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 1)
		{
			int arg0 = (int)LuaScriptMgr.GetNumber(L, 1);
			GlobalRegister.BeginSample(arg0);
			return 0;
		}
		else if (count == 2)
		{
			int arg0 = (int)LuaScriptMgr.GetNumber(L, 1);
			string arg1 = LuaScriptMgr.GetLuaString(L, 2);
			GlobalRegister.BeginSample(arg0,arg1);
			return 0;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: GlobalRegister.BeginSample");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int EndSample(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 0);
		GlobalRegister.EndSample();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int OpenUICustom(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		string arg0 = LuaScriptMgr.GetLuaString(L, 1);
		GameObject arg1 = (GameObject)LuaScriptMgr.GetUnityObject(L, 2, typeof(GameObject));
		GlobalRegister.OpenUICustom(arg0,arg1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ClearChildrens(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		Transform arg0 = (Transform)LuaScriptMgr.GetUnityObject(L, 1, typeof(Transform));
		GlobalRegister.ClearChildrens(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetMCurrentCopyID(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 0);
		uint o = GlobalRegister.GetMCurrentCopyID();
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetNpcDlgShowState(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		bool arg0 = LuaScriptMgr.GetBoolean(L, 1);
		GlobalRegister.SetNpcDlgShowState(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetMainPlayerHeroId(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 0);
		uint o = GlobalRegister.GetMainPlayerHeroId();
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int TryToShowCompleteCopyView(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 0);
		GlobalRegister.TryToShowCompleteCopyView();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetPathwayID(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 0);
		string o = GlobalRegister.GetPathwayID();
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetLineId(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 0);
		int o = GlobalRegister.GetLineId();
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetResourcePropsBaseBy(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		uint arg0 = (uint)LuaScriptMgr.GetNumber(L, 1);
		PropsBase o = GlobalRegister.GetResourcePropsBaseBy(arg0);
		LuaScriptMgr.PushObject(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetResourcePropsBaseBy_Obj(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		Obj.t_Object arg0 = (Obj.t_Object)LuaScriptMgr.GetNetObject(L, 1, typeof(Obj.t_Object));
		PropsBase o = GlobalRegister.GetResourcePropsBaseBy_Obj(arg0);
		LuaScriptMgr.PushObject(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetCurrenyStr(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		uint arg0 = (uint)LuaScriptMgr.GetNumber(L, 1);
		string o = GlobalRegister.GetCurrenyStr(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetItemTextName(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		Text arg0 = (Text)LuaScriptMgr.GetUnityObject(L, 1, typeof(Text));
		LuaTable arg1 = LuaScriptMgr.GetLuaTable(L, 2);
		GlobalRegister.SetItemTextName(arg0,arg1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetMainPlayerAttributeData(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 0);
		cs_AttributeData o = GlobalRegister.GetMainPlayerAttributeData();
		LuaScriptMgr.PushObject(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetCharacterMapData(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		CharactorBase arg0 = (CharactorBase)LuaScriptMgr.GetNetObject(L, 1, typeof(CharactorBase));
		cs_MapUserData o = GlobalRegister.GetCharacterMapData(arg0);
		LuaScriptMgr.PushObject(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetSelfFightData(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 0);
		uint o = GlobalRegister.GetSelfFightData();
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetByteCount(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		string arg0 = LuaScriptMgr.GetLuaString(L, 1);
		int o = GlobalRegister.GetByteCount(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetGameTime(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 0);
		GameTime o = GlobalRegister.GetGameTime();
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_flag(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		uint arg0 = (uint)LuaScriptMgr.GetNumber(L, 1);
		uint arg1 = (uint)LuaScriptMgr.GetNumber(L, 2);
		bool o = GlobalRegister.get_flag(arg0,arg1);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int bor_flag(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		uint arg0 = (uint)LuaScriptMgr.GetNumber(L, 1);
		uint arg1 = (uint)LuaScriptMgr.GetNumber(L, 2);
		uint o = GlobalRegister.bor_flag(arg0,arg1);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int bxor_flag(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		uint arg0 = (uint)LuaScriptMgr.GetNumber(L, 1);
		uint arg1 = (uint)LuaScriptMgr.GetNumber(L, 2);
		uint o = GlobalRegister.bxor_flag(arg0,arg1);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int utf8to(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		string arg0 = LuaScriptMgr.GetLuaString(L, 1);
		string o = GlobalRegister.utf8to(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int OpenMouseState(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		int arg0 = (int)LuaScriptMgr.GetNumber(L, 1);
		GlobalRegister.OpenMouseState(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int AddTextTipComponent(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		GameObject arg0 = (GameObject)LuaScriptMgr.GetUnityObject(L, 1, typeof(GameObject));
		uint arg1 = (uint)LuaScriptMgr.GetNumber(L, 2);
		GlobalRegister.AddTextTipComponent(arg0,arg1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int AddTextTipComponentString(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		GameObject arg0 = (GameObject)LuaScriptMgr.GetUnityObject(L, 1, typeof(GameObject));
		string arg1 = LuaScriptMgr.GetLuaString(L, 2);
		GlobalRegister.AddTextTipComponentString(arg0,arg1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int AddTextTipTimerComponent(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 5);
		GameObject arg0 = (GameObject)LuaScriptMgr.GetUnityObject(L, 1, typeof(GameObject));
		uint arg1 = (uint)LuaScriptMgr.GetNumber(L, 2);
		float arg2 = (float)LuaScriptMgr.GetNumber(L, 3);
		float arg3 = (float)LuaScriptMgr.GetNumber(L, 4);
		float arg4 = (float)LuaScriptMgr.GetNumber(L, 5);
		GlobalRegister.AddTextTipTimerComponent(arg0,arg1,arg2,arg3,arg4);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int AddTextTipUpdateComponent(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 5);
		GameObject arg0 = (GameObject)LuaScriptMgr.GetUnityObject(L, 1, typeof(GameObject));
		uint arg1 = (uint)LuaScriptMgr.GetNumber(L, 2);
		float arg2 = (float)LuaScriptMgr.GetNumber(L, 3);
		float arg3 = (float)LuaScriptMgr.GetNumber(L, 4);
		float arg4 = (float)LuaScriptMgr.GetNumber(L, 5);
		GlobalRegister.AddTextTipUpdateComponent(arg0,arg1,arg2,arg3,arg4);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ReqSplitOperate(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		uint arg0 = (uint)LuaScriptMgr.GetNumber(L, 1);
		int arg1 = (int)LuaScriptMgr.GetNumber(L, 2);
		GlobalRegister.ReqSplitOperate(arg0,arg1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetColorByName(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		string arg0 = LuaScriptMgr.GetLuaString(L, 1);
		Color o = GlobalRegister.GetColorByName(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int UpdateLastOutlineColor(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		Transform arg0 = (Transform)LuaScriptMgr.GetUnityObject(L, 1, typeof(Transform));
		string arg1 = LuaScriptMgr.GetLuaString(L, 2);
		GlobalRegister.UpdateLastOutlineColor(arg0,arg1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int UpdateTextrue(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 3);
		Image arg0 = (Image)LuaScriptMgr.GetUnityObject(L, 1, typeof(Image));
		uint arg1 = (uint)LuaScriptMgr.GetNumber(L, 2);
		string arg2 = LuaScriptMgr.GetLuaString(L, 3);
		GlobalRegister.UpdateTextrue(arg0,arg1,arg2);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int UpdateIconBG(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		Image arg0 = (Image)LuaScriptMgr.GetUnityObject(L, 1, typeof(Image));
		uint arg1 = (uint)LuaScriptMgr.GetNumber(L, 2);
		GlobalRegister.UpdateIconBG(arg0,arg1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int MainUIAddMesssage(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 3);
		int arg0 = (int)LuaScriptMgr.GetNumber(L, 1);
		int arg1 = (int)LuaScriptMgr.GetNumber(L, 2);
		Action arg2 = null;
		LuaTypes funcType3 = LuaDLL.lua_type(L, 3);

		if (funcType3 != LuaTypes.LUA_TFUNCTION)
		{
			 arg2 = (Action)LuaScriptMgr.GetNetObject(L, 3, typeof(Action));
		}
		else
		{
			LuaFunction func = LuaScriptMgr.GetLuaFunction(L, 3);
			arg2 = () =>
			{
				func.Call();
			};
		}

		GlobalRegister.MainUIAddMesssage(arg0,arg1,arg2);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int MainUIReadMesssage(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		int arg0 = (int)LuaScriptMgr.GetNumber(L, 1);
		GlobalRegister.MainUIReadMesssage(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int RegisertConvenientFunction(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		uint arg0 = (uint)LuaScriptMgr.GetNumber(L, 1);
		LuaFunction arg1 = LuaScriptMgr.GetLuaFunction(L, 2);
		GlobalRegister.RegisertConvenientFunction(arg0,arg1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ShowMsgBoxToFriend(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		bool arg0 = LuaScriptMgr.GetBoolean(L, 1);
		LuaFunction arg1 = LuaScriptMgr.GetLuaFunction(L, 2);
		GlobalRegister.ShowMsgBoxToFriend(arg0,arg1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ShowMsgBoxUnpublishMasterApprentice(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		bool arg0 = LuaScriptMgr.GetBoolean(L, 1);
		LuaFunction arg1 = LuaScriptMgr.GetLuaFunction(L, 2);
		GlobalRegister.ShowMsgBoxUnpublishMasterApprentice(arg0,arg1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ShowMsgBoxTryQuitApprentice(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 3);
		bool arg0 = LuaScriptMgr.GetBoolean(L, 1);
		string arg1 = LuaScriptMgr.GetLuaString(L, 2);
		LuaFunction arg2 = LuaScriptMgr.GetLuaFunction(L, 3);
		GlobalRegister.ShowMsgBoxTryQuitApprentice(arg0,arg1,arg2);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ShowEnsureMsgBox(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 6);
		string arg0 = LuaScriptMgr.GetLuaString(L, 1);
		string arg1 = LuaScriptMgr.GetLuaString(L, 2);
		string arg2 = LuaScriptMgr.GetLuaString(L, 3);
		string arg3 = LuaScriptMgr.GetLuaString(L, 4);
		LuaFunction arg4 = LuaScriptMgr.GetLuaFunction(L, 5);
		LuaFunction arg5 = LuaScriptMgr.GetLuaFunction(L, 6);
		GlobalRegister.ShowEnsureMsgBox(arg0,arg1,arg2,arg3,arg4,arg5);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ShowMsgBoxPreQuitApprentice(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 6);
		uint arg0 = (uint)LuaScriptMgr.GetNumber(L, 1);
		string arg1 = LuaScriptMgr.GetLuaString(L, 2);
		uint arg2 = (uint)LuaScriptMgr.GetNumber(L, 3);
		uint arg3 = (uint)LuaScriptMgr.GetNumber(L, 4);
		uint arg4 = (uint)LuaScriptMgr.GetNumber(L, 5);
		LuaFunction arg5 = LuaScriptMgr.GetLuaFunction(L, 6);
		GlobalRegister.ShowMsgBoxPreQuitApprentice(arg0,arg1,arg2,arg3,arg4,arg5);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ShowPrivateChat(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 4);
		ulong arg0 = (ulong)LuaScriptMgr.GetNumber(L, 1);
		string arg1 = LuaScriptMgr.GetLuaString(L, 2);
		uint arg2 = (uint)LuaScriptMgr.GetNumber(L, 3);
		uint arg3 = (uint)LuaScriptMgr.GetNumber(L, 4);
		GlobalRegister.ShowPrivateChat(arg0,arg1,arg2,arg3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int InviteToTeam(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		ulong arg0 = (ulong)LuaScriptMgr.GetNumber(L, 1);
		GlobalRegister.InviteToTeam(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ApplyTeam(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		uint arg0 = (uint)LuaScriptMgr.GetNumber(L, 1);
		GlobalRegister.ApplyTeam(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ActiveNewMailTips(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		int arg0 = (int)LuaScriptMgr.GetNumber(L, 1);
		GlobalRegister.ActiveNewMailTips(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetTextTimer(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		Text arg0 = (Text)LuaScriptMgr.GetUnityObject(L, 1, typeof(Text));
		uint arg1 = (uint)LuaScriptMgr.GetNumber(L, 2);
		GlobalRegister.SetTextTimer(arg0,arg1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ShowMasterOrApprenticeListIsFull(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		bool arg0 = LuaScriptMgr.GetBoolean(L, 1);
		GlobalRegister.ShowMasterOrApprenticeListIsFull(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetTimeInHours(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		uint arg0 = (uint)LuaScriptMgr.GetNumber(L, 1);
		string o = GlobalRegister.GetTimeInHours(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetTimeInMinutes(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		uint arg0 = (uint)LuaScriptMgr.GetNumber(L, 1);
		string o = GlobalRegister.GetTimeInMinutes(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetTimeInDays(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		uint arg0 = (uint)LuaScriptMgr.GetNumber(L, 1);
		string o = GlobalRegister.GetTimeInDays(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int CopyToSameParent(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		Transform arg0 = (Transform)LuaScriptMgr.GetUnityObject(L, 1, typeof(Transform));
		int arg1 = (int)LuaScriptMgr.GetNumber(L, 2);
		GlobalRegister.CopyToSameParent(arg0,arg1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int AddCountDownComponent(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 3);
		GameObject arg0 = (GameObject)LuaScriptMgr.GetUnityObject(L, 1, typeof(GameObject));
		Text arg1 = (Text)LuaScriptMgr.GetUnityObject(L, 2, typeof(Text));
		uint arg2 = (uint)LuaScriptMgr.GetNumber(L, 3);
		CountDownItem o = GlobalRegister.AddCountDownComponent(arg0,arg1,arg2);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetMainPlayerName(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		string arg0 = LuaScriptMgr.GetLuaString(L, 1);
		GlobalRegister.SetMainPlayerName(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ShowMsgBox(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 3);
		string arg0 = LuaScriptMgr.GetLuaString(L, 1);
		LuaFunction arg1 = LuaScriptMgr.GetLuaFunction(L, 2);
		LuaFunction arg2 = LuaScriptMgr.GetLuaFunction(L, 3);
		GlobalRegister.ShowMsgBox(arg0,arg1,arg2);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ShowTwoBtnMsgBox(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 7);
		uint arg0 = (uint)LuaScriptMgr.GetNumber(L, 1);
		uint arg1 = (uint)LuaScriptMgr.GetNumber(L, 2);
		uint arg2 = (uint)LuaScriptMgr.GetNumber(L, 3);
		uint arg3 = (uint)LuaScriptMgr.GetNumber(L, 4);
		UIManager.ParentType arg4 = (UIManager.ParentType)LuaScriptMgr.GetNetObject(L, 5, typeof(UIManager.ParentType));
		LuaFunction arg5 = LuaScriptMgr.GetLuaFunction(L, 6);
		LuaFunction arg6 = LuaScriptMgr.GetLuaFunction(L, 7);
		GlobalRegister.ShowTwoBtnMsgBox(arg0,arg1,arg2,arg3,arg4,arg5,arg6);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ShowTips(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		uint arg0 = (uint)LuaScriptMgr.GetNumber(L, 1);
		GlobalRegister.ShowTips(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetDragDropButtonDataNull(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		GameObject arg0 = (GameObject)LuaScriptMgr.GetUnityObject(L, 1, typeof(GameObject));
		GlobalRegister.SetDragDropButtonDataNull(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ShowLoadTip(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		string arg0 = LuaScriptMgr.GetLuaString(L, 1);
		GlobalRegister.ShowLoadTip(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int CloseLoadTip(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 0);
		GlobalRegister.CloseLoadTip();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ShowCommonItemDesc(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		uint arg0 = (uint)LuaScriptMgr.GetNumber(L, 1);
		Transform arg1 = (Transform)LuaScriptMgr.GetUnityObject(L, 2, typeof(Transform));
		GlobalRegister.ShowCommonItemDesc(arg0,arg1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ShowRaneItemDesc(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 6);
		Transform arg0 = (Transform)LuaScriptMgr.GetUnityObject(L, 1, typeof(Transform));
		LuaTable arg1 = LuaScriptMgr.GetLuaTable(L, 2);
		string arg2 = LuaScriptMgr.GetLuaString(L, 3);
		int arg3 = (int)LuaScriptMgr.GetNumber(L, 4);
		Action arg4 = null;
		LuaTypes funcType5 = LuaDLL.lua_type(L, 5);

		if (funcType5 != LuaTypes.LUA_TFUNCTION)
		{
			 arg4 = (Action)LuaScriptMgr.GetNetObject(L, 5, typeof(Action));
		}
		else
		{
			LuaFunction func = LuaScriptMgr.GetLuaFunction(L, 5);
			arg4 = () =>
			{
				func.Call();
			};
		}

		Action arg5 = null;
		LuaTypes funcType6 = LuaDLL.lua_type(L, 6);

		if (funcType6 != LuaTypes.LUA_TFUNCTION)
		{
			 arg5 = (Action)LuaScriptMgr.GetNetObject(L, 6, typeof(Action));
		}
		else
		{
			LuaFunction func = LuaScriptMgr.GetLuaFunction(L, 6);
			arg5 = () =>
			{
				func.Call();
			};
		}

		GlobalRegister.ShowRaneItemDesc(arg0,arg1,arg2,arg3,arg4,arg5);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int CheckNameFormat(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		string arg0 = LuaScriptMgr.GetLuaString(L, 1);
		bool o = GlobalRegister.CheckNameFormat(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int LoadByteData(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		string arg0 = LuaScriptMgr.GetLuaString(L, 1);
		LuaFunction arg1 = LuaScriptMgr.GetLuaFunction(L, 2);
		GlobalRegister.LoadByteData(arg0,arg1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int EnterCompetition(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 0);
		GlobalRegister.EnterCompetition();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ExitCompetition(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 0);
		GlobalRegister.ExitCompetition();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetTeamList(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 0);
		List<Team.Memember> o = GlobalRegister.GetTeamList();
		LuaScriptMgr.PushObject(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetTeamInfo(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 0);
		Team.MSG_TeamMemeberList_SC o = GlobalRegister.GetTeamInfo();
		LuaScriptMgr.PushObject(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ChangeMainPanelMenu(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		uint arg0 = (uint)LuaScriptMgr.GetNumber(L, 1);
		bool arg1 = LuaScriptMgr.GetBoolean(L, 2);
		GlobalRegister.ChangeMainPanelMenu(arg0,arg1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ShowChatUI(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 0);
		GlobalRegister.ShowChatUI();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetChatUIState(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 0);
		GlobalRegister.SetChatUIState();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetBagUICloseState(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 0);
		GlobalRegister.SetBagUICloseState();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetTaskUICloseState(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 0);
		GlobalRegister.SetTaskUICloseState();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetHeroUICloseState(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 0);
		GlobalRegister.SetHeroUICloseState();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetCharacterUIAttr(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 0);
		GlobalRegister.SetCharacterUIAttr();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetGeneUICloseState(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		string arg0 = LuaScriptMgr.GetLuaString(L, 1);
		GlobalRegister.SetGeneUICloseState(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetInstanceUICloseState(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		string arg0 = LuaScriptMgr.GetLuaString(L, 1);
		GlobalRegister.SetInstanceUICloseState(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetMentoringUICloseState(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		string arg0 = LuaScriptMgr.GetLuaString(L, 1);
		GlobalRegister.SetMentoringUICloseState(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetSysSettingUICloseState(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 0);
		GlobalRegister.SetSysSettingUICloseState();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int OpenShortcutsConfig(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 0);
		GlobalRegister.OpenShortcutsConfig();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int OpenResetSecondPwd(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 0);
		GlobalRegister.OpenResetSecondPwd();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetConstStoneID(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 0);
		uint o = GlobalRegister.GetConstStoneID();
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetConstMoneyID(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 0);
		uint o = GlobalRegister.GetConstMoneyID();
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetConstWelPointID(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 0);
		uint o = GlobalRegister.GetConstWelPointID();
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ShortCutUseEquipController_Reset(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 0);
		GlobalRegister.ShortCutUseEquipController_Reset();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int MainUIController_AddMessage(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 0);
		GlobalRegister.MainUIController_AddMessage();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int MainUIController_ReadMessage(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 0);
		GlobalRegister.MainUIController_ReadMessage();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int InitBagManager(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 0);
		GlobalRegister.InitBagManager();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int OpenInfo(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		PropsBase arg0 = (PropsBase)LuaScriptMgr.GetNetObject(L, 1, typeof(PropsBase));
		bool arg1 = LuaScriptMgr.GetBoolean(L, 2);
		GlobalRegister.OpenInfo(arg0,arg1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ActivateInputField(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		InputField arg0 = (InputField)LuaScriptMgr.GetUnityObject(L, 1, typeof(InputField));
		GlobalRegister.ActivateInputField(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetImageGrey(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		Image arg0 = (Image)LuaScriptMgr.GetUnityObject(L, 1, typeof(Image));
		bool arg1 = LuaScriptMgr.GetBoolean(L, 2);
		GlobalRegister.SetImageGrey(arg0,arg1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int OpenInfoCommon(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		uint arg0 = (uint)LuaScriptMgr.GetNumber(L, 1);
		Transform arg1 = (Transform)LuaScriptMgr.GetUnityObject(L, 2, typeof(Transform));
		GlobalRegister.OpenInfoCommon(arg0,arg1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int OpenInfoRune(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 6);
		Transform arg0 = (Transform)LuaScriptMgr.GetUnityObject(L, 1, typeof(Transform));
		LuaTable arg1 = LuaScriptMgr.GetLuaTable(L, 2);
		string arg2 = LuaScriptMgr.GetLuaString(L, 3);
		int arg3 = (int)LuaScriptMgr.GetNumber(L, 4);
		Action arg4 = null;
		LuaTypes funcType5 = LuaDLL.lua_type(L, 5);

		if (funcType5 != LuaTypes.LUA_TFUNCTION)
		{
			 arg4 = (Action)LuaScriptMgr.GetNetObject(L, 5, typeof(Action));
		}
		else
		{
			LuaFunction func = LuaScriptMgr.GetLuaFunction(L, 5);
			arg4 = () =>
			{
				func.Call();
			};
		}

		Action arg5 = null;
		LuaTypes funcType6 = LuaDLL.lua_type(L, 6);

		if (funcType6 != LuaTypes.LUA_TFUNCTION)
		{
			 arg5 = (Action)LuaScriptMgr.GetNetObject(L, 6, typeof(Action));
		}
		else
		{
			LuaFunction func = LuaScriptMgr.GetLuaFunction(L, 6);
			arg5 = () =>
			{
				func.Call();
			};
		}

		GlobalRegister.OpenInfoRune(arg0,arg1,arg2,arg3,arg4,arg5);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int OpenInfoShortcutUse(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		PropsBase arg0 = (PropsBase)LuaScriptMgr.GetNetObject(L, 1, typeof(PropsBase));
		GlobalRegister.OpenInfoShortcutUse(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int CloseInfo(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 0);
		GlobalRegister.CloseInfo();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int OnPlayerCuccencyUpdateEvent_Add(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 0);
		GlobalRegister.OnPlayerCuccencyUpdateEvent_Add();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int OnPlayerCuccencyUpdateEvent_Delete(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 0);
		GlobalRegister.OnPlayerCuccencyUpdateEvent_Delete();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int AddCurrencyItem(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		uint arg0 = (uint)LuaScriptMgr.GetNumber(L, 1);
		uint arg1 = (uint)LuaScriptMgr.GetNumber(L, 2);
		GlobalRegister.AddCurrencyItem(arg0,arg1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int AddShortcutItem(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		PropsBase arg0 = (PropsBase)LuaScriptMgr.GetNetObject(L, 1, typeof(PropsBase));
		GlobalRegister.AddShortcutItem(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int OnPlayerCuccencyUpdateEvent(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 0);
		GlobalRegister.OnPlayerCuccencyUpdateEvent();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int IntToEnum(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		int arg0 = (int)LuaScriptMgr.GetNumber(L, 1);
		Obj.ObjectType o = GlobalRegister.IntToEnum(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetTidyPackCD(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 0);
		uint o = GlobalRegister.GetTidyPackCD();
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetTidyPackInfo(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 0);
		Obj.t_TidyPackInfo o = GlobalRegister.GetTidyPackInfo();
		LuaScriptMgr.PushObject(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetServerDateTimeByTimeStamp(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		int arg0 = (int)LuaScriptMgr.GetNumber(L, 1);
		string o = GlobalRegister.GetServerDateTimeByTimeStamp(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetServerDateTimeByTimeStampWithAllTime(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		int arg0 = (int)LuaScriptMgr.GetNumber(L, 1);
		string o = GlobalRegister.GetServerDateTimeByTimeStampWithAllTime(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ShowMsgBoxDisengageApprentice(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 3);
		string arg0 = LuaScriptMgr.GetLuaString(L, 1);
		LuaFunction arg1 = LuaScriptMgr.GetLuaFunction(L, 2);
		LuaFunction arg2 = LuaScriptMgr.GetLuaFunction(L, 3);
		GlobalRegister.ShowMsgBoxDisengageApprentice(arg0,arg1,arg2);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetPropsInPackage(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		int arg0 = (int)LuaScriptMgr.GetNumber(L, 1);
		List<PropsBase> o = GlobalRegister.GetPropsInPackage(arg0);
		LuaScriptMgr.PushObject(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetResourceProps(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 0);
		List<PropsBase> o = GlobalRegister.GetResourceProps();
		LuaScriptMgr.PushObject(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetMainPlayerMapUserData(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 0);
		cs_MapUserData o = GlobalRegister.GetMainPlayerMapUserData();
		LuaScriptMgr.PushObject(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ReturnRenderTexture(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 0);
		RenderTextureMgr o = GlobalRegister.ReturnRenderTexture();
		LuaScriptMgr.PushObject(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetRawCharatorMgr(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 0);
		RawCharactorMgr o = GlobalRegister.GetRawCharatorMgr();
		LuaScriptMgr.PushObject(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetNpcTalkRawCharatorMgr(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 0);
		NpctalkRawCharactorMgr o = GlobalRegister.GetNpcTalkRawCharatorMgr();
		LuaScriptMgr.PushObject(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SwitchHeroCb(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		LuaTable arg0 = LuaScriptMgr.GetLuaTable(L, 1);
		GlobalRegister.SwitchHeroCb(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int InitFiveanglerObj(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		GameObject arg0 = (GameObject)LuaScriptMgr.GetUnityObject(L, 1, typeof(GameObject));
		float[] objs1 = LuaScriptMgr.GetArrayNumber<float>(L, 2);
		GlobalRegister.InitFiveanglerObj(arg0,objs1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int DeleteObjMaterialImmediate(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		Material arg0 = (Material)LuaScriptMgr.GetUnityObject(L, 1, typeof(Material));
		GlobalRegister.DeleteObjMaterialImmediate(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int UpdateInputNumPos(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		Transform arg0 = (Transform)LuaScriptMgr.GetUnityObject(L, 1, typeof(Transform));
		Transform arg1 = (Transform)LuaScriptMgr.GetUnityObject(L, 2, typeof(Transform));
		GlobalRegister.UpdateInputNumPos(arg0,arg1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetCurSubCopyMapID(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 0);
		uint o = GlobalRegister.GetCurSubCopyMapID();
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int TryEnterAutoTaskState(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		uint arg0 = (uint)LuaScriptMgr.GetNumber(L, 1);
		uint arg1 = (uint)LuaScriptMgr.GetNumber(L, 2);
		GlobalRegister.TryEnterAutoTaskState(arg0,arg1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int RefreshTaskInfoInMainUI(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 0);
		GlobalRegister.RefreshTaskInfoInMainUI();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int PathFindByIDAndState(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 3);
		uint arg0 = (uint)LuaScriptMgr.GetNumber(L, 1);
		uint arg1 = (uint)LuaScriptMgr.GetNumber(L, 2);
		bool arg2 = LuaScriptMgr.GetBoolean(L, 3);
		GlobalRegister.PathFindByIDAndState(arg0,arg1,arg2);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int RefreshPKMode(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 0);
		GlobalRegister.RefreshPKMode();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ReqExecuteQuest(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 4);
		uint arg0 = (uint)LuaScriptMgr.GetNumber(L, 1);
		string arg1 = LuaScriptMgr.GetLuaString(L, 2);
		uint arg2 = (uint)LuaScriptMgr.GetNumber(L, 3);
		uint arg3 = (uint)LuaScriptMgr.GetNumber(L, 4);
		GlobalRegister.ReqExecuteQuest(arg0,arg1,arg2,arg3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ReqExecuteQuest2ChangeBuf(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 5);
		uint arg0 = (uint)LuaScriptMgr.GetNumber(L, 1);
		string arg1 = LuaScriptMgr.GetLuaString(L, 2);
		uint arg2 = (uint)LuaScriptMgr.GetNumber(L, 3);
		uint arg3 = (uint)LuaScriptMgr.GetNumber(L, 4);
		ulong arg4 = (ulong)LuaScriptMgr.GetNumber(L, 5);
		GlobalRegister.ReqExecuteQuest2ChangeBuf(arg0,arg1,arg2,arg3,arg4);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ChangeTextModel(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		string arg0 = LuaScriptMgr.GetLuaString(L, 1);
		string o = GlobalRegister.ChangeTextModel(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ShowMsgBoxAbandonQuest(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		LuaFunction arg0 = LuaScriptMgr.GetLuaFunction(L, 1);
		LuaFunction arg1 = LuaScriptMgr.GetLuaFunction(L, 2);
		GlobalRegister.ShowMsgBoxAbandonQuest(arg0,arg1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ShowMsgBoxAllUnload(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		LuaFunction arg0 = LuaScriptMgr.GetLuaFunction(L, 1);
		LuaFunction arg1 = LuaScriptMgr.GetLuaFunction(L, 2);
		GlobalRegister.ShowMsgBoxAllUnload(arg0,arg1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ShowMsgBoxActiveRune(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		LuaFunction arg0 = LuaScriptMgr.GetLuaFunction(L, 1);
		LuaFunction arg1 = LuaScriptMgr.GetLuaFunction(L, 2);
		GlobalRegister.ShowMsgBoxActiveRune(arg0,arg1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ShowMsgBoxSelectActivateRune(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		LuaFunction arg0 = LuaScriptMgr.GetLuaFunction(L, 1);
		LuaFunction arg1 = LuaScriptMgr.GetLuaFunction(L, 2);
		GlobalRegister.ShowMsgBoxSelectActivateRune(arg0,arg1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetModelColor(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		uint arg0 = (uint)LuaScriptMgr.GetNumber(L, 1);
		Color o = GlobalRegister.GetModelColor(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int RefreshTaskState(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		uint arg0 = (uint)LuaScriptMgr.GetNumber(L, 1);
		uint arg1 = (uint)LuaScriptMgr.GetNumber(L, 2);
		GlobalRegister.RefreshTaskState(arg0,arg1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ShowAllNPCInMap(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 0);
		GlobalRegister.ShowAllNPCInMap();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetCameraDistance(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		int arg0 = (int)LuaScriptMgr.GetNumber(L, 1);
		GlobalRegister.SetCameraDistance(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetCameraTrack(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		bool arg0 = LuaScriptMgr.GetBoolean(L, 1);
		GlobalRegister.SetCameraTrack(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetCameraSpeed(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		int arg0 = (int)LuaScriptMgr.GetNumber(L, 1);
		GlobalRegister.SetCameraSpeed(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetScreenFloor(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		bool arg0 = LuaScriptMgr.GetBoolean(L, 1);
		GlobalRegister.SetScreenFloor(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetScreenShadow(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		bool arg0 = LuaScriptMgr.GetBoolean(L, 1);
		GlobalRegister.SetScreenShadow(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetScreenDepthOfField(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		bool arg0 = LuaScriptMgr.GetBoolean(L, 1);
		GlobalRegister.SetScreenDepthOfField(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetScreenAntialiasing(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		bool arg0 = LuaScriptMgr.GetBoolean(L, 1);
		GlobalRegister.SetScreenAntialiasing(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetScreenBloomOptimized(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		bool arg0 = LuaScriptMgr.GetBoolean(L, 1);
		GlobalRegister.SetScreenBloomOptimized(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetLoadLowPriorityObject(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		bool arg0 = LuaScriptMgr.GetBoolean(L, 1);
		GlobalRegister.SetLoadLowPriorityObject(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetScreenResolution(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 1)
		{
			int arg0 = (int)LuaScriptMgr.GetNumber(L, 1);
			GlobalRegister.SetScreenResolution(arg0);
			return 0;
		}
		else if (count == 3)
		{
			int arg0 = (int)LuaScriptMgr.GetNumber(L, 1);
			int arg1 = (int)LuaScriptMgr.GetNumber(L, 2);
			bool arg2 = LuaScriptMgr.GetBoolean(L, 3);
			GlobalRegister.SetScreenResolution(arg0,arg1,arg2);
			return 0;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: GlobalRegister.SetScreenResolution");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int LogoutGame(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 0);
		GlobalRegister.LogoutGame();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetServerName(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 0);
		string o = GlobalRegister.GetServerName();
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetTeamIconInfo(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 4);
		string arg0 = LuaScriptMgr.GetLuaString(L, 1);
		float arg1 = (float)LuaScriptMgr.GetNumber(L, 2);
		float arg2 = (float)LuaScriptMgr.GetNumber(L, 3);
		int arg3 = (int)LuaScriptMgr.GetNumber(L, 4);
		GlobalRegister.SetTeamIconInfo(arg0,arg1,arg2,arg3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int RetNewApply(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		int arg0 = (int)LuaScriptMgr.GetNumber(L, 1);
		GlobalRegister.RetNewApply(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int TeamActivityChanged(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		uint arg0 = (uint)LuaScriptMgr.GetNumber(L, 1);
		GlobalRegister.TeamActivityChanged(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int IsMainPlayerTeamLeader(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 0);
		bool o = GlobalRegister.IsMainPlayerTeamLeader();
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int IsMainPlayerHasTeam(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 0);
		bool o = GlobalRegister.IsMainPlayerHasTeam();
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetMyTeamMemCount(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 0);
		uint o = GlobalRegister.GetMyTeamMemCount();
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetMyTeamMemMaxCount(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 0);
		uint o = GlobalRegister.GetMyTeamMemMaxCount();
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int CheckIfTeamFull(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 0);
		bool o = GlobalRegister.CheckIfTeamFull();
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetTeamMemberName(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		string arg0 = LuaScriptMgr.GetLuaString(L, 1);
		string o = GlobalRegister.GetTeamMemberName(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ShowBoxWhenEnterAdventure(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 0);
		bool o = GlobalRegister.ShowBoxWhenEnterAdventure();
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ShowMsgBoxWhenEnterAdventure(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		LuaFunction arg0 = LuaScriptMgr.GetLuaFunction(L, 1);
		LuaFunction arg1 = LuaScriptMgr.GetLuaFunction(L, 2);
		GlobalRegister.ShowMsgBoxWhenEnterAdventure(arg0,arg1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetIsReqNearBy(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		bool arg0 = LuaScriptMgr.GetBoolean(L, 1);
		GlobalRegister.SetIsReqNearBy(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int RefreshOccupyNpcShow(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 0);
		GlobalRegister.RefreshOccupyNpcShow();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetMainPlayerIdStr(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 0);
		string o = GlobalRegister.GetMainPlayerIdStr();
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int CheckCloseAllTeam(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 0);
		GlobalRegister.CheckCloseAllTeam();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int PathFindWithPathWayId(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		uint arg0 = (uint)LuaScriptMgr.GetNumber(L, 1);
		GlobalRegister.PathFindWithPathWayId(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetCopyInfiniteNum(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 0);
		uint o = GlobalRegister.GetCopyInfiniteNum();
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetTeamLeaderID(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 0);
		string o = GlobalRegister.GetTeamLeaderID();
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetOpenChatType(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 0);
		string o = GlobalRegister.GetOpenChatType();
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SaveOpenChatType(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		string arg0 = LuaScriptMgr.GetLuaString(L, 1);
		GlobalRegister.SaveOpenChatType(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetCurOpenChatType(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		string arg0 = LuaScriptMgr.GetLuaString(L, 1);
		GlobalRegister.SetCurOpenChatType(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int AddPrivateChatData(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		LuaTable arg0 = LuaScriptMgr.GetLuaTable(L, 1);
		GlobalRegister.AddPrivateChatData(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int OnReceiveFriendChatData(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		LuaTable arg0 = LuaScriptMgr.GetLuaTable(L, 1);
		GlobalRegister.OnReceiveFriendChatData(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int OnReceiveChatData(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		LuaTable arg0 = LuaScriptMgr.GetLuaTable(L, 1);
		GlobalRegister.OnReceiveChatData(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int RegistGMInputField(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		InputField arg0 = (InputField)LuaScriptMgr.GetUnityObject(L, 1, typeof(InputField));
		GlobalRegister.RegistGMInputField(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int RemoveGMInputField(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 0);
		GlobalRegister.RemoveGMInputField();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int IsInCompleteCopy(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 0);
		bool o = GlobalRegister.IsInCompleteCopy();
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ShowLittleChat(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 0);
		GlobalRegister.ShowLittleChat();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ShowMsgBoxByChat(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		string arg0 = LuaScriptMgr.GetLuaString(L, 1);
		GlobalRegister.ShowMsgBoxByChat(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ShowNotice(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		LuaTable arg0 = LuaScriptMgr.GetLuaTable(L, 1);
		GlobalRegister.ShowNotice(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int RemoveTextModel(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		string arg0 = LuaScriptMgr.GetLuaString(L, 1);
		string o = GlobalRegister.RemoveTextModel(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ShowWindow(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		LuaTable arg0 = LuaScriptMgr.GetLuaTable(L, 1);
		GlobalRegister.ShowWindow(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int AddChatItem(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		LuaTable arg0 = LuaScriptMgr.GetLuaTable(L, 1);
		GlobalRegister.AddChatItem(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int AddChatItem_Tips(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		LuaTable arg0 = LuaScriptMgr.GetLuaTable(L, 1);
		GlobalRegister.AddChatItem_Tips(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetCampNeutralID(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 0);
		uint o = GlobalRegister.GetCampNeutralID();
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetModelColorStr(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		string arg0 = LuaScriptMgr.GetLuaString(L, 1);
		string o = GlobalRegister.GetModelColorStr(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ProcessGMWithSpace(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		string arg0 = LuaScriptMgr.GetLuaString(L, 1);
		string o = GlobalRegister.ProcessGMWithSpace(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int IsLocalGmText(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		string arg0 = LuaScriptMgr.GetLuaString(L, 1);
		bool o = GlobalRegister.IsLocalGmText(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ShowMsgBoxCreateTeam(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		LuaFunction arg0 = LuaScriptMgr.GetLuaFunction(L, 1);
		GlobalRegister.ShowMsgBoxCreateTeam(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int AddTextModel(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		string arg0 = LuaScriptMgr.GetLuaString(L, 1);
		string arg1 = LuaScriptMgr.GetLuaString(L, 2);
		string o = GlobalRegister.AddTextModel(arg0,arg1);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ForceRebuild(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		RectTransform arg0 = (RectTransform)LuaScriptMgr.GetUnityObject(L, 1, typeof(RectTransform));
		GlobalRegister.ForceRebuild(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int NewRichText(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 4);
		LuaTable arg0 = LuaScriptMgr.GetLuaTable(L, 1);
		string arg1 = LuaScriptMgr.GetLuaString(L, 2);
		GameObject arg2 = (GameObject)LuaScriptMgr.GetUnityObject(L, 3, typeof(GameObject));
		float arg3 = (float)LuaScriptMgr.GetNumber(L, 4);
		RichText o = GlobalRegister.NewRichText(arg0,arg1,arg2,arg3);
		LuaScriptMgr.PushObject(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int IsteamMember(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 0);
		bool o = GlobalRegister.IsteamMember();
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int IsActiveAutoButton(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 0);
		bool o = GlobalRegister.IsActiveAutoButton();
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetAutoAttack(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		bool arg0 = LuaScriptMgr.GetBoolean(L, 1);
		GlobalRegister.SetAutoAttack(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetAttactFollowTeamLeader(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		bool arg0 = LuaScriptMgr.GetBoolean(L, 1);
		GlobalRegister.SetAttactFollowTeamLeader(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetUIWorldToScreenPoint(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		Transform arg0 = (Transform)LuaScriptMgr.GetUnityObject(L, 1, typeof(Transform));
		Vector2 o = GlobalRegister.GetUIWorldToScreenPoint(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetUIScreenPointToLocalPointInRectangle(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		Transform arg0 = (Transform)LuaScriptMgr.GetUnityObject(L, 1, typeof(Transform));
		Vector2 arg1 = LuaScriptMgr.GetVector2(L, 2);
		Vector2 o = GlobalRegister.GetUIScreenPointToLocalPointInRectangle(arg0,arg1);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetPlayerHeroID(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		ulong arg0 = (ulong)LuaScriptMgr.GetNumber(L, 1);
		uint o = GlobalRegister.GetPlayerHeroID(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetPlayerTeamID(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		ulong arg0 = (ulong)LuaScriptMgr.GetNumber(L, 1);
		uint o = GlobalRegister.GetPlayerTeamID(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetPlayerGuildID(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		ulong arg0 = (ulong)LuaScriptMgr.GetNumber(L, 1);
		ulong o = GlobalRegister.GetPlayerGuildID(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetMainPlayerGuildID(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 0);
		ulong o = GlobalRegister.GetMainPlayerGuildID();
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetPlayerGuildName(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		ulong arg0 = (ulong)LuaScriptMgr.GetNumber(L, 1);
		string o = GlobalRegister.GetPlayerGuildName(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ProcessLuaCallback(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		string arg0 = LuaScriptMgr.GetLuaString(L, 1);
		GlobalRegister.ProcessLuaCallback(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ResetNPCDir(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 0);
		GlobalRegister.ResetNPCDir();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetNpcTalkName(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 0);
		string o = GlobalRegister.GetNpcTalkName();
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetTalkNpcGameObject(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 0);
		GameObject o = GlobalRegister.GetTalkNpcGameObject();
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ViewMask(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		float arg0 = (float)LuaScriptMgr.GetNumber(L, 1);
		GlobalRegister.ViewMask(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ProcessNpcdlgBreakAutoattack(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		bool arg0 = LuaScriptMgr.GetBoolean(L, 1);
		GlobalRegister.ProcessNpcdlgBreakAutoattack(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int TextureasseetToSprite(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		UITextureAsset arg0 = (UITextureAsset)LuaScriptMgr.GetNetObject(L, 1, typeof(UITextureAsset));
		Sprite o = GlobalRegister.TextureasseetToSprite(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int PlayDramaActionShake(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		Transform arg0 = (Transform)LuaScriptMgr.GetUnityObject(L, 1, typeof(Transform));
		uint arg1 = (uint)LuaScriptMgr.GetNumber(L, 2);
		GlobalRegister.PlayDramaActionShake(arg0,arg1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int CheckIfInTeam(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		string arg0 = LuaScriptMgr.GetLuaString(L, 1);
		bool o = GlobalRegister.CheckIfInTeam(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ShowMsgBoxCommon(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 3);
		string arg0 = LuaScriptMgr.GetLuaString(L, 1);
		LuaFunction arg1 = LuaScriptMgr.GetLuaFunction(L, 2);
		LuaFunction arg2 = LuaScriptMgr.GetLuaFunction(L, 3);
		GlobalRegister.ShowMsgBoxCommon(arg0,arg1,arg2);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetCurrentCopyID(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 0);
		uint o = GlobalRegister.GetCurrentCopyID();
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetClientDirVector2ByServerDir(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		int arg0 = (int)LuaScriptMgr.GetNumber(L, 1);
		Vector2 o = GlobalRegister.GetClientDirVector2ByServerDir(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetServerPosByWorldPos(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		Vector3 arg0 = LuaScriptMgr.GetVector3(L, 1);
		Vector2 o = GlobalRegister.GetServerPosByWorldPos(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetWorldPosByServerPos(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		Vector2 arg0 = LuaScriptMgr.GetVector2(L, 1);
		Vector3 o = GlobalRegister.GetWorldPosByServerPos(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetServerDirByClientDir(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		Vector2 arg0 = LuaScriptMgr.GetVector2(L, 1);
		uint o = GlobalRegister.GetServerDirByClientDir(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetCurrencyByID(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		uint arg0 = (uint)LuaScriptMgr.GetNumber(L, 1);
		uint o = GlobalRegister.GetCurrencyByID(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetBluePointToday(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 0);
		uint o = GlobalRegister.GetBluePointToday();
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetMainCameraTrans(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 0);
		Transform o = GlobalRegister.GetMainCameraTrans();
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetMainCameraAngle(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 0);
		Vector3 o = GlobalRegister.GetMainCameraAngle();
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetMainPlayerTrans(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 0);
		Transform o = GlobalRegister.GetMainPlayerTrans();
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetMapID(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 0);
		int o = GlobalRegister.GetMapID();
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int BeginFindPath(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 3);
		float arg0 = (float)LuaScriptMgr.GetNumber(L, 1);
		float arg1 = (float)LuaScriptMgr.GetNumber(L, 2);
		LuaFunction arg2 = LuaScriptMgr.GetLuaFunction(L, 3);
		GlobalRegister.BeginFindPath(arg0,arg1,arg2);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ReSetAndPlayTweens(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		GameObject arg0 = (GameObject)LuaScriptMgr.GetUnityObject(L, 1, typeof(GameObject));
		GlobalRegister.ReSetAndPlayTweens(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int RefreshCopyTask(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 0);
		GlobalRegister.RefreshCopyTask();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ShowMsgBoxUseVitPill(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 3);
		int arg0 = (int)LuaScriptMgr.GetNumber(L, 1);
		LuaFunction arg1 = LuaScriptMgr.GetLuaFunction(L, 2);
		LuaFunction arg2 = LuaScriptMgr.GetLuaFunction(L, 3);
		GlobalRegister.ShowMsgBoxUseVitPill(arg0,arg1,arg2);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ShowAddNewItem(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		PropsBase arg0 = (PropsBase)LuaScriptMgr.GetNetObject(L, 1, typeof(PropsBase));
		GlobalRegister.ShowAddNewItem(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int RefreshBagItem(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 0);
		GlobalRegister.RefreshBagItem();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int RefreshTreasureHuntInfo(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		PropsBase arg0 = (PropsBase)LuaScriptMgr.GetNetObject(L, 1, typeof(PropsBase));
		GlobalRegister.RefreshTreasureHuntInfo(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int CloseTreasureHunt(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		string arg0 = LuaScriptMgr.GetLuaString(L, 1);
		GlobalRegister.CloseTreasureHunt(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetMapNameAction(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 3);
		string arg0 = LuaScriptMgr.GetLuaString(L, 1);
		string arg1 = LuaScriptMgr.GetLuaString(L, 2);
		string arg2 = LuaScriptMgr.GetLuaString(L, 3);
		GlobalRegister.SetMapNameAction(arg0,arg1,arg2);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetStoryNameAction(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		string arg0 = LuaScriptMgr.GetLuaString(L, 1);
		GlobalRegister.SetStoryNameAction(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetImage(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 4);
		int arg0 = (int)LuaScriptMgr.GetNumber(L, 1);
		string arg1 = LuaScriptMgr.GetLuaString(L, 2);
		Image arg2 = (Image)LuaScriptMgr.GetUnityObject(L, 3, typeof(Image));
		bool arg3 = LuaScriptMgr.GetBoolean(L, 4);
		GlobalRegister.SetImage(arg0,arg1,arg2,arg3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetSpriteFromAtlas(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 2)
		{
			string arg0 = LuaScriptMgr.GetLuaString(L, 1);
			Image arg1 = (Image)LuaScriptMgr.GetUnityObject(L, 2, typeof(Image));
			GlobalRegister.GetSpriteFromAtlas(arg0,arg1);
			return 0;
		}
		else if (count == 3)
		{
			int arg0 = (int)LuaScriptMgr.GetNumber(L, 1);
			string arg1 = LuaScriptMgr.GetLuaString(L, 2);
			Image arg2 = (Image)LuaScriptMgr.GetUnityObject(L, 3, typeof(Image));
			GlobalRegister.GetSpriteFromAtlas(arg0,arg1,arg2);
			return 0;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: GlobalRegister.GetSpriteFromAtlas");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetCanvasRenderer(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		TweenAlpha arg0 = (TweenAlpha)LuaScriptMgr.GetUnityObject(L, 1, typeof(TweenAlpha));
		GlobalRegister.GetCanvasRenderer(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int OnTaskStateChange(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 0);
		GlobalRegister.OnTaskStateChange();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetImageBlur(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		Image arg0 = (Image)LuaScriptMgr.GetUnityObject(L, 1, typeof(Image));
		GlobalRegister.SetImageBlur(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetRawImageBlur(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		RawImage arg0 = (RawImage)LuaScriptMgr.GetUnityObject(L, 1, typeof(RawImage));
		GlobalRegister.SetRawImageBlur(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ActiveAllPublicCD(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 0);
		GlobalRegister.ActiveAllPublicCD();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetCurrentBattleHeroThisid(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 0);
		string o = GlobalRegister.GetCurrentBattleHeroThisid();
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int IsPointerEnterUI(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		Vector2 arg0 = LuaScriptMgr.GetVector2(L, 1);
		Transform arg1 = (Transform)LuaScriptMgr.GetUnityObject(L, 2, typeof(Transform));
		bool o = GlobalRegister.IsPointerEnterUI(arg0,arg1);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetUIInfoPos(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		Transform arg0 = (Transform)LuaScriptMgr.GetUnityObject(L, 1, typeof(Transform));
		Transform arg1 = (Transform)LuaScriptMgr.GetUnityObject(L, 2, typeof(Transform));
		GlobalRegister.SetUIInfoPos(arg0,arg1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ShowMsgBoxHeroUpgradeBind(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 4);
		uint arg0 = (uint)LuaScriptMgr.GetNumber(L, 1);
		uint arg1 = (uint)LuaScriptMgr.GetNumber(L, 2);
		LuaFunction arg2 = LuaScriptMgr.GetLuaFunction(L, 3);
		LuaFunction arg3 = LuaScriptMgr.GetLuaFunction(L, 4);
		GlobalRegister.ShowMsgBoxHeroUpgradeBind(arg0,arg1,arg2,arg3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetHeroSimpleAttrPos(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		Transform arg0 = (Transform)LuaScriptMgr.GetUnityObject(L, 1, typeof(Transform));
		Transform arg1 = (Transform)LuaScriptMgr.GetUnityObject(L, 2, typeof(Transform));
		GlobalRegister.SetHeroSimpleAttrPos(arg0,arg1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int RefrashShortcutItemCount(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 0);
		GlobalRegister.RefrashShortcutItemCount();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int LogMessage(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 1)
		{
			string arg0 = LuaScriptMgr.GetLuaString(L, 1);
			GlobalRegister.LogMessage(arg0);
			return 0;
		}
		else if (count == 2)
		{
			string arg0 = LuaScriptMgr.GetLuaString(L, 1);
			int arg1 = (int)LuaScriptMgr.GetNumber(L, 2);
			GlobalRegister.LogMessage(arg0,arg1);
			return 0;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: GlobalRegister.LogMessage");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetShortcutKeyEnableState(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		bool arg0 = LuaScriptMgr.GetBoolean(L, 1);
		GlobalRegister.SetShortcutKeyEnableState(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ConfigColorToRichTextFormat(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		string arg0 = LuaScriptMgr.GetLuaString(L, 1);
		string o = GlobalRegister.ConfigColorToRichTextFormat(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int StripColorText(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		string arg0 = LuaScriptMgr.GetLuaString(L, 1);
		string o = GlobalRegister.StripColorText(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetWayFindItemByQuestID(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		LuaTable arg0 = LuaScriptMgr.GetLuaTable(L, 1);
		string arg1 = LuaScriptMgr.GetLuaString(L, 2);
		WayFindItem o = GlobalRegister.GetWayFindItemByQuestID(arg0,arg1);
		LuaScriptMgr.PushObject(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetTaskGoalStr(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		uint arg0 = (uint)LuaScriptMgr.GetNumber(L, 1);
		List<string> o = GlobalRegister.GetTaskGoalStr(arg0);
		LuaScriptMgr.PushObject(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetWayFindItemByStr(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		string arg0 = LuaScriptMgr.GetLuaString(L, 1);
		WayFindItem o = GlobalRegister.GetWayFindItemByStr(arg0);
		LuaScriptMgr.PushObject(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int VisitLastVisitNpc(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 0);
		GlobalRegister.VisitLastVisitNpc();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int OnCurActiveQuestFrash(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		LuaTable arg0 = LuaScriptMgr.GetLuaTable(L, 1);
		GlobalRegister.OnCurActiveQuestFrash(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetLastVisitNpcTmpId(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 0);
		ulong o = GlobalRegister.GetLastVisitNpcTmpId();
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetLastVisitNpcId(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 0);
		ulong o = GlobalRegister.GetLastVisitNpcId();
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Clear3DIconData(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 0);
		GlobalRegister.Clear3DIconData();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ShowMainPlayerRTT(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		RawImage arg0 = (RawImage)LuaScriptMgr.GetUnityObject(L, 1, typeof(RawImage));
		uint arg1 = (uint)LuaScriptMgr.GetNumber(L, 2);
		GlobalRegister.ShowMainPlayerRTT(arg0,arg1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ShowNpcOrPlayerRTT(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 4);
		RawImage arg0 = (RawImage)LuaScriptMgr.GetUnityObject(L, 1, typeof(RawImage));
		uint arg1 = (uint)LuaScriptMgr.GetNumber(L, 2);
		int arg2 = (int)LuaScriptMgr.GetNumber(L, 3);
		Action<GameObject> arg3 = null;
		LuaTypes funcType4 = LuaDLL.lua_type(L, 4);

		if (funcType4 != LuaTypes.LUA_TFUNCTION)
		{
			 arg3 = (Action<GameObject>)LuaScriptMgr.GetNetObject(L, 4, typeof(Action<GameObject>));
		}
		else
		{
			LuaFunction func = LuaScriptMgr.GetLuaFunction(L, 4);
			arg3 = (param0) =>
			{
				int top = func.BeginPCall();
				LuaScriptMgr.Push(L, param0);
				func.PCall(top, 1);
				func.EndPCall(top);
			};
		}

		GlobalRegister.ShowNpcOrPlayerRTT(arg0,arg1,arg2,arg3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int OpenMailUI(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 0);
		GlobalRegister.OpenMailUI();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int UpdateBubbleOfMail(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		bool arg0 = LuaScriptMgr.GetBoolean(L, 1);
		GlobalRegister.UpdateBubbleOfMail(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ShowPlayerRTT(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 6);
		RawImage arg0 = (RawImage)LuaScriptMgr.GetUnityObject(L, 1, typeof(RawImage));
		uint arg1 = (uint)LuaScriptMgr.GetNumber(L, 2);
		uint arg2 = (uint)LuaScriptMgr.GetNumber(L, 3);
		uint[] objs3 = LuaScriptMgr.GetArrayNumber<uint>(L, 4);
		int arg4 = (int)LuaScriptMgr.GetNumber(L, 5);
		Action<GameObject> arg5 = null;
		LuaTypes funcType6 = LuaDLL.lua_type(L, 6);

		if (funcType6 != LuaTypes.LUA_TFUNCTION)
		{
			 arg5 = (Action<GameObject>)LuaScriptMgr.GetNetObject(L, 6, typeof(Action<GameObject>));
		}
		else
		{
			LuaFunction func = LuaScriptMgr.GetLuaFunction(L, 6);
			arg5 = (param0) =>
			{
				int top = func.BeginPCall();
				LuaScriptMgr.Push(L, param0);
				func.PCall(top, 1);
				func.EndPCall(top);
			};
		}

		GlobalRegister.ShowPlayerRTT(arg0,arg1,arg2,objs3,arg4,arg5);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetRTTInfo(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 6);
		PlayerCharactorCreateHelper arg0 = (PlayerCharactorCreateHelper)LuaScriptMgr.GetNetObject(L, 1, typeof(PlayerCharactorCreateHelper));
		RawImage arg1 = (RawImage)LuaScriptMgr.GetUnityObject(L, 2, typeof(RawImage));
		GameObject arg2 = (GameObject)LuaScriptMgr.GetUnityObject(L, 3, typeof(GameObject));
		Vector3 arg3 = LuaScriptMgr.GetVector3(L, 4);
		Vector3 arg4 = LuaScriptMgr.GetVector3(L, 5);
		bool arg5 = LuaScriptMgr.GetBoolean(L, 6);
		GlobalRegister.SetRTTInfo(arg0,arg1,arg2,arg3,arg4,arg5);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int RemoveEffectLayerObject(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		GameObject arg0 = (GameObject)LuaScriptMgr.GetUnityObject(L, 1, typeof(GameObject));
		GlobalRegister.RemoveEffectLayerObject(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int OpenGeneUI(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		int arg0 = (int)LuaScriptMgr.GetNumber(L, 1);
		GlobalRegister.OpenGeneUI(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int OpenUnLockSkillsUI(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 0);
		GlobalRegister.OpenUnLockSkillsUI();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int OnRetLevelupHeroSkill(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 4);
		string arg0 = LuaScriptMgr.GetLuaString(L, 1);
		uint arg1 = (uint)LuaScriptMgr.GetNumber(L, 2);
		uint arg2 = (uint)LuaScriptMgr.GetNumber(L, 3);
		uint arg3 = (uint)LuaScriptMgr.GetNumber(L, 4);
		GlobalRegister.OnRetLevelupHeroSkill(arg0,arg1,arg2,arg3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int DelayCloseUI(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		float arg0 = (float)LuaScriptMgr.GetNumber(L, 1);
		string arg1 = LuaScriptMgr.GetLuaString(L, 2);
		GlobalRegister.DelayCloseUI(arg0,arg1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ItemSplitNumAddBtnDownUpListen(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		GameObject arg0 = (GameObject)LuaScriptMgr.GetUnityObject(L, 1, typeof(GameObject));
		GlobalRegister.ItemSplitNumAddBtnDownUpListen(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ItemSplitNumMinusBtnDownUpListen(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		GameObject arg0 = (GameObject)LuaScriptMgr.GetUnityObject(L, 1, typeof(GameObject));
		GlobalRegister.ItemSplitNumMinusBtnDownUpListen(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetMainPlayerCharID(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 0);
		string o = GlobalRegister.GetMainPlayerCharID();
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetCurrentPriChatFileName(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 0);
		string o = GlobalRegister.GetCurrentPriChatFileName();
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SaveChatDataAsFile(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		LuaStringBuffer arg0 = LuaScriptMgr.GetStringBuffer(L, 1);
		GlobalRegister.SaveChatDataAsFile(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ProcessChatTime(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		Text arg0 = (Text)LuaScriptMgr.GetUnityObject(L, 1, typeof(Text));
		int arg1 = (int)LuaScriptMgr.GetNumber(L, 2);
		GlobalRegister.ProcessChatTime(arg0,arg1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ProcessMailRemainTime(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		string arg0 = LuaScriptMgr.GetLuaString(L, 1);
		Text arg1 = (Text)LuaScriptMgr.GetUnityObject(L, 2, typeof(Text));
		GlobalRegister.ProcessMailRemainTime(arg0,arg1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetPropsBaseByID(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		uint arg0 = (uint)LuaScriptMgr.GetNumber(L, 1);
		PropsBase o = GlobalRegister.GetPropsBaseByID(arg0);
		LuaScriptMgr.PushObject(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int CompareLong(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		string arg0 = LuaScriptMgr.GetLuaString(L, 1);
		string arg1 = LuaScriptMgr.GetLuaString(L, 2);
		int o = GlobalRegister.CompareLong(arg0,arg1);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetSelectTargetID(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 0);
		ulong o = GlobalRegister.GetSelectTargetID();
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int IsBitTrue(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		uint arg0 = (uint)LuaScriptMgr.GetNumber(L, 1);
		int arg1 = (int)LuaScriptMgr.GetNumber(L, 2);
		bool o = GlobalRegister.IsBitTrue(arg0,arg1);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int MainUserDeathOrRelive(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		bool arg0 = LuaScriptMgr.GetBoolean(L, 1);
		GlobalRegister.MainUserDeathOrRelive(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int InitTaskTrackConcurrentItem(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 6);
		GameObject arg0 = (GameObject)LuaScriptMgr.GetUnityObject(L, 1, typeof(GameObject));
		uint arg1 = (uint)LuaScriptMgr.GetNumber(L, 2);
		int arg2 = (int)LuaScriptMgr.GetNumber(L, 3);
		string arg3 = LuaScriptMgr.GetLuaString(L, 4);
		uint arg4 = (uint)LuaScriptMgr.GetNumber(L, 5);
		uint arg5 = (uint)LuaScriptMgr.GetNumber(L, 6);
		GlobalRegister.InitTaskTrackConcurrentItem(arg0,arg1,arg2,arg3,arg4,arg5);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ClearSelectNpcState(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 0);
		GlobalRegister.ClearSelectNpcState();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int PlayProgressBarAndAnimationInLocal(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		string arg0 = LuaScriptMgr.GetLuaString(L, 1);
		GlobalRegister.PlayProgressBarAndAnimationInLocal(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int OpenShopUI(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		uint arg0 = (uint)LuaScriptMgr.GetNumber(L, 1);
		GlobalRegister.OpenShopUI(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int OpenExchange(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		uint arg0 = (uint)LuaScriptMgr.GetNumber(L, 1);
		GlobalRegister.OpenExchange(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int UIPostEvent(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		string arg0 = LuaScriptMgr.GetLuaString(L, 1);
		GlobalRegister.UIPostEvent(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int AddUISoundListener(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		GameObject arg0 = (GameObject)LuaScriptMgr.GetUnityObject(L, 1, typeof(GameObject));
		string arg1 = LuaScriptMgr.GetLuaString(L, 2);
		GlobalRegister.AddUISoundListener(arg0,arg1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int AddItemTip(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 3);
		GameObject arg0 = (GameObject)LuaScriptMgr.GetUnityObject(L, 1, typeof(GameObject));
		GameObject arg1 = (GameObject)LuaScriptMgr.GetUnityObject(L, 2, typeof(GameObject));
		int arg2 = (int)LuaScriptMgr.GetNumber(L, 3);
		GlobalRegister.AddItemTip(arg0,arg1,arg2);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int CheckTalkCloseGuideNext(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 0);
		GlobalRegister.CheckTalkCloseGuideNext();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int RefreshShortcutCD(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		string arg0 = LuaScriptMgr.GetLuaString(L, 1);
		GlobalRegister.RefreshShortcutCD(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int RefreshBagNewTip(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		bool arg0 = LuaScriptMgr.GetBoolean(L, 1);
		GlobalRegister.RefreshBagNewTip(arg0);
		return 0;
	}
}

