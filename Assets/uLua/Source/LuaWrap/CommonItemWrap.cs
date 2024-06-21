using System;
using UnityEngine.EventSystems;
using UnityEngine;
using LuaInterface;

public class CommonItemWrap
{
	public static void Register(IntPtr L)
	{
		LuaMethod[] regs = new LuaMethod[]
		{
			new LuaMethod("SetCommonItem", SetCommonItem),
			new LuaMethod("SetCommonItemInLua", SetCommonItemInLua),
			new LuaMethod("SetNextUseTime", SetNextUseTime),
			new LuaMethod("SetCommonItemData", SetCommonItemData),
			new LuaMethod("DragDropDataClear", DragDropDataClear),
			new LuaMethod("SetCommonItemInMainPackage", SetCommonItemInMainPackage),
			new LuaMethod("SetCommonItemInMainPackageInLua", SetCommonItemInMainPackageInLua),
			new LuaMethod("Dispose", Dispose),
			new LuaMethod("btn_item_on_enter", btn_item_on_enter),
			new LuaMethod("btn_item_on_exit", btn_item_on_exit),
			new LuaMethod("PlayCDAnim", PlayCDAnim),
			new LuaMethod("GetBaseId", GetBaseId),
			new LuaMethod("GetBtnItem", GetBtnItem),
			new LuaMethod("New", _CreateCommonItem),
			new LuaMethod("GetClassType", GetClassType),
		};

		LuaField[] fields = new LuaField[]
		{
		};

		LuaScriptMgr.RegisterLib(L, "CommonItem", typeof(CommonItem), regs, fields, typeof(object));
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateCommonItem(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 1)
		{
			Transform arg0 = (Transform)LuaScriptMgr.GetUnityObject(L, 1, typeof(Transform));
			CommonItem obj = new CommonItem(arg0);
			LuaScriptMgr.PushObject(L, obj);
			return 1;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: CommonItem.New");
		}

		return 0;
	}

	static Type classType = typeof(CommonItem);

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetClassType(IntPtr L)
	{
		LuaScriptMgr.Push(L, classType);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetCommonItem(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 4)
		{
			CommonItem obj = (CommonItem)LuaScriptMgr.GetNetObjectSelf(L, 1, "CommonItem");
			uint arg0 = (uint)LuaScriptMgr.GetNumber(L, 2);
			uint arg1 = (uint)LuaScriptMgr.GetNumber(L, 3);
			Action<uint> arg2 = null;
			LuaTypes funcType4 = LuaDLL.lua_type(L, 4);

			if (funcType4 != LuaTypes.LUA_TFUNCTION)
			{
				 arg2 = (Action<uint>)LuaScriptMgr.GetNetObject(L, 4, typeof(Action<uint>));
			}
			else
			{
				LuaFunction func = LuaScriptMgr.GetLuaFunction(L, 4);
				arg2 = (param0) =>
				{
					int top = func.BeginPCall();
					LuaScriptMgr.Push(L, param0);
					func.PCall(top, 1);
					func.EndPCall(top);
				};
			}

			obj.SetCommonItem(arg0,arg1,arg2);
			return 0;
		}
		else if (count == 6)
		{
			CommonItem obj = (CommonItem)LuaScriptMgr.GetNetObjectSelf(L, 1, "CommonItem");
			uint arg0 = (uint)LuaScriptMgr.GetNumber(L, 2);
			uint arg1 = (uint)LuaScriptMgr.GetNumber(L, 3);
			bool arg2 = LuaScriptMgr.GetBoolean(L, 4);
			uint arg3 = (uint)LuaScriptMgr.GetNumber(L, 5);
			Action<uint> arg4 = null;
			LuaTypes funcType6 = LuaDLL.lua_type(L, 6);

			if (funcType6 != LuaTypes.LUA_TFUNCTION)
			{
				 arg4 = (Action<uint>)LuaScriptMgr.GetNetObject(L, 6, typeof(Action<uint>));
			}
			else
			{
				LuaFunction func = LuaScriptMgr.GetLuaFunction(L, 6);
				arg4 = (param0) =>
				{
					int top = func.BeginPCall();
					LuaScriptMgr.Push(L, param0);
					func.PCall(top, 1);
					func.EndPCall(top);
				};
			}

			obj.SetCommonItem(arg0,arg1,arg2,arg3,arg4);
			return 0;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: CommonItem.SetCommonItem");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetCommonItemInLua(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 5);
		CommonItem obj = (CommonItem)LuaScriptMgr.GetNetObjectSelf(L, 1, "CommonItem");
		uint arg0 = (uint)LuaScriptMgr.GetNumber(L, 2);
		int arg1 = (int)LuaScriptMgr.GetNumber(L, 3);
		uint arg2 = (uint)LuaScriptMgr.GetNumber(L, 4);
		Action<uint> arg3 = null;
		LuaTypes funcType5 = LuaDLL.lua_type(L, 5);

		if (funcType5 != LuaTypes.LUA_TFUNCTION)
		{
			 arg3 = (Action<uint>)LuaScriptMgr.GetNetObject(L, 5, typeof(Action<uint>));
		}
		else
		{
			LuaFunction func = LuaScriptMgr.GetLuaFunction(L, 5);
			arg3 = (param0) =>
			{
				int top = func.BeginPCall();
				LuaScriptMgr.Push(L, param0);
				func.PCall(top, 1);
				func.EndPCall(top);
			};
		}

		obj.SetCommonItemInLua(arg0,arg1,arg2,arg3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetNextUseTime(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		CommonItem obj = (CommonItem)LuaScriptMgr.GetNetObjectSelf(L, 1, "CommonItem");
		uint arg0 = (uint)LuaScriptMgr.GetNumber(L, 2);
		obj.SetNextUseTime(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetCommonItemData(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 7);
		CommonItem obj = (CommonItem)LuaScriptMgr.GetNetObjectSelf(L, 1, "CommonItem");
		uint arg0 = (uint)LuaScriptMgr.GetNumber(L, 2);
		string arg1 = LuaScriptMgr.GetLuaString(L, 3);
		string arg2 = LuaScriptMgr.GetLuaString(L, 4);
		uint arg3 = (uint)LuaScriptMgr.GetNumber(L, 5);
		int arg4 = (int)LuaScriptMgr.GetNumber(L, 6);
		uint arg5 = (uint)LuaScriptMgr.GetNumber(L, 7);
		obj.SetCommonItemData(arg0,arg1,arg2,arg3,arg4,arg5);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int DragDropDataClear(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		CommonItem obj = (CommonItem)LuaScriptMgr.GetNetObjectSelf(L, 1, "CommonItem");
		obj.DragDropDataClear();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetCommonItemInMainPackage(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 3);
		CommonItem obj = (CommonItem)LuaScriptMgr.GetNetObjectSelf(L, 1, "CommonItem");
		ulong arg0 = (ulong)LuaScriptMgr.GetNumber(L, 2);
		Action<uint> arg1 = null;
		LuaTypes funcType3 = LuaDLL.lua_type(L, 3);

		if (funcType3 != LuaTypes.LUA_TFUNCTION)
		{
			 arg1 = (Action<uint>)LuaScriptMgr.GetNetObject(L, 3, typeof(Action<uint>));
		}
		else
		{
			LuaFunction func = LuaScriptMgr.GetLuaFunction(L, 3);
			arg1 = (param0) =>
			{
				int top = func.BeginPCall();
				LuaScriptMgr.Push(L, param0);
				func.PCall(top, 1);
				func.EndPCall(top);
			};
		}

		bool o = obj.SetCommonItemInMainPackage(arg0,arg1);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetCommonItemInMainPackageInLua(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 3);
		CommonItem obj = (CommonItem)LuaScriptMgr.GetNetObjectSelf(L, 1, "CommonItem");
		string arg0 = LuaScriptMgr.GetLuaString(L, 2);
		Action<uint> arg1 = null;
		LuaTypes funcType3 = LuaDLL.lua_type(L, 3);

		if (funcType3 != LuaTypes.LUA_TFUNCTION)
		{
			 arg1 = (Action<uint>)LuaScriptMgr.GetNetObject(L, 3, typeof(Action<uint>));
		}
		else
		{
			LuaFunction func = LuaScriptMgr.GetLuaFunction(L, 3);
			arg1 = (param0) =>
			{
				int top = func.BeginPCall();
				LuaScriptMgr.Push(L, param0);
				func.PCall(top, 1);
				func.EndPCall(top);
			};
		}

		bool o = obj.SetCommonItemInMainPackageInLua(arg0,arg1);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Dispose(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		CommonItem obj = (CommonItem)LuaScriptMgr.GetNetObjectSelf(L, 1, "CommonItem");
		obj.Dispose();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int btn_item_on_enter(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		CommonItem obj = (CommonItem)LuaScriptMgr.GetNetObjectSelf(L, 1, "CommonItem");
		PointerEventData arg0 = (PointerEventData)LuaScriptMgr.GetNetObject(L, 2, typeof(PointerEventData));
		obj.btn_item_on_enter(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int btn_item_on_exit(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		CommonItem obj = (CommonItem)LuaScriptMgr.GetNetObjectSelf(L, 1, "CommonItem");
		PointerEventData arg0 = (PointerEventData)LuaScriptMgr.GetNetObject(L, 2, typeof(PointerEventData));
		obj.btn_item_on_exit(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int PlayCDAnim(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		CommonItem obj = (CommonItem)LuaScriptMgr.GetNetObjectSelf(L, 1, "CommonItem");
		obj.PlayCDAnim();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetBaseId(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		CommonItem obj = (CommonItem)LuaScriptMgr.GetNetObjectSelf(L, 1, "CommonItem");
		uint o = obj.GetBaseId();
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetBtnItem(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		CommonItem obj = (CommonItem)LuaScriptMgr.GetNetObjectSelf(L, 1, "CommonItem");
		Transform o = obj.GetBtnItem();
		LuaScriptMgr.Push(L, o);
		return 1;
	}
}

