using System;
using System.Collections.Generic;
using Framework.Managers;
using Team;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UI_PublicDrop : UIPanelBase
{
    private TeamController teamController
    {
        get
        {
            return ControllerManager.Instance.GetController<TeamController>();
        }
    }

    public override void OnDispose()
    {
        base.Dispose();
        this.ClearItems();
        UserSysSettingController.onSysSettingChanged = (UserSysSettingController.OnSysSettingChangedHandle)Delegate.Remove(UserSysSettingController.onSysSettingChanged, new UserSysSettingController.OnSysSettingChangedHandle(this.RefreshSetting));
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
        this.btn_close = root.transform.Find("Offset_TeamAssign/Panel_Roll/background/btn_close").GetComponent<Button>();
        this.btn_set = root.transform.Find("Offset_TeamAssign/Panel_Roll/btn_set").GetComponent<Button>();
        this.btn_setclose = root.transform.Find("Offset_TeamAssign/Panel_Roll/btn_close").GetComponent<Button>();
        this.obj_itemprefab = root.transform.Find("Offset_TeamAssign/Panel_Roll/list/Rect/Item").gameObject;
        this.obj_set = root.transform.Find("Offset_TeamAssign/Panel_Roll/set").gameObject;
        this.tg_white = root.transform.Find("Offset_TeamAssign/Panel_Roll/set/choose/toggle_1").GetComponent<Button>();
        this.tg_green = root.transform.Find("Offset_TeamAssign/Panel_Roll/set/choose/toggle_2").GetComponent<Button>();
        this.tg_blue = root.transform.Find("Offset_TeamAssign/Panel_Roll/set/choose/toggle_3").GetComponent<Button>();
        this.obj_set.SetActive(false);
        this.obj_itemprefab.SetActive(false);
        this.btn_set.gameObject.SetActive(false);
        this.btn_setclose.gameObject.SetActive(false);
        this.RefreshSetting();
    }

    private void InitEvent()
    {
        this.btn_close.onClick.RemoveAllListeners();
        this.btn_close.onClick.AddListener(new UnityAction(this.TempClose));
        this.btn_set.onClick.RemoveAllListeners();
        this.btn_set.onClick.AddListener(new UnityAction(this.ShowSet));
        this.btn_setclose.onClick.RemoveAllListeners();
        this.btn_setclose.onClick.AddListener(new UnityAction(this.HideSet));
        this.tg_white.onClick.RemoveAllListeners();
        this.tg_white.onClick.AddListener(delegate ()
        {
            ControllerManager.Instance.GetController<UserSysSettingController>().ReqSysSetting(SYSSETTING.SETTING_ROLL_WHITE, !this.teamController.IgnoreWhite);
        });
        this.tg_green.onClick.RemoveAllListeners();
        this.tg_green.onClick.AddListener(delegate ()
        {
            ControllerManager.Instance.GetController<UserSysSettingController>().ReqSysSetting(SYSSETTING.SETTING_ROLL_GREEN, !this.teamController.IgnoreGreen);
        });
        this.tg_blue.onClick.RemoveAllListeners();
        this.tg_blue.onClick.AddListener(delegate ()
        {
            ControllerManager.Instance.GetController<UserSysSettingController>().ReqSysSetting(SYSSETTING.SETTING_ROLL_BLUE, !this.teamController.IgnoreBlue);
        });
        UserSysSettingController.onSysSettingChanged = (UserSysSettingController.OnSysSettingChangedHandle)Delegate.Combine(UserSysSettingController.onSysSettingChanged, new UserSysSettingController.OnSysSettingChangedHandle(this.RefreshSetting));
    }

    public bool isViewOn
    {
        get
        {
            return this.root.gameObject.activeSelf;
        }
    }

    public void RefreshSetting()
    {
        this.tg_white.transform.Find("Background/Checkmark").gameObject.SetActive(this.teamController.IgnoreWhite);
        this.tg_green.transform.Find("Background/Checkmark").gameObject.SetActive(this.teamController.IgnoreGreen);
        this.tg_blue.transform.Find("Background/Checkmark").gameObject.SetActive(this.teamController.IgnoreBlue);
    }

    private void TempClose()
    {
        this.root.gameObject.SetActive(false);
        this.teamController.CheckShowPublicDropOnMain();
    }

    public void ShowThis()
    {
        this.root.gameObject.SetActive(true);
        this.teamController.CheckShowPublicDropOnMain();
    }

    private void ShowSet()
    {
        this.btn_set.gameObject.SetActive(false);
        this.btn_setclose.gameObject.SetActive(true);
        this.obj_set.SetActive(true);
    }

    private void HideSet()
    {
        this.btn_set.gameObject.SetActive(true);
        this.btn_setclose.gameObject.SetActive(false);
        this.obj_set.SetActive(false);
    }

    public void InitView()
    {
        this.teamController.DropItems.BetterForeach(delegate (KeyValuePair<ulong, teamDropItem> pair)
        {
            if (!this.mapItems.ContainsKey(pair.Key))
            {
                TeamDropItem value = new TeamDropItem(pair.Value, this.obj_itemprefab, delegate ()
                {
                    this.teamController.ProcessChooseTeamDrop(pair.Key.ToString());
                });
                this.mapItems.Add(ulong.Parse(pair.Value.thisid), value);
            }
        });
    }

    public void OnProcessItem(ulong id)
    {
        if (this.mapItems.ContainsKey(id))
        {
            TeamDropItem teamDropItem = this.mapItems[id];
            this.mapItems.Remove(id);
            teamDropItem.DisposeThis();
        }
    }

    public void ClearItems()
    {
        this.mapItems.BetterForeach(delegate (KeyValuePair<ulong, TeamDropItem> pair)
        {
            pair.Value.DisposeThis();
        });
        this.mapItems.Clear();
    }

    private Transform root;

    private Button btn_close;

    private Button btn_set;

    private Button btn_setclose;

    private GameObject obj_itemprefab;

    private GameObject obj_set;

    private Button tg_white;

    private Button tg_green;

    private Button tg_blue;

    private BetterDictionary<ulong, TeamDropItem> mapItems = new BetterDictionary<ulong, TeamDropItem>();

    private Material matoutline;
}
