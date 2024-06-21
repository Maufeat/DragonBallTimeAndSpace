using System;
using System.Collections.Generic;
using System.Xml;
using battle;
using Chat;
using Framework.Managers;
using Game.Scene;
using LuaInterface;
using mobapk;
using msg;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Map : UIPanelBase
{
    private UIMapController MapController
    {
        get
        {
            return ControllerManager.Instance.GetController<UIMapController>();
        }
    }

    public bool IsAreamapShow
    {
        get
        {
            return !(this.Panel_Large == null) && this.Panel_Large.gameObject.activeSelf && this.Areamap.gameObject.activeSelf;
        }
    }

    public override void OnInit(Transform root)
    {
        base.OnInit(root);
        this.Root = root;
        this.transBattlePanel = this.Root.Find("Offset_Map/Panel_battlewar");
        this.txtBattleTimeCooldown = this.transBattlePanel.Find("txt_time").GetComponent<Text>();
        this.txtScore1 = this.transBattlePanel.Find("img_side1/txt_num1").GetComponent<Text>();
        this.txtScore2 = this.transBattlePanel.Find("img_side2/txt_num1").GetComponent<Text>();
        this.txtScore1.text = "0";
        this.txtScore2.text = "0";
        this.objBall1 = this.transBattlePanel.Find("img_side1/Image").gameObject;
        this.objBall2 = this.transBattlePanel.Find("img_side2/Image").gameObject;
        this.objBall1.SetActive(false);
        this.objBall2.SetActive(false);
        this.Panel_Large = this.Root.Find("Offset_Map/Panel_Large");
        this.Panel_MiniMap = this.Root.Find("Offset_Map/Panel_MiniMap");
        this.PanelMap = this.Root.Find("Offset_Map/Panel_MiniMap/Panel_main/PanelMap");
        this.Areamap = this.Panel_Large.Find("areamap");
        this.uiWidth = (this.Areamap.Find("map").transform as RectTransform).sizeDelta.x;
        this.PanelMapImage = this.PanelMap.Find("img_outline/map/map_bg").GetComponent<RawImage>();
        this.AreamapImage = this.Areamap.Find("map/map_bg").GetComponent<RawImage>();
        this.CnMapName = this.Areamap.Find("title/txt_title").GetComponent<Text>();
        this.EnMapName = this.Areamap.Find("title/txt_title_en").GetComponent<Text>();
        this.minmaipTop = this.Root.Find("Offset_Map/Panel_MiniMap/Panel_top");
        this.lineList = this.Root.Find("Offset_Map/Panel_MiniMap/Panel_top/Dropdown").GetComponent<Dropdown>();
        this.btnMap = this.Root.Find("Offset_Map/Panel_MiniMap/Panel_main/Panel_sys/btn_map");
        this.hoverTipsRoot = this.Root.Find("Offset_Map/img_bg_tips");
        this.hoverTipsRoot.transform.SetParent(this.AreamapImage.transform);
        this.hoverTipsRoot.transform.localScale = Vector3.one;
        this.hoverTipsRoot.transform.localPosition = Vector3.zero;
        this.hoverTipsRoot.gameObject.SetActive(false);
        this.btnHide = this.Panel_MiniMap.Find("btn_hide");
        this.btnGm = this.Panel_MiniMap.Find("Panel_main/Panel_sys/btn_gm").GetComponent<Button>();
        this.imgGmNewMsg = this.Panel_MiniMap.Find("Panel_main/Panel_sys/newmessage").GetComponent<Image>();
        this.btnGm.onClick.RemoveAllListeners();
        this.btnGm.onClick.AddListener(new UnityAction(this.btn_gmchat_onclick));
        if (ControllerManager.Instance.GetController<GmChatController>().mMapInitGmBtnShow)
        {
            this.btnGm.gameObject.SetActive(true);
            this.imgGmNewMsg.gameObject.SetActive(true);
            ControllerManager.Instance.GetController<GmChatController>().mMapInitGmBtnShow = false;
        }
        this.btnPvp = this.Panel_MiniMap.Find("Panel_main/Panel_sys/btn_wudou");
        this.btnDuoQi = this.Panel_MiniMap.Find("Panel_main/Panel_sys/btn_bf");
        this.textTip = this.btnDuoQi.GetComponent<TextTip>();
        if (this.textTip == null)
        {
            this.textTip = this.btnDuoQi.gameObject.AddComponent<TextTip>();
            this.textTip.SetText("1");
        }
        this.btnExitCopy = this.Panel_MiniMap.Find("Panel_main/Panel_sys/btn_exit");
        this.btnMail = this.Panel_MiniMap.Find("Panel_main/Panel_sys/btn_mail");
        this.newMailTips = this.Panel_MiniMap.Find("Panel_main/Panel_sys/newmail");
        this.newMailTips.gameObject.SetActive(false);
        this.mapRoot = this.Panel_MiniMap.Find("Panel_main");
        GameObject gameObject = this.Panel_Large.Find("background/img_bg1/btn_close").gameObject;
        UIEventListener.Get(gameObject).onClick = new UIEventListener.VoidDelegate(this.CloseBigMap);
        UIEventListener.Get(this.PanelMap.gameObject).onClick = delegate (PointerEventData data)
        {
            Vector2 zero = Vector2.zero;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(this.PanelMapImage.rectTransform, data.position, data.pressEventCamera, out zero))
            {
                this.PathFindToClickPos(zero, false);
            }
        };
        UIEventListener.Get(this.AreamapImage.gameObject).onClick = delegate (PointerEventData data)
        {
            if (data.button == PointerEventData.InputButton.Left)
            {
                Vector2 zero = Vector2.zero;
                if (RectTransformUtility.ScreenPointToLocalPointInRectangle(this.AreamapImage.rectTransform, data.position, data.pressEventCamera, out zero))
                {
                    this.PathFindToClickPos(zero, true);
                }
            }
            else if (Constant.CUR_VRESION != Constant.Version.Release && data.button == PointerEventData.InputButton.Right && (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)))
            {
                this.GoBigMapPointFast();
            }
        };
        if (this.AreamapImage.GetComponent<DragMove>() == null)
        {
            this.AreamapImage.gameObject.AddComponent<DragMove>();
        }
        UIEventListener.Get(this.AreamapImage.gameObject).onEnter = new UIEventListener.VoidDelegate(this.areamap_on_enter);
        UIEventListener.Get(this.AreamapImage.gameObject).onExit = new UIEventListener.VoidDelegate(this.areamap_on_exit);
        Scheduler.Instance.AddFrame(1U, true, new Scheduler.OnScheduler(this.ShowCurMapHoverPos));
        Button component = this.btnHide.GetComponent<Button>();
        component.onClick.RemoveAllListeners();
        component.onClick.AddListener(new UnityAction(this.MapSwitch));
        Button component2 = this.btnPvp.GetComponent<Button>();
        component2.onClick.RemoveAllListeners();
        component2.onClick.AddListener(new UnityAction(this.OnPvpButtonDown));
        this.btnPvp.gameObject.SetActive(false);
        Button component3 = this.btnDuoQi.GetComponent<Button>();
        component3.onClick.RemoveAllListeners();
        component3.onClick.AddListener(new UnityAction(this.OnDuoQiButtonDown));
        component3.gameObject.SetActive(false);
        Button component4 = this.btnExitCopy.GetComponent<Button>();
        component4.onClick.RemoveAllListeners();
        component4.onClick.AddListener(new UnityAction(this.OnExitCopyButtonDown));
        this.btnExitCopy.gameObject.SetActive(false);
        TextTip textTip = this.btnExitCopy.GetComponent<TextTip>();
        if (null == textTip)
        {
            textTip = this.btnExitCopy.gameObject.AddComponent<TextTip>();
        }
        textTip.SetTextCb(() => "退出副本");
        this.lineList.onValueChanged.RemoveAllListeners();
        this.lineList.onValueChanged.AddListener(new UnityAction<int>(this.OnSelectLine));
        UIMapController controller = ControllerManager.Instance.GetController<UIMapController>();
        controller.RefreshLinesData();
        ShortcutsConfigController scc = ControllerManager.Instance.GetController<ShortcutsConfigController>();
        Button component5 = this.btnMail.GetComponent<Button>();
        component5.onClick.RemoveAllListeners();
        component5.onClick.AddListener(new UnityAction(this.OnMailButtonDown));
        textTip = component5.gameObject.GetComponent<TextTip>();
        if (null == textTip)
        {
            textTip = component5.gameObject.AddComponent<TextTip>();
        }
        textTip.SetTextCb(() => "邮件(" + scc.GetKeyNameForItemByFunctionType(ShortcutkeyFunctionType.Mail) + ")");
        GlobalRegister.UpdateBubbleOfMail(true);
        Button component6 = this.btnMap.GetComponent<Button>();
        component6.onClick.RemoveAllListeners();
        component6.onClick.AddListener(new UnityAction(this.BigMapSwitch));
        textTip = this.btnMap.gameObject.GetComponent<TextTip>();
        if (null == textTip)
        {
            textTip = this.btnMap.gameObject.AddComponent<TextTip>();
        }
        textTip.SetTextCb(() => "地图(" + scc.GetKeyNameForItemByFunctionType(ShortcutkeyFunctionType.Map) + ")");
    }

    private void btn_gmchat_onclick()
    {
        this.btnGm.gameObject.SetActive(false);
        this.imgGmNewMsg.gameObject.SetActive(false);
        ControllerManager.Instance.GetController<GmChatController>().OpenGmChatPanel();
    }

    private void BigMapSwitch()
    {
        UI_Map uiobject = UIManager.GetUIObject<UI_Map>();
        if (uiobject != null)
        {
            if (uiobject.Panel_Large.gameObject.activeInHierarchy)
            {
                uiobject.CloseBigMap(null);
            }
            else
            {
                uiobject.OpenBigMap(null);
            }
        }
    }

    public void GoBigMapPointFast()
    {
        Vector3 serverPosByMouseOnBigMapPoint = this.GetServerPosByMouseOnBigMapPoint();
        GameScene manager = ManagerCenter.Instance.GetManager<GameScene>();
        uint num = manager.CurrentSceneData.mapID();
        int currentLineID = manager.CurrentLineID;
        string content = string.Concat(new object[]
        {
            "//gomap id=",
            num,
            " pos=",
            serverPosByMouseOnBigMapPoint.x,
            ",",
            serverPosByMouseOnBigMapPoint.y,
            " line=",
            currentLineID
        });
        ControllerManager.Instance.GetController<ChatControl>().SendChat(ChannelType.ChannelType_World, content, 0U, null);
        this.CloseBigMap(null);
    }

    public Vector3 GetServerPosByMouseOnBigMapPoint()
    {
        Camera worldCamera = UIManager.Instance.UIRoot.GetComponent<Canvas>().worldCamera;
        Vector3 position = worldCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector3 vector = this.AreamapImage.transform.InverseTransformPoint(position);
        if (this.MapController != null && this.MapController.AreamapMgr != null)
        {
            GameMap areamapMgr = this.MapController.AreamapMgr;
            float num = areamapMgr.SceneSize / 2f;
            Vector3 result = new Vector3(vector.x / areamapMgr.MapSecneRate + num, (vector.y / areamapMgr.MapSecneRate - num) * -1f, 0f);
            return result;
        }
        return Vector3.zero;
    }

    public void InitmapView()
    {
        this.ClearAreamapMapInUI();
        this.ClearPanelMapInUI();
        this.ResetCompetition();
        this.Panel_Large.gameObject.SetActive(false);
        this.Areamap.gameObject.SetActive(false);
        GameScene manager = ManagerCenter.Instance.GetManager<GameScene>();
        if (manager != null)
        {
            this.Panel_MiniMap.Find("Panel_top/Dropdown/Text_name").GetComponent<Text>().text = manager.CurrentSceneData.showName();
            this.PanelMap.gameObject.SetActive(true);
        }
    }

    private void OnSelectLine(int index)
    {
        UIMapController controller = ControllerManager.Instance.GetController<UIMapController>();
        controller.OnSelectLine(index);
    }

    public void ResetLines()
    {
        this.lineList.ClearOptions();
    }

    public void AddLine(List<Dropdown.OptionData> arg, int currline)
    {
        this.ResetLines();
        this.lineList.AddOptions(arg);
        this.lineList.value = currline;
    }

    public void ResetBack(int currline)
    {
        this.lineList.value = currline;
    }

    private void MapSwitch()
    {
        TweenRotation component = this.btnHide.gameObject.GetComponent<TweenRotation>();
        TweenPosition component2 = this.mapRoot.gameObject.GetComponent<TweenPosition>();
        TweenPosition component3 = this.minmaipTop.gameObject.GetComponent<TweenPosition>();
        if (component)
        {
            component.Play(this.isShowMap);
        }
        if (component2)
        {
            component2.Play(this.isShowMap);
        }
        if (component3)
        {
            component3.Play(this.isShowMap);
        }
        this.isShowMap = !this.isShowMap;
    }

    private void OnMailButtonDown()
    {
        GlobalRegister.OpenMailUI();
    }

    private void OnPvpButtonDown()
    {
        if (ControllerManager.Instance.GetController<PVPMatchController>().pvpState > StageType.Prepare)
        {
            UIManager.Instance.ShowUI<UI_MessageBox>("UI_MessageBox", delegate ()
            {
                UI_MessageBox uiobject = UIManager.GetUIObject<UI_MessageBox>();
                uiobject.SetOkCb(new MessageOkCb2(this.LeaveCompetition), "离开");
                uiobject.SetContent("是否离开武道会", "提示", true);
            }, UIManager.ParentType.CommonUI, false);
        }
        else
        {
            ControllerManager.Instance.GetController<PVPMatchController>().ShowMatchCompleteUI();
        }
    }

    private void OnDuoQiButtonDown()
    {
        ControllerManager.Instance.GetController<DuoQiController>().OpenPanel();
    }

    private void LeaveCompetition(string str)
    {
        ControllerManager.Instance.GetController<PVPCompetitionController>().Req_ExitCopymap_SC();
    }

    private void OnExitCopyButtonDown()
    {
        ManagerCenter.Instance.GetManager<CopyManager>().ReqExitCopy();
    }

    public void SetMiddleExitCopyState(bool b)
    {
        this.btnExitCopy.gameObject.SetActive(b);
    }

    public void ActivePvpBtn(bool active)
    {
        this.btnPvp.gameObject.SetActive(active);
    }

    public void ActiveNewMailTips(bool active)
    {
        this.newMailTips.gameObject.SetActive(active);
    }

    private void ResetCompetition()
    {
        ControllerManager.Instance.GetController<PVPCompetitionController>().CloseScoreUI();
        if (ControllerManager.Instance.GetController<PVPMatchController>().pvpState == StageType.Finish)
        {
            ControllerManager.Instance.GetController<PVPMatchController>().pvpState = StageType.None_Stage;
            this.ActivePvpBtn(false);
        }
    }

    private void ShowCurMapHoverPos()
    {
        if (this.MapController.isShowAreaTipReaded && this.MapController.AreamapMgr != null && this.isAreaMapHovering)
        {
            Vector3 serverPosByMouseOnBigMapPoint = this.GetServerPosByMouseOnBigMapPoint();
            this.hoverTipsRoot.GetChild(0).GetComponent<Text>().text = string.Concat(new object[]
            {
                "(",
                (int)serverPosByMouseOnBigMapPoint.x,
                ",",
                (int)serverPosByMouseOnBigMapPoint.y,
                ")"
            });
            float num = this.MapController.AreamapMgr.SceneSize / 2f;
            Vector3 localPosition = new Vector3((serverPosByMouseOnBigMapPoint.x - num) * this.MapController.AreamapMgr.MapSecneRate, (serverPosByMouseOnBigMapPoint.y * -1f + num) * this.MapController.AreamapMgr.MapSecneRate, 0f);
            this.hoverTipsRoot.transform.localPosition = localPosition;
            this.hoverTipsRoot.gameObject.SetActive(true);
        }
    }

    private void areamap_on_enter(PointerEventData eventData)
    {
        this.isAreaMapHovering = true;
        this.hoverTipsRoot.gameObject.SetActive(true);
    }

    private void areamap_on_exit(PointerEventData eventData)
    {
        this.isAreaMapHovering = false;
        this.hoverTipsRoot.gameObject.SetActive(false);
    }

    public void CloseBigMap(PointerEventData ed)
    {
        ManagerCenter.Instance.GetManager<EscManager>().CloseUI("UI_Map");
        this.Panel_Large.gameObject.SetActive(false);
        this.Areamap.gameObject.SetActive(false);
        this.hoverTipsRoot.gameObject.SetActive(false);
        this.MapController.isShowAreaTipReaded = true;
        if (this.MapController.PanelmapMgr != null)
        {
            this.MapController.PanelmapMgr.CloseShowOneNpc();
        }
        if (this.MapController.AreamapMgr != null)
        {
            this.MapController.AreamapMgr.ClearShowOneData();
        }
        this.ClearAreamapMapInUI();
    }

    public void OpenBigMap(PointerEventData data)
    {
        AbattoirMatchController controller = ControllerManager.Instance.GetController<AbattoirMatchController>();
        controller.SendReqRadarPos();
        ManagerCenter.Instance.GetManager<EscManager>().OpenUI("UI_Map");
        this.Panel_Large.gameObject.SetActive(true);
        this.OpenCurrAreamap();
        this.RefreshAbattoirAreaList();
    }

    private float GetMapSceneRate()
    {
        if ((float)LSingleton<CurrentMapAccesser>.Instance.CellNumX <= this.uiWidth * 2f)
        {
            return this.uiWidth / (float)LSingleton<CurrentMapAccesser>.Instance.CellNumX;
        }
        return 0.5f;
    }

    public void InitPanelmapInUI()
    {
        uint num = ManagerCenter.Instance.GetManager<GameScene>().CurrentSceneData.mapID();
        LuaTable cacheField_Table = LuaConfigManager.GetXmlConfigTable("scenesinfo").GetCacheField_Table("mapinfo").GetCacheField_Table(num.ToString());
        if (cacheField_Table == null)
        {
            return;
        }
        RectTransform component = this.PanelMap.Find("img_outline/map").GetComponent<RectTransform>();
        this.MapController.PanelmapMgr = new GameMap(this.PanelMapImage, this.PanelMap.Find("icon"), (float)LSingleton<CurrentMapAccesser>.Instance.CellNumX, this.GetMapSceneRate(), new Vector2(component.rect.width, component.rect.height), cacheField_Table.GetField_Float("fovrotationY"), string.Empty);
    }

    private void InitPanelNpcIcon()
    {
        EntitiesManager manager = ManagerCenter.Instance.GetManager<EntitiesManager>();
        if (manager != null)
        {
            manager.NpcList.BetterForeach(delegate (KeyValuePair<ulong, Npc> pair)
            {
                Npc value = pair.Value;
                PlayerBufferControl component = value.GetComponent<PlayerBufferControl>();
                if (component != null && component.ContainsState(UserState.USTATE_TIPS))
                {
                    value.TryUpdateMapIcon(true);
                }
            });
        }
    }

    public void InitAreamapMapInUI()
    {
        uint mapText = ManagerCenter.Instance.GetManager<GameScene>().CurrentSceneData.mapID();
        LuaTable cacheField_Table = LuaConfigManager.GetXmlConfigTable("scenesinfo").GetCacheField_Table("mapinfo").GetCacheField_Table(mapText.ToString());
        if (cacheField_Table == null)
        {
            return;
        }
        this.MapController.AreamapMgr = new GameMap(this.AreamapImage, this.Areamap.Find("icon"), (float)LSingleton<CurrentMapAccesser>.Instance.CellNumX, this.GetMapSceneRate(), Vector2.zero, cacheField_Table.GetField_Float("fovrotationY"), string.Empty);
        this.CnMapName.text = cacheField_Table.GetField_String("showName");
        this.EnMapName.text = cacheField_Table.GetField_String("name_en");
        this.MapController.SetMapText(mapText);
    }

    private void ClearAreamapMapInUI()
    {
        if (this.MapController.AreamapMgr != null)
        {
            this.MapController.AreamapMgr.ClearMap();
            this.MapController.AreamapMgr = null;
        }
    }

    private void ClearPanelMapInUI()
    {
        if (this.MapController.PanelmapMgr != null)
        {
            this.MapController.PanelmapMgr.ClearMap();
            this.MapController.PanelmapMgr = null;
        }
    }

    public void InitNpcBtnList()
    {
        if (!this.IsAreamapShow)
        {
            return;
        }
        if (this.Areamap == null)
        {
            FFDebug.LogWarning(this, "Areamap null");
            return;
        }
        if (this.MapController.AreamapMgr == null)
        {
            FFDebug.LogWarning(this, "AreamapMgr null");
            return;
        }
        this.ClearNpcBtnList();
        if (!this.IsOnCurrAreamap)
        {
            return;
        }
        GameObject gameObject = this.Areamap.transform.Find("npclist/list/Rect/Item").gameObject;
        gameObject.SetActive(false);
        List<MapItem> npcItemList = this.MapController.AreamapMgr.GetNpcItemList();
        for (int i = 0; i < npcItemList.Count; i++)
        {
            UI_Map.NpcBtn npcBtn = new UI_Map.NpcBtn();
            npcBtn.SetData(npcItemList[i]);
            this.NpcBtnList.Add(npcBtn);
        }
        this.NpcBtnList.Sort(this.mNpcBtnIComparer);
        for (int j = 0; j < this.NpcBtnList.Count; j++)
        {
            GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(gameObject);
            UI_Map.NpcBtn npcBtn2 = this.NpcBtnList[j];
            if (npcBtn2 != null)
            {
                npcBtn2.SetObjNpcBtn(gameObject2, gameObject);
                npcBtn2.OnKick = new Action<UI_Map.NpcBtn>(this.OnKickNpcBtn);
            }
            gameObject2.transform.SetParent(gameObject.transform.parent);
            gameObject2.transform.localScale = gameObject.transform.localScale;
            gameObject2.SetActive(true);
        }
        LayoutRebuilder.ForceRebuildLayoutImmediate(gameObject.transform.parent.GetComponent<RectTransform>());
    }

    private void InitMonsterBtnList()
    {
        this.monsterNpcXmlDic = new Dictionary<uint, List<NpcMapXmlData>>();
        uint mapID = ManagerCenter.Instance.GetManager<GameScene>().CurrentSceneData.mapID();
        XmlNodeList mapDataXmlNodeList = Npc.GetMapDataXmlNodeList(mapID);
        foreach (object obj in mapDataXmlNodeList)
        {
            XmlElement xmlElement = (XmlElement)obj;
            if (xmlElement.Name == "npc")
            {
                uint num = uint.Parse(xmlElement.GetAttribute("id"));
                LuaTable configTable = LuaConfigManager.GetConfigTable("npc_data", (ulong)num);
                if (configTable != null)
                {
                    NpcType cacheField_Uint = (NpcType)configTable.GetCacheField_Uint("kind");
                    bool flag = !CommonTools.IsCurrentInFubenScence() && (cacheField_Uint == NpcType.NPC_TYPE_NOACTIVE || cacheField_Uint == NpcType.NPC_TYPE_ACTIVE || cacheField_Uint == NpcType.NPC_TYPE_MONSTER);
                    if (cacheField_Uint == NpcType.NPC_TYPE_NOACTSKILL || flag || cacheField_Uint == NpcType.NPC_TYPE_BOSS || (cacheField_Uint == NpcType.NPC_TYPE_QUESTGATHER && this.CheckTaskCollectInUI(num)))
                    {
                        if (!this.monsterNpcXmlDic.ContainsKey(num))
                        {
                            this.monsterNpcXmlDic.Add(num, new List<NpcMapXmlData>());
                        }
                        NpcMapXmlData npcMapXmlData = new NpcMapXmlData();
                        npcMapXmlData.mId = num;
                        npcMapXmlData.mName = xmlElement.GetAttribute("name");
                        npcMapXmlData.mCellPosX = uint.Parse(xmlElement.GetAttribute("x"));
                        npcMapXmlData.mCellPosY = uint.Parse(xmlElement.GetAttribute("y"));
                        ulong mapId = (ulong)ManagerCenter.Instance.GetManager<GameScene>().CurrentSceneData.mapID();
                        npcMapXmlData.mPathWayId = this.MapController.GetPathWayId(mapId, num);
                        this.monsterNpcXmlDic[num].Add(npcMapXmlData);
                    }
                }
            }
        }
        List<NpcMapXmlData> list = new List<NpcMapXmlData>();
        foreach (List<NpcMapXmlData> list2 in this.monsterNpcXmlDic.Values)
        {
            list.Add(list2[0]);
        }
        Dictionary<uint, List<NpcMapXmlData>> dictionary = new Dictionary<uint, List<NpcMapXmlData>>();
        for (int i = 0; i < list.Count; i++)
        {
            dictionary.Add(list[i].mId, this.monsterNpcXmlDic[list[i].mId]);
        }
        this.monsterNpcXmlDic = dictionary;
        if (!this.IsOnCurrAreamap)
        {
            return;
        }
        GameObject gameObject = this.Areamap.transform.Find("monsterlist/list/Rect/Item").gameObject;
        gameObject.SetActive(false);
        List<GameObject> list3 = new List<GameObject>();
        foreach (object obj2 in gameObject.transform.parent)
        {
            Transform transform = (Transform)obj2;
            if (transform.name != "Item")
            {
                list3.Add(transform.gameObject);
            }
        }
        for (int j = 0; j < list3.Count; j++)
        {
            UnityEngine.Object.Destroy(list3[j]);
        }
        int num2 = 0;
        foreach (uint key in this.monsterNpcXmlDic.Keys)
        {
            GameObject Clone = UnityEngine.Object.Instantiate<GameObject>(gameObject);
            Clone.transform.SetParent(gameObject.transform.parent);
            Clone.transform.localScale = gameObject.transform.localScale;
            Clone.SetActive(true);
            num2++;
            NpcMapXmlData npcMapXmlData2 = this.monsterNpcXmlDic[key][0];
            Clone.transform.Find("txt_name").GetComponent<Text>().text = string.Concat(new object[]
            {
                npcMapXmlData2.mName,
                "(",
                this.monsterNpcXmlDic[key].Count,
                ")"
            });
            Clone.transform.Find("txt_pos").GetComponent<Text>().text = string.Concat(new object[]
            {
                "(",
                npcMapXmlData2.mCellPosX,
                ",",
                npcMapXmlData2.mCellPosY,
                ")"
            });
            Clone.name = key.ToString();
            Clone.GetComponent<Button>().onClick.AddListener(delegate ()
            {
                this.monsterBtn_on_click(Clone);
            });
        }
        this.InitBossIconInMap();
        LayoutRebuilder.ForceRebuildLayoutImmediate(gameObject.transform.parent.GetComponent<RectTransform>());
    }

    private bool CheckTaskCollectInUI(uint id)
    {
        bool ret = false;
        ControllerManager.Instance.GetController<TaskController>().NpcTaskMap.BetterForeach(delegate (KeyValuePair<uint, NpcTask> item)
        {
            NpcTask value = item.Value;
            TaskInfo firstShowTask = value.GetFirstShowTask();
            LuaTable configTable = LuaConfigManager.GetConfigTable("questconfig", (ulong)firstShowTask.questid);
            if (configTable != null)
            {
                string cacheField_String = configTable.GetCacheField_String("gatherid");
                if (!string.IsNullOrEmpty(cacheField_String))
                {
                    string[] array = cacheField_String.Split(new char[]
                    {
                        '-'
                    });
                    for (int i = 0; i < array.Length; i++)
                    {
                        uint num = (uint)float.Parse(array[i]);
                        if (id == num && firstShowTask.ShowPriority != 3)
                        {
                            ret = true;
                        }
                    }
                }
            }
        });
        return ret;
    }

    private void InitBossIconInMap()
    {
        for (int i = 0; i < this.bossIcons.Count; i++)
        {
            UnityEngine.Object.DestroyImmediate(this.bossIcons[i]);
        }
        this.bossIcons.Clear();
        foreach (uint key in this.monsterNpcXmlDic.Keys)
        {
            List<NpcMapXmlData> list = this.monsterNpcXmlDic[key];
            for (int j = 0; j < list.Count; j++)
            {
                NpcMapXmlData npcMapXmlData = list[j];
                UIMapController controller = ControllerManager.Instance.GetController<UIMapController>();
                if (controller != null)
                {
                    GameMap areamapMgr = controller.AreamapMgr;
                    Vector3 localPosition = new Vector3(npcMapXmlData.mCellPosX - areamapMgr.SceneSize / 2f, (float)(-(float)((ulong)npcMapXmlData.mCellPosY)) + areamapMgr.SceneSize / 2f, 0f) * areamapMgr.MapSecneRate;
                    LuaTable configTable = LuaConfigManager.GetConfigTable("npc_data", (ulong)npcMapXmlData.mId);
                    if (configTable.GetField_Uint("kind") == 5U)
                    {
                        GameObject gameObject = this.Areamap.transform.Find("icon/iconenemy/img_player").gameObject;
                        GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(gameObject);
                        gameObject2.transform.SetParent(this.Areamap.transform.Find("map/map_bg"));
                        gameObject2.transform.localPosition = localPosition;
                        gameObject2.transform.localScale = Vector3.one;
                        this.bossIcons.Add(gameObject2);
                    }
                }
            }
        }
    }

    private void monsterBtn_on_click(GameObject btn)
    {
        if (this.clickMonsterBtn != btn || false || (DateTime.Now - this.clickMonsterTime).TotalMilliseconds > 1000.0)
        {
            if (this.clickMonsterBtn != btn)
            {
                btn.transform.Find("sp_on").gameObject.SetActive(true);
                if (this.clickMonsterBtn != null)
                {
                    this.clickMonsterBtn.transform.Find("sp_on").gameObject.SetActive(false);
                }
                if (this.selMonsterList == null)
                {
                    this.selMonsterList = new List<GameObject>();
                }
                for (int i = 0; i < this.selMonsterList.Count; i++)
                {
                    UnityEngine.Object.Destroy(this.selMonsterList[i]);
                }
                this.selMonsterList = new List<GameObject>();
                if (this.monsterNpcXmlDic.ContainsKey(uint.Parse(btn.name)))
                {
                    List<NpcMapXmlData> list = this.monsterNpcXmlDic[uint.Parse(btn.name)];
                    Vector3 leftBottomPos = Vector3.zero;
                    Vector3 rightUpPos = Vector3.zero;
                    for (int j = 0; j < list.Count; j++)
                    {
                        NpcMapXmlData npcMapXmlData = list[j];
                        UIMapController controller = ControllerManager.Instance.GetController<UIMapController>();
                        if (controller != null)
                        {
                            GameMap areamapMgr = controller.AreamapMgr;
                            Vector3 vector = new Vector3(npcMapXmlData.mCellPosX - areamapMgr.SceneSize / 2f, (float)(-(float)((ulong)npcMapXmlData.mCellPosY)) + areamapMgr.SceneSize / 2f, 0f) * areamapMgr.MapSecneRate;
                            LuaTable configTable = LuaConfigManager.GetConfigTable("npc_data", (ulong)npcMapXmlData.mId);
                            if (configTable.GetField_Uint("kind") != 5U)
                            {
                                GameObject gameObject = this.Areamap.transform.Find("icon/iconenemy/img_player").gameObject;
                                GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(gameObject);
                                gameObject2.transform.SetParent(this.Areamap.transform.Find("map/map_bg"));
                                gameObject2.transform.localPosition = vector;
                                gameObject2.transform.localScale = Vector3.one;
                                gameObject2.transform.SetAsFirstSibling();
                                this.selMonsterList.Add(gameObject2);
                            }
                            if (j == 0)
                            {
                                leftBottomPos = vector;
                                rightUpPos = vector;
                            }
                            else
                            {
                                if (vector.x < leftBottomPos.x)
                                {
                                    leftBottomPos.x = vector.x;
                                }
                                if (vector.y < leftBottomPos.y)
                                {
                                    leftBottomPos.y = vector.y;
                                }
                                if (vector.x > rightUpPos.x)
                                {
                                    rightUpPos.x = vector.x;
                                }
                                if (vector.y > rightUpPos.y)
                                {
                                    rightUpPos.y = vector.y;
                                }
                            }
                        }
                    }
                    this.ReSetMapFramePos(leftBottomPos, rightUpPos);
                }
            }
            this.clickMonsterBtn = btn;
            this.clickMonsterTime = DateTime.Now;
            return;
        }
        TaskUIController controller2 = ControllerManager.Instance.GetController<TaskUIController>();
        if (controller2 == null)
        {
            return;
        }
        if (!this.monsterNpcXmlDic.ContainsKey(uint.Parse(btn.name)))
        {
            return;
        }
        uint mPathWayId = this.monsterNpcXmlDic[uint.Parse(btn.name)][0].mPathWayId;
        controller2.FindPath(mPathWayId, null);
    }

    private void ReSetMapFramePos(Vector3 leftBottomPos, Vector3 rightUpPos)
    {
        RectTransform rectTransform = this.Areamap.Find("map/map_bg") as RectTransform;
        float num = this.uiWidth;
        float num2 = this.uiWidth;
        Vector2 anchoredPosition = rectTransform.anchoredPosition;
        float num3 = -anchoredPosition.x;
        float num4 = -anchoredPosition.y;
        Vector3 vector = new Vector3(-num / 2f - anchoredPosition.x, -num2 / 2f - anchoredPosition.y, 0f);
        Vector3 vector2 = new Vector3(num / 2f - anchoredPosition.x, num2 / 2f - anchoredPosition.y, 0f);
        if (leftBottomPos.x < vector.x)
        {
            anchoredPosition.x += vector.x - leftBottomPos.x + 20f;
        }
        if (rightUpPos.x > vector2.x)
        {
            anchoredPosition.x += vector2.x - rightUpPos.x - 20f;
        }
        if (leftBottomPos.y < vector.y)
        {
            anchoredPosition.y += vector.y - leftBottomPos.y + 20f;
        }
        if (rightUpPos.y > vector2.y)
        {
            anchoredPosition.y += vector2.y - rightUpPos.y - 20f;
        }
        float num5 = (rectTransform.sizeDelta.x - (rectTransform.parent as RectTransform).sizeDelta.x) / 2f;
        if (anchoredPosition.x > num5)
        {
            anchoredPosition.x = num5;
        }
        else if (anchoredPosition.x < -num5)
        {
            anchoredPosition.x = -num5;
        }
        if (anchoredPosition.y > num5)
        {
            anchoredPosition.y = num5;
        }
        else if (anchoredPosition.y < -num5)
        {
            anchoredPosition.y = -num5;
        }
        rectTransform.anchoredPosition = anchoredPosition;
    }

    private void OnKickNpcBtn(UI_Map.NpcBtn btn)
    {
        if (btn == null)
        {
            return;
        }
        if (btn.PathWayId == 0U)
        {
            return;
        }
        for (int i = 0; i < this.NpcBtnList.Count; i++)
        {
            this.NpcBtnList[i].SetSelect(this.NpcBtnList[i] == btn);
        }
        if (this.clickBtn != btn || false || (DateTime.Now - this.clickTime).TotalMilliseconds > 1000.0)
        {
            if (this.clickBtn != null)
            {
                Transform transform = this.AreamapImage.transform.Find(this.clickBtn.mId);
                if (transform != null)
                {
                    transform.GetChild(0).gameObject.SetActive(false);
                }
                else
                {
                    this.clickBtn = null;
                }
            }
            Transform transform2 = this.AreamapImage.transform.Find(btn.mId);
            if (transform2 != null)
            {
                transform2.GetChild(0).gameObject.SetActive(true);
            }
            this.clickBtn = btn;
            this.clickTime = DateTime.Now;
        }
        else
        {
            TaskUIController controller = ControllerManager.Instance.GetController<TaskUIController>();
            if (controller == null)
            {
                return;
            }
            controller.FindPath(btn.PathWayId, null);
        }
        this.MapController.PanelmapMgr.OpenShowOneNpc(btn.mBaseid);
        this.MapController.AreamapMgr.OpenShowOneNpc(btn.mBaseid);
        Vector3 npcAreaLeftBottomPos = this.MapController.AreamapMgr.GetNpcAreaLeftBottomPos(btn.mBaseid);
        Vector3 npcAreaRightTopPos = this.MapController.AreamapMgr.GetNpcAreaRightTopPos(btn.mBaseid);
        this.ReSetMapFramePos(npcAreaLeftBottomPos, npcAreaRightTopPos);
    }

    private void ClearNpcBtnList()
    {
        for (int i = 0; i < this.NpcBtnList.Count; i++)
        {
            this.NpcBtnList[i].Dispose();
        }
        this.NpcBtnList.Clear();
    }

    private void OpenCurrAreamap()
    {
        this.ClearAreamapMapInUI();
        this.IsOnCurrAreamap = true;
        this.InitAreamapMapInUI();
        if (this.MapController.AreamapMgr != null)
        {
            this.MapController.SetupCharactorPos(delegate
            {
                this.Areamap.gameObject.SetActive(true);
                this.InitNpcBtnList();
            });
        }
        this.InitMonsterBtnList();
    }

    public Vector2 CovertToServerPos(Vector2 localPos, bool isInAraMap)
    {
        float mapSecneRate;
        float sceneSize;
        if (isInAraMap)
        {
            mapSecneRate = this.MapController.AreamapMgr.MapSecneRate;
            sceneSize = this.MapController.AreamapMgr.SceneSize;
        }
        else
        {
            mapSecneRate = this.MapController.PanelmapMgr.MapSecneRate;
            sceneSize = this.MapController.PanelmapMgr.SceneSize;
        }
        Vector2 vector = localPos / mapSecneRate;
        int num = Mathf.RoundToInt(vector.x + sceneSize * 0.5f);
        int num2 = Mathf.RoundToInt((vector.y - sceneSize * 0.5f) * -1f);
        Vector2 result = new Vector2((float)num, (float)num2);
        return result;
    }

    private void PathFindToClickPos(Vector2 localPos, bool isInAraMap)
    {
        Vector2 vector = this.CovertToServerPos(localPos, isInAraMap);
        if (!GraphUtils.PosIsInMap(vector, true))
        {
            return;
        }
        PathFindComponent pfc = MainPlayer.Self.Pfc;
        pfc.BeginFindPath(vector, PathFindComponent.AutoMoveState.MoveToPointWithoutSign, null, null);
        this.CancelSelectAllNpcBtn();
    }

    public void SelectNPCBtnByPathID(uint pathID)
    {
        if (this.NpcBtnList == null || this.NpcBtnList.Count < 1)
        {
            return;
        }
        for (int i = 0; i < this.NpcBtnList.Count; i++)
        {
            UI_Map.NpcBtn npcBtn = this.NpcBtnList[i];
            if (npcBtn.PathWayId == pathID)
            {
                if (npcBtn.OnKick != null)
                {
                    npcBtn.OnKick(npcBtn);
                }
                break;
            }
        }
    }

    private void CancelSelectAllNpcBtn()
    {
        if (this.NpcBtnList == null || this.NpcBtnList.Count < 1)
        {
            return;
        }
        for (int i = 0; i < this.NpcBtnList.Count; i++)
        {
            UI_Map.NpcBtn npcBtn = this.NpcBtnList[i];
            npcBtn.SetSelect(false);
        }
    }

    public override void OnDispose()
    {
        this.ClearNpcBtnList();
        base.OnDispose();
        this.ClearAreamapMapInUI();
        this.ClearPanelMapInUI();
        Scheduler.Instance.RemoveFrame(new Scheduler.OnScheduler(this.ShowCurMapHoverPos));
        Scheduler.Instance.RemoveFrame(new Scheduler.OnScheduler(this.InitPanelNpcIcon));
    }

    public void RetHoldFlagTeamScore(List<HoldFlagCampScore> scores)
    {
        DuoQiController controller = ControllerManager.Instance.GetController<DuoQiController>();
        for (int i = 0; i < scores.Count; i++)
        {
            HoldFlagCampScore holdFlagCampScore = scores[i];
            if (controller.IsSameCamp(holdFlagCampScore.campId))
            {
                this.txtScore1.text = holdFlagCampScore.score.ToString();
            }
            else
            {
                this.txtScore2.text = holdFlagCampScore.score.ToString();
            }
        }
    }

    public void RetHoldFlagDBState(List<HoldFlagDBState> states)
    {
        DuoQiController controller = ControllerManager.Instance.GetController<DuoQiController>();
        for (int i = 0; i < states.Count; i++)
        {
            HoldFlagDBState holdFlagDBState = states[i];
            if (controller.IsSameCamp(holdFlagDBState.campId))
            {
                this.objBall1.SetActive(holdFlagDBState.DBState);
            }
            else
            {
                this.objBall2.SetActive(holdFlagDBState.DBState);
            }
        }
    }

    public void RetHoldFlagCountDown(uint second)
    {
        this.mBattleCooldownSecond = (int)second;
        Scheduler.Instance.RemoveTimer(new Scheduler.OnScheduler(this.BattleCooldownUpdate));
        Scheduler.Instance.AddTimer(1f, true, new Scheduler.OnScheduler(this.BattleCooldownUpdate));
    }

    private void BattleCooldownUpdate()
    {
        if (this.txtBattleTimeCooldown == null)
        {
            Scheduler.Instance.RemoveTimer(new Scheduler.OnScheduler(this.BattleCooldownUpdate));
            return;
        }
        if (this.mBattleCooldownSecond >= 0)
        {
            this.txtBattleTimeCooldown.text = SingletonForMono<GameTime>.Instance.GetTimeText1(this.mBattleCooldownSecond);
            this.mBattleCooldownSecond--;
        }
        else
        {
            Scheduler.Instance.RemoveTimer(new Scheduler.OnScheduler(this.BattleCooldownUpdate));
        }
    }

    public void ShowHideDuoqiBtn(bool show)
    {
        this.btnDuoQi.gameObject.SetActive(show);
    }

    public void SetDuoqiMatchTipText(uint time)
    {
        string timeString = CommonTools.GetTimeString(this.MapController.mAvageTime);
        string text = "战场匹配中\r\n预计匹配时间" + timeString + "\r\n队列等待时间" + CommonTools.GetTimeString(time);
        this.DoSetDuoqiMatchTipText(text);
    }

    public void DoSetDuoqiMatchTipText(string text)
    {
        UI_TextTip uiobject = UIManager.GetUIObject<UI_TextTip>();
        if (uiobject != null)
        {
            if (uiobject.mEventData == this.btnDuoQi.gameObject)
            {
                if (this.transBattlePanel.gameObject.activeSelf)
                {
                    this.textTip.SetTextUI(string.Empty);
                }
                else
                {
                    this.textTip.SetTextUI(text);
                }
            }
        }
        else if (this.transBattlePanel.gameObject.activeSelf)
        {
            this.textTip.SetText(string.Empty);
        }
        else
        {
            this.textTip.SetText(text);
        }
    }

    public void ShowBattlePanel()
    {
        this.ShowHideDuoqiBtn(true);
        this.transBattlePanel.gameObject.SetActive(true);
    }

    public void HideBattlePanel()
    {
        this.ShowHideDuoqiBtn(false);
        this.transBattlePanel.gameObject.SetActive(false);
    }

    public void RefreshAbattoirAreaList()
    {
        GameScene manager = ManagerCenter.Instance.GetManager<GameScene>();
        if (!manager.isAbattoirScene)
        {
            for (int i = 0; i < this.reliveAreaIconList.Count; i++)
            {
                GameObject gameObject = this.reliveAreaIconList[i];
                if (gameObject != null)
                {
                    UnityEngine.Object.Destroy(gameObject);
                }
            }
            for (int j = 0; j < this.radarAreaIconList.Count; j++)
            {
                GameObject gameObject2 = this.radarAreaIconList[j];
                if (gameObject2 != null)
                {
                    UnityEngine.Object.Destroy(gameObject2);
                }
            }
            this.reliveAreaIconList.Clear();
            this.radarAreaIconList.Clear();
            return;
        }
        GameMap areamapMgr = this.MapController.AreamapMgr;
        if (areamapMgr == null)
        {
            return;
        }
        AbattoirMatchController controller = ControllerManager.Instance.GetController<AbattoirMatchController>();
        Transform transform = this.Areamap.transform.Find("icon/img_area_relive");
        List<AbattoirMatchController.ReliveAreaData> reliveDataList = controller.GetReliveDataList();
        for (int k = 0; k < reliveDataList.Count; k++)
        {
            AbattoirMatchController.ReliveAreaData reliveAreaData = reliveDataList[k];
            GameObject gameObject3;
            TextTip textTip;
            if (this.reliveAreaIconList.Count > k)
            {
                gameObject3 = this.reliveAreaIconList[k];
                if (gameObject3 == null)
                {
                    gameObject3 = UnityEngine.Object.Instantiate<GameObject>(transform.gameObject);
                    textTip = gameObject3.gameObject.AddComponent<TextTip>();
                    this.reliveAreaIconList[k] = gameObject3;
                }
                else
                {
                    textTip = gameObject3.gameObject.GetComponent<TextTip>();
                    if (textTip == null)
                    {
                        textTip = gameObject3.gameObject.AddComponent<TextTip>();
                    }
                }
            }
            else
            {
                gameObject3 = UnityEngine.Object.Instantiate<GameObject>(transform.gameObject);
                textTip = gameObject3.gameObject.AddComponent<TextTip>();
                this.reliveAreaIconList.Add(gameObject3);
            }
            Vector3 localPosition = new Vector3(reliveAreaData.x - areamapMgr.SceneSize / 2f, (float)(-(float)((ulong)reliveAreaData.y)) + areamapMgr.SceneSize / 2f, 0f) * areamapMgr.MapSecneRate;
            gameObject3.transform.SetParent(this.Areamap.transform.Find("map/map_bg"));
            gameObject3.transform.localPosition = localPosition;
            gameObject3.transform.localScale = Vector3.one;
            RectTransform rectTransform = gameObject3.transform as RectTransform;
            rectTransform.sizeDelta = Vector2.one * reliveAreaData.radius;
            textTip.SetText(string.Format("复活点（{0}，{1}）", reliveAreaData.x, reliveAreaData.y));
        }
        for (int l = reliveDataList.Count; l < this.reliveAreaIconList.Count; l++)
        {
            GameObject gameObject4 = this.reliveAreaIconList[l];
            if (gameObject4 != null)
            {
                UnityEngine.Object.Destroy(gameObject4);
            }
            this.reliveAreaIconList.RemoveAt(l);
            l--;
        }
        Transform transform2 = this.Areamap.transform.Find("icon/img_area_relive");
        List<TeamUser> users = controller.myTeamInfo.users;
        int getRadarRadius = controller.getRadarRadius;
        for (int m = 0; m < users.Count; m++)
        {
            TeamUser teamUser = users[m];
            GameObject gameObject5;
            TextTip textTip2;
            if (this.radarAreaIconList.Count > m)
            {
                gameObject5 = this.radarAreaIconList[m];
                if (gameObject5 == null)
                {
                    gameObject5 = UnityEngine.Object.Instantiate<GameObject>(transform.gameObject);
                    textTip2 = gameObject5.gameObject.AddComponent<TextTip>();
                    this.radarAreaIconList[m] = gameObject5;
                }
                else
                {
                    textTip2 = gameObject5.gameObject.GetComponent<TextTip>();
                    if (textTip2 == null)
                    {
                        textTip2 = gameObject5.gameObject.AddComponent<TextTip>();
                    }
                }
            }
            else
            {
                gameObject5 = UnityEngine.Object.Instantiate<GameObject>(transform.gameObject);
                gameObject5.GetComponent<Image>().color = new Color(1f, 1f, 0f, 0.2f);
                textTip2 = gameObject5.gameObject.AddComponent<TextTip>();
                this.radarAreaIconList.Add(gameObject5);
            }
            Vector3 localPosition2 = new Vector3(teamUser.x - areamapMgr.SceneSize / 2f, (float)(-(float)((ulong)teamUser.y)) + areamapMgr.SceneSize / 2f, 0f) * areamapMgr.MapSecneRate;
            gameObject5.transform.SetParent(this.Areamap.transform.Find("map/map_bg"));
            gameObject5.transform.localPosition = localPosition2;
            gameObject5.transform.localScale = Vector3.one;
            RectTransform rectTransform2 = gameObject5.transform as RectTransform;
            rectTransform2.sizeDelta = Vector2.one * (float)getRadarRadius;
            textTip2.SetText(string.Format("龙珠雷达（{0}，{1}）", teamUser.x, teamUser.y));
        }
        for (int n = users.Count; n < this.radarAreaIconList.Count; n++)
        {
            GameObject gameObject6 = this.radarAreaIconList[n];
            if (gameObject6 != null)
            {
                UnityEngine.Object.Destroy(gameObject6);
            }
            this.radarAreaIconList.RemoveAt(n);
            n--;
        }
    }

    public void ShowRadarAreaIcon(bool show)
    {
        for (int i = 0; i < this.radarAreaIconList.Count; i++)
        {
            this.radarAreaIconList[i].SetActive(show);
        }
    }

    private const int EXTRA_RANGE = 20;

    public RawImage PanelMapImage;

    public RawImage AreamapImage;

    public Transform Panel_Large;

    public Transform Areamap;

    public Transform Panel_MiniMap;

    public Transform PanelMap;

    public Dropdown lineList;

    public Transform hoverTipsRoot;

    public Transform btnHide;

    public Transform btnPvp;

    public Transform btnDuoQi;

    public TextTip textTip;

    public Transform btnExitCopy;

    public Transform btnMail;

    public Transform newMailTips;

    public Transform minmaipTop;

    public Transform mapRoot;

    public Transform btnMap;

    public Button btnGm;

    public Image imgGmNewMsg;

    private Slider AreamapSlider;

    private Transform Root;

    private float uiWidth;

    private Dictionary<uint, List<NpcMapXmlData>> monsterNpcXmlDic;

    private bool isShowMap = true;

    public bool IsOnCurrAreamap;

    private Text CnMapName;

    private Text EnMapName;

    private Transform transBattlePanel;

    private Text txtBattleTimeCooldown;

    private Text txtScore1;

    private Text txtScore2;

    private GameObject objBall1;

    private GameObject objBall2;

    private bool isAreaMapHovering;

    private List<GameObject> reliveAreaIconList = new List<GameObject>();

    private List<GameObject> radarAreaIconList = new List<GameObject>();

    private List<UI_Map.NpcBtn> NpcBtnList = new List<UI_Map.NpcBtn>();

    private List<GameObject> bossIcons = new List<GameObject>();

    private List<GameObject> selMonsterList;

    private GameObject clickMonsterBtn;

    private DateTime clickMonsterTime;

    private UI_Map.NpcBtn clickBtn;

    private DateTime clickTime;

    private UI_Map.NpcBtnIComparer mNpcBtnIComparer = new UI_Map.NpcBtnIComparer();

    private int mBattleCooldownSecond;

    private class NpcBtn
    {
        public void SetObjNpcBtn(GameObject obj, GameObject org)
        {
            this.Root = obj;
            if (this.Root == null)
            {
                return;
            }
            try
            {
                UGUICopyTool.ImageCopy(obj, org, "sp_on", null);
                UGUICopyTool.ImageCopy(obj, org, "sp_back", null);
                this.mImg = UGUICopyTool.ImageCopy(obj, org, "img_icon", this.Msprite);
                this.mText = UGUICopyTool.TextCopy(obj, org, "txt_name");
                this.mTextFunction = UGUICopyTool.TextCopy(obj, org, "txt_function");
                this.OnSelectGg = this.Root.transform.Find("sp_on").gameObject;
            }
            catch (Exception arg)
            {
                FFDebug.LogWarning(this, "SetObjNpcBtn : " + arg);
            }
            if (this.mText != null)
            {
                this.mText.text = this.MtextStr;
            }
            else
            {
                FFDebug.LogWarning(this, "SetObjNpcBtn mText  null");
            }
            if (this.mTextFunction != null)
            {
                this.mTextFunction.text = this.MtextFunctionStr;
            }
            else
            {
                FFDebug.LogWarning(this, "SetObjNpcBtn mTextFunction  null");
            }
            Button component = this.Root.GetComponent<Button>();
            if (component != null)
            {
                component.onClick.AddListener(delegate ()
                {
                    if (this.OnKick != null)
                    {
                        this.OnKick(this);
                    }
                });
            }
            if (this.mImg != null)
            {
                this.mImg.sprite = this.Msprite;
                this.mImg.color = this.MspriteColor;
            }
            else
            {
                FFDebug.LogWarning(this, "SetObjNpcBtn mImg  null");
            }
            if (this.OnSelectGg != null)
            {
                this.OnSelectGg.SetActive(this.IsSelect);
            }
        }

        public void SetData(MapItem mItem)
        {
            if (mItem == null)
            {
                return;
            }
            this.PriorityByType = 99999;
            this.Msprite = mItem.GetImageSprite();
            this.MspriteColor = mItem.GetImageColor();
            if (mItem.OwnerChar != null && mItem.OwnerChar.EID.Etype == CharactorType.NPC)
            {
                Npc npc = mItem.OwnerChar as Npc;
                this.mBaseid = npc.NpcData.MapNpcData.baseid;
                LuaTable configTable = LuaConfigManager.GetConfigTable("npc_data", (ulong)this.mBaseid);
                this.MtextStr = configTable.GetField_String("name");
                UIMapController controller = ControllerManager.Instance.GetController<UIMapController>();
                if (controller != null)
                {
                    LuaTable npcIconuimapinfo = controller.GetNpcIconuimapinfo(npc);
                    if (npcIconuimapinfo != null)
                    {
                        this.MtextFunctionStr = npcIconuimapinfo.GetField_String("Nickname");
                        ulong mapId = (ulong)ManagerCenter.Instance.GetManager<GameScene>().CurrentSceneData.mapID();
                        uint baseid = npc.NpcData.MapNpcData.baseid;
                        this.PathWayId = controller.GetPathWayId(mapId, baseid);
                        mItem.mPathWayId = this.PathWayId;
                        this.PriorityByNpc = (int)npcIconuimapinfo.GetField_Uint("id");
                        uint field_Uint = npcIconuimapinfo.GetField_Uint("Order");
                        if (mItem.currPriority <= 0)
                        {
                            mItem.currPriority = (int)(field_Uint * uint.MaxValue);
                        }
                    }
                }
                this.PriorityByType = mItem.currPriority;
                this.mId = mItem.MID;
            }
        }

        public void Dispose()
        {
            if (this.Root != null)
            {
                UnityEngine.Object.Destroy(this.Root);
            }
        }

        public void SetSelect(bool isSelect)
        {
            if (isSelect == this.IsSelect)
            {
                return;
            }
            this.IsSelect = isSelect;
            if (this.OnSelectGg != null)
            {
                this.OnSelectGg.SetActive(this.IsSelect);
            }
        }

        private GameObject Root;

        public Image mImg;

        public Text mText;

        public Text mTextFunction;

        public Sprite Msprite;

        public Color MspriteColor;

        public string MtextStr;

        public string MtextFunctionStr;

        public uint PathWayId;

        public Action<UI_Map.NpcBtn> OnKick;

        public bool IsSelect;

        public int PriorityByType;

        public int PriorityByNpc;

        public GameObject OnSelectGg;

        public string mId;

        public uint mBaseid;
    }

    private class NpcBtnIComparer : IComparer<UI_Map.NpcBtn>
    {
        public int Compare(UI_Map.NpcBtn x, UI_Map.NpcBtn y)
        {
            if (x.PriorityByType != y.PriorityByType)
            {
                return y.PriorityByType - x.PriorityByType;
            }
            UIMapController controller = ControllerManager.Instance.GetController<UIMapController>();
            if (controller != null)
            {
                return controller.CompreName(x.MtextStr, y.MtextStr);
            }
            return 0;
        }
    }

    private class MapLoctionNode
    {
        public MapLoctionNode(Transform root)
        {
            this.node = root;
            this.Name = this.node.name;
            this.img_location = this.node.Find("img_location");
            this.img_point = this.node.Find("img_point");
            UIEventListener.Get(this.node.gameObject).onClick = delegate (PointerEventData data)
            {
                if (this.OnClick != null)
                {
                    this.OnClick(this);
                }
            };
        }

        public void AddConfig(LuaTable config)
        {
            this.ConfigList.Add(config);
            if (config.GetField_Int("mainmap") == 1)
            {
                this.MainConfig = config;
            }
        }

        public bool IsCurrMapNode()
        {
            for (int i = 0; i < this.ConfigList.Count; i++)
            {
                if (this.ConfigList[i].GetField_Uint("mapID") == ManagerCenter.Instance.GetManager<GameScene>().CurrentSceneData.mapID())
                {
                    return true;
                }
            }
            return false;
        }

        public void ShowOrHideOnLoction(bool show)
        {
            this.img_point.gameObject.SetActive(show);
            this.img_location.gameObject.SetActive(show);
        }

        public string Name;

        public Transform node;

        public List<LuaTable> ConfigList = new List<LuaTable>();

        public LuaTable MainConfig;

        private Transform img_location;

        private Transform img_point;

        public Action<UI_Map.MapLoctionNode> OnClick;
    }
}
