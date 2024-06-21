using System;
using System.Collections.Generic;
using Framework.Base;
using Framework.Managers;
using LuaInterface;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : IManager
{
    public event Action<string> eventShowUI;

    public event Action<string> eventDeleteUI;

    public static UIManager Instance
    {
        get
        {
            return ManagerCenter.Instance.GetManager<UIManager>();
        }
    }

    public static T GetUIObject<T>() where T : UIPanelBase
    {
        Type typeFromHandle = typeof(T);
        if (UIManager.UIPanelBaseMap.ContainsKey(typeFromHandle))
        {
            return UIManager.UIPanelBaseMap[typeFromHandle] as T;
        }
        return (T)((object)null);
    }

    public static UIPanelBase GetUIObject(string uiname)
    {
        if (UIManager.UINameToTypeMap.ContainsKey(uiname))
        {
            Type key = UIManager.UINameToTypeMap[uiname];
            if (UIManager.UIPanelBaseMap.ContainsKey(key))
            {
                return UIManager.UIPanelBaseMap[key];
            }
        }
        return null;
    }

    public static void AddUIPanel(UIPanelBase panel)
    {
        if (!object.ReferenceEquals(panel, null))
        {
            UIManager.UIPanelBaseMap[panel.uiClassType] = panel;
            UIManager.UINameToTypeMap[panel.uiName] = panel.uiClassType;
        }
    }

    public static void AddLuaUIPanel(LuaPanelBase panel)
    {
        if (!object.ReferenceEquals(panel, null))
        {
            UIManager.LuaPanelBaseMap[panel.uiName] = panel;
            if (UIManager.lunfunaddopeningui == null)
            {
                UIManager.lunfunaddopeningui = LuaScriptMgr.Instance.GetLuaFunction("UIMgr.AddOpeningUI");
            }
            UIManager.lunfunaddopeningui.Call(new object[]
            {
                panel.uiName
            });
        }
    }

    public static LuaPanelBase GetLuaUIPanel(string uiname)
    {
        if (UIManager.LuaPanelBaseMap.ContainsKey(uiname))
        {
            return UIManager.LuaPanelBaseMap[uiname];
        }
        return null;
    }

    public static bool IsLuaPanelExists(string uiname)
    {
        return !string.IsNullOrEmpty(uiname) && UIManager.LuaPanelBaseMap.ContainsKey(uiname);
    }

    public static bool IsUIPanelExists(string uiname)
    {
        return UIManager.UINameToTypeMap.ContainsKey(uiname);
    }

    public static bool IsUIPanelExists(Type uiClassType)
    {
        return UIManager.UIPanelBaseMap.ContainsKey(uiClassType);
    }

    public static void RemoveUIPanel(UIPanelBase panel)
    {
        if (!object.ReferenceEquals(panel, null))
        {
            UIManager.UIPanelBaseMap.Remove(panel.uiClassType);
            UIManager.UINameToTypeMap.Remove(panel.uiName);
        }
    }

    public static void RemoveLuaUIPanel(LuaPanelBase panel)
    {
        if (!object.ReferenceEquals(panel, null))
        {
            UIManager.LuaPanelBaseMap.Remove(panel.uiName);
            if (UIManager.lunfunremoveopeningui == null)
            {
                UIManager.lunfunremoveopeningui = LuaScriptMgr.Instance.GetLuaFunction("UIMgr.RemoveOpeningUI");
            }
            UIManager.lunfunremoveopeningui.Call(new object[]
            {
                panel.uiName
            });
        }
    }

    private FontMgr mFontMgr
    {
        get
        {
            return ManagerCenter.Instance.GetManager<FontMgr>();
        }
    }

    public Image Img_mask
    {
        get
        {
            return this.img_mask;
        }
    }

    public Transform UIRoot
    {
        get
        {
            return this.uiRoot;
        }
    }

    public void Init()
    {
        UGUINameContent.RegisterUINameAndAssetbundleName();
        this.uiRoot = GameObject.Find("UIRoot").transform;
        this.layerMask = this.uiRoot.Find("LayerMask");
        UI_Loading.LoadView();
        this.ParentMap[UIManager.ParentType.UIRoot] = this.uiRoot;
        this.ParentMap[UIManager.ParentType.Map] = this.uiRoot.transform.Find("LayerMap").transform;
        this.ParentMap[UIManager.ParentType.Main] = this.uiRoot.transform.Find("LayerMain").transform;
        this.ParentMap[UIManager.ParentType.CommonUI] = this.uiRoot.transform.Find("LayerCommon").transform;
        this.ParentMap[UIManager.ParentType.Loading] = this.uiRoot.transform.Find("LayerLoading").transform;
        this.ParentMap[UIManager.ParentType.HPRoot] = GameObject.Find("hpRoot").transform;
        this.ParentMap[UIManager.ParentType.Tips] = this.uiRoot.transform.Find("LayerTips").transform;
        this.ParentMap[UIManager.ParentType.UICamera] = GameObject.Find("Camera").transform;
        this.ParentMap[UIManager.ParentType.Guide] = this.uiRoot.transform.Find("LayerGuide").transform;
        this.img_mask = this.uiRoot.transform.Find("Img_Mask").gameObject.GetComponent<Image>();
        this.SetMaskImage(false);
        FFDebug.OnResourceError = delegate (object obj, object str)
        {
            TipsWindow.ShowWindow(str.ToString());
        };
        ControllerManager.Instance.GetController<GuideController>().eventGuideDeleteUI += this.GuideDeleteUI;
    }

    public void ReLoad()
    {
        UI_Loading.LoadView();
    }

    public Transform GetUICamera()
    {
        return this.ParentMap[UIManager.ParentType.UICamera];
    }

    public Transform GetUIParent(UIManager.ParentType PType)
    {
        return this.ParentMap[PType];
    }

    private bool CheckeUIStack(string name)
    {
        for (int i = 0; i < this.listUIStack.Count; i++)
        {
            if (this.listUIStack[i].Equals(name))
            {
                return true;
            }
        }
        return false;
    }

    private void OpenUI(string uiname)
    {
        if (this.dicUIAndTagDic.ContainsKey(uiname))
        {
            EscManager manager = ManagerCenter.Instance.GetManager<EscManager>();
            if (manager == null)
                Debug.LogError("1");
            if(manager.mHidePanelList == null)
                Debug.LogError("2");
            if (!manager.mHidePanelList.Contains(uiname))
            {
                manager.OpenUI(uiname);
            }
            if (this.dicUIAndTagDic[uiname] == UITag.FirstUI)
            {
                if (this.CheckeUIStack(uiname))
                {
                    this.listUIStack.Remove(uiname);
                    this.listUIStack.Add(uiname);
                }
                else
                {
                    if (this.listUIStack.Count >= this.UIStackCount)
                    {
                        this.DeleteUI(this.listUIStack[0]);
                        this.listUIStack.Remove(this.listUIStack[0]);
                    }
                    this.listUIStack.Add(uiname);
                }
                for (int i = 0; i < this.listUIStack.Count; i++)
                {
                    if (this.listUIStack[i].Equals(uiname))
                    {
                        UIPanelBase uiobject = UIManager.GetUIObject(this.listUIStack[i]);
                        if (uiobject != null)
                        {
                            uiobject.SetActive(true);
                        }
                        LuaPanelBase luaUIPanel = UIManager.GetLuaUIPanel(this.listUIStack[i]);
                        if (luaUIPanel != null)
                        {
                            luaUIPanel.SetActive(true);
                        }
                    }
                }
            }
            else if (this.dicUIAndTagDic[uiname] == UITag.SecondUI)
            {
                if (!string.IsNullOrEmpty(this.tagSecondUI) && !uiname.Equals(this.tagSecondUI))
                {
                    this.DeleteUI(this.tagSecondUI);
                }
                this.tagSecondUI = uiname;
            }
            else if (this.dicUIAndTagDic[uiname] == UITag.ThirdUI)
            {
                if (!string.IsNullOrEmpty(this.tagThirdUI) && !uiname.Equals(this.tagThirdUI))
                {
                    this.DeleteUI(this.tagThirdUI);
                }
                this.tagThirdUI = uiname;
            }
        }
    }

    public void maskFadeInOut(float from, float to, float duration, UIManager.OnMaskFinished callback)
    {
        this.onMaskFinished = callback;
        this.img_mask.gameObject.SetActive(true);
        this.img_mask.enabled = true;
        TweenAlpha tweenAlpha = this.img_mask.gameObject.GetComponent<TweenAlpha>();
        if (null == tweenAlpha)
        {
            tweenAlpha = this.img_mask.gameObject.AddComponent<TweenAlpha>();
        }
        tweenAlpha.from = from;
        tweenAlpha.to = to;
        tweenAlpha.ignoreTimeScale = false;
        tweenAlpha.duration = duration;
        tweenAlpha.onFinished = new UITweener.OnFinished(this.onFadeInOut);
        tweenAlpha.Reset();
        tweenAlpha.Play(true);
    }

    public void onFadeInOut(UITweener tween)
    {
        this.img_mask.gameObject.SetActive(false);
        this.img_mask.enabled = false;
        if (this.onMaskFinished != null)
        {
            this.onMaskFinished();
        }
    }

    public void ResetMaskInfo(bool visible)
    {
        this.img_mask.gameObject.SetActive(visible);
        this.img_mask.enabled = visible;
    }

    public void SetUIVisible(UIManager.ParentType parentType, bool visible)
    {
        if (!this.ParentMap.ContainsKey(parentType))
        {
            return;
        }
        this.ParentMap[parentType].gameObject.SetActive(visible);
    }

    private void CloseUI(string uiname)
    {
        if (this.dicUIAndTagDic.ContainsKey(uiname))
        {
            if (null != CameraController.Self)
            {
                CameraController.Self.OpenUIActiveSceneCamera(true);
                this.ParentMap[UIManager.ParentType.Main].gameObject.SetActive(true);
            }
            if (this.dicUIAndTagDic[uiname] == UITag.FirstUI)
            {
                if (this.CheckeUIStack(uiname))
                {
                    this.listUIStack.Remove(uiname);
                }
                if (this.listUIStack.Count > 0)
                {
                }
            }
            else if (this.dicUIAndTagDic[uiname] == UITag.SecondUI)
            {
                if (this.tagSecondUI.Equals(uiname))
                {
                    this.tagSecondUI = null;
                }
            }
            else if (this.dicUIAndTagDic[uiname] == UITag.ThirdUI && this.tagThirdUI.Equals(uiname))
            {
                this.tagThirdUI = null;
            }
        }
    }

    public void ShowUI(string uiName, UIManager.ParentType UILayer = UIManager.ParentType.CommonUI)
    {
        this.ShowUI(uiName, null, UILayer);
    }

    public void ShowUI(string uiName, LuaFunction lunFunction, UIManager.ParentType UILayer = UIManager.ParentType.CommonUI)
    {
        for (int i = 0; i < this.luaNotDelUINames.Length; i++)
        {
            if (uiName == this.luaNotDelUINames[i])
            {
                LuaPanelBase luaUIPanel = UIManager.GetLuaUIPanel(uiName);
                if (luaUIPanel != null)
                {
                    luaUIPanel.uiRoot.transform.localPosition = Vector3.zero;
                    luaUIPanel.uiRoot.transform.SetAsLastSibling();
                    if (lunFunction != null)
                    {
                        lunFunction.Call();
                    }
                    EscManager manager = ManagerCenter.Instance.GetManager<EscManager>();
                    if (!manager.mHidePanelList.Contains(uiName))
                    {
                        manager.OpenUI(uiName);
                    }
                    return;
                }
            }
        }
        if (UIManager.IsUIPanelExists(uiName))
        {
            this.OpenUI(uiName);
            if (UILayer == UIManager.ParentType.CommonUI && this.dicUIAndTagDic[uiName] == UITag.FirstUI && null != CameraController.Self)
            {
                CameraController.Self.OpenUIActiveSceneCamera(GameSystemSettings.GetActiveSceneCamera());
                this.ParentMap[UIManager.ParentType.Main].gameObject.SetActive(GameSystemSettings.GetActiveSceneCamera());
            }
            if (lunFunction != null)
            {
                lunFunction.Call(new object[]
                {
                    UIManager.GetLuaUIPanel(uiName).uiRoot
                });
            }
            if (this.eventShowUI != null)
            {
                this.eventShowUI(uiName);
            }
            return;
        }
        if (UIManager.IsLuaPanelExists(uiName))
        {
            this.OpenUI(uiName);
            if (UILayer == UIManager.ParentType.CommonUI && this.dicUIAndTagDic[uiName] == UITag.FirstUI && null != CameraController.Self)
            {
                CameraController.Self.OpenUIActiveSceneCamera(GameSystemSettings.GetActiveSceneCamera());
                this.ParentMap[UIManager.ParentType.Main].gameObject.SetActive(GameSystemSettings.GetActiveSceneCamera());
            }
            if (lunFunction != null)
            {
                lunFunction.Call(new object[]
                {
                    UIManager.GetLuaUIPanel(uiName).uiRoot
                });
            }
            if (this.eventShowUI != null)
            {
                this.eventShowUI(uiName);
            }
            return;
        }
        if (this.m_LoadingUIAssetbundle.Contains(uiName))
        {
            return;
        }
        this.m_LoadingUIAssetbundle.Add(uiName);
        string bundlename = UGUINameContent.UINamePairs[uiName];
        this.LoadUI(uiName, bundlename, delegate (GameObject o)
        {
            this.m_LoadingUIAssetbundle.Remove(uiName);
            if (o == null)
            {
                FFDebug.LogWarning(this, "Load ui " + uiName + " error!!");
                if (lunFunction != null)
                {
                    lunFunction.Call();
                }
                return;
            }
            GameObject gameObject = null;
            this.InitUI(o, uiName, out gameObject, UILayer, false);
            this.OpenUI(uiName);
            if (UILayer == UIManager.ParentType.CommonUI && this.dicUIAndTagDic[uiName] == UITag.FirstUI && null != CameraController.Self)
            {
                CameraController.Self.OpenUIActiveSceneCamera(GameSystemSettings.GetActiveSceneCamera());
                this.ParentMap[UIManager.ParentType.Main].gameObject.SetActive(GameSystemSettings.GetActiveSceneCamera());
            }
            if (lunFunction != null)
            {
                lunFunction.Call(new object[]
                {
                    gameObject
                });
            }
            if (this.eventShowUI != null)
            {
                this.eventShowUI(uiName);
            }
        });
    }

    public void ShowUIByNpcdlg(string uiName, UIManager.ParentType UILayer = UIManager.ParentType.CommonUI)
    {
        this.ShowUIByNpcdlg(uiName, null, UILayer);
    }

    public void ShowUIByNpcdlg(string uiName, LuaFunction lunFunction, UIManager.ParentType UILayer = UIManager.ParentType.CommonUI)
    {
        if (UIManager.IsUIPanelExists(uiName))
        {
            this.OpenUI(uiName);
            if (lunFunction != null)
            {
                lunFunction.Call(new object[]
                {
                    UIManager.GetLuaUIPanel(uiName).uiRoot
                });
            }
            if (this.eventShowUI != null)
            {
                this.eventShowUI(uiName);
            }
            return;
        }
        if (UIManager.IsLuaPanelExists(uiName))
        {
            this.OpenUI(uiName);
            if (lunFunction != null)
            {
                lunFunction.Call(new object[]
                {
                    UIManager.GetLuaUIPanel(uiName).uiRoot
                });
            }
            if (this.eventShowUI != null)
            {
                this.eventShowUI(uiName);
            }
            return;
        }
        if (this.m_LoadingUIAssetbundle.Contains(uiName))
        {
            return;
        }
        this.m_LoadingUIAssetbundle.Add(uiName);
        string bundlename = UGUINameContent.UINamePairs[uiName];
        this.LoadUI(uiName, bundlename, delegate (GameObject o)
        {
            this.m_LoadingUIAssetbundle.Remove(uiName);
            if (o == null)
            {
                FFDebug.LogWarning(this, "Load ui " + uiName + " error!!");
                if (lunFunction != null)
                {
                    lunFunction.Call();
                }
                return;
            }
            GameObject gameObject = null;
            this.InitUI(o, uiName, out gameObject, UILayer, true);
            this.OpenUI(uiName);
            if (lunFunction != null)
            {
                lunFunction.Call(new object[]
                {
                    gameObject
                });
            }
            if (this.eventShowUI != null)
            {
                this.eventShowUI(uiName);
            }
        });
    }

    private void InitUI(GameObject o, string uiname, out GameObject go, UIManager.ParentType mParentType, bool bynpcdlg)
    {
        Transform transform = UnityEngine.Object.Instantiate<Transform>(o.transform);
        go = transform.gameObject;
        if (transform.CompareTag(Const.Tags.Mask))
        {
            Transform transform2 = UnityEngine.Object.Instantiate<Transform>(this.layerMask);
            transform2.gameObject.SetActive(true);
            transform2.SetParent(transform, false);
        }
        Transform parent = this.ParentMap[mParentType];
        transform.SetParent(parent, false);
        transform.name = uiname;
        transform.transform.localScale = new Vector3(1f, 1f, 1f);
        transform.transform.localPosition = new Vector3(0f, 0f, 0f);
        if (transform.CompareTag(Const.Tags.First))
        {
            this.dicUIAndTagDic[uiname] = UITag.FirstUI;
        }
        else if (transform.CompareTag(Const.Tags.Second))
        {
            this.dicUIAndTagDic[uiname] = UITag.SecondUI;
        }
        else if (transform.CompareTag(Const.Tags.Third))
        {
            this.dicUIAndTagDic[uiname] = UITag.ThirdUI;
        }
        else
        {
            this.dicUIAndTagDic[uiname] = UITag.BasicUI;
        }
        LuaPanelBase luaPanelBase = new LuaPanelBase(go);
        this.checkAndSetBlurScreenShot(transform.transform);
        this.getUIInformation(transform);
        UIManager.AddLuaUIPanel(luaPanelBase);
        luaPanelBase.Awake(bynpcdlg);
    }

    private void checkAndSetBlurScreenShot(Transform uitran)
    {
        ScreenShotManager manager = ManagerCenter.Instance.GetManager<ScreenShotManager>();
        if (manager == null)
        {
            return;
        }
        foreach (UIIdentifier uiidentifier in uitran.GetComponentsInChildren<UIIdentifier>())
        {
            if (uiidentifier._type == UIIdentifier.IdentifierType.UIBlurScreenShot)
            {
                manager.FillRawImageWithScreenShotBlur(uiidentifier.transform);
            }
        }
    }

    private bool IsSpecialUI(string strUIName)
    {
        for (int i = 0; i < this.strSpecialUI.Length; i++)
        {
            if (strUIName.CompareTo(this.strSpecialUI[i]) == 0)
            {
                return true;
            }
        }
        return false;
    }

    public void ShowUI<T>(string uiname, Action complete = null, UIManager.ParentType mParentType = UIManager.ParentType.CommonUI, bool bylua = false) where T : UIPanelBase, new()
    {
        if (UIManager.IsUIPanelExists(uiname))
        {
            this.OpenUI(uiname);
            if (mParentType == UIManager.ParentType.CommonUI && this.dicUIAndTagDic[uiname] == UITag.FirstUI && !this.IsSpecialUI(uiname) && null != CameraController.Self)
            {
                CameraController.Self.OpenUIActiveSceneCamera(GameSystemSettings.GetActiveSceneCamera());
                this.ParentMap[UIManager.ParentType.Main].gameObject.SetActive(GameSystemSettings.GetActiveSceneCamera());
            }
            if (complete != null)
            {
                complete();
            }
            if (this.eventShowUI != null)
            {
                this.eventShowUI(uiname);
            }
            return;
        }
        if (this.m_LoadingUIAssetbundle.Contains(uiname))
        {
            return;
        }
        this.m_LoadingUIAssetbundle.Add(uiname);
        string bundlename = UGUINameContent.UINamePairs[uiname];
        this.LoadUI(uiname, bundlename, delegate (GameObject o)
        {
            this.m_LoadingUIAssetbundle.Remove(uiname);
            if (o == null)
            {
                FFDebug.LogWarning(this, "Load ui " + uiname + " error!!");
                if (complete != null)
                {
                    complete();
                }
                return;
            }
            this.InitUI<T>(o, uiname, mParentType, bylua);
            this.OpenUI(uiname);
            if (mParentType == UIManager.ParentType.CommonUI && this.dicUIAndTagDic[uiname] == UITag.FirstUI && !this.IsSpecialUI(uiname) && null != CameraController.Self)
            {
                CameraController.Self.OpenUIActiveSceneCamera(GameSystemSettings.GetActiveSceneCamera());
                this.ParentMap[UIManager.ParentType.Main].gameObject.SetActive(GameSystemSettings.GetActiveSceneCamera());
            }
            if (complete != null)
            {
                complete();
            }
            if (this.eventShowUI != null)
            {
                this.eventShowUI(uiname);
            }
        });
    }

    private void InitUI<T>(GameObject o, string uiname, UIManager.ParentType mParentType, bool bylua) where T : UIPanelBase, new()
    {
        Transform transform = UnityEngine.Object.Instantiate<Transform>(o.transform);
        if (transform.CompareTag(Const.Tags.Mask))
        {
            Transform transform2 = UnityEngine.Object.Instantiate<Transform>(this.layerMask);
            transform2.gameObject.SetActive(true);
            transform2.SetParent(transform, false);
        }
        Transform parent = this.ParentMap[mParentType];
        transform.SetParent(parent, false);
        transform.name = uiname;
        transform.transform.localScale = new Vector3(1f, 1f, 1f);
        transform.transform.localPosition = new Vector3(0f, 0f, 0f);
        if (transform.CompareTag(Const.Tags.First))
        {
            this.dicUIAndTagDic[uiname] = UITag.FirstUI;
        }
        else if (transform.CompareTag(Const.Tags.Second))
        {
            this.dicUIAndTagDic[uiname] = UITag.SecondUI;
        }
        else if (transform.CompareTag(Const.Tags.Third))
        {
            this.dicUIAndTagDic[uiname] = UITag.ThirdUI;
        }
        else
        {
            this.dicUIAndTagDic[uiname] = UITag.BasicUI;
        }
        T t = Activator.CreateInstance<T>();
        t.Init(transform, uiname, bylua);
        if (transform.GetComponent<Canvas>() == null)
        {
            transform.gameObject.AddComponent<GraphicRaycaster>();
        }
        this.getUIInformation(transform);
        UIManager.AddUIPanel(t);
        t.OnInit(t.uiPanelRoot);
        t.AfterInit();
    }

    public void getUIInformation(Transform root)
    {
        UIInformationList[] componentsInChildren = root.gameObject.GetComponentsInChildren<UIInformationList>(true);
        LuaTable luaTable = null;
        for (int i = 0; i < componentsInChildren.Length; i++)
        {
            Text component = componentsInChildren[i].GetComponent<Text>();
            if (componentsInChildren[i].listInformation.Count > 0)
            {
                ControllerManager.Instance.GetController<TextModelController>().setText(component, componentsInChildren[i].listInformation[0].content);
            }
            if (LuaConfigManager.GetXmlConfigTable("UIInformation") != null)
            {
                luaTable = LuaConfigManager.GetXmlConfigTable("UIInformation").GetCacheField_Table("item");
            }
            for (int j = 0; j < componentsInChildren[i].listInformation.Count; j++)
            {
                if (luaTable == null)
                {
                    return;
                }
                LuaTable cacheField_Table = luaTable.GetCacheField_Table(componentsInChildren[i].listInformation[j].id);
                if (cacheField_Table != null)
                {
                    string cacheField_String = cacheField_Table.GetCacheField_String("content");
                    componentsInChildren[i].listInformation[j].content = ControllerManager.Instance.GetController<TextModelController>().ChangeTextModel(cacheField_String);
                }
            }
            if (componentsInChildren[i].listInformation.Count > 0)
            {
                component.text = componentsInChildren[i].listInformation[0].content.ToString();
            }
        }
    }

    public void DeleteUI(string UIName)
    {
        for (int i = 0; i < this.luaNotDelUINames.Length; i++)
        {
            if (UIName == this.luaNotDelUINames[i])
            {
                UIManager.GetLuaUIPanel(UIName).uiRoot.transform.localPosition = new Vector3(10000f, 10000f, 0f);
                EscManager manager = ManagerCenter.Instance.GetManager<EscManager>();
                manager.CloseUI(UIName);
                return;
            }
        }
        if (ControllerManager.Instance.GetController<GuideController>().IsGuide && this.guideDeleteUIName == UIName)
        {
            UIPanelBase uiobject = UIManager.GetUIObject(UIName);
            if (uiobject != null)
            {
                uiobject.uiPanelRoot.SetAsLastSibling();
            }
            LuaPanelBase luaUIPanel = UIManager.GetLuaUIPanel(UIName);
            if (luaUIPanel != null)
            {
                luaUIPanel.uiRoot.transform.SetAsLastSibling();
            }
            return;
        }
        UIPanelBase uiobject2 = UIManager.GetUIObject(UIName);
        this.DeleteUI(uiobject2);
        LuaPanelBase luaUIPanel2 = UIManager.GetLuaUIPanel(UIName);
        this.DeleteUI(luaUIPanel2, false, false);
    }

    public void DeleteUI(string UIName, bool isImmediately)
    {
        if (ControllerManager.Instance.GetController<GuideController>().IsGuide && this.guideDeleteUIName == UIName)
        {
            UIPanelBase uiobject = UIManager.GetUIObject(UIName);
            if (uiobject != null)
            {
                uiobject.uiPanelRoot.SetAsLastSibling();
            }
            LuaPanelBase luaUIPanel = UIManager.GetLuaUIPanel(UIName);
            if (luaUIPanel != null)
            {
                luaUIPanel.uiRoot.transform.SetAsLastSibling();
            }
            return;
        }
        UIPanelBase uiobject2 = UIManager.GetUIObject(UIName);
        this.DeleteUI(uiobject2, isImmediately);
        LuaPanelBase luaUIPanel2 = UIManager.GetLuaUIPanel(UIName);
        this.DeleteUI(luaUIPanel2, isImmediately, false);
    }

    public void DeleteUI<T>() where T : UIPanelBase
    {
        T uiobject = UIManager.GetUIObject<T>();
        this.DeleteUI(uiobject);
    }

    public void DeleteUI(UIPanelBase panel)
    {
        if (panel != null && !this.uiDeling.Contains(panel.uiName))
        {
            EscManager escManager = ManagerCenter.Instance.GetManager<EscManager>();
            if (escManager.IsFunctionUI(panel.uiName))
            {
                escManager.PlayFadeInOut(panel.uiName, false);
                this.uiDeling.Add(panel.uiName);
                Scheduler.Instance.AddTimer(EscManager.DURATION, false, delegate
                {
                    escManager.CloseUI(panel.uiName);
                    this.DeleteUIDo(panel);
                    this.uiDeling.Remove(panel.uiName);
                });
            }
            else
            {
                this.DeleteUIDo(panel);
            }
        }
    }

    public void DeleteUI(UIPanelBase panel, bool isImmediately)
    {
        if (panel != null && !this.uiDeling.Contains(panel.uiName))
        {
            EscManager escManager = ManagerCenter.Instance.GetManager<EscManager>();
            if (escManager.IsFunctionUI(panel.uiName))
            {
                escManager.PlayFadeInOut(panel.uiName, false);
                this.uiDeling.Add(panel.uiName);
                if (isImmediately)
                {
                    escManager.CloseUI(panel.uiName);
                    this.DeleteUIDo(panel);
                    this.uiDeling.Remove(panel.uiName);
                }
                else
                {
                    Scheduler.Instance.AddTimer(EscManager.DURATION, false, delegate
                    {
                        escManager.CloseUI(panel.uiName);
                        this.DeleteUIDo(panel);
                        this.uiDeling.Remove(panel.uiName);
                    });
                }
            }
            else
            {
                this.DeleteUIDo(panel);
            }
        }
    }

    private void DeleteUIDo(UIPanelBase panel)
    {
        panel.OnDispose();
        if (this.eventDeleteUI != null)
        {
            this.eventDeleteUI(panel.uiName);
        }
        this.CloseUI(panel.uiName);
        panel.Dispose();
        UIManager.RemoveUIPanel(panel);
        this.UnloadUIAb(panel.uiName);
    }

    public void DeleteUI(LuaPanelBase panel, bool needDeleteTask = false, bool isImmediately = false)
    {
        if (panel != null && !this.uiDeling.Contains(panel.uiName))
        {
            EscManager escManager = ManagerCenter.Instance.GetManager<EscManager>();
            if (escManager.IsFunctionUI(panel.uiName))
            {
                escManager.PlayFadeInOut(panel.uiName, false);
                this.uiDeling.Add(panel.uiName);
                Action actionDel = delegate ()
                {
                    escManager.CloseUI(panel.uiName);
                    this.DeleteUIDo(panel, needDeleteTask);
                    this.uiDeling.Remove(panel.uiName);
                };
                if (isImmediately)
                {
                    actionDel();
                }
                else
                {
                    Scheduler.Instance.AddTimer(EscManager.DURATION, false, delegate
                    {
                        actionDel();
                    });
                }
            }
            else
            {
                this.DeleteUIDo(panel, needDeleteTask);
            }
        }
    }

    private void DeleteUIDo(LuaPanelBase panel, bool needDeleteTask = false)
    {
        panel.OnDispose();
        if (this.eventDeleteUI != null)
        {
            this.eventDeleteUI(panel.uiName);
        }
        this.CloseUI(panel.uiName);
        panel.Dispose();
        UIManager.RemoveLuaUIPanel(panel);
        this.UnloadUIAb(panel.uiName);
    }

    private void UnloadUIAb(string uiname)
    {
        string key = "ui/" + UGUINameContent.UINamePairs[uiname] + ".u";
        if (UILoader.UIAssetBundles.ContainsKey(key))
        {
            UILoader.UIAssetBundles[key].UnLoadThis();
        }
    }

    public void DeleteAllUI(bool isImmediately = false)
    {
        if (TaskController.CurrnetNpcDlg != null)
        {
            TaskController.CurrnetNpcDlg.UnInit();
        }
        this.RmoveList.Clear();
        UIManager.UIPanelBaseMap.BetterForeach(delegate (KeyValuePair<Type, UIPanelBase> item)
        {
            this.RmoveList.Add(item.Value);
        });
        ManagerCenter.Instance.GetManager<ObjectPoolManager>().RemoveAllNotAutoRemoveObjectPool();
        IconRenderCtrl.Reset();
        for (int i = 0; i < this.RmoveList.Count; i++)
        {
            UIPanelBase panel = this.RmoveList[i];
            this.DeleteUI(panel, isImmediately);
        }
        this.RmoveListForLua.Clear();
        UIManager.LuaPanelBaseMap.BetterForeach(delegate (KeyValuePair<string, LuaPanelBase> item)
        {
            this.RmoveListForLua.Add(item.Value);
        });
        for (int j = 0; j < this.RmoveListForLua.Count; j++)
        {
            LuaPanelBase panel2 = this.RmoveListForLua[j];
            this.DeleteUI(panel2, true, true);
        }
    }

    public void DeleteAllUIWithOutList(List<Type> uiList)
    {
        this.RmoveList.Clear();
        UIManager.UIPanelBaseMap.BetterForeach(delegate (KeyValuePair<Type, UIPanelBase> item)
        {
            if (!uiList.Contains(item.Key))
            {
                this.RmoveList.Add(item.Value);
            }
        });
        for (int i = 0; i < this.RmoveList.Count; i++)
        {
            UIPanelBase panel = this.RmoveList[i];
            this.DeleteUI(panel);
        }
        this.RmoveList.Clear();
    }

    public void OnMapChangeCloseUI()
    {
        this.DeleteAllUIWithOutList(new List<string>
        {
            "UI_Guide",
            "UI_Tips",
            "UI_Main",
            "UI_NPCtalk",
            "UI_LoadTips",
            "ui_loading",
            "UI_HpHit",
            "UI_HpSystem",
            "UI_Map",
            "UI_NPCtalkAndTaskDlg",
            "UI_CoolDown",
            "UI_AbattoirMatch",
            "UI_Confirm",
            "UI_MatchComplete"
        });
    }

    public void RegUINameOpenByNpc(string uiName)
    {
        if (!this.uiNamesOpenByNpc.Contains(uiName))
        {
            this.uiNamesOpenByNpc.Add(uiName);
        }
    }

    public void UnRegUINameOpenByNpc(string uiName)
    {
        if (this.uiNamesOpenByNpc.Contains(uiName))
        {
            this.uiNamesOpenByNpc.Remove(uiName);
        }
    }

    public void CloseOpenByNpcUI()
    {
        this.DeleteUIWithNameList(this.uiNamesOpenByNpc);
        this.uiNamesOpenByNpc.Clear();
    }

    public void DeleteUIWithNameList(List<string> uiList)
    {
        this.RmoveList.Clear();
        UIManager.UINameToTypeMap.BetterForeach(delegate (KeyValuePair<string, Type> item)
        {
            if (uiList.Contains(item.Key))
            {
                UIPanelBase uiobject = UIManager.GetUIObject(item.Key);
                this.RmoveList.Add(uiobject);
            }
        });
        for (int i = 0; i < this.RmoveList.Count; i++)
        {
            UIPanelBase panel = this.RmoveList[i];
            this.DeleteUI(panel);
        }
        this.RmoveListForLua.Clear();
        UIManager.LuaPanelBaseMap.BetterForeach(delegate (KeyValuePair<string, LuaPanelBase> item)
        {
            if (uiList.Contains(item.Key))
            {
                this.RmoveListForLua.Add(item.Value);
            }
        });
        for (int j = 0; j < this.RmoveListForLua.Count; j++)
        {
            LuaPanelBase panel2 = this.RmoveListForLua[j];
            this.DeleteUI(panel2, true, true);
        }
        this.RmoveList.Clear();
    }

    public void DeleteAllUIWithOutList(List<string> uiList)
    {
        this.RmoveList.Clear();
        UIManager.UINameToTypeMap.BetterForeach(delegate (KeyValuePair<string, Type> item)
        {
            if (!uiList.Contains(item.Key))
            {
                UIPanelBase uiobject = UIManager.GetUIObject(item.Key);
                this.RmoveList.Add(uiobject);
            }
        });
        for (int i = 0; i < this.RmoveList.Count; i++)
        {
            UIPanelBase panel = this.RmoveList[i];
            this.DeleteUI(panel);
        }
        this.RmoveListForLua.Clear();
        UIManager.LuaPanelBaseMap.BetterForeach(delegate (KeyValuePair<string, LuaPanelBase> item)
        {
            if (!uiList.Contains(item.Key))
            {
                this.RmoveListForLua.Add(item.Value);
            }
        });
        for (int j = 0; j < this.RmoveListForLua.Count; j++)
        {
            LuaPanelBase panel2 = this.RmoveListForLua[j];
            this.DeleteUI(panel2, true, true);
        }
        this.RmoveList.Clear();
    }

    public void DramaHideUI(GameObject npcdlg)
    {
        if (npcdlg == null)
        {
            return;
        }
        npcdlg.transform.localPosition = new Vector3(0f, -1000f, 0f);
        this.ParentMap[UIManager.ParentType.Main].localPosition = new Vector3(0f, 1000f, 0f);
        this.ParentMap[UIManager.ParentType.CommonUI].localPosition = new Vector3(0f, 1000f, 0f);
        this.ParentMap[UIManager.ParentType.Tips].localPosition = new Vector3(0f, 1000f, 0f);
    }

    public void DramaActiviteUI(GameObject npcdlg)
    {
        if (npcdlg == null)
        {
            return;
        }
        npcdlg.transform.localPosition = Vector3.zero;
        this.ParentMap[UIManager.ParentType.Main].localPosition = Vector3.zero;
        this.ParentMap[UIManager.ParentType.CommonUI].localPosition = Vector3.zero;
        this.ParentMap[UIManager.ParentType.Tips].localPosition = Vector3.zero;
    }

    public void LoadUI(string uiname, string bundlename, Action<GameObject> complete)
    {
        UILoader.StartLoadDependcy("ui", bundlename + ".u", delegate (UIAssetBundle bundle)
        {
            if (bundle == null)
            {
                FFDebug.LogWarning(this, "Load ui bundle Error " + bundlename);
                return;
            }
            bundle.LoadAssetByName(uiname, delegate (UIAssetObj obj)
            {
                if (obj == null)
                {
                    complete(null);
                    return;
                }
                if (obj.Obj == null)
                {
                    FFDebug.LogWarning(this, "bundlename :" + bundlename);
                }
                GameObject gameObject = obj.Obj as GameObject;
                if (gameObject != null)
                {
                    complete(gameObject);
                }
                else
                {
                    complete(null);
                }
            });
        });
    }

    public void SetMaskImage(bool isShow)
    {
        this.img_mask.gameObject.SetActive(isShow);
        this.img_mask.enabled = isShow;
        this.img_mask.transform.SetAsLastSibling();
    }

    public void SetMaskImageAlpha(float alpha)
    {
        this.img_mask.color = new Color(this.img_mask.color.r, this.img_mask.color.g, this.img_mask.color.b, alpha);
    }

    public void RegisterUIName(string UIName, string AssetbundleName)
    {
        UGUINameContent.RegisterUINameAndAssetbundleName(UIName, AssetbundleName);
    }

    public void RegisterUIName(string UIName)
    {
        UGUINameContent.RegisterUINameAndAssetbundleName(UIName, UIName);
    }

    public string GetItemBgSpriteName(uint quality)
    {
        string result = "st0099";
        switch (quality)
        {
            case 1U:
                result = "st0099";
                break;
            case 2U:
                result = "st0100";
                break;
            case 3U:
                result = "st0101";
                break;
            case 4U:
                result = "st0102";
                break;
        }
        return result;
    }

    public string GetItemTipBigBgImg(uint quality)
    {
        string result = "tips_1";
        switch (quality)
        {
            case 1U:
                result = "tips_1";
                break;
            case 2U:
                result = "tips_2";
                break;
            case 3U:
                result = "tips_3";
                break;
        }
        return result;
    }

    public void ShowMainUI()
    {
        Transform transform;
        if (this.ParentMap.TryGetValue(UIManager.ParentType.Main, out transform))
        {
            transform.gameObject.SetActive(true);
        }
    }

    public string ManagerName
    {
        get
        {
            return base.GetType().Name;
        }
    }

    public void OnUpdate()
    {
    }

    public void OnReSet()
    {
        this.DeleteAllUI(false);
    }

    public static T FindInParents<T>(GameObject go) where T : Component
    {
        if (go == null)
        {
            return (T)((object)null);
        }
        T component = go.GetComponent<T>();
        if (component != null)
        {
            return component;
        }
        Transform parent = go.transform.parent;
        while (parent != null && component == null)
        {
            component = parent.gameObject.GetComponent<T>();
            parent = parent.parent;
        }
        return component;
    }

    public void ClearListChildrens(Transform grid, int startIndex = 1)
    {
        List<GameObject> list = new List<GameObject>();
        for (int i = 0; i < grid.childCount; i++)
        {
            if (i < startIndex)
            {
                grid.GetChild(i).gameObject.SetActive(false);
            }
            else
            {
                list.Add(grid.GetChild(i).gameObject);
            }
        }
        for (int j = 0; j < list.Count; j++)
        {
            UnityEngine.Object.Destroy(list[j]);
        }
    }

    public void SetRawImage(ImageType type, RawImage img, string name)
    {
        ManagerCenter.Instance.GetManager<UITextureMgr>().GetTexture(type, name, delegate (UITextureAsset asset)
        {
            if (img == null)
            {
                return;
            }
            Texture2D textureObj = asset.textureObj;
            img.texture = asset.textureObj;
            img.color = Color.white;
            img.gameObject.SetActive(true);
        });
    }

    public static Transform GetUITransform(string uiName)
    {
        LuaPanelBase luaUIPanel = UIManager.GetLuaUIPanel(uiName);
        if (luaUIPanel != null)
        {
            if (luaUIPanel.uiRoot == null)
            {
                return null;
            }
            return luaUIPanel.uiRoot.transform;
        }
        else
        {
            UIPanelBase uiobject = UIManager.GetUIObject(uiName);
            if (uiobject != null)
            {
                return uiobject.uiPanelRoot;
            }
            return null;
        }
    }

    public void GuideDeleteUI(string uinama)
    {
        this.guideDeleteUIName = uinama;
    }

    private static BetterDictionary<Type, UIPanelBase> UIPanelBaseMap = new BetterDictionary<Type, UIPanelBase>();

    private static BetterDictionary<string, Type> UINameToTypeMap = new BetterDictionary<string, Type>();

    private static BetterDictionary<string, LuaPanelBase> LuaPanelBaseMap = new BetterDictionary<string, LuaPanelBase>();

    private static LuaFunction lunfunaddopeningui = null;

    private static LuaFunction lunfunremoveopeningui = null;

    private BetterDictionary<string, UIPanelBase> dicOpeningUI = new BetterDictionary<string, UIPanelBase>();

    private List<string> listUIStack = new List<string>();

    private BetterDictionary<string, UITag> dicUIAndTagDic = new BetterDictionary<string, UITag>();

    private string tagSecondUI;

    private string tagThirdUI;

    private List<string> m_LoadingUIAssetbundle = new List<string>();

    private Transform uiRoot;

    private Transform layerMask;

    private Image img_mask;

    private UIManager.OnMaskFinished onMaskFinished;

    private string[] luaNotDelUINames = new string[]
    {
        "UI_Bag",
        "UI_TaskList"
    };

    private Dictionary<UIManager.ParentType, Transform> ParentMap = new Dictionary<UIManager.ParentType, Transform>();

    private int UIStackCount = 5;

    private string[] strSpecialUI = new string[]
    {
        "UI_Alert",
        "UI_NPCtalkAndTaskDlg"
    };

    private List<string> uiDeling = new List<string>();

    private List<UIPanelBase> RmoveList = new List<UIPanelBase>();

    private List<LuaPanelBase> RmoveListForLua = new List<LuaPanelBase>();

    private List<string> uiNamesOpenByNpc = new List<string>();

    private string guideDeleteUIName;

    public enum ParentType
    {
        UIRoot,
        Map,
        Main,
        CommonUI,
        Loading,
        HPRoot,
        Tips,
        UICamera,
        Guide
    }

    public delegate void OnMaskFinished();
}
