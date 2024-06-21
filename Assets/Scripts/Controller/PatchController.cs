using System;
using Framework.Managers;
using Models;
using UnityEngine;
using UnityEngine.UI;

public class PatchController : ControllerBase
{
    public PatchNetWork patchNetWork;
    public UI_Patch ui_Patch;
    public Action<long, long, float, string> OnInDownload;
    public Action<float, string> OnInUzip;
    public Action<string> OnVersionChange;
    public Action OnCallBack;
    public Action OnInitAssets;
    public Action<int, int> OnPatchNum;
    private bool _isInit;
    private GameObject patchUI;

    public override string ControllerName
    {
        get
        {
            return GetType().ToString();
        }
    }

    public override void OnUpdate()
    {
    }

    public override void Awake()
    {
        if (_isInit)
            return;
        _isInit = true;
        patchNetWork = new PatchNetWork();
    }

    public void Send(Action callBack)
    {
        OnCallBack = (Action)(() =>
        {
            callBack();
            LoginCallBack();
        });
        patchNetWork.CheckVersionXml();
    }

    public void UpPatchInfo(string ver, int num, long size, Action okCallback)
    {
        this.ShowPatch();
        float num1 = (float)(size / 1024.0 / 1024.0);
        this.ui_Patch.ShowMessageBox("服务器版本为" + ver + "\n需要下载" + (object)num + "个补丁包," + "总大小" + (object)num1 + "M\n是否立即更新？（更新后才可进入游戏）", new Action(this.ClosePatch), okCallback, new Action(this.ClosePatch), (string)null, (string)null);
    }

    public void InitPatchUI()
    {
        this.patchUI = GameObject.Find("UIRoot/LayerLoading/UI_Patch");
        this.ui_Patch = new UI_Patch(patchUI.transform);
        this.patchUI.SetActive(false);
    }

    public void ClosePatch()
    {
        this.ui_Patch.transform.gameObject.SetActive(false);
    }

    public void ShowPatch()
    {
        this.ui_Patch.transform.gameObject.SetActive(true);
        ControllerManager.Instance.GetController<LoadTipsController>().CloseReconnectTips();
    }

    public void SetVersionText(string str)
    {
        this.ui_Patch.SetVersionUI(str);
    }

    public void CloseVersionText()
    {
        this.ui_Patch.CloseVersionText();
    }

    public void PatchFinish()
    {
        this.ui_Patch.ShowMessageBox("更新成功！", new Action(this.ClosePatch), new Action(this.ClosePatch), new Action(this.ClosePatch), (string)null, (string)null);
    }

    public void VersionBack(string version, Action okCallback)
    {
        this.ShowPatch();
        this.ui_Patch.ShowMessageBox(string.Format("当前版本:{0}高于服务器版本:{1}，\n点击确定将恢复到{1}\n此过程不消耗流量！", (object)UserInfoStorage.StorageInfo.LastVersion, (object)version), new Action(this.ClosePatch), (Action)(() =>
        {
            okCallback();
            this.OnInitAssets();
        }), new Action(this.ClosePatch), (string)null, (string)null);
    }

    public void LoginCallBack()
    {
        if (!(null != patchUI))
            return;
        GameObject child = patchUI.transform.Find("Offset_Patch/Panel_Patch/Text_tips").gameObject;
        if (!(null != child))
            return;
        Text component = child.GetComponent<Text>();
        if (!(null != component))
            return;
        Resources.UnloadAsset(component.font);
    }

    public void OnLargeVersion(string version, Action okCallback)
    {
        this.ShowPatch();
        this.ui_Patch.ShowMessageBox(string.Format("有新版本{0}的安装包需要下载更新\n点击确定将下载安装此版本！", (object)version), new Action(this.ClosePatch), (Action)(() =>
        {
            okCallback();
            this.OnInitAssets();
        }), new Action(this.ClosePatch), (string)null, (string)null);
    }

    public void OnBadNetWork(Action sureCallBack, Action cancelCallBack, Action closeCallBack)
    {
        this.ShowPatch();
        this.ui_Patch.ShowMessageBox("当前网络不可用，是否需要重试? ", closeCallBack, sureCallBack, cancelCallBack, "重试", "离开");
    }

    public void OnGetServerListFailed(
      Action sureCallBack,
      Action cancelCallBack,
      Action closeCallBack)
    {
        this.ShowPatch();
        this.ui_Patch.ShowMessageBox("获取服务器列表出错，是否要重试? ", closeCallBack, sureCallBack, cancelCallBack, "重试", "离开");
    }
}
