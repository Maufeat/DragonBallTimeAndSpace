using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UICirculationList : MonoBehaviour
{
    public void Init(int maxLength, bool ListSizeChange, Action<Transform, int> updateItem)
    {
        this._listSizeNeedChange = ListSizeChange;
        this.Init(maxLength, updateItem);
    }

    public void Init(int maxLength, Action<Transform, int> updateItem)
    {
        this.InitObj();
        this.m_MaxLength = maxLength;
        if (maxLength > 0)
        {
            this.m_listMaxLength = maxLength - 1;
        }
        this.m_updateItem = updateItem;
        this.InitItems();
    }

    private void InitObj()
    {
        this.m_itemParent = base.transform.Find("Rect").GetComponent<RectTransform>();
        this.m_scrollrect = base.transform.GetComponent<ScrollRect>();
        this.m_showArea = this.m_scrollrect.GetComponent<RectTransform>().sizeDelta;
        this.m_item = this.m_itemParent.Find("Item").gameObject;
        this.m_startrange = base.transform.Find("img_startrange");
        this.m_endrange = base.transform.Find("img_endrange");
        this.ResetThis();
        if (this.m_scrollrect.horizontal && !this.m_scrollrect.vertical)
        {
            this.m_Direction = ListType.Horizontal;
        }
        else if (!this.m_scrollrect.horizontal && this.m_scrollrect.vertical)
        {
            this.m_Direction = ListType.Vertical;
        }
        this.m_scrollrect.onValueChanged.RemoveAllListeners();
        this.m_scrollrect.onValueChanged.AddListener(new UnityAction<Vector2>(this.OnDragScroll));
        this.m_item.SetActive(false);
        this.m_firstItemPos.y = this.m_item.transform.localPosition.y;
        this.m_curShowStartIndex = 0;
    }

    private void InitItems()
    {
        this.TotalItemCount = Mathf.CeilToInt(this.m_showArea.y / (float)this.m_itemMinHeight) + 5;
        if (this.m_MaxLength > this.TotalItemCount)
        {
            this.m_curShowEndIndex = this.TotalItemCount - 1;
            this.m_maxShowIndex = this.TotalItemCount - 1;
            for (int i = 0; i < this.TotalItemCount; i++)
            {
                Transform arg = this.CreateItem(i);
                if (this.m_updateItem != null)
                {
                    this.m_updateItem(arg, i);
                }
            }
        }
        else if (this.m_MaxLength > 0)
        {
            this.m_curShowEndIndex = this.m_MaxLength - 1;
            this.m_maxShowIndex = this.m_MaxLength - 1;
            for (int j = 0; j < this.m_MaxLength; j++)
            {
                Transform arg2 = this.CreateItem(j);
                if (this.m_updateItem != null)
                {
                    this.m_updateItem(arg2, j);
                }
            }
        }
        else
        {
            this.m_curShowEndIndex = 0;
            this.m_maxShowIndex = 0;
        }
        this.SetLayout();
    }

    public void ReBuild(int newlength)
    {
        this.ResetThis();
        if (this.m_updateItem != null)
        {
            this.Init(newlength, this.m_updateItem);
        }
    }

    public void RefreshWhenListCountChanged(int newlength)
    {
        if (newlength < this.m_MaxLength)
        {
            this.ReBuild(newlength);
        }
        else if (newlength > this.m_MaxLength)
        {
            this.m_MaxLength = newlength;
            this.m_listMaxLength = newlength - 1;
            if (this.m_listMaxLength > this.TotalItemCount)
            {
                this.CheckMoveFirstToLast();
                this.CheckMoveLastToFirst();
            }
            else
            {
                this.m_curShowEndIndex = this.m_listMaxLength;
                this.m_maxShowIndex = this.m_listMaxLength;
                Transform transform = this.CreateItem(this.m_listMaxLength);
                if (this.m_updateItem != null)
                {
                    this.m_updateItem(transform, this.m_listMaxLength);
                }
                if (this.m_listMaxLength > 0)
                {
                    this.m_lastItemPos.y = this.m_lastItemPos.y - this.AllItems[this.m_listMaxLength - 1].GetComponent<LayoutElement>().preferredHeight;
                    transform.transform.localPosition = this.m_lastItemPos;
                }
                if (this._listSizeNeedChange)
                {
                    this.m_itemParent.sizeDelta += new Vector2(0f, transform.GetComponent<LayoutElement>().preferredHeight);
                }
                else
                {
                    this.m_itemParent.sizeDelta = new Vector2(0f, transform.GetComponent<LayoutElement>().preferredHeight * (float)this.m_MaxLength);
                }
            }
        }
    }

    public void SetNormalizedPositionByItemIndex(int index)
    {
        if (this.m_MaxLength > 0)
        {
            float normalizedPosition = 1f - Mathf.Clamp01((float)index / (float)this.m_MaxLength);
            this.SetNormalizedPosition(normalizedPosition);
        }
    }

    public void RecordNormalizedPosition()
    {
        this.LastNormalizedPosition = this.m_scrollrect.verticalNormalizedPosition;
        this.m_itemParentPos = this.m_itemParent.position;
        this.LastMaxLength = this.m_MaxLength;
    }

    public void RevertNormalizedPosition()
    {
        float num = 1f - (1f - this.LastNormalizedPosition) * (float)this.LastMaxLength / (float)this.m_MaxLength;
        num = Mathf.Clamp01(num);
        if (Mathf.Abs(num - this.m_scrollrect.verticalNormalizedPosition) > 1E-05f)
        {
            this.CheckMoveFirstToLast();
            this.CheckMoveLastToFirst();
        }
        this.m_itemParent.position = this.m_itemParentPos;
    }

    public void SetNormalizedPosition(float f)
    {
        if (this.TotalItemCount <= this.m_listMaxLength + 1)
        {
            while (Mathf.Abs(f - this.m_scrollrect.verticalNormalizedPosition) > 1E-05f)
            {
                this.m_scrollrect.verticalNormalizedPosition = f;
                this.CheckMoveFirstToLast();
                this.CheckMoveLastToFirst();
            }
        }
        else
        {
            this.m_scrollrect.verticalNormalizedPosition = f;
        }
    }

    private void ResetThis()
    {
        for (int i = 0; i < this.AllItems.Count; i++)
        {
            UnityEngine.Object.Destroy(this.AllItems[i]);
        }
        this.AllItems.Clear();
        this.m_firstItemPos = this.m_item.transform.localPosition;
        this.m_lastItemPos = this.m_firstItemPos;
        this.m_listMaxLength = 0;
        this.m_curShowStartIndex = 0;
        this.m_curShowEndIndex = 0;
        this.m_maxShowIndex = 0;
    }

    private Transform CreateItem(int index)
    {
        Transform transform = UnityEngine.Object.Instantiate<GameObject>(this.m_item).transform;
        transform.gameObject.SetActive(true);
        transform.SetParent(this.m_itemParent);
        transform.name = index.ToString();
        transform.localScale = Vector3.one;
        transform.localPosition = this.m_item.transform.localPosition;
        this.AllItems.Add(transform.gameObject);
        return transform;
    }

    private void SetLayout()
    {
        this.m_lastItemPos.y = this.m_item.transform.localPosition.y;
        this.m_itemParent.sizeDelta = Vector2.zero;
        if (this.AllItems.Count == 0)
        {
            return;
        }
        if (this._listSizeNeedChange)
        {
            this.m_itemParent.sizeDelta += new Vector2(0f, this.AllItems[0].GetComponent<LayoutElement>().preferredHeight);
        }
        else
        {
            this.m_itemParent.sizeDelta = new Vector2(0f, this.AllItems[0].GetComponent<LayoutElement>().preferredHeight * (float)this.m_MaxLength);
        }
        for (int i = 1; i < this.AllItems.Count; i++)
        {
            this.m_lastItemPos.y = this.m_lastItemPos.y - this.AllItems[i - 1].GetComponent<LayoutElement>().preferredHeight;
            this.AllItems[i].transform.localPosition = this.m_lastItemPos;
            if (this._listSizeNeedChange)
            {
                this.m_itemParent.sizeDelta += new Vector2(0f, this.AllItems[i].GetComponent<LayoutElement>().preferredHeight);
            }
        }
    }

    public void RefreshShowItem()
    {
        for (int i = 0; i < this.AllItems.Count; i++)
        {
            int arg = this.m_curShowStartIndex + i;
            if (this.m_updateItem != null)
            {
                this.m_updateItem(this.AllItems[i].transform, arg);
            }
        }
    }

    private void OnDragScroll(Vector2 v2)
    {
        if (this.m_listMaxLength < this.TotalItemCount)
        {
            return;
        }
        if (this.m_itemParent.localPosition.y - this.m_lastItmeParentPosition > 0f && this.m_scrollrect.verticalNormalizedPosition <= 1f)
        {
            this.CheckMoveFirstToLast();
        }
        else if (this.m_itemParent.localPosition.y - this.m_lastItmeParentPosition < 0f && this.m_scrollrect.verticalNormalizedPosition >= 0f)
        {
            this.CheckMoveLastToFirst();
        }
        this.m_lastItmeParentPosition = this.m_itemParent.localPosition.y;
    }

    private void CheckMoveFirstToLast()
    {
        float num = 0f;
        for (int i = this.AllItems.Count - 1; i > this.AllItems.Count - 5; i--)
        {
            num += this.AllItems[i].GetComponent<LayoutElement>().preferredHeight;
        }
        while (Mathf.Abs(this.AllItems[0].transform.localPosition.y + this.m_itemParent.localPosition.y - this.m_startrange.localPosition.y) > num)
        {
            if (this.m_curShowEndIndex == this.m_listMaxLength)
            {
                return;
            }
            GameObject gameObject = this.AllItems[0];
            float preferredHeight = gameObject.GetComponent<LayoutElement>().preferredHeight;
            this.AllItems.RemoveAt(0);
            this.AllItems.Add(gameObject);
            if (this.m_updateItem != null)
            {
                this.m_updateItem(gameObject.transform, this.m_curShowEndIndex + 1);
            }
            this.m_curShowStartIndex++;
            this.m_curShowEndIndex++;
            if (this.m_maxShowIndex < this.m_curShowEndIndex)
            {
                this.m_maxShowIndex = this.m_curShowEndIndex;
                if (this._listSizeNeedChange)
                {
                    this.m_itemParent.sizeDelta += new Vector2(0f, gameObject.GetComponent<LayoutElement>().preferredHeight);
                }
            }
            this.m_lastItemPos.y = this.m_lastItemPos.y - this.AllItems[this.AllItems.Count - 2].GetComponent<LayoutElement>().preferredHeight;
            this.m_firstItemPos.y = this.m_firstItemPos.y - preferredHeight;
            gameObject.transform.localPosition = this.m_lastItemPos;
        }
    }

    private void CheckMoveLastToFirst()
    {
        float num = 0f;
        for (int i = 0; i < 4; i++)
        {
            num += this.AllItems[i].GetComponent<LayoutElement>().preferredHeight;
        }
        while (Mathf.Abs(this.AllItems[this.AllItems.Count - 1].transform.localPosition.y + this.m_itemParent.localPosition.y - this.m_endrange.localPosition.y) + this.AllItems[this.AllItems.Count - 1].GetComponent<LayoutElement>().preferredHeight > num)
        {
            if (this.m_curShowStartIndex == 0)
            {
                return;
            }
            GameObject gameObject = this.AllItems[this.AllItems.Count - 1];
            float preferredHeight = this.AllItems[this.AllItems.Count - 2].GetComponent<LayoutElement>().preferredHeight;
            this.AllItems.RemoveAt(this.AllItems.Count - 1);
            this.AllItems.Insert(0, gameObject);
            if (this.m_updateItem != null)
            {
                this.m_updateItem(gameObject.transform, this.m_curShowStartIndex - 1);
            }
            this.m_curShowStartIndex--;
            this.m_curShowEndIndex--;
            this.m_lastItemPos.y = this.m_lastItemPos.y + preferredHeight;
            this.m_firstItemPos.y = this.m_firstItemPos.y + gameObject.GetComponent<LayoutElement>().preferredHeight;
            gameObject.transform.localPosition = this.m_firstItemPos;
        }
    }

    private ScrollRect m_scrollrect;

    private RectTransform m_itemParent;

    private GameObject m_item;

    public int m_itemMinHeight;

    private Vector2 m_showArea = Vector2.zero;

    private Vector3 m_firstItemPos = Vector3.zero;

    private Vector3 m_lastItemPos = Vector3.zero;

    private int m_listMaxLength;

    private int m_curShowStartIndex;

    private int m_curShowEndIndex;

    private int m_maxShowIndex;

    private Action<Transform, int> m_updateItem;

    private int TotalItemCount;

    [HideInInspector]
    public List<GameObject> AllItems = new List<GameObject>();

    private Transform m_startrange;

    private Transform m_endrange;

    private ListType m_Direction;

    private int m_MaxLength;

    private bool _listSizeNeedChange = true;

    private int LastMaxLength;

    private float LastNormalizedPosition;

    private Vector3 m_itemParentPos;

    private float m_lastItmeParentPosition;
}
