using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIFoldOutList : MonoBehaviour
{
    private void Awake()
    {
        if (this.bUIFoldout)
        {
            for (int i = 0; i < this.FoldItems.Count; i++)
            {
                UIFoldItem tmpItem = this.FoldItems[i];
                if (tmpItem.myButton != null)
                {
                    tmpItem.myButton.onClick.AddListener(delegate ()
                    {
                        this.ClickFoldItems(tmpItem);
                    });
                }
            }
        }
        this.mRect = base.transform.Find("Rect");
        if (this.mRect.Find("btn_last") != null)
        {
            this.btn_last = this.mRect.Find("btn_last");
        }
        if (this.mRect.Find("btn_next") != null)
        {
            this.btn_next = this.mRect.Find("btn_next");
        }
    }

    private void initPage(UnityAction pageeup, UnityAction pagedown)
    {
        if (this.btn_last != null && this.btn_next != null)
        {
            this.btn_last.GetComponent<Button>().onClick.RemoveAllListeners();
            this.btn_last.GetComponent<Button>().onClick.RemoveAllListeners();
            this.btn_next.GetComponent<Button>().onClick.RemoveAllListeners();
            this.btn_last.GetComponent<Button>().onClick.AddListener(pageeup);
            this.btn_next.GetComponent<Button>().onClick.RemoveAllListeners();
            this.btn_next.GetComponent<Button>().onClick.AddListener(pagedown);
            this.btn_last.gameObject.SetActive(false);
            this.btn_next.gameObject.SetActive(false);
        }
    }

    public void ClickFoldItems(UIFoldItem item)
    {
        if (item.gameObject == this.currItem)
        {
            this.currItem = null;
            item.startTweenScale();
        }
        else
        {
            if (this.currItem != null)
            {
                this.currItem.GetComponent<UIFoldItem>().startTweenScale();
            }
            this.currItem = item.gameObject;
            item.startTweenScale();
        }
    }

    public GameObject AddItem()
    {
        if (this.Item == null)
        {
            UIFoldItem componentInChildren = base.gameObject.GetComponentInChildren<UIFoldItem>();
            if (componentInChildren != null)
            {
                this.Item = componentInChildren.gameObject;
            }
        }
        GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.Item);
        gameObject.transform.SetParent(this.mRect);
        gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
        gameObject.transform.localPosition = new Vector3(0f, 0f, 0f);
        if (this.bUIFoldout)
        {
            LayoutElement tmpElement = this.Item.GetComponent<UIFoldItem>().tmpElement;
            if (tmpElement != null)
            {
                GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(tmpElement.gameObject);
                gameObject.GetComponent<UIFoldItem>().tmpElement = gameObject2.GetComponent<LayoutElement>();
                gameObject.GetComponent<UIFoldItem>().tweenFinish = new Action(this.setScrollBar);
                gameObject2.transform.SetParent(this.mRect);
                gameObject2.transform.localScale = new Vector3(1f, 1f, 1f);
                gameObject2.transform.localPosition = new Vector3(0f, 0f, 0f);
                gameObject2.SetActive(true);
            }
            this.AddItem(gameObject.GetComponent<UIFoldItem>());
        }
        gameObject.SetActive(true);
        return gameObject;
    }

    public void AddItem(UIFoldItem item)
    {
        if (item.myButton != null)
        {
            item.myButton.onClick.AddListener(delegate ()
            {
                this.ClickFoldItems(item);
            });
        }
        this.FoldItems.Add(item);
    }

    public void InitList(int totalcount, int itemcountofpage, int type = 1)
    {
        this.listtype = (ListType)type;
        this.scroll = base.transform.GetComponent<ScrollRect>();
        this.m_totalCount = totalcount;
        this.m_itemCountOfPage = itemcountofpage;
        this.m_pageCount = (int)Math.Ceiling((double)((float)this.m_totalCount / (float)this.m_itemCountOfPage));
        if (this.bUIFoldout && this.Item.GetComponent<UIFoldItem>().tmpElement != null)
        {
            this.Item.GetComponent<UIFoldItem>().tmpElement.gameObject.SetActive(false);
        }
        this.Item.SetActive(false);
        this.currPage = 1;
        this.initPage(new UnityAction(this.PageUp), new UnityAction(this.PageDown));
        this.refreshPage();
        this.InitCurrPageItem();
        this.resetUIFoldOut();
    }

    public void resetUIFoldOut()
    {
        if (this.currItem != null)
        {
            this.currItem.GetComponent<UIFoldItem>().resetCantainView();
            this.currItem = null;
        }
    }

    public void SetPageInfo(int curPage, int pageCount, int itemCount, UnityAction pageup, UnityAction pagedown)
    {
        FFDebug.Log(this, FFLogType.UI, string.Concat(new object[]
        {
            "curPage : ",
            curPage,
            "   pageCount : ",
            pageCount,
            "  itemCount : ",
            itemCount
        }));
        this.scrollBarDisable = false;
        this.Item.SetActive(false);
        if (this.Item.GetComponent<UIFoldItem>().tmpElement != null)
        {
            this.Item.GetComponent<UIFoldItem>().tmpElement.gameObject.SetActive(false);
        }
        if (itemCount == 0)
        {
            if (this.btn_last != null)
            {
                this.btn_last.gameObject.SetActive(false);
            }
            if (this.btn_next != null)
            {
                this.btn_next.gameObject.SetActive(false);
            }
        }
        this.scroll = base.transform.GetComponent<ScrollRect>();
        this.startIndex = 0;
        this.m_itemCountOfPage = itemCount;
        this.currPage = curPage;
        this.m_totalCount = itemCount;
        this.m_pageCount = pageCount;
        this.InitCurrPageItem(0);
        this.initPage(pageup, pagedown);
        this.refreshPage();
    }

    public int GetCurrentPage()
    {
        return this.currPage;
    }

    public int GetPageCount()
    {
        return this.m_pageCount;
    }

    private void InitCurrPageItem()
    {
        this.InitCurrPageItem((this.currPage - 1) * this.m_itemCountOfPage);
    }

    private void InitCurrPageItem(int start)
    {
        this.startIndex = start;
        for (int i = 0; i < this.objlist.Count; i++)
        {
            if (this.bUIFoldout)
            {
                this.objlist[i].GetComponent<UIFoldItem>().resetCantainView();
            }
            this.objlist[i].SetActive(false);
        }
        for (int j = this.startIndex; j < this.m_itemCountOfPage; j++)
        {
            if (j >= this.m_totalCount)
            {
                break;
            }
            int num = j % this.m_itemCountOfPage;
            GameObject gameObject;
            if (this.objlist.Count > num)
            {
                gameObject = this.objlist[num];
            }
            else
            {
                gameObject = this.AddItem();
                this.objlist.Add(gameObject);
            }
            gameObject.name = j.ToString();
            gameObject.SetActive(true);
            if (gameObject == null)
            {
                return;
            }
            if (this.InitListAction != null)
            {
                this.InitListAction(j, gameObject);
            }
        }
        Canvas.ForceUpdateCanvases();
        this.setScrollBar();
    }

    private void refreshPage()
    {
        if (this.currPage > 1)
        {
            if (this.btn_last != null)
            {
                this.btn_last.gameObject.SetActive(true);
                this.btn_last.Find("txt_lastpage").GetComponent<Text>().text = "第" + (this.currPage - 1).ToString() + "页";
            }
        }
        else if (this.btn_last != null)
        {
            this.btn_last.gameObject.SetActive(false);
        }
        if (this.currPage < this.m_pageCount)
        {
            if (this.btn_next != null)
            {
                this.btn_next.SetParent(null);
                this.btn_next.SetParent(this.mRect);
                this.btn_next.gameObject.SetActive(true);
                this.btn_next.Find("txt_nextpage").GetComponent<Text>().text = "第" + (this.currPage + 1).ToString() + "页";
            }
        }
        else if (this.btn_next != null)
        {
            this.btn_next.gameObject.SetActive(false);
        }
    }

    private void PageUp()
    {
        this.currPage--;
        this.refreshPage();
        this.InitCurrPageItem();
    }

    private void PageDown()
    {
        this.currPage++;
        this.refreshPage();
        this.InitCurrPageItem();
    }

    public void SetScrollBarAutoDisable(bool b)
    {
        this.scrollBarDisable = b;
    }

    private void setScrollBar()
    {
        if (this.scrollBarDisable)
        {
            if (base.transform.GetComponent<RectTransform>().rect.height > this.mRect.GetComponent<RectTransform>().rect.height)
            {
                this.scroll.enabled = false;
                if (this.scroll.verticalScrollbar != null)
                {
                    this.scroll.verticalScrollbar.gameObject.SetActive(false);
                }
            }
            else
            {
                this.scroll.enabled = true;
                if (this.scroll.verticalScrollbar != null)
                {
                    this.scroll.verticalScrollbar.gameObject.SetActive(true);
                }
            }
        }
    }

    public List<UIFoldItem> FoldItems = new List<UIFoldItem>();

    private Transform btn_last;

    private Transform btn_next;

    private GameObject currItem;

    public GameObject Item;

    private Transform mRect;

    private int startIndex;

    private int endIndex;

    private bool scrollBarDisable = true;

    private ScrollRect scroll;

    private ListType listtype = ListType.Vertical;

    public bool bUIFoldout = true;

    private List<GameObject> objlist = new List<GameObject>();

    private int m_totalCount;

    private int m_itemCountOfPage;

    private int currPage = 1;

    public Action<int, GameObject> InitListAction;

    private int m_pageCount;
}
