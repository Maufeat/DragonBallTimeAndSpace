using System;
using System.Collections.Generic;

public class BufferStateFactory
{
    private static Dictionary<UserState, BufferStateFactory.BufferStateCreater> BufferStateRegisterMap
    {
        get
        {
            if (BufferStateFactory._bufferStateRegisterMap == null)
            {
                BufferStateFactory.RegisterBufferState();
            }
            return BufferStateFactory._bufferStateRegisterMap;
        }
    }

    private static void RegisterBufferState()
    {
        BufferStateFactory._bufferStateRegisterMap = new Dictionary<UserState, BufferStateFactory.BufferStateCreater>();
        BufferStateFactory._bufferStateRegisterMap[UserState.USTATE_HOLD_DONE] = ((UserState flag) => new BufferStateOccupy(flag));
        BufferStateFactory._bufferStateRegisterMap[UserState.USTATE_HOLD_TRANSFORM] = ((UserState flag) => new BufferStateOccupy(flag));
        BufferStateFactory._bufferStateRegisterMap[UserState.USTATE_BATTLE] = ((UserState flag) => new BufferStateBattle(flag));
        BufferStateFactory._bufferStateRegisterMap[UserState.USTATE_DOGWATCHED] = ((UserState flag) => new BufferStateDogWatched(flag));
        BufferStateFactory._bufferStateRegisterMap[UserState.USTATE_RUN] = ((UserState flag) => new BufferStateRun(flag));
        BufferStateFactory._bufferStateRegisterMap[UserState.USTATE_PEACE] = ((UserState flag) => new BufferStatePeace(flag));
        BufferStateFactory._bufferStateRegisterMap[UserState.USTATE_HIDE_ME] = ((UserState flag) => new BufferStateInvisibility(flag));
        BufferStateFactory._bufferStateRegisterMap[UserState.USTATE_WEAK] = ((UserState flag) => new BufferStateDrinkBlood(flag));
        BufferStateFactory._bufferStateRegisterMap[UserState.USTATE_QUEST_DOING] = ((UserState flag) => new BufferStateBeGather(flag));
        BufferStateFactory._bufferStateRegisterMap[UserState.USTATE_DEATH] = ((UserState flag) => new BufferStateDie(flag));
        BufferStateFactory._bufferStateRegisterMap[UserState.USTATE_DOOR_OPEN] = ((UserState flag) => new BufferStateDoorOpen(flag));
        BufferStateFactory._bufferStateRegisterMap[UserState.USTATE_QUESTBAR] = ((UserState flag) => new BufferStateQusetBar(flag));
        BufferStateFactory._bufferStateRegisterMap[UserState.USTATE_EXP_DAY_FULL] = ((UserState flag) => new BufferStateExpDayFull(flag));
        BufferStateFactory._bufferStateRegisterMap[UserState.USTATE_FLY] = ((UserState flag) => new BufferStateFly(flag));
        BufferStateFactory._bufferStateRegisterMap[UserState.USTATE_YMC_FAKEDEATH_STATE] = ((UserState flag) => new BufferStateFakeDie(flag));
        BufferStateFactory._bufferStateRegisterMap[UserState.USTATE_PEACE_BACK] = ((UserState flag) => new BufferStateRegression(flag));
        BufferStateFactory._bufferStateRegisterMap[UserState.USTATE_FLY_FLOWER] = ((UserState flag) => new BufferStateFlyFlower(flag));
        BufferStateFactory._bufferStateRegisterMap[UserState.USTATE_TURTLE_SHEILD] = ((UserState flag) => new BufferStateTurtleShellTaskRun(flag));
        BufferStateFactory._bufferStateRegisterMap[UserState.USTATE_HAVE_TURTLE_SHELL] = ((UserState flag) => new BufferStateTurtleShell(flag));
        BufferStateFactory._bufferStateRegisterMap[UserState.USTATE_AREA_CIRCLE] = ((UserState flag) => new BufferStateAreaCircle(flag));
        BufferStateFactory._bufferStateRegisterMap[UserState.USTATE_BELL_QTE] = ((UserState flag) => new BufferStateBellQTE(flag));
        BufferStateFactory._bufferStateRegisterMap[UserState.USTATE_VISIT_NPC_HOLDING] = ((UserState flag) => new BufferStateNPCHolding(flag));
        BufferStateFactory._bufferStateRegisterMap[UserState.USTATE_DEEP_THINK] = ((UserState flag) => new BufferStateDeepThink(flag));
        BufferStateFactory._bufferStateRegisterMap[UserState.USTATE_BE_STONE] = ((UserState flag) => new BufferStateBeStone(flag));
        BufferStateFactory._bufferStateRegisterMap[UserState.USTATE_HOLLOW_MAN] = ((UserState flag) => new BufferStateHollowMan(flag));
        BufferStateFactory._bufferStateRegisterMap[UserState.USTATE_STONE_FALL] = ((UserState flag) => new BufferStateStoneFall(flag));
        BufferStateFactory._bufferStateRegisterMap[UserState.USTATE_EVIL_TOILET] = ((UserState flag) => new BufferStateEvilToliet(flag));
        BufferStateFactory._bufferStateRegisterMap[UserState.USTATE_FAMILY_SHEILD_COLOR] = ((UserState flag) => new BufferStateGuildSheild(flag));
        BufferStateFactory.RegisterBuffer_Escape();
    }

    public static BufferState ProduceBufferState(UserState Flag)
    {
        if (BufferStateFactory.BufferStateRegisterMap.ContainsKey(Flag))
        {
            return BufferStateFactory.BufferStateRegisterMap[Flag](Flag);
        }
        return new BufferState(Flag);
    }

    private static void RegisterBuffer_Escape()
    {
        string cacheField_String = LuaConfigManager.GetXmlConfigTable("careerskill").GetCacheField_Table("struggletimes").GetCacheField_String("stateid");
        if (string.IsNullOrEmpty(cacheField_String))
        {
            FFDebug.LogWarning("PlayerBufferControl", "Buffer Escape cant find right data!!! please check carerskill.xml struggletimes ");
            return;
        }
        string[] array = cacheField_String.Split(new char[]
        {
            '-'
        }, StringSplitOptions.RemoveEmptyEntries);
        if (array == null || array.Length < 1)
        {
            FFDebug.LogWarning("PlayerBufferControl", "Buffer Escape escape data field is null !!! please check carerskill.xml struggletimes ");
            return;
        }
        for (int i = 0; i < array.Length; i++)
        {
            UserState key = (UserState)int.Parse(array[i]);
            BufferStateFactory._bufferStateRegisterMap[key] = ((UserState flag) => new BufferStateEscape(flag));
        }
    }

    private static Dictionary<UserState, BufferStateFactory.BufferStateCreater> _bufferStateRegisterMap;

    private delegate BufferState BufferStateCreater(UserState Flag);
}
