using System;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine;
using LuaInterface;

public class CharactorBaseWrap
{
	public static void Register(IntPtr L)
	{
		LuaMethod[] regs = new LuaMethod[]
		{
			new LuaMethod("UpdateHatredList", UpdateHatredList),
			new LuaMethod("IsInHatredList", IsInHatredList),
			new LuaMethod("AddComponent", AddComponent),
			new LuaMethod("AddComponentImmediate", AddComponentImmediate),
			new LuaMethod("RemoveComponent", RemoveComponent),
			new LuaMethod("ResetAllCompment", ResetAllCompment),
			new LuaMethod("InitComponent", InitComponent),
			new LuaMethod("Init", Init),
			new LuaMethod("SetPlayerPosition", SetPlayerPosition),
			new LuaMethod("SetPlayerLookAt", SetPlayerLookAt),
			new LuaMethod("SetPlayerDirection", SetPlayerDirection),
			new LuaMethod("setPetPostionAndDir", setPetPostionAndDir),
			new LuaMethod("SetPlayerLastDirection", SetPlayerLastDirection),
			new LuaMethod("CharactorCheckMove", CharactorCheckMove),
			new LuaMethod("ForceSetCharBasePositionTo", ForceSetCharBasePositionTo),
			new LuaMethod("SetPhysicsPos", SetPhysicsPos),
			new LuaMethod("SetWorldPosition", SetWorldPosition),
			new LuaMethod("GetCharactorY", GetCharactorY),
			new LuaMethod("RefreshPhysicsPos", RefreshPhysicsPos),
			new LuaMethod("IsCanMove", IsCanMove),
			new LuaMethod("IsCanJump", IsCanJump),
			new LuaMethod("IsCanRotate", IsCanRotate),
			new LuaMethod("InCastSkillState", InCastSkillState),
			new LuaMethod("RecodeSeverMoveData", RecodeSeverMoveData),
			new LuaMethod("MoveDir", MoveDir),
			new LuaMethod("MoveTo", MoveTo),
			new LuaMethod("Moving", Moving),
			new LuaMethod("Jump", Jump),
			new LuaMethod("inJumpState", inJumpState),
			new LuaMethod("inWalkState", inWalkState),
			new LuaMethod("canJumpStateChange", canJumpStateChange),
			new LuaMethod("OnJumpLand", OnJumpLand),
			new LuaMethod("OnMainCameraChanged", OnMainCameraChanged),
			new LuaMethod("Rotatint", Rotatint),
			new LuaMethod("CheckNeedJump", CheckNeedJump),
			new LuaMethod("StopMoving", StopMoving),
			new LuaMethod("StopMoveImmediate", StopMoveImmediate),
			new LuaMethod("StopMoveImmediateWithOutSetPos", StopMoveImmediateWithOutSetPos),
			new LuaMethod("OnMoveStateChange", OnMoveStateChange),
			new LuaMethod("SetInputDir", SetInputDir),
			new LuaMethod("FastMoveTo", FastMoveTo),
			new LuaMethod("UpdateFastMove", UpdateFastMove),
			new LuaMethod("GetPosBySelf", GetPosBySelf),
			new LuaMethod("GetPosByTarget", GetPosByTarget),
			new LuaMethod("Die", Die),
			new LuaMethod("DelayRelive", DelayRelive),
			new LuaMethod("ClearEffect", ClearEffect),
			new LuaMethod("HandleHit", HandleHit),
			new LuaMethod("HitOther", HitOther),
			new LuaMethod("RevertHpMp", RevertHpMp),
			new LuaMethod("DestroyThisInNineScreen", DestroyThisInNineScreen),
			new LuaMethod("TargetSelect", TargetSelect),
			new LuaMethod("CancelSelect", CancelSelect),
			new LuaMethod("OnUpdateCharacterBuff", OnUpdateCharacterBuff),
			new LuaMethod("OnRemoveCharacterBuff", OnRemoveCharacterBuff),
			new LuaMethod("IsShowHitAnim", IsShowHitAnim),
			new LuaMethod("GetTempSelectTrans", GetTempSelectTrans),
			new LuaMethod("GetFeetSelectTrans", GetFeetSelectTrans),
			new LuaMethod("OnRelationChange", OnRelationChange),
			new LuaMethod("InitCameraFocusPos", InitCameraFocusPos),
			new LuaMethod("New", _CreateCharactorBase),
			new LuaMethod("GetClassType", GetClassType),
		};

		LuaField[] fields = new LuaField[]
		{
			new LuaField("EID", get_EID, set_EID),
			new LuaField("petBarCount", get_petBarCount, set_petBarCount),
			new LuaField("petBarUnlockcount", get_petBarUnlockcount, set_petBarUnlockcount),
			new LuaField("FightPet", get_FightPet, set_FightPet),
			new LuaField("AssistPet", get_AssistPet, set_AssistPet),
			new LuaField("hpdata", get_hpdata, set_hpdata),
			new LuaField("CharEvtMgr", get_CharEvtMgr, set_CharEvtMgr),
			new LuaField("HightLightControl", get_HightLightControl, set_HightLightControl),
			new LuaField("hatredList", get_hatredList, set_hatredList),
			new LuaField("rlationType", get_rlationType, set_rlationType),
			new LuaField("animator", get_animator, set_animator),
			new LuaField("ComponentMgr", get_ComponentMgr, set_ComponentMgr),
			new LuaField("isFalling", get_isFalling, set_isFalling),
			new LuaField("JumpHeight", get_JumpHeight, set_JumpHeight),
			new LuaField("FallCheckingHeight", get_FallCheckingHeight, set_FallCheckingHeight),
			new LuaField("CustomPosY", get_CustomPosY, set_CustomPosY),
			new LuaField("RetMoveDataQueue", get_RetMoveDataQueue, set_RetMoveDataQueue),
			new LuaField("NextJumPos", get_NextJumPos, set_NextJumPos),
			new LuaField("AStarPathDataQueue", get_AStarPathDataQueue, set_AStarPathDataQueue),
			new LuaField("OnAStarPathDataQueueDequeue", get_OnAStarPathDataQueueDequeue, set_OnAStarPathDataQueueDequeue),
			new LuaField("rotateSpeed", get_rotateSpeed, set_rotateSpeed),
			new LuaField("moveDir", get_moveDir, set_moveDir),
			new LuaField("TargetPos_", get_TargetPos_, set_TargetPos_),
			new LuaField("MinimunMoveUnit", get_MinimunMoveUnit, set_MinimunMoveUnit),
			new LuaField("OnStopMoveHandle", get_OnStopMoveHandle, set_OnStopMoveHandle),
			new LuaField("OnMoveDataChange", get_OnMoveDataChange, set_OnMoveDataChange),
			new LuaField("noJump", get_noJump, set_noJump),
			new LuaField("OnRestTime", get_OnRestTime, set_OnRestTime),
			new LuaField("OnFastMove", get_OnFastMove, set_OnFastMove),
			new LuaField("OnDestroyThisInNineScreen", get_OnDestroyThisInNineScreen, set_OnDestroyThisInNineScreen),
			new LuaField("CharState", get_CharState, set_CharState),
			new LuaField("Fbc", get_Fbc, set_Fbc),
			new LuaField("Pfc", get_Pfc, set_Pfc),
			new LuaField("ModelObj", get_ModelObj, set_ModelObj),
			new LuaField("animatorControllerName", get_animatorControllerName, null),
			new LuaField("BaseData", get_BaseData, null),
			new LuaField("IsMoving", get_IsMoving, null),
			new LuaField("IsCurFrameMoved", get_IsCurFrameMoved, null),
			new LuaField("IsBufferNoMove", get_IsBufferNoMove, null),
			new LuaField("IsFly", get_IsFly, null),
			new LuaField("IsFallChecking", get_IsFallChecking, null),
			new LuaField("IsBattleState", get_IsBattleState, null),
			new LuaField("CurrentPosition2D", get_CurrentPosition2D, null),
			new LuaField("NextPosition2D", get_NextPosition2D, set_NextPosition2D),
			new LuaField("moveSpeed", get_moveSpeed, set_moveSpeed),
			new LuaField("TargetPos", get_TargetPos, set_TargetPos),
			new LuaField("FaceDir", get_FaceDir, set_FaceDir),
			new LuaField("ServerDir", get_ServerDir, set_ServerDir),
			new LuaField("CurrMoveData", get_CurrMoveData, null),
			new LuaField("CurrServerPos", get_CurrServerPos, null),
			new LuaField("IsLive", get_IsLive, null),
			new LuaField("IsSoul", get_IsSoul, null),
			new LuaField("TopPos", get_TopPos, null),
			new LuaField("FeetPos", get_FeetPos, null),
			new LuaField("CameraPos", get_CameraPos, null),
		};

		LuaScriptMgr.RegisterLib(L, "CharactorBase", typeof(CharactorBase), regs, fields, typeof(object));
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateCharactorBase(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 0)
		{
			CharactorBase obj = new CharactorBase();
			LuaScriptMgr.PushObject(L, obj);
			return 1;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: CharactorBase.New");
		}

		return 0;
	}

	static Type classType = typeof(CharactorBase);

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetClassType(IntPtr L)
	{
		LuaScriptMgr.Push(L, classType);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_EID(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		CharactorBase obj = (CharactorBase)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name EID");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index EID on a nil value");
			}
		}

		LuaScriptMgr.PushValue(L, obj.EID);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_petBarCount(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		CharactorBase obj = (CharactorBase)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name petBarCount");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index petBarCount on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.petBarCount);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_petBarUnlockcount(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		CharactorBase obj = (CharactorBase)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name petBarUnlockcount");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index petBarUnlockcount on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.petBarUnlockcount);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_FightPet(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		CharactorBase obj = (CharactorBase)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name FightPet");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index FightPet on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.FightPet);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_AssistPet(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		CharactorBase obj = (CharactorBase)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name AssistPet");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index AssistPet on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.AssistPet);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_hpdata(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		CharactorBase obj = (CharactorBase)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name hpdata");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index hpdata on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.hpdata);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_CharEvtMgr(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		CharactorBase obj = (CharactorBase)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name CharEvtMgr");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index CharEvtMgr on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.CharEvtMgr);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_HightLightControl(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		CharactorBase obj = (CharactorBase)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name HightLightControl");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index HightLightControl on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.HightLightControl);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_hatredList(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		CharactorBase obj = (CharactorBase)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name hatredList");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index hatredList on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.hatredList);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_rlationType(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		CharactorBase obj = (CharactorBase)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name rlationType");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index rlationType on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.rlationType);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_animator(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		CharactorBase obj = (CharactorBase)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name animator");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index animator on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.animator);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_ComponentMgr(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		CharactorBase obj = (CharactorBase)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name ComponentMgr");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index ComponentMgr on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.ComponentMgr);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_isFalling(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		CharactorBase obj = (CharactorBase)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name isFalling");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index isFalling on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.isFalling);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_JumpHeight(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		CharactorBase obj = (CharactorBase)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name JumpHeight");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index JumpHeight on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.JumpHeight);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_FallCheckingHeight(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		CharactorBase obj = (CharactorBase)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name FallCheckingHeight");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index FallCheckingHeight on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.FallCheckingHeight);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_CustomPosY(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		CharactorBase obj = (CharactorBase)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name CustomPosY");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index CustomPosY on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.CustomPosY);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_RetMoveDataQueue(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		CharactorBase obj = (CharactorBase)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name RetMoveDataQueue");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index RetMoveDataQueue on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.RetMoveDataQueue);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_NextJumPos(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		CharactorBase obj = (CharactorBase)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name NextJumPos");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index NextJumPos on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.NextJumPos);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_AStarPathDataQueue(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		CharactorBase obj = (CharactorBase)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name AStarPathDataQueue");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index AStarPathDataQueue on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.AStarPathDataQueue);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_OnAStarPathDataQueueDequeue(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		CharactorBase obj = (CharactorBase)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name OnAStarPathDataQueueDequeue");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index OnAStarPathDataQueueDequeue on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.OnAStarPathDataQueueDequeue);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_rotateSpeed(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		CharactorBase obj = (CharactorBase)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name rotateSpeed");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index rotateSpeed on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.rotateSpeed);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_moveDir(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		CharactorBase obj = (CharactorBase)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name moveDir");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index moveDir on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.moveDir);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_TargetPos_(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		CharactorBase obj = (CharactorBase)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name TargetPos_");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index TargetPos_ on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.TargetPos_);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_MinimunMoveUnit(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		CharactorBase obj = (CharactorBase)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name MinimunMoveUnit");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index MinimunMoveUnit on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.MinimunMoveUnit);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_OnStopMoveHandle(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		CharactorBase obj = (CharactorBase)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name OnStopMoveHandle");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index OnStopMoveHandle on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.OnStopMoveHandle);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_OnMoveDataChange(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		CharactorBase obj = (CharactorBase)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name OnMoveDataChange");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index OnMoveDataChange on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.OnMoveDataChange);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_noJump(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		CharactorBase obj = (CharactorBase)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name noJump");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index noJump on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.noJump);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_OnRestTime(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		CharactorBase obj = (CharactorBase)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name OnRestTime");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index OnRestTime on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.OnRestTime);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_OnFastMove(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		CharactorBase obj = (CharactorBase)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name OnFastMove");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index OnFastMove on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.OnFastMove);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_OnDestroyThisInNineScreen(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		CharactorBase obj = (CharactorBase)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name OnDestroyThisInNineScreen");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index OnDestroyThisInNineScreen on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.OnDestroyThisInNineScreen);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_CharState(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		CharactorBase obj = (CharactorBase)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name CharState");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index CharState on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.CharState);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_Fbc(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		CharactorBase obj = (CharactorBase)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name Fbc");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index Fbc on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.Fbc);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_Pfc(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		CharactorBase obj = (CharactorBase)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name Pfc");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index Pfc on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.Pfc);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_ModelObj(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		CharactorBase obj = (CharactorBase)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name ModelObj");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index ModelObj on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.ModelObj);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_animatorControllerName(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		CharactorBase obj = (CharactorBase)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name animatorControllerName");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index animatorControllerName on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.animatorControllerName);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_BaseData(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		CharactorBase obj = (CharactorBase)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name BaseData");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index BaseData on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.BaseData);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_IsMoving(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		CharactorBase obj = (CharactorBase)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name IsMoving");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index IsMoving on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.IsMoving);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_IsCurFrameMoved(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		CharactorBase obj = (CharactorBase)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name IsCurFrameMoved");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index IsCurFrameMoved on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.IsCurFrameMoved);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_IsBufferNoMove(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		CharactorBase obj = (CharactorBase)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name IsBufferNoMove");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index IsBufferNoMove on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.IsBufferNoMove);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_IsFly(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		CharactorBase obj = (CharactorBase)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name IsFly");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index IsFly on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.IsFly);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_IsFallChecking(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		CharactorBase obj = (CharactorBase)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name IsFallChecking");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index IsFallChecking on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.IsFallChecking);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_IsBattleState(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		CharactorBase obj = (CharactorBase)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name IsBattleState");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index IsBattleState on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.IsBattleState);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_CurrentPosition2D(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		CharactorBase obj = (CharactorBase)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name CurrentPosition2D");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index CurrentPosition2D on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.CurrentPosition2D);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_NextPosition2D(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		CharactorBase obj = (CharactorBase)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name NextPosition2D");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index NextPosition2D on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.NextPosition2D);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_moveSpeed(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		CharactorBase obj = (CharactorBase)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name moveSpeed");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index moveSpeed on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.moveSpeed);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_TargetPos(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		CharactorBase obj = (CharactorBase)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name TargetPos");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index TargetPos on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.TargetPos);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_FaceDir(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		CharactorBase obj = (CharactorBase)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name FaceDir");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index FaceDir on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.FaceDir);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_ServerDir(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		CharactorBase obj = (CharactorBase)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name ServerDir");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index ServerDir on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.ServerDir);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_CurrMoveData(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		CharactorBase obj = (CharactorBase)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name CurrMoveData");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index CurrMoveData on a nil value");
			}
		}

		LuaScriptMgr.PushObject(L, obj.CurrMoveData);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_CurrServerPos(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		CharactorBase obj = (CharactorBase)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name CurrServerPos");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index CurrServerPos on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.CurrServerPos);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_IsLive(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		CharactorBase obj = (CharactorBase)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name IsLive");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index IsLive on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.IsLive);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_IsSoul(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		CharactorBase obj = (CharactorBase)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name IsSoul");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index IsSoul on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.IsSoul);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_TopPos(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		CharactorBase obj = (CharactorBase)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name TopPos");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index TopPos on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.TopPos);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_FeetPos(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		CharactorBase obj = (CharactorBase)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name FeetPos");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index FeetPos on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.FeetPos);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_CameraPos(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		CharactorBase obj = (CharactorBase)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name CameraPos");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index CameraPos on a nil value");
			}
		}

		LuaScriptMgr.Push(L, obj.CameraPos);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_EID(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		CharactorBase obj = (CharactorBase)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name EID");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index EID on a nil value");
			}
		}

		obj.EID = (EntitiesID)LuaScriptMgr.GetNetObject(L, 3, typeof(EntitiesID));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_petBarCount(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		CharactorBase obj = (CharactorBase)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name petBarCount");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index petBarCount on a nil value");
			}
		}

		obj.petBarCount = (uint)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_petBarUnlockcount(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		CharactorBase obj = (CharactorBase)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name petBarUnlockcount");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index petBarUnlockcount on a nil value");
			}
		}

		obj.petBarUnlockcount = (uint)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_FightPet(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		CharactorBase obj = (CharactorBase)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name FightPet");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index FightPet on a nil value");
			}
		}

		obj.FightPet = (Pet.PetBase)LuaScriptMgr.GetNetObject(L, 3, typeof(Pet.PetBase));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_AssistPet(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		CharactorBase obj = (CharactorBase)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name AssistPet");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index AssistPet on a nil value");
			}
		}

		obj.AssistPet = (Pet.PetBase)LuaScriptMgr.GetNetObject(L, 3, typeof(Pet.PetBase));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_hpdata(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		CharactorBase obj = (CharactorBase)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name hpdata");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index hpdata on a nil value");
			}
		}

		obj.hpdata = (HpStruct)LuaScriptMgr.GetNetObject(L, 3, typeof(HpStruct));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_CharEvtMgr(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		CharactorBase obj = (CharactorBase)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name CharEvtMgr");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index CharEvtMgr on a nil value");
			}
		}

		obj.CharEvtMgr = (CharacterEventMgr)LuaScriptMgr.GetNetObject(L, 3, typeof(CharacterEventMgr));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_HightLightControl(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		CharactorBase obj = (CharactorBase)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name HightLightControl");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index HightLightControl on a nil value");
			}
		}

		obj.HightLightControl = (HighlighterController)LuaScriptMgr.GetUnityObject(L, 3, typeof(HighlighterController));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_hatredList(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		CharactorBase obj = (CharactorBase)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name hatredList");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index hatredList on a nil value");
			}
		}

		obj.hatredList = (List<ulong>)LuaScriptMgr.GetNetObject(L, 3, typeof(List<ulong>));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_rlationType(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		CharactorBase obj = (CharactorBase)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name rlationType");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index rlationType on a nil value");
			}
		}

		obj.rlationType = (RelationType)LuaScriptMgr.GetNetObject(L, 3, typeof(RelationType));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_animator(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		CharactorBase obj = (CharactorBase)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name animator");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index animator on a nil value");
			}
		}

		obj.animator = (Animator)LuaScriptMgr.GetUnityObject(L, 3, typeof(Animator));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_ComponentMgr(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		CharactorBase obj = (CharactorBase)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name ComponentMgr");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index ComponentMgr on a nil value");
			}
		}

		obj.ComponentMgr = (FFComponentMgr)LuaScriptMgr.GetNetObject(L, 3, typeof(FFComponentMgr));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_isFalling(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		CharactorBase obj = (CharactorBase)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name isFalling");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index isFalling on a nil value");
			}
		}

		obj.isFalling = LuaScriptMgr.GetBoolean(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_JumpHeight(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		CharactorBase obj = (CharactorBase)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name JumpHeight");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index JumpHeight on a nil value");
			}
		}

		obj.JumpHeight = (float)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_FallCheckingHeight(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		CharactorBase obj = (CharactorBase)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name FallCheckingHeight");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index FallCheckingHeight on a nil value");
			}
		}

		obj.FallCheckingHeight = (float)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_CustomPosY(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		CharactorBase obj = (CharactorBase)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name CustomPosY");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index CustomPosY on a nil value");
			}
		}

		obj.CustomPosY = (float)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_RetMoveDataQueue(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		CharactorBase obj = (CharactorBase)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name RetMoveDataQueue");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index RetMoveDataQueue on a nil value");
			}
		}

		obj.RetMoveDataQueue = (Queue<cs_MoveData>)LuaScriptMgr.GetNetObject(L, 3, typeof(Queue<cs_MoveData>));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_NextJumPos(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		CharactorBase obj = (CharactorBase)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name NextJumPos");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index NextJumPos on a nil value");
			}
		}

		obj.NextJumPos = (cs_MoveData)LuaScriptMgr.GetNetObject(L, 3, typeof(cs_MoveData));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_AStarPathDataQueue(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		CharactorBase obj = (CharactorBase)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name AStarPathDataQueue");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index AStarPathDataQueue on a nil value");
			}
		}

		obj.AStarPathDataQueue = (Queue<cs_MoveData>)LuaScriptMgr.GetNetObject(L, 3, typeof(Queue<cs_MoveData>));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_OnAStarPathDataQueueDequeue(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		CharactorBase obj = (CharactorBase)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name OnAStarPathDataQueueDequeue");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index OnAStarPathDataQueueDequeue on a nil value");
			}
		}

		LuaTypes funcType = LuaDLL.lua_type(L, 3);

		if (funcType != LuaTypes.LUA_TFUNCTION)
		{
			obj.OnAStarPathDataQueueDequeue = (Action)LuaScriptMgr.GetNetObject(L, 3, typeof(Action));
		}
		else
		{
			LuaFunction func = LuaScriptMgr.ToLuaFunction(L, 3);
			obj.OnAStarPathDataQueueDequeue = () =>
			{
				func.Call();
			};
		}
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_rotateSpeed(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		CharactorBase obj = (CharactorBase)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name rotateSpeed");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index rotateSpeed on a nil value");
			}
		}

		obj.rotateSpeed = (float)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_moveDir(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		CharactorBase obj = (CharactorBase)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name moveDir");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index moveDir on a nil value");
			}
		}

		obj.moveDir = LuaScriptMgr.GetVector3(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_TargetPos_(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		CharactorBase obj = (CharactorBase)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name TargetPos_");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index TargetPos_ on a nil value");
			}
		}

		obj.TargetPos_ = LuaScriptMgr.GetVector3(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_MinimunMoveUnit(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		CharactorBase obj = (CharactorBase)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name MinimunMoveUnit");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index MinimunMoveUnit on a nil value");
			}
		}

		obj.MinimunMoveUnit = (float)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_OnStopMoveHandle(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		CharactorBase obj = (CharactorBase)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name OnStopMoveHandle");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index OnStopMoveHandle on a nil value");
			}
		}

		LuaTypes funcType = LuaDLL.lua_type(L, 3);

		if (funcType != LuaTypes.LUA_TFUNCTION)
		{
			obj.OnStopMoveHandle = (Action)LuaScriptMgr.GetNetObject(L, 3, typeof(Action));
		}
		else
		{
			LuaFunction func = LuaScriptMgr.ToLuaFunction(L, 3);
			obj.OnStopMoveHandle = () =>
			{
				func.Call();
			};
		}
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_OnMoveDataChange(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		CharactorBase obj = (CharactorBase)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name OnMoveDataChange");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index OnMoveDataChange on a nil value");
			}
		}

		LuaTypes funcType = LuaDLL.lua_type(L, 3);

		if (funcType != LuaTypes.LUA_TFUNCTION)
		{
			obj.OnMoveDataChange = (Action<CharactorBase>)LuaScriptMgr.GetNetObject(L, 3, typeof(Action<CharactorBase>));
		}
		else
		{
			LuaFunction func = LuaScriptMgr.ToLuaFunction(L, 3);
			obj.OnMoveDataChange = (param0) =>
			{
				int top = func.BeginPCall();
				LuaScriptMgr.PushObject(L, param0);
				func.PCall(top, 1);
				func.EndPCall(top);
			};
		}
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_noJump(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		CharactorBase obj = (CharactorBase)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name noJump");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index noJump on a nil value");
			}
		}

		obj.noJump = LuaScriptMgr.GetBoolean(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_OnRestTime(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		CharactorBase obj = (CharactorBase)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name OnRestTime");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index OnRestTime on a nil value");
			}
		}

		obj.OnRestTime = LuaScriptMgr.GetBoolean(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_OnFastMove(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		CharactorBase obj = (CharactorBase)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name OnFastMove");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index OnFastMove on a nil value");
			}
		}

		obj.OnFastMove = LuaScriptMgr.GetBoolean(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_OnDestroyThisInNineScreen(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		CharactorBase obj = (CharactorBase)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name OnDestroyThisInNineScreen");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index OnDestroyThisInNineScreen on a nil value");
			}
		}

		LuaTypes funcType = LuaDLL.lua_type(L, 3);

		if (funcType != LuaTypes.LUA_TFUNCTION)
		{
			obj.OnDestroyThisInNineScreen = (Action<CharactorBase>)LuaScriptMgr.GetNetObject(L, 3, typeof(Action<CharactorBase>));
		}
		else
		{
			LuaFunction func = LuaScriptMgr.ToLuaFunction(L, 3);
			obj.OnDestroyThisInNineScreen = (param0) =>
			{
				int top = func.BeginPCall();
				LuaScriptMgr.PushObject(L, param0);
				func.PCall(top, 1);
				func.EndPCall(top);
			};
		}
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_CharState(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		CharactorBase obj = (CharactorBase)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name CharState");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index CharState on a nil value");
			}
		}

		obj.CharState = (CharactorState)LuaScriptMgr.GetNetObject(L, 3, typeof(CharactorState));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_Fbc(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		CharactorBase obj = (CharactorBase)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name Fbc");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index Fbc on a nil value");
			}
		}

		obj.Fbc = (FFBehaviourControl)LuaScriptMgr.GetNetObject(L, 3, typeof(FFBehaviourControl));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_Pfc(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		CharactorBase obj = (CharactorBase)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name Pfc");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index Pfc on a nil value");
			}
		}

		obj.Pfc = (PathFindComponent)LuaScriptMgr.GetNetObject(L, 3, typeof(PathFindComponent));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_ModelObj(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		CharactorBase obj = (CharactorBase)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name ModelObj");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index ModelObj on a nil value");
			}
		}

		obj.ModelObj = (GameObject)LuaScriptMgr.GetUnityObject(L, 3, typeof(GameObject));
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_NextPosition2D(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		CharactorBase obj = (CharactorBase)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name NextPosition2D");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index NextPosition2D on a nil value");
			}
		}

		obj.NextPosition2D = LuaScriptMgr.GetVector2(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_moveSpeed(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		CharactorBase obj = (CharactorBase)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name moveSpeed");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index moveSpeed on a nil value");
			}
		}

		obj.moveSpeed = (float)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_TargetPos(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		CharactorBase obj = (CharactorBase)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name TargetPos");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index TargetPos on a nil value");
			}
		}

		obj.TargetPos = LuaScriptMgr.GetVector3(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_FaceDir(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		CharactorBase obj = (CharactorBase)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name FaceDir");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index FaceDir on a nil value");
			}
		}

		obj.FaceDir = (uint)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_ServerDir(IntPtr L)
	{
		object o = LuaScriptMgr.GetLuaObject(L, 1);
		CharactorBase obj = (CharactorBase)o;

		if (obj == null)
		{
			LuaTypes types = LuaDLL.lua_type(L, 1);

			if (types == LuaTypes.LUA_TTABLE)
			{
				LuaDLL.luaL_error(L, "unknown member name ServerDir");
			}
			else
			{
				LuaDLL.luaL_error(L, "attempt to index ServerDir on a nil value");
			}
		}

		obj.ServerDir = (uint)LuaScriptMgr.GetNumber(L, 3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int UpdateHatredList(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		CharactorBase obj = (CharactorBase)LuaScriptMgr.GetNetObjectSelf(L, 1, "CharactorBase");
		List<ulong> arg0 = (List<ulong>)LuaScriptMgr.GetNetObject(L, 2, typeof(List<ulong>));
		obj.UpdateHatredList(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int IsInHatredList(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		CharactorBase obj = (CharactorBase)LuaScriptMgr.GetNetObjectSelf(L, 1, "CharactorBase");
		ulong arg0 = (ulong)LuaScriptMgr.GetNumber(L, 2);
		bool o = obj.IsInHatredList(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int AddComponent(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		CharactorBase obj = (CharactorBase)LuaScriptMgr.GetNetObjectSelf(L, 1, "CharactorBase");
		IFFComponent arg0 = (IFFComponent)LuaScriptMgr.GetNetObject(L, 2, typeof(IFFComponent));
		obj.AddComponent(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int AddComponentImmediate(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		CharactorBase obj = (CharactorBase)LuaScriptMgr.GetNetObjectSelf(L, 1, "CharactorBase");
		IFFComponent arg0 = (IFFComponent)LuaScriptMgr.GetNetObject(L, 2, typeof(IFFComponent));
		obj.AddComponentImmediate(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int RemoveComponent(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		CharactorBase obj = (CharactorBase)LuaScriptMgr.GetNetObjectSelf(L, 1, "CharactorBase");
		IFFComponent arg0 = (IFFComponent)LuaScriptMgr.GetNetObject(L, 2, typeof(IFFComponent));
		obj.RemoveComponent(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ResetAllCompment(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		CharactorBase obj = (CharactorBase)LuaScriptMgr.GetNetObjectSelf(L, 1, "CharactorBase");
		obj.ResetAllCompment();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int InitComponent(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		CharactorBase obj = (CharactorBase)LuaScriptMgr.GetNetObjectSelf(L, 1, "CharactorBase");
		obj.InitComponent();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Init(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		CharactorBase obj = (CharactorBase)LuaScriptMgr.GetNetObjectSelf(L, 1, "CharactorBase");
		obj.Init();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetPlayerPosition(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 2)
		{
			CharactorBase obj = (CharactorBase)LuaScriptMgr.GetNetObjectSelf(L, 1, "CharactorBase");
			cs_FloatMovePos arg0 = (cs_FloatMovePos)LuaScriptMgr.GetNetObject(L, 2, typeof(cs_FloatMovePos));
			obj.SetPlayerPosition(arg0);
			return 0;
		}
		else if (count == 3 && LuaScriptMgr.CheckTypes(L, 1, typeof(CharactorBase), typeof(LuaTable), typeof(uint)))
		{
			CharactorBase obj = (CharactorBase)LuaScriptMgr.GetNetObjectSelf(L, 1, "CharactorBase");
			Vector2 arg0 = LuaScriptMgr.GetVector2(L, 2);
			uint arg1 = (uint)LuaDLL.lua_tonumber(L, 3);
			obj.SetPlayerPosition(arg0,arg1);
			return 0;
		}
		else if (count == 3 && LuaScriptMgr.CheckTypes(L, 1, typeof(CharactorBase), typeof(cs_FloatMovePos), typeof(uint)))
		{
			CharactorBase obj = (CharactorBase)LuaScriptMgr.GetNetObjectSelf(L, 1, "CharactorBase");
			cs_FloatMovePos arg0 = (cs_FloatMovePos)LuaScriptMgr.GetLuaObject(L, 2);
			uint arg1 = (uint)LuaDLL.lua_tonumber(L, 3);
			obj.SetPlayerPosition(arg0,arg1);
			return 0;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: CharactorBase.SetPlayerPosition");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetPlayerLookAt(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 3);
		CharactorBase obj = (CharactorBase)LuaScriptMgr.GetNetObjectSelf(L, 1, "CharactorBase");
		Vector3 arg0 = LuaScriptMgr.GetVector3(L, 2);
		bool arg1 = LuaScriptMgr.GetBoolean(L, 3);
		obj.SetPlayerLookAt(arg0,arg1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetPlayerDirection(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 3);
		CharactorBase obj = (CharactorBase)LuaScriptMgr.GetNetObjectSelf(L, 1, "CharactorBase");
		uint arg0 = (uint)LuaScriptMgr.GetNumber(L, 2);
		bool arg1 = LuaScriptMgr.GetBoolean(L, 3);
		obj.SetPlayerDirection(arg0,arg1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int setPetPostionAndDir(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 3);
		CharactorBase obj = (CharactorBase)LuaScriptMgr.GetNetObjectSelf(L, 1, "CharactorBase");
		Vector2 arg0 = LuaScriptMgr.GetVector2(L, 2);
		uint arg1 = (uint)LuaScriptMgr.GetNumber(L, 3);
		obj.setPetPostionAndDir(arg0,arg1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetPlayerLastDirection(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		CharactorBase obj = (CharactorBase)LuaScriptMgr.GetNetObjectSelf(L, 1, "CharactorBase");
		obj.SetPlayerLastDirection();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int CharactorCheckMove(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		CharactorBase obj = (CharactorBase)LuaScriptMgr.GetNetObjectSelf(L, 1, "CharactorBase");
		Vector2 arg0 = LuaScriptMgr.GetVector2(L, 2);
		bool o = obj.CharactorCheckMove(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ForceSetCharBasePositionTo(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		CharactorBase obj = (CharactorBase)LuaScriptMgr.GetNetObjectSelf(L, 1, "CharactorBase");
		Vector2 arg0 = LuaScriptMgr.GetVector2(L, 2);
		obj.ForceSetCharBasePositionTo(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetPhysicsPos(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 2 && LuaScriptMgr.CheckTypes(L, 1, typeof(CharactorBase), typeof(LuaTable)))
		{
			CharactorBase obj = (CharactorBase)LuaScriptMgr.GetNetObjectSelf(L, 1, "CharactorBase");
			Vector2 arg0 = LuaScriptMgr.GetVector2(L, 2);
			obj.SetPhysicsPos(arg0);
			return 0;
		}
		else if (count == 2 && LuaScriptMgr.CheckTypes(L, 1, typeof(CharactorBase), typeof(LuaTable)))
		{
			CharactorBase obj = (CharactorBase)LuaScriptMgr.GetNetObjectSelf(L, 1, "CharactorBase");
			Vector3 arg0 = LuaScriptMgr.GetVector3(L, 2);
			obj.SetPhysicsPos(arg0);
			return 0;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: CharactorBase.SetPhysicsPos");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetWorldPosition(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		CharactorBase obj = (CharactorBase)LuaScriptMgr.GetNetObjectSelf(L, 1, "CharactorBase");
		Vector3 arg0 = LuaScriptMgr.GetVector3(L, 2);
		obj.SetWorldPosition(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetCharactorY(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 2 && LuaScriptMgr.CheckTypes(L, 1, typeof(CharactorBase), typeof(LuaTable)))
		{
			CharactorBase obj = (CharactorBase)LuaScriptMgr.GetNetObjectSelf(L, 1, "CharactorBase");
			Vector3 arg0 = LuaScriptMgr.GetVector3(L, 2);
			Vector3 o = obj.GetCharactorY(arg0);
			LuaScriptMgr.Push(L, o);
			return 1;
		}
		else if (count == 2 && LuaScriptMgr.CheckTypes(L, 1, typeof(CharactorBase), typeof(LuaTable)))
		{
			CharactorBase obj = (CharactorBase)LuaScriptMgr.GetNetObjectSelf(L, 1, "CharactorBase");
			Vector2 arg0 = LuaScriptMgr.GetVector2(L, 2);
			float o = obj.GetCharactorY(arg0);
			LuaScriptMgr.Push(L, o);
			return 1;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: CharactorBase.GetCharactorY");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int RefreshPhysicsPos(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		CharactorBase obj = (CharactorBase)LuaScriptMgr.GetNetObjectSelf(L, 1, "CharactorBase");
		obj.RefreshPhysicsPos();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int IsCanMove(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		CharactorBase obj = (CharactorBase)LuaScriptMgr.GetNetObjectSelf(L, 1, "CharactorBase");
		bool o = obj.IsCanMove();
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int IsCanJump(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		CharactorBase obj = (CharactorBase)LuaScriptMgr.GetNetObjectSelf(L, 1, "CharactorBase");
		bool o = obj.IsCanJump();
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int IsCanRotate(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		CharactorBase obj = (CharactorBase)LuaScriptMgr.GetNetObjectSelf(L, 1, "CharactorBase");
		bool o = obj.IsCanRotate();
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int InCastSkillState(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		CharactorBase obj = (CharactorBase)LuaScriptMgr.GetNetObjectSelf(L, 1, "CharactorBase");
		bool o = obj.InCastSkillState();
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int RecodeSeverMoveData(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		CharactorBase obj = (CharactorBase)LuaScriptMgr.GetNetObjectSelf(L, 1, "CharactorBase");
		cs_MoveData arg0 = (cs_MoveData)LuaScriptMgr.GetNetObject(L, 2, typeof(cs_MoveData));
		obj.RecodeSeverMoveData(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int MoveDir(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		CharactorBase obj = (CharactorBase)LuaScriptMgr.GetNetObjectSelf(L, 1, "CharactorBase");
		cs_MoveData arg0 = (cs_MoveData)LuaScriptMgr.GetNetObject(L, 2, typeof(cs_MoveData));
		obj.MoveDir(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int MoveTo(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 2 && LuaScriptMgr.CheckTypes(L, 1, typeof(CharactorBase), typeof(LuaTable)))
		{
			CharactorBase obj = (CharactorBase)LuaScriptMgr.GetNetObjectSelf(L, 1, "CharactorBase");
			Vector2 arg0 = LuaScriptMgr.GetVector2(L, 2);
			obj.MoveTo(arg0);
			return 0;
		}
		else if (count == 2 && LuaScriptMgr.CheckTypes(L, 1, typeof(CharactorBase), typeof(cs_MoveData)))
		{
			CharactorBase obj = (CharactorBase)LuaScriptMgr.GetNetObjectSelf(L, 1, "CharactorBase");
			cs_MoveData arg0 = (cs_MoveData)LuaScriptMgr.GetLuaObject(L, 2);
			obj.MoveTo(arg0);
			return 0;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: CharactorBase.MoveTo");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Moving(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		CharactorBase obj = (CharactorBase)LuaScriptMgr.GetNetObjectSelf(L, 1, "CharactorBase");
		obj.Moving();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Jump(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 3);
		CharactorBase obj = (CharactorBase)LuaScriptMgr.GetNetObjectSelf(L, 1, "CharactorBase");
		bool arg0 = LuaScriptMgr.GetBoolean(L, 2);
		bool arg1 = LuaScriptMgr.GetBoolean(L, 3);
		obj.Jump(arg0,arg1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int inJumpState(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		CharactorBase obj = (CharactorBase)LuaScriptMgr.GetNetObjectSelf(L, 1, "CharactorBase");
		bool o = obj.inJumpState();
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int inWalkState(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		CharactorBase obj = (CharactorBase)LuaScriptMgr.GetNetObjectSelf(L, 1, "CharactorBase");
		bool o = obj.inWalkState();
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int canJumpStateChange(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		CharactorBase obj = (CharactorBase)LuaScriptMgr.GetNetObjectSelf(L, 1, "CharactorBase");
		bool o = obj.canJumpStateChange();
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int OnJumpLand(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		CharactorBase obj = (CharactorBase)LuaScriptMgr.GetNetObjectSelf(L, 1, "CharactorBase");
		bool o = obj.OnJumpLand();
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int OnMainCameraChanged(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		CharactorBase obj = (CharactorBase)LuaScriptMgr.GetNetObjectSelf(L, 1, "CharactorBase");
		obj.OnMainCameraChanged();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Rotatint(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		CharactorBase obj = (CharactorBase)LuaScriptMgr.GetNetObjectSelf(L, 1, "CharactorBase");
		obj.Rotatint();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int CheckNeedJump(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		CharactorBase obj = (CharactorBase)LuaScriptMgr.GetNetObjectSelf(L, 1, "CharactorBase");
		Vector2 arg0 = LuaScriptMgr.GetVector2(L, 2);
		obj.CheckNeedJump(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int StopMoving(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		CharactorBase obj = (CharactorBase)LuaScriptMgr.GetNetObjectSelf(L, 1, "CharactorBase");
		Action arg0 = null;
		LuaTypes funcType2 = LuaDLL.lua_type(L, 2);

		if (funcType2 != LuaTypes.LUA_TFUNCTION)
		{
			 arg0 = (Action)LuaScriptMgr.GetNetObject(L, 2, typeof(Action));
		}
		else
		{
			LuaFunction func = LuaScriptMgr.GetLuaFunction(L, 2);
			arg0 = () =>
			{
				func.Call();
			};
		}

		obj.StopMoving(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int StopMoveImmediate(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		CharactorBase obj = (CharactorBase)LuaScriptMgr.GetNetObjectSelf(L, 1, "CharactorBase");
		Action arg0 = null;
		LuaTypes funcType2 = LuaDLL.lua_type(L, 2);

		if (funcType2 != LuaTypes.LUA_TFUNCTION)
		{
			 arg0 = (Action)LuaScriptMgr.GetNetObject(L, 2, typeof(Action));
		}
		else
		{
			LuaFunction func = LuaScriptMgr.GetLuaFunction(L, 2);
			arg0 = () =>
			{
				func.Call();
			};
		}

		obj.StopMoveImmediate(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int StopMoveImmediateWithOutSetPos(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		CharactorBase obj = (CharactorBase)LuaScriptMgr.GetNetObjectSelf(L, 1, "CharactorBase");
		Action arg0 = null;
		LuaTypes funcType2 = LuaDLL.lua_type(L, 2);

		if (funcType2 != LuaTypes.LUA_TFUNCTION)
		{
			 arg0 = (Action)LuaScriptMgr.GetNetObject(L, 2, typeof(Action));
		}
		else
		{
			LuaFunction func = LuaScriptMgr.GetLuaFunction(L, 2);
			arg0 = () =>
			{
				func.Call();
			};
		}

		obj.StopMoveImmediateWithOutSetPos(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int OnMoveStateChange(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		CharactorBase obj = (CharactorBase)LuaScriptMgr.GetNetObjectSelf(L, 1, "CharactorBase");
		bool arg0 = LuaScriptMgr.GetBoolean(L, 2);
		obj.OnMoveStateChange(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetInputDir(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		CharactorBase obj = (CharactorBase)LuaScriptMgr.GetNetObjectSelf(L, 1, "CharactorBase");
		int arg0 = (int)LuaScriptMgr.GetNumber(L, 2);
		obj.SetInputDir(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int FastMoveTo(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 5);
		CharactorBase obj = (CharactorBase)LuaScriptMgr.GetNetObjectSelf(L, 1, "CharactorBase");
		Vector2 arg0 = LuaScriptMgr.GetVector2(L, 2);
		float arg1 = (float)LuaScriptMgr.GetNumber(L, 3);
		float arg2 = (float)LuaScriptMgr.GetNumber(L, 4);
		float arg3 = (float)LuaScriptMgr.GetNumber(L, 5);
		obj.FastMoveTo(arg0,arg1,arg2,arg3);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int UpdateFastMove(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		CharactorBase obj = (CharactorBase)LuaScriptMgr.GetNetObjectSelf(L, 1, "CharactorBase");
		obj.UpdateFastMove();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetPosBySelf(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 3);
		CharactorBase obj = (CharactorBase)LuaScriptMgr.GetNetObjectSelf(L, 1, "CharactorBase");
		float arg0 = (float)LuaScriptMgr.GetNumber(L, 2);
		float arg1 = (float)LuaScriptMgr.GetNumber(L, 3);
		Vector2 o = obj.GetPosBySelf(arg0,arg1);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetPosByTarget(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 3);
		CharactorBase obj = (CharactorBase)LuaScriptMgr.GetNetObjectSelf(L, 1, "CharactorBase");
		Vector2 arg0 = LuaScriptMgr.GetVector2(L, 2);
		float arg1 = (float)LuaScriptMgr.GetNumber(L, 3);
		Vector2 o = obj.GetPosByTarget(arg0,arg1);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Die(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		CharactorBase obj = (CharactorBase)LuaScriptMgr.GetNetObjectSelf(L, 1, "CharactorBase");
		obj.Die();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int DelayRelive(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		CharactorBase obj = (CharactorBase)LuaScriptMgr.GetNetObjectSelf(L, 1, "CharactorBase");
		obj.DelayRelive();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ClearEffect(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		CharactorBase obj = (CharactorBase)LuaScriptMgr.GetNetObjectSelf(L, 1, "CharactorBase");
		obj.ClearEffect();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int HandleHit(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		CharactorBase obj = (CharactorBase)LuaScriptMgr.GetNetObjectSelf(L, 1, "CharactorBase");
		magic.MSG_Ret_MagicAttack_SC arg0 = (magic.MSG_Ret_MagicAttack_SC)LuaScriptMgr.GetNetObject(L, 2, typeof(magic.MSG_Ret_MagicAttack_SC));
		obj.HandleHit(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int HitOther(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 3);
		CharactorBase obj = (CharactorBase)LuaScriptMgr.GetNetObjectSelf(L, 1, "CharactorBase");
		magic.MSG_Ret_MagicAttack_SC arg0 = (magic.MSG_Ret_MagicAttack_SC)LuaScriptMgr.GetNetObject(L, 2, typeof(magic.MSG_Ret_MagicAttack_SC));
		CharactorBase[] objs1 = LuaScriptMgr.GetArrayObject<CharactorBase>(L, 3);
		obj.HitOther(arg0,objs1);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int RevertHpMp(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		CharactorBase obj = (CharactorBase)LuaScriptMgr.GetNetObjectSelf(L, 1, "CharactorBase");
		magic.MSG_Ret_HpMpPop_SC arg0 = (magic.MSG_Ret_HpMpPop_SC)LuaScriptMgr.GetNetObject(L, 2, typeof(magic.MSG_Ret_HpMpPop_SC));
		obj.RevertHpMp(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int DestroyThisInNineScreen(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		CharactorBase obj = (CharactorBase)LuaScriptMgr.GetNetObjectSelf(L, 1, "CharactorBase");
		obj.DestroyThisInNineScreen();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int TargetSelect(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		CharactorBase obj = (CharactorBase)LuaScriptMgr.GetNetObjectSelf(L, 1, "CharactorBase");
		obj.TargetSelect();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int CancelSelect(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		CharactorBase obj = (CharactorBase)LuaScriptMgr.GetNetObjectSelf(L, 1, "CharactorBase");
		obj.CancelSelect();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int OnUpdateCharacterBuff(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		CharactorBase obj = (CharactorBase)LuaScriptMgr.GetNetObjectSelf(L, 1, "CharactorBase");
		UserState arg0 = (UserState)LuaScriptMgr.GetNetObject(L, 2, typeof(UserState));
		obj.OnUpdateCharacterBuff(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int OnRemoveCharacterBuff(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		CharactorBase obj = (CharactorBase)LuaScriptMgr.GetNetObjectSelf(L, 1, "CharactorBase");
		UserState arg0 = (UserState)LuaScriptMgr.GetNetObject(L, 2, typeof(UserState));
		obj.OnRemoveCharacterBuff(arg0);
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int IsShowHitAnim(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 2);
		CharactorBase obj = (CharactorBase)LuaScriptMgr.GetNetObjectSelf(L, 1, "CharactorBase");
		int arg0 = (int)LuaScriptMgr.GetNumber(L, 2);
		bool o = obj.IsShowHitAnim(arg0);
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetTempSelectTrans(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		CharactorBase obj = (CharactorBase)LuaScriptMgr.GetNetObjectSelf(L, 1, "CharactorBase");
		Transform o = obj.GetTempSelectTrans();
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetFeetSelectTrans(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		CharactorBase obj = (CharactorBase)LuaScriptMgr.GetNetObjectSelf(L, 1, "CharactorBase");
		Transform o = obj.GetFeetSelectTrans();
		LuaScriptMgr.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int OnRelationChange(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		CharactorBase obj = (CharactorBase)LuaScriptMgr.GetNetObjectSelf(L, 1, "CharactorBase");
		obj.OnRelationChange();
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int InitCameraFocusPos(IntPtr L)
	{
		LuaScriptMgr.CheckArgsCount(L, 1);
		CharactorBase obj = (CharactorBase)LuaScriptMgr.GetNetObjectSelf(L, 1, "CharactorBase");
		obj.InitCameraFocusPos();
		return 0;
	}
}

