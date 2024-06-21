using System;
using Framework.Managers;
using UnityEngine;
using UnityEngine.UI;

public class UI_P2PLogin : UIPanelBase
{
    public static void LoadView(Action Loadover)
    {
        ManagerCenter.Instance.GetManager<UIManager>().ShowUI<UI_P2PLogin>("UI_P2PLogin", Loadover, UIManager.ParentType.CommonUI, false);
    }

    public override void OnInit(Transform root)
    {
        base.OnInit(root);
        this.Root = root;
        this.isLoginByEnterKey = false;
        UI_P2PLogin.OnloginView = true;
        this.LoginBtn = this.Root.Find("Offset_Example/Panel/btn_login").gameObject;
        this.SeverBenSelect = this.Root.Find("Offset_Example/Panel/Login/Panel_server/InputField").GetComponent<InputField>();
        this.AccountText = this.Root.Find("Offset_Example/Panel/Login/Panel_account/InputField").GetComponent<InputField>();
        this.AccountText.gameObject.AddComponent<InputNavigator>();
        this.AccountPwd = this.Root.Find("Offset_Example/Panel/Login/Panel_password/InputField").GetComponent<InputField>();
        Button component = this.LoginBtn.GetComponent<Button>();
        component.onClick.RemoveAllListeners();
        component.onClick.AddListener(delegate ()
        {
            this.TryLogin();
        });
        GameObject gameObject = this.Root.Find("Offset_Example/Panel/CloseButton").gameObject;
        Button component2 = gameObject.GetComponent<Button>();
        component2.onClick.RemoveAllListeners();
        component2.onClick.AddListener(delegate ()
        {
            Application.Quit();
        });
        this.animRoot = this.Root.Find("Offset_Example/Panel/Panel_ani").gameObject;
        this.bakImg = this.Root.Find("Offset_Example/Panel/img_bg (1)").gameObject;
        Image component3 = this.bakImg.GetComponent<Image>();
        UnityEngine.Object.DestroyImmediate(component3);
        this.rig = this.bakImg.AddComponent<RawImage>();
        this.rt = new RenderTexture(1920, 1080, 24, RenderTextureFormat.ARGB32);
        this.rig.texture = this.rt;
        this.bakImg.SetActive(false);
        string text = string.Empty;
        int num = LaunchHelp.RecommendServer;
        text = UserInfoStorage.StorageInfo.Uid;
        num = UserInfoStorage.StorageInfo.LastServer;
        if (!string.IsNullOrEmpty(text))
        {
            this.AccountText.text = text;
        }
        string text2 = this.ReadFLconfig();
        this.SeverBenSelect.text = text2;
        this.AccountPwd.text = string.Empty;
        Scheduler.Instance.AddUpdator(new Scheduler.OnScheduler(this.Update));
        UIManager manager = ManagerCenter.Instance.GetManager<UIManager>();
        if (manager != null)
        {
            manager.DeleteUI("UI_Main");
        }
        EntitiesManager manager2 = ManagerCenter.Instance.GetManager<EntitiesManager>();
        if (manager2.wegame)
        {
            this.LoginBtn.SetActive(false);
            GameObject gameObject2 = this.Root.Find("Offset_Example/Panel/Login").gameObject;
            gameObject2.SetActive(false);
        }
    }

    public string ReadFLconfig()
    {
        return this.GetServerFormPlayerPrefer();
    }

    private string GetServerFormPlayerPrefer()
    {
        string @string = MyPlayerPrefs.GetString("server_info");
        if (@string == string.Empty)
        {
            return string.Empty;
        }
        string[] array = @string.Split(new char[]
        {
            ','
        });
        if (array.Length < 3)
        {
            return string.Empty;
        }
        return array[0] + "-" + array[1];
    }

    public string GetSavePath(string strPath)
    {
        return Application.dataPath + "/StreamingAssets/Datas/" + strPath;
    }

    public void SetAnimVisible()
    {
        Scheduler.Instance.AddTimer(0.8f, false, delegate
        {
            this.animRoot.SetActive(true);
        });
    }

    private void TryLogin()
    {
        bool flag = false;
        string text = this.AccountText.text;
        string text2 = this.AccountPwd.text;
        UserInfoStorage.StorageInfo.Uid = text;
        UserInfoStorage.StorageInfo.Pwd = ((!flag) ? ((!string.IsNullOrEmpty(text2)) ? text2 : "111111") : "111111");
        Host.WriteUserInfoStorage(true);
        string text3 = this.SeverBenSelect.text;
        if (AppConst.DisableEncryption)
        {
            UserInfoStorage.StorageInfo.Zone = "3";
            ControllerManager.Instance.GetLoginController().ParseNewFLAddress("127.0.0.1:60222");
        }
        else
        {
            UserInfoStorage.StorageInfo.Zone = "2";
            ControllerManager.Instance.GetLoginController().ParseNewFLAddress("101.132.95.10:7000");
        }
        ControllerManager.Instance.GetLoginController().loginModel.Account = text;
        ControllerManager.Instance.GetLoginController().Login();
        ControllerManager.Instance.GetController<LoadTipsController>().ShowReconnectTips("正在连接中");
        PlayerPrefs.SetString("cur_account", text);
    }

    private void Update()
    {
        if (!this.isLoginByEnterKey && (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)))
        {
            this.isLoginByEnterKey = true;
            this.TryLogin();
        }
    }

    public void close()
    {
        if (this.Root != null)
        {
            UnityEngine.Object.Destroy(this.Root.gameObject);
        }
        ManagerCenter.Instance.GetManager<UIManager>().DeleteUI("UI_P2PLogin");
    }

    public override void OnDispose()
    {
        if (null != this.screenShot)
        {
            UnityEngine.Object.DestroyImmediate(this.screenShot);
        }
        if (null != this.rt)
        {
            UnityEngine.Object.DestroyImmediate(this.rt);
        }
        UI_P2PLogin.OnloginView = false;
        base.OnDispose();
        Scheduler.Instance.RemoveUpdator(new Scheduler.OnScheduler(this.Update));
    }

    public void ShowStaticImg()
    {
        this.bakImg.SetActive(true);
        Camera.main.targetTexture = (this.rig.texture as RenderTexture);
        Camera.main.Render();
        RenderTexture.active = this.rt;
        this.screenShot = new Texture2D(this.rt.width, this.rt.height, TextureFormat.RGB24, false);
        RenderTexture active = RenderTexture.active;
        this.screenShot.ReadPixels(new Rect(0f, 0f, (float)this.rt.width, (float)this.rt.height), 0, 0);
        this.screenShot.Apply();
        this.bakImg.GetComponent<RawImage>().texture = this.screenShot;
        Camera.main.targetTexture = null;
        RenderTexture.active = null;
    }

    public Transform Root;

    public GameObject LoginBtn;

    public InputField SeverBenSelect;

    public InputField AccountText;

    public InputField AccountPwd;

    public static bool OnloginView;

    private bool isLoginByEnterKey;

    public GameObject bakImg;

    private RawImage rig;

    private RenderTexture rt;

    private Texture2D screenShot;

    private GameObject animRoot;
}
