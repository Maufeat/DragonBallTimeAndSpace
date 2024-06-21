using System;
using UnityEngine;
using UnityEngine.UI;

public class UIFoldItem : MonoBehaviour
{
    private void Awake()
    {
        if (this.myButton == null)
        {
            FFDebug.Log(this, FFLogType.UI, " button  is  null");
        }
        if (!(this.rect == null))
        {
            this.rect.transform.localScale = new Vector3(1f, 0f, 1f);
            this.height = this.rect.rect.height + 2f;
        }
        if (this.tmpElement == null)
        {
            FFDebug.LogWarning(this, " tmpelement  is  null");
        }
        else
        {
            this.tmpElement.preferredHeight = 0f;
        }
    }

    public void startTweenScale()
    {
        if (this.rect == null)
        {
            return;
        }
        TweenScale tweenScale = this.rect.gameObject.GetComponent<TweenScale>();
        if (tweenScale == null)
        {
            tweenScale = this.rect.gameObject.AddComponent<TweenScale>();
            tweenScale.from = new Vector3(1f, 0f, 1f);
            tweenScale.to = new Vector3(1f, 1f, 1f);
            tweenScale.duration = 0.2f;
        }
        tweenScale.Play(!this.isContainView);
        this.isTweening = true;
        tweenScale.onFinished = delegate (UITweener teener)
        {
            this.isTweening = false;
            this.tmpElement.preferredHeight = this.height * this.rect.localScale.y;
            if (this.tweenFinish != null)
            {
                this.tweenFinish();
            }
        };
        this.isContainView = !this.isContainView;
    }

    private void FixedUpdate()
    {
        if (this.isTweening)
        {
            this.tmpElement.preferredHeight = this.height * this.rect.localScale.y;
        }
    }

    private void Start()
    {
    }

    public void resetCantainView()
    {
        if (this.rect == null)
        {
            return;
        }
        this.rect.transform.localScale = new Vector3(1f, 0f, 1f);
        this.tmpElement.preferredHeight = 0f;
        this.isContainView = false;
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(this.tmpElement.gameObject);
        this.tmpElement = null;
    }

    public Button myButton;

    public RectTransform rect;

    public bool isContainView;

    public LayoutElement tmpElement;

    private float height;

    public Action tweenFinish;

    private bool isTweening;
}
