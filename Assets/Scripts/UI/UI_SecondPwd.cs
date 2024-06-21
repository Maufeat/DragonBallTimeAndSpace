using System;
using System.Collections.Generic;
using Framework.Managers;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UI_SecondPwd : UIPanelBase
{
    public override void OnInit(Transform root)
    {
        base.OnInit(root);
        Scheduler.Instance.AddUpdator(new Scheduler.OnScheduler(this.Update));
        this.control = ControllerManager.Instance.GetController<SecondPwdControl>();
        this.Root = root;
        this.Panel_SetGo = this.Root.Find("Offset_Example/Panel_set").gameObject;
        GameObject gameObject = this.Panel_SetGo.transform.Find("Panel_top/btn_rule").gameObject;
        Button component = gameObject.GetComponent<Button>();
        component.onClick.RemoveAllListeners();
        component.onClick.AddListener(delegate ()
        {
            this.showOrHideRule();
        });
        GameObject gameObject2 = this.Panel_SetGo.transform.Find("Panel_bottom/btn_reset").gameObject;
        Button component2 = gameObject2.GetComponent<Button>();
        component2.onClick.RemoveAllListeners();
        component2.onClick.AddListener(new UnityAction(this.resetSecondPwdInput));
        GameObject gameObject3 = this.Panel_SetGo.transform.Find("Panel_bottom/btn_finish").gameObject;
        this.setBtn = gameObject3.GetComponent<Button>();
        this.setBtn.onClick.RemoveAllListeners();
        this.setBtn.onClick.AddListener(new UnityAction(this.setSecondPwd));
        this.setBtn.interactable = false;
        GameObject gameObject4 = this.Panel_SetGo.transform.Find("Panel_input/txt_insert/InputField").gameObject;
        this.firstPwdInput = gameObject4.GetComponent<InputField>();
        this.firstPwdInput.onValueChanged.RemoveAllListeners();
        this.firstPwdInput.onValueChanged.AddListener(new UnityAction<string>(this.onInputPwdChange));
        gameObject4.AddComponent<InputNavigator>();
        gameObject4 = this.Panel_SetGo.transform.Find("Panel_input/txt_insert/txt_error").gameObject;
        this.firstPwdErrorShow = gameObject4.GetComponent<Text>();
        this.firstPwdErrorUIInformation = gameObject4.GetComponent<UIInformationList>();
        gameObject4 = this.Panel_SetGo.transform.Find("Panel_input/txt_insert/txt_adopt").gameObject;
        this.firstPwdPassShow = gameObject4.GetComponent<Text>();
        this.firstPwdPassShowUIInformation = gameObject4.GetComponent<UIInformationList>();
        gameObject4 = this.Panel_SetGo.transform.Find("Panel_input/txt_confirm/InputField").gameObject;
        this.secondPwdInput = gameObject4.GetComponent<InputField>();
        this.secondPwdInput.onValueChanged.RemoveAllListeners();
        this.secondPwdInput.onValueChanged.AddListener(new UnityAction<string>(this.onConfirmPwdChange));
        gameObject4 = this.Panel_SetGo.transform.Find("Panel_input/txt_confirm/txt_error").gameObject;
        this.secondPwdErrorShow = gameObject4.GetComponent<Text>();
        this.secondPwdErrorShowUIInformation = gameObject4.GetComponent<UIInformationList>();
        this.ruleGo = this.Panel_SetGo.transform.Find("Panel_rule").gameObject;
        this.Panel_SetGo.SetActive(this.control.ShowPage == SecondPwdControl.Second_PWD_Show_Page.PAGE_SET_SECOND_PWD);
        this.Panel_VerGo = this.Root.Find("Offset_Example/Panel_ver").gameObject;
        gameObject4 = this.Panel_VerGo.transform.Find("Panel_bottom/btn_confirm").gameObject;
        this.verifyBtn = gameObject4.GetComponent<Button>();
        this.verifyBtn.onClick.RemoveAllListeners();
        this.verifyBtn.onClick.AddListener(new UnityAction(this.verifyConfirm));
        this.verifyBtn.enabled = false;
        this.verifyErrorTip = this.Panel_VerGo.transform.Find("txt_error").gameObject.GetComponent<Text>();
        this.verifyErrorTip.gameObject.SetActive(false);
        this.verifyErrorTipUIInformation = this.verifyErrorTip.GetComponent<UIInformationList>();
        gameObject4 = this.Panel_VerGo.transform.Find("Panel_bottom/btn_reset").gameObject;
        Button component3 = gameObject4.GetComponent<Button>();
        component3.onClick.RemoveAllListeners();
        component3.onClick.AddListener(new UnityAction(this.verifyResetInput));
        gameObject4 = this.Panel_VerGo.transform.Find("btn_forget").gameObject;
        Button component4 = gameObject4.GetComponent<Button>();
        component4.onClick.RemoveAllListeners();
        component4.onClick.AddListener(new UnityAction(this.onClickForgetBtn));
        for (int i = 1; i < 7; i++)
        {
            this.inputPwdObj[i - 1] = this.Panel_VerGo.transform.Find("Panel_password/Image" + i.ToString() + "/Text").gameObject;
        }
        for (int j = 0; j < 10; j++)
        {
            gameObject4 = this.Panel_VerGo.transform.Find("Panel_keyboard/btn_num" + j.ToString()).gameObject;
            Button goBtn = gameObject4.GetComponent<Button>();
            goBtn.onClick.RemoveAllListeners();
            goBtn.onClick.AddListener(delegate ()
            {
                this.onKeyboardInput(goBtn.gameObject);
            });
            this.keybordText[j] = gameObject4.transform.Find("Text").gameObject.GetComponent<Text>();
        }
        if (this.control.ShowPage == SecondPwdControl.Second_PWD_Show_Page.PAGE_VERIFY_SECOND_PWD)
        {
            this.resetKeyboardData();
        }
        gameObject4 = this.Panel_VerGo.transform.Find("Panel_top/btn_close").gameObject;
        Button component5 = gameObject4.GetComponent<Button>();
        component5.onClick.RemoveAllListeners();
        component5.onClick.AddListener(delegate ()
        {
            this.close();
        });
        this.Panel_VerGo.SetActive(this.control.ShowPage == SecondPwdControl.Second_PWD_Show_Page.PAGE_VERIFY_SECOND_PWD);
        this.Panel_ChangeStep1Go = this.Root.Find("Offset_Example/Panel_changestep1").gameObject;
        this.Panel_ChangeStep1Go.SetActive(this.control.ShowPage == SecondPwdControl.Second_PWD_Show_Page.PAGE_RESET_SECOND_PWD);
        gameObject4 = this.Panel_ChangeStep1Go.transform.Find("Panel_bottom/btn_confirm").gameObject;
        this.resetVerifyBtn = gameObject4.GetComponent<Button>();
        this.resetVerifyBtn.onClick.RemoveAllListeners();
        this.resetVerifyBtn.onClick.AddListener(new UnityAction(this.verifyConfirmReset));
        this.resetVerifyBtn.interactable = false;
        this.resetVerifyErrorTip = this.Panel_ChangeStep1Go.transform.Find("txt_error").gameObject.GetComponent<Text>();
        this.resetVerifyErrorTip.gameObject.SetActive(false);
        this.resetVerifyErrorTipUIInformationReset = this.resetVerifyErrorTip.gameObject.GetComponent<UIInformationList>();
        gameObject4 = this.Panel_ChangeStep1Go.transform.Find("Panel_bottom/btn_reset").gameObject;
        Button component6 = gameObject4.GetComponent<Button>();
        component6.onClick.RemoveAllListeners();
        component6.onClick.AddListener(new UnityAction(this.resetKeyboardDataReset));
        gameObject4 = this.Panel_ChangeStep1Go.transform.Find("btn_forget").gameObject;
        Button component7 = gameObject4.GetComponent<Button>();
        component7.onClick.RemoveAllListeners();
        component7.onClick.AddListener(new UnityAction(this.onClickForgetBtnReset));
        for (int k = 1; k < 7; k++)
        {
            this.resetInputPwdObj[k - 1] = this.Panel_ChangeStep1Go.transform.Find("Panel_password/Image" + k.ToString() + "/Text").gameObject;
        }
        for (int l = 0; l < 10; l++)
        {
            gameObject4 = this.Panel_ChangeStep1Go.transform.Find("Panel_keyboard/btn_num" + l.ToString()).gameObject;
            Button goBtn = gameObject4.GetComponent<Button>();
            goBtn.onClick.RemoveAllListeners();
            goBtn.onClick.AddListener(delegate ()
            {
                this.onKeyboardInputReset(goBtn.gameObject);
            });
            this.resetKeybordText[l] = gameObject4.transform.Find("Text").gameObject.GetComponent<Text>();
        }
        if (this.control.ShowPage == SecondPwdControl.Second_PWD_Show_Page.PAGE_RESET_SECOND_PWD)
        {
            this.resetKeyboardDataReset();
        }
        gameObject4 = this.Panel_ChangeStep1Go.transform.Find("Panel_top/btn_close").gameObject;
        Button component8 = gameObject4.GetComponent<Button>();
        component8.onClick.RemoveAllListeners();
        component8.onClick.AddListener(delegate ()
        {
            this.close();
        });
        this.Panel_ChangeStep2Go = this.Root.Find("Offset_Example/Panel_changestep2").gameObject;
        gameObject4 = this.Panel_ChangeStep2Go.transform.Find("Panel_bottom/btn_reset").gameObject;
        Button component9 = gameObject4.GetComponent<Button>();
        component9.onClick.RemoveAllListeners();
        component9.onClick.AddListener(delegate ()
        {
            this.resetSecondPwdInputReset();
        });
        gameObject4 = this.Panel_ChangeStep2Go.transform.Find("Panel_bottom/btn_finish").gameObject;
        this.setBtnReset = gameObject4.GetComponent<Button>();
        this.setBtnReset.onClick.RemoveAllListeners();
        this.setBtnReset.onClick.AddListener(delegate ()
        {
            this.setSecondPwdReset();
        });
        this.setBtnReset.interactable = false;
        gameObject4 = this.Panel_ChangeStep2Go.transform.Find("Panel_input/txt_insert/InputField").gameObject;
        this.firstPwdInputReset = gameObject4.GetComponent<InputField>();
        this.firstPwdInputReset.onValueChanged.RemoveAllListeners();
        this.firstPwdInputReset.onValueChanged.AddListener(new UnityAction<string>(this.onInputPwdChangeReset));
        gameObject4 = this.Panel_ChangeStep2Go.transform.Find("Panel_input/txt_insert/txt_error").gameObject;
        this.firstPwdErrorShowReset = gameObject4.GetComponent<Text>();
        this.firstPwdErrorUIInformationReset = gameObject4.GetComponent<UIInformationList>();
        gameObject4 = this.Panel_ChangeStep2Go.transform.Find("Panel_input/txt_insert/txt_adopt").gameObject;
        this.firstPwdPassShowReset = gameObject4.GetComponent<Text>();
        this.firstPwdPassShowUIInformationReset = gameObject4.GetComponent<UIInformationList>();
        gameObject4 = this.Panel_ChangeStep2Go.transform.Find("Panel_input/txt_confirm/InputField").gameObject;
        this.secondPwdInputReset = gameObject4.GetComponent<InputField>();
        this.secondPwdInputReset.onValueChanged.RemoveAllListeners();
        this.secondPwdInputReset.onValueChanged.AddListener(new UnityAction<string>(this.onConfirmPwdChangeReset));
        gameObject4 = this.Panel_ChangeStep2Go.transform.Find("Panel_input/txt_confirm/txt_error").gameObject;
        this.secondPwdErrorShowReset = gameObject4.GetComponent<Text>();
        this.secondPwdErrorShowUIInformationReset = gameObject4.GetComponent<UIInformationList>();
        gameObject4 = this.Panel_ChangeStep2Go.transform.Find("Panel_top/btn_close").gameObject;
        Button component10 = gameObject4.GetComponent<Button>();
        component10.onClick.RemoveAllListeners();
        component10.onClick.AddListener(delegate ()
        {
            this.close();
        });
        this.Panel_ChangeStep2Go.SetActive(false);
    }

    private void onInputPwdChange(string pwd)
    {
        this.control.onInputPwdChanged(pwd);
    }

    private void onConfirmPwdChange(string pwd)
    {
        this.control.onConfirmPwdChanged(this.firstPwdInput.text, pwd);
    }

    private void showOrHideRule()
    {
        this.ruleGo.SetActive(!this.ruleGo.activeSelf);
    }

    private void resetSecondPwdInput()
    {
        this.firstPwdInput.text = string.Empty;
        this.secondPwdInput.text = string.Empty;
        this.firstPwdErrorShow.gameObject.SetActive(false);
        this.firstPwdPassShow.gameObject.SetActive(false);
        this.secondPwdErrorShow.gameObject.SetActive(false);
    }

    private void setSecondPwd()
    {
        this.control.ReqSetSecondPwd(this.firstPwdInput.text);
    }

    public void showFirstInputErrorTips(int index)
    {
        this.firstPwdErrorShow.text = this.firstPwdErrorUIInformation.listInformation[index].content;
        this.firstPwdErrorShow.gameObject.SetActive(true);
        this.firstPwdPassShow.gameObject.SetActive(false);
    }

    public void showFirstInputSuccessTips()
    {
        this.firstPwdErrorShow.gameObject.SetActive(false);
        this.firstPwdPassShow.gameObject.SetActive(true);
    }

    public void showConfirmInputTips(bool error)
    {
        this.secondPwdErrorShow.gameObject.SetActive(error);
        this.setBtn.interactable = !error;
    }

    private void resetKeyboardData()
    {
        this.currInputPwd = string.Empty;
        this.currInputIndex = 0;
        for (int i = 0; i < 6; i++)
        {
            this.inputPwdObj[i].SetActive(false);
        }
        this.generateRand(this.keybordText, 10);
        this.verifyBtn.enabled = false;
    }

    private void generateRand(Text[] txtobj, int range)
    {
        List<int> list = new List<int>();
        for (int i = 0; i < range; i++)
        {
            list.Add(i);
        }
        System.Random random = new System.Random();
        int num = 0;
        while (list.Count > 0)
        {
            int index = random.Next(0, list.Count);
            txtobj[num++].text = list[index].ToString();
            list.RemoveAt(index);
        }
    }

    public void OnVerifyError(int errorindex)
    {
        this.resetKeyboardData();
        this.ShowVerifyError(errorindex);
    }

    public void ShowVerifyError(int errorindex)
    {
        this.verifyErrorTip.text = this.verifyErrorTipUIInformation.listInformation[errorindex].content;
        this.verifyErrorTip.gameObject.SetActive(true);
    }

    private void onClickForgetBtn()
    {
    }

    private void verifyResetInput()
    {
        this.resetKeyboardData();
    }

    private void verifyConfirm()
    {
        this.control.ReqVerifySecondPwd(this.currInputPwd);
    }

    private void onKeyboardInput(GameObject go)
    {
        if (this.currInputIndex >= 6)
        {
            return;
        }
        Text component = go.transform.Find("Text").GetComponent<Text>();
        this.currInputIndex++;
        this.inputPwdObj[this.currInputIndex - 1].SetActive(true);
        this.currInputPwd += component.text.ToString();
        this.verifyBtn.enabled = (this.currInputIndex >= 6);
        this.verifyErrorTip.gameObject.SetActive(false);
    }

    private void resetKeyboardDataReset()
    {
        this.resetCurrInputPwd = string.Empty;
        this.resetCurrInputIndex = 0;
        for (int i = 0; i < 6; i++)
        {
            this.resetInputPwdObj[i].SetActive(false);
        }
        this.generateRand(this.resetKeybordText, 10);
        this.resetVerifyBtn.interactable = false;
    }

    public void ShowVerifyReset(int errorIndex, bool success)
    {
        if (success)
        {
            this.Panel_ChangeStep1Go.SetActive(false);
            this.Panel_ChangeStep2Go.SetActive(true);
        }
        else
        {
            this.resetVerifyErrorTip.gameObject.SetActive(true);
            this.resetVerifyErrorTip.text = this.resetVerifyErrorTipUIInformationReset.listInformation[errorIndex].content;
            this.resetKeyboardDataReset();
        }
    }

    private void onClickForgetBtnReset()
    {
    }

    private void verifyResetInputReset()
    {
        this.resetKeyboardDataReset();
    }

    private void verifyConfirmReset()
    {
        this.control.ReqVerifySecondPwdReset(this.resetCurrInputPwd);
    }

    private void onKeyboardInputReset(GameObject go)
    {
        if (this.resetCurrInputIndex >= 6)
        {
            return;
        }
        Text component = go.transform.Find("Text").GetComponent<Text>();
        this.resetCurrInputIndex++;
        this.resetInputPwdObj[this.resetCurrInputIndex - 1].SetActive(true);
        this.resetCurrInputPwd += component.text.ToString();
        this.resetVerifyBtn.interactable = (this.resetCurrInputIndex >= 6);
        this.resetVerifyErrorTip.gameObject.SetActive(false);
    }

    private void resetSecondPwdInputReset()
    {
        this.firstPwdInputReset.text = string.Empty;
        this.secondPwdInputReset.text = string.Empty;
        this.firstPwdErrorShowReset.gameObject.SetActive(false);
        this.firstPwdPassShowReset.gameObject.SetActive(false);
        this.secondPwdErrorShowReset.gameObject.SetActive(false);
    }

    private void setSecondPwdReset()
    {
        this.control.ReqReSetSecondPwd(this.firstPwdInputReset.text);
    }

    private void onInputPwdChangeReset(string pwd)
    {
        this.control.onInputResetPwdChanged(pwd);
    }

    public void showFirstInputErrorTipsReset(int index)
    {
        this.firstPwdErrorShowReset.text = this.firstPwdErrorUIInformation.listInformation[index].content;
        this.firstPwdErrorShowReset.gameObject.SetActive(true);
        this.firstPwdPassShowReset.gameObject.SetActive(false);
    }

    public void showConfirmInputTipsReset(bool error)
    {
        this.secondPwdErrorShowReset.gameObject.SetActive(error);
        this.setBtnReset.interactable = !error;
    }

    public void showFirstInputSuccessTipsReset()
    {
        this.firstPwdErrorShowReset.gameObject.SetActive(false);
        this.firstPwdPassShowReset.gameObject.SetActive(true);
    }

    private void onConfirmPwdChangeReset(string pwd)
    {
        this.control.onConfirmPwdChangedReset(this.firstPwdInputReset.text, pwd);
    }

    private void Update()
    {
        if (this.control.ShowPage == SecondPwdControl.Second_PWD_Show_Page.PAGE_VERIFY_SECOND_PWD && Input.anyKeyDown && !Input.GetMouseButton(0) && !Input.GetMouseButton(1))
        {
            this.ShowVerifyError(0);
        }
    }

    public void close()
    {
        ManagerCenter.Instance.GetManager<UIManager>().DeleteUI("UI_SecondPassword");
    }

    public override void OnDispose()
    {
        Scheduler.Instance.RemoveUpdator(new Scheduler.OnScheduler(this.Update));
    }

    public Transform Root;

    private GameObject Panel_SetGo;

    private Text firstPwdErrorShow;

    private UIInformationList firstPwdErrorUIInformation;

    private Text firstPwdPassShow;

    private UIInformationList firstPwdPassShowUIInformation;

    private InputField firstPwdInput;

    private Text secondPwdErrorShow;

    private UIInformationList secondPwdErrorShowUIInformation;

    private Text secondPwdPassShow;

    private InputField secondPwdInput;

    private GameObject ruleGo;

    private Button setBtn;

    private GameObject Panel_VerGo;

    private GameObject[] inputPwdObj = new GameObject[6];

    private Text[] keybordText = new Text[10];

    private Button verifyBtn;

    private Text verifyErrorTip;

    private UIInformationList verifyErrorTipUIInformation;

    private int currInputIndex;

    private string currInputPwd = string.Empty;

    private GameObject Panel_ChangeStep1Go;

    private GameObject[] resetInputPwdObj = new GameObject[6];

    private Text[] resetKeybordText = new Text[10];

    private Button resetVerifyBtn;

    private Text resetVerifyErrorTip;

    private UIInformationList resetVerifyErrorTipUIInformationReset;

    private int resetCurrInputIndex;

    private string resetCurrInputPwd = string.Empty;

    private GameObject Panel_ChangeStep2Go;

    private Text firstPwdErrorShowReset;

    private UIInformationList firstPwdErrorUIInformationReset;

    private Text firstPwdPassShowReset;

    private UIInformationList firstPwdPassShowUIInformationReset;

    private InputField firstPwdInputReset;

    private Text secondPwdErrorShowReset;

    private UIInformationList secondPwdErrorShowUIInformationReset;

    private Text secondPwdPassShowReset;

    private InputField secondPwdInputReset;

    private Button setBtnReset;

    private SecondPwdControl control;
}
