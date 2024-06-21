﻿using System;
using UnityEngine.EventSystems;
using UnityEngine;
using LuaInterface;

public class ApplicationWrap
{
	public static void Register(IntPtr L)
	{
		LuaMethod[] regs = new LuaMethod[]
		{
			new LuaMethod("Quit", Quit),
			new LuaMethod("CancelQuit", CancelQuit),
			new LuaMethod("GetStreamProgressForLevel", GetStreamProgressForLevel),
			new LuaMethod("CanStreamedLevelBeLoaded", CanStreamedLevelBeLoaded),
			new LuaMethod("CaptureScreenshot", CaptureScreenshot),
			new LuaMethod("HasProLicense", HasProLicense),
			new LuaMethod("ExternalCall", ExternalCall),
			new LuaMethod("RequestAdvertisingIdentifierAsync", RequestAdvertisingIdentifierAsync),
			new LuaMethod("OpenURL", OpenURL),
			new LuaMethod("RequestUserAuthorization", RequestUserAuthorization),
			new LuaMethod("HasUserAuthorization", HasUserAuthorization),
			new LuaMethod("New", _CreateApplication),
			new LuaMethod("GetClassType", GetClassType),
		};

		LuaField[] fields = new LuaField[]
		{
			new LuaField("streamedBytes", get_streamedBytes, null),
			new LuaField("isPlaying", get_isPlaying, null),
			new LuaField("isEditor", get_isEditor, null),
			new LuaField("isWebPlayer", get_isWebPlayer, null),
			new LuaField("platform", get_platform, null),
			new LuaField("isMobilePlatform", get_isMobilePlatform, null),
			new LuaField("isConsolePlatform", get_isConsolePlatform, null),
			new LuaField("runInBackground", get_runInBackground, set_runInBackground),
			new LuaField("dataPath", get_dataPath, null),
			new LuaField("streamingAssetsPath", get_streamingAssetsPath, null),
			new LuaField("persistentDataPath", get_persistentDataPath, null),
			new LuaField("temporaryCachePath", get_temporaryCachePath, null),
			new LuaField("srcValue", get_srcValue, null),
			new LuaField("absoluteURL", get_absoluteURL, null),
			new LuaField("unityVersion", get_unityVersion, null),
			new LuaField("version", get_version, null),
			new LuaField("bundleIdentifier", get_bundleIdentifier, null),
			new LuaField("installMode", get_installMode, null),
			new LuaField("sandboxType", get_sandboxType, null),
			new LuaField("productName", get_productName, null),
			new LuaField("companyName", get_companyName, null),
			new LuaField("cloudProjectId", get_cloudProjectId, null),
			new LuaField("webSecurityEnabled", get_webSecurityEnabled, null),
			new LuaField("webSecurityHostUrl", get_webSecurityHostUrl, null),
			new LuaField("targetFrameRate", get_targetFrameRate, set_targetFrameRate),
			new LuaField("systemLanguage", get_systemLanguage, null),
			new LuaField("stackTraceLogType", get_stackTraceLogType, set_stackTraceLogType),
			new LuaField("backgroundLoadingPriority", get_backgroundLoadingPriority, set_backgroundLoadingPriority),
			new LuaField("internetReachability", get_internetReachability, null),
			new LuaField("genuine", get_genuine, null),
			new LuaField("genuineCheckAvailable", get_genuineCheckAvailable, null),
			new LuaField("isShowingSplashScreen", get_isShowingSplashScreen, null),
		};

		LuaScriptMgr.RegisterLib(L, "UnityEngine.Application", typeof(Application), regs, fields, typeof(object));
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateApplication(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 0)
		{
			Application obj = new Application();
			LuaScriptMgr.PushObject(L, obj);
			return 1;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: Application.New");
		}

		return 0;
	}

	static Type classType = typeof(Application);

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetClassType(IntPtr L)
	{
		LuaScriptMgr.Push(L, classType);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_streamedBytes(IntPtr L)
	{
		LuaScriptMgr.Push(L, Application.streamedBytes);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_isPlaying(IntPtr L)
	{
		LuaScriptMgr.Push(L, Application.isPlaying);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_isEditor(IntPtr L)
	{
		LuaScriptMgr.Push(L, Application.isEditor);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_isWebPlayer(IntPtr L)
	{
		LuaScriptMgr.Push(L, Application.isWebPlayer);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_platform(IntPtr L)
	{
		LuaScriptMgr.Push(L, Application.platform);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_isMobilePlatform(IntPtr L)
	{
		LuaScriptMgr.Push(L, Application.isMobilePlatform);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_isConsolePlatform(IntPtr L)
	{
		LuaScriptMgr.Push(L, Application.isConsolePlatform);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_runInBackground(IntPtr L)
	{
		LuaScriptMgr.Push(L, Application.runInBackground);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_dataPath(IntPtr L)
	{
		LuaScriptMgr.Push(L, Application.dataPath);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_streamingAssetsPath(IntPtr L)
	{
		LuaScriptMgr.Push(L, Application.streamingAssetsPath);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_persistentDataPath(IntPtr L)
	{
		LuaScriptMgr.Push(L, Application.persistentDataPath);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_temporaryCachePath(IntPtr L)
	{
		LuaScriptMgr.Push(L, Application.temporaryCachePath);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_srcValue(IntPtr L)
	{
		LuaScriptMgr.Push(L, Application.srcValue);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_absoluteURL(IntPtr L)
	{
		LuaScriptMgr.Push(L, Application.absoluteURL);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_unityVersion(IntPtr L)
	{
		LuaScriptMgr.Push(L, Application.unityVersion);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_version(IntPtr L)
	{
		LuaScriptMgr.Push(L, Application.version);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_bundleIdentifier(IntPtr L)
	{
		LuaScriptMgr.Push(L, Application.bundleIdentifier);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_installMode(IntPtr L)
	{
		LuaScriptMgr.Push(L, Application.installMode);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_sandboxType(IntPtr L)
	{
		LuaScriptMgr.Push(L, Application.sandboxType);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_productName(IntPtr L)
	{
		LuaScriptMgr.Push(L, Application.productName);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_companyName(IntPtr L)
	{
		LuaScriptMgr.Push(L, Application.companyName);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_cloudProjectId(IntPtr L)
	{
		LuaScriptMgr.Push(L, Application.cloudProjectId);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_webSecurityEnabled(IntPtr L)
	{
		LuaScriptMgr.Push(L, Application.webSecurityEnabled);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_webSecurityHostUrl(IntPtr L)
	{
		LuaScriptMgr.Push(L, Application.webSecurityHostUrl);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_targetFrameRate(IntPtr L)
	{
		LuaScriptMgr.Push(L, Application.targetFrameRate);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_systemLanguage(IntPtr L)
	{
		LuaScriptMgr.Push(L, Application.systemLanguage);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_stackTraceLogType(IntPtr L)
	{
		LuaScriptMgr.Push(L, Application.stackTraceLogType);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_backgroundLoadingPriority(IntPtr L)
	{
		LuaScriptMgr.Push(L, Application.backgroundLoadingPriority);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_internetReachability(IntPtr L)
	{
		LuaScriptMgr.Push(L, Application.internetReachability);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_genuine(IntPtr L)
	{
		LuaScriptMgr.Push(L, Application.genuine);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_genuineCheckAvailable(IntPtr L)
	{
		LuaScriptMgr.Push(L, Application.genuineCheckAvailable);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_isShowingSplashScreen(IntPtr L)
	{
		LuaScriptMgr.Push(L, Application.isShowingSplashScreen);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_runInBackground(IntPtr L)
	{
		Application.runInBackground = LuaScriptMgr.GetBoolean(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_targetFrameRate(IntPtr L)
	{
		Application.targetFrameRate = (int)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_stackTraceLogType(IntPtr L)
	{
		Application.stackTraceLogType = (StackTraceLogType)LuaScriptMgr.GetNetObject(L, 3, typeof(StackTraceLogType));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_backgroundLoadingPriority(IntPtr L)
	{
		Application.backgroundLoadingPriority = (ThreadPriority)LuaScriptMgr.GetNetObject(L, 3, typeof(ThreadPriority));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Quit(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 0);
		Application.Quit();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int CancelQuit(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 0);
		Application.CancelQuit();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetStreamProgressForLevel(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 1 && LuaScriptMgr.CheckTypes(L, 1, typeof(string)))
		{
			string arg0 = LuaScriptMgr.GetString(L, 1);
			float o = Application.GetStreamProgressForLevel(arg0);
			LuaScriptMgr.Push(L, o);
			return 1;
		}
		else if (count == 1 && LuaScriptMgr.CheckTypes(L, 1, typeof(int)))
		{
			int arg0 = (int)LuaDLL.lua_tonumber(L, 1);
			float o = Application.GetStreamProgressForLevel(arg0);
			LuaScriptMgr.Push(L, o);
			return 1;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: Application.GetStreamProgressForLevel");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int CanStreamedLevelBeLoaded(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 1 && LuaScriptMgr.CheckTypes(L, 1, typeof(string)))
		{
			string arg0 = LuaScriptMgr.GetString(L, 1);
			bool o = Application.CanStreamedLevelBeLoaded(arg0);
			LuaScriptMgr.Push(L, o);
			return 1;
		}
		else if (count == 1 && LuaScriptMgr.CheckTypes(L, 1, typeof(int)))
		{
			int arg0 = (int)LuaDLL.lua_tonumber(L, 1);
			bool o = Application.CanStreamedLevelBeLoaded(arg0);
			LuaScriptMgr.Push(L, o);
			return 1;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: Application.CanStreamedLevelBeLoaded");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int CaptureScreenshot(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 1)
		{
			string arg0 = LuaScriptMgr.GetLuaString(L, 1);
			Application.CaptureScreenshot(arg0);
			return 0;
		}
		else if (count == 2)
		{
			string arg0 = LuaScriptMgr.GetLuaString(L, 1);
			int arg1 = (int)LuaScriptMgr.GetNumber(L, 2);
			Application.CaptureScreenshot(arg0,arg1);
			return 0;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: Application.CaptureScreenshot");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int HasProLicense(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 0);
		bool o = Application.HasProLicense();
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ExternalCall(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);
		string arg0 = LuaScriptMgr.GetLuaString(L, 1);
		object[] objs1 = LuaScriptMgr.GetParamsObject(L, 2, count - 1);
		Application.ExternalCall(arg0,objs1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int RequestAdvertisingIdentifierAsync(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		Application.AdvertisingIdentifierCallback arg0 = null;
		LuaTypes funcType1 = LuaDLL.lua_type(L, 1);

		if (funcType1 != LuaTypes.LUA_TFUNCTION)
		{
			 arg0 = (Application.AdvertisingIdentifierCallback)LuaScriptMgr.GetNetObject(L, 1, typeof(Application.AdvertisingIdentifierCallback));
		}
		else
		{
			LuaFunction func = LuaScriptMgr.GetLuaFunction(L, 1);
			arg0 = (param0, param1, param2) =>
			{
				int top = func.BeginPCall();
				LuaScriptMgr.Push(L, param0);
				LuaScriptMgr.Push(L, param1);
				LuaScriptMgr.Push(L, param2);
				func.PCall(top, 3);
				func.EndPCall(top);
			};
		}

		bool o = Application.RequestAdvertisingIdentifierAsync(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int OpenURL(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		string arg0 = LuaScriptMgr.GetLuaString(L, 1);
		Application.OpenURL(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int RequestUserAuthorization(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		UserAuthorization arg0 = (UserAuthorization)LuaScriptMgr.GetNetObject(L, 1, typeof(UserAuthorization));
		AsyncOperation o = Application.RequestUserAuthorization(arg0);
		LuaScriptMgr.PushObject(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int HasUserAuthorization(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		UserAuthorization arg0 = (UserAuthorization)LuaScriptMgr.GetNetObject(L, 1, typeof(UserAuthorization));
		bool o = Application.HasUserAuthorization(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}
}

