using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIList : MonoBehaviour
{
    private void Awake()
    {
    }

    private void Start()
    {
    }

    public void Init(int count, Action<GameObject, int> drawcallback)
    {
        this.rect = base.GetComponentInChildren<ScrollRect>(true);
        if (this.rect == null)
        {
            this.content = base.gameObject;
        }
        if (this.content == null)
        {
            this.content = this.rect.content.gameObject;
        }
        if (this.item == null)
        {
            this.item = this.content.transform.GetChild(0).gameObject;
        }
        this.item.SetActive(false);
        if (this._count > 0)
        {
            this.Init(this._count, null);
        }
        this.Clear();
        for (int i = 0; i < count; i++)
        {
            GameObject gameObject;
            if (this.listItem.Count > i)
            {
                gameObject = this.listItem[i];
            }
            else
            {
                gameObject = UnityEngine.Object.Instantiate<GameObject>(this.item);
            }
            gameObject.transform.SetParent(this.content.transform, false);
            gameObject.SetActive(true);
            gameObject.name = i.ToString();
            this.listItem.Add(gameObject);
            if (drawcallback != null)
            {
                drawcallback(gameObject, i);
            }
        }
    }

    public void ResreshObj(Action<GameObject, int> drawcallback)
    {
        for (int i = 0; i < this.listItem.Count; i++)
        {
            if (drawcallback != null)
            {
                drawcallback(this.listItem[i], i);
            }
        }
    }

    public void Clear()
    {
        this.rect = base.GetComponentInChildren<ScrollRect>(true);
        if (this.rect == null)
        {
            this.content = base.gameObject;
        }
        if (this.content == null)
        {
            this.content = this.rect.content.gameObject;
        }
        for (int i = 1; i < this.content.transform.childCount; i++)
        {
            UnityEngine.Object.Destroy(this.content.transform.GetChild(i).gameObject);
        }
        this.listItem.Clear();
    }

    public int _count;

    private GameObject item;

    private GameObject content;

    private ScrollRect rect;

    private List<GameObject> listItem = new List<GameObject>();
}
