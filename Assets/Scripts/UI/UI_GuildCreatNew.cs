using System;
using Framework.Managers;
using LuaInterface;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_GuildCreatNew : UIPanelBase
{
    public override void OnInit(Transform root)
    {
        for (int i = 0; i < root.childCount; i++)
        {
            root.GetChild(i).gameObject.SetActive(false);
        }
        root.Find("Panel_creat").gameObject.SetActive(true);
        this.mController = ControllerManager.Instance.GetController<GuildControllerNew>();
        this.mRoot = root.Find("Panel_creat");
        this.trans_level_warn = this.mRoot.Find("txt_warning");
        this.obj_icon_item = this.mRoot.Find("Scroll View/Viewport/Content/img_icon").gameObject;
        this.lbl_need_player_level = this.mRoot.Find("txt_lv/Text").GetComponent<Text>();
        this.img_need_item_icon = this.mRoot.Find("txt_creat/img_coin").GetComponent<RawImage>();
        this.lbl_need_item_num = this.mRoot.Find("txt_creat/Text").GetComponent<Text>();
        this.img_icon = this.mRoot.Find("Panel_set/img_badge").GetComponent<RawImage>();
        this.iputfield_name = this.mRoot.Find("Panel_set/Input_name").GetComponent<InputField>();
        this.btn_creat = this.mRoot.Find("btn_creat").GetComponent<Button>();
        this.btn_cancel = this.mRoot.Find("Panel_top/btn_close").GetComponent<Button>();
        this.btn_creat.onClick.AddListener(new UnityAction(this.btn_create_onclick));
        this.btn_cancel.onClick.AddListener(new UnityAction(this.btn_cancel_onclick));
        this.ltGuildConfig = LuaConfigManager.GetXmlConfigTable("guildConfig");
        this.LEVEL_LIMIT = CommonTools.GetTextById(643UL);
        this.COST_LIMIT = CommonTools.GetTextById(644UL);
        this.TEXT_LONG_LIMIT = CommonTools.GetTextById(638UL);
        this.TEXT_SHORT_LIMIT = CommonTools.GetTextById(639UL);
        this.TEXT_MINGAN_LIMIT = CommonTools.GetTextById(640UL);
        this.SetupPanel();
        base.OnInit(root);
    }

    private void SetupPanel()
    {
        UIManager.Instance.ClearListChildrens(this.obj_icon_item.transform.parent, 1);
        for (int i = 1; i < 9; i++)
        {
            GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.obj_icon_item);
            RawImage component = gameObject.transform.GetChild(0).GetComponent<RawImage>();
            UIManager.Instance.SetRawImage(ImageType.ICON, component, "guild" + i);
            gameObject.transform.SetParent(this.obj_icon_item.transform.parent);
            gameObject.transform.localScale = Vector3.one;
            gameObject.SetActive(true);
            UIEventListener.Get(gameObject).onClick = new UIEventListener.VoidDelegate(this.btn_icon_onclick);
        }
        UIManager.Instance.SetRawImage(ImageType.ICON, this.img_icon, "guild1");
        this.trans_level_warn.gameObject.SetActive(MainPlayer.Self.GetCurLevel() < this.ltGuildConfig.GetField_Uint("createLevel"));
        this.lbl_need_player_level.text = this.ltGuildConfig.GetField_Uint("createLevel").ToString();
        this.lbl_need_player_level.color = ((MainPlayer.Self.GetCurLevel() >= this.ltGuildConfig.GetField_Uint("createLevel")) ? Color.white : Color.red);
        uint field_Uint = this.ltGuildConfig.GetCacheField_Table("createCost").GetField_Uint("objid");
        uint field_Uint2 = this.ltGuildConfig.GetCacheField_Table("createCost").GetField_Uint("objnum");
        LuaTable configTable = LuaConfigManager.GetConfigTable("objects", (ulong)field_Uint);
        if (configTable != null)
        {
            UIManager.Instance.SetRawImage(ImageType.ITEM, this.img_need_item_icon, configTable.GetField_String("icon"));
        }
        this.lbl_need_item_num.text = field_Uint2.ToString();
        this.lbl_need_item_num.color = ((MainPlayer.Self.GetCurrencyByID(field_Uint) >= field_Uint2) ? Color.white : Color.red);
    }

    private void btn_icon_onclick(PointerEventData eventData)
    {
        string name = eventData.pointerPress.transform.GetChild(0).GetComponent<RawImage>().mainTexture.name;
        if (name != this.img_icon.GetComponent<RawImage>().mainTexture.name)
        {
            UIManager.Instance.SetRawImage(ImageType.ICON, this.img_icon, name);
        }
    }

    private void btn_create_onclick()
    {
        if (this.CostEnoughCheck() && this.GuildNameValid(this.iputfield_name.text))
        {
            this.mController.CreateGuild(this.iputfield_name.text, this.img_icon.texture.name);
        }
    }

    private bool CostEnoughCheck()
    {
        if (MainPlayer.Self.GetCurLevel() < this.ltGuildConfig.GetField_Uint("createLevel"))
        {
            TipsWindow.ShowWindow(this.LEVEL_LIMIT);
            return false;
        }
        uint field_Uint = this.ltGuildConfig.GetCacheField_Table("createCost").GetField_Uint("objid");
        uint field_Uint2 = this.ltGuildConfig.GetCacheField_Table("createCost").GetField_Uint("objnum");
        if (MainPlayer.Self.GetCurrencyByID(field_Uint) < field_Uint2)
        {
            TipsWindow.ShowWindow(this.COST_LIMIT);
            return false;
        }
        return true;
    }

    private bool GuildNameValid(string name)
    {
        if (string.Compare(name, KeyWordFilter.ChatFilter(name)) != 0)
        {
            TipsWindow.ShowWindow(TipsType.HAVE_SENSITIVE, null);
            return false;
        }
        return true;
    }

    private void btn_cancel_onclick()
    {
        UIManager.Instance.DeleteUI<UI_GuildCreatNew>();
    }

    public override void OnDispose()
    {
        base.OnDispose();
    }

    private const string ICON_PRE = "guild";

    private GuildControllerNew mController;

    private Transform mRoot;

    private Transform trans_level_warn;

    private GameObject obj_icon_item;

    private Text lbl_need_player_level;

    private RawImage img_need_item_icon;

    private Text lbl_need_item_num;

    private RawImage img_icon;

    private InputField iputfield_name;

    private Button btn_creat;

    private Button btn_cancel;

    private LuaTable ltGuildConfig;

    private string LEVEL_LIMIT = string.Empty;

    private string COST_LIMIT = string.Empty;

    private string TEXT_LONG_LIMIT = string.Empty;

    private string TEXT_SHORT_LIMIT = string.Empty;

    private string TEXT_MINGAN_LIMIT = string.Empty;
}
