using System;
using System.Collections.Generic;
using Framework.Base;
using Framework.Managers;
using LuaInterface;
using msg;

public class BufferStateManager : IManager
{
    private EntitiesManager mEntitiesManager
    {
        get
        {
            return ManagerCenter.Instance.GetManager<EntitiesManager>();
        }
    }

    public void Init()
    {
        this.NetWork.Initialize();
        List<LuaTable> configTableList = LuaConfigManager.GetConfigTableList("charstate");
        for (int i = 0; i < configTableList.Count; i++)
        {
            LuaTable luaTable = configTableList[i];
            this.BufferStateList[(UserState)luaTable.GetField_Uint("id")] = luaTable;
        }
    }

    public LuaTable GetBufferConfig(UserState flag)
    {
        if (!this.BufferStateList.ContainsKey(flag))
        {
            FFDebug.LogWarning(this, "can not find key :" + flag);
            return null;
        }
        return this.BufferStateList[flag];
    }

    public void HandleSetBufferState(MSG_Ret_SetState_SC data)
    {
        this.mEntitiesManager.DoEentityActionOrCache(new EntitiesID(data.id, (CharactorType)data.type), delegate (CharactorBase Charactor)
        {
            if (data.state == null || data.state.Count < 1)
            {
                return;
            }
            for (int i = 0; i < data.state.Count; i++)
            {
                StateItem serverdata = data.state[i];
                BufferServerDate buffServerData = CommonTools.GetBuffServerData(serverdata);
                if (buffServerData.flag == UserState.USTATE_DAMRATE)
                {
                    return;
                }
                if (Charactor == null)
                {
                    return;
                }
                PlayerBufferControl component = Charactor.GetComponent<PlayerBufferControl>();
                if (component == null)
                {
                    return;
                }
                ControllerManager.Instance.GetController<MainUIController>().AddBuffLog(buffServerData, Charactor);
                component.UpdateState(buffServerData);
            }
        });
    }

    public void HandleClearBufferState(MSG_Ret_ClearState_SC data)
    {
        this.mEntitiesManager.DoEentityActionOrCache(new EntitiesID(data.id, (CharactorType)data.type), delegate (CharactorBase Charactor)
        {
            BufferServerDate buffServerData = CommonTools.GetBuffServerData(data.state);
            if (buffServerData.flag == UserState.USTATE_DAMRATE)
            {
                return;
            }
            if (Charactor == null)
            {
                return;
            }
            PlayerBufferControl component = Charactor.GetComponent<PlayerBufferControl>();
            if (component == null)
            {
                return;
            }
            component.RemoveState(buffServerData);
        });
    }

    public string ManagerName
    {
        get
        {
            return base.GetType().ToString();
        }
    }

    public void OnUpdate()
    {
    }

    public void OnReSet()
    {
    }

    private BufferNetWork NetWork = new BufferNetWork();

    public Dictionary<UserState, LuaTable> BufferStateList = new Dictionary<UserState, LuaTable>();
}
