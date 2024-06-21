using System;
using System.Collections.Generic;
using Framework.Managers;
using relation;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_FriendNew : UIPanelBase
{
    private FriendControllerNew mController
    {
        get
        {
            return ControllerManager.Instance.GetController<FriendControllerNew>();
        }
    }

    public override void OnInit(Transform _root)
    {
        base.OnInit(_root);
        Transform transform = _root.Find("Main/Center/ToggleGroup/Panel_tab/ApplyToggle");
        this.tgl_apply = transform.GetComponent<Toggle>();
        this.obj_apply_tip = transform.Find("NumberTip").gameObject;
        this.tgl_apply.onValueChanged.AddListener(delegate (bool isOn)
        {
            if (isOn)
            {
                this.mTabType = FriendTabType.Apply;
            }
        });
        _root.Find("Main/Center/ToggleGroup/Panel_tab/FriendToggle").GetComponent<Toggle>().onValueChanged.AddListener(delegate (bool isOn)
        {
            if (isOn)
            {
                this.mTabType = FriendTabType.Friend;
            }
        });
        _root.Find("Main/Center/ToggleGroup/Panel_tab/RecentToggle").GetComponent<Toggle>().onValueChanged.AddListener(delegate (bool isOn)
        {
            if (isOn)
            {
                this.mTabType = FriendTabType.Recent;
            }
        });
        _root.Find("Main/Center/ToggleGroup/Panel_tab/BlackToggle").GetComponent<Toggle>().onValueChanged.AddListener(delegate (bool isOn)
        {
            if (isOn)
            {
                this.mTabType = FriendTabType.Black;
            }
        });
        Transform transform2 = _root.Find("Main/Center/ToggleGroup/Panel_tab/FriendToggle/NewMsgTag");
        Transform transform3 = _root.Find("Main/Center/ToggleGroup/Panel_tab/FriendToggle/NewFriendTag");
        if (transform2 != null)
        {
            this.obj_newmsgtag = transform2.gameObject;
        }
        if (transform3 != null)
        {
            this.obj_newfriendtag = transform3.gameObject;
        }
        Transform transform4 = _root.Find("Main/Head/CloseButton");
        transform4.gameObject.GetComponent<Button>().onClick.AddListener(delegate ()
        {
            UIManager.Instance.DeleteUI<UI_FriendNew>();
        });
        Transform transform5 = _root.Find("Main/Center/ContentPanel/FriendListPanel");
        this.obj_tabitem = transform5.Find("Center/ScrollRect/Content/Group").gameObject;
        this.obj_tabitem.SetActive(false);
        this.obj_tabitem.transform.Find("Head/ReNameButton").gameObject.SetActive(false);
        this.obj_tabitem.transform.Find("Head/DelGroupButton").gameObject.SetActive(false);
        this.obj_tabitem.FindChild("List/FriendItem").AddComponent<UI_FriendItem>();
        Transform transform6 = transform5.Find("Bottom");
        this.lbl_friend_num = transform6.Find("Number").GetComponent<Text>();
        Transform transform7 = transform6.Find("AddFriendButton");
        Transform transform8 = transform6.Find("AddGroupButton");
        this.transInputOpePanel = transform5.Find("AddFriendPanel");
        this.lbl_add_friendid = this.transInputOpePanel.Find("Panel/InputField").GetComponent<InputField>();
        transform7.GetComponent<Button>().onClick.AddListener(delegate ()
        {
            this.SetupShowOpePanel(UI_FriendNew.InputPageOpeType.AddFriend);
        });
        transform8.GetComponent<Button>().onClick.AddListener(delegate ()
        {
            this.SetupShowOpePanel(UI_FriendNew.InputPageOpeType.AddGroup);
        });
        this.transInputOpePanel.Find("Panel/OkButton").GetComponent<Button>().onClick.AddListener(delegate ()
        {
            if (string.Compare(this.lbl_add_friendid.text, KeyWordFilter.ChatFilter(this.lbl_add_friendid.text)) != 0)
            {
                TipsWindow.ShowWindow(TipsType.HAVE_SENSITIVE, null);
                return;
            }
            switch (this.mCurInputPageOpeType)
            {
                case UI_FriendNew.InputPageOpeType.AddFriend:
                    this.mController.mFriendNetWork.ReqSearchRelation(this.lbl_add_friendid.text, new FriendNetWork.SearchCb(this.AddFriendSearchCb));
                    break;
                case UI_FriendNew.InputPageOpeType.AddGroup:
                    this.mController.mFriendNetWork.ReqModifyPageName(this.lbl_add_friendid.text, string.Empty, 3U);
                    break;
                case UI_FriendNew.InputPageOpeType.SetSecName:
                    this.mController.mFriendNetWork.ReqModifyPageName(this.transInputOpePanel.name, this.lbl_add_friendid.text, 1U);
                    break;
                case UI_FriendNew.InputPageOpeType.SetNickName:
                    this.mController.mFriendNetWork.ReqChangeNickName(ulong.Parse(this.transInputOpePanel.name), this.lbl_add_friendid.text);
                    break;
            }
            this.transInputOpePanel.gameObject.SetActive(false);
        });
        this.transInputOpePanel.Find("Panel/CancelButton").GetComponent<Button>().onClick.AddListener(delegate ()
        {
            this.transInputOpePanel.gameObject.SetActive(false);
        });
        Transform transform9 = _root.Find("Main/Center/ContentPanel/FriendReqPanel");
        Transform transform10 = transform9.Find("Center/ScrollRect/List");
        this.obj_applyitem = transform10.Find("ApplyItem").gameObject;
        this.obj_applyitem.AddComponent<UI_FriendItem>();
        this.obj_applyitem.SetActive(false);
        GameObject gameObject = _root.Find("SecondMenu").gameObject;
        this.mUISecondMenu = gameObject.AddComponent<UI_FriendSecondMenu>();
        Transform transform11 = _root.Find("Main/Center/ContentPanel/BlackListPanel");
        this.obj_blackitem = transform11.Find("Center/ScrollRect/List/RecentItem").gameObject;
        this.obj_blackitem.AddComponent<UI_FriendItem>();
        this.obj_blackitem.SetActive(false);
        Transform transform12 = _root.Find("Main/Center/ContentPanel/RecentPanel");
        this.obj_recentitem = transform12.Find("Center/ScrollRect/List/RecentItem").gameObject;
        this.obj_recentitem.AddComponent<UI_FriendItem>();
        this.obj_recentitem.SetActive(false);
        this.btn_clear_apply = transform9.Find("Bottom/Button").gameObject;
        this.btn_clear_recent = transform12.Find("Bottom/Button").gameObject;
        this.btn_clear_black = transform11.Find("Bottom/Button").gameObject;
        UIEventListener.Get(this.btn_clear_apply).onClick = new UIEventListener.VoidDelegate(this.btn_commonclear_on_click);
        UIEventListener.Get(this.btn_clear_recent).onClick = new UIEventListener.VoidDelegate(this.btn_commonclear_on_click);
        UIEventListener.Get(this.btn_clear_black).onClick = new UIEventListener.VoidDelegate(this.btn_commonclear_on_click);
        this.lbl_search = transform5.Find("Top/InputField").GetComponent<InputField>();
        this.lbl_search.onValueChanged.AddListener(new UnityAction<string>(this.input_search_on_valuechanged));
        this.obj_center_container = transform5.Find("Center").gameObject;
        this.obj_centersearch_container = transform5.Find("CenterSearch").gameObject;
        this.obj_searchitem = transform5.Find("CenterSearch/ScrollRect/List/FriendItem").gameObject;
        this.obj_searchitem.AddComponent<UI_FriendItem>();
        this.obj_searchitem.SetActive(false);
        this.obj_notfoundtext = transform5.Find("CenterSearch/NotFoundText").gameObject;
        transform5.Find("Top/InputField/ClearButton").GetComponent<Button>().onClick.AddListener(delegate ()
        {
            this.lbl_search.text = string.Empty;
        });
        transform5.Find("Top/Toggle").GetComponent<Toggle>().onValueChanged.AddListener(delegate (bool isOn)
        {
            this.isSelectOnline = isOn;
            this.SetupFriendListPanel();
        });
        this.mController.mFriendNetWork.ReqAllFriendPage();
        this.mController.mFriendNetWork.ReqBlackList();
        this.SetupRecentListPanel();
        this.SetupApplyListPanel();
        Scheduler.Instance.AddUpdator(new Scheduler.OnScheduler(this.Update));
        if (this.mController.mApplyDic.Count > 0)
        {
            this.ShowApplyTab();
        }
    }

    private void AddFriendSearchCb(relation_item item)
    {
        if (item == null)
        {
            TipsWindow.ShowWindow("该玩家名或ID不存在！");
            return;
        }
        this.mController.ReqApplyFriend(item.relationid);
    }

    private void input_search_on_valuechanged(string str)
    {
        if (string.IsNullOrEmpty(str))
        {
            this.obj_center_container.SetActive(true);
            this.obj_centersearch_container.SetActive(false);
        }
        else
        {
            this.SetupSearchPanel(str);
            this.obj_center_container.SetActive(false);
            this.obj_centersearch_container.SetActive(true);
        }
    }

    private void SetupSearchPanel(string str)
    {
        Dictionary<ulong, relation_item> dictionary = new Dictionary<ulong, relation_item>();
        foreach (relation_item relation_item in this.mController.mFriendDic.Values)
        {
            if (this.CheckNameContainsKey(relation_item.nickName, str) || this.CheckNameContainsKey(relation_item.relationname, str))
            {
                dictionary.Add(relation_item.relationid, relation_item);
            }
        }
        Transform parent = this.obj_searchitem.transform.parent;
        this.SetupListItem(this.obj_searchitem, dictionary);
        this.obj_notfoundtext.SetActive(dictionary.Count == 0);
    }

    private bool CheckNameContainsKey(string name, string key)
    {
        key = key.ToLower();
        string text = name.Replace(" ", string.Empty);
        string text2 = name.ToLower();
        return name.Contains(key) || text.Contains(key) || text2.Contains(key);
    }

    public void SetupShowOpePanel(UI_FriendNew.InputPageOpeType type)
    {
        this.lbl_add_friendid.text = string.Empty;
        this.mCurInputPageOpeType = type;
        this.transInputOpePanel.gameObject.SetActive(true);
        Text component = this.transInputOpePanel.Find("Panel/txt_title").GetComponent<Text>();
        Text component2 = this.transInputOpePanel.Find("Panel/InputField/Placeholder").GetComponent<Text>();
        int index = 0;
        int index2 = 0;
        switch (type)
        {
            case UI_FriendNew.InputPageOpeType.AddFriend:
                index = 0;
                index2 = 0;
                break;
            case UI_FriendNew.InputPageOpeType.AddGroup:
                index = 1;
                index2 = 1;
                break;
            case UI_FriendNew.InputPageOpeType.SetSecName:
                index = 2;
                index2 = 1;
                break;
        }
        component.text = component.GetComponent<UIInformationList>().listInformation[index].content;
        component2.text = component2.GetComponent<UIInformationList>().listInformation[index2].content;
    }

    public void SetupFriendTabListPanel()
    {
        this.mFriendTabsDic = new Dictionary<string, GameObject>();
        this.ClearListChildrens(this.obj_tabitem.transform.parent);
        for (int i = 0; i < this.friendTabTitleList.Count; i++)
        {
            GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.obj_tabitem);
            gameObject.transform.SetParent(this.obj_tabitem.transform.parent);
            gameObject.transform.localScale = Vector3.one;
            gameObject.SetActive(true);
            string title = this.friendTabTitleList[i];
            gameObject.transform.Find("Head/group_name/text_g").GetComponent<Text>().text = title;
            gameObject.transform.Find("Head/group_name/text_s").GetComponent<Text>().text = title;
            gameObject.transform.Find("Head/group_name/text_p").GetComponent<Text>().text = title;
            gameObject.transform.Find("Head/ReNameButton").gameObject.GetComponent<Button>().onClick.AddListener(delegate ()
            {
                this.SetupShowOpePanel(UI_FriendNew.InputPageOpeType.SetSecName);
                this.transInputOpePanel.name = title;
            });
            gameObject.transform.Find("Head/DelGroupButton").gameObject.GetComponent<Button>().onClick.AddListener(delegate ()
            {
                this.mController.mFriendNetWork.ReqModifyPageName(title, string.Empty, 2U);
            });
            UIEventListener.Get(gameObject.transform.Find("Head").gameObject).onClick = new UIEventListener.VoidDelegate(this.obj_tab_head_click);
            UIEventListener.Get(gameObject.transform.Find("Head").gameObject).onEnter = new UIEventListener.VoidDelegate(this.obj_tab_head_enter);
            UIEventListener.Get(gameObject.transform.Find("Head").gameObject).onExit = new UIEventListener.VoidDelegate(this.obj_tab_head_exit);
            this.mFriendTabsDic.Add(title, gameObject);
        }
        this.SetupFriendListPanel();
    }

    private void obj_tab_head_click(PointerEventData data)
    {
        UI_FriendNew.TabState state = UI_FriendNew.TabState.Close;
        if (this.mGroupStateDic.ContainsKey(data.pointerPress.gameObject) && this.mGroupStateDic[data.pointerPress.gameObject] == UI_FriendNew.TabState.Close)
        {
            state = UI_FriendNew.TabState.Open;
        }
        this.ChangeGroupStateUI(data.pointerPress.gameObject, state);
    }

    private void obj_tab_head_enter(PointerEventData data)
    {
        this.ChangeGroupStateUI(data.pointerEnter.gameObject, UI_FriendNew.TabState.Stay);
    }

    private void obj_tab_head_exit(PointerEventData data)
    {
        this.ChangeGroupStateUI(data.pointerEnter.gameObject, UI_FriendNew.TabState.QuitStay);
    }

    private void ChangeGroupStateUI(GameObject objGroupHead, UI_FriendNew.TabState state)
    {
        if (objGroupHead == null)
        {
            return;
        }
        if (!this.mGroupStateDic.ContainsKey(objGroupHead))
        {
            this.mGroupStateDic.Add(objGroupHead, UI_FriendNew.TabState.Close);
        }
        if (state != UI_FriendNew.TabState.Stay && state != UI_FriendNew.TabState.QuitStay)
        {
            this.mGroupStateDic[objGroupHead] = state;
        }
        GameObject gameObject = objGroupHead.transform.Find("group_bg/group_g").gameObject;
        GameObject gameObject2 = objGroupHead.transform.Find("group_bg/group_s").gameObject;
        GameObject gameObject3 = objGroupHead.transform.Find("group_bg/group_p").gameObject;
        GameObject gameObject4 = objGroupHead.transform.Find("group_name/text_g").gameObject;
        GameObject gameObject5 = objGroupHead.transform.Find("group_name/text_s").gameObject;
        GameObject gameObject6 = objGroupHead.transform.Find("group_name/text_p").gameObject;
        switch (state)
        {
            case UI_FriendNew.TabState.Open:
                gameObject.SetActive(false);
                gameObject3.SetActive(true);
                objGroupHead.transform.parent.Find("List").gameObject.SetActive(true);
                break;
            case UI_FriendNew.TabState.Close:
                gameObject3.SetActive(false);
                gameObject.SetActive(true);
                objGroupHead.transform.parent.Find("List").gameObject.SetActive(false);
                break;
            case UI_FriendNew.TabState.Stay:
                if (this.mGroupStateDic[objGroupHead] == UI_FriendNew.TabState.Close)
                {
                    gameObject2.SetActive(true);
                }
                gameObject5.SetActive(true);
                gameObject4.SetActive(false);
                gameObject6.SetActive(false);
                objGroupHead.transform.Find("ReNameButton").gameObject.SetActive(true);
                objGroupHead.transform.Find("DelGroupButton").gameObject.SetActive(true);
                break;
            case UI_FriendNew.TabState.QuitStay:
                gameObject2.SetActive(false);
                gameObject5.SetActive(false);
                if (this.mGroupStateDic[objGroupHead] == UI_FriendNew.TabState.Open)
                {
                    gameObject4.SetActive(true);
                    objGroupHead.transform.Find("ReNameButton").gameObject.SetActive(true);
                    objGroupHead.transform.Find("DelGroupButton").gameObject.SetActive(true);
                }
                else
                {
                    gameObject6.SetActive(true);
                    objGroupHead.transform.Find("ReNameButton").gameObject.SetActive(false);
                    objGroupHead.transform.Find("DelGroupButton").gameObject.SetActive(false);
                }
                break;
        }
    }

    public void SetupFriendListPanel()
    {
        if (this.lbl_search.text != string.Empty)
        {
            this.SetupSearchPanel(this.lbl_search.text);
        }
        Dictionary<string, Dictionary<ulong, relation_item>> dictionary = new Dictionary<string, Dictionary<ulong, relation_item>>();
        Dictionary<string, Dictionary<ulong, relation_item>> dictionary2 = new Dictionary<string, Dictionary<ulong, relation_item>>();
        foreach (relation_item relation_item in this.mController.mFriendDic.Values)
        {
            if (!dictionary.ContainsKey(relation_item.page))
            {
                dictionary.Add(relation_item.page, new Dictionary<ulong, relation_item>());
                dictionary2.Add(relation_item.page, new Dictionary<ulong, relation_item>());
            }
            dictionary[relation_item.page].Add(relation_item.relationid, relation_item);
            if (relation_item.status == 1U)
            {
                dictionary2[relation_item.page].Add(relation_item.relationid, relation_item);
            }
        }
        bool flag = false;
        bool flag2 = false;
        foreach (string key in this.mFriendTabsDic.Keys)
        {
            Transform transform = this.mFriendTabsDic[key].transform.Find("List");
            GameObject gameObject = transform.Find("FriendItem").gameObject;
            gameObject.SetActive(false);
            if (dictionary.ContainsKey(key))
            {
                if (!this.isSelectOnline)
                {
                    this.SetupListItem(gameObject, dictionary[key]);
                }
                else
                {
                    this.SetupListItem(gameObject, dictionary2[key]);
                }
                foreach (relation_item relation_item2 in dictionary[key].Values)
                {
                    if (!flag2)
                    {
                        flag2 = this.mController.JudgeNewFriend(relation_item2.relationid.ToString());
                    }
                    if (!flag2 && !flag)
                    {
                        flag = this.mController.isFriendHasUnReadMsg(relation_item2.relationid.ToString());
                    }
                }
                string text = dictionary2[key].Count + "/" + dictionary[key].Count;
                this.mFriendTabsDic[key].transform.Find("Head/group_name/text_g/txt_value").GetComponent<Text>().text = text;
                this.mFriendTabsDic[key].transform.Find("Head/group_name/text_s/txt_value").GetComponent<Text>().text = text;
                this.mFriendTabsDic[key].transform.Find("Head/group_name/text_p/txt_value").GetComponent<Text>().text = text;
            }
            else
            {
                this.ClearListChildrens(gameObject.transform.parent);
                this.mFriendTabsDic[key].transform.Find("Head/group_name/text_g/txt_value").GetComponent<Text>().text = "0/0";
                this.mFriendTabsDic[key].transform.Find("Head/group_name/text_s/txt_value").GetComponent<Text>().text = "0/0";
                this.mFriendTabsDic[key].transform.Find("Head/group_name/text_p/txt_value").GetComponent<Text>().text = "0/0";
            }
        }
        this.obj_newfriendtag.SetActive(flag2);
        this.obj_newmsgtag.SetActive(flag);
        this.lbl_friend_num.text = this.mController.mFriendDic.Count + "/255";
    }

    public void SetupApplyListPanel()
    {
        Transform parent = this.obj_applyitem.transform.parent;
        this.SetupListItem(this.obj_applyitem, this.mController.mApplyDic);
        this.obj_apply_tip.SetActive(this.mController.mApplyDic.Count > 0);
        this.obj_apply_tip.FindChild("num").GetComponent<Text>().text = this.mController.mApplyDic.Count.ToString();
        foreach (relation_item relation_item in this.mController.mApplyDic.Values)
        {
            UI_FriendItem component = parent.Find(relation_item.relationid.ToString()).GetComponent<UI_FriendItem>();
            if (component)
            {
                component.InitApplyButton();
            }
        }
    }

    public void SetupRecentListPanel()
    {
        Transform parent = this.obj_recentitem.transform.parent;
        this.SetupListItem(this.obj_recentitem, this.mController.mRecentDic);
    }

    public void SetupBlackListPanel()
    {
        Transform parent = this.obj_blackitem.transform.parent;
        this.ClearListChildrens(parent);
        foreach (BlackItem blackItem in this.mController.mBlackDic.Values)
        {
            relation_item relation_item = new relation_item
            {
                relationid = blackItem.charid,
                relationname = blackItem.name,
                level = blackItem.level,
                viplevel = blackItem.viplevel
            };
            GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.obj_blackitem);
            gameObject.name = relation_item.relationid.ToString();
            gameObject.transform.SetParent(parent);
            gameObject.transform.localScale = Vector3.one;
            gameObject.SetActive(true);
            UI_FriendItem component = gameObject.GetComponent<UI_FriendItem>();
            if (component != null)
            {
                component.Initilize(relation_item);
            }
        }
    }

    public void ClearListChildrens(Transform grid)
    {
        UIManager.Instance.ClearListChildrens(grid, 1);
    }

    private void SetupListItem(GameObject _obj, Dictionary<ulong, relation_item> dic)
    {
        Transform parent = _obj.transform.parent;
        this.ClearListChildrens(parent);
        foreach (relation_item relation_item in dic.Values)
        {
            GameObject obj = UnityEngine.Object.Instantiate<GameObject>(_obj);
            obj.name = relation_item.relationid.ToString();
            obj.transform.SetParent(parent);
            obj.transform.localScale = Vector3.one;
            obj.SetActive(true);
            Button button = obj.GetComponent<Button>();
            if (!button)
            {
                button = obj.AddComponent<Button>();
            }
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(delegate ()
            {
                this.mController.AddNewFriend(obj.name);
            });
            UI_FriendItem component = obj.GetComponent<UI_FriendItem>();
            if (component != null)
            {
                component.Initilize(relation_item);
            }
        }
    }

    public void ShowApplyTab()
    {
        this.tgl_apply.isOn = true;
    }

    public void btn_second_on_click(PointerEventData eventData, relation_item item)
    {
        this.mUISecondMenu.Initilize(item, this.mTabType, this.friendTabTitleList, eventData.pointerPress.transform.position);
    }

    public void btn_commonclear_on_click(PointerEventData eventData)
    {
        switch (this.mTabType)
        {
            case FriendTabType.Apply:
                foreach (relation_item relation_item in this.mController.mApplyDic.Values)
                {
                    this.mController.ReqAnswerApplyRelation(relation_item.relationid, 0U);
                }
                break;
            case FriendTabType.Recent:
                this.mController.ClearRecentList();
                break;
            case FriendTabType.Black:
                this.mController.mFriendNetWork.ReqOperateBlackList(0UL, 3U);
                break;
        }
    }

    public override void OnDispose()
    {
        base.OnDispose();
        this.UnInit();
    }

    private void Update()
    {
    }

    public void UnInit()
    {
        Scheduler.Instance.RemoveUpdator(new Scheduler.OnScheduler(this.Update));
    }

    public Transform transInputOpePanel;

    private Toggle tgl_apply;

    private GameObject obj_apply_tip;

    private UI_FriendSecondMenu mUISecondMenu;

    public List<string> friendTabTitleList = new List<string>();

    private Dictionary<string, GameObject> mFriendTabsDic = new Dictionary<string, GameObject>();

    private GameObject obj_newmsgtag;

    private GameObject obj_newfriendtag;

    private Text lbl_friend_num;

    private GameObject obj_tabitem;

    private GameObject obj_applyitem;

    private GameObject obj_recentitem;

    private GameObject obj_blackitem;

    private InputField lbl_add_friendid;

    private GameObject btn_clear_apply;

    private GameObject btn_clear_recent;

    private GameObject btn_clear_black;

    private InputField lbl_search;

    private GameObject obj_center_container;

    private GameObject obj_centersearch_container;

    private GameObject obj_searchitem;

    private GameObject obj_notfoundtext;

    private UI_FriendNew.InputPageOpeType mCurInputPageOpeType;

    private FriendTabType mTabType;

    private bool isSelectOnline;

    private Dictionary<GameObject, UI_FriendNew.TabState> mGroupStateDic = new Dictionary<GameObject, UI_FriendNew.TabState>();

    public enum InputPageOpeType
    {
        AddFriend,
        AddGroup,
        SetSecName,
        SetNickName
    }

    private enum TabState
    {
        Open,
        Close,
        Stay,
        QuitStay
    }
}
