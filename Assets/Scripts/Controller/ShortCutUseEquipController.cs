using System;
using System.Collections.Generic;
using Framework.Managers;
using Models;
using Obj;

public class ShortCutUseEquipController : ControllerBase
{
    public UI_ShortCutUseEquip uiShortCutUseEquip
    {
        get
        {
            return UIManager.GetUIObject<UI_ShortCutUseEquip>();
        }
    }

    public override void Awake()
    {
    }

    public override void OnUpdate()
    {
    }

    public override string ControllerName
    {
        get
        {
            return "equip_shortcutuse";
        }
    }

    private bool checkNeedShowShortcutItem(PropsBase pb)
    {
        if (UIManager.GetLuaUIPanel("UI_InstanceOver") != null)
        {
            return false;
        }
        if (CameraController.Self != null)
        {
            BossDieCameraMove bossDieCameraMove = CameraController.Self.CurrStatebyType<BossDieCameraMove>();
            if (bossDieCameraMove != null)
            {
                return false;
            }
        }
        return true;
    }

    public bool NeedDelayShowAddItem
    {
        get
        {
            return this.needDelayShowAddItem;
        }
        set
        {
            this.needDelayShowAddItem = value;
        }
    }

    public void AddShortcutItem(PropsBase pb)
    {
        if (!this.checkNeedShowShortcutItem(pb))
        {
            return;
        }
        if ((pb._obj.type >= ObjectType.OBJTYPE_WEAPON_MIN && pb._obj.type <= ObjectType.OBJTYPE_WEAPON_MAX) || (pb._obj.type >= ObjectType.OBJTYPE_EQUIP_MIN && pb._obj.type <= ObjectType.OBJTYPE_EQUIP_MAX))
        {
            if (this.WhetherShowEquipShortcut(pb))
            {
                bool flag = false;
                for (int i = 0; i < this.listPropsBases.Count; i++)
                {
                    if (this.listPropsBases[i].config.GetField_Uint("type") == pb.config.GetField_Uint("type"))
                    {
                        flag = true;
                    }
                }
                if (flag)
                {
                    for (int j = 0; j < this.listPropsBases.Count; j++)
                    {
                        if (this.listPropsBases[j].config.GetField_Uint("type") == pb.config.GetField_Uint("type") && pb.FightValue > this.listPropsBases[j].FightValue)
                        {
                            this.listPropsBases.RemoveAt(j);
                            this.listPropsBases.Add(pb);
                            break;
                        }
                    }
                }
                else
                {
                    this.listPropsBases.Add(pb);
                }
            }
        }
        else if (this.WhetherShowItemUseShortcut(pb))
        {
            this.listPropsBases.Add(pb);
        }
    }

    public void ShowAddItemList()
    {
        this.NeedDelayShowAddItem = false;
        for (int i = 0; i < this.listAddItem.Count; i++)
        {
            this.ShowAddNewItem(this.listAddItem[i]);
        }
        this.listAddItem.Clear();
    }

    public void AddCurrencyItem(uint id, uint addNum)
    {
        PropsBase propsBase = new PropsBase(id, addNum);
        if (this.needDelayShowAddItem)
        {
            this.listAddItem.Add(propsBase);
        }
        else
        {
            this.ShowAddNewItem(propsBase);
        }
    }

    private bool WhetherShowEquipShortcut(PropsBase data)
    {
        bool result = false;
        if ((bool)LuaScriptMgr.Instance.CallLuaFunction("BagCtrl.CanEquipIsLegal", new object[]
        {
            Util.GetLuaTable("BagCtrl"),
            data
        })[0])
        {
            PropsBase propsBase = (PropsBase)LuaScriptMgr.Instance.CallLuaFunction("BagCtrl.GetContrastEquip", new object[]
            {
                Util.GetLuaTable("BagCtrl"),
                data
            })[0];
            if (propsBase != null)
            {
                if (data.FightValue > propsBase.FightValue)
                {
                    result = true;
                }
            }
            else
            {
                result = true;
            }
        }
        return result;
    }

    private bool WhetherShowItemUseShortcut(PropsBase data)
    {
        bool result = false;
        if (data.config.GetField_Uint("promptuse") == 1U && (bool)LuaScriptMgr.Instance.CallLuaFunction("BagCtrl.CanUseIsLegal", new object[]
        {
            Util.GetLuaTable("BagCtrl"),
            data
        })[0])
        {
            result = true;
        }
        return result;
    }

    public void ProcessedOne()
    {
        if (this.listPropsBases != null && this.listPropsBases.Count > 0)
        {
            this.listPropsBases.RemoveAt(this.listPropsBases.Count - 1);
            this.RefreshThis();
        }
        else if (this._treasureProp != null)
        {
            if (this._treasureProp._obj.num > 0U)
            {
                this.ShowTreasureHunt(this._treasureProp);
                if (this._treasureProp._obj.num == 0U)
                {
                    this.Reset();
                }
            }
            else
            {
                this.Reset();
            }
        }
        else
        {
            this.Reset();
        }
    }

    public void TryClose()
    {
        if (this._treasureProp != null)
        {
            this.Reset();
        }
        else
        {
            this.ProcessedOne();
        }
    }

    public void StartShowShortcutTips()
    {
        this.RefreshThis();
    }

    private void ShowShortcutTips(PropsBase pb)
    {
        if (this.uiShortCutUseEquip != null)
        {
            this.uiShortCutUseEquip.ShowThis(pb);
        }
    }

    public void Reset()
    {
        this.listPropsBases.Clear();
        if (this.uiShortCutUseEquip != null)
        {
            this.uiShortCutUseEquip.SetUseItemActive(false);
        }
        this._treasureProp = null;
    }

    private void RefreshThis()
    {
        if (this.listPropsBases.Count > 0)
        {
            PropsBase pb = this.listPropsBases[this.listPropsBases.Count - 1];
            this.ShowShortcutTips(pb);
        }
        else
        {
            this.Reset();
        }
    }

    public void ShowAddNewItem(PropsBase pb)
    {
        if (this.needDelayShowAddItem)
        {
            this.listAddItem.Add(pb);
        }
        else if (this.uiShortCutUseEquip == null)
        {
            this._allNewProps.Add(pb);
            ManagerCenter.Instance.GetManager<UIManager>().ShowUI<UI_ShortCutUseEquip>("UI_ItemUse", delegate ()
            {
                if (this.uiShortCutUseEquip != null)
                {
                    this.uiShortCutUseEquip.ShowGetNewItems(this._allNewProps);
                    this.uiShortCutUseEquip.SetUseItemActive(false);
                }
                this._allNewProps.Clear();
            }, UIManager.ParentType.Tips, false);
        }
        else
        {
            this.uiShortCutUseEquip.ShowGetNewItem(pb);
        }
    }

    public void ShowTreasureHunt(PropsBase pb)
    {
        this._treasureProp = pb;
        if (this.uiShortCutUseEquip == null)
        {
            ManagerCenter.Instance.GetManager<UIManager>().ShowUI<UI_ShortCutUseEquip>("UI_ItemUse", delegate ()
            {
                if (this.uiShortCutUseEquip != null)
                {
                    this.uiShortCutUseEquip.ShowTreasureFind(pb);
                }
            }, UIManager.ParentType.Tips, false);
        }
        else
        {
            this.uiShortCutUseEquip.ShowTreasureFind(pb);
        }
    }

    public void RefreshTreasureHunt(PropsBase pb)
    {
        if (this._treasureProp == null)
        {
            return;
        }
        if (this.uiShortCutUseEquip == null)
        {
            return;
        }
        if (!this.uiShortCutUseEquip.IsCurrentShowItem(pb._obj.thisid))
        {
            return;
        }
        this.ShowTreasureHunt(pb);
    }

    public void CloseTreasureHunt(string thisid)
    {
        if (this.uiShortCutUseEquip == null)
        {
            return;
        }
        if (!this.uiShortCutUseEquip.IsCurrentShowItem(thisid))
        {
            return;
        }
        this.Reset();
    }

    public void ShowUseProp(PropsBase pb)
    {
        if (this.uiShortCutUseEquip == null)
        {
            ManagerCenter.Instance.GetManager<UIManager>().ShowUI<UI_ShortCutUseEquip>("UI_ItemUse", delegate ()
            {
                if (this.uiShortCutUseEquip != null)
                {
                    this.uiShortCutUseEquip.ShowThis(pb, 3);
                }
            }, UIManager.ParentType.Tips, false);
        }
        else
        {
            this.uiShortCutUseEquip.ShowThis(pb, 3);
        }
    }

    private List<PropsBase> listPropsBases = new List<PropsBase>();

    private List<PropsBase> _allNewProps = new List<PropsBase>();

    private PropsBase _treasureProp;

    private bool needDelayShowAddItem;

    private List<PropsBase> listAddItem = new List<PropsBase>();
}
