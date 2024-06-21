using System;
using Framework.Managers;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UI_JoinCamp : UIPanelBase
{
    private CampController campController
    {
        get
        {
            return ControllerManager.Instance.GetController<CampController>();
        }
    }

    public override void OnDispose()
    {
        base.OnDispose();
        GameObject gameObject = this.root.gameObject;
        this.root = null;
        UnityEngine.Object.Destroy(gameObject);
    }

    public override void OnInit(Transform root)
    {
        base.OnInit(root);
        this.InitObj(root);
        this.InitEvent();
    }

    private void InitObj(Transform root)
    {
        this.root = root;
        this.tg_camp1 = root.transform.Find("Offset_ChooseCamp/Panel_ChooseCamp/btn_A").GetComponent<Toggle>();
        this.tg_camp2 = root.transform.Find("Offset_ChooseCamp/Panel_ChooseCamp/btn_B").GetComponent<Toggle>();
    }

    private void InitEvent()
    {
        this.tg_camp1.onValueChanged.RemoveAllListeners();
        this.tg_camp1.onValueChanged.AddListener(new UnityAction<bool>(this.OnTgCamp1));
        this.tg_camp2.onValueChanged.RemoveAllListeners();
        this.tg_camp2.onValueChanged.AddListener(new UnityAction<bool>(this.OnTgCamp2));
    }

    private void OnTgCamp1(bool state)
    {
        if (state)
        {
            this.ShowDoubleCheck(1U);
        }
    }

    private void OnTgCamp2(bool state)
    {
        if (state)
        {
            this.ShowDoubleCheck(2U);
        }
    }

    private void ShowDoubleCheck(uint campid)
    {
        ControllerManager.Instance.GetController<MsgBoxController>().ShowMsgBox(MsgBoxController.MsgLevelSystemNotification, CommonUtil.GetText(dynamic_textid.IDs.camp_join), MsgBoxController.MsgOptionConfirm, MsgBoxController.MsgOptionCancel, UIManager.ParentType.CommonUI, delegate ()
        {
            this.campController.ReqJoinCamp(campid);
            this.campController.CloseJoinCamp();
        }, delegate ()
        {
            this.campController.CloseJoinCamp();
        }, null);
    }

    private Transform root;

    private Toggle tg_camp1;

    private Toggle tg_camp2;
}
