using System;
using System.Collections.Generic;
using Framework.Managers;
using LuaInterface;
using Models;
using npc;

public class PickDropController : ControllerBase
{
    public override void Awake()
    {
        this.pickDropNetWork = new PickDropNetWork();
        this.pickDropNetWork.Initialize();
    }

    public override string ControllerName
    {
        get
        {
            return "pickdrop";
        }
    }

    public UI_PickDrop ui_PickDrop
    {
        get
        {
            return UIManager.GetUIObject<UI_PickDrop>();
        }
    }

    public void Dispose()
    {
    }

    public void MSG_ReqWatchSceneBag_CS(Npc npc, Action<ulong> endAction = null)
    {
        this.curNpc = npc;
        npc.OnDestroyThisInNineScreen = (Action<CharactorBase>)Delegate.Combine(npc.OnDestroyThisInNineScreen, new Action<CharactorBase>(delegate (CharactorBase n)
        {
            if (this.ui_PickDrop != null)
            {
                this.ui_PickDrop.Close();
            }
        }));
        this.pickDropNetWork.MSG_ReqWatchSceneBag_CS(npc.EID.Id);
        this.watchBagEndEvent = endAction;
    }

    public void ReqPickObjItem(ulong npcID, params uint[] itemID)
    {
        this.pickDropNetWork.MSG_ReqPickObjFromSceneBag_CS(npcID, itemID);
    }

    public void ReqPickAll(ulong npcID = 0UL)
    {
        if (this.curNpc == null && npcID == 0UL)
        {
            return;
        }
        if (npcID == 0UL)
        {
            npcID = this.curNpc.EID.Id;
        }
        this.pickDropNetWork.MSG_ReqPickAllSceneBag_CS(npcID);
    }

    public void ResetBagCacheData()
    {
        this.bagCacheData = null;
    }

    public bool CheckCanPick()
    {
        bool result = false;
        if (this.bagCacheData != null)
        {
            for (int i = 0; i < this.bagCacheData.items.Count; i++)
            {
                uint baseid = this.bagCacheData.items[i].obj.baseid;
                LuaTable configTable = LuaConfigManager.GetConfigTable("objects", (ulong)baseid);
                if (configTable != null)
                {
                    uint cacheField_Uint = configTable.GetCacheField_Uint("maxnum");
                    if (cacheField_Uint != 1U)
                    {
                        if (cacheField_Uint > 1U)
                        {
                            object obj = LuaScriptMgr.Instance.CallLuaFunction("BagCtrl.GetObjectNumByidFromMainPackage", new object[]
                            {
                                Util.GetLuaTable("BagCtrl"),
                                baseid
                            })[0];
                            uint num = (uint)((double)obj);
                            if (num != 0U)
                            {
                                if (num % cacheField_Uint != 0U)
                                {
                                    return true;
                                }
                            }
                        }
                    }
                }
            }
        }
        return result;
    }

    internal void OnRetMSG_RefreshSceneBag_SC(MSG_RefreshSceneBag_SC msg)
    {
        this.bagCacheData = msg;
        Action action = delegate ()
        {
            if (this.curNpc != null)
            {
                if (this.curNpc.EID.Id == msg.tempid)
                {
                    this.ui_PickDrop.RefrashPickListUI(msg);
                }
            }
            else
            {
                this.ui_PickDrop.RefrashPickListUI(msg);
            }
            Scheduler.Instance.AddTimer(1f, false, delegate
            {
                if (this.watchBagEndEvent != null)
                {
                    this.watchBagEndEvent(msg.tempid);
                    this.watchBagEndEvent = null;
                }
            });
        };
        if (this.ui_PickDrop != null)
        {
            action();
        }
        else if (!msg.isrefresh)
        {
            UIManager.Instance.ShowUI<UI_PickDrop>("UI_PickDrop", action, UIManager.ParentType.CommonUI, false);
        }
    }

    public void ShortcutQuickPickAll(Npc autoPickTarget)
    {
        UI_PickDrop uiobject = UIManager.GetUIObject<UI_PickDrop>();
        if (uiobject != null)
        {
            this.ReqPickAll(0UL);
            uiobject.Close();
        }
        if (autoPickTarget != null)
        {
            EntitiesManager em = ManagerCenter.Instance.GetManager<EntitiesManager>();
            if (em == null)
            {
                return;
            }
            MainPlayer.Self.GetComponent<PathFindFollowTarget>().StartFwllowTarget(autoPickTarget, PathFindFollowTarget.FollowType.FollowToVisite, delegate (CharactorBase charTarget, bool b)
            {
                Npc npc = em.GetNpc(autoPickTarget.EID.Id);
                if (npc != null)
                {
                    this.MSG_ReqWatchSceneBag_CS(npc, null);
                }
            });
        }
    }

    public Npc CheckIsHaveBagNpcInNineScreen(Queue<ulong> blackID = null)
    {
        EntitiesManager manager = ManagerCenter.Instance.GetManager<EntitiesManager>();
        if (manager == null)
        {
            return null;
        }
        Npc autoPickTarget = null;
        if (manager != null)
        {
            manager.NpcList.BetterForeach(delegate (KeyValuePair<ulong, Npc> item)
            {
                Npc value = item.Value;
                LuaTable configTable = LuaConfigManager.GetConfigTable("npc_data", (ulong)value.NpcData.MapNpcData.baseid);
                if (configTable != null && (long)configTable.GetCacheField_Int("kind") == 20L)
                {
                    if (blackID != null)
                    {
                        if (!blackID.Contains(value.EID.Id))
                        {
                            autoPickTarget = value;
                        }
                    }
                    else
                    {
                        autoPickTarget = value;
                    }
                }
            });
        }
        return autoPickTarget;
    }

    public PickDropNetWork pickDropNetWork;

    public Npc curNpc;

    public Action<ulong> watchBagEndEvent;

    private MSG_RefreshSceneBag_SC bagCacheData;
}
