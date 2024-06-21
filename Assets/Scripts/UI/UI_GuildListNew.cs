using System;
using System.Collections.Generic;
using Framework.Managers;
using guild;
using LuaInterface;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_GuildListNew : UIPanelBase
{
    public override void OnInit(Transform root)
    {
        this.ltGuildConfig = LuaConfigManager.GetXmlConfigTable("guildConfig");
        this.APPLY_TOTOAL_NUM = this.ltGuildConfig.GetField_Uint("maxApplyCountInTime");
        for (int i = 0; i < root.childCount; i++)
        {
            root.GetChild(i).gameObject.SetActive(false);
        }
        this.mController = ControllerManager.Instance.GetController<GuildControllerNew>();
        this.mRoot = root.Find("Panel_apply");
        this.inputfield_search = this.mRoot.Find("Panel_search/InputField").GetComponent<InputField>();
        this.btn_search = this.mRoot.Find("Panel_search/btn_search").GetComponent<Button>();
        this.trans_guilditem_parent = this.mRoot.Find("Panel_list/Scroll View/Viewport/Content");
        this.obj_guilditem = this.trans_guilditem_parent.Find("Panel_info").gameObject;
        this.lbl_applyNum = this.mRoot.Find("Panel_list/bottom/txt_apply/Text").GetComponent<Text>();
        this.lbl_pageNum = this.mRoot.Find("Panel_list/bottom/Panel_page/Text").GetComponent<Text>();
        this.btn_prepage = this.mRoot.Find("Panel_list/bottom/Panel_page/btn_previous").GetComponent<Button>();
        this.btn_nextpage = this.mRoot.Find("Panel_list/bottom/Panel_page/btn_next").GetComponent<Button>();
        GameObject gameObject = this.mRoot.Find("txt_creat/txt_npc").gameObject;
        UIEventListener.Get(gameObject).onClick = new UIEventListener.VoidDelegate(this.btn_findpath_onclick);
        this.btn_close = this.mRoot.Find("Panel_top/btn_close").GetComponent<Button>();
        this.btn_search.onClick.AddListener(new UnityAction(this.btn_search_onclick));
        this.btn_close.onClick.AddListener(new UnityAction(this.btn_close_onclick));
        this.btn_prepage.onClick.AddListener(new UnityAction(this.btn_prepage_onclick));
        this.btn_nextpage.onClick.AddListener(new UnityAction(this.btn_nextpage_onclick));
        base.OnInit(root);
    }

    private void SetPreNextPageBtnUIState()
    {
        this.btn_prepage.interactable = (this.mCurPageIndex != 1);
        this.btn_nextpage.interactable = (this.mCurPageIndex != this.mTotalPageNum);
    }

    public void GuildListCb()
    {
        this.SetupListPanel(null);
    }

    public void RefreshListPanel()
    {
        this.SetupListPanel(null);
    }

    private void SetupListPanel(List<guildListItem> guildItemList = null)
    {
        if (guildItemList == null)
        {
            guildItemList = this.mController.GetGuildItemList();
        }
        UIManager.Instance.ClearListChildrens(this.trans_guilditem_parent, 1);
        this.applyNum = 0;
        for (int i = 0; i < guildItemList.Count; i++)
        {
            guildListItem guildListItem = guildItemList[i];
            if ((this.mCurPageIndex - 1) * 10 <= i && i < this.mCurPageIndex * 10)
            {
                GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.obj_guilditem);
                gameObject.name = guildListItem.guild.guildid.ToString();
                Text component = gameObject.transform.Find("txt_id").GetComponent<Text>();
                Text component2 = gameObject.transform.Find("txt_id/txt_name").GetComponent<Text>();
                Text component3 = gameObject.transform.Find("txt_lv").GetComponent<Text>();
                Text component4 = gameObject.transform.Find("txt_leader").GetComponent<Text>();
                Text component5 = gameObject.transform.Find("txt_num").GetComponent<Text>();
                Button component6 = gameObject.transform.Find("btn_apply").GetComponent<Button>();
                Button component7 = gameObject.transform.Find("btn_cancel").GetComponent<Button>();
                component.text = guildListItem.guild.guildid.ToString();
                component2.text = guildListItem.guild.guildname;
                component3.text = guildListItem.guild.guildlevel.ToString();
                component4.text = guildListItem.guild.mastername;
                component5.text = guildListItem.guild.sumcount + "/" + guildListItem.guild.countlimit;
                component6.gameObject.SetActive(guildListItem.guildtype == 0U || guildListItem.guildtype == 3U);
                component7.gameObject.SetActive(guildListItem.guildtype == 2U);
                component6.onClick.AddListener(delegate ()
                {
                    this.mController.ApplyGuild(guildListItem.guild.guildid, true);
                });
                component7.onClick.AddListener(delegate ()
                {
                    this.mController.ApplyGuild(guildListItem.guild.guildid, false);
                });
                gameObject.transform.SetParent(this.trans_guilditem_parent);
                gameObject.transform.localScale = Vector3.one;
                gameObject.SetActive(true);
            }
            if (guildListItem.guildtype == 2U)
            {
                this.applyNum++;
            }
        }
        this.lbl_applyNum.text = this.applyNum + "/" + this.APPLY_TOTOAL_NUM;
        this.mTotalPageNum = guildItemList.Count / 10 + ((guildItemList.Count % 10 != 0) ? 1 : 0);
        if (this.mTotalPageNum == 0)
        {
            this.mTotalPageNum = 1;
        }
        this.lbl_pageNum.text = this.mCurPageIndex + "/" + this.mTotalPageNum;
        this.SetPreNextPageBtnUIState();
        this.mRoot.gameObject.SetActive(true);
    }

    public void ApplyGuildCb(ulong guildid, bool isApplyNotCancel, bool issucc)
    {
        if (issucc)
        {
            Transform transform = this.trans_guilditem_parent.Find(guildid.ToString());
            transform.Find("btn_apply").gameObject.SetActive(!isApplyNotCancel);
            transform.Find("btn_cancel").gameObject.SetActive(isApplyNotCancel);
            if (isApplyNotCancel)
            {
                this.applyNum++;
            }
            else
            {
                this.applyNum--;
            }
            this.lbl_applyNum.text = this.applyNum + "/" + this.APPLY_TOTOAL_NUM;
        }
    }

    private void btn_search_onclick()
    {
        this.mCurPageIndex = 1;
        string text = this.inputfield_search.text;
        if (string.IsNullOrEmpty(text))
        {
            this.SetupListPanel(null);
        }
        else
        {
            List<guildListItem> guildItemList = this.mController.GetGuildItemList();
            for (int i = 0; i < guildItemList.Count; i++)
            {
                if (guildItemList[i].guild.guildid.ToString() == text || guildItemList[i].guild.guildname.Contains(text))
                {
                    this.SetupListPanel(new List<guildListItem>
                    {
                        guildItemList[i]
                    });
                    return;
                }
            }
            this.SetupListPanel(new List<guildListItem>());
        }
    }

    private void btn_findpath_onclick(PointerEventData eventData)
    {
        if (ControllerManager.Instance.GetController<TaskUIController>() == null)
        {
            return;
        }
        uint pathWayId = CommonTools.GetPathWayId(695UL, 2099U);
        GlobalRegister.PathFindWithPathWayId(pathWayId);
    }

    private void btn_close_onclick()
    {
        UIManager.Instance.DeleteUI<UI_GuildListNew>();
    }

    private void btn_prepage_onclick()
    {
        this.mCurPageIndex--;
        this.SetPreNextPageBtnUIState();
        this.RefreshListPanel();
    }

    private void btn_nextpage_onclick()
    {
        this.mCurPageIndex++;
        this.SetPreNextPageBtnUIState();
        this.RefreshListPanel();
    }

    public override void OnDispose()
    {
        base.OnDispose();
    }

    private const int ONE_PAGE_ITEM_NUM = 10;

    private GuildControllerNew mController;

    private Transform mRoot;

    private InputField inputfield_search;

    private Button btn_search;

    private Transform trans_guilditem_parent;

    private GameObject obj_guilditem;

    private Button btn_close;

    private Text lbl_applyNum;

    private Text lbl_pageNum;

    private Button btn_prepage;

    private Button btn_nextpage;

    private LuaTable ltGuildConfig;

    private uint APPLY_TOTOAL_NUM = 10U;

    private int mCurPageIndex = 1;

    private int mTotalPageNum = 1;

    private int applyNum;
}
