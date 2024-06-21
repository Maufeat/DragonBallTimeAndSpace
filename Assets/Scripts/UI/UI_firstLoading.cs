using System;
using UnityEngine;
using UnityEngine.UI;

public class UI_firstLoading
{
    public static UI_firstLoading LoadView()
    {
        GameObject gameObject = GameObject.Find("UIRoot");
        Transform transform = gameObject.transform.Find("LayerLoading/UI_firstLoading");
        UI_firstLoading ui_firstLoading = new UI_firstLoading();
        ui_firstLoading.Init(transform.transform);
        return ui_firstLoading;
    }

    public void Init(Transform root)
    {
        this.Root = root;
        this.mSlider = this.Root.transform.Find("Offset_Loading/Panel_Loading/Slider").GetComponent<Slider>();
        this.Root.gameObject.SetActive(true);
        Scheduler.Instance.AddUpdator(new Scheduler.OnScheduler(this.Update));
    }

    public float Progress
    {
        get
        {
            if (this.TrueProgress >= 1f)
            {
                return 1f;
            }
            float num = this.FakeProgress * this.FakeRate + this.TrueProgress * (1f - this.FakeRate);
            return (num <= 1f) ? num : 1f;
        }
    }

    private void Update()
    {
        if (this.FakeProgress < 1f)
        {
            this.FakeProgress += Time.deltaTime * this.FakeSpeed;
        }
        else
        {
            this.FakeProgress = 1f;
        }
        if (this.FakeProgress > 0.5f && this.speedState == 0)
        {
            this.FakeSpeed *= 0.5f;
            this.speedState++;
        }
        else if (this.FakeProgress > 0.7f && this.speedState == 1)
        {
            this.FakeSpeed *= 0.5f;
            this.speedState++;
        }
        else if (this.FakeProgress > 0.8f && this.speedState == 2)
        {
            this.FakeSpeed *= 0.5f;
            this.speedState++;
        }
        else if (this.FakeProgress > 0.9f && this.speedState == 2)
        {
            this.FakeSpeed = 0.07f;
            this.speedState++;
        }
        this.mSlider.value = this.Progress;
        if (this.NeedHide >= 0)
        {
            this.NeedHide--;
            if (this.NeedHide == 0)
            {
                this.TrueHide();
            }
        }
    }

    public void Hide()
    {
        this.NeedHide = 5;
        this.TrueProgress = 1f;
    }

    private void TrueHide()
    {
        this.NeedHide = -1;
        this.FakeProgress = 0f;
        this.TrueProgress = 0f;
        this.speedState = 0;
        this.Root.gameObject.SetActive(false);
        Scheduler.Instance.RemoveUpdator(new Scheduler.OnScheduler(this.Update));
    }

    public void SetTrueProgress(float Progress)
    {
        this.TrueProgress = ((Progress <= 1f) ? Progress : 1f);
    }

    private Transform Root;

    private Slider mSlider;

    private float FakeRate = 0.7f;

    private float FakeSpeed = 1f;

    private float FakeProgress;

    private float TrueProgress;

    private int speedState;

    private int NeedHide = -1;
}
