using UnityEngine;

public class UITextureAsset
{
    private UIAssetObj assetObj;
    private int usecount;

    public UITextureAsset(UIAssetObj obj)
    {
        this.assetObj = obj;
        this.usecount = 0;
    }

    public Texture2D textureObj
    {
        get
        {
            if (this.assetObj == null)
                return (Texture2D)null;
            if (this.assetObj.Obj == (Object)null)
                return (Texture2D)null;
            return !(this.assetObj.Obj is Texture2D) ? (Texture2D)null : this.assetObj.Obj as Texture2D;
        }
    }

    public void AddUse()
    {
        if (this.assetObj == null)
            this.usecount = 0;
        else
            ++this.usecount;
    }

    public void TryUnload()
    {
        if (this.assetObj == null)
        {
            this.usecount = 0;
        }
        else
        {
            --this.usecount;
            if (this.usecount < 0)
            {
                FFDebug.LogWarning((object)this, (object)("DeductUse UITextureAsset " + this.assetObj.ObjName + " Exception"));
                this.usecount = 0;
            }
            if (this.usecount != 0)
                return;
            UITextureMgr.MapTextureAssets.Remove(this.assetObj.ObjName.ToLower());
            this.assetObj.UnLoadThis(true);
            this.assetObj = (UIAssetObj)null;
        }
    }
}
