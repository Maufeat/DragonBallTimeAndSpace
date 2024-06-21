using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[AddComponentMenu("UI/LinkImageText", 10)]
public class LinkImageText : Text, IPointerClickHandler, IEventSystemHandler
{
    public LinkImageText.HrefClickEvent onHrefClick
    {
        get
        {
            return this.m_OnHrefClick;
        }
        set
        {
            this.m_OnHrefClick = value;
        }
    }

    public override void SetVerticesDirty()
    {
        base.SetVerticesDirty();
        this.UpdateQuadImage();
    }

    protected void UpdateQuadImage()
    {
        this.m_OutputText = this.GetOutputText(this.text);
        this.m_ImagesVertexIndex.Clear();
        foreach (object obj in LinkImageText.s_ImageRegex.Matches(this.m_OutputText))
        {
            Match match = (Match)obj;
            int index = match.Index;
            int item = index * 4 + 3;
            this.m_ImagesVertexIndex.Add(item);
            this.m_ImagesPool.RemoveAll((Image image) => image == null);
            if (this.m_ImagesPool.Count == 0)
            {
                base.GetComponentsInChildren<Image>(this.m_ImagesPool);
            }
            if (this.m_ImagesVertexIndex.Count > this.m_ImagesPool.Count)
            {
                GameObject gameObject = DefaultControls.CreateImage(default(DefaultControls.Resources));
                gameObject.layer = base.gameObject.layer;
                RectTransform rectTransform = gameObject.transform as RectTransform;
                if (rectTransform)
                {
                    rectTransform.SetParent(base.rectTransform);
                    rectTransform.localPosition = Vector3.zero;
                    rectTransform.localRotation = Quaternion.identity;
                    rectTransform.localScale = Vector3.one;
                }
                this.m_ImagesPool.Add(gameObject.GetComponent<Image>());
            }
            string value = match.Groups[1].Value;
            float num = float.Parse(match.Groups[2].Value);
            Image image2 = this.m_ImagesPool[this.m_ImagesVertexIndex.Count - 1];
            if (image2.sprite == null || image2.sprite.name != value)
            {
                CommonTools.SetFaceIcon(value, image2);
            }
            image2.rectTransform.sizeDelta = new Vector2(num, num);
            image2.enabled = true;
        }
        for (int i = this.m_ImagesVertexIndex.Count; i < this.m_ImagesPool.Count; i++)
        {
            if (this.m_ImagesPool[i])
            {
                this.m_ImagesPool[i].enabled = false;
            }
        }
    }

    private void OnDrawGizmos()
    {
        for (int i = 0; i < this.m_CallbackInfos.Count; i++)
        {
            List<Rect> boxes = this.m_CallbackInfos[i].boxes;
            for (int j = 0; j < boxes.Count; j++)
            {
                this.DrawRect(boxes[j]);
            }
        }
    }

    private void DrawRect(Rect rect)
    {
        Vector3 vector = rect.min + rect.height * Vector2.up;
        Vector3 vector2 = rect.max;
        Vector3 vector3 = rect.min + Vector2.right * rect.width;
        Vector3 vector4 = rect.min;
        vector = base.transform.TransformPoint(vector);
        vector2 = base.transform.TransformPoint(vector2);
        vector3 = base.transform.TransformPoint(vector3);
        vector4 = base.transform.TransformPoint(vector4);
        Gizmos.DrawLine(vector, vector2);
        Gizmos.DrawLine(vector2, vector3);
        Gizmos.DrawLine(vector3, vector4);
        Gizmos.DrawLine(vector4, vector);
    }

    protected override void OnPopulateMesh(VertexHelper toFill)
    {
        string text = this.m_Text;
        this.m_Text = this.m_OutputText;
        base.OnPopulateMesh(toFill);
        this.m_Text = text;
        UIVertex vertex = default(UIVertex);
        for (int i = 0; i < this.m_ImagesVertexIndex.Count; i++)
        {
            int num = this.m_ImagesVertexIndex[i];
            RectTransform rectTransform = this.m_ImagesPool[i].rectTransform;
            Vector2 sizeDelta = rectTransform.sizeDelta;
            if (num < toFill.currentVertCount)
            {
                toFill.PopulateUIVertex(ref vertex, num);
                rectTransform.anchoredPosition = new Vector2(vertex.position.x + sizeDelta.x / 2f, vertex.position.y + sizeDelta.y / 2f);
                toFill.PopulateUIVertex(ref vertex, num - 3);
                Vector3 position = vertex.position;
                int j = num;
                int num2 = num - 3;
                while (j > num2)
                {
                    toFill.PopulateUIVertex(ref vertex, num);
                    vertex.position = position;
                    toFill.SetUIVertex(vertex, j);
                    j--;
                }
            }
        }
        if (this.m_ImagesVertexIndex.Count != 0)
        {
            this.m_ImagesVertexIndex.Clear();
        }
        foreach (LinkImageText.CallbackInfo callbackInfo in this.m_CallbackInfos)
        {
            callbackInfo.boxes.Clear();
            if (callbackInfo.startIndex < toFill.currentVertCount)
            {
                toFill.PopulateUIVertex(ref vertex, callbackInfo.startIndex);
                Vector3 position2 = vertex.position;
                Bounds bounds = new Bounds(position2, Vector3.zero);
                int k = callbackInfo.startIndex;
                int endIndex = callbackInfo.endIndex;
                while (k < endIndex)
                {
                    if (k >= toFill.currentVertCount)
                    {
                        break;
                    }
                    toFill.PopulateUIVertex(ref vertex, k);
                    position2 = vertex.position;
                    if (position2.x < bounds.min.x)
                    {
                        callbackInfo.boxes.Add(new Rect(bounds.min, bounds.size));
                        bounds = new Bounds(position2, Vector3.zero);
                    }
                    else
                    {
                        bounds.Encapsulate(position2);
                    }
                    k++;
                }
                callbackInfo.boxes.Add(new Rect(bounds.min, bounds.size));
            }
        }
        foreach (LinkImageText.HrefInfo hrefInfo in this.m_HrefInfos)
        {
            hrefInfo.boxes.Clear();
            if (hrefInfo.startIndex < toFill.currentVertCount)
            {
                toFill.PopulateUIVertex(ref vertex, hrefInfo.startIndex);
                Vector3 position3 = vertex.position;
                Bounds bounds2 = new Bounds(position3, Vector3.zero);
                int l = hrefInfo.startIndex;
                int endIndex2 = hrefInfo.endIndex;
                while (l < endIndex2)
                {
                    if (l >= toFill.currentVertCount)
                    {
                        break;
                    }
                    toFill.PopulateUIVertex(ref vertex, l);
                    position3 = vertex.position;
                    if (position3.x < bounds2.min.x)
                    {
                        hrefInfo.boxes.Add(new Rect(bounds2.min, bounds2.size));
                        bounds2 = new Bounds(position3, Vector3.zero);
                    }
                    else
                    {
                        bounds2.Encapsulate(position3);
                    }
                    l++;
                }
                hrefInfo.boxes.Add(new Rect(bounds2.min, bounds2.size));
            }
        }
    }

    protected virtual string GetOutputText(string outputText)
    {
        LinkImageText.s_TextBuilder.Length = 0;
        this.m_CallbackInfos.Clear();
        int num = 0;
        foreach (object obj in LinkImageText.s_CallbackRegex.Matches(outputText))
        {
            Match match = (Match)obj;
            LinkImageText.s_TextBuilder.Append(outputText.Substring(num, match.Index - num));
            LinkImageText.s_TextBuilder.Append("<size=14>");
            Group group = match.Groups[1];
            LinkImageText.CallbackInfo item = new LinkImageText.CallbackInfo
            {
                startIndex = LinkImageText.s_TextBuilder.Length * 4,
                endIndex = (LinkImageText.s_TextBuilder.Length + group.Length - 1) * 4 + 3,
                name = group.Value
            };
            this.m_CallbackInfos.Add(item);
            LinkImageText.s_TextBuilder.Append(group.Value);
            LinkImageText.s_TextBuilder.Append("</size>");
            num = match.Index + match.Length;
        }
        LinkImageText.s_TextBuilder.Append(outputText.Substring(num, outputText.Length - num));
        outputText = LinkImageText.s_TextBuilder.ToString();
        LinkImageText.s_TextBuilder.Length = 0;
        this.m_HrefInfos.Clear();
        num = 0;
        foreach (object obj2 in LinkImageText.s_HrefRegex.Matches(outputText))
        {
            Match match2 = (Match)obj2;
            LinkImageText.s_TextBuilder.Append(outputText.Substring(num, match2.Index - num));
            LinkImageText.s_TextBuilder.Append("<color=blue>");
            Group group2 = match2.Groups[1];
            LinkImageText.HrefInfo item2 = new LinkImageText.HrefInfo
            {
                startIndex = LinkImageText.s_TextBuilder.Length * 4,
                endIndex = (LinkImageText.s_TextBuilder.Length + match2.Groups[2].Length - 1) * 4 + 3,
                name = group2.Value
            };
            this.m_HrefInfos.Add(item2);
            LinkImageText.s_TextBuilder.Append(match2.Groups[2].Value);
            LinkImageText.s_TextBuilder.Append("</color>");
            num = match2.Index + match2.Length;
        }
        LinkImageText.s_TextBuilder.Append(outputText.Substring(num, outputText.Length - num));
        return LinkImageText.s_TextBuilder.ToString();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Vector2 point;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(base.rectTransform, eventData.position, eventData.pressEventCamera, out point);
        if (eventData.button == PointerEventData.InputButton.Right || eventData.button == PointerEventData.InputButton.Left)
        {
            foreach (LinkImageText.CallbackInfo callbackInfo in this.m_CallbackInfos)
            {
                List<Rect> boxes = callbackInfo.boxes;
                for (int i = 0; i < boxes.Count; i++)
                {
                    if (boxes[i].Contains(point))
                    {
                        if (this.onCallback != null)
                        {
                            this.onCallback(callbackInfo.name, eventData);
                        }
                        return;
                    }
                }
            }
        }
        foreach (LinkImageText.HrefInfo hrefInfo in this.m_HrefInfos)
        {
            List<Rect> boxes2 = hrefInfo.boxes;
            for (int j = 0; j < boxes2.Count; j++)
            {
                if (boxes2[j].Contains(point))
                {
                    this.m_OnHrefClick.Invoke(hrefInfo.name);
                    return;
                }
            }
        }
    }

    private string m_OutputText;

    protected readonly List<Image> m_ImagesPool = new List<Image>();

    private readonly List<int> m_ImagesVertexIndex = new List<int>();

    private readonly List<LinkImageText.HrefInfo> m_HrefInfos = new List<LinkImageText.HrefInfo>();

    private readonly List<LinkImageText.CallbackInfo> m_CallbackInfos = new List<LinkImageText.CallbackInfo>();

    protected static readonly StringBuilder s_TextBuilder = new StringBuilder();

    [SerializeField]
    private LinkImageText.HrefClickEvent m_OnHrefClick = new LinkImageText.HrefClickEvent();

    public Action<string, PointerEventData> onCallback;

    private static readonly Regex s_ImageRegex = new Regex("<quad name=(.+?) size=(\\d*\\.?\\d+%?) width=(\\d*\\.?\\d+%?) />", RegexOptions.Singleline);

    private static readonly Regex s_HrefRegex = new Regex("<a href=([^>\\n\\s]+)>(.*?)(</a>)", RegexOptions.Singleline);

    private static readonly Regex s_CallbackRegex = new Regex("<size=14[^>]*?>(?<Text>[^<]*)</size>", RegexOptions.Singleline);

    public static Func<string, Sprite> funLoadSprite;

    [Serializable]
    public class HrefClickEvent : UnityEvent<string>
    {
    }

    private class HrefInfo
    {
        public int startIndex;

        public int endIndex;

        public string name;

        public readonly List<Rect> boxes = new List<Rect>();
    }

    private class CallbackInfo
    {
        public int startIndex;

        public int endIndex;

        public string name;

        public readonly List<Rect> boxes = new List<Rect>();
    }
}
