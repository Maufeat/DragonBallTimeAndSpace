using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using LuaInterface;
using Models;
using TextModelpackage;
using UnityEngine;
using UnityEngine.UI;

public class TextModelController : ControllerBase
{
    public void setText(Text tex, string content)
    {
        int num = content.IndexOf("[");
        int num2 = content.IndexOf("]");
        if (num < 0 || num > content.Length || num2 < 0 || num2 > content.Length)
        {
            return;
        }
        string text = content.Substring(num + 1, num2 - num - 1);
        if (this.dicTrextModel.Keys.Contains(text))
        {
            TextModelContentProto textModelContentProto = this.dicTrextModel[text];
            if (textModelContentProto.fontSize != 0U)
            {
                tex.fontSize = (int)textModelContentProto.fontSize;
            }
        }
    }

    public void setTextdefaultModelOnPara(Text tex, string para)
    {
        UIInformationList component = tex.gameObject.GetComponent<UIInformationList>();
        if (component != null)
        {
            FFDebug.LogWarning(this, "       info.listInformation       " + component.listInformation.Count);
            if (component.listInformation.Count > 0)
            {
                tex.text = string.Format(this.ChangeTextModel(component.listInformation[0].content), para);
            }
        }
    }

    public void SetTextModel(Text text, string value, int index = 0)
    {
        UIInformationList component = text.gameObject.GetComponent<UIInformationList>();
        if (component != null)
        {
            index %= 10;
            if (component.listInformation.Count > index)
            {
                text.text = string.Format(this.ChangeTextModel(component.listInformation[index].content), value);
            }
            else
            {
                text.text = string.Empty;
            }
        }
    }

    public string GetModelText(Text text, int index)
    {
        UIInformationList component = text.gameObject.GetComponent<UIInformationList>();
        if (component != null)
        {
            index %= 10;
            index = Mathf.Clamp(index, 0, component.listInformation.Count - 1);
            if (component.listInformation.Count > 0)
            {
                return component.listInformation[index].content;
            }
        }
        return string.Empty;
    }

    public string ChangeTextModel(string tex)
    {
        string text = tex;
        for (int i = 0; i < this.textModelList.key.Count; i++)
        {
            text = text.Replace("[" + this.textModelList.key[i] + "]", this.textModelList.modelList[i].modelBegin);
            text = text.Replace("[/" + this.textModelList.key[i] + "]", this.textModelList.modelList[i].modelEnd);
        }
        return text;
    }

    public string RemoveTextModel(string tex)
    {
        string text = tex;
        for (int i = 0; i < this.textModelList.key.Count; i++)
        {
            text = text.Replace("[" + this.textModelList.key[i] + "]", string.Empty);
            text = text.Replace("[/" + this.textModelList.key[i] + "]", string.Empty);
        }
        return text;
    }

    public string GetContentByIDWithoutColorText(string id)
    {
        string text = this.GetContentByID(id);
        string pattern = "\\[(.+?)\\]";
        Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
        if (regex.IsMatch(text))
        {
            text = regex.Replace(text, string.Empty);
        }
        return text;
    }

    public string GetContentByID(string id)
    {
        LuaTable cacheField_Table = LuaConfigManager.GetXmlConfigTable("UIInformation").GetCacheField_Table("item").GetCacheField_Table(id);
        if (cacheField_Table == null)
        {
            FFDebug.LogWarning(this, "can not find id: " + id);
            return "Error: " + id;
        }
        return this.ChangeTextModel(cacheField_Table.GetCacheField_String("content"));
    }

    public string AddTextModel(string content, string model)
    {
        if (string.IsNullOrEmpty(model))
        {
            return content;
        }
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append("[");
        stringBuilder.Append(model);
        stringBuilder.Append("]");
        stringBuilder.Append(content);
        stringBuilder.Append("[/");
        stringBuilder.Append(model);
        stringBuilder.Append("]");
        return stringBuilder.ToString();
    }

    public string GetModelColor(string modelname)
    {
        string result = string.Empty;
        if (this.dicTrextModel.Keys.Contains(modelname))
        {
            string text = this.dicTrextModel[modelname].modelBegin;
            text = text.Replace("<b>", string.Empty);
            text = text.Replace("</b>", string.Empty);
            text = text.Replace("<i>", string.Empty);
            text = text.Replace("</i>", string.Empty);
            if (text.StartsWith("<color="))
            {
                int num = text.IndexOf(">");
                result = text.Substring(8, num - 8);
            }
        }
        return result;
    }

    public override void Awake()
    {
    }

    public override void OnUpdate()
    {
    }

    public override string ControllerName
    {
        get
        {
            return "textmodel";
        }
    }

    public Dictionary<string, TextModelContentProto> dicTrextModel = new Dictionary<string, TextModelContentProto>();

    public TextModelContentListProto textModelList;
}
