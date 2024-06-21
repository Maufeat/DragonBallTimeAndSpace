using Framework.Base;
using Framework.Managers;
using LuaInterface;
using System;
using UnityEngine;
using UnityEngine.UI;

public class UITextureMgr : IManager
{
    public static BetterDictionary<string, UITextureAsset> MapTextureAssets = new BetterDictionary<string, UITextureAsset>();
    private readonly string _strGreyShaderPath = "UI/Default Grey";
    private readonly string _strNormalShaderPath = "Unlit/RGBplusA";
    private Material _matGrey;
    private Material _matHeadIconMask;

    public static UITextureMgr Instance
    {
        get
        {
            return ManagerCenter.Instance.GetManager<UITextureMgr>();
        }
    }

    public string ManagerName
    {
        get
        {
            return this.GetType().Name;
        }
    }

    public void GetTexture(ImageType type, string imgname, Action<UITextureAsset> callback)
    {
        if (string.IsNullOrEmpty(imgname))
        {
            callback((UITextureAsset)null);
        }
        else
        {
            string Imgname = imgname.ToLower();
            if (UITextureMgr.MapTextureAssets.ContainsKey(Imgname))
            {
                UITextureMgr.MapTextureAssets[Imgname].AddUse();
                callback(UITextureMgr.MapTextureAssets[Imgname]);
            }
            else
            {
                string str = string.Empty;
                switch (type)
                {
                    case ImageType.ITEM:
                        str = "items";
                        break;
                    case ImageType.ICON:
                        str = "icons";
                        break;
                    case ImageType.OTHERS:
                        str = "others";
                        break;
                    case ImageType.ROLES:
                        str = "role";
                        break;
                    case ImageType.RANK:
                        str = "rank";
                        break;
                    case ImageType.CHARACTER:
                        str = "character";
                        break;
                    case ImageType.STARTUP:
                        str = "startup";
                        break;
                }
                UILoader.LoadObject("ui/image/" + str, Imgname + ".u", Imgname, (Action<UIAssetObj>)(obj =>
                {
                    if (UITextureMgr.MapTextureAssets.ContainsKey(Imgname))
                    {
                        UITextureMgr.MapTextureAssets[Imgname].AddUse();
                        callback(UITextureMgr.MapTextureAssets[Imgname]);
                    }
                    else if (obj != null && obj.Obj is Texture2D)
                    {
                        UITextureAsset uiTextureAsset = new UITextureAsset(obj);
                        UITextureMgr.MapTextureAssets[Imgname] = uiTextureAsset;
                        UITextureMgr.MapTextureAssets[Imgname].AddUse();
                        callback(UITextureMgr.MapTextureAssets[Imgname]);
                    }
                    else
                    {
                        FFDebug.LogWarning((object)this, (object)("Load texture: " + Imgname + " Failed"));
                        callback((UITextureAsset)null);
                    }
                }));
            }
        }
    }

    public void GetTexture(int type, string imgname, LuaFunction callback)
    {
        this.GetTexture((ImageType)type, imgname, (Action<UITextureAsset>)(asset =>
        {
            if (asset == null)
                return;
            callback.Call((object)asset);
        }));
    }

    public void GetModelSkinnedAssets(
      string charactorname,
      string skinnedname,
      Action<FFAssetBundle> callback)
    {
        if (string.IsNullOrEmpty(skinnedname))
            return;
        string lower = skinnedname.ToLower();
        string str1 = "SkinnedTexute/characters/characterstexture/" + charactorname + "/" + (skinnedname + ".u");
        string str2 = "characters/characterstexture/" + charactorname + "/";
        FFAssetBundleRequest.CleverRequest(CharactorAndEffectBundleType.CharactorTexture, lower, (ab => { }));
    }

    public void GetSpriteFromAtlas(string atlasname, string spritename, Action<Sprite> callback)
    {
        UILoader.LoadObject("ui", "atlas/" + atlasname.ToLower() + ".u", atlasname + "Atlas", (obj =>
        {
            if (obj == (UnityEngine.Object)null)
            {
                FFDebug.LogWarning((object)this, (object)("Load Sprite " + spritename + " From Atlas " + atlasname + "data" + " Error"));
                callback((Sprite)null);
            }
            else
            {
                AtlasData atlasData = obj as AtlasData;
                if ((UnityEngine.Object)atlasData == (UnityEngine.Object)null)
                {
                    FFDebug.LogWarning((object)this, (object)"atlasData == null");
                    callback((Sprite)null);
                }
                else
                {
                    Sprite sprite = atlasData.GetSprite(spritename);
                    if ((UnityEngine.Object)sprite != (UnityEngine.Object)null)
                    {
                        callback(sprite);
                    }
                    else
                    {
                        FFDebug.Log((object)this, (object)FFLogType.UI, (object)("Does not contain sprite " + spritename + " in atlas " + atlasname));
                        callback((Sprite)null);
                    }
                }
            }
        }));
    }

    public void LoadAtlas(string atlasname, Action<Sprite[]> callback)
    {
        UILoader.LoadAssetBundle("ui", "atlas/" + atlasname.ToLower() + ".u", (Action<UIAssetBundle>)(bundle =>
        {
            if (bundle != null)
            {
                callback(bundle.LoadAllAsset<Sprite>());
            }
            else
            {
                FFDebug.LogWarning((object)this, (object)("Atlas  " + atlasname + " is null!!"));
                callback((Sprite[])null);
            }
        }));
    }

    public void SetImageGrey(Image img, bool isGrey)
    {
        if (isGrey)
        {
            if ((UnityEngine.Object)this._matGrey == (UnityEngine.Object)null)
                this._matGrey = new Material(Shader.Find(this._strGreyShaderPath));
            img.material = this._matGrey;
        }
        else
            img.material = (Material)null;
    }

    public void SetImageGrey(RawImage img, bool isGrey)
    {
        if (isGrey)
        {
            if ((UnityEngine.Object)this._matGrey == (UnityEngine.Object)null)
                this._matGrey = new Material(Shader.Find(this._strGreyShaderPath));
            img.material = this._matGrey;
        }
        else
            img.material = (Material)null;
    }

    public void SetImageGrey4Head(RawImage img, bool isGrey)
    {
        if ((UnityEngine.Object)this._matHeadIconMask == (UnityEngine.Object)null && (UnityEngine.Object)img.material != (UnityEngine.Object)null)
            this._matHeadIconMask = img.material;
        if (isGrey)
        {
            if ((UnityEngine.Object)this._matGrey == (UnityEngine.Object)null)
                this._matGrey = new Material(Shader.Find(this._strGreyShaderPath));
            img.material = this._matGrey;
        }
        else
            img.material = this._matHeadIconMask;
    }

    public void OnUpdate()
    {
    }

    public void OnReSet()
    {
    }
}
