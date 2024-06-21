﻿using System;
using UnityEngine.EventSystems;
using LuaInterface;

public class TypeWrap
{
	public static void Register(IntPtr L)
	{
		LuaMethod[] regs = new LuaMethod[]
		{
			new LuaMethod("Equals", Equals),
			new LuaMethod("GetType", GetType),
			new LuaMethod("GetTypeArray", GetTypeArray),
			new LuaMethod("GetTypeCode", GetTypeCode),
			new LuaMethod("GetTypeFromCLSID", GetTypeFromCLSID),
			new LuaMethod("GetTypeFromHandle", GetTypeFromHandle),
			new LuaMethod("GetTypeFromProgID", GetTypeFromProgID),
			new LuaMethod("GetTypeHandle", GetTypeHandle),
			new LuaMethod("IsSubclassOf", IsSubclassOf),
			new LuaMethod("FindInterfaces", FindInterfaces),
			new LuaMethod("GetInterface", GetInterface),
			new LuaMethod("GetInterfaceMap", GetInterfaceMap),
			new LuaMethod("GetInterfaces", GetInterfaces),
			new LuaMethod("IsAssignableFrom", IsAssignableFrom),
			new LuaMethod("IsInstanceOfType", IsInstanceOfType),
			new LuaMethod("GetArrayRank", GetArrayRank),
			new LuaMethod("GetElementType", GetElementType),
			new LuaMethod("GetEvent", GetEvent),
			new LuaMethod("GetEvents", GetEvents),
			new LuaMethod("GetField", GetField),
			new LuaMethod("GetFields", GetFields),
			new LuaMethod("GetHashCode", GetHashCode),
			new LuaMethod("GetMember", GetMember),
			new LuaMethod("GetMembers", GetMembers),
			new LuaMethod("GetMethod", GetMethod),
			new LuaMethod("GetMethods", GetMethods),
			new LuaMethod("GetNestedType", GetNestedType),
			new LuaMethod("GetNestedTypes", GetNestedTypes),
			new LuaMethod("GetProperties", GetProperties),
			new LuaMethod("GetProperty", GetProperty),
			new LuaMethod("GetConstructor", GetConstructor),
			new LuaMethod("GetConstructors", GetConstructors),
			new LuaMethod("GetDefaultMembers", GetDefaultMembers),
			new LuaMethod("FindMembers", FindMembers),
			new LuaMethod("InvokeMember", InvokeMember),
			new LuaMethod("ToString", ToString),
			new LuaMethod("GetGenericArguments", GetGenericArguments),
			new LuaMethod("GetGenericTypeDefinition", GetGenericTypeDefinition),
			new LuaMethod("MakeGenericType", MakeGenericType),
			new LuaMethod("GetGenericParameterConstraints", GetGenericParameterConstraints),
			new LuaMethod("MakeArrayType", MakeArrayType),
			new LuaMethod("MakeByRefType", MakeByRefType),
			new LuaMethod("MakePointerType", MakePointerType),
			new LuaMethod("ReflectionOnlyGetType", ReflectionOnlyGetType),
			new LuaMethod("New", _CreateType),
			new LuaMethod("GetClassType", GetClassType),
			new LuaMethod("__tostring", Lua_ToString),
		};

		LuaField[] fields = new LuaField[]
		{
			new LuaField("Delimiter", get_Delimiter, null),
			new LuaField("EmptyTypes", get_EmptyTypes, null),
			new LuaField("FilterAttribute", get_FilterAttribute, null),
			new LuaField("FilterName", get_FilterName, null),
			new LuaField("FilterNameIgnoreCase", get_FilterNameIgnoreCase, null),
			new LuaField("Missing", get_Missing, null),
			new LuaField("Assembly", get_Assembly, null),
			new LuaField("AssemblyQualifiedName", get_AssemblyQualifiedName, null),
			new LuaField("Attributes", get_Attributes, null),
			new LuaField("BaseType", get_BaseType, null),
			new LuaField("DeclaringType", get_DeclaringType, null),
			new LuaField("DefaultBinder", get_DefaultBinder, null),
			new LuaField("FullName", get_FullName, null),
			new LuaField("GUID", get_GUID, null),
			new LuaField("HasElementType", get_HasElementType, null),
			new LuaField("IsAbstract", get_IsAbstract, null),
			new LuaField("IsAnsiClass", get_IsAnsiClass, null),
			new LuaField("IsArray", get_IsArray, null),
			new LuaField("IsAutoClass", get_IsAutoClass, null),
			new LuaField("IsAutoLayout", get_IsAutoLayout, null),
			new LuaField("IsByRef", get_IsByRef, null),
			new LuaField("IsClass", get_IsClass, null),
			new LuaField("IsCOMObject", get_IsCOMObject, null),
			new LuaField("IsContextful", get_IsContextful, null),
			new LuaField("IsEnum", get_IsEnum, null),
			new LuaField("IsExplicitLayout", get_IsExplicitLayout, null),
			new LuaField("IsImport", get_IsImport, null),
			new LuaField("IsInterface", get_IsInterface, null),
			new LuaField("IsLayoutSequential", get_IsLayoutSequential, null),
			new LuaField("IsMarshalByRef", get_IsMarshalByRef, null),
			new LuaField("IsNestedAssembly", get_IsNestedAssembly, null),
			new LuaField("IsNestedFamANDAssem", get_IsNestedFamANDAssem, null),
			new LuaField("IsNestedFamily", get_IsNestedFamily, null),
			new LuaField("IsNestedFamORAssem", get_IsNestedFamORAssem, null),
			new LuaField("IsNestedPrivate", get_IsNestedPrivate, null),
			new LuaField("IsNestedPublic", get_IsNestedPublic, null),
			new LuaField("IsNotPublic", get_IsNotPublic, null),
			new LuaField("IsPointer", get_IsPointer, null),
			new LuaField("IsPrimitive", get_IsPrimitive, null),
			new LuaField("IsPublic", get_IsPublic, null),
			new LuaField("IsSealed", get_IsSealed, null),
			new LuaField("IsSerializable", get_IsSerializable, null),
			new LuaField("IsSpecialName", get_IsSpecialName, null),
			new LuaField("IsUnicodeClass", get_IsUnicodeClass, null),
			new LuaField("IsValueType", get_IsValueType, null),
			new LuaField("MemberType", get_MemberType, null),
			new LuaField("Module", get_Module, null),
			new LuaField("Namespace", get_Namespace, null),
			new LuaField("ReflectedType", get_ReflectedType, null),
			new LuaField("TypeHandle", get_TypeHandle, null),
			new LuaField("TypeInitializer", get_TypeInitializer, null),
			new LuaField("UnderlyingSystemType", get_UnderlyingSystemType, null),
			new LuaField("ContainsGenericParameters", get_ContainsGenericParameters, null),
			new LuaField("IsGenericTypeDefinition", get_IsGenericTypeDefinition, null),
			new LuaField("IsGenericType", get_IsGenericType, null),
			new LuaField("IsGenericParameter", get_IsGenericParameter, null),
			new LuaField("IsNested", get_IsNested, null),
			new LuaField("IsVisible", get_IsVisible, null),
			new LuaField("GenericParameterPosition", get_GenericParameterPosition, null),
			new LuaField("GenericParameterAttributes", get_GenericParameterAttributes, null),
			new LuaField("DeclaringMethod", get_DeclaringMethod, null),
			new LuaField("StructLayoutAttribute", get_StructLayoutAttribute, null),
		};

		LuaScriptMgr.RegisterLib(L, "System.Type", typeof(Type), regs, fields, typeof(System.Object));
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateType(IntPtr L)
	{
		LuaDLL.luaL_error(L, "Type class does not have a constructor function");
		return 0;
	}

	static Type classType = typeof(Type);

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetClassType(IntPtr L)
	{
		LuaScriptMgr.Push(L, classType);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_Delimiter(IntPtr L)
	{
		LuaScriptMgr.Push(L, Type.Delimiter);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_EmptyTypes(IntPtr L)
	{
		LuaScriptMgr.PushArray(L, Type.EmptyTypes);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_FilterAttribute(IntPtr L)
	{
		LuaScriptMgr.Push(L, Type.FilterAttribute);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_FilterName(IntPtr L)
	{
		LuaScriptMgr.Push(L, Type.FilterName);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_FilterNameIgnoreCase(IntPtr L)
	{
		LuaScriptMgr.Push(L, Type.FilterNameIgnoreCase);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_Missing(IntPtr L)
	{
		LuaScriptMgr.PushVarObject(L, Type.Missing);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_Assembly(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Type obj = (Type)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name Assembly");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index Assembly on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.Assembly);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_AssemblyQualifiedName(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Type obj = (Type)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name AssemblyQualifiedName");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index AssemblyQualifiedName on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.AssemblyQualifiedName);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_Attributes(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Type obj = (Type)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name Attributes");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index Attributes on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.Attributes);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_BaseType(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Type obj = (Type)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name BaseType");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index BaseType on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.BaseType);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_DeclaringType(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Type obj = (Type)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name DeclaringType");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index DeclaringType on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.DeclaringType);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_DefaultBinder(IntPtr L)
	{
		LuaScriptMgr.PushObject(L, Type.DefaultBinder);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_FullName(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Type obj = (Type)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name FullName");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index FullName on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.FullName);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_GUID(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Type obj = (Type)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name GUID");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index GUID on a nil value");
			}
		}

		LuaScriptMgr.PushValue(L, obj.GUID);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_HasElementType(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Type obj = (Type)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name HasElementType");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index HasElementType on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.HasElementType);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_IsAbstract(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Type obj = (Type)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name IsAbstract");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index IsAbstract on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.IsAbstract);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_IsAnsiClass(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Type obj = (Type)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name IsAnsiClass");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index IsAnsiClass on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.IsAnsiClass);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_IsArray(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Type obj = (Type)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name IsArray");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index IsArray on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.IsArray);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_IsAutoClass(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Type obj = (Type)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name IsAutoClass");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index IsAutoClass on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.IsAutoClass);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_IsAutoLayout(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Type obj = (Type)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name IsAutoLayout");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index IsAutoLayout on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.IsAutoLayout);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_IsByRef(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Type obj = (Type)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name IsByRef");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index IsByRef on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.IsByRef);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_IsClass(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Type obj = (Type)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name IsClass");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index IsClass on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.IsClass);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_IsCOMObject(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Type obj = (Type)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name IsCOMObject");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index IsCOMObject on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.IsCOMObject);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_IsContextful(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Type obj = (Type)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name IsContextful");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index IsContextful on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.IsContextful);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_IsEnum(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Type obj = (Type)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name IsEnum");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index IsEnum on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.IsEnum);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_IsExplicitLayout(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Type obj = (Type)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name IsExplicitLayout");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index IsExplicitLayout on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.IsExplicitLayout);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_IsImport(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Type obj = (Type)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name IsImport");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index IsImport on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.IsImport);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_IsInterface(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Type obj = (Type)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name IsInterface");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index IsInterface on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.IsInterface);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_IsLayoutSequential(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Type obj = (Type)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name IsLayoutSequential");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index IsLayoutSequential on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.IsLayoutSequential);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_IsMarshalByRef(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Type obj = (Type)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name IsMarshalByRef");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index IsMarshalByRef on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.IsMarshalByRef);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_IsNestedAssembly(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Type obj = (Type)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name IsNestedAssembly");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index IsNestedAssembly on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.IsNestedAssembly);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_IsNestedFamANDAssem(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Type obj = (Type)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name IsNestedFamANDAssem");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index IsNestedFamANDAssem on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.IsNestedFamANDAssem);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_IsNestedFamily(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Type obj = (Type)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name IsNestedFamily");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index IsNestedFamily on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.IsNestedFamily);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_IsNestedFamORAssem(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Type obj = (Type)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name IsNestedFamORAssem");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index IsNestedFamORAssem on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.IsNestedFamORAssem);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_IsNestedPrivate(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Type obj = (Type)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name IsNestedPrivate");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index IsNestedPrivate on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.IsNestedPrivate);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_IsNestedPublic(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Type obj = (Type)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name IsNestedPublic");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index IsNestedPublic on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.IsNestedPublic);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_IsNotPublic(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Type obj = (Type)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name IsNotPublic");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index IsNotPublic on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.IsNotPublic);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_IsPointer(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Type obj = (Type)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name IsPointer");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index IsPointer on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.IsPointer);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_IsPrimitive(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Type obj = (Type)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name IsPrimitive");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index IsPrimitive on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.IsPrimitive);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_IsPublic(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Type obj = (Type)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name IsPublic");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index IsPublic on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.IsPublic);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_IsSealed(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Type obj = (Type)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name IsSealed");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index IsSealed on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.IsSealed);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_IsSerializable(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Type obj = (Type)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name IsSerializable");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index IsSerializable on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.IsSerializable);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_IsSpecialName(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Type obj = (Type)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name IsSpecialName");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index IsSpecialName on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.IsSpecialName);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_IsUnicodeClass(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Type obj = (Type)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name IsUnicodeClass");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index IsUnicodeClass on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.IsUnicodeClass);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_IsValueType(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Type obj = (Type)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name IsValueType");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index IsValueType on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.IsValueType);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_MemberType(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Type obj = (Type)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name MemberType");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index MemberType on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.MemberType);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_Module(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Type obj = (Type)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name Module");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index Module on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.Module);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_Namespace(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Type obj = (Type)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name Namespace");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index Namespace on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.Namespace);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_ReflectedType(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Type obj = (Type)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name ReflectedType");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index ReflectedType on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.ReflectedType);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_TypeHandle(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Type obj = (Type)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name TypeHandle");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index TypeHandle on a nil value");
			}
		}

		LuaScriptMgr.PushValue(L, obj.TypeHandle);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_TypeInitializer(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Type obj = (Type)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name TypeInitializer");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index TypeInitializer on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.TypeInitializer);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_UnderlyingSystemType(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Type obj = (Type)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name UnderlyingSystemType");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index UnderlyingSystemType on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.UnderlyingSystemType);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_ContainsGenericParameters(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Type obj = (Type)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name ContainsGenericParameters");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index ContainsGenericParameters on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.ContainsGenericParameters);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_IsGenericTypeDefinition(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Type obj = (Type)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name IsGenericTypeDefinition");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index IsGenericTypeDefinition on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.IsGenericTypeDefinition);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_IsGenericType(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Type obj = (Type)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name IsGenericType");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index IsGenericType on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.IsGenericType);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_IsGenericParameter(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Type obj = (Type)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name IsGenericParameter");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index IsGenericParameter on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.IsGenericParameter);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_IsNested(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Type obj = (Type)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name IsNested");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index IsNested on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.IsNested);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_IsVisible(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Type obj = (Type)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name IsVisible");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index IsVisible on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.IsVisible);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_GenericParameterPosition(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Type obj = (Type)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name GenericParameterPosition");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index GenericParameterPosition on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.GenericParameterPosition);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_GenericParameterAttributes(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Type obj = (Type)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name GenericParameterAttributes");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index GenericParameterAttributes on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.GenericParameterAttributes);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_DeclaringMethod(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Type obj = (Type)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name DeclaringMethod");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index DeclaringMethod on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.DeclaringMethod);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_StructLayoutAttribute(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		Type obj = (Type)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name StructLayoutAttribute");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index StructLayoutAttribute on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.StructLayoutAttribute);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Lua_ToString(IntPtr L)
	{
		object obj = LuaScriptMgr.GetLuaObject(L, 1);

		if (obj != null)
		{
			LuaScriptMgr.Push(L, obj.ToString());
		}
		else
		{
			LuaScriptMgr.Push(L, "Table: System.Type");
		}

		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Equals(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 2 && LuaScriptMgr.CheckTypes(L, 1, typeof(Type), typeof(Type)))
		{
			Type obj = LuaScriptMgr.GetVarObject(L, 1) as Type;
			Type arg0 = LuaScriptMgr.GetTypeObject(L, 2);
			bool o = obj != null ? obj.Equals(arg0) : arg0 == null;
			LuaScriptMgr.Push(L, o);
			return 1;
		}
		else if (count == 2 && LuaScriptMgr.CheckTypes(L, 1, typeof(Type), typeof(object)))
		{
			Type obj = LuaScriptMgr.GetVarObject(L, 1) as Type;
			object arg0 = LuaScriptMgr.GetVarObject(L, 2);
			bool o = obj != null ? obj.Equals(arg0) : arg0 == null;
			LuaScriptMgr.Push(L, o);
			return 1;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: Type.Equals");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetType(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 1)
		{
			Type obj = LuaScriptMgr.GetTypeObject(L, 1);
			Type o = obj.GetType();
			LuaScriptMgr.Push(L, o);
			return 1;
		}
		else if (count == 1 && LuaScriptMgr.CheckTypes(L, 1, typeof(string)))
		{
			string arg0 = LuaScriptMgr.GetString(L, 1);
			Type o = Type.GetType(arg0);
			LuaScriptMgr.Push(L, o);
			return 1;
		}
		else if (count == 2)
		{
			string arg0 = LuaScriptMgr.GetLuaString(L, 1);
			bool arg1 = LuaScriptMgr.GetBoolean(L, 2);
			Type o = Type.GetType(arg0,arg1);
			LuaScriptMgr.Push(L, o);
			return 1;
		}
		else if (count == 3)
		{
			string arg0 = LuaScriptMgr.GetLuaString(L, 1);
			bool arg1 = LuaScriptMgr.GetBoolean(L, 2);
			bool arg2 = LuaScriptMgr.GetBoolean(L, 3);
			Type o = Type.GetType(arg0,arg1,arg2);
			LuaScriptMgr.Push(L, o);
			return 1;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: Type.GetType");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetTypeArray(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		object[] objs0 = LuaScriptMgr.GetArrayObject<object>(L, 1);
		Type[] o = Type.GetTypeArray(objs0);
		LuaScriptMgr.PushArray(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetTypeCode(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		Type arg0 = LuaScriptMgr.GetTypeObject(L, 1);
		TypeCode o = Type.GetTypeCode(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetTypeFromCLSID(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 1)
		{
			Guid arg0 = (Guid)LuaScriptMgr.GetNetObject(L, 1, typeof(Guid));
			Type o = Type.GetTypeFromCLSID(arg0);
			LuaScriptMgr.Push(L, o);
			return 1;
		}
		else if (count == 2 && LuaScriptMgr.CheckTypes(L, 1, typeof(Guid), typeof(string)))
		{
			Guid arg0 = (Guid)LuaScriptMgr.GetLuaObject(L, 1);
			string arg1 = LuaScriptMgr.GetString(L, 2);
			Type o = Type.GetTypeFromCLSID(arg0,arg1);
			LuaScriptMgr.Push(L, o);
			return 1;
		}
		else if (count == 2 && LuaScriptMgr.CheckTypes(L, 1, typeof(Guid), typeof(bool)))
		{
			Guid arg0 = (Guid)LuaScriptMgr.GetLuaObject(L, 1);
			bool arg1 = LuaDLL.lua_toboolean(L, 2);
			Type o = Type.GetTypeFromCLSID(arg0,arg1);
			LuaScriptMgr.Push(L, o);
			return 1;
		}
		else if (count == 3)
		{
			Guid arg0 = (Guid)LuaScriptMgr.GetNetObject(L, 1, typeof(Guid));
			string arg1 = LuaScriptMgr.GetLuaString(L, 2);
			bool arg2 = LuaScriptMgr.GetBoolean(L, 3);
			Type o = Type.GetTypeFromCLSID(arg0,arg1,arg2);
			LuaScriptMgr.Push(L, o);
			return 1;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: Type.GetTypeFromCLSID");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetTypeFromHandle(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		RuntimeTypeHandle arg0 = (RuntimeTypeHandle)LuaScriptMgr.GetNetObject(L, 1, typeof(RuntimeTypeHandle));
		Type o = Type.GetTypeFromHandle(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetTypeFromProgID(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 1)
		{
			string arg0 = LuaScriptMgr.GetLuaString(L, 1);
			Type o = Type.GetTypeFromProgID(arg0);
			LuaScriptMgr.Push(L, o);
			return 1;
		}
		else if (count == 2 && LuaScriptMgr.CheckTypes(L, 1, typeof(string), typeof(string)))
		{
			string arg0 = LuaScriptMgr.GetString(L, 1);
			string arg1 = LuaScriptMgr.GetString(L, 2);
			Type o = Type.GetTypeFromProgID(arg0,arg1);
			LuaScriptMgr.Push(L, o);
			return 1;
		}
		else if (count == 2 && LuaScriptMgr.CheckTypes(L, 1, typeof(string), typeof(bool)))
		{
			string arg0 = LuaScriptMgr.GetString(L, 1);
			bool arg1 = LuaDLL.lua_toboolean(L, 2);
			Type o = Type.GetTypeFromProgID(arg0,arg1);
			LuaScriptMgr.Push(L, o);
			return 1;
		}
		else if (count == 3)
		{
			string arg0 = LuaScriptMgr.GetLuaString(L, 1);
			string arg1 = LuaScriptMgr.GetLuaString(L, 2);
			bool arg2 = LuaScriptMgr.GetBoolean(L, 3);
			Type o = Type.GetTypeFromProgID(arg0,arg1,arg2);
			LuaScriptMgr.Push(L, o);
			return 1;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: Type.GetTypeFromProgID");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetTypeHandle(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		object arg0 = LuaScriptMgr.GetVarObject(L, 1);
		RuntimeTypeHandle o = Type.GetTypeHandle(arg0);
		LuaScriptMgr.PushValue(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int IsSubclassOf(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		Type obj = LuaScriptMgr.GetTypeObject(L, 1);
		Type arg0 = LuaScriptMgr.GetTypeObject(L, 2);
		bool o = obj.IsSubclassOf(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int FindInterfaces(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 3);
		Type obj = LuaScriptMgr.GetTypeObject(L, 1);
		System.Reflection.TypeFilter arg0 = null;
		LuaTypes funcType2 = LuaDLL.lua_type(L, 2);

		if (funcType2 != LuaTypes.LUA_TFUNCTION)
		{
			 arg0 = (System.Reflection.TypeFilter)LuaScriptMgr.GetNetObject(L, 2, typeof(System.Reflection.TypeFilter));
		}
		else
		{
			LuaFunction func = LuaScriptMgr.GetLuaFunction(L, 2);
			arg0 = (param0, param1) =>
			{
				int top = func.BeginPCall();
				LuaScriptMgr.Push(L, param0);
				LuaScriptMgr.PushVarObject(L, param1);
				func.PCall(top, 2);
				object[] objs = func.PopValues(top);
				func.EndPCall(top);
				return (bool)objs[0];
			};
		}

		object arg1 = LuaScriptMgr.GetVarObject(L, 3);
		Type[] o = obj.FindInterfaces(arg0,arg1);
		LuaScriptMgr.PushArray(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetInterface(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 2)
		{
			Type obj = LuaScriptMgr.GetTypeObject(L, 1);
			string arg0 = LuaScriptMgr.GetLuaString(L, 2);
			Type o = obj.GetInterface(arg0);
			LuaScriptMgr.Push(L, o);
			return 1;
		}
		else if (count == 3)
		{
			Type obj = LuaScriptMgr.GetTypeObject(L, 1);
			string arg0 = LuaScriptMgr.GetLuaString(L, 2);
			bool arg1 = LuaScriptMgr.GetBoolean(L, 3);
			Type o = obj.GetInterface(arg0,arg1);
			LuaScriptMgr.Push(L, o);
			return 1;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: Type.GetInterface");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetInterfaceMap(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		Type obj = LuaScriptMgr.GetTypeObject(L, 1);
		Type arg0 = LuaScriptMgr.GetTypeObject(L, 2);
		System.Reflection.InterfaceMapping o = obj.GetInterfaceMap(arg0);
		LuaScriptMgr.PushValue(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetInterfaces(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		Type obj = LuaScriptMgr.GetTypeObject(L, 1);
		Type[] o = obj.GetInterfaces();
		LuaScriptMgr.PushArray(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int IsAssignableFrom(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		Type obj = LuaScriptMgr.GetTypeObject(L, 1);
		Type arg0 = LuaScriptMgr.GetTypeObject(L, 2);
		bool o = obj.IsAssignableFrom(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int IsInstanceOfType(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		Type obj = LuaScriptMgr.GetTypeObject(L, 1);
		object arg0 = LuaScriptMgr.GetVarObject(L, 2);
		bool o = obj.IsInstanceOfType(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetArrayRank(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		Type obj = LuaScriptMgr.GetTypeObject(L, 1);
		int o = obj.GetArrayRank();
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetElementType(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		Type obj = LuaScriptMgr.GetTypeObject(L, 1);
		Type o = obj.GetElementType();
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetEvent(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 2)
		{
			Type obj = LuaScriptMgr.GetTypeObject(L, 1);
			string arg0 = LuaScriptMgr.GetLuaString(L, 2);
			System.Reflection.EventInfo o = obj.GetEvent(arg0);
			LuaScriptMgr.PushObject(L, o);
			return 1;
		}
		else if (count == 3)
		{
			Type obj = LuaScriptMgr.GetTypeObject(L, 1);
			string arg0 = LuaScriptMgr.GetLuaString(L, 2);
			System.Reflection.BindingFlags arg1 = (System.Reflection.BindingFlags)LuaScriptMgr.GetNetObject(L, 3, typeof(System.Reflection.BindingFlags));
			System.Reflection.EventInfo o = obj.GetEvent(arg0,arg1);
			LuaScriptMgr.PushObject(L, o);
			return 1;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: Type.GetEvent");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetEvents(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 1)
		{
			Type obj = LuaScriptMgr.GetTypeObject(L, 1);
			System.Reflection.EventInfo[] o = obj.GetEvents();
			LuaScriptMgr.PushArray(L, o);
			return 1;
		}
		else if (count == 2)
		{
			Type obj = LuaScriptMgr.GetTypeObject(L, 1);
			System.Reflection.BindingFlags arg0 = (System.Reflection.BindingFlags)LuaScriptMgr.GetNetObject(L, 2, typeof(System.Reflection.BindingFlags));
			System.Reflection.EventInfo[] o = obj.GetEvents(arg0);
			LuaScriptMgr.PushArray(L, o);
			return 1;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: Type.GetEvents");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetField(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 2)
		{
			Type obj = LuaScriptMgr.GetTypeObject(L, 1);
			string arg0 = LuaScriptMgr.GetLuaString(L, 2);
			System.Reflection.FieldInfo o = obj.GetField(arg0);
			LuaScriptMgr.PushObject(L, o);
			return 1;
		}
		else if (count == 3)
		{
			Type obj = LuaScriptMgr.GetTypeObject(L, 1);
			string arg0 = LuaScriptMgr.GetLuaString(L, 2);
			System.Reflection.BindingFlags arg1 = (System.Reflection.BindingFlags)LuaScriptMgr.GetNetObject(L, 3, typeof(System.Reflection.BindingFlags));
			System.Reflection.FieldInfo o = obj.GetField(arg0,arg1);
			LuaScriptMgr.PushObject(L, o);
			return 1;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: Type.GetField");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetFields(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 1)
		{
			Type obj = LuaScriptMgr.GetTypeObject(L, 1);
			System.Reflection.FieldInfo[] o = obj.GetFields();
			LuaScriptMgr.PushArray(L, o);
			return 1;
		}
		else if (count == 2)
		{
			Type obj = LuaScriptMgr.GetTypeObject(L, 1);
			System.Reflection.BindingFlags arg0 = (System.Reflection.BindingFlags)LuaScriptMgr.GetNetObject(L, 2, typeof(System.Reflection.BindingFlags));
			System.Reflection.FieldInfo[] o = obj.GetFields(arg0);
			LuaScriptMgr.PushArray(L, o);
			return 1;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: Type.GetFields");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetHashCode(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		Type obj = LuaScriptMgr.GetTypeObject(L, 1);
		int o = obj.GetHashCode();
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetMember(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 2)
		{
			Type obj = LuaScriptMgr.GetTypeObject(L, 1);
			string arg0 = LuaScriptMgr.GetLuaString(L, 2);
			System.Reflection.MemberInfo[] o = obj.GetMember(arg0);
			LuaScriptMgr.PushArray(L, o);
			return 1;
		}
		else if (count == 3)
		{
			Type obj = LuaScriptMgr.GetTypeObject(L, 1);
			string arg0 = LuaScriptMgr.GetLuaString(L, 2);
			System.Reflection.BindingFlags arg1 = (System.Reflection.BindingFlags)LuaScriptMgr.GetNetObject(L, 3, typeof(System.Reflection.BindingFlags));
			System.Reflection.MemberInfo[] o = obj.GetMember(arg0,arg1);
			LuaScriptMgr.PushArray(L, o);
			return 1;
		}
		else if (count == 4)
		{
			Type obj = LuaScriptMgr.GetTypeObject(L, 1);
			string arg0 = LuaScriptMgr.GetLuaString(L, 2);
			System.Reflection.MemberTypes arg1 = (System.Reflection.MemberTypes)LuaScriptMgr.GetNetObject(L, 3, typeof(System.Reflection.MemberTypes));
			System.Reflection.BindingFlags arg2 = (System.Reflection.BindingFlags)LuaScriptMgr.GetNetObject(L, 4, typeof(System.Reflection.BindingFlags));
			System.Reflection.MemberInfo[] o = obj.GetMember(arg0,arg1,arg2);
			LuaScriptMgr.PushArray(L, o);
			return 1;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: Type.GetMember");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetMembers(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 1)
		{
			Type obj = LuaScriptMgr.GetTypeObject(L, 1);
			System.Reflection.MemberInfo[] o = obj.GetMembers();
			LuaScriptMgr.PushArray(L, o);
			return 1;
		}
		else if (count == 2)
		{
			Type obj = LuaScriptMgr.GetTypeObject(L, 1);
			System.Reflection.BindingFlags arg0 = (System.Reflection.BindingFlags)LuaScriptMgr.GetNetObject(L, 2, typeof(System.Reflection.BindingFlags));
			System.Reflection.MemberInfo[] o = obj.GetMembers(arg0);
			LuaScriptMgr.PushArray(L, o);
			return 1;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: Type.GetMembers");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetMethod(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 2)
		{
			Type obj = LuaScriptMgr.GetTypeObject(L, 1);
			string arg0 = LuaScriptMgr.GetLuaString(L, 2);
			System.Reflection.MethodInfo o = obj.GetMethod(arg0);
			LuaScriptMgr.PushObject(L, o);
			return 1;
		}
		else if (count == 3 && LuaScriptMgr.CheckTypes(L, 1, typeof(Type), typeof(string), typeof(Type[])))
		{
			Type obj = LuaScriptMgr.GetTypeObject(L, 1);
			string arg0 = LuaScriptMgr.GetString(L, 2);
			Type[] objs1 = LuaScriptMgr.GetArrayObject<Type>(L, 3);
			System.Reflection.MethodInfo o = obj.GetMethod(arg0,objs1);
			LuaScriptMgr.PushObject(L, o);
			return 1;
		}
		else if (count == 3 && LuaScriptMgr.CheckTypes(L, 1, typeof(Type), typeof(string), typeof(System.Reflection.BindingFlags)))
		{
			Type obj = LuaScriptMgr.GetTypeObject(L, 1);
			string arg0 = LuaScriptMgr.GetString(L, 2);
			System.Reflection.BindingFlags arg1 = (System.Reflection.BindingFlags)LuaScriptMgr.GetLuaObject(L, 3);
			System.Reflection.MethodInfo o = obj.GetMethod(arg0,arg1);
			LuaScriptMgr.PushObject(L, o);
			return 1;
		}
		else if (count == 4)
		{
			Type obj = LuaScriptMgr.GetTypeObject(L, 1);
			string arg0 = LuaScriptMgr.GetLuaString(L, 2);
			Type[] objs1 = LuaScriptMgr.GetArrayObject<Type>(L, 3);
			System.Reflection.ParameterModifier[] objs2 = LuaScriptMgr.GetArrayObject<System.Reflection.ParameterModifier>(L, 4);
			System.Reflection.MethodInfo o = obj.GetMethod(arg0,objs1,objs2);
			LuaScriptMgr.PushObject(L, o);
			return 1;
		}
		else if (count == 6)
		{
			Type obj = LuaScriptMgr.GetTypeObject(L, 1);
			string arg0 = LuaScriptMgr.GetLuaString(L, 2);
			System.Reflection.BindingFlags arg1 = (System.Reflection.BindingFlags)LuaScriptMgr.GetNetObject(L, 3, typeof(System.Reflection.BindingFlags));
			System.Reflection.Binder arg2 = (System.Reflection.Binder)LuaScriptMgr.GetNetObject(L, 4, typeof(System.Reflection.Binder));
			Type[] objs3 = LuaScriptMgr.GetArrayObject<Type>(L, 5);
			System.Reflection.ParameterModifier[] objs4 = LuaScriptMgr.GetArrayObject<System.Reflection.ParameterModifier>(L, 6);
			System.Reflection.MethodInfo o = obj.GetMethod(arg0,arg1,arg2,objs3,objs4);
			LuaScriptMgr.PushObject(L, o);
			return 1;
		}
		else if (count == 7)
		{
			Type obj = LuaScriptMgr.GetTypeObject(L, 1);
			string arg0 = LuaScriptMgr.GetLuaString(L, 2);
			System.Reflection.BindingFlags arg1 = (System.Reflection.BindingFlags)LuaScriptMgr.GetNetObject(L, 3, typeof(System.Reflection.BindingFlags));
			System.Reflection.Binder arg2 = (System.Reflection.Binder)LuaScriptMgr.GetNetObject(L, 4, typeof(System.Reflection.Binder));
			System.Reflection.CallingConventions arg3 = (System.Reflection.CallingConventions)LuaScriptMgr.GetNetObject(L, 5, typeof(System.Reflection.CallingConventions));
			Type[] objs4 = LuaScriptMgr.GetArrayObject<Type>(L, 6);
			System.Reflection.ParameterModifier[] objs5 = LuaScriptMgr.GetArrayObject<System.Reflection.ParameterModifier>(L, 7);
			System.Reflection.MethodInfo o = obj.GetMethod(arg0,arg1,arg2,arg3,objs4,objs5);
			LuaScriptMgr.PushObject(L, o);
			return 1;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: Type.GetMethod");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetMethods(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 1)
		{
			Type obj = LuaScriptMgr.GetTypeObject(L, 1);
			System.Reflection.MethodInfo[] o = obj.GetMethods();
			LuaScriptMgr.PushArray(L, o);
			return 1;
		}
		else if (count == 2)
		{
			Type obj = LuaScriptMgr.GetTypeObject(L, 1);
			System.Reflection.BindingFlags arg0 = (System.Reflection.BindingFlags)LuaScriptMgr.GetNetObject(L, 2, typeof(System.Reflection.BindingFlags));
			System.Reflection.MethodInfo[] o = obj.GetMethods(arg0);
			LuaScriptMgr.PushArray(L, o);
			return 1;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: Type.GetMethods");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetNestedType(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 2)
		{
			Type obj = LuaScriptMgr.GetTypeObject(L, 1);
			string arg0 = LuaScriptMgr.GetLuaString(L, 2);
			Type o = obj.GetNestedType(arg0);
			LuaScriptMgr.Push(L, o);
			return 1;
		}
		else if (count == 3)
		{
			Type obj = LuaScriptMgr.GetTypeObject(L, 1);
			string arg0 = LuaScriptMgr.GetLuaString(L, 2);
			System.Reflection.BindingFlags arg1 = (System.Reflection.BindingFlags)LuaScriptMgr.GetNetObject(L, 3, typeof(System.Reflection.BindingFlags));
			Type o = obj.GetNestedType(arg0,arg1);
			LuaScriptMgr.Push(L, o);
			return 1;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: Type.GetNestedType");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetNestedTypes(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 1)
		{
			Type obj = LuaScriptMgr.GetTypeObject(L, 1);
			Type[] o = obj.GetNestedTypes();
			LuaScriptMgr.PushArray(L, o);
			return 1;
		}
		else if (count == 2)
		{
			Type obj = LuaScriptMgr.GetTypeObject(L, 1);
			System.Reflection.BindingFlags arg0 = (System.Reflection.BindingFlags)LuaScriptMgr.GetNetObject(L, 2, typeof(System.Reflection.BindingFlags));
			Type[] o = obj.GetNestedTypes(arg0);
			LuaScriptMgr.PushArray(L, o);
			return 1;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: Type.GetNestedTypes");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetProperties(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 1)
		{
			Type obj = LuaScriptMgr.GetTypeObject(L, 1);
			System.Reflection.PropertyInfo[] o = obj.GetProperties();
			LuaScriptMgr.PushArray(L, o);
			return 1;
		}
		else if (count == 2)
		{
			Type obj = LuaScriptMgr.GetTypeObject(L, 1);
			System.Reflection.BindingFlags arg0 = (System.Reflection.BindingFlags)LuaScriptMgr.GetNetObject(L, 2, typeof(System.Reflection.BindingFlags));
			System.Reflection.PropertyInfo[] o = obj.GetProperties(arg0);
			LuaScriptMgr.PushArray(L, o);
			return 1;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: Type.GetProperties");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetProperty(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 2)
		{
			Type obj = LuaScriptMgr.GetTypeObject(L, 1);
			string arg0 = LuaScriptMgr.GetLuaString(L, 2);
			System.Reflection.PropertyInfo o = obj.GetProperty(arg0);
			LuaScriptMgr.PushObject(L, o);
			return 1;
		}
		else if (count == 3 && LuaScriptMgr.CheckTypes(L, 1, typeof(Type), typeof(string), typeof(Type[])))
		{
			Type obj = LuaScriptMgr.GetTypeObject(L, 1);
			string arg0 = LuaScriptMgr.GetString(L, 2);
			Type[] objs1 = LuaScriptMgr.GetArrayObject<Type>(L, 3);
			System.Reflection.PropertyInfo o = obj.GetProperty(arg0,objs1);
			LuaScriptMgr.PushObject(L, o);
			return 1;
		}
		else if (count == 3 && LuaScriptMgr.CheckTypes(L, 1, typeof(Type), typeof(string), typeof(Type)))
		{
			Type obj = LuaScriptMgr.GetTypeObject(L, 1);
			string arg0 = LuaScriptMgr.GetString(L, 2);
			Type arg1 = LuaScriptMgr.GetTypeObject(L, 3);
			System.Reflection.PropertyInfo o = obj.GetProperty(arg0,arg1);
			LuaScriptMgr.PushObject(L, o);
			return 1;
		}
		else if (count == 3 && LuaScriptMgr.CheckTypes(L, 1, typeof(Type), typeof(string), typeof(System.Reflection.BindingFlags)))
		{
			Type obj = LuaScriptMgr.GetTypeObject(L, 1);
			string arg0 = LuaScriptMgr.GetString(L, 2);
			System.Reflection.BindingFlags arg1 = (System.Reflection.BindingFlags)LuaScriptMgr.GetLuaObject(L, 3);
			System.Reflection.PropertyInfo o = obj.GetProperty(arg0,arg1);
			LuaScriptMgr.PushObject(L, o);
			return 1;
		}
		else if (count == 4)
		{
			Type obj = LuaScriptMgr.GetTypeObject(L, 1);
			string arg0 = LuaScriptMgr.GetLuaString(L, 2);
			Type arg1 = LuaScriptMgr.GetTypeObject(L, 3);
			Type[] objs2 = LuaScriptMgr.GetArrayObject<Type>(L, 4);
			System.Reflection.PropertyInfo o = obj.GetProperty(arg0,arg1,objs2);
			LuaScriptMgr.PushObject(L, o);
			return 1;
		}
		else if (count == 5)
		{
			Type obj = LuaScriptMgr.GetTypeObject(L, 1);
			string arg0 = LuaScriptMgr.GetLuaString(L, 2);
			Type arg1 = LuaScriptMgr.GetTypeObject(L, 3);
			Type[] objs2 = LuaScriptMgr.GetArrayObject<Type>(L, 4);
			System.Reflection.ParameterModifier[] objs3 = LuaScriptMgr.GetArrayObject<System.Reflection.ParameterModifier>(L, 5);
			System.Reflection.PropertyInfo o = obj.GetProperty(arg0,arg1,objs2,objs3);
			LuaScriptMgr.PushObject(L, o);
			return 1;
		}
		else if (count == 7)
		{
			Type obj = LuaScriptMgr.GetTypeObject(L, 1);
			string arg0 = LuaScriptMgr.GetLuaString(L, 2);
			System.Reflection.BindingFlags arg1 = (System.Reflection.BindingFlags)LuaScriptMgr.GetNetObject(L, 3, typeof(System.Reflection.BindingFlags));
			System.Reflection.Binder arg2 = (System.Reflection.Binder)LuaScriptMgr.GetNetObject(L, 4, typeof(System.Reflection.Binder));
			Type arg3 = LuaScriptMgr.GetTypeObject(L, 5);
			Type[] objs4 = LuaScriptMgr.GetArrayObject<Type>(L, 6);
			System.Reflection.ParameterModifier[] objs5 = LuaScriptMgr.GetArrayObject<System.Reflection.ParameterModifier>(L, 7);
			System.Reflection.PropertyInfo o = obj.GetProperty(arg0,arg1,arg2,arg3,objs4,objs5);
			LuaScriptMgr.PushObject(L, o);
			return 1;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: Type.GetProperty");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetConstructor(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 2)
		{
			Type obj = LuaScriptMgr.GetTypeObject(L, 1);
			Type[] objs0 = LuaScriptMgr.GetArrayObject<Type>(L, 2);
			System.Reflection.ConstructorInfo o = obj.GetConstructor(objs0);
			LuaScriptMgr.PushObject(L, o);
			return 1;
		}
		else if (count == 5)
		{
			Type obj = LuaScriptMgr.GetTypeObject(L, 1);
			System.Reflection.BindingFlags arg0 = (System.Reflection.BindingFlags)LuaScriptMgr.GetNetObject(L, 2, typeof(System.Reflection.BindingFlags));
			System.Reflection.Binder arg1 = (System.Reflection.Binder)LuaScriptMgr.GetNetObject(L, 3, typeof(System.Reflection.Binder));
			Type[] objs2 = LuaScriptMgr.GetArrayObject<Type>(L, 4);
			System.Reflection.ParameterModifier[] objs3 = LuaScriptMgr.GetArrayObject<System.Reflection.ParameterModifier>(L, 5);
			System.Reflection.ConstructorInfo o = obj.GetConstructor(arg0,arg1,objs2,objs3);
			LuaScriptMgr.PushObject(L, o);
			return 1;
		}
		else if (count == 6)
		{
			Type obj = LuaScriptMgr.GetTypeObject(L, 1);
			System.Reflection.BindingFlags arg0 = (System.Reflection.BindingFlags)LuaScriptMgr.GetNetObject(L, 2, typeof(System.Reflection.BindingFlags));
			System.Reflection.Binder arg1 = (System.Reflection.Binder)LuaScriptMgr.GetNetObject(L, 3, typeof(System.Reflection.Binder));
			System.Reflection.CallingConventions arg2 = (System.Reflection.CallingConventions)LuaScriptMgr.GetNetObject(L, 4, typeof(System.Reflection.CallingConventions));
			Type[] objs3 = LuaScriptMgr.GetArrayObject<Type>(L, 5);
			System.Reflection.ParameterModifier[] objs4 = LuaScriptMgr.GetArrayObject<System.Reflection.ParameterModifier>(L, 6);
			System.Reflection.ConstructorInfo o = obj.GetConstructor(arg0,arg1,arg2,objs3,objs4);
			LuaScriptMgr.PushObject(L, o);
			return 1;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: Type.GetConstructor");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetConstructors(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 1)
		{
			Type obj = LuaScriptMgr.GetTypeObject(L, 1);
			System.Reflection.ConstructorInfo[] o = obj.GetConstructors();
			LuaScriptMgr.PushArray(L, o);
			return 1;
		}
		else if (count == 2)
		{
			Type obj = LuaScriptMgr.GetTypeObject(L, 1);
			System.Reflection.BindingFlags arg0 = (System.Reflection.BindingFlags)LuaScriptMgr.GetNetObject(L, 2, typeof(System.Reflection.BindingFlags));
			System.Reflection.ConstructorInfo[] o = obj.GetConstructors(arg0);
			LuaScriptMgr.PushArray(L, o);
			return 1;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: Type.GetConstructors");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetDefaultMembers(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		Type obj = LuaScriptMgr.GetTypeObject(L, 1);
		System.Reflection.MemberInfo[] o = obj.GetDefaultMembers();
		LuaScriptMgr.PushArray(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int FindMembers(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 5);
		Type obj = LuaScriptMgr.GetTypeObject(L, 1);
		System.Reflection.MemberTypes arg0 = (System.Reflection.MemberTypes)LuaScriptMgr.GetNetObject(L, 2, typeof(System.Reflection.MemberTypes));
		System.Reflection.BindingFlags arg1 = (System.Reflection.BindingFlags)LuaScriptMgr.GetNetObject(L, 3, typeof(System.Reflection.BindingFlags));
		System.Reflection.MemberFilter arg2 = null;
		LuaTypes funcType4 = LuaDLL.lua_type(L, 4);

		if (funcType4 != LuaTypes.LUA_TFUNCTION)
		{
			 arg2 = (System.Reflection.MemberFilter)LuaScriptMgr.GetNetObject(L, 4, typeof(System.Reflection.MemberFilter));
		}
		else
		{
			LuaFunction func = LuaScriptMgr.GetLuaFunction(L, 4);
			arg2 = (param0, param1) =>
			{
				int top = func.BeginPCall();
				LuaScriptMgr.PushObject(L, param0);
				LuaScriptMgr.PushVarObject(L, param1);
				func.PCall(top, 2);
				object[] objs = func.PopValues(top);
				func.EndPCall(top);
				return (bool)objs[0];
			};
		}

		object arg3 = LuaScriptMgr.GetVarObject(L, 5);
		System.Reflection.MemberInfo[] o = obj.FindMembers(arg0,arg1,arg2,arg3);
		LuaScriptMgr.PushArray(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int InvokeMember(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 6)
		{
			Type obj = LuaScriptMgr.GetTypeObject(L, 1);
			string arg0 = LuaScriptMgr.GetLuaString(L, 2);
			System.Reflection.BindingFlags arg1 = (System.Reflection.BindingFlags)LuaScriptMgr.GetNetObject(L, 3, typeof(System.Reflection.BindingFlags));
			System.Reflection.Binder arg2 = (System.Reflection.Binder)LuaScriptMgr.GetNetObject(L, 4, typeof(System.Reflection.Binder));
			object arg3 = LuaScriptMgr.GetVarObject(L, 5);
			object[] objs4 = LuaScriptMgr.GetArrayObject<object>(L, 6);
			object o = obj.InvokeMember(arg0,arg1,arg2,arg3,objs4);
			LuaScriptMgr.PushVarObject(L, o);
			return 1;
		}
		else if (count == 7)
		{
			Type obj = LuaScriptMgr.GetTypeObject(L, 1);
			string arg0 = LuaScriptMgr.GetLuaString(L, 2);
			System.Reflection.BindingFlags arg1 = (System.Reflection.BindingFlags)LuaScriptMgr.GetNetObject(L, 3, typeof(System.Reflection.BindingFlags));
			System.Reflection.Binder arg2 = (System.Reflection.Binder)LuaScriptMgr.GetNetObject(L, 4, typeof(System.Reflection.Binder));
			object arg3 = LuaScriptMgr.GetVarObject(L, 5);
			object[] objs4 = LuaScriptMgr.GetArrayObject<object>(L, 6);
			System.Globalization.CultureInfo arg5 = (System.Globalization.CultureInfo)LuaScriptMgr.GetNetObject(L, 7, typeof(System.Globalization.CultureInfo));
			object o = obj.InvokeMember(arg0,arg1,arg2,arg3,objs4,arg5);
			LuaScriptMgr.PushVarObject(L, o);
			return 1;
		}
		else if (count == 9)
		{
			Type obj = LuaScriptMgr.GetTypeObject(L, 1);
			string arg0 = LuaScriptMgr.GetLuaString(L, 2);
			System.Reflection.BindingFlags arg1 = (System.Reflection.BindingFlags)LuaScriptMgr.GetNetObject(L, 3, typeof(System.Reflection.BindingFlags));
			System.Reflection.Binder arg2 = (System.Reflection.Binder)LuaScriptMgr.GetNetObject(L, 4, typeof(System.Reflection.Binder));
			object arg3 = LuaScriptMgr.GetVarObject(L, 5);
			object[] objs4 = LuaScriptMgr.GetArrayObject<object>(L, 6);
			System.Reflection.ParameterModifier[] objs5 = LuaScriptMgr.GetArrayObject<System.Reflection.ParameterModifier>(L, 7);
			System.Globalization.CultureInfo arg6 = (System.Globalization.CultureInfo)LuaScriptMgr.GetNetObject(L, 8, typeof(System.Globalization.CultureInfo));
			string[] objs7 = LuaScriptMgr.GetArrayString(L, 9);
			object o = obj.InvokeMember(arg0,arg1,arg2,arg3,objs4,objs5,arg6,objs7);
			LuaScriptMgr.PushVarObject(L, o);
			return 1;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: Type.InvokeMember");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ToString(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		Type obj = LuaScriptMgr.GetTypeObject(L, 1);
		string o = obj.ToString();
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetGenericArguments(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		Type obj = LuaScriptMgr.GetTypeObject(L, 1);
		Type[] o = obj.GetGenericArguments();
		LuaScriptMgr.PushArray(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetGenericTypeDefinition(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		Type obj = LuaScriptMgr.GetTypeObject(L, 1);
		Type o = obj.GetGenericTypeDefinition();
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int MakeGenericType(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);
		Type obj = LuaScriptMgr.GetTypeObject(L, 1);
		Type[] objs0 = LuaScriptMgr.GetParamsObject<Type>(L, 2, count - 1);
		Type o = obj.MakeGenericType(objs0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetGenericParameterConstraints(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		Type obj = LuaScriptMgr.GetTypeObject(L, 1);
		Type[] o = obj.GetGenericParameterConstraints();
		LuaScriptMgr.PushArray(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int MakeArrayType(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 1)
		{
			Type obj = LuaScriptMgr.GetTypeObject(L, 1);
			Type o = obj.MakeArrayType();
			LuaScriptMgr.Push(L, o);
			return 1;
		}
		else if (count == 2)
		{
			Type obj = LuaScriptMgr.GetTypeObject(L, 1);
			int arg0 = (int)LuaScriptMgr.GetNumber(L, 2);
			Type o = obj.MakeArrayType(arg0);
			LuaScriptMgr.Push(L, o);
			return 1;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: Type.MakeArrayType");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int MakeByRefType(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		Type obj = LuaScriptMgr.GetTypeObject(L, 1);
		Type o = obj.MakeByRefType();
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int MakePointerType(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		Type obj = LuaScriptMgr.GetTypeObject(L, 1);
		Type o = obj.MakePointerType();
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ReflectionOnlyGetType(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 3);
		string arg0 = LuaScriptMgr.GetLuaString(L, 1);
		bool arg1 = LuaScriptMgr.GetBoolean(L, 2);
		bool arg2 = LuaScriptMgr.GetBoolean(L, 3);
		Type o = Type.ReflectionOnlyGetType(arg0,arg1,arg2);
		LuaScriptMgr.Push(L, o);
		return 1;
	}
}

