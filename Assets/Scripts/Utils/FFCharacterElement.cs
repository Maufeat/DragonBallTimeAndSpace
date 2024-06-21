using System;
using System.Collections.Generic;
using UnityEngine;

public class FFCharacterElement
{
    public FFCharacterElement(FFAssetBundle ab, string name)
    {
        this.AB = ab;
        this.ElementName = name;
        this.Character = this.ElementName.Split(new char[]
        {
            '/'
        })[0];
        this.Model = this.ElementName.Split(new char[]
        {
            '/'
        })[1].Split(new char[]
        {
            '@'
        })[0];
        this.rendererobject = this.AB.GetAssetByName<GameObject>("rendererobject");
        this.bones = this.AB.GetAssetByName<StringHolder>("bonenames");
        if (this.rendererobject == null)
        {
            FFDebug.LogWarning(this, string.Concat(new object[]
            {
                "RendererObj：",
                this.ElementName,
                " rendererobject null  AB assetscount :",
                this.AB.allAssets.Length
            }));
        }
        if (this.bones == null)
        {
            FFDebug.LogWarning(this, string.Concat(new object[]
            {
                "RendererObj：",
                this.ElementName,
                " bones null  AB assetscount :",
                this.AB.allAssets.Length
            }));
        }
    }

    public static FFCharacterElement GetFFCharacterElement(string Key)
    {
        string text = Key.ToLower();
        if (FFCharacterElement.FFCharacterElementMap.ContainsKey(text))
        {
            return FFCharacterElement.FFCharacterElementMap[text];
        }
        FFDebug.LogWarning("FFCharacterElement", "cant find  key :" + text + "  in  FFCharacterElementMap");
        return null;
    }

    public SkinnedMeshRenderer GetSkinnedMeshRenderer()
    {
        GameObject gameObject = this.rendererobject;
        return (SkinnedMeshRenderer)gameObject.GetComponent<Renderer>();
    }

    public string[] GetBoneNames()
    {
        return this.bones.content;
    }

    public void DisPose()
    {
        if (this.rendererobject != null)
        {
            UnityEngine.Object.Destroy(this.rendererobject);
        }
        if (this.bones != null)
        {
            UnityEngine.Object.Destroy(this.bones);
        }
        this.AB.Unload();
        FFCharacterElement.FFCharacterElementMap.Remove(this.ElementName);
    }

    public static Dictionary<string, FFCharacterElement> FFCharacterElementMap = new Dictionary<string, FFCharacterElement>();

    public string ElementName;

    public string Model;

    public string Character;

    public GameObject rendererobject;

    public StringHolder bones;

    public FFAssetBundle AB;
}
