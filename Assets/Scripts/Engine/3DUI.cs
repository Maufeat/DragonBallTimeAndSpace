using System;
using UnityEngine;
using UnityEngine.UI;

public class UI3D : MonoBehaviour
{
    public void SetRenderRect(int w, int h)
    {
        this.OnDisable();
        if (this.target == null)
        {
            this.target = new RenderTexture(w, h, 0);
        }
        else if (this.target.width != w || this.target.height != h)
        {
            UnityEngine.Object.DestroyImmediate(this.target);
            this.target = new RenderTexture(w, h, 0);
        }
        if (this.image == null)
        {
            this.image = base.transform.GetComponent<RawImage>();
        }
        //this.image.texture = this.target;
        if (this._targetCamera == null)
        {
            this.InitCamera();
        }
        base.transform.parent.gameObject.SetActive(true);
    }

    private void InitCamera()
    {
        if (this.target == null)
        {
            return;
        }
        this._targetCamera = Camera.main;
        if (this._targetCamera != null)
        {
            this.target3D = this._targetCamera.GetComponent<UI3DTarget>();
            if (this.target3D == null)
            {
                this.target3D = this._targetCamera.gameObject.AddComponent<UI3DTarget>();
            }
            this.target3D.target = this.target;
            this._targetCamera.pixelRect = new Rect(0f, 0f, (float)this.target.width, (float)this.target.height);
        }
    }

    private void OnDisable()
    {
        if (this.target3D != null && this.target3D.target != null)
        {
            this.target3D.target = null;
        }
        if (this._targetCamera != null)
        {
            this.originalPixelRect.width = (float)Screen.currentResolution.width;
            this.originalPixelRect.height = (float)Screen.currentResolution.height;
            this._targetCamera.pixelRect = this.originalPixelRect;
            this._targetCamera = null;
        }
    }

    private void Update()
    {
        if (this.image == null)
        {
            return;
        }
        if (this._targetCamera != null)
        {
            return;
        }
        this.InitCamera();
    }

    private RenderTexture target;

    private Camera _targetCamera;

    private UI3DTarget target3D;

    public RawImage image;

    private Rect originalPixelRect = new Rect(0f, 0f, 800f, 600f);
}
