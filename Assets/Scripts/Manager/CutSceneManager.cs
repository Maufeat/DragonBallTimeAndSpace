using System;
using System.Collections.Generic;
using Framework.Base;
using Framework.Managers;
using Game.Scene;
using ResoureManager;
using Subtitle;
using UnityEngine;

public class CutSceneManager : IManager
{
    private CutSceneAssetManager mCharacterModelMgr
    {
        get
        {
            return ManagerCenter.Instance.GetManager<CutSceneAssetManager>();
        }
    }

    public void LoadSubtitleProto()
    {
        this.DicCutSceneSubtitle.Clear();
        try
        {
            TotalCutSceneSubTitle totalCutSceneSubTitle = ManagerCenter.Instance.GetManager<ResourceManager>().LoadProtoBuffData<TotalCutSceneSubTitle>("subtitle/subtitle.bytes");
            if (totalCutSceneSubTitle != null)
            {
                for (int i = 0; i < totalCutSceneSubTitle.subtitlelist.Count; i++)
                {
                    CutSceneSubTitle cutSceneSubTitle = totalCutSceneSubTitle.subtitlelist[i];
                    List<SubtitleContent> list = new List<SubtitleContent>();
                    for (int j = 0; j < cutSceneSubTitle.subtitlelist.Count; j++)
                    {
                        SubtitleContent item = cutSceneSubTitle.subtitlelist[j];
                        list.Add(item);
                    }
                    list.Sort(new SubtitleContentList());
                    this.DicCutSceneSubtitle[cutSceneSubTitle.key.ToLower()] = list;
                }
            }
        }
        catch (Exception ex)
        {
            FFDebug.LogWarning(this, ex.Message);
        }
    }

    public void GetCutSceneContent(string cutscenekey, Action<CutSceneContent> Callback)
    {
        if (!string.IsNullOrEmpty(this.CurCutscene))
        {
            return;
        }
        this.CurCutscene = cutscenekey;
        if (this.DicCutSceneContent.ContainsKey(cutscenekey))
        {
            return;
        }
        this.SetmainCameraFox(false);
        this.OnLoadingCutScene = true;
        this.mCharacterModelMgr.LoadFromAB(cutscenekey, delegate (CutSceneContent cutscene)
        {
            if (cutscene == null)
            {
                this.SetMainCamera(true);
                Callback(null);
                this.OnLoadingCutScene = false;
            }
            else
            {
                this.GetCurCutSceneSubtitle(cutscenekey);
                cutscene.OnCutSceneUpdate = new Action<float>(this.CutSceneUpdate);
                if (cutscene.CameraAnimator != null)
                {
                    DragonBallObjLOD.ResetTarget(cutscene.CameraAnimator.transform);
                }
                if (cutscene != null)
                {
                    cutscene.LoadAssets = delegate ()
                    {
                        this.ContentLoadAssets(cutscene);
                    };
                    cutscene.OnLoadAsstesFinish = delegate ()
                    {
                        this.SetMainCamera(false);
                        this.SetUICamera(false);
                        this.OnLoadingCutScene = false;
                    };
                    cutscene.UnLoadAssets = delegate ()
                    {
                        this.CurCutscene = string.Empty;
                        this.RemoveCutScene(cutscene.Key);
                    };
                }
                this.DicCutSceneContent[cutscenekey] = cutscene;
                this.DicCutSceneContent[cutscenekey].Key = cutscenekey;
                Callback(this.DicCutSceneContent[cutscenekey]);
            }
        });
    }

    public void ContentLoadAssets(CutSceneContent content)
    {
        int loadCount = 0;
        content.Listanimator.Clear();
        this.CutSceneCharacterObject.Clear();
        if (content.ListModelsNames.Count == 0)
        {
            content.LoadAssetsFinish();
        }
        for (int i = 0; i < content.ListModelsNames.Count; i++)
        {
            this.LoadCharacter(content.ListModelsNames[i], content.ListControllerNames[i], delegate (GameObject obj)
            {
                if (ManagerCenter.Instance.GetManager<GameScene>().LightInfo != null)
                {
                    ManagerCenter.Instance.GetManager<GameScene>().SetMatLightInfo(obj, false);
                }
                obj.SetActive(false);
                Animator component = obj.GetComponent<Animator>();
                if (component != null)
                {
                    content.Listanimator.Add(component);
                }
                loadCount++;
                if (loadCount == content.ListModelsNames.Count)
                {
                    this.ShowCharactors();
                    content.LoadAssetsFinish();
                }
            }, content);
        }
    }

    public CutSceneContent GetCurSceneContent()
    {
        if (!this.DicCutSceneContent.ContainsKey(this.CurCutscene))
        {
            return null;
        }
        return this.DicCutSceneContent[this.CurCutscene];
    }

    public Transform GetEffectParentByIndex(int index)
    {
        if (index < 0)
        {
            return null;
        }
        if (!this.DicCutSceneContent.ContainsKey(this.CurCutscene))
        {
            return null;
        }
        return this.DicCutSceneContent[this.CurCutscene].GetObjectByIndex(index);
    }

    public void ShowCharactors()
    {
        for (int i = 0; i < this.CutSceneCharacterObject.Count; i++)
        {
            this.CutSceneCharacterObject[i].rootObj.SetActive(true);
        }
    }

    public void LoadCharacter(string modelName, string CtrlName, Action<GameObject> CallBack, CutSceneContent content)
    {
        PlayerCharactorCreateHelper.LoadBody(string.Empty, CharactorAndEffectBundleType.Charactor, CtrlName, CtrlName, modelName, string.Empty, string.Empty, string.Empty, string.Empty, delegate (PlayerCharactorCreateHelper o)
        {
            if (o != null && o.rootObj != null && CallBack != null)
            {
                o.rootObj.transform.localPosition = Vector3.zero;
                o.rootObj.SetLayer(Const.Layer.Cutscene, true);
                Animator component = o.rootObj.GetComponent<Animator>();
                if (null != component)
                {
                    component.cullingMode = AnimatorCullingMode.AlwaysAnimate;
                }
                this.CutSceneCharacterObject.Add(o);
                CallBack(o.rootObj);
            }
        }, null, false, 0UL);
    }

    public Material getMaterialByName(string name)
    {
        for (int i = 0; i < this.CutSceneCharacterObject.Count; i++)
        {
            if (name.CompareTo(this.CutSceneCharacterObject[i].bodyName) == 0)
            {
                SkinnedMeshRenderer componentInChildren = this.CutSceneCharacterObject[i].rootObj.GetComponentInChildren<SkinnedMeshRenderer>(true);
                if (componentInChildren != null)
                {
                    return componentInChildren.sharedMaterial;
                }
            }
        }
        return null;
    }

    public void Dispose(CutSceneContent content)
    {
        for (int i = 0; i < this.CutSceneCharacterObject.Count; i++)
        {
            this.CutSceneCharacterObject[i].DestroyBonePObj();
        }
        this.CutSceneCharacterObject.Clear();
    }

    public bool InPlayCutScene()
    {
        return !string.IsNullOrEmpty(this.CurCutscene);
    }

    public bool IsOnLoadingCutScene()
    {
        return this.OnLoadingCutScene;
    }

    public void Stop()
    {
        if (this.OnLoadingCutScene)
        {
            return;
        }
        if (string.IsNullOrEmpty(this.CurCutscene))
        {
            return;
        }
        if (!this.DicCutSceneContent.ContainsKey(this.CurCutscene))
        {
            if (null != AVPlayOP.Instance)
            {
                AVPlayOP.Instance.StopPlay();
            }
            return;
        }
        this.DicCutSceneContent[this.CurCutscene].End();
    }

    public void ActiveCurrBlomm(bool active)
    {
        if (string.IsNullOrEmpty(this.CurCutscene))
        {
            return;
        }
        if (!this.DicCutSceneContent.ContainsKey(this.CurCutscene))
        {
            return;
        }
        this.DicCutSceneContent[this.CurCutscene].BloomEnabled = active;
    }

    public void SetUICamera(bool bactive)
    {
        Camera component = UIManager.Instance.GetUICamera().GetComponent<Camera>();
        if (bactive)
        {
            component.cullingMask = 1 << LayerMask.NameToLayer("UI");
        }
        else
        {
            component.cullingMask = 1 << LayerMask.NameToLayer("CutSceneUI");
        }
        UIManager.Instance.SetUIVisible(UIManager.ParentType.HPRoot, bactive);
    }

    public void SetmainCameraFox(bool bactive)
    {
        Camera camera = null;
        GameObject gameObject = GameObject.FindWithTag("MainCamera");
        if (gameObject)
        {
            camera = gameObject.GetComponent<Camera>();
        }
        if (camera == null)
        {
            return;
        }
        FxPro componentInChildren = camera.GetComponentInChildren<FxPro>(true);
        if (componentInChildren != null)
        {
            componentInChildren.enabled = bactive;
        }
    }

    public void SetMainCamera(bool bactive)
    {
        if (bactive)
        {
            this.SetUICamera(true);
        }
        Camera camera = null;
        GameObject gameObject = GameObject.FindWithTag("MainCamera");
        if (gameObject)
        {
            camera = gameObject.GetComponent<Camera>();
        }
        if (camera == null)
        {
            return;
        }
        FxPro componentInChildren = camera.GetComponentInChildren<FxPro>(true);
        if (componentInChildren != null)
        {
            componentInChildren.enabled = bactive;
        }
        camera.enabled = bactive;
    }

    public void RemoveCutScene(string cutscenekey)
    {
        for (int i = 0; i < this.CutSceneCharacterObject.Count; i++)
        {
            if (ManagerCenter.Instance.GetManager<GameScene>().LightInfo != null)
            {
                ManagerCenter.Instance.GetManager<GameScene>().SetMatLightInfo(this.CutSceneCharacterObject[i].rootObj, false);
            }
        }
        if (this.DicCutSceneContent.ContainsKey(cutscenekey))
        {
            CutSceneContent cutSceneContent = this.DicCutSceneContent[cutscenekey];
            cutSceneContent.gameObject.SetActive(false);
            this.DicCutSceneContent.Remove(cutscenekey);
            this.Dispose(cutSceneContent);
            UnityEngine.Object.Destroy(cutSceneContent.transform.gameObject);
        }
        this.mCharacterModelMgr.RemoveCutScene(cutscenekey);
        this.SetMainCamera(true);
    }

    public void UpdateCutSceneRoleRenderInfo(Color color, Vector4 dir, float intensity)
    {
        for (int i = 0; i < this.CutSceneCharacterObject.Count; i++)
        {
            Renderer[] componentsInChildren = this.CutSceneCharacterObject[i].rootObj.GetComponentsInChildren<Renderer>();
            for (int j = 0; j < componentsInChildren.Length; j++)
            {
                if (componentsInChildren[j].sharedMaterial != null)
                {
                    if (componentsInChildren[j].sharedMaterial.HasProperty("_LightColor"))
                    {
                        componentsInChildren[j].sharedMaterial.SetColor("_LightColor", color);
                    }
                    if (componentsInChildren[j].sharedMaterial.HasProperty("_LightDir"))
                    {
                        componentsInChildren[j].sharedMaterial.SetVector("_LightDir", dir);
                    }
                    if (componentsInChildren[j].sharedMaterial.HasProperty("_LightIntensity"))
                    {
                        componentsInChildren[j].sharedMaterial.SetFloat("_LightIntensity", intensity);
                    }
                }
            }
        }
    }

    public string ManagerName
    {
        get
        {
            return base.GetType().ToString();
        }
    }

    private void CutSceneUpdate(float runtime)
    {
        if (null == this.uicutscenepass)
        {
            this.uicutscenepass = UI_CutScenePass.GetView();
        }
        if (this.curSubtitleContent == null || this.uicutscenepass == null)
        {
            return;
        }
        if (runtime >= this.curSubtitleContent.showTime)
        {
            if (this.uicutscenepass != null)
            {
                this.uicutscenepass.SetSubtitle(this.curSubtitleContent.subtitle, this.curSubtitleContent.duration);
            }
            this.GetCurSubtitleContent();
        }
    }

    private void GetCurCutSceneSubtitle(string key)
    {
        this.curSubtitleList = null;
        this.curSubtitleContent = null;
        this.CurSubtitleIndex = 0;
        if (this.DicCutSceneSubtitle.ContainsKey(key))
        {
            this.curSubtitleList = this.DicCutSceneSubtitle[key];
            this.GetCurSubtitleContent();
        }
    }

    private void GetCurSubtitleContent()
    {
        if (this.curSubtitleList.Count > this.CurSubtitleIndex)
        {
            this.curSubtitleContent = this.curSubtitleList[this.CurSubtitleIndex];
            this.CurSubtitleIndex++;
        }
        else
        {
            this.curSubtitleContent = null;
        }
    }

    public void OnUpdate()
    {
    }

    public void OnReSet()
    {
    }

    private UI_CutScenePass uicutscenepass;

    private List<SubtitleContent> curSubtitleList;

    private SubtitleContent curSubtitleContent;

    private int CurSubtitleIndex;

    private BetterDictionary<string, List<SubtitleContent>> DicCutSceneSubtitle = new BetterDictionary<string, List<SubtitleContent>>();

    private BetterDictionary<string, CutSceneContent> DicCutSceneContent = new BetterDictionary<string, CutSceneContent>();

    public string CurCutscene = string.Empty;

    private bool OnLoadingCutScene;

    public List<PlayerCharactorCreateHelper> CutSceneCharacterObject = new List<PlayerCharactorCreateHelper>();
}
