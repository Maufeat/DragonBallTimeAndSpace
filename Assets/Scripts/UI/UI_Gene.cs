using System;
using System.Collections.Generic;
using System.Text;
using apprentice;
using Framework.Managers;
using Game.Scene;
using hero;
using LuaInterface;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Gene : UIPanelBase
{
    public GeneController gc
    {
        get
        {
            if (this.gc_ == null)
            {
                this.gc_ = ControllerManager.Instance.GetController<GeneController>();
            }
            return this.gc_;
        }
        set
        {
            this.gc_ = value;
        }
    }

    private ItemTipController itc
    {
        get
        {
            if (this.itc_ == null)
            {
                this.itc_ = ControllerManager.Instance.GetController<ItemTipController>();
            }
            return this.itc_;
        }
    }

    public override void OnInit(Transform root)
    {
        base.OnInit(root);
        this.gc.gddFuseDic.Clear();
        this.InitObj(root);
        this.InitEvent();
    }

    public static bool IsInFuseOrInsert()
    {
        UI_Gene uiobject = UIManager.GetUIObject<UI_Gene>();
        return !(uiobject == null) && (uiobject.curGot == GeneOperationType.Fuse || uiobject.curGot == GeneOperationType.Insert);
    }

    private void Update()
    {
        if (EventSystem.current != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
        }
    }

    private void InitObj(Transform root)
    {
        this.ui_root = root;
        this.panelRoot = root.FindChild("Offset_Gene/Panel_root");
        this.panelRootDetailInSwapPage = root.FindChild("Offset_Gene/Panel_root/Panel_attribute_detail");
        this.panelRootDetailInInsertPage = root.FindChild("Offset_Gene/Panel_root/Panel_att_left");
        this.panelRootFuse = root.FindChild("Offset_Gene/Panel_root/Panel_Gene_fuse");
        this.panelRootSeal = root.FindChild("Offset_Gene/Panel_root/Panel_Gene_seal");
        this.panelRootBag = root.FindChild("Offset_Gene/Panel_root/Panel_Gene_bag");
        this.panelRootInsert = root.FindChild("Offset_Gene/Panel_root/Panel_insert_into");
        this.btnClose = root.FindChild("Offset_Gene/Panel_root/Panel_Gene_close/btn_close");
        this.btnEdtePageName = root.FindChild("Offset_Gene/Panel_root/Dropdown/Btn_edite_tapname");
        this.dropDownList = root.FindChild("Offset_Gene/Panel_root/Dropdown");
        this.inputEditeName = root.FindChild("Offset_Gene/Panel_root/Dropdown/InputField");
        this.atPageRectRoot = root.FindChild("Offset_Gene/Panel_root/Panel_insert_into/Panel_dna_atk");
        this.dfPageRectRoot = root.FindChild("Offset_Gene/Panel_root/Panel_insert_into/Panel_dna_def");
        this.btnFuse = this.panelRootFuse.FindChild("Button_fuse");
        this.btnCancelFuse = this.panelRootFuse.FindChild("Button_cancel");
        this.btnDetailAttribute = root.FindChild("Offset_Gene/Panel_root/btn_att");
        this.btnUnloadAll = root.FindChild("Offset_Gene/Panel_root/Panel_insert_into/Btn_all_remove");
        this.inputfield_num = this.panelRootFuse.FindChild("Text_count/input_number").GetComponent<InputField>();
        this.inputfield_num.onValueChanged.AddListener(new UnityAction<string>(this.inputfield_num_onvaluechanged));
        this.txtTitle = root.FindChild("Offset_Gene/Panel_root/Panel_title/Text_title");
        this.panelTips = root.FindChild("Offset_Gene/Panel_root/Panel_tip");
        this.imgTitleInsert = root.FindChild("Offset_Gene/Panel_root/Panel_title/img_title_insert");
        this.attRootAtt = root.FindChild("Offset_Gene/Panel_root/Panel_attribute_detail/Panel_atkdetail");
        this.attRootDef = root.FindChild("Offset_Gene/Panel_root/Panel_attribute_detail/Panel_defdetail");
        this.btnEnablePage = root.FindChild("Offset_Gene/Panel_root/Panel_attribute_detail/btn_enable");
        this.textInUseText = root.FindChild("Offset_Gene/Panel_root/Panel_attribute_detail/Text_inuse");
        this.togleGeneGroupOrLib = root.FindChild("Offset_Gene/Panel_root/ToggleGroup");
        this.togleGeneGroup = this.togleGeneGroupOrLib.FindChild("Panel_tab/DNAplan");
        this.togleGeneLib = this.togleGeneGroupOrLib.FindChild("Panel_tab/GeneLibrary");
        this.togleShowGeneNotInPage = root.FindChild("Offset_Gene/Panel_root/Panel_Gene_bag/Toggle");
        this.fusePreview = root.FindChild("Offset_Gene/Panel_root/Panel_Gene_fuse/fuse_preview");
        this.wayFind = root.FindChild("Offset_Gene/Panel_root/txt_way_find/txt_npc");
        this.imageHelp = root.FindChild("Offset_Gene/Panel_root/Panel_title/Image_help");
        this.dropDownList.FindChild("Label").GetComponent<Text>().text = string.Empty;
        this.btnRuleTips = root.FindChild("Offset_Gene/Panel_root/Panel_Gene_bag/img_bg/btn_rule");
        this.ruleTipPanel = root.FindChild("Offset_Gene/Panel_root/Panel_rule");
        this.fuseEffectObjs.Add(root.FindChild("Offset_Gene/Panel_root/Panel_Gene_fuse/ui_hecheng01"));
        this.fuseEffectObjs.Add(root.FindChild("Offset_Gene/Panel_root/Panel_Gene_fuse/ui_hecheng01_3"));
        this.fuseEffectObjs.Add(root.FindChild("Offset_Gene/Panel_root/Panel_Gene_fuse/ui_hecheng01_2"));
        this.fuseEffectObjs.Add(root.FindChild("Offset_Gene/Panel_root/Panel_Gene_fuse/ui_hecheng01_1"));
        this.fuseEffectObjs.Add(root.FindChild("Offset_Gene/Panel_root/Panel_Gene_fuse/ui_hecheng02"));
        this.wayFind.parent.gameObject.SetActive(false);
    }

    private void InitEvent()
    {
        Button component = this.btnClose.GetComponent<Button>();
        component.onClick.RemoveAllListeners();
        component.onClick.AddListener(delegate ()
        {
            UIManager.Instance.DeleteUI<UI_Gene>();
        });
        Button component2 = this.btnEdtePageName.GetComponent<Button>();
        component2.onClick.RemoveAllListeners();
        component2.onClick.AddListener(new UnityAction(this.OnEditePageNameClick));
        Dropdown component3 = this.dropDownList.GetComponent<Dropdown>();
        component3.onValueChanged.RemoveAllListeners();
        component3.onValueChanged.AddListener(new UnityAction<int>(this.OnSelectPage));
        Button component4 = this.btnFuse.GetComponent<Button>();
        component4.onClick.RemoveAllListeners();
        component4.onClick.AddListener(new UnityAction(this.Fuse));
        this.SetFuseButtonState(false);
        Button component5 = this.btnDetailAttribute.GetComponent<Button>();
        component5.onClick.RemoveAllListeners();
        component5.onClick.AddListener(new UnityAction(this.DetailUIClick));
        Button component6 = this.btnCancelFuse.GetComponent<Button>();
        component6.onClick.RemoveAllListeners();
        component6.onClick.AddListener(new UnityAction(this.FuseCancel));
        Button component7 = this.btnUnloadAll.GetComponent<Button>();
        component7.onClick.RemoveAllListeners();
        component7.onClick.AddListener(new UnityAction(this.UnloadAllGeneFromPage));
        Toggle component8 = this.togleGeneGroup.GetComponent<Toggle>();
        component8.onValueChanged.RemoveAllListeners();
        component8.onValueChanged.AddListener(new UnityAction<bool>(this.OnGeneGroupSelect));
        Toggle component9 = this.togleGeneLib.GetComponent<Toggle>();
        component9.onValueChanged.RemoveAllListeners();
        component9.onValueChanged.AddListener(new UnityAction<bool>(this.OnGeneLibSelect));
        Toggle component10 = this.togleShowGeneNotInPage.GetComponent<Toggle>();
        component10.onValueChanged.RemoveAllListeners();
        component10.onValueChanged.AddListener(new UnityAction<bool>(this.OnTogleShowGeneNotInPage));
        Button component11 = this.btnEnablePage.GetComponent<Button>();
        component11.onClick.RemoveAllListeners();
        component11.onClick.AddListener(new UnityAction(this.SwapPage));
        this.gc.RegDragCallBack();
        UIEventListener.Get(component3.gameObject).onClick = delegate (PointerEventData pd)
        {
            this.InitDropDownEditeEvent();
        };
        Button component12 = this.btnRuleTips.GetComponent<Button>();
        component12.onClick.RemoveAllListeners();
        component12.onClick.AddListener(delegate ()
        {
            this.RuleTipSwitch(!this.ruleState);
        });
        Button component13 = this.ruleTipPanel.FindChild("Panel_title/btn_close").GetComponent<Button>();
        component13.onClick.RemoveAllListeners();
        component13.onClick.AddListener(delegate ()
        {
            this.RuleTipSwitch(false);
        });
        Button button = this.wayFind.GetComponent<Button>();
        if (!button)
        {
            button = this.wayFind.gameObject.AddComponent<Button>();
        }
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(delegate ()
        {
            this.NavigateToStrengthenDoctorNpc();
        });
        Button component14 = this.imageHelp.GetComponent<Button>();
        component14.onClick.RemoveAllListeners();
        component14.onClick.AddListener(new UnityAction(this.Help));
    }

    private void Help()
    {
        GuideController controller = ControllerManager.Instance.GetController<GuideController>();
        switch (this.curGot)
        {
            case GeneOperationType.SwapPage:
                controller.ViewGuideUI(18U, true);
                break;
            case GeneOperationType.Fuse:
                controller.ViewGuideUI(19U, true);
                break;
            case GeneOperationType.Insert:
                controller.ViewGuideUI(17U, true);
                break;
        }
    }

    private void NavigateToStrengthenDoctorNpc()
    {
        GameScene manager = ManagerCenter.Instance.GetManager<GameScene>();
        if (manager.sceneInfo.mapid == 708U)
        {
            GlobalRegister.PathFindWithPathWayId(900048U);
        }
        else
        {
            GlobalRegister.PathFindWithPathWayId(900000U);
        }
        UIManager.Instance.DeleteUI<UI_Gene>();
    }

    private void SwapPage()
    {
        if (MainPlayer.Self != null && MainPlayer.Self.IsBattleState)
        {
            TipsWindow.ShowNotice(4007U);
            return;
        }
        DNAPage dnapage = this.dropDownList.GetComponent<Dropdown>().value + DNAPage.PAGE1;
        if (dnapage != this.gc.curPageInUse)
        {
            this.gc.SwapPage(dnapage);
        }
    }

    private void RuleTipSwitch(bool state)
    {
        this.ruleTipPanel.gameObject.SetActive(state);
        this.ruleState = state;
        if (state)
        {
            TextModelController controller = ControllerManager.Instance.GetController<TextModelController>();
            Text component = this.ruleTipPanel.FindChild("Panel_title/Text").GetComponent<Text>();
            Text component2 = this.ruleTipPanel.FindChild("Scroll View/Viewport/Content/Text").GetComponent<Text>();
            switch (this.curGot)
            {
                case GeneOperationType.Fuse:
                    controller.SetTextModel(component, string.Empty, 1);
                    controller.SetTextModel(component2, string.Empty, 1);
                    break;
                case GeneOperationType.Insert:
                    controller.SetTextModel(component, string.Empty, 0);
                    controller.SetTextModel(component2, string.Empty, 0);
                    break;
            }
            component2.text = component2.text.Replace("\\n", "\n");
        }
    }

    private void InitDropDownEditeEvent()
    {
        Transform transform = this.dropDownList.transform.FindChild("Dropdown List/Viewport/Content");
        for (int i = 1; i < transform.childCount; i++)
        {
            int index = i;
            Transform child = transform.GetChild(index);
            Button component = child.FindChild("Button").GetComponent<Button>();
            InputField intpuField = child.FindChild("InputField").GetComponent<InputField>();
            Text itemLabel = child.FindChild("Item Label").GetComponent<Text>();
            component.onClick.RemoveAllListeners();
            component.onClick.AddListener(delegate ()
            {
                intpuField.gameObject.SetActive(!intpuField.gameObject.activeInHierarchy);
                intpuField.onEndEdit.RemoveAllListeners();
                intpuField.ActivateInputField();
                intpuField.onEndEdit.AddListener(delegate (string str)
                {
                    string text = intpuField.text;
                    if (string.Compare(text, KeyWordFilter.ChatFilter(text)) != 0)
                    {
                        TipsWindow.ShowWindow(TipsType.HAVE_SENSITIVE, null);
                        return;
                    }
                    if (text.Contains("|"))
                    {
                        TipsWindow.ShowNotice(2905U);
                        return;
                    }
                    if (string.IsNullOrEmpty(text))
                    {
                        TipsWindow.ShowNotice(2905U);
                        return;
                    }
                    int num = Encoding.UTF8.GetBytes(text).Length;
                    if (num > 15)
                    {
                        TipsWindow.ShowNotice(2906U);
                        return;
                    }
                    this.OnEndEditeName(text, index - 1);
                    intpuField.gameObject.SetActive(false);
                    itemLabel.text = text;
                });
            });
        }
    }

    internal void OnCombineDna(MSG_ReqCombineDnaInBag_CS msg)
    {
        TipsWindow.ShowNotice(3004U);
        this.FrashCurrentGoldText();
    }

    internal void OnCombineDna(MSG_ReqCombineDna_CS msg)
    {
        TipsWindow.ShowNotice(3004U);
        this.FrashCurrentGoldText();
    }

    private void FrashCurrentGoldText()
    {
        Text component = this.panelRootFuse.FindChild("img_current/Text_cost_value").GetComponent<Text>();
        component.text = GlobalRegister.GetCurrencyByID(3U) + string.Empty;
    }

    internal void OnPageChange(DNAPage page)
    {
        this.SetEnablePageBtnState(false);
    }

    private void OnClickEditePageName(int index)
    {
    }

    private void OnGeneGroupSelect(bool b)
    {
        this.panelRootBag.gameObject.SetActive(!b);
        this.panelRootDetailInSwapPage.gameObject.SetActive(b);
        this.panelRootInsert.gameObject.SetActive(b);
        this.dropDownList.gameObject.SetActive(b);
    }

    private void OnGeneLibSelect(bool b)
    {
        this.panelRootBag.gameObject.SetActive(b);
        this.panelRootDetailInSwapPage.gameObject.SetActive(!b);
        this.panelRootInsert.gameObject.SetActive(!b);
        this.dropDownList.gameObject.SetActive(!b);
        this.SetBagUISize();
        Toggle component = this.togleShowGeneNotInPage.GetComponent<Toggle>();
        this.OnTogleShowGeneNotInPage(component.isOn);
    }

    internal void OnGetDnaBagInfo(MSG_DnaBagInfo_CSC msg)
    {
        this.SetEnablePageBtnState(false);
    }

    private void SetEnablePageBtnState(bool state)
    {
        this.textInUseText.gameObject.SetActive(!state);
        this.btnEnablePage.gameObject.SetActive(state);
        Button component = this.btnEnablePage.GetComponent<Button>();
        component.interactable = state;
    }

    private void OnTogleShowGeneNotInPage(bool b)
    {
        if (b)
        {
            this.RefrashNotInPageBageUI();
        }
        else
        {
            this.RefrashBaseGeneBagUI();
        }
    }

    public void SetGot(int index)
    {
        this.gc.isFirstOpen = true;
        this.gc.isFusedGeneInPageBefor = false;
        this.curGot = (GeneOperationType)index;
        this.panelRootFuse.gameObject.SetActive(false);
        this.panelRootSeal.gameObject.SetActive(false);
        this.panelRootBag.gameObject.SetActive(true);
        this.btnUnloadAll.gameObject.SetActive(false);
        GuideController controller = ControllerManager.Instance.GetController<GuideController>();
        switch (this.curGot)
        {
            case GeneOperationType.SwapPage:
                this.panelRootBag.transform.FindChild("img_bg").gameObject.SetActive(false);
                this.panelRootBag.gameObject.SetActive(false);
                this.btnEnablePage.gameObject.SetActive(true);
                this.SetSwapPageUI();
                this.SetTitle(0);
                break;
            case GeneOperationType.Fuse:
                {
                    this.panelRootBag.gameObject.SetActive(true);
                    this.panelRootFuse.gameObject.SetActive(true);
                    this.btnUnloadAll.gameObject.SetActive(false);
                    this.togleGeneGroupOrLib.gameObject.SetActive(false);
                    this.panelRootDetailInSwapPage.gameObject.SetActive(false);
                    this.togleShowGeneNotInPage.gameObject.SetActive(false);
                    this.panelRootBag.transform.FindChild("img_bg").gameObject.SetActive(true);
                    Text component = this.panelRootFuse.FindChild("Text_count").GetComponent<Text>();
                    Text component2 = this.panelRootFuse.FindChild("img_cost/Text_cost_value").GetComponent<Text>();
                    component.gameObject.SetActive(false);
                    component2.text = "0";
                    this.FrashCurrentGoldText();
                    this.SetFusePageUI();
                    this.SetTitle(2);
                    controller.CheckIsNeedGuideByID(19U);
                    break;
                }
            case GeneOperationType.Seal:
                this.panelRootSeal.gameObject.SetActive(true);
                break;
            case GeneOperationType.Insert:
                this.panelRootBag.transform.FindChild("img_bg").gameObject.SetActive(true);
                this.panelRootBag.gameObject.SetActive(true);
                this.btnUnloadAll.gameObject.SetActive(true);
                this.togleGeneGroupOrLib.gameObject.SetActive(false);
                this.panelRootDetailInSwapPage.gameObject.SetActive(false);
                this.togleShowGeneNotInPage.gameObject.SetActive(false);
                this.btnDetailAttribute.gameObject.SetActive(true);
                this.SetInsertPageUI();
                this.SetTitle(1);
                controller.CheckIsNeedGuideByID(17U);
                break;
        }
        this.gc.RegOnDnaPageNameChange(null);
        ServerStorageManager.Instance.GetData(ServerStorageKey.GenePageName, 0U);
        this.gc.ReqGeneBagData();
        if (index == 3 || index == 1)
        {
            this.wayFind.parent.gameObject.SetActive(false);
            base.RegOpenUIByNpc(string.Empty);
        }
        else
        {
            this.wayFind.parent.gameObject.SetActive(true);
            base.UnRegOpenUIByNpc(string.Empty);
        }
    }

    private void SetTitle(int index)
    {
        TextModelController controller = ControllerManager.Instance.GetController<TextModelController>();
        controller.SetTextModel(this.txtTitle.GetComponent<Text>(), string.Empty, index);
    }

    private void SetSwapPageUI()
    {
        this.dropDownList.GetComponent<RectTransform>().anchoredPosition = new Vector2(551f, -168f);
        RectTransform component = this.panelRootInsert.GetComponent<RectTransform>();
        float num = component.sizeDelta.x + Mathf.Abs(component.anchoredPosition.x);
        RectTransform component2 = this.panelRootDetailInSwapPage.GetComponent<RectTransform>();
        num += Mathf.Abs(component2.anchoredPosition.x) + component2.sizeDelta.x;
        this.panelRootInsert.gameObject.SetActive(true);
        this.panelRootDetailInSwapPage.gameObject.SetActive(true);
    }

    private void SetBagUISize()
    {
    }

    private void SetInsertPageUI()
    {
        this.dropDownList.GetComponent<RectTransform>().anchoredPosition = new Vector2(181f, -106f);
        RectTransform component = this.panelRootInsert.GetComponent<RectTransform>();
        float num = component.sizeDelta.x;
        RectTransform component2 = this.panelRootBag.GetComponent<RectTransform>();
        num += component2.sizeDelta.x + Mathf.Abs(component2.anchoredPosition.x);
        RectTransform component3 = this.panelRoot.GetComponent<RectTransform>();
        component3.sizeDelta = new Vector2(num, component3.sizeDelta.y);
    }

    private void SetFusePageUI()
    {
        this.dropDownList.GetComponent<RectTransform>().anchoredPosition = new Vector2(181f, -106f);
        RectTransform component = this.panelRootInsert.GetComponent<RectTransform>();
        float num = component.sizeDelta.x + Mathf.Abs(component.anchoredPosition.x);
        RectTransform component2 = this.panelRootBag.GetComponent<RectTransform>();
        num += component2.sizeDelta.x + Mathf.Abs(component2.anchoredPosition.x);
        RectTransform component3 = this.panelRoot.GetComponent<RectTransform>();
        component3.sizeDelta = new Vector2(num, component3.sizeDelta.y);
    }

    public void ReqPageNameBack(MSG_Req_OperateClientDatas_CS msg)
    {
        if (!string.IsNullOrEmpty(msg.value) && msg.value.Contains("|"))
        {
            this.OnPageNameChange(msg.value);
        }
    }

    private void OnEditePageNameClick()
    {
        this.dropDownList.FindChild("img_name").gameObject.SetActive(false);
        this.dropDownList.FindChild("Label").gameObject.SetActive(false);
        this.dropDownList.FindChild("Arrow").gameObject.SetActive(false);
        this.inputEditeName.gameObject.SetActive(true);
        Dropdown component = this.dropDownList.GetComponent<Dropdown>();
        int curIndex = component.value;
        string text = component.options[curIndex].text;
        InputField component2 = this.inputEditeName.GetComponent<InputField>();
        component2.ActivateInputField();
        component2.text = text;
        component2.onEndEdit.RemoveAllListeners();
        component2.onEndEdit.AddListener(delegate (string s)
        {
            if (string.Compare(s, KeyWordFilter.ChatFilter(s)) != 0)
            {
                TipsWindow.ShowWindow(TipsType.HAVE_SENSITIVE, null);
                return;
            }
            if (s.Contains("|"))
            {
                TipsWindow.ShowNotice(2905U);
                return;
            }
            if (string.IsNullOrEmpty(s))
            {
                TipsWindow.ShowNotice(2905U);
                return;
            }
            int num = Encoding.UTF8.GetBytes(s).Length;
            if (num > 15)
            {
                TipsWindow.ShowNotice(2906U);
                return;
            }
            if (string.Compare(s, KeyWordFilter.ChatFilter(s)) != 0)
            {
                TipsWindow.ShowWindow(TipsType.HAVE_SENSITIVE, null);
                return;
            }
            this.dropDownList.transform.FindChild("Label").GetComponent<Text>().text = s;
            this.dropDownList.gameObject.SetActive(true);
            this.inputEditeName.gameObject.SetActive(false);
            this.dropDownList.FindChild("img_name").gameObject.SetActive(true);
            this.dropDownList.FindChild("Label").gameObject.SetActive(true);
            this.dropDownList.FindChild("Arrow").gameObject.SetActive(true);
            this.OnEndEditeName(s, curIndex);
        });
    }

    public void SetCurDnaPageToDropdwonUI(DNAPage page)
    {
        Dropdown component = this.dropDownList.GetComponent<Dropdown>();
        component.value = page - DNAPage.PAGE1;
        component.RefreshShownValue();
    }

    public DNAPage GetDnaPageInDropDown()
    {
        Dropdown component = this.dropDownList.GetComponent<Dropdown>();
        return component.value + DNAPage.PAGE1;
    }

    private void OnEndEditeName(string newName, int editeIndex)
    {
        string text = string.Empty;
        Dropdown component = this.dropDownList.GetComponent<Dropdown>();
        for (int i = 0; i < component.options.Count; i++)
        {
            if (editeIndex == i)
            {
                text += newName;
            }
            else
            {
                text += component.options[i].text;
            }
            if (i != component.options.Count - 1)
            {
                text += "|";
            }
        }
        this.SaveNewPageNameToServer(text);
    }

    private void SaveNewPageNameToServer(string s)
    {
        ServerStorageManager.Instance.AddUpdateData(ServerStorageKey.GenePageName, s, 0U);
    }

    public static void ShowTip(LuaTable lt, Transform uiRoot, Transform panelRoot, Transform tipPanel, Transform itemRoot, bool isDnaChip = true)
    {
        tipPanel.gameObject.SetActive(true);
        Image imgIcon = tipPanel.FindChild("img_icon").GetComponent<Image>();
        Text component = tipPanel.FindChild("txt_name").GetComponent<Text>();
        Text component2 = tipPanel.FindChild("Panel_desc/txt_desc").GetComponent<Text>();
        StringBuilder stringBuilder = new StringBuilder();
        string field = (!isDnaChip) ? "icon" : "chipicon";
        component.text = lt.GetCacheField_String((!isDnaChip) ? "name" : "chipname");
        UITextureMgr.Instance.GetTexture(ImageType.ITEM, lt.GetCacheField_String(field), delegate (UITextureAsset tex)
        {
            if (tex.textureObj != null && imgIcon != null)
            {
                Texture2D textureObj = tex.textureObj;
                imgIcon.sprite = Sprite.Create(textureObj, new Rect(0f, 0f, (float)textureObj.width, (float)textureObj.height), new Vector2(0f, 0f));
                imgIcon.material = null;
            }
        });
        if (isDnaChip)
        {
            stringBuilder.Append("类型:");
            stringBuilder.Append((lt.GetCacheField_Uint("type") != 0U) ? "攻击" : "防御");
            stringBuilder.Append("\n");
            stringBuilder.Append("等级:");
            stringBuilder.Append(lt.GetCacheField_Uint("level"));
            stringBuilder.Append("\n");
            stringBuilder.Append("说明:" + lt.GetCacheField_String("chiptips"));
        }
        else
        {
            stringBuilder.Append(lt.GetCacheField_String("desc"));
        }
        component2.text = stringBuilder.ToString().Replace("\\n", "\n");
        UI_Gene.SetTipPanelPos(uiRoot, itemRoot, panelRoot, tipPanel);
    }

    public static void SetTipPanelPos(Transform uiRoot, Transform itemRoot, Transform panelRoot, Transform tipPanel)
    {
        Vector3 vector = uiRoot.parent.InverseTransformPoint(itemRoot.transform.position);
        int num = (vector.x >= 0f) ? -1 : 1;
        int num2 = (vector.y >= 0f) ? -1 : 1;
        RectTransform component = itemRoot.GetComponent<RectTransform>();
        RectTransform component2 = tipPanel.GetComponent<RectTransform>();
        Vector3 b = new Vector3((float)num * component2.sizeDelta.x / 2f, (float)num2 * component2.sizeDelta.y / 2f, 0f);
        Vector3 a = new Vector3((float)num * component.sizeDelta.x / 2f, (float)num2 * component.sizeDelta.y / 2f, 0f);
        Vector3 vector2 = panelRoot.InverseTransformPoint(itemRoot.position);
        vector2 += a + b;
        vector2.z = tipPanel.localPosition.z;
        tipPanel.localPosition = vector2;
    }

    private void OnPageNameChange(string newNames)
    {
        if (string.IsNullOrEmpty(newNames))
        {
            return;
        }
        string[] array = newNames.Split(new char[]
        {
            '|'
        });
        if (array.Length >= 4)
        {
            Dropdown component = this.dropDownList.GetComponent<Dropdown>();
            for (int i = 0; i < component.options.Count; i++)
            {
                if (i < array.Length)
                {
                    component.options[i].text = array[i];
                }
            }
            component.RefreshShownValue();
        }
    }

    private void OnSelectPage(int index)
    {
        if (GeneController.notCallSelectEvent)
        {
            GeneController.notCallSelectEvent = false;
            return;
        }
        DNAPage dnapage = index + DNAPage.PAGE1;
        if (this.curGot == GeneOperationType.SwapPage)
        {
            this.SetEnablePageBtnState(dnapage != this.gc.curPageInUse);
        }
        this.gc.mNetWork.ReqGeneDnaPageInfo(dnapage);
    }

    public void RefrashBaseGeneBagUI()
    {
        List<DnaItem> listItem = this.gc.InitBaseItem();
        this.RefrashGeneBagUI(listItem);
    }

    public void RefrashNotInPageBageUI()
    {
        List<DnaItem> listItem = this.gc.FilterBagDnaList(this.gc.GetCurDnaPageType(), true);
        this.RefrashGeneBagUI(listItem);
    }

    public void RefrashGeneBagUI(List<DnaItem> listItem)
    {
        listItem.Sort((DnaItem a, DnaItem b) => (a.id + a.level).CompareTo(b.id + b.level));
        Transform transform = this.panelRootBag.FindChild("ScrollbarRect/Viewport/Rect");
        GameObject gameObject = transform.GetChild(0).gameObject;
        ScrollRect sr = transform.parent.parent.GetComponent<ScrollRect>();
        int num = Mathf.Max(transform.childCount, listItem.Count);
        GridLayoutGroup component = transform.GetComponent<GridLayoutGroup>();
        if (num % component.constraintCount != 0)
        {
            num = (num / component.constraintCount + 1) * component.constraintCount;
        }
        for (int i = 0; i < num; i++)
        {
            GameObject gameObject2;
            if (i < transform.childCount)
            {
                gameObject2 = transform.GetChild(i).gameObject;
            }
            else
            {
                gameObject2 = UnityEngine.Object.Instantiate<GameObject>(gameObject);
                gameObject2.transform.SetParent(transform, false);
                gameObject2.transform.localScale = gameObject.transform.localScale;
            }
            this.ClearEvent(gameObject2);
            if (i < listItem.Count)
            {
                this.SetDnaBagItem(gameObject2, i, listItem[i]);
            }
            else
            {
                this.SetDnaBagItem(gameObject2, 0, null);
            }
        }
        RectTransform rectTr = transform.GetComponent<RectTransform>();
        RectTransform rectRoot = this.panelRootBag.FindChild("ScrollbarRect").GetComponent<RectTransform>();
        Scheduler.Instance.AddFrame(2U, false, delegate
        {
            sr.horizontal = false;
            if (rectTr.sizeDelta.y > rectRoot.sizeDelta.y + 5f)
            {
                sr.vertical = true;
            }
            else
            {
                sr.vertical = false;
            }
        });
    }

    private void SetDnaBagItem(GameObject itemObj, int index = 0, DnaItem dnaItem = null)
    {
        Text component = itemObj.transform.FindChild("Text_count").GetComponent<Text>();
        Image component2 = itemObj.transform.FindChild("icon").GetComponent<Image>();
        DragDropButton dragDropButton = itemObj.GetComponent<DragDropButton>();
        component2.color = Color.white;
        if (dragDropButton == null)
        {
            dragDropButton = itemObj.AddComponent<DragDropButton>();
        }
        if (dnaItem != null)
        {
            itemObj.gameObject.SetActive(true);
            component.gameObject.SetActive(true);
            component2.gameObject.SetActive(true);
            component.text = dnaItem.num + string.Empty;
            component2.color = ((dnaItem.num <= 0U) ? Color.grey : Color.white);
            this.SetGeneIcon(component2, dnaItem.id, dnaItem.level);
            GeneDragDropData geneDragDropData = new GeneDragDropData(dnaItem, SourceFrom.Bag, DNASlotType.ATT);
            geneDragDropData.ReInitDst();
            dragDropButton.Initilize(UIRootType.Gene, new Vector2((float)index, 0f), "icon", geneDragDropData);
            this.AddFuseClickEvent(itemObj, dragDropButton);
        }
        else
        {
            component.gameObject.SetActive(false);
            component2.gameObject.SetActive(false);
            GeneDragDropData data = new GeneDragDropData(null, SourceFrom.Bag, DNASlotType.ATT);
            dragDropButton.Initilize(UIRootType.Gene, new Vector2((float)index, 0f), "icon", data);
        }
    }

    private void AddExpendClickEvent(GameObject itemObj, DragDropButton ddb)
    {
        if (this.curGot == GeneOperationType.Insert)
        {
            Button button = itemObj.GetComponent<Button>();
            if (button == null)
            {
                button = itemObj.AddComponent<Button>();
            }
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(delegate ()
            {
                this.gc.RightClickItem(ddb);
            });
        }
    }

    private void AddFuseClickEvent(GameObject itemObj, DragDropButton ddb)
    {
        if (this.curGot == GeneOperationType.Fuse)
        {
            Button button = itemObj.GetComponent<Button>();
            if (button == null)
            {
                button = itemObj.AddComponent<Button>();
            }
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(delegate ()
            {
                this.gc.RightClickItem(ddb);
            });
        }
    }

    public void RefrashDnaPageUI(MSG_DnaPageInfo_CSC dpi)
    {
        List<DnaItem> collection = this.InitOneDnaPage(this.atPageRectRoot, dpi, DNASlotType.ATT);
        List<DnaItem> collection2 = this.InitOneDnaPage(this.dfPageRectRoot, dpi, DNASlotType.DEF);
        List<DnaItem> list = new List<DnaItem>();
        list.AddRange(collection);
        list.AddRange(collection2);
        Dictionary<string, int> attributeDataByDnaList = this.GetAttributeDataByDnaList(list, 1U);
        Dictionary<string, int> attributeDataByDnaList2 = this.GetAttributeDataByDnaList(list, 0U);
        this.RefrashGeneAddAttributeUI(this.attRootAtt, attributeDataByDnaList);
        this.RefrashGeneAddAttributeUI(this.attRootDef, attributeDataByDnaList2);
        Transform listRoot = this.panelRootDetailInInsertPage.FindChild("Panel_attribute_detail/Panel_atkdetail");
        Transform listRoot2 = this.panelRootDetailInInsertPage.FindChild("Panel_attribute_detail/Panel_defdetail");
        this.RefrashGeneAddAttributeUI(listRoot, attributeDataByDnaList);
        this.RefrashGeneAddAttributeUI(listRoot2, attributeDataByDnaList2);
    }

    public Dictionary<string, int> GetAttributeDataByDnaList(List<DnaItem> dil, uint type)
    {
        Dictionary<string, int> dictionary = new Dictionary<string, int>();
        for (int i = 0; i < dil.Count; i++)
        {
            uint num = dil[i].id + dil[i].level;
            LuaTable configTable = LuaConfigManager.GetConfigTable("dnachip_config", (ulong)num);
            if (configTable != null)
            {
                string cacheField_String = configTable.GetCacheField_String("property");
                string[] array = cacheField_String.Split(new char[]
                {
                    '|'
                });
                if (array != null && array.Length > 0)
                {
                    for (int j = 0; j < array.Length; j++)
                    {
                        string[] array2 = array[j].Split(new char[]
                        {
                            ','
                        });
                        if (array2.Length > 1)
                        {
                            if (dictionary.ContainsKey(array2[0]))
                            {
                                Dictionary<string, int> dictionary3;
                                Dictionary<string, int> dictionary2 = dictionary3 = dictionary;
                                string key2;
                                string key = key2 = array2[0];
                                int num2 = dictionary3[key2];
                                dictionary2[key] = num2 + int.Parse(array2[1]);
                            }
                            else
                            {
                                dictionary[array2[0]] = int.Parse(array2[1]);
                            }
                        }
                    }
                }
            }
        }
        List<string> list = new List<string>(dictionary.Keys);
        List<AttDatas> list2 = new List<AttDatas>();
        for (int k = 0; k < list.Count; k++)
        {
            AttDatas attAttDatasByConfigKey = this.gc.GetAttAttDatasByConfigKey(list[k]);
            if (attAttDatasByConfigKey != null && attAttDatasByConfigKey.type == type)
            {
                list2.Add(attAttDatasByConfigKey);
            }
        }
        list2.Sort((AttDatas a, AttDatas b) => a.id.CompareTo(b.id));
        Dictionary<string, int> dictionary4 = new Dictionary<string, int>();
        for (int l = 0; l < list2.Count; l++)
        {
            dictionary4.Add(list2[l].attName, dictionary[list2[l].attName]);
        }
        return dictionary4;
    }

    private List<DnaItem> InitOneDnaPage(Transform rectTrRoot, MSG_DnaPageInfo_CSC dpi, DNASlotType dst)
    {
        List<Hole> holeList = (dst != DNASlotType.ATT) ? dpi.def_holes : dpi.att_holes;
        GameObject gameObject = rectTrRoot.GetChild(0).gameObject;
        Dictionary<uint, Hole> dictionary = this.HoleListToHoleDic(holeList);
        List<DnaItem> list = new List<DnaItem>();
        uint num = (dst != DNASlotType.ATT) ? dpi.def_opened_num : dpi.att_opened_num;
        int num2 = 0;
        while ((long)num2 < 9L)
        {
            GameObject gameObject2;
            if (num2 < rectTrRoot.transform.childCount)
            {
                gameObject2 = rectTrRoot.transform.GetChild(num2).gameObject;
            }
            else
            {
                gameObject2 = UnityEngine.Object.Instantiate<GameObject>(gameObject);
                gameObject2.transform.SetParent(rectTrRoot.transform, false);
                gameObject2.transform.localScale = gameObject.transform.localScale;
            }
            this.ClearEvent(gameObject2);
            Hole hole = dictionary[(uint)(num2 + 1)];
            Image icon = gameObject2.transform.FindChild("dna_icon").GetComponent<Image>();
            Image component = gameObject2.transform.FindChild("img_atk_close").GetComponent<Image>();
            icon.color = Color.white;
            DragDropButton dragDropButton = gameObject2.GetComponent<DragDropButton>();
            if (dragDropButton == null)
            {
                dragDropButton = gameObject2.AddComponent<DragDropButton>();
            }
            uint num3 = hole.id + hole.level;
            GeneDragDropData geneDragDropData = new GeneDragDropData(null, SourceFrom.Bag, DNASlotType.ATT);
            geneDragDropData.sf = SourceFrom.Page;
            geneDragDropData.dst = dst;
            geneDragDropData.inPagePos = (uint)(num2 + 1);
            geneDragDropData.isThisSlotOpen = ((long)num2 < (long)((ulong)num));
            Text component2 = gameObject2.transform.FindChild("txt_open_lv").GetComponent<Text>();
            component2.gameObject.SetActive(false);
            component.gameObject.SetActive(false);
            component2.text = string.Empty;
            if (!geneDragDropData.isThisSlotOpen)
            {
                component.gameObject.SetActive(true);
                component2.gameObject.SetActive(true);
                uint num4 = this.gc.SlotOpenLevel(num2 + 1, this.gc.GetCurDnaPageType(), dst, num);
                if (num4 != 0U)
                {
                    component2.text = num4 + "级开启";
                }
                if (this.curGot == GeneOperationType.Insert)
                {
                    this.AddExpendClickEvent(gameObject2, dragDropButton);
                }
            }
            if (hole != null && hole.id != 0U)
            {
                icon.gameObject.SetActive(true);
                this.SetGeneIcon(icon, hole.id, hole.level);
                DnaItem dnaItem = new DnaItem();
                dnaItem.id = hole.id;
                dnaItem.level = hole.level;
                geneDragDropData.curDnaItem = dnaItem;
                dragDropButton.Initilize(UIRootType.Gene, new Vector2((float)(num2 + 1), 0f), "dna_icon", geneDragDropData);
                list.Add(dnaItem);
                bool isThisCardInFusePage = this.gc.IsThisGeneCardInFuse((uint)(num2 + 1), dst);
                this.AddFuseClickEvent(gameObject2, dragDropButton);
                Scheduler.Instance.AddFrame(1U, false, delegate
                {
                    if (isThisCardInFusePage)
                    {
                        icon.color = Color.white / 2f;
                    }
                    else
                    {
                        icon.color = Color.white;
                    }
                });
            }
            else
            {
                icon.gameObject.SetActive(false);
                dragDropButton.Initilize(UIRootType.Gene, new Vector2((float)(num2 + 1), 0f), "dna_icon", geneDragDropData);
            }
            num2++;
        }
        return list;
    }

    public void RefrashGeneAddAttributeUI(Transform listRoot, Dictionary<string, int> attributeItem)
    {
        Transform transform = listRoot.FindChild("ScrollbarRect/Rect");
        GameObject gameObject = transform.GetChild(0).gameObject;
        int num = Mathf.Max(transform.childCount, attributeItem.Count);
        List<string> list = new List<string>(attributeItem.Keys);
        for (int i = 0; i < num; i++)
        {
            GameObject gameObject2;
            if (i < transform.childCount)
            {
                gameObject2 = transform.GetChild(i).gameObject;
            }
            else
            {
                gameObject2 = UnityEngine.Object.Instantiate<GameObject>(gameObject);
                gameObject2.transform.SetParent(transform, false);
                gameObject2.transform.localScale = gameObject.transform.localScale;
            }
            if (i < list.Count)
            {
                gameObject2.gameObject.SetActive(true);
                Text component = gameObject2.transform.FindChild("Text_name").GetComponent<Text>();
                component.text = this.gc.GetAttShowNameByConfigKey(list[i]);
                Text component2 = gameObject2.transform.FindChild("Text_value").GetComponent<Text>();
                component2.text = "+" + attributeItem[list[i]];
            }
            else
            {
                gameObject2.gameObject.SetActive(false);
            }
        }
    }

    private void ClearEvent(GameObject itemObj)
    {
        Button component = itemObj.GetComponent<Button>();
        if (component != null)
        {
            component.onClick.RemoveAllListeners();
        }
        this.panelTips.gameObject.SetActive(false);
        HoverEventListener component2 = itemObj.GetComponent<HoverEventListener>();
        if (component2 != null)
        {
            UnityEngine.Object.DestroyImmediate(component2);
        }
    }

    private void SetGeneIcon(Image imge, uint id, uint level)
    {
        uint num = id + level;
        LuaTable configTable = LuaConfigManager.GetConfigTable("dnachip_config", (ulong)num);
        if (configTable != null)
        {
            string cacheField_String = configTable.GetCacheField_String("chipicon");
            base.GetTexture(ImageType.ITEM, cacheField_String, delegate (Texture2D t)
            {
                if (t != null)
                {
                    imge.overrideSprite = Sprite.Create(t, new Rect(0f, 0f, (float)t.width, (float)t.height), new Vector2(0.5f, 0.5f));
                }
            });
            imge.raycastTarget = false;
            HoverEventListener.Get(imge.transform.parent.gameObject).onEnter = delegate (PointerEventData data)
            {
                if (this.itc != null)
                {
                    this.itc.OpenGenePanel(id, level, imge.transform.parent.gameObject);
                }
            };
            HoverEventListener.Get(imge.transform.parent.gameObject).onExit = delegate (PointerEventData data)
            {
                if (this.itc != null)
                {
                }
            };
        }
    }

    public uint GetFuseNum()
    {
        uint num = uint.Parse(this.inputfield_num.text);
        return num / 3U;
    }

    private void inputfield_num_onvaluechanged(string text)
    {
        int num = int.Parse(text);
        if (num < 0)
        {
            this.inputfield_num.text = "1";
            return;
        }
        if (num > this.maxNum)
        {
            this.inputfield_num.text = this.maxNum.ToString();
            return;
        }
        uint num2 = this.mGddd.curDnaItem.id + this.mGddd.curDnaItem.level + 1U;
        LuaTable configTable = LuaConfigManager.GetConfigTable("dnachip_config", (ulong)num2);
        Text component = this.panelRootFuse.FindChild("img_cost/Text_cost_value").GetComponent<Text>();
        component.text = ((long)((ulong)configTable.GetCacheField_Uint("goldcost") * (ulong)((long)(num / 3)))).ToString();
    }

    public void RefrashFuseUI(GeneDragDropData gddd)
    {
        this.mGddd = gddd;
        int bagCardMinCountExceptInPage = this.gc.GetBagCardMinCountExceptInPage(gddd.curDnaItem.id, gddd.curDnaItem.level);
        Text component = this.panelRootFuse.FindChild("Text_count").GetComponent<Text>();
        this.panelRootFuse.FindChild("Button_item").gameObject.SetActive(true);
        Image component2 = this.panelRootFuse.FindChild("Button_item/icon").GetComponent<Image>();
        component2.enabled = true;
        component2.gameObject.SetActive(true);
        component2.color = Color.white;
        this.SetGeneIcon(component2, gddd.curDnaItem.id, gddd.curDnaItem.level);
        Image component3 = this.fusePreview.FindChild("icon").GetComponent<Image>();
        component3.sprite = null;
        uint geneMaxLevel = this.gc.GetGeneMaxLevel();
        Text component4 = this.panelRootFuse.FindChild("img_cost/Text_cost_value").GetComponent<Text>();
        component4.text = "0";
        if (gddd.curDnaItem.level + 1U <= geneMaxLevel)
        {
            uint num = gddd.curDnaItem.id + gddd.curDnaItem.level + 1U;
            LuaTable configTable = LuaConfigManager.GetConfigTable("dnachip_config", (ulong)num);
            int num2;
            if (gddd.sf == SourceFrom.Bag)
            {
                num2 = bagCardMinCountExceptInPage / 3;
            }
            else
            {
                num2 = (bagCardMinCountExceptInPage + 1) / 3;
            }
            component4.text = ((long)((ulong)configTable.GetCacheField_Uint("goldcost") * (ulong)((long)num2))).ToString();
            component3.color = Color.gray;
            component3.gameObject.SetActive(true);
            this.SetGeneIcon(component3, gddd.curDnaItem.id, gddd.curDnaItem.level + 1U);
        }
        if (gddd.sf == SourceFrom.Bag)
        {
            this.maxNum = bagCardMinCountExceptInPage;
            this.inputfield_num.text = bagCardMinCountExceptInPage.ToString();
            component.text = "/3";
            this.SetFuseButtonState(bagCardMinCountExceptInPage >= 3);
        }
        else
        {
            int num3 = bagCardMinCountExceptInPage + 1;
            this.maxNum = num3;
            this.inputfield_num.text = num3.ToString();
            component.text = "/3";
            this.SetFuseButtonState(num3 >= 3);
        }
        component.gameObject.SetActive(true);
        this.inputfield_num.ActivateInputField();
        UIEventListener.Get(component2.transform.parent.gameObject).onClick = delegate (PointerEventData pd)
        {
            if (pd.pointerId == -2)
            {
                this.FuseCancel(false);
            }
        };
    }

    public void FuseCancel(bool previewIconState = false)
    {
        Image component = this.panelRootFuse.FindChild("Button_item/icon").GetComponent<Image>();
        component.enabled = false;
        this.SetFuseButtonState(false);
        Text component2 = this.panelRootFuse.FindChild("Text_count").GetComponent<Text>();
        component2.gameObject.SetActive(false);
        Text component3 = this.panelRootFuse.FindChild("img_cost/Text_cost_value").GetComponent<Text>();
        component3.text = "0";
        this.fusePreview.FindChild("icon").gameObject.SetActive(previewIconState);
    }

    public void UnloadAllGeneFromPage()
    {
        this.UnloadOneDnaPage(this.atPageRectRoot);
        this.UnloadOneDnaPage(this.dfPageRectRoot);
    }

    private void UnloadOneDnaPage(Transform pageRectRoot)
    {
        int childCount = pageRectRoot.childCount;
        for (int i = 0; i < childCount; i++)
        {
            GameObject gameObject = pageRectRoot.GetChild(i).gameObject;
            DragDropButton component = gameObject.GetComponent<DragDropButton>();
            if (component != null)
            {
                GeneDragDropData geneDragDropData = component.mData as GeneDragDropData;
                if (geneDragDropData != null && geneDragDropData.isThisSlotOpen && geneDragDropData.IsThisPosHaveCard())
                {
                    ActionData ad = new ActionData(ActionType.UnInsert, geneDragDropData, null, 0U);
                    this.gc.AddOperationAction(ad);
                }
            }
        }
    }

    public void OnUpdate()
    {
        this.PlayeUIMoveAnimation();
    }

    private void PlayeUIMoveAnimation()
    {
        if (this.isNeedMove)
        {
            RectTransform component = this.panelRootDetailInSwapPage.transform.FindChild("Panel_detail").GetComponent<RectTransform>();
            RectTransform component2 = this.panelRootDetailInSwapPage.GetComponent<RectTransform>();
            Vector3 a = component.anchoredPosition;
            Vector3 localEulerAngles = this.btnDetailAttribute.transform.localEulerAngles;
            Vector3 b = Vector3.zero;
            Vector3 b2 = Vector3.forward * 180f;
            if (this.dus == DetailUIState.Hide)
            {
                b = Vector3.right * (component2.sizeDelta.x + 10f);
                b2 = Vector3.zero;
            }
            this.progress += Time.deltaTime / this.animationTime + (1f - this.progress) * 0.2f;
            this.progress = Mathf.Clamp01(this.progress);
            component.anchoredPosition = Vector3.Lerp(a, b, this.progress);
            this.btnDetailAttribute.transform.localEulerAngles = Vector3.Lerp(localEulerAngles, b2, this.progress);
            if (Vector3.Distance(a, b) < 1f)
            {
                this.isNeedMove = false;
            }
        }
    }

    private void DetailUIClick()
    {
        if (this.dus == DetailUIState.Hide)
        {
            this.panelRootDetailInInsertPage.gameObject.SetActive(true);
            this.dus = DetailUIState.Show;
        }
        else
        {
            this.panelRootDetailInInsertPage.gameObject.SetActive(false);
            this.dus = DetailUIState.Hide;
        }
    }

    private void SetFuseButtonState(bool isMatchFuseCondition)
    {
        Button component = this.btnFuse.GetComponent<Button>();
        component.interactable = isMatchFuseCondition;
    }

    private void Fuse()
    {
        Text component = this.panelRootFuse.FindChild("img_cost/Text_cost_value").GetComponent<Text>();
        int num = int.Parse(component.text);
        if ((ulong)MainPlayer.Self.GetCurrencyByID(3U) < (ulong)((long)num))
        {
            TipsWindow.ShowNotice(3006U);
            return;
        }
        if (this.inPlayEffect)
        {
            return;
        }
        this.inPlayEffect = true;
        for (int i = 0; i < this.fuseEffectObjs.Count; i++)
        {
            this.fuseEffectObjs[i].gameObject.SetActive(true);
        }
        Scheduler.Instance.AddTimer(1f, false, delegate
        {
            if (UIManager.GetUIObject<UI_Gene>() == null)
            {
                return;
            }
            GameObject gameObject = this.panelRootFuse.FindChild("Button_item").gameObject;
            gameObject.SetActive(false);
        });
        Scheduler.Instance.AddTimer(2f, false, delegate
        {
            if (UIManager.GetUIObject<UI_Gene>() == null)
            {
                return;
            }
            for (int j = 0; j < this.fuseEffectObjs.Count; j++)
            {
                this.fuseEffectObjs[j].gameObject.SetActive(false);
            }
            Image component2 = this.fusePreview.FindChild("icon").GetComponent<Image>();
            component2.color = Color.white;
            this.gc.StartFuse();
            this.FuseCancel(true);
            this.inPlayEffect = false;
        });
    }

    private Dictionary<uint, Hole> HoleListToHoleDic(List<Hole> holeList)
    {
        Dictionary<uint, Hole> dictionary = new Dictionary<uint, Hole>();
        for (int i = 0; i < holeList.Count; i++)
        {
            dictionary[holeList[i].pos] = holeList[i];
        }
        return dictionary;
    }

    public override void OnDispose()
    {
        base.OnDispose();
        this.gc_ = null;
    }

    public const uint dnaPageTotalNum = 9U;

    public const int fuseCardCount = 3;

    private Transform ui_root;

    private GeneController gc_;

    public Transform panelRoot;

    public Transform panelTips;

    public Transform panelRootDetailInSwapPage;

    public Transform panelRootDetailInInsertPage;

    public Transform panelRootFuse;

    public Transform panelRootSeal;

    public Transform panelRootBag;

    public Transform panelRootInsert;

    public Transform btnClose;

    public Transform btnEdtePageName;

    public Transform dropDownList;

    public Transform inputEditeName;

    public Transform atPageRectRoot;

    public Transform dfPageRectRoot;

    public Transform btnUnloadAll;

    public InputField inputfield_num;

    public Transform btnFuse;

    public Transform btnCancelFuse;

    public Transform btnDetailAttribute;

    public Transform txtTitle;

    public Transform imgTitleInsert;

    public Transform attRootAtt;

    public Transform attRootDef;

    public Transform btnEnablePage;

    public Transform textInUseText;

    public Transform togleGeneGroupOrLib;

    public Transform togleGeneGroup;

    public Transform togleGeneLib;

    public Transform fusePreview;

    public Transform wayFind;

    public Transform imageHelp;

    public Transform btnRuleTips;

    private Transform ruleTipPanel;

    public Transform togleShowGeneNotInPage;

    public GeneOperationType curGot;

    private ItemTipController itc_;

    private bool ruleState;

    private List<Transform> fuseEffectObjs = new List<Transform>();

    private GeneDragDropData mGddd;

    private int maxNum;

    private float animationTime = 0.5f;

    private float progress;

    private bool isNeedMove;

    private DetailUIState dus = DetailUIState.Hide;

    public bool inPlayEffect;
}
