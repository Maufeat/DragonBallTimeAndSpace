using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class WayNode
{
    public void PaseData(string str)
    {
        string text = GlobalRegister.ConfigColorToRichTextFormat(str);
        text = text.Replace("{", string.Empty).Replace("}", string.Empty);
        this.text = text;
        string pattern = "[[][0-9|a-f]{6}[]]";
        Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
        IEnumerator enumerator = regex.Matches(str).GetEnumerator();
        this.color = Color.white;
        if (enumerator.MoveNext())
        {
            Match match = (Match)enumerator.Current;
            string value = match.Value.Substring(1, 2);
            string value2 = match.Value.Substring(3, 2);
            string value3 = match.Value.Substring(5, 2);
            this.color = new Color((float)Convert.ToInt32(value, 16) / 255f, (float)Convert.ToInt32(value2, 16) / 255f, (float)Convert.ToInt32(value3, 16) / 255f);
        }
    }

    public string text;

    public Color color;

    public WayNodeType wayNodeType;

    public Dictionary<uint, uint> endPreFix;

    public int wayIdIndex;
}
