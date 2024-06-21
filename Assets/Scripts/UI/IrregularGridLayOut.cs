using System;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class IrregularGridLayOut : MonoBehaviour
{
    public void FitChildItem()
    {
        this.InitChildGraphicContent();
        this.FitAllText();
        this.SetItemPos();
        this.updateUIAction = null;
    }

    private void InitChildGraphicContent()
    {
        this.childContents = new Graphic[base.transform.childCount];
        for (int i = 0; i < base.transform.childCount; i++)
        {
            this.childContents[i] = base.transform.GetChild(i).GetComponent<Graphic>();
        }
    }

    private void FitAllText()
    {
        if (this.childContents != null)
        {
            for (int i = 0; i < this.childContents.Length; i++)
            {
                Graphic graphic = this.childContents[i];
                Text text = graphic as Text;
                if (text)
                {
                    this.FitText(text);
                }
            }
        }
    }

    private void SetItemPos()
    {
        if (this.childContents != null && this.childContents.Length > 0)
        {
            float num = 0f;
            float num2 = 0f;
            float height = this.childContents[0].rectTransform.rect.height;
            float num3 = height;
            RectTransform component = base.transform.parent.parent.GetComponent<RectTransform>();
            float width = component.rect.width;
            for (int i = 0; i < this.childContents.Length; i++)
            {
                Graphic graphic = this.childContents[i];
                if (graphic.gameObject.activeInHierarchy)
                {
                    graphic.rectTransform.pivot = new Vector2(0f, 1f);
                    if (num + graphic.rectTransform.rect.width < width)
                    {
                        graphic.rectTransform.anchoredPosition = new Vector2(num, num2);
                        num += graphic.rectTransform.rect.width;
                    }
                    else
                    {
                        string text = string.Empty;
                        Text text2 = graphic as Text;
                        if (text2)
                        {
                            text = text2.text;
                        }
                        num3 += height;
                        num = 0f;
                        num2 -= height;
                        graphic.rectTransform.anchoredPosition = new Vector2(num, num2);
                        num += graphic.rectTransform.rect.width;
                    }
                }
            }
            LayoutElement component2 = base.GetComponent<LayoutElement>();
            component2.minHeight = num3;
            component2.preferredHeight = num3;
        }
    }

    private void FitText(Text t)
    {
        string text = t.text;
        string text2 = text;
        string pattern = "<.*?>";
        Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
        foreach (object obj in regex.Matches(text))
        {
            Match match = (Match)obj;
            text2 = text2.Replace(match.Value, string.Empty);
        }
        float num = 0f;
        for (int i = 0; i < text2.Length; i++)
        {
            t.text = text2[i].ToString();
            num += t.preferredWidth;
        }
        t.text = text;
        t.rectTransform.sizeDelta = new Vector2(num + this.contentOffset, t.rectTransform.sizeDelta.y);
        LayoutElement component = t.GetComponent<LayoutElement>();
        if (component)
        {
            component.CalculateLayoutInputHorizontal();
            component.minWidth = num;
            t.SetLayoutDirty();
        }
    }

    private float contentOffset = 2f;

    private Action updateUIAction;

    private Graphic[] childContents;
}
