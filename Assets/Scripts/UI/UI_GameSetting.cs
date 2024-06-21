using System;
using System.Collections.Generic;
using AudioStudio;
using Engine;
using Framework.Managers;
using LuaInterface;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_GameSetting : UIPanelBase
{
    private SystemSettingController settingControl
    {
        get
        {
            return ControllerManager.Instance.GetController<SystemSettingController>();
        }
    }

    public override void OnInit(Transform root)
    {
        base.OnInit(root);
        this.mRoot = root;
        this.InitGameObject();
        this.InitEvent();
        this.InitObjGameSetting_Game();
        this.InitObjGameSetting_Sys();
        this.InitObjGameSetting_MessageBox();
        this.InitObjAutoFight();
        this.ActiveMenuObj(true);
    }

    private void InitGameObject()
    {
        this.transMenu = this.mRoot.Find("Offset_Example/Panel_menu");
        this.transSys = this.mRoot.Find("Offset_Example/Panel_sys");
        this.transGame = this.mRoot.Find("Offset_Example/Panel_game");
        this.transMessageBox = this.mRoot.Find("Offset_Example/Panel_MessageBox");
        this.transAutoFight = this.mRoot.Find("Offset_Example/Panel_auto_fight");
        this.btnSys = this.mRoot.Find("Offset_Example/Panel_menu/Panel_btn/btn_sys").GetComponent<Button>();
        this.btnGame = this.mRoot.Find("Offset_Example/Panel_menu/Panel_btn/btn_game").GetComponent<Button>();
        this.btnKeyboard = this.mRoot.Find("Offset_Example/Panel_menu/Panel_btn/btn_keyboard").GetComponent<Button>();
        this.btnCharacter = this.mRoot.Find("Offset_Example/Panel_menu/Panel_btn/btn_character").GetComponent<Button>();
        this.btnExit = this.mRoot.Find("Offset_Example/Panel_menu/Panel_btn/btn_exit").GetComponent<Button>();
        this.btnClose = this.mRoot.Find("Offset_Example/Panel_menu/Panel_title/Button").GetComponent<Button>();
        this.transMenu.gameObject.SetActive(false);
        this.transSys.gameObject.SetActive(false);
        this.transGame.gameObject.SetActive(false);
        this.transAutoFight.gameObject.SetActive(false);
        Transform transform = this.mRoot.Find("Offset_Example/Panel_menu/Panel_btn/btn_outStuck");
        if (transform != null)
        {
            this.btnOutStuck = transform.GetComponent<Button>();
        }
    }

    private void InitEvent()
    {
        this.btnSys.onClick.AddListener(new UnityAction(this.OnBtnSysClick));
        this.btnGame.onClick.AddListener(new UnityAction(this.OnBtnGameClick));
        this.btnKeyboard.onClick.AddListener(new UnityAction(this.OnBtnKeyboardClick));
        this.btnCharacter.onClick.AddListener(new UnityAction(this.OnBtnCharacterClick));
        this.btnExit.onClick.AddListener(new UnityAction(this.OnBtnExitClick));
        this.btnClose.onClick.AddListener(new UnityAction(this.OnBtnCloseClick));
        if (this.btnOutStuck != null)
        {
            this.btnOutStuck.onClick.AddListener(new UnityAction(this.OnBtnOutStuckClick));
        }
    }

    public void OnBtnSysClick()
    {
        this.ActiveMenuObj(false);
        this.OpenGameSettingSys();
    }

    public void OnBtnGameClick()
    {
        this.ActiveMenuObj(false);
        this.InitGameSetting_Game();
    }

    public void OnBtnKeyboardClick()
    {
        this.settingControl.CloseUI();
        UIManager.Instance.ShowUI<UI_ShortcutsConfig>("UI_ShortcutsConfig", null, UIManager.ParentType.CommonUI, false);
    }

    public void OnBtnCharacterClick()
    {
        if (MainPlayer.Self.IsBattleState)
        {
            TipsWindow.ShowWindow(1604U);
            return;
        }
        this.ActiveMenuObj(false);
        this.InitGameSetting_MessageBox(UI_GameSetting.MessageBox_Type.ReturnSelectRole, delegate
        {
            this.settingControl.OnLoginOut();
        }, delegate
        {
            this.ActiveMenuObj(true);
        });
    }

    public void OnBtnExitClick()
    {
        if (MainPlayer.Self.IsBattleState)
        {
            TipsWindow.ShowWindow(1605U);
            return;
        }
        this.ActiveMenuObj(false);
        this.InitGameSetting_MessageBox(UI_GameSetting.MessageBox_Type.ExitGame, delegate
        {
            this.settingControl.OnQuitGame();
        }, delegate
        {
            this.ActiveMenuObj(true);
        });
    }

    public void OnBtnCloseClick()
    {
        this.settingControl.CloseUI();
    }

    private void ActiveMenuObj(bool active)
    {
        this.transMenu.gameObject.SetActive(active);
    }

    private void OnBtnOutStuckClick()
    {
        this.ActiveMenuObj(false);
        ControllerManager.Instance.GetController<MsgBoxController>().ShowMsgBox("提示", "是否确定脱离卡点？\n确认后将角色传送至最近复活点。", "是", "否", UIManager.ParentType.CommonUI, new Action(this.settingControl.SendOutStuck), delegate ()
        {
            this.ActiveMenuObj(true);
        }, null);
    }

    public override void OnDispose()
    {
        base.OnDispose();
    }

    private ShortcutsConfigController shortControll
    {
        get
        {
            if (this.shortControll_ == null)
            {
                this.shortControll_ = ControllerManager.Instance.GetController<ShortcutsConfigController>();
            }
            return this.shortControll_;
        }
    }

    private AutoFightController autoFightControll
    {
        get
        {
            if (this.autoFightControll_ == null)
            {
                this.autoFightControll_ = ControllerManager.Instance.GetController<AutoFightController>();
            }
            return this.autoFightControll_;
        }
    }

    public void InitObjAutoFight()
    {
        this.btnAutoFightClose = this.transAutoFight.Find("Panel_title/Button").GetComponent<Button>();
        this.toggleAutoPick = this.transAutoFight.Find("Item_auto_pick").GetComponent<Toggle>();
        this.conditionListRoot = this.transAutoFight.Find("Panel_condition");
        this.btnResumeDefault = this.transAutoFight.Find("btn_default").GetComponent<Button>();
        this.btnApply = this.transAutoFight.Find("btn_apply").GetComponent<Button>();
        this.InitEventAutoFight();
    }

    private void InitEventAutoFight()
    {
        this.btnAutoFightClose.onClick.RemoveAllListeners();
        this.btnAutoFightClose.onClick.AddListener(new UnityAction(this.OnCloseClick));
        this.btnResumeDefault.onClick.RemoveAllListeners();
        this.btnResumeDefault.onClick.AddListener(new UnityAction(this.ResumeDefault));
        this.btnApply.onClick.RemoveAllListeners();
        this.btnApply.onClick.AddListener(new UnityAction(this.ApplyCurrentChange));
        this.toggleAutoPick.onValueChanged.RemoveAllListeners();
        this.toggleAutoPick.onValueChanged.AddListener(new UnityAction<bool>(this.OnAutoPickValueChange));
    }

    public void OnOpenAutoFight()
    {
        ServerStorageManager.Instance.GetData(ServerStorageKey.AutoFight, 0U);
        this.settingControl.RegAutoFightDataCallBack(new Action<string>(this.OnReceiveAutoFightDataBack));
    }

    private void OnReceiveAutoFightDataBack(string data)
    {
        if (!string.IsNullOrEmpty(data))
        {
            this.autoFightDataOriginal = (MyJson.Parse(data) as MyJson.JsonNode_Object);
            this.autoFightData = (MyJson.Parse(data) as MyJson.JsonNode_Object);
            this.cfgListType = this.autoFightControll.InitCfgTypeListByJsonData(this.autoFightData);
        }
        else
        {
            this.autoFightDataOriginal = new MyJson.JsonNode_Object();
            this.autoFightData = new MyJson.JsonNode_Object();
            this.LoadDefaultSetting(true);
        }
        this.RefreshAutoFightSettingUI();
        this.CheckApplyButtonState();
    }

    private void OnCloseClick()
    {
        this.transAutoFight.gameObject.SetActive(false);
        this.transGame.gameObject.SetActive(true);
        ServerStorageManager.Instance.UnRegReqCallBack(ServerStorageKey.AutoFight.ToString());
    }

    private void ResumeDefault()
    {
        this.LoadDefaultSetting(false);
        this.SaveData();
    }

    private void ApplyCurrentChange()
    {
        this.SaveData();
    }

    private void OnAutoPickValueChange(bool b)
    {
        this.autoFightData[this.autoPickJsonKey] = new MyJson.JsonNode_ValueNumber(b);
        this.CheckApplyButtonState();
    }

    private void LoadDefaultSetting(bool isFirst)
    {
        if (this.autoFightDataOriginal == null || this.autoFightData == null)
        {
            return;
        }
        this.autoFightData[this.autoPickJsonKey] = new MyJson.JsonNode_ValueNumber(true);
        this.cfgListType = this.autoFightControll.InitDefaultCfgList();
        this.autoFightData[this.skillsJsonKey] = this.CfgListToJObjArray();
        if (isFirst)
        {
            this.autoFightDataOriginal[this.skillsJsonKey] = this.CfgListToJObjArray();
            this.autoFightDataOriginal[this.autoPickJsonKey] = new MyJson.JsonNode_ValueNumber(true);
        }
        this.RefreshAutoFightSettingUI();
    }

    private void SaveData()
    {
        if (this.autoFightDataOriginal == null || this.autoFightData == null)
        {
            return;
        }
        string content = this.autoFightData.ToString();
        ServerStorageManager.Instance.AddUpdateData(ServerStorageKey.AutoFight, content, 0U);
    }

    private void CheckApplyButtonState()
    {
        this.btnApply.interactable = this.IsHaveDataChange();
    }

    private bool IsHaveDataChange()
    {
        if (this.autoFightDataOriginal == null || this.autoFightData == null)
        {
            return false;
        }
        string a = this.autoFightDataOriginal.ToString();
        string b = this.autoFightData.ToString();
        return a != b;
    }

    private void RefreshAutoFightSettingUI()
    {
        bool isOn = true;
        if (this.autoFightData.ContainsKey(this.autoPickJsonKey))
        {
            isOn = (this.autoFightData[this.autoPickJsonKey] as MyJson.JsonNode_ValueNumber);
        }
        this.toggleAutoPick.isOn = isOn;
        int num = 0;
        Dictionary<int, ShortcutkeyFunctionType> inUsingKeyDic = new Dictionary<int, ShortcutkeyFunctionType>();
        List<Dropdown> dropDowns = new List<Dropdown>();
        this.conditionListRoot.gameObject.SetActive(true);
        int num2 = 0;
        while (num2 < this.conditionListRoot.childCount && num < this.cfgListType.Count)
        {
            Transform child = this.conditionListRoot.GetChild(num2);
            if (child.childCount > 0)
            {
                this.InitSkillCfgItem(child, this.cfgListType[num], num, inUsingKeyDic, dropDowns);
                num++;
            }
            num2++;
        }
    }

    private void InitSkillCfgItem(Transform itemRoot, AutoFightCfg cfg, int index, Dictionary<int, ShortcutkeyFunctionType> inUsingKeyDic, List<Dropdown> dropDowns)
    {
        InputField input = itemRoot.GetComponentInChildren<InputField>();
        if (cfg.keyType != ShortcutkeyFunctionType.Role)
        {
            inUsingKeyDic[index] = cfg.keyType;
        }
        if (input)
        {
            input.text = cfg.percent.ToString();
            input.onValueChanged.RemoveAllListeners();
            input.onValueChanged.AddListener(delegate (string newValue)
            {
                if (string.IsNullOrEmpty(newValue))
                {
                    return;
                }
                int num = int.Parse(newValue);
                int num2 = Mathf.Clamp(num, 1, 100);
                if (num != num2)
                {
                    input.text = num2.ToString();
                    return;
                }
                cfg.percent = num2;
                this.SetDataChangeToJson_Blood(index, cfg.percent);
                this.CheckApplyButtonState();
            });
        }
        Toggle componentInChildren = itemRoot.GetComponentInChildren<Toggle>();
        componentInChildren.isOn = cfg.isEnable;
        componentInChildren.onValueChanged.RemoveAllListeners();
        componentInChildren.onValueChanged.AddListener(delegate (bool b)
        {
            cfg.isEnable = b;
            this.SetDataChangeToJson_State(index, b);
            this.CheckApplyButtonState();
        });
        Dropdown componentInChildren2 = itemRoot.GetComponentInChildren<Dropdown>();
        if (componentInChildren2)
        {
            componentInChildren2.options = this.GetDropDownList(cfg.cfgType);
            int value = ((int)cfg.keyType % 100);
            componentInChildren2.value = value;
            componentInChildren2.onValueChanged.RemoveAllListeners();
            componentInChildren2.onValueChanged.AddListener(delegate (int i)
            {
                ShortcutkeyFunctionType keyIndexByIndexAndType = this.GetKeyIndexByIndexAndType(i, cfg.cfgType);
                int num = this.CheckIsKeyUseID(inUsingKeyDic, keyIndexByIndexAndType);
                if (num >= 0)
                {
                    dropDowns[num].value = 0;
                }
                inUsingKeyDic.Remove(num);
                inUsingKeyDic[index] = keyIndexByIndexAndType;
                cfg.keyType = keyIndexByIndexAndType;
                this.SetDataChangeToJson_KeyType(index, (int)cfg.keyType);
                this.CheckApplyButtonState();
            });
            dropDowns.Add(componentInChildren2);
        }
    }

    private int CheckIsKeyUseID(Dictionary<int, ShortcutkeyFunctionType> checkDic, ShortcutkeyFunctionType curKey)
    {
        foreach (KeyValuePair<int, ShortcutkeyFunctionType> keyValuePair in checkDic)
        {
            if (curKey == keyValuePair.Value)
            {
                return keyValuePair.Key;
            }
        }
        return -1;
    }

    private List<Dropdown.OptionData> GetDropDownList(AutoFightCfgType type)
    {
        List<Dropdown.OptionData> list = new List<Dropdown.OptionData>();
        int num = 501;
        int num2 = 512;
        if (type == AutoFightCfgType.Prop)
        {
            num = 101;
            num2 = 112;
        }
        list.Add(new Dropdown.OptionData
        {
            text = "未设置"
        });
        for (int i = num; i <= num2; i++)
        {
            string titleNameForItemByFunctionType = this.shortControll.GetTitleNameForItemByFunctionType((ShortcutkeyFunctionType)i);
            string keyNameForItemByFunctionType = this.shortControll.GetKeyNameForItemByFunctionType((ShortcutkeyFunctionType)i);
            list.Add(new Dropdown.OptionData
            {
                text = keyNameForItemByFunctionType
            });
        }
        return list;
    }

    private ShortcutkeyFunctionType GetKeyIndexByIndexAndType(int index, AutoFightCfgType type)
    {
        if (index == 0)
        {
            return ShortcutkeyFunctionType.Role;
        }
        int num = 501;
        if (type == AutoFightCfgType.Prop)
        {
            num = 101;
        }
        return (ShortcutkeyFunctionType)(num + index - 1);
    }

    private void SetDataChangeToJson_Blood(int index, int data)
    {
        MyJson.JsonNode_Array jsonNode_Array = this.autoFightData[this.skillsJsonKey] as MyJson.JsonNode_Array;
        MyJson.JsonNode_Object jsonNode_Object = jsonNode_Array[index] as MyJson.JsonNode_Object;
        jsonNode_Object["percent"] = new MyJson.JsonNode_ValueNumber((double)data);
    }

    private void SetDataChangeToJson_State(int index, bool data)
    {
        MyJson.JsonNode_Array jsonNode_Array = this.autoFightData[this.skillsJsonKey] as MyJson.JsonNode_Array;
        MyJson.JsonNode_Object jsonNode_Object = jsonNode_Array[index] as MyJson.JsonNode_Object;
        jsonNode_Object["isEnable"] = new MyJson.JsonNode_ValueNumber(data);
    }

    private void SetDataChangeToJson_KeyType(int index, int data)
    {
        MyJson.JsonNode_Array jsonNode_Array = this.autoFightData[this.skillsJsonKey] as MyJson.JsonNode_Array;
        MyJson.JsonNode_Object jsonNode_Object = jsonNode_Array[index] as MyJson.JsonNode_Object;
        jsonNode_Object["keyType"] = new MyJson.JsonNode_ValueNumber((double)data);
    }

    private MyJson.JsonNode_Object CfgObjToJObj(AutoFightCfg cfgObj)
    {
        MyJson.JsonNode_Object jsonNode_Object = new MyJson.JsonNode_Object();
        jsonNode_Object["isEnable"] = new MyJson.JsonNode_ValueNumber(cfgObj.isEnable);
        jsonNode_Object["percent"] = new MyJson.JsonNode_ValueNumber((double)cfgObj.percent);
        jsonNode_Object["keyType"] = new MyJson.JsonNode_ValueNumber((double)cfgObj.keyType);
        jsonNode_Object["cfgType"] = new MyJson.JsonNode_ValueNumber((double)cfgObj.cfgType);
        return jsonNode_Object;
    }

    private MyJson.JsonNode_Array CfgListToJObjArray()
    {
        MyJson.JsonNode_Array jsonNode_Array = new MyJson.JsonNode_Array();
        for (int i = 0; i < this.cfgListType.Count; i++)
        {
            jsonNode_Array.Add(this.CfgObjToJObj(this.cfgListType[i]));
        }
        return jsonNode_Array;
    }

    private void InitObjGameSetting_Game()
    {
        this.togGamePlayerBar = this.transGame.Find("Panel_function/Viewport/Content/Panel_info/Panel_own/Panel_check/hp").GetComponent<Toggle>();
        this.togGamePlayerName = this.transGame.Find("Panel_function/Viewport/Content/Panel_info/Panel_own/Panel_check/name").GetComponent<Toggle>();
        this.togGamePlayerGuild = this.transGame.Find("Panel_function/Viewport/Content/Panel_info/Panel_own/Panel_check/family").GetComponent<Toggle>();
        this.togGameOthersBar = this.transGame.Find("Panel_function/Viewport/Content/Panel_info/Panel_player/Panel_check/hp").GetComponent<Toggle>();
        this.togGameOthersName = this.transGame.Find("Panel_function/Viewport/Content/Panel_info/Panel_player/Panel_check/name").GetComponent<Toggle>();
        this.togGameOthersGuild = this.transGame.Find("Panel_function/Viewport/Content/Panel_info/Panel_player/Panel_check/family").GetComponent<Toggle>();
        this.togGameEnemyBar = this.transGame.Find("Panel_function/Viewport/Content/Panel_info/Panel_enemy/Panel_check/hp").GetComponent<Toggle>();
        this.togGameEnemyName = this.transGame.Find("Panel_function/Viewport/Content/Panel_info/Panel_enemy/Panel_check/name").GetComponent<Toggle>();
        this.togGameEnemyGuild = this.transGame.Find("Panel_function/Viewport/Content/Panel_info/Panel_enemy/Panel_check/family").GetComponent<Toggle>();
        this.togGameNPCName = this.transGame.Find("Panel_function/Viewport/Content/Panel_info/Panel_NPC/Panel_check/name").GetComponent<Toggle>();
        this.togGameAllowPartyInvite = this.transGame.Find("Panel_function/Viewport/Content/Panel_check/team").GetComponent<Toggle>();
        this.togGameAllowGuildInvite = this.transGame.Find("Panel_function/Viewport/Content/Panel_check/family").GetComponent<Toggle>();
        this.togGameAllowFriendInvite = this.transGame.Find("Panel_function/Viewport/Content/Panel_check/friend").GetComponent<Toggle>();
        this.togGameHealthWarning = this.transGame.Find("Panel_function/Viewport/Content/Panel_check/lowhealth").GetComponent<Toggle>();
        this.togGameMouseMove = this.transGame.Find("Panel_function/Viewport/Content/Panel_check/clickmove").GetComponent<Toggle>();
        this.togGame2ndPw = this.transGame.Find("Panel_function/Viewport/Content/Panel_2ndpw/switch").GetComponent<Toggle>();
        this.btnGameExit = this.transGame.Find("Panel_title/Button").GetComponent<Button>();
        this.btnGameDefault = this.transGame.Find("btn_default").GetComponent<Button>();
        this.btnGameApply = this.transGame.Find("btn_apply").GetComponent<Button>();
        this.btnGameChange2ndPw = this.transGame.Find("Panel_function/Viewport/Content/Panel_2ndpw/btn_change").GetComponent<Button>();
        this.btnAutoFight = this.transGame.Find("Panel_function/Viewport/Content/btn_auto_attack").GetComponent<Button>();
        this.btnGameExit.onClick.RemoveAllListeners();
        this.btnGameExit.onClick.AddListener(new UnityAction(this.OnBtnGameExitClick));
        this.btnGameDefault.onClick.RemoveAllListeners();
        this.btnGameDefault.onClick.AddListener(new UnityAction(this.OnBtnGameDefaultClick));
        this.btnGameApply.onClick.RemoveAllListeners();
        this.btnGameApply.onClick.AddListener(new UnityAction(this.OnBtnGameApplyClick));
        this.btnGameChange2ndPw.onClick.RemoveAllListeners();
        this.btnGameChange2ndPw.onClick.AddListener(new UnityAction(this.OnBtnGameChange2ndPwClick));
        this.btnAutoFight.onClick.RemoveAllListeners();
        this.btnAutoFight.onClick.AddListener(new UnityAction(this.OnBtnAutoFightClick));
        this.togGamePlayerBar.onValueChanged.RemoveAllListeners();
        this.togGamePlayerBar.onValueChanged.AddListener(new UnityAction<bool>(this.OnTogGamePlayerBarValueChange));
        this.togGamePlayerName.onValueChanged.RemoveAllListeners();
        this.togGamePlayerName.onValueChanged.AddListener(new UnityAction<bool>(this.OnTogGamePlayerNameValueChange));
        this.togGamePlayerGuild.onValueChanged.RemoveAllListeners();
        this.togGamePlayerGuild.onValueChanged.AddListener(new UnityAction<bool>(this.OnTogGamePlayerGuildValueChange));
        this.togGameOthersBar.onValueChanged.RemoveAllListeners();
        this.togGameOthersBar.onValueChanged.AddListener(new UnityAction<bool>(this.OnTogGameOthersBarValueChange));
        this.togGameOthersName.onValueChanged.RemoveAllListeners();
        this.togGameOthersName.onValueChanged.AddListener(new UnityAction<bool>(this.OnTogGameOthersNameValueChange));
        this.togGameOthersGuild.onValueChanged.RemoveAllListeners();
        this.togGameOthersGuild.onValueChanged.AddListener(new UnityAction<bool>(this.OnTogGameOthersGuildValueChange));
        this.togGameEnemyBar.onValueChanged.RemoveAllListeners();
        this.togGameEnemyBar.onValueChanged.AddListener(new UnityAction<bool>(this.OnTogGameEnemyBarValueChange));
        this.togGameEnemyName.onValueChanged.RemoveAllListeners();
        this.togGameEnemyName.onValueChanged.AddListener(new UnityAction<bool>(this.OnTogGameEnemyNameValueChange));
        this.togGameEnemyGuild.onValueChanged.RemoveAllListeners();
        this.togGameEnemyGuild.onValueChanged.AddListener(new UnityAction<bool>(this.OnTogGameEnemyGuildValueChange));
        this.togGameNPCName.onValueChanged.RemoveAllListeners();
        this.togGameNPCName.onValueChanged.AddListener(new UnityAction<bool>(this.OnTogGameNPCNameValueChange));
        this.togGameAllowPartyInvite.onValueChanged.RemoveAllListeners();
        this.togGameAllowPartyInvite.onValueChanged.AddListener(new UnityAction<bool>(this.OnTogGameAllowTeamInviteValueChange));
        this.togGameAllowGuildInvite.onValueChanged.RemoveAllListeners();
        this.togGameAllowGuildInvite.onValueChanged.AddListener(new UnityAction<bool>(this.OnTogGameAllowGuildInviteValueChange));
        this.togGameAllowFriendInvite.onValueChanged.RemoveAllListeners();
        this.togGameAllowFriendInvite.onValueChanged.AddListener(new UnityAction<bool>(this.OnTogGameAllowFriendInviteValueChange));
        this.togGameHealthWarning.onValueChanged.RemoveAllListeners();
        this.togGameHealthWarning.onValueChanged.AddListener(new UnityAction<bool>(this.OnTogGameHealthWarningValueChange));
        this.togGameMouseMove.onValueChanged.RemoveAllListeners();
        this.togGameMouseMove.onValueChanged.AddListener(new UnityAction<bool>(this.OnTogGameMouseMoveValueChange));
        this.togGame2ndPw.onValueChanged.RemoveAllListeners();
        this.togGame2ndPw.onValueChanged.AddListener(new UnityAction<bool>(this.OnTogGame2ndPwValueChange));
        this.curFuntionData = new SystemSettingData_Funtion();
        this.curShowData = new SystemSettingData_Show();
    }

    public void InitGameSetting_Game()
    {
        this.transGame.gameObject.SetActive(true);
        if (this.settingControl.funtionData != null)
        {
            this.curFuntionData.AllowPartyInvite = this.settingControl.funtionData.AllowPartyInvite;
            this.curFuntionData.AllowGuildInvite = this.settingControl.funtionData.AllowGuildInvite;
            this.curFuntionData.AllowFriendInvite = this.settingControl.funtionData.AllowFriendInvite;
            this.curFuntionData.LowHealthWarning = this.settingControl.funtionData.LowHealthWarning;
            this.curFuntionData.IfMouse = this.settingControl.funtionData.IfMouse;
            this.curFuntionData.Close2ndPW = !this.settingControl.funtionData.Close2ndPW;
            this.InitFuntionUI(this.curFuntionData);
        }
        if (this.settingControl.showData != null)
        {
            this.curShowData.PlayerBarShow = this.settingControl.showData.PlayerBarShow;
            this.curShowData.PlayerNameShow = this.settingControl.showData.PlayerNameShow;
            this.curShowData.PlayerGuildShow = this.settingControl.showData.PlayerGuildShow;
            this.curShowData.OthersBarShow = this.settingControl.showData.OthersBarShow;
            this.curShowData.OthersNameShow = this.settingControl.showData.OthersNameShow;
            this.curShowData.OthersGuildShow = this.settingControl.showData.OthersGuildShow;
            this.curShowData.EnemyBarShow = this.settingControl.showData.EnemyBarShow;
            this.curShowData.EnemyNameShow = this.settingControl.showData.EnemyNameShow;
            this.curShowData.EnemyGuildShow = this.settingControl.showData.EnemyGuildShow;
            this.curShowData.NpcNameShow = this.settingControl.showData.NpcNameShow;
            this.InitShowUI(this.curShowData);
        }
        this.isFuntionChange = false;
        this.isShowChange = false;
        this.CheckGameButtonState();
    }

    private void InitFuntionUI(SystemSettingData_Funtion data)
    {
        this.togGameAllowPartyInvite.isOn = data.AllowPartyInvite;
        this.togGameAllowGuildInvite.isOn = data.AllowGuildInvite;
        this.togGameAllowFriendInvite.isOn = data.AllowFriendInvite;
        this.togGameHealthWarning.isOn = data.LowHealthWarning;
        this.togGameMouseMove.isOn = data.IfMouse;
        this.togGame2ndPw.isOn = data.Close2ndPW;
    }

    private void InitShowUI(SystemSettingData_Show data)
    {
        this.togGamePlayerBar.isOn = data.PlayerBarShow;
        this.togGamePlayerName.isOn = data.PlayerNameShow;
        this.togGamePlayerGuild.isOn = data.PlayerGuildShow;
        this.togGameOthersBar.isOn = data.OthersBarShow;
        this.togGameOthersName.isOn = data.OthersNameShow;
        this.togGameOthersGuild.isOn = data.OthersGuildShow;
        this.togGameEnemyBar.isOn = data.EnemyBarShow;
        this.togGameEnemyName.isOn = data.EnemyNameShow;
        this.togGameEnemyGuild.isOn = data.EnemyGuildShow;
        this.togGameNPCName.isOn = data.NpcNameShow;
    }

    private void OnBtnGameExitClick()
    {
        this.CloseGameSetting();
    }

    private void CloseGameSetting()
    {
        this.InitGameSetting_Game();
        this.transGame.gameObject.SetActive(false);
        this.ActiveMenuObj(true);
    }

    private void OnBtnGameDefaultClick()
    {
        this.InitFuntionUI(this.settingControl.defaultFuntionData);
        this.InitShowUI(this.settingControl.defaultShowData);
    }

    private void OnBtnGameApplyClick()
    {
        this.GameApply();
    }

    private void OnBtnGameChange2ndPwClick()
    {
        GlobalRegister.OpenResetSecondPwd();
    }

    private void OnBtnAutoFightClick()
    {
        this.transGame.gameObject.SetActive(false);
        this.transAutoFight.gameObject.SetActive(true);
        this.OnOpenAutoFight();
    }

    private void GameApply()
    {
        if (this.isFuntionChange)
        {
        }
        if (this.isShowChange)
        {
        }
        if (this.isFuntionChange || this.isShowChange)
        {
            this.settingControl.SaveStorageData(this.curFuntionData, this.curShowData);
        }
    }

    private void OnTogGamePlayerBarValueChange(bool state)
    {
        this.curShowData.PlayerBarShow = state;
        this.CheckChangeShowData();
    }

    private void OnTogGamePlayerNameValueChange(bool state)
    {
        this.curShowData.PlayerNameShow = state;
        this.CheckChangeShowData();
    }

    private void OnTogGamePlayerGuildValueChange(bool state)
    {
        this.curShowData.PlayerGuildShow = state;
        this.CheckChangeShowData();
    }

    private void OnTogGameOthersBarValueChange(bool state)
    {
        this.curShowData.OthersBarShow = state;
        this.CheckChangeShowData();
    }

    private void OnTogGameOthersNameValueChange(bool state)
    {
        this.curShowData.OthersNameShow = state;
        this.CheckChangeShowData();
    }

    private void OnTogGameOthersGuildValueChange(bool state)
    {
        this.curShowData.OthersGuildShow = state;
        this.CheckChangeShowData();
    }

    private void OnTogGameEnemyBarValueChange(bool state)
    {
        this.curShowData.EnemyBarShow = state;
        this.CheckChangeShowData();
    }

    private void OnTogGameEnemyNameValueChange(bool state)
    {
        this.curShowData.EnemyNameShow = state;
        this.CheckChangeShowData();
    }

    private void OnTogGameEnemyGuildValueChange(bool state)
    {
        this.curShowData.EnemyGuildShow = state;
        this.CheckChangeShowData();
    }

    private void OnTogGameNPCNameValueChange(bool state)
    {
        this.curShowData.NpcNameShow = state;
        this.CheckChangeShowData();
    }

    private void OnTogGameAllowTeamInviteValueChange(bool state)
    {
        this.curFuntionData.AllowPartyInvite = state;
        this.CheckChangeFuntionData();
    }

    private void OnTogGameAllowGuildInviteValueChange(bool state)
    {
        this.curFuntionData.AllowGuildInvite = state;
        this.CheckChangeFuntionData();
    }

    private void OnTogGameAllowFriendInviteValueChange(bool state)
    {
        this.curFuntionData.AllowFriendInvite = state;
        this.CheckChangeFuntionData();
    }

    private void OnTogGameHealthWarningValueChange(bool state)
    {
        this.curFuntionData.LowHealthWarning = state;
        this.CheckChangeFuntionData();
    }

    private void OnTogGameMouseMoveValueChange(bool state)
    {
        this.curFuntionData.IfMouse = state;
        this.CheckChangeFuntionData();
    }

    private void OnTogGame2ndPwValueChange(bool state)
    {
        this.curFuntionData.Close2ndPW = !state;
        this.CheckChangeFuntionData();
    }

    private void CheckChangeFuntionData()
    {
        if (this.settingControl.funtionData == null)
        {
            return;
        }
        if (ServerStorageManager.Instance.SerializeClass<SystemSettingData_Funtion>(this.settingControl.funtionData) == ServerStorageManager.Instance.SerializeClass<SystemSettingData_Funtion>(this.curFuntionData))
        {
            this.isFuntionChange = false;
        }
        else
        {
            this.isFuntionChange = true;
        }
        this.CheckGameButtonState();
    }

    private void CheckChangeShowData()
    {
        if (this.settingControl.showData == null)
        {
            return;
        }
        if (ServerStorageManager.Instance.SerializeClass<SystemSettingData_Show>(this.settingControl.showData) == ServerStorageManager.Instance.SerializeClass<SystemSettingData_Show>(this.curShowData))
        {
            this.isShowChange = false;
        }
        else
        {
            this.isShowChange = true;
        }
        this.CheckGameButtonState();
    }

    private void CheckGameButtonState()
    {
        if (!this.isFuntionChange && !this.isShowChange)
        {
            this.btnGameApply.interactable = false;
        }
        else
        {
            this.btnGameApply.interactable = true;
        }
    }

    private void InitObjGameSetting_MessageBox()
    {
        this.txtMsgExit = this.transMessageBox.Find("Text_Exit").GetComponent<Text>();
        this.txtMsgExitCD = this.transMessageBox.Find("Text_Exit/Text_CountDown").GetComponent<Text>();
        this.txtMsgReturn = this.transMessageBox.Find("Text_ReturnSeleteRole").GetComponent<Text>();
        this.txtMsgReturnCD = this.transMessageBox.Find("Text_ReturnSeleteRole/Text_CountDown").GetComponent<Text>();
        this.txtMsgConfirm = this.transMessageBox.Find("Text_Confirm").GetComponent<Text>();
        this.txtMsgConfirmCD = this.transMessageBox.Find("Text_Confirm/txt_countdown").GetComponent<Text>();
        this.btnMsgOK = this.transMessageBox.Find("btn_confirm").GetComponent<Button>();
        this.btnMsgCancel = this.transMessageBox.Find("btn_cancel").GetComponent<Button>();
        this.btnMsgOK.onClick.RemoveAllListeners();
        this.btnMsgOK.onClick.AddListener(new UnityAction(this.OnBtnMsgOKClick));
        this.btnMsgCancel.onClick.RemoveAllListeners();
        this.btnMsgCancel.onClick.AddListener(new UnityAction(this.OnBtnMsgCancelClick));
    }

    private void InitGameSetting_MessageBox(UI_GameSetting.MessageBox_Type type, Action okcallback, Action cancelcallback)
    {
        this.transMessageBox.gameObject.SetActive(true);
        this._type = type;
        this._okAction = okcallback;
        this._cancelAction = cancelcallback;
        if (this._type == UI_GameSetting.MessageBox_Type.SecondConfirm)
        {
            this._lastTime = this.CD_TIME;
            this.txtMsgConfirmCD.text = this._lastTime.ToString();
            Scheduler.Instance.AddTimer(1f, true, new Scheduler.OnScheduler(this.ConfirmTimer));
            this.txtMsgReturn.gameObject.SetActive(false);
            this.txtMsgExit.gameObject.SetActive(false);
            this.txtMsgConfirm.gameObject.SetActive(true);
        }
        else if (this._type == UI_GameSetting.MessageBox_Type.ReturnSelectRole)
        {
            this._lastTime = this.CD_TIME;
            this.txtMsgReturnCD.text = this._lastTime.ToString();
            Scheduler.Instance.AddTimer(1f, true, new Scheduler.OnScheduler(this.ReturnSelectRoleTimer));
            this.txtMsgReturn.gameObject.SetActive(true);
            this.txtMsgExit.gameObject.SetActive(false);
            this.txtMsgConfirm.gameObject.SetActive(false);
        }
        else
        {
            this._lastTime = this.CD_TIME;
            this.txtMsgExitCD.text = this._lastTime.ToString();
            Scheduler.Instance.AddTimer(1f, true, new Scheduler.OnScheduler(this.ExitTimer));
            this.txtMsgReturn.gameObject.SetActive(false);
            this.txtMsgExit.gameObject.SetActive(true);
            this.txtMsgConfirm.gameObject.SetActive(false);
        }
    }

    private void CloseMessageBox()
    {
        this.transMessageBox.gameObject.SetActive(false);
    }

    private void OnBtnMsgOKClick()
    {
        if (this._type == UI_GameSetting.MessageBox_Type.SecondConfirm)
        {
            Scheduler.Instance.RemoveTimer(new Scheduler.OnScheduler(this.ConfirmTimer));
        }
        else if (this._type == UI_GameSetting.MessageBox_Type.ReturnSelectRole)
        {
            Scheduler.Instance.RemoveTimer(new Scheduler.OnScheduler(this.ReturnSelectRoleTimer));
        }
        else
        {
            Scheduler.Instance.RemoveTimer(new Scheduler.OnScheduler(this.ExitTimer));
        }
        if (this._okAction != null)
        {
            this._okAction();
        }
        this.CloseMessageBox();
    }

    private void OnBtnMsgCancelClick()
    {
        if (this._type == UI_GameSetting.MessageBox_Type.SecondConfirm)
        {
            Scheduler.Instance.RemoveTimer(new Scheduler.OnScheduler(this.ConfirmTimer));
        }
        else if (this._type == UI_GameSetting.MessageBox_Type.ReturnSelectRole)
        {
            Scheduler.Instance.RemoveTimer(new Scheduler.OnScheduler(this.ReturnSelectRoleTimer));
        }
        else
        {
            Scheduler.Instance.RemoveTimer(new Scheduler.OnScheduler(this.ExitTimer));
        }
        if (this._cancelAction != null)
        {
            this._cancelAction();
        }
        this.CloseMessageBox();
    }

    private void ExitTimer()
    {
        if (this._lastTime <= 0f)
        {
            this.OnBtnMsgOKClick();
            return;
        }
        this._lastTime -= 1f;
        if (this._lastTime <= 0f)
        {
            this._lastTime = 0f;
        }
        this.txtMsgExitCD.text = this._lastTime.ToString();
    }

    private void ReturnSelectRoleTimer()
    {
        if (this._lastTime <= 0f)
        {
            this.OnBtnMsgOKClick();
            return;
        }
        this._lastTime -= 1f;
        if (this._lastTime <= 0f)
        {
            this._lastTime = 0f;
        }
        this.txtMsgReturnCD.text = this._lastTime.ToString();
    }

    private void ConfirmTimer()
    {
        if (this._lastTime <= 0f)
        {
            this.OnBtnMsgCancelClick();
            return;
        }
        this._lastTime -= 1f;
        if (this._lastTime <= 0f)
        {
            this._lastTime = 0f;
        }
        this.txtMsgConfirmCD.text = this._lastTime.ToString();
    }

    private void InitObjGameSetting_Sys()
    {
        this.transSysVideo = this.transSys.Find("Panel_video");
        this.transSysAudio = this.transSys.Find("Panel_audio");
        this.togSysVideo = this.transSys.Find("ToggleGroup/Panel_tab/Video").GetComponent<Toggle>();
        this.togSysAudio = this.transSys.Find("ToggleGroup/Panel_tab/Audio").GetComponent<Toggle>();
        this.togMainSoundEffectOpen = this.transSys.Find("Panel_audio/Viewport/Content/Panel_own/Toggle_open").GetComponent<Toggle>();
        this.togMainSoundEffectClose = this.transSys.Find("Panel_audio/Viewport/Content/Panel_own/Toggle_close").GetComponent<Toggle>();
        this.togTeamSoundEffectOpen = this.transSys.Find("Panel_audio/Viewport/Content/Panel_team/Toggle_open").GetComponent<Toggle>();
        this.togTeamSoundEffectClose = this.transSys.Find("Panel_audio/Viewport/Content/Panel_team/Toggle_close").GetComponent<Toggle>();
        this.togOtherSoundEffectOpen = this.transSys.Find("Panel_audio/Viewport/Content/Panel_others/Toggle_open").GetComponent<Toggle>();
        this.togOtherSoundEffectClose = this.transSys.Find("Panel_audio/Viewport/Content/Panel_others/Toggle_close").GetComponent<Toggle>();
        this.togNPCSoundEffectOpen = this.transSys.Find("Panel_audio/Viewport/Content/Panel_enemy/Toggle_open").GetComponent<Toggle>();
        this.togNPCSoundEffectClose = this.transSys.Find("Panel_audio/Viewport/Content/Panel_enemy/Toggle_close").GetComponent<Toggle>();
        this.togSysScreen = this.transSys.Find("Panel_video/Viewport/Content/Panel_model/Toggle_screen").GetComponent<Toggle>();
        this.togSysWindow = this.transSys.Find("Panel_video/Viewport/Content/Panel_model/Toggle_window").GetComponent<Toggle>();
        this.togSysFull = this.transSys.Find("Panel_video/Viewport/Content/Panel_model/Toggle_full").GetComponent<Toggle>();
        this.btnSysExit = this.transSys.Find("Panel_title/Button").GetComponent<Button>();
        this.btnSysDefault = this.transSys.Find("btn_default").GetComponent<Button>();
        this.btnSysApply = this.transSys.Find("btn_apply").GetComponent<Button>();
        this.tgSysViewModel = this.transSys.Find("Panel_video/Viewport/Content/Panel_model").GetComponent<ToggleGroup>();
        this.resolutionOption = this.transSys.Find("Panel_video/Viewport/Content/Panel_resolution").gameObject;
        this.dpSysResolution = this.transSys.Find("Panel_video/Viewport/Content/Panel_resolution/Dropdown").GetComponent<Dropdown>();
        this.dpSysEffect = this.transSys.Find("Panel_video/Viewport/Content/Panel_effect/Dropdown").GetComponent<Dropdown>();
        this.dpSysShadow = this.transSys.Find("Panel_video/Viewport/Content/Panel_advanced/Panel_skilleffect/Dropdown").GetComponent<Dropdown>();
        this.dpSysShadowDistance = this.transSys.Find("Panel_video/Viewport/Content/Panel_advanced/Panel_shadow/Dropdown").GetComponent<Dropdown>();
        this.dpSysVsync = this.transSys.Find("Panel_video/Viewport/Content/Panel_advanced/Panel_vs/Dropdown").GetComponent<Dropdown>();
        this.dpSysAntialiasing = this.transSys.Find("Panel_video/Viewport/Content/Panel_advanced/Panel_msaa/Dropdown").GetComponent<Dropdown>();
        this.sliderCameraMaxdistance = this.transSys.Find("Panel_video/Viewport/Content/Panel_distance/Slider").GetComponent<Slider>();
        this.sliderUIScale = this.transSys.Find("Panel_video/Viewport/Content/Panel_uiscale/Slider").GetComponent<Slider>();
        this.txtCameraMaxdistance = this.transSys.Find("Panel_video/Viewport/Content/Panel_distance/txt_value").GetComponent<Text>();
        this.txtUIScale = this.transSys.Find("Panel_video/Viewport/Content/Panel_uiscale/txt_value").GetComponent<Text>();
        this.sliderMouseSpeed = this.transSys.Find("Panel_video/Viewport/Content/Panel_sensitivity/Slider").GetComponent<Slider>();
        this.txtMouseSpeed = this.transSys.Find("Panel_video/Viewport/Content/Panel_sensitivity/txt_value").GetComponent<Text>();
        this.sliderPixelPercent = this.transSys.Find("Panel_video/Viewport/Content/Panel_pixelpercent/Slider").GetComponent<Slider>();
        this.txtPixelPercent = this.transSys.Find("Panel_video/Viewport/Content/Panel_pixelpercent/txt_value").GetComponent<Text>();
        this.txtMinPixelPercent = this.transSys.Find("Panel_video/Viewport/Content/Panel_pixelpercent/txt_min_value").GetComponent<Text>();
        this.sliderSysSoundBg = this.transSys.Find("Panel_audio/Viewport/Content/Panel_music/Slider").GetComponent<Slider>();
        this.sliderSysSoundEffect = this.transSys.Find("Panel_audio/Viewport/Content/Panel_sound/Slider").GetComponent<Slider>();
        this.sliderSysSoundVoice = this.transSys.Find("Panel_audio/Viewport/Content/Panel_voice/Slider").GetComponent<Slider>();
        this.sliderSysSoundMain = this.transSys.Find("Panel_audio/Viewport/Content/Panel_main/Slider").GetComponent<Slider>();
        this.textSysSoundBg = this.transSys.Find("Panel_audio/Viewport/Content/Panel_music/txt_value").GetComponent<Text>();
        this.textSysSoundEffect = this.transSys.Find("Panel_audio/Viewport/Content/Panel_sound/txt_value").GetComponent<Text>();
        this.textSysSoundVoice = this.transSys.Find("Panel_audio/Viewport/Content/Panel_voice/txt_value").GetComponent<Text>();
        this.textSysySoundMain = this.transSys.Find("Panel_audio/Viewport/Content/Panel_main/txt_value").GetComponent<Text>();
        this.dpSysResolution.ClearOptions();
        this.dpSysEffect.ClearOptions();
        this.dpSysShadow.ClearOptions();
        this.dpSysShadowDistance.ClearOptions();
        this.dpSysVsync.ClearOptions();
        this.dpSysAntialiasing.ClearOptions();
        this.togSysVideo.onValueChanged.RemoveAllListeners();
        this.togSysVideo.onValueChanged.AddListener(new UnityAction<bool>(this.OnTogSysVideoValueChange));
        this.togSysAudio.onValueChanged.RemoveAllListeners();
        this.togSysAudio.onValueChanged.AddListener(new UnityAction<bool>(this.OnTogSysAudioValueChange));
        this.togMainSoundEffectOpen.onValueChanged.RemoveAllListeners();
        this.togMainSoundEffectOpen.onValueChanged.AddListener(new UnityAction<bool>(this.OnTogMainOpenValueChange));
        this.togMainSoundEffectClose.onValueChanged.RemoveAllListeners();
        this.togMainSoundEffectClose.onValueChanged.AddListener(new UnityAction<bool>(this.OnTogMainCloseValueChange));
        this.togTeamSoundEffectClose.onValueChanged.RemoveAllListeners();
        this.togTeamSoundEffectClose.onValueChanged.AddListener(new UnityAction<bool>(this.OnTogTeamCloseValueChange));
        this.togTeamSoundEffectOpen.onValueChanged.RemoveAllListeners();
        this.togTeamSoundEffectOpen.onValueChanged.AddListener(new UnityAction<bool>(this.OnTogTeamOpenValueChange));
        this.togOtherSoundEffectOpen.onValueChanged.RemoveAllListeners();
        this.togOtherSoundEffectOpen.onValueChanged.AddListener(new UnityAction<bool>(this.OnTogOtherOpenValueChange));
        this.togOtherSoundEffectClose.onValueChanged.RemoveAllListeners();
        this.togOtherSoundEffectClose.onValueChanged.AddListener(new UnityAction<bool>(this.OnTogOtherCloseValueChange));
        this.togNPCSoundEffectOpen.onValueChanged.RemoveAllListeners();
        this.togNPCSoundEffectOpen.onValueChanged.AddListener(new UnityAction<bool>(this.OnTogNPCOpenValueChange));
        this.togNPCSoundEffectClose.onValueChanged.RemoveAllListeners();
        this.togNPCSoundEffectClose.onValueChanged.AddListener(new UnityAction<bool>(this.OnTogNPCCloseValueChange));
        this.togSysScreen.onValueChanged.RemoveAllListeners();
        this.togSysScreen.onValueChanged.AddListener(new UnityAction<bool>(this.OnTogSysScreenValueChange));
        UIEventListener.Get(this.togSysScreen.gameObject).onClick = new UIEventListener.VoidDelegate(this.TogSysScreenClick);
        this.togSysWindow.onValueChanged.RemoveAllListeners();
        this.togSysWindow.onValueChanged.AddListener(new UnityAction<bool>(this.OnTogSysWindowValueChange));
        UIEventListener.Get(this.togSysWindow.gameObject).onClick = new UIEventListener.VoidDelegate(this.TogSysWindowClick);
        this.togSysFull.onValueChanged.RemoveAllListeners();
        this.togSysFull.onValueChanged.AddListener(new UnityAction<bool>(this.OnTogSysFullValueChange));
        this.btnSysExit.onClick.RemoveAllListeners();
        this.btnSysExit.onClick.AddListener(new UnityAction(this.OnBtnSysExitClick));
        this.btnSysDefault.onClick.RemoveAllListeners();
        this.btnSysDefault.onClick.AddListener(new UnityAction(this.OnBtnSysDefaultClick));
        this.btnSysApply.onClick.RemoveAllListeners();
        this.btnSysApply.onClick.AddListener(new UnityAction(this.OnBtnSysApplyClick));
        this.dpSysResolution.onValueChanged.RemoveAllListeners();
        this.dpSysResolution.onValueChanged.AddListener(new UnityAction<int>(this.OnDpSysResolutionValueChange));
        this.dpSysEffect.onValueChanged.RemoveAllListeners();
        this.dpSysEffect.onValueChanged.AddListener(new UnityAction<int>(this.OnDpSysEffectValueChange));
        this.dpSysShadow.onValueChanged.RemoveAllListeners();
        this.dpSysShadow.onValueChanged.AddListener(new UnityAction<int>(this.OnDpSysShadowValueChange));
        this.dpSysShadowDistance.onValueChanged.RemoveAllListeners();
        this.dpSysShadowDistance.onValueChanged.AddListener(new UnityAction<int>(this.OnDpSysShadowDistanceValueChange));
        this.dpSysVsync.onValueChanged.RemoveAllListeners();
        this.dpSysVsync.onValueChanged.AddListener(new UnityAction<int>(this.OnDpSysVsyncValueChange));
        this.dpSysAntialiasing.onValueChanged.RemoveAllListeners();
        this.dpSysAntialiasing.onValueChanged.AddListener(new UnityAction<int>(this.OnDpSysAntialiasingValueChange));
        this.sliderCameraMaxdistance.onValueChanged.RemoveAllListeners();
        this.sliderCameraMaxdistance.onValueChanged.AddListener(new UnityAction<float>(this.OnSliderCameraMaxdistanceValueChange));
        this.sliderUIScale.onValueChanged.RemoveAllListeners();
        this.sliderUIScale.onValueChanged.AddListener(new UnityAction<float>(this.OnSliderUIScaleValueChange));
        this.sliderMouseSpeed.onValueChanged.RemoveAllListeners();
        this.sliderMouseSpeed.onValueChanged.AddListener(new UnityAction<float>(this.OnSliderMouseSpeedValueChange));
        this.sliderPixelPercent.onValueChanged.RemoveAllListeners();
        this.sliderPixelPercent.onValueChanged.AddListener(new UnityAction<float>(this.OnSliderPixelPercentValueChange));
        this.sliderSysSoundBg.onValueChanged.RemoveAllListeners();
        this.sliderSysSoundBg.onValueChanged.AddListener(new UnityAction<float>(this.OnSliderSysSoundBgValueChange));
        this.sliderSysSoundEffect.onValueChanged.RemoveAllListeners();
        this.sliderSysSoundEffect.onValueChanged.AddListener(new UnityAction<float>(this.OnSliderSysSoundEffectValueChange));
        this.sliderSysSoundVoice.onValueChanged.RemoveAllListeners();
        this.sliderSysSoundVoice.onValueChanged.AddListener(new UnityAction<float>(this.OnSliderSysSoundVoiceValueChange));
        this.sliderSysSoundMain.onValueChanged.RemoveAllListeners();
        this.sliderSysSoundMain.onValueChanged.AddListener(new UnityAction<float>(this.OnSliderSysSoundMainValueChange));
        this.curScreenData = new ResolutionInfo();
        this.curSoundData = new SystemSettingData_Sound();
        this.curQualityData = new SystemSettingData_Quality();
        this.togSysFull.gameObject.SetActive(false);
        this.SetCameraDistanceSlider();
        this.togSysScreen.isOn = false;
        this.togSysWindow.isOn = false;
        this.togSysFull.isOn = false;
    }

    private void TogSysScreenClick(PointerEventData eventData)
    {
        this.checkUIScale = true;
        this.OnDpSysResolutionValueChange(0);
    }

    private void TogSysWindowClick(PointerEventData eventData)
    {
        this.checkUIScale = true;
        this.OnDpSysResolutionValueChange(this.curScreenData.ReferenceResolution);
    }

    private void InitDropDown()
    {
        this.dpSysEffect.ClearOptions();
        List<string> list = new List<string>();
        for (int i = 0; i < this.settingControl.screenSettingConfig.Count; i++)
        {
            list.Add(this.settingControl.screenSettingConfig[i].GetField_String("name"));
        }
        this.dpSysEffect.AddOptions(list);
        this.dpSysAntialiasing.ClearOptions();
        this.dpSysAntialiasing.AddOptions(this.settingControl.antialiasingConfig);
        this.dpSysShadow.ClearOptions();
        this.dpSysShadow.AddOptions(this.settingControl.shadowsConfig);
        this.dpSysShadowDistance.ClearOptions();
        this.dpSysShadowDistance.AddOptions(this.settingControl.shadowdistanceConfig);
        this.dpSysVsync.ClearOptions();
        this.dpSysVsync.AddOptions(this.settingControl.vsyncConfig);
        this.CheckDpResolution();
    }

    private void InitSoundUI(SystemSettingData_Sound data)
    {
        this.sliderSysSoundMain.value = (float)data.MainSound / 100f;
        this.sliderSysSoundBg.value = (float)data.BgMusic / 100f;
        this.sliderSysSoundEffect.value = (float)data.EffectMusic / 100f;
        this.sliderSysSoundVoice.value = (float)data.Voice / 100f;
        this.textSysySoundMain.text = data.MainSound.ToString();
        this.textSysSoundBg.text = data.BgMusic.ToString();
        this.textSysSoundEffect.text = data.EffectMusic.ToString();
        this.textSysSoundVoice.text = data.Voice.ToString();
        this.togMainSoundEffectOpen.isOn = data.IsMain;
        this.togMainSoundEffectClose.isOn = !data.IsMain;
        this.togTeamSoundEffectOpen.isOn = data.IsTeam;
        this.togTeamSoundEffectClose.isOn = !data.IsTeam;
        this.togOtherSoundEffectOpen.isOn = data.IsOther;
        this.togOtherSoundEffectClose.isOn = !data.IsOther;
        this.togNPCSoundEffectOpen.isOn = data.IsNPC;
        this.togNPCSoundEffectClose.isOn = !data.IsNPC;
        if (this.recoverDefault)
        {
            return;
        }
        AudioCtrl.SetMusicVolume((float)data.BgMusic * this.sliderSysSoundMain.value);
        AudioCtrl.SetSoundVolume((float)data.EffectMusic * this.sliderSysSoundMain.value);
        AudioCtrl.SetVoiceVolume((float)data.Voice * this.sliderSysSoundMain.value);
    }

    private void CheckDpResolution()
    {
        this.dpSysResolution.ClearOptions();
        List<string> list = new List<string>();
        for (int i = 0; i < this.settingControl.resolutionList.Count; i++)
        {
            if (this.curScreenData.FullScreen != 1)
            {
                list.Add(this.settingControl.resolutionList[i]);
            }
        }
        if (list.Count == 0)
        {
            list.Add(this.settingControl.resolutionList[this.settingControl.resolutionList.Count - 1]);
            this.dpSysResolution.AddOptions(list);
        }
        else
        {
            this.dpSysResolution.AddOptions(list);
        }
        bool flag = false;
        for (int j = 0; j < list.Count; j++)
        {
            if (list[j] == this.settingControl.resolutionList[this.curScreenData.ReferenceResolution])
            {
                this.checkUIScale = false;
                this.dpSysResolution.value = j;
                flag = true;
                break;
            }
        }
        if (!flag)
        {
            for (int k = 0; k < list.Count; k++)
            {
                if (list[k] == this.settingControl.resolutionList[this.curScreenData.CurResolution])
                {
                    this.checkUIScale = false;
                    this.dpSysResolution.value = k;
                    flag = true;
                    break;
                }
            }
        }
        if (!flag)
        {
            this.dpSysResolution.value = this.dpSysResolution.options.Count - 1;
        }
    }

    public void OpenGameSettingSys()
    {
        this.togSysVideo.isOn = true;
        this.togSysAudio.isOn = false;
        this.transSysVideo.gameObject.SetActive(true);
        this.transSysAudio.gameObject.SetActive(false);
        this.InitGameSetting_Sys(this.settingControl.screenData, this.settingControl.qualityData);
        this.checkUIScale = true;
    }

    public void InitGameSetting_Sys(ResolutionInfo data, SystemSettingData_Quality quality)
    {
        this.transSys.gameObject.SetActive(true);
        if (data != null)
        {
            this.curScreenData.FullScreen = data.FullScreen;
            this.curScreenData.Height = data.Height;
            this.curScreenData.Width = data.Width;
            this.curScreenData.ReferenceResolution = (this.curScreenData.CurResolution = data.ReferenceResolution);
            this.curScreenData.ModeIndex = data.ModeIndex;
            this.curScreenData.CameraMaxdistance = data.CameraMaxdistance;
            this.curScreenData.UIScale = data.UIScale;
            this.curScreenData.mouseSpeed = data.mouseSpeed;
            this.curScreenData.pixelpercent = data.pixelpercent;
            this.InitScreenUI(this.curScreenData);
        }
        if (this.settingControl.soundData != null)
        {
            this.curSoundData.MainSound = this.settingControl.soundData.MainSound;
            this.curSoundData.BgMusic = this.settingControl.soundData.BgMusic;
            this.curSoundData.EffectMusic = this.settingControl.soundData.EffectMusic;
            this.curSoundData.Voice = this.settingControl.soundData.Voice;
            this.curSoundData.IsMain = this.settingControl.soundData.IsMain;
            this.curSoundData.IsOther = this.settingControl.soundData.IsOther;
            this.curSoundData.IsTeam = this.settingControl.soundData.IsTeam;
            this.curSoundData.IsNPC = this.settingControl.soundData.IsNPC;
        }
        else
        {
            this.curSoundData.MainSound = this.halfVolume * 2;
            this.curSoundData.BgMusic = this.halfVolume;
            this.curSoundData.EffectMusic = this.halfVolume;
            this.curSoundData.Voice = this.halfVolume;
            this.curSoundData.IsTeam = true;
            this.curSoundData.IsOther = true;
            this.curSoundData.IsTeam = true;
            this.curSoundData.IsNPC = true;
        }
        if (quality != null)
        {
            this.curQualityData.Antialiasing = quality.Antialiasing;
            this.curQualityData.QualityLevel = quality.QualityLevel;
            this.curQualityData.Shadow = quality.Shadow;
            this.curQualityData.ShadowDistance = quality.ShadowDistance;
            this.curQualityData.IsAutoSetting = quality.IsAutoSetting;
            this.curQualityData.Vsync = quality.Vsync;
        }
        this.isScreenChange = false;
        this.isSoundChange = false;
        this.isQualityChange = false;
        this.InitDropDown();
        this.InitSoundUI(this.curSoundData);
        this.QualityValueChange(this.dpSysEffect.value);
        this.SetShowStateToggle(this.curScreenData.FullScreen);
        this.CheckSysButtonState();
    }

    private void InitScreenUI(ResolutionInfo data)
    {
        this.SetShowStateToggle(data.FullScreen);
        this.sliderCameraMaxdistance.value = (float)data.CameraMaxdistance;
        this.txtCameraMaxdistance.text = data.CameraMaxdistance.ToString();
        this.sliderUIScale.value = (float)data.UIScale;
        this.txtUIScale.text = data.UIScale.ToString();
        this.sliderMouseSpeed.value = data.mouseSpeed;
        this.txtMouseSpeed.text = data.mouseSpeed.ToString();
        this.sliderPixelPercent.value = data.pixelpercent;
        this.txtPixelPercent.text = data.pixelpercent.ToString();
    }

    public void OnTogSysVideoValueChange(bool state)
    {
        if (!state)
        {
            return;
        }
        this.transSysVideo.gameObject.SetActive(true);
        this.transSysAudio.gameObject.SetActive(false);
    }

    public void OnTogSysAudioValueChange(bool state)
    {
        if (!state)
        {
            return;
        }
        this.transSysVideo.gameObject.SetActive(false);
        this.transSysAudio.gameObject.SetActive(true);
    }

    public void OnTogMainOpenValueChange(bool state)
    {
        if (!state)
        {
            return;
        }
        this.curSoundData.IsMain = state;
        this.CheckChangeSoundData();
        this.InitSoundUI(this.curSoundData);
    }

    public void OnTogMainCloseValueChange(bool state)
    {
        if (!state)
        {
            return;
        }
        this.curSoundData.IsMain = !state;
        this.CheckChangeSoundData();
        this.InitSoundUI(this.curSoundData);
    }

    public void OnTogTeamOpenValueChange(bool state)
    {
        if (!state)
        {
            return;
        }
        this.curSoundData.IsTeam = state;
        this.CheckChangeSoundData();
        this.InitSoundUI(this.curSoundData);
    }

    public void OnTogTeamCloseValueChange(bool state)
    {
        if (!state)
        {
            return;
        }
        this.curSoundData.IsTeam = !state;
        this.CheckChangeSoundData();
        this.InitSoundUI(this.curSoundData);
    }

    public void OnTogOtherOpenValueChange(bool state)
    {
        if (!state)
        {
            return;
        }
        this.curSoundData.IsOther = state;
        this.CheckChangeSoundData();
        this.InitSoundUI(this.curSoundData);
    }

    public void OnTogOtherCloseValueChange(bool state)
    {
        if (!state)
        {
            return;
        }
        this.curSoundData.IsOther = !state;
        this.CheckChangeSoundData();
        this.InitSoundUI(this.curSoundData);
    }

    public void OnTogNPCOpenValueChange(bool state)
    {
        if (!state)
        {
            return;
        }
        this.curSoundData.IsNPC = state;
        this.CheckChangeSoundData();
        this.InitSoundUI(this.curSoundData);
    }

    public void OnTogNPCCloseValueChange(bool state)
    {
        if (!state)
        {
            return;
        }
        this.curSoundData.IsNPC = !state;
        this.CheckChangeSoundData();
        this.InitSoundUI(this.curSoundData);
    }

    public void OnTogSysScreenValueChange(bool state)
    {
        if (!state)
        {
            return;
        }
        this.curScreenData.FullScreen = 1;
        this.CheckDpResolution();
        this.CheckChangeScreenData(false);
        this.dpSysResolution.interactable = false;
        this.resolutionOption.gameObject.SetActive(false);
    }

    public void OnTogSysWindowValueChange(bool state)
    {
        if (!state)
        {
            return;
        }
        this.resolutionOption.gameObject.SetActive(true);
        this.dpSysResolution.interactable = true;
        this.curScreenData.FullScreen = 2;
        this.CheckDpResolution();
        this.CheckChangeScreenData(false);
    }

    public void OnTogSysFullValueChange(bool state)
    {
        if (!state)
        {
            return;
        }
        this.curScreenData.FullScreen = 3;
        this.CheckDpResolution();
        this.CheckChangeScreenData(false);
    }

    public void OnBtnSysExitClick()
    {
        this.CloseSysSetting();
    }

    private void CloseSysSetting()
    {
        this.InitGameSetting_Sys(this.settingControl.screenData, this.settingControl.qualityData);
        this.transSys.gameObject.SetActive(false);
        this.ActiveMenuObj(true);
    }

    public void OnBtnSysDefaultClick()
    {
        this.recoverDefault = true;
        this.InitScreenUI(this.settingControl.defaultScreenData);
        this.InitSoundUI(this.settingControl.defaultSoundData);
        this.InitQualityUI(this.settingControl.defaultQualityData);
        this.recoverDefault = false;
    }

    public void OnBtnSysApplyClick()
    {
        if (this.transSysAudio.gameObject.activeSelf)
        {
            this.ApplySys();
        }
        else
        {
            this.settingControl.ApplyScreenData(this.curScreenData, this.curQualityData);
            this.settingControl.ApplyQualityData(this.curQualityData);
            this.InitGameSetting_MessageBox(UI_GameSetting.MessageBox_Type.SecondConfirm, delegate
            {
                this.ApplySys();
            }, delegate
            {
                this.InitGameSetting_Sys(this.settingControl.screenData, this.settingControl.qualityData);
                this.ApplySys();
                this.settingControl.ApplyScreenData(this.curScreenData, this.curQualityData);
            });
        }
    }

    private void ApplySys()
    {
        if (this.isScreenChange)
        {
        }
        if (this.isSoundChange)
        {
        }
        this.settingControl.SaveLocalData(this.curScreenData, this.curSoundData, this.curQualityData);
        this.checkUIScale = true;
    }

    public void OnDpSysResolutionValueChange(int value)
    {
        RectTransform component = this.dpSysResolution.transform.Find("Template/Viewport/Content").GetComponent<RectTransform>();
        float y = this.dpSysResolution.transform.Find("Template").GetComponent<RectTransform>().sizeDelta.y;
        if ((float)this.dpSysResolution.options.Count > y / (float)this.resolutionItemHeight)
        {
            float y2 = (float)Mathf.Min(this.resolutionItemHeight * value, (this.dpSysResolution.options.Count + 1) / 2 * this.resolutionItemHeight + this.resolutionItemHeight);
            component.localPosition = new Vector2(component.localPosition.x, y2);
        }
        else
        {
            component.localPosition = new Vector2(component.localPosition.x, 0f);
        }
        this.curScreenData.ReferenceResolution = this.settingControl.CheckResolutionIndex(this.dpSysResolution.options[value].text);
        this.curScreenData.Width = this.dpSysResolution.options[value].text.Split(new char[]
        {
            'X'
        })[0];
        this.curScreenData.Height = this.dpSysResolution.options[value].text.Split(new char[]
        {
            'X'
        })[1];
        this.SetMinPixelPercentSlider(this.curScreenData.Width.ToInt());
        int resolutionUIScale = this.GetResolutionUIScale(this.curScreenData.Width, this.curScreenData.Height);
        if (resolutionUIScale != 0 && this.checkUIScale)
        {
            this.curScreenData.UIScale = resolutionUIScale;
        }
        this.checkUIScale = true;
        this.CheckChangeScreenData(false);
    }

    private void SetMinPixelPercentSlider(int w)
    {
        int num = 72000 / w;
        num = Mathf.Clamp(num, 1, 80);
        this.sliderPixelPercent.minValue = (float)num;
        this.txtMinPixelPercent.text = num.ToString();
    }

    public void OnDpSysEffectValueChange(int value)
    {
        this.QualityValueChange(value);
        this.CheckChangeQualityData();
    }

    private void QualityValueChange(int value)
    {
        if (value == 0)
        {
            this.curQualityData.IsAutoSetting = true;
            this.SetQualityValue((QualityShadow)this.curQualityData.Shadow, (QualityShadow)this.curQualityData.ShadowDistance, this.curQualityData.Vsync, (QualityMSAA)this.curQualityData.Antialiasing);
        }
        else
        {
            this.curQualityData.IsAutoSetting = false;
            this.curQualityData.QualityLevel = value;
            switch (value)
            {
                case 1:
                    this.SetQualityValue(QualityShadow.Low, QualityShadow.Low, false, QualityMSAA.Low);
                    break;
                case 2:
                    this.SetQualityValue(QualityShadow.Mid, QualityShadow.Mid, true, QualityMSAA.Mid);
                    break;
                case 3:
                    this.SetQualityValue(QualityShadow.High, QualityShadow.High, true, QualityMSAA.High);
                    break;
            }
        }
    }

    private void SetQualityValue(QualityShadow shadow, QualityShadow disance, bool vsync, QualityMSAA anti)
    {
        this.curQualityData.Shadow = (int)shadow;
        this.curQualityData.ShadowDistance = (int)disance;
        this.curQualityData.Vsync = vsync;
        this.curQualityData.Antialiasing = (int)anti;
        this.InitQualityUI(this.curQualityData);
    }

    private void InitQualityUI(SystemSettingData_Quality data)
    {
        this.dpSysShadow.value = data.Shadow;
        this.dpSysShadowDistance.value = data.ShadowDistance;
        this.dpSysAntialiasing.value = data.Antialiasing;
        this.dpSysVsync.value = ((!data.Vsync) ? 0 : 1);
        this.dpSysEffect.value = ((!data.IsAutoSetting) ? this.curQualityData.QualityLevel : 0);
    }

    private bool IsShadowValueBelongQuality(int value, int quality)
    {
        if (quality == 1)
        {
            return value == 0;
        }
        if (quality == 2)
        {
            return value == 1;
        }
        return value == 2;
    }

    private bool IsMsaaValueBelongQuality(int value, int quality)
    {
        if (quality == 1)
        {
            return value == 1;
        }
        if (quality == 2)
        {
            return value == 2;
        }
        return value == 3;
    }

    private bool IsVsyncValueBelongQuality(int quality)
    {
        if (quality == 1)
        {
            return !this.curQualityData.Vsync;
        }
        return this.curQualityData.Vsync;
    }

    public void OnDpSysShadowValueChange(int value)
    {
        this.curQualityData.Shadow = value;
        if (!this.curQualityData.IsAutoSetting && !this.IsShadowValueBelongQuality(value, this.curQualityData.QualityLevel))
        {
            this.curQualityData.IsAutoSetting = true;
            this.dpSysEffect.value = 0;
        }
        this.CheckChangeQualityData();
    }

    public void OnDpSysShadowDistanceValueChange(int value)
    {
        this.curQualityData.ShadowDistance = value;
        if (!this.curQualityData.IsAutoSetting && !this.IsShadowValueBelongQuality(value, this.curQualityData.QualityLevel))
        {
            this.curQualityData.IsAutoSetting = true;
            this.dpSysEffect.value = 0;
        }
        this.CheckChangeQualityData();
    }

    public void OnDpSysVsyncValueChange(int value)
    {
        this.curQualityData.Vsync = (value != 0);
        if (!this.curQualityData.IsAutoSetting && !this.IsVsyncValueBelongQuality(this.curQualityData.QualityLevel))
        {
            this.curQualityData.IsAutoSetting = true;
            this.dpSysEffect.value = 0;
        }
        this.CheckChangeQualityData();
    }

    public void OnDpSysAntialiasingValueChange(int value)
    {
        this.curQualityData.Antialiasing = value;
        if (!this.curQualityData.IsAutoSetting && !this.IsMsaaValueBelongQuality(value, this.curQualityData.QualityLevel))
        {
            this.curQualityData.IsAutoSetting = true;
            this.dpSysEffect.value = 0;
        }
        this.CheckChangeQualityData();
    }

    private void SetCameraDistanceSlider()
    {
        LuaTable xmlConfigTable = LuaConfigManager.GetXmlConfigTable("massiveConfig");
        float cacheField_Float = xmlConfigTable.GetCacheField_Table("CameraMaxdistance").GetCacheField_Float("value");
        float cacheField_Float2 = xmlConfigTable.GetCacheField_Table("CameraMindistance").GetCacheField_Float("value");
        this.sliderCameraMaxdistance.maxValue = cacheField_Float;
        this.sliderCameraMaxdistance.minValue = cacheField_Float2;
    }

    private void OnSliderCameraMaxdistanceValueChange(float value)
    {
        this.txtCameraMaxdistance.text = value.ToString();
        this.curScreenData.CameraMaxdistance = value.ToInt();
        this.CheckChangeScreenData(false);
        if (this.recoverDefault)
        {
            return;
        }
        if (null != CameraController.Self)
        {
            CameraController.Self.SetMaxDistance(value);
        }
    }

    private void OnSliderUIScaleValueChange(float value)
    {
        this.txtUIScale.text = value.ToString();
        this.curScreenData.UIScale = value.ToInt();
        this.CheckChangeScreenData(false);
        if (this.recoverDefault)
        {
            return;
        }
        CommonTools.SetUIScale((int)value);
    }

    private void OnSliderMouseSpeedValueChange(float value)
    {
        this.txtMouseSpeed.text = value.ToString();
        this.curScreenData.mouseSpeed = (uint)value.ToInt();
        this.CheckChangeScreenData(false);
        if (this.recoverDefault)
        {
            return;
        }
    }

    private void OnSliderPixelPercentValueChange(float value)
    {
        this.txtPixelPercent.text = value.ToString();
        this.curScreenData.pixelpercent = (uint)value.ToInt();
        this.CheckChangeScreenData(false);
        if (this.recoverDefault)
        {
            return;
        }
    }

    public void OnSliderSysSoundBgValueChange(float value)
    {
        this.curSoundData.BgMusic = (value * 100f).ToInt();
        this.CheckChangeSoundData();
        if (this.recoverDefault)
        {
            return;
        }
        this.InitSoundUI(this.curSoundData);
    }

    public void OnSliderSysSoundEffectValueChange(float value)
    {
        this.curSoundData.EffectMusic = (value * 100f).ToInt();
        this.CheckChangeSoundData();
        if (this.recoverDefault)
        {
            return;
        }
        this.InitSoundUI(this.curSoundData);
    }

    public void OnSliderSysSoundVoiceValueChange(float value)
    {
        this.curSoundData.Voice = (value * 100f).ToInt();
        this.CheckChangeSoundData();
        if (this.recoverDefault)
        {
            return;
        }
        this.InitSoundUI(this.curSoundData);
    }

    public void OnSliderSysSoundMainValueChange(float value)
    {
        this.curSoundData.MainSound = (value * 100f).ToInt();
        this.CheckChangeSoundData();
        if (this.recoverDefault)
        {
            return;
        }
        this.InitSoundUI(this.curSoundData);
    }

    private void CheckChangeScreenData(bool isDp = false)
    {
        if (this.settingControl.screenData == null)
        {
            return;
        }
        if (ServerStorageManager.Instance.SerializeClassLocal<ResolutionInfo>(this.settingControl.screenData) == ServerStorageManager.Instance.SerializeClassLocal<ResolutionInfo>(this.curScreenData))
        {
            this.isScreenChange = false;
        }
        else
        {
            this.isScreenChange = true;
        }
        this.CheckSysButtonState();
    }

    private void CheckChangeSoundData()
    {
        if (this.settingControl.soundData == null)
        {
            return;
        }
        if (ServerStorageManager.Instance.SerializeClass<SystemSettingData_Sound>(this.settingControl.soundData) == ServerStorageManager.Instance.SerializeClass<SystemSettingData_Sound>(this.curSoundData))
        {
            this.isSoundChange = false;
        }
        else
        {
            this.isSoundChange = true;
        }
        this.CheckSysButtonState();
    }

    private void CheckChangeQualityData()
    {
        if (this.settingControl.qualityData == null)
        {
            return;
        }
        if (ServerStorageManager.Instance.SerializeClass<SystemSettingData_Quality>(this.settingControl.qualityData) == ServerStorageManager.Instance.SerializeClass<SystemSettingData_Quality>(this.curQualityData))
        {
            this.isQualityChange = false;
        }
        else
        {
            this.isQualityChange = true;
        }
        this.CheckSysButtonState();
    }

    private void CheckSysButtonState()
    {
        if (!this.isScreenChange && !this.isSoundChange && !this.isQualityChange)
        {
            this.btnSysApply.interactable = false;
        }
        else
        {
            this.btnSysApply.interactable = true;
        }
    }

    private void SetShowStateToggle(int state)
    {
        if (state == 1)
        {
            this.togSysScreen.isOn = true;
        }
        else if (state == 2)
        {
            this.togSysWindow.isOn = true;
        }
        else if (state == 3)
        {
            this.togSysFull.isOn = true;
        }
    }

    public int GetResolutionUIScale(string width, string height)
    {
        for (int i = 0; i < this.settingControl.windowratioConfig.Count; i++)
        {
            int cacheField_Int = this.settingControl.windowratioConfig[i].GetCacheField_Int("width");
            int cacheField_Int2 = this.settingControl.windowratioConfig[i].GetCacheField_Int("hight");
            if (cacheField_Int == width.ToInt() && cacheField_Int2 == height.ToInt())
            {
                return int.Parse(this.settingControl.windowratioConfig[i].GetField_String("scale"));
            }
        }
        return 0;
    }

    public Transform mRoot;

    private Transform transMenu;

    private Transform transSys;

    private Transform transGame;

    private Transform transMessageBox;

    private Transform transAutoFight;

    private Button btnOutStuck;

    private Button btnSys;

    private Button btnGame;

    private Button btnKeyboard;

    private Button btnCharacter;

    private Button btnExit;

    private Button btnClose;

    private ShortcutsConfigController shortControll_;

    private AutoFightController autoFightControll_;

    private Button btnAutoFightClose;

    private Toggle toggleAutoPick;

    private Transform conditionListRoot;

    private Button btnResumeDefault;

    private Button btnApply;

    private MyJson.JsonNode_Object autoFightDataOriginal;

    private MyJson.JsonNode_Object autoFightData;

    private string autoPickJsonKey = "auto_pick";

    private string skillsJsonKey = "skill_settings";

    private List<AutoFightCfg> cfgListType = new List<AutoFightCfg>();

    private Toggle togGamePlayerBar;

    private Toggle togGamePlayerName;

    private Toggle togGamePlayerGuild;

    private Toggle togGameOthersBar;

    private Toggle togGameOthersName;

    private Toggle togGameOthersGuild;

    private Toggle togGameEnemyBar;

    private Toggle togGameEnemyName;

    private Toggle togGameEnemyGuild;

    private Toggle togGameNPCName;

    private Toggle togGameAllowPartyInvite;

    private Toggle togGameAllowGuildInvite;

    private Toggle togGameAllowFriendInvite;

    private Toggle togGameHealthWarning;

    private Toggle togGameMouseMove;

    private Toggle togGame2ndPw;

    private Button btnGameExit;

    private Button btnGameDefault;

    private Button btnGameApply;

    private Button btnGameChange2ndPw;

    private Button btnAutoFight;

    private SystemSettingData_Funtion curFuntionData;

    private SystemSettingData_Show curShowData;

    private bool isFuntionChange;

    private bool isShowChange;

    private Text txtMsgExit;

    private Text txtMsgExitCD;

    private Text txtMsgReturn;

    private Text txtMsgReturnCD;

    private Text txtMsgConfirm;

    private Text txtMsgConfirmCD;

    private Button btnMsgOK;

    private Button btnMsgCancel;

    private UI_GameSetting.MessageBox_Type _type;

    private float _lastTime;

    private float CD_TIME = 20f;

    private Action _okAction;

    private Action _cancelAction;

    private Transform transSysVideo;

    private Transform transSysAudio;

    private Toggle togSysVideo;

    private Toggle togSysAudio;

    private Toggle togSysScreen;

    private Toggle togSysWindow;

    private Toggle togSysFull;

    private Button btnSysExit;

    private Button btnSysDefault;

    private Button btnSysApply;

    private ToggleGroup tgSysViewModel;

    private GameObject resolutionOption;

    private Dropdown dpSysResolution;

    private Dropdown dpSysEffect;

    private Dropdown dpSysShadow;

    private Dropdown dpSysShadowDistance;

    private Dropdown dpSysVsync;

    private Dropdown dpSysAntialiasing;

    private Slider sliderCameraMaxdistance;

    private Slider sliderUIScale;

    private Text txtCameraMaxdistance;

    private Text txtUIScale;

    private Slider sliderMouseSpeed;

    private Text txtMouseSpeed;

    private Slider sliderPixelPercent;

    private Text txtPixelPercent;

    private Text txtMinPixelPercent;

    private Slider sliderSysSoundBg;

    private Slider sliderSysSoundEffect;

    private Slider sliderSysSoundVoice;

    private Slider sliderSysSoundMain;

    private Text textSysSoundBg;

    private Text textSysSoundEffect;

    private Text textSysSoundVoice;

    private Text textSysySoundMain;

    private int halfVolume = 50;

    private int resolutionItemHeight = 20;

    private Toggle togMainSoundEffectOpen;

    private Toggle togMainSoundEffectClose;

    private Toggle togOtherSoundEffectOpen;

    private Toggle togOtherSoundEffectClose;

    private Toggle togTeamSoundEffectOpen;

    private Toggle togTeamSoundEffectClose;

    private Toggle togNPCSoundEffectOpen;

    private Toggle togNPCSoundEffectClose;

    private ResolutionInfo curScreenData;

    private SystemSettingData_Sound curSoundData;

    private SystemSettingData_Quality curQualityData;

    private bool isScreenChange;

    private bool isSoundChange;

    private bool isQualityChange;

    private bool checkUIScale = true;

    private bool recoverDefault;

    public enum MessageBox_Type
    {
        ReturnSelectRole = 1,
        ExitGame,
        SecondConfirm
    }
}
