using System;
using HighlightingSystem;
using UnityEngine;

public class HighlighterController : MonoBehaviour
{
    private void Start()
    {
        this.h = base.gameObject.GetComponent<Highlighter>();
        if (null == this.h)
        {
            this.h = base.gameObject.AddComponent<Highlighter>();
        }
        this.h.SeeThroughOn();
    }

    public void MouseOver(RelationType _relationType, bool isPlayer)
    {
        if (this.h != null)
        {
            this.h.On(Const.ColorByRelation(_relationType, isPlayer));
        }
    }

    public void SetColor(Color _color)
    {
        this.h.ConstantOnImmediate(_color);
    }

    public void OnCull(bool cull)
    {
        if (null == this.h)
        {
            return;
        }
        if (cull)
        {
            this.h.ConstantOnImmediate(Color.red);
        }
        else
        {
            this.h.ConstantOff();
        }
    }

    private Highlighter h;
}
