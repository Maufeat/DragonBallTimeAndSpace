using System;
using System.Collections.Generic;
using UnityEngine;

public enum LayOutTween
{
    left,
    top,
    right,
    bottom
}

public class LayoutTween : MonoBehaviour
{
    private void Start()
    {
        this.setChild();
    }

    private int activeChildCount()
    {
        this.listActiveObj.Clear();
        for (int i = 0; i < base.transform.childCount; i++)
        {
            Transform child = base.transform.GetChild(i);
            if (child.gameObject.activeSelf)
            {
                if (!this.listActiveObj.Contains(child))
                {
                    this.listActiveObj.Add(child);
                }
            }
            else if (this.listActiveObj.Contains(child))
            {
                this.listActiveObj.Remove(child);
            }
        }
        return this.listActiveObj.Count;
    }

    public void setChild()
    {
        this.childCount = this.activeChildCount();
        float num = 0f;
        switch (this.type)
        {
            case LayOutTween.left:
                num = this.offsetInit;
                break;
            case LayOutTween.top:
                num = 0f - this.offsetInit;
                break;
            case LayOutTween.right:
                num = 0f - this.offsetInit;
                break;
            case LayOutTween.bottom:
                num = this.offsetInit;
                break;
        }
        for (int i = 0; i < this.childCount; i++)
        {
            Transform transform = this.listActiveObj[i];
            switch (this.type)
            {
                case LayOutTween.left:
                    transform.localPosition = new Vector3(num, 0f, 0f);
                    num += transform.GetComponent<RectTransform>().rect.width * transform.localScale.x + this.offsetElement;
                    break;
                case LayOutTween.top:
                    transform.localPosition = new Vector3(0f, num, 0f);
                    num -= transform.GetComponent<RectTransform>().rect.height * transform.localScale.y + this.offsetElement;
                    break;
                case LayOutTween.right:
                    transform.localPosition = new Vector3(num, 0f, 0f);
                    num -= transform.GetComponent<RectTransform>().rect.width * transform.localScale.x + this.offsetElement;
                    break;
                case LayOutTween.bottom:
                    transform.localPosition = new Vector3(0f, num + this.offsetElement, 0f);
                    num += transform.GetComponent<RectTransform>().rect.height * transform.localScale.y + this.offsetElement;
                    break;
            }
        }
    }

    private void refreshChild()
    {
        this.childCount = this.activeChildCount();
        float num = 0f;
        switch (this.type)
        {
            case LayOutTween.left:
                num = this.offsetInit;
                break;
            case LayOutTween.top:
                num = 0f - this.offsetInit;
                break;
            case LayOutTween.right:
                num = 0f - this.offsetInit;
                break;
            case LayOutTween.bottom:
                num = this.offsetInit;
                break;
        }
        for (int i = 0; i < this.childCount; i++)
        {
            Transform transform = this.listActiveObj[i];
            switch (this.type)
            {
                case LayOutTween.left:
                    {
                        Vector3 vector = new Vector3(num, 0f, 0f);
                        if (transform.localPosition != vector)
                        {
                            this.setChildItem(transform, vector);
                        }
                        num += transform.GetComponent<RectTransform>().rect.width * transform.localScale.x + this.offsetElement;
                        break;
                    }
                case LayOutTween.top:
                    {
                        Vector3 vector2 = new Vector3(0f, num, 0f);
                        if (transform.localPosition != vector2)
                        {
                            this.setChildItem(transform, vector2);
                        }
                        num -= transform.GetComponent<RectTransform>().rect.height * transform.localScale.y + this.offsetElement;
                        break;
                    }
                case LayOutTween.right:
                    {
                        Vector3 vector3 = new Vector3(num, 0f, 0f);
                        if (transform.localPosition != vector3)
                        {
                            this.setChildItem(transform, vector3);
                        }
                        num -= transform.GetComponent<RectTransform>().rect.width * transform.localScale.x + this.offsetElement;
                        break;
                    }
                case LayOutTween.bottom:
                    {
                        Vector3 vector4 = new Vector3(0f, num + this.offsetElement, 0f);
                        if (transform.localPosition != vector4)
                        {
                            this.setChildItem(transform, vector4);
                        }
                        num += transform.GetComponent<RectTransform>().rect.height * transform.localScale.y + this.offsetElement;
                        break;
                    }
            }
        }
    }

    private void setChildItem(Transform tmpChild, Vector3 tmpV)
    {
        TweenPosition tweenPosition = tmpChild.GetComponent<TweenPosition>();
        if (tweenPosition == null)
        {
            tweenPosition = tmpChild.gameObject.AddComponent<TweenPosition>();
        }
        tweenPosition.from = tmpChild.localPosition;
        tweenPosition.to = tmpV;
        tweenPosition.duration = this.tweenDuration;
        tweenPosition.Play(true);
        tweenPosition.Reset();
    }

    private void Update()
    {
    }

    private int childCount;

    public LayOutTween type = LayOutTween.right;

    public float offsetElement = 16f;

    public float tweenDuration = 0.2f;

    public float offsetInit = 10f;

    private List<Transform> listActiveObj = new List<Transform>();
}
