using System;
using System.Collections.Generic;
using Chat;
using Framework.Managers;
using Models;
using relation;
using UnityEngine;

public class FriendControllerNew : ControllerBase
{
    public UI_FriendNew mUIFriend
    {
        get
        {
            return UIManager.GetUIObject<UI_FriendNew>();
        }
    }

    public UI_FriendPrivateChat mUIFriendPrivateChat
    {
        get
        {
            return UIManager.GetUIObject<UI_FriendPrivateChat>();
        }
    }

    public override void Awake()
    {
        this.Init();
    }

    private void Init()
    {
        this.loginTime = SingletonForMono<GameTime>.Instance.GetCurrServerTimeBySecond();
        this.RECENT_NUM_LIMIT = LuaConfigManager.GetXmlConfigTable("relation").GetCacheField_Table("RecentrelationMaxnum").GetCacheField_Int("value");
        this.mFriendNetWork = new FriendNetWork();
        this.mFriendNetWork.Initialize();
    }

    public bool IsFriend(ulong id)
    {
        foreach (relation_item relation_item in this.mFriendDic.Values)
        {
            if (relation_item.relationid == id)
            {
                return true;
            }
        }
        return false;
    }

    public relation_item GetFriend(ulong id)
    {
        foreach (relation_item relation_item in this.mFriendDic.Values)
        {
            if (relation_item.relationid == id)
            {
                return relation_item;
            }
        }
        return null;
    }

    public void AddRecentItem(string id)
    {
        if (id == string.Empty || id == MainPlayer.Self.OtherPlayerData.BaseData.id.ToString())
        {
            return;
        }
        string @string = PlayerPrefs.GetString("recentlist");
        string[] array = @string.Split(new string[]
        {
            ","
        }, StringSplitOptions.RemoveEmptyEntries);
        List<string> list = new List<string>();
        for (int i = 0; i < array.Length; i++)
        {
            list.Add(array[i]);
            if (array[i] == id)
            {
                return;
            }
        }
        while (list.Count > this.RECENT_NUM_LIMIT)
        {
            list.RemoveAt(0);
        }
        list.Add(id);
        string text = string.Empty;
        for (int j = 0; j < list.Count; j++)
        {
            text = text + list[j] + ",";
        }
        PlayerPrefs.SetString("recentlist", text);
        this.mFriendNetWork.ReqSearchRelation(id, null);
        if (this.mUIFriend != null)
        {
            this.mUIFriend.SetupRecentListPanel();
        }
    }

    public void ClearRecentList()
    {
        PlayerPrefs.SetString("recentlist", string.Empty);
        this.mRecentDic = new Dictionary<ulong, relation_item>();
        if (this.mUIFriend != null)
        {
            this.mUIFriend.SetupRecentListPanel();
        }
    }

    public void InitChatHistory()
    {
        this.mFriendNetWork.ReqOffLineChat();
    }

    public void InitRecentList()
    {
        this.mRecentDic = new Dictionary<ulong, relation_item>();
        string @string = PlayerPrefs.GetString("recentlist");
        string[] array = @string.Split(new string[]
        {
            ","
        }, StringSplitOptions.RemoveEmptyEntries);
        for (int i = 0; i < array.Length; i++)
        {
            this.mFriendNetWork.ReqSearchRelation(array[i], null);
        }
        if (this.mUIFriend != null)
        {
            this.mUIFriend.SetupRecentListPanel();
        }
    }

    public void ShowSearchResult(relation_item item)
    {
        if (item != null)
        {
            this.mRecentDic.Add(item.relationid, item);
        }
    }

    public void InitFriendList(List<relation_item> flist)
    {
        this.mFriendDic.Clear();
        for (int i = 0; i < flist.Count; i++)
        {
            this.mFriendDic.Add(flist[i].relationid, flist[i]);
        }
        if (this.mUIFriend != null)
        {
            this.mUIFriend.SetupFriendListPanel();
        }
        if (this.mUIFriendPrivateChat != null)
        {
            this.mUIFriendPrivateChat.RefreshOnOffLineState();
        }
    }

    public void InitFriendApplyForList(List<relation_item> applylist)
    {
        this.mApplyDic.Clear();
        for (int i = 0; i < applylist.Count; i++)
        {
            this.mApplyDic.Add(applylist[i].relationid, applylist[i]);
            ControllerManager.Instance.GetController<MainUIController>().AddMessage(MessageType.Friend, 1, delegate
            {
                ControllerManager.Instance.GetController<FriendControllerNew>().ShowFriendApplyUI();
                ControllerManager.Instance.GetController<MainUIController>().ReadMessage(MessageType.Friend);
            });
        }
        if (this.mUIFriend != null)
        {
            this.mUIFriend.SetupApplyListPanel();
        }
    }

    public void RefreshRecentList(List<relation_item> ritems)
    {
    }

    public void ShowFriendApplyUI()
    {
        Action action = delegate ()
        {
            if (this.mUIFriend != null)
            {
                this.mUIFriend.ShowApplyTab();
            }
        };
        if (this.mUIFriend == null)
        {
            this.ShowFriendUI(action);
        }
        else
        {
            action();
        }
    }

    public void ShowFriendUI(Action action = null)
    {
        UI_FriendNew uiobject = UIManager.GetUIObject<UI_FriendNew>();
        if (uiobject != null)
        {
            UIManager.Instance.DeleteUI<UI_FriendNew>();
        }
        else
        {
            ManagerCenter.Instance.GetManager<UIManager>().ShowUI<UI_FriendNew>("UI_FriendNew", delegate ()
            {
                if (action != null)
                {
                    action();
                }
            }, UIManager.ParentType.CommonUI, false);
        }
    }

    public void RemoveItemFromFriendList(ulong id)
    {
        foreach (ulong num in this.mFriendDic.Keys)
        {
            if (num == id)
            {
                this.mFriendDic.RemoveAt(num);
                break;
            }
        }
        if (this.mUIFriend != null)
        {
            this.mUIFriend.SetupFriendListPanel();
        }
    }

    public void ReqAnswerApplyRelation(ulong id, uint type)
    {
        this.mFriendNetWork.ReqAnswerApplyRelation(id, type);
    }

    public void RemoveItemFromApplyList(ulong id)
    {
        this.mApplyDic.RemoveAt(id);
        this.RefreshApplyListUI();
    }

    private void RefreshApplyListUI()
    {
        if (this.mUIFriend != null)
        {
            this.mUIFriend.SetupApplyListPanel();
        }
    }

    public void RefreshRelationItem(relation_item _item)
    {
    }

    public void ReqApplyFriend(ulong id)
    {
        if (this.GetFrienByID(id) == null)
        {
            if (this.mApplyDic.ContainsKey(id))
            {
                UIManager.Instance.ShowUI<UI_MessageBox>("UI_MessageBox", delegate ()
                {
                    UI_MessageBox uiobject = UIManager.GetUIObject<UI_MessageBox>();
                    uiobject.SetContent("该玩家也向你发送了请求，与TA成为好友?", "提示", true);
                    uiobject.SetOkCb(new MessageOkCb2(this.ReqAddFriendAgreeCb), id.ToString());
                }, UIManager.ParentType.CommonUI, false);
            }
            else
            {
                this.mFriendNetWork.ReqApplyRelation(id);
            }
        }
    }

    private void ReqAddFriendAgreeCb(string data)
    {
        this.mFriendNetWork.ReqAnswerApplyRelation(ulong.Parse(data), 1U);
    }

    private relation_item GetFrienByID(ulong id)
    {
        if (this.mFriendDic.ContainsKey(id))
        {
            return this.mFriendDic[id];
        }
        return null;
    }

    public void SendChat(ulong toid, string content, uint showtype = 0U, List<ChatLink> links = null, bool shake = false)
    {
        ControllerManager.Instance.GetController<ChatControl>().CNetWork.ReqChat(ChannelType.ChannelType_Private, MainPlayer.Self.GetCharID(), MainPlayer.Self.OtherPlayerData.MapUserData.name, MainPlayer.Self.OtherPlayerData.MapUserData.mapdata.country, content, showtype, links, toid, string.Empty, shake);
    }

    public void OnReceiveChatData(MyChatData data)
    {
        this.AddPrivateChatLocalData(data);
        if (this.mUIFriendPrivateChat != null)
        {
            this.mUIFriendPrivateChat.AddPrivateChatItem(data);
        }
        this.RefreshFriendTip();
    }

    public void RefreshFriendTip()
    {
        UI_FriendNew uiobject = UIManager.GetUIObject<UI_FriendNew>();
        if (uiobject != null)
        {
            uiobject.SetupFriendListPanel();
        }
        bool flag = false;
        if (this.mApplyDic.Count > 0)
        {
            flag = true;
        }
        else
        {
            foreach (List<MyChatData> list in this.chatDataDic.Values)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    if (this.mFriendDic.ContainsKey(ulong.Parse(list[i].charid)) && !list[i].readed)
                    {
                        flag = true;
                        break;
                    }
                }
                if (flag)
                {
                    break;
                }
            }
        }
        if (!flag)
        {
            foreach (ulong num in this.mFriendDic.Keys)
            {
                if (this.JudgeNewFriend(num.ToString()))
                {
                    flag = true;
                    break;
                }
            }
        }
        MainUIController controller = ControllerManager.Instance.GetController<MainUIController>();
        if (controller.mainView != null)
        {
            controller.mainView.ShowFriendTip(flag);
        }
    }

    public void OnReceiveOfflineChat(List<ChatData> datas)
    {
        for (int i = 0; i < datas.Count; i++)
        {
            this.AddPrivateChatLocalData(new MyChatData(datas[i], false));
        }
        this.RefreshFriendTip();
        this.RefreshGmTip();
    }

    private void RefreshGmTip()
    {
        if (this.chatDataDic.ContainsKey("0"))
        {
            List<MyChatData> list = this.chatDataDic["0"];
            for (int i = 0; i < list.Count; i++)
            {
                if (!list[i].readed)
                {
                    ControllerManager.Instance.GetController<GmChatController>().mMapInitGmBtnShow = true;
                    ControllerManager.Instance.GetController<GmChatController>().mNewGmData = list[i];
                    return;
                }
            }
        }
    }

    public string GetTargetId(MyChatData chatData)
    {
        string text = chatData.charid;
        if (text == MainPlayer.Self.GetCharID().ToString())
        {
            text = chatData.tocharid;
        }
        return text;
    }

    public void AddPrivateChatLocalData(MyChatData chatData)
    {
        string targetId = this.GetTargetId(chatData);
        if (!this.chatDataDic.ContainsKey(targetId))
        {
            this.chatDataDic.Add(targetId, new List<MyChatData>());
        }
        this.chatDataDic[targetId].Add(chatData);
        this.UpdatePrivateChatData();
    }

    public void ClearPrivateChatLocalData(ulong id)
    {
        if (this.chatDataDic.ContainsKey(id.ToString()))
        {
            this.chatDataDic[id.ToString()] = new List<MyChatData>();
            this.UpdatePrivateChatData();
        }
    }

    public void ReadedPrivateChatLocalData(ulong id)
    {
        if (this.chatDataDic.ContainsKey(id.ToString()))
        {
            for (int i = 0; i < this.chatDataDic[id.ToString()].Count; i++)
            {
                this.chatDataDic[id.ToString()][i].readed = true;
            }
            this.UpdatePrivateChatData();
        }
    }

    public void InitPrivateChatLocalData()
    {
        string @string = PlayerPrefs.GetString("ChatData");
        string[] array = @string.Split(new string[]
        {
            ","
        }, StringSplitOptions.RemoveEmptyEntries);
        Dictionary<string, string> dictionary = new Dictionary<string, string>();
        for (int i = 0; i < array.Length; i++)
        {
            string[] array2 = array[i].Split(new char[]
            {
                '-'
            });
            if (array2.Length >= 2 && !dictionary.ContainsKey(array2[0]))
            {
                dictionary.Add(array2[0], array2[1]);
            }
        }
        string string2 = PlayerPrefs.GetString("FriendPrivateChatModelKey");
        if (!string.IsNullOrEmpty(string2))
        {
            FriendPrivateChatModel friendPrivateChatModel = ServerStorageManager.Instance.DeserializeClassLocal<FriendPrivateChatModel>(string2);
            if (friendPrivateChatModel != null)
            {
                this.chatDataDic = new Dictionary<string, List<MyChatData>>();
                if (friendPrivateChatModel.chatDataList != null)
                {
                    for (int j = 0; j < friendPrivateChatModel.chatDataList.Count; j++)
                    {
                        MyChatData myChatData = friendPrivateChatModel.chatDataList[j];
                        string key = (myChatData.charid + myChatData.chattime).ToString();
                        if (dictionary.ContainsKey(key))
                        {
                            myChatData.content = dictionary[key];
                        }
                        string targetId = this.GetTargetId(myChatData);
                        if (!this.chatDataDic.ContainsKey(targetId))
                        {
                            this.chatDataDic.Add(targetId, new List<MyChatData>());
                        }
                        this.chatDataDic[targetId].Add(myChatData);
                    }
                }
            }
        }
    }

    private void UpdatePrivateChatData()
    {
        FriendPrivateChatModel friendPrivateChatModel = new FriendPrivateChatModel();
        friendPrivateChatModel.chatDataList = new List<MyChatData>();
        string text = string.Empty;
        foreach (List<MyChatData> list in this.chatDataDic.Values)
        {
            if (list != null)
            {
                MyChatData[] array = list.ToArray();
                for (int i = 0; i < array.Length; i++)
                {
                    MyChatData myChatData = array[i].Clone() as MyChatData;
                    if (!string.IsNullOrEmpty(myChatData.content))
                    {
                        string content = myChatData.content;
                        string text2 = text;
                        text = string.Concat(new string[]
                        {
                            text2,
                            myChatData.charid,
                            myChatData.chattime,
                            "-",
                            content,
                            ","
                        });
                        myChatData.content = "1";
                        friendPrivateChatModel.chatDataList.Add(myChatData);
                    }
                }
            }
        }
        PlayerPrefs.SetString("ChatData", text);
        string value = ServerStorageManager.Instance.SerializeClassLocal<FriendPrivateChatModel>(friendPrivateChatModel);
        PlayerPrefs.SetString("FriendPrivateChatModelKey", value);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }

    public override string ControllerName
    {
        get
        {
            return "friend_controllernew";
        }
    }

    public void AddNewFriend(string id)
    {
        if (!this.OldFriendContainId(id))
        {
            ServerStorageManager.Instance.AddUpdateData(ServerStorageKey.FRIEND_IDS, this.mOldFrinedIds + id + ",", 0U);
        }
    }

    public void DelNewFriend(string id)
    {
        if (this.OldFriendContainId(id))
        {
            ServerStorageManager.Instance.AddUpdateData(ServerStorageKey.FRIEND_IDS, this.mOldFrinedIds.Replace(id + ",", string.Empty), 0U);
        }
    }

    public bool JudgeNewFriend(string id)
    {
        return !this.OldFriendContainId(id);
    }

    private bool OldFriendContainId(string id)
    {
        return !string.IsNullOrEmpty(this.mOldFrinedIds) && new List<string>(this.mOldFrinedIds.Split(new char[]
        {
            ','
        })).Contains(id);
    }

    public bool isFriendHasUnReadMsg(string id)
    {
        if (ControllerManager.Instance.GetController<FriendControllerNew>().chatDataDic.ContainsKey(id))
        {
            IList<MyChatData> list = ControllerManager.Instance.GetController<FriendControllerNew>().chatDataDic[id];
            for (int i = 0; i < list.Count; i++)
            {
                if (!list[i].readed)
                {
                    return true;
                }
            }
        }
        return false;
    }

    private const string RECENT_KEY = "recentlist";

    public FriendNetWork mFriendNetWork;

    public Dictionary<ulong, relation_item> mFriendDic = new Dictionary<ulong, relation_item>();

    public Dictionary<ulong, relation_item> mApplyDic = new Dictionary<ulong, relation_item>();

    public Dictionary<ulong, relation_item> mRecentDic = new Dictionary<ulong, relation_item>();

    public Dictionary<ulong, BlackItem> mBlackDic = new Dictionary<ulong, BlackItem>();

    public uint loginTime;

    private int RECENT_NUM_LIMIT = 50;

    public Dictionary<string, List<MyChatData>> chatDataDic = new Dictionary<string, List<MyChatData>>();

    public string mOldFrinedIds;
}
