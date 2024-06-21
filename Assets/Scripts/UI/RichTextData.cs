using System;
using Chat;

public class RichTextData
{
    public RichTextData(string _content, RichTextType type, ChatLink _link, string _color = "", bool _withunderline = false)
    {
        this.content = _content;
        this.linkdata = _link;
        this.textType = type;
        this.color = _color;
        this.withunderline = _withunderline;
    }

    public string content;

    public string color;

    public ChatLink linkdata;

    public RichTextType textType;

    public bool withunderline;
}
