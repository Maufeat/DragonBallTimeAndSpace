using System;
using Framework.Managers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Patch
{
    public UI_Patch(Transform _transform)
    {
        this.transform = _transform;
        this.text_Version = GameObject.Find("UIRoot/LayerLoading/Text_version").GetComponent<Text>();
        this.text_PatchProcess = this.transform.Find("Offset_Patch/Panel_Patch/Text_Process").GetComponent<Text>();
        this.slider_process = this.transform.Find("Offset_Patch/Panel_Patch/Slider_process").GetComponent<Slider>();
        this.text_tips = this.transform.Find("Offset_Patch/Panel_Patch/Text_tips").GetComponent<Text>();
        this.Panel_MessageRoot = this.transform.Find("Offset_Patch/Panel_Message");
        this.text_messge = this.Panel_MessageRoot.Find("Text_message").GetComponent<Text>();
        this.btn_close = this.Panel_MessageRoot.Find("btn_close").GetComponent<Button>();
        this.btn_sure = this.Panel_MessageRoot.Find("btn_sure").GetComponent<Button>();
        this.text_sure = this.Panel_MessageRoot.Find("btn_sure/Text_sure").GetComponent<Text>();
        this.text_tips.gameObject.SetActive(false);
        this.slider_process.gameObject.SetActive(false);
        this.text_PatchProcess.gameObject.SetActive(false);
        this.Register();
    }

    public void Register()
    {
        PatchController controller = ControllerManager.Instance.GetController<PatchController>();
        controller.OnInUzip = new Action<float, string>(this.OnInUzip);
        controller.OnInDownload = new Action<long, long, float, string>(this.OnInDownload);
        controller.OnVersionChange = new Action<string>(this.SetVersionUI);
        controller.OnInitAssets = new Action(this.OnInitAssets);
        controller.OnPatchNum = new Action<int, int>(this.OnPatchNum);
    }

    public void SetSliderProcess(float value)
    {
        this.text_tips.gameObject.SetActive(true);
        this.slider_process.gameObject.SetActive(true);
        this.slider_process.value = value;
    }

    public void OnInDownload(long size1, long size2, float value, string version)
    {
        this.text_tips.gameObject.SetActive(true);
        this.text_tips.text = "正在下载补丁包：" + version;
        this.SetSliderProcess(value);
    }

    public void OnInUzip(float value, string versionPatch)
    {
        this.text_tips.gameObject.SetActive(true);
        this.text_tips.text = "正在安装补丁包：" + versionPatch;
        this.SetSliderProcess(value);
    }

    public void StartPatch()
    {
        this.text_tips.gameObject.SetActive(true);
        this.text_tips.text = "正在更新，您的数据保存很安全，除非核战争。";
    }

    public void OnInitAssets()
    {
        this.text_tips.gameObject.SetActive(true);
        this.text_tips.text = "正在还原本地资源请稍后……";
    }

    public void OnPatchNum(int num, int count)
    {
        this.text_PatchProcess.gameObject.SetActive(true);
        this.text_PatchProcess.text = num.ToString() + "/" + count.ToString();
    }

    public void ShowMessageBox(string message, Action closeCallback, Action sureCallback, Action cancelCallBack, string sureText = null, string cancelText = null)
    {
        this.Panel_MessageRoot.gameObject.SetActive(true);
        this.text_messge.text = message;
        if (string.IsNullOrEmpty(sureText))
        {
            this.text_sure.text = "确定";
        }
        else
        {
            this.text_sure.text = sureText;
        }
        if (string.IsNullOrEmpty(cancelText))
        {
            this.text_cancel.text = "取消";
        }
        else
        {
            this.text_cancel.text = cancelText;
        }
        UIEventListener.Get(this.btn_close.gameObject).onClick = delegate (PointerEventData data)
        {
            this.Panel_MessageRoot.gameObject.SetActive(false);
            closeCallback();
        };
        UIEventListener.Get(this.btn_sure.gameObject).onClick = delegate (PointerEventData data)
        {
            this.Panel_MessageRoot.gameObject.SetActive(false);
            sureCallback();
        };
        UIEventListener.Get(this.btn_cancel.gameObject).onClick = delegate (PointerEventData data)
        {
            this.Panel_MessageRoot.gameObject.SetActive(false);
            cancelCallBack();
        };
    }

    public void SetVersionUI(string version)
    {
        this.text_Version.gameObject.SetActive(true);
        this.text_Version.text = "Ver:" + version;
    }

    public void CloseVersionText()
    {
        this.text_Version.gameObject.SetActive(false);
    }

    protected Text text_Version;

    protected Text text_PatchProcess;

    protected Text text_tips;

    protected Slider slider_process;

    protected Transform Panel_MessageRoot;

    protected Text text_messge;

    protected Button btn_close;

    protected Button btn_sure;

    protected Text text_sure;

    protected Button btn_cancel;

    protected Text text_cancel;

    public Transform transform;
}
