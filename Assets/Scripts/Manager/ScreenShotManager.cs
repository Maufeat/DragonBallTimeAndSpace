using System;
using Framework.Base;
using UnityEngine;
using UnityEngine.UI;

public class ScreenShotManager : IManager
{
    public Material Blurmat
    {
        get
        {
            if (this._blurmat == null)
            {
                this._blurmat = new Material(Shader.Find("CustomUI/Bloom"));
                this._blurmat.SetFloat("_Distance", 0.0025f);
            }
            return this._blurmat;
        }
    }

    public void Init()
    {
        this.tempCamera = GameObject.Find("ScreenShotTempCamera").GetComponent<Camera>();
        this.tempCamera.gameObject.SetActive(false);
    }

    public string ManagerName
    {
        get
        {
            return base.GetType().ToString();
        }
    }

    public void OnUpdate()
    {
    }

    public void OnReSet()
    {
    }

    private Texture GetScreenShot()
    {
        if (this.tempCamera == null)
        {
            return null;
        }
        if (this.tempRt == null)
        {
            this.tempRt = new RenderTexture((int)((float)Screen.width * this.ScreenShotSize), (int)((float)Screen.height * this.ScreenShotSize), 16, RenderTextureFormat.ARGB32);
            this.tempRt.anisoLevel = 0;
            this.tempRt.name = "BackGroundScreenShot";
        }
        this.tempCamera.gameObject.SetActive(true);
        this.tempCamera.CopyFrom(Camera.main);
        this.tempCamera.targetTexture = this.tempRt;
        this.tempCamera.Render();
        this.tempCamera.gameObject.SetActive(false);
        return this.tempRt;
    }

    public void FillRawImageWithScreenShotBlur(Transform tran)
    {
        if (tran == null)
        {
            return;
        }
        RawImage component = tran.GetComponent<RawImage>();
        this.FillRawImageWithScreenShotBlur(component);
    }

    public void FillRawImageWithScreenShotBlur(RawImage rimg)
    {
        if (rimg != null)
        {
            rimg.texture = this.GetScreenShot();
            rimg.material = this.Blurmat;
        }
    }

    public void FillImageWithScreenShotBlur(Image img)
    {
    }

    private Camera tempCamera;

    public float ScreenShotSize = 0.5f;

    private Material _blurmat;

    private RenderTexture tempRt;
}
