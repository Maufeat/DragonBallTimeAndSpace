using System;
using System.Collections.Generic;
using Framework.Managers;
using LuaInterface;
using Obj;
using quiz;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_SevenDays : UIPanelBase
{
    public override void OnInit(Transform root)
    {
        base.OnInit(root);
        this.controller = ControllerManager.Instance.GetController<SevenDaysController>();
        this.InitObj(root);
        this.InitEvent();
        this.InitInfo();
    }

    private void InitObj(Transform root)
    {
        this.ui_root = root;
        this.panelRoot = root.Find("Offset/Panel_root");
        this.btnClose = this.panelRoot.Find("img_title/Button_close").GetComponent<Button>();
        this.btnGetAll = this.panelRoot.Find("btn_getall").GetComponent<Button>();
        for (int i = 0; i < this.DAY_NUM; i++)
        {
            Transform item = this.panelRoot.Find("Panel_charge/Image_" + (i + 1));
            this.itemList.Add(item);
        }
    }

    private void InitEvent()
    {
        this.btnClose.onClick.RemoveAllListeners();
        this.btnClose.onClick.AddListener(new UnityAction(this.OnCloseButtonClick));
        this.btnGetAll.onClick.RemoveAllListeners();
        this.btnGetAll.onClick.AddListener(new UnityAction(this.OnGetAllButtonClick));
    }

    private void OnCloseButtonClick()
    {
        this.CloseUI();
    }

    private void OnGetAllButtonClick()
    {
        this.controller.mNetWork.Req_RecvDay7ActivityPrize_CS();
    }

    private void CloseUI()
    {
        this.controller.CloseSevenDaysUI();
    }

    public void InitInfo()
    {
        if (this.controller.SevenInfo.data == null)
        {
            return;
        }
        this.btnGetAll.gameObject.SetActive(false);
        for (int i = 0; i < this.controller.SevenInfo.data.Count; i++)
        {
            this.SetActivityState(this.itemList[i], i);
            string cacheField_String = this.controller.SevenDaysConfig[i].GetCacheField_String("targetdes1");
            string cacheField_String2 = this.controller.SevenDaysConfig[i].GetCacheField_String("targetdes2");
            this.itemList[i].Find("Panel_desc/text").GetComponent<Text>().text = cacheField_String;
            this.itemList[i].Find("Panel_desc/text (1)").GetComponent<Text>().text = cacheField_String2;
            string cacheField_String3 = this.controller.SevenDaysConfig[i].GetCacheField_String("reward");
            string[] array = cacheField_String3.Split(new char[]
            {
                '|'
            });
            for (int j = 0; j < 3; j++)
            {
                GameObject item = this.itemList[i].Find("Panel_awards/item_" + (j + 1) + "/icon").gameObject;
                if (j >= array.Length)
                {
                    item.transform.parent.gameObject.SetActive(false);
                }
                else
                {
                    string[] array2 = array[j].Split(new char[]
                    {
                        '-'
                    });
                    string rewardId = array2[0];
                    string text = array2[1];
                    string text2 = array2[2];
                    this.itemList[i].Find("Panel_awards/item_" + (j + 1) + "/Text_count").GetComponent<Text>().text = text;
                    this.itemList[i].Find("Panel_awards/item_" + (j + 1) + "/quality").gameObject.SetActive(false);
                    LuaTable configTable = LuaConfigManager.GetConfigTable("objects", (ulong)uint.Parse(rewardId));
                    GlobalRegister.SetImage(0, configTable.GetField_String("icon"), item.GetComponent<Image>(), true);
                    UIEventListener.Get(item.transform.parent.gameObject).onEnter = delegate (PointerEventData ed)
                    {
                        ControllerManager.Instance.GetController<ItemTipController>().OpenPanel(new t_Object
                        {
                            baseid = uint.Parse(rewardId)
                        }, item.transform.parent.gameObject);
                    };
                    UIEventListener.Get(item.transform.parent.gameObject).onExit = delegate (PointerEventData ed)
                    {
                        ControllerManager.Instance.GetController<ItemTipController>().ClosePanel();
                    };
                }
            }
        }
        this.controller.MainView.EnableSevenDays(this.controller.SevenInfo);
    }

    private void SetActivityState(Transform item, int index)
    {
        item.Find("Image").gameObject.SetActive(false);
        item.Find("txt_2").gameObject.SetActive(false);
        item.Find("txt_3").gameObject.SetActive(false);
        item.Find("txt_4").gameObject.SetActive(false);
        switch (this.controller.SevenInfo.data[index].state)
        {
            case ActivityState.ACTIVITY_STATE_OPEN:
                item.Find("Image").gameObject.SetActive(true);
                break;
            case ActivityState.ACTIVITY_STATE_COMPLETE:
                item.Find("txt_2").gameObject.SetActive(true);
                this.btnGetAll.gameObject.SetActive(true);
                break;
            case ActivityState.ACTIVITY_STATE_GOTPRIZE:
                item.Find("txt_3").gameObject.SetActive(true);
                break;
            case ActivityState.ACTIVITY_STATE_UNCOMPLETE:
                item.Find("txt_4").gameObject.SetActive(true);
                break;
        }
    }

    public override void OnDispose()
    {
        base.OnDispose();
    }

    private Transform ui_root;

    private SevenDaysController controller;

    private Transform panelRoot;

    private Button btnClose;

    private Button btnGetAll;

    private List<Transform> itemList = new List<Transform>();

    private int DAY_NUM = 7;
}
