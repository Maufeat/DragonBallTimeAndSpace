using System;
using Framework.Managers;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_NumberInput : UIPanelBase
{
    public override void OnInit(Transform root)
    {
        base.OnInit(root);
        this.controller = ControllerManager.Instance.GetController<NumberInputController>();
        this.InitObj(root);
        this.InitEvent();
        this.Panel_NumberInput_0.localPosition = new Vector3(24000f, 24000f, 0f);
        Scheduler.Instance.AddFrame(4U, true, new Scheduler.OnScheduler(this.controller.UpdatePosition));
    }

    public override void OnDispose()
    {
        base.OnDispose();
        Scheduler.Instance.RemoveFrame(new Scheduler.OnScheduler(this.controller.UpdatePosition));
    }

    private void InitObj(Transform root)
    {
        this.panelRoot = root.Find("Offset_NumberInput");
        this.Panel_NumberInput_0 = this.panelRoot.Find("Panel_NumberInput");
        this.img_bg_3 = this.panelRoot.Find("Panel_NumberInput/img_bg").GetComponent<Image>();
        this.btn_1_0 = this.panelRoot.Find("Panel_NumberInput/btn_1").GetComponent<Button>();
        this.btn_2_1 = this.panelRoot.Find("Panel_NumberInput/btn_2").GetComponent<Button>();
        this.btn_3_2 = this.panelRoot.Find("Panel_NumberInput/btn_3").GetComponent<Button>();
        this.btn_4_4 = this.panelRoot.Find("Panel_NumberInput/btn_4").GetComponent<Button>();
        this.btn_5_5 = this.panelRoot.Find("Panel_NumberInput/btn_5").GetComponent<Button>();
        this.btn_6_6 = this.panelRoot.Find("Panel_NumberInput/btn_6").GetComponent<Button>();
        this.btn_0_7 = this.panelRoot.Find("Panel_NumberInput/btn_0").GetComponent<Button>();
        this.btn_7_8 = this.panelRoot.Find("Panel_NumberInput/btn_7").GetComponent<Button>();
        this.btn_8_9 = this.panelRoot.Find("Panel_NumberInput/btn_8").GetComponent<Button>();
        this.btn_9_10 = this.panelRoot.Find("Panel_NumberInput/btn_9").GetComponent<Button>();
        this.btn_ok_11 = this.panelRoot.Find("Panel_NumberInput/btn_ok").GetComponent<Button>();
        this.btn_reback_3 = this.panelRoot.Find("Panel_NumberInput/btn_reback").GetComponent<Button>();
    }

    private void InitEvent()
    {
        this.btn_1_0.onClick.RemoveAllListeners();
        this.btn_1_0.onClick.AddListener(new UnityAction(this.onclick_btn_1_0));
        this.btn_2_1.onClick.RemoveAllListeners();
        this.btn_2_1.onClick.AddListener(new UnityAction(this.onclick_btn_2_1));
        this.btn_3_2.onClick.RemoveAllListeners();
        this.btn_3_2.onClick.AddListener(new UnityAction(this.onclick_btn_3_2));
        this.btn_4_4.onClick.RemoveAllListeners();
        this.btn_4_4.onClick.AddListener(new UnityAction(this.onclick_btn_4_4));
        this.btn_5_5.onClick.RemoveAllListeners();
        this.btn_5_5.onClick.AddListener(new UnityAction(this.onclick_btn_5_5));
        this.btn_6_6.onClick.RemoveAllListeners();
        this.btn_6_6.onClick.AddListener(new UnityAction(this.onclick_btn_6_6));
        this.btn_0_7.onClick.RemoveAllListeners();
        this.btn_0_7.onClick.AddListener(new UnityAction(this.onclick_btn_0_7));
        this.btn_7_8.onClick.RemoveAllListeners();
        this.btn_7_8.onClick.AddListener(new UnityAction(this.onclick_btn_7_8));
        this.btn_8_9.onClick.RemoveAllListeners();
        this.btn_8_9.onClick.AddListener(new UnityAction(this.onclick_btn_8_9));
        this.btn_9_10.onClick.RemoveAllListeners();
        this.btn_9_10.onClick.AddListener(new UnityAction(this.onclick_btn_9_10));
        this.btn_ok_11.onClick.RemoveAllListeners();
        this.btn_ok_11.onClick.AddListener(new UnityAction(this.onclick_btn_ok_11));
        this.btn_reback_3.onClick.RemoveAllListeners();
        this.btn_reback_3.onClick.AddListener(new UnityAction(this.onclick_btn_reback_3));
    }

    private void onclick_btn_1_0()
    {
        this.InputNumber(1);
    }

    private void onclick_btn_2_1()
    {
        this.InputNumber(2);
    }

    private void onclick_btn_3_2()
    {
        this.InputNumber(3);
    }

    private void onclick_btn_4_4()
    {
        this.InputNumber(4);
    }

    private void onclick_btn_5_5()
    {
        this.InputNumber(5);
    }

    private void onclick_btn_6_6()
    {
        this.InputNumber(6);
    }

    private void onclick_btn_0_7()
    {
        this.InputNumber(0);
    }

    private void onclick_btn_7_8()
    {
        this.InputNumber(7);
    }

    private void onclick_btn_8_9()
    {
        this.InputNumber(8);
    }

    private void onclick_btn_9_10()
    {
        this.InputNumber(9);
    }

    private void onclick_btn_ok_11()
    {
        this.CloseInputUI(null);
    }

    private void onclick_btn_reback_3()
    {
        this.DeleteNumber();
    }

    private void CloseInputUI(PointerEventData data)
    {
        this.controller.CloseInput();
    }

    private void InputNumber(int n)
    {
        this.controller.InputNumber(n);
    }

    private void DeleteNumber()
    {
        this.controller.DeleteNumber();
    }

    private Transform ui_root;

    private NumberInputController controller;

    private Transform panelRoot;

    public Transform Panel_NumberInput_0;

    private Image img_bg_3;

    private Button btn_1_0;

    private Button btn_2_1;

    private Button btn_3_2;

    private Button btn_4_4;

    private Button btn_5_5;

    private Button btn_6_6;

    private Button btn_0_7;

    private Button btn_7_8;

    private Button btn_8_9;

    private Button btn_9_10;

    private Button btn_ok_11;

    private Button btn_reback_3;
}
