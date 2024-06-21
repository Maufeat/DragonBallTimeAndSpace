using System;
using UnityEngine;
using UnityEngine.UI;

[AddComponentMenu("NGUI/Tween/Alpha")]
public class TweenAlpha : UITweener
{
    public float alpha
    {
        get
        {
            if (this.canvasRender != null)
            {
                return this.canvasRender.GetAlpha();
            }
            if (this.graphic != null)
            {
                return this.graphic.color.a;
            }
            if (this.canvasGroup != null)
            {
                return this.canvasGroup.alpha;
            }
            return 1f;
        }
        set
        {
            if (this.canvasRender != null)
            {
                this.canvasRender.SetAlpha(value);
            }
            if (this.graphic != null)
            {
                this.graphic.color = new Color(this.graphic.color.r, this.graphic.color.g, this.graphic.color.b, value);
            }
            else if (this.canvasGroup != null)
            {
                this.canvasGroup.alpha = value;
            }
        }
    }

    private void Awake()
    {
        this.canvasRender = base.GetComponent<CanvasRenderer>();
        this.graphic = base.GetComponent<Graphic>();
        this.canvasGroup = base.GetComponent<CanvasGroup>();
    }

    protected override void OnUpdate(float factor, bool isFinished)
    {
        this.alpha = Mathf.Lerp(this.from, this.to, factor);
    }

    public static TweenAlpha Begin(GameObject go, float duration, float alpha)
    {
        TweenAlpha tweenAlpha = UITweener.Begin<TweenAlpha>(go, duration);
        tweenAlpha.from = tweenAlpha.alpha;
        tweenAlpha.to = alpha;
        if (duration <= 0f)
        {
            tweenAlpha.Sample(1f, true);
            tweenAlpha.enabled = false;
        }
        return tweenAlpha;
    }

    public float from = 1f;

    public float to = 1f;

    private Transform mTrans;

    private Graphic graphic;

    private CanvasRenderer canvasRender;

    private CanvasGroup canvasGroup;
}
