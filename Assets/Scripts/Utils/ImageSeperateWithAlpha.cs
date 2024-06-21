using System;
using System.Collections.Generic;
using Framework.Managers;
using UnityEngine;
using UnityEngine.UI;

public class ImageSeperateWithAlpha
{
    public ImageSeperateWithAlpha()
    {
        this.textureassets.Clear();
    }

    public void ProcessRawImageSeperateRGBA(RawImage rimage, string texturename, Action callback = null)
    {
        Texture2D texturergb = null;
        Texture2D texturea = null;
        if (ImageSeperateWithAlpha.RGBPA == null)
        {
            ImageSeperateWithAlpha.RGBPA = Shader.Find("Unlit/RGBplusA");
        }
        ManagerCenter.Instance.GetManager<UITextureMgr>().GetTexture(ImageType.ROLES, texturename, delegate (UITextureAsset asset1)
        {
            if (asset1 == null)
            {
                if (callback != null)
                {
                    callback();
                }
                return;
            }
            this.textureassets.Add(asset1);
            texturergb = asset1.textureObj;
            if (texturergb == null)
            {
                if (callback != null)
                {
                    callback();
                }
                return;
            }
            texturergb.wrapMode = TextureWrapMode.Clamp;
            ManagerCenter.Instance.GetManager<UITextureMgr>().GetTexture(ImageType.ROLES, texturename + "_Alpha", delegate (UITextureAsset asset2)
            {
                if (asset2 == null)
                {
                    if (callback != null)
                    {
                        callback();
                    }
                    return;
                }
                this.textureassets.Add(asset2);
                texturea = asset2.textureObj;
                if (texturea == null)
                {
                    if (callback != null)
                    {
                        callback();
                    }
                    return;
                }
                texturea.wrapMode = TextureWrapMode.Clamp;
                if (rimage != null)
                {
                    if (string.Compare(rimage.material.shader.name, "Unlit/RGBplusA") != 0)
                    {
                        rimage.material = new Material(ImageSeperateWithAlpha.RGBPA);
                    }
                    rimage.material.SetTexture("_MainTex", texturergb);
                    rimage.material.SetTexture("_MaskTex", texturea);
                    rimage.SetMaterialDirty();
                }
                if (callback != null)
                {
                    callback();
                }
            });
        });
    }

    public void Dispose()
    {
        for (int i = 0; i < this.textureassets.Count; i++)
        {
            this.textureassets[i].TryUnload();
        }
        this.textureassets.Clear();
    }

    private static Shader RGBPA;

    private List<UITextureAsset> textureassets = new List<UITextureAsset>();
}
