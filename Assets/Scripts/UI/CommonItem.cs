using System;
using Framework.Managers;
using LuaInterface;
using Obj;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CommonItem
{
    public CommonItem(Transform tran)
    {
        this.root = tran;
        this.btnitem = this.root.GetComponent<Button>();
        this.imgback = this.root.Find("img_bg").GetComponent<Image>();
        this.imgicon = this.root.Find("img_icon").GetComponent<Image>();
        if (this.root.Find("effect_ui_01") != null)
        {
            this.objEffect1 = this.root.Find("effect_ui_01").gameObject;
        }
        if (this.root.Find("img_cd") != null)
        {
            this.imgcd = this.root.Find("img_cd").GetComponent<Image>();
            this.txtcd = this.root.Find("img_cd/txt_cd").GetComponent<Text>();
        }
        if (this.root.Find("img_repair") != null)
        {
            this.imgRepair = this.root.Find("img_repair").GetComponent<Image>();
        }
        if (this.imgicon == null)
        {
            UnityEngine.Object.DestroyImmediate(this.root.Find("img_icon").GetComponent<RawImage>());
            this.imgicon = this.root.Find("img_icon").gameObject.AddComponent<Image>();
            this.imgicon.raycastTarget = false;
        }
        this.imgicon.color = new Color(0f, 0f, 0f, 0f);
        this.txtnum = this.root.Find("txt_count").GetComponent<Text>();
        if (this.root.Find("txt_itemname") != null)
        {
            this.txtname = this.root.Find("txt_itemname").GetComponent<Text>();
        }
        if (this.root.Find("img_lock") != null)
        {
            this.imgBind = this.root.Find("img_lock").GetComponent<Image>();
        }
        this.imgmask = this.root.Find("img_mask");
        if (this.root.Find("quality") != null)
        {
            this.qualityicon = this.root.Find("quality").GetComponent<RawImage>();
        }
    }

    public void SetCommonItem(uint id, uint num = 1U, Action<uint> onclickitem = null)
    {
        if (id <= 0U)
        {
            return;
        }
        this.mId = id;
        this.monclickitem = onclickitem;
        if (num <= 1U)
        {
            this.txtnum.gameObject.SetActive(false);
        }
        else
        {
            this.txtnum.gameObject.SetActive(true);
            this.txtnum.text = num.ToString();
        }
        LuaTable configTable = LuaConfigManager.GetConfigTable("objects", (ulong)id);
        if (configTable == null)
        {
            FFDebug.LogWarning("CommonItem", string.Format("Does not exist item with id: " + id, new object[0]));
            return;
        }
        string spritename = string.Empty;
        uint field_Uint = configTable.GetField_Uint("quality");
        switch (field_Uint)
        {
            case 1U:
                spritename = "st0099";
                break;
            case 2U:
                spritename = "st0100";
                break;
            case 3U:
                spritename = "st0101";
                break;
            case 4U:
                spritename = "st0102";
                break;
        }
        ManagerCenter.Instance.GetManager<UITextureMgr>().GetSpriteFromAtlas("base", spritename, delegate (Sprite sprite)
        {
            if (this.imgback != null)
            {
                this.imgback.overrideSprite = sprite;
            }
        });
        ManagerCenter.Instance.GetManager<UITextureMgr>().GetTexture(ImageType.ITEM, configTable.GetField_String("icon"), delegate (UITextureAsset asset)
        {
            if (asset == null)
            {
                FFDebug.LogWarning("CommonItem", "  req  texture   is  null ");
                return;
            }
            if (this.imgicon == null)
            {
                return;
            }
            this.textureasset = asset;
            Texture2D textureObj = asset.textureObj;
            Sprite sprite = Sprite.Create(textureObj, new Rect(0f, 0f, (float)textureObj.width, (float)textureObj.height), new Vector2(0f, 0f));
            this.imgicon.overrideSprite = sprite;
            this.imgicon.sprite = sprite;
            this.imgicon.color = Color.white;
            this.imgicon.gameObject.SetActive(true);
        });
        if (field_Uint >= 2U)
        {
            string imgname = "quality" + field_Uint;
            if (this.qualityicon != null)
            {
                this.qualityicon.gameObject.SetActive(false);
                ManagerCenter.Instance.GetManager<UITextureMgr>().GetTexture(ImageType.RANK, imgname, delegate (UITextureAsset asset)
                {
                    if (asset == null)
                    {
                        return;
                    }
                    if (this.qualityicon != null)
                    {
                        this.qualityicon.gameObject.SetActive(true);
                        this.qualityicon.texture = asset.textureObj;
                    }
                });
            }
        }
        else if (this.qualityicon != null)
        {
            this.qualityicon.gameObject.SetActive(false);
        }
        if (this.txtname != null)
        {
            this.txtname.text = configTable.GetField_String("name");
            switch (configTable.GetField_Uint("quality"))
            {
                case 1U:
                    {
                        string modelColor = ControllerManager.Instance.GetController<TextModelController>().GetModelColor("Item1");
                        this.txtname.color = CommonTools.HexToColor(modelColor);
                        break;
                    }
                case 2U:
                    {
                        string modelColor2 = ControllerManager.Instance.GetController<TextModelController>().GetModelColor("Item2");
                        this.txtname.color = CommonTools.HexToColor(modelColor2);
                        break;
                    }
                case 3U:
                    {
                        string modelColor3 = ControllerManager.Instance.GetController<TextModelController>().GetModelColor("Item3");
                        this.txtname.color = CommonTools.HexToColor(modelColor3);
                        break;
                    }
                case 4U:
                    {
                        string modelColor4 = ControllerManager.Instance.GetController<TextModelController>().GetModelColor("Item4");
                        this.txtname.color = CommonTools.HexToColor(modelColor4);
                        break;
                    }
            }
        }
        if (this.btnitem != null)
        {
            this.btnitem.onClick.RemoveAllListeners();
            if (onclickitem == null)
            {
                this.btnitem.onClick.AddListener(delegate ()
                {
                    LuaScriptMgr.Instance.CallLuaFunction("BagCtrl.OpenInfoCommon", new object[]
                    {
                        Util.GetLuaTable("BagCtrl"),
                        id,
                        this.root
                    });
                });
            }
            else
            {
                this.btnitem.onClick.AddListener(delegate ()
                {
                    onclickitem(id);
                });
            }
            UIEventListener.Get(this.root.parent.gameObject).onEnter = new UIEventListener.VoidDelegate(this.btn_item_on_enter);
            UIEventListener.Get(this.root.parent.gameObject).onDestroy = new UIEventListener.VoidDelegate(this.btn_item_on_exit);
            UIEventListener.Get(this.root.parent.gameObject).onExit = new UIEventListener.VoidDelegate(this.btn_item_on_exit);
        }
        if (this.root != null)
        {
            this.root.gameObject.SetActive(true);
        }
        this.SetCommonItemBind(0U);
        this.SetCommonItemMask(true);
    }

    public void SetCommonItemInLua(uint id, int name, uint num = 1U, Action<uint> onclickitem = null)
    {
        this.SetCommonItem(id, num, onclickitem);
    }

    public void SetNextUseTime(uint nextusetime)
    {
        this.cdtime = (int)(nextusetime - SingletonForMono<GameTime>.Instance.GetCurrServerTimeBySecond());
    }

    public void SetCommonItem(uint id, uint bind, bool canUseOrEquip, uint num = 1U, Action<uint> onclickitem = null)
    {
        LuaTable configTable = LuaConfigManager.GetConfigTable("objects", (ulong)id);
        this.cdtimeconfig = configTable.GetField_Uint("cdtime");
        this.CheckIsGuildSkillEffectObject(id);
        this.SetCommonItem(id, num, onclickitem);
        this.SetCommonItemBind(bind);
        this.SetCommonItemMask(canUseOrEquip);
    }

    private void CheckIsGuildSkillEffectObject(uint id)
    {
        GuildControllerNew gcn = ControllerManager.Instance.GetController<GuildControllerNew>();
        if (gcn != null)
        {
            gcn.TryGetGuildSkillLv(delegate (uint lv)
            {
                if (lv > 0U)
                {
                    LuaTable guildSkillConfigByID = gcn.GetGuildSkillConfigByID(500U + lv);
                    if (guildSkillConfigByID != null)
                    {
                        string cacheField_String = guildSkillConfigByID.GetCacheField_String("skillstaus");
                        string[] array = cacheField_String.Split(new char[]
                        {
                            ','
                        });
                        for (int i = 0; i < array.Length; i++)
                        {
                            if (array[i].Contains("-" + id + "-"))
                            {
                                string[] array2 = array[i].Split(new char[]
                                {
                                    '-'
                                });
                                if (array2.Length > 2)
                                {
                                    float num = 1f - float.Parse(array2[2]) / 100f;
                                    this.cdtimeconfig *= num;
                                }
                            }
                        }
                    }
                }
            }, 5U);
        }
    }

    public void SetCommonItemData(uint id, string thisidstr, string packtype, uint lock_end_time, int uiRootType = 1, uint bind = 0U)
    {
        this.mId = id;
        this.mThisid = thisidstr;
        this.mData = this.GetData();
        this.mBind = bind;
        this.btnitem.GetComponent<Image>().raycastTarget = false;
        this.imgBind.gameObject.SetActive(lock_end_time == 1U);
        string s = "1";
        switch (packtype)
        {
            case "PACK_TYPE_NONE":
                s = "0";
                break;
            case "PACK_TYPE_MAIN":
                s = "1";
                break;
            case "PACK_TYPE_EQUIP":
                s = "2";
                break;
            case "PACK_TYPE_QUEST":
                s = "3";
                break;
            case "PACK_TYPE_HERO_CARD":
                s = "4";
                break;
            case "PACK_TYPE_MAX":
                s = "6";
                break;
            case "PACK_TYPE_CPASULE":
                s = "9";
                break;
        }
        DragDropButton dragDropButton = this.root.parent.GetComponent<DragDropButton>();
        if (dragDropButton == null)
        {
            dragDropButton = this.root.parent.gameObject.AddComponent<DragDropButton>();
        }
        int num2 = 0;
        if (int.TryParse(this.root.parent.gameObject.name, out num2))
        {
            num2--;
        }
        if (id != 0U)
        {
            dragDropButton.Initilize((UIRootType)uiRootType, new Vector2(0f, (float)num2), "Item/img_icon", new BagDragDropButtonData(id, thisidstr, (PackType)int.Parse(s), lock_end_time, this.root, this.monclickitem));
        }
        else
        {
            dragDropButton.Initilize((UIRootType)uiRootType, new Vector2(0f, (float)num2), "Item/img_icon", null);
        }
        dragDropButton.SetMaskShow(GlobalRegister.isMainUserDeathing);
        if (null != this.imgRepair)
        {
            this.imgRepair.gameObject.SetActive(false);
            LuaTable configTable = LuaConfigManager.GetConfigTable("carddata_config", (ulong)this.mId);
            if (configTable != null && this.mData.card_data != null)
            {
                uint field_Uint = configTable.GetField_Uint("maxdurable");
                if (this.mData.card_data.durability < field_Uint)
                {
                    this.imgRepair.transform.localScale = new Vector3(1f, 1f - this.mData.card_data.durability * 1f / field_Uint, 1f);
                    this.imgRepair.gameObject.SetActive(true);
                }
            }
        }
    }

    public void DragDropDataClear()
    {
        DragDropButton component = this.root.parent.GetComponent<DragDropButton>();
        if (component != null)
        {
            component.DataClear();
        }
    }

    public bool SetCommonItemInMainPackage(ulong thisid, Action<uint> onclickitem = null)
    {
        object[] array = LuaScriptMgr.Instance.CallLuaFunction("BagCtrl.MainPackageDicContainsKey", new object[]
        {
            Util.GetLuaTable("BagCtrl"),
            thisid
        });
        if (!(bool)array[0])
        {
            return false;
        }
        array = LuaScriptMgr.Instance.CallLuaFunction("BagCtrl.MainPackageDic", new object[]
        {
            Util.GetLuaTable("BagCtrl"),
            thisid
        });
        PropsBase propsBase = (PropsBase)array[0];
        this.SetCommonItem(propsBase.config.GetField_Uint("id"), propsBase.Count, onclickitem);
        return true;
    }

    public bool SetCommonItemInMainPackageInLua(string thisidstr, Action<uint> onclickitem = null)
    {
        ulong thisid = 0UL;
        return ulong.TryParse(thisidstr, out thisid) && this.SetCommonItemInMainPackage(thisid, onclickitem);
    }

    private void SetCommonItemBind(uint bind)
    {
        if (this.imgBind == null)
        {
            return;
        }
        switch (bind)
        {
            case 0U:
                this.imgBind.gameObject.SetActive(false);
                break;
            case 1U:
                this.imgBind.gameObject.SetActive(true);
                this.imgBind.color = Const.GetColorByName("bindwhite");
                break;
            case 2U:
                this.imgBind.gameObject.SetActive(true);
                this.imgBind.color = Const.GetColorByName("bindgrey");
                break;
            default:
                this.imgBind.gameObject.SetActive(false);
                break;
        }
    }

    private void SetCommonItemMask(bool canUseOrEquip)
    {
        if (!canUseOrEquip)
        {
            this.PlayCDAnim();
        }
        else
        {
            if (this.txtcd != null)
            {
                this.txtcd.text = string.Empty;
            }
            if (this.imgcd != null)
            {
                this.imgcd.fillAmount = 0f;
            }
        }
    }

    public void Dispose()
    {
        if (this.textureasset != null)
        {
            this.textureasset.TryUnload();
            this.textureasset = null;
        }
        Scheduler.Instance.RemoveTimer(new Scheduler.OnScheduler(this.PalyCDAnimCoolDown));
        Scheduler.Instance.RemoveFrame(new Scheduler.OnScheduler(this.PlayCDAnimDo));
    }

    public void btn_item_on_enter(PointerEventData eventData)
    {
        this.mCurEnterBtn = eventData.pointerCurrentRaycast.gameObject;
        if (MouseStateControoler.Instan.IsContinuedMouseState())
        {
            Scheduler.Instance.AddTimer(0.5f, false, new Scheduler.OnScheduler(this.TryShowItemTip));
        }
        else
        {
            this.TryShowItemTip();
        }
    }

    private t_Object GetData()
    {
        t_Object itemData = CommonTools.GetItemData(this.mThisid);
        if (itemData == null)
        {
            return new t_Object
            {
                baseid = this.mId,
                thisid = this.mThisid
            };
        }
        return itemData;
    }

    private void TryShowItemTip()
    {
        DragDropButton component = this.root.parent.GetComponent<DragDropButton>();
        if (component != null && component.mData != null)
        {
            LuaTable luaTable = null;
            object[] array = LuaScriptMgr.Instance.CallLuaFunction("BagCtrl.GetItemData", new object[]
            {
                this.mThisid
            });
            if (array.Length > 0)
            {
                luaTable = (array[0] as LuaTable);
            }
            if (luaTable == null)
            {
                ControllerManager.Instance.GetController<ItemTipController>().OpenPanel(new t_Object
                {
                    baseid = this.mId,
                    bind = this.mBind
                }, this.mCurEnterBtn);
            }
            else
            {
                ControllerManager.Instance.GetController<ItemTipController>().OpenPanel(array[0] as LuaTable, this.mCurEnterBtn);
            }
            this.objEffect1.SetActive(true);
        }
    }

    public void btn_item_on_exit(PointerEventData eventData)
    {
        Scheduler.Instance.RemoveTimer(new Scheduler.OnScheduler(this.TryShowItemTip));
        if (this.objEffect1 != null)
        {
            this.objEffect1.SetActive(false);
        }
    }

    public void PlayCDAnim()
    {
        if (this.cdtime <= 0)
        {
            return;
        }
        this.cdsecond = this.cdtime;
        this.cdprogress = (float)this.cdtime;
        this.txtcd.text = this.cdtime.ToString();
        Scheduler.Instance.AddTimer(1f, true, new Scheduler.OnScheduler(this.PalyCDAnimCoolDown));
        Scheduler.Instance.AddFrame(1U, true, new Scheduler.OnScheduler(this.PlayCDAnimDo));
    }

    private void PalyCDAnimCoolDown()
    {
        this.cdsecond--;
        this.txtcd.text = this.cdsecond.ToString();
        if (this.cdsecond <= 0)
        {
            this.txtcd.text = string.Empty;
            Scheduler.Instance.RemoveTimer(new Scheduler.OnScheduler(this.PalyCDAnimCoolDown));
        }
    }

    private void PlayCDAnimDo()
    {
        this.cdprogress -= Time.deltaTime;
        this.imgcd.fillAmount = this.cdprogress / this.cdtimeconfig;
        if (this.cdprogress <= 0f)
        {
            this.imgcd.fillAmount = 0f;
            Scheduler.Instance.RemoveFrame(new Scheduler.OnScheduler(this.PlayCDAnimDo));
        }
    }

    public uint GetBaseId()
    {
        return this.mId;
    }

    public Transform GetBtnItem()
    {
        return this.btnitem.transform;
    }

    private Transform root;

    private Button btnitem;

    private Image imgback;

    private Image imgicon;

    private RawImage qualityicon;

    private Text txtnum;

    private Text txtname;

    private Image imgBind;

    private Transform imgmask;

    private GameObject objEffect1;

    private Image imgcd;

    private Text txtcd;

    private Image imgRepair;

    private UITextureAsset textureasset;

    private Action<uint> monclickitem;

    private uint mId;

    private string mThisid = "0";

    private t_Object mData;

    private uint mBind;

    private GameObject mCurEnterBtn;

    private int cdtime;

    private float cdtimeconfig;

    private int cdsecond;

    private float cdprogress;
}
