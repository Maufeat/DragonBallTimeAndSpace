using System;
using Framework.Managers;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UI_AntiAddiction : UIPanelBase
{
    public override void OnInit(Transform root)
    {
        this.panel1 = root.Find("Offset_Example/1st").gameObject;
        this.panel2 = root.Find("Offset_Example/2nd").gameObject;
        this.panel3 = root.Find("Offset_Example/Panel").gameObject;
        this.panel5 = root.Find("Offset_Example/5th").gameObject;
        this.panel6 = root.Find("Offset_Example/login").gameObject;
        root.Find("Offset_Example/1st/Button").GetComponent<Button>().onClick.AddListener(new UnityAction(this.btn_register_onclick));
        root.Find("Offset_Example/2nd/Button").GetComponent<Button>().onClick.AddListener(new UnityAction(this.btn_register_onclick));
        root.Find("Offset_Example/Panel/Button").GetComponent<Button>().onClick.AddListener(new UnityAction(this.btn_register_onclick));
        root.Find("Offset_Example/5th/Button").GetComponent<Button>().onClick.AddListener(new UnityAction(this.btn_register_onclick));
        root.Find("Offset_Example/login/Button").GetComponent<Button>().onClick.AddListener(new UnityAction(this.btn_register_onclick));
        root.Find("Offset_Example/1st/panel_title/btn_close").GetComponent<Button>().onClick.AddListener(new UnityAction(this.btn_close_onclick));
        root.Find("Offset_Example/2nd/panel_title/btn_close").GetComponent<Button>().onClick.AddListener(new UnityAction(this.btn_close_onclick));
        root.Find("Offset_Example/Panel/panel_title/btn_close").GetComponent<Button>().onClick.AddListener(new UnityAction(this.btn_close_onclick));
        root.Find("Offset_Example/5th/panel_title/btn_close").GetComponent<Button>().onClick.AddListener(new UnityAction(this.btn_close_onclick));
        root.Find("Offset_Example/login/panel_title/btn_close").GetComponent<Button>().onClick.AddListener(new UnityAction(this.btn_close_onclick));
        this.mController = ControllerManager.Instance.GetController<AntiAddictionController>();
        base.OnInit(root);
    }

    public void SetupPanel(int hour)
    {
        this.panel1.gameObject.SetActive(false);
        this.panel2.gameObject.SetActive(false);
        this.panel3.gameObject.SetActive(false);
        this.panel5.gameObject.SetActive(false);
        this.panel6.gameObject.SetActive(false);
        switch (hour + 1)
        {
            case 0:
                this.panel6.gameObject.SetActive(true);
                break;
            case 2:
                this.panel1.gameObject.SetActive(true);
                break;
            case 3:
                this.panel2.gameObject.SetActive(true);
                break;
            case 4:
            case 5:
                this.panel3.gameObject.SetActive(true);
                break;
            case 6:
                this.panel5.gameObject.SetActive(true);
                break;
        }
    }

    private void btn_register_onclick()
    {
        Application.OpenURL("http://my.ztgame.com/plugin/account/bind-idcard-page?account=" + UserInfoStorage.StorageInfo.Uid);
        this.mController.CloseUI();
    }

    private void btn_close_onclick()
    {
        this.mController.CloseUI();
    }

    public override void OnDispose()
    {
        base.OnDispose();
    }

    private AntiAddictionController mController;

    private GameObject panel1;

    private GameObject panel2;

    private GameObject panel3;

    private GameObject panel5;

    private GameObject panel6;
}
