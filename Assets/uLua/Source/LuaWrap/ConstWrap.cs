using System;
using UnityEngine.EventSystems;
using UnityEngine;
using LuaInterface;

public class ConstWrap
{
	public static void Register(IntPtr L)
	{
		LuaMethod[] regs = new LuaMethod[]
		{
			new LuaMethod("GetColorByName", GetColorByName),
			new LuaMethod("ColorByRelation", ColorByRelation),
			new LuaMethod("GetMainPackageSize", GetMainPackageSize),
			new LuaMethod("GetQusetPackageSize", GetQusetPackageSize),
			new LuaMethod("GetOnceExtendSize", GetOnceExtendSize),
			new LuaMethod("GetMaxExtendTimes", GetMaxExtendTimes),
			new LuaMethod("GetBlankTex", GetBlankTex),
			new LuaMethod("GetBlankSprite", GetBlankSprite),
			new LuaMethod("GetHitPercentage", GetHitPercentage),
			new LuaMethod("GetWeeklyOpenLevel", GetWeeklyOpenLevel),
			new LuaMethod("New", _CreateConst),
			new LuaMethod("GetClassType", GetClassType),
		};

		LuaField[] fields = new LuaField[]
		{
			new LuaField("NineScreenX", get_NineScreenX, null),
			new LuaField("NineScreenY", get_NineScreenY, null),
			new LuaField("NoCareerLimit", get_NoCareerLimit, null),
			new LuaField("CopyInfiniteNum", get_CopyInfiniteNum, null),
			new LuaField("TempBuildingNpcIDLimite", get_TempBuildingNpcIDLimite, null),
			new LuaField("STR_DEFAULTPWD", get_STR_DEFAULTPWD, null),
			new LuaField("CopyBaseNum", get_CopyBaseNum, null),
			new LuaField("StrMoviePath", get_StrMoviePath, null),
			new LuaField("FMovieDuration", get_FMovieDuration, null),
			new LuaField("StoneID", get_StoneID, null),
			new LuaField("MoneyID", get_MoneyID, null),
			new LuaField("WelPointID", get_WelPointID, null),
			new LuaField("GuildPointID", get_GuildPointID, null),
			new LuaField("BluePointID", get_BluePointID, null),
			new LuaField("PurplePointID", get_PurplePointID, null),
			new LuaField("EduPointID", get_EduPointID, null),
			new LuaField("HelpPointID", get_HelpPointID, null),
			new LuaField("ActivePointID", get_ActivePointID, null),
			new LuaField("DoublePointID", get_DoublePointID, null),
			new LuaField("MaxSaveChatCount", get_MaxSaveChatCount, null),
			new LuaField("StrPrivateChatPath", get_StrPrivateChatPath, null),
			new LuaField("TidyPackCD", get_TidyPackCD, null),
			new LuaField("StrNPCVisitIcon", get_StrNPCVisitIcon, null),
			new LuaField("StrNPCAcceptTaskIcon", get_StrNPCAcceptTaskIcon, null),
			new LuaField("StrNPCTaskDoingIcon", get_StrNPCTaskDoingIcon, null),
			new LuaField("StrNPCTaskFinishIcon", get_StrNPCTaskFinishIcon, null),
			new LuaField("ShowMapFlag", get_ShowMapFlag, null),
			new LuaField("NotShowMapFlag", get_NotShowMapFlag, null),
			new LuaField("PathFinderSafetyRange", get_PathFinderSafetyRange, null),
			new LuaField("PathFinderTaskNpcOffset", get_PathFinderTaskNpcOffset, null),
			new LuaField("PathFinderTaskNpcMinOffset", get_PathFinderTaskNpcMinOffset, null),
			new LuaField("PathFinderTaskNpcMaxOffsetAngle", get_PathFinderTaskNpcMaxOffsetAngle, null),
			new LuaField("DramaTalkInterval", get_DramaTalkInterval, null),
			new LuaField("DramaTipsInterval", get_DramaTipsInterval, null),
			new LuaField("ComicInterval", get_ComicInterval, null),
			new LuaField("VitalityRefreshTime", get_VitalityRefreshTime, null),
			new LuaField("OpenChatType", get_OpenChatType, null),
			new LuaField("SelectTargetMaxDistance", get_SelectTargetMaxDistance, null),
			new LuaField("MsConvertSecondUnit", get_MsConvertSecondUnit, null),
			new LuaField("MapConversionUnit", get_MapConversionUnit, null),
			new LuaField("DebugMode", get_DebugMode, set_DebugMode),
			new LuaField("EnableDebugLog", get_EnableDebugLog, set_EnableDebugLog),
			new LuaField("ShowDebugLogTime", get_ShowDebugLogTime, set_ShowDebugLogTime),
			new LuaField("DestoryEffect", get_DestoryEffect, set_DestoryEffect),
			new LuaField("ExportLog", get_ExportLog, set_ExportLog),
			new LuaField("DebugOutput", get_DebugOutput, set_DebugOutput),
			new LuaField("ConstPanel", get_ConstPanel, set_ConstPanel),
			new LuaField("QueuePanel", get_QueuePanel, set_QueuePanel),
			new LuaField("UILayer", get_UILayer, set_UILayer),
			new LuaField("DefaultLayer", get_DefaultLayer, set_DefaultLayer),
			new LuaField("AppName", get_AppName, set_AppName),
			new LuaField("ExtName", get_ExtName, set_ExtName),
			new LuaField("AssetDir", get_AssetDir, set_AssetDir),
			new LuaField("charactorOriShader", get_charactorOriShader, set_charactorOriShader),
			new LuaField("createAssetBundleType", get_createAssetBundleType, set_createAssetBundleType),
			new LuaField("MaxLevel", get_MaxLevel, set_MaxLevel),
			new LuaField("Camp1ID", get_Camp1ID, set_Camp1ID),
			new LuaField("Camp2ID", get_Camp2ID, set_Camp2ID),
			new LuaField("CampNeutralID", get_CampNeutralID, set_CampNeutralID),
			new LuaField("TextColorNormal", get_TextColorNormal, null),
			new LuaField("TextColorNormalInUI", get_TextColorNormalInUI, null),
			new LuaField("TextColorFriend", get_TextColorFriend, null),
			new LuaField("TextColorEnemy", get_TextColorEnemy, null),
			new LuaField("TextColorTeammate", get_TextColorTeammate, null),
			new LuaField("TextColorTipsGreen", get_TextColorTipsGreen, null),
			new LuaField("TextColorTipsRed", get_TextColorTipsRed, null),
			new LuaField("SkillLocked", get_SkillLocked, null),
			new LuaField("DistNpcVisitResponse", get_DistNpcVisitResponse, null),
			new LuaField("DistMovingNpcVisitResponse", get_DistMovingNpcVisitResponse, null),
		};

		LuaScriptMgr.RegisterLib(L, "Const", typeof(Const), regs, fields, typeof(ConstClient));
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateConst(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 0)
		{
			Const obj = new Const();
			LuaScriptMgr.PushObject(L, obj);
			return 1;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: Const.New");
		}

		return 0;
	}

	static Type classType = typeof(Const);

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetClassType(IntPtr L)
	{
		LuaScriptMgr.Push(L, classType);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_NineScreenX(IntPtr L)
	{
		LuaScriptMgr.Push(L, Const.NineScreenX);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_NineScreenY(IntPtr L)
	{
		LuaScriptMgr.Push(L, Const.NineScreenY);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_NoCareerLimit(IntPtr L)
	{
		LuaScriptMgr.Push(L, Const.NoCareerLimit);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_CopyInfiniteNum(IntPtr L)
	{
		LuaScriptMgr.Push(L, Const.CopyInfiniteNum);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_TempBuildingNpcIDLimite(IntPtr L)
	{
		LuaScriptMgr.Push(L, Const.TempBuildingNpcIDLimite);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_STR_DEFAULTPWD(IntPtr L)
	{
		LuaScriptMgr.Push(L, Const.STR_DEFAULTPWD);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_CopyBaseNum(IntPtr L)
	{
		LuaScriptMgr.Push(L, Const.CopyBaseNum);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_StrMoviePath(IntPtr L)
	{
		LuaScriptMgr.Push(L, Const.StrMoviePath);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_FMovieDuration(IntPtr L)
	{
		LuaScriptMgr.Push(L, Const.FMovieDuration);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_StoneID(IntPtr L)
	{
		LuaScriptMgr.Push(L, Const.StoneID);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_MoneyID(IntPtr L)
	{
		LuaScriptMgr.Push(L, Const.MoneyID);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_WelPointID(IntPtr L)
	{
		LuaScriptMgr.Push(L, Const.WelPointID);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_GuildPointID(IntPtr L)
	{
		LuaScriptMgr.Push(L, Const.GuildPointID);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_BluePointID(IntPtr L)
	{
		LuaScriptMgr.Push(L, Const.BluePointID);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_PurplePointID(IntPtr L)
	{
		LuaScriptMgr.Push(L, Const.PurplePointID);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_EduPointID(IntPtr L)
	{
		LuaScriptMgr.Push(L, Const.EduPointID);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_HelpPointID(IntPtr L)
	{
		LuaScriptMgr.Push(L, Const.HelpPointID);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_ActivePointID(IntPtr L)
	{
		LuaScriptMgr.Push(L, Const.ActivePointID);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_DoublePointID(IntPtr L)
	{
		LuaScriptMgr.Push(L, Const.DoublePointID);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_MaxSaveChatCount(IntPtr L)
	{
		LuaScriptMgr.Push(L, Const.MaxSaveChatCount);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_StrPrivateChatPath(IntPtr L)
	{
		LuaScriptMgr.Push(L, Const.StrPrivateChatPath);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_TidyPackCD(IntPtr L)
	{
		LuaScriptMgr.Push(L, Const.TidyPackCD);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_StrNPCVisitIcon(IntPtr L)
	{
		LuaScriptMgr.Push(L, Const.StrNPCVisitIcon);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_StrNPCAcceptTaskIcon(IntPtr L)
	{
		LuaScriptMgr.Push(L, Const.StrNPCAcceptTaskIcon);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_StrNPCTaskDoingIcon(IntPtr L)
	{
		LuaScriptMgr.Push(L, Const.StrNPCTaskDoingIcon);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_StrNPCTaskFinishIcon(IntPtr L)
	{
		LuaScriptMgr.Push(L, Const.StrNPCTaskFinishIcon);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_ShowMapFlag(IntPtr L)
	{
		LuaScriptMgr.Push(L, Const.ShowMapFlag);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_NotShowMapFlag(IntPtr L)
	{
		LuaScriptMgr.Push(L, Const.NotShowMapFlag);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_PathFinderSafetyRange(IntPtr L)
	{
		LuaScriptMgr.Push(L, Const.PathFinderSafetyRange);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_PathFinderTaskNpcOffset(IntPtr L)
	{
		LuaScriptMgr.Push(L, Const.PathFinderTaskNpcOffset);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_PathFinderTaskNpcMinOffset(IntPtr L)
	{
		LuaScriptMgr.Push(L, Const.PathFinderTaskNpcMinOffset);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_PathFinderTaskNpcMaxOffsetAngle(IntPtr L)
	{
		LuaScriptMgr.Push(L, Const.PathFinderTaskNpcMaxOffsetAngle);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_DramaTalkInterval(IntPtr L)
	{
		LuaScriptMgr.Push(L, Const.DramaTalkInterval);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_DramaTipsInterval(IntPtr L)
	{
		LuaScriptMgr.Push(L, Const.DramaTipsInterval);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_ComicInterval(IntPtr L)
	{
		LuaScriptMgr.Push(L, Const.ComicInterval);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_VitalityRefreshTime(IntPtr L)
	{
		LuaScriptMgr.Push(L, Const.VitalityRefreshTime);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_OpenChatType(IntPtr L)
	{
		LuaScriptMgr.Push(L, Const.OpenChatType);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_SelectTargetMaxDistance(IntPtr L)
	{
		LuaScriptMgr.Push(L, Const.SelectTargetMaxDistance);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_MsConvertSecondUnit(IntPtr L)
	{
		LuaScriptMgr.Push(L, Const.MsConvertSecondUnit);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_MapConversionUnit(IntPtr L)
	{
		LuaScriptMgr.Push(L, Const.MapConversionUnit);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_DebugMode(IntPtr L)
	{
		LuaScriptMgr.Push(L, Const.DebugMode);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_EnableDebugLog(IntPtr L)
	{
		LuaScriptMgr.Push(L, Const.EnableDebugLog);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_ShowDebugLogTime(IntPtr L)
	{
		LuaScriptMgr.Push(L, Const.ShowDebugLogTime);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_DestoryEffect(IntPtr L)
	{
		LuaScriptMgr.Push(L, Const.DestoryEffect);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_ExportLog(IntPtr L)
	{
		LuaScriptMgr.Push(L, Const.ExportLog);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_DebugOutput(IntPtr L)
	{
		LuaScriptMgr.Push(L, Const.DebugOutput);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_ConstPanel(IntPtr L)
	{
		LuaScriptMgr.Push(L, Const.ConstPanel);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_QueuePanel(IntPtr L)
	{
		LuaScriptMgr.Push(L, Const.QueuePanel);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_UILayer(IntPtr L)
	{
		LuaScriptMgr.Push(L, Const.UILayer);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_DefaultLayer(IntPtr L)
	{
		LuaScriptMgr.Push(L, Const.DefaultLayer);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_AppName(IntPtr L)
	{
		LuaScriptMgr.Push(L, Const.AppName);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_ExtName(IntPtr L)
	{
		LuaScriptMgr.Push(L, Const.ExtName);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_AssetDir(IntPtr L)
	{
		LuaScriptMgr.Push(L, Const.AssetDir);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_charactorOriShader(IntPtr L)
	{
		LuaScriptMgr.Push(L, Const.charactorOriShader);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_createAssetBundleType(IntPtr L)
	{
		LuaScriptMgr.Push(L, Const.createAssetBundleType);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_MaxLevel(IntPtr L)
	{
		LuaScriptMgr.Push(L, Const.MaxLevel);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_Camp1ID(IntPtr L)
	{
		LuaScriptMgr.Push(L, Const.Camp1ID);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_Camp2ID(IntPtr L)
	{
		LuaScriptMgr.Push(L, Const.Camp2ID);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_CampNeutralID(IntPtr L)
	{
		LuaScriptMgr.Push(L, Const.CampNeutralID);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_TextColorNormal(IntPtr L)
	{
		LuaScriptMgr.Push(L, Const.TextColorNormal);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_TextColorNormalInUI(IntPtr L)
	{
		LuaScriptMgr.Push(L, Const.TextColorNormalInUI);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_TextColorFriend(IntPtr L)
	{
		LuaScriptMgr.Push(L, Const.TextColorFriend);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_TextColorEnemy(IntPtr L)
	{
		LuaScriptMgr.Push(L, Const.TextColorEnemy);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_TextColorTeammate(IntPtr L)
	{
		LuaScriptMgr.Push(L, Const.TextColorTeammate);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_TextColorTipsGreen(IntPtr L)
	{
		LuaScriptMgr.Push(L, Const.TextColorTipsGreen);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_TextColorTipsRed(IntPtr L)
	{
		LuaScriptMgr.Push(L, Const.TextColorTipsRed);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_SkillLocked(IntPtr L)
	{
		LuaScriptMgr.Push(L, Const.SkillLocked);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_DistNpcVisitResponse(IntPtr L)
	{
		LuaScriptMgr.Push(L, Const.DistNpcVisitResponse);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_DistMovingNpcVisitResponse(IntPtr L)
	{
		LuaScriptMgr.Push(L, Const.DistMovingNpcVisitResponse);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_DebugMode(IntPtr L)
	{
		Const.DebugMode = LuaScriptMgr.GetBoolean(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_EnableDebugLog(IntPtr L)
	{
		Const.EnableDebugLog = LuaScriptMgr.GetBoolean(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_ShowDebugLogTime(IntPtr L)
	{
		Const.ShowDebugLogTime = LuaScriptMgr.GetBoolean(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_DestoryEffect(IntPtr L)
	{
		Const.DestoryEffect = LuaScriptMgr.GetBoolean(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_ExportLog(IntPtr L)
	{
		Const.ExportLog = LuaScriptMgr.GetBoolean(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_DebugOutput(IntPtr L)
	{
		Const.DebugOutput = LuaScriptMgr.GetBoolean(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_ConstPanel(IntPtr L)
	{
		Const.ConstPanel = (int)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_QueuePanel(IntPtr L)
	{
		Const.QueuePanel = (int)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_UILayer(IntPtr L)
	{
		Const.UILayer = LuaScriptMgr.GetString(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_DefaultLayer(IntPtr L)
	{
		Const.DefaultLayer = LuaScriptMgr.GetString(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_AppName(IntPtr L)
	{
		Const.AppName = LuaScriptMgr.GetString(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_ExtName(IntPtr L)
	{
		Const.ExtName = LuaScriptMgr.GetString(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_AssetDir(IntPtr L)
	{
		Const.AssetDir = LuaScriptMgr.GetString(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_charactorOriShader(IntPtr L)
	{
		Const.charactorOriShader = (Shader)LuaScriptMgr.GetUnityObject(L, 3, typeof(Shader));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_createAssetBundleType(IntPtr L)
	{
		Const.createAssetBundleType = (CreateAssetBundleType)LuaScriptMgr.GetNetObject(L, 3, typeof(CreateAssetBundleType));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_MaxLevel(IntPtr L)
	{
		Const.MaxLevel = (uint)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_Camp1ID(IntPtr L)
	{
		Const.Camp1ID = (uint)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_Camp2ID(IntPtr L)
	{
		Const.Camp2ID = (uint)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_CampNeutralID(IntPtr L)
	{
		Const.CampNeutralID = (uint)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetColorByName(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		string arg0 = LuaScriptMgr.GetLuaString(L, 1);
		Color o = Const.GetColorByName(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ColorByRelation(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		RelationType arg0 = (RelationType)LuaScriptMgr.GetNetObject(L, 1, typeof(RelationType));
		bool arg1 = LuaScriptMgr.GetBoolean(L, 2);
		Color o = Const.ColorByRelation(arg0,arg1);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetMainPackageSize(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 0);
		uint o = Const.GetMainPackageSize();
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetQusetPackageSize(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 0);
		uint o = Const.GetQusetPackageSize();
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetOnceExtendSize(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 0);
		uint o = Const.GetOnceExtendSize();
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetMaxExtendTimes(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 0);
		uint o = Const.GetMaxExtendTimes();
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetBlankTex(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 0);
		Texture2D o = Const.GetBlankTex();
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetBlankSprite(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 0);
		Sprite o = Const.GetBlankSprite();
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetHitPercentage(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		string arg0 = LuaScriptMgr.GetLuaString(L, 1);
		float o = Const.GetHitPercentage(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetWeeklyOpenLevel(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 0);
		uint o = Const.GetWeeklyOpenLevel();
		LuaScriptMgr.Push(L, o);
		return 1;
	}
}

