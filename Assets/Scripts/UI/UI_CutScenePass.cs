using System;
using UnityEngine;
using UnityEngine.UI;

public class UI_CutScenePass : MonoBehaviour
{
    private void Start()
    {
    }

    private void Awake()
    {
        this.textInfo = base.gameObject.FindChild("txt_info").GetComponent<Text>();
        UI_CutScenePass._cutScenePass = base.GetComponent<UI_CutScenePass>();
        this._maskAlpha = base.gameObject.FindChild("Img_Mask").GetComponent<TweenAlpha>();
        this._maskAlpha.onFinished = delegate (UITweener tweener)
        {
        };
    }

    public static UI_CutScenePass GetView()
    {
        return UI_CutScenePass._cutScenePass;
    }

    public void SetSubtitle(string str, float durtaion)
    {
        if (this.textInfo == null)
        {
            return;
        }
        this.curtime = durtaion;
        this.textInfo.gameObject.SetActive(true);
        this.textInfo.text = str;
    }

    private void Update()
    {
        if (this.curtime <= 0f)
        {
            if (this.textInfo != null && this.textInfo.gameObject.activeSelf)
            {
                this.textInfo.gameObject.SetActive(false);
            }
            return;
        }
        this.curtime -= Time.deltaTime;
    }

    public void ShowMaskAnimation(float duration)
    {
        Graphic component = this._maskAlpha.gameObject.GetComponent<Graphic>();
        component.CrossFadeAlpha(1f, 2f, true);
    }

    private static UI_CutScenePass _cutScenePass;

    public Action<string> OnMaskPlayFinished;

    public string CurrKey = string.Empty;

    private Text textInfo;

    private TweenAlpha _maskAlpha;

    private float titleShowTime = 2f;

    private float curtime;
}
