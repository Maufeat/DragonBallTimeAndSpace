using System;
using Framework.Base;
using Framework.Managers;
using UnityEngine;
using UnityEngine.UI;

public class FontMgr : IManager
{
    public string ManagerName
    {
        get
        {
            return base.GetType().Name;
        }
    }

    public void Init()
    {
        Font.textureRebuilt += this.FontTextureRebuild;
    }

    private void LoadFont(string strFontsPath)
    {
        this.m_fonts = Resources.LoadAll<Font>(strFontsPath);
        TextAsset textAsset = Resources.Load<TextAsset>("CommonChinese/CommonChinese");
        for (int i = 0; i < this.m_fonts.Length; i++)
        {
            if (this.m_fonts[i].name == "chinese")
            {
                Font font = this.m_fonts[i];
                font.RequestCharactersInTexture(textAsset.text);
            }
        }
    }

    private void SetTextFont(GameObject obj, string fontName)
    {
        if (null == obj)
        {
            FFDebug.LogWarning(this, "input ui prefab is null,please check!");
            return;
        }
        Text[] componentsInChildren = obj.GetComponentsInChildren<Text>(true);
        if (componentsInChildren.Length > 0)
        {
            for (int i = 0; i < componentsInChildren.Length; i++)
            {
                componentsInChildren[i].font = this.GetFont(fontName);
            }
        }
    }

    public Font GetFont(string fontName)
    {
        if (this.m_fonts == null || string.IsNullOrEmpty(fontName))
        {
            return null;
        }
        if (this.m_fonts.Length == 0)
        {
            return null;
        }
        for (int i = 0; i < this.m_fonts.Length; i++)
        {
            if (this.m_fonts[i].name.Equals(fontName))
            {
                return this.m_fonts[i];
            }
        }
        return null;
    }

    public void SetUIFont(Transform root)
    {
        foreach (Text text in root.GetComponentsInChildren<Text>(true))
        {
            this.SetTextFont(text.gameObject, text.GetComponent<TextFontName>().name.strFontName);
        }
    }

    private void FontTextureRebuild(Font f)
    {
        if (string.Compare(f.name, "chinese") == 0)
        {
            this.dynamicfontDirty = true;
        }
    }

    public void OnUpdate()
    {
        UIManager manager = ManagerCenter.Instance.GetManager<UIManager>();
        if (this.dynamicfontDirty && manager != null)
        {
            Text[] componentsInChildren = manager.GetUIParent(UIManager.ParentType.UIRoot).GetComponentsInChildren<Text>(true);
            for (int i = 0; i < componentsInChildren.Length; i++)
            {
                if (!(componentsInChildren[i].font == null))
                {
                    if (componentsInChildren[i].font.name == "chinese")
                    {
                        componentsInChildren[i].FontTextureChanged();
                    }
                }
            }
            Text[] componentsInChildren2 = manager.GetUIParent(UIManager.ParentType.HPRoot).GetComponentsInChildren<Text>(true);
            for (int j = 0; j < componentsInChildren2.Length; j++)
            {
                if (!(componentsInChildren2[j].font == null))
                {
                    if (componentsInChildren2[j].font.name == "chinese")
                    {
                        componentsInChildren2[j].FontTextureChanged();
                    }
                }
            }
            this.dynamicfontDirty = false;
        }
    }

    public void OnReSet()
    {
        Font.textureRebuilt -= this.FontTextureRebuild;
    }

    private Font[] m_fonts;

    public readonly string strFontsPath = "Font";

    public readonly string strFontNum = string.Empty;

    public readonly string strFontEnglish = string.Empty;

    private bool dynamicfontDirty;
}
