using System;
using System.Collections.Generic;
using Chat;
using UnityEngine;
using UnityEngine.UI;

public class RichTextLine
{
    public RichTextLine(RichText richtext, GameObject linepre)
    {
        this.richText = richtext;
        this.textListInLine.Clear();
        this.Tran = UnityEngine.Object.Instantiate<GameObject>(linepre).transform;
        this.Tran.SetParent(linepre.transform.parent);
        this.Tran.localScale = Vector3.one;
        this.textPrefab = this.Tran.Find("text").GetComponent<Text>();
        this.Tran.gameObject.SetActive(true);
    }

    public float GetWidth()
    {
        float num = 0f;
        for (int i = 0; i < this.textListInLine.Count; i++)
        {
            num += this.textListInLine[i].GetWidth();
        }
        return num;
    }

    public void CreatRichTextStruct(RichTextType type, string content, ChatLink link, string color = "", bool withunderline = false)
    {
        RichTextStruct item = new RichTextStruct(type, content, this, link, color, withunderline);
        this.textListInLine.Add(item);
    }

    public void DestroyThis()
    {
        for (int i = 0; i < this.textListInLine.Count; i++)
        {
            this.textListInLine[i].DestroyThis();
        }
        this.textListInLine.Clear();
    }

    private List<RichTextStruct> textListInLine = new List<RichTextStruct>();

    public Transform Tran;

    public Text textPrefab;

    public RichText richText;
}
