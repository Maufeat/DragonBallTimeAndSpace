using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Chat;
using Engine;
using Framework.Managers;
using Game.Scene;
using LuaInterface;
using Obj;
using ResoureManager;
using Team;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public static class GlobalRegister
{
    [Conditional("UNITY_EDITOR")]
    public static void BeginSample(int id)
    {
        string text;
        GlobalRegister._showNames.TryGetValue(id, out text);
        text = (text ?? string.Empty);
        GlobalRegister._sampleDepth++;
    }

    [Conditional("UNITY_EDITOR")]
    public static void BeginSample(int id, string name)
    {
        name = (name ?? string.Empty);
        GlobalRegister._showNames[id] = name;
        GlobalRegister._sampleDepth++;
    }

    [Conditional("UNITY_EDITOR")]
    internal static void BeginSample(string name)
    {
        name = (name ?? string.Empty);
        GlobalRegister._sampleDepth++;
    }

    [Conditional("UNITY_EDITOR")]
    public static void EndSample()
    {
        if (GlobalRegister._sampleDepth > 0)
        {
            GlobalRegister._sampleDepth--;
        }
    }

    public static void OpenUICustom(string openUIName, GameObject obj)
    {
        ManagerCenter.Instance.GetManager<EscManager>().OpenUI(openUIName);
        obj.transform.SetAsLastSibling();
    }

    public static void ClearChildrens(Transform parent)
    {
        UIManager.Instance.ClearListChildrens(parent, 1);
    }

    public static uint GetMCurrentCopyID()
    {
        if (ManagerCenter.Instance.GetManager<CopyManager>() != null)
        {
            return ManagerCenter.Instance.GetManager<CopyManager>().MCurrentCopyID;
        }
        return 0U;
    }

    public static void SetNpcDlgShowState(bool isshow)
    {
        BreakAutoattackUIMgr component = MainPlayer.Self.GetComponent<BreakAutoattackUIMgr>();
        if (component != null)
        {
            if (isshow)
            {
                component.ProcessOpenedUI("UI_NPCtalk", true);
            }
            else
            {
                component.ProcessDeletedUI("UI_NPCtalk");
            }
        }
        if (MainPlayer.Self == null || isshow)
        {
        }
    }

    public static uint GetMainPlayerHeroId()
    {
        if (MainPlayer.Self != null && MainPlayer.Self.BaseData != null)
        {
            return MainPlayer.Self.BaseData.GetBaseOrHeroId();
        }
        return 0U;
    }

    public static void TryToShowCompleteCopyView()
    {
        ControllerManager.Instance.GetController<CompleteCopyController>().ShowCompleteCopyView();
    }

    public static string GetPathwayID()
    {
        if (ControllerManager.Instance.GetController<TaskController>() != null)
        {
            string wantobject = ControllerManager.Instance.GetController<TaskController>().wantobject;
            ControllerManager.Instance.GetController<TaskController>().wantobject = null;
            return wantobject;
        }
        return null;
    }

    public static int GetLineId()
    {
        if (ManagerCenter.Instance.GetManager<GameScene>() != null)
        {
            return ManagerCenter.Instance.GetManager<GameScene>().CurrentLineID;
        }
        return 0;
    }

    public static PropsBase GetResourcePropsBaseBy(uint ResourceId)
    {
        return new PropsBase(ResourceId, MainPlayer.Self.GetCurrencyByID(ResourceId));
    }

    public static PropsBase GetResourcePropsBaseBy_Obj(t_Object obj)
    {
        return new PropsBase(obj);
    }

    public static string GetCurrenyStr(uint num)
    {
        return CommonTools.GetCurrenyStr(num);
    }

    public static void SetItemTextName(Text nametext, LuaTable config)
    {
        nametext.text = config.GetField_String("name");
        switch (config.GetField_Uint("quality"))
        {
            case 1U:
                {
                    string modelColor = ControllerManager.Instance.GetController<TextModelController>().GetModelColor("Item1");
                    nametext.color = CommonTools.HexToColor(modelColor);
                    break;
                }
            case 2U:
                {
                    string modelColor2 = ControllerManager.Instance.GetController<TextModelController>().GetModelColor("Item2");
                    nametext.color = CommonTools.HexToColor(modelColor2);
                    break;
                }
            case 3U:
                {
                    string modelColor3 = ControllerManager.Instance.GetController<TextModelController>().GetModelColor("Item3");
                    nametext.color = CommonTools.HexToColor(modelColor3);
                    break;
                }
            case 4U:
                {
                    string modelColor4 = ControllerManager.Instance.GetController<TextModelController>().GetModelColor("Item4");
                    nametext.color = CommonTools.HexToColor(modelColor4);
                    break;
                }
        }
    }

    public static cs_AttributeData GetMainPlayerAttributeData()
    {
        if (MainPlayer.Self != null && MainPlayer.Self.MainPlayeData != null)
        {
            return MainPlayer.Self.MainPlayeData.AttributeData;
        }
        return null;
    }

    public static cs_MapUserData GetCharacterMapData(CharactorBase charactor)
    {
        OtherPlayer otherPlayer = charactor as OtherPlayer;
        if (otherPlayer != null)
        {
            return otherPlayer.OtherPlayerData.MapUserData;
        }
        return null;
    }

    public static uint GetSelfFightData()
    {
        if (MainPlayer.Self == null)
        {
            return 0U;
        }
        return MainPlayer.Self.MainPlayeData.FightData.curfightvalue;
    }

    public static int GetByteCount(string content)
    {
        return Encoding.UTF8.GetBytes(content).Length;
    }

    public static GameTime GetGameTime()
    {
        return SingletonForMono<GameTime>.Instance;
    }

    public static bool get_flag(uint code, uint flag)
    {
        return (code & flag) != 0U;
    }

    public static uint bor_flag(uint code, uint flag)
    {
        return code | flag;
    }

    public static uint bxor_flag(uint code, uint flag)
    {
        return code ^ flag;
    }

    public static string utf8to(string value)
    {
        byte[] array = Encoding.Convert(Encoding.UTF7, Encoding.UTF8, Encoding.Unicode.GetBytes(value));
        if (array != null && array.Length > 0)
        {
            return Encoding.UTF8.GetString(array);
        }
        return string.Empty;
    }

    public static void OpenMouseState(int state)
    {
        MouseStateControoler.Instan.SetMoseState((MoseState)state);
    }

    public static void AddTextTipComponent(GameObject obj, uint textid)
    {
        TextTip textTip = obj.GetComponent<TextTip>();
        if (textTip == null)
        {
            textTip = obj.AddComponent<TextTip>();
        }
        TextModelController controller = ControllerManager.Instance.GetController<TextModelController>();
        string contentByIDWithoutColorText = controller.GetContentByIDWithoutColorText(textid.ToString());
        textTip.SetText(contentByIDWithoutColorText);
    }

    public static void AddTextTipComponentString(GameObject obj, string text)
    {
        TextTip textTip = obj.GetComponent<TextTip>();
        if (textTip == null)
        {
            textTip = obj.AddComponent<TextTip>();
        }
        textTip.SetText(text);
    }

    public static void AddTextTipTimerComponent(GameObject obj, uint textid, float time, float offsetX = 0f, float offsetY = 0f)
    {
        TextTipTimer textTipTimer = obj.GetComponent<TextTipTimer>();
        if (textTipTimer == null)
        {
            textTipTimer = obj.AddComponent<TextTipTimer>();
        }
        TextModelController controller = ControllerManager.Instance.GetController<TextModelController>();
        string contentByIDWithoutColorText = controller.GetContentByIDWithoutColorText(textid.ToString());
        textTipTimer.SetCountDownText(contentByIDWithoutColorText, time, 0f, 0f);
    }

    public static void AddTextTipUpdateComponent(GameObject obj, uint pvpMatchType, float time, float offsetX = 0f, float offsetY = 0f)
    {
        TextTipUpdate textTipUpdate = obj.GetComponent<TextTipUpdate>();
        if (textTipUpdate == null)
        {
            textTipUpdate = obj.AddComponent<TextTipUpdate>();
        }
        textTipUpdate.SetTipCountDownText((PvpMatchType)pvpMatchType, time, 0f, 0f);
    }

    public static void ReqSplitOperate(uint num, int type)
    {
        UIBagManager.Instance.ReqSplitObject(num, type);
    }

    public static Color GetColorByName(string name)
    {
        return Const.GetColorByName(name);
    }

    public static void UpdateLastOutlineColor(Transform trans, string colorName)
    {
        Color colorByName = Const.GetColorByName(colorName);
        Outline[] componentsInChildren = trans.GetComponentsInChildren<Outline>();
        if (componentsInChildren == null || componentsInChildren.Length < 1)
        {
            return;
        }
        Outline outline = componentsInChildren[componentsInChildren.Length - 1];
        outline.effectColor = colorByName;
    }

    public static void UpdateTextrue(Image img, uint type, string strIcon)
    {
        if (img == null)
        {
            return;
        }
        ImageType imgType = (ImageType)type;
        ManagerCenter.Instance.GetManager<UITextureMgr>().GetTexture(imgType, strIcon, delegate (UITextureAsset asset)
        {
            if (img == null)
            {
                return;
            }
            if (asset == null)
            {
                FFDebug.LogWarning("GlobalRegister", string.Format("UpdateTextrue req texture is null ImgaeType = {0} strIcon = {1}", imgType, strIcon));
                return;
            }
            Texture2D textureObj = asset.textureObj;
            Sprite overrideSprite = Sprite.Create(textureObj, new Rect(0f, 0f, (float)textureObj.width, (float)textureObj.height), new Vector2(0f, 0f));
            img.overrideSprite = overrideSprite;
            img.gameObject.SetActive(true);
        });
    }

    public static void UpdateIconBG(Image img, uint quality)
    {
        string spritename = string.Empty;
        switch (quality)
        {
            case 1U:
                spritename = "st0099";
                break;
            case 2U:
                spritename = "st0100";
                break;
            case 3U:
                spritename = "st0101";
                break;
            case 4U:
                spritename = "st0102";
                break;
        }
        ManagerCenter.Instance.GetManager<UITextureMgr>().GetSpriteFromAtlas("base", spritename, delegate (Sprite sprite)
        {
            if (img == null)
            {
                return;
            }
            img.overrideSprite = sprite;
            img.gameObject.SetActive(true);
        });
    }

    public static void MainUIAddMesssage(int Msgtype, int InviteCountNoRead, Action callback)
    {
        ControllerManager.Instance.GetController<MainUIController>().AddMessage((MessageType)Msgtype, InviteCountNoRead, delegate
        {
            if (callback != null)
            {
                callback();
            }
            ControllerManager.Instance.GetController<MainUIController>().ReadMessage((MessageType)Msgtype);
        });
    }

    public static void MainUIReadMesssage(int Msgtype)
    {
        ControllerManager.Instance.GetController<MainUIController>().ReadMessage((MessageType)Msgtype);
    }

    public static void RegisertConvenientFunction(uint id, LuaFunction callback)
    {
        ConvenientProcess.RegisertConvenientFunctionLua(id, callback);
    }

    public static void ShowMsgBoxToFriend(bool fromTakemaster, LuaFunction SureCallback)
    {
        string s_describle = string.Empty;
        if (fromTakemaster)
        {
            s_describle = CommonUtil.GetText(dynamic_textid.IDs.friendisfullcannottakemaster);
        }
        else
        {
            s_describle = CommonUtil.GetText(dynamic_textid.IDs.friendisfullcannottakeapprentice);
        }
        ControllerManager.Instance.GetController<MsgBoxController>().ShowMsgBox(MsgBoxController.MsgLevelNormal, s_describle, CommonUtil.GetText(dynamic_textid.BaseIDs.confirm), CommonUtil.GetText(dynamic_textid.BaseIDs.cancel), UIManager.ParentType.CommonUI, delegate ()
        {
            if (SureCallback != null)
            {
                SureCallback.Call();
            }
            ControllerManager.Instance.GetController<FriendControllerNew>().ShowFriendUI(null);
        }, null, null);
    }

    public static void ShowMsgBoxUnpublishMasterApprentice(bool unPublishTakeMaster, LuaFunction SureCallback)
    {
        string s_describle = string.Empty;
        if (unPublishTakeMaster)
        {
            s_describle = CommonUtil.GetText(dynamic_textid.IDs.unpublish_takemaster);
        }
        else
        {
            s_describle = CommonUtil.GetText(dynamic_textid.IDs.unpublish_takeapprentice);
        }
        ControllerManager.Instance.GetController<MsgBoxController>().ShowMsgBox(MsgBoxController.MsgLevelNormal, s_describle, CommonUtil.GetText(dynamic_textid.BaseIDs.confirm), CommonUtil.GetText(dynamic_textid.BaseIDs.cancel), UIManager.ParentType.CommonUI, delegate ()
        {
            if (SureCallback != null)
            {
                SureCallback.Call();
            }
        }, null, null);
    }

    public static void ShowMsgBoxTryQuitApprentice(bool isNow, string name, LuaFunction SureCallback)
    {
    }

    public static void ShowEnsureMsgBox(string title, string des, string ensure, string cancel, LuaFunction sureCallback, LuaFunction cancelCall)
    {
        MsgBoxController controller = ControllerManager.Instance.GetController<MsgBoxController>();
        if (controller != null)
        {
            controller.ShowMsgBox(title, des, ensure, cancel, UIManager.ParentType.CommonUI, delegate ()
            {
                if (sureCallback != null)
                {
                    sureCallback.Call();
                }
            }, delegate ()
            {
                if (cancelCall != null)
                {
                    cancelCall.Call();
                }
            }, delegate ()
            {
                UnityEngine.Debug.LogError("close");
            });
        }
    }

    public static void ShowMsgBoxPreQuitApprentice(uint reqType, string name, uint quitTime, uint offlinetime, uint punishTime, LuaFunction SureCallback)
    {
        string s_describle = string.Empty;
        if (reqType == 1U)
        {
            uint num = quitTime / 3600U;
            uint num2 = punishTime / 3600U;
            s_describle = string.Format(CommonUtil.GetText(dynamic_textid.IDs.apprentice_removetips), name, num, num2);
        }
        else if (reqType == 2U)
        {
            s_describle = CommonUtil.GetText(dynamic_textid.IDs.apprentice_beremoved);
        }
        else
        {
            if (reqType != 3U)
            {
                return;
            }
            uint num3 = offlinetime / 86400U;
            s_describle = string.Format(CommonUtil.GetText(dynamic_textid.IDs.apprentice_removenow), name, num3);
        }
        ControllerManager.Instance.GetController<MsgBoxController>().ShowMsgBox(MsgBoxController.MsgLevelNormal, s_describle, CommonUtil.GetText(dynamic_textid.BaseIDs.confirm), CommonUtil.GetText(dynamic_textid.BaseIDs.cancel), UIManager.ParentType.CommonUI, delegate ()
        {
            if (SureCallback != null)
            {
                SureCallback.Call();
            }
        }, null, null);
    }

    public static void ShowPrivateChat(ulong id, string username, uint userlevel, uint career)
    {
    }

    public static void InviteToTeam(ulong id)
    {
        ControllerManager.Instance.GetController<TeamController>().ReqInviteIntoTeam_CS(id.ToString());
    }

    public static void ApplyTeam(uint id)
    {
        ControllerManager.Instance.GetController<TeamController>().ApplyTeam(id);
    }

    public static void ActiveNewMailTips(int status)
    {
        ControllerManager.Instance.GetController<UIMapController>().ActiveNewMailTips(status);
    }

    public static void SetTextTimer(Text tex, uint leftTime)
    {
        UIInformationList component = tex.GetComponent<UIInformationList>();
        if (component != null && component.listInformation.Count > 0)
        {
            tex.text = ControllerManager.Instance.GetController<TextModelController>().ChangeTextModel(string.Format(component.listInformation[0].content, leftTime));
        }
    }

    public static void ShowMasterOrApprenticeListIsFull(bool bMaster)
    {
        if (bMaster)
        {
            TipsWindow.ShowWindow(TipsType.MASTERFULL, null);
        }
        else
        {
            TipsWindow.ShowWindow(TipsType.APPRENTICEFULL, null);
        }
    }

    public static string GetTimeInHours(uint leftTime)
    {
        uint num = 0U;
        uint num2 = 0U;
        uint num3 = leftTime;
        if (num3 > 60U)
        {
            num2 = num3 / 60U;
            num3 %= 60U;
        }
        if (num2 > 60U)
        {
            num = num2 / 60U;
            num2 %= 60U;
        }
        if (num > 0U)
        {
            return string.Format("{0}:{1:D2}:{2:D2}", num, num2, num3);
        }
        if (num2 > 0U)
        {
            return string.Format("00:{0:D2}:{1:D2}", num2, num3);
        }
        return string.Format("00:00:{0:D2}", num3);
    }

    public static string GetTimeInMinutes(uint curTime)
    {
        uint num = 0U;
        uint num2 = curTime;
        if (num2 > 60U)
        {
            num = num2 / 60U;
            num2 %= 60U;
        }
        if (num > 0U)
        {
            return string.Format("{0:D2}:{1:D2}", num, num2);
        }
        return string.Format("00:{0:D2}", num2);
    }

    public static string GetTimeInDays(uint leftTime)
    {
        uint num = 0U;
        uint num2 = 0U;
        uint num3 = 0U;
        if (leftTime > 60U)
        {
            num3 = leftTime / 60U;
            uint num4 = leftTime % 60U;
        }
        if (num3 > 60U)
        {
            num2 = num3 / 60U;
            num3 %= 60U;
        }
        if (num2 > 24U)
        {
            num = num2 / 60U;
            num2 %= 24U;
        }
        if (num > 0U)
        {
            return string.Format("{0:D2}天{1:D2}时{2:D2}分", num, num2, num3);
        }
        if (num2 > 0U)
        {
            return string.Format("00天{0:D2}时{1:D2}分", num2, num3);
        }
        return string.Format("00天00时{0:D2}分", num3);
    }

    public static void CopyToSameParent(Transform trans, int num)
    {
        if (trans == null)
        {
            return;
        }
        int num2 = trans.parent.childCount - 1;
        if (num2 < num)
        {
            for (int i = 0; i < num - num2; i++)
            {
                GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(trans.gameObject);
                gameObject.transform.SetParent(trans.parent);
                gameObject.transform.localScale = trans.localScale;
                gameObject.transform.localRotation = trans.localRotation;
            }
        }
    }

    public static CountDownItem AddCountDownComponent(GameObject rootObj, Text txt, uint duartion)
    {
        if (rootObj == null)
        {
            return null;
        }
        CountDownItem countDownItem = rootObj.AddComponent<CountDownItem>();
        countDownItem.InitItem(rootObj, txt, duartion);
        return countDownItem;
    }

    public static void SetMainPlayerName(string name)
    {
        MainPlayer.Self.OtherPlayerData.MapUserData.name = name;
        UI_MainView mainView = ControllerManager.Instance.GetController<MainUIController>().mainView;
        if (mainView != null)
        {
            mainView.setPersonal();
        }
    }

    public static void ShowMsgBox(string tmpStr, LuaFunction SureCallback, LuaFunction cancelCallback)
    {
        ulong heroId = ulong.Parse(tmpStr);
        GlobalRegister.ShowGetHeroTips(heroId);
    }

    private static void ShowGetHeroTips(ulong heroId)
    {
        UIManager.Instance.ShowUI<UI_GetNewHero>("UI_GetNewHero", delegate ()
        {
            UI_GetNewHero uiobject = UIManager.GetUIObject<UI_GetNewHero>();
            if (uiobject == null)
            {
                GlobalRegister.ShowGetHeroTips(heroId);
            }
            else
            {
                uiobject.AddHeroId(heroId);
            }
        }, UIManager.ParentType.CommonUI, false);
    }

    public static void ShowTwoBtnMsgBox(uint s_title_textid, uint s_describle_textid, uint confirm_textid, uint cancel_textid, UIManager.ParentType ParentType, LuaFunction SureCallback, LuaFunction cancelCallback)
    {
        ControllerManager.Instance.GetController<MsgBoxController>().ShowMsgBox(CommonUtil.GetText(s_title_textid), CommonUtil.GetText(s_describle_textid), CommonUtil.GetText(confirm_textid), CommonUtil.GetText(cancel_textid), ParentType, delegate ()
        {
            if (SureCallback != null)
            {
                SureCallback.Call();
            }
        }, delegate ()
        {
            if (cancelCallback != null)
            {
                cancelCallback.Call();
            }
        }, null);
    }

    public static void ShowTips(uint tipsId)
    {
        TipsWindow.ShowWindow(tipsId);
    }

    public static void SetDragDropButtonDataNull(GameObject obj)
    {
        DragDropButton component = obj.GetComponent<DragDropButton>();
        if (component != null)
        {
            component.mData = null;
        }
    }

    public static void ShowLoadTip(string text)
    {
        if (ControllerManager.Instance.GetController<LoadTipsController>() != null)
        {
            ControllerManager.Instance.GetController<LoadTipsController>().ShowReconnectTips(text);
        }
    }

    public static void CloseLoadTip()
    {
        if (ControllerManager.Instance.GetController<LoadTipsController>() != null)
        {
            ControllerManager.Instance.GetController<LoadTipsController>().CloseReconnectTips();
        }
    }

    public static void ShowCommonItemDesc(uint id, Transform rootTrans)
    {
        Scheduler.Instance.AddFrame(2U, false, delegate
        {
            PropsBase data = new PropsBase(id, 1U);
            data._obj.thisid = "0";
            if (UIManager.GetUIObject<UI_EquipItemInfo>() == null)
            {
                ManagerCenter.Instance.GetManager<UIManager>().ShowUI<UI_EquipItemInfo>("UI_ItemInfo", delegate ()
                {
                    if (UIManager.GetUIObject<UI_EquipItemInfo>() != null)
                    {
                        UIManager.GetUIObject<UI_EquipItemInfo>().ViewInfoCommon(data, rootTrans);
                    }
                }, UIManager.ParentType.CommonUI, false);
            }
            else
            {
                UIManager.GetUIObject<UI_EquipItemInfo>().ViewInfoCommon(data, rootTrans);
            }
        });
    }

    public static void ShowRaneItemDesc(Transform tran, LuaTable data, string thisidstr, int type, Action btn1evt, Action btn2evt)
    {
        Scheduler.Instance.AddFrame(2U, false, delegate
        {
            if (UIManager.GetUIObject<UI_EquipItemInfo>() == null)
            {
                ManagerCenter.Instance.GetManager<UIManager>().ShowUI<UI_EquipItemInfo>("UI_ItemInfo", delegate ()
                {
                    if (UIManager.GetUIObject<UI_EquipItemInfo>() != null)
                    {
                        UIManager.GetUIObject<UI_EquipItemInfo>().OpenMyRuneViewInfo(tran, data, thisidstr, type, btn1evt, btn2evt);
                    }
                }, UIManager.ParentType.CommonUI, false);
            }
            else
            {
                UIManager.GetUIObject<UI_EquipItemInfo>().OpenMyRuneViewInfo(tran, data, thisidstr, type, btn1evt, btn2evt);
            }
        });
    }

    public static bool CheckNameFormat(string str)
    {
        return CommonTools.CheckNameFormat(str);
    }

    public static void LoadByteData(string path, LuaFunction callback)
    {
        ManagerCenter.Instance.GetManager<ResourceManager>().LoadByteData(path, delegate (byte[] bytes)
        {
            callback.Call(new object[]
            {
                new LuaStringBuffer(bytes)
            });
        });
    }

    public static void EnterCompetition()
    {
        ControllerManager.Instance.GetController<MainUIController>().EnterCompetition();
        ManagerCenter.Instance.GetManager<EntitiesManager>().setPlayerShowState(PlayerShowState.hide);
        CameraController component = Camera.main.gameObject.GetComponent<CameraController>();
        if (component != null)
        {
            component.EnterPrepareState();
        }
        else
        {
            GameSystemSettings.SetCameraState(CameraState.CameraFollowPrepare);
        }
    }

    public static void ExitCompetition()
    {
        ControllerManager.Instance.GetController<MainUIController>().ExitCompetition();
        ManagerCenter.Instance.GetManager<EntitiesManager>().setPlayerShowState(PlayerShowState.show);
        CameraController component = Camera.main.gameObject.GetComponent<CameraController>();
        if (component != null)
        {
            component.RestoreCamera();
        }
    }

    public static List<Memember> GetTeamList()
    {
        List<Memember> list = new List<Memember>();
        if (ControllerManager.Instance.GetController<TeamController>().myTeamInfo.mem.Count == 0)
        {
            return null;
        }
        for (int i = 0; i < ControllerManager.Instance.GetController<TeamController>().myTeamInfo.mem.Count; i++)
        {
            list.Add(ControllerManager.Instance.GetController<TeamController>().myTeamInfo.mem[i]);
        }
        return list;
    }

    public static MSG_TeamMemeberList_SC GetTeamInfo()
    {
        if (ControllerManager.Instance.GetController<TeamController>().myTeamInfo.mem.Count == 0)
        {
            return null;
        }
        return ControllerManager.Instance.GetController<TeamController>().myTeamInfo;
    }

    public static void ChangeMainPanelMenu(uint menuId, bool menuState)
    {
        ControllerManager.Instance.GetController<MainUIController>().ChangeMainPanelMenu(menuId, menuState);
    }

    public static void ShowChatUI()
    {
    }

    public static void SetChatUIState()
    {
    }

    public static void SetBagUICloseState()
    {
    }

    public static void SetTaskUICloseState()
    {
    }

    public static void SetHeroUICloseState()
    {
    }

    public static void SetCharacterUIAttr()
    {
        UI_Character uiobject = UIManager.GetUIObject<UI_Character>();
        if (uiobject != null)
        {
            uiobject.SetupHeroInfoPanel();
        }
    }

    public static void SetGeneUICloseState(string state)
    {
    }

    public static void SetInstanceUICloseState(string state)
    {
    }

    public static void SetMentoringUICloseState(string state)
    {
    }

    public static void SetSysSettingUICloseState()
    {
        ControllerManager.Instance.GetController<ShortcutsConfigController>().SetSettingUICloseState();
    }

    public static void OpenShortcutsConfig()
    {
        UIManager.Instance.ShowUI<UI_ShortcutsConfig>("UI_ShortcutsConfig", delegate ()
        {
        }, UIManager.ParentType.CommonUI, false);
    }

    public static void OpenResetSecondPwd()
    {
        ControllerManager.Instance.GetController<SecondPwdControl>().ShowSecondPwd(SecondPwdControl.Second_PWD_Show_Page.PAGE_RESET_SECOND_PWD, false);
    }

    public static uint GetConstStoneID()
    {
        return 2U;
    }

    public static uint GetConstMoneyID()
    {
        return 3U;
    }

    public static uint GetConstWelPointID()
    {
        return 4U;
    }

    public static void ShortCutUseEquipController_Reset()
    {
        ControllerManager.Instance.GetController<ShortCutUseEquipController>().Reset();
    }

    public static void MainUIController_AddMessage()
    {
        ControllerManager.Instance.GetController<MainUIController>().AddMessage(MessageType.Bag, 0, delegate
        {
            LuaScriptMgr.Instance.CallLuaFunction("BagCtrl.EnterPackage", new object[]
            {
                Util.GetLuaTable("BagCtrl")
            });
        });
    }

    public static void MainUIController_ReadMessage()
    {
        ControllerManager.Instance.GetController<MainUIController>().ReadMessage(MessageType.Bag);
    }

    public static void InitBagManager()
    {
        DragDropManager.Instance.Init();
        UIBagManager.Instance.Initilize();
    }

    public static void OpenInfo(PropsBase data, bool isequip)
    {
        LuaTable cacheField_Table = LuaConfigManager.GetXmlConfigTable("btnaction").GetCacheField_Table("btnactionmapbyid").GetCacheField_Table(data._obj.baseid.ToString());
        if (cacheField_Table == null)
        {
            cacheField_Table = LuaConfigManager.GetXmlConfigTable("btnaction").GetCacheField_Table("btnactionmap").GetCacheField_Table(((int)data._obj.type).ToString());
            if (cacheField_Table != null && cacheField_Table.GetCacheField_String("actions") == "use")
            {
                GlobalRegister.ReqUseItem(data);
                return;
            }
        }
        else if (cacheField_Table.GetCacheField_String("actions") == "use")
        {
            GlobalRegister.ReqUseItem(data);
            return;
        }
    }

    private static void ReqUseItem(PropsBase data)
    {
        if (data.config.GetField_Uint("type") == 67U)
        {
            GlobalRegister.ReqExecuteQuest2ChangeBuf(0U, "u" + data.config.GetField_Uint("id"), 0U, 0U, GlobalRegister.GetSelectTargetID());
            return;
        }
        LuaTable config = data.config;
        if (config.GetField_Uint("maxnum") > 1U && config.GetField_Uint("cdtime") == 0U && data.Count > 1U)
        {
            UIBagManager.Instance.TryReqUseObject(data);
            return;
        }
        LuaScriptMgr.Instance.CallLuaFunction("BagCtrl.ReqUseItem", new object[]
        {
            Util.GetLuaTable("BagCtrl"),
            data,
            1
        });
    }

    public static void ActivateInputField(InputField inputfield)
    {
        inputfield.ActivateInputField();
    }

    public static void SetImageGrey(Image img, bool isGrey)
    {
        if (null != img)
        {
            if (isGrey)
            {
                img.color = Color.gray;
            }
            else
            {
                img.color = Color.white;
            }
        }
    }

    public static void OpenInfoCommon(uint Id, Transform tran)
    {
        Scheduler.Instance.AddFrame(2U, false, delegate
        {
            PropsBase data = new PropsBase(Id, 1U);
            data._obj.thisid = "0";
            if (UIManager.GetUIObject<UI_EquipItemInfo>() == null)
            {
                ManagerCenter.Instance.GetManager<UIManager>().ShowUI<UI_EquipItemInfo>("UI_ItemInfo", delegate ()
                {
                    if (UIManager.GetUIObject<UI_EquipItemInfo>() != null)
                    {
                        UIManager.GetUIObject<UI_EquipItemInfo>().ViewInfoCommon(data, tran);
                    }
                }, UIManager.ParentType.CommonUI, false);
            }
            else
            {
                UIManager.GetUIObject<UI_EquipItemInfo>().ViewInfoCommon(data, tran);
            }
        });
    }

    public static void OpenInfoRune(Transform tran, LuaTable data, string thisidstr, int type, Action btn1evt, Action btn2evt)
    {
        Scheduler.Instance.AddFrame(2U, false, delegate
        {
            if (UIManager.GetUIObject<UI_EquipItemInfo>() == null)
            {
                ManagerCenter.Instance.GetManager<UIManager>().ShowUI<UI_EquipItemInfo>("UI_ItemInfo", delegate ()
                {
                    if (UIManager.GetUIObject<UI_EquipItemInfo>() != null)
                    {
                        UIManager.GetUIObject<UI_EquipItemInfo>().OpenMyRuneViewInfo(tran, data, thisidstr, type, btn1evt, btn2evt);
                    }
                }, UIManager.ParentType.CommonUI, false);
            }
            else
            {
                UIManager.GetUIObject<UI_EquipItemInfo>().OpenMyRuneViewInfo(tran, data, thisidstr, type, btn1evt, btn2evt);
            }
        });
    }

    public static void OpenInfoShortcutUse(PropsBase data)
    {
        Scheduler.Instance.AddFrame(2U, false, delegate
        {
            if (UIManager.GetUIObject<UI_EquipItemInfo>() == null)
            {
                ManagerCenter.Instance.GetManager<UIManager>().ShowUI<UI_EquipItemInfo>("UI_ItemInfo", delegate ()
                {
                    if (UIManager.GetUIObject<UI_EquipItemInfo>() != null)
                    {
                        UIManager.GetUIObject<UI_EquipItemInfo>().ViewInfoCommon(data, null);
                    }
                }, UIManager.ParentType.CommonUI, false);
            }
            else
            {
                UIManager.GetUIObject<UI_EquipItemInfo>().ViewInfoCommon(data, null);
            }
        });
    }

    public static void CloseInfo()
    {
        if (UIManager.GetUIObject<UI_EquipItemInfo>() != null)
        {
            ManagerCenter.Instance.GetManager<UIManager>().DeleteUI("UI_ItemInfo");
        }
    }

    public static void OnPlayerCuccencyUpdateEvent_Add()
    {
        MainPlayer self = MainPlayer.Self;
        self.OnPlayerCuccencyUpdateEvent = (MainPlayer.VoidDelegate)Delegate.Remove(self.OnPlayerCuccencyUpdateEvent, new MainPlayer.VoidDelegate(MainPlayer.Self.UpdateCurrency));
        MainPlayer self2 = MainPlayer.Self;
        self2.OnPlayerCuccencyUpdateEvent = (MainPlayer.VoidDelegate)Delegate.Combine(self2.OnPlayerCuccencyUpdateEvent, new MainPlayer.VoidDelegate(MainPlayer.Self.UpdateCurrency));
    }

    public static void OnPlayerCuccencyUpdateEvent_Delete()
    {
    }

    public static void AddCurrencyItem(uint id, uint num)
    {
        ShortCutUseEquipController controller = ControllerManager.Instance.GetController<ShortCutUseEquipController>();
        if (controller != null)
        {
            controller.AddCurrencyItem(id, num);
        }
    }

    public static void AddShortcutItem(PropsBase pb)
    {
        ShortCutUseEquipController controller = ControllerManager.Instance.GetController<ShortCutUseEquipController>();
        if (controller != null)
        {
            controller.AddShortcutItem(pb);
        }
    }

    public static void OnPlayerCuccencyUpdateEvent()
    {
        if (MainPlayer.Self.OnPlayerCuccencyUpdateEvent != null)
        {
            MainPlayer.Self.OnPlayerCuccencyUpdateEvent();
        }
    }

    public static ObjectType IntToEnum(int i)
    {
        return (ObjectType)i;
    }

    public static uint GetTidyPackCD()
    {
        return 5U;
    }

    public static t_TidyPackInfo GetTidyPackInfo()
    {
        return new t_TidyPackInfo();
    }

    public static string GetServerDateTimeByTimeStamp(int time)
    {
        DateTime serverDateTimeByTimeStamp = SingletonForMono<GameTime>.Instance.GetServerDateTimeByTimeStamp((ulong)((long)time * 1000L));
        string text = string.Empty;
        if (serverDateTimeByTimeStamp.Month < 10)
        {
            text = "0" + serverDateTimeByTimeStamp.Month.ToString();
        }
        else
        {
            text = serverDateTimeByTimeStamp.Month.ToString();
        }
        string text2 = string.Empty;
        if (serverDateTimeByTimeStamp.Day < 10)
        {
            text2 = "0" + serverDateTimeByTimeStamp.Day.ToString();
        }
        else
        {
            text2 = serverDateTimeByTimeStamp.Day.ToString();
        }
        string text3 = string.Empty;
        if (serverDateTimeByTimeStamp.Hour < 10)
        {
            text3 = "0" + serverDateTimeByTimeStamp.Hour.ToString();
        }
        else
        {
            text3 = serverDateTimeByTimeStamp.Hour.ToString();
        }
        string text4 = string.Empty;
        if (serverDateTimeByTimeStamp.Minute < 10)
        {
            text4 = "0" + serverDateTimeByTimeStamp.Minute.ToString();
        }
        else
        {
            text4 = serverDateTimeByTimeStamp.Minute.ToString();
        }
        return string.Concat(new string[]
        {
            text,
            "-",
            text2,
            " ",
            text3,
            ":",
            text4
        });
    }

    public static string GetServerDateTimeByTimeStampWithAllTime(int time)
    {
        DateTime serverDateTimeByTimeStamp = SingletonForMono<GameTime>.Instance.GetServerDateTimeByTimeStamp((ulong)((long)time * 1000L));
        string text = string.Empty;
        text = serverDateTimeByTimeStamp.Year.ToString();
        string text2 = string.Empty;
        if (serverDateTimeByTimeStamp.Month < 10)
        {
            text2 = "0" + serverDateTimeByTimeStamp.Month.ToString();
        }
        else
        {
            text2 = serverDateTimeByTimeStamp.Month.ToString();
        }
        string text3 = string.Empty;
        if (serverDateTimeByTimeStamp.Day < 10)
        {
            text3 = "0" + serverDateTimeByTimeStamp.Day.ToString();
        }
        else
        {
            text3 = serverDateTimeByTimeStamp.Day.ToString();
        }
        string text4 = string.Empty;
        if (serverDateTimeByTimeStamp.Hour < 10)
        {
            text4 = "0" + serverDateTimeByTimeStamp.Hour.ToString();
        }
        else
        {
            text4 = serverDateTimeByTimeStamp.Hour.ToString();
        }
        string text5 = string.Empty;
        if (serverDateTimeByTimeStamp.Minute < 10)
        {
            text5 = "0" + serverDateTimeByTimeStamp.Minute.ToString();
        }
        else
        {
            text5 = serverDateTimeByTimeStamp.Minute.ToString();
        }
        string text6 = string.Empty;
        if (serverDateTimeByTimeStamp.Second < 10)
        {
            text6 = "0" + serverDateTimeByTimeStamp.Second.ToString();
        }
        else
        {
            text6 = serverDateTimeByTimeStamp.Second.ToString();
        }
        return string.Concat(new string[]
        {
            text,
            "-",
            text2,
            "-",
            text3,
            " ",
            text4,
            ":",
            text5,
            ":",
            text6
        });
    }

    public static void ShowMsgBoxDisengageApprentice(string playerName, LuaFunction sureCallback, LuaFunction cancelCallback)
    {
        string s_describle = string.Format(CommonUtil.GetText(dynamic_textid.IDs.apprentice_disengage), playerName);
        ControllerManager.Instance.GetController<MsgBoxController>().ShowMsgBox(MsgBoxController.MsgLevelNormal, s_describle, CommonUtil.GetText(dynamic_textid.BaseIDs.confirm), CommonUtil.GetText(dynamic_textid.BaseIDs.cancel), UIManager.ParentType.CommonUI, delegate ()
        {
            if (sureCallback != null)
            {
                sureCallback.Call();
            }
        }, delegate ()
        {
            if (cancelCallback != null)
            {
                cancelCallback.Call();
            }
        }, null);
    }

    public static List<PropsBase> GetPropsInPackage(int Ptype)
    {
        if (Ptype == 1)
        {
            LuaTable luaTable = (LuaTable)LuaScriptMgr.Instance.CallLuaFunction("BagCtrl.MainPackageDicTable_GetValueList", new object[]
            {
                Util.GetLuaTable("BagCtrl")
            })[0];
            List<PropsBase> list = new List<PropsBase>();
            if (luaTable != null)
            {
                IEnumerator enumerator = luaTable.Values.GetEnumerator();
                enumerator.Reset();
                while (enumerator.MoveNext())
                {
                    object obj = enumerator.Current;
                    PropsBase propsBase = (PropsBase)obj;
                    propsBase.ThisidStr = propsBase._obj.thisid;
                    list.Add(propsBase);
                }
            }
            return list;
        }
        if (Ptype == 2)
        {
            LuaTable luaTable2 = (LuaTable)LuaScriptMgr.Instance.CallLuaFunction("BagCtrl.EquipPackageDicTable_GetValueList", new object[]
            {
                Util.GetLuaTable("BagCtrl")
            })[0];
            List<PropsBase> list2 = new List<PropsBase>();
            if (luaTable2 != null)
            {
                IEnumerator enumerator2 = luaTable2.Values.GetEnumerator();
                enumerator2.Reset();
                while (enumerator2.MoveNext())
                {
                    object obj2 = enumerator2.Current;
                    PropsBase propsBase2 = (PropsBase)obj2;
                    propsBase2.ThisidStr = propsBase2._obj.thisid;
                    list2.Add(propsBase2);
                }
            }
            return list2;
        }
        if (Ptype == 3)
        {
            LuaTable luaTable3 = (LuaTable)LuaScriptMgr.Instance.CallLuaFunction("BagCtrl.TaskPackageDicTable_GetValueList", new object[]
            {
                Util.GetLuaTable("BagCtrl")
            })[0];
            List<PropsBase> list3 = new List<PropsBase>();
            if (luaTable3 != null)
            {
                IEnumerator enumerator3 = luaTable3.Values.GetEnumerator();
                enumerator3.Reset();
                while (enumerator3.MoveNext())
                {
                    object obj3 = enumerator3.Current;
                    PropsBase propsBase3 = (PropsBase)obj3;
                    propsBase3.ThisidStr = propsBase3._obj.thisid;
                    list3.Add(propsBase3);
                }
            }
            return list3;
        }
        return null;
    }

    public static List<PropsBase> GetResourceProps()
    {
        LuaTable luaTable = (LuaTable)LuaScriptMgr.Instance.CallLuaFunction("BagCtrl.ResourcePackageDicTable_GetValueList", new object[]
        {
            Util.GetLuaTable("BagCtrl")
        })[0];
        List<PropsBase> list = new List<PropsBase>();
        if (luaTable != null)
        {
            IEnumerator enumerator = luaTable.Values.GetEnumerator();
            enumerator.Reset();
            while (enumerator.MoveNext())
            {
                object obj = enumerator.Current;
                list.Add((PropsBase)obj);
            }
        }
        return list;
    }

    public static cs_MapUserData GetMainPlayerMapUserData()
    {
        return MainPlayer.Self.OtherPlayerData.MapUserData;
    }

    public static RenderTextureMgr ReturnRenderTexture()
    {
        return ManagerCenter.Instance.GetManager<RenderTextureMgr>();
    }

    public static RawCharactorMgr GetRawCharatorMgr()
    {
        return ManagerCenter.Instance.GetManager<RawCharactorMgr>();
    }

    public static NpctalkRawCharactorMgr GetNpcTalkRawCharatorMgr()
    {
        return ManagerCenter.Instance.GetManager<NpctalkRawCharactorMgr>();
    }

    public static void SwitchHeroCb(LuaTable msg)
    {
        UI_Character uiobject = UIManager.GetUIObject<UI_Character>();
        if (uiobject != null)
        {
            uiobject.SwitchHeroCb(msg);
        }
        else
        {
            ControllerManager.Instance.GetController<CardController>().ReqCardPackInfo();
        }
        HeroHandbookController controller = ControllerManager.Instance.GetController<HeroHandbookController>();
        if (controller != null)
        {
            controller.mCharacterNetwork.ReqGetMainHero();
        }
        UI_MainView uiobject2 = UIManager.GetUIObject<UI_MainView>();
        if (uiobject2 != null)
        {
            uiobject2.SwitchHeroCb();
        }
        MainPlayerSkillHolder.Instance.ResetSkillState();
        MainPlayer.Self.ClearEffect();
    }

    public static void InitFiveanglerObj(GameObject FiveanglerObj, float[] radius)
    {
        CommonTools.CreateSectorByAray(FiveanglerObj, radius);
    }

    public static void DeleteObjMaterialImmediate(Material material)
    {
        if (material != null)
        {
            if (material.mainTexture != null)
            {
                UnityEngine.Object.DestroyImmediate(material.mainTexture, true);
            }
            UnityEngine.Object.DestroyImmediate(material, true);
        }
    }

    public static void UpdateInputNumPos(Transform inputTrans, Transform inputNumberTrans)
    {
        RectTransform rectTransform = inputNumberTrans as RectTransform;
        rectTransform.anchoredPosition = new Vector2(20000f, 20000f);
        Camera component = ManagerCenter.Instance.GetManager<UIManager>().GetUICamera().GetComponent<Camera>();
        Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(component, inputTrans.position);
        RectTransform rectTransform2 = inputTrans as RectTransform;
        Vector2 vector;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(inputNumberTrans.parent as RectTransform, screenPoint, component, out vector);
        int width = Screen.width;
        int height = Screen.height;
        float num = 10f;
        float num2 = 10f;
        float y = 0f;
        float x = 0f;
        float num3 = (float)width / 1334f;
        float num4 = (float)height / 750f;
        if (rectTransform.sizeDelta.x * num3 + num2 < (float)width - screenPoint.x - rectTransform2.sizeDelta.x * num3 / 2f)
        {
            x = vector.x + rectTransform2.sizeDelta.x / 2f + rectTransform.sizeDelta.x / 2f + num2;
            if (rectTransform.sizeDelta.y * num4 / 2f < (float)height - screenPoint.y && rectTransform.sizeDelta.y * num4 / 2f < screenPoint.y)
            {
                y = vector.y;
            }
            else if (rectTransform.sizeDelta.y * num4 / 2f > (float)height - screenPoint.y && rectTransform.sizeDelta.y * num4 / 2f < screenPoint.y)
            {
                num = rectTransform.sizeDelta.y * num4 / 2f - ((float)height - screenPoint.y);
                y = vector.y - num;
            }
            else if (rectTransform.sizeDelta.y * num4 / 2f > screenPoint.y && rectTransform.sizeDelta.y * num4 / 2f < (float)height - screenPoint.y)
            {
                num = rectTransform.sizeDelta.y * num4 / 2f - screenPoint.y;
                y = vector.y + num;
            }
            rectTransform.anchoredPosition = new Vector2(x, y);
            return;
        }
        if (rectTransform.sizeDelta.y * num4 + num < screenPoint.y - rectTransform2.sizeDelta.y * num4 / 2f)
        {
            y = vector.y - rectTransform2.sizeDelta.y / 2f - rectTransform.sizeDelta.y / 2f - num;
        }
        else
        {
            y = vector.y + rectTransform2.sizeDelta.y / 2f + rectTransform.sizeDelta.y / 2f + num;
        }
        if (rectTransform.sizeDelta.x * num3 / 2f < (float)width - screenPoint.x && rectTransform.sizeDelta.x * num3 / 2f < screenPoint.x)
        {
            x = vector.x;
        }
        else if (rectTransform.sizeDelta.x * num3 / 2f > (float)width - screenPoint.x && rectTransform.sizeDelta.x * num3 / 2f < screenPoint.x)
        {
            num2 = rectTransform.sizeDelta.x * num3 / 2f - ((float)width - screenPoint.x);
            x = vector.x - num2;
        }
        else if (rectTransform.sizeDelta.x * num3 / 2f > screenPoint.x && rectTransform.sizeDelta.x * num3 / 2f < (float)width - screenPoint.x)
        {
            num2 = rectTransform.sizeDelta.x * num3 / 2f - screenPoint.x;
            x = vector.x + num2;
        }
        rectTransform.anchoredPosition = new Vector2(x, y);
    }

    public static uint GetCurSubCopyMapID()
    {
        CopyManager manager = ManagerCenter.Instance.GetManager<CopyManager>();
        if (manager != null)
        {
            return manager.MCurrentSubCopyMapId;
        }
        return 0U;
    }

    public static void TryEnterAutoTaskState(uint id, uint state)
    {
    }

    public static void RefreshTaskInfoInMainUI()
    {
    }

    public static void PathFindByIDAndState(uint id, uint state, bool server)
    {
        ControllerManager.Instance.GetController<TaskUIController>().PathFindByIDAndState(id, state, server);
    }

    public static void RefreshPKMode()
    {
        ControllerManager.Instance.GetController<UIHpSystem>().ResetPKModel();
    }

    public static void ReqExecuteQuest(uint id, string target, uint offset, uint questdesccrc)
    {
        ControllerManager.Instance.GetController<TaskController>().taskNetWork.ReqExecuteQuest(id, target, offset, questdesccrc, 0UL, false);
    }

    public static void ReqExecuteQuest2ChangeBuf(uint id, string target, uint offset, uint questdesccrc, ulong chartarget)
    {
        ControllerManager.Instance.GetController<TaskController>().taskNetWork.ReqExecuteQuest(id, target, offset, questdesccrc, chartarget, false);
    }

    public static string ChangeTextModel(string str)
    {
        TextModelController controller = ControllerManager.Instance.GetController<TextModelController>();
        if (controller != null)
        {
            return controller.ChangeTextModel(str);
        }
        return str;
    }

    public static void ShowMsgBoxAbandonQuest(LuaFunction SureCallback, LuaFunction CancelCallback)
    {
        string text = CommonUtil.GetText(dynamic_textid.IDs.task_forgo);
        ControllerManager.Instance.GetController<MsgBoxController>().ShowMsgBox(MsgBoxController.MsgLevelNormal, text, MsgBoxController.MsgOptionConfirm, MsgBoxController.MsgOptionCancel, UIManager.ParentType.Tips, delegate ()
        {
            if (SureCallback != null)
            {
                SureCallback.Call();
            }
        }, delegate ()
        {
            if (CancelCallback != null)
            {
                CancelCallback.Call();
            }
        }, null);
    }

    public static void ShowMsgBoxAllUnload(LuaFunction SureCallback, LuaFunction CancelCallback)
    {
        string text = CommonUtil.GetText(dynamic_textid.IDs.rune_allunload);
        ControllerManager.Instance.GetController<MsgBoxController>().ShowMsgBox(MsgBoxController.MsgLevelNormal, text, MsgBoxController.MsgOptionConfirm, MsgBoxController.MsgOptionCancel, UIManager.ParentType.Tips, delegate ()
        {
            if (SureCallback != null)
            {
                SureCallback.Call();
            }
        }, delegate ()
        {
            if (CancelCallback != null)
            {
                CancelCallback.Call();
            }
        }, null);
    }

    public static void ShowMsgBoxActiveRune(LuaFunction SureCallback, LuaFunction CancelCallback)
    {
        string text = CommonUtil.GetText(dynamic_textid.IDs.rune_activite);
        ControllerManager.Instance.GetController<MsgBoxController>().ShowMsgBox(MsgBoxController.MsgLevelNormal, text, MsgBoxController.MsgOptionConfirm, MsgBoxController.MsgOptionCancel, UIManager.ParentType.Tips, delegate ()
        {
            if (SureCallback != null)
            {
                SureCallback.Call();
            }
        }, delegate ()
        {
            if (CancelCallback != null)
            {
                CancelCallback.Call();
            }
        }, null);
    }

    public static void ShowMsgBoxSelectActivateRune(LuaFunction SureCallback, LuaFunction CancelCallback)
    {
        string text = CommonUtil.GetText(dynamic_textid.IDs.rune_selectactivite);
        ControllerManager.Instance.GetController<MsgBoxController>().ShowMsgBox(MsgBoxController.MsgLevelNormal, text, MsgBoxController.MsgOptionConfirm, MsgBoxController.MsgOptionCancel, UIManager.ParentType.Tips, delegate ()
        {
            if (SureCallback != null)
            {
                SureCallback.Call();
            }
        }, delegate ()
        {
            if (CancelCallback != null)
            {
                CancelCallback.Call();
            }
        }, null);
    }

    public static Color GetModelColor(uint type)
    {
        string text = string.Empty;
        switch (type)
        {
            case 1U:
                text = ControllerManager.Instance.GetController<TextModelController>().GetModelColor("Item1");
                break;
            case 2U:
                text = ControllerManager.Instance.GetController<TextModelController>().GetModelColor("Item2");
                break;
            case 3U:
                text = ControllerManager.Instance.GetController<TextModelController>().GetModelColor("Item3");
                break;
            case 4U:
                text = ControllerManager.Instance.GetController<TextModelController>().GetModelColor("Item4");
                break;
        }
        Color result = Color.white;
        if (string.IsNullOrEmpty(text))
        {
            FFDebug.LogWarning("GlobalRegister", string.Format("GetModelColorStr type must 1-4; type is = {0} ", type));
        }
        else
        {
            result = CommonTools.HexToColor(text);
        }
        return result;
    }

    public static void RefreshTaskState(uint id, uint state)
    {
        TaskInfoRefreshData serverData = default(TaskInfoRefreshData);
        serverData.questid = id;
        serverData.state = state;
        TaskController controller = ControllerManager.Instance.GetController<TaskController>();
        if (controller != null)
        {
            controller.RefreshTaskState(serverData);
        }
    }

    public static void ShowAllNPCInMap()
    {
        ControllerManager.Instance.GetController<TaskController>().ShowAllNpcTaskInMap();
    }

    public static void SetCameraDistance(int nDis)
    {
        CameraDistanceType cameraDistanceType;
        if (0 >= nDis)
        {
            cameraDistanceType = CameraDistanceType.normal;
        }
        else
        {
            cameraDistanceType = CameraDistanceType.far;
        }
        GameSystemSettings.SetCameraDistanceType(cameraDistanceType);
        if (null != CameraController.Self)
        {
            CameraController.Self.setCameraDistance();
        }
    }

    public static void SetCameraTrack(bool bTrack)
    {
        GameSystemSettings.SetCameraTrack(bTrack);
    }

    public static void SetCameraSpeed(int nSpeed)
    {
        CameraSpeedType cameraSpeedType;
        if (0 >= nSpeed)
        {
            cameraSpeedType = CameraSpeedType.slow;
        }
        else if (nSpeed == 1)
        {
            cameraSpeedType = CameraSpeedType.normal;
        }
        else
        {
            cameraSpeedType = CameraSpeedType.fast;
        }
        GameSystemSettings.SetCameraSpeedType(cameraSpeedType);
        if (null != CameraController.Self)
        {
            CameraController.Self.setCameraSpeed();
        }
    }

    public static void SetScreenFloor(bool bActive)
    {
        if (ManagerCenter.Instance.GetManager<GameScene>() != null)
        {
            ManagerCenter.Instance.GetManager<GameScene>().SetActiveFloor(bActive);
        }
    }

    public static void SetScreenShadow(bool bActive)
    {
        if (ManagerCenter.Instance.GetManager<GameScene>() != null)
        {
            ManagerCenter.Instance.GetManager<GameScene>().SetActiveShadow(bActive);
        }
    }

    public static void SetScreenDepthOfField(bool bActive)
    {
        if (ManagerCenter.Instance.GetManager<GameScene>() != null)
        {
            ManagerCenter.Instance.GetManager<GameScene>().SetActiveDepthOfField(bActive);
        }
    }

    public static void SetScreenAntialiasing(bool bActive)
    {
        if (ManagerCenter.Instance.GetManager<GameScene>() != null)
        {
            ManagerCenter.Instance.GetManager<GameScene>().SetActiveAntialiasing(bActive);
        }
    }

    public static void SetScreenBloomOptimized(bool bActive)
    {
        if (ManagerCenter.Instance.GetManager<GameScene>() != null)
        {
            ManagerCenter.Instance.GetManager<GameScene>().SetActiveBloomOptimized(bActive);
        }
    }

    public static void SetLoadLowPriorityObject(bool bLoad)
    {
        GameSystemSettings.SetLoadLowPriorityObject(bLoad);
    }

    public static void SetScreenResolution(int nLevel)
    {
        if (null != CameraController.Self)
        {
            CameraController.Self.SetResolution(nLevel);
        }
    }

    public static void SetScreenResolution(int height, int width, bool isFull)
    {
        ResolutionManager.Instance.SetResolution(width, height, (!isFull) ? 2 : 1);
    }

    public static void LogoutGame()
    {
        ManagerCenter.Instance.GetManager<GameMainManager>().Logout(false);
    }

    public static string GetServerName()
    {
        string strB = UserInfoStorage.StorageInfo.LastServer.ToString();
        if (LaunchHelp.ServerList != null && LaunchHelp.ServerList.Length != 0)
        {
            for (int i = 0; i < LaunchHelp.ServerList.Length; i++)
            {
                string[] array = LaunchHelp.ServerList[i].Split(new char[]
                {
                    ','
                });
                if (array != null && array.Length == 2 && array[0].CompareTo(strB) == 0)
                {
                    return array[1];
                }
            }
        }
        return null;
    }

    public static void SetTeamIconInfo(string id, float x, float y, int index)
    {
        UIMapController controller = ControllerManager.Instance.GetController<UIMapController>();
        EntitiesID eid = new EntitiesID(ulong.Parse(id), CharactorType.Player);
        if (eid.Equals(MainPlayer.Self.EID))
        {
            return;
        }
        Vector2 serverPos = new Vector2(x, y);
        controller.SetTeamIconInfo(eid, serverPos, index);
    }

    public static void RetNewApply(int newApplyCount)
    {
        MainUIController MainController = ControllerManager.Instance.GetController<MainUIController>();
        if (newApplyCount > 0)
        {
            MainController.AddMessage(MessageType.Team, newApplyCount, delegate
            {
                LuaScriptMgr.Instance.CallLuaFunction("TeamCtrl.RetApplyList", new object[]
                {
                    Util.GetLuaTable("TeamCtrl")
                });
                MainController.ReadMessage(MessageType.Team);
            });
        }
    }

    public static void TeamActivityChanged(uint actid)
    {
        TeamController controller = ControllerManager.Instance.GetController<TeamController>();
        if (controller != null)
        {
            controller.UpdateTeamActivity(actid);
        }
    }

    public static bool IsMainPlayerTeamLeader()
    {
        TeamController controller = ControllerManager.Instance.GetController<TeamController>();
        return controller != null && controller.IsMainPlayerLeader();
    }

    public static bool IsMainPlayerHasTeam()
    {
        TeamController controller = ControllerManager.Instance.GetController<TeamController>();
        return controller != null && controller.IsMainPlayerHasTeam();
    }

    public static uint GetMyTeamMemCount()
    {
        TeamController controller = ControllerManager.Instance.GetController<TeamController>();
        if (controller != null)
        {
            return (uint)controller.myTeamInfo.mem.Count;
        }
        return 0U;
    }

    public static uint GetMyTeamMemMaxCount()
    {
        TeamController controller = ControllerManager.Instance.GetController<TeamController>();
        if (controller != null)
        {
            return controller.myTeamInfo.maxmember;
        }
        return 0U;
    }

    public static bool CheckIfTeamFull()
    {
        TeamController controller = ControllerManager.Instance.GetController<TeamController>();
        return controller != null && controller.CheckIfTeamFull();
    }

    public static string GetTeamMemberName(string strID)
    {
        TeamController controller = ControllerManager.Instance.GetController<TeamController>();
        if (controller != null)
        {
            for (int i = 0; i < controller.myTeamInfo.mem.Count; i++)
            {
                Memember memember = controller.myTeamInfo.mem[i];
                if (memember.mememberid.CompareTo(strID) == 0)
                {
                    return memember.name;
                }
            }
        }
        return string.Empty;
    }

    public static bool ShowBoxWhenEnterAdventure()
    {
        TeamController controller = ControllerManager.Instance.GetController<TeamController>();
        return controller != null && controller.myTeamInfo != null && controller.myTeamInfo.id != 0U && !controller.myTeamInfo.leaderid.Equals(MainPlayer.Self.OtherPlayerData.MapUserData.charid.ToString());
    }

    public static void ShowMsgBoxWhenEnterAdventure(LuaFunction sureclick, LuaFunction cancelclick)
    {
        ControllerManager.Instance.GetController<MsgBoxController>().ShowMsgBox(MsgBoxController.MsgLevelNormal, CommonUtil.GetText(dynamic_textid.IDs.enter_adventure), CommonUtil.GetText(dynamic_textid.BaseIDs.confirm), CommonUtil.GetText(dynamic_textid.BaseIDs.cancel), UIManager.ParentType.CommonUI, delegate ()
        {
            if (sureclick != null)
            {
                sureclick.Call();
            }
        }, delegate ()
        {
            if (cancelclick != null)
            {
                cancelclick.Call();
            }
        }, null);
    }

    public static void SetIsReqNearBy(bool isNearBy)
    {
        TeamController controller = ControllerManager.Instance.GetController<TeamController>();
        if (controller != null && controller.uiTeam != null)
        {
            controller.uiTeam.IsReqNearBy = isNearBy;
        }
    }

    public static void RefreshOccupyNpcShow()
    {
        ControllerManager.Instance.GetController<OccupyController>().RefreshOccupyNpcShow();
    }

    public static string GetMainPlayerIdStr()
    {
        if (MainPlayer.Self != null)
        {
            return MainPlayer.Self.EID.IdStr;
        }
        return string.Empty;
    }

    public static void CheckCloseAllTeam()
    {
        TeamController controller = ControllerManager.Instance.GetController<TeamController>();
        if (controller != null && controller.uiTeam != null)
        {
            controller.uiTeam.CheckCloseAllTeam();
        }
    }

    public static void PathFindWithPathWayId(uint pathwayid)
    {
        ControllerManager.Instance.GetController<TaskUIController>().FindPathInterface(pathwayid, null);
    }

    public static uint GetCopyInfiniteNum()
    {
        return 9999U;
    }

    public static string GetTeamLeaderID()
    {
        string result = string.Empty;
        TeamController controller = ControllerManager.Instance.GetController<TeamController>();
        if (controller != null)
        {
            Memember teamLaederInfo = controller.GetTeamLaederInfo();
            result = teamLaederInfo.mememberid;
        }
        return result;
    }

    public static string GetOpenChatType()
    {
        return CommonTools.GetOpenChatType();
    }

    public static void SaveOpenChatType(string str)
    {
        CommonTools.SaveOpenChatType(str);
    }

    public static void SetCurOpenChatType(string str)
    {
        MainUIController controller = ControllerManager.Instance.GetController<MainUIController>();
        if (controller != null)
        {
            controller.SetCurOpenChatType(str);
        }
    }

    public static void AddPrivateChatData(LuaTable lt)
    {
    }

    public static void OnReceiveFriendChatData(LuaTable lt)
    {
    }

    public static void OnReceiveChatData(LuaTable lt)
    {
    }

    private static ChatData CoverToChatData(LuaTable lt)
    {
        ChatData chatData = new ChatData();
        chatData.channel = lt.GetField_Uint("channel");
        chatData.charid = ulong.Parse(lt.GetField_String("charid"));
        chatData.charname = lt.GetField_String("charname");
        chatData.charcountry = lt.GetField_Uint("charcountry");
        chatData.chattime = lt.GetField_Uint("chattime");
        chatData.content = lt.GetField_String("content");
        chatData.show_type = lt.GetField_Uint("show_type");
        chatData.tocharid = ulong.Parse(lt.GetField_String("tocharid"));
        LuaTable field_Table = lt.GetField_Table("link");
        IEnumerator enumerator = field_Table.Values.GetEnumerator();
        enumerator.Reset();
        while (enumerator.MoveNext())
        {
            object obj = enumerator.Current;
            LuaTable luaTable = obj as LuaTable;
            ChatLink chatLink = new ChatLink();
            chatLink.linktype = luaTable.GetField_Uint("linktype");
            LuaTable field_Table2 = luaTable.GetField_Table("data_args");
            IEnumerator enumerator2 = field_Table2.Values.GetEnumerator();
            enumerator2.Reset();
            while (enumerator2.MoveNext())
            {
                object obj2 = enumerator2.Current;
                string item = obj2.ToString();
                chatLink.data_args.Add(item);
            }
            chatData.link.Add(chatLink);
        }
        return chatData;
    }

    public static void RegistGMInputField(InputField input)
    {
        LocalGMController controller = ControllerManager.Instance.GetController<LocalGMController>();
        if (controller != null)
        {
            controller.RegistInputField(input);
        }
    }

    public static void RemoveGMInputField()
    {
        LocalGMController controller = ControllerManager.Instance.GetController<LocalGMController>();
        if (controller != null)
        {
            controller.RemoveInputField();
        }
    }

    public static bool IsInCompleteCopy()
    {
        bool result = false;
        CopyManager manager = ManagerCenter.Instance.GetManager<CopyManager>();
        if (manager != null && manager.InCompetitionCopy)
        {
            result = true;
        }
        return result;
    }

    public static void ShowLittleChat()
    {
    }

    public static void ShowMsgBoxByChat(string content)
    {
        ControllerManager.Instance.GetController<MsgBoxController>().ShowMsgBox(MsgBoxController.MsgLevelNormal, content, MsgBoxController.MsgOptionConfirm, UIManager.ParentType.CommonUI, null, null);
    }

    public static void ShowNotice(LuaTable lt)
    {
        TipsWindow.ShowNotice(new NoticeModel
        {
            content = lt.GetField_String("content"),
            channeltype = (ChannelType)lt.GetField_Int("channeltype")
        });
    }

    public static string RemoveTextModel(string str)
    {
        TextModelController controller = ControllerManager.Instance.GetController<TextModelController>();
        if (controller != null)
        {
            return controller.RemoveTextModel(str);
        }
        return str;
    }

    public static void ShowWindow(LuaTable lt)
    {
        NoticeModel noticeModel = new NoticeModel();
        noticeModel.content = lt.GetField_String("content");
        noticeModel.texEffectColor = Color.gray;
        string field_String = lt.GetField_String("colorname");
        if (field_String.Contains("Red"))
        {
            noticeModel.texEffectColor = Const.TextColorTipsRed;
        }
        else if (field_String.Contains("Green"))
        {
            noticeModel.texEffectColor = Const.TextColorTipsGreen;
        }
        TipsWindow.ShowNotice(noticeModel);
    }

    public static void AddChatItem(LuaTable lt)
    {
        ChatData data = GlobalRegister.CoverToChatData(lt);
        ControllerManager.Instance.GetController<MainUIController>().AddChatItem(data);
    }

    public static void AddChatItem_Tips(LuaTable lt)
    {
        ChatData data = GlobalRegister.CoverToChatData(lt);
        ControllerManager.Instance.GetController<MainUIController>().AddChatItem_Tips(data);
    }

    public static uint GetCampNeutralID()
    {
        return Const.CampNeutralID;
    }

    public static string GetModelColorStr(string name)
    {
        return ControllerManager.Instance.GetController<TextModelController>().GetModelColor(name);
    }

    public static string ProcessGMWithSpace(string content)
    {
        LocalGMController controller = ControllerManager.Instance.GetController<LocalGMController>();
        if (controller != null)
        {
            content = controller.ProcessGMWithSpace(content);
        }
        return content;
    }

    public static bool IsLocalGmText(string content)
    {
        bool result = false;
        LocalGMController controller = ControllerManager.Instance.GetController<LocalGMController>();
        if (controller != null)
        {
            result = controller.IsLocalGmText(content);
            controller.AddCachedCommand(content);
        }
        return result;
    }

    public static void ShowMsgBoxCreateTeam(LuaFunction SureCallback)
    {
        ControllerManager.Instance.GetController<MsgBoxController>().ShowMsgBox(MsgBoxController.MsgLevelNormal, CommonUtil.GetText(dynamic_textid.IDs.team_create), MsgBoxController.MsgOptionCreate, MsgBoxController.MsgOptionCancel, UIManager.ParentType.Tips, delegate ()
        {
            if (SureCallback != null)
            {
                SureCallback.Call();
            }
        }, delegate ()
        {
        }, null);
    }

    public static string AddTextModel(string content, string model)
    {
        TextModelController controller = ControllerManager.Instance.GetController<TextModelController>();
        if (controller != null)
        {
            return controller.AddTextModel(content, model);
        }
        return content;
    }

    public static void ForceRebuild(RectTransform content)
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate(content);
    }

    public static RichText NewRichText(LuaTable lt, string prefix, GameObject underLine, float maxWidth)
    {
        ChatData data = GlobalRegister.CoverToChatData(lt);
        return new RichText(data, prefix, underLine, maxWidth);
    }

    public static bool IsteamMember()
    {
        TeamController controller = ControllerManager.Instance.GetController<TeamController>();
        return controller.myTeamInfo != null && controller.myTeamInfo.id != 0U && !controller.myTeamInfo.leaderid.Equals(ManagerCenter.Instance.GetManager<EntitiesManager>().MainPlayer.GetCharID().ToString());
    }

    public static bool IsActiveAutoButton()
    {
        return false;
    }

    public static void SetAutoAttack(bool isAutoBattle)
    {
        AutoAttack component = MainPlayer.Self.GetComponent<AutoAttack>();
        if (component != null)
        {
            component.SwitchModle(isAutoBattle);
        }
    }

    public static void SetAttactFollowTeamLeader(bool isAutoBattle)
    {
        AttactFollowTeamLeader component = MainPlayer.Self.GetComponent<AttactFollowTeamLeader>();
        if (component != null)
        {
            component.SwitchModle(isAutoBattle);
            return;
        }
        EntitiesManager manager = ManagerCenter.Instance.GetManager<EntitiesManager>();
        if (manager != null)
        {
            manager.DoEentityActionOrCache(MainPlayer.Self.EID, delegate (CharactorBase basePlayer)
            {
                AttactFollowTeamLeader component2 = MainPlayer.Self.GetComponent<AttactFollowTeamLeader>();
                if (component2 != null)
                {
                    component2.SwitchModle(isAutoBattle);
                }
                else
                {
                    FFDebug.LogWarning("GR", "MainPlayer's TeamLeader is null");
                }
            });
        }
    }

    public static Vector2 GetUIWorldToScreenPoint(Transform tran)
    {
        Vector2 result = Vector2.zero;
        UIManager manager = ManagerCenter.Instance.GetManager<UIManager>();
        if (manager != null)
        {
            result = RectTransformUtility.WorldToScreenPoint(manager.GetUICamera().GetComponent<Camera>(), tran.position);
        }
        return result;
    }

    public static Vector2 GetUIScreenPointToLocalPointInRectangle(Transform tran, Vector2 v2ScreenPoint)
    {
        Vector2 zero = Vector2.zero;
        UIManager manager = ManagerCenter.Instance.GetManager<UIManager>();
        if (manager != null)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(tran.parent as RectTransform, v2ScreenPoint, manager.GetUICamera().GetComponent<Camera>(), out zero);
        }
        return zero;
    }

    public static uint GetPlayerHeroID(ulong ulCharID)
    {
        HeroHandbookController controller = ControllerManager.Instance.GetController<HeroHandbookController>();
        int mainHeroId = controller.GetMainHeroId();
        uint heroid = MainPlayer.Self.OtherPlayerData.MapUserData.mapshow.heroid;
        uint heroid2 = MainPlayer.Self.OtherPlayerData.MapUserData.bakmapshow.heroid;
        uint result = 0U;
        if (mainHeroId == 0)
        {
            result = heroid;
        }
        else
        {
            if ((long)mainHeroId == (long)((ulong)heroid))
            {
                result = heroid2;
            }
            if ((long)mainHeroId != (long)((ulong)heroid))
            {
                result = heroid;
            }
        }
        return result;
    }

    public static uint GetPlayerTeamID(ulong ulCharID)
    {
        EntitiesManager manager = ManagerCenter.Instance.GetManager<EntitiesManager>();
        uint result = 0U;
        if (manager != null)
        {
            OtherPlayer playerByID = manager.GetPlayerByID(ulCharID);
            if (playerByID != null && playerByID.OtherPlayerData.MapUserData != null && playerByID.OtherPlayerData.MapUserData.mapdata != null)
            {
                result = playerByID.OtherPlayerData.MapUserData.mapdata.teamid;
            }
        }
        return result;
    }

    public static ulong GetPlayerGuildID(ulong ulCharID)
    {
        EntitiesManager manager = ManagerCenter.Instance.GetManager<EntitiesManager>();
        ulong result = 0UL;
        if (manager != null)
        {
            OtherPlayer playerByID = manager.GetPlayerByID(ulCharID);
            if (playerByID != null)
            {
                cs_MapUserData mapUserData = playerByID.OtherPlayerData.MapUserData;
                if (mapUserData != null && mapUserData.mapdata != null)
                {
                    result = mapUserData.mapdata.guildid;
                }
            }
        }
        return result;
    }

    public static ulong GetMainPlayerGuildID()
    {
        ulong result = 0UL;
        if (MainPlayer.Self != null)
        {
            cs_MapUserData mapUserData = MainPlayer.Self.OtherPlayerData.MapUserData;
            if (mapUserData != null && mapUserData.mapdata != null)
            {
                result = mapUserData.mapdata.guildid;
            }
        }
        return result;
    }

    public static string GetPlayerGuildName(ulong ulCharID)
    {
        EntitiesManager manager = ManagerCenter.Instance.GetManager<EntitiesManager>();
        string result = string.Empty;
        if (manager != null)
        {
            OtherPlayer playerByID = manager.GetPlayerByID(ulCharID);
            if (playerByID != null && playerByID.OtherPlayerData.MapUserData != null && playerByID.OtherPlayerData.MapUserData.mapdata != null)
            {
                result = playerByID.OtherPlayerData.MapUserData.mapdata.guildname;
            }
        }
        return result;
    }

    public static void ProcessLuaCallback(string callbackinfo)
    {
        if (string.IsNullOrEmpty(callbackinfo))
        {
            return;
        }
        if (callbackinfo.StartsWith("this."))
        {
            LuaScriptMgr.Instance.CallLuaFunction(callbackinfo, new object[0]);
        }
        else
        {
            LuaProcess.ProcessLua2CsFunction(callbackinfo);
        }
    }

    public static void ResetNPCDir()
    {
        if (MainPlayer.Self == null)
        {
            return;
        }
        FFDetectionNpcControl component = MainPlayer.Self.GetComponent<FFDetectionNpcControl>();
        if (component == null)
        {
            return;
        }
        component.TryResetNPCDir();
    }

    public static string GetNpcTalkName()
    {
        string result = string.Empty;
        FFDetectionNpcControl component = MainPlayer.Self.GetComponent<FFDetectionNpcControl>();
        ulong num = 0UL;
        if (component != null)
        {
            num = component.CurrentVisteNpcID;
        }
        if (num != 0UL)
        {
            Npc npc = ManagerCenter.Instance.GetManager<EntitiesManager>().GetCharactorByID(num, CharactorType.NPC) as Npc;
            if (npc != null)
            {
                result = npc.NpcData.MapNpcData.name;
            }
        }
        return result;
    }

    public static GameObject GetTalkNpcGameObject()
    {
        FFDetectionNpcControl component = MainPlayer.Self.GetComponent<FFDetectionNpcControl>();
        ulong currentVisteNpcID = component.CurrentVisteNpcID;
        if (currentVisteNpcID != 0UL)
        {
            Npc npc = ManagerCenter.Instance.GetManager<EntitiesManager>().GetCharactorByID(currentVisteNpcID, CharactorType.NPC) as Npc;
            if (npc != null)
            {
                return npc.ModelObj;
            }
        }
        return null;
    }

    public static void ViewMask(float time)
    {
        ControllerManager.Instance.GetController<ImgMaskController>().ViewMask(time);
    }

    public static void ProcessNpcdlgBreakAutoattack(bool state)
    {
        BreakAutoattackUIMgr component = MainPlayer.Self.GetComponent<BreakAutoattackUIMgr>();
        if (component != null)
        {
            if (state)
            {
                component.ProcessOpenedUI("UI_NPCtalk", true);
            }
            else
            {
                component.ProcessDeletedUI("UI_NPCtalk");
            }
        }
    }

    public static Sprite TextureasseetToSprite(UITextureAsset textureAsset)
    {
        if (textureAsset == null)
        {
            return null;
        }
        return Sprite.Create(textureAsset.textureObj, new Rect(0f, 0f, (float)textureAsset.textureObj.width, (float)textureAsset.textureObj.height), new Vector2(0f, 0f));
    }

    public static void PlayDramaActionShake(Transform tran, uint action)
    {
        SingletonForMono<DramaAction>.Instance.PlayAction(tran, (DramaAction.ActionType)action);
    }

    public static bool CheckIfInTeam(string memid)
    {
        return ControllerManager.Instance.GetController<TeamController>().CheckIfInTeam(memid);
    }

    public static void ShowMsgBoxCommon(string tmpStr, LuaFunction SureCallback, LuaFunction cancelCallback)
    {
        ControllerManager.Instance.GetController<MsgBoxController>().ShowMsgBox(MsgBoxController.MsgLevelNormal, tmpStr, CommonUtil.GetText(dynamic_textid.BaseIDs.confirm), CommonUtil.GetText(dynamic_textid.BaseIDs.cancel), UIManager.ParentType.CommonUI, delegate ()
        {
            if (SureCallback != null)
            {
                SureCallback.Call();
            }
        }, delegate ()
        {
            if (cancelCallback != null)
            {
                cancelCallback.Call();
            }
        }, null);
    }

    public static uint GetCurrentCopyID()
    {
        CopyManager manager = ManagerCenter.Instance.GetManager<CopyManager>();
        if (manager != null)
        {
            return manager.MCurrentCopyID;
        }
        return 0U;
    }

    public static Vector2 GetClientDirVector2ByServerDir(int serverDir)
    {
        return CommonTools.GetClientDirVector2ByServerDir(serverDir);
    }

    public static Vector2 GetServerPosByWorldPos(Vector3 worldPos)
    {
        return GraphUtils.GetServerPosByWorldPos(worldPos, true);
    }

    public static Vector3 GetWorldPosByServerPos(Vector2 serverPos)
    {
        return GraphUtils.GetWorldPosByServerPos(serverPos);
    }

    public static uint GetServerDirByClientDir(Vector2 clientDir)
    {
        return CommonTools.GetServerDirByClientDir(clientDir);
    }

    public static uint GetCurrencyByID(uint id)
    {
        if (MainPlayer.Self == null)
        {
            return 0U;
        }
        return MainPlayer.Self.GetCurrencyByID(id);
    }

    public static uint GetBluePointToday()
    {
        if (MainPlayer.Self == null || MainPlayer.Self.MainPlayeData.CharacterBaseData == null)
        {
            return 0U;
        }
        return MainPlayer.Self.MainPlayeData.CharacterBaseData.bluecrystalincnum;
    }

    public static Transform GetMainCameraTrans()
    {
        if (Camera.main != null)
        {
            return Camera.main.transform;
        }
        return null;
    }

    public static Vector3 GetMainCameraAngle()
    {
        if (Camera.main != null)
        {
            return Camera.main.transform.localEulerAngles;
        }
        return Vector3.zero;
    }

    public static Transform GetMainPlayerTrans()
    {
        if (MainPlayer.Self != null)
        {
            return MainPlayer.Self.ModelObj.transform;
        }
        return null;
    }

    public static int GetMapID()
    {
        if (ManagerCenter.Instance.GetManager<GameScene>().CurrentSceneData == null)
        {
            return -1;
        }
        return (int)ManagerCenter.Instance.GetManager<GameScene>().CurrentSceneData.mapID();
    }

    public static void BeginFindPath(float x, float y, LuaFunction callback)
    {
        PathFindComponent component = MainPlayer.Self.GetComponent<PathFindComponent>();
        component.PathFindOfDeviation(new Vector2(x, y), delegate
        {
            if (callback != null)
            {
                callback.Call();
            }
        });
    }

    public static void ReSetAndPlayTweens(GameObject go)
    {
        UITweener[] componentsInChildren = go.GetComponentsInChildren<UITweener>(false);
        for (int i = 0; i < componentsInChildren.Length; i++)
        {
            componentsInChildren[i].Reset();
            componentsInChildren[i].Play(true);
        }
    }

    public static void RefreshCopyTask()
    {
        if (MainPlayer.Self == null)
        {
            return;
        }
        if (MainPlayer.Self.GetComponent<FindDirections>() == null)
        {
            return;
        }
        MainPlayer.Self.GetComponent<FindDirections>().RefreshCopyTask();
    }

    public static void ShowMsgBoxUseVitPill(int maxNum, LuaFunction sureCallback, LuaFunction cancelCallback)
    {
        string s_describle = string.Format(CommonUtil.GetText(dynamic_textid.IDs.vit_max_limit), maxNum);
        ControllerManager.Instance.GetController<MsgBoxController>().ShowMsgBox(MsgBoxController.MsgLevelNormal, s_describle, CommonUtil.GetText(dynamic_textid.BaseIDs.contiune_use), CommonUtil.GetText(dynamic_textid.BaseIDs.cancel), UIManager.ParentType.CommonUI, delegate ()
        {
            if (sureCallback != null)
            {
                sureCallback.Call();
            }
        }, null, null);
    }

    public static void ShowAddNewItem(PropsBase pb)
    {
        ShortCutUseEquipController controller = ControllerManager.Instance.GetController<ShortCutUseEquipController>();
        if (controller != null)
        {
            controller.ShowAddNewItem(pb);
        }
    }

    public static void RefreshBagItem()
    {
        uint currencyByID = GlobalRegister.GetCurrencyByID(3U);
        MainUIController controller = ControllerManager.Instance.GetController<MainUIController>();
        controller.CheckSkillNewIconShowByMoney(currencyByID);
        HeroAwakeController controller2 = ControllerManager.Instance.GetController<HeroAwakeController>();
        if (controller2 != null && null != controller2.mUICharacter)
        {
            controller2.mUICharacter.RefreshAwakeInfo();
        }
    }

    public static void RefreshTreasureHuntInfo(PropsBase pb)
    {
        ShortCutUseEquipController controller = ControllerManager.Instance.GetController<ShortCutUseEquipController>();
        if (controller != null)
        {
            controller.RefreshTreasureHunt(pb);
        }
    }

    public static void CloseTreasureHunt(string thisid)
    {
        ShortCutUseEquipController controller = ControllerManager.Instance.GetController<ShortCutUseEquipController>();
        if (controller != null)
        {
            controller.CloseTreasureHunt(thisid);
        }
    }

    public static void SetMapNameAction(string name, string name_en, string icon)
    {
        MainUIController controller = ControllerManager.Instance.GetController<MainUIController>();
        if (controller != null)
        {
            controller.SetMapNameAction(name, name_en, icon);
        }
    }

    public static void SetStoryNameAction(string id)
    {
        MainUIController controller = ControllerManager.Instance.GetController<MainUIController>();
        if (controller != null)
        {
            controller.SetStoryNameAction(id);
        }
    }

    public static void SetImage(int name, string icon, Image imgicon, bool needShow = true)
    {
        ManagerCenter.Instance.GetManager<UITextureMgr>().GetTexture((ImageType)name, icon, delegate (UITextureAsset asset)
        {
            if (asset == null)
            {
                FFDebug.LogWarning("CommonItem", "  req  texture   is  null ");
                return;
            }
            if (imgicon == null)
            {
                return;
            }
            Texture2D textureObj = asset.textureObj;
            Sprite sprite = Sprite.Create(textureObj, new Rect(0f, 0f, (float)textureObj.width, (float)textureObj.height), new Vector2(0f, 0f));
            imgicon.sprite = sprite;
            imgicon.overrideSprite = sprite;
            imgicon.material = null;
            imgicon.color = Color.white;
            if (needShow)
            {
                imgicon.gameObject.SetActive(true);
            }
        });
    }

    public static void GetSpriteFromAtlas(int name, string icon, Image img)
    {
        if (string.IsNullOrEmpty(icon))
        {
            FFDebug.LogWarning("GR", "Icon name is null!" + icon);
            img.overrideSprite = null;
            return;
        }
        ManagerCenter.Instance.GetManager<UITextureMgr>().GetSpriteFromAtlas(((AtlasName)name).ToString(), icon, delegate (Sprite sprite)
        {
            if (img == null)
            {
                return;
            }
            if (null != sprite)
            {
                img.sprite = sprite;
                img.overrideSprite = sprite;
            }
            else
            {
                img.overrideSprite = null;
            }
        });
    }

    public static void GetSpriteFromAtlas(string icon, Image img)
    {
        ManagerCenter.Instance.GetManager<UITextureMgr>().GetSpriteFromAtlas("base", icon, delegate (Sprite sprite)
        {
            if (img == null)
            {
                return;
            }
            img.overrideSprite = sprite;
        });
    }

    public static void GetCanvasRenderer(TweenAlpha ta)
    {
        ta.GetComponent<CanvasRenderer>().SetAlpha(0f);
    }

    public static void OnTaskStateChange()
    {
        ManagerCenter.Instance.GetManager<EntitiesManager>().RefreshNPCShowState();
    }

    public static void SetImageBlur(Image img)
    {
        ManagerCenter.Instance.GetManager<ScreenShotManager>().FillImageWithScreenShotBlur(img);
    }

    public static void SetRawImageBlur(RawImage rimg)
    {
        ManagerCenter.Instance.GetManager<ScreenShotManager>().FillRawImageWithScreenShotBlur(rimg);
    }

    public static void ActiveAllPublicCD()
    {
        MainPlayer.Self.GetComponent<SkillPublicCDControl>().ActiveAllPublicCD();
    }

    public static string GetCurrentBattleHeroThisid()
    {
        return MainPlayer.Self.MainPlayeData.CharacterBaseData.herothisid;
    }

    public static bool IsPointerEnterUI(Vector2 mousepos, Transform uiroot)
    {
        bool result = false;
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        pointerEventData.position = mousepos;
        EventSystem.current.RaycastAll(pointerEventData, GlobalRegister.overuiresults);
        if (GlobalRegister.overuiresults.Count > 0 && UITools.IsChild(uiroot, GlobalRegister.overuiresults[0].gameObject.transform))
        {
            result = true;
        }
        return result;
    }

    public static void SetUIInfoPos(Transform btntrans, Transform infotrans)
    {
        if (infotrans == null || btntrans == null)
        {
            FFDebug.LogWarning("SetUIInfoPos", "infotrans == null || btntrans == null");
            return;
        }
        Camera component = ManagerCenter.Instance.GetManager<UIManager>().GetUICamera().GetComponent<Camera>();
        RectTransform rectTransform = infotrans as RectTransform;
        Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(component, btntrans.position);
        Vector2 vector;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(infotrans.parent as RectTransform, screenPoint, component, out vector);
        float x = rectTransform.sizeDelta.x;
        float y = rectTransform.sizeDelta.y;
        float x2;
        float y2;
        if (btntrans.Find("img_bg") != null)
        {
            RectTransform rectTransform2 = btntrans.Find("img_bg") as RectTransform;
            x2 = rectTransform2.sizeDelta.x;
            y2 = rectTransform2.sizeDelta.y;
        }
        else if (btntrans.Find("btn_icon") != null)
        {
            RectTransform rectTransform3 = btntrans.Find("btn_icon") as RectTransform;
            x2 = rectTransform3.sizeDelta.x;
            y2 = rectTransform3.sizeDelta.y;
        }
        else
        {
            RectTransform rectTransform4 = btntrans as RectTransform;
            x2 = rectTransform4.sizeDelta.x;
            y2 = rectTransform4.sizeDelta.y;
        }
        if (x < (float)Screen.width - screenPoint.x - x2 / 2f && y < (float)Screen.height - screenPoint.y - y2 / 2f)
        {
            rectTransform.anchoredPosition = new Vector2(vector.x + x / 2f + x2 / 2f, vector.y + y / 2f + y2 / 2f);
            return;
        }
        if (x < (float)Screen.width - screenPoint.x - x2 / 2f && y < screenPoint.y - y2 / 2f)
        {
            rectTransform.anchoredPosition = new Vector2(vector.x + x / 2f + x2 / 2f, vector.y - y / 2f - y2 / 2f);
            return;
        }
        if (x < screenPoint.x - x2 / 2f && y < (float)Screen.height - screenPoint.y - y2 / 2f)
        {
            rectTransform.anchoredPosition = new Vector2(vector.x - x / 2f - x2 / 2f, vector.y + y / 2f + y2 / 2f);
            return;
        }
        if (x < screenPoint.x - x2 / 2f && y < screenPoint.y - y2 / 2f)
        {
            rectTransform.anchoredPosition = new Vector2(vector.x - x / 2f - x2 / 2f, vector.y - y / 2f - y2 / 2f);
            return;
        }
        if (x < (float)Screen.width - screenPoint.x - x2 / 2f && y > (float)Screen.height - screenPoint.y - y2 / 2f && y > screenPoint.y - y2 / 2f)
        {
            rectTransform.anchoredPosition = new Vector2(vector.x + x / 2f + x2 / 2f, vector.y);
            return;
        }
        if (x < screenPoint.x - x2 / 2f && y > (float)Screen.height - screenPoint.y - y2 / 2f && y > screenPoint.y - y2 / 2f)
        {
            rectTransform.anchoredPosition = new Vector2(vector.x - rectTransform.sizeDelta.x / 2f - x2 / 2f, vector.y);
            return;
        }
        rectTransform.anchoredPosition = Vector2.zero;
    }

    public static void ShowMsgBoxHeroUpgradeBind(uint sourceBind, uint costBind, LuaFunction sureCallback, LuaFunction cancelCallback)
    {
        string s_describle = string.Empty;
        if (sourceBind != 0U && costBind == 0U)
        {
            s_describle = CommonUtil.GetText(dynamic_textid.IDs.source_hero_bind);
        }
        else if (sourceBind == 0U && costBind != 0U)
        {
            s_describle = CommonUtil.GetText(dynamic_textid.IDs.cost_hero_bind);
        }
        else if (sourceBind != 0U && costBind != 0U)
        {
            s_describle = CommonUtil.GetText(dynamic_textid.IDs.both_heros_bind);
        }
        ControllerManager.Instance.GetController<MsgBoxController>().ShowMsgBox(MsgBoxController.MsgLevelNormal, s_describle, CommonUtil.GetText(dynamic_textid.BaseIDs.confirm), CommonUtil.GetText(dynamic_textid.BaseIDs.cancel), UIManager.ParentType.CommonUI, delegate ()
        {
            if (sureCallback != null)
            {
                sureCallback.Call();
            }
        }, delegate ()
        {
            if (sureCallback != null)
            {
                cancelCallback.Call();
            }
        }, null);
    }

    public static void SetHeroSimpleAttrPos(Transform btntrans, Transform infotrans)
    {
        if (infotrans == null || btntrans == null)
        {
            FFDebug.LogWarning("SetHeroSimpleAttrPos", "infotrans == null || btntrans == null");
            return;
        }
        Camera component = ManagerCenter.Instance.GetManager<UIManager>().GetUICamera().GetComponent<Camera>();
        RectTransform rectTransform = infotrans as RectTransform;
        Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(component, btntrans.position);
        Vector2 vector;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(infotrans.parent as RectTransform, screenPoint, component, out vector);
        float x = rectTransform.sizeDelta.x;
        float y = rectTransform.sizeDelta.y;
        float x2;
        if (btntrans.Find("img_bg") != null)
        {
            RectTransform rectTransform2 = btntrans.Find("img_bg") as RectTransform;
            x2 = rectTransform2.sizeDelta.x;
            float y2 = rectTransform2.sizeDelta.y;
        }
        else if (btntrans.Find("btn_icon") != null)
        {
            RectTransform rectTransform3 = btntrans.Find("btn_icon") as RectTransform;
            x2 = rectTransform3.sizeDelta.x;
            float y2 = rectTransform3.sizeDelta.y;
        }
        else
        {
            RectTransform rectTransform4 = btntrans as RectTransform;
            x2 = rectTransform4.sizeDelta.x;
            float y2 = rectTransform4.sizeDelta.y;
        }
        if (x < (float)Screen.width - screenPoint.x - x2 / 2f)
        {
            rectTransform.anchoredPosition = new Vector2(vector.x + x / 2f + x2 / 2f, 0f);
            return;
        }
        rectTransform.anchoredPosition = new Vector2(vector.x - x / 2f - x2 / 2f, 0f);
    }

    public static void RefrashShortcutItemCount()
    {
        if (GlobalRegister.usc == null)
        {
            GlobalRegister.usc = UnityEngine.Object.FindObjectOfType<UI_ShortcutControl>();
        }
        if (GlobalRegister.usc != null)
        {
            GlobalRegister.usc.RefrashItemCount();
        }
        ControllerManager.Instance.GetController<CardController>().ReqBagFullState();
        ControllerManager.Instance.GetController<AutoFightController>().InitMaxRecoveryPropData();
    }

    public static void LogMessage(string content, int type)
    {
        switch (type)
        {
            case 1:
                UnityEngine.Debug.Log(content);
                break;
            case 2:
                UnityEngine.Debug.LogWarning(content);
                break;
            case 3:
                UnityEngine.Debug.LogError(content);
                break;
        }
    }

    public static void LogMessage(string content)
    {
        GlobalRegister.LogMessage(content, 3);
    }

    public static void SetShortcutKeyEnableState(bool state)
    {
        ShortcutsConfigController controller = ControllerManager.Instance.GetController<ShortcutsConfigController>();
        if (controller != null)
        {
            controller.SetShortcutKeyEnableState(state, true);
        }
    }

    public static string ConfigColorToRichTextFormat(string orignalInput)
    {
        string pattern = "[[][0-9|a-f]{6}[]]";
        string pattern2 = "[[][-][]]";
        Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
        MatchCollection matchCollection = regex.Matches(orignalInput);
        foreach (object obj in matchCollection)
        {
            Match match = (Match)obj;
            string text = match.Value.Replace("[", string.Empty).Replace("]", string.Empty);
            text = "<color=#" + text + ">";
            orignalInput = orignalInput.Replace(match.Value, text);
        }
        orignalInput = Regex.Replace(orignalInput, pattern2, "</color>");
        return orignalInput;
    }

    public static string StripColorText(string orignalInput)
    {
        string pattern = "[#][\\S|\\s]{6}[>]";
        string pattern2 = "</color>";
        Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
        MatchCollection matchCollection = regex.Matches(orignalInput);
        foreach (object obj in matchCollection)
        {
            Match match = (Match)obj;
            orignalInput = orignalInput.Replace(match.Value, string.Empty);
        }
        orignalInput = Regex.Replace(orignalInput, pattern2, string.Empty);
        orignalInput = Regex.Replace(orignalInput, "<color=", string.Empty);
        return orignalInput;
    }

    public static WayFindItem GetWayFindItemByQuestID(LuaTable questCfg, string degreeVar)
    {
        if (questCfg != null)
        {
            string field_String = questCfg.GetField_String("degree");
            string value = string.Empty;
            if (!string.IsNullOrEmpty(field_String))
            {
                string[] array = field_String.Split(new string[]
                {
                    ";"
                }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < array.Length; i++)
                {
                    if (array[i].Contains(degreeVar))
                    {
                        string[] array2 = array[i].Split(new string[]
                        {
                            ":"
                        }, StringSplitOptions.RemoveEmptyEntries);
                        if (array2.Length > 1)
                        {
                            value = array2[1];
                        }
                    }
                }
            }
            uint[] array3 = null;
            string[] array4 = questCfg.GetField_String("pathwaydoing").Split(new string[]
            {
                ";"
            }, StringSplitOptions.RemoveEmptyEntries);
            for (int j = 0; j < array4.Length; j++)
            {
                string[] array5 = array4[j].Split(new string[]
                {
                    "-"
                }, StringSplitOptions.RemoveEmptyEntries);
                array3 = new uint[array5.Length - 1];
                if (array4.Length == 1)
                {
                    for (j = 1; j < array5.Length; j++)
                    {
                        array3[j - 1] = uint.Parse(array5[j]);
                    }
                    break;
                }
                if (array5.Length > 1 && array4[j].StartsWith(value))
                {
                    for (j = 1; j < array5.Length; j++)
                    {
                        array3[j - 1] = uint.Parse(array5[j]);
                    }
                    break;
                }
            }
            string field_String2 = questCfg.GetField_String("degreename");
            if (!string.IsNullOrEmpty(field_String2))
            {
                string[] array6 = field_String2.Split(new string[]
                {
                    ";"
                }, StringSplitOptions.RemoveEmptyEntries);
                for (int k = 0; k < array6.Length; k++)
                {
                    if (array6[k].EndsWith(value))
                    {
                        return new WayFindItem(array6[k])
                        {
                            pathWayIds = array3
                        };
                    }
                }
            }
        }
        return null;
    }

    public static List<string> GetTaskGoalStr(uint id)
    {
        LuaTable configTable = LuaConfigManager.GetConfigTable("questconfig", (ulong)id);
        string cacheField_String = configTable.GetCacheField_String("degreename");
        string cacheField_String2 = configTable.GetCacheField_String("degree");
        List<string> list = new List<string>();
        if (configTable != null && !string.IsNullOrEmpty(cacheField_String) && !string.IsNullOrEmpty(cacheField_String2))
        {
            string[] array = cacheField_String.Split(new char[]
            {
                ';'
            });
            string[] array2 = cacheField_String2.Split(new char[]
            {
                ';'
            });
            string pattern = "[-][0-9]{1,}[:][0-9]{1,}";
            foreach (string txt in array)
            {
                if (!string.IsNullOrEmpty(txt))
                {
                    Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
                    MatchCollection matchCollection = regex.Matches(txt);
                    IEnumerator enumerator = matchCollection.GetEnumerator();
                    if (enumerator.MoveNext())
                    {
                        Match match = (Match)enumerator.Current;
                        var text = txt.Replace(match.Value, string.Empty);
                        text = text.Replace("{", string.Empty);
                        text = text.Replace("}", string.Empty);
                        text = GlobalRegister.ConfigColorToRichTextFormat(text);
                        list.Add(text);
                    }
                }
            }
        }
        return list;
    }

    public static WayFindItem GetWayFindItemByStr(string content)
    {
        return new WayFindItem(content);
    }

    public static void VisitLastVisitNpc()
    {
        ulong lId = GlobalRegister.GetLastVisitNpcTmpId();
        TaskController tc = ControllerManager.Instance.GetController<TaskController>();
        if (lId != 0UL && tc != null)
        {
            Scheduler.Instance.AddTimer(0.05f, false, delegate
            {
                tc.ReqVisteNpc(lId);
            });
        }
    }

    public static void OnCurActiveQuestFrash(LuaTable lt)
    {
        UI_TaskList uiobject = UIManager.GetUIObject<UI_TaskList>();
        if (uiobject != null)
        {
            uiobject.OnGetActiveQuestData(lt);
        }
    }

    public static ulong GetLastVisitNpcTmpId()
    {
        TaskController controller = ControllerManager.Instance.GetController<TaskController>();
        if (controller != null && controller.lastVisitNpcId != 0UL)
        {
            return controller.lastVisitNpcId;
        }
        return 0UL;
    }

    public static ulong GetLastVisitNpcId()
    {
        TaskController controller = ControllerManager.Instance.GetController<TaskController>();
        if (controller != null && controller.lastVisitNpcId != 0UL)
        {
            EntitiesManager manager = ManagerCenter.Instance.GetManager<EntitiesManager>();
            if (manager != null)
            {
                Npc npc = manager.GetNpc(controller.lastVisitNpcId);
                if (npc != null)
                {
                    return (ulong)npc.NpcData.MapNpcData.baseid;
                }
            }
        }
        return 0UL;
    }

    public static void Clear3DIconData()
    {
    }

    public static void ShowMainPlayerRTT(RawImage ri, uint heroId)
    {
        cs_CharacterMapShow cs_CharacterMapShow;
        if (MainPlayer.Self.OtherPlayerData.MapUserData.bakmapshow.heroid == heroId)
        {
            cs_CharacterMapShow = MainPlayer.Self.OtherPlayerData.MapUserData.bakmapshow;
        }
        else
        {
            cs_CharacterMapShow = MainPlayer.Self.OtherPlayerData.MapUserData.mapshow;
        }
        uint[] featureIDs = new uint[]
        {
            cs_CharacterMapShow.haircolor,
            cs_CharacterMapShow.hairstyle,
            cs_CharacterMapShow.facestyle,
            cs_CharacterMapShow.antenna,
            cs_CharacterMapShow.avatarid
        };
        GlobalRegister.ShowPlayerRTT(ri, heroId, cs_CharacterMapShow.bodystyle, featureIDs, 0, null);
    }

    public static void ShowNpcOrPlayerRTT(RawImage ri, uint heroId, int uitype, Action<GameObject> back = null)
    {
        if (ri == null)
        {
            return;
        }
        ri.color = new Color(0f, 0f, 0f, 0f);
        LuaTable configTable = LuaConfigManager.GetConfigTable("heros", (ulong)heroId);
        Vector3 npcLocalPos = Vector3.zero;
        Vector3 npcLocalRot = Vector3.zero;
        float[] resolution = new float[]
        {
            ri.rectTransform.sizeDelta.x,
            ri.rectTransform.sizeDelta.y
        };
        CommonTools.Get3DIconPosAndRot(heroId, out npcLocalPos, out npcLocalRot, resolution, (GlobalRegister.ModelIconUIType)uitype);
        if (configTable != null)
        {
            ri.transform.name = heroId.ToString();
            FFCharacterModelHold.CreatePlayer(heroId, delegate (PlayerCharactorCreateHelper hold)
            {
                if (ri == null)
                {
                    return;
                }
                if (ri.transform.name.CompareTo(heroId.ToString()) == 0)
                {
                    GlobalRegister.SetRTTInfo(hold, ri, hold.rootObj, npcLocalPos, npcLocalRot, false);
                    ManagerCenter.Instance.GetManager<GameScene>().SetMatLightInfo(hold.rootObj, true);
                    if (back != null)
                    {
                        back(hold.rootObj);
                    }
                }
                else
                {
                    hold.DisposeBonePObj();
                }
            });
        }
        else
        {
            uint[] featureIDs = null;
            ri.transform.name = heroId.ToString();
            FFCharacterModelHold.CreateModel(heroId, 0U, featureIDs, string.Empty, delegate (PlayerCharactorCreateHelper hold)
            {
                if (ri.transform.name.CompareTo(heroId.ToString()) == 0)
                {
                    GlobalRegister.SetRTTInfo(hold, ri, hold.rootObj, npcLocalPos, npcLocalRot, false);
                    ManagerCenter.Instance.GetManager<GameScene>().SetMatLightInfo(hold.rootObj, true);
                    if (back != null)
                    {
                        back(hold.rootObj);
                    }
                }
                else
                {
                    hold.DisposeBonePObj();
                }
            }, null, 0UL);
        }
    }

    public static void OpenMailUI()
    {
        MailControl controller = ControllerManager.Instance.GetController<MailControl>();
        if (controller != null)
        {
            controller.ShowMailUI();
        }
    }

    public static void UpdateBubbleOfMail(bool open)
    {
        MailControl controller = ControllerManager.Instance.GetController<MailControl>();
        if (controller != null)
        {
            controller.UpdateBubbleOfMail(open);
        }
    }

    public static void ShowPlayerRTT(RawImage ri, uint heroId, uint bodyid, uint[] featureIDs, int uiType, Action<GameObject> back = null)
    {
        if (null == ri || null == ri.rectTransform)
        {
            return;
        }
        ri.color = new Color(0f, 0f, 0f, 0f);
        LuaTable cacheField_Table = LuaConfigManager.GetConfig("npc_data").GetCacheField_Table(heroId);
        if (cacheField_Table == null)
        {
            return;
        }
        string field_String = cacheField_Table.GetField_String("animatorcontroller");
        Vector3 npcLocalPos = Vector3.zero;
        Vector3 npcLocalRot = Vector3.zero;
        float[] resolution = new float[]
        {
            ri.rectTransform.sizeDelta.x,
            ri.rectTransform.sizeDelta.y
        };
        CommonTools.Get3DIconPosAndRot(heroId, out npcLocalPos, out npcLocalRot, resolution, (GlobalRegister.ModelIconUIType)uiType);
        ri.transform.name = heroId.ToString() + "|" + bodyid.ToString();
        FFCharacterModelHold.CreateModel(heroId, bodyid, featureIDs, field_String, delegate (PlayerCharactorCreateHelper o)
        {
            if (ri == null)
            {
                return;
            }
            if (ri.transform.name.CompareTo(heroId.ToString() + "|" + bodyid.ToString()) == 0)
            {
                GlobalRegister.SetRTTInfo(o, ri, o.rootObj, npcLocalPos, npcLocalRot, false);
                ManagerCenter.Instance.GetManager<GameScene>().SetMatLightInfo(o.rootObj, true);
                if (back != null)
                {
                    back(o.rootObj);
                }
            }
            else
            {
                o.DisposeBonePObj();
            }
        }, null, 0UL);
    }

    public static GameObject rtObjRoot
    {
        get
        {
            if (GlobalRegister.rtObjRoot_ == null)
            {
                GlobalRegister.rtObjRoot_ = new GameObject("_IconObjectRoot");
                GlobalRegister.rtObjRoot_.transform.position = new Vector3(0f, -200f, 0f);
                UnityEngine.Object.DontDestroyOnLoad(GlobalRegister.rtObjRoot_);
                GlobalRegister.nextIconObjPos = GlobalRegister.rtObjRoot_.transform.position;
                Light light = GlobalRegister.rtObjRoot_.gameObject.AddComponent<Light>();
                light.type = LightType.Directional;
                light.intensity = 1f;
                light.cullingMask = 1 << Const.Layer.RT;
            }
            return GlobalRegister.rtObjRoot_;
        }
    }

    public static void SetRTTInfo(PlayerCharactorCreateHelper modelHelpHolder, RawImage ri, GameObject target, Vector3 targetLocalPos, Vector3 localRot, bool isNeedDestoryOrignalTargetOnDestroy = false)
    {
        if (null == target)
        {
            return;
        }
        Animator component = target.GetComponent<Animator>();
        if (component != null)
        {
            component.cullingMode = AnimatorCullingMode.AlwaysAnimate;
        }
        try
        {
            if (ri != null)
            {
                ri.color = Color.white;
                if (GlobalRegister.camFor3dIcon == null)
                {
                    GlobalRegister.camFor3dIcon = new GameObject("Cam_For3d_Icon").AddComponent<Camera>();
                    UnityEngine.Object.DontDestroyOnLoad(GlobalRegister.camFor3dIcon.gameObject);
                    GlobalRegister.camFor3dIcon.enabled = false;
                    int cullingMask = 1 << LayerMask.NameToLayer("RT");
                    GlobalRegister.camFor3dIcon.cullingMask = cullingMask;
                    GlobalRegister.camFor3dIcon.nearClipPlane = 0.01f;
                    GlobalRegister.camFor3dIcon.farClipPlane = 5f;
                    GlobalRegister.camFor3dIcon.useOcclusionCulling = false;
                }
                GlobalRegister.camFor3dIcon.transform.position = Vector3.one * 100f;
                IconRenderCtrl iconRenderCtrl = ri.gameObject.GetComponent<IconRenderCtrl>();
                if (iconRenderCtrl == null)
                {
                    iconRenderCtrl = ri.gameObject.AddComponent<IconRenderCtrl>();
                }
                else
                {
                    iconRenderCtrl.CheckInLstControl();
                }
                if (iconRenderCtrl.ModelHelpHolder != null)
                {
                    iconRenderCtrl.ModelHelpHolder.DisposeBonePObj();
                }
                iconRenderCtrl.ModelHelpHolder = modelHelpHolder;
                RenderTexture renderTexture = null;
                CommonTools.SetGameObjectLayer(target, "RT", true);
                Renderer[] componentsInChildren = target.GetComponentsInChildren<Renderer>(true);
                for (int i = 0; i < componentsInChildren.Length; i++)
                {
                    for (int j = 0; j < componentsInChildren[i].materials.Length; j++)
                    {
                        componentsInChildren[i].materials[j].SetFloat("_RJ", 0f);
                    }
                }
                if (iconRenderCtrl.rt != null)
                {
                    if (renderTexture != null)
                    {
                        RenderTexture.ReleaseTemporary(renderTexture);
                    }
                    renderTexture = iconRenderCtrl.rt;
                }
                else
                {
                    renderTexture = RenderTexture.GetTemporary((int)ri.rectTransform.sizeDelta.x, (int)ri.rectTransform.sizeDelta.y, 24, RenderTextureFormat.Default, RenderTextureReadWrite.Default, 8);
                    iconRenderCtrl.rt = renderTexture;
                }
                iconRenderCtrl.target = target;
                iconRenderCtrl.cameraRT = GlobalRegister.camFor3dIcon;
                Color black = Color.black;
                black.a = 0f;
                GlobalRegister.camFor3dIcon.clearFlags = CameraClearFlags.Color;
                GlobalRegister.camFor3dIcon.backgroundColor = black;
                target.transform.SetParent(GlobalRegister.rtObjRoot.transform, true);
                iconRenderCtrl.localPos = targetLocalPos;
                iconRenderCtrl.localRot = localRot;
                iconRenderCtrl.camWorldPos = GlobalRegister.nextIconObjPos;
                target.transform.position = GlobalRegister.nextIconObjPos;
                target.transform.localRotation = Quaternion.identity;
                Vector3 position = target.transform.TransformPoint(targetLocalPos);
                target.transform.localScale = Vector3.one;
                target.transform.position = position;
                GlobalRegister.nextIconObjPos += Vector3.right * 5f;
                target.transform.localRotation = Quaternion.Euler(localRot);
                GlobalRegister.camFor3dIcon.targetTexture = renderTexture;
                ri.texture = renderTexture;
                iconRenderCtrl.ReStart();
            }
            else
            {
                FFDebug.LogError("SetRTTInfo", "The Target Texture Has Destroyed!!!");
                modelHelpHolder.DisposeBonePObj();
            }
        }
        catch (Exception ex)
        {
            FFDebug.LogError("SetRTTInfo", ex.ToString());
            modelHelpHolder.DisposeBonePObj();
        }
    }

    public static void RemoveEffectLayerObject(GameObject obj)
    {
        int childCount = obj.transform.childCount;
        for (int i = childCount - 1; i >= 0; i--)
        {
            GameObject gameObject = obj.transform.GetChild(i).gameObject;
            if (gameObject.layer == LayerMask.NameToLayer("Effect"))
            {
                UnityEngine.Object.DestroyObject(gameObject);
            }
            else
            {
                GlobalRegister.RemoveEffectLayerObject(gameObject);
            }
        }
    }

    public static void OpenGeneUI(int index)
    {
        GameScene manager = ManagerCenter.Instance.GetManager<GameScene>();
        if (manager.isAbattoirScene)
        {
            TipsWindow.ShowWindow(5001U);
            return;
        }
        UI_Gene uiobject = UIManager.GetUIObject<UI_Gene>();
        GeneController controller = ControllerManager.Instance.GetController<GeneController>();
        GeneOperationType wantToOpen = (GeneOperationType)index;
        Action open = delegate ()
        {
            if (wantToOpen == GeneOperationType.Fuse || wantToOpen == GeneOperationType.Insert)
            {
                ManagerCenter.Instance.GetManager<EscManager>().ProgressSameInLogic("UI_Gene", 2);
            }
            UIManager.Instance.ShowUI<UI_Gene>("UI_Gene", delegate ()
            {
                UIManager.GetUIObject<UI_Gene>().SetGot(index);
            }, UIManager.ParentType.CommonUI, false);
        };
        if (uiobject == null)
        {
            open();
        }
        else
        {
            UIManager.Instance.DeleteUI<UI_Gene>();
            if (uiobject.curGot != wantToOpen)
            {
                UIManager.Instance.DeleteUI<UI_Gene>();
                Scheduler.Instance.AddFrame(20U, false, delegate
                {
                    open();
                });
            }
        }
    }

    public static void OpenUnLockSkillsUI()
    {
        UnLockSkillsController controller = ControllerManager.Instance.GetController<UnLockSkillsController>();
        if (controller != null)
        {
            controller.OpenFrame(UnLockSkillsController.ManualType.Self);
        }
    }

    public static void OnRetLevelupHeroSkill(string herothisid, uint skillbaseid, uint skilllevel, uint skillorgid)
    {
        UnLockSkillsController controller = ControllerManager.Instance.GetController<UnLockSkillsController>();
        if (controller != null)
        {
            controller._network.OnRetMSG_LevelupHeroSkill_SC(herothisid, skillbaseid, skilllevel, skillorgid);
        }
        MainUIController controller2 = ControllerManager.Instance.GetController<MainUIController>();
        controller2.CheckSkillNewIconShow();
    }

    public static void DelayCloseUI(float time, string uiname)
    {
        Scheduler.Instance.AddFrame(1U, false, delegate
        {
            LuaPanelBase luaUIPanel = UIManager.GetLuaUIPanel(uiname);
            if (luaUIPanel != null)
            {
                luaUIPanel.uiRoot.gameObject.SetActive(false);
            }
        });
    }

    public static void ItemSplitNumAddBtnDownUpListen(GameObject obj)
    {
        UIEventListener.Get(obj).onDown = new UIEventListener.VoidDelegate(GlobalRegister.ItemSplitNumAddBtnDown);
        UIEventListener.Get(obj).onUp = new UIEventListener.VoidDelegate(GlobalRegister.ItemSplitNumAddBtnUp);
    }

    private static void ItemSplitNumAddBtnDown(PointerEventData eventData)
    {
        Scheduler.Instance.AddTimer(GlobalRegister.mAddMinusSplitTimer, true, new Scheduler.OnScheduler(GlobalRegister.DoSplitNumAdd));
    }

    private static void ItemSplitNumAddBtnUp(PointerEventData eventData)
    {
        Scheduler.Instance.RemoveTimer(new Scheduler.OnScheduler(GlobalRegister.DoSplitNumAdd));
    }

    private static void DoSplitNumAdd()
    {
        LuaScriptMgr.Instance.CallLuaFunction("BagCtrl.SplitNumAddMinus", new object[]
        {
            true
        });
    }

    public static void ItemSplitNumMinusBtnDownUpListen(GameObject obj)
    {
        UIEventListener.Get(obj).onDown = new UIEventListener.VoidDelegate(GlobalRegister.ItemSplitNumMinusBtnDown);
        UIEventListener.Get(obj).onUp = new UIEventListener.VoidDelegate(GlobalRegister.ItemSplitNumMinusBtnUp);
    }

    private static void ItemSplitNumMinusBtnDown(PointerEventData eventData)
    {
        Scheduler.Instance.AddTimer(GlobalRegister.mAddMinusSplitTimer, true, new Scheduler.OnScheduler(GlobalRegister.DoSplitNumMinus));
    }

    private static void ItemSplitNumMinusBtnUp(PointerEventData eventData)
    {
        Scheduler.Instance.RemoveTimer(new Scheduler.OnScheduler(GlobalRegister.DoSplitNumMinus));
    }

    private static void DoSplitNumMinus()
    {
        LuaScriptMgr.Instance.CallLuaFunction("BagCtrl.SplitNumAddMinus", new object[]
        {
            false
        });
    }

    public static string GetMainPlayerCharID()
    {
        string result = string.Empty;
        if (MainPlayer.Self != null)
        {
            result = MainPlayer.Self.OtherPlayerData.MapUserData.charid.ToString();
        }
        return result;
    }

    public static string GetCurrentPriChatFileName()
    {
        return "/pchatdata_" + UserInfoStorage.StorageInfo.LastServer.ToString() + "_" + UserInfoStorage.StorageInfo.Uid.ToString();
    }

    public static void SaveChatDataAsFile(LuaStringBuffer data)
    {
        try
        {
            string currentPriChatFileName = GlobalRegister.GetCurrentPriChatFileName();
            string path = string.Empty;
            path = Application.dataPath + "/StreamingAssets/Datas/" + currentPriChatFileName;
            Stream stream = File.Create(path);
            stream.Write(data.buffer, 0, data.buffer.Length);
            stream.Close();
        }
        catch (Exception ex)
        {
            FFDebug.LogError("SaveDataAsFile", ex.Message);
        }
    }

    public static void ProcessChatTime(Text text, int timestamp1)
    {
        DateTime currServerDateTime = SingletonForMono<GameTime>.Instance.GetCurrServerDateTime();
        DateTime serverDateTimeByTimeStamp = SingletonForMono<GameTime>.Instance.GetServerDateTimeByTimeStamp((ulong)((long)timestamp1 * 1000L));
        TimeSpan timeSpan = currServerDateTime - serverDateTimeByTimeStamp;
        UIInformationList component = text.GetComponent<UIInformationList>();
        string text2 = string.Empty;
        if (timeSpan.Hours > 24)
        {
            text2 = ControllerManager.Instance.GetController<TextModelController>().ChangeTextModel(component.listInformation[2].content);
            text2 = string.Format(text2, CommonTools.ParseIntToDoubleString(serverDateTimeByTimeStamp.Month), CommonTools.ParseIntToDoubleString(serverDateTimeByTimeStamp.Day), CommonTools.ParseIntToDoubleString(serverDateTimeByTimeStamp.Hour) + ":" + CommonTools.ParseIntToDoubleString(serverDateTimeByTimeStamp.Minute));
        }
        else if ((serverDateTimeByTimeStamp + new TimeSpan(1, 0, 0, 0)).Date == currServerDateTime.Date)
        {
            text2 = ControllerManager.Instance.GetController<TextModelController>().ChangeTextModel(component.listInformation[1].content);
            text2 = string.Format(text2, CommonTools.ParseIntToDoubleString(serverDateTimeByTimeStamp.Hour) + ":" + CommonTools.ParseIntToDoubleString(serverDateTimeByTimeStamp.Minute));
        }
        else
        {
            text2 = ControllerManager.Instance.GetController<TextModelController>().ChangeTextModel(component.listInformation[0].content);
            text2 = string.Format(text2, CommonTools.ParseIntToDoubleString(serverDateTimeByTimeStamp.Hour) + ":" + CommonTools.ParseIntToDoubleString(serverDateTimeByTimeStamp.Minute));
        }
        text.text = text2;
    }

    public static void ProcessMailRemainTime(string sdeltime, Text txttime)
    {
        long num = 0L;
        long.TryParse(sdeltime, out num);
        long currServerUlongTimeBySecond = (long)SingletonForMono<GameTime>.Instance.GetCurrServerUlongTimeBySecond();
        long num2 = (num - currServerUlongTimeBySecond) / 3600L;
        long num3 = (num - currServerUlongTimeBySecond) / 86400L;
        if (num3 > 0L)
        {
            ControllerManager.Instance.GetController<TextModelController>().SetTextModel(txttime, num3.ToString(), 0);
        }
        else
        {
            num2 = ((num2 > 0L) ? num2 : 1L);
            ControllerManager.Instance.GetController<TextModelController>().SetTextModel(txttime, num2.ToString(), 1);
        }
    }

    public static PropsBase GetPropsBaseByID(uint baseID)
    {
        try
        {
            return new PropsBase(baseID, 1U);
        }
        catch (Exception arg)
        {
            FFDebug.LogWarning("GlobalRegister", "GetPropsBaseByID Exception error: " + arg);
        }
        return null;
    }

    public static int CompareLong(string sl1, string sl2)
    {
        long num = 0L;
        long.TryParse(sl1, out num);
        long num2 = 0L;
        long.TryParse(sl2, out num2);
        int result = 0;
        if (num > num2)
        {
            result = 1;
        }
        else if (num < num2)
        {
            result = -1;
        }
        return result;
    }

    public static ulong GetSelectTargetID()
    {
        MainPlayerTargetSelectMgr component = MainPlayer.Self.GetComponent<MainPlayerTargetSelectMgr>();
        if (component.manualSelect.Target != null && component.manualSelect.Target.BaseData != null)
        {
            return component.manualSelect.Target.BaseData.BaseData.id;
        }
        return 0UL;
    }

    public static bool IsBitTrue(uint relive_type, int index)
    {
        return ((ulong)relive_type & (ulong)(1L << (index & 31))) != 0UL;
    }

    public static void MainUserDeathOrRelive(bool isDead)
    {
        GlobalRegister.isMainUserDeathing = isDead;
        UIBagManager.Instance.ShowDeadMask(GlobalRegister.isMainUserDeathing);
        if (!isDead)
        {
            ControllerManager.Instance.GetController<MainUIController>().ShowBattleLog("你重新站起来了", true);
        }
        if (UIManager.GetUIObject<UI_MainView>() != null && UIManager.GetUIObject<UI_MainView>().Root != null)
        {
            UI_ShortcutControl component = UIManager.GetUIObject<UI_MainView>().Root.GetComponent<UI_ShortcutControl>();
            for (int i = 0; i < component.mAllDragDropButtonList.Count; i++)
            {
                component.mAllDragDropButtonList[i].SetMaskShow(GlobalRegister.isMainUserDeathing);
            }
        }
    }

    public static void InitTaskTrackConcurrentItem(GameObject itemRoot, uint questID, int finishState, string degreeVar, uint curDegree, uint maxDegree)
    {
        TaskUIController controller = ControllerManager.Instance.GetController<TaskUIController>();
        controller.CacheTaskTrackItem(itemRoot, questID, finishState, degreeVar, curDegree, maxDegree);
    }

    public static void ClearSelectNpcState()
    {
        MainPlayerTargetSelectMgr component = MainPlayer.Self.GetComponent<MainPlayerTargetSelectMgr>();
        if (component != null)
        {
            component.SetTargetNull();
            component.manualSelect.lastSelectNpcPos = Vector2.zero;
        }
    }

    public static void PlayProgressBarAndAnimationInLocal(string param)
    {
        ProgressUIController controller = ControllerManager.Instance.GetController<ProgressUIController>();
        string[] datas = param.Split(new char[]
        {
            ','
        });
        FFBehaviourControl behaveiorCtrl = MainPlayer.Self.GetComponent<FFBehaviourControl>();
        UI_ProgressBar uiobject = UIManager.GetUIObject<UI_ProgressBar>();
        if (uiobject != null)
        {
            controller.BreakProgressBar();
            behaveiorCtrl.PlayNormalAction(1U, false, 0.1f);
            return;
        }
        controller.StrInfo = datas[2];
        uint actionid = uint.Parse(datas[0]);
        uint actionIDEnd = uint.Parse(datas[1]);
        float durationtime = behaveiorCtrl.PlayNormalAction(actionid, false, 0.1f);
        controller.ShowProgressBar(durationtime, delegate ()
        {
            if (datas.Length <= 3)
            {
                return;
            }
            string text = string.Empty;
            for (int i = 3; i < datas.Length; i++)
            {
                text += datas[i];
                if (i < datas.Length - 1)
                {
                    text += ",";
                }
            }
            LuaProcess.ProcessLua2CsFunction(text);
            float time = behaveiorCtrl.PlayNormalAction(actionIDEnd, false, 0.1f);
            Scheduler.Instance.AddTimer(time, false, delegate
            {
                behaveiorCtrl.PlayNormalAction(1U, false, 0.1f);
            });
        });
    }

    public static void OpenShopUI(uint param)
    {
        ControllerManager.Instance.GetController<ShopController>().OnOpenShop(param);
    }

    public static void OpenExchange(uint param)
    {
        if (param == 2U)
        {
            UI_Exchange uiobject = UIManager.GetUIObject<UI_Exchange>();
            UIManager.Instance.ShowUI<UI_Exchange>("UI_Exchange", delegate ()
            {
            }, UIManager.ParentType.CommonUI, false);
        }
        else if (param == 1U)
        {
            UIManager.Instance.ShowUI<UI_ExchangeGem>("UI_Exchange", delegate ()
            {
            }, UIManager.ParentType.CommonUI, false);
        }
        LuaScriptMgr.Instance.CallLuaFunction("BagCtrl.CloseBagUI", new object[]
        {
            Util.GetLuaTable("BagCtrl")
        });
    }

    public static void UIPostEvent(string eventName)
    {
    }

    public static void AddUISoundListener(GameObject obj, string eventName)
    {
    }

    public static void AddItemTip(GameObject obj, GameObject imgIcon, int baseId)
    {
        if (!obj)
        {
            return;
        }
        if (!imgIcon)
        {
            return;
        }
        Graphic component = obj.GetComponent<Graphic>();
        if (component)
        {
            component.raycastTarget = true;
        }
        HoverEventListener.Get(obj).onEnter = delegate (PointerEventData ed)
        {
            ControllerManager.Instance.GetController<ItemTipController>().OpenPanel(new t_Object
            {
                baseid = (uint)baseId
            }, imgIcon);
        };
        HoverEventListener.Get(obj).onExit = delegate (PointerEventData ed)
        {
            ControllerManager.Instance.GetController<ItemTipController>().ClosePanel();
        };
    }

    public static void CheckTalkCloseGuideNext()
    {
        ControllerManager.Instance.GetController<GuideController>().CheckNpcGuide((uint)MainPlayer.Self.GetComponent<FFDetectionNpcControl>().CurrentVisteNpcID);
    }

    public static void RefreshShortcutCD(string cddata)
    {
        UI_ShortcutControl component = UIManager.GetUIObject<UI_MainView>().Root.GetComponent<UI_ShortcutControl>();
        string[] array = cddata.Split(new string[]
        {
            ","
        }, StringSplitOptions.RemoveEmptyEntries);
        Dictionary<uint, float> dictionary = new Dictionary<uint, float>();
        for (int i = 0; i < array.Length; i++)
        {
            string[] array2 = array[i].Split(new string[]
            {
                "_"
            }, StringSplitOptions.RemoveEmptyEntries);
            try
            {
                dictionary.Add(uint.Parse(array2[0]), uint.Parse(array2[1]) - SingletonForMono<GameTime>.Instance.GetCurrServerTimeBySecond());
            }
            catch
            {
            }
        }
        component.RefreshCDPanel(dictionary);
    }

    public static void RefreshBagNewTip(bool hasNew)
    {
        UI_MainView mainView = ControllerManager.Instance.GetController<MainUIController>().mainView;
        if (mainView != null)
        {
            mainView.RefreshBagNewTip(hasNew);
        }
    }

    private static int _sampleDepth;

    private static readonly Dictionary<int, string> _showNames = new Dictionary<int, string>();

    private static List<RaycastResult> overuiresults = new List<RaycastResult>();

    private static UI_ShortcutControl usc;

    private static Camera camFor3dIcon = null;

    private static GameObject rtObjRoot_;

    private static Vector3 nextIconObjPos;

    private static float mAddMinusSplitTimer = 0.2f;

    public static bool isMainUserDeathing = false;

    public enum ModelIconUIType
    {
        Head3D,
        Charactor,
        Guide,
        Herohandbook
    }
}
