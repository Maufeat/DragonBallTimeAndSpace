using System;
using Framework.Managers;
using UnityEngine;
using UnityEngine.UI;

public class UI_ProgressBar : UIPanelBase
{
    public override void OnInit(Transform root)
    {
        base.OnInit(root);
        this.mRoot = root;
        this.InitGameObject();
        this.InitEvent();
    }

    public override void OnDispose()
    {
        base.OnDispose();
        this.UnInit();
        GameObject gameObject = this.mRoot.gameObject;
        this.mRoot = null;
        UnityEngine.Object.Destroy(gameObject);
    }

    public void UnInit()
    {
        Scheduler.Instance.RemoveUpdator(new Scheduler.OnScheduler(this.Update));
    }

    private void InitGameObject()
    {
        this.mRoot.gameObject.SetActive(false);
        this.progresSlider = this.mRoot.transform.Find("Offset_ProgressBar/Panel_ProgressBar/Slider").GetComponent<Slider>();
        this.img_icon = this.mRoot.transform.Find("Offset_ProgressBar/Panel_ProgressBar/Image/img_icon").GetComponent<RawImage>();
        CommonUtil.SetTransfromScale(this.img_icon.gameObject, 0f);
        this.img_iconparent = this.mRoot.transform.Find("Offset_ProgressBar/Panel_ProgressBar/Image").GetComponent<Image>();
        CommonUtil.SetTransfromScale(this.img_iconparent.gameObject, 0f);
        this.txt_info = this.mRoot.transform.Find("Offset_ProgressBar/Panel_ProgressBar/txt_info").GetComponent<Text>();
        CommonUtil.SetTransfromScale(this.txt_info.gameObject, 0f);
        this.txt_name = this.mRoot.transform.Find("Offset_ProgressBar/Panel_ProgressBar/txt_name").GetComponent<Text>();
        CommonUtil.SetTransfromScale(this.txt_name.gameObject, 0f);
    }

    private void InitEvent()
    {
        this.progresSlider.value = this.progresSlider.minValue;
        Scheduler.Instance.AddUpdator(new Scheduler.OnScheduler(this.Update));
    }

    public void Close()
    {
        CommonUtil.SetTransfromScale(this.img_icon.gameObject, 0f);
        CommonUtil.SetTransfromScale(this.img_iconparent.gameObject, 0f);
        CommonUtil.SetTransfromScale(this.txt_info.gameObject, 0f);
        CommonUtil.SetTransfromScale(this.txt_name.gameObject, 0f);
        ManagerCenter.Instance.GetManager<UIManager>().DeleteUI("UI_ProgressBar");
    }

    public void Update()
    {
        if (this.start)
        {
            if (this.sliderDir == SliderDirection.LeftToRight)
            {
                this.HandleLeftToRightDir();
            }
            else
            {
                this.HandleRightToLeftDir();
            }
            this.UpdateTimeInfo();
        }
    }

    private void HandleLeftToRightDir()
    {
        this.timeprogress += Time.deltaTime;
        if (this.DurationTime.Equals(0f))
        {
            return;
        }
        float num = this.timeprogress / this.DurationTime;
        this.progresSlider.value = num;
        if (num >= this.progresSlider.maxValue)
        {
            this.start = false;
            if (this.OnComplete != null)
            {
                this.OnComplete();
            }
            this.Close();
        }
    }

    private void HandleRightToLeftDir()
    {
        this.timeprogress -= Time.deltaTime;
        if (this.DurationTime.Equals(0f))
        {
            return;
        }
        float num = this.timeprogress / this.DurationTime;
        this.progresSlider.value = num;
        if (num <= this.progresSlider.minValue)
        {
            this.start = false;
            if (this.OnComplete != null)
            {
                this.OnComplete();
            }
            this.Close();
        }
    }

    public void InitThis(float duration, Action callback, SliderDirection dir)
    {
        this.DurationTime = duration;
        this.OnComplete = callback;
        this.sliderDir = dir;
        if (this.sliderDir == SliderDirection.LeftToRight)
        {
            this.progresSlider.value = this.progresSlider.minValue;
            this.timeprogress = 0f;
        }
        else
        {
            this.progresSlider.value = this.progresSlider.maxValue;
            this.timeprogress = this.DurationTime;
        }
        this.start = true;
        this.mRoot.gameObject.SetActive(true);
    }

    private void UpdateIcon(string icon)
    {
        CommonUtil.SetTransfromScale(this.img_icon.gameObject, 0f);
        CommonUtil.SetTransfromScale(this.img_iconparent.gameObject, 0f);
        if (string.IsNullOrEmpty(icon))
        {
            return;
        }
        base.GetTexture(ImageType.ICON, icon, delegate (Texture2D item)
        {
            if (this.img_icon != null && item != null)
            {
                Sprite sprite = Sprite.Create(item, new Rect(0f, 0f, (float)item.width, (float)item.height), new Vector2(0f, 0f));
                this.img_icon.texture = sprite.texture;
                this.img_icon.color = Color.white;
                CommonUtil.SetTransfromScale(this.img_iconparent.gameObject, 1f);
                CommonUtil.SetTransfromScale(this.img_icon.gameObject, 1f);
            }
        });
    }

    private void UpdateInfo(string info)
    {
        CommonUtil.SetTransfromScale(this.txt_name.gameObject, 0f);
        if (string.IsNullOrEmpty(info))
        {
            return;
        }
        if (this.txt_name != null)
        {
            this.txt_name.text = info;
            CommonUtil.SetTransfromScale(this.txt_name.gameObject, 1f);
        }
    }

    private void UpdateTimeInfo()
    {
        if (!this.mShowTime)
        {
            return;
        }
        if (this.txt_info != null)
        {
            this.txt_info.text = this.timeprogress.ToString("f1") + "/" + this.DurationTime;
        }
    }

    public void RefreshInfo(string _name, string _icon, bool _showtime)
    {
        this.mShowTime = _showtime;
        if (this.mShowTime)
        {
            CommonUtil.SetTransfromScale(this.txt_info.gameObject, 1f);
        }
        this.UpdateInfo(_name);
    }

    private Transform mRoot;

    private Slider progresSlider;

    private RawImage img_icon;

    private Image img_iconparent;

    private Text txt_info;

    private Text txt_name;

    public Action OnComplete;

    public float DurationTime;

    private bool start;

    private SliderDirection sliderDir;

    private float timeprogress;

    private bool mShowTime;
}
