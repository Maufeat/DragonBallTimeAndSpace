using System;
using System.Collections.Generic;
using UnityEngine;

public class AtlasData : ScriptableObject
{
    private void init()
    {
        if (!this.isinit)
        {
            this.isinit = true;
            for (int i = 0; i < this.SpriteDatas.Count; i++)
            {
                this.SpriteMap[this.SpriteDatas[i].name] = this.SpriteDatas[i];
            }
        }
    }

    public Sprite GetSprite(string name)
    {
        Sprite result = null;
        this.init();
        if (this.SpriteMap.ContainsKey(name))
        {
            result = this.SpriteMap[name];
        }
        return result;
    }

    public List<Sprite> SpriteDatas = new List<Sprite>();

    private bool isinit;

    private Dictionary<string, Sprite> SpriteMap = new Dictionary<string, Sprite>();
}
