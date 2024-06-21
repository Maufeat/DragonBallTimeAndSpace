using System;
using System.Collections.Generic;
using Chat;
using Framework.Managers;
using Game.Scene;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_GM : UIPanelBase
{
    public void Initilize(GMToolManager _tool)
    {
        this.toolManager = _tool;
    }

    public override void OnInit(Transform root)
    {
        this.lc = ControllerManager.Instance.GetController<LocalGMController>();
        if (this.lc.cachedcommand.Count > 0)
        {
            this.quickInputOrderIndex = this.lc.cachedcommand.Count - 1;
        }
        else
        {
            this.quickInputOrderIndex = 0;
        }
        GameScene manager = ManagerCenter.Instance.GetManager<GameScene>();
        uint num = manager.CurrentSceneData.mapID();
        int currentLineID = manager.CurrentLineID;
        this.commandDic.Add("AddItem1Button", "//fetch id=1 num=10000");
        this.commandDic.Add("AddItem2Button", "//fetch id=2 num=10000");
        this.commandDic.Add("AddItem3Button", "//fetch id=3 num=10000");
        this.commandDic.Add("AddItem4Button", "//fetch id=4 num=10000");
        this.commandDic.Add("AddItem201Button", "//fetch id=90000 num=100");
        this.commandDic.Add("PassBirthButton", "//passbirth");
        this.commandDic.Add("GoSceneButton", "//gomap id=610 pos=800,800 line=" + currentLineID);
        this.commandDic.Add("GoMapPosButton", string.Concat(new object[]
        {
            "//gomap id=",
            num,
            " pos=800,800 line=",
            currentLineID
        }));
        Button[] componentsInChildren = root.GetComponentsInChildren<Button>();
        for (int i = 0; i < componentsInChildren.Length; i++)
        {
            UIEventListener.Get(componentsInChildren[i].gameObject).onClick = new UIEventListener.VoidDelegate(this.btn_gm_on_click);
        }
        this.lbl_command = root.Find("InputField").GetComponent<InputField>();
        this.OnInitAutoAttack(root);
        base.OnInit(root);
        Scheduler.Instance.AddUpdator(new Scheduler.OnScheduler(this.Update));
    }

    private void btn_gm_on_click(PointerEventData eventdata)
    {
        string name = eventdata.pointerPress.name;
        if (this.commandDic.ContainsKey(name))
        {
            this.lbl_command.text = this.commandDic[name];
        }
        else if (name == "SubmitButton")
        {
            if (this.lbl_command.text != string.Empty)
            {
                ControllerManager.Instance.GetController<ChatControl>().SendChat(ChannelType.ChannelType_GmTool, this.lbl_command.text, 0U, null);
                this.lbl_command.text = string.Empty;
            }
            UIManager.Instance.DeleteUI<UI_GM>();
            this.toolManager.isGMToolOpen = false;
        }
    }

    public override void OnDispose()
    {
        Scheduler.Instance.RemoveUpdator(new Scheduler.OnScheduler(this.Update));
        base.OnDispose();
    }

    public void InputFieldActive(bool isSend = false)
    {
        if (isSend)
        {
            if (this.lbl_command.text != string.Empty)
            {
                ControllerManager.Instance.GetController<ChatControl>().SendChat(ChannelType.ChannelType_World, this.lbl_command.text, 0U, null);
                this.lc.AddCachedCommand(this.lbl_command.text);
                this.lbl_command.text = string.Empty;
                UIManager.Instance.DeleteUI<UI_GM>();
                if (this.lc.cachedcommand.Count > 0)
                {
                    this.quickInputOrderIndex = this.lc.cachedcommand.Count - 1;
                }
                else
                {
                    this.quickInputOrderIndex = 0;
                }
                this.toolManager.isGMToolOpen = false;
            }
            EventSystem.current.SetSelectedGameObject(null);
        }
        else
        {
            this.lbl_command.ActivateInputField();
        }
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            this.quickInputOrderIndex = Mathf.Clamp(this.quickInputOrderIndex, 0, this.lc.cachedcommand.Count - 1);
            this.SetOrder(this.quickInputOrderIndex);
            this.quickInputOrderIndex++;
            if (this.quickInputOrderIndex >= this.lc.cachedcommand.Count)
            {
                this.quickInputOrderIndex = 0;
            }
        }
        if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            this.quickInputOrderIndex = Mathf.Clamp(this.quickInputOrderIndex, 0, this.lc.cachedcommand.Count - 1);
            this.SetOrder(this.quickInputOrderIndex);
            this.quickInputOrderIndex--;
            if (this.quickInputOrderIndex < 0)
            {
                this.quickInputOrderIndex = this.lc.cachedcommand.Count - 1;
            }
        }
    }

    private void SetOrder(int index)
    {
        if (this.lbl_command.isFocused && this.lc != null && this.lc.cachedcommand.Count > 0)
        {
            this.lbl_command.text = this.lc.cachedcommand[index];
        }
    }

    private void OnInitAutoAttack(Transform root)
    {
        UIEventListener.Get(root.Find("AddItem1000Button").gameObject).onClick = new UIEventListener.VoidDelegate(this.OpenOrCloseAutoAttack);
        this.txt_close = root.Find("AddItem1000Button/close").GetComponent<Text>();
        this.txt_open = root.Find("AddItem1000Button/open").GetComponent<Text>();
        this.txt_close.gameObject.SetActive(!this.lc.IsOpen);
        this.txt_open.gameObject.SetActive(this.lc.IsOpen);
    }

    public void OpenOrCloseAutoAttack(PointerEventData eventdata)
    {
        this.lc.SetAutoAttackState();
        if (this.txt_close != null)
        {
            this.txt_close.gameObject.SetActive(!this.lc.IsOpen);
        }
        if (this.txt_open != null)
        {
            this.txt_open.gameObject.SetActive(this.lc.IsOpen);
        }
    }

    private InputField lbl_command;

    public Dictionary<string, string> commandDic = new Dictionary<string, string>();

    public GMToolManager toolManager;

    private LocalGMController lc;

    private int quickInputOrderIndex;

    private Text txt_open;

    private Text txt_close;
}
