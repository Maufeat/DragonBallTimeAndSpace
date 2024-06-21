using System;
using Framework.Managers;
using LuaInterface;
using UnityEngine;
using UnityEngine.UI;

public class ClassSetItemTexture
{
    public static ClassSetItemTexture Instance
    {
        get
        {
            if (ClassSetItemTexture._instance == null)
            {
                ClassSetItemTexture._instance = new ClassSetItemTexture();
            }
            return ClassSetItemTexture._instance;
        }
    }

    public void SetItemTexture(Image tmpImage, uint itemId, ImageType type, Action<UITextureAsset> callback)
    {
        LuaTable configTable = LuaConfigManager.GetConfigTable("objects", (ulong)itemId);
        if (configTable != null)
        {
            this.setTexture(tmpImage, configTable.GetField_String("icon"), type, callback);
        }
        else
        {
            FFDebug.LogWarning(null, " config   is  null  with this id" + itemId);
        }
    }

    public void setTexture(Image tmpImage, string imagename, ImageType type, Action<UITextureAsset> callback)
    {
        ManagerCenter.Instance.GetManager<UITextureMgr>().GetTexture(type, imagename, delegate (UITextureAsset asset)
        {
            if (tmpImage == null)
            {
                return;
            }
            if (asset == null)
            {
                FFDebug.LogWarning(null, "    req  texture   is  null ");
                callback(null);
                return;
            }
            if (asset.textureObj == null)
            {
                callback(null);
                return;
            }
            Texture2D textureObj = asset.textureObj;
            Sprite sprite = Sprite.Create(textureObj, new Rect(0f, 0f, (float)textureObj.width, (float)textureObj.height), new Vector2(0f, 0f));
            tmpImage.overrideSprite = sprite;
            tmpImage.sprite = sprite;
            callback(asset);
        });
    }

    public void SetItemTexture(RawImage tmpImage, uint itemId, ImageType type, Action<UITextureAsset> callback)
    {
        LuaTable configTable = LuaConfigManager.GetConfigTable("objects", (ulong)itemId);
        if (configTable != null)
        {
            this.setTexture(tmpImage, configTable.GetField_String("icon"), type, callback);
        }
        else
        {
            FFDebug.LogWarning(null, " config   is  null  with this id" + itemId);
        }
    }

    public void setTexture(RawImage tmpImage, string imagename, ImageType type, Action<UITextureAsset> callback)
    {
        ManagerCenter.Instance.GetManager<UITextureMgr>().GetTexture(type, imagename, delegate (UITextureAsset asset)
        {
            if (tmpImage == null)
            {
                return;
            }
            if (asset == null)
            {
                FFDebug.LogWarning(null, "    req  texture   is  null ");
                callback(null);
                return;
            }
            if (asset.textureObj == null)
            {
                callback(null);
                return;
            }
            Texture2D textureObj = asset.textureObj;
            Sprite sprite = Sprite.Create(textureObj, new Rect(0f, 0f, (float)textureObj.width, (float)textureObj.height), new Vector2(0f, 0f));
            tmpImage.texture = sprite.texture;
            tmpImage.color = Color.white;
            callback(asset);
        });
    }

    private static ClassSetItemTexture _instance;
}
