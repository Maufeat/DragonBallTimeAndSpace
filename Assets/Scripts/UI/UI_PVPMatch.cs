using System;
using System.Collections.Generic;
using Framework.Managers;
using LuaInterface;
using msg;
using Obj;
using rankpk_msg;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_PVPMatch : UIPanelBase
{
    public override void OnInit(Transform root)
    {
        base.OnInit(root);
        this.controller = ControllerManager.Instance.GetController<PVPMatchController>();
        this.InitObj(root);
        this.InitEvent();
        this.SwitchState(this.controller.pvpState);
        this.controller.mNetWork.MSG_ReqRankPKCurStage_CS();
        this.controller.mNetWork.Req_MatchMemberInfo_CS();
        this.InitRewardInfo();
        this.OnMatchToggleSelect(true);
        this.InitAbattorInfo();
        GuideController guideController = ControllerManager.Instance.GetController<GuideController>();
        guideController.CheckIsNeedGuideByID(20U);
    }

    private void InitObj(Transform root)
    {
        this.ui_root = root;
        this.panelRoot = root.Find("Offset_PVPMatch/background");
        this.txtDan = this.panelRoot.Find("Panel_wudaohui/txt_rank");
        this.imgRank = this.panelRoot.Find("Panel_wudaohui/img_rank").GetComponent<Image>();
        this.imgRankLv = this.panelRoot.Find("Panel_wudaohui/img_rank/img_rank_lv").GetComponent<Image>();
        this.transStar = this.panelRoot.Find("Panel_wudaohui/img_rank/Panel_star/img_star");
        this.btnMatch = this.panelRoot.Find("Panel_wudaohui/btn_matching");
        this.btnCancelMatch = this.panelRoot.Find("Panel_wudaohui/btn_matchcancel");
        this.btnExit = this.panelRoot.Find("Panel_title/btn_close");
        this.tgMatch = this.panelRoot.Find("ToggleGroup/Panel_tab/wudaohui");
        this.tgBattle = this.panelRoot.Find("ToggleGroup/Panel_tab/battlefield");
        this.btnRule = this.panelRoot.Find("Panel_wudaohui/btn_rule");
        this.abattoirMatch = this.panelRoot.Find("ToggleGroup/Panel_tab/abattoir");
        this.btnCloseRule = root.Find("Offset_PVPMatch/Panel_rule/Panel_title/btn_close");
        this.tgRank = this.panelRoot.Find("ToggleGroup/Panel_tab/rankinglist");
        this.tgReward = this.panelRoot.Find("ToggleGroup/Panel_tab/rewards");
        this.transRankItem = this.panelRoot.Find("Panel_ranklist/Scroll View/Viewport/Content/Panel_info");
        this.togRankFriend = this.panelRoot.Find("Panel_ranklist/Toggle").GetComponent<Toggle>();
        this.togRankFriend.isOn = false;
        this.btnAbattoirMatch = this.panelRoot.Find("Panel_abattoir/btn_matching");
        this.btnAbattoirCancelMatch = this.panelRoot.Find("Panel_abattoir/btn_matchcancel");
        AbattoirMatchController abattoirMatchController = ControllerManager.Instance.GetController<AbattoirMatchController>();
        this.btnAbattoirMatch.gameObject.SetActive(abattoirMatchController.getState == AbattoirMatchState.None);
        this.btnAbattoirCancelMatch.gameObject.SetActive(abattoirMatchController.getState == AbattoirMatchState.Matching);
        this.txtSeasonValue = this.panelRoot.Find("Panel_wudaohui/txt_season");
        this.txtSeasonWinValue = this.panelRoot.Find("Panel_wudaohui/Panel_seasonmark/img2/txt2_value");
        this.txtSeasonWinRatioValue = this.panelRoot.Find("Panel_wudaohui/Panel_seasonmark/img3/txt3_value");
        this.txtSeasonRankValue = this.panelRoot.Find("Panel_wudaohui/Panel_seasonmark/img1/txt1_value");
        this.txtBestRankValue = this.panelRoot.Find("Panel_wudaohui/Panel_history/Text2/txt_value");
        this.txtBestDanValue = this.panelRoot.Find("Panel_wudaohui/Panel_history/Text1/txt_value");
        this.txtSeasonLeftDayValue = this.panelRoot.Find("Panel_wudaohui/txt_overplus");
        this.txtRankListSeasonValue = this.panelRoot.Find("Panel_ranklist/txt_season");
        this.btnHelp = this.panelRoot.Find("Panel_title/Image_help");
        this.transPvpInfo = this.panelRoot.Find("Panel_wudaohui");
        this.transPvpList = this.panelRoot.Find("Panel_ranklist");
        this.transAbattoirInfo = this.panelRoot.Find("Panel_abattoir");
        this.transPvpAward = this.panelRoot.Find("Panel_award");
        this.transPvpRule = root.Find("Offset_PVPMatch/Panel_rule");
        for (int i = 0; i < this.RANK_NUM; i++)
        {
            this.imgRewardRank.Add(this.panelRoot.Find("Panel_award/img_rank" + (i + 1)).GetComponent<Image>());
        }
        for (int j = 0; j < this.RANK_NUM; j++)
        {
            this.transRewardPanel.Add(this.panelRoot.Find("Panel_award/Panel_award" + (j + 1)));
        }
        this.btnCancelMatch.gameObject.SetActive(true);
        this.transPvpRule.gameObject.SetActive(false);
        this.obj_awarditem = this.panelRoot.Find("Panel_bf/img_bg/award/icon").gameObject;
        this.imgMap = this.panelRoot.Find("Panel_bf/img_bg/map").GetComponent<RawImage>();
        this.btn_matching = this.panelRoot.Find("Panel_bf/img_bg/btn_matching").GetComponent<Button>();
        this.btn_matching.onClick.AddListener(new UnityAction(this.btn_matching_onclick));
        this.btn_cancelmatching = this.panelRoot.Find("Panel_bf/img_bg/btn_cancelmatching").GetComponent<Button>();
        this.btn_cancelmatching.onClick.AddListener(new UnityAction(this.btn_cancelmatching_onclick));
        this.txtMatchResult = this.panelRoot.Find("Panel_bf/img_bg/Text").GetComponent<Text>();
        this.btn_matching.gameObject.SetActive(!this.controller.mIsInMathingState);
        this.btn_cancelmatching.gameObject.SetActive(this.controller.mIsInMathingState);
        Transform transform = this.panelRoot.Find("Panel_bf/bftype/ScrollRect/grid");
        for (int k = 0; k < transform.childCount; k++)
        {
            UIEventListener uieventListener = UIEventListener.Get(transform.GetChild(k).gameObject);
            uieventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uieventListener.onClick, new UIEventListener.VoidDelegate(this.btn_battlemenuparent_onclick));
        }
        this.transBattleContainer = this.panelRoot.Find("Panel_bf");
        this.SetupBattlePanel();
    }

    public void SetBattleBtnMatchingState(bool state)
    {
        this.btn_matching.gameObject.SetActive(!state);
        this.btn_cancelmatching.gameObject.SetActive(state);
    }

    private void InitUIEffect(Transform parent, string effectName, Vector2 pos)
    {
        ManagerCenter.Instance.GetManager<FFEffectManager>().LoadUIEffobj(effectName, delegate
        {
            ObjectPool<EffectObjInPool> effobj = ManagerCenter.Instance.GetManager<FFEffectManager>().GetEffobj(effectName);
            if (effobj != null)
            {
                effobj.GetItemFromPool(delegate (EffectObjInPool OIP)
                {
                    GameObject itemObj = OIP.ItemObj;
                    itemObj.SetLayer(Const.Layer.UI, true);
                    itemObj.transform.SetParent(parent);
                    itemObj.transform.localScale = Vector3.one;
                    itemObj.transform.localPosition = pos;
                });
            }
        });
    }

    private void InitEvent()
    {
        Button component = this.btnMatch.GetComponent<Button>();
        component.onClick.AddListener(new UnityAction(this.OnMatchButtonClick));
        Button component2 = this.btnCancelMatch.GetComponent<Button>();
        component2.onClick.AddListener(new UnityAction(this.OnCancelMatchButtonClick));
        Button component3 = this.btnExit.GetComponent<Button>();
        component3.onClick.AddListener(new UnityAction(this.OnExitButtonClick));
        Button component4 = this.btnRule.GetComponent<Button>();
        component4.onClick.AddListener(new UnityAction(this.OnRuleButtonClick));
        Button component5 = this.btnCloseRule.GetComponent<Button>();
        component5.onClick.AddListener(new UnityAction(this.OnCloseRuleButtonClick));
        Button component6 = this.btnAbattoirMatch.GetComponent<Button>();
        component6.onClick.AddListener(new UnityAction(this.OnAbattoirMatchButtonClick));
        Button component7 = this.btnAbattoirCancelMatch.GetComponent<Button>();
        component7.onClick.AddListener(new UnityAction(this.OnAbattoirCancelMatchButtonClick));
        Toggle component8 = this.tgMatch.GetComponent<Toggle>();
        component8.onValueChanged.AddListener(new UnityAction<bool>(this.OnMatchToggleSelect));
        Toggle component9 = this.tgBattle.GetComponent<Toggle>();
        component9.onValueChanged.AddListener(new UnityAction<bool>(this.OnBattleToggleSelect));
        Toggle component10 = this.tgRank.GetComponent<Toggle>();
        component10.onValueChanged.AddListener(new UnityAction<bool>(this.OnRankToggleSelect));
        Toggle component11 = this.abattoirMatch.GetComponent<Toggle>();
        component11.onValueChanged.AddListener(new UnityAction<bool>(this.OnAbattoirMatchToggleSelect));
        Toggle component12 = this.tgReward.GetComponent<Toggle>();
        component12.onValueChanged.AddListener(new UnityAction<bool>(this.OnRewardToggleSelect));
        this.togRankFriend.onValueChanged.AddListener(new UnityAction<bool>(this.OnTogRankFriendValueChange));
        Button component13 = this.btnHelp.GetComponent<Button>();
        component13.onClick.RemoveAllListeners();
        component13.onClick.AddListener(new UnityAction(this.Help));
    }

    private void Help()
    {
        GuideController guideController = ControllerManager.Instance.GetController<GuideController>();
        guideController.ViewGuideUI(20U, true);
    }

    private void OnMatchButtonClick()
    {
        this.controller.WudaohuiStartMatch();
    }

    private void OnCancelMatchButtonClick()
    {
        this.controller.WudaohuiCancelMatch();
    }

    private void OnExitButtonClick()
    {
        this.controller.CloseUI();
    }

    private void OnRuleButtonClick()
    {
        this.transPvpRule.gameObject.SetActive(true);
    }

    private void OnCloseRuleButtonClick()
    {
        this.transPvpRule.gameObject.SetActive(false);
    }

    private void OnAbattoirMatchButtonClick()
    {
        ControllerManager.Instance.GetController<AbattoirMatchController>().SendReqMatch();
    }

    private void OnAbattoirCancelMatchButtonClick()
    {
        ControllerManager.Instance.GetController<AbattoirMatchController>().SendCancelMatch();
    }

    private void OnMatchToggleSelect(bool state)
    {
        if (state)
        {
            this.transPvpInfo.gameObject.SetActive(true);
            this.transBattleContainer.gameObject.SetActive(false);
            this.transPvpList.gameObject.SetActive(false);
            this.transAbattoirInfo.gameObject.SetActive(false);
            this.transPvpAward.gameObject.SetActive(false);
        }
    }

    private void OnBattleToggleSelect(bool state)
    {
        if (state)
        {
            this.transPvpInfo.gameObject.SetActive(false);
            this.transBattleContainer.gameObject.SetActive(true);
            this.transPvpList.gameObject.SetActive(false);
            this.transAbattoirInfo.gameObject.SetActive(false);
            this.transPvpAward.gameObject.SetActive(false);
        }
    }

    private void OnRankToggleSelect(bool state)
    {
        if (state)
        {
            this.transPvpInfo.gameObject.SetActive(false);
            this.transBattleContainer.gameObject.SetActive(false);
            this.transPvpList.gameObject.SetActive(true);
            this.transAbattoirInfo.gameObject.SetActive(false);
            this.transPvpAward.gameObject.SetActive(false);
            this.controller.mNetWork.ReqRankPKList_CS(RankPKListType.RankPKListType_All);
        }
    }

    private void OnAbattoirMatchToggleSelect(bool state)
    {
        if (state)
        {
            this.transPvpInfo.gameObject.SetActive(false);
            this.transBattleContainer.gameObject.SetActive(false);
            this.transPvpList.gameObject.SetActive(false);
            this.transAbattoirInfo.gameObject.SetActive(true);
            this.transPvpAward.gameObject.SetActive(false);
        }
    }

    private void OnRewardToggleSelect(bool state)
    {
        if (state)
        {
            this.transPvpInfo.gameObject.SetActive(false);
            this.transBattleContainer.gameObject.SetActive(false);
            this.transPvpList.gameObject.SetActive(false);
            this.transAbattoirInfo.gameObject.SetActive(false);
            this.transPvpAward.gameObject.SetActive(true);
        }
    }

    private void OnTogRankFriendValueChange(bool state)
    {
        this.controller.mNetWork.ReqRankPKList_CS((!state) ? RankPKListType.RankPKListType_All : RankPKListType.RankPKListType_Friend);
    }

    public void InitRoleInfo(UserRankPkInfo info, MSG_Ret_MatchMemberInfo_SC matchInfo)
    {
        LuaTable configTable = LuaConfigManager.GetConfigTable("rankpk_level", (ulong)info.rank_level);
        if (configTable != null)
        {
            this.txtDan.GetComponent<Text>().text = configTable.GetField_String("name");
            GlobalRegister.SetImage(4, configTable.GetField_String("icon"), this.imgRank, true);
            GlobalRegister.SetImage(4, configTable.GetField_String("icon_small"), this.imgRankLv, true);
            this.transStar.gameObject.SetActive(false);
            this.InitUIEffect(this.transPvpInfo, this.GetCupEffectName(info.rank_level), this.GetCupEffectOffect(info.rank_level));
            GlobalRegister.CopyToSameParent(this.transStar, configTable.GetField_Int("max_star"));
            for (int i = 1; i < this.transStar.parent.childCount; i++)
            {
                Transform child = this.transStar.parent.GetChild(i);
                GlobalRegister.SetImage(4, "rankstar1", child.GetComponent<Image>(), true);
                GlobalRegister.SetImage(4, "rankstar2", child.Find("img_getstar").GetComponent<Image>(), false);
                child.gameObject.SetActive(true);
                if ((long)i <= (long)((ulong)info.rank_star))
                {
                    this.InitUIEffect(child.Find("img_getstar"), "lz_jiangbei_star01", Vector2.zero);
                    child.Find("img_getstar").gameObject.SetActive(true);
                }
                else
                {
                    child.Find("img_getstar").gameObject.SetActive(false);
                }
            }
        }
        int num = 0;
        if (info.seanson_battles != 0U)
        {
            num = (int)(info.success_battles / (info.seanson_battles * 1f) * 100f);
        }
        UIInformationList component = this.txtSeasonValue.GetComponent<UIInformationList>();
        if (component != null)
        {
            string timeText = SingletonForMono<GameTime>.Instance.GetTimeText((ulong)matchInfo.start_time);
            string timeText2 = SingletonForMono<GameTime>.Instance.GetTimeText((ulong)matchInfo.end_time);
            string[] array = timeText.Split(new char[]
            {
                ' '
            })[0].Split(new char[]
            {
                '-'
            });
            string[] array2 = timeText2.Split(new char[]
            {
                ' '
            })[0].Split(new char[]
            {
                '-'
            });
            string text = string.Format(component.listInformation[0].content, new object[]
            {
                array[0],
                matchInfo.season_id,
                array[1],
                array[2],
                array2[1],
                array2[2]
            });
            this.txtSeasonValue.GetComponent<Text>().text = text;
            this.txtRankListSeasonValue.GetComponent<Text>().text = text;
        }
        this.txtSeasonWinValue.GetComponent<Text>().text = info.success_battles.ToString();
        this.txtSeasonWinRatioValue.GetComponent<Text>().text = num.ToString();
        this.txtSeasonRankValue.GetComponent<Text>().text = info.rank.ToString();
        this.txtBestRankValue.GetComponent<Text>().text = info.best_rank.ToString();
        this.txtSeasonLeftDayValue.GetComponent<Text>().text = "剩余天数" + matchInfo.leftdays + "天";
        if (info.seanson_battles == 0U)
        {
            this.txtSeasonRankValue.GetComponent<Text>().text = "--";
        }
        LuaTable configTable2 = LuaConfigManager.GetConfigTable("rankpk_level", (ulong)info.best_rank_level);
        if (configTable2 != null)
        {
            this.txtBestDanValue.GetComponent<Text>().text = configTable2.GetField_String("name");
        }
    }

    private string GetCupEffectName(uint rankLevel)
    {
        string result = string.Empty;
        if (rankLevel < 6U)
        {
            result = "lz_jiangbei_wuzhe";
        }
        else if (rankLevel < 11U)
        {
            result = "lz_jiangbei_dashi";
        }
        else if (rankLevel < 14U)
        {
            result = "lz_jiangbei_zongshi";
        }
        else
        {
            result = "lz_jiangbei_zhanshi";
        }
        return result;
    }

    private Vector2 GetCupEffectOffect(uint rankLevel)
    {
        Vector2 zero = Vector2.zero;
        if (rankLevel < 6U)
        {
            zero = new Vector2(-200f, -170f);
        }
        else if (rankLevel < 11U)
        {
            zero = new Vector2(-260f, -215f);
        }
        else if (rankLevel < 14U)
        {
            zero = new Vector2(-260f, -215f);
        }
        else
        {
            zero = new Vector2(-260f, -190f);
        }
        return zero;
    }

    public void InitRewardInfo()
    {
        for (int i = 0; i < this.RANK_NUM; i++)
        {
            LuaTable configTable = LuaConfigManager.GetConfigTable("rankpk_level", (ulong)this.rankRewardAttr[i]);
            if (configTable != null)
            {
                GlobalRegister.SetImage(4, configTable.GetField_String("icon"), this.imgRewardRank[i], true);
                string[] array = configTable.GetField_String("season_reward").Split(new char[]
                {
                    ';'
                });
                if (array != null && array.Length != 0)
                {
                    List<int> list = new List<int>();
                    List<int> list2 = new List<int>();
                    for (int j = 0; j < array.Length; j++)
                    {
                        list.Add(array[j].Split(new char[]
                        {
                            '-'
                        })[0].ToInt());
                        list2.Add(array[j].Split(new char[]
                        {
                            '-'
                        })[1].ToInt());
                    }
                    GlobalRegister.CopyToSameParent(this.transRewardPanel[i].GetChild(0), array.Length);
                    this.transRewardPanel[i].GetChild(0).gameObject.SetActive(false);
                    for (int k = 1; k < this.transRewardPanel[i].childCount; k++)
                    {
                        Transform child = this.transRewardPanel[i].GetChild(k);
                        child.Find("img_icon").gameObject.SetActive(k <= list.Count);
                        child.Find("Text").gameObject.SetActive(k <= list.Count);
                        if (k <= list.Count)
                        {
                            this.ShowRewardItem(child, (uint)list[k - 1], (uint)list2[k - 1]);
                        }
                        else
                        {
                            child.gameObject.SetActive(false);
                        }
                    }
                }
            }
        }
    }

    private void ShowRewardItem(Transform rewardItem, uint baseId, uint num)
    {
        rewardItem.gameObject.SetActive(true);
        LuaTable configTable = LuaConfigManager.GetConfigTable("objects", (ulong)baseId);
        if (configTable == null)
        {
            return;
        }
        GlobalRegister.SetImage(0, configTable.GetField_String("icon"), rewardItem.Find("img_icon").GetComponent<Image>(), true);
        rewardItem.Find("Text").GetComponent<Text>().text = num.ToString();
        UIEventListener.Get(rewardItem.Find("img_icon").gameObject).onEnter = delegate (PointerEventData ed)
        {
            ControllerManager.Instance.GetController<ItemTipController>().OpenPanel(new t_Object
            {
                baseid = baseId
            }, rewardItem.Find("img_icon").gameObject);
        };
        UIEventListener.Get(rewardItem.Find("img_icon").gameObject).onExit = delegate (PointerEventData ed)
        {
            ControllerManager.Instance.GetController<ItemTipController>().ClosePanel();
        };
    }

    public void InitRankInfo(MSG_RetRankPKList_SC info)
    {
        GlobalRegister.CopyToSameParent(this.transRankItem, info.data.Count);
        this.transRankItem.gameObject.SetActive(false);
        for (int i = 1; i < this.transRankItem.parent.childCount; i++)
        {
            if (i <= info.data.Count)
            {
                this.ShowRankItem(this.transRankItem.parent.GetChild(i), info.data[i - 1]);
            }
            else
            {
                this.transRankItem.parent.GetChild(i).gameObject.SetActive(false);
            }
        }
    }

    private void ShowRankItem(Transform trans, RankPKListItem item)
    {
        trans.gameObject.SetActive(true);
        trans.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
        Text component = trans.Find("txt_rank").GetComponent<Text>();
        Text component2 = trans.Find("txt_name").GetComponent<Text>();
        Text component3 = trans.Find("txt_family").GetComponent<Text>();
        Text component4 = trans.Find("txt_win").GetComponent<Text>();
        Text component5 = trans.Find("txt_rate").GetComponent<Text>();
        Image component6 = trans.Find("img_rank").GetComponent<Image>();
        Image component7 = trans.Find("img_level").GetComponent<Image>();
        component.text = item.position.ToString();
        component2.text = item.name.ToString();
        component3.text = item.guildname.ToString();
        component4.text = item.winbattle.ToString();
        component5.text = item.winrate.ToString();
        LuaTable configTable = LuaConfigManager.GetConfigTable("rankpk_level", (ulong)item.ranklevel);
        if (configTable != null)
        {
            GlobalRegister.SetImage(4, configTable.GetField_String("icon"), component7, true);
        }
        component.gameObject.SetActive(item.position > 3U);
    }

    private void InitAbattorInfo()
    {
        try
        {
            LuaTable xmlConfigTable = LuaConfigManager.GetXmlConfigTable("mobapk");
            string cacheField_String = xmlConfigTable.GetCacheField_String("introduction");
            string cacheField_String2 = xmlConfigTable.GetCacheField_String("rewardtext");
            string cacheField_String3 = xmlConfigTable.GetCacheField_String("introimage");
            List<uint> list = new List<uint>();
            LuaTable cacheField_Table = xmlConfigTable.GetCacheField_Table("UIreward");
            for (int i = 0; i < cacheField_Table.Count; i++)
            {
                LuaTable luaTable = cacheField_Table[i + 1] as LuaTable;
                uint field_Uint = luaTable.GetField_Uint("id");
                list.Add(field_Uint);
            }
            RawImage component = this.transAbattoirInfo.Find("img_flag").GetComponent<RawImage>();
            this.SetImage(component, cacheField_String3);
            this.transAbattoirInfo.Find("rule/txt_rule").GetComponent<Text>().text = cacheField_String;
            this.transAbattoirInfo.Find("rule/wingetrewardword").GetComponent<Text>().text = cacheField_String2;
            for (int j = 0; j < 3; j++)
            {
                GameObject gameObject = this.transAbattoirInfo.Find("rule/award").GetChild(j).gameObject;
                if (j >= list.Count)
                {
                    gameObject.gameObject.SetActive(false);
                }
                else
                {
                    this.SetGoodsImage(gameObject, list[j]);
                    gameObject.gameObject.SetActive(true);
                }
            }
        }
        catch (Exception ex)
        {
            FFDebug.LogError(this, ex.Message);
        }
    }

    private void SetGoodsImage(GameObject obj, uint id)
    {
        UI_PVPMatch.EAwardItem eawardItem = default(UI_PVPMatch.EAwardItem);
        UI_PVPMatch.EAwardItem eawardItem2 = eawardItem;
        eawardItem2.mId = (int)id;
        eawardItem = eawardItem2;
        LuaTable configTable = LuaConfigManager.GetConfigTable("objects", (ulong)((long)eawardItem.mId));
        string field_String = configTable.GetField_String("icon");
        foreach (object obj2 in obj.transform)
        {
            Transform transform = (Transform)obj2;
            transform.GetComponent<Graphic>().raycastTarget = false;
        }
        obj.transform.localScale = Vector3.one;
        RawImage rawImage = obj.transform.Find("img_icon").GetComponent<RawImage>();
        ManagerCenter.Instance.GetManager<UITextureMgr>().GetTexture(ImageType.ITEM, field_String, delegate (UITextureAsset asset)
        {
            if (asset == null)
            {
                FFDebug.LogWarning("CommonItem", "  req  texture   is  null ");
                return;
            }
            if (rawImage == null)
            {
                return;
            }
            Texture2D textureObj = asset.textureObj;
            Sprite sprite = Sprite.Create(textureObj, new Rect(0f, 0f, (float)textureObj.width, (float)textureObj.height), new Vector2(0f, 0f));
            rawImage.texture = sprite.texture;
            rawImage.color = Color.white;
            rawImage.gameObject.SetActive(true);
        });
        obj.SetActive(true);
        obj.name = eawardItem.mId.ToString();
        UIEventListener.Get(obj).onEnter = new UIEventListener.VoidDelegate(this.btn_item_on_enter);
    }

    private void SetImage(RawImage image, string imageName)
    {
        base.GetTexture(ImageType.OTHERS, imageName, delegate (Texture2D item)
        {
            if (image != null && item != null)
            {
                image.texture = item;
            }
        });
    }

    public void InitPvpState(MSG_RetRankPKCurStage_SC info)
    {
        this.SwitchState(info.curstage);
    }

    public void SwitchState(StageType stage)
    {
        switch (stage)
        {
            case StageType.None_Stage:
                this.btnMatch.gameObject.SetActive(true);
                this.btnCancelMatch.gameObject.SetActive(false);
                break;
            case StageType.Match:
                this.btnMatch.gameObject.SetActive(false);
                this.btnCancelMatch.gameObject.SetActive(true);
                break;
        }
    }

    private void btn_battlemenuparent_onclick(PointerEventData eventData)
    {
        this.mBattleId = uint.Parse(eventData.pointerPress.name);
        this.SetupBattlePanel();
    }

    private string GetBattleMapName()
    {
        string result = "dqfb_01";
        uint num = this.mBattleId;
        if (num == 1U)
        {
            result = "dqfb_01";
        }
        return result;
    }

    private void SetupBattlePanel()
    {
        this.mBattleConfig = LuaConfigManager.GetConfigTable("battle_config", (ulong)this.mBattleId);
        if (this.mBattleConfig == null)
        {
            return;
        }
        ManagerCenter.Instance.GetManager<UITextureMgr>().GetTexture(ImageType.OTHERS, this.GetBattleMapName(), delegate (UITextureAsset asset)
        {
            if (this.imgMap == null)
            {
                return;
            }
            if (asset == null)
            {
                return;
            }
            this.usedTextureAssets.Add(asset);
            this.imgMap.gameObject.SetActive(true);
            this.imgMap.texture = asset.textureObj;
            this.imgMap.color = Color.white;
        });
        Text component = this.transBattleContainer.Find("img_bg/Text").GetComponent<Text>();
        string field_String = this.mBattleConfig.GetField_String("daily_reward_display");
        string field_String2 = this.mBattleConfig.GetField_String("win_reward_display");
        List<UI_PVPMatch.EAwardItem> awardList = this.GetAwardList(field_String);
        List<UI_PVPMatch.EAwardItem> awardList2 = this.GetAwardList(field_String2);
        GameObject gameObject = this.transBattleContainer.Find("img_bg/award/icon").gameObject;
        UIManager.Instance.ClearListChildrens(gameObject.transform.parent, 1);
        this.CreateAwardListUI(awardList, gameObject);
        gameObject = this.transBattleContainer.Find("img_bg/winaward/icon").gameObject;
        UIManager.Instance.ClearListChildrens(gameObject.transform.parent, 1);
        this.CreateAwardListUI(awardList2, gameObject);
        this.txtBattleCount = this.transBattleContainer.Find("img_bg/txt_time").GetComponent<Text>();
        this.controller.ReqBattleTimes();
    }

    private void CreateAwardListUI(List<UI_PVPMatch.EAwardItem> firstReward, GameObject objItem)
    {
        for (int i = 0; i < ((firstReward.Count <= 3) ? firstReward.Count : 3); i++)
        {
            UI_PVPMatch.EAwardItem eawardItem = firstReward[i];
            LuaTable configTable = LuaConfigManager.GetConfigTable("objects", (ulong)((long)eawardItem.mId));
            string field_String = configTable.GetField_String("icon");
            GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(objItem);
            gameObject.transform.SetParent(objItem.transform.parent);
            gameObject.transform.localScale = Vector3.one;
            RawImage rawImage = gameObject.transform.Find("img_icon").GetComponent<RawImage>();
            ManagerCenter.Instance.GetManager<UITextureMgr>().GetTexture(ImageType.ITEM, field_String, delegate (UITextureAsset asset)
            {
                if (asset == null)
                {
                    FFDebug.LogWarning("CommonItem", "  req  texture   is  null ");
                    return;
                }
                if (rawImage == null)
                {
                    return;
                }
                Texture2D textureObj = asset.textureObj;
                Sprite sprite = Sprite.Create(textureObj, new Rect(0f, 0f, (float)textureObj.width, (float)textureObj.height), new Vector2(0f, 0f));
                rawImage.texture = sprite.texture;
                rawImage.color = Color.white;
                rawImage.gameObject.SetActive(true);
            });
            Text component = gameObject.transform.Find("txt_num").GetComponent<Text>();
            component.text = eawardItem.mNum.ToString();
            component.gameObject.SetActive(true);
            gameObject.SetActive(true);
            gameObject.name = eawardItem.mId.ToString();
            UIEventListener.Get(gameObject).onEnter = new UIEventListener.VoidDelegate(this.btn_item_on_enter);
        }
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

    private void TryShowItemTip()
    {
        ControllerManager.Instance.GetController<ItemTipController>().OpenPanel(uint.Parse(this.mCurEnterBtn.name), this.mCurEnterBtn);
    }

    private List<UI_PVPMatch.EAwardItem> GetAwardList(string str)
    {
        List<UI_PVPMatch.EAwardItem> result;
        try
        {
            List<UI_PVPMatch.EAwardItem> list = new List<UI_PVPMatch.EAwardItem>();
            foreach (string text in str.Split(new char[]
            {
                '|'
            }))
            {
                string[] array2 = text.Split(new char[]
                {
                    '-'
                });
                list.Add(new UI_PVPMatch.EAwardItem
                {
                    mId = int.Parse(array2[0]),
                    mNum = int.Parse(array2[1])
                });
            }
            result = list;
        }
        catch (Exception)
        {
            result = new List<UI_PVPMatch.EAwardItem>();
        }
        return result;
    }

    private void btn_matching_onclick()
    {
        this.ReqBattleMatch();
    }

    private void btn_cancelmatching_onclick()
    {
        this.ReqBattleMatchCancel();
    }

    private void ReqBattleMatch()
    {
        this.controller.ReqBattleMatch(this.mBattleId);
    }

    public void RetBattleMatchCb(bool issucc)
    {
        if (issucc)
        {
            this.btn_matching.gameObject.SetActive(false);
            this.btn_cancelmatching.gameObject.SetActive(true);
        }
    }

    private void ReqBattleMatchCancel()
    {
        this.controller.ReqBattleCancelMatch(this.mBattleId);
    }

    public void RetBattleMatchCancelCb(bool issucc)
    {
        if (issucc)
        {
            this.btn_matching.gameObject.SetActive(true);
            this.btn_cancelmatching.gameObject.SetActive(false);
        }
    }

    public void RetBattleTimes(uint times)
    {
        string field_String = this.mBattleConfig.GetField_String("win_reward");
        string text = field_String.Split(new char[]
        {
            '|'
        })[0];
        string arg = text.Split(new char[]
        {
            '-'
        })[1];
        this.txtBattleCount.text = times + "/" + arg;
    }

    private Transform ui_root;

    private PVPMatchController controller;

    public Transform panelRoot;

    public Transform txtDan;

    public Image imgRank;

    public Image imgRankLv;

    public Transform transStar;

    public Transform btnMatch;

    public Transform btnCancelMatch;

    public Transform btnExit;

    public Transform btnRule;

    public Transform btnCloseRule;

    public Transform btnAbattoirMatch;

    public Transform btnAbattoirCancelMatch;

    public Transform tgMatch;

    public Transform tgBattle;

    public Transform tgRank;

    public Transform abattoirMatch;

    public Transform tgReward;

    public Transform transRankItem;

    public Toggle togRankFriend;

    public Transform txtSeasonValue;

    public Transform txtSeasonWinValue;

    public Transform txtSeasonWinRatioValue;

    public Transform txtSeasonRankValue;

    public Transform txtBestRankValue;

    public Transform txtBestDanValue;

    public Transform txtSeasonLeftDayValue;

    public Transform txtRankListSeasonValue;

    public Transform btnHelp;

    private Transform transPvpInfo;

    private Transform transPvpList;

    private Transform transAbattoirInfo;

    private Transform transPvpAward;

    private Transform transPvpRule;

    private List<Image> imgRewardRank = new List<Image>();

    private List<Transform> transRewardPanel = new List<Transform>();

    private RawImage imgMap;

    public GameObject obj_awarditem;

    public Button btn_matching;

    public Button btn_cancelmatching;

    private Text txtMatchResult;

    private Transform transBattleContainer;

    private int RANK_NUM = 4;

    private uint[] rankRewardAttr = new uint[]
    {
        1U,
        6U,
        11U,
        14U
    };

    private List<GameObject> effectObjects = new List<GameObject>();

    private Text txtBattleCount;

    private uint mBattleId = 1U;

    private LuaTable mBattleConfig;

    private mapinfo mMapConfig;

    private GameObject mCurEnterBtn;

    private struct EAwardItem
    {
        public int mId;

        public int mNum;
    }
}
