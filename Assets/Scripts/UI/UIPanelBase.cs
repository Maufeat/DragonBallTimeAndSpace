using Framework.Managers;
using System;
using System.Collections.Generic;
using UnityEngine;

public class UIPanelBase
{
    public List<UITextureAsset> usedTextureAssets = new List<UITextureAsset>();
    private string bankName = "Ui";
    private UIManager _uiMgr;

    public UIManager uiMgr
    {
        get
        {
            if (this._uiMgr == null)
                this._uiMgr = ManagerCenter.Instance.GetManager<UIManager>();
            return this._uiMgr;
        }
    }

    public bool byLua { get; private set; }

    public string uiName { get; private set; }

    public Transform uiPanelRoot { get; private set; }

    public System.Type uiClassType { get; private set; }

    public virtual void OnInit(Transform root)
    {
        this.uiPanelRoot = root;
        this.usedTextureAssets.Clear();
    }

    public virtual void AfterInit()
    {
    }

    public virtual void OnDispose()
    {
    }

    public void Init(Transform root, string uianme, bool bylua = false)
    {
        this.uiName = uianme;
        this.uiClassType = this.GetType();
        this.uiPanelRoot = root;
        this.byLua = bylua;
    }

    public void UnRegUIByName(string uiName)
    {
    }

    public void RegOpenUIByNpc(string uiNameOther = "")
    {
        if (this.uiMgr == null)
            return;
        if (!string.IsNullOrEmpty(uiNameOther))
            this.uiMgr.RegUINameOpenByNpc(uiNameOther);
        else
            this.uiMgr.RegUINameOpenByNpc(this.uiName);
    }

    public void UnRegOpenUIByNpc(string uiNameOther = "")
    {
        if (!string.IsNullOrEmpty(uiNameOther))
            this.uiMgr.UnRegUINameOpenByNpc(uiNameOther);
        else
            this.uiMgr.UnRegUINameOpenByNpc(this.uiName);
    }

    public void Dispose()
    {
        for (int index = 0; index < this.usedTextureAssets.Count; ++index)
            this.usedTextureAssets[index].TryUnload();
        this.usedTextureAssets.Clear();
        if (!((UnityEngine.Object)this.uiPanelRoot != (UnityEngine.Object)null))
            return;
        UnityEngine.Object.DestroyImmediate((UnityEngine.Object)this.uiPanelRoot.gameObject);
    }

    public void SetActive(bool bactive)
    {
        if (!((UnityEngine.Object)this.uiPanelRoot != (UnityEngine.Object)null))
            return;
        if (bactive)
            this.uiPanelRoot.localPosition = Vector3.zero;
        else
            this.uiPanelRoot.localPosition = new Vector3(0.0f, 5000f, 0.0f);
    }

    public void GetTexture(ImageType type, string imgname, Action<Texture2D> callback)
    {
        ManagerCenter.Instance.GetManager<UITextureMgr>().GetTexture(type, imgname, (Action<UITextureAsset>)(_asset =>
        {
            if (_asset == null)
            {
                callback((Texture2D)null);
            }
            else
            {
                this.usedTextureAssets.Add(_asset);
                if ((UnityEngine.Object)_asset.textureObj == (UnityEngine.Object)null)
                    callback((Texture2D)null);
                else
                    callback(_asset.textureObj);
            }
        }));
    }

    public void GetSprite(ImageType type, string imgname, Action<Sprite> callback)
    {
        this.GetTexture(type, imgname, (Action<Texture2D>)(texture =>
        {
            if ((UnityEngine.Object)texture != (UnityEngine.Object)null)
                callback(Sprite.Create(texture, new Rect(0.0f, 0.0f, (float)texture.width, (float)texture.height), new Vector2(0.0f, 0.0f)));
            else
                callback((Sprite)null);
        }));
    }

    public static bool operator ==(UIPanelBase x, UIPanelBase y)
    {
        if (object.ReferenceEquals((object)x, (object)null))
            return !(bool)y;
        return object.ReferenceEquals((object)y, (object)null) ? !(bool)x : object.ReferenceEquals((object)x, (object)y);
    }

    public static bool operator !=(UIPanelBase x, UIPanelBase y)
    {
        if (object.ReferenceEquals((object)x, (object)null))
            return (bool)y;
        return object.ReferenceEquals((object)y, (object)null) ? (bool)x : !object.ReferenceEquals((object)x, (object)y);
    }

    public static implicit operator bool(UIPanelBase exists)
    {
        return !object.ReferenceEquals((object)exists, (object)null) && UIManager.IsUIPanelExists(exists.uiClassType);
    }
}
