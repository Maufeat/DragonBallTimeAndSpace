using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Framework.Managers;
using Game.Scene;
using LuaInterface;
using msg;
using Net;
using UI.Login;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityStandardAssets.CinematicEffects;

public class UI_SelectRole : UIPanelBase
{
    private AVPlayOP avPlayOP
    {
        get
        {
            if (this.avPlayOP_ == null)
            {
                this.avPlayOP_ = UnityEngine.Object.FindObjectOfType<AVPlayOP>();
            }
            return this.avPlayOP_;
        }
    }

    private List<UI_SelectRole.HeroBaseData> hbdl
    {
        get
        {
            if (this.hbdl_ == null)
            {
                this.hbdl_ = this.InitHeroBaseData();
            }
            return this.hbdl_;
        }
        set
        {
            this.hbdl_ = value;
        }
    }

    public Dictionary<uint, UI_SelectRole.HeroBaseData> hbdDic
    {
        get
        {
            if (this.hbdDic_ == null)
            {
                this.InitHeroBaseData();
            }
            return this.hbdDic_;
        }
    }

    public void SetCharData(MSG_Ret_LoginOnReturnCharList_SC data)
    {
        this.charData = data;
        this.charData.charList.Sort((SelectUserInfo a, SelectUserInfo b) => b.offlinetime.CompareTo(a.offlinetime));
        this.ClearModelCache();
        this.InitUI();
    }

    public override void OnInit(Transform root)
    {
        CommonTools.SetScalerMode(CanvasScaler.ScaleMode.ScaleWithScreenSize);
        base.OnInit(root);
        this.sexSelect = SEX.NONE;
        this.uiRoot = root;
        this.InitSkillTable();
        this.InitObject();
        this.InitEvent();
        Shader.EnableKeyword("CUSTOM_SHADOW");
        if (GameObject.Find("Scene") == null)
        {
            UnityEngine.Object.Instantiate<GameObject>(Resources.Load<GameObject>("Scene"));
        }
        Scheduler.Instance.AddTimer(1f, true, new Scheduler.OnScheduler(this.UpdateProtectTime));
        Scheduler.Instance.AddUpdator(new Scheduler.OnScheduler(this.Update));
        UIManager.Instance.ResetMaskInfo(false);
        UIManager manager = ManagerCenter.Instance.GetManager<UIManager>();
        if (manager != null)
        {
            manager.DeleteUI("UI_Main");
        }
        this.isInLoadHairOrFaceCount = 0;
        this.TryReportClientCrashTimes();
    }

    private void TryReportClientCrashTimes()
    {
        string pattern = "[0-9]{4}[-][0-9]{2}[-][0-9]{2}[_][0-9]{6}";
        string[] directories = Directory.GetDirectories(Environment.CurrentDirectory);
        uint num = 0U;
        for (int i = 0; i < directories.Length; i++)
        {
            if (Regex.IsMatch(directories[i], pattern))
            {
                num += 1U;
            }
        }
        int @int = PlayerPrefs.GetInt("crash_time");
        if ((ulong)num > (ulong)((long)@int))
        {
            PlayerPrefs.SetInt("crash_time", (int)num);
            MSG_Upload_Crash_Info msg_Upload_Crash_Info = new MSG_Upload_Crash_Info();
            msg_Upload_Crash_Info.crashnum = num;
            ManagerCenter.Instance.GetManager<EntitiesManager>().PNetWork.SendMsg<MSG_Upload_Crash_Info>(CommandID.MSG_Upload_Crash_Info, msg_Upload_Crash_Info, false);
        }
    }

    private void InitObject()
    {
        this.tr_windowRootSelect = this.uiRoot.FindChild("Offset_SelectRole/UIPanel_Window_select");
        this.tr_windowRootCreate = this.uiRoot.FindChild("Offset_SelectRole/UIPanel_Window_create");
        this.tr_rtRawImage = this.uiRoot.FindChild("Offset_SelectRole/RawImage_role_show");
        this.tr_btn_SelectBack = this.tr_windowRootSelect.FindChild("btn_back");
        this.tr_txt_serverName = this.tr_windowRootSelect.FindChild("img_bg_name/txt_server_name");
        this.tr_txt_roleCount = this.tr_windowRootSelect.FindChild("txt_role_count");
        this.tr_selectScrollRoot = this.tr_windowRootSelect.FindChild("ScrollbarRect/Rect");
        this.tr_btn_startDelRole = this.tr_windowRootSelect.FindChild("btn_del_role");
        this.tr_btn_cancelDelRole = this.tr_windowRootSelect.FindChild("img_bottom/img_del_end_ensure/btn_cancel_del");
        this.tr_btn_sureDelRole = this.tr_windowRootSelect.FindChild("img_bottom/img_del_end_ensure/btn_ensure_del");
        this.tr_btn_cancelDelRole2 = this.tr_windowRootSelect.FindChild("img_bottom/img_cancel_del/btn_cancel_del");
        this.tr_btn_createCenter = this.tr_windowRootSelect.FindChild("img_bottom/btn_create_role_center");
        this.tr_btn_create = this.tr_windowRootSelect.FindChild("btn_create_role");
        this.tr_btn_startGame = this.tr_windowRootSelect.FindChild("img_bottom/btn_start_game");
        this.tr_btn_cancelDel = this.tr_windowRootSelect.FindChild("img_bottom/img_cancel_del");
        this.tr_roleDelProtectLeftTime = this.tr_windowRootSelect.FindChild("img_bottom/img_cancel_del/txt_del_left_timer").GetComponent<Text>();
        this.tr_btn_cancelDelEnsure = this.tr_windowRootSelect.FindChild("img_bottom/img_del_end_ensure");
        this.tr_panel_hair_shap = this.tr_windowRootCreate.FindChild("Panel_decorate/bg_features/Panel_hair_shap");
        this.tr_panel_hair_color = this.tr_windowRootCreate.FindChild("Panel_decorate/bg_features/Panel_hair_color");
        this.tr_panel_face_shap = this.tr_windowRootCreate.FindChild("Panel_decorate/bg_features/Panel_face_shap");
        this.tr_panel_skill = this.tr_windowRootCreate.FindChild("Panel_decorate/Panel_skill_show");
        this.tr_panel_skill.FindChild("img_skill_vedio").GetComponent<Image>().enabled = false;
        this.tr_mesh_vedio = this.tr_panel_skill.FindChild("img_skill_vedio/mesh_vedio");
        this.tr_scrollRootCreate = this.tr_windowRootCreate.FindChild("Panel_race_select/ScrollbarRect/Rect");
        this.tr_btn_createBack = this.tr_windowRootCreate.FindChild("bottom/btn_back");
        this.tr_txt_create_tip = this.tr_windowRootCreate.FindChild("img_bg/txt_create_tip");
        this.tr_select_role_information = this.tr_windowRootCreate.FindChild("select_role_information");
        this.tr_bottom = this.tr_windowRootCreate.FindChild("bottom");
        this.tr_btn_ok = this.tr_windowRootCreate.FindChild("bottom/btn_ok");
        this.tr_input_name = this.tr_windowRootCreate.FindChild("bottom/input_name");
        this.tr_sex = this.tr_windowRootCreate.FindChild("Panel_decorate/sex_select");
        this.tr_btn_roll = this.tr_windowRootCreate.FindChild("bottom/input_name/btn_roll");
        this.randomFeature = this.tr_windowRootCreate.FindChild("Panel_decorate/btn_random");
        RectTransform component = this.tr_rtRawImage.GetComponent<RectTransform>();
        this.originalRtPos = component.anchoredPosition.x;
        Transform transform = UIManager.Instance.GetUIParent(UIManager.ParentType.Loading).FindChild("Text_version");
        if (transform)
        {
            transform.gameObject.SetActive(false);
        }
    }

    private void InitEvent()
    {
        Button component = this.tr_btn_createCenter.gameObject.GetComponent<Button>();
        component.onClick.AddListener(new UnityAction(this.ToCreateRolePage));
        Button component2 = this.tr_btn_createBack.GetComponent<Button>();
        component2.onClick.AddListener(new UnityAction(this.ToSelectRolePage));
        Button component3 = this.tr_btn_ok.GetComponent<Button>();
        component3.onClick.AddListener(new UnityAction(this.CreateRole));
        Button component4 = this.tr_btn_SelectBack.GetComponent<Button>();
        component4.onClick.AddListener(new UnityAction(this.BackToLogin));
        this.tr_btn_SelectBack.gameObject.SetActive(!ManagerCenter.Instance.GetManager<EntitiesManager>().wegame);
        Button component5 = this.tr_btn_startGame.GetComponent<Button>();
        component5.onClick.AddListener(new UnityAction(this.StartGame));
        Button component6 = this.tr_btn_create.GetComponent<Button>();
        component6.onClick.AddListener(new UnityAction(this.ToCreateRolePage));
        Button component7 = this.tr_btn_startDelRole.GetComponent<Button>();
        component7.onClick.AddListener(new UnityAction(this.StartDelRole));
        Button component8 = this.tr_btn_cancelDelRole.GetComponent<Button>();
        component8.onClick.AddListener(new UnityAction(this.CancelDelRole));
        component8 = this.tr_btn_cancelDelRole2.GetComponent<Button>();
        component8.onClick.AddListener(new UnityAction(this.CancelDelRole));
        Button component9 = this.tr_btn_sureDelRole.GetComponent<Button>();
        component9.onClick.AddListener(new UnityAction(this.SureDelRole));
        Button component10 = this.tr_btn_roll.GetComponent<Button>();
        component10.onClick.AddListener(new UnityAction(this.RollName));
        Button component11 = this.tr_sex.FindChild("toggle_male").GetComponent<Button>();
        component11.onClick.AddListener(delegate ()
        {
            if (this.sexSelect == SEX.MALE)
            {
                return;
            }
            this.curSelectIndex.Clear();
            if (this.modelHangHelperInstance != null && this.modelHangHelperInstance.useID != null)
            {
                for (int i = 0; i < this.modelHangHelperInstance.useID.Length; i++)
                {
                    this.modelHangHelperInstance.useID[i] = 0U;
                }
            }
            this.sexSelect = SEX.MALE;
            this.SetMaleUIState(SEX.MALE, this.lastSelectCharName);
        });
        Button component12 = this.tr_sex.FindChild("toggle_female").GetComponent<Button>();
        component12.onClick.AddListener(delegate ()
        {
            if (this.sexSelect == SEX.FEMALE)
            {
                return;
            }
            this.curSelectIndex.Clear();
            if (this.modelHangHelperInstance != null && this.modelHangHelperInstance.useID != null)
            {
                for (int i = 0; i < this.modelHangHelperInstance.useID.Length; i++)
                {
                    this.modelHangHelperInstance.useID[i] = 0U;
                }
            }
            this.sexSelect = SEX.FEMALE;
            this.SetMaleUIState(SEX.FEMALE, this.lastSelectCharName);
        });
        InputField component13 = this.tr_input_name.GetComponent<InputField>();
        component13.onValueChanged.AddListener(new UnityAction<string>(this.CheckInputName));
        Button component14 = this.randomFeature.GetComponent<Button>();
        component14.onClick.AddListener(new UnityAction(this.RandomFeature));
    }

    private void CheckInputName(string s)
    {
        InputField component = this.tr_input_name.GetComponent<InputField>();
        string text = string.Empty;
        int num = 0;
        for (int i = 0; i < s.Length; i++)
        {
            int num2 = Encoding.UTF8.GetBytes(s[i].ToString()).Length;
            if (num2 > 1)
            {
                num += 2;
            }
            else
            {
                num++;
            }
            if (num > 14)
            {
                break;
            }
            text += s[i].ToString();
        }
        component.text = text;
    }

    private void InitSkillTable()
    {
        List<LuaTable> configTableList = LuaConfigManager.GetConfigTableList("skill_data");
        for (int i = 0; i < configTableList.Count; i++)
        {
            this.skillDataTable[configTableList[i].GetCacheField_Uint("skillid")] = configTableList[i];
        }
    }

    private void InitUI()
    {
        this.tr_windowRootSelect.gameObject.SetActive(false);
        this.tr_windowRootCreate.gameObject.SetActive(false);
        this.tr_rtRawImage.gameObject.SetActive(false);
        this.tr_btn_SelectBack.gameObject.SetActive(false);
        this.tr_txt_roleCount.gameObject.SetActive(false);
        this.tr_selectScrollRoot.gameObject.SetActive(false);
        this.tr_btn_startDelRole.gameObject.SetActive(false);
        this.tr_btn_createCenter.gameObject.SetActive(false);
        this.tr_btn_create.gameObject.SetActive(false);
        this.tr_btn_startGame.gameObject.SetActive(false);
        this.tr_btn_cancelDel.gameObject.SetActive(false);
        this.tr_btn_cancelDelEnsure.gameObject.SetActive(false);
        this.tr_txt_create_tip.gameObject.SetActive(false);
        this.tr_select_role_information.gameObject.SetActive(false);
        this.tr_bottom.gameObject.SetActive(false);
        this.tr_txt_serverName.gameObject.GetComponent<Text>().text = MyPlayerPrefs.GetString("last_login_server");
        this.FillHaveHeroList();
    }

    private void FillHaveHeroList()
    {
        this.tr_btn_SelectBack.gameObject.SetActive(true);
        if (this.charData == null || this.charData.charList == null || this.charData.charList.Count == 0)
        {
            this.tr_windowRootSelect.gameObject.SetActive(true);
            this.tr_btn_createCenter.gameObject.SetActive(true);
            this.tr_txt_roleCount.gameObject.SetActive(true);
            this.tr_txt_roleCount.GetComponent<Text>().text = "0/" + 4;
            this.tr_btn_cancelDelEnsure.gameObject.SetActive(false);
            this.tr_btn_cancelDel.gameObject.SetActive(false);
            this.tr_btn_startDelRole.gameObject.SetActive(false);
            this.tr_btn_create.gameObject.SetActive(false);
            for (int i = 0; i < this.tr_selectScrollRoot.childCount; i++)
            {
                GameObject gameObject = this.tr_selectScrollRoot.GetChild(i).gameObject;
                gameObject.SetActive(false);
            }
            this.ToCreateRolePage();
        }
        else
        {
            this.SetRoleShowRtImagePos(true);
            this.tr_rtRawImage.gameObject.SetActive(true);
            this.tr_windowRootSelect.gameObject.SetActive(true);
            this.tr_selectScrollRoot.gameObject.SetActive(true);
            this.tr_btn_startGame.gameObject.SetActive(true);
            this.tr_btn_startDelRole.gameObject.SetActive(true);
            this.tr_btn_create.gameObject.SetActive(true);
            this.tr_txt_roleCount.gameObject.SetActive(true);
            this.tr_txt_roleCount.GetComponent<Text>().text = this.charData.charList.Count + "/" + 4;
            GameObject gameObject2 = this.tr_selectScrollRoot.FindChild("item").gameObject;
            int num = Mathf.Max(4, this.charData.charList.Count);
            ScrollRect componentInParent = this.tr_selectScrollRoot.GetComponentInParent<ScrollRect>();
            if (null != componentInParent)
            {
                componentInParent.vertical = false;
            }
            List<GameObject> allItems = new List<GameObject>();
            long num2 = long.MinValue;
            GameObject gameObject3 = null;
            for (int j = 0; j < num; j++)
            {
                GameObject itemObj = null;
                if (j < this.tr_selectScrollRoot.childCount)
                {
                    itemObj = this.tr_selectScrollRoot.GetChild(j).gameObject;
                }
                else
                {
                    itemObj = UnityEngine.Object.Instantiate<GameObject>(gameObject2);
                    itemObj.transform.SetParent(this.tr_selectScrollRoot);
                    itemObj.transform.localPosition = Vector3.zero;
                    itemObj.transform.localScale = gameObject2.transform.localScale;
                    itemObj.name = gameObject2.name;
                }
                itemObj.gameObject.SetActive(true);
                this.haveHeroState[itemObj] = false;
                HoverEventListener.Get(itemObj).onEnter = delegate (PointerEventData pd)
                {
                    this.SetHeroHoverState(true, allItems, itemObj);
                };
                HoverEventListener.Get(itemObj).onExit = delegate (PointerEventData pd)
                {
                    this.SetHeroHoverState(false, allItems, itemObj);
                };
                UIButtonScale uibuttonScale = itemObj.GetComponent<UIButtonScale>();
                Button button = itemObj.GetComponent<Button>();
                if (j >= this.charData.charList.Count)
                {
                    for (int k = 0; k < itemObj.transform.childCount; k++)
                    {
                        GameObject gameObject4 = itemObj.transform.GetChild(k).gameObject;
                        gameObject4.SetActive(false);
                        if (gameObject4.name.Equals("img_bar_e"))
                        {
                            gameObject4.SetActive(true);
                        }
                    }
                    if (uibuttonScale)
                    {
                        uibuttonScale.enabled = false;
                    }
                    if (button != null)
                    {
                        button.enabled = false;
                    }
                }
                else
                {
                    SelectUserInfo sui = this.charData.charList[j];
                    if ((ulong)sui.offlinetime >= (ulong)num2)
                    {
                        this.suiSelectLogin = sui;
                        gameObject3 = itemObj;
                        num2 = (long)((ulong)sui.offlinetime);
                        this.ShowSelectBtnState();
                    }
                    allItems.Add(itemObj);
                    itemObj.transform.FindChild("txt_del_timer").gameObject.SetActive(false);
                    uibuttonScale = itemObj.GetComponent<UIButtonScale>();
                    if (uibuttonScale == null)
                    {
                        uibuttonScale = itemObj.AddComponent<UIButtonScale>();
                    }
                    uibuttonScale.enabled = true;
                    itemObj.transform.FindChild("txt_lv_name").GetComponent<Text>().text = CommonTools.GetLevelFormat(sui.level);
                    itemObj.transform.FindChild("txt_name").GetComponent<Text>().text = sui.name;
                    if (!string.IsNullOrEmpty(sui.mapname) && sui.mapname.Contains(":"))
                    {
                        itemObj.transform.FindChild("txt_map").GetComponent<Text>().text = sui.mapname.Split(new char[]
                        {
                            ':'
                        })[0];
                    }
                    itemObj.transform.FindChild("img_bar_s").gameObject.SetActive(false);
                    if (button == null)
                    {
                        button = itemObj.AddComponent<Button>();
                    }
                    button.enabled = true;
                    button.onClick.AddListener(delegate ()
                    {
                        if (this.suiSelectLogin == sui)
                        {
                            return;
                        }
                        this.SetUISelctStateHaveHero(allItems, itemObj.gameObject);
                        this.suiSelectLogin = sui;
                        this.ShowSelectBtnState();
                        uint[] featureIDs2 = new uint[]
                        {
                            this.suiSelectLogin.haircolor,
                            this.suiSelectLogin.hairstyle,
                            this.suiSelectLogin.facestyle,
                            this.suiSelectLogin.antenna,
                            this.suiSelectLogin.avatarid
                        };
                        this.CreateShowModel(sui.heroid, sui.curheroid, featureIDs2, sui.bodystyle, false);
                    });
                    if (j == 1)
                        button.onClick.Invoke();
                }
            }
            /*
            this.suiSelectLogin = charData.charList[0];
            suiSelectLogin.

            uint[] featureIDs = new uint[]
            {
                            this.suiSelectLogin.haircolor,
                            this.suiSelectLogin.hairstyle,
                            this.suiSelectLogin.facestyle,
                            this.suiSelectLogin.antenna,
                            this.suiSelectLogin.avatarid
            };

            this.CreateShowModel(this.suiSelectLogin.heroid, this.suiSelectLogin.curheroid, featureIDs, this.suiSelectLogin.bodystyle, false);
            gameObject3.transform.FindChild("img_bar_s").gameObject.SetActive(true);
            gameObject3.transform.FindChild("img_bar_us").gameObject.SetActive(false);
            */
        }
    }

    private void SetHeroHoverState(bool isEnter, List<GameObject> allItems, GameObject curItem)
    {
        if (this.haveHeroState[curItem])
        {
            return;
        }
        for (int i = 0; i < allItems.Count; i++)
        {
            GameObject gameObject = allItems[i];
            Transform transform = gameObject.transform.FindChild("img_bar_us");
            Transform transform2 = gameObject.transform.FindChild("img_bar_h");
            if (isEnter && curItem == gameObject)
            {
                transform2.gameObject.SetActive(true);
                transform.gameObject.SetActive(false);
            }
            else
            {
                transform2.gameObject.SetActive(false);
                transform.gameObject.SetActive(true);
            }
        }
    }

    private void ShowSelectBtnState()
    {
        if (this.suiSelectLogin == null)
        {
            return;
        }
        ulong num = SingletonForMono<GameTime>.Instance.GetNowMsecond() / 1000UL;
        this.tr_btn_startGame.gameObject.SetActive(this.suiSelectLogin.delTime == 0U);
        LuaTable configTable = LuaConfigManager.GetConfigTable("levelconfig", (ulong)this.suiSelectLogin.level);
        uint num2 = 0U;
        if (configTable == null)
        {
            FFDebug.LogWarning("ShowSelectBtnState", "Select role level invalid level = " + this.suiSelectLogin.level.ToString());
        }
        else
        {
            num2 = configTable.GetField_Uint("del_protect_time");
        }
        this.delProtectLeftTime = (long)((ulong)(num2 + this.suiSelectLogin.delTime) - num);
        this.delProtectLeftTime = ((this.delProtectLeftTime < 0L) ? 0L : this.delProtectLeftTime);
        this.tr_btn_cancelDelEnsure.gameObject.SetActive(this.suiSelectLogin.delTime > 0U && this.delProtectLeftTime == 0L);
        this.tr_btn_cancelDel.gameObject.SetActive(this.suiSelectLogin.delTime > 0U && this.delProtectLeftTime > 0L);
        this.tr_btn_startDelRole.gameObject.SetActive(this.suiSelectLogin.delTime == 0U);
        if (this.ruac != null)
        {
            RawImage component = this.tr_rtRawImage.gameObject.GetComponent<RawImage>();
            component.color = Color.clear;
            Color c = (this.suiSelectLogin.delTime != 0U && this.onSelectPage) ? Color.gray : Color.white;
            this.SetModelColor(this.ruac.controlTarget, c);
            this.ruac.OnDelete = (this.suiSelectLogin.delTime > 0U && this.onSelectPage);
        }
        this.showProtectLeftTime(0);
    }

    private void showProtectLeftTime(int subTime)
    {
        if (this.suiSelectLogin == null || this.suiSelectLogin.delTime <= 0U || this.delProtectLeftTime < 0L)
        {
            return;
        }
        this.delProtectLeftTime -= (long)subTime;
        if (this.delProtectLeftTime <= 0L)
        {
            this.tr_btn_cancelDelEnsure.gameObject.SetActive(true);
            this.tr_btn_cancelDel.gameObject.SetActive(false);
        }
        else
        {
            long num = this.delProtectLeftTime / 3600L;
            long num2 = this.delProtectLeftTime % 3600L;
            long num3 = num2 / 60L;
            num2 %= 60L;
            this.tr_roleDelProtectLeftTime.text = string.Format("{0:D2}:{1:D2}:{2:D2}", num, num3, num2);
        }
    }

    public void handleDelCallBack(MSG_Req_Delete_Char_CSC msgb)
    {
        if (msgb.retcode == 1U)
        {
            return;
        }
        for (int i = 0; i < this.charData.charList.Count; i++)
        {
            SelectUserInfo selectUserInfo = this.charData.charList[i];
            if (selectUserInfo.charid == msgb.charid)
            {
                if (msgb.opcode == 0U)
                {
                    selectUserInfo.delTime = (uint)(SingletonForMono<GameTime>.Instance.GetNowMsecond() / 1000UL);
                }
                else if (msgb.opcode == 1U)
                {
                    selectUserInfo.delTime = 0U;
                }
                else if (msgb.opcode == 2U)
                {
                    this.charData.charList.RemoveAt(i);
                }
                break;
            }
        }
        if (msgb.opcode != 2U)
        {
            if (this.suiSelectLogin != null && msgb.charid == this.suiSelectLogin.charid)
            {
                this.ShowSelectBtnState();
            }
        }
        else
        {
            this.FillHaveHeroList();
        }
    }

    private void SetRoleShowRtImagePos(bool isInSelect)
    {
        this.onSelectPage = isInSelect;
        if (this.onSelectPage)
        {
            this.tr_rtRawImage.gameObject.SetActive(this.charData.charList.Count != 0);
        }
        else
        {
            this.tr_rtRawImage.gameObject.SetActive(false);
        }
        RectTransform component = this.tr_rtRawImage.GetComponent<RectTransform>();
        Vector3 v = component.anchoredPosition;
        v.x = ((!this.onSelectPage) ? this.originalRtPos : 0f);
        component.anchoredPosition = v;
        this.tr_rtRawImage.SetParent((!this.onSelectPage) ? this.tr_windowRootCreate : this.tr_windowRootSelect, true);
        this.tr_rtRawImage.SetSiblingIndex(0);
    }

    private void BackToLogin()
    {
        UIManager.Instance.maskFadeInOut(0f, 1f, 0.3f, new UIManager.OnMaskFinished(this.back));
    }

    private void back()
    {
        UIManager.Instance.SetMaskImage(true);
        LSingleton<NetWorkModule>.Instance.Close();
        ManagerCenter.Instance.GetManager<EntitiesManager>().PNetWork.OnReLogin();
        ManagerCenter.Instance.GetManager<GameScene>().OnReSet();
        UIManager.Instance.DeleteUI<UI_SelectRole>();
        ManagerCenter.Instance.GetManager<GameMainManager>().loadLoginScene(new Action(this.loginLoadFinish));
    }

    private void loginLoadFinish()
    {
        UIManager.Instance.maskFadeInOut(1f, 0f, 2f, null);
        ControllerManager.Instance.GetController<LoginP2PController>().InitLoginView(delegate ()
        {
            UI_P2PLogin uiobject = UIManager.GetUIObject<UI_P2PLogin>();
            if (null != uiobject)
            {
                uiobject.SetAnimVisible();
            }
            else
            {
                LoginP2PController controller = ControllerManager.Instance.GetController<LoginP2PController>();
                controller.onLogonShow = (Action)Delegate.Combine(controller.onLogonShow, new Action(delegate ()
                {
                    UIManager.GetUIObject<UI_P2PLogin>().SetAnimVisible();
                }));
            }
        });
    }

    private void StartGame()
    {
        if (this.suiSelectLogin != null)
        {
            Scheduler.OnScheduler action = delegate ()
            {
                MSG_Req_SelectCharToLogin_CS msg_Req_SelectCharToLogin_CS = new MSG_Req_SelectCharToLogin_CS();
                msg_Req_SelectCharToLogin_CS.charid = this.suiSelectLogin.charid;
                ManagerCenter.Instance.GetManager<EntitiesManager>().PNetWork.SendMsg<MSG_Req_SelectCharToLogin_CS>(CommandID.MSG_Req_SelectCharToLogin_CS, msg_Req_SelectCharToLogin_CS, false);
                MyPlayerPrefs.SetString("cur_login_charid", this.suiSelectLogin.charid + string.Empty);
            };
            if (this.ruac == null)
            {
                Debug.Log("suiSelectLogin ruac==null");
                return;
            }
            this.ruac.PlayStartAnimationAndDoActon(action, 0.4f);
        }
        else
        {
            Debug.Log("not select login role");
        }
    }

    public static void ReconectLogin()
    {
        MSG_Req_SelectCharToLogin_CS msg_Req_SelectCharToLogin_CS = new MSG_Req_SelectCharToLogin_CS();
        msg_Req_SelectCharToLogin_CS.charid = ulong.Parse(MyPlayerPrefs.GetString("cur_login_charid"));
        ManagerCenter.Instance.GetManager<EntitiesManager>().PNetWork.SendMsg<MSG_Req_SelectCharToLogin_CS>(CommandID.MSG_Req_SelectCharToLogin_CS, msg_Req_SelectCharToLogin_CS, false);
    }

    private void CloseRtImage()
    {
        this.tr_rtRawImage.gameObject.SetActive(false);
    }

    private void RollName()
    {
        if (this.npd == null)
        {
            this.npd = new UI_SelectRole.NamePoolData(LuaConfigManager.GetConfigTableList("namepool"));
        }
        InputField component = this.tr_input_name.GetComponent<InputField>();
        Image component2 = this.tr_sex.FindChild("toggle_male/img_bg/img_s").GetComponent<Image>();
        bool enabled = component2.enabled;
        string randomName = this.npd.GetRandomName(enabled);
        component.text = randomName;
    }

    private void StartDelRole()
    {
        if (this.suiSelectLogin != null)
        {
            UIManager.Instance.ShowUI<UI_MessageBox>("UI_MessageBox", delegate ()
            {
                UI_MessageBox uiobject = UIManager.GetUIObject<UI_MessageBox>();
                uiobject.SetOkCb(new MessageOkCb2(this.onStartDelRole), this.suiSelectLogin.charid.ToString());
                string contentByID = ControllerManager.Instance.GetController<TextModelController>().GetContentByID("1310096");
                uiobject.SetContent(contentByID, "提示", true);
            }, UIManager.ParentType.CommonUI, false);
        }
    }

    private void onStartDelRole(string data)
    {
        PlayerPrefs.DeleteKey("last_login_hero");
        ManagerCenter.Instance.GetManager<EntitiesManager>().PNetWork.DelRole(this.suiSelectLogin.charid, 0U);
    }

    private void CancelDelRole()
    {
        if (this.suiSelectLogin != null)
        {
            PlayerPrefs.DeleteKey("last_login_hero");
            ManagerCenter.Instance.GetManager<EntitiesManager>().PNetWork.DelRole(this.suiSelectLogin.charid, 1U);
        }
    }

    private void SureDelRole()
    {
        PlayerPrefs.DeleteKey("last_login_hero");
        ManagerCenter.Instance.GetManager<EntitiesManager>().PNetWork.DelRole(this.suiSelectLogin.charid, 2U);
    }

    private void onSureDelRole(string data)
    {
        PlayerPrefs.DeleteKey("last_login_hero");
        ManagerCenter.Instance.GetManager<EntitiesManager>().PNetWork.DelRole(this.suiSelectLogin.charid, 2U);
    }

    private void ToSelectRolePage()
    {
        if (this.charData == null || this.charData.charList == null || this.charData.charList.Count == 0)
        {
            this.BackToLogin();
            return;
        }
        this.tr_windowRootSelect.gameObject.SetActive(true);
        this.tr_windowRootCreate.gameObject.SetActive(false);
        this.SetRoleShowRtImagePos(true);
        if (this.suiSelectLogin != null)
        {
            uint[] featureIDs = new uint[]
            {
                this.suiSelectLogin.haircolor,
                this.suiSelectLogin.hairstyle,
                this.suiSelectLogin.facestyle,
                this.suiSelectLogin.antenna,
                this.suiSelectLogin.avatarid
            };
            this.CreateShowModel(this.suiSelectLogin.heroid, this.suiSelectLogin.curheroid, featureIDs, this.suiSelectLogin.bodystyle, false);
        }
        else if (this.ruac != null)
        {
            this.ruac.StartModelMoveOutAnimation();
        }
    }

    private uint GetHeroChractorIDByName(string name)
    {
        if (string.IsNullOrEmpty(name) && this.charData.charList.Count > 0)
        {
            return this.charData.charList[0].heroid;
        }
        for (int i = 0; i < this.charData.charList.Count; i++)
        {
            if (name.Equals(this.charData.charList[i].name))
            {
                return this.charData.charList[i].heroid;
            }
        }
        return 0U;
    }

    private void ToCreateRolePage()
    {
        if (this.charData.charList.Count >= 4)
        {
            TipsWindow.ShowWindow("用户角色数量达到上限");
            return;
        }
        this.tr_windowRootSelect.gameObject.SetActive(false);
        this.tr_windowRootCreate.gameObject.SetActive(true);
        this.tr_select_role_information.gameObject.SetActive(false);
        this.tr_input_name.GetComponent<InputField>().text = string.Empty;
        this.SetRoleShowRtImagePos(false);
        this.tr_mesh_vedio.GetComponent<RawImage>().color = Color.black;
        this.FillBaseHeroList(this.hbdl);
    }

    private void FillBaseHeroList(List<UI_SelectRole.HeroBaseData> data)
    {
        int minSelectIndex = 0;
        int minCreatIndex = 0;
        this.tr_scrollRootCreate.GetComponentInParent<ScrollRect>().vertical = false;
        this.tr_scrollRootCreate.GetComponentInParent<ScrollRect>().horizontal = false;
        Transform child = this.tr_scrollRootCreate.GetChild(0);
        List<string> list = new List<string>();
        int num = Mathf.Max(data.Count, this.tr_scrollRootCreate.childCount);
        List<GameObject> allItems = new List<GameObject>();
        Action selectDefault = null;
        int num2 = UnityEngine.Random.Range(Math.Max(minCreatIndex, minSelectIndex), data.Count);
        SEX sexRandom = data[num2].sex;
        this.sexSelect = sexRandom;
        for (int i = 0; i < num; i++)
        {
            Transform itemObj = null;
            if (i < this.tr_scrollRootCreate.childCount)
            {
                itemObj = this.tr_scrollRootCreate.GetChild(i);
            }
            else
            {
                itemObj = UnityEngine.Object.Instantiate<GameObject>(child.gameObject).transform;
                itemObj.SetParent(this.tr_scrollRootCreate, false);
                itemObj.name = child.name;
            }
            allItems.Add(itemObj.gameObject);
            int index = i;
            itemObj.FindChild("txt_name").GetComponent<Text>().text = data[index].name;
            Image component = itemObj.transform.FindChild("g").GetComponent<Image>();
            Image component2 = itemObj.transform.FindChild("s").GetComponent<Image>();
            Image component3 = itemObj.transform.FindChild("h").GetComponent<Image>();
            if (i < data.Count)
            {
                UI_SelectRole.HeroBaseData heroBaseDataByNameAndSex = this.GetHeroBaseDataByNameAndSex(data[i].name, sexRandom, true);
                string str = heroBaseDataByNameAndSex.icon.Replace("us", string.Empty);
                this.SetTargetSprite(component, str + "us");
                this.SetTargetSprite(component2, str + "s");
                this.SetTargetSprite(component3, str + "h");
                component.gameObject.SetActive(true);
                this.raceBtnSelectState[itemObj] = false;
                List<Image> list2 = new List<Image>();
                UIEventListener.Get(itemObj.gameObject).onEnter = delegate (PointerEventData p)
                {
                    if (index < minSelectIndex)
                    {
                        return;
                    }
                    this.OnRaceBtnHover(true, this.raceBtnSelectState[itemObj], itemObj);
                };
                UIEventListener.Get(itemObj.gameObject).onExit = delegate (PointerEventData p)
                {
                    if (index < minSelectIndex)
                    {
                        return;
                    }
                    this.OnRaceBtnHover(false, this.raceBtnSelectState[itemObj], itemObj);
                };
                this.raceItemSpriteName[itemObj] = data[index].icon;
                Button component4 = itemObj.GetComponent<Button>();
                itemObj.transform.FindChild("s").gameObject.SetActive(false);
                if (selectDefault == null && index == num2)
                {
                    selectDefault = delegate ()
                    {
                        this.curSelectIndex.Clear();
                        this.lastSelectItemObj = itemObj.gameObject;
                        this.raceBtnSelectState[itemObj] = true;
                        this.SetUISelctState(allItems, index);
                        SEX sex = sexRandom;
                        this.lastSelectCharName = data[index].name;
                        if (!this.GetIsNeedShowToggle(data[index].name))
                        {
                            sex = SEX.MALE;
                        }
                        this.SetMaleUIState(sex, data[index].name);
                        selectDefault = null;
                        this.SetCreatable(index >= minCreatIndex);
                    };
                }
                component4.onClick.AddListener(delegate ()
                {
                    if (index < minSelectIndex)
                    {
                        return;
                    }
                    if (this.IsInPlayMoveAnimaton())
                    {
                        return;
                    }
                    if (this.isInCreateModel || this.isInLoadHairOrFaceCount > 0)
                    {
                        return;
                    }
                    if (this.raceBtnSelectState[itemObj])
                    {
                        return;
                    }
                    this.curSelectIndex.Clear();
                    this.raceBtnSelectState[itemObj] = true;
                    this.lastSelectItemObj = itemObj.gameObject;
                    this.SetUISelctState(allItems, index);
                    SEX sex = SEX.MALE;
                    if (this.sexSelect != SEX.NONE)
                    {
                        sex = this.sexSelect;
                    }
                    if (!this.GetIsNeedShowToggle(data[index].name))
                    {
                        sex = SEX.MALE;
                    }
                    this.lastSelectCharName = data[index].name;
                    this.SetMaleUIState(sex, data[index].name);
                    this.SetCreatable(index >= minCreatIndex);
                });
            }
            if (list.Contains(data[i].name))
            {
                itemObj.gameObject.SetActive(false);
            }
            else
            {
                itemObj.gameObject.SetActive(true);
                list.Add(data[i].name);
            }
        }
        selectDefault();
    }

    private void SetCreatable(bool creatable)
    {
        this.tr_btn_ok.GetComponent<Button>().interactable = creatable;
    }

    private void FrashTapIconBySex(List<UI_SelectRole.HeroBaseData> data, SEX sex)
    {
        Transform child = this.tr_scrollRootCreate.GetChild(0);
        List<string> list = new List<string>();
        int num = Mathf.Max(data.Count, this.tr_scrollRootCreate.childCount);
        for (int i = 0; i < num; i++)
        {
            Transform transform;
            if (i < this.tr_scrollRootCreate.childCount)
            {
                transform = this.tr_scrollRootCreate.GetChild(i);
            }
            else
            {
                transform = UnityEngine.Object.Instantiate<GameObject>(child.gameObject).transform;
                transform.SetParent(this.tr_scrollRootCreate, false);
                transform.name = child.name;
            }
            transform.FindChild("txt_name").GetComponent<Text>().text = data[i].name;
            Image component = transform.transform.FindChild("g").GetComponent<Image>();
            Image component2 = transform.transform.FindChild("s").GetComponent<Image>();
            Image component3 = transform.transform.FindChild("h").GetComponent<Image>();
            if (i < data.Count)
            {
                UI_SelectRole.HeroBaseData heroBaseDataByNameAndSex = this.GetHeroBaseDataByNameAndSex(data[i].name, sex, true);
                string str = heroBaseDataByNameAndSex.icon.Replace("us", string.Empty);
                this.SetTargetSprite(component, str + "us");
                this.SetTargetSprite(component2, str + "s");
                this.SetTargetSprite(component3, str + "h");
                component.gameObject.SetActive(true);
            }
            if (list.Contains(data[i].name))
            {
                transform.gameObject.SetActive(false);
            }
            else
            {
                transform.gameObject.SetActive(true);
                list.Add(data[i].name);
            }
        }
    }

    private bool IsInPlayMoveAnimaton()
    {
        if (this.ruac == null)
        {
            return false;
        }
        if (this.ruac.controlTarget == null)
        {
            return false;
        }
        TweenPosition component = this.ruac.controlTarget.GetComponent<TweenPosition>();
        return !(component == null) && component.enabled;
    }

    private void OnRaceBtnHover(bool isEnter, bool isSelected, Transform itemObj)
    {
        if (isSelected)
        {
            return;
        }
        if (isEnter)
        {
            itemObj.FindChild("h").gameObject.SetActive(true);
            itemObj.FindChild("g").gameObject.SetActive(false);
        }
        else
        {
            itemObj.FindChild("h").gameObject.SetActive(false);
            itemObj.FindChild("g").gameObject.SetActive(true);
        }
    }

    private void OnRaceBtnExit(Image hoverImg)
    {
        hoverImg.gameObject.SetActive(false);
    }

    private void SetFeatureConifgUI(UI_SelectRole.HeroBaseData hbd)
    {
        this.randomFeatureActions = null;
        this.setFeatrueDefault = null;
        this.SetConfig(hbd.id, hbd.featureicon, hbd.facestyle, this.tr_panel_face_shap.FindChild("Scroll View/Viewport/Content"), ConfigType.FaceStyle);
        if (string.IsNullOrEmpty(hbd.hairdoicon))
        {
            this.tr_panel_hair_shap.FindChild("img_title_bg/txt_title").gameObject.SetActive(false);
            this.tr_panel_hair_shap.FindChild("img_title_bg/txt_title1").gameObject.SetActive(true);
            this.SetConfig(hbd.id, hbd.antennaicon, hbd.antenna, this.tr_panel_hair_shap.FindChild("Scroll View/Viewport/Content"), ConfigType.AntennaStyle);
        }
        else
        {
            this.tr_panel_hair_shap.FindChild("img_title_bg/txt_title").gameObject.SetActive(true);
            this.tr_panel_hair_shap.FindChild("img_title_bg/txt_title1").gameObject.SetActive(false);
            this.SetConfig(hbd.id, hbd.hairdoicon, hbd.hairstyle, this.tr_panel_hair_shap.FindChild("Scroll View/Viewport/Content"), ConfigType.HairStyle);
        }
        if (string.IsNullOrEmpty(hbd.coloricon))
        {
            this.tr_panel_hair_color.gameObject.SetActive(false);
        }
        else
        {
            this.tr_panel_hair_color.gameObject.SetActive(true);
            this.SetConfig(hbd.id, hbd.coloricon, hbd.color, this.tr_panel_hair_color.FindChild("Scroll View/Viewport/Content"), ConfigType.HairColor);
        }
        this.SetSkillShowUI(hbd);
    }

    private void SetConfig(uint id, string iconName, string resID, Transform contentRoot, ConfigType ct)
    {
        if (!string.IsNullOrEmpty(iconName) && !string.IsNullOrEmpty(resID))
        {
            GameObject gameObject = contentRoot.GetChild(0).gameObject;
            UI_SelectRole.HeroBaseData heroBaseData;
            this.hbdDic.TryGetValue(id, out heroBaseData);
            string[] iconArray = iconName.Split(new char[]
            {
                '|'
            });
            ConfigType curCt = ct;
            int num = Mathf.Max(iconArray.Length, contentRoot.childCount);
            List<Transform> curListItems = new List<Transform>();
            for (int i = 0; i < num; i++)
            {
                GameObject itemObj = null;
                if (i < contentRoot.childCount)
                {
                    itemObj = contentRoot.GetChild(i).gameObject;
                }
                else
                {
                    itemObj = UnityEngine.Object.Instantiate<GameObject>(gameObject);
                    itemObj.transform.SetParent(contentRoot, false);
                    itemObj.transform.localScale = gameObject.transform.localScale;
                    itemObj.name = gameObject.name;
                }
                curListItems.Add(itemObj.transform);
                int index = i;
                if (index < iconArray.Length)
                {
                    Image icon = itemObj.transform.FindChild("icon").GetComponent<Image>();
                    base.GetSprite(ImageType.CHARACTER, iconArray[index], delegate (Sprite sp)
                    {
                        icon.sprite = sp;
                        icon.material = null;
                    });
                    Button button = itemObj.GetComponent<Button>();
                    if (button == null)
                    {
                        button = itemObj.AddComponent<Button>();
                    }
                    Action btnAction = delegate ()
                    {
                        this.SetFeature(curListItems, itemObj.transform, resID, index, ct);
                        if (curCt == ConfigType.HairColor)
                        {
                            this.setColorFeatrueAction = null;
                            this.setColorFeatrueAction = delegate ()
                            {
                                this.SetFeature(curListItems, itemObj.transform, resID, index, ct);
                            };
                        }
                    };
                    if (index == 0)
                    {
                        this.setFeatrueDefault = (Action)Delegate.Combine(this.setFeatrueDefault, btnAction);
                    }
                    button.onClick.RemoveAllListeners();
                    button.onClick.AddListener(delegate ()
                    {
                        btnAction();
                        if (curCt == ConfigType.HairStyle && this.setColorFeatrueAction != null)
                        {
                            this.setColorFeatrueAction();
                        }
                    });
                    itemObj.gameObject.SetActive(true);
                }
                else
                {
                    itemObj.gameObject.SetActive(false);
                }
            }
            Action b = delegate ()
            {
                int index = UnityEngine.Random.Range(0, iconArray.Length);
                this.SetFeature(curListItems, curListItems[index], resID, index, ct);
            };
            this.randomFeatureActions = (Action)Delegate.Combine(this.randomFeatureActions, b);
        }
    }

    private void RandomFeature()
    {
        this.randomActionTimer = 0f;
        if (this.randomFeatureActions != null)
        {
            this.randomFeatureActions();
        }
    }

    private void SetFeature(List<Transform> curListItems, Transform itemObj, string resID, int index, ConfigType curCt)
    {
        this.SetConfigButtonSelectState(curListItems, itemObj.transform);
        UI_SelectRole.HeroBaseData heroBaseData = this.hbdCurSelect;
        string[] array = null;
        if (curCt != ConfigType.HairColor)
        {
            array = resID.Split(new char[]
            {
                '|'
            });
            if (curCt == ConfigType.HairStyle && heroBaseData.colorStyleIndex != null)
            {
                int num = 0;
                if (this.curSelectIndex.ContainsKey(ConfigType.HairColor))
                {
                    num = this.curSelectIndex[ConfigType.HairColor];
                }
                List<string> list = new List<string>(heroBaseData.colorStyleIndex.Keys);
                string s = heroBaseData.colorStyleIndex[list[index]][num];
                LuaTable configTable = LuaConfigManager.GetConfigTable("looksconfig", (ulong)uint.Parse(s));
                if (configTable != null)
                {
                    this.modelHangHelperInstance.hairColorName = configTable.GetCacheField_String("resource");
                }
            }
        }
        else if (resID.Contains(")"))
        {
            if (heroBaseData != null && heroBaseData.colorStyleIndex != null)
            {
                List<string> list2 = new List<string>(heroBaseData.colorStyleIndex.Keys);
                int index2 = 0;
                if (this.curSelectIndex.ContainsKey(ConfigType.HairStyle))
                {
                    index2 = this.curSelectIndex[ConfigType.HairStyle];
                }
                array = new List<string>(heroBaseData.colorStyleIndex[list2[index2]]).ToArray();
            }
        }
        else
        {
            array = resID.Split(new char[]
            {
                '|'
            });
        }
        if (array != null)
        {
            this.ApplyConfig(array[index], index);
        }
    }

    private void SetConfigButtonSelectState(List<Transform> allItems, Transform curItem)
    {
        for (int i = 0; i < allItems.Count; i++)
        {
            Transform transform = allItems[i].FindChild("img_selected");
            transform.gameObject.SetActive(allItems[i] == curItem);
        }
    }

    private void SetScrollRectState(Transform content)
    {
        RectTransform component = content.parent.GetComponent<RectTransform>();
        HorizontalLayoutGroup componentInParent = content.GetComponentInParent<HorizontalLayoutGroup>();
        float num = 0f;
        for (int i = 0; i < content.childCount; i++)
        {
            RectTransform component2 = content.GetChild(i).GetComponent<RectTransform>();
            if (component2.gameObject.activeInHierarchy)
            {
                num += component2.rect.x;
                if (i < content.childCount - 1)
                {
                    num += componentInParent.spacing;
                }
            }
        }
        component.GetComponentInParent<ScrollRect>().enabled = (num >= component.GetComponentInParent<ScrollRect>().content.rect.width);
    }

    private void SetSkillShowUI(UI_SelectRole.HeroBaseData hbd)
    {
        if (!string.IsNullOrEmpty(hbd.skill))
        {
            Transform transform = this.tr_panel_skill.FindChild("Scroll View/Viewport/Content");
            GameObject gameObject = transform.GetChild(0).gameObject;
            string[] array = hbd.skill.Split(new char[]
            {
                '|'
            });
            string[] array2 = hbd.videotape.Split(new char[]
            {
                '|'
            });
            int num = Mathf.Max(transform.childCount, array.Length);
            for (int i = 0; i < num; i++)
            {
                GameObject itemObj = null;
                if (i < transform.childCount)
                {
                    itemObj = transform.GetChild(i).gameObject;
                }
                else
                {
                    itemObj = UnityEngine.Object.Instantiate<GameObject>(gameObject);
                    itemObj.transform.SetParent(transform, false);
                    itemObj.transform.localScale = gameObject.transform.localScale;
                    itemObj.name = gameObject.name;
                }
                if (i < array.Length)
                {
                    itemObj.gameObject.SetActive(true);
                    uint num2 = uint.Parse(array[i]);
                    string vedioName = array2[i];
                    LuaTable skillTabelBySkillID = this.GetSkillTabelBySkillID(num2);
                    if (skillTabelBySkillID != null)
                    {
                        Image icon = itemObj.transform.FindChild("icon").GetComponent<Image>();
                        base.GetSprite(ImageType.ITEM, skillTabelBySkillID.GetCacheField_String("skillicon"), delegate (Sprite sp)
                        {
                            icon.sprite = sp;
                            icon.material = null;
                        });
                        itemObj.transform.FindChild("img_mask").gameObject.SetActive(false);
                        HoverEventListener.Get(itemObj).onEnter = delegate (PointerEventData p)
                        {
                            icon.enabled = false;
                            itemObj.transform.FindChild("img_mask").gameObject.SetActive(true);
                            this.PlaySkillVedio(vedioName);
                        };
                        HoverEventListener.Get(itemObj).onExit = delegate (PointerEventData p)
                        {
                            icon.enabled = true;
                            itemObj.transform.FindChild("img_mask").gameObject.SetActive(false);
                            this.StopSkillVedio();
                        };
                    }
                    else
                    {
                        Debug.LogError("skill id config not find:" + num2);
                    }
                }
                else
                {
                    itemObj.gameObject.SetActive(false);
                }
            }
        }
    }

    private LuaTable GetSkillTabelBySkillID(uint skillID)
    {
        LuaTable result = null;
        if (this.skillDataTable.TryGetValue(skillID, out result))
        {
            return result;
        }
        return null;
    }

    private void ApplyConfig(string resID, int curResIndex)
    {
        if (this.modelHangHelperInstance != null)
        {
            LuaTable configTable = LuaConfigManager.GetConfigTable("looksconfig", (ulong)((uint)float.Parse(resID)));
            uint cacheField_Uint = configTable.GetCacheField_Uint("type");
            this.modelHangHelperInstance.useID[(int)((UIntPtr)cacheField_Uint)] = (uint)float.Parse(resID);
            string cacheField_String = configTable.GetCacheField_String("resource");
            this.curSelectIndex[(ConfigType)(cacheField_Uint - 1U)] = curResIndex;
            LuaTable lt = LuaConfigManager.GetConfigTable("newUser", (ulong)this.hbdCurSelect.id);
            Action setLight = delegate ()
            {
                this.isInLoadHairOrFaceCount--;
                this.SetCharLightDataByConfig(this.modelHangHelperInstance.rootObj, lt);
            };
            Action<PlayerCharactorCreateHelper> back = delegate (PlayerCharactorCreateHelper pcch)
            {
                this.isInLoadHairOrFaceCount--;
            };
            string faceColorStr = string.Empty;
            string s = string.Empty;
            string s2 = string.Empty;
            int index = 0;
            int num = 0;
            int index2 = 0;
            if (this.hbdCurSelect.colorStyleIndex != null)
            {
                List<string> list = new List<string>(this.hbdCurSelect.colorStyleIndex.Keys);
                this.curSelectIndex.TryGetValue(ConfigType.FaceStyle, out index);
                this.curSelectIndex.TryGetValue(ConfigType.HairColor, out num);
                this.curSelectIndex.TryGetValue(ConfigType.HairStyle, out index2);
                s = this.hbdCurSelect.colorStyleIndex[list[index]][num];
                LuaTable configTable2 = LuaConfigManager.GetConfigTable("looksconfig", (ulong)uint.Parse(s));
                faceColorStr = configTable2.GetCacheField_String("resource");
                this.modelHangHelperInstance.headColorName = faceColorStr;
                s2 = this.hbdCurSelect.colorStyleIndex[list[index2]][num];
                LuaTable configTable3 = LuaConfigManager.GetConfigTable("looksconfig", (ulong)uint.Parse(s2));
                this.modelHangHelperInstance.hairColorName = configTable3.GetCacheField_String("resource");
            }
            switch (cacheField_Uint)
            {
                case 1U:
                    this.isInLoadHairOrFaceCount += 2;
                    this.modelHangHelperInstance.SetHairColor(cacheField_String, back);
                    this.modelHangHelperInstance.SetFaceColor(faceColorStr, back);
                    break;
                case 2U:
                    this.isInLoadHairOrFaceCount++;
                    this.modelHangHelperInstance.LoadHair(cacheField_String, null, setLight);
                    break;
                case 3U:
                    {
                        this.isInLoadHairOrFaceCount++;
                        Action finishCall = delegate ()
                        {
                            this.modelHangHelperInstance.SetFaceColor(faceColorStr, null);
                            setLight();
                        };
                        this.modelHangHelperInstance.LoadFace(cacheField_String, null, finishCall, false);
                        break;
                    }
                case 4U:
                    this.isInLoadHairOrFaceCount++;
                    this.modelHangHelperInstance.LoadHair(cacheField_String, null, setLight);
                    break;
            }
        }
    }

    private void PlaySkillVedio(string vName)
    {
        this.tr_mesh_vedio.parent.gameObject.SetActive(true);
        RawImage component = this.tr_mesh_vedio.GetComponent<RawImage>();
        component.color = Color.white;
        this.avPlayOP.PlayMoveToImage(vName, true, component, 0f, null);
        this.tr_mesh_vedio.GetComponent<RawImage>().color = Color.white;
    }

    private void StopSkillVedio()
    {
        this.avPlayOP.StopPlay();
        RawImage component = this.tr_mesh_vedio.GetComponent<RawImage>();
        component.texture = null;
        this.tr_mesh_vedio.GetComponent<RawImage>().color = Color.white;
        this.tr_mesh_vedio.parent.gameObject.SetActive(false);
    }

    private void SetTargetSprite(Image image, string spName)
    {
        base.GetSprite(ImageType.ROLES, spName, delegate (Sprite rsp)
        {
            image.sprite = rsp;
            image.color = Color.white;
            image.material = null;
        });
    }

    private void SetUISelctStateHaveHero(List<GameObject> listObj, GameObject curObj)
    {
        foreach (GameObject gameObject in listObj)
        {
            bool value = gameObject == curObj;
            this.haveHeroState[gameObject] = value;
            if (gameObject == curObj)
            {
                gameObject.transform.FindChild("img_bar_s").gameObject.SetActive(true);
                gameObject.transform.FindChild("img_bar_us").gameObject.SetActive(false);
            }
            else
            {
                gameObject.transform.FindChild("img_bar_s").gameObject.SetActive(false);
                gameObject.transform.FindChild("img_bar_us").gameObject.SetActive(true);
            }
        }
    }

    private void SetUISelctState(List<GameObject> listObj, int index)
    {
        int num = index / 2;
        for (int i = 0; i < listObj.Count; i++)
        {
            Transform transform = listObj[i].transform;
            if (num == i / 2)
            {
                string[] array = this.raceItemSpriteName[transform.transform].Split(new char[]
                {
                    '_'
                });
                transform.FindChild("g").gameObject.SetActive(false);
                transform.FindChild("h").gameObject.SetActive(false);
                transform.FindChild("s").gameObject.SetActive(true);
                this.raceBtnSelectState[transform.transform] = true;
            }
            else
            {
                this.raceBtnSelectState[transform.transform] = false;
                transform.FindChild("g").gameObject.SetActive(true);
                transform.FindChild("h").gameObject.SetActive(false);
                transform.FindChild("s").gameObject.SetActive(false);
            }
        }
    }

    private void SetMaleUIState(SEX sex, string curSelectName)
    {
        UI_SelectRole.HeroBaseData heroBaseDataByNameAndSex = this.GetHeroBaseDataByNameAndSex(curSelectName, sex, true);
        if (!heroBaseDataByNameAndSex.isLoadAllRes)
        {
            heroBaseDataByNameAndSex.isLoadAllRes = true;
            this.LoadAllNeedRes(heroBaseDataByNameAndSex);
        }
        bool isNeedShowToggle = this.GetIsNeedShowToggle(heroBaseDataByNameAndSex.name);
        if (isNeedShowToggle)
        {
            this.FrashTapIconBySex(this.hbdl, sex);
        }
        this.hbdCurSelect = heroBaseDataByNameAndSex;
        this.SetFeatureConifgUI(heroBaseDataByNameAndSex);
        Image component = this.tr_sex.FindChild("toggle_male/img_bg/img_s").GetComponent<Image>();
        Image component2 = this.tr_sex.FindChild("toggle_female/img_bg/img_s").GetComponent<Image>();
        component.enabled = false;
        component2.enabled = false;
        component.gameObject.SetActive(true);
        component2.gameObject.SetActive(true);
        if (sex == SEX.MALE)
        {
            component.enabled = true;
        }
        else
        {
            component2.enabled = true;
        }
        this.tr_sex.gameObject.SetActive(true);
        if (!isNeedShowToggle)
        {
            this.tr_sex.transform.FindChild("toggle_male").gameObject.SetActive(false);
            this.tr_sex.transform.FindChild("toggle_female").gameObject.SetActive(false);
            this.tr_sex.transform.FindChild("Text").gameObject.SetActive(true);
        }
        else
        {
            this.tr_sex.transform.FindChild("toggle_male").gameObject.SetActive(true);
            this.tr_sex.transform.FindChild("toggle_female").gameObject.SetActive(true);
            this.tr_sex.transform.FindChild("Text").gameObject.SetActive(false);
        }
        this.CreateShowModel(heroBaseDataByNameAndSex.id, heroBaseDataByNameAndSex.id, heroBaseDataByNameAndSex.defaultFeatureIDs, heroBaseDataByNameAndSex.bodyid, true);
        this.ShowSelectHeroBaseInformation(heroBaseDataByNameAndSex);
    }

    private void LoadAllNeedRes(UI_SelectRole.HeroBaseData hbd)
    {
    }

    private List<UI_SelectRole.HeroBaseData> InitHeroBaseData()
    {
        List<LuaTable> configTableList = LuaConfigManager.GetConfigTableList("newUser");
        List<UI_SelectRole.HeroBaseData> list = new List<UI_SelectRole.HeroBaseData>();
        this.hbdDic_ = new Dictionary<uint, UI_SelectRole.HeroBaseData>();
        if (configTableList != null)
        {
            for (int i = 0; i < configTableList.Count; i++)
            {
                IEnumerator enumerator = configTableList[i].Keys.GetEnumerator();
                UI_SelectRole.HeroBaseData heroBaseData = new UI_SelectRole.HeroBaseData();
                while (enumerator.MoveNext())
                {
                    object obj = enumerator.Current;
                    string text = obj.ToString();
                    string text2 = text;
                    switch (text2)
                    {
                        case "id":
                            heroBaseData.id = configTableList[i].GetField_Uint(text);
                            break;
                        case "name":
                            heroBaseData.name = configTableList[i].GetField_String(text);
                            break;
                        case "Model":
                            heroBaseData.model = configTableList[i].GetField_String(text);
                            break;
                        case "Icon":
                            heroBaseData.icon = configTableList[i].GetField_String(text);
                            break;
                        case "Des":
                            heroBaseData.des = configTableList[i].GetField_String(text);
                            break;
                        case "Charact":
                            heroBaseData.charactFeature = configTableList[i].GetField_String(text);
                            break;
                        case "Ability":
                            heroBaseData.ablility = configTableList[i].GetField_String(text);
                            break;
                        case "hairdoicon":
                            heroBaseData.hairdoicon = configTableList[i].GetField_String(text);
                            break;
                        case "hairdo":
                            heroBaseData.hairstyle = configTableList[i].GetField_String(text);
                            break;
                        case "coloricon":
                            heroBaseData.coloricon = configTableList[i].GetField_String(text);
                            break;
                        case "color":
                            heroBaseData.color = configTableList[i].GetField_String(text);
                            break;
                        case "featureicon":
                            heroBaseData.featureicon = configTableList[i].GetField_String(text);
                            break;
                        case "feature":
                            heroBaseData.facestyle = configTableList[i].GetField_String(text);
                            break;
                        case "antennaicon":
                            heroBaseData.antennaicon = configTableList[i].GetField_String(text);
                            break;
                        case "antenna":
                            heroBaseData.antenna = configTableList[i].GetField_String(text);
                            break;
                        case "skill":
                            heroBaseData.skill = configTableList[i].GetField_String(text);
                            break;
                        case "videotape":
                            heroBaseData.videotape = configTableList[i].GetField_String(text);
                            break;
                        case "Sex":
                            {
                                heroBaseData.sex = SEX.NONE;
                                string text3 = configTableList[i].GetField_String(text);
                                text3 = text3.Replace(".0", string.Empty);
                                if ("2".Equals(text3))
                                {
                                    heroBaseData.sex = SEX.MALE;
                                }
                                if ("3".Equals(text3))
                                {
                                    heroBaseData.sex = SEX.FEMALE;
                                }
                                break;
                            }
                        case "Stage":
                            heroBaseData.stage = configTableList[i].GetField_String(text);
                            break;
                    }
                }
                heroBaseData.defaultFeatureIDs = new uint[4];
                if (!string.IsNullOrEmpty(heroBaseData.color))
                {
                    if (heroBaseData.color.Contains("("))
                    {
                        string contentByMatch = UI_SelectRole.GetContentByMatch(heroBaseData.color, "[[][0-9]{1,}[]]", 0, new string[]
                        {
                            "[",
                            "]"
                        });
                        heroBaseData.colorStyleIndex = new Dictionary<string, string[]>();
                        int num2 = heroBaseData.color.Split(new char[]
                        {
                            ')'
                        }).Length;
                        for (int j = 0; j < num2; j++)
                        {
                            string contentByMatch2 = UI_SelectRole.GetContentByMatch(heroBaseData.color, "[(][0-9|\\|]{1,}[)]", j, new string[]
                            {
                                "(",
                                ")"
                            });
                            if (!string.IsNullOrEmpty(contentByMatch2))
                            {
                                string contentByMatch3 = UI_SelectRole.GetContentByMatch(heroBaseData.color, "[[][0-9]{1,}[]]", j, new string[]
                                {
                                    "[",
                                    "]"
                                });
                                heroBaseData.colorStyleIndex[contentByMatch3] = contentByMatch2.Split(new char[]
                                {
                                    '|'
                                });
                            }
                        }
                        heroBaseData.defaultFeatureIDs[0] = uint.Parse(heroBaseData.colorStyleIndex[contentByMatch][0]);
                    }
                    else
                    {
                        heroBaseData.defaultFeatureIDs[0] = (uint)float.Parse(heroBaseData.color.Split(new char[]
                        {
                            '|'
                        })[0]);
                    }
                }
                if (!string.IsNullOrEmpty(heroBaseData.hairstyle))
                {
                    heroBaseData.defaultFeatureIDs[1] = (uint)float.Parse(heroBaseData.hairstyle.Split(new char[]
                    {
                        '|'
                    })[0]);
                }
                if (!string.IsNullOrEmpty(heroBaseData.facestyle))
                {
                    heroBaseData.defaultFeatureIDs[2] = (uint)float.Parse(heroBaseData.facestyle.Split(new char[]
                    {
                        '|'
                    })[0]);
                }
                if (!string.IsNullOrEmpty(heroBaseData.antenna))
                {
                    heroBaseData.defaultFeatureIDs[3] = (uint)float.Parse(heroBaseData.antenna.Split(new char[]
                    {
                        '|'
                    })[0]);
                }
                if (heroBaseData.id > 0U)
                {
                    LuaTable cacheField_Table = LuaConfigManager.GetConfig("heros").GetCacheField_Table(heroBaseData.id);
                    if (cacheField_Table != null)
                    {
                        int cacheField_Int = cacheField_Table.GetCacheField_Int("newavatar");
                        LuaTable configTable = LuaConfigManager.GetConfigTable("avatar_config", (ulong)((long)cacheField_Int));
                        if (configTable != null)
                        {
                            heroBaseData.bodyid = configTable.GetCacheField_Uint("body");
                        }
                    }
                }
                list.Add(heroBaseData);
                this.hbdDic_[heroBaseData.id] = heroBaseData;
            }
        }
        list.Sort((UI_SelectRole.HeroBaseData a, UI_SelectRole.HeroBaseData b) => a.id.CompareTo(b.id));
        return list;
    }

    public static string GetContentByMatch(string src, string patten, int index = 0, params string[] stripStr)
    {
        string empty = string.Empty;
        Regex regex = new Regex(patten, RegexOptions.IgnoreCase);
        IEnumerator enumerator = regex.Matches(src).GetEnumerator();
        int num = 0;
        while (enumerator.MoveNext())
        {
            object obj = enumerator.Current;
            Match match = (Match)obj;
            if (index == num && !string.IsNullOrEmpty(match.Value))
            {
                string text = match.Value;
                if (stripStr != null && stripStr.Length > 0)
                {
                    for (int i = 0; i < stripStr.Length; i++)
                    {
                        text = text.Replace(stripStr[i], string.Empty);
                    }
                }
                return text;
            }
            num++;
        }
        return empty;
    }

    private SEX GetCurSexSetting()
    {
        Image component = this.tr_sex.FindChild("toggle_male/img_bg/img_s").GetComponent<Image>();
        return (!component.enabled) ? SEX.FEMALE : SEX.MALE;
    }

    private void ShowSelectHeroBaseInformation(UI_SelectRole.HeroBaseData hbd)
    {
        SEX curSexSetting = this.GetCurSexSetting();
        this.tr_txt_create_tip.gameObject.SetActive(false);
        this.tr_bottom.gameObject.SetActive(true);
        this.tr_rtRawImage.gameObject.SetActive(true);
        this.tr_windowRootCreate.FindChild("Panel_race_select/img_race_des_bg/img_title_bg/txt_race_title").GetComponent<Text>().text = hbd.name;
        this.tr_windowRootCreate.FindChild("Panel_race_select/img_race_des_bg/txt_race_des").GetComponent<Text>().text = hbd.des;
        this.tr_windowRootCreate.FindChild("Panel_race_select/img_race_des_bg/txt_race_cha").GetComponent<Text>().text = hbd.charactFeature;
    }

    private void InitBaseAbilityGraphic(string datas)
    {
        if (string.IsNullOrEmpty(datas))
        {
            FFDebug.LogError(this, "ability data is empty please check out it");
            return;
        }
        GameObject gameObject = this.tr_select_role_information.FindChild("img_feature_des/img_bg_rad/mesh_rad").gameObject;
        MeshFilter component = gameObject.GetComponent<MeshFilter>();
        Mesh mesh = new Mesh();
        List<Vector3> list = new List<Vector3>();
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            list.Add(gameObject.transform.GetChild(i).localPosition);
        }
        Vector3[] array = new Vector3[6];
        Vector2[] array2 = new Vector2[6];
        array2[0] = new Vector2(0f, 0f);
        array2[1] = (array2[2] = (array2[3] = (array2[4] = (array2[5] = new Vector2(0.5f, 0.4f)))));
        string[] array3 = datas.Split(new string[]
        {
            "-"
        }, StringSplitOptions.RemoveEmptyEntries);
        int[] array4 = new int[15];
        int num = 0;
        while (num < array3.Length && num + 1 < array.Length)
        {
            float num2 = float.Parse(array3[num]) / 6f;
            num2 = Mathf.Clamp(num2, 0f, 1f);
            array[num + 1] = Vector3.Lerp(array[0], list[num + 1], num2);
            num++;
        }
        int num3 = 5;
        for (int j = 0; j < num3; j++)
        {
            int num4 = j * 3;
            array4[num4] = 0;
            if (j == num3 - 1)
            {
                array4[num4 + 1] = j + 1;
                array4[num4 + 2] = 1;
            }
            else
            {
                array4[num4 + 1] = j + 1;
                array4[num4 + 2] = j + 2;
            }
        }
        mesh.vertices = array;
        mesh.triangles = array4;
        mesh.uv = array2;
        mesh.RecalculateNormals();
        component.sharedMesh = mesh;
        MeshRenderer component2 = component.gameObject.gameObject.GetComponent<MeshRenderer>();
        if (component2.material == null)
        {
            component2.material = new Material(Shader.Find("Standard"))
            {
                name = "rad"
            };
        }
        component2.material.SetColor("_EmissionColor", Color.green / 2f);
        component2.material.EnableKeyword("_EMISSION");
    }

    private UI_SelectRole.HeroBaseData GetHeroBaseDataByNameAndSex(string name, SEX sex, bool isNeedCheckSex = true)
    {
        for (int i = 0; i < this.hbdl.Count; i++)
        {
            if (isNeedCheckSex)
            {
                if (this.hbdl[i].name.Equals(name) && this.hbdl[i].sex == sex)
                {
                    return this.hbdl[i];
                }
            }
            else if (this.hbdl[i].name.Equals(name))
            {
                return this.hbdl[i];
            }
        }
        return this.hbdl[this.hbdl.Count - 1];
    }

    private bool GetIsNeedShowToggle(string name)
    {
        int num = 0;
        for (int i = 0; i < this.hbdl.Count; i++)
        {
            if (this.hbdl[i].name.Equals(name))
            {
                num++;
            }
        }
        return num > 1;
    }

    private string GetHeroIconNameByID(uint id)
    {
        for (int i = 0; i < this.hbdl.Count; i++)
        {
            if (this.hbdl[i].id == id)
            {
                return this.hbdl[i].icon;
            }
        }
        return string.Empty;
    }

    private void CreateRole()
    {
        if (this.isInCreateModel || this.isInLoadHairOrFaceCount > 0)
        {
            return;
        }
        if (this.hbdCurSelect == null)
        {
            Debug.Log("Not select create hero");
            return;
        }
        InputField component = this.tr_input_name.GetComponent<InputField>();
        if (string.Compare(component.text, KeyWordFilter.TextFilter(component.text)) != 0)
        {
            TipsWindow.ShowWindow(TipsType.NAME_HAVE_SENSITIVE, null);
            return;
        }
        if (string.IsNullOrEmpty(component.text))
        {
            TipsWindow.ShowWindow(TipsType.NAME_CANNOT_NULL, null);
            return;
        }
        SEX curSexSetting = this.GetCurSexSetting();
        this.hbdCurSelect = this.GetHeroBaseDataByNameAndSex(this.hbdCurSelect.name, curSexSetting, true);
        PlayerPrefs.DeleteKey("last_login_hero");
        this.modelHangHelperInstance.useID[0] = this.hbdCurSelect.id;
        ManagerCenter.Instance.GetManager<EntitiesManager>().PNetWork.RegRole(component.text, this.modelHangHelperInstance.useID, curSexSetting);
    }

    private void ClearModelCache()
    {
    }

    public void CreateShowModel(uint charactID, uint currheroid, uint[] featureIDs = null, uint bodyid = 0U, bool isRandom = false)
    {
        string bonename = string.Empty;
        if (currheroid > 0U)
        {
            LuaTable cacheField_Table = LuaConfigManager.GetConfig("npc_data").GetCacheField_Table(currheroid);
            if (cacheField_Table != null)
            {
                bonename = cacheField_Table.GetField_String("animatorcontroller");
            }
        }
        LuaTable npcConfig = LuaConfigManager.GetConfigTable("newUser", (ulong)charactID);
        if (npcConfig == null)
        {
            FFDebug.LogWarning("CreateShowModel", "Invalid charactid = " + charactID.ToString());
            return;
        }
        this.isInCreateModel = true;
        RawImage ri = this.tr_rtRawImage.gameObject.GetComponent<RawImage>();
        ri.gameObject.SetActive(true);
        string scenename = npcConfig.GetCacheField_String("scenename");
        RectTransform component = ri.GetComponent<RectTransform>();
        if (this.rtCam == null)
        {
            this.sceneObj = GameObject.Find("Scene");
            if (this.sceneObj != null)
            {
                this.camRoot = this.sceneObj.FindChild("ZoneRoot_0/NonStatic_0");
            }
        }
        Vector3 camPos;
        GameObject currCamera = this.SwitchScene(npcConfig, out camPos);
        Camera cam = this.rtCam.GetComponent<Camera>();
        if (cam == null)
        {
            cam = this.rtCam.gameObject.AddComponent<Camera>();
        }
        cam.enabled = true;
        FxPro component2 = cam.gameObject.GetComponent<FxPro>();
        if (component2 != null)
        {
            component2.enabled = false;
            component2.enabled = true;
        }
        int cullingMask = 1 << LayerMask.NameToLayer("RT") | Const.LayerForMask.Default | Const.LayerForMask.Wall | Const.LayerForMask.Terrian;
        cam.cullingMask = cullingMask;
        Color gray = Color.gray;
        gray.a = 0f;
        cam.clearFlags = CameraClearFlags.Color;
        cam.backgroundColor = gray;
        ri.color = Color.clear;
        float camMaxDis = float.Parse(npcConfig.GetCacheField_String("cammaxdis"));
        float camMinDis = float.Parse(npcConfig.GetCacheField_String("cammindis"));
        uint heroid = (currheroid > 0U) ? currheroid : charactID;
        Action<PlayerCharactorCreateHelper> callback = delegate (PlayerCharactorCreateHelper mhh)
        {
            if (this.modelHangHelperInstance != null)
            {
                this.modelHangHelperInstance.DisposeBonePObj();
            }
            this.modelHangHelperInstance = null;
            this.modelHangHelperInstance = mhh;
            this.modelHangHelperInstance.useID[1] = featureIDs[0];
            this.modelHangHelperInstance.useID[2] = featureIDs[1];
            this.modelHangHelperInstance.useID[3] = featureIDs[2];
            this.modelHangHelperInstance.useID[4] = featureIDs[3];
            mhh.TrySetFaceColorRamp(heroid, featureIDs[2].ToString(), featureIDs[0].ToString());
            GameObject rootObj = mhh.rootObj;
            rootObj.gameObject.SetActive(true);
            this.modelHangHelperInstance.sceneNameKey = scenename;
            Quaternion rotation = Quaternion.Euler(0f, 180f + cam.transform.rotation.eulerAngles.y, 0f);
            rootObj.transform.rotation = rotation;
            this.ruac = ri.gameObject.GetComponent<RoleUIAnimationControl>();
            float camEndHeigth = 0f;
            if (null != mhh.headObj)
            {
                camEndHeigth = mhh.rootObj.transform.InverseTransformPoint(mhh.headObj.transform.position).y;
            }
            float num = 1.5f;
            camPos.y = num;
            camPos.z = camMaxDis;
            cam.transform.position = camPos;
            if (this.ruac == null)
            {
                this.ruac = ri.gameObject.AddComponent<RoleUIAnimationControl>();
            }
            this.ruac.distToChange = 0f;
            this.ruac.modelDestPos = cam.transform.position.x * Vector3.right;
            this.ruac.modelStartPos = this.ruac.modelDestPos;
            this.ruac.modelStartPos.y = 0f;
            this.ruac.modelOutPos = cam.transform.position + cam.transform.forward * camMaxDis - cam.transform.right * 5f;
            this.ruac.modelOutPos.y = 0f;
            this.ruac.targetDistLimit = new float[]
            {
                camMinDis,
                camMaxDis
            };
            this.ruac.camStartHeigth = num;
            this.ruac.camEndHeigth = camEndHeigth;
            this.ruac.OnDelete = ((this.suiSelectLogin == null || this.suiSelectLogin.delTime > 0U) && this.onSelectPage);
            rootObj.transform.position = this.ruac.modelStartPos;
            this.ruac.curHeroid = heroid;
            this.ruac.controlTarget = rootObj;
            this.ruac.camMain = cam;
            Color c = (this.onSelectPage && this.suiSelectLogin.delTime != 0U) ? Color.gray : Color.white;
            this.SetModelColor(this.ruac.controlTarget, c);
            this.SetCharLightDataByConfig(rootObj, npcConfig);
            this.isInCreateModel = false;
            DepthOfField component3 = currCamera.GetComponent<DepthOfField>();
            if (null != component3)
            {
                UnityEngine.Object.DestroyImmediate(component3);
            }
            FxPro component4 = Camera.main.gameObject.GetComponent<FxPro>();
            if (null != component4)
            {
                component4.DOFEnabled = true;
                component4.DOFParams.Target = rootObj.transform;
            }
            if (isRandom && this.randomFeatureActions != null)
            {
                this.randomFeatureActions();
            }
        };
        FFCharacterModelHold.CreateModel(heroid, bodyid, featureIDs, bonename, callback, null, 0UL);
    }

    private string GetFaceColorByFaceAndColorIndex(UI_SelectRole.HeroBaseData hbd, uint faceResID, uint colorResID)
    {
        return string.Empty;
    }

    public static void SetGameObjectLight(GameObject ModelObj, string sceneKey = "")
    {
        GameScene manager = ManagerCenter.Instance.GetManager<GameScene>();
        if (manager != null)
        {
            manager.SetMatLightInfoByLightObjKeyName(ModelObj, sceneKey);
        }
    }

    private void SetCharLightDataByConfig(GameObject rootObj, LuaTable newUserCfg)
    {
        Color color = Color.white;
        float value = 1f;
        Vector3 forward = Vector3.forward;
        string text = newUserCfg.GetCacheField_String("scenename");
        text = text.ToLower();
        if (this.sceneObj != null)
        {
            Transform transform = this.sceneObj.transform.FindChild("ZoneRoot_0/Static_0/batchZone0");
            if (transform)
            {
                for (int i = 0; i < transform.transform.childCount; i++)
                {
                    Transform child = transform.transform.GetChild(i);
                    if (child.GetComponent<Light>() != null && child.name.ToLower().StartsWith(text) && child.name.ToLower().Contains("player"))
                    {
                        Light component = child.GetComponent<Light>();
                        color = component.color;
                        value = component.intensity;
                        forward = child.forward;
                        break;
                    }
                }
            }
            Renderer[] componentsInChildren = rootObj.GetComponentsInChildren<Renderer>();
            for (int j = 0; j < componentsInChildren.Length; j++)
            {
                if (componentsInChildren[j].sharedMaterial != null)
                {
                    if (componentsInChildren[j].sharedMaterial.HasProperty("_LightColor"))
                    {
                        componentsInChildren[j].sharedMaterial.SetColor("_LightColor", color);
                    }
                    if (componentsInChildren[j].sharedMaterial.HasProperty("_LightDir"))
                    {
                        componentsInChildren[j].sharedMaterial.SetVector("_LightDir", forward);
                    }
                    if (componentsInChildren[j].sharedMaterial.HasProperty("_LightIntensity"))
                    {
                        componentsInChildren[j].sharedMaterial.SetFloat("_LightIntensity", value);
                    }
                }
            }
        }
    }

    private GameObject SwitchScene(LuaTable newUserCfg, out Vector3 camPosO)
    {
        GameObject gameObject = GameObject.Find("Scene");
        Transform transform = gameObject.transform.root.FindChild("ZoneRoot_0/Static_0/batchZone0");
        string cacheField_String = newUserCfg.GetCacheField_String("scenename");
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            if (child.GetComponent<Camera>() != null || child.GetComponent<Light>() != null)
            {
                child.GetComponent<Light>().shadows = LightShadows.Soft;
                child.gameObject.SetActive(child.name.ToLower().StartsWith(cacheField_String.ToLower()));
            }
        }
        string text = newUserCfg.GetCacheField_String("campos");
        Vector3 zero = Vector3.zero;
        if (!string.IsNullOrEmpty(text))
        {
            text = text.Replace("(", string.Empty).Replace(")", string.Empty);
            string[] array = text.Split(new char[]
            {
                ','
            });
            zero = new Vector3(float.Parse(array[0]), float.Parse(array[1]), float.Parse(array[2]));
        }
        string cacheField_String2 = newUserCfg.GetCacheField_String("fogparam");
        string pattern = "[(][\\s|\\S]{1,}[)]";
        string pattern2 = "{.*?}";
        Color white = Color.white;
        Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
        IEnumerator enumerator = regex.Matches(cacheField_String2).GetEnumerator();
        if (enumerator.MoveNext())
        {
            Match match = (Match)enumerator.Current;
            if (!string.IsNullOrEmpty(match.Value))
            {
                string text2 = match.Value.Replace("(", string.Empty).Replace(")", string.Empty);
                string[] array2 = text2.Split(new char[]
                {
                    ','
                });
                if (array2.Length > 3)
                {
                    white = new Color(float.Parse(array2[0]) / 255f, float.Parse(array2[1]) / 255f, float.Parse(array2[2]) / 255f, float.Parse(array2[3]) / 255f);
                }
            }
        }
        Regex regex2 = new Regex(pattern2, RegexOptions.IgnoreCase);
        IEnumerator enumerator2 = regex2.Matches(cacheField_String2).GetEnumerator();
        float fogStartDistance = 30f;
        float fogEndDistance = 300f;
        if (enumerator2.MoveNext())
        {
            Match match2 = (Match)enumerator2.Current;
            if (!string.IsNullOrEmpty(match2.Value))
            {
                string text3 = match2.Value.Replace("{", string.Empty).Replace("}", string.Empty);
                string[] array3 = text3.Split(new char[]
                {
                    ','
                });
                if (array3.Length > 1)
                {
                    fogStartDistance = float.Parse(array3[0]);
                    fogEndDistance = float.Parse(array3[1]);
                }
            }
        }
        RenderSettings.fogColor = white;
        RenderSettings.fogStartDistance = fogStartDistance;
        RenderSettings.fogEndDistance = fogEndDistance;
        camPosO = zero;
        GameObject result = null;
        if (this.camRoot != null)
        {
            for (int j = 0; j < this.camRoot.transform.childCount; j++)
            {
                GameObject gameObject2 = this.camRoot.transform.GetChild(j).gameObject;
                if (gameObject2.name.StartsWith(cacheField_String))
                {
                    this.rtCam = gameObject2.transform;
                }
                if (gameObject2.name.StartsWith(cacheField_String))
                {
                    result = gameObject2;
                }
                gameObject2.SetActive(gameObject2.name.StartsWith(cacheField_String));
            }
        }
        return result;
    }

    private void SetModelColor(GameObject target, Color c)
    {
        if (target == null)
        {
            return;
        }
        Renderer[] componentsInChildren = target.GetComponentsInChildren<Renderer>();
        if (componentsInChildren != null)
        {
            for (int i = 0; i < componentsInChildren.Length; i++)
            {
                componentsInChildren[i].material.SetColor("_Color", c);
            }
        }
    }

    private void UpdateProtectTime()
    {
        if (this.loadAo != null && this.loadAo.isDone)
        {
            Debug.LogError("Load over");
            this.loadAo = null;
        }
        this.showProtectLeftTime(1);
    }

    private void Update()
    {
        if (this.onSelectPage && (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)))
        {
            this.StartGame();
        }
    }

    public override void OnDispose()
    {
        base.OnDispose();
        this.npd = null;
        this.hbdl = null;
        if (this.rtCam != null)
        {
            UnityEngine.Object.Destroy(this.rtCam.gameObject);
        }
        this.skillDataTable.Clear();
        Scheduler.Instance.RemoveTimer(new Scheduler.OnScheduler(this.UpdateProtectTime));
        Scheduler.Instance.RemoveUpdator(new Scheduler.OnScheduler(this.Update));
        for (int i = 0; i < this.loadedTexRes.Count; i++)
        {
            if (this.loadedTexRes[i] != null)
            {
                this.loadedTexRes[i].TryUnload();
            }
        }
        if (this.modelHangHelperInstance != null)
        {
            this.modelHangHelperInstance.DisposeBonePObj();
        }
    }

    private const int MAX_CHAR_COUNT = 4;

    private Dictionary<uint, PlayerCharactorCreateHelper> modelDic = new Dictionary<uint, PlayerCharactorCreateHelper>();

    private PlayerCharactorCreateHelper modelHangHelperInstance;

    private AVPlayOP avPlayOP_;

    private Dictionary<Transform, bool> raceBtnSelectState = new Dictionary<Transform, bool>();

    private Dictionary<Transform, string> raceItemSpriteName = new Dictionary<Transform, string>();

    private Dictionary<uint, LuaTable> skillDataTable = new Dictionary<uint, LuaTable>();

    private Dictionary<uint, WWW> showVideoCache = new BetterDictionary<uint, WWW>();

    private MSG_Ret_LoginOnReturnCharList_SC charData;

    private SelectUserInfo suiSelectLogin;

    private SEX sexSelect = SEX.NONE;

    private bool isInCreateModel;

    private int isInLoadHairOrFaceCount;

    private long delProtectLeftTime;

    private Text tr_roleDelProtectLeftTime;

    private Transform tr_windowRootSelect;

    private Transform tr_windowRootCreate;

    private Transform tr_rtRawImage;

    private Transform tr_btn_SelectBack;

    private Transform tr_txt_serverName;

    private Transform tr_txt_roleCount;

    private Transform tr_selectScrollRoot;

    private Transform tr_btn_startDelRole;

    private Transform tr_btn_cancelDelRole;

    private Transform tr_btn_cancelDelRole2;

    private Transform tr_btn_sureDelRole;

    private Transform tr_btn_createCenter;

    private Transform tr_btn_create;

    private Transform tr_btn_startGame;

    private Transform tr_btn_cancelDel;

    private Transform tr_btn_cancelDelEnsure;

    private Transform tr_panel_hair_shap;

    private Transform tr_panel_hair_color;

    private Transform tr_panel_face_shap;

    private Transform tr_panel_skill;

    private Transform tr_scrollRootCreate;

    private Transform tr_txt_create_tip;

    private Transform tr_btn_createBack;

    private Transform tr_select_role_information;

    private Transform tr_mesh_vedio;

    private Transform tr_bottom;

    private Transform tr_btn_ok;

    private Transform tr_input_name;

    private Transform tr_sex;

    private Transform tr_btn_roll;

    private Transform randomFeature;

    public bool onSelectPage = true;

    private float originalRtPos;

    private UI_SelectRole.NamePoolData npd;

    private List<UI_SelectRole.HeroBaseData> hbdl_;

    private GameObject lastSelectItemObj;

    private GameObject camRoot;

    private Action randomFeatureActions;

    private Action setFeatrueDefault;

    private Action setColorFeatrueAction;

    private List<UITextureAsset> loadedTexRes = new List<UITextureAsset>();

    private Dictionary<uint, UI_SelectRole.HeroBaseData> hbdDic_;

    public UI_SelectRole.HeroBaseData hbdCurSelect;

    private AsyncOperation loadAo;

    public Transform uiRoot;

    private bool lastMaleSex;

    private float randomActionTimer;

    public Dictionary<ConfigType, int> curSelectIndex = new Dictionary<ConfigType, int>();

    private Dictionary<GameObject, bool> haveHeroState = new Dictionary<GameObject, bool>();

    private string lastSelectCharName = string.Empty;

    private Transform rtCam;

    private RoleUIAnimationControl ruac;

    private GameObject sceneObj;

    private MovieTexture mtTem;

    private class NamePoolData
    {
        public NamePoolData(List<LuaTable> tableNameList)
        {
            if (tableNameList != null)
            {
                for (int i = 0; i < tableNameList.Count; i++)
                {
                    LuaTable luaTable = tableNameList[i];
                    foreach (object obj in luaTable.Keys)
                    {
                        string text = obj.ToString();
                        if (!this.namePool.Keys.Contains(text))
                        {
                            this.namePool[text] = new List<string>();
                        }
                        string field_String = luaTable.GetField_String(text);
                        if (!string.IsNullOrEmpty(field_String))
                        {
                            this.namePool[text].Add(field_String);
                        }
                    }
                }
            }
        }

        public string GetRandomName(bool isMale)
        {
            string randomFirstName = this.GetRandomFirstName();
            string randomCnName = this.GetRandomCnName(isMale);
            string randomEnName = this.GetRandomEnName(isMale);
            string randomSymbol = this.GetRandomSymbol();
            string randomAdjective = this.GetRandomAdjective();
            int num = UnityEngine.Random.Range(0, 100);
            string src = string.Empty;
            if (num < 20)
            {
                src = randomFirstName + randomCnName;
            }
            else if (num < 40 && num >= 20)
            {
                src = randomEnName;
            }
            else if (num < 60 && num >= 40)
            {
                src = randomAdjective + randomCnName;
            }
            else if (num < 80 && num >= 60)
            {
                src = randomAdjective + randomEnName;
            }
            else if (num < 95 && num >= 80)
            {
                src = randomAdjective + randomFirstName + randomCnName;
            }
            else
            {
                src = randomAdjective + randomSymbol + randomFirstName + randomCnName;
            }
            string text = this.LimitString(src, 16);
            KeyWordFilter.InitFilter();
            string text2 = KeyWordFilter.ChatFilter(text);
            if (text != text2)
            {
                Debug.LogErrorFormat("randomName:{0} randomNameChecked:{1}", new object[]
                {
                    text,
                    text2
                });
                text = this.GetRandomName(isMale);
            }
            return text;
        }

        private string GetRandomCnName(bool isMale)
        {
            string nameKey = (!isMale) ? "cnfemalename" : "cnmalename";
            return this.GetRodomNameByKey(nameKey);
        }

        private string GetRandomEnName(bool isMale)
        {
            string nameKey = (!isMale) ? "enfemalename" : "enmalename";
            return this.GetRodomNameByKey(nameKey);
        }

        private string GetRandomSymbol()
        {
            return this.GetRodomNameByKey("symbol");
        }

        private string GetRandomFirstName()
        {
            return this.GetRodomNameByKey("firstname");
        }

        private string GetRandomAdjective()
        {
            return this.GetRodomNameByKey("adjective");
        }

        private string GetRodomNameByKey(string nameKey)
        {
            if (this.namePool.ContainsKey(nameKey))
            {
                int index = UnityEngine.Random.Range(0, this.namePool[nameKey].Count);
                return this.namePool[nameKey][index];
            }
            return string.Empty;
        }

        private string LimitString(string src, int length)
        {
            int length2 = src.Length;
            if (length2 > length)
            {
                src = src.Substring(0, 16);
            }
            return src;
        }

        private Dictionary<string, List<string>> namePool = new BetterDictionary<string, List<string>>();
    }

    public class HeroBaseData
    {
        public uint id;

        public uint bodyid;

        public string name;

        public string model;

        public string icon;

        public string des;

        public string charactFeature;

        public string ablility;

        public SEX sex;

        public string stage;

        public string hairdoicon;

        public string hairstyle;

        public string coloricon;

        public string color;

        public string featureicon;

        public string facestyle;

        public string antennaicon;

        public string antenna;

        public string skill;

        public string videotape;

        public uint[] defaultFeatureIDs;

        public Dictionary<string, string[]> colorStyleIndex;

        public bool isLoadAllRes;
    }
}
