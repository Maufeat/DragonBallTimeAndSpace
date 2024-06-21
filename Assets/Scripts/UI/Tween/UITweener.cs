using System;
using System.Collections.Generic;
using AnimationOrTween;
using UnityEngine;

public abstract class UITweener : IgnoreTimeScale
{
    public float amountPerDelta
    {
        get
        {
            if (this.mDuration != this.duration)
            {
                this.mDuration = this.duration;
                this.mAmountPerDelta = Mathf.Abs((this.duration <= 0f) ? 1000f : (1f / this.duration));
            }
            return this.mAmountPerDelta;
        }
    }

    public float tweenFactor
    {
        get
        {
            return this.mFactor;
        }
    }

    public Direction direction
    {
        get
        {
            return (this.mAmountPerDelta >= 0f) ? Direction.Forward : Direction.Reverse;
        }
    }

    private void Start()
    {
        this.Update();
    }

    private void Update()
    {
        float num = (!this.ignoreTimeScale) ? Time.deltaTime : base.UpdateRealTimeDelta();
        float num2 = (!this.ignoreTimeScale) ? Time.time : base.realTime;
        if (!this.mStarted)
        {
            this.mStarted = true;
            this.mStartTime = num2 + this.delay;
        }
        if (num2 < this.mStartTime)
        {
            return;
        }
        this.mFactor += this.amountPerDelta * num;
        if (this.style == UITweener.Style.Loop)
        {
            if (this.mFactor > 1f)
            {
                this.mFactor -= Mathf.Floor(this.mFactor);
            }
        }
        else if (this.style == UITweener.Style.PingPong)
        {
            if (this.mFactor > 1f)
            {
                this.mFactor = 1f - (this.mFactor - Mathf.Floor(this.mFactor));
                this.mAmountPerDelta = -this.mAmountPerDelta;
            }
            else if (this.mFactor < 0f)
            {
                this.mFactor = -this.mFactor;
                this.mFactor -= Mathf.Floor(this.mFactor);
                this.mAmountPerDelta = -this.mAmountPerDelta;
            }
        }
        if (this.style == UITweener.Style.Once && (this.mFactor > 1f || this.mFactor < 0f))
        {
            this.mFactor = Mathf.Clamp01(this.mFactor);
            this.Sample(this.mFactor, true);
            if (this.eventReceiver != null && !string.IsNullOrEmpty(this.callWhenFinished))
            {
                this.eventReceiver.SendMessage(this.callWhenFinished, this, SendMessageOptions.DontRequireReceiver);
            }
            if (this.onFinishedObjActive != null)
            {
                this.onFinishedObjActive.SetActive(true);
                UITools.SetActiveChildren(this.onFinishedObjActive, true);
            }
            if (this.onFinishedObjDisActive != null)
            {
                this.onFinishedObjDisActive.SetActive(false);
            }
            for (int i = 0; i < this.onFinishedObjDisActiveList.Count; i++)
            {
                GameObject gameObject = this.onFinishedObjDisActiveList[i];
                if (gameObject != null)
                {
                    gameObject.SetActive(false);
                }
            }
            if (this.onFinished != null)
            {
                this.onFinished(this);
            }
            if (((this.mFactor == 1f && this.mAmountPerDelta > 0f) || (this.mFactor == 0f && this.mAmountPerDelta < 0f)) && this != null)
            {
                base.enabled = false;
            }
        }
        else
        {
            this.Sample(this.mFactor, false);
        }
    }

    private void OnDisable()
    {
        this.mStarted = false;
    }

    public void Sample(float factor, bool isFinished)
    {
        float num = Mathf.Clamp01(factor);
        switch (this.method)
        {
            case UITweener.Method.EaseInSin:
                num = this.easeInSin(0f, 1f, num);
                break;
            case UITweener.Method.EaseOutSin:
                num = this.easeOutSin(0f, 1f, num);
                break;
            case UITweener.Method.EaseInOutSin:
                num = this.easeInOutSin(0f, 1f, num);
                break;
            case UITweener.Method.BounceIn:
                num = this.BounceLogic(num);
                break;
            case UITweener.Method.BounceOut:
                num = 1f - this.BounceLogic(1f - num);
                break;
            case UITweener.Method.easeOutQuad:
                num = this.easeOutQuad(0f, 1f, num);
                break;
            case UITweener.Method.easeInOutQuad:
                num = this.easeInOutQuad(0f, 1f, num);
                break;
            case UITweener.Method.easeInCubic:
                num = this.easeInCubic(0f, 1f, num);
                break;
            case UITweener.Method.easeOutCubic:
                num = this.easeOutCubic(0f, 1f, num);
                break;
            case UITweener.Method.easeInOutCubic:
                num = this.easeInOutCubic(0f, 1f, num);
                break;
            case UITweener.Method.easeInQuart:
                num = this.easeInQuart(0f, 1f, num);
                break;
            case UITweener.Method.easeOutQuart:
                num = this.easeOutQuart(0f, 1f, num);
                break;
            case UITweener.Method.easeInOutQuart:
                num = this.easeInOutQuart(0f, 1f, num);
                break;
            case UITweener.Method.easeInQuint:
                num = this.easeInQuint(0f, 1f, num);
                break;
            case UITweener.Method.easeOutQuint:
                num = this.easeOutQuint(0f, 1f, num);
                break;
            case UITweener.Method.easeInOutQuint:
                num = this.easeInOutQuint(0f, 1f, num);
                break;
            case UITweener.Method.easeInExpo:
                num = this.easeInExpo(0f, 1f, num);
                break;
            case UITweener.Method.easeOutExpo:
                num = this.easeOutExpo(0f, 1f, num);
                break;
            case UITweener.Method.easeInOutExpo:
                num = this.easeInOutExpo(0f, 1f, num);
                break;
            case UITweener.Method.easeInCirc:
                num = this.easeInCirc(0f, 1f, num);
                break;
            case UITweener.Method.easeOutCirc:
                num = this.easeOutCirc(0f, 1f, num);
                break;
            case UITweener.Method.easeInOutCirc:
                num = this.easeInOutCirc(0f, 1f, num);
                break;
            case UITweener.Method.spring:
                num = this.spring(0f, 1f, num);
                break;
            case UITweener.Method.easeInBack:
                num = this.easeInBack(0f, 1f, num);
                break;
            case UITweener.Method.easeOutBack:
                num = this.easeOutBack(0f, 1f, num);
                break;
            case UITweener.Method.easeInOutBack:
                num = this.easeInOutBack(0f, 1f, num);
                break;
            case UITweener.Method.elastic:
                num = this.elastic(0f, 1f, num);
                break;
        }
        this.OnUpdate((this.animationCurve == null) ? num : this.animationCurve.Evaluate(num), isFinished);
    }

    private float easeInSin(float start, float end, float val)
    {
        val = 1f - Mathf.Sin(1.57079637f * (1f - val));
        if (this.steeperCurves)
        {
            val *= val;
        }
        return val;
    }

    private float easeOutSin(float start, float end, float val)
    {
        val = Mathf.Sin(1.57079637f * val);
        if (this.steeperCurves)
        {
            val = 1f - val;
            val = 1f - val * val;
        }
        return val;
    }

    private float easeInOutSin(float start, float end, float val)
    {
        val -= Mathf.Sin(val * 6.28318548f) / 6.28318548f;
        if (this.steeperCurves)
        {
            val = val * 2f - 1f;
            float num = Mathf.Sign(val);
            val = 1f - Mathf.Abs(val);
            val = 1f - val * val;
            val = num * val * 0.5f + 0.5f;
        }
        return val;
    }

    private float BounceLogic(float val)
    {
        if (val < 0.363636f)
        {
            val = 7.5685f * val * val;
        }
        else if (val < 0.727272f)
        {
            val = 7.5625f * (val -= 0.545454f) * val + 0.75f;
        }
        else if (val < 0.90909f)
        {
            val = 7.5625f * (val -= 0.818181f) * val + 0.9375f;
        }
        else
        {
            val = 7.5625f * (val -= 0.9545454f) * val + 0.984375f;
        }
        return val;
    }

    private float clerp(float start, float end, float value)
    {
        float num = 0f;
        float num2 = 360f;
        float num3 = Mathf.Abs((num2 - num) / 2f);
        float result;
        if (end - start < -num3)
        {
            float num4 = (num2 - start + end) * value;
            result = start + num4;
        }
        else if (end - start > num3)
        {
            float num4 = -(num2 - end + start) * value;
            result = start + num4;
        }
        else
        {
            result = start + (end - start) * value;
        }
        return result;
    }

    private float spring(float start, float end, float value)
    {
        value = Mathf.Clamp01(value);
        value = (Mathf.Sin(value * 3.14159274f * (0.2f + 2.5f * value * value * value)) * Mathf.Pow(1f - value, 2.2f) + value) * (1f + 1.2f * (1f - value));
        return start + (end - start) * value;
    }

    private float easeInQuad(float start, float end, float value)
    {
        end -= start;
        return end * value * value + start;
    }

    private float easeOutQuad(float start, float end, float value)
    {
        end -= start;
        return -end * value * (value - 2f) + start;
    }

    private float easeInOutQuad(float start, float end, float value)
    {
        value /= 0.5f;
        end -= start;
        if (value < 1f)
        {
            return end / 2f * value * value + start;
        }
        value -= 1f;
        return -end / 2f * (value * (value - 2f) - 1f) + start;
    }

    private float easeInCubic(float start, float end, float value)
    {
        end -= start;
        return end * value * value * value + start;
    }

    private float easeOutCubic(float start, float end, float value)
    {
        value -= 1f;
        end -= start;
        return end * (value * value * value + 1f) + start;
    }

    private float easeInOutCubic(float start, float end, float value)
    {
        value /= 0.5f;
        end -= start;
        if (value < 1f)
        {
            return end / 2f * value * value * value + start;
        }
        value -= 2f;
        return end / 2f * (value * value * value + 2f) + start;
    }

    private float easeInQuart(float start, float end, float value)
    {
        end -= start;
        return end * value * value * value * value + start;
    }

    private float easeOutQuart(float start, float end, float value)
    {
        value -= 1f;
        end -= start;
        return -end * (value * value * value * value - 1f) + start;
    }

    private float easeInOutQuart(float start, float end, float value)
    {
        value /= 0.5f;
        end -= start;
        if (value < 1f)
        {
            return end / 2f * value * value * value * value + start;
        }
        value -= 2f;
        return -end / 2f * (value * value * value * value - 2f) + start;
    }

    private float easeInQuint(float start, float end, float value)
    {
        end -= start;
        return end * value * value * value * value * value + start;
    }

    private float easeOutQuint(float start, float end, float value)
    {
        value -= 1f;
        end -= start;
        return end * (value * value * value * value * value + 1f) + start;
    }

    private float easeInOutQuint(float start, float end, float value)
    {
        value /= 0.5f;
        end -= start;
        if (value < 1f)
        {
            return end / 2f * value * value * value * value * value + start;
        }
        value -= 2f;
        return end / 2f * (value * value * value * value * value + 2f) + start;
    }

    private float easeInSine(float start, float end, float value)
    {
        end -= start;
        return -end * Mathf.Cos(value / 1f * 1.57079637f) + end + start;
    }

    private float easeOutSine(float start, float end, float value)
    {
        end -= start;
        return end * Mathf.Sin(value / 1f * 1.57079637f) + start;
    }

    private float easeInOutSine(float start, float end, float value)
    {
        end -= start;
        return -end / 2f * (Mathf.Cos(3.14159274f * value / 1f) - 1f) + start;
    }

    private float easeInExpo(float start, float end, float value)
    {
        end -= start;
        return end * Mathf.Pow(2f, 10f * (value / 1f - 1f)) + start;
    }

    private float easeOutExpo(float start, float end, float value)
    {
        end -= start;
        return end * (-Mathf.Pow(2f, -10f * value / 1f) + 1f) + start;
    }

    private float easeInOutExpo(float start, float end, float value)
    {
        value /= 0.5f;
        end -= start;
        if (value < 1f)
        {
            return end / 2f * Mathf.Pow(2f, 10f * (value - 1f)) + start;
        }
        value -= 1f;
        return end / 2f * (-Mathf.Pow(2f, -10f * value) + 2f) + start;
    }

    private float easeInCirc(float start, float end, float value)
    {
        end -= start;
        return -end * (Mathf.Sqrt(1f - value * value) - 1f) + start;
    }

    private float easeOutCirc(float start, float end, float value)
    {
        value -= 1f;
        end -= start;
        return end * Mathf.Sqrt(1f - value * value) + start;
    }

    private float easeInOutCirc(float start, float end, float value)
    {
        value /= 0.5f;
        end -= start;
        if (value < 1f)
        {
            return -end / 2f * (Mathf.Sqrt(1f - value * value) - 1f) + start;
        }
        value -= 2f;
        return end / 2f * (Mathf.Sqrt(1f - value * value) + 1f) + start;
    }

    private float bounce(float start, float end, float value)
    {
        value /= 1f;
        end -= start;
        if (value < 0.363636374f)
        {
            return end * (7.5625f * value * value) + start;
        }
        if (value < 0.727272749f)
        {
            value -= 0.545454562f;
            return end * (7.5625f * value * value + 0.75f) + start;
        }
        if ((double)value < 0.90909090909090906)
        {
            value -= 0.8181818f;
            return end * (7.5625f * value * value + 0.9375f) + start;
        }
        value -= 0.954545438f;
        return end * (7.5625f * value * value + 0.984375f) + start;
    }

    private float easeInBack(float start, float end, float value)
    {
        end -= start;
        value /= 1f;
        float num = 1.70158f;
        return end * value * value * ((num + 1f) * value - num) + start;
    }

    private float easeOutBack(float start, float end, float value)
    {
        float num = 1.70158f;
        end -= start;
        value = value / 1f - 1f;
        return end * (value * value * ((num + 1f) * value + num) + 1f) + start;
    }

    private float easeInOutBack(float start, float end, float value)
    {
        float num = 1.70158f;
        end -= start;
        value /= 0.5f;
        if (value < 1f)
        {
            num *= 1.525f;
            return end / 2f * (value * value * ((num + 1f) * value - num)) + start;
        }
        value -= 2f;
        num *= 1.525f;
        return end / 2f * (value * value * ((num + 1f) * value + num) + 2f) + start;
    }

    private float punch(float amplitude, float value)
    {
        if (value == 0f)
        {
            return 0f;
        }
        if (value == 1f)
        {
            return 0f;
        }
        float num = 0.3f;
        float num2 = num / 6.28318548f * Mathf.Asin(0f);
        return amplitude * Mathf.Pow(2f, -10f * value) * Mathf.Sin((value * 1f - num2) * 6.28318548f / num);
    }

    private float elastic(float start, float end, float value)
    {
        end -= start;
        float num = 1f;
        float num2 = num * 0.3f;
        float num3 = 0f;
        if (value == 0f)
        {
            return start;
        }
        if ((value /= num) == 1f)
        {
            return start + end;
        }
        float num4;
        if (num3 == 0f || num3 < Mathf.Abs(end))
        {
            num3 = end;
            num4 = num2 / 4f;
        }
        else
        {
            num4 = num2 / 6.28318548f * Mathf.Asin(end / num3);
        }
        return num3 * Mathf.Pow(2f, -10f * value) * Mathf.Sin((value * num - num4) * 6.28318548f / num2) + end + start;
    }

    public void Play(bool forward)
    {
        this.mAmountPerDelta = Mathf.Abs(this.amountPerDelta);
        if (!forward)
        {
            this.mAmountPerDelta = -this.mAmountPerDelta;
        }
        base.enabled = true;
    }

    public void Reset()
    {
        this.mStarted = false;
        this.mFactor = ((this.mAmountPerDelta >= 0f) ? 0f : 1f);
        this.Sample(this.mFactor, false);
    }

    public void Toggle()
    {
        if (this.mFactor > 0f)
        {
            this.mAmountPerDelta = -this.amountPerDelta;
        }
        else
        {
            this.mAmountPerDelta = Mathf.Abs(this.amountPerDelta);
        }
        base.enabled = true;
    }

    protected abstract void OnUpdate(float factor, bool isFinished);

    public static T Begin<T>(GameObject go, float duration) where T : UITweener
    {
        T t = go.GetComponent<T>();
        if (t == null)
        {
            t = go.AddComponent<T>();
        }
        t.mStarted = false;
        t.duration = duration;
        t.mFactor = 0f;
        t.mAmountPerDelta = Mathf.Abs(t.mAmountPerDelta);
        t.style = UITweener.Style.Once;
        t.animationCurve = new AnimationCurve(new Keyframe[]
        {
            new Keyframe(0f, 0f, 0f, 1f),
            new Keyframe(1f, 1f, 1f, 0f)
        });
        t.eventReceiver = null;
        t.callWhenFinished = null;
        t.onFinished = null;
        t.enabled = true;
        return t;
    }

    [ContextMenu("Reset and play forward")]
    public void ResetAndPlayForward()
    {
        this.Reset();
        this.Play(true);
    }

    [ContextMenu("Reset and play reverse")]
    public void ResetAndPlayReverse()
    {
        this.Reset();
        this.Play(false);
    }

    public string strName = string.Empty;

    public UITweener.OnFinished onFinished;

    public GameObject onFinishedObjActive;

    public GameObject onFinishedObjDisActive;

    public List<GameObject> onFinishedObjDisActiveList = new List<GameObject>();

    public UITweener.Method method;

    public UITweener.Style style;

    public AnimationCurve animationCurve = new AnimationCurve(new Keyframe[]
    {
        new Keyframe(0f, 0f, 0f, 1f),
        new Keyframe(1f, 1f, 1f, 0f)
    });

    public bool ignoreTimeScale = true;

    public float delay;

    public float duration = 1f;

    public bool steeperCurves;

    public int tweenGroup;

    public GameObject eventReceiver;

    public string callWhenFinished;

    private bool mStarted;

    private float mStartTime;

    private float mDuration;

    private float mAmountPerDelta = 1f;

    private float mFactor;

    public bool IsFirstOne;

    public enum Method
    {
        Linear,
        EaseInSin,
        EaseOutSin,
        EaseInOutSin,
        BounceIn,
        BounceOut,
        easeInQuad,
        easeOutQuad,
        easeInOutQuad,
        easeInCubic,
        easeOutCubic,
        easeInOutCubic,
        easeInQuart,
        easeOutQuart,
        easeInOutQuart,
        easeInQuint,
        easeOutQuint,
        easeInOutQuint,
        easeInExpo,
        easeOutExpo,
        easeInOutExpo,
        easeInCirc,
        easeOutCirc,
        easeInOutCirc,
        spring,
        easeInBack,
        easeOutBack,
        easeInOutBack,
        elastic,
        punch
    }

    public enum Style
    {
        Once,
        Loop,
        PingPong
    }

    public delegate void OnFinished(UITweener tween);
}
