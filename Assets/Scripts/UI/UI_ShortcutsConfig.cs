using System;
using System.Collections.Generic;
using Framework.Managers;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UI_ShortcutsConfig : UIPanelBase
{
    public override void OnInit(Transform root)
    {
        base.OnInit(root);
        this.uiRoot = root;
        this.scc = ControllerManager.Instance.GetController<ShortcutsConfigController>();
        this.InitEvent(root);
        this.sfdlCopy = this.scc.CloneShortcutsFunctionData(ShortcutsConfigController.sfdl);
        this.scc.SetShortcutsConfigUIState(true);
    }

    public override void OnDispose()
    {
        base.OnDispose();
        ShortcutsConfigController.sfdl = this.scc.CloneShortcutsFunctionData(this.sfdlCopy);
        this.sfdlCopy = null;
        this.scc.SetShortcutsConfigUIState(false);
    }

    private void InitEvent(Transform root)
    {
        Button component = root.Find("Offset_ShortcutsConfig/Panel_root/title/btn_close").GetComponent<Button>();
        component.onClick.AddListener(new UnityAction(this.CloseConfig));
        Button component2 = root.Find("Offset_ShortcutsConfig/Panel_root/tap_root/btn_asis_interface").GetComponent<Button>();
        component2.onClick.AddListener(new UnityAction(this.InterfaceAsis));
        Button component3 = root.Find("Offset_ShortcutsConfig/Panel_root/tap_root/btn_asis_fight").GetComponent<Button>();
        component3.onClick.AddListener(new UnityAction(this.FightAsis));
        Button component4 = root.Find("Offset_ShortcutsConfig/Panel_root/tap_root/btn_asis_shortcutsitem").GetComponent<Button>();
        component4.onClick.AddListener(new UnityAction(this.ExtendAsis));
        Button component5 = root.Find("Offset_ShortcutsConfig/Panel_root/bottom_root/btn_set_default").GetComponent<Button>();
        component5.onClick.AddListener(new UnityAction(this.SetDefault));
        this.btnEnsure = root.Find("Offset_ShortcutsConfig/Panel_root/bottom_root/btn_ensure").GetComponent<Button>();
        this.btnEnsure.onClick.AddListener(new UnityAction(this.Ensure));
        this.SetApplyConfigButtonState(false);
        Button component6 = root.Find("Offset_ShortcutsConfig/Panel_root/bottom_root/btn_cancel").GetComponent<Button>();
        component6.onClick.AddListener(new UnityAction(this.Cancel));
        this.btnsTap.Add(component2);
        this.btnsTap.Add(component3);
        this.btnsTap.Add(component4);
        root.Find("Offset_ShortcutsConfig/Panel_root/title/Text_success_tip").gameObject.SetActive(false);
        this.btnSkillSetTr = root.Find("Offset_ShortcutsConfig/Panel_root/items_root/Panel/GroupItem/btn_skill_set");
        this.btnGoodsSetTr = root.Find("Offset_ShortcutsConfig/Panel_root/items_root/Panel/GroupItem1/btn_goods_set");
        this.scrollRectTr = root.Find("Offset_ShortcutsConfig/Panel_root/items_root/Panel");
        this.btnSkillSetTr.gameObject.SetActive(false);
        this.btnGoodsSetTr.gameObject.SetActive(false);
        Button component7 = this.btnSkillSetTr.GetComponent<Button>();
        component7.onClick.AddListener(new UnityAction(this.ExtendSkillAsis));
        Button component8 = this.btnGoodsSetTr.GetComponent<Button>();
        component8.onClick.AddListener(new UnityAction(this.ExtendGoodsAsis));
        this.InterfaceAsis();
    }

    private void InterfaceAsis()
    {
        this.ExtendTabButtonSwitch(false);
        this.FillFunctionUIByAsisType(KeyConfigTapType.InterfaceAsis);
        this.SetTapBtnSelectState("interface");
        this.SetListStateUI(KeyConfigTapType.InterfaceAsis, true);
    }

    private void FightAsis()
    {
        this.ExtendTabButtonSwitch(false);
        this.FillFunctionUIByAsisType(KeyConfigTapType.PlayerAsis);
        this.SetTapBtnSelectState("fight");
        this.SetListStateUI(KeyConfigTapType.PlayerAsis, true);
    }

    public void SetApplyConfigButtonState(bool isNeedApply)
    {
        if (this.btnEnsure != null)
        {
            this.btnEnsure.interactable = isNeedApply;
        }
    }

    private void SetListStateUI(KeyConfigTapType type, bool isMenu = true)
    {
        Transform transform = this.uiRoot.Find("Offset_ShortcutsConfig/Panel_root/items_root/Panel/GroupItem/ItemList");
        GameObject gameObject = this.uiRoot.Find("Offset_ShortcutsConfig/Panel_root/items_root/Panel/GroupItem/ItemList/item").gameObject;
        Transform transform2 = this.uiRoot.Find("Offset_ShortcutsConfig/Panel_root/items_root/Panel/GroupItem1/ItemList");
        GameObject gameObject2 = this.uiRoot.Find("Offset_ShortcutsConfig/Panel_root/items_root/Panel/GroupItem1/ItemList/item").gameObject;
        if (type == KeyConfigTapType.InterfaceAsis || type == KeyConfigTapType.PlayerAsis)
        {
            transform.gameObject.SetActive(true);
            transform2.gameObject.SetActive(false);
        }
        if (type == KeyConfigTapType.ExtendSkillAsis)
        {
            if (isMenu)
            {
                transform.gameObject.SetActive(true);
                transform2.gameObject.SetActive(false);
            }
            else
            {
                transform.gameObject.SetActive(!transform.gameObject.activeSelf);
            }
        }
        if (type == KeyConfigTapType.ExtendItemsAsis)
        {
            transform2.gameObject.SetActive(!transform2.gameObject.activeSelf);
        }
        this.SetGroupBgStateUI(this.uiRoot.Find("Offset_ShortcutsConfig/Panel_root/items_root/Panel/GroupItem/btn_skill_set/group_bg"), transform.gameObject.activeSelf);
        this.SetGroupBgStateUI(this.uiRoot.Find("Offset_ShortcutsConfig/Panel_root/items_root/Panel/GroupItem1/btn_goods_set/group_bg"), transform2.gameObject.activeSelf);
    }

    private void SetGroupBgStateUI(Transform trans, bool isOpen)
    {
        trans.Find("group_g").gameObject.SetActive(!isOpen);
        trans.Find("group_p").gameObject.SetActive(isOpen);
    }

    private void ExtendAsis()
    {
        this.ExtendTabButtonSwitch(true);
        this.FillFunctionUIByAsisType(KeyConfigTapType.ExtendSkillAsis);
        this.SetTapTrIndex(UI_ShortcutsConfig.ExtendsType.SkillTap);
        this.SetListStateUI(KeyConfigTapType.ExtendSkillAsis, true);
        this.SetTapBtnSelectState("shortcutsitem");
        this.SetTapTrIndex(UI_ShortcutsConfig.ExtendsType.Init);
    }

    private void ExtendTabButtonSwitch(bool state)
    {
        this.btnSkillSetTr.gameObject.SetActive(state);
        this.btnGoodsSetTr.gameObject.SetActive(state);
    }

    private void ExtendSkillAsis()
    {
        this.FillFunctionUIByAsisType(KeyConfigTapType.ExtendSkillAsis);
        this.SetTapTrIndex(UI_ShortcutsConfig.ExtendsType.SkillTap);
        this.SetListStateUI(KeyConfigTapType.ExtendSkillAsis, false);
    }

    private void ExtendGoodsAsis()
    {
        this.FillFunctionUIByAsisType(KeyConfigTapType.ExtendItemsAsis);
        this.SetTapTrIndex(UI_ShortcutsConfig.ExtendsType.GoodsTap);
        this.SetListStateUI(KeyConfigTapType.ExtendItemsAsis, false);
    }

    private void SetTapTrIndex(UI_ShortcutsConfig.ExtendsType toState)
    {
    }

    private void SetDefault()
    {
        this.SetApplyConfigButtonState(true);
        this.scc.BindShortcutsKeyDefaultKeyCode(false);
        this.scc.RegShortcutsKeyEvent();
        this.RefrashUIState();
        SkillViewControll controller = ControllerManager.Instance.GetController<SkillViewControll>();
        if (controller != null)
        {
            controller.ReBindShortcutEvent();
        }
    }

    private void Ensure()
    {
        this.scc.SetConfigState(ShortcutsConfigState.InPickFunction);
        this.scc.SaveConfig();
        this.RefrashUIState();
        this.sfdlCopy = this.scc.CloneShortcutsFunctionData(ShortcutsConfigController.sfdl);
        UI_MainView uiobject = UIManager.GetUIObject<UI_MainView>();
        if (uiobject != null)
        {
            uiobject.FrashShortcutUIShowName();
        }
        UI_ShortcutControl component = UIManager.GetUIObject<UI_MainView>().Root.GetComponent<UI_ShortcutControl>();
        if (component)
        {
            component.FrashShortcutUIShowName();
        }
        this.SetApplyConfigButtonState(false);
        TipsWindow.ShowNotice("修改成功");
    }

    private void Cancel()
    {
        ShortcutsConfigController.sfdl = this.scc.CloneShortcutsFunctionData(this.sfdlCopy);
        this.RefrashUIState();
    }

    private void SetTapBtnSelectState(string selectStr)
    {
        foreach (Button button in this.btnsTap)
        {
            button.transform.Find("bg_select").gameObject.SetActive(button.name.EndsWith(selectStr));
            button.transform.Find("bg_non_select").gameObject.SetActive(!button.name.EndsWith(selectStr));
        }
    }

    private void CloseConfig()
    {
        this.Cancel();
        UIManager.Instance.DeleteUI<UI_ShortcutsConfig>();
        this.scc.SetShortcutsConfigUIState(false);
    }

    private void FillFunctionUIByAsisType(KeyConfigTapType at)
    {
        this.lastSelectItemTransform = null;
        this.scc.SetConfigState(ShortcutsConfigState.InPickFunction);
        List<ShortcutsFunctionData> shortcutsDatayByAsisType = this.scc.GetShortcutsDatayByAsisType(at);
        Transform transform = this.uiRoot.Find("Offset_ShortcutsConfig/Panel_root/items_root/Panel/GroupItem/ItemList");
        GameObject gameObject = this.uiRoot.Find("Offset_ShortcutsConfig/Panel_root/items_root/Panel/GroupItem/ItemList/item").gameObject;
        Transform transform2 = this.uiRoot.Find("Offset_ShortcutsConfig/Panel_root/items_root/Panel/GroupItem1/ItemList");
        GameObject gameObject2 = this.uiRoot.Find("Offset_ShortcutsConfig/Panel_root/items_root/Panel/GroupItem1/ItemList/item").gameObject;
        if (at == KeyConfigTapType.ExtendItemsAsis)
        {
            transform = transform2;
            gameObject = gameObject2;
        }
        UIManager.Instance.ClearListChildrens(transform, 1);
        for (int i = 0; i < shortcutsDatayByAsisType.Count; i++)
        {
            GameObject gameObject3 = UnityEngine.Object.Instantiate<GameObject>(gameObject);
            gameObject3.transform.SetParent(transform, true);
            gameObject3.transform.localScale = gameObject.transform.localScale;
            gameObject3.name = "item" + i;
            gameObject3.SetActive(true);
            if (i < shortcutsDatayByAsisType.Count)
            {
                this.SetItemData(gameObject3, shortcutsDatayByAsisType[i]);
            }
        }
        this.abt = at;
    }

    private void SetItemData(GameObject itemObj, ShortcutsFunctionData sfd)
    {
        Text component = itemObj.transform.Find("Text_function_name").GetComponent<Text>();
        component.text = sfd.functionName;
        Text textCurKey = itemObj.transform.Find("btn_set/Text_key").GetComponent<Text>();
        string skName = this.GetKeyName(sfd);
        textCurKey.text = skName;
        Button component2 = itemObj.transform.Find("btn_set").GetComponent<Button>();
        component2.onClick.RemoveAllListeners();
        itemObj.FindChild("btn_set/bg_normal").gameObject.SetActive(true);
        itemObj.FindChild("btn_set/bg_setstate").gameObject.SetActive(false);
        component2.onClick.AddListener(delegate ()
        {
            if (this.lastSelctSfd != null && this.lastSelctSfd != sfd && this.scc.configState == ShortcutsConfigState.InPickKey)
            {
                this.lastSelectItemTransform.Find("btn_set/bg_normal").gameObject.SetActive(true);
                this.lastSelectItemTransform.Find("btn_set/bg_setstate").gameObject.SetActive(false);
                Text component3 = this.lastSelectItemTransform.transform.Find("btn_set/Text_key").GetComponent<Text>();
                component3.text = this.GetKeyName(this.lastSelctSfd);
                itemObj.FindChild("btn_set/bg_normal").gameObject.SetActive(false);
                itemObj.FindChild("btn_set/bg_setstate").gameObject.SetActive(true);
                textCurKey = itemObj.transform.Find("btn_set/Text_key").GetComponent<Text>();
                textCurKey.text = "请输入按键";
            }
            else if (this.scc.configState == ShortcutsConfigState.InPickFunction)
            {
                itemObj.FindChild("btn_set/bg_normal").gameObject.SetActive(false);
                itemObj.FindChild("btn_set/bg_setstate").gameObject.SetActive(true);
                this.scc.SetConfigState(ShortcutsConfigState.InPickKey);
                textCurKey.text = "请输入按键";
            }
            else
            {
                itemObj.FindChild("btn_set/bg_normal").gameObject.SetActive(true);
                itemObj.FindChild("btn_set/bg_setstate").gameObject.SetActive(false);
                textCurKey.text = skName;
                this.scc.SetConfigState(ShortcutsConfigState.InPickFunction);
            }
            this.lastSelectItemTransform = itemObj.transform;
            this.lastSelctSfd = sfd;
        });
    }

    private string GetKeyName(ShortcutsFunctionData kd)
    {
        string text = "未指定";
        if (kd.keys != null && kd.keys.Count > 0)
        {
            text = kd.keyShowNameInSetting;
            if (text.EndsWith("+"))
            {
                text = text.Substring(0, text.Length - 1);
            }
        }
        return text;
    }

    internal void RefrashUIState()
    {
        this.FillFunctionUIByAsisType(this.abt);
    }

    private ShortcutsConfigController scc;

    private Transform uiRoot;

    internal List<ShortcutsFunctionData> sfdlCopy;

    private Button btnEnsure;

    private KeyConfigTapType abt;

    private Transform lastSelectItemTransform;

    internal ShortcutsFunctionData lastSelctSfd;

    private List<Button> btnsTap = new List<Button>();

    private Transform btnSkillSetTr;

    private Transform btnGoodsSetTr;

    private Transform scrollRectTr;

    private UI_ShortcutsConfig.ExtendsType lastEtState = UI_ShortcutsConfig.ExtendsType.Init;

    public enum ExtendsType
    {
        SkillTap,
        GoodsTap,
        AllClose,
        Init
    }
}
