using System;
using System.Collections.Generic;
using Framework.Base;
using Framework.Managers;
using UnityEngine;
using UnityEngine.UI;

public class RawCharactorMgr : IManager
{
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
    }

    public void Init()
    {
    }

    public RawCharactorMgr GetRawCharacterMgr(ModelUI modelUI)
    {
        if (!this.rawCharacterMgrDic.ContainsKey(modelUI))
        {
            RawCharactorMgr rawCharactorMgr = new RawCharactorMgr();
            rawCharactorMgr.Init(modelUI, this.rawCharacterMgrDic.Count);
            this.rawCharacterMgrDic.Add(modelUI, rawCharactorMgr);
        }
        return this.rawCharacterMgrDic[modelUI];
    }

    private void Init(ModelUI modelUI, int index)
    {
        this.strRTName = modelUI.ToString();
        this.cameraPos = new Vector3(0f, 1001.5f + (float)(index * 100), 0f);
        this.rolePos = new Vector3(0f, (float)(1000 + index * 100), 0f);
    }

    public void DisposeCharactor(string disposInUI)
    {
        RenderTextureMgr manager = ManagerCenter.Instance.GetManager<RenderTextureMgr>();
        if (this.UINeedRaw.Contains(disposInUI))
        {
            this.UINeedRaw.Remove(disposInUI);
            this.dicRawImg.Remove(disposInUI);
            if (this.UINeedRaw.Count > 0 && manager != null)
            {
                GameObject value = manager.UseRTGameObject(this.strRTName);
                Camera camera = value.transform.Find("cam").GetComponent<Camera>();
                RawImage rawImage = this.dicRawImg[this.UINeedRaw[this.UINeedRaw.Count - 1]];
                if (rawImage == null)
                {
                    FFDebug.LogWarning(null, "     rw_role  ==null ");
                }
                camera.targetTexture = (rawImage.texture as RenderTexture);
            }
        }
        if (this.UINeedRaw.Count > 0)
        {
            return;
        }
        if (manager != null)
        {
            manager.FreeRTGameObject(this.strRTName);
        }
        if (this.RTRawActor != null)
        {
            this.RTRawActor.DestroyThisInNineScreen();
            this.RTRawActor.TrueDestory();
            this.RTRawActor = null;
        }
    }

    public void ChangeModel(string modelname)
    {
        if (this.RTRawActor != null)
        {
            this.RTRawActor.ChangeModel(modelname, delegate
            {
            });
        }
    }

    public void ChangeWeapon(uint modelid)
    {
        if (this.RTRawActor != null)
        {
            this.RTRawActor.ChangeWeapon(modelid);
        }
    }

    public void GetRawCharactor(RawImage rw_role, uint npcid, string uiname, int angle = 300)
    {
        if (!this.UINeedRaw.Contains(uiname))
        {
            this.UINeedRaw.Add(uiname);
            this.dicRawImg[uiname] = rw_role;
            RenderTextureMgr manager = ManagerCenter.Instance.GetManager<RenderTextureMgr>();
            if (manager == null)
            {
                return;
            }
            GameObject gameObject = manager.UseRTGameObject(this.strRTName);
            if (gameObject == null)
            {
                return;
            }
            Camera camera = gameObject.transform.Find("cam").GetComponent<Camera>();
            if (rw_role == null)
            {
                FFDebug.LogWarning(null, "     rw_role  ==null ");
            }
            RenderTexture renderTexture = rw_role.texture as RenderTexture;
            renderTexture.antiAliasing = 8;
            camera.targetTexture = renderTexture;
            Vector3 v3Pos = this.cameraPos;
            manager.ResetRootPosition(this.strRTName, v3Pos);
        }
        if (this.RTRawActor == null)
        {
            this.InitRtRole(this.strRTName, npcid, angle);
        }
        else
        {
            this.RTRawActor.RefreshModel(this.strRTName, npcid, Const.Layer.RT, this.rolePos, new Vector3(0f, (float)angle, 0f));
        }
    }

    private void InitRtRole(string strRTName, uint npcid, int angle)
    {
        this.RTRawActor = new RawCharactor();
        this.RTRawActor.CreateModel(strRTName, npcid, Const.Layer.RT, this.rolePos, new Vector3(0f, (float)angle, 0f));
    }

    public GameObject GetModelObj()
    {
        if (this.RTRawActor == null)
        {
            return null;
        }
        return this.RTRawActor.ModelObj;
    }

    public void DragRTView(Vector2 delta)
    {
        if (this.RTRawActor == null)
        {
            return;
        }
        this.RTRawActor.ModelObj.transform.eulerAngles += new Vector3(0f, -delta.x, 0f);
    }

    private Dictionary<ModelUI, RawCharactorMgr> rawCharacterMgrDic = new Dictionary<ModelUI, RawCharactorMgr>();

    private RawCharactor RTRawActor;

    private string strRTName = "HeroHandbook";

    private Vector3 cameraPos = new Vector3(0f, 1001.5f, 0f);

    private Vector3 rolePos = new Vector3(0f, 1000f, 0f);

    public List<string> UINeedRaw = new List<string>();

    private Dictionary<string, RawImage> dicRawImg = new Dictionary<string, RawImage>();
}
