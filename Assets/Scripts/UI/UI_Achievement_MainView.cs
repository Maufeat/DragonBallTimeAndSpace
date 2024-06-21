using System;
using Framework.Managers;
using LuaInterface;
using Obj;
using quiz;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Achievement_MainView
{
    public void Init(Transform mainviewroot)
    {
        this.controller = ControllerManager.Instance.GetController<SevenDaysController>();
        this.txtInfo = mainviewroot.Find("Offset_Main/Panel_achievement/Text").GetComponent<Text>();
        this.txtNum = mainviewroot.Find("Offset_Main/Panel_achievement/Image/Item/txt_count").GetComponent<Text>();
        this.imgItem = mainviewroot.Find("Offset_Main/Panel_achievement/Image/Item/img_bg").GetComponent<Image>();
        this.imgQuality = mainviewroot.Find("Offset_Main/Panel_achievement/Image/Item/quality").GetComponent<RawImage>();
        this.btnGet = mainviewroot.Find("Offset_Main/Panel_achievement/btn_get").GetComponent<Button>();
        mainviewroot.Find("Offset_Main/Panel_achievement/Image/Item/img_icon").gameObject.SetActive(false);
        this.btnGet.onClick.RemoveAllListeners();
        this.btnGet.onClick.AddListener(new UnityAction(this.OnGetButtonClick));
        this.imgItem.gameObject.SetActive(true);
        this.imgQuality.gameObject.SetActive(false);
    }

    public void EnableAchievement(Transform root)
    {
        this._root = root.Find("Offset_Main/Panel_achievement");
        this._root.gameObject.SetActive(true);
        this.InitInfo(this.controller.SeekInfo);
    }

    public void InitInfo(MSG_Ret_SeekActivityInfo_SC info)
    {
        if (info.id == 0U)
        {
            this._root.gameObject.SetActive(false);
            return;
        }
        this.achievemengtId = info.id;
        this.growtargetConfig = LuaConfigManager.GetConfigTable("growtarget_config", (ulong)this.achievemengtId);
        this.txtInfo.text = this.growtargetConfig.GetField_String("targetdes");
        string[] array = this.growtargetConfig.GetField_String("reward").Split(new char[]
        {
            '|'
        });
        string rewardId = array[0].Split(new char[]
        {
            '-'
        })[0];
        string text = array[0].Split(new char[]
        {
            '-'
        })[1];
        string text2 = array[0].Split(new char[]
        {
            '-'
        })[2];
        this.txtNum.text = text;
        LuaTable configTable = LuaConfigManager.GetConfigTable("objects", (ulong)uint.Parse(rewardId));
        GlobalRegister.SetImage(0, configTable.GetField_String("icon"), this.imgItem.GetComponent<Image>(), true);
        UIEventListener.Get(this.imgItem.transform.parent.parent.gameObject).onEnter = delegate (PointerEventData ed)
        {
            ControllerManager.Instance.GetController<ItemTipController>().OpenPanel(new t_Object
            {
                baseid = uint.Parse(rewardId)
            }, this.imgItem.transform.parent.parent.gameObject);
        };
        UIEventListener.Get(this.imgItem.transform.parent.parent.gameObject).onExit = delegate (PointerEventData ed)
        {
            ControllerManager.Instance.GetController<ItemTipController>().ClosePanel();
        };
        this.btnGet.gameObject.SetActive(false);
        switch (info.state)
        {
            case ActivityState.ACTIVITY_STATE_OPEN:
                this._root.gameObject.SetActive(true);
                break;
            case ActivityState.ACTIVITY_STATE_COMPLETE:
                this._root.gameObject.SetActive(true);
                this.btnGet.gameObject.SetActive(true);
                break;
            case ActivityState.ACTIVITY_STATE_GOTPRIZE:
                this._root.gameObject.SetActive(false);
                break;
            default:
                Debug.Log(info.state);
                break;
        }
    }

    private void OnGetButtonClick()
    {
        this.controller.mNetWork.Req_RecvSeekActivityPrize_CS(this.achievemengtId);
    }

    public void ShowAchievement(bool show)
    {
        if (this._root != null)
        {
            this._root.gameObject.SetActive(show);
        }
    }

    private SevenDaysController controller;

    private LuaTable growtargetConfig;

    private Transform _root;

    private uint achievemengtId;

    private Text txtInfo;

    private Text txtNum;

    private Image imgItem;

    private RawImage imgQuality;

    private Button btnGet;
}
