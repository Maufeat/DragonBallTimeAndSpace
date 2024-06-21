using System;
using System.Collections.Generic;
using Framework.Managers;
using Obj;
using UnityEngine;
using UnityEngine.UI;

public class UI_ShortCutUseEquip : UIPanelBase
{
    public override void OnDispose()
    {
        base.OnDispose();
        this.UnInit();
        GameObject gameObject = this.root.gameObject;
        this.root = null;
        UnityEngine.Object.Destroy(gameObject);
    }

    public override void OnInit(Transform root)
    {
        base.OnInit(root);
        this.InitObj(root);
        this.InitEvent();
        this._interval = LuaConfigManager.GetXmlConfigTable("massiveConfig").GetCacheField_Table("AddItemInterval").GetCacheField_Float("value");
        this._showTime = LuaConfigManager.GetXmlConfigTable("massiveConfig").GetCacheField_Table("AddItemShowTime").GetCacheField_Float("value");
        this._tweenTime = LuaConfigManager.GetXmlConfigTable("massiveConfig").GetCacheField_Table("AddItemTweenTime").GetCacheField_Float("value");
        Scheduler.Instance.AddUpdator(new Scheduler.OnScheduler(this.UpdateCD));
    }

    private void InitObj(Transform _root)
    {
        this.root = _root;
        this.trans_ItemUseParent = this.root.transform.Find("Offset_ItemUse/Panel_ItemUse");
        this.transitem = this.root.transform.Find("Offset_ItemUse/Panel_ItemUse/Item");
        this.btnequip = this.root.transform.Find("Offset_ItemUse/Panel_ItemUse/btn_ok").GetComponent<Button>();
        this.btnclose = this.root.transform.Find("Offset_ItemUse/Panel_ItemUse/btn_close").GetComponent<Button>();
        this.txttitle = this.root.transform.Find("Offset_ItemUse/Panel_ItemUse/txt_title").GetComponent<Text>();
        this.txtbtncontent = this.root.transform.Find("Offset_ItemUse/Panel_ItemUse/btn_ok/Text").GetComponent<Text>();
        this.trans_ItemAddParent = this.root.transform.Find("Offset_ItemUse/Panel_ItemAdd");
        this.obj_ItemSource = this.root.transform.Find("Offset_ItemUse/Panel_ItemAdd/Items/Item_Source").gameObject;
        this.obj_ItemSource.SetActive(false);
        this.Img_CD = this.root.transform.Find("Offset_ItemUse/Panel_ItemUse/Item/img_cd").GetComponent<Image>();
        this.Txt_CD = this.root.transform.Find("Offset_ItemUse/Panel_ItemUse/Item/img_cd/Text").GetComponent<Text>();
        this.Img_CD.type = Image.Type.Filled;
        this.Img_CD.gameObject.SetActive(false);
    }

    private void InitEvent()
    {
        this.btnclose.onClick.RemoveAllListeners();
        this.btnclose.onClick.AddListener(delegate ()
        {
            this.OnClickClose();
        });
        this.btnequip.onClick.RemoveAllListeners();
        this.btnequip.onClick.AddListener(delegate ()
        {
            this.OnClickEquip();
        });
    }

    public void UnInit()
    {
        Scheduler.Instance.RemoveUpdator(new Scheduler.OnScheduler(this.UpdateCD));
        this._curItemNum = 0U;
        this._isShowUseItem = false;
        this.propsBase = null;
    }

    public void SetUseItemActive(bool isActive)
    {
        this.trans_ItemUseParent.gameObject.SetActive(isActive);
        this._isShowUseItem = isActive;
    }

    public void ShowThis(PropsBase pb)
    {
        this.SetUseItemActive(true);
        CommonItem commonItem = new CommonItem(this.transitem);
        commonItem.SetCommonItem(pb._obj.baseid, 1U, delegate (uint u)
        {
            LuaScriptMgr.Instance.CallLuaFunction("BagCtrl.OpenInfoShortcutUse", new object[]
            {
                Util.GetLuaTable("BagCtrl"),
                pb
            });
        });
        this.commonItems.Add(commonItem);
        UIInformationList component = this.txttitle.GetComponent<UIInformationList>();
        UIInformationList component2 = this.txtbtncontent.GetComponent<UIInformationList>();
        if ((pb._obj.type >= ObjectType.OBJTYPE_WEAPON_MIN && pb._obj.type <= ObjectType.OBJTYPE_WEAPON_MAX) || (pb._obj.type >= ObjectType.OBJTYPE_EQUIP_MIN && pb._obj.type <= ObjectType.OBJTYPE_EQUIP_MAX))
        {
            this.transitem.Find("img_up").gameObject.SetActive(true);
            this.txttitle.text = ControllerManager.Instance.GetController<TextModelController>().ChangeTextModel(component.listInformation[0].content);
            this.txtbtncontent.text = ControllerManager.Instance.GetController<TextModelController>().ChangeTextModel(component2.listInformation[0].content);
        }
        else
        {
            this.transitem.Find("img_up").gameObject.SetActive(false);
            this.txttitle.text = ControllerManager.Instance.GetController<TextModelController>().ChangeTextModel(component.listInformation[1].content);
            this.txtbtncontent.text = ControllerManager.Instance.GetController<TextModelController>().ChangeTextModel(component2.listInformation[1].content);
        }
        this._configCD = (long)pb.config.GetCacheField_Int("cdtime");
        this._nextUseTime = (long)((ulong)pb._obj.nextusetime);
        this.propsBase = pb;
    }

    public void ShowThis(PropsBase pb, int num)
    {
        this.ShowThis(pb);
        UIInformationList component = this.txttitle.GetComponent<UIInformationList>();
        this.txttitle.text = ControllerManager.Instance.GetController<TextModelController>().ChangeTextModel(component.listInformation[num].content);
    }

    public void ShowTreasureFind(PropsBase pb)
    {
        this.SetUseItemActive(true);
        CommonItem commonItem = new CommonItem(this.transitem);
        commonItem.SetCommonItem(pb._obj.baseid, pb._obj.num, delegate (uint u)
        {
            LuaScriptMgr.Instance.CallLuaFunction("BagCtrl.OpenInfoShortcutUse", new object[]
            {
                Util.GetLuaTable("BagCtrl"),
                pb
            });
        });
        this.commonItems.Add(commonItem);
        UIInformationList component = this.txttitle.GetComponent<UIInformationList>();
        UIInformationList component2 = this.txtbtncontent.GetComponent<UIInformationList>();
        this.transitem.Find("img_up").gameObject.SetActive(false);
        this.txttitle.text = ControllerManager.Instance.GetController<TextModelController>().ChangeTextModel(component.listInformation[2].content);
        this.txtbtncontent.text = ControllerManager.Instance.GetController<TextModelController>().ChangeTextModel(component2.listInformation[1].content);
        this._configCD = (long)pb.config.GetCacheField_Int("cdtime");
        this._nextUseTime = (long)((ulong)pb._obj.nextusetime);
        this.propsBase = pb;
    }

    public void ShowGetNewItems(List<PropsBase> list)
    {
        if (list == null || list.Count < 0)
        {
            return;
        }
        int count = list.Count;
        for (int i = count - 1; i >= 0; i--)
        {
            PropsBase pb = list[i];
            this.ShowGetNewItem(pb);
            list.RemoveAt(i);
        }
    }

    public void ShowGetNewItem(PropsBase pb)
    {
        if (!this.trans_ItemAddParent.gameObject.activeSelf)
        {
            this.trans_ItemAddParent.gameObject.SetActive(true);
        }
        this._curItemNum += 1U;
    }

    private void UpdateItemTweens(GameObject go, uint index, Vector3 targetPos)
    {
        TweenPosition component = go.GetComponent<TweenPosition>();
        TweenScale component2 = go.GetComponent<TweenScale>();
        component.from = go.transform.localPosition;
        component.to = targetPos;
        component.duration = this._tweenTime;
        component2.duration = this._tweenTime;
        component.delay = this._showTime + index * this._interval;
        component2.delay = this._showTime + index * this._interval;
        component.enabled = true;
        component2.enabled = true;
        component.onFinished = delegate (UITweener teener)
        {
            this.TweenComplete(go, index);
        };
    }

    private void UpdateItemIcon(GameObject go, string strIcon)
    {
        RawImage icon = go.transform.Find("img_icon").GetComponent<RawImage>();
        icon.gameObject.SetActive(false);
        base.GetTexture(ImageType.ITEM, strIcon, delegate (Texture2D sp)
        {
            if (sp == null)
            {
                FFDebug.LogWarning(this, string.Format("GetTexture Tex is null config.icon = {0}", strIcon));
                return;
            }
            Sprite sprite = Sprite.Create(sp, new Rect(0f, 0f, (float)sp.width, (float)sp.height), new Vector2(0f, 0f));
            icon.texture = sprite.texture;
            icon.color = Color.white;
            icon.gameObject.SetActive(true);
        });
    }

    private GameObject InstantiateItem(Transform source, Transform parentTrans)
    {
        GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(source.gameObject);
        gameObject.transform.SetParent(parentTrans);
        gameObject.transform.position = source.position;
        gameObject.transform.localScale = source.localScale;
        return gameObject;
    }

    private void TweenComplete(GameObject go, uint index)
    {
        if (go == null)
        {
            return;
        }
        UnityEngine.Object.Destroy(go);
        if (this._curItemNum >= 1U)
        {
            this._curItemNum -= 1U;
        }
        else
        {
            this._curItemNum = 0U;
        }
        if (index == 0U && !this._isShowUseItem)
        {
            ShortCutUseEquipController controller = ControllerManager.Instance.GetController<ShortCutUseEquipController>();
            if (controller != null)
            {
                controller.StartShowShortcutTips();
            }
        }
    }

    public bool IsCurrentShowItem(string thisid)
    {
        return this.propsBase != null && this.propsBase._obj.thisid == thisid;
    }

    private void UpdateCD()
    {
        if (this._configCD == 0L)
        {
            this.Img_CD.gameObject.SetActive(false);
            return;
        }
        if (this._nextUseTime == 0L)
        {
            this.Img_CD.gameObject.SetActive(false);
        }
        else if (this._nextUseTime > 0L)
        {
            long currServerTime = (long)SingletonForMono<GameTime>.Instance.GetCurrServerTime();
            long num = this._nextUseTime * 1000L;
            if (num < currServerTime)
            {
                this.Img_CD.gameObject.SetActive(false);
            }
            else
            {
                uint leftTime = (uint)(num - currServerTime) / 1000U;
                this.Img_CD.fillAmount = (float)(num - currServerTime) / (float)(this._configCD * 1000L);
                this.Txt_CD.text = this.GetTimeStr(leftTime);
                this.Img_CD.gameObject.SetActive(true);
            }
        }
    }

    private string GetTimeStr(uint leftTime)
    {
        uint num = 0U;
        uint num2 = 0U;
        uint num3 = leftTime;
        if (num3 > 60U)
        {
            num2 = num3 / 60U;
            num3 %= 60U;
        }
        if (num2 > 60U)
        {
            num = num2 / 60U;
            num2 %= 60U;
        }
        if (num > 0U)
        {
            return string.Format("{0}:{1:D2}:{2:D2}", num, num2, num3);
        }
        if (num2 > 0U)
        {
            return string.Format("{0:D2}:{1:D2}", num2, num3);
        }
        return num3.ToString();
    }

    private void OnClickEquip()
    {
        if ((this.propsBase._obj.type >= ObjectType.OBJTYPE_WEAPON_MIN && this.propsBase._obj.type <= ObjectType.OBJTYPE_WEAPON_MAX) || (this.propsBase._obj.type >= ObjectType.OBJTYPE_EQUIP_MIN && this.propsBase._obj.type <= ObjectType.OBJTYPE_EQUIP_MAX))
        {
            if (this.propsBase != null)
            {
                LuaScriptMgr.Instance.CallLuaFunction("BagCtrl.WearEquipCS", new object[]
                {
                    Util.GetLuaTable("BagCtrl"),
                    this.propsBase
                });
            }
        }
        else if (this.propsBase != null)
        {
            LuaScriptMgr.Instance.CallLuaFunction("BagCtrl.ReqUseItem", new object[]
            {
                Util.GetLuaTable("BagCtrl"),
                this.propsBase,
                1
            });
        }
        ControllerManager.Instance.GetController<ShortCutUseEquipController>().ProcessedOne();
    }

    private void OnClickClose()
    {
        ControllerManager.Instance.GetController<ShortCutUseEquipController>().TryClose();
    }

    private Transform root;

    private Transform trans_ItemUseParent;

    private Transform transitem;

    private Button btnequip;

    private Button btnclose;

    private PropsBase propsBase;

    private Text txttitle;

    private Text txtbtncontent;

    private Transform trans_ItemAddParent;

    private GameObject obj_ItemSource;

    private uint _curItemNum;

    private bool _isShowUseItem;

    private Vector3 _targetPos = new Vector3(678f, -572f);

    private float _interval;

    private float _showTime;

    private float _tweenTime;

    private List<CommonItem> commonItems = new List<CommonItem>();

    private Image Img_CD;

    private Text Txt_CD;

    private long _nextUseTime;

    private long _configCD;
}
