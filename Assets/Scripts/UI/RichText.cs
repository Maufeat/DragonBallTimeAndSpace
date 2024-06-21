using System;
using System.Collections.Generic;
using System.Text;
using Chat;
using UnityEngine;

public class RichText
{
    public RichText(ChatData data, string prefix, GameObject lineprefab, float maxwidth)
    {
        this.chatData = data;
        this.prefixdata = prefix;
        this.LinePrefab = lineprefab;
        this.maxWidth = maxwidth;
        this.Lines.BetterForeach(delegate (KeyValuePair<int, RichTextLine> pair)
        {
            pair.Value.DestroyThis();
        });
        this.Lines.Clear();
    }

    public int LineCount
    {
        get
        {
            return this.Lines.Count;
        }
    }

    public void GenerateRichText()
    {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append(this.prefixdata);
        stringBuilder.Append(this.chatData.content);
        string text = this.ConvertRichText(stringBuilder.ToString());
        List<RichTextData> list = new List<RichTextData>();
        int num = 0;
        try
        {
            while (text.Length > 0)
            {
                int num2 = text.IndexOf("*#");
                if (num2 > 0)
                {
                    string content = text.Substring(0, num2);
                    RichTextData item = new RichTextData(content, RichTextType.NormalText, null, string.Empty, false);
                    list.Add(item);
                    text = text.Remove(0, num2);
                }
                int num3 = text.IndexOf("#*");
                if (num3 > 0)
                {
                    string text2 = text.Substring(2, num3 - 2);
                    string[] array = text2.Split(new char[]
                    {
                        ','
                    });
                    string content2 = array[0];
                    string a = array[1];
                    string color = array[2];
                    string a2 = array[3];
                    bool withunderline = false;
                    if (a2 == "u")
                    {
                        withunderline = true;
                    }
                    RichTextData richTextData = null;
                    if (a == "lk")
                    {
                        if (this.chatData.link.Count > num)
                        {
                            richTextData = new RichTextData(content2, RichTextType.HyperText, this.chatData.link[num], color, withunderline);
                            num++;
                        }
                        else
                        {
                            richTextData = new RichTextData(content2, RichTextType.HyperText, null, color, withunderline);
                        }
                    }
                    else if (a == "cn")
                    {
                        richTextData = new RichTextData(content2, RichTextType.CharName, null, color, withunderline);
                    }
                    else if (a == string.Empty || a == "nt")
                    {
                        richTextData = new RichTextData(content2, RichTextType.NormalText, null, color, withunderline);
                    }
                    if (richTextData != null)
                    {
                        list.Add(richTextData);
                        text = text.Remove(0, num3 + 2);
                    }
                }
                else
                {
                    string content3 = text.Substring(0, text.Length);
                    RichTextData item2 = new RichTextData(content3, RichTextType.NormalText, null, string.Empty, false);
                    list.Add(item2);
                    text = text.Remove(0, text.Length);
                }
            }
        }
        catch
        {
            RichTextData item3 = new RichTextData(text, RichTextType.NormalText, null, string.Empty, false);
            list.Add(item3);
        }
        for (int i = 0; i < list.Count; i++)
        {
            this.CreatRichText(list[i]);
        }
    }

    public void CreatRichText(RichTextData data)
    {
        string text = data.content;
        RichTextType textType = data.textType;
        string color = data.color;
        bool withunderline = data.withunderline;
        text = UITools.RemoveRichText(text);
        while (text.Length > 0)
        {
            RichTextLine richTextLine;
            if (this.Lines.ContainsKey(this.currlineid))
            {
                richTextLine = this.Lines[this.currlineid];
            }
            else
            {
                richTextLine = new RichTextLine(this, this.LinePrefab);
                this.Lines[this.currlineid] = richTextLine;
            }
            StringBuilder stringBuilder = new StringBuilder();
            float width = richTextLine.GetWidth();
            for (int i = 0; i < text.Length; i++)
            {
                stringBuilder.Append(text[i]);
                if (width + UITools.GetTextWidth(richTextLine.textPrefab, stringBuilder.ToString()) > this.maxWidth)
                {
                    stringBuilder.Remove(stringBuilder.Length - 1, 1);
                    this.currlineid++;
                    break;
                }
            }
            richTextLine.CreatRichTextStruct(textType, stringBuilder.ToString(), data.linkdata, color, withunderline);
            text = text.Remove(0, stringBuilder.Length);
        }
    }

    public string ConvertRichText(string content)
    {
        string text = content.Replace("<b>", string.Empty);
        text = text.Replace("</b>", string.Empty);
        text = text.Replace("<i>", string.Empty);
        text = text.Replace("</i>", string.Empty);
        StringBuilder stringBuilder = new StringBuilder();
        try
        {
            while (text.Length > 0)
            {
                int num = text.IndexOf("<color=");
                if (num >= 0)
                {
                    string str = text.Substring(0, num);
                    stringBuilder.Append(this.ProcessColorText(str));
                    text = text.Remove(0, num);
                }
                int num2 = text.IndexOf("</color>");
                if (num2 >= 0)
                {
                    string str2 = text.Substring(0, num2 + 8);
                    stringBuilder.Append(this.ProcessColorText(str2));
                    text = text.Remove(0, num2 + 8);
                }
                else
                {
                    string str3 = text.Substring(0, text.Length);
                    stringBuilder.Append(this.ProcessColorText(str3));
                    text = text.Remove(0, text.Length);
                }
            }
        }
        catch (Exception)
        {
            stringBuilder.Append(content);
        }
        return stringBuilder.ToString();
    }

    private string ProcessColorText(string str)
    {
        StringBuilder stringBuilder = new StringBuilder();
        try
        {
            if (!str.StartsWith("<color="))
            {
                stringBuilder.Append(str);
            }
            else
            {
                int num = str.IndexOf(">");
                string value = str.Substring(8, num - 8);
                int num2 = str.IndexOf(">");
                int num3 = str.IndexOf("</color>");
                string value2 = str.Substring(num2 + 1, num3 - num2 - 1);
                stringBuilder.Append("*#");
                stringBuilder.Append(value2);
                stringBuilder.Append(",,");
                stringBuilder.Append(value);
                stringBuilder.Append(",");
                stringBuilder.Append("#*");
            }
        }
        catch (Exception)
        {
            return str;
        }
        return stringBuilder.ToString();
    }

    private int currlineid = 1;

    private float maxWidth;

    private BetterDictionary<int, RichTextLine> Lines = new BetterDictionary<int, RichTextLine>();

    private string prefixdata;

    public ChatData chatData;

    private GameObject LinePrefab;
}
