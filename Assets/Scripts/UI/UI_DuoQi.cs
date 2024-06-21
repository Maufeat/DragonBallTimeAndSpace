using System;
using System.Collections.Generic;
using battle;
using Framework.Managers;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UI_DuoQi : UIPanelBase
{
    private DuoQiController mController
    {
        get
        {
            return ControllerManager.Instance.GetController<DuoQiController>();
        }
    }

    public override void AfterInit()
    {
        base.AfterInit();
    }

    public override void OnInit(Transform root)
    {
        this.mRoot = root;
        base.OnInit(root);
    }

    public void Initilize(bool isResult, bool isSucc = false)
    {
        string text = (!isResult) ? "Panel_live" : "BFresult";
        if (isResult)
        {
            this.mRoot.Find("Offset/" + text + "/Panel_title/img_win").gameObject.SetActive(isSucc);
            this.mRoot.Find("Offset/" + text + "/Panel_title/img_lose").gameObject.SetActive(!isSucc);
        }
        this.transItem = this.mRoot.Find("Offset/" + text + "/Scroll View/Viewport/Content/Panel_info");
        this.transItemOther = this.mRoot.Find("Offset/" + text + "/Scroll View/Viewport/Content/Panel_info (1)");
        Button component = this.mRoot.Find("Offset/" + text + "/Panel_title/btn_close").GetComponent<Button>();
        component.onClick.RemoveAllListeners();
        component.onClick.AddListener(new UnityAction(this.btn_close_onclick));
        Button component2 = this.mRoot.Find("Offset/" + text + "/btn_exit").GetComponent<Button>();
        component2.onClick.RemoveAllListeners();
        component2.onClick.AddListener(new UnityAction(this.btn_exitcopymap_onclick));
        Toggle component3 = this.mRoot.Find(string.Concat(new string[]
        {
            "Offset/",
            text,
            "/ToggleGroup/",
            (!isResult) ? "Panel_tab/" : string.Empty,
            "all"
        })).GetComponent<Toggle>();
        Toggle component4 = this.mRoot.Find(string.Concat(new string[]
        {
            "Offset/",
            text,
            "/ToggleGroup/",
            (!isResult) ? "Panel_tab/" : string.Empty,
            "side1"
        })).GetComponent<Toggle>();
        Toggle component5 = this.mRoot.Find(string.Concat(new string[]
        {
            "Offset/",
            text,
            "/ToggleGroup/",
            (!isResult) ? "Panel_tab/" : string.Empty,
            "side2"
        })).GetComponent<Toggle>();
        component3.onValueChanged.AddListener(new UnityAction<bool>(this.SwitchTabToggleAll));
        component4.onValueChanged.AddListener(new UnityAction<bool>(this.SwitchTabToggleSide1));
        component5.onValueChanged.AddListener(new UnityAction<bool>(this.SwitchTabToggleSide2));
        this.mRoot.Find("Offset/Panel_live").gameObject.SetActive(!isResult);
        this.mRoot.Find("Offset/BFresult").gameObject.SetActive(isResult);
    }

    private void SwitchTabToggleAll(bool isTrue)
    {
        if (!isTrue)
        {
            return;
        }
        this.DoSwitch(UI_DuoQi.TabType.All);
    }

    private void SwitchTabToggleSide1(bool isTrue)
    {
        if (!isTrue)
        {
            return;
        }
        this.DoSwitch(UI_DuoQi.TabType.SameTeam);
    }

    private void SwitchTabToggleSide2(bool isTrue)
    {
        if (!isTrue)
        {
            return;
        }
        this.DoSwitch(UI_DuoQi.TabType.OtherTeam);
    }

    private void DoSwitch(UI_DuoQi.TabType tabType)
    {
        this.mTabType = tabType;
        this.SetupPanel(this.mController.mItemList);
    }

    public void SetupPanel(List<HoldFlagReport> itemList)
    {
        UIManager.Instance.ClearListChildrens(this.transItem.parent, 2);
        for (int i = 0; i < itemList.Count; i++)
        {
            HoldFlagReport holdFlagReport = itemList[i];
            if (this.mTabType != UI_DuoQi.TabType.SameTeam || this.mController.IsSameCamp(holdFlagReport.campId))
            {
                if (this.mTabType != UI_DuoQi.TabType.OtherTeam || !this.mController.IsSameCamp(holdFlagReport.campId))
                {
                    GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>((!this.mController.IsSameCamp(holdFlagReport.campId)) ? this.transItemOther.gameObject : this.transItem.gameObject);
                    gameObject.transform.SetParent(this.transItem.parent);
                    gameObject.transform.localScale = Vector3.one;
                    gameObject.SetActive(true);
                    gameObject.name = i.ToString();
                    Image component = gameObject.GetComponent<Image>();
                    Text component2;
                    if (gameObject.transform.Find("txt_player") != null)
                    {
                        component2 = gameObject.transform.Find("txt_player").GetComponent<Text>();
                    }
                    else
                    {
                        component2 = gameObject.transform.Find("txt_name").GetComponent<Text>();
                    }
                    Text component3 = gameObject.transform.Find("txt_damage").GetComponent<Text>();
                    Text component4 = gameObject.transform.Find("txt_cure").GetComponent<Text>();
                    Text component5 = gameObject.transform.Find("txt_kill").GetComponent<Text>();
                    Text component6 = gameObject.transform.Find("txt_dead").GetComponent<Text>();
                    Text component7 = gameObject.transform.Find("txt_rob").GetComponent<Text>();
                    Transform transform = gameObject.transform.Find("txt_return");
                    component2.text = holdFlagReport.name;
                    component3.text = holdFlagReport.hurtNum.ToString();
                    component4.text = holdFlagReport.cureNum.ToString();
                    component5.text = holdFlagReport.killNum.ToString();
                    component6.text = holdFlagReport.deadNum.ToString();
                    component7.text = holdFlagReport.captureDBNum.ToString();
                    if (transform != null)
                    {
                        transform.GetComponent<Text>().text = holdFlagReport.backDBNum.ToString();
                    }
                    else
                    {
                        Debug.LogError("transReturn is null");
                    }
                }
            }
        }
    }

    private void btn_close_onclick()
    {
        UIManager.Instance.DeleteUI<UI_DuoQi>();
    }

    private void btn_exitcopymap_onclick()
    {
        this.mController.ReqExitCopy();
    }

    public override void OnDispose()
    {
        base.OnDispose();
    }

    private UI_DuoQi.TabType mTabType;

    public Transform transItem;

    public Transform transItemOther;

    private Transform mRoot;

    private enum TabType
    {
        All,
        SameTeam,
        OtherTeam
    }
}
