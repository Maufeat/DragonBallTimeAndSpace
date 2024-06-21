using System;
using System.Collections.Generic;
using Framework.Managers;
using LuaInterface;
using Models;
using Obj;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemTipController : ControllerBase
{
    public UI_ItemTip UIItemTip
    {
        get
        {
            return UIManager.GetUIObject<UI_ItemTip>();
        }
    }

    public void OpenPanel(LuaTable ltb, GameObject btnObj)
    {
        t_Object objectData = this.GetObjectData(ltb);
        this.OpenPanel(objectData, btnObj);
    }

    public void OpenPanel(uint baseid, GameObject btnObj)
    {
        t_Object data = new t_Object
        {
            baseid = baseid
        };
        this.OpenPanel(data, btnObj);
    }

    public void OpenGenePanel(uint geneId, uint geneLevel, GameObject btnObj)
    {
        this.OpenPanel(new t_Object
        {
            baseid = geneId,
            level = geneLevel
        }, btnObj);
    }

    public void OpenPanel(t_Object data, GameObject btnObj)
    {
        if (this.UIItemTip != null)
        {
            this.UIItemTip.Root.position = new Vector3(1000f, 1000f);
            this.SetupPanel(data, btnObj);
        }
        else
        {
            UIManager.Instance.ShowUI<UI_ItemTip>("UI_ItemTip", delegate ()
            {
                this.UIItemTip.Root.gameObject.AddComponent<UIFocus>();
                this.SetupPanel(data, btnObj);
            }, UIManager.ParentType.Tips, false);
        }
    }

    public void OpenSkillPanel(MainPlayerSkillBase skillData, GameObject btnObj)
    {
        if (this.UIItemTip != null)
        {
            this.UIItemTip.Root.position = new Vector3(1000f, 1000f);
            this.SetupSkillPanel(skillData, btnObj);
        }
        else
        {
            UIManager.Instance.ShowUI<UI_ItemTip>("UI_ItemTip", delegate ()
            {
                this.UIItemTip.Root.gameObject.AddComponent<UIFocus>();
                this.SetupSkillPanel(skillData, btnObj);
            }, UIManager.ParentType.Tips, false);
        }
    }

    public void OpenSkinPanel(int skinid, bool getted, GameObject btnObj)
    {
        if (this.UIItemTip != null)
        {
            this.UIItemTip.Root.position = new Vector3(1000f, 1000f);
            this.SetupSkinPanel(skinid, getted, btnObj);
        }
        else
        {
            UIManager.Instance.ShowUI<UI_ItemTip>("UI_ItemTip", delegate ()
            {
                this.UIItemTip.Root.gameObject.AddComponent<UIFocus>();
                this.SetupSkinPanel(skinid, getted, btnObj);
            }, UIManager.ParentType.Tips, false);
        }
    }

    public void OpenGuildSkillPanel(uint skillId, GameObject btnObj)
    {
        if (this.UIItemTip != null)
        {
            this.UIItemTip.Root.position = new Vector3(1000f, 1000f);
            this.SetupGuildSkillPanel(skillId, btnObj);
        }
        else
        {
            UIManager.Instance.ShowUI<UI_ItemTip>("UI_ItemTip", delegate ()
            {
                this.UIItemTip.Root.gameObject.AddComponent<UIFocus>();
                this.SetupGuildSkillPanel(skillId, btnObj);
            }, UIManager.ParentType.Tips, false);
        }
    }

    public void OpenHeroAwaktarPanel(int awakeheroid, GameObject btnObj)
    {
        if (this.UIItemTip != null)
        {
            this.UIItemTip.Root.position = new Vector3(1000f, 1000f);
            this.SetupHeroAwakePanel(awakeheroid, btnObj);
        }
        else
        {
            UIManager.Instance.ShowUI<UI_ItemTip>("UI_ItemTip", delegate ()
            {
                this.UIItemTip.Root.gameObject.AddComponent<UIFocus>();
                this.SetupHeroAwakePanel(awakeheroid, btnObj);
            }, UIManager.ParentType.Tips, false);
        }
    }

    public void OpenBuffPanel(UserState Flag, GameObject btnObj, List<ulong> effects)
    {
        if (this.UIItemTip != null)
        {
            this.UIItemTip.Root.position = new Vector3(1000f, 1000f);
            this.SetupBuffPanel(Flag, btnObj, effects);
        }
        else
        {
            UIManager.Instance.ShowUI<UI_ItemTip>("UI_ItemTip", delegate ()
            {
                this.UIItemTip.Root.gameObject.AddComponent<UIFocus>();
                this.SetupBuffPanel(Flag, btnObj, effects);
            }, UIManager.ParentType.Tips, false);
        }
    }

    public void OpenAttrPanel(Dictionary<string, int> allAttr, GameObject btnObj)
    {
        if (this.UIItemTip != null)
        {
            this.UIItemTip.Root.position = new Vector3(1000f, 1000f);
            this.SetupAttrPanel(allAttr, btnObj);
        }
        else
        {
            UIManager.Instance.ShowUI<UI_ItemTip>("UI_ItemTip", delegate ()
            {
                this.UIItemTip.Root.gameObject.AddComponent<UIFocus>();
                this.SetupAttrPanel(allAttr, btnObj);
            }, UIManager.ParentType.Tips, false);
        }
    }

    public t_Object GetItemData(string thisid)
    {
        LuaTable ltb = (LuaTable)LuaScriptMgr.Instance.CallLuaFunction("BagCtrl.GetItemData", new object[]
        {
            thisid
        })[0];
        return this.GetObjectData(ltb);
    }

    public t_Object GetObjectData(LuaTable ltb)
    {
        if (ltb == null)
        {
            return null;
        }
        t_Object t_Object = new t_Object();
        t_Object.baseid = ltb.GetField_Uint("baseid");
        t_Object.thisid = ltb.GetField_String("thisid");
        t_Object.name = ltb.GetField_String("name");
        t_Object.num = ltb.GetField_Uint("num");
        t_Object.bind = ltb.GetField_Uint("bind");
        t_Object.grid_x = ltb.GetField_Uint("grid_x");
        t_Object.grid_y = ltb.GetField_Uint("grid_y");
        t_Object.quality = ltb.GetField_Uint("quality");
        t_Object.level = ltb.GetField_Uint("level");
        t_Object.timer = ltb.GetField_Uint("timer");
        t_Object.nextusetime = ltb.GetField_Uint("nextusetime");
        LuaTable cacheField_Table = ltb.GetCacheField_Table("card_data");
        if (cacheField_Table != null && cacheField_Table.GetField_Uint("cardtype") != 0U)
        {
            t_Object.card_data = new CardData();
            t_Object.card_data.cardtype = cacheField_Table.GetField_Uint("cardtype");
            t_Object.card_data.cardstar = cacheField_Table.GetField_Uint("cardstar");
            t_Object.card_data.cardgroup = cacheField_Table.GetField_Uint("cardgroup");
            t_Object.card_data.durability = cacheField_Table.GetField_Uint("durability");
            LuaTable cacheField_Table2 = cacheField_Table.GetCacheField_Table("base_effect");
            if (cacheField_Table2 != null)
            {
                for (int i = 1; i < cacheField_Table2.Count + 1; i++)
                {
                    CardEffectItem cardEffectItem = new CardEffectItem();
                    LuaTable luaTable = cacheField_Table2[i] as LuaTable;
                    cardEffectItem.id = luaTable.GetField_Uint("id");
                    cardEffectItem.trigger = luaTable.GetField_Uint("trigger");
                    cardEffectItem.value = luaTable.GetField_Uint("value");
                    cardEffectItem.varname = luaTable.GetField_String("varname");
                    t_Object.card_data.base_effect.Add(cardEffectItem);
                }
            }
            LuaTable cacheField_Table3 = cacheField_Table.GetCacheField_Table("rand_effect");
            if (cacheField_Table3 != null)
            {
                for (int j = 1; j < cacheField_Table3.Count + 1; j++)
                {
                    CardEffectItem cardEffectItem2 = new CardEffectItem();
                    LuaTable luaTable2 = cacheField_Table3[j] as LuaTable;
                    cardEffectItem2.id = luaTable2.GetField_Uint("id");
                    cardEffectItem2.trigger = luaTable2.GetField_Uint("trigger");
                    cardEffectItem2.value = luaTable2.GetField_Uint("value");
                    cardEffectItem2.varname = luaTable2.GetField_String("varname");
                    t_Object.card_data.rand_effect.Add(cardEffectItem2);
                }
            }
        }
        return t_Object;
    }

    private GameObject tmpBtnObj
    {
        get
        {
            return this._tmpBtnObj;
        }
        set
        {
            this._tmpBtnObj = value;
            ManagerCenter.Instance.GetManager<EscManager>().SetCurTipObj(this._tmpBtnObj);
            GameObjectDestroyListener.Get(this._tmpBtnObj).onDes = new Action(this.OnDestroy);
        }
    }

    private void OnDestroy(PointerEventData eventData)
    {
        this.ClosePanel();
    }

    private void SetupPanel(t_Object data, GameObject btnObj)
    {
        this.UIItemTip.SetupPanel(data);
        this.tmpBtnObj = btnObj;
        Scheduler.Instance.AddFrame(3U, false, new Scheduler.OnScheduler(this.SetTipPos));
    }

    private void SetupSkillPanel(MainPlayerSkillBase skillData, GameObject btnObj)
    {
        this.UIItemTip.SetupSkillPanel(skillData);
        this.tmpBtnObj = btnObj;
        Scheduler.Instance.AddFrame(3U, false, new Scheduler.OnScheduler(this.SetTipPos));
    }

    private void SetupSkinPanel(int skinid, bool getted, GameObject btnObj)
    {
        this.UIItemTip.SetupSkinPanel(skinid, getted);
        this.tmpBtnObj = btnObj;
        Scheduler.Instance.AddFrame(3U, false, new Scheduler.OnScheduler(this.SetTipPos));
    }

    private void SetupGuildSkillPanel(uint skinid, GameObject btnObj)
    {
        this.UIItemTip.SetupGuildSkillPanel(skinid);
        this.tmpBtnObj = btnObj;
        Scheduler.Instance.AddFrame(3U, false, new Scheduler.OnScheduler(this.SetTipPos));
    }

    private void SetupHeroAwakePanel(int awakeheroid, GameObject btnObj)
    {
        this.UIItemTip.SetupHeroAwakePanel(awakeheroid);
        this.tmpBtnObj = btnObj;
        Scheduler.Instance.AddFrame(3U, false, new Scheduler.OnScheduler(this.SetTipPos));
    }

    private void SetupBuffPanel(UserState Flag, GameObject btnObj, List<ulong> effects)
    {
        this.UIItemTip.SetupBuffPanel(Flag, effects);
        this.tmpBtnObj = btnObj;
        Scheduler.Instance.AddFrame(3U, false, new Scheduler.OnScheduler(this.SetTipPos));
    }

    private void SetupAttrPanel(Dictionary<string, int> allAttr, GameObject btnObj)
    {
        this.UIItemTip.SetupAttributeDetailPanel(allAttr);
        this.tmpBtnObj = btnObj;
        Scheduler.Instance.AddFrame(3U, false, new Scheduler.OnScheduler(this.SetTipPos));
    }

    private void SetTipPos()
    {
        if (this.tmpBtnObj == null || this.UIItemTip == null || this.UIItemTip.uiPanelRoot == null)
        {
            FFDebug.LogError(this, "tmpBtnObj==null|| UIItemTip==null|| UIItemTip.uiPanelRoot == null");
            return;
        }
        TipPosSetManager.Instance.Initilize(this.tmpBtnObj, this.UIItemTip.uiPanelRoot.gameObject);
    }

    public void ClosePanel()
    {
        if (this.UIItemTip != null)
        {
            this.UIItemTip.Root.position = new Vector3(1000f, 1000f, 0f);
        }
        Scheduler.Instance.RemoveFrame(new Scheduler.OnScheduler(this.SetTipPos));
    }

    public override void Awake()
    {
        base.Awake();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }

    public override string ControllerName
    {
        get
        {
            return "item_tip_controller";
        }
    }

    private GameObject _tmpBtnObj;
}
