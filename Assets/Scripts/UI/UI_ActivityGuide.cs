using System;
using System.Collections.Generic;
using Framework.Managers;
using UnityEngine;
using UnityEngine.UI;

public class UI_ActivityGuide : UIPanelBase
{
    private ActivityController activityController
    {
        get
        {
            return ControllerManager.Instance.GetController<ActivityController>();
        }
    }

    public override void OnInit(Transform root)
    {
        base.OnInit(root);
        this.InitGameObject(root);
    }

    private void InitGameObject(Transform root)
    {
        this.mRoot = root;
        this.btnClose = this.mRoot.Find("Offset_ActivityGuide/Panel_Large/title/btn_close");
        this.slider_CurVitality = this.mRoot.Find("Offset_ActivityGuide/Panel_Large/Point/progress").GetComponent<Image>();
        this.txtActivity = this.mRoot.Find("Offset_ActivityGuide/Panel_Large/Point/txt_Activity").GetComponent<Text>();
        this.activityAwardItemSkin = this.mRoot.Find("Offset_ActivityGuide/Panel_Large/Point/Award/item").gameObject;
        this.menuSkin = this.mRoot.Find("Offset_ActivityGuide/Panel_Large/Panel/bftype/ScrollRect/grid/item").gameObject;
        this.menuItemSkin = this.mRoot.Find("Offset_ActivityGuide/Panel_Large/Panel/item").gameObject;
        this.menuItemParent = this.mRoot.Find("Offset_ActivityGuide/Panel_Large/Panel/Area/item");
        this.infoRoot = this.mRoot.Find("Offset_ActivityGuide/Panel_Info");
        this.infoTile = this.mRoot.Find("Offset_ActivityGuide/Panel_Info/title").GetComponent<Text>();
        this.infoTime = this.mRoot.Find("Offset_ActivityGuide/Panel_Info/time/txt_nolimit").GetComponent<Text>();
        this.infoType = this.mRoot.Find("Offset_ActivityGuide/Panel_Info/type/txt_nolimit").GetComponent<Text>();
        this.infoLimit = this.mRoot.Find("Offset_ActivityGuide/Panel_Info/limit/txt_nolimit").GetComponent<Text>();
        this.infoDetail = this.mRoot.Find("Offset_ActivityGuide/Panel_Info/detail/txt_nolimit").GetComponent<Text>();
        this.infoNotice = this.mRoot.Find("Offset_ActivityGuide/Panel_Info/notice/txt_nolimit").GetComponent<Text>();
        this.noticeShow = this.mRoot.Find("Offset_ActivityGuide/Panel_Info/notice").gameObject;
        this.infoAwardParent = this.mRoot.Find("Offset_ActivityGuide/Panel_Info/award/list");
        this.infoAwardSkin = this.mRoot.Find("Offset_ActivityGuide/Panel_Info/award/list/Item").gameObject;
        this.btnJoin = this.mRoot.Find("Offset_ActivityGuide/Panel_Info/Button_join").gameObject;
        this.menuItemParent.gameObject.SetActive(false);
        this.activityAwardItemSkin.SetActive(false);
        this.menuSkin.SetActive(false);
        this.menuItemSkin.SetActive(false);
        this.infoAwardSkin.SetActive(false);
        this.infoRoot.gameObject.SetActive(false);
    }

    public UI_ActivityGuide.MenuSkin AddNewMenu(string name)
    {
        UI_ActivityGuide.MenuSkin menuSkin = new UI_ActivityGuide.MenuSkin();
        GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.menuSkin);
        gameObject.transform.SetParent(this.menuSkin.transform.parent);
        gameObject.transform.Reset();
        menuSkin.SetSkin(gameObject);
        menuSkin.name.text = name;
        gameObject.SetActive(true);
        return menuSkin;
    }

    public UI_ActivityGuide.MenuItemSkin AddNewMenuItem(int menuIndex, string name, int timeslimit, int peractive, int levellimit)
    {
        UI_ActivityGuide.MenuItemSkin menuItemSkin = new UI_ActivityGuide.MenuItemSkin();
        GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.menuItemSkin);
        gameObject.transform.SetParent(this.GetMenuItemParent(menuIndex).content);
        gameObject.transform.SetSiblingIndex(levellimit);
        gameObject.transform.Reset();
        menuItemSkin.SetSkin(gameObject);
        menuItemSkin.name.text = name;
        menuItemSkin.timesProgress.text = 0 + "/" + timeslimit;
        menuItemSkin.activeProgress.text = 0 + "/" + peractive * timeslimit;
        menuItemSkin.levelLimit.text = levellimit + "级开启";
        gameObject.SetActive(true);
        return menuItemSkin;
    }

    public UI_ActivityGuide.RewardItemSkin AddActiveRewardItem(int active, string reward)
    {
        UI_ActivityGuide.RewardItemSkin rewardItemSkin = new UI_ActivityGuide.RewardItemSkin();
        GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.activityAwardItemSkin);
        gameObject.transform.SetParent(this.activityAwardItemSkin.transform.parent);
        gameObject.transform.SetSiblingIndex(active);
        gameObject.transform.Reset();
        rewardItemSkin.SetSkin(gameObject);
        string[] array = reward.Split(new char[]
        {
            '-'
        });
        uint num = uint.Parse(array[0]);
        string text = array[1];
        gameObject.SetActive(true);
        return rewardItemSkin;
    }

    public UI_ActivityGuide.MenuItemParentSkin GetMenuItemParent(int menuIndex)
    {
        UI_ActivityGuide.MenuItemParentSkin menuItemParentSkin;
        if (!this.parentDic.TryGetValue(menuIndex, out menuItemParentSkin))
        {
            Transform transform = UnityEngine.Object.Instantiate<GameObject>(this.menuItemParent.gameObject).transform;
            transform.SetParent(this.menuItemParent.parent);
            transform.SetSiblingIndex(menuIndex);
            transform.localPosition = this.menuItemParent.localPosition;
            transform.localRotation = this.menuItemParent.localRotation;
            transform.localScale = this.menuItemParent.localScale;
            (transform as RectTransform).offsetMin = Vector2.zero;
            (transform as RectTransform).offsetMax = Vector2.zero;
            transform.name = menuIndex.ToString();
            menuItemParentSkin = new UI_ActivityGuide.MenuItemParentSkin();
            menuItemParentSkin.SetSkin(transform.gameObject);
            this.parentDic.Add(menuIndex, menuItemParentSkin);
        }
        return menuItemParentSkin;
    }

    public void UpdateActivity(int curActivity, int maxActivity)
    {
        if (maxActivity == 0)
        {
            return;
        }
        this.slider_CurVitality.fillAmount = Mathf.Lerp(0f, 1f, (float)curActivity * 1f / (float)maxActivity);
        this.txtActivity.text = string.Format("{0}/{1}", curActivity, maxActivity);
    }

    public void UpdateShowInfoAwards(int length)
    {
        int count = this.infoAwards.Count;
        if (length == count)
        {
            return;
        }
        if (length < count)
        {
            for (int i = length; i < count; i++)
            {
                this.infoAwards[i].skin.SetActive(false);
            }
            return;
        }
        if (length > count)
        {
            for (int j = count; j < length; j++)
            {
                UI_ActivityGuide.InfoAwardItemSkin infoAwardItemSkin = new UI_ActivityGuide.InfoAwardItemSkin();
                GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.infoAwardSkin);
                gameObject.transform.SetParent(this.infoAwardSkin.transform.parent);
                gameObject.transform.SetSiblingIndex(j);
                gameObject.transform.Reset();
                infoAwardItemSkin.SetSkin(gameObject);
                this.infoAwards.Add(infoAwardItemSkin);
            }
        }
    }

    public UI_ActivityGuide.InfoAwardItemSkin GetInfoAward(int i)
    {
        if (i > this.infoAwards.Count)
        {
            return null;
        }
        UI_ActivityGuide.InfoAwardItemSkin infoAwardItemSkin = this.infoAwards[i];
        infoAwardItemSkin.skin.SetActive(true);
        return infoAwardItemSkin;
    }

    public override void OnDispose()
    {
        base.OnDispose();
        this.activityController.OnDispos();
        this.DisposeEvent();
        this.DisposeGameObject();
    }

    private void DisposeEvent()
    {
    }

    private void DisposeGameObject()
    {
        if (this.mRoot != null)
        {
            UnityEngine.Object.Destroy(this.mRoot.gameObject);
            this.mRoot = null;
        }
    }

    public Transform mRoot;

    public Transform btnClose;

    public Transform infoRoot;

    private Image slider_CurVitality;

    public Text txtActivity;

    public Text infoTile;

    public Text infoTime;

    public Text infoType;

    public Text infoLimit;

    public Text infoDetail;

    public Text infoNotice;

    public GameObject noticeShow;

    public Transform infoAwardParent;

    public GameObject infoAwardSkin;

    public GameObject btnJoin;

    private GameObject activityAwardItemSkin;

    private GameObject menuSkin;

    private GameObject menuItemSkin;

    private Transform menuItemParent;

    private Dictionary<int, UI_ActivityGuide.MenuItemParentSkin> parentDic = new Dictionary<int, UI_ActivityGuide.MenuItemParentSkin>();

    private List<UI_ActivityGuide.InfoAwardItemSkin> infoAwards = new List<UI_ActivityGuide.InfoAwardItemSkin>();

    public class MenuSkin
    {
        public void SetSkin(GameObject skin)
        {
            this.skin = skin;
            this.btn = skin.GetComponent<Button>();
            this.name = skin.transform.Find("txt_name").GetComponent<Text>();
            this.selected = skin.transform.Find("img_bar_hover").gameObject;
        }

        public GameObject skin;

        public Button btn;

        public Text name;

        public GameObject selected;
    }

    public class MenuItemParentSkin
    {
        public void SetSkin(GameObject skin)
        {
            this.skin = skin;
            this.scrollRect = skin.transform.Find("ScrollView").GetComponent<ScrollRect>();
            this.content = skin.transform.Find("ScrollView/content");
            this.bar = this.scrollRect.verticalScrollbar;
        }

        public GameObject skin;

        public ScrollRect scrollRect;

        public Transform content;

        public Scrollbar bar;
    }

    public class MenuItemSkin
    {
        public void SetSkin(GameObject skin)
        {
            this.skin = skin;
            this.btn = skin.GetComponent<Button>();
            this.name = skin.transform.Find("txt_name").GetComponent<Text>();
            this.timesProgress = skin.transform.Find("times/txt_times").GetComponent<Text>();
            this.activeProgress = skin.transform.Find("point/txt_times").GetComponent<Text>();
            this.selected = skin.transform.Find("img_select").gameObject;
            this.levelLimit = skin.transform.Find("level").GetComponent<Text>();
        }

        public GameObject skin;

        public Button btn;

        public Text name;

        public Text timesProgress;

        public Text activeProgress;

        public GameObject selected;

        public Text levelLimit;
    }

    public class RewardItemSkin
    {
        public void SetSkin(GameObject skin)
        {
            this.skin = skin;
            this.btn = skin.GetComponent<Button>();
            this.img_0 = skin.transform.Find("img_geted").GetComponent<Image>();
            this.img_1 = skin.transform.Find("img_unopen").GetComponent<Image>();
            this.img_2 = skin.transform.Find("img_opened").GetComponent<Image>();
            this.img_0.gameObject.SetActive(false);
            this.img_1.gameObject.SetActive(true);
            this.img_2.gameObject.SetActive(false);
        }

        public GameObject skin;

        public Button btn;

        public Image img_0;

        public Image img_1;

        public Image img_2;
    }

    public class InfoAwardItemSkin
    {
        public void SetSkin(GameObject skin)
        {
            this.skin = skin;
            this.hoverObj = skin.transform.Find("img_bg").gameObject;
            this.img = skin.transform.Find("img_icon").GetComponent<Image>();
            this.img.enabled = false;
            this.img.gameObject.SetActive(true);
        }

        public GameObject skin;

        public GameObject hoverObj;

        public Image img;
    }
}
