using System;
using System.Collections.Generic;
using System.Text;
using Framework.Managers;
using guild;
using LuaInterface;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_GuildInfoNew : UIPanelBase
{
    public override void OnInit(Transform _root)
    {
        this.root = _root;
        this.mController = ControllerManager.Instance.GetController<GuildControllerNew>();
        this.mGuildMemberSecondMenu = new GuildMemberSecondMenu(this, this.root.Find("Panel_secondmenu"));
        for (int i = 0; i < this.root.childCount; i++)
        {
            this.root.GetChild(i).gameObject.SetActive(false);
        }
        this.mRoot = this.root.Find("Panel_family");
        Transform transform = this.mRoot.Find("Panel_staff/Panel_info");
        this.img_icon = transform.Find("Image/img_icon").GetComponent<RawImage>();
        this.txt_guildname = transform.Find("txt_name").GetComponent<Text>();
        this.txt_guildid = transform.Find("txt_id/Text").GetComponent<Text>();
        this.txt_guildlevel = transform.Find("txt_lv").GetComponent<Text>();
        this.slider_guildexp = transform.Find("txt_lv/Slider_lv").GetComponent<Slider>();
        this.txt_guildposition = transform.Find("txt_position/Text").GetComponent<Text>();
        this.txt_guildcontribution = transform.Find("txt_contribution/Text").GetComponent<Text>();
        this.txt_guildcontribution.gameObject.AddComponent<TextTip>().SetTextCb(new TextTipContentCb(this.GetMyContributeTipContent));
        this.txt_guildmoney = transform.Find("txt_wealth/Text").GetComponent<Text>();
        this.txt_ownnum = this.mRoot.Find("Panel_staff/Panel_list/bottom/txt_num/ownnum").GetComponent<Text>();
        this.txt_limitnum = this.mRoot.Find("Panel_staff/Panel_list/bottom/txt_num/limitnum").GetComponent<Text>();
        this.obj_memberitem_parent = this.mRoot.Find("Panel_staff/Panel_list/Scroll View/Viewport/Content");
        this.obj_memberitem = this.obj_memberitem_parent.Find("Panel_info").gameObject;
        this.tgl_online = this.mRoot.Find("Panel_staff/Panel_list/bottom/Toggle").GetComponent<Toggle>();
        this.tgl_online.isOn = (PlayerPrefs.GetInt("guildMemberOnline", 0) == 1);
        Button component = this.mRoot.Find("Panel_staff/Panel_list/title/0/btn_up").GetComponent<Button>();
        Button component2 = this.mRoot.Find("Panel_staff/Panel_list/title/0/btn_down").GetComponent<Button>();
        Button component3 = this.mRoot.Find("Panel_staff/Panel_list/title/2/btn_up").GetComponent<Button>();
        Button component4 = this.mRoot.Find("Panel_staff/Panel_list/title/2/btn_down").GetComponent<Button>();
        Button component5 = this.mRoot.Find("Panel_staff/Panel_list/title/1/btn_up").GetComponent<Button>();
        Button component6 = this.mRoot.Find("Panel_staff/Panel_list/title/1/btn_down").GetComponent<Button>();
        Button component7 = this.mRoot.Find("Panel_staff/Panel_list/title/3/btn_up").GetComponent<Button>();
        Button component8 = this.mRoot.Find("Panel_staff/Panel_list/title/3/btn_down").GetComponent<Button>();
        UIEventListener.Get(component.gameObject).onClick = new UIEventListener.VoidDelegate(this.btn_sortup_onclick);
        UIEventListener.Get(component3.gameObject).onClick = new UIEventListener.VoidDelegate(this.btn_sortup_onclick);
        UIEventListener.Get(component5.gameObject).onClick = new UIEventListener.VoidDelegate(this.btn_sortup_onclick);
        UIEventListener.Get(component7.gameObject).onClick = new UIEventListener.VoidDelegate(this.btn_sortup_onclick);
        UIEventListener.Get(component2.gameObject).onClick = new UIEventListener.VoidDelegate(this.btn_sortdown_onclick);
        UIEventListener.Get(component4.gameObject).onClick = new UIEventListener.VoidDelegate(this.btn_sortdown_onclick);
        UIEventListener.Get(component6.gameObject).onClick = new UIEventListener.VoidDelegate(this.btn_sortdown_onclick);
        UIEventListener.Get(component8.gameObject).onClick = new UIEventListener.VoidDelegate(this.btn_sortdown_onclick);
        Button component9 = transform.Find("btn_signout").GetComponent<Button>();
        component9.onClick.AddListener(new UnityAction(this.btn_exitguild_onclick));
        this.inputfield_notice = transform.Find("Panel_notice/input").GetComponent<InputField>();
        this.inputfield_notice.onValueChanged.AddListener(new UnityAction<string>(this.inputfield_notice_onvaluechanged));
        this.noticeTextLimit = transform.Find("Panel_notice/btn_confirm/TextLimit").GetComponent<Text>();
        this.btn_edit = transform.Find("Panel_notice/btn_edit").GetComponent<Button>();
        this.btn_edit.onClick.AddListener(new UnityAction(this.btn_edit_onclick));
        this.btn_noticesure = transform.Find("Panel_notice/btn_confirm").GetComponent<Button>();
        this.btn_noticesure.onClick.AddListener(new UnityAction(this.btn_noticesure_onclick));
        this.btn_noticecancel = transform.Find("Panel_notice/btn_cancel").GetComponent<Button>();
        this.btn_noticecancel.onClick.AddListener(new UnityAction(this.btn_noticecancel_onclick));
        Button component10 = transform.Find("txt_wealth/btn_donate").GetComponent<Button>();
        component10.onClick.AddListener(new UnityAction(this.btn_donate_onclick));
        this.trans_donate = this.mRoot.Find("Panel_donate");
        this.inputfield_donate = this.trans_donate.Find("InputField").GetComponent<InputField>();
        this.txt_donate_money_own = this.trans_donate.Find("panel_have/txt_value").GetComponent<Text>();
        Button component11 = this.trans_donate.Find("btn_ok").GetComponent<Button>();
        Button component12 = this.trans_donate.Find("btn_cancel").GetComponent<Button>();
        component11.onClick.AddListener(new UnityAction(this.btn_donate_sure_onclick));
        component12.onClick.AddListener(new UnityAction(this.btn_donate_cancel_onclick));
        this.txt_guildmoney.gameObject.AddComponent<TextTip>().SetTextCb(new TextTipContentCb(this.GetGuildMoneyTipContent));
        Button component13 = this.mRoot.Find("Panel_top/btn_close").GetComponent<Button>();
        component13.onClick.AddListener(new UnityAction(this.btn_close_onclick));
        this.btn_positionset = this.mRoot.Find("Panel_toggle/btn_position").GetComponent<Button>();
        this.btn_positionset.onClick.AddListener(new UnityAction(this.btn_positionset_onclick));
        this.btn_positionsetclose = this.root.Find("Panel_positionadd/Panel_top/btn_close").GetComponent<Button>();
        this.btn_positionsetclose.onClick.AddListener(new UnityAction(this.btn_positionclose_onclick));
        this.btn_positionsetadd = this.root.Find("Panel_positionadd/Panel_top/btn_add").GetComponent<Button>();
        this.btn_positionsetadd.onClick.AddListener(new UnityAction(this.btn_positionadd_onclick));
        this.trans_positionsetitemparent = this.root.Find("Panel_positionadd/Panel_list/Scroll View/Viewport/Content");
        this.obj_positionsetitem = this.trans_positionsetitemparent.Find("Panel_info").gameObject;
        this.btn_positionadd_sure = this.root.Find("Panel_positionset/btn_ok").GetComponent<Button>();
        this.btn_positionadd_sure.onClick.AddListener(new UnityAction(this.btn_positionadd_sure_onclick));
        this.btn_positionadd_cancel = this.root.Find("Panel_positionset/btn_cancel").GetComponent<Button>();
        this.btn_positionadd_cancel.onClick.AddListener(new UnityAction(this.btn_positionadd_cancel_onclick));
        this.btn_applylist = this.mRoot.Find("Panel_toggle/btn_apply").GetComponent<Button>();
        this.btn_applylist.onClick.AddListener(new UnityAction(this.btn_applylist_onclick));
        this.trans_applyconfirm = this.root.Find("Panel_applyconfirm");
        Button component14 = this.trans_applyconfirm.Find("Panel_top/btn_close").GetComponent<Button>();
        component14.onClick.AddListener(new UnityAction(this.btn_applyconfirmclose_onclick));
        this.tgl_applylist = this.trans_applyconfirm.Find("Panel_list/bottom/Toggle").GetComponent<Toggle>();
        this.tgl_applylist.onValueChanged.AddListener(new UnityAction<bool>(this.tgl_applylist_onvaluechanged));
        Button component15 = this.trans_applyconfirm.Find("Panel_list/title/txt2/btn_up").GetComponent<Button>();
        Button component16 = this.trans_applyconfirm.Find("Panel_list/title/txt2/btn_down").GetComponent<Button>();
        component15.onClick.AddListener(new UnityAction(this.btn_levelupsort_onclick));
        component16.onClick.AddListener(new UnityAction(this.btn_leveldownsort_onclick));
        this.ltGuildConfig = LuaConfigManager.GetXmlConfigTable("guildConfig");
        this.toggleGuidSkill = this.root.Find("Panel_family/Panel_toggle/Panel_tab/skill").GetComponent<Toggle>();
        this.toggleGuidSkill.onValueChanged.AddListener(new UnityAction<bool>(this.OnSkillToggleValueChanged));
        this.guildSkillPanel = this.root.Find("Panel_family/Panel_staff/SkillPanel");
        this.guildSkillPanel.gameObject.SetActive(false);
        this.mRoot.Find("Panel_staff/Panel_list").gameObject.SetActive(true);
        base.OnInit(this.root);
    }

    public void TryGetContributeSkillLv()
    {
        this.mController.TryGetGuildSkillLv(delegate (uint lv)
        {
            this.contributeSkillLv = lv;
        }, 3U);
    }

    private void inputfield_notice_onvaluechanged(string s)
    {
        string text = string.Empty;
        float num = 0f;
        int field_Uint = (int)this.ltGuildConfig.GetField_Uint("announceiLimit");
        for (int i = 0; i < s.Length; i++)
        {
            int num2 = Encoding.UTF8.GetBytes(s[i].ToString()).Length;
            if (num2 > 1)
            {
                num += 1f;
            }
            else
            {
                num += 0.5f;
            }
            if (num > (float)field_Uint)
            {
                this.inputfield_notice.text = text;
                return;
            }
            text += s[i].ToString();
        }
        this.noticeTextLimit.text = Mathf.Floor(num) + "/" + this.ltGuildConfig.GetField_Uint("announceiLimit");
    }

    private void btn_close_onclick()
    {
        UIManager.Instance.DeleteUI<UI_GuildInfoNew>();
    }

    private string GetGuildMoneyTipContent()
    {
        string text = "每日维护资金为" + this.levelConfig.GetField_Uint("guild_daily_maintain_fund");
        if (this.mGuildInfo.salary_rest_day != 0U)
        {
            string text2 = text;
            text = string.Concat(new object[]
            {
                text2,
                "\r\n<color=#ff0000ff>还有",
                this.mGuildInfo.salary_rest_day,
                "天家族将自动解散</color>"
            });
        }
        return text;
    }

    private string GetMyContributeTipContent()
    {
        string format = "我的今日贡献度" + this.mDailyContribute + "/{0}";
        List<LuaTable> configTableList = LuaConfigManager.GetConfigTableList("skill_guild");
        uint num = this.ltGuildConfig.GetField_Uint("contriLimit");
        LuaTable guildSkillConfigByID = this.mController.GetGuildSkillConfigByID(300U + this.contributeSkillLv);
        if (guildSkillConfigByID != null)
        {
            string cacheField_String = guildSkillConfigByID.GetCacheField_String("skillstaus");
            string[] array = cacheField_String.Split(new char[]
            {
                '-'
            });
            if (array.Length > 1)
            {
                num += uint.Parse(array[1]);
            }
        }
        return string.Format(format, num);
    }

    public void DailyContributeCb(uint value)
    {
        this.mDailyContribute = value;
    }

    public void SetupInfo(guildInfo guildInfo, guildMember myinfo)
    {
        this.mGuildInfo = guildInfo;
        this.mMyInfo = myinfo;
        this.txt_limitnum.text = this.mGuildInfo.countlimit.ToString();
        this.btn_positionset.gameObject.SetActive(this.mController.IsGuildMaster());
        UIManager.Instance.SetRawImage(ImageType.ICON, this.img_icon, guildInfo.icon);
        this.txt_guildname.text = guildInfo.guildname;
        this.txt_guildid.text = guildInfo.guildid.ToString();
        this.txt_guildlevel.text = CommonTools.GetLevelFormat(guildInfo.guildlevel);
        this.levelConfig = this.mController.GetLevelConfigByLevel(guildInfo.guildlevel);
        uint num = guildInfo.exp;
        uint num2 = this.levelConfig.GetField_Uint("guild_levelup_exp");
        Transform transform = this.slider_guildexp.transform.Find("Text");
        if (num2 == 0U)
        {
            if (transform != null)
            {
                transform.GetComponent<Text>().text = "MAX";
                this.slider_guildexp.value = num * 1f / num2;
            }
            this.slider_guildexp.value = 1f;
        }
        else
        {
            LuaTable levelConfigByLevel = this.mController.GetLevelConfigByLevel(guildInfo.guildlevel - 1U);
            if (levelConfigByLevel != null)
            {
                num -= levelConfigByLevel.GetField_Uint("guild_levelup_exp");
                num2 -= levelConfigByLevel.GetField_Uint("guild_levelup_exp");
            }
            if (transform != null)
            {
                transform.GetComponent<Text>().text = num + "/" + num2;
            }
            this.slider_guildexp.value = num * 1f / num2;
        }
        this.txt_guildposition.text = this.mController.GetPositionInfo(myinfo.positionid).name;
        this.txt_guildcontribution.text = myinfo.contribute.ToString();
        this.txt_guildmoney.text = guildInfo.salary.ToString();
        this.inputfield_notice.text = guildInfo.notify;
        this.inputfield_notice.characterLimit = 0;
        if (this.mGuildInfo.salary_rest_day != 0U)
        {
            this.txt_guildmoney.color = Color.red;
            if (!this.isDonateMsgboxOpened)
            {
                string s_describle = string.Format(CommonTools.GetTextById(653UL), guildInfo.salary_rest_day);
                ControllerManager.Instance.GetController<MsgBoxController>().ShowMsgBox("提示", s_describle, "捐赠", "取消", UIManager.ParentType.Tips, new Action(this.btn_donate_onclick), null, null);
                this.isDonateMsgboxOpened = true;
            }
        }
        else
        {
            this.txt_guildmoney.color = Color.white;
        }
        this.btn_edit.gameObject.SetActive(this.mController.GetPositionGuildPrivilegeSelf(GuildPrivilege.GUILDPRI_MODIFY_NOTIFY));
        this.mRoot.gameObject.SetActive(true);
        Transform transform2 = this.obj_memberitem_parent.Find(this.mMyInfo.memberid.ToString());
        if (transform2 != null)
        {
            Text component = transform2.Find("txt_contribution").GetComponent<Text>();
            Text component2 = transform2.Find("donate/Text").GetComponent<Text>();
            component.text = this.mMyInfo.contribute.ToString();
            component2.text = this.mMyInfo.donatesalary.ToString();
            this.mController.UpdateListMemberData(this.mMyInfo);
        }
        this.UpdateFamilyContributeValue(MainPlayer.Self.GetCurrencyByID(11U) + string.Empty);
        LuaTable configTable = LuaConfigManager.GetConfigTable("objects", 11UL);
        Image img = this.mRoot.Find("Panel_staff/Panel_info/txt_familycoin/Panel/Image").GetComponent<Image>();
        if (configTable != null)
        {
            string cacheField_String = configTable.GetCacheField_String("icon");
            ManagerCenter.Instance.GetManager<UITextureMgr>().GetTexture(ImageType.ITEM, cacheField_String, delegate (UITextureAsset asset)
            {
                if (img == null)
                {
                    return;
                }
                Texture2D textureObj = asset.textureObj;
                Sprite overrideSprite = Sprite.Create(textureObj, new Rect(0f, 0f, (float)textureObj.width, (float)textureObj.height), new Vector2(0f, 0f));
                img.overrideSprite = overrideSprite;
                img.material = null;
                this.usedTextureAssets.Add(asset);
            });
        }
        img.raycastTarget = true;
        HoverEventListener.Get(img.gameObject).onEnter = delegate (PointerEventData pd)
        {
            ItemTipController controller = ControllerManager.Instance.GetController<ItemTipController>();
            controller.OpenPanel(11U, img.gameObject);
        };
    }

    public void UpdateFamilyContributeValue(string s)
    {
        Text component = this.mRoot.Find("Panel_staff/Panel_info/txt_familycoin/Panel/Text").GetComponent<Text>();
        component.text = s;
    }

    public void RefreshListPanel()
    {
        this.SetupListPanel(this.mController.GetNormalMembers());
    }

    private void tgl_online_onvaluechanged(bool isOn)
    {
        PlayerPrefs.SetInt("guildMemberOnline", (!isOn) ? 0 : 1);
        this.RefreshListPanel();
    }

    public void SetupListPanel(List<guildMember> members)
    {
        List<GuildCompare> compareRule = this.GetCompareRule(this.curCompareType, this.curCompareUp);
        for (int i = compareRule.Count - 1; i >= 0; i--)
        {
            members.Sort(compareRule[i]);
        }
        this.objItemList = new List<GameObject>();
        UIManager.Instance.ClearListChildrens(this.obj_memberitem_parent, 1);
        int num = 0;
        for (int j = 0; j < members.Count; j++)
        {
            guildMember guildMember = members[j];
            if (guildMember.isonline)
            {
                num++;
            }
            if (!this.tgl_online.isOn || guildMember.isonline)
            {
                GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.obj_memberitem);
                gameObject.name = guildMember.memberid.ToString();
                gameObject.transform.SetParent(this.obj_memberitem_parent);
                gameObject.transform.localScale = Vector3.one;
                gameObject.gameObject.SetActive(true);
                this.objItemList.Add(gameObject);
                UIEventListener.Get(gameObject).onClick = new UIEventListener.VoidDelegate(this.btn_member_onrightclick);
                Image component = gameObject.GetComponent<Image>();
                if (component != null)
                {
                    component.color = ((j % 2 != 0) ? new Color(component.color.r, component.color.g, component.color.b, 0f) : new Color(component.color.r, component.color.g, component.color.b, 1f));
                }
                Text component2 = gameObject.transform.Find("txt_name").GetComponent<Text>();
                Text component3 = gameObject.transform.Find("txt_position").GetComponent<Text>();
                Text component4 = gameObject.transform.Find("txt_lv").GetComponent<Text>();
                Text component5 = gameObject.transform.Find("txt_contribution").GetComponent<Text>();
                Text component6 = gameObject.transform.Find("txt_online").GetComponent<Text>();
                Text component7 = gameObject.transform.Find("txt_offline").GetComponent<Text>();
                Text component8 = gameObject.transform.Find("donate/Text").GetComponent<Text>();
                component2.text = guildMember.membername;
                component3.text = this.mController.GetGuildPositionInfo(guildMember.positionid).name;
                component4.text = guildMember.memberlevel.ToString();
                component5.text = guildMember.contribute.ToString();
                component6.gameObject.SetActive(guildMember.isonline);
                component7.gameObject.SetActive(!guildMember.isonline);
                component7.text = CommonTools.GetOfflineText(guildMember.lastonlinetime);
                component8.text = guildMember.donatesalary.ToString();
            }
        }
        this.txt_ownnum.text = members.Count.ToString();
        this.tgl_online.onValueChanged.RemoveAllListeners();
        this.tgl_online.onValueChanged.AddListener(new UnityAction<bool>(this.tgl_online_onvaluechanged));
    }

    private List<GuildCompare> GetCompareRule(UI_GuildInfoNew.CompareType type, bool isUp)
    {
        List<GuildCompare> list = new List<GuildCompare>();
        GuildCompare item = null;
        if (isUp)
        {
            switch (type)
            {
                case UI_GuildInfoNew.CompareType.Position:
                    item = new PositionUpComparer();
                    break;
                case UI_GuildInfoNew.CompareType.Contribute:
                    item = new ContributeUpComparer();
                    break;
                case UI_GuildInfoNew.CompareType.Donate:
                    item = new DonateUpComparer();
                    break;
                case UI_GuildInfoNew.CompareType.Level:
                    item = new MemberLevelUpComparer();
                    break;
            }
        }
        else
        {
            switch (type)
            {
                case UI_GuildInfoNew.CompareType.Position:
                    item = new PositionDownComparer();
                    break;
                case UI_GuildInfoNew.CompareType.Contribute:
                    item = new ContributeDownComparer();
                    break;
                case UI_GuildInfoNew.CompareType.Donate:
                    item = new DonateDownComparer();
                    break;
                case UI_GuildInfoNew.CompareType.Level:
                    item = new MemberLevelDownComparer();
                    break;
            }
        }
        list.Add(item);
        for (int i = 0; i < this.mCompareDefaultArr.Length; i++)
        {
            if (i != (int)type)
            {
                list.Add(this.mCompareDefaultArr[i]);
            }
        }
        list.Add(new TimeUpCompare());
        return list;
    }

    private void btn_sortup_onclick(PointerEventData eventData)
    {
        this.curCompareType = (UI_GuildInfoNew.CompareType)int.Parse(eventData.pointerPress.transform.parent.name);
        this.curCompareUp = true;
        this.RefreshListPanel();
    }

    private void btn_sortdown_onclick(PointerEventData eventData)
    {
        this.curCompareType = (UI_GuildInfoNew.CompareType)int.Parse(eventData.pointerPress.transform.parent.name);
        this.curCompareUp = false;
        this.RefreshListPanel();
    }

    private void btn_edit_onclick()
    {
        this.oldNotice = this.inputfield_notice.text;
        this.SetNoticeEditUIState(true);
    }

    private void btn_noticesure_onclick()
    {
        string text = this.inputfield_notice.text;
        if (string.Compare(text, KeyWordFilter.ChatFilter(text)) != 0)
        {
            TipsWindow.ShowWindow(TipsType.HAVE_SENSITIVE, null);
            return;
        }
        this.mController.SetGuildNotify(text);
        this.SetNoticeEditUIState(false);
    }

    private void btn_noticecancel_onclick()
    {
        this.inputfield_notice.text = this.oldNotice;
        this.SetNoticeEditUIState(false);
    }

    private void SetNoticeEditUIState(bool edit)
    {
        this.inputfield_notice.interactable = edit;
        this.btn_noticesure.gameObject.SetActive(edit);
        this.btn_noticecancel.gameObject.SetActive(edit);
    }

    public void SetGuildNotifyCb(bool issucc)
    {
        if (!issucc)
        {
            this.inputfield_notice.text = this.oldNotice;
        }
    }

    private void btn_donate_onclick()
    {
        this.txt_donate_money_own.text = MainPlayer.Self.GetCurrencyByID(3U).ToString();
        this.trans_donate.gameObject.SetActive(true);
    }

    private void btn_donate_sure_onclick()
    {
        uint num = 0U;
        if (uint.TryParse(this.inputfield_donate.text, out num))
        {
            if (num <= MainPlayer.Self.GetCurrencyByID(3U))
            {
                this.mController.DonateGuild(num);
                this.trans_donate.gameObject.SetActive(false);
            }
            else
            {
                TipsWindow.ShowWindow(CommonTools.GetTextById(656UL));
            }
        }
        else
        {
            TipsWindow.ShowWindow(CommonTools.GetTextById(655UL));
        }
    }

    private void btn_donate_cancel_onclick()
    {
        this.trans_donate.gameObject.SetActive(false);
    }

    private void btn_member_onrightclick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            guildMember member = this.mController.GetMember(ulong.Parse(eventData.pointerPress.name));
            if (member.memberid != MainPlayer.Self.OtherPlayerData.MapUserData.charid)
            {
                this.mGuildMemberSecondMenu.OpenPanel(eventData, member);
            }
        }
    }

    private void btn_exitguild_onclick()
    {
        this.mController.ExitGuild();
    }

    private void btn_positionset_onclick()
    {
        this.RefreshPositionPanel();
        this.root.Find("Panel_positionadd").gameObject.SetActive(true);
    }

    public void RefreshPositionPanel()
    {
        Transform transform = this.root.Find("Panel_positionadd");
        Text component = transform.Find("Panel_top/txt_position/Text").GetComponent<Text>();
        List<GuildPositionInfo> guildPositionInfoList = this.mController.GetGuildPositionInfoList();
        component.text = guildPositionInfoList.Count + "/" + this.ltGuildConfig.GetCacheField_Table("positionmaxnum").GetField_Uint("num");
        UIManager.Instance.ClearListChildrens(this.trans_positionsetitemparent, 1);
        for (int i = 0; i < guildPositionInfoList.Count; i++)
        {
            GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.obj_positionsetitem);
            gameObject.transform.SetParent(this.trans_positionsetitemparent);
            gameObject.transform.localScale = Vector3.one;
            gameObject.SetActive(true);
            GuildPositionInfo guildPositionInfo = guildPositionInfoList[i];
            gameObject.name = guildPositionInfo.positionid.ToString();
            gameObject.transform.Find("txt_name").GetComponent<Text>().text = guildPositionInfo.name;
            GameObject gameObject2 = gameObject.transform.Find("btn_set").gameObject;
            GameObject gameObject3 = gameObject.transform.Find("btn_del").gameObject;
            gameObject2.SetActive(true);
            gameObject3.SetActive(guildPositionInfo.positionid != 1U && guildPositionInfo.positionid != 99U);
            UIEventListener.Get(gameObject2).onClick = new UIEventListener.VoidDelegate(this.btn_positionitemset_onclick);
            UIEventListener.Get(gameObject3).onClick = new UIEventListener.VoidDelegate(this.btn_positionitemdel_onclick);
        }
    }

    private void btn_positionadd_onclick()
    {
        Transform transform = this.root.Find("Panel_positionset");
        InputField component = transform.Find("InputField").GetComponent<InputField>();
        Toggle[] componentsInChildren = transform.Find("Panel").GetComponentsInChildren<Toggle>();
        component.text = string.Empty;
        for (int i = 0; i < componentsInChildren.Length; i++)
        {
            componentsInChildren[i].isOn = false;
            componentsInChildren[i].interactable = true;
        }
        this.btn_positionadd_sure.name = "0";
        transform.gameObject.SetActive(true);
    }

    private GuildPositionInfo GetGuildPositionInfoByPositionid(uint positionid)
    {
        List<GuildPositionInfo> guildPositionInfoList = this.mController.GetGuildPositionInfoList();
        for (int i = 0; i < guildPositionInfoList.Count; i++)
        {
            if (guildPositionInfoList[i].positionid == positionid)
            {
                return guildPositionInfoList[i];
            }
        }
        return null;
    }

    private void btn_positionitemset_onclick(PointerEventData eventData)
    {
        Transform transform = this.root.Find("Panel_positionset");
        InputField component = transform.Find("InputField").GetComponent<InputField>();
        Toggle[] componentsInChildren = transform.Find("Panel").GetComponentsInChildren<Toggle>();
        GuildPositionInfo guildPositionInfoByPositionid = this.GetGuildPositionInfoByPositionid(uint.Parse(eventData.pointerPress.transform.parent.name));
        component.text = guildPositionInfoByPositionid.name;
        GuildPrivilege[] array = new GuildPrivilege[]
        {
            GuildPrivilege.GUILDPRI_MODIFY_NOTIFY,
            GuildPrivilege.GUILDPRI_ACCEPT_JOIN,
            GuildPrivilege.GUILDPRI_REMOVE_MEMBER
        };
        for (int i = 0; i < componentsInChildren.Length; i++)
        {
            componentsInChildren[i].isOn = this.mController.GetPositionGuildPrivilege(guildPositionInfoByPositionid.positionid, array[i]);
            componentsInChildren[i].interactable = (guildPositionInfoByPositionid.positionid != 1U && guildPositionInfoByPositionid.positionid != 99U);
        }
        this.btn_positionadd_sure.name = guildPositionInfoByPositionid.positionid.ToString();
        transform.gameObject.SetActive(true);
    }

    private void btn_positionitemdel_onclick(PointerEventData eventData)
    {
        uint positionid = uint.Parse(eventData.pointerPress.transform.parent.name);
        this.mController.DeleteGuildPosition(positionid);
    }

    private void btn_positionadd_sure_onclick()
    {
        Transform transform = this.root.Find("Panel_positionset");
        InputField component = transform.Find("InputField").GetComponent<InputField>();
        string text = component.text;
        if (string.Compare(text, KeyWordFilter.ChatFilter(text)) != 0)
        {
            TipsWindow.ShowWindow(TipsType.HAVE_SENSITIVE, null);
            return;
        }
        List<GuildPrivilege> list = new List<GuildPrivilege>();
        Toggle[] componentsInChildren = transform.Find("Panel").GetComponentsInChildren<Toggle>();
        if (componentsInChildren[0].isOn)
        {
            list.Add(GuildPrivilege.GUILDPRI_MODIFY_NOTIFY);
        }
        if (componentsInChildren[1].isOn)
        {
            list.Add(GuildPrivilege.GUILDPRI_ACCEPT_JOIN);
        }
        if (componentsInChildren[2].isOn)
        {
            list.Add(GuildPrivilege.GUILDPRI_REMOVE_MEMBER);
        }
        uint flagByPrivilege = this.mController.GetFlagByPrivilege(list);
        uint num = uint.Parse(this.btn_positionadd_sure.name);
        if (num == 0U)
        {
            GuildPositionInfo guildPositionInfo = new GuildPositionInfo();
            guildPositionInfo.name = component.text;
            List<GuildPositionInfo> guildPositionInfoList = this.mController.GetGuildPositionInfoList();
            for (uint num2 = 11U; num2 < 90U; num2 += 1U)
            {
                bool flag = false;
                for (int i = 0; i < guildPositionInfoList.Count; i++)
                {
                    if (guildPositionInfoList[i].positionid == num2)
                    {
                        flag = true;
                        break;
                    }
                }
                if (!flag)
                {
                    guildPositionInfo.positionid = num2;
                    break;
                }
            }
            guildPositionInfo.orderid = (uint)guildPositionInfoList.Count;
            guildPositionInfo.privilege = flagByPrivilege;
            this.mController.AddGuildPosition(guildPositionInfo);
        }
        else
        {
            GuildPositionInfo guildPositionInfo2 = this.mController.GetGuildPositionInfo(num);
            if (component.text != guildPositionInfo2.name)
            {
                this.mController.ChangePositionName(num, component.text);
            }
            this.mController.ChangePositionPrivilege(num, flagByPrivilege);
        }
        transform.gameObject.SetActive(false);
    }

    private void btn_positionadd_cancel_onclick()
    {
        Transform transform = this.root.Find("Panel_positionset");
        transform.gameObject.SetActive(false);
    }

    private void btn_positionclose_onclick()
    {
        this.root.Find("Panel_positionadd").gameObject.SetActive(false);
    }

    private void btn_applylist_onclick()
    {
        this.mController.GuildMemberList(ReqMemberListType.APPLYFOR);
    }

    private void btn_levelupsort_onclick()
    {
        this.SetupApplyListBySort(UI_GuildInfoNew.APPLY_SORT.LEVELUP);
    }

    private void btn_leveldownsort_onclick()
    {
        this.SetupApplyListBySort(UI_GuildInfoNew.APPLY_SORT.LEVELDOWN);
    }

    public void SetupApplyListBySort(UI_GuildInfoNew.APPLY_SORT sort)
    {
        if (sort == UI_GuildInfoNew.APPLY_SORT.NONE)
        {
            sort = this.sortRecord;
        }
        else
        {
            this.sortRecord = sort;
        }
        List<guildMember> applyMemberList = this.mController.GetApplyMemberList();
        switch (sort)
        {
            case UI_GuildInfoNew.APPLY_SORT.DEFAULT:
                applyMemberList.Sort(new ApplyDefaultComparer());
                break;
            case UI_GuildInfoNew.APPLY_SORT.LEVELUP:
                applyMemberList.Sort(new ApplyDefaultComparer());
                applyMemberList.Sort(new MemberLevelUpComparer());
                break;
            case UI_GuildInfoNew.APPLY_SORT.LEVELDOWN:
                applyMemberList.Sort(new ApplyDefaultComparer());
                applyMemberList.Sort(new MemberLevelDownComparer());
                break;
        }
        this.SetupApplyPanel(applyMemberList);
    }

    private void tgl_applylist_onvaluechanged(bool isOn)
    {
        this.SetupApplyListBySort(UI_GuildInfoNew.APPLY_SORT.NONE);
    }

    private void SetupApplyPanel(List<guildMember> memebers)
    {
        GameObject gameObject = this.trans_applyconfirm.Find("Panel_list/Scroll View/Viewport/Content/Panel_info").gameObject;
        UIManager.Instance.ClearListChildrens(gameObject.transform.parent, 1);
        int num = 0;
        for (int i = 0; i < memebers.Count; i++)
        {
            guildMember guildMember = memebers[i];
            if (!this.tgl_applylist.isOn || guildMember.isonline)
            {
                num++;
                GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(gameObject);
                gameObject2.transform.SetParent(gameObject.transform.parent);
                gameObject2.transform.localScale = Vector3.one;
                gameObject2.SetActive(true);
                gameObject2.name = guildMember.memberid.ToString();
                Text component = gameObject2.transform.Find("txt_name").GetComponent<Text>();
                Text component2 = gameObject2.transform.Find("txt_lv").GetComponent<Text>();
                Text component3 = gameObject2.transform.Find("txt_online").GetComponent<Text>();
                Text component4 = gameObject2.transform.Find("txt_offline").GetComponent<Text>();
                Text component5 = gameObject2.transform.Find("txt_time").GetComponent<Text>();
                component.text = guildMember.membername;
                component2.text = guildMember.memberlevel.ToString();
                component3.gameObject.SetActive(guildMember.isonline);
                component4.gameObject.SetActive(!guildMember.isonline);
                ulong num2 = (ulong)SingletonForMono<GameTime>.Instance.GetCurrServerTimeBySecond();
                ulong time = num2 - (ulong)guildMember.applytime;
                component5.text = SingletonForMono<GameTime>.Instance.GetTimeBySecond(time, "前");
                GameObject gameObject3 = gameObject2.transform.Find("btn_agree").gameObject;
                GameObject gameObject4 = gameObject2.transform.Find("btn_refuse").gameObject;
                bool positionGuildPrivilege = this.mController.GetPositionGuildPrivilege(this.mMyInfo.positionid, GuildPrivilege.GUILDPRI_ACCEPT_JOIN);
                gameObject3.SetActive(positionGuildPrivilege);
                gameObject4.SetActive(positionGuildPrivilege);
                UIEventListener.Get(gameObject3).onClick = new UIEventListener.VoidDelegate(this.btn_agree_onclick);
                UIEventListener.Get(gameObject4).onClick = new UIEventListener.VoidDelegate(this.btn_refuse_onclick);
            }
        }
        Text component6 = this.trans_applyconfirm.Find("Panel_list/bottom/txt_num/Text").GetComponent<Text>();
        component6.text = num + "/" + this.ltGuildConfig.GetField_Uint("applyListLimit");
        this.trans_applyconfirm.gameObject.SetActive(true);
    }

    private void btn_agree_onclick(PointerEventData eventData)
    {
        ulong applyid = ulong.Parse(eventData.pointerPress.transform.parent.gameObject.name);
        this.mController.AnswerApplyForGuild(applyid, true);
    }

    private void btn_refuse_onclick(PointerEventData eventData)
    {
        ulong applyid = ulong.Parse(eventData.pointerPress.transform.parent.gameObject.name);
        this.mController.AnswerApplyForGuild(applyid, false);
    }

    private void btn_applyconfirmclose_onclick()
    {
        this.trans_applyconfirm.gameObject.SetActive(false);
    }

    private void OnSkillToggleValueChanged(bool b)
    {
        if (b)
        {
            this.mController.ReqGuildSkillInfo(null);
        }
    }

    public void OnSkillLstDataBack(List<guildSkill> skillLst = null)
    {
        if (skillLst == null)
        {
            this.guildSkillPanel.Find("Scroll View/Viewport/Content/Panel").gameObject.SetActive(false);
        }
        Transform transform = this.guildSkillPanel.Find("Scroll View/Viewport/Content");
        GameObject gameObject = this.guildSkillPanel.Find("Scroll View/Viewport/Content/Panel").gameObject;
        List<LuaTable> configTableList = LuaConfigManager.GetConfigTableList("skill_guild");
        Dictionary<uint, LuaTable> showSkillLst = this.GetShowSkillLst(configTableList, skillLst);
        List<uint> list = new List<uint>(showSkillLst.Keys);
        list.Sort((uint a, uint b) => a.CompareTo(b));
        Transform parent = gameObject.transform.parent;
        for (int i = 0; i < list.Count; i++)
        {
            int index = i;
            GameObject gameObject2;
            if (i < parent.childCount)
            {
                gameObject2 = parent.GetChild(i).gameObject;
            }
            else
            {
                gameObject2 = UnityEngine.Object.Instantiate<GameObject>(gameObject);
                gameObject2.transform.SetParent(parent, false);
            }
            gameObject2.SetActive(true);
            this.InitSkillItem(gameObject2, showSkillLst[list[i]], index);
        }
    }

    private void InitSkillItem(GameObject itemObj, LuaTable lt, int index)
    {
        Image panelBg = itemObj.GetComponent<Image>();
        Color color = panelBg.color;
        color.a = ((index % 2 != 0) ? 0f : 1f);
        panelBg.color = color;
        Image icon = itemObj.transform.Find("Image/img_icon").GetComponent<Image>();
        itemObj.transform.Find("Image").GetComponent<Image>().raycastTarget = false;
        string cacheField_String = lt.GetCacheField_String("skillicon");
        icon.raycastTarget = false;
        UITextureMgr.Instance.GetTexture(ImageType.ITEM, cacheField_String, delegate (UITextureAsset item)
        {
            if (item != null && item.textureObj != null)
            {
                if (icon == null)
                {
                    return;
                }
                Sprite overrideSprite = Sprite.Create(item.textureObj, new Rect(0f, 0f, (float)item.textureObj.width, (float)item.textureObj.height), new Vector2(0f, 0f));
                icon.overrideSprite = overrideSprite;
                this.usedTextureAssets.Add(item);
            }
        });
        Transform transform = itemObj.transform.Find("Panel_reqirement");
        uint skillId = lt.GetCacheField_Uint("skillid");
        uint cacheField_Uint = lt.GetCacheField_Uint("level");
        Text component = itemObj.transform.Find("txt_name").GetComponent<Text>();
        component.text = lt.GetCacheField_String("skillname");
        Text component2 = itemObj.transform.Find("txt_lv").GetComponent<Text>();
        component2.text = "LV." + cacheField_Uint;
        bool cacheField_Bool = lt.GetCacheField_Bool("is_learn");
        component2.gameObject.SetActive(cacheField_Bool);
        Text component3 = itemObj.transform.Find("txt_unlearn").GetComponent<Text>();
        Text component4 = itemObj.transform.Find("txt_max").GetComponent<Text>();
        component4.gameObject.SetActive(false);
        component3.gameObject.SetActive(!cacheField_Bool);
        Button component5 = itemObj.transform.Find("btn_lvup").GetComponent<Button>();
        component5.onClick.RemoveAllListeners();
        component5.onClick.AddListener(delegate ()
        {
            this.mController.ReqLearnGuildSkill(skillId + 1U);
        });
        Button component6 = itemObj.transform.Find("btn_learn").GetComponent<Button>();
        component6.onClick.RemoveAllListeners();
        component6.onClick.AddListener(delegate ()
        {
            this.mController.ReqLearnGuildSkill(skillId);
        });
        Text component7 = transform.Find("Panel_needlv/txt_lv_num").GetComponent<Text>();
        Text component8 = transform.Find("Panel_costgold/txt_gold_num").GetComponent<Text>();
        LuaTable guildSkillConfigByID = this.mController.GetGuildSkillConfigByID(skillId + 1U);
        component7.text = string.Empty;
        if (cacheField_Bool)
        {
            if (guildSkillConfigByID != null)
            {
                component7.text = guildSkillConfigByID.GetCacheField_Uint("unlocklevel") + "级";
                component7.color = ((this.mGuildInfo.guildlevel < guildSkillConfigByID.GetCacheField_Uint("unlocklevel")) ? Color.red : Color.white);
            }
        }
        else
        {
            component7.color = Color.white;
            component7.text = lt.GetCacheField_Uint("unlocklevel") + "级";
            component7.color = ((this.mGuildInfo.guildlevel < lt.GetCacheField_Uint("unlocklevel")) ? Color.red : Color.white);
        }
        string[] array = guildSkillConfigByID.GetCacheField_String("levelupcost").Split(new char[]
        {
            '-'
        });
        component8.text = array[1];
        uint key = skillId / 100U;
        uint num = uint.Parse(array[1]);
        component8.color = ((num <= this.mGuildInfo.salary) ? Color.white : Color.red);
        if (!this.mController.IsGuildMaster())
        {
            component5.gameObject.SetActive(false);
            component6.gameObject.SetActive(false);
        }
        else if (cacheField_Bool)
        {
            component6.gameObject.SetActive(false);
            List<LuaTable> configTableList = LuaConfigManager.GetConfigTableList("skill_guild");
            for (int i = 0; i < configTableList.Count; i++)
            {
                if (configTableList[i].GetCacheField_Uint("skillid") == skillId + 1U)
                {
                    LuaTable luaTable = configTableList[i];
                    array = luaTable.GetCacheField_String("levelupcost").Split(new char[]
                    {
                        '-'
                    });
                    component8.text = array[1];
                    num = uint.Parse(array[1]);
                    component8.color = ((num <= this.mGuildInfo.salary) ? Color.white : Color.red);
                    break;
                }
            }
            if (this.skillMaxLevel.ContainsKey(key))
            {
                if (cacheField_Uint < this.skillMaxLevel[key])
                {
                    component5.gameObject.SetActive(true);
                }
                else
                {
                    component4.gameObject.SetActive(true);
                    component5.gameObject.SetActive(false);
                    component6.gameObject.SetActive(false);
                    transform.gameObject.SetActive(false);
                }
            }
        }
        else
        {
            component5.gameObject.SetActive(false);
            component6.gameObject.SetActive(true);
        }
        HoverEventListener.Get(panelBg.gameObject).onEnter = delegate (PointerEventData pd)
        {
            ItemTipController controller = ControllerManager.Instance.GetController<ItemTipController>();
            controller.OpenGuildSkillPanel(skillId, panelBg.gameObject);
        };
    }

    private Dictionary<uint, LuaTable> GetShowSkillLst(List<LuaTable> cfgTableLst, List<guildSkill> skillLst = null)
    {
        Dictionary<uint, LuaTable> dictionary = new Dictionary<uint, LuaTable>();
        cfgTableLst.Sort((LuaTable a, LuaTable b) => a.GetCacheField_Uint("skillid").CompareTo(b.GetCacheField_Uint("skillid")));
        if (this.skillMaxLevel.Count == 0)
        {
            for (int i = 0; i < cfgTableLst.Count; i++)
            {
                uint cacheField_Uint = cfgTableLst[i].GetCacheField_Uint("skillid");
                uint key = cacheField_Uint / 100U;
                if (this.skillMaxLevel.ContainsKey(key))
                {
                    if (this.skillMaxLevel[key] < cfgTableLst[i].GetCacheField_Uint("level"))
                    {
                        this.skillMaxLevel[key] = cfgTableLst[i].GetCacheField_Uint("level");
                    }
                }
                else
                {
                    this.skillMaxLevel[key] = cfgTableLst[i].GetCacheField_Uint("level");
                }
            }
        }
        if (skillLst == null || skillLst.Count == 0)
        {
            for (int j = 0; j < cfgTableLst.Count; j++)
            {
                uint cacheField_Uint2 = cfgTableLst[j].GetCacheField_Uint("skillid");
                uint key2 = cacheField_Uint2 / 100U;
                if (!dictionary.ContainsKey(key2))
                {
                    dictionary.Add(key2, cfgTableLst[j]);
                }
                else if (dictionary[key2].GetCacheField_Uint("level") > cfgTableLst[j].GetCacheField_Uint("level"))
                {
                    dictionary[key2] = cfgTableLst[j];
                }
            }
        }
        else
        {
            for (int k = 0; k < cfgTableLst.Count; k++)
            {
                uint cacheField_Uint3 = cfgTableLst[k].GetCacheField_Uint("skillid");
                uint key3 = cacheField_Uint3 / 100U;
                bool flag = false;
                for (int l = 0; l < skillLst.Count; l++)
                {
                    if (skillLst[l].skillid == cacheField_Uint3)
                    {
                        flag = true;
                        cfgTableLst[k]["is_learn"] = true;
                        dictionary[key3] = cfgTableLst[k];
                        break;
                    }
                }
                if (!flag)
                {
                    if (!dictionary.ContainsKey(key3))
                    {
                        dictionary[key3] = cfgTableLst[k];
                    }
                    else if (dictionary[key3].GetCacheField_Uint("level") > cfgTableLst[k].GetCacheField_Uint("level"))
                    {
                        dictionary[key3] = cfgTableLst[k];
                    }
                }
            }
        }
        return dictionary;
    }

    public override void OnDispose()
    {
        base.OnDispose();
    }

    private const string ONLINE_KEY = "guildMemberOnline";

    private Transform root;

    private Transform mRoot;

    private RawImage img_icon;

    private Text txt_guildname;

    private Text txt_guildid;

    private Text txt_guildlevel;

    private Slider slider_guildexp;

    private Text txt_guildposition;

    private Text txt_guildcontribution;

    private Text txt_guildmoney;

    private Text txt_ownnum;

    private Text txt_limitnum;

    private Transform obj_memberitem_parent;

    private GameObject obj_memberitem;

    private Toggle tgl_online;

    private Button btn_edit;

    private Button btn_noticesure;

    private Button btn_noticecancel;

    private InputField inputfield_notice;

    private Text noticeTextLimit;

    private Transform trans_donate;

    private InputField inputfield_donate;

    private Text txt_donate_money_own;

    private List<GameObject> objItemList;

    private GuildControllerNew mController;

    private uint mDailyContribute;

    private GuildCompare[] mCompareDefaultArr = new GuildCompare[]
    {
        new PositionDownComparer(),
        new ContributeDownComparer(),
        new MemberLevelDownComparer()
    };

    private UI_GuildInfoNew.CompareType curCompareType;

    private bool curCompareUp;

    private Button btn_positionset;

    private Button btn_positionsetclose;

    private Button btn_positionsetadd;

    private Transform trans_positionsetitemparent;

    private GameObject obj_positionsetitem;

    private Button btn_positionadd_sure;

    private Button btn_positionadd_cancel;

    private Button btn_applylist;

    private Transform trans_applyconfirm;

    private Toggle tgl_applylist;

    private guildInfo mGuildInfo;

    private guildMember mMyInfo;

    private LuaTable levelConfig;

    private LuaTable ltGuildConfig;

    private GuildMemberSecondMenu mGuildMemberSecondMenu;

    private Toggle toggleGuidSkill;

    private Transform guildSkillPanel;

    private Transform guildSkillTipsPanel;

    private uint contributeSkillLv;

    private bool isDonateMsgboxOpened;

    private string oldNotice = string.Empty;

    private UI_GuildInfoNew.APPLY_SORT sortRecord;

    private Dictionary<uint, uint> skillMaxLevel = new Dictionary<uint, uint>();

    private enum CompareType
    {
        Position,
        Contribute,
        Donate,
        Level
    }

    public enum APPLY_SORT
    {
        NONE,
        DEFAULT,
        LEVELUP,
        LEVELDOWN
    }
}
public class GuildCompare : Comparer<guildMember>
{
    public override int Compare(guildMember x, guildMember y)
    {
        return x.contribute.CompareTo(y.contribute);
    }
}

public class ContributeDownComparer : GuildCompare
{
    public override int Compare(guildMember x, guildMember y)
    {
        return y.contribute.CompareTo(x.contribute);
    }
}

public class ContributeUpComparer : GuildCompare
{
    public override int Compare(guildMember x, guildMember y)
    {
        return x.contribute.CompareTo(y.contribute);
    }
}

public class DonateDownComparer : GuildCompare
{
    public override int Compare(guildMember x, guildMember y)
    {
        return y.donatesalary.CompareTo(x.donatesalary);
    }
}
public class DonateUpComparer : GuildCompare
{
    public override int Compare(guildMember x, guildMember y)
    {
        return x.donatesalary.CompareTo(y.donatesalary);
    }
}

public class MemberLevelDownComparer : GuildCompare
{
    public override int Compare(guildMember x, guildMember y)
    {
        return y.memberlevel.CompareTo(x.memberlevel);
    }
}

public class MemberLevelUpComparer : GuildCompare
{
    public override int Compare(guildMember x, guildMember y)
    {
        return x.memberlevel.CompareTo(y.memberlevel);
    }
}

public class PositionDownComparer : GuildCompare
{
    public override int Compare(guildMember x, guildMember y)
    {
        return x.positionid.CompareTo(y.positionid);
    }
}
public class PositionUpComparer : GuildCompare
{
    public override int Compare(guildMember x, guildMember y)
    {
        return y.positionid.CompareTo(x.positionid);
    }
}

