using System;
using UnityEngine;
using UnityEngine.UI;

public class TipsUI : UIPanelBase
{
    public override void OnInit(Transform tran)
    {
        base.OnInit(tran);
        this.tipsRoot = tran;
        this.tranOffset = tran.FindChild("Offset_Tips");
        this.panelTips = tran.FindChild("Offset_Tips/Panel_tips");
        this.txt_tips = this.panelTips.FindChild("txt_tips").gameObject;
        this.noticePanel = this.tranOffset.FindChild("Panel_notice");
        this.noticeItem = this.noticePanel.FindChild("noticeContent/Item").gameObject;
        this.noticeItem.SetActive(false);
        this.panelTaskTips = tran.FindChild("Offset_Tips/Panel_task");
        this.txtTaskTips = this.panelTaskTips.FindChild("txt_tips").GetComponent<Text>();
        this.taTask = this.txtTaskTips.GetComponent<TweenAlpha>();
        this.taTaskBg = this.panelTaskTips.GetComponent<TweenAlpha>();
        this.panelTaskTips.gameObject.SetActive(false);
        this.setTipsTween();
    }

    public override void OnDispose()
    {
        base.OnDispose();
    }

    private void setTipsTween()
    {
        this.texTween = this.txt_tips.GetComponent<TweenAlpha>();
        if (this.texTween == null)
        {
            this.texTween = this.txt_tips.AddComponent<TweenAlpha>();
        }
        this.panelPostion = this.panelTips.GetComponent<TweenPosition>();
        if (this.panelPostion == null)
        {
            this.panelPostion = this.panelTips.gameObject.AddComponent<TweenPosition>();
        }
        this.texTween.duration = 2f;
        this.texTween.from = 1f;
        this.texTween.to = 0.01f;
        this.panelPostion.duration = 2f;
        this.panelPostion.method = UITweener.Method.easeOutQuart;
        this.panelPostion.onFinished = delegate (UITweener t)
        {
            this.panelTips.gameObject.SetActive(false);
        };
        this.panelTips.gameObject.SetActive(false);
    }

    public void ShowTips(NoticeModel contain)
    {
        Transform parent = this.tipsRoot.transform.parent;
        this.tipsRoot.SetParent(parent);
        this.txt_tips.GetComponent<Text>().text = contain.content;
        this.txt_tips.GetComponent<Outline>().effectColor = contain.texEffectColor;
        this.panelTips.gameObject.SetActive(true);
        this.txt_tips.gameObject.SetActive(true);
        this.panelPostion.Reset();
        this.texTween.Reset();
        this.texTween.Play(true);
        this.panelPostion.Play(true);
    }

    public void ShowNotice()
    {
        this.noticePanel.gameObject.SetActive(true);
        while (TipsWindow.noticeContain.Count > 0)
        {
            NoticeModel noticeModel = TipsWindow.noticeContain.Dequeue();
            GameObject item = UnityEngine.Object.Instantiate<GameObject>(this.noticeItem);
            item.transform.SetParent(this.noticeItem.transform.parent);
            item.transform.localScale = this.noticeItem.transform.localScale;
            Text text = item.transform.FindChild("txt_tips").GetComponent<Text>();
            if (text == null)
            {
                text = item.transform.FindChild("txt_tips").gameObject.AddComponent<Text>();
                Text component = this.noticeItem.FindChild("txt_tips").GetComponent<Text>();
                text.font = component.font;
                text.fontStyle = component.fontStyle;
                text.fontSize = component.fontSize;
                text.lineSpacing = component.lineSpacing;
                text.alignment = component.alignment;
                text.horizontalOverflow = component.horizontalOverflow;
                text.color = component.color;
            }
            text.text = noticeModel.content;
            float x = item.transform.FindChild("txt_tips").GetComponent<Text>().preferredWidth + 80f;
            text.rectTransform.sizeDelta = new Vector2(x, text.rectTransform.sizeDelta.y);
            Image image = item.transform.FindChild("sp_back").GetComponent<Image>();
            if (image == null)
            {
                image = item.transform.FindChild("sp_back").gameObject.AddComponent<Image>();
                Image component2 = this.noticeItem.FindChild("sp_back").GetComponent<Image>();
                image.eventAlphaThreshold = component2.eventAlphaThreshold;
                image.fillAmount = component2.fillAmount;
                image.fillCenter = component2.fillCenter;
                image.fillClockwise = component2.fillClockwise;
                image.fillMethod = component2.fillMethod;
                image.fillOrigin = component2.fillOrigin;
                image.overrideSprite = component2.overrideSprite;
                image.preserveAspect = component2.preserveAspect;
                image.sprite = component2.sprite;
                image.type = component2.type;
            }
            image.rectTransform.sizeDelta = new Vector2(x, image.rectTransform.sizeDelta.y);
            TweenAlpha tweenAlpha = item.transform.FindChild("sp_back").GetComponent<TweenAlpha>();
            if (tweenAlpha == null)
            {
                tweenAlpha = item.transform.FindChild("sp_back").gameObject.AddComponent<TweenAlpha>();
                TweenAlpha component3 = this.noticeItem.FindChild("sp_back").GetComponent<TweenAlpha>();
                tweenAlpha.from = component3.from;
                tweenAlpha.to = component3.to;
                tweenAlpha.duration = component3.duration;
                tweenAlpha.delay = component3.delay;
                tweenAlpha.steeperCurves = component3.steeperCurves;
                tweenAlpha.animationCurve = component3.animationCurve;
                tweenAlpha.style = component3.style;
                tweenAlpha.onFinished = component3.onFinished;
            }
            UIInformationList uiinformationList = item.transform.FindChild("txt_tips").GetComponent<UIInformationList>();
            if (uiinformationList == null)
            {
                uiinformationList = item.transform.FindChild("txt_tips").gameObject.AddComponent<UIInformationList>();
                UIInformationList component4 = this.noticeItem.FindChild("txt_tips").GetComponent<UIInformationList>();
                uiinformationList.listInformation = component4.listInformation;
                uiinformationList.modelName = component4.modelName;
            }
            TweenAlpha tweenAlpha2 = item.transform.FindChild("txt_tips").GetComponent<TweenAlpha>();
            if (tweenAlpha2 == null)
            {
                tweenAlpha2 = item.transform.FindChild("txt_tips").gameObject.AddComponent<TweenAlpha>();
                TweenAlpha component5 = this.noticeItem.FindChild("txt_tips").GetComponent<TweenAlpha>();
                tweenAlpha2.from = component5.from;
                tweenAlpha2.to = component5.to;
                tweenAlpha2.duration = component5.duration;
                tweenAlpha2.delay = component5.delay;
                tweenAlpha2.steeperCurves = component5.steeperCurves;
                tweenAlpha2.animationCurve = component5.animationCurve;
                tweenAlpha2.style = component5.style;
            }
            item.SetActive(true);
            tweenAlpha2.onFinished = delegate (UITweener tweener)
            {
                UnityEngine.Object.Destroy(item);
            };
        }
    }

    public void ShowTaskTips(string content)
    {
        this.panelTaskTips.gameObject.SetActive(true);
        this.txtTaskTips.text = content;
        this.taTaskBg.Reset();
        this.taTaskBg.Play(true);
        this.taTask.Reset();
        this.taTask.Play(true);
        this.taTask.onFinished = delegate (UITweener tween)
        {
            this.panelTaskTips.gameObject.SetActive(false);
        };
    }

    public Transform tipsRoot;

    private Transform panelTips;

    private Transform tranOffset;

    private GameObject txt_tips;

    private TweenAlpha texTween;

    private TweenPosition panelPostion;

    private Transform noticePanel;

    private GameObject noticeItem;

    private Transform panelTaskTips;

    private Text txtTaskTips;

    private TweenAlpha taTask;

    private TweenAlpha taTaskBg;
}
