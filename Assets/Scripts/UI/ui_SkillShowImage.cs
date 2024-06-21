using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ui_SkillShowImage
{
    public void Init(GameObject root)
    {
        this.obj = root;
        if (this.obj == null)
        {
            return;
        }
        this.obj.SetActive(false);
        GameObject gameObject = this.obj.transform.Find("icon/img_icon").gameObject;
        if (gameObject == null)
        {
            return;
        }
        this.Image = gameObject.GetComponent<RawImage>();
        this.goinLst.Clear();
        this.playLst.Clear();
        this.gooutLst.Clear();
        TweenPosition[] componentsInChildren = this.obj.GetComponentsInChildren<TweenPosition>(true);
        if (componentsInChildren != null)
        {
            foreach (TweenPosition tweenPosition in componentsInChildren)
            {
                if (tweenPosition.strName == "goin")
                {
                    tweenPosition.enabled = false;
                    this.goinLst.Add(tweenPosition);
                }
                else if (tweenPosition.strName == "play")
                {
                    tweenPosition.enabled = false;
                    this.playLst.Add(tweenPosition);
                }
                else if (tweenPosition.strName == "goout")
                {
                    tweenPosition.enabled = false;
                    this.gooutLst.Add(tweenPosition);
                }
            }
        }
    }

    public void ShowSkillImage(string name, uint length)
    {
        if (string.IsNullOrEmpty(name))
        {
            return;
        }
        if (length == 0U)
        {
            return;
        }
        this.PlayerOver();
        this.imageSeperateWithAlpha.ProcessRawImageSeperateRGBA(this.Image, name, delegate
        {
            this.Play(length / 1000f);
        });
    }

    private void Play(float Length)
    {
        this.PlayerLength = Length;
        this.isPlay = true;
        this.runningTime = 0f;
        this.SetenabledUITweenerList(this.goinLst, false);
        this.SetenabledUITweenerList(this.playLst, false);
        this.SetenabledUITweenerList(this.gooutLst, false);
        this.PlayUITweenerList(this.goinLst);
        this.PlayUITweenerList(this.playLst);
        this.obj.SetActive(true);
    }

    public void PlayUITweenerList(List<UITweener> List)
    {
        for (int i = 0; i < List.Count; i++)
        {
            UITweener uitweener = List[i];
            uitweener.Reset();
            uitweener.Play(true);
        }
    }

    public void Updata()
    {
        if (this.Image == null)
        {
            return;
        }
        if (this.isPlay)
        {
            this.runningTime += Time.deltaTime;
            if (this.runningTime >= this.PlayerLength && !this.OnHide)
            {
                this.Hide();
            }
            else if (this.runningTime >= this.PlayerLength && this.OnHide)
            {
                this.PlayerOver();
            }
        }
    }

    private void Hide()
    {
        this.OnHide = true;
        this.PlayerLength += 3f;
        this.PlayUITweenerList(this.gooutLst);
    }

    private void PlayerOver()
    {
        this.OnHide = false;
        this.isPlay = false;
        this.imageSeperateWithAlpha.Dispose();
        this.obj.SetActive(false);
    }

    private void SetenabledUITweenerList(List<UITweener> List, bool enabled)
    {
        for (int i = 0; i < List.Count; i++)
        {
            UITweener uitweener = List[i];
            uitweener.enabled = enabled;
        }
    }

    public void Dispose()
    {
        if (this.Image == null)
        {
            return;
        }
        this.imageSeperateWithAlpha.Dispose();
    }

    private GameObject obj;

    private RawImage Image;

    private List<UITweener> goinLst = new List<UITweener>();

    private List<UITweener> playLst = new List<UITweener>();

    private List<UITweener> gooutLst = new List<UITweener>();

    private bool isPlay;

    private float runningTime;

    private float PlayerLength;

    private ImageSeperateWithAlpha imageSeperateWithAlpha = new ImageSeperateWithAlpha();

    private bool OnHide;
}
