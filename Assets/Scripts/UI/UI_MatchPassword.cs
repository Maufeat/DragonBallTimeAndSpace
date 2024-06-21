using System;
using System.Collections.Generic;
using Framework.Managers;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_MatchPassword : UIPanelBase
{
    private MatchPasswordController mpc
    {
        get
        {
            if (this.mpc_ == null)
            {
                this.mpc_ = ControllerManager.Instance.GetController<MatchPasswordController>();
            }
            return this.mpc_;
        }
    }

    public override void OnInit(Transform root)
    {
        base.OnInit(root);
        this.curPsl.Clear();
        this.InitObj(root);
        this.InitEvent();
    }

    private void InitObj(Transform root)
    {
        this.ui_root = root;
        this.btnTr = this.ui_root.Find("Panel_offset/btn_close");
        this.showInputPanel = this.ui_root.Find("Panel_offset/Panel_show_input");
        this.needInputPanel = this.ui_root.Find("Panel_offset/Panel_need_input");
        this.inputPanel = this.ui_root.Find("Panel_offset/Scroll View_num");
        this.tipBtn = this.ui_root.Find("Panel_offset/btn_pw");
    }

    private void InitEvent()
    {
        this.maxPasswordCount = this.showInputPanel.childCount;
        this.SetCurPassword(this.curPsl);
        this.RandomNeedInput();
        this.InitInputNumEvent();
        Button component = this.btnTr.gameObject.GetComponent<Button>();
        component.onClick.RemoveAllListeners();
        component.onClick.AddListener(new UnityAction(this.Close));
        HoverEventListener.Get(this.tipBtn.gameObject).onEnter = delegate (PointerEventData pd)
        {
            this.TipsSwitch(true);
        };
        HoverEventListener.Get(this.tipBtn.gameObject).onExit = delegate (PointerEventData pd)
        {
            this.TipsSwitch(false);
        };
    }

    private void RandomNeedInput()
    {
        List<int> list = this.mpc.RandomInputPassWord(this.maxPasswordCount);
        for (int i = 0; i < this.needInputPanel.childCount; i++)
        {
            Transform child = this.needInputPanel.GetChild(i);
            Text component = child.Find("txt_num").GetComponent<Text>();
            component.text = string.Empty;
            if (i < list.Count)
            {
                component.text = list[i].ToString();
            }
        }
    }

    private void TipsSwitch(bool state)
    {
        this.needInputPanel.gameObject.SetActive(state);
    }

    private void SetCurPassword(List<int> psl)
    {
        for (int i = 0; i < this.maxPasswordCount; i++)
        {
            Transform transform = this.showInputPanel.transform.Find("img_bg" + (i + 1));
            if (transform != null)
            {
                transform.Find("txt_num").GetComponent<Text>().text = ((i >= psl.Count) ? "_" : psl[i].ToString());
            }
        }
    }

    private void InitInputNumEvent()
    {
        Transform transform = this.inputPanel.Find("Viewport/Content");
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            Button component = child.gameObject.GetComponent<Button>();
            component.onClick.RemoveAllListeners();
            int btnIndex = i;
            if (btnIndex == 9)
            {
                component.onClick.AddListener(new UnityAction(this.InputCancel));
            }
            else if (btnIndex == 10)
            {
                component.onClick.AddListener(delegate ()
                {
                    this.InputOneNum(0);
                });
            }
            else if (btnIndex == 11)
            {
                component.onClick.AddListener(new UnityAction(this.InputEnsure));
            }
            else
            {
                component.onClick.AddListener(delegate ()
                {
                    this.InputOneNum(btnIndex + 1);
                });
            }
        }
    }

    private void InputOneNum(int i)
    {
        if (this.curPsl.Count < this.maxPasswordCount)
        {
            this.curPsl.Add(i);
        }
        else
        {
            this.curPsl[this.curPsl.Count - 1] = i;
        }
        this.SetCurPassword(this.curPsl);
    }

    private void InputCancel()
    {
        this.curPsl.Clear();
        this.SetCurPassword(this.curPsl);
    }

    private void InputEnsure()
    {
        if (this.mpc.OnEnsure(this.curPsl))
        {
            this.Close();
        }
        else
        {
            TipsWindow.ShowNotice("不对，重新输入");
            this.curPsl.Clear();
            this.SetCurPassword(this.curPsl);
        }
    }

    private void Close()
    {
        UIManager.Instance.DeleteUI<UI_MatchPassword>();
    }

    private Transform ui_root;

    private GeneController gc;

    private Transform btnTr;

    private Transform showInputPanel;

    private Transform needInputPanel;

    private Transform inputPanel;

    private Transform tipBtn;

    private int maxPasswordCount;

    private List<int> curPsl = new List<int>();

    private MatchPasswordController mpc_;
}
