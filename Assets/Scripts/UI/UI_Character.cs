using System;
using System.Collections.Generic;
using career;
using Framework.Managers;
using Game.Scene;
using LuaInterface;
using magic;
using Obj;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Character : UIPanelBase
{
    private CardController cardController
    {
        get
        {
            return ControllerManager.Instance.GetController<CardController>();
        }
    }

    private HeroHandbookController heroHandbookController
    {
        get
        {
            return ControllerManager.Instance.GetController<HeroHandbookController>();
        }
    }

    public override void OnInit(Transform root)
    {
        this.uiRoot = root;
        this.mainPlayerSkillHolder = MainPlayer.Self.GetComponent<MainPlayerSkillHolder>();
        this.btn_switchhero = this.uiRoot.Find("Offset_Character/Panel_tab/btn_switchhero").GetComponent<Button>();
        this.trans_Mask = (this.uiRoot.Find("Offset_Character/HeroShow/Mask") as RectTransform);
        this.obj_img_icon = this.uiRoot.Find("Offset_Character/HeroShow/Mask/img_model").GetComponent<RawImage>();
        Button component = this.uiRoot.Find("Offset_Character/Panel_title/CloseButton").GetComponent<Button>();
        Button component2 = this.uiRoot.Find("Offset_Character/AttributePanel/Attribute/btn_detail").GetComponent<Button>();
        Button component3 = this.uiRoot.Find("Offset_Character/AttributePanel/DNA/btn_change").GetComponent<Button>();
        this.objGene = this.uiRoot.Find("Offset_Character/AttributePanel/DNA/RawImage_dna").gameObject;
        GameObject gameObject = this.uiRoot.Find("Offset_Character/lv_player/lv_pro/Background").gameObject;
        this.textTip = gameObject.GetComponent<TextTip>();
        if (this.textTip == null)
        {
            this.textTip = gameObject.AddComponent<TextTip>();
        }
        this.BASE_HEIGHT = Mathf.CeilToInt((this.obj_img_icon.GetComponent<Transform>() as RectTransform).sizeDelta.x);
        this.btn_switchhero.onClick.AddListener(new UnityAction(this.btn_switchhero_on_click));
        component.onClick.AddListener(new UnityAction(this.btn_close_on_click));
        component2.onClick.AddListener(new UnityAction(this.btn_attrdetail_on_click));
        component3.onClick.AddListener(new UnityAction(this.btn_genechange_on_click));
        UIEventListener.Get(this.obj_img_icon.gameObject).onDrag = new UIEventListener.VoidDelegate(this.obj_img_icon_on_drag);
        UIEventListener.Get(this.objGene).onEnter = new UIEventListener.VoidDelegate(this.btn_gene_enter);
        UIEventListener.Get(this.objGene).onDestroy = new UIEventListener.VoidDelegate(this.btn_gene_exit);
        UIEventListener.Get(this.objGene).onExit = new UIEventListener.VoidDelegate(this.btn_gene_exit);
        Transform transform = this.uiRoot.Find("Offset_Character/AttributePanel/Card/Content");
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform transform2 = transform.GetChild(i).Find("Content");
            for (int j = 0; j < transform2.childCount; j++)
            {
                Transform child = transform2.GetChild(j);
                if (child.GetComponent<DragDropButton>() == null)
                {
                    DragDropButton dragDropButton = child.gameObject.AddComponent<DragDropButton>();
                    dragDropButton.Initilize(UIRootType.Card, new Vector2(0f, (float)(i * 10 + j)), "Icon", null);
                    if (j == 0)
                    {
                        this.mCardBtnDic.Add(i + 1, new List<DragDropButton>());
                    }
                    this.mCardBtnDic[i + 1].Add(dragDropButton);
                }
                UIEventListener.Get(child.gameObject).onEnter = new UIEventListener.VoidDelegate(this.btn_card_enter);
                UIEventListener.Get(child.gameObject).onDestroy = new UIEventListener.VoidDelegate(this.btn_card_exit);
                UIEventListener.Get(child.gameObject).onExit = new UIEventListener.VoidDelegate(this.btn_card_exit);
            }
        }
        this.mHeroConfigDic = new Dictionary<int, LuaTable>();
        List<LuaTable> configTableList = LuaConfigManager.GetConfigTableList("heros");
        for (int k = 0; k < configTableList.Count; k++)
        {
            LuaTable luaTable = configTableList[k];
            this.mHeroConfigDic.Add(luaTable.GetField_Int("id"), luaTable);
        }
        this.mNpcConfigDic = new Dictionary<int, LuaTable>();
        List<LuaTable> configTableList2 = LuaConfigManager.GetConfigTableList("npc_data");
        for (int l = 0; l < configTableList2.Count; l++)
        {
            this.mNpcConfigDic.Add(configTableList2[l].GetField_Int("id"), configTableList2[l]);
        }
        this.mAttrConfigDic = new Dictionary<string, LuaTable>();
        List<LuaTable> configTableList3 = LuaConfigManager.GetConfigTableList("attribute_config");
        for (int m = 0; m < configTableList3.Count; m++)
        {
            if (!this.mAttrConfigDic.ContainsKey(configTableList3[m].GetField_String("attribute")))
            {
                this.mAttrConfigDic.Add(configTableList3[m].GetField_String("attribute"), configTableList3[m]);
            }
        }
        this.mSkillShowConfigDic = new Dictionary<int, LuaTable>();
        List<LuaTable> configTableList4 = LuaConfigManager.GetConfigTableList("skillshow");
        for (int n = 0; n < configTableList4.Count; n++)
        {
            this.mSkillShowConfigDic.Add(configTableList4[n].GetField_Int("id"), configTableList4[n]);
        }
        this.fashionSample = this.uiRoot.Find("Offset_Character/SkinPanel/breaklist/breakbar").gameObject;
        this.fashionSample.SetActive(false);
        this.fasshionEquip = this.uiRoot.Find("Offset_Character/SkinPanel/btn_use").GetComponent<Button>();
        this.fasshionEquip.onClick.AddListener(new UnityAction(this.btn_equip_fashion));
        this.fashionGetTips = this.uiRoot.Find("Offset_Character/HeroShow/Mask/img_lock").gameObject;
        this.fashionGetTipsTxt = this.uiRoot.Find("Offset_Character/HeroShow/Mask/img_lock/Text").GetComponent<Text>();
        this.heroAwakeRoot = this.uiRoot.Find("Offset_Character/Break").gameObject;
        this.heroAwakeSample = this.heroAwakeRoot.FindChild("breaklist/breakbar").gameObject;
        this.heroAwakeSample.SetActive(false);
        GameObject gameObject2 = this.heroAwakeRoot.FindChild("Panel_title/btn_close").gameObject;
        UIEventListener.Get(gameObject2.gameObject).onClick = new UIEventListener.VoidDelegate(this.onClickCloseHeroAwake);
        GameObject gameObject3 = this.uiRoot.Find("Offset_Character/HeroShow/btn_break").gameObject;
        UIEventListener.Get(gameObject3.gameObject).onClick = new UIEventListener.VoidDelegate(this.onClickOpenHeroAwake);
        this.SetupPanel();
        Transform transform3 = this.uiRoot.Find("Offset_Character/Panel_tab/ToggleGroup/skin");
        transform3.GetComponent<Toggle>().onValueChanged.AddListener(new UnityAction<bool>(this.onToggleChange));
        base.OnInit(root);
        GameScene manager = ManagerCenter.Instance.GetManager<GameScene>();
        bool flag = !manager.isAbattoirScene;
        this.btn_switchhero.gameObject.SetActive(flag);
        transform3.gameObject.SetActive(flag);
        this.uiRoot.Find("Offset_Character/AttributePanel/DNA").gameObject.SetActive(flag);
        ControllerManager.Instance.GetController<GeneController>().RegOnDnaPageDataChange(new Action<Dictionary<string, int>, string>(this.RefreshGenePanel), flag);
    }

    private void onToggleChange(bool on)
    {
        if (on)
        {
            FashionController controller = ControllerManager.Instance.GetController<FashionController>();
            this.onSelectFashion((int)controller.AllFasion.equipId);
        }
        else
        {
            this.fashionGetTips.SetActive(false);
            this.resetMapShowInfo();
            this.showPlayRttInfo();
        }
    }

    public void SetupPanel()
    {
        this.SetupMainPanel();
        this.SetupSkillPanel();
        this.RefreshFashionInfo();
    }

    private void RefreshOnlineHeroData()
    {
        ulong thisid = ulong.Parse(MainPlayer.Self.MainPlayeData.CharacterBaseData.herothisid);
        int heroIdByThisid = this.heroHandbookController.GetHeroIdByThisid(thisid);
        this.ltOnlineHeroData = this.heroHandbookController.GetHeroDataByHeroId(heroIdByThisid);
    }

    private void SetupMainPanel()
    {
        this.RefreshOnlineHeroData();
        GameScene manager = ManagerCenter.Instance.GetManager<GameScene>();
        if (!manager.isAbattoirScene)
        {
            this.heroHandbookController.mCharacterNetwork.ReqGetMainHero();
            this.cardController.ReqCardPackInfo();
            HeroAwakeController controller = ControllerManager.Instance.GetController<HeroAwakeController>();
            controller.reqEvolutionData = true;
        }
        LuaScriptMgr.Instance.CallLuaFunction("HerosCtrl.ReqHeroAttributeDataCs", new object[]
        {
            MainPlayer.Self.MainPlayeData.CharacterBaseData.herothisid
        });
        this.SetupMainInfoPanel();
    }

    private void SetupMainInfoPanel()
    {
        if (this.ltOnlineHeroData == null)
        {
            FFDebug.LogError(this, "ltOnlineHeroData == null");
            return;
        }
        Transform transform = this.uiRoot.Find("Offset_Character/HeroShow");
        Transform transform2 = transform.Find("Name");
        Transform transform3 = transform.Find("Desc");
        Transform transform4 = transform.Find("img_skinlock");
        Transform transform5 = transform.Find("Mask/Panel/panel_ce/panel_value/img_num");
        int field_Int = this.ltOnlineHeroData.GetField_Int("baseid");
        if (!this.mHeroConfigDic.ContainsKey(field_Int))
        {
            return;
        }
        LuaTable luaTable = this.mHeroConfigDic[field_Int];
        if (transform2)
        {
            transform2.GetComponent<Text>().text = luaTable.GetField_String("heroname");
        }
        if (transform3)
        {
            transform3.GetComponent<Text>().text = luaTable.GetField_String("desc");
        }
        this.resetMapShowInfo();
        this.avatarid = MainPlayer.Self.OtherPlayerData.MapUserData.mapshow.avatarid;
        this.showPlayRttInfo();
        if (transform4)
        {
            transform4.gameObject.SetActive(false);
        }
        this.RefreshExpPanel();
        this.RefreshFightValue();
        Transform transform6 = this.uiRoot.Find("Offset_Character/AttributePanel/DNA/txt_name");
        transform6.GetComponent<Text>().text = this.mGeneTabName;
    }

    private void showPlayRttInfo()
    {
        int field_Int = this.ltOnlineHeroData.GetField_Int("baseid");
        uint[] featureIDs = new uint[]
        {
            MainPlayer.Self.OtherPlayerData.MapUserData.mapshow.haircolor,
            this.hair,
            this.head,
            MainPlayer.Self.OtherPlayerData.MapUserData.mapshow.antenna,
            this.avatarid
        };
        GlobalRegister.ShowPlayerRTT(this.obj_img_icon, (uint)field_Int, this.body, featureIDs, 1, new Action<GameObject>(this.InitScale));
    }

    private void InitScale(GameObject obj)
    {
        LuaTable configTable = LuaConfigManager.GetConfigTable("npc_data", (ulong)((long)this.ltOnlineHeroData.GetField_Int("baseid")));
        if (configTable != null)
        {
            string field_String = configTable.GetField_String("uimodelsize");
            float num = 1f;
            float.TryParse(field_String, out num);
            num = ((num <= 0f) ? 1f : num);
            obj.transform.localScale = Vector3.one * num;
        }
    }

    public void RefreshExpPanel()
    {
        Transform transform = this.uiRoot.Find("Offset_Character/lv_player/txt_lv");
        Transform transform2 = this.uiRoot.Find("Offset_Character/lv_player/lv_pro");
        MainUIController controller = ControllerManager.Instance.GetController<MainUIController>();
        uint curLv = controller.GetCurLv();
        ulong curExp = controller.GetCurExp();
        uint curLevelAllExp = controller.GetCurLevelAllExp(curLv);
        if (transform)
        {
            transform.GetComponent<Text>().text = curLv.ToString();
        }
        if (transform2)
        {
            if (curLevelAllExp != 0U)
            {
                transform2.GetComponent<Slider>().value = curExp * 1f / curLevelAllExp;
            }
            this.textTip.SetText(curExp + "/" + curLevelAllExp);
        }
    }

    public void RefreshFightValue()
    {
        Transform transform = this.uiRoot.Find("Offset_Character/HeroShow");
        Transform transform2 = transform.Find("Mask/Panel/panel_ce/panel_value/img_num");
        FightValueNum fightValueNum = transform2.transform.parent.GetComponent<FightValueNum>();
        if (fightValueNum == null)
        {
            fightValueNum = transform2.transform.parent.gameObject.AddComponent<FightValueNum>();
        }
        fightValueNum.SetNum(MainPlayer.Self.GetMainPlayerFightValue());
    }

    public void SetupCardPanel(CardPackInfo cardPackInfo)
    {
        Dictionary<uint, t_Object> dictionary = new Dictionary<uint, t_Object>();
        for (int i = 0; i < cardPackInfo.objs.Count; i++)
        {
            t_Object t_Object = cardPackInfo.objs[i];
            dictionary.Add(t_Object.grid_y, t_Object);
        }
        CardController controller = ControllerManager.Instance.GetController<CardController>();
        using (Dictionary<int, List<DragDropButton>>.KeyCollection.Enumerator enumerator = this.mCardBtnDic.Keys.GetEnumerator())
        {
            while (enumerator.MoveNext())
            {
                uint num = (uint)enumerator.Current;
                uint cardTypeSlotNum = controller.GetCardTypeSlotNum(num);
                uint cardTypeSlotOpenNum = controller.GetCardTypeSlotOpenNum(num);
                List<DragDropButton> list = this.mCardBtnDic[(int)num];
                for (int j = 0; j < list.Count; j++)
                {
                    DragDropButton dragDraopBtn = list[j];
                    dragDraopBtn.gameObject.SetActive((long)j < (long)((ulong)cardTypeSlotNum));
                    dragDraopBtn.transform.Find("img_close").gameObject.SetActive((long)j >= (long)((ulong)cardTypeSlotOpenNum));
                    dragDraopBtn.GetComponent<Image>().color = (((long)j < (long)((ulong)cardTypeSlotOpenNum)) ? new Color(1f, 1f, 1f, 1f) : new Color(1f, 1f, 1f, 0f));
                    GameObject gameObject = dragDraopBtn.transform.Find("img_repair").gameObject;
                    gameObject.SetActive(false);
                    if (dictionary.ContainsKey((uint)dragDraopBtn.mPos.y))
                    {
                        dragDraopBtn.mData = new CardDragDropButtonData(controller.GetCardByPos((uint)dragDraopBtn.mPos.y));
                        t_Object t_Object2 = dictionary[(uint)dragDraopBtn.mPos.y];
                        dragDraopBtn.mData.thisid = t_Object2.thisid;
                        dragDraopBtn.mData.mId = t_Object2.baseid;
                        LuaTable configTable = LuaConfigManager.GetConfigTable("objects", (ulong)dragDraopBtn.mData.mId);
                        if (configTable != null)
                        {
                            ManagerCenter.Instance.GetManager<UITextureMgr>().GetTexture(ImageType.ITEM, configTable.GetField_String("icon"), delegate (UITextureAsset asset)
                            {
                                if (asset == null)
                                {
                                    FFDebug.LogWarning("CommonItem", "  req  texture   is  null ");
                                    return;
                                }
                                if (dragDraopBtn == null || dragDraopBtn.mIcon == null)
                                {
                                    return;
                                }
                                Texture2D textureObj = asset.textureObj;
                                Sprite sprite = Sprite.Create(textureObj, new Rect(0f, 0f, (float)textureObj.width, (float)textureObj.height), new Vector2(0f, 0f));
                                dragDraopBtn.mIcon.sprite = sprite;
                                dragDraopBtn.mIcon.overrideSprite = sprite;
                                dragDraopBtn.mIcon.color = Color.white;
                                dragDraopBtn.mIcon.gameObject.SetActive(true);
                            });
                        }
                        GameObject qualityObj = dragDraopBtn.transform.Find("quality").gameObject;
                        qualityObj.SetActive(false);
                        string imgname = "quality" + configTable.GetField_Uint("quality");
                        ManagerCenter.Instance.GetManager<UITextureMgr>().GetTexture(ImageType.RANK, imgname, delegate (UITextureAsset asset)
                        {
                            if (asset == null)
                            {
                                return;
                            }
                            if (qualityObj == null)
                            {
                                return;
                            }
                            qualityObj.SetActive(true);
                            qualityObj.GetComponent<RawImage>().texture = asset.textureObj;
                        });
                        if (t_Object2 != null)
                        {
                            LuaTable configTable2 = LuaConfigManager.GetConfigTable("carddata_config", (ulong)dragDraopBtn.mData.mId);
                            if (configTable2 != null)
                            {
                                uint field_Uint = configTable2.GetField_Uint("maxdurable");
                                if (t_Object2.card_data.durability < field_Uint)
                                {
                                    gameObject.transform.localScale = new Vector3(1f, 1f - t_Object2.card_data.durability * 1f / field_Uint, 1f);
                                    gameObject.SetActive(true);
                                }
                            }
                        }
                    }
                    else
                    {
                        dragDraopBtn.mData = null;
                        dragDraopBtn.mIcon.color = new Color(0f, 0f, 0f, 0f);
                        dragDraopBtn.transform.Find("quality").gameObject.SetActive(false);
                    }
                }
            }
        }
    }

    private void btn_card_enter(PointerEventData eventData)
    {
        DragDropButton component = eventData.pointerEnter.GetComponent<DragDropButton>();
        if (DragDropManager.Instance.IsDraging())
        {
            CardController controller = ControllerManager.Instance.GetController<CardController>();
            if (!component.transform.Find("img_close").gameObject.activeSelf && controller.EqualCurDragButtonType(component))
            {
                if (!controller.EnterCheckCard(component))
                {
                    component.transform.Find("img_green").gameObject.SetActive(false);
                    component.transform.Find("img_red").gameObject.SetActive(true);
                }
                else
                {
                    component.transform.Find("img_green").gameObject.SetActive(true);
                    component.transform.Find("img_red").gameObject.SetActive(false);
                }
            }
        }
        CardDragDropButtonData cardDragDropButtonData = component.mData as CardDragDropButtonData;
        if (cardDragDropButtonData != null && cardDragDropButtonData.data != null)
        {
            ControllerManager.Instance.GetController<ItemTipController>().OpenPanel(cardDragDropButtonData.data, eventData.pointerCurrentRaycast.gameObject);
        }
    }

    private void btn_card_exit(PointerEventData eventData)
    {
        if (eventData != null)
        {
            eventData.pointerEnter.transform.Find("img_green").gameObject.SetActive(false);
            eventData.pointerEnter.transform.Find("img_red").gameObject.SetActive(false);
        }
    }

    public void StartDragCard(uint cardtype)
    {
        foreach (int num in this.mCardBtnDic.Keys)
        {
            for (int i = 0; i < this.mCardBtnDic[num].Count; i++)
            {
                bool flag = (long)num != (long)((ulong)cardtype) && cardtype != 0U;
                Transform transform = this.mCardBtnDic[num][i].transform;
                transform.GetComponent<Image>().color = ((!flag) ? new Color(1f, 1f, 1f, 1f) : new Color(1f, 1f, 1f, 0.5f));
                transform.Find("img_close").GetComponent<Image>().color = ((!flag) ? new Color(1f, 1f, 1f, 1f) : new Color(1f, 1f, 1f, 0.5f));
                if (transform.Find("img_close").gameObject.activeSelf)
                {
                    transform.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0f);
                }
            }
        }
    }

    public void CacelDragCard()
    {
        foreach (int key in this.mCardBtnDic.Keys)
        {
            for (int i = 0; i < this.mCardBtnDic[key].Count; i++)
            {
                Transform transform = this.mCardBtnDic[key][i].transform;
                transform.Find("img_green").gameObject.SetActive(false);
                transform.Find("img_red").gameObject.SetActive(false);
                transform.Find("img_close").GetComponent<Image>().color = Color.white;
                transform.GetComponent<Image>().color = Color.white;
                if (transform.Find("img_close").gameObject.activeSelf)
                {
                    transform.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0f);
                }
            }
        }
    }

    public void SetupHeroInfoPanel()
    {
        List<AttrDataItemUI> list = new List<AttrDataItemUI>();
        if (this.ltOnlineHeroData == null)
        {
            FFDebug.LogError(this, "ltOnlineHeroData == null");
            return;
        }
        LuaTable cacheField_Table = this.ltOnlineHeroData.GetCacheField_Table("AllAttribute");
        int field_Int = this.ltOnlineHeroData.GetField_Int("baseid");
        MainUIController controller = ControllerManager.Instance.GetController<MainUIController>();
        int curLv = (int)controller.GetCurLv();
        foreach (string text in this.mAttrConfigDic.Keys)
        {
            if (this.mAttrConfigDic[text].GetField_Int("cui") == 1)
            {
                string luaAttrName = CommonTools.GetLuaAttrName(text);
                float num = (float)cacheField_Table.GetField_Int(luaAttrName);
                float num2 = (float)this.mHeroConfigDic[field_Int].GetField_Int(text) / 10000f * (float)curLv + (float)this.mNpcConfigDic[field_Int].GetField_Int(text);
                float num3 = num - num2;
                string field_String = this.mAttrConfigDic[text].GetField_String("name");
                AttrDataItemUI item = new AttrDataItemUI
                {
                    name = field_String,
                    totalValue = num,
                    extraValue = ((num3 >= 0f) ? num3 : 0f)
                };
                list.Add(item);
            }
        }
        Transform transform = this.uiRoot.Find("Offset_Character/AttributePanel/Attribute/Panel_Affix/Affix");
        Transform transform2 = this.uiRoot.Find("Offset_Character/AttributePanel/Attribute/Panel_Value/Value");
        UIManager.Instance.ClearListChildrens(transform.parent, 1);
        UIManager.Instance.ClearListChildrens(transform2.parent, 1);
        for (int i = 0; i < 5; i++)
        {
            AttrDataItemUI attrDataItemUI = list[i];
            GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(transform.gameObject);
            gameObject.transform.SetParent(transform.parent);
            gameObject.transform.localScale = Vector3.one;
            gameObject.SetActive(true);
            gameObject.GetComponent<Text>().text = attrDataItemUI.name;
            GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(transform2.gameObject);
            gameObject2.transform.SetParent(transform2.parent);
            gameObject2.transform.localScale = Vector3.one;
            gameObject2.SetActive(true);
            gameObject2.transform.Find("txt_total").GetComponent<Text>().text = Mathf.CeilToInt(attrDataItemUI.totalValue).ToString();
            gameObject2.transform.Find("txt_add").GetComponent<Text>().text = ((Mathf.CeilToInt(attrDataItemUI.extraValue) <= 0) ? string.Empty : ("(+" + Mathf.CeilToInt(attrDataItemUI.extraValue) + ")"));
        }
        Transform transform3 = this.uiRoot.Find("Offset_Character/AttributeDetail/detail/Panel/Attr");
        UIManager.Instance.ClearListChildrens(transform3.parent, 1);
        for (int j = 0; j < list.Count; j++)
        {
            AttrDataItemUI attrDataItemUI2 = list[j];
            GameObject gameObject3 = UnityEngine.Object.Instantiate<GameObject>(transform3.gameObject);
            gameObject3.transform.SetParent(transform3.parent);
            gameObject3.transform.localScale = Vector3.one;
            gameObject3.SetActive(true);
            gameObject3.transform.Find("Name").GetComponent<Text>().text = attrDataItemUI2.name;
            gameObject3.transform.Find("Value/total").GetComponent<Text>().text = Mathf.CeilToInt(attrDataItemUI2.totalValue).ToString();
            gameObject3.transform.Find("Value/extra").GetComponent<Text>().text = ((Mathf.CeilToInt(attrDataItemUI2.extraValue) <= 0) ? string.Empty : ("(+" + Mathf.CeilToInt(attrDataItemUI2.extraValue) + ")"));
        }
    }

    private void RefreshGenePanel(Dictionary<string, int> attrDic, string geneTabName)
    {
        this.mGeneTabName = geneTabName;
        this.mAttrDic = new Dictionary<string, int>();
        foreach (string text in attrDic.Keys)
        {
            string key = text;
            if (this.mAttrConfigDic.ContainsKey(text))
            {
                key = this.mAttrConfigDic[text].GetField_String("name");
            }
            this.mAttrDic.Add(key, attrDic[text]);
        }
        if (this.uiRoot != null)
        {
            Transform transform = this.uiRoot.Find("Offset_Character/AttributePanel/DNA/txt_name");
            if (transform != null)
            {
                transform.GetComponent<Text>().text = this.mGeneTabName;
            }
        }
    }

    private void btn_gene_enter(PointerEventData eventData)
    {
        if (this.mAttrDic != null)
        {
            Scheduler.Instance.AddTimer(0.5f, false, new Scheduler.OnScheduler(this.ShowGeneTip));
        }
    }

    private void ShowGeneTip()
    {
        ControllerManager.Instance.GetController<ItemTipController>().OpenAttrPanel(this.mAttrDic, this.objGene);
    }

    private void btn_gene_exit(PointerEventData eventData)
    {
        Scheduler.Instance.RemoveTimer(new Scheduler.OnScheduler(this.ShowGeneTip));
    }

    private void btn_genechange_on_click()
    {
        GlobalRegister.OpenGeneUI(0);
    }

    private void btn_attrdetail_on_click()
    {
        GameObject gameObject = this.uiRoot.Find("Offset_Character/AttributeDetail").gameObject;
        gameObject.SetActive(!gameObject.activeSelf);
    }

    private void btn_close_on_click()
    {
        UIManager.Instance.DeleteUI<UI_Character>();
    }

    private void obj_img_icon_on_drag(PointerEventData eventData)
    {
        IconRenderCtrl component = this.obj_img_icon.GetComponent<IconRenderCtrl>();
        if (component != null && component.target != null)
        {
            component.target.transform.eulerAngles += new Vector3(0f, -eventData.delta.x, 0f);
        }
    }

    public override void OnDispose()
    {
        base.OnDispose();
    }

    private void btn_switchhero_on_click()
    {
        this.heroHandbookController.SwitchHero();
    }

    private void skill_button_enter(PointerEventData eventData)
    {
        this.mEnterSkillButton = eventData.pointerEnter.gameObject;
        Scheduler.Instance.AddTimer(0.2f, false, new Scheduler.OnScheduler(this.TryShowSkillTip));
    }

    private void TryShowSkillTip()
    {
        if (this.mEnterSkillButton == null)
        {
            return;
        }
        uint mMouseEnterSkillId = uint.Parse(this.mEnterSkillButton.name);
        MainPlayerSkillBase skillData = this.GetSkillData(mMouseEnterSkillId);
        if (skillData != null)
        {
            ControllerManager.Instance.GetController<ItemTipController>().OpenSkillPanel(skillData, this.mEnterSkillButton);
        }
    }

    private MainPlayerSkillBase GetSkillData(uint mMouseEnterSkillId)
    {
        if (this.mainPlayerSkillHolder.MainPlayerSkillList.ContainsKey(mMouseEnterSkillId))
        {
            return this.mainPlayerSkillHolder.MainPlayerSkillList[mMouseEnterSkillId];
        }
        if (UI_Character.SkillList.ContainsKey(mMouseEnterSkillId))
        {
            return UI_Character.SkillList[mMouseEnterSkillId];
        }
        MainPlayerSkillBase mainPlayerSkillBase = new MainPlayerSkillBase(MainPlayerSkillHolder.Instance, mMouseEnterSkillId, 1U, 0U);
        mainPlayerSkillBase.skilllock = true;
        UI_Character.SkillList.Add(mMouseEnterSkillId, mainPlayerSkillBase);
        return mainPlayerSkillBase;
    }

    private MainPlayerSkillBase GetBeidongSkillData(uint skillid)
    {
        List<careerunlockItem> dicEquipSkill = ControllerManager.Instance.GetController<SkillViewControll>().dicEquipSkill;
        for (int i = 0; i < dicEquipSkill.Count; i++)
        {
            SkillData skill = dicEquipSkill[i].skill;
            if (skill.skillid == skillid)
            {
                return new MainPlayerSkillBase(MainPlayer.Self.GetComponent<MainPlayerSkillHolder>(), skill.skillid, skill.level, skill.skillcd);
            }
        }
        return null;
    }

    private void skill_button_exit(PointerEventData eventData)
    {
        this.mEnterSkillButton = null;
        Scheduler.Instance.RemoveTimer(new Scheduler.OnScheduler(this.TryShowSkillTip));
    }

    private void SetupSkillPanel()
    {
        Transform transform = this.uiRoot.Find("Offset_Character/SkillPanel/skillshow/Content/Panel");
        transform.Find("Image/txt_name").GetComponent<Text>().text = "通用";
        Transform transform2 = transform.Find("skill_icon/img_skill");
        UIManager.Instance.ClearListChildrens(transform2.parent, 1);
        if (this.ltOnlineHeroData == null)
        {
            FFDebug.LogError(this, "ltOnlineHeroData == null");
            return;
        }
        int field_Int = this.ltOnlineHeroData.GetField_Int("baseid");
        if (!this.mSkillShowConfigDic.ContainsKey(field_Int))
        {
            return;
        }
        string field_String = this.mSkillShowConfigDic[field_Int].GetField_String("skill");
        string[] array = field_String.Split(new string[]
        {
            "|"
        }, StringSplitOptions.RemoveEmptyEntries);
        for (int i = 0; i < array.Length; i++)
        {
            int mMouseEnterSkillId = int.Parse(array[i]);
            GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(transform2.gameObject);
            gameObject.name = mMouseEnterSkillId.ToString();
            gameObject.transform.SetParent(transform2.parent);
            gameObject.transform.localScale = Vector3.one;
            gameObject.SetActive(true);
            UIEventListener.Get(gameObject).onEnter = new UIEventListener.VoidDelegate(this.skill_button_enter);
            UIEventListener.Get(gameObject).onDestroy = new UIEventListener.VoidDelegate(this.skill_button_exit);
            UIEventListener.Get(gameObject).onExit = new UIEventListener.VoidDelegate(this.skill_button_exit);
            MainPlayerSkillBase skillData = this.GetSkillData((uint)mMouseEnterSkillId);
            if (skillData != null)
            {
                string field_String2 = skillData.SkillConfig.GetField_String("skillname");
                uint field_Uint = skillData.SkillConfig.GetField_Uint("unlocklevel");
                bool canBePassive = skillData.GetCanBePassive();
                Text component = gameObject.transform.Find("txt_name").GetComponent<Text>();
                Text component2 = gameObject.transform.Find("txt_learn").GetComponent<Text>();
                GameObject gameObject2 = gameObject.transform.Find("Image/img_lock").gameObject;
                GameObject gameObject3 = gameObject.transform.Find("txt_type1").gameObject;
                GameObject gameObject4 = gameObject.transform.Find("txt_type2").gameObject;
                Text component3 = gameObject.transform.Find("txt_value").GetComponent<Text>();
                component.text = field_String2;
                component2.text = field_Uint.ToString();
                gameObject2.SetActive(skillData.skilllock);
                gameObject3.SetActive(canBePassive);
                gameObject4.SetActive(!canBePassive);
                component3.text = skillData.Level.ToString() + "级";
                GameObject gameObject5 = gameObject.transform.Find("Image/img_icon").gameObject;
                gameObject5.SetActive(true);
                gameObject5.name = mMouseEnterSkillId.ToString();
                Image imgIcon = gameObject5.GetComponent<Image>();
                ManagerCenter.Instance.GetManager<UITextureMgr>().GetTexture(ImageType.ITEM, skillData.GetCurSkillIconName(), delegate (UITextureAsset asset)
                {
                    if (asset != null && imgIcon != null)
                    {
                        Texture2D textureObj = asset.textureObj;
                        Sprite sprite = Sprite.Create(textureObj, new Rect(0f, 0f, (float)textureObj.width, (float)textureObj.height), new Vector2(0f, 0f));
                        imgIcon.sprite = sprite;
                        imgIcon.overrideSprite = sprite;
                        imgIcon.color = Color.white;
                        imgIcon.gameObject.SetActive(true);
                        imgIcon.material = null;
                    }
                });
            }
        }
        Transform transform3 = this.uiRoot.Find("Offset_Character/SkillPanel/skillshow/Content/Panel (1)");
        transform3.Find("Image/txt_name").GetComponent<Text>().text = "觉醒";
        Transform transform4 = transform3.Find("skill_icon/img_skill");
        UIManager.Instance.ClearListChildrens(transform4.parent, 1);
        string field_String3 = this.mSkillShowConfigDic[field_Int].GetField_String("breakskill");
        array = field_String3.Split(new string[]
        {
            "|"
        }, StringSplitOptions.RemoveEmptyEntries);
        for (int j = 0; j < array.Length; j++)
        {
            int mMouseEnterSkillId2 = int.Parse(array[j]);
            GameObject gameObject6 = UnityEngine.Object.Instantiate<GameObject>(transform4.gameObject);
            gameObject6.name = mMouseEnterSkillId2.ToString();
            gameObject6.transform.SetParent(transform4.parent);
            gameObject6.transform.localScale = Vector3.one;
            gameObject6.SetActive(true);
            UIEventListener.Get(gameObject6).onEnter = new UIEventListener.VoidDelegate(this.skill_button_enter);
            UIEventListener.Get(gameObject6).onDestroy = new UIEventListener.VoidDelegate(this.skill_button_exit);
            UIEventListener.Get(gameObject6).onExit = new UIEventListener.VoidDelegate(this.skill_button_exit);
            MainPlayerSkillBase skillData2 = this.GetSkillData((uint)mMouseEnterSkillId2);
            if (skillData2 != null)
            {
                string field_String4 = skillData2.SkillConfig.GetField_String("skillname");
                uint field_Uint2 = skillData2.SkillConfig.GetField_Uint("unlocklevel");
                bool canBePassive2 = skillData2.GetCanBePassive();
                Text component4 = gameObject6.transform.Find("txt_name").GetComponent<Text>();
                Text component5 = gameObject6.transform.Find("txt_learn").GetComponent<Text>();
                GameObject gameObject7 = gameObject6.transform.Find("Image/img_lock").gameObject;
                GameObject gameObject8 = gameObject6.transform.Find("txt_type1").gameObject;
                GameObject gameObject9 = gameObject6.transform.Find("txt_type2").gameObject;
                Text component6 = gameObject6.transform.Find("txt_value").GetComponent<Text>();
                component4.text = field_String4;
                component5.text = field_Uint2.ToString();
                gameObject7.SetActive(skillData2.skilllock);
                gameObject8.SetActive(canBePassive2);
                gameObject9.SetActive(!canBePassive2);
                component6.text = skillData2.Level.ToString() + "级";
                GameObject gameObject10 = gameObject6.transform.Find("Image/img_icon").gameObject;
                gameObject10.SetActive(true);
                gameObject10.name = mMouseEnterSkillId2.ToString();
                Image imgIcon = gameObject10.GetComponent<Image>();
                ManagerCenter.Instance.GetManager<UITextureMgr>().GetTexture(ImageType.ITEM, skillData2.GetCurSkillIconName(), delegate (UITextureAsset asset)
                {
                    if (asset != null && imgIcon != null)
                    {
                        Texture2D textureObj = asset.textureObj;
                        Sprite sprite = Sprite.Create(textureObj, new Rect(0f, 0f, (float)textureObj.width, (float)textureObj.height), new Vector2(0f, 0f));
                        imgIcon.sprite = sprite;
                        imgIcon.overrideSprite = sprite;
                        imgIcon.color = Color.white;
                        imgIcon.gameObject.SetActive(true);
                        imgIcon.material = null;
                    }
                });
            }
        }
    }

    public void RefreshFashionInfo()
    {
        ulong thisid = ulong.Parse(MainPlayer.Self.MainPlayeData.CharacterBaseData.herothisid);
        int heroIdByThisid = this.heroHandbookController.GetHeroIdByThisid(thisid);
        if (heroIdByThisid != this.lastHeroid)
        {
            this.resetAllShowInfo();
        }
        this.lastHeroid = heroIdByThisid;
        if (!this.mHeroConfigDic.ContainsKey(heroIdByThisid))
        {
            this.resetAllShowInfo();
            return;
        }
        FashionController controller = ControllerManager.Instance.GetController<FashionController>();
        LuaTable luaTable = this.mHeroConfigDic[heroIdByThisid];
        string[] array = luaTable.GetField_String("avatar").Split(new char[]
        {
            '|'
        });
        for (int i = 0; i < array.Length; i++)
        {
            int num = -1;
            int.TryParse(array[i], out num);
            if (num > 0)
            {
                bool actived = controller.CheckActived(num);
                bool equiped = controller.CheckEquiped(num);
                if (!this.m_fashionInfos.ContainsKey(num))
                {
                    GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.fashionSample);
                    gameObject.transform.SetParent(this.fashionSample.transform.parent);
                    gameObject.transform.localScale = Vector3.one;
                    gameObject.SetActive(true);
                    FashionItem fashionItem = gameObject.AddComponent<FashionItem>();
                    fashionItem.SetFashionInfo(this, gameObject, num);
                    this.m_fashionInfos[num] = fashionItem;
                }
                LuaTable configTable = LuaConfigManager.GetConfigTable("avatar_config", (ulong)((long)num));
                this.m_fashionInfos[num].RefreshItem(configTable, equiped, actived);
            }
        }
    }

    public void UpdateSelectFashion(int selectid)
    {
        this.lastSelectFashionid = selectid;
    }

    private void resetAllShowInfo()
    {
        foreach (KeyValuePair<int, FashionItem> keyValuePair in this.m_fashionInfos)
        {
            UnityEngine.Object.Destroy(keyValuePair.Value.Root);
        }
        this.m_fashionInfos.Clear();
    }

    private void btn_equip_fashion()
    {
        if (this.lastSelectFashionid <= -1)
        {
            return;
        }
        ControllerManager.Instance.GetController<FashionController>().ReqEquipFasion((uint)this.lastSelectFashionid);
    }

    public void onSelectFashion(int fashionid)
    {
        if (fashionid == this.lastSelectFashionid)
        {
            return;
        }
        if (this.m_fashionInfos.ContainsKey(this.lastSelectFashionid))
        {
            this.m_fashionInfos[this.lastSelectFashionid].SetSelect(false);
        }
        this.lastSelectFashionid = fashionid;
        if (this.m_fashionInfos.ContainsKey(this.lastSelectFashionid))
        {
            this.m_fashionInfos[this.lastSelectFashionid].SetSelect(true);
        }
        bool flag = ControllerManager.Instance.GetController<FashionController>().CheckActived(this.lastSelectFashionid);
        LuaTable configTable = LuaConfigManager.GetConfigTable("avatar_config", (ulong)((long)this.lastSelectFashionid));
        this.fashionGetTips.SetActive(!flag);
        this.fashionGetTipsTxt.text = configTable.GetField_String("unlockdesc");
        this.updateShowInfo(this.lastSelectFashionid);
        this.showPlayRttInfo();
    }

    private void updateShowInfo(int fashionid)
    {
        LuaTable configTable = LuaConfigManager.GetConfigTable("avatar_config", (ulong)((long)fashionid));
        if (configTable == null)
        {
            return;
        }
        int mainHeroId = this.heroHandbookController.GetMainHeroId();
        int field_Int = this.ltOnlineHeroData.GetField_Int("baseid");
        LuaTable luaTable = this.mHeroConfigDic[field_Int];
        int cacheField_Int = luaTable.GetCacheField_Int("newavatar");
        if (cacheField_Int == fashionid && field_Int != mainHeroId)
        {
            this.resetMapShowInfo();
        }
        else
        {
            int field_Int2 = configTable.GetField_Int("hair");
            int field_Int3 = configTable.GetField_Int("body");
            int field_Int4 = configTable.GetField_Int("head");
            this.avatarid = (uint)fashionid;
            if (field_Int2 > 0)
            {
                this.hair = (uint)field_Int2;
            }
            if (field_Int3 > 0)
            {
                this.body = (uint)field_Int3;
            }
            if (field_Int4 > 0)
            {
                this.head = (uint)field_Int4;
            }
        }
    }

    private void resetMapShowInfo()
    {
        this.hair = MainPlayer.Self.OtherPlayerData.MapUserData.mapshow.hairstyle;
        this.body = MainPlayer.Self.OtherPlayerData.MapUserData.mapshow.bodystyle;
        this.head = MainPlayer.Self.OtherPlayerData.MapUserData.mapshow.facestyle;
        this.avatarid = MainPlayer.Self.OtherPlayerData.MapUserData.mapshow.avatarid;
    }

    private void onClickCloseHeroAwake(PointerEventData eventData)
    {
        this.heroAwakeRoot.SetActive(false);
    }

    private void onClickOpenHeroAwake(PointerEventData eventData)
    {
        this.heroAwakeRoot.SetActive(true);
        this.RefreshAwakeInfo();
    }

    public void RefreshAwakeInfo()
    {
        if (!this.heroAwakeRoot.activeSelf)
        {
            return;
        }
        ulong thisid = ulong.Parse(MainPlayer.Self.MainPlayeData.CharacterBaseData.herothisid);
        int heroIdByThisid = this.heroHandbookController.GetHeroIdByThisid(thisid);
        if (heroIdByThisid != this.lastAwakeHeroid)
        {
            this.resetAllAwakeInfo();
        }
        this.lastAwakeHeroid = heroIdByThisid;
        if (!this.mHeroConfigDic.ContainsKey(heroIdByThisid))
        {
            this.resetAllAwakeInfo();
            return;
        }
        HeroAwakeController controller = ControllerManager.Instance.GetController<HeroAwakeController>();
        LuaTable luaTable = this.mHeroConfigDic[heroIdByThisid];
        string[] array = luaTable.GetField_String("evolution").Split(new char[]
        {
            '|'
        });
        List<int> list = new List<int>
        {
            0
        };
        for (int i = 0; i < array.Length; i++)
        {
            int num = -1;
            int.TryParse(array[i], out num);
            if (num > 0)
            {
                bool flag = controller.CheckActived(num);
                if (!this.m_heroAwakeInfos.ContainsKey(num))
                {
                    GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.heroAwakeSample);
                    gameObject.transform.parent = this.heroAwakeSample.transform.parent;
                    gameObject.transform.localScale = Vector3.one;
                    gameObject.SetActive(true);
                    HeroAwakeItem heroAwakeItem = gameObject.AddComponent<HeroAwakeItem>();
                    heroAwakeItem.SetAwakeHeroInfo(this, gameObject, num);
                    this.m_heroAwakeInfos[num] = heroAwakeItem;
                }
                LuaTable configTable = LuaConfigManager.GetConfigTable("evolution_config", (ulong)((long)num));
                int cacheField_Int = configTable.GetCacheField_Int("preid");
                bool increaseCondition = list.Contains(cacheField_Int);
                this.m_heroAwakeInfos[num].RefreshItem(configTable, flag, increaseCondition);
                if (flag)
                {
                    list.Add(num);
                }
            }
        }
    }

    private void resetAllAwakeInfo()
    {
        foreach (KeyValuePair<int, HeroAwakeItem> keyValuePair in this.m_heroAwakeInfos)
        {
            UnityEngine.Object.Destroy(keyValuePair.Value.Root);
        }
        this.m_heroAwakeInfos.Clear();
    }

    public void ReqHeroAwake(uint id)
    {
        HeroAwakeController controller = ControllerManager.Instance.GetController<HeroAwakeController>();
        controller.ReqHeroAwake(id);
    }

    public void SwitchHeroCb(LuaTable msg)
    {
        this.SetupPanel();
    }

    private Transform uiRoot;

    private Button btn_switchhero;

    private GameObject objGene;

    private int BASE_HEIGHT = 516;

    private RectTransform trans_Mask;

    private RawImage obj_img_icon;

    public Dictionary<int, List<DragDropButton>> mCardBtnDic = new Dictionary<int, List<DragDropButton>>();

    private LuaTable ltOnlineHeroData;

    private Dictionary<int, LuaTable> mHeroConfigDic;

    private Dictionary<int, LuaTable> mNpcConfigDic;

    private Dictionary<string, LuaTable> mAttrConfigDic;

    private Dictionary<int, LuaTable> mSkillShowConfigDic;

    private TextTip textTip;

    private uint hair;

    private uint body;

    private uint head;

    private uint avatarid;

    private GameObject fashionSample;

    private GameObject fashionGetTips;

    private Text fashionGetTipsTxt;

    private Button fasshionEquip;

    private int lastHeroid = -1;

    private int lastSelectFashionid = -1;

    private Dictionary<int, FashionItem> m_fashionInfos = new Dictionary<int, FashionItem>();

    private GameObject heroAwakeRoot;

    private GameObject heroAwakeSample;

    private int lastAwakeHeroid = -1;

    private Dictionary<int, HeroAwakeItem> m_heroAwakeInfos = new Dictionary<int, HeroAwakeItem>();

    private string mGeneTabName;

    private Dictionary<string, int> mAttrDic;

    private GameObject mEnterSkillButton;

    private MainPlayerSkillHolder mainPlayerSkillHolder;

    private static BetterDictionary<uint, MainPlayerSkillBase> SkillList = new BetterDictionary<uint, MainPlayerSkillBase>();
}
