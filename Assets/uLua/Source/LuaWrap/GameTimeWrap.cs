using System;
using UnityEngine.EventSystems;
using UnityEngine;
using LuaInterface;
using Object = UnityEngine.Object;

public class GameTimeWrap
{
	public static void Register(IntPtr L)
	{
		LuaMethod[] regs = new LuaMethod[]
		{
			new LuaMethod("GetTimeInterval", GetTimeInterval),
			new LuaMethod("GetNowMsecond", GetNowMsecond),
			new LuaMethod("GetIntervalMsecond", GetIntervalMsecond),
			new LuaMethod("GetNowSecond", GetNowSecond),
			new LuaMethod("Init", Init),
			new LuaMethod("OnServerTimeInit", OnServerTimeInit),
			new LuaMethod("OnServerTimeReq", OnServerTimeReq),
			new LuaMethod("SetServerTime", SetServerTime),
			new LuaMethod("GetCurrServerTime", GetCurrServerTime),
			new LuaMethod("GetCurrServerTimeBySecond", GetCurrServerTimeBySecond),
			new LuaMethod("GetCurrServerUlongTimeBySecond", GetCurrServerUlongTimeBySecond),
			new LuaMethod("GetCurrServerDateTime", GetCurrServerDateTime),
			new LuaMethod("GetTimeBySecond", GetTimeBySecond),
			new LuaMethod("GetDayBySecond", GetDayBySecond),
			new LuaMethod("GetHorBySecond", GetHorBySecond),
			new LuaMethod("GetMinBySecond", GetMinBySecond),
			new LuaMethod("GetServerDateTimeByTimeStamp", GetServerDateTimeByTimeStamp),
			new LuaMethod("GetTimeText", GetTimeText),
			new LuaMethod("GetTimeText1", GetTimeText1),
			new LuaMethod("ReqPing", ReqPing),
			new LuaMethod("OnRetPing", OnRetPing),
			new LuaMethod("New", _CreateGameTime),
			new LuaMethod("GetClassType", GetClassType),
			new LuaMethod("__eq", Lua_Eq),
		};

		LuaField[] fields = new LuaField[]
		{
			new LuaField("v0", get_v0, set_v0),
			new LuaField("a", get_a, set_a),
			new LuaField("HeartBeatInterval", get_HeartBeatInterval, null),
			new LuaField("Ping", get_Ping, set_Ping),
			new LuaField("lastreqpingtime", get_lastreqpingtime, set_lastreqpingtime),
			new LuaField("CheckPing", get_CheckPing, set_CheckPing),
		};

		LuaScriptMgr.RegisterLib(L, "GameTime", typeof(GameTime), regs, fields, typeof(SingletonForMono<GameTime>));
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateGameTime(IntPtr L)
	{
		LuaDLL.luaL_error(L, "GameTime class does not have a constructor function");
		return 0;
	}

	static Type classType = typeof(GameTime);

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetClassType(IntPtr L)
	{
		LuaScriptMgr.Push(L, classType);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_v0(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		GameTime obj = (GameTime)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name v0");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index v0 on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.v0);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_a(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		GameTime obj = (GameTime)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name a");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index a on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.a);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_HeartBeatInterval(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		GameTime obj = (GameTime)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name HeartBeatInterval");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index HeartBeatInterval on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.HeartBeatInterval);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_Ping(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		GameTime obj = (GameTime)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name Ping");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index Ping on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.Ping);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_lastreqpingtime(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		GameTime obj = (GameTime)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name lastreqpingtime");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index lastreqpingtime on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.lastreqpingtime);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_CheckPing(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		GameTime obj = (GameTime)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name CheckPing");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index CheckPing on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.CheckPing);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_v0(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		GameTime obj = (GameTime)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name v0");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index v0 on a nil value");
			}
		}

		obj.v0 = (float)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_a(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		GameTime obj = (GameTime)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name a");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index a on a nil value");
			}
		}

		obj.a = (float)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_Ping(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		GameTime obj = (GameTime)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name Ping");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index Ping on a nil value");
			}
		}

		obj.Ping = (uint)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_lastreqpingtime(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		GameTime obj = (GameTime)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name lastreqpingtime");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index lastreqpingtime on a nil value");
			}
		}

		obj.lastreqpingtime = (ulong)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_CheckPing(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		GameTime obj = (GameTime)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name CheckPing");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index CheckPing on a nil value");
			}
		}

		obj.CheckPing = LuaScriptMgr.GetBoolean(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetTimeInterval(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		GameTime obj = (GameTime)LuaScriptMgr.GetUnityObjectSelf(L, 1, "GameTime");
		ulong o = obj.GetTimeInterval();
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetNowMsecond(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		GameTime obj = (GameTime)LuaScriptMgr.GetUnityObjectSelf(L, 1, "GameTime");
		ulong o = obj.GetNowMsecond();
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetIntervalMsecond(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		GameTime obj = (GameTime)LuaScriptMgr.GetUnityObjectSelf(L, 1, "GameTime");
		uint o = obj.GetIntervalMsecond();
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetNowSecond(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		GameTime obj = (GameTime)LuaScriptMgr.GetUnityObjectSelf(L, 1, "GameTime");
		uint o = obj.GetNowSecond();
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Init(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		GameTime obj = (GameTime)LuaScriptMgr.GetUnityObjectSelf(L, 1, "GameTime");
		obj.Init();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int OnServerTimeInit(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		GameTime obj = (GameTime)LuaScriptMgr.GetUnityObjectSelf(L, 1, "GameTime");
		msg.MSG_Ret_GameTime_SC arg0 = (msg.MSG_Ret_GameTime_SC)LuaScriptMgr.GetNetObject(L, 2, typeof(msg.MSG_Ret_GameTime_SC));
		obj.OnServerTimeInit(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int OnServerTimeReq(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		GameTime obj = (GameTime)LuaScriptMgr.GetUnityObjectSelf(L, 1, "GameTime");
		msg.MSG_Req_UserGameTime_SC arg0 = (msg.MSG_Req_UserGameTime_SC)LuaScriptMgr.GetNetObject(L, 2, typeof(msg.MSG_Req_UserGameTime_SC));
		obj.OnServerTimeReq(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetServerTime(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		GameTime obj = (GameTime)LuaScriptMgr.GetUnityObjectSelf(L, 1, "GameTime");
		ulong arg0 = (ulong)LuaScriptMgr.GetNumber(L, 2);
		obj.SetServerTime(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetCurrServerTime(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		GameTime obj = (GameTime)LuaScriptMgr.GetUnityObjectSelf(L, 1, "GameTime");
		ulong o = obj.GetCurrServerTime();
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetCurrServerTimeBySecond(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		GameTime obj = (GameTime)LuaScriptMgr.GetUnityObjectSelf(L, 1, "GameTime");
		uint o = obj.GetCurrServerTimeBySecond();
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetCurrServerUlongTimeBySecond(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		GameTime obj = (GameTime)LuaScriptMgr.GetUnityObjectSelf(L, 1, "GameTime");
		ulong o = obj.GetCurrServerUlongTimeBySecond();
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetCurrServerDateTime(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		GameTime obj = (GameTime)LuaScriptMgr.GetUnityObjectSelf(L, 1, "GameTime");
		DateTime o = obj.GetCurrServerDateTime();
		LuaScriptMgr.PushValue(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetTimeBySecond(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 2)
		{
			GameTime obj = (GameTime)LuaScriptMgr.GetUnityObjectSelf(L, 1, "GameTime");
			ulong arg0 = (ulong)LuaScriptMgr.GetNumber(L, 2);
			string o = obj.GetTimeBySecond(arg0);
			LuaScriptMgr.Push(L, o);
			return 1;
		}
		else if (count == 3)
		{
			GameTime obj = (GameTime)LuaScriptMgr.GetUnityObjectSelf(L, 1, "GameTime");
			ulong arg0 = (ulong)LuaScriptMgr.GetNumber(L, 2);
			string arg1 = LuaScriptMgr.GetLuaString(L, 3);
			string o = obj.GetTimeBySecond(arg0,arg1);
			LuaScriptMgr.Push(L, o);
			return 1;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: GameTime.GetTimeBySecond");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetDayBySecond(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		GameTime obj = (GameTime)LuaScriptMgr.GetUnityObjectSelf(L, 1, "GameTime");
		ulong arg0 = (ulong)LuaScriptMgr.GetNumber(L, 2);
		uint o = obj.GetDayBySecond(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetHorBySecond(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		GameTime obj = (GameTime)LuaScriptMgr.GetUnityObjectSelf(L, 1, "GameTime");
		ulong arg0 = (ulong)LuaScriptMgr.GetNumber(L, 2);
		uint o = obj.GetHorBySecond(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetMinBySecond(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		GameTime obj = (GameTime)LuaScriptMgr.GetUnityObjectSelf(L, 1, "GameTime");
		ulong arg0 = (ulong)LuaScriptMgr.GetNumber(L, 2);
		uint o = obj.GetMinBySecond(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetServerDateTimeByTimeStamp(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		GameTime obj = (GameTime)LuaScriptMgr.GetUnityObjectSelf(L, 1, "GameTime");
		ulong arg0 = (ulong)LuaScriptMgr.GetNumber(L, 2);
		DateTime o = obj.GetServerDateTimeByTimeStamp(arg0);
		LuaScriptMgr.PushValue(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetTimeText(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		GameTime obj = (GameTime)LuaScriptMgr.GetUnityObjectSelf(L, 1, "GameTime");
		ulong arg0 = (ulong)LuaScriptMgr.GetNumber(L, 2);
		string o = obj.GetTimeText(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetTimeText1(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		GameTime obj = (GameTime)LuaScriptMgr.GetUnityObjectSelf(L, 1, "GameTime");
		int arg0 = (int)LuaScriptMgr.GetNumber(L, 2);
		string o = obj.GetTimeText1(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ReqPing(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		GameTime obj = (GameTime)LuaScriptMgr.GetUnityObjectSelf(L, 1, "GameTime");
		obj.ReqPing();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int OnRetPing(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		GameTime obj = (GameTime)LuaScriptMgr.GetUnityObjectSelf(L, 1, "GameTime");
		msg.MSG_Req_Ping_CS arg0 = (msg.MSG_Req_Ping_CS)LuaScriptMgr.GetNetObject(L, 2, typeof(msg.MSG_Req_Ping_CS));
		obj.OnRetPing(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Lua_Eq(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		Object arg0 = LuaScriptMgr.GetLuaObject(L, 1) as Object;
		Object arg1 = LuaScriptMgr.GetLuaObject(L, 2) as Object;
		bool o = arg0 == arg1;
		LuaScriptMgr.Push(L, o);
		return 1;
	}
}

