using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using LuaInterface;

public class ImageSeperateWithAlphaWrap
{
	public static void Register(IntPtr L)
	{
		LuaMethod[] regs = new LuaMethod[]
		{
			new LuaMethod("ProcessRawImageSeperateRGBA", ProcessRawImageSeperateRGBA),
			new LuaMethod("Dispose", Dispose),
			new LuaMethod("New", _CreateImageSeperateWithAlpha),
			new LuaMethod("GetClassType", GetClassType),
		};

		LuaField[] fields = new LuaField[]
		{
		};

		LuaScriptMgr.RegisterLib(L, "ImageSeperateWithAlpha", typeof(ImageSeperateWithAlpha), regs, fields, typeof(object));
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateImageSeperateWithAlpha(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 0)
		{
			ImageSeperateWithAlpha obj = new ImageSeperateWithAlpha();
			LuaScriptMgr.PushObject(L, obj);
			return 1;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: ImageSeperateWithAlpha.New");
		}

		return 0;
	}

	static Type classType = typeof(ImageSeperateWithAlpha);

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetClassType(IntPtr L)
	{
		LuaScriptMgr.Push(L, classType);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ProcessRawImageSeperateRGBA(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 4);
		ImageSeperateWithAlpha obj = (ImageSeperateWithAlpha)LuaScriptMgr.GetNetObjectSelf(L, 1, "ImageSeperateWithAlpha");
		RawImage arg0 = (RawImage)LuaScriptMgr.GetUnityObject(L, 2, typeof(RawImage));
		string arg1 = LuaScriptMgr.GetLuaString(L, 3);
		Action arg2 = null;
		LuaTypes funcType4 = LuaDLL.lua_type(L, 4);

		if (funcType4 != LuaTypes.LUA_TFUNCTION)
		{
			 arg2 = (Action)LuaScriptMgr.GetNetObject(L, 4, typeof(Action));
		}
		else
		{
			LuaFunction func = LuaScriptMgr.GetLuaFunction(L, 4);
			arg2 = () =>
			{
				func.Call();
			};
		}

		obj.ProcessRawImageSeperateRGBA(arg0,arg1,arg2);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Dispose(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		ImageSeperateWithAlpha obj = (ImageSeperateWithAlpha)LuaScriptMgr.GetNetObjectSelf(L, 1, "ImageSeperateWithAlpha");
		obj.Dispose();
		return 0;
	}
}

