using System;
using System.Collections.Generic;
using Framework.Managers;
using LuaInterface;
using UnityEngine;
using UnityEngine.UI;

public class UI_Loading : UIPanelBase
{
    public static void LoadView()
    {
        ManagerCenter.Instance.GetManager<UIManager>().ShowUI<UI_Loading>("ui_loading", null, UIManager.ParentType.Loading, false);
    }

    public static void StartLoading(SceneInfo sceneInfo = null)
    {
        CommonTools.SetScalerMode(CanvasScaler.ScaleMode.ScaleWithScreenSize);
        if (UI_Loading.isLoading)
        {
            return;
        }
        UI_Loading.isLoading = true;
        UI_Loading uiobject = UIManager.GetUIObject<UI_Loading>();
        if (uiobject)
        {
            uiobject.Show();
            uiobject.LoadMapTexture(sceneInfo);
        }
    }

    public static void SetLoadingProgress(float progress)
    {
        UI_Loading uiobject = UIManager.GetUIObject<UI_Loading>();
        if (uiobject)
        {
            uiobject.SetTrueProgress(progress);
        }
    }

    public static void EndLoading()
    {
        if (!UI_Loading.isLoading)
        {
            return;
        }
        UI_Loading.isLoading = false;
        UI_Loading uiobject = UIManager.GetUIObject<UI_Loading>();
        if (uiobject)
        {
            uiobject.Hide();
        }
    }

    public override void OnInit(Transform root)
    {
        base.OnInit(root);
        this.Root = root;
        this.Root.gameObject.SetActive(false);
        this.mSlider = this.Root.transform.Find("Slider").GetComponent<Slider>();
        this.bg = this.Root.Find("Offset_Loading/Panel_Loading/img_bg").GetComponent<Image>();
        this.textProgress = this.Root.Find("Offset_Loading/txt_loading").GetComponent<Text>();
        this.bg.gameObject.SetActive(false);
    }

    public void Show()
    {
        if (this.Root.gameObject.activeSelf)
        {
            return;
        }
        this.FakeProgress = 0f;
        this.TrueProgress = 0f;
        this.NeedHide = -1;
        Scheduler.Instance.AddUpdator(new Scheduler.OnScheduler(this.Update));
        this.Root.gameObject.SetActive(true);
        this.Update();
    }

    private void LoadMapTexture(SceneInfo sceneInfo)
    {
        uint mapid = sceneInfo.mapid;
        string texName = "sgc_01";
        if (mapid != 0U)
        {
            LuaTable xmlConfigTable = LuaConfigManager.GetXmlConfigTable("scenesinfo");
            if (xmlConfigTable != null)
            {
                LuaTable cacheField_Table = xmlConfigTable.GetCacheField_Table("mapinfo");
                LuaTable cacheField_Table2 = cacheField_Table.GetCacheField_Table(mapid.ToString());
                if (cacheField_Table2 != null)
                {
                    string field_String = cacheField_Table2.GetField_String("loadingtexture");
                    if (!string.IsNullOrEmpty(field_String))
                    {
                        string[] array = field_String.Split(new char[]
                        {
                            ','
                        });
                        string texName2 = array[UnityEngine.Random.Range(0, array.Length)];
                        texName = texName2;
                    }
                }
            }
            AbattoirMatchController controller = ControllerManager.Instance.GetController<AbattoirMatchController>();
            bool flag = sceneInfo.copymapid.ToString() == controller.copyid;
            if (flag)
            {
                controller.SetMatchState(AbattoirMatchState.Entering);
            }
            UI_MainView uiobject = UIManager.GetUIObject<UI_MainView>();
            if (uiobject != null)
            {
                uiobject.refreshPersonal(flag);
                uiobject.refreshSelfTeamColorSpr(false);
                uiobject.ShowLvFuncShow(!flag);
            }
        }
        if (this.Root != null)
        {
            string name = this.bg.sprite.name;
            if (texName != name && !string.IsNullOrEmpty(texName))
            {
                UITextureMgr.Instance.GetTexture(ImageType.OTHERS, texName, delegate (UITextureAsset tex)
                {
                    if (this.bg == null)
                    {
                        return;
                    }
                    if (tex != null && tex.textureObj != null)
                    {
                        Sprite sprite = Sprite.Create(tex.textureObj, new Rect(0f, 0f, (float)tex.textureObj.width, (float)tex.textureObj.height), new Vector2(0.5f, 0.5f));
                        sprite.name = texName;
                        this.bg.sprite = sprite;
                        this.bg.material = null;
                        this.usedTextureAssets.Add(tex);
                    }
                });
            }
        }
        this.bg.gameObject.SetActive(true);
        this.SetLoadTips();
    }

    private void SetLoadTips()
    {
        if (this.Root != null)
        {
            Transform transform = this.Root.Find("Offset_Loading/Text_tips");
            if (transform != null)
            {
                Text component = transform.GetComponent<Text>();
                if (component != null)
                {
                    component.text = this.GetRandomTipTest();
                }
            }
        }
    }

    private string GetRandomTipTest()
    {
        string result = string.Empty;
        if (this.loadingTipsLst.Count == 0)
        {
            this.InitLoadTips();
        }
        int num = 1;
        if (MainPlayer.Self != null)
        {
            num = (int)MainPlayer.Self.OtherPlayerData.MapUserData.mapdata.level;
        }
        if (this.lstInLevelLimit == null)
        {
            this.lstInLevelLimit = new List<LoadingTipItem>();
        }
        else
        {
            this.lstInLevelLimit.Clear();
        }
        if (this.loadingTipsLst.Count > 0)
        {
            for (int i = 0; i < this.loadingTipsLst.Count; i++)
            {
                if (num >= this.loadingTipsLst[i].levelMin && num <= this.loadingTipsLst[i].levelMax)
                {
                    this.lstInLevelLimit.Add(this.loadingTipsLst[i]);
                }
            }
        }
        if (this.lstInLevelLimit.Count > 0)
        {
            int count = this.lstInLevelLimit.Count;
            int index = UnityEngine.Random.Range(0, count);
            result = this.lstInLevelLimit[index].text;
            this.loadingTipsLst.RemoveAt(index);
        }
        return result;
    }

    private void InitLoadTips()
    {
        List<LuaTable> configTableList = LuaConfigManager.GetConfigTableList("loadingtips");
        for (int i = 0; i < configTableList.Count; i++)
        {
            LuaTable luaTable = configTableList[i];
            LoadingTipItem loadingTipItem = new LoadingTipItem();
            loadingTipItem.levelMin = int.Parse(luaTable.GetField_String("min_lv"));
            loadingTipItem.levelMax = int.Parse(luaTable.GetField_String("max_lv"));
            loadingTipItem.text = luaTable.GetField_String("tips");
            this.loadingTipsLst.Add(loadingTipItem);
        }
    }

    public void SetTrueProgress(float Progress)
    {
        this.TrueProgress = ((Progress <= 1f) ? Progress : 1f);
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
        this.textProgress.text = (this.Progress * 100f).ToString("f0") + "%";
        if (this.NeedHide >= 0)
        {
            this.NeedHide--;
            if (this.NeedHide == 0)
            {
                this.TrueHide();
            }
        }
        Color color = this.bg.color;
        color.a = Mathf.Lerp(color.a, 1f, this.FakeProgress);
        this.bg.color = color;
    }

    public void Hide()
    {
        this.NeedHide = 5;
        this.TrueProgress = 1f;
        Color white = Color.white;
        white.a = 0f;
        this.bg.color = white;
        this.Update();
    }

    private void TrueHide()
    {
        this.NeedHide = -1;
        this.FakeProgress = 0f;
        this.TrueProgress = 0f;
        this.speedState = 0;
        this.Root.gameObject.SetActive(false);
        this.bg.gameObject.SetActive(false);
        Scheduler.Instance.RemoveUpdator(new Scheduler.OnScheduler(this.Update));
    }

    public override void OnDispose()
    {
        this.TrueHide();
        base.OnDispose();
    }

    public static bool isLoading;

    private Transform Root;

    private Image bg;

    private Text textProgress;

    private Slider mSlider;

    private List<LoadingTipItem> loadingTipsLst = new List<LoadingTipItem>();

    private int UPDATE_TIPS_TIME = 8;

    private List<LoadingTipItem> lstInLevelLimit;

    private float FakeRate = 0.8f;

    private float FakeSpeed = 1f;

    private float FakeProgress;

    private float TrueProgress;

    private int speedState;

    private int NeedHide = -1;
}
