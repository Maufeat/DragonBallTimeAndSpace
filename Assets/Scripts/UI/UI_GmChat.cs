using System;
using System.Collections.Generic;
using Framework.Managers;
using relation;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_GmChat : UIPanelBase
{
    public override void OnDispose()
    {
        base.OnDispose();
    }

    public override void OnInit(Transform _root)
    {
        this.root = _root;
        this.obj_frienditem = this.root.Find("Main/Center/Left/ScrollRect/List/FriendItem").gameObject;
        this.obj_frienditem.SetActive(false);
        this.obj_chatitem = this.root.Find("Main/Center/Right/Center/ScrollRect/List/ChatItem").gameObject;
        this.obj_chatitem.SetActive(false);
        this.obj_iconitem = this.root.Find("Main/Panel_expression/Scroll View/Viewport/Content/Image").gameObject;
        this.obj_iconitem.GetComponent<Image>().material = null;
        this.obj_iconitem.SetActive(false);
        this.sb_chatcontent = this.root.Find("Main/Center/Right/Center/Scrollbar").GetComponent<Scrollbar>();
        this.sb_chatcontent.onValueChanged.AddListener(new UnityAction<float>(this.ChatContentScrollBarOnValueChanged));
        this.obj_face_container = this.root.Find("Main/Panel_expression").gameObject;
        UIEventListener.Get(this.obj_face_container).onClick = new UIEventListener.VoidDelegate(this.btn_iconunsel_on_click);
        this.lbl_input = this.root.Find("Main/Center/Right/Bottom/InputField").GetComponent<InputField>();
        UIEventListener.Get(this.lbl_input.gameObject).onClick = new UIEventListener.VoidDelegate(this.lbl_input_onpointerclick);
        GameObject gameObject = this.root.Find("Main/Center/Right/Bottom/Bottom/IconButton").gameObject;
        GameObject gameObject2 = this.root.Find("Main/Center/Right/Bottom/Bottom/ZhenButton").gameObject;
        GameObject gameObject3 = this.root.Find("Main/Center/Right/Bottom/Bottom/ClearButton").gameObject;
        GameObject gameObject4 = this.root.Find("Main/Center/Right/Bottom/Bottom/CloseButton").gameObject;
        GameObject gameObject5 = this.root.Find("Main/Center/Right/Bottom/Bottom/SendButton").gameObject;
        UIEventListener.Get(gameObject).onClick = new UIEventListener.VoidDelegate(this.btn_icon_on_click);
        GlobalRegister.AddTextTipComponent(gameObject, 70005U);
        UIEventListener.Get(gameObject2).onClick = new UIEventListener.VoidDelegate(this.btn_zhen_on_click);
        GlobalRegister.AddTextTipComponent(gameObject2, 70006U);
        UIEventListener.Get(gameObject3).onClick = new UIEventListener.VoidDelegate(this.btn_clear_on_click);
        GlobalRegister.AddTextTipComponent(gameObject3, 70007U);
        UIEventListener.Get(gameObject4).onClick = new UIEventListener.VoidDelegate(this.btn_close_on_click);
        UIEventListener.Get(gameObject5).onClick = new UIEventListener.VoidDelegate(this.btn_send_on_click);
        GameObject gameObject6 = this.root.Find("Main/Head/CloseButton").gameObject;
        UIEventListener.Get(gameObject6).onClick = new UIEventListener.VoidDelegate(this.btn_closeall_on_click);
        base.OnInit(this.root);
        this.list = new List<relation_item>();
        this.friendController = ControllerManager.Instance.GetController<FriendControllerNew>();
        this.mController = ControllerManager.Instance.GetController<GmChatController>();
        this.AddPrivateChatFriend(new relation_item
        {
            relationid = ulong.Parse(this.mController.mNewGmData.charid),
            relationname = this.mController.mNewGmData.charname,
            level = 1U,
            status = 1U
        });
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

    private MyChatData GetSelfChatData(string content, bool shake = false)
    {
        return new MyChatData
        {
            charid = MainPlayer.Self.GetCharID().ToString(),
            tocharid = this.mCurChatTargetId.ToString(),
            content = content,
            channel = 8.ToString(),
            charname = MainPlayer.Self.OtherPlayerData.MapUserData.name,
            chattime = ((uint)(SingletonForMono<GameTime>.Instance.GetCurrServerTime() / 1000UL)).ToString(),
            show_type = "0",
            charcountry = MainPlayer.Self.OtherPlayerData.MapUserData.mapdata.country.ToString(),
            shake = shake
        };
    }

    private void btn_closeall_on_click(PointerEventData eventData)
    {
        UIManager.Instance.DeleteUI<UI_GmChat>();
        this.mController.ShowBtnGm(false);
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

    private void btn_zhen_on_click(PointerEventData eventData)
    {
        this.isScrollEnd = true;
        this.mController.SendChat(this.mCurChatTargetId, "对方给您发送了一次窗口抖动", 0U, null, true);
        this.mController.OnReceiveChatData(this.GetSelfChatData("您给对方发送了一次窗口抖动", true));
    }

    private void btn_clear_on_click(PointerEventData eventData)
    {
        this.friendController.ClearPrivateChatLocalData(this.mCurChatTargetId);
        this.SetupChatPanel();
    }

    private void btn_close_on_click(PointerEventData eventData)
    {
        if (this.list.Count == 1)
        {
            this.btn_closeall_on_click(null);
        }
        else
        {
            UnityEngine.Object.Destroy(this.obj_frienditem.transform.parent.GetChild(1).gameObject);
            this.list.RemoveAt(0);
        }
    }

    private void btn_send_on_click(PointerEventData eventData)
    {
        string text = this.lbl_input.text;
        if (text == string.Empty)
        {
            return;
        }
        this.mController.SendChat(this.mCurChatTargetId, text, 0U, null, false);
        this.mController.OnReceiveChatData(this.GetSelfChatData(text, false));
        this.lbl_input.text = string.Empty;
    }

    private relation_item GetFriendItemById(ulong id)
    {
        relation_item result = null;
        for (int i = 0; i < this.list.Count; i++)
        {
            if (this.list[i].relationid == id)
            {
                result = this.list[i];
                break;
            }
        }
        return result;
    }

    public void RefreshOnOffLineState()
    {
        Dictionary<string, uint> dictionary = new Dictionary<string, uint>();
        foreach (relation_item relation_item in this.friendController.mFriendDic.Values)
        {
            dictionary.Add(relation_item.relationid.ToString(), relation_item.status);
        }
        for (int i = 1; i < this.obj_frienditem.transform.parent.childCount; i++)
        {
            Transform child = this.obj_frienditem.transform.parent.GetChild(i);
            if (dictionary.ContainsKey(child.gameObject.name))
            {
                child.Find("Panel_friend/Image_online").gameObject.SetActive(dictionary[child.gameObject.name] == 1U);
                GameObject gameObject = child.Find("Panel_friend/Offline").gameObject;
                gameObject.SetActive(dictionary[child.gameObject.name] == 0U);
                if (dictionary[child.gameObject.name] == 0U)
                {
                    gameObject.GetComponent<Text>().text = CommonTools.GetOfflineText(this.friendController.mFriendDic[ulong.Parse(child.gameObject.name)].offlineTime);
                }
            }
        }
    }

    public void AddPrivateChatFriend(relation_item frienddata)
    {
        relation_item friendItemById = this.GetFriendItemById(frienddata.relationid);
        if (friendItemById != null)
        {
            this.obj_frienditem.transform.parent.Find(frienddata.relationid.ToString()).SetSiblingIndex(1);
            this.list.Remove(friendItemById);
            this.list.Insert(0, frienddata);
        }
        else
        {
            this.list.Insert(0, frienddata);
            GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.obj_frienditem);
            gameObject.transform.SetParent(this.obj_frienditem.transform.parent);
            gameObject.transform.localScale = Vector3.one;
            gameObject.SetActive(true);
            gameObject.transform.SetSiblingIndex(1);
            gameObject.name = frienddata.relationid.ToString();
            gameObject.transform.Find("Panel_friend/name").GetComponent<Text>().text = frienddata.relationname;
            gameObject.transform.Find("Panel_friend/lv").GetComponent<Text>().text = frienddata.level + "级";
            gameObject.transform.Find("Panel_friend/Image_online").gameObject.SetActive(frienddata.status == 1U);
            GameObject gameObject2 = gameObject.transform.Find("Panel_friend/Offline").gameObject;
            gameObject2.SetActive(frienddata.status == 0U);
            if (frienddata.status == 0U)
            {
                gameObject2.GetComponent<Text>().text = CommonTools.GetOfflineText(frienddata.offlineTime);
            }
            gameObject.transform.Find("tipicon").gameObject.SetActive(false);
            UIEventListener.Get(gameObject).onClick = new UIEventListener.VoidDelegate(this.obj_frienditem_on_click);
            UIEventListener.Get(gameObject).onEnter = new UIEventListener.VoidDelegate(this.obj_frienditem_on_enter);
            UIEventListener.Get(gameObject).onExit = new UIEventListener.VoidDelegate(this.obj_frienditem_on_exit);
        }
        this.mCurChatTargetId = this.list[0].relationid;
        this.mCurChatTarget = this.list[0];
        this.lbl_input.text = string.Empty;
        this.SetupContentPanel();
    }

    private void obj_frienditem_on_enter(PointerEventData data)
    {
        data.pointerEnter.gameObject.transform.Find("Image_stay").gameObject.SetActive(true);
        data.pointerEnter.gameObject.transform.Find("Panel_friend/head/Image_stay").gameObject.SetActive(true);
        data.pointerEnter.gameObject.transform.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0f);
    }

    private void obj_frienditem_on_exit(PointerEventData data)
    {
        data.pointerEnter.gameObject.transform.Find("Image_stay").gameObject.SetActive(false);
        data.pointerEnter.gameObject.transform.Find("Panel_friend/head/Image_stay").gameObject.SetActive(false);
        data.pointerEnter.gameObject.transform.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
    }

    private void obj_frienditem_on_click(PointerEventData data)
    {
        ulong num = ulong.Parse(data.pointerPress.name);
        relation_item friendItemById = this.GetFriendItemById(num);
        if (friendItemById != null && num != this.mCurChatTargetId)
        {
            this.lbl_input.text = string.Empty;
            this.mCurChatTargetId = num;
            for (int i = 0; i < this.list.Count; i++)
            {
                if (this.list[i].relationid == this.mCurChatTargetId)
                {
                    this.mCurChatTarget = this.list[i];
                    break;
                }
            }
            Transform transform = this.obj_frienditem.transform.parent.Find(num.ToString());
            transform.Find("tipicon").gameObject.SetActive(false);
            this.isScrollEnd = true;
            this.SetupContentPanel();
        }
    }

    private void SetFriendListItemSelect(Transform transItem, bool isSel)
    {
        Transform transform = transItem.Find("Image_selected");
        Transform transform2 = transItem.Find("Panel_friend/head/Image_selected");
        transform.gameObject.SetActive(isSel);
        transform2.gameObject.SetActive(isSel);
    }

    private void SetupContentPanel()
    {
        relation_item relation_item = this.mCurChatTarget;
        Transform transform = this.root.Find("Main/Center/Left/ScrollRect/List");
        for (int i = 0; i < transform.childCount; i++)
        {
            this.SetFriendListItemSelect(transform.GetChild(i), transform.GetChild(i).gameObject.name == relation_item.relationid.ToString());
        }
        this.SetupChatPanel();
    }

    private void SetupChatPanel()
    {
        UIManager.Instance.ClearListChildrens(this.obj_chatitem.transform.parent, 1);
        relation_item relation_item = this.mCurChatTarget;
        if (this.friendController.chatDataDic.ContainsKey(relation_item.relationid.ToString()))
        {
            IList<MyChatData> list = this.friendController.chatDataDic[relation_item.relationid.ToString()];
            for (int i = 0; i < list.Count; i++)
            {
                MyChatData myChatData = list[i];
                string b = MainPlayer.Self.GetCharID().ToString();
                if (!(myChatData.charid != b) || !(myChatData.tocharid != b))
                {
                    bool flag = false;
                    if (!myChatData.readed || i == list.Count - 1 || uint.Parse(myChatData.chattime) > this.friendController.loginTime)
                    {
                        flag = true;
                    }
                    else if (i < list.Count - 1 && (!list[i + 1].readed || uint.Parse(list[i + 1].chattime) > this.friendController.loginTime))
                    {
                        flag = true;
                    }
                    if (flag || this.oldChatDataList.Contains(myChatData))
                    {
                        this.CreateChatItem(myChatData);
                    }
                }
            }
            this.friendController.ReadedPrivateChatLocalData(relation_item.relationid);
            if (this.sb_chatcontent.value < 0.01f || this.isScrollEnd)
            {
                this.isScrollEnd = false;
                Scheduler.Instance.AddTimer(0.1f, false, new Scheduler.OnScheduler(this.RepositonScrollBarBottom));
            }
        }
    }

    private void RepositonScrollBarBottom()
    {
        this.sb_chatcontent.value = 0f;
    }

    public void AddPrivateChatItem(MyChatData chatData)
    {
        string text = chatData.charid;
        if (chatData.charid == MainPlayer.Self.GetCharID().ToString())
        {
            text = chatData.tocharid;
        }
        if (this.GetFriendItemById(ulong.Parse(text)) != null)
        {
            if (text == this.mCurChatTargetId.ToString())
            {
                this.isScrollEnd = true;
                this.SetupChatPanel();
                if (chatData.shake)
                {
                    this.DoShake();
                }
            }
            else
            {
                this.obj_frienditem.transform.parent.Find(text.ToString()).Find("tipicon").gameObject.SetActive(true);
            }
        }
    }

    private void DoShake()
    {
        Shake shake = this.root.GetComponent<Shake>();
        if (shake == null)
        {
            shake = this.root.gameObject.AddComponent<Shake>();
        }
        shake.StartShake();
    }

    private void ChatContentScrollBarOnValueChanged(float value)
    {
        if (value > 0.99f)
        {
            int num = 0;
            if (this.friendController.chatDataDic.ContainsKey(this.mCurChatTargetId.ToString()))
            {
                IList<MyChatData> list = this.friendController.chatDataDic[this.mCurChatTargetId.ToString()];
                for (int i = list.Count - 1; i >= 0; i--)
                {
                    if (num >= 5)
                    {
                        break;
                    }
                    MyChatData myChatData = list[i];
                    bool flag = false;
                    if (!myChatData.readed || i == list.Count - 1 || uint.Parse(myChatData.chattime) > this.friendController.loginTime)
                    {
                        flag = true;
                    }
                    else if (i < list.Count - 1 && (!list[i + 1].readed || uint.Parse(list[i + 1].chattime) > this.friendController.loginTime))
                    {
                        flag = true;
                    }
                    if (!flag && !this.oldChatDataList.Contains(myChatData))
                    {
                        this.oldChatDataList.Add(myChatData);
                        num++;
                        this.CreateChatItem(myChatData).transform.SetSiblingIndex(1);
                    }
                }
            }
        }
    }

    private GameObject CreateChatItem(MyChatData chatData)
    {
        GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.obj_chatitem);
        gameObject.transform.SetParent(this.obj_chatitem.transform.parent);
        gameObject.transform.localScale = Vector3.one;
        gameObject.SetActive(true);
        Transform transform = gameObject.transform.Find("title/name");
        Transform transform2 = gameObject.transform.Find("title/time");
        Transform transform3 = gameObject.transform.Find("title/day");
        Transform transform4 = gameObject.transform.Find("Text/Text");
        bool flag = chatData.charid == MainPlayer.Self.GetCharID().ToString();
        transform.GetComponent<Text>().text = chatData.charname;
        transform.GetComponent<Text>().color = Color.white;
        string timeText = SingletonForMono<GameTime>.Instance.GetTimeText(ulong.Parse(chatData.chattime));
        transform2.GetComponent<Text>().text = timeText.Split(new char[]
        {
            ' '
        })[1];
        transform3.GetComponent<Text>().text = timeText.Split(new char[]
        {
            ' '
        })[0];
        transform4.GetComponent<Text>().text = chatData.content;
        gameObject.GetComponent<VerticalLayoutGroup>().childAlignment = ((!flag) ? TextAnchor.UpperLeft : TextAnchor.UpperRight);
        string imgname = (!flag) ? "text_others" : "text_me";
        Image imgicon = transform4.GetComponent<Image>();
        ManagerCenter.Instance.GetManager<UITextureMgr>().GetTexture(ImageType.OTHERS, imgname, delegate (UITextureAsset asset)
        {
            if (asset == null)
            {
                FFDebug.LogWarning("UI_GmChat", "  req  texture   is  null ");
                return;
            }
            if (imgicon == null)
            {
                return;
            }
            Texture2D textureObj = asset.textureObj;
            Sprite sprite = Sprite.Create(textureObj, new Rect(0f, 0f, (float)textureObj.width, (float)textureObj.height), new Vector2(0.5f, 0.5f), 100f, 1U, SpriteMeshType.FullRect, new Vector4(16f, 0f, 16f, 0f));
            imgicon.sprite = sprite;
            imgicon.overrideSprite = sprite;
            imgicon.color = Color.white;
            imgicon.gameObject.SetActive(true);
            imgicon.material = null;
        });
        transform3.SetSiblingIndex((!flag) ? 2 : 0);
        transform3.GetComponent<Text>().alignment = ((!flag) ? TextAnchor.MiddleRight : TextAnchor.MiddleLeft);
        transform.SetSiblingIndex((!flag) ? 0 : 2);
        transform.GetComponent<Text>().alignment = ((!flag) ? TextAnchor.MiddleLeft : TextAnchor.MiddleRight);
        return gameObject;
    }

    private void lbl_input_onpointerclick(PointerEventData eventData)
    {
        this.RepositonScrollBarBottom();
    }

    public void InputFieldActive(bool isSend = false)
    {
        if (isSend)
        {
            if (this.lbl_input.text != string.Empty)
            {
                this.btn_send_on_click(null);
            }
            EventSystem.current.SetSelectedGameObject(null);
        }
        else
        {
            this.lbl_input.ActivateInputField();
        }
    }

    private Transform root;

    private GameObject obj_frienditem;

    private GameObject obj_chatitem;

    private GameObject obj_iconitem;

    private Scrollbar sb_chatcontent;

    private InputField lbl_input;

    private GameObject obj_face_container;

    public List<relation_item> list;

    private List<MyChatData> oldChatDataList = new List<MyChatData>();

    private FriendControllerNew friendController;

    private GmChatController mController;

    private bool isScrollEnd;

    private ulong mCurChatTargetId;

    private relation_item mCurChatTarget;
}
