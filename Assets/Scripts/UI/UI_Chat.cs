using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Chat;
using Framework.Managers;
using Game.Scene;
using Team;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Chat
{
    private ChatControl mChatControl
    {
        get
        {
            return ControllerManager.Instance.GetController<ChatControl>();
        }
    }

    public int mCurTabIndex
    {
        get
        {
            return this._mCurTabIndex;
        }
        set
        {
            if (this._mCurTabIndex != value)
            {
                if (this._mCurTabIndex != -1)
                {
                    this.btn_topitems[this._mCurTabIndex].GetComponent<Image>().sprite.name = "tab_us_2";
                }
                this._mCurTabIndex = value;
                this.btn_topitems[this._mCurTabIndex].GetComponent<Image>().sprite.name = "tab_s_2";
            }
        }
    }

    private void AddRevmoeTabDataListDo()
    {
        if (this.allChannelChatData == null)
        {
            this.allChannelChatData = new Dictionary<int, List<ChatData>>();
        }
        for (int i = 0; i < this.mTabDataList.Count; i++)
        {
            if (!this.allChannelChatData.ContainsKey(i))
            {
                this.allChannelChatData.Add(i, new List<ChatData>());
            }
        }
        List<int> list = new List<int>();
        foreach (int num in this.allChannelChatData.Keys)
        {
            bool flag = false;
            for (int j = 0; j < this.mTabDataList.Count; j++)
            {
                if (j == num)
                {
                    flag = true;
                    break;
                }
            }
            if (!flag)
            {
                list.Add(num);
            }
        }
        for (int k = 0; k < list.Count; k++)
        {
            this.allChannelChatData.RemoveAt(list[k]);
        }
    }

    public void Init(Transform root)
    {
        this.transRoot = root.Find("Panel");
        this.masks[0] = this.transRoot.Find("RightMousePanel/Mask").gameObject;
        this.masks[1] = root.parent.Find("TabPageSettingPanel/Mask").gameObject;
        this.masks[2] = this.transRoot.Find("ChannelPanel/Mask").gameObject;
        this.obj_topmenu_container = this.transRoot.Find("TopMenu").gameObject;
        this.obj_rightmouse_container = this.transRoot.Find("RightMousePanel").gameObject;
        this.obj_tabpagesetting_container = root.parent.Find("TabPageSettingPanel").gameObject;
        this.obj_inputchannelsel_container = this.transRoot.Find("ChannelPanel").gameObject;
        this.btn_inputchannel = this.transRoot.Find("InputChannelSelButton").gameObject;
        this.obj_special_container = this.transRoot.Find("SpecialPanel").gameObject;
        this.btn_special = this.transRoot.Find("SpecialButton").gameObject;
        this.btn_send = this.transRoot.Find("SendButton").gameObject;
        this.lbl_input = this.transRoot.Find("InputField").gameObject.GetComponent<InputField>();
        this.lbl_input.onValueChanged.AddListener(new UnityAction<string>(this.lbl_input_valuechanged));
        this.lbl_content = this.transRoot.Find("ScrollRect/Panel/Text").gameObject.GetComponent<Text>();
        this.sr_content = this.transRoot.Find("ScrollRect").gameObject.GetComponent<ScrollRect>();
        this.btn_clear = this.transRoot.Find("SpecialPanel/ClearButton").gameObject;
        this.lbl_content.text = string.Empty;
        (this.lbl_content as LinkImageText).onCallback = delegate (string name, PointerEventData eventData)
        {
            name = ((name.Length <= 2) ? name : name.Substring(1, name.Length - 2));
            if (MainPlayer.Self.OtherPlayerData.MapUserData.name == name)
            {
                FFDebug.LogError(this, "玩家不能和自己互动");
                return;
            }
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                ulong charId = this.GetCharIdByNameInChatMsg(name);
                if (charId == 0UL)
                {
                    FFDebug.LogError(this, "通过玩家name在当前聊天记录中 未找到玩家id");
                    return;
                }
                CommonTools.SetSecondPanelPos(eventData);
                UIManager.Instance.ShowUI<UI_TeamSecondary>("UI_TeamSecondary", delegate ()
                {
                    TeamController controller = ControllerManager.Instance.GetController<TeamController>();
                    Memember memember = controller.GetTeamMememberInfo(charId);
                    if (memember == null)
                    {
                        memember = new Memember();
                        memember.mememberid = charId.ToString();
                        memember.name = name;
                    }
                    UIManager.GetUIObject<UI_TeamSecondary>().Initilize(memember, eventData, false);
                }, UIManager.ParentType.CommonUI, false);
            }
            else if (eventData.button == PointerEventData.InputButton.Left)
            {
                this.SecritChat(name);
            }
        };
        this.obj_iconitem = root.parent.Find("Panel_expression/Scroll View/Viewport/Content/Image").gameObject;
        this.obj_iconitem.GetComponent<Image>().material = null;
        this.obj_iconitem.SetActive(false);
        this.obj_face_container = root.parent.Find("Panel_expression").gameObject;
        ManagerCenter.Instance.GetManager<EscManager>().RegisterSecondPanel(this.obj_face_container.transform);
        this.obj_face_container.transform.position = this.transRoot.Find("ExpressionButton").position;
        UIEventListener.Get(this.obj_face_container.FindChild("tab/0").gameObject).onClick = new UIEventListener.VoidDelegate(this.btn_face_onclick);
        UIEventListener.Get(this.obj_face_container.FindChild("tab/1").gameObject).onClick = new UIEventListener.VoidDelegate(this.btn_face_onclick);
        UIEventListener.Get(this.obj_face_container.FindChild("tab/2").gameObject).onClick = new UIEventListener.VoidDelegate(this.btn_face_onclick);
        this.obj_face_container.FindChild("tab/1").gameObject.SetActive(false);
        this.obj_face_container.FindChild("tab/2").gameObject.SetActive(false);
        this.ExpressionBtn = this.transRoot.Find("ExpressionButton").gameObject;
        UIEventListener.Get(this.ExpressionBtn).onClick = new UIEventListener.VoidDelegate(this.btn_icon_on_click);
        Transform transform = this.transRoot.Find("SpecialPanel/LinkButton");
        if (transform != null)
        {
            this.btn_mypos_send = transform.GetComponent<Button>();
            this.btn_mypos_send.onClick.RemoveAllListeners();
            this.btn_mypos_send.onClick.AddListener(new UnityAction(this.btn_mypos_send_onclick));
        }
        Sprite sprite = this.lbl_input.GetComponent<Image>().sprite;
        Vector4 border = new Vector4(8f, 8f, 8f, 8f);
        this.lbl_input.GetComponent<Image>().sprite = Sprite.Create(sprite.texture, sprite.rect, sprite.pivot, 100f, 0U, SpriteMeshType.Tight, border);
        DragArea dragArea = root.Find("DragArea").gameObject.AddComponent<DragArea>();
        dragArea.bg = root.parent.Find("DragAreaBg").gameObject;
        dragArea.canDragBg = root.parent.Find("CanDragArea").gameObject;
        dragArea.Initilize(null);
        this.OnInit();
    }

    public char onUserInput(string text, int charIndex, char addedChar)
    {
        if (addedChar == '<' || addedChar == '>')
        {
            return ' ';
        }
        return addedChar;
    }

    public void OnDestroy()
    {
    }

    private void DragAreaFinish()
    {
    }

    private void OnInit()
    {
        for (int i = 0; i < this.masks.Length; i++)
        {
            UIEventListener.Get(this.masks[i]).onClick = new UIEventListener.VoidDelegate(this.btn_mask_on_click);
        }
        for (int j = 0; j < this.obj_topmenu_container.transform.childCount - 1; j++)
        {
            GameObject gameObject = this.obj_topmenu_container.transform.GetChild(j).gameObject;
            UIEventListener.Get(gameObject).onClick = new UIEventListener.VoidDelegate(this.btn_receivechannelsel_on_click);
            this.btn_topitems[j] = gameObject;
        }
        UIEventListener.Get(this.obj_topmenu_container.transform.Find("AddButton").gameObject).onClick = new UIEventListener.VoidDelegate(this.btn_addtgl_on_click);
        Transform transform = this.obj_rightmouse_container.transform;
        this.btn_deltab = transform.Find("DelButton").gameObject;
        this.btn_modifytab = transform.Find("ModifyButton").gameObject;
        UIEventListener.Get(this.btn_deltab).onClick = new UIEventListener.VoidDelegate(this.btn_delbutton_on_click);
        UIEventListener.Get(this.btn_modifytab).onClick = new UIEventListener.VoidDelegate(this.btn_modifybutton_on_click);
        Transform transform2 = this.obj_tabpagesetting_container.transform;
        this.btn_modifysure = transform2.Find("ModifyButton").gameObject;
        UIEventListener.Get(transform2.Find("CancelButton").gameObject).onClick = new UIEventListener.VoidDelegate(this.btn_mask_on_click);
        UIEventListener.Get(transform2.Find("CreateButton").gameObject).onClick = new UIEventListener.VoidDelegate(this.btn_create_on_click);
        UIEventListener.Get(this.btn_modifysure).onClick = new UIEventListener.VoidDelegate(this.btn_modify_on_click);
        for (int k = 0; k < this.obj_inputchannelsel_container.transform.Find("LayoutGroup").childCount; k++)
        {
            UIEventListener.Get(this.obj_inputchannelsel_container.transform.Find("LayoutGroup").GetChild(k).gameObject).onClick = new UIEventListener.VoidDelegate(this.btn_inputchannelsel_on_click);
        }
        UIEventListener.Get(this.btn_inputchannel).onClick = new UIEventListener.VoidDelegate(this.btn_inputchannelsel_on_click);
        for (int l = 0; l < this.obj_special_container.transform.childCount; l++)
        {
            UIEventListener.Get(this.obj_special_container.transform.GetChild(l).gameObject).onClick = new UIEventListener.VoidDelegate(this.btn_special_on_click);
        }
        UIEventListener.Get(this.btn_special).onClick = new UIEventListener.VoidDelegate(this.btn_special_on_click);
        UIEventListener.Get(this.btn_send).onClick = new UIEventListener.VoidDelegate(this.btn_send_on_click);
        UIEventListener.Get(this.btn_clear).onClick = new UIEventListener.VoidDelegate(this.btn_clear_on_click);
        this.isInputChannelSelContainerShow = false;
        this.mCurInputChannelType = ChannelType.ChannelType_Scene;
        this.mCurTabIndex = 0;
        this.SetupInputChannelPanel();
        ServerStorageManager.Instance.GetData(ServerStorageKey.ChatTab, 0U);
    }

    private void lbl_input_valuechanged(string content)
    {
        string text = KeyWordFilter.ChatFilter(this.lbl_input.text);
        if (text != this.lbl_input.text)
        {
            this.lbl_input.text = text;
        }
    }

    private TabData StorageContentToTabData(string itemStr)
    {
        TabData tabData = new TabData();
        string[] array = itemStr.Split(new string[]
        {
            "_"
        }, StringSplitOptions.RemoveEmptyEntries);
        tabData.name = array[0];
        tabData.chennelIdList = new List<ChannelType>();
        if (array.Length > 1)
        {
            string[] array2 = array[1].Split(new string[]
            {
                "-"
            }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < array2.Length; i++)
            {
                tabData.chennelIdList.Add((ChannelType)int.Parse(array2[i]));
            }
        }
        return tabData;
    }

    public void SetupTabPanelByStorageData(string storageTabContent)
    {
        this.mTabDataList = new List<TabData>();
        if (storageTabContent == string.Empty)
        {
            this.mTabDataList.Add(new TabData
            {
                name = "综合",
                chennelIdList = new List<ChannelType>
                {
                    ChannelType.ChannelType_Camp,
                    ChannelType.ChannelType_Guild,
                    ChannelType.ChannelType_Secret,
                    ChannelType.ChannelType_Scene,
                    ChannelType.ChannelType_Sys,
                    ChannelType.ChannelType_Team,
                    ChannelType.ChannelType_World
                }
            });
            this.mTabDataList.Add(new TabData
            {
                name = "个人",
                chennelIdList = new List<ChannelType>
                {
                    ChannelType.ChannelType_Team,
                    ChannelType.ChannelType_Secret
                }
            });
        }
        else
        {
            string[] array = storageTabContent.Split(new string[]
            {
                ","
            }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < array.Length; i++)
            {
                this.mTabDataList.Add(this.StorageContentToTabData(array[i]));
            }
        }
        this.AddRevmoeTabDataListDo();
        this.SetupTopPanel();
    }

    public bool CurChannelEqual(ChannelType _type)
    {
        return this.mTabDataList != null && this.mTabDataList[this.mCurTabIndex].chennelIdList.Contains(_type);
    }

    private void SetupTopPanel()
    {
        for (int i = 0; i < this.btn_topitems.Length; i++)
        {
            this.SetupTopItem((this.mTabDataList.Count <= i) ? null : this.mTabDataList[i], this.btn_topitems[i]);
        }
        this.obj_topmenu_container.transform.Find("AddButton").gameObject.SetActive(this.mTabDataList.Count < 5);
    }

    private void SetupTopItem(TabData tabData, GameObject objItem)
    {
        if (tabData == null)
        {
            objItem.SetActive(false);
        }
        else
        {
            objItem.transform.Find("Text").GetComponent<Text>().text = tabData.name;
            objItem.name = this.mTabDataList.IndexOf(tabData).ToString();
            objItem.SetActive(true);
        }
    }

    private void btn_receivechannelsel_on_click(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            this.mCurTabIndex = int.Parse(eventData.pointerPress.name);
            this.SetupMsgPanel();
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            this.btn_deltab.SetActive(int.Parse(eventData.pointerPress.name) >= 2);
            UnityEngine.Object @object = this.btn_deltab;
            string name = eventData.pointerPress.name;
            this.btn_modifytab.name = name;
            @object.name = name;
            this.obj_rightmouse_container.transform.position = eventData.pointerPress.transform.position;
            this.obj_rightmouse_container.gameObject.SetActive(true);
        }
    }

    private List<ChatData> GetChannelData()
    {
        TabData tabData = this.mTabDataList[this.mCurTabIndex];
        List<ChatData> list = this.allChannelChatData[this.mCurTabIndex];
        list.Sort((ChatData a, ChatData b) => (int)(a.chattime - b.chattime));
        return list;
    }

    private void SetupMsgPanel()
    {
        List<ChatData> channelData = this.GetChannelData();
        StringBuilder stringBuilder = new StringBuilder();
        for (int i = 0; i < channelData.Count; i++)
        {
            string chatTextModel = this.GetChatTextModel(channelData[i]);
            stringBuilder.Append(chatTextModel + "\r\n");
        }
        this.lbl_content.text = stringBuilder.ToString();
    }

    private void btn_mask_on_click(PointerEventData eventData)
    {
        this.obj_rightmouse_container.SetActive(false);
        this.obj_tabpagesetting_container.SetActive(false);
        this.obj_inputchannelsel_container.SetActive(false);
        this.isInputChannelSelContainerShow = false;
    }

    private void btn_delbutton_on_click(PointerEventData eventData)
    {
        int num = int.Parse(eventData.pointerPress.name);
        if (num >= 2)
        {
            this.mTabDataList.RemoveAt(num);
            this.mCurTabIndex = 0;
            this.SetupTopPanel();
            this.btn_mask_on_click(null);
            this.AddUpdateTabStorage();
        }
    }

    private void btn_modifybutton_on_click(PointerEventData eventData)
    {
        this.btn_mask_on_click(null);
        this.btn_modifysure.name = eventData.pointerPress.name;
        this.SetupTabPageSettingPanel(this.mTabDataList[int.Parse(eventData.pointerPress.name)]);
        this.obj_tabpagesetting_container.SetActive(true);
    }

    private void btn_addtgl_on_click(PointerEventData eventData)
    {
        if (this.mTabDataList.Count < 5)
        {
            this.SetupTabPageSettingPanel(null);
            this.obj_tabpagesetting_container.SetActive(true);
        }
    }

    private void SetupTabPageSettingPanel(TabData tabData)
    {
        Transform transform = this.obj_tabpagesetting_container.transform;
        InputField component = transform.Find("InputField").GetComponent<InputField>();
        GameObject gameObject = transform.Find("CreateButton").gameObject;
        Transform transform2 = transform.Find("ChannelGrid");
        if (tabData == null)
        {
            component.text = string.Empty;
            gameObject.SetActive(true);
            this.btn_modifysure.SetActive(false);
            component.interactable = true;
        }
        else
        {
            component.text = tabData.name;
            gameObject.SetActive(false);
            this.btn_modifysure.SetActive(true);
            component.interactable = (int.Parse(this.btn_modifysure.name) >= 2);
        }
        int offset = 0;
        Func<int, ChatChannelData> getNext = null;
        getNext = delegate (int i)
        {
            if (i + offset >= this.mChatControl.ChatChannelDataList.Count)
            {
                return null;
            }
            ChatChannelData chatChannelData2 = this.mChatControl.ChatChannelDataList[i + offset];
            if (chatChannelData2.ChannelType == ChannelType.ChannelType_Private || chatChannelData2.ChannelType == ChannelType.ChannelType_GmTool || chatChannelData2.ChannelType == ChannelType.ChannelType_Moba)
            {
                offset++;
                return getNext(i);
            }
            return chatChannelData2;
        };
        for (int j = 0; j < transform2.childCount; j++)
        {
            Transform child = transform2.GetChild(j);
            if (j < 7)
            {
                child.gameObject.SetActive(true);
                ChatChannelData chatChannelData = getNext(j);
                child.Find("Label").GetComponent<Text>().text = chatChannelData.ChannelConfig.GetField_String("name");
                child.gameObject.GetComponent<Toggle>().isOn = ((tabData != null && this.mTabDataList[this.mCurTabIndex].chennelIdList.Contains(chatChannelData.ChannelType)) || tabData == null);
                UnityEngine.Object @object = child;
                int channelType = (int)chatChannelData.ChannelType;
                @object.name = channelType.ToString();
            }
            else
            {
                child.gameObject.GetComponent<Toggle>().isOn = false;
                child.gameObject.SetActive(false);
            }
        }
    }

    private TabData GetCurrentSettingPageInputData()
    {
        Transform transform = this.obj_tabpagesetting_container.transform;
        InputField component = transform.Find("InputField").GetComponent<InputField>();
        TabData tabData = new TabData();
        tabData.name = component.text;
        tabData.chennelIdList = new List<ChannelType>();
        for (int i = 0; i < this.obj_tabpagesetting_container.transform.Find("ChannelGrid").childCount; i++)
        {
            GameObject gameObject = this.obj_tabpagesetting_container.transform.Find("ChannelGrid").GetChild(i).gameObject;
            if (gameObject.GetComponent<Toggle>().isOn)
            {
                tabData.chennelIdList.Add((ChannelType)int.Parse(gameObject.name));
            }
        }
        return tabData;
    }

    private bool isNameLengthTooLong()
    {
        Transform transform = this.obj_tabpagesetting_container.transform;
        if (transform.Find("InputField").GetComponent<InputField>().text.Length > 4)
        {
            TipsWindow.ShowWindow("名称最多4个字符");
            return true;
        }
        return false;
    }

    private void btn_create_on_click(PointerEventData eventData)
    {
        if (this.isNameLengthTooLong())
        {
            return;
        }
        string text = this.obj_tabpagesetting_container.FindChild("InputField").GetComponent<InputField>().text;
        if (string.Compare(text, KeyWordFilter.ChatFilter(text)) != 0)
        {
            TipsWindow.ShowWindow(TipsType.HAVE_SENSITIVE, null);
            return;
        }
        TabData currentSettingPageInputData = this.GetCurrentSettingPageInputData();
        if (!string.IsNullOrEmpty(currentSettingPageInputData.name))
        {
            this.mTabDataList.Add(currentSettingPageInputData);
            this.SetupTopPanel();
            this.btn_mask_on_click(null);
            this.AddUpdateTabStorage();
        }
        else
        {
            Debug.LogWarning("name can't be null");
        }
    }

    private void btn_modify_on_click(PointerEventData eventData)
    {
        if (this.isNameLengthTooLong())
        {
            return;
        }
        int num = int.Parse(eventData.pointerPress.name);
        List<ChatData> value = this.allChannelChatData[num];
        this.mTabDataList[num] = this.GetCurrentSettingPageInputData();
        this.SetupTopPanel();
        this.btn_mask_on_click(null);
        this.SetupMsgPanel();
        this.AddUpdateTabStorage();
        this.allChannelChatData[num] = value;
    }

    private void AddUpdateTabStorage()
    {
        string text = string.Empty;
        for (int i = 0; i < this.mTabDataList.Count; i++)
        {
            TabData tabData = this.mTabDataList[i];
            text += tabData.name;
            for (int j = 0; j < tabData.chennelIdList.Count; j++)
            {
                string text2 = text;
                text = string.Concat(new object[]
                {
                    text2,
                    (j != 0) ? string.Empty : "_",
                    (int)tabData.chennelIdList[j],
                    (j != tabData.chennelIdList.Count - 1) ? "-" : ","
                });
            }
            if (tabData.chennelIdList.Count == 0)
            {
                text += ",";
            }
        }
        ServerStorageManager.Instance.AddUpdateData(ServerStorageKey.ChatTab, text, 0U);
    }

    private void SetupInputChannelPanel()
    {
        int offset = 0;
        Func<int, ChatChannelData> getNext = null;
        getNext = delegate (int i)
        {
            if (i + offset >= this.mChatControl.ChatChannelDataList.Count)
            {
                return null;
            }
            ChatChannelData chatChannelData2 = this.mChatControl.ChatChannelDataList[i + offset];
            if (chatChannelData2.ChannelType == ChannelType.ChannelType_Sys || chatChannelData2.ChannelType == ChannelType.ChannelType_Private || chatChannelData2.ChannelType == ChannelType.ChannelType_GmTool || chatChannelData2.ChannelType == ChannelType.ChannelType_Moba)
            {
                offset++;
                return getNext(i);
            }
            return chatChannelData2;
        };
        for (int j = 0; j < this.obj_inputchannelsel_container.transform.Find("LayoutGroup").childCount; j++)
        {
            Transform child = this.obj_inputchannelsel_container.transform.Find("LayoutGroup").GetChild(j);
            ChatChannelData chatChannelData = getNext(j);
            if (chatChannelData != null)
            {
                if (chatChannelData.ChannelType != this.mCurInputChannelType)
                {
                    if (chatChannelData.ChannelConfig == null)
                    {
                        return;
                    }
                    string field_String = chatChannelData.ChannelConfig.GetField_String("name_short");
                    if (child.Find("Text_g") != null)
                    {
                        child.Find("Text_g").GetComponent<Text>().text = field_String;
                    }
                    if (child.Find("Text_s") != null)
                    {
                        child.Find("Text_s").GetComponent<Text>().text = field_String;
                    }
                    if (child.Find("Text_p") != null)
                    {
                        child.Find("Text_p").GetComponent<Text>().text = field_String;
                    }
                    UnityEngine.Object gameObject = child.gameObject;
                    int channelType = (int)chatChannelData.ChannelType;
                    gameObject.name = channelType.ToString();
                    child.gameObject.SetActive(true);
                }
                else
                {
                    this.btn_inputchannel.transform.Find("Text").GetComponent<Text>().text = chatChannelData.ChannelConfig.GetField_String("name_short");
                    UnityEngine.Object @object = this.btn_inputchannel;
                    int channelType2 = (int)chatChannelData.ChannelType;
                    @object.name = channelType2.ToString();
                    this.btn_inputchannel.transform.Find("Text").gameObject.SetActive(true);
                    child.gameObject.SetActive(false);
                }
                if (chatChannelData.ChannelType == ChannelType.ChannelType_Scene && this.mCurInputChannelType != ChannelType.ChannelType_Scene)
                {
                    child.gameObject.SetActive(!UIManager.GetUIObject<UI_MainView>().IsInBattleScene());
                }
            }
            else
            {
                child.gameObject.SetActive(false);
            }
        }
        this.obj_inputchannelsel_container.SetActive(this.isInputChannelSelContainerShow);
    }

    private void btn_inputchannelsel_on_click(PointerEventData eventData)
    {
        this.SwitchInputChannel((ChannelType)int.Parse(eventData.pointerPress.name));
        this.isInputChannelSelContainerShow = !this.isInputChannelSelContainerShow;
        this.obj_inputchannelsel_container.SetActive(this.isInputChannelSelContainerShow);
    }

    public void SwitchInputChannel(ChannelType type)
    {
        this.mCurInputChannelType = type;
        this.SetupInputChannelPanel();
    }

    private void btn_special_on_click(PointerEventData eventData)
    {
    }

    private string handleContent(string content)
    {
        Regex regex = new Regex("<quad.+?/>");
        string text = content;
        MatchCollection matchCollection = regex.Matches(content);
        List<string> list = new List<string>();
        for (int i = 0; i < matchCollection.Count; i++)
        {
            list.Add(matchCollection[i].Groups[0].Value);
        }
        if (matchCollection.Count > 0)
        {
            for (int j = 0; j < matchCollection.Count; j++)
            {
                string value = matchCollection[j].Groups[0].Value;
                string[] array = content.Split(new string[]
                {
                    value
                }, StringSplitOptions.RemoveEmptyEntries);
                if (array.Length > 0)
                {
                    for (int k = 0; k < array.Length; k++)
                    {
                        string text2 = array[k];
                        if (!list.Contains(text2))
                        {
                            if (array[k] != "<" && array[k] != ">")
                            {
                                text2 = text2.Replace('<', '*');
                                text2 = text2.Replace('>', '*');
                                text = text.Replace(array[k], text2);
                            }
                        }
                    }
                }
            }
        }
        else
        {
            text = text.Replace('<', '*');
            text = text.Replace('>', '*');
        }
        return text;
    }

    public void btn_send_on_click(PointerEventData eventData)
    {
        string text = this.lbl_input.text;
        text = this.handleContent(text);
        if (string.IsNullOrEmpty(text))
        {
            return;
        }
        LocalGMController controller = ControllerManager.Instance.GetController<LocalGMController>();
        if (controller.IsLocalGmText(text))
        {
            controller.AddCachedCommand(text);
            this.lbl_input.text = string.Empty;
            return;
        }
        this.mChatControl.SendChat(this.mCurInputChannelType, text, 0U, null);
        this.lbl_input.text = string.Empty;
        this.mCacheSendMsg.Add(text);
        this.mCacheSendMsgIndex = this.mCacheSendMsg.Count;
    }

    private void btn_clear_on_click(PointerEventData eventData)
    {
        this.lbl_content.text = string.Empty;
        this.allChannelChatData[this.mCurTabIndex].Clear();
    }

    public void AddChatItem(ChatData data)
    {
        if (ControllerManager.Instance.GetController<FriendControllerNew>().mBlackDic.ContainsKey(data.charid))
        {
            return;
        }
        if (this.allChannelChatData == null)
        {
            return;
        }
        foreach (int num in this.allChannelChatData.Keys)
        {
            if (this.mTabDataList[num].chennelIdList.Contains((ChannelType)data.channel))
            {
                this.allChannelChatData[num].Add(data);
                if (this.allChannelChatData[num].Count > 60)
                {
                    this.allChannelChatData[num].RemoveAt(0);
                }
            }
        }
        if (this.mTabDataList[this.mCurTabIndex].chennelIdList.Contains((ChannelType)data.channel))
        {
            string chatTextModel = this.GetChatTextModel(data);
            if (!string.IsNullOrEmpty(this.lbl_content.text))
            {
                string text = this.lbl_content.text;
                MatchCollection matchCollection = Regex.Matches(text, "\r\n");
                int count = matchCollection.Count;
                bool flag = count >= 60;
                if (flag)
                {
                    int num2 = text.IndexOf("\r\n");
                    string text2 = text.Substring(num2 + 2);
                    this.lbl_content.text = text2;
                }
            }
            Text text3 = this.lbl_content;
            text3.text = text3.text + chatTextModel.Replace("\r\n", string.Empty) + "\r\n";
        }
    }

    public ulong GetCharIdByNameInChatMsg(string name)
    {
        foreach (ChatData chatData in this.allChannelChatData[this.mCurTabIndex])
        {
            if (chatData.charname == name)
            {
                return chatData.charid;
            }
            if (chatData.toname == name)
            {
                return chatData.tocharid;
            }
        }
        return 0UL;
    }

    private ScrollRect scrollRectChatContent
    {
        get
        {
            if (this.m_scrollRectChatContent == null)
            {
                this.m_scrollRectChatContent = this.lbl_content.transform.parent.GetComponent<ScrollRect>();
            }
            return this.m_scrollRectChatContent;
        }
    }

    private string GetChatTextModel(ChatData data)
    {
        ChannelType channel = (ChannelType)data.channel;
        if (channel != ChannelType.ChannelType_Secret)
        {
            return string.Concat(new string[]
            {
                this.GetChannelTextColor((ChannelType)data.channel),
                "[",
                this.mChatControl.ChatChannelDatas[(ChannelType)data.channel].ChannelConfig.GetField_String("name"),
                "]",
                (data.channel != 1U) ? ((!string.IsNullOrEmpty(data.charname)) ? ("<size=14>[" + data.charname + "]</size>") : string.Empty) : string.Empty,
                ":",
                data.content,
                "</color>"
            });
        }
        return string.Concat(new string[]
        {
            this.GetChannelTextColor((ChannelType)data.channel),
            "[",
            this.mChatControl.ChatChannelDatas[(ChannelType)data.channel].ChannelConfig.GetField_String("name"),
            "]",
            (!(data.charname == MainPlayer.Self.OtherPlayerData.MapUserData.name)) ? ("<size=14>[" + data.charname + "]</size>对") : "你对",
            (!(data.toname == MainPlayer.Self.OtherPlayerData.MapUserData.name)) ? ("<size=14>[" + data.toname + "]</size>说") : "你说",
            ":",
            data.content,
            "</color>"
        });
    }

    private string GetChannelTextColor(ChannelType type)
    {
        string str = "<color=#";
        switch (type)
        {
            case ChannelType.ChannelType_None:
            case ChannelType.ChannelType_Sys:
                str += "ffc71e";
                goto IL_D8;
            case ChannelType.ChannelType_Team:
                str += "5fff55";
                goto IL_D8;
            case ChannelType.ChannelType_Guild:
                str += "d452ff";
                goto IL_D8;
            case ChannelType.ChannelType_Camp:
                str += "53b0ff";
                goto IL_D8;
            case ChannelType.ChannelType_World:
                str += "00ffff";
                goto IL_D8;
            case ChannelType.ChannelType_Scene:
                str += "ffffff";
                goto IL_D8;
            case ChannelType.ChannelType_Private:
                str += "ff4ee0";
                goto IL_D8;
            case ChannelType.ChannelType_Secret:
                str += "ff1cae";
                goto IL_D8;
        }
        str += "ffffff";
    IL_D8:
        return str + ">";
    }

    private void btn_face_onclick(PointerEventData eventData)
    {
        int pageIndex = int.Parse(eventData.pointerPress.gameObject.name) + 1;
        this.SetupIconPanel(pageIndex);
    }

    private void SetupIconPanel(int pageIndex)
    {
        UIManager.Instance.ClearListChildrens(this.obj_iconitem.transform.parent, 1);
        BiaoqingManager manager = ManagerCenter.Instance.GetManager<BiaoqingManager>();
        List<BiaoqingManager.ImageData> images = manager.GetImages(pageIndex);
        for (int i = 0; i < images.Count; i++)
        {
            string name = images[i].name;
            GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.obj_iconitem);
            gameObject.transform.SetParent(this.obj_iconitem.transform.parent);
            gameObject.transform.localScale = Vector3.one;
            gameObject.name = name;
            Image component = gameObject.GetComponent<Image>();
            CommonTools.SetFaceIcon(name, component);
            gameObject.SetActive(true);
            UIEventListener.Get(gameObject).onClick = new UIEventListener.VoidDelegate(this.btn_iconsel_on_click);
        }
    }

    private void btn_icon_on_click(PointerEventData eventData)
    {
        if (!this.obj_face_container.activeSelf)
        {
            this.SetupIconPanel(1);
            this.obj_face_container.SetActive(true);
        }
        else
        {
            this.obj_face_container.SetActive(false);
        }
    }

    private void btn_iconunsel_on_click(PointerEventData eventData)
    {
        this.obj_face_container.SetActive(false);
    }

    private void btn_iconsel_on_click(PointerEventData eventData)
    {
        InputField inputField = this.lbl_input;
        inputField.text = inputField.text + "<quad name=" + eventData.pointerPress.name + " size=12 width=1 />";
        this.obj_face_container.SetActive(false);
    }

    public void Update()
    {
        if (this.lbl_input != null && this.lbl_input.isFocused)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                this.SetShortcutMsg(false);
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                this.SetShortcutMsg(true);
            }
        }
    }

    private void SetShortcutMsg(bool next)
    {
        if (this.mCacheSendMsg.Count == 0)
        {
            return;
        }
        if (next)
        {
            this.mCacheSendMsgIndex++;
        }
        else
        {
            this.mCacheSendMsgIndex--;
        }
        if (this.mCacheSendMsgIndex < 0)
        {
            this.mCacheSendMsgIndex = this.mCacheSendMsg.Count - 1;
        }
        else if (this.mCacheSendMsgIndex > this.mCacheSendMsg.Count - 1)
        {
            this.mCacheSendMsgIndex = 0;
        }
        this.lbl_input.text = this.mCacheSendMsg[this.mCacheSendMsgIndex];
        this.lbl_input.MoveTextEnd(false);
    }

    private void btn_mypos_send_onclick()
    {
        string text = CommonTools.GetTextById(4016UL);
        string arg = string.Empty;
        GameScene manager = ManagerCenter.Instance.GetManager<GameScene>();
        if (manager != null)
        {
            arg = manager.CurrentSceneData.showName();
        }
        text = string.Format(text, arg, (int)MainPlayer.Self.CurrServerPos.x, (int)MainPlayer.Self.CurrServerPos.y);
        this.mChatControl.SendChat(this.mCurInputChannelType, text, 0U, null);
        this.mCacheSendMsg.Add(text);
        this.mCacheSendMsgIndex = this.mCacheSendMsg.Count;
    }

    public void SecritChat(string name)
    {
        this.lbl_input.text = string.Format("@{0} ", name);
        if (!this.lbl_input.isFocused)
        {
            this.lbl_input.ActivateInputField();
            this.SwitchInputChannel(ChannelType.ChannelType_Secret);
        }
        Scheduler.Instance.AddFrame(2U, false, delegate
        {
            this.lbl_input.MoveTextEnd(false);
        });
    }

    private const int TAB_NUM_LIMIT_MIN = 2;

    private const int TAB_NUM_LIMIT_MAX = 5;

    private const int CHANNEL_TYPE_NUM = 7;

    private const int TAB_CHAT_MSG_MAX_NUM = 60;

    public GameObject[] masks = new GameObject[3];

    public GameObject obj_topmenu_container;

    public GameObject[] btn_topitems = new GameObject[5];

    public GameObject obj_rightmouse_container;

    private GameObject btn_deltab;

    private GameObject btn_modifytab;

    public GameObject obj_tabpagesetting_container;

    private GameObject btn_modifysure;

    public GameObject obj_inputchannelsel_container;

    public GameObject btn_inputchannel;

    private bool isInputChannelSelContainerShow;

    private ChannelType mCurInputChannelType;

    public GameObject obj_special_container;

    public GameObject btn_special;

    public GameObject btn_send;

    public InputField lbl_input;

    public ScrollRect sr_content;

    public Text lbl_content;

    public GameObject btn_clear;

    public GameObject ExpressionBtn;

    private GameObject obj_iconitem;

    public GameObject obj_face_container;

    private Transform transRoot;

    private Button btn_mypos_send;

    private int _mCurTabIndex = -1;

    private List<TabData> mTabDataList;

    private Dictionary<int, List<ChatData>> allChannelChatData;

    private bool isScrollLastPage = true;

    private ScrollRect m_scrollRectChatContent;

    private List<string> mCacheSendMsg = new List<string>();

    private int mCacheSendMsgIndex;

    private enum TabDataState
    {
        Active,
        Hide
    }
}
