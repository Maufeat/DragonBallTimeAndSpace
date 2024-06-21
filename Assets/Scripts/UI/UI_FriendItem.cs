using System;
using Framework.Managers;
using relation;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_FriendItem : MonoBehaviour
{
    private void OnInit()
    {
        Transform transform = base.transform.Find("HeadIcon");
        Transform transform2 = transform.Find("vip");
        Transform transform3 = base.transform.Find("Name");
        Transform transform4 = base.transform.Find("level");
        Transform transform5 = base.transform.Find("Online");
        Transform transform6 = base.transform.Find("Offline");
        Transform transform7 = base.transform.Find("releation");
        Transform transform8 = base.transform.Find("GainTiliButton");
        Transform transform9 = base.transform.Find("SendTiliButton");
        Transform transform10 = base.transform.Find("time");
        Transform transform11 = base.transform.Find("Image");
        Transform transform12 = base.transform.Find("NewFriendTag");
        Transform transform13 = base.transform.Find("img_h");
        if (transform13 != null)
        {
            this.img_HighLight = transform13.GetComponent<Image>();
        }
        if (transform)
        {
            this.img_head = transform.GetComponent<Image>();
        }
        if (transform2)
        {
            this.lbl_vip = transform2.GetComponent<Text>();
        }
        if (transform3)
        {
            this.lbl_name = transform3.GetComponent<Text>();
        }
        if (transform4)
        {
            this.lbl_level = transform4.GetComponent<Text>();
        }
        if (transform5)
        {
            this.obj_online = transform5.gameObject;
        }
        if (transform6)
        {
            this.obj_offline = transform6.gameObject;
        }
        if (transform7)
        {
            this.obj_relation = transform7.gameObject;
        }
        if (transform8)
        {
            this.btn_gain_tili = transform8.gameObject;
        }
        if (transform9)
        {
            this.btn_send_tili = transform9.gameObject;
        }
        if (transform10)
        {
            this.lbl_time = transform10.GetComponent<Text>();
        }
        if (transform11)
        {
            this.obj_tip = transform11.gameObject;
        }
        if (transform12)
        {
            this.obj_newfriendtag = transform12.gameObject;
        }
        UIEventListener.Get(base.gameObject).onClick = new UIEventListener.VoidDelegate(this.btn_second_onClick);
        UIEventListener.Get(base.gameObject).onEnter = new UIEventListener.VoidDelegate(this.btn_second_onEnter);
        UIEventListener.Get(base.gameObject).onExit = new UIEventListener.VoidDelegate(this.btn_second_onExit);
    }

    public void InitApplyButton()
    {
        Transform transform = base.transform.parent.Find(this.itemData.relationid.ToString()).Find("AcceptButton");
        Transform transform2 = base.transform.parent.Find(this.itemData.relationid.ToString()).Find("UnAcceptButton");
        if (transform)
        {
            this.btn_Accept = transform.GetComponent<Button>();
            this.btn_Accept.onClick.RemoveAllListeners();
            this.btn_Accept.onClick.AddListener(delegate ()
            {
                ControllerManager.Instance.GetController<FriendControllerNew>().ReqAnswerApplyRelation(this.itemData.relationid, 1U);
            });
            this.btn_Accept.gameObject.SetActive(false);
        }
        if (transform2)
        {
            this.btn_UnAccept = transform2.GetComponent<Button>();
            this.btn_UnAccept.onClick.RemoveAllListeners();
            this.btn_UnAccept.onClick.AddListener(delegate ()
            {
                ControllerManager.Instance.GetController<FriendControllerNew>().ReqAnswerApplyRelation(this.itemData.relationid, 0U);
            });
            this.btn_UnAccept.gameObject.SetActive(false);
        }
    }

    private void btn_second_onClick(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Right)
        {
            return;
        }
        UI_FriendNew uiobject = UIManager.GetUIObject<UI_FriendNew>();
        if (uiobject != null)
        {
            uiobject.btn_second_on_click(eventData, this.itemData);
        }
    }

    private void btn_second_onEnter(PointerEventData data)
    {
        if (this.img_HighLight != null)
        {
            this.img_HighLight.gameObject.SetActive(true);
        }
        if (this.btn_Accept == null && this.btn_UnAccept == null)
        {
            this.InitApplyButton();
        }
        if (this.btn_Accept != null)
        {
            this.btn_Accept.gameObject.SetActive(true);
        }
        if (this.btn_UnAccept != null)
        {
            this.btn_UnAccept.gameObject.SetActive(true);
        }
    }

    private void btn_second_onExit(PointerEventData data)
    {
        if (this.img_HighLight != null)
        {
            this.img_HighLight.gameObject.SetActive(false);
        }
        if (this.btn_Accept == null && this.btn_UnAccept == null)
        {
            this.InitApplyButton();
        }
        if (this.btn_Accept != null)
        {
            this.btn_Accept.gameObject.SetActive(false);
        }
        if (this.btn_UnAccept != null)
        {
            this.btn_UnAccept.gameObject.SetActive(false);
        }
    }

    public void Initilize(relation_item item)
    {
        this.itemData = item;
        this.OnInit();
        if (this.img_head)
        {
        }
        if (this.lbl_vip)
        {
            this.lbl_vip.text = item.viplevel.ToString();
        }
        if (this.lbl_name)
        {
            this.lbl_name.text = ((!string.IsNullOrEmpty(item.nickName) && !(item.nickName == item.relationname)) ? ("(" + item.nickName + ")") : string.Empty) + item.relationname;
        }
        if (this.lbl_level)
        {
            this.lbl_level.text = item.level + "级";
        }
        if (this.obj_online)
        {
            this.obj_online.SetActive(item.status == 1U);
        }
        if (this.obj_offline)
        {
            this.obj_offline.SetActive(item.status == 0U);
            if (item.status == 0U)
            {
                this.obj_offline.GetComponent<Text>().text = CommonTools.GetOfflineText(item.offlineTime);
            }
        }
        if (item.status == 0U)
        {
            Color color = new Color(0.3882353f, 0.6117647f, 0.827451f);
            this.lbl_name.color = color;
            this.lbl_level.color = color;
        }
        else
        {
            this.lbl_name.color = Color.white;
            this.lbl_level.color = Color.white;
        }
        if (this.obj_relation)
        {
            for (int i = 0; i < this.obj_relation.transform.childCount; i++)
            {
                this.obj_relation.transform.GetChild(i).gameObject.SetActive((long)i < (long)((ulong)item.love_degree));
            }
        }
        if (this.btn_gain_tili)
        {
            this.btn_gain_tili.SetActive(false);
        }
        if (this.btn_send_tili)
        {
            this.btn_send_tili.SetActive(false);
        }
        if (this.lbl_time)
        {
            this.lbl_time.text = SingletonForMono<GameTime>.Instance.GetServerDateTimeByTimeStamp((ulong)item.createTime * 1000UL).ToString("yyyy-MM-dd HH:mm:ss");
            this.lbl_time.gameObject.SetActive(true);
        }
        if (this.obj_tip)
        {
            bool active = ControllerManager.Instance.GetController<FriendControllerNew>().isFriendHasUnReadMsg(item.relationid.ToString());
            this.obj_tip.SetActive(active);
        }
        if (this.obj_newfriendtag)
        {
            bool active2 = ControllerManager.Instance.GetController<FriendControllerNew>().JudgeNewFriend(item.relationid.ToString());
            this.obj_newfriendtag.SetActive(active2);
        }
    }

    private Image img_head;

    private Image img_HighLight;

    private Text lbl_vip;

    private Text lbl_name;

    private Text lbl_level;

    private GameObject obj_online;

    private GameObject obj_offline;

    private GameObject obj_relation;

    private GameObject btn_gain_tili;

    private GameObject btn_send_tili;

    private GameObject obj_tip;

    private GameObject obj_newfriendtag;

    private Button btn_Accept;

    private Button btn_UnAccept;

    private Text lbl_time;

    private relation_item itemData;
}
