using System;
using System.Collections.Generic;
using Framework.Managers;
using LuaInterface;
using npc;
using Obj;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_PickDrop : UIPanelBase
{
    private PickDropController pdc
    {
        get
        {
            return ControllerManager.Instance.GetController<PickDropController>();
        }
    }

    public override void OnInit(Transform root)
    {
        this.ui_root = root;
        this.InitObject();
        this.InitEvent();
        Scheduler.Instance.AddFixedUpdator(new Scheduler.OnScheduler(this.CheckDistToBag));
    }

    private void InitObject()
    {
        this.btnClose = this.ui_root.Find("Offset_PickDrop/Panel_root/btn_close").GetComponent<Button>();
        this.btnPickAll = this.ui_root.Find("Offset_PickDrop/Panel_root/btn_pick_all").GetComponent<Button>();
        this.srRoot = this.ui_root.Find("Offset_PickDrop/Panel_root/Scroll View").GetComponent<ScrollRect>();
        this.itemPrefabObj = this.srRoot.gameObject.transform.Find("Viewport/Content/Item").gameObject;
        this.tipPanel = this.ui_root.Find("Offset_PickDrop/Panel_root/Panel_tip");
        this.panelRoot = this.ui_root.Find("Offset_PickDrop/Panel_root");
        this.transCard = this.ui_root.Find("Offset_PickDrop/Panel_root/Card");
        this.imgBackground = this.transCard.GetComponent<Image>();
        Transform transform = this.transCard.Find("Head");
        this.imgIcon = transform.Find("Icon").GetComponent<Image>();
        Transform transform2 = transform.Find("Info");
        this.txtName = transform2.Find("Name").GetComponent<Text>();
        this.transStar = transform2.Find("Star");
        this.imgBind = transform2.Find("Bind").GetComponent<Image>();
        this.txtCardType = transform2.Find("CardType/Type").GetComponent<Text>();
        this.txtNeedLevel = transform2.Find("NeedLevel/Level").GetComponent<Text>();
        this.imgBaseAttrBg = this.transCard.Find("Center").GetComponent<Image>();
        this.transBaseAttrList = new List<Transform>();
        for (int i = 0; i < this.transCard.Find("Center").childCount; i++)
        {
            this.transBaseAttrList.Add(this.transCard.Find("Center").GetChild(i));
        }
        this.transRandAttrList = new List<Transform>();
        for (int j = 0; j < 5; j++)
        {
            this.transRandAttrList.Add(this.transCard.Find("Bottom").GetChild(j));
        }
        this.transRandTriggerList = new List<Transform>();
        for (int k = 5; k < 10; k++)
        {
            this.transRandTriggerList.Add(this.transCard.Find("Bottom").GetChild(k));
        }
        this.itemPrefabObj.gameObject.SetActive(false);
        Image component = this.itemPrefabObj.GetComponent<Image>();
        component.type = Image.Type.Sliced;
    }

    private void InitEvent()
    {
        this.btnClose.onClick.RemoveAllListeners();
        this.btnClose.onClick.AddListener(new UnityAction(this.Close));
        this.btnPickAll.onClick.RemoveAllListeners();
        this.btnPickAll.onClick.AddListener(new UnityAction(this.PickAll));
    }

    public void Close()
    {
        UIManager.Instance.DeleteUI<UI_PickDrop>();
        this.pdc.ResetBagCacheData();
    }

    private void PickAll()
    {
        if (this.pdc != null)
        {
            this.pdc.ReqPickAll(0UL);
        }
    }

    public override void OnDispose()
    {
        base.OnDispose();
        Scheduler.Instance.RemoveFixedUpdator(new Scheduler.OnScheduler(this.CheckDistToBag));
        this.pdc.ResetBagCacheData();
    }

    private void CheckDistToBag()
    {
        if (this.pdc != null && this.pdc.curNpc != null && Vector2.Distance(this.pdc.curNpc.NextPosition2D, MainPlayer.Self.NextPosition2D) > Const.DistNpcVisitResponse + 3f)
        {
            this.pdc.curNpc = null;
            this.Close();
        }
    }

    public void RefrashPickListUI(MSG_RefreshSceneBag_SC msg)
    {
        if (msg.items.Count == 0)
        {
            this.Close();
            return;
        }
        ItemTipController itc = ControllerManager.Instance.GetController<ItemTipController>();
        Transform parent = this.itemPrefabObj.transform.parent;
        List<ObjItem> items = msg.items;
        int num = Mathf.Max(parent.transform.childCount, items.Count);
        for (int i = 0; i < num; i++)
        {
            GameObject gameObject;
            if (i < parent.childCount)
            {
                gameObject = parent.GetChild(i).gameObject;
            }
            else
            {
                gameObject = UnityEngine.Object.Instantiate<GameObject>(this.itemPrefabObj);
                gameObject.transform.SetParent(parent, false);
            }
            Button btnItem = gameObject.GetComponent<Button>();
            btnItem.onClick.RemoveAllListeners();
            if (i < items.Count)
            {
                gameObject.gameObject.SetActive(true);
                ObjItem oi = items[i];
                btnItem.onClick.AddListener(delegate ()
                {
                    this.OnPickItemClick(msg.tempid, oi.thisid);
                });
                RawImage imageIcon = gameObject.FindChild("img_icon").GetComponent<RawImage>();
                Text component = gameObject.FindChild("img_icon/txt_count").GetComponent<Text>();
                Text component2 = gameObject.FindChild("txt_name").GetComponent<Text>();
                component.text = oi.obj.num + string.Empty;
                HoverEventListener.Get(btnItem.gameObject).onExit = delegate (PointerEventData pData)
                {
                    this.tipPanel.gameObject.SetActive(false);
                };
                Scheduler.Instance.AddFrame(1U, false, delegate
                {
                    if (btnItem)
                    {
                        CommonTools.DestroyComponent<UIEventListener>(btnItem.transform, false);
                    }
                });
                string imgname = string.Empty;
                if (oi.obj.type == ObjectType.OBJTYPE_DNA)
                {
                    LuaTable configTable = LuaConfigManager.GetConfigTable("dnachip_config", (ulong)(oi.obj.baseid + oi.obj.level));
                    component2.text = configTable.GetCacheField_String("chipname");
                    imgname = configTable.GetCacheField_String("chipicon");
                }
                else
                {
                    LuaTable configTable2 = LuaConfigManager.GetConfigTable("objects", (ulong)oi.obj.baseid);
                    component2.text = configTable2.GetCacheField_String("name");
                    imgname = configTable2.GetCacheField_String("icon");
                }
                UITextureMgr.Instance.GetTexture(ImageType.ITEM, imgname, delegate (UITextureAsset tex)
                {
                    if (tex.textureObj != null && imageIcon != null)
                    {
                        Texture2D textureObj = tex.textureObj;
                        imageIcon.texture = textureObj;
                    }
                });
                HoverEventListener.Get(btnItem.gameObject).onEnter = delegate (PointerEventData pData)
                {
                    if (itc != null)
                    {
                        itc.OpenPanel(oi.obj, imageIcon.transform.parent.gameObject);
                    }
                };
            }
            else
            {
                gameObject.gameObject.SetActive(false);
            }
        }
    }

    private void SetupCard(Transform itemRoot, LuaTable mConfig, t_Object mData)
    {
        ManagerCenter.Instance.GetManager<UITextureMgr>().GetSpriteFromAtlas("other", UIManager.Instance.GetItemTipBigBgImg(mConfig.GetField_Uint("quality")), delegate (Sprite sprite)
        {
            if (this.imgBackground != null)
            {
                this.imgBackground.overrideSprite = sprite;
            }
        });
        ManagerCenter.Instance.GetManager<UITextureMgr>().GetTexture(ImageType.ITEM, mConfig.GetField_String("icon"), delegate (UITextureAsset asset)
        {
            if (asset != null && this.imgIcon != null)
            {
                Texture2D textureObj = asset.textureObj;
                Sprite sprite = Sprite.Create(textureObj, new Rect(0f, 0f, (float)textureObj.width, (float)textureObj.height), new Vector2(0f, 0f));
                this.imgIcon.sprite = sprite;
                this.imgIcon.overrideSprite = sprite;
                this.imgIcon.color = Color.white;
                this.imgIcon.gameObject.SetActive(true);
                this.imgIcon.material = null;
            }
        });
        this.txtName.text = mConfig.GetField_String("name");
        this.txtName.color = CommonTools.GetQualityColor(mConfig.GetField_Uint("quality"));
        this.imgBaseAttrBg.color = this.GetBaseAttrBgColor(mConfig.GetField_Uint("quality"));
        for (int i = 0; i < this.transStar.childCount; i++)
        {
            this.transStar.GetChild(i).gameObject.SetActive((long)i < (long)((ulong)mData.card_data.cardstar));
        }
        string text = "金";
        switch (mData.card_data.cardtype)
        {
            case 2U:
                text = "木";
                break;
            case 3U:
                text = "水";
                break;
            case 4U:
                text = "火";
                break;
            case 5U:
                text = "土";
                break;
        }
        this.txtCardType.text = text;
        this.txtNeedLevel.text = mConfig.GetField_String("uselevel");
        this.SetCommonItemBind(mData.bind, this.imgBind);
        for (int j = 0; j < this.transBaseAttrList.Count; j++)
        {
            Transform transform = this.transBaseAttrList[j];
            if (j < mData.card_data.base_effect.Count)
            {
                CardEffectItem cardEffectItem = mData.card_data.base_effect[j];
                Text component = transform.GetComponent<Text>();
                Text component2 = transform.Find("Text").GetComponent<Text>();
                LuaTable configTable = LuaConfigManager.GetConfigTable("cardeffect_config", (ulong)cardEffectItem.id);
                if (configTable != null)
                {
                    component.text = configTable.GetField_String("cardeffectdes");
                    component2.text = "+" + cardEffectItem.value;
                }
                transform.gameObject.SetActive(true);
            }
            else
            {
                transform.gameObject.SetActive(false);
            }
        }
        for (int k = 0; k < this.transRandAttrList.Count; k++)
        {
            Transform transform2 = this.transRandAttrList[k];
            if (k < mData.card_data.rand_effect.Count)
            {
                CardEffectItem cardEffectItem2 = mData.card_data.rand_effect[k];
                Text component3 = transform2.GetComponent<Text>();
                Text component4 = transform2.Find("Text").GetComponent<Text>();
                LuaTable configTable2 = LuaConfigManager.GetConfigTable("cardeffect_config", (ulong)cardEffectItem2.id);
                if (configTable2 != null)
                {
                    component3.text = configTable2.GetField_String("cardeffectdes");
                    component4.text = "+" + cardEffectItem2.value;
                }
                else
                {
                    Debug.LogError("card_data.rand_effect.id=0");
                }
                transform2.gameObject.SetActive(true);
            }
            else
            {
                transform2.gameObject.SetActive(false);
            }
        }
        for (int l = 0; l < this.transRandTriggerList.Count; l++)
        {
            this.transRandTriggerList[l].gameObject.SetActive(false);
        }
        this.transCard.gameObject.SetActive(true);
        UI_Gene.SetTipPanelPos(this.ui_root, itemRoot, this.panelRoot, this.transCard);
    }

    private Color GetBaseAttrBgColor(uint quality)
    {
        Color result = new Color(0.101960786f, 0.196078435f, 0.09411765f);
        if (quality != 2U)
        {
            if (quality == 3U)
            {
                result = new Color(0.20784314f, 0.0470588244f, 0.203921571f);
            }
        }
        else
        {
            result = new Color(0.0784313753f, 0.145098045f, 0.3254902f);
        }
        return result;
    }

    private void SetCommonItemBind(uint bind, Image _imgBind)
    {
        if (_imgBind == null)
        {
            return;
        }
        switch (bind)
        {
            case 0U:
                _imgBind.gameObject.SetActive(false);
                break;
            case 1U:
                _imgBind.gameObject.SetActive(true);
                _imgBind.color = Const.GetColorByName("bindwhite");
                break;
            case 2U:
                _imgBind.gameObject.SetActive(true);
                _imgBind.color = Const.GetColorByName("bindgrey");
                break;
            default:
                _imgBind.gameObject.SetActive(false);
                break;
        }
    }

    private void OnPickItemClick(ulong npcID, uint id)
    {
        if (this.pdc != null)
        {
            this.pdc.ReqPickObjItem(npcID, new uint[]
            {
                id
            });
        }
    }

    private Transform ui_root;

    private Button btnClose;

    private Button btnPickAll;

    private ScrollRect srRoot;

    private GameObject itemPrefabObj;

    private Transform tipPanel;

    private Transform panelRoot;

    private Transform transCard;

    private Image imgBaseAttrBg;

    private Image imgBackground;

    private Image imgIcon;

    private Text txtName;

    private Transform transStar;

    private Image imgBind;

    private Text txtCardType;

    private Text txtNeedLevel;

    private List<Transform> transBaseAttrList;

    private List<Transform> transRandAttrList;

    private List<Transform> transRandTriggerList;
}
