using System;
using Framework.Managers;
using UI.Login;
using UnityEngine;
using UnityEngine.UI;

public class Display : MonoBehaviour
{
    public static global::Display Instance
    {
        get
        {
            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;
        setMask(true);
    }

    private void Start()
    {
        alpha = rawImage.gameObject.GetComponent<TweenAlpha>();
        if (null == alpha)
        {
            alpha = rawImage.gameObject.AddComponent<TweenAlpha>();
        }
    }

    public void StartDisplay()
    {
        alpha = rawImage.gameObject.GetComponent<TweenAlpha>();
        if (null == alpha)
        {
            alpha = rawImage.gameObject.AddComponent<TweenAlpha>();
        }

        this.alpha.from = 1f;
        this.alpha.to = 0f;
        this.alpha.ignoreTimeScale = false;
        this.alpha.delay = 0.5f;
        this.alpha.duration = 0.5f;
        this.alpha.onFinished = new UITweener.OnFinished(this.onFadeOut);
        this.alpha.Reset();
        this.alpha.Play(true);
    }

    private void LoadMap(string imagename)
    {
        if (string.IsNullOrEmpty(imagename))
        {
            this.EndDisplay();
            return;
        }
        FFDebug.Log(this, FFLogType.Default, string.Format("loadmap : {0}", imagename));
        ManagerCenter.Instance.GetManager<UITextureMgr>().GetTexture(ImageType.STARTUP, imagename, delegate (UITextureAsset item)
        {
            if (this.lastLoadedTexture != null)
            {
                this.lastLoadedTexture.TryUnload();
            }
            if (this.rawImage == null)
            {
                return;
            }
            if (item == null)
            {
                this.EndDisplay();
                return;
            }
            this.lastLoadedTexture = item;
            this.rawImage.texture = item.textureObj;
            this.rawImage.color = Color.white;
            this.alpha.from = 0f;
            this.alpha.to = 1f;
            this.alpha.ignoreTimeScale = true;
            this.alpha.delay = 0f;
            this.alpha.duration = 0.5f;
            this.alpha.onFinished = new UITweener.OnFinished(this.onFadeIn);
            this.alpha.Reset();
            this.alpha.Play(true);
        });
    }

    public void onFadeIn(UITweener tween)
    {
        this.alpha.from = 1f;
        this.alpha.to = 0f;
        this.alpha.ignoreTimeScale = true;
        this.alpha.delay = 2f;
        this.alpha.duration = 0.5f;
        this.alpha.onFinished = new UITweener.OnFinished(this.onFadeOut);
        this.alpha.Reset();
        this.alpha.Play(true);
    }

    public void onFadeOut(UITweener tween)
    {
        if (this.currDisIndex >= this.displayimgs.Length)
        {
            if (this.lastLoadedTexture != null)
            {
                this.lastLoadedTexture.TryUnload();
            }
            this.EndDisplay();
        }
        else
        {
            this.LoadMap(this.displayimgs[this.currDisIndex++]);
        }
    }

    private void EndDisplay()
    {
        this.rawImage.transform.parent.GetComponent<Image>().enabled = false;
        this.rawImage.transform.gameObject.SetActive(false);
        this.setMask(false);
        if (UIManager.Instance.UIRoot != null)
        {
            Transform transform = UIManager.Instance.UIRoot.Find("CameraWakeup");
            if (transform != null)
            {
                transform.gameObject.SetActive(true);
            }
        }
        UI_P2PLogin uiobject = UIManager.GetUIObject<UI_P2PLogin>();
        if (null != uiobject)
        {
            uiobject.SetAnimVisible();
            EntitiesManager manager = ManagerCenter.Instance.GetManager<EntitiesManager>();
        }
        else
        {
            LoginP2PController controller = ControllerManager.Instance.GetController<LoginP2PController>();
            controller.onLogonShow = (Action)Delegate.Combine(controller.onLogonShow, new Action(delegate ()
            {
                UIManager.GetUIObject<UI_P2PLogin>().SetAnimVisible();
                EntitiesManager manager2 = ManagerCenter.Instance.GetManager<EntitiesManager>();
            }));
        }
    }

    public void setMask(bool visible)
    {
        this.layerMask.gameObject.SetActive(visible);
        this.layerMask.GetComponent<Image>().enabled = visible;
    }

    public RawImage rawImage;

    private UITextureAsset lastLoadedTexture;

    private string[] displayimgs = new string[]
    {
        "bandai_02",
        "giant_03",
        "jkyx_04"
    };

    private int currDisIndex;

    private static global::Display _instance;

    public GameObject layerMask;

    private TweenAlpha alpha;
}
