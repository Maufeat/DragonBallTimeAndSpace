using System;
using System.Collections;
using HighlightingSystem;
using UnityEngine;

[RequireComponent(typeof(Highlighter))]
public class AutoHighLightingController : MonoBehaviour
{
    private void Start()
    {
        this.m_HighLighter = base.GetComponent<Highlighter>();
        if (null == this.m_HighLighter)
        {
            this.m_HighLighter = base.gameObject.AddComponent<Highlighter>();
        }
        if (this.showType == AutoHighLightingController.ShowType.Always)
        {
            this.m_HighLighter.ConstantOn(this.m_Color);
        }
        else
        {
            base.StartCoroutine(this.ChangeColor());
        }
    }

    private IEnumerator ChangeColor()
    {
        yield return new WaitForSeconds(this.m_StartTime);
        this.m_HighLighter.ConstantOn(this.m_Color);
        yield return new WaitForSeconds(this.m_EndTime);
        this.m_HighLighter.ConstantOff();
        yield break;
    }

    private void Destory()
    {
        base.StopAllCoroutines();
    }

    public Color m_Color;

    public float m_StartTime;

    public float m_EndTime;

    public AutoHighLightingController.ShowType showType;

    private Highlighter m_HighLighter;

    public enum ShowType
    {
        Always,
        ByTime
    }
}
