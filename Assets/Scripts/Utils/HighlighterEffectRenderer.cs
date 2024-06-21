using System;
using System.Collections.Generic;
using HighlightingSystem;
using UnityEngine;

public class HighlighterEffectRenderer : MonoBehaviour
{
    private void Update()
    {
        if (!this.m_onPlay)
        {
            return;
        }
        this.runningTime += Time.deltaTime;
        this.OnUpdate();
    }

    private void OnUpdate()
    {
        switch (this.m_cullState)
        {
            case HighlighterEffectRenderer.CullState.None:
                if (this.runningTime > this.m_startDelay)
                {
                    this.m_cullState = HighlighterEffectRenderer.CullState.Normal;
                }
                break;
            case HighlighterEffectRenderer.CullState.Normal:
                this.OnCull(true);
                this.m_cullState = HighlighterEffectRenderer.CullState.Highlight;
                break;
            case HighlighterEffectRenderer.CullState.Highlight:
                if (this.runningTime > this.m_overtime)
                {
                    this.m_cullState = HighlighterEffectRenderer.CullState.Disabled;
                }
                break;
            case HighlighterEffectRenderer.CullState.Disabled:
                this.OnCull(false);
                this.m_cullState = HighlighterEffectRenderer.CullState.Closed;
                break;
            case HighlighterEffectRenderer.CullState.Closed:
                if (this.runningTime < this.m_overtime)
                {
                    this.m_cullState = HighlighterEffectRenderer.CullState.None;
                }
                break;
        }
    }

    public void SetHighterlighterState(GameObject _target, List<string> _BindPointNodes)
    {
        this.OnReset();
        this.m_targetObject = _target;
        this.m_BindPointNodes = _BindPointNodes;
        this.AddCompomentOnTarget();
    }

    public void SetHighterlighterState(GameObject _target, bool applychild)
    {
        this.OnReset();
        this.m_targetObject = _target;
        this.m_bApplyChild = applychild;
        this.m_renderTarget = HighlighterEffectRenderer.RenderTarget.All;
        this.AddCompomentOnTarget();
    }

    public void OnPlay(float time)
    {
        if (this.m_onPlay)
        {
            return;
        }
        this.runningTime = time;
        this.OnUpdate();
    }

    public void OnPlay()
    {
        this.m_onPlay = true;
    }

    private void AddCompomentOnTarget()
    {
        if (null != this.m_targetObject)
        {
            switch (this.m_renderTarget)
            {
                case HighlighterEffectRenderer.RenderTarget.All:
                    this.RenderTargetByAll();
                    break;
                case HighlighterEffectRenderer.RenderTarget.ModelSkinned:
                    this.RenderTargetByModelSkinned();
                    break;
                case HighlighterEffectRenderer.RenderTarget.BindPoint:
                    this.RenderTargetByBindPoint();
                    break;
            }
        }
        if (null != Camera.main)
        {
            HighlightingRenderer y = Camera.main.GetComponent<HighlightingRenderer>();
            if (null == y)
            {
                y = Camera.main.gameObject.AddComponent<HighlightingRenderer>();
            }
        }
    }

    private void OnCull(bool cull)
    {
        if (this.hgtlist == null && this.hgtlist.Count <= 0)
        {
            return;
        }
        for (int i = 0; i < this.hgtlist.Count; i++)
        {
            if (cull)
            {
                this.hgtlist[i].ConstantOnImmediate(Color.red);
            }
            else
            {
                this.hgtlist[i].ConstantOff();
            }
        }
    }

    private void RenderTargetByAll()
    {
        Highlighter highlighter = this.m_targetObject.GetComponent<Highlighter>();
        if (null == highlighter)
        {
            highlighter = this.m_targetObject.AddComponent<Highlighter>();
        }
        this.hgtlist.Add(highlighter);
    }

    private void RenderTargetByModelSkinned()
    {
        foreach (Transform transform in this.m_targetObject.GetComponentsInChildren<Transform>(true))
        {
            if (transform.GetComponent<Renderer>())
            {
                if (!this.ContainBindPointByParentTransform(transform))
                {
                    Highlighter highlighter = transform.GetComponent<Highlighter>();
                    if (null == highlighter)
                    {
                        highlighter = transform.gameObject.AddComponent<Highlighter>();
                    }
                    this.hgtlist.Add(highlighter);
                }
            }
        }
    }

    private void RenderTargetByBindPoint()
    {
        foreach (object obj in this.m_targetObject.GetComponentInChildren<Transform>(true))
        {
            Transform transform = (Transform)obj;
            if (this.IsBindPointNode(transform.name))
            {
                Highlighter highlighter = transform.GetComponent<Highlighter>();
                if (null == highlighter)
                {
                    highlighter = transform.gameObject.AddComponent<Highlighter>();
                }
                this.hgtlist.Add(highlighter);
            }
        }
    }

    public void RemoveHighlighter()
    {
        if (this.hgtlist != null && this.hgtlist.Count > 0)
        {
            for (int i = 0; i < this.hgtlist.Count; i++)
            {
                Transform transform = this.hgtlist[i].transform;
                if (transform != null)
                {
                    UnityEngine.Object.Destroy(transform.GetComponent<Highlighter>());
                }
            }
        }
    }

    private bool IsBindPointNode(string node)
    {
        for (int i = 0; i < this.m_BindPointNodes.Count; i++)
        {
            if (node.ToLower() == this.m_BindPointNodes[i].ToLower())
            {
                return true;
            }
        }
        return false;
    }

    private bool ContainBindPointByParentTransform(Transform tran)
    {
        Transform transform = tran;
        while (transform != null)
        {
            if (this.IsBindPointNode(transform.name))
            {
                return true;
            }
            transform = transform.parent;
        }
        return false;
    }

    private void OnReset()
    {
        this.m_BindPointNodes.Clear();
        this.m_targetObject = null;
        this.m_cullState = HighlighterEffectRenderer.CullState.None;
        this.runningTime = 0f;
    }

    public float m_startDelay;

    public float m_overtime;

    public HighlighterEffectRenderer.TargetType m_targetType;

    public HighlighterEffectRenderer.RenderTarget m_renderTarget = HighlighterEffectRenderer.RenderTarget.All;

    private bool m_bapplyChild;

    private GameObject m_targetObject;

    private List<string> m_BindPointNodes = new List<string>();

    private HighlighterEffectRenderer.CullState m_cullState;

    private bool m_bApplyChild = true;

    private float runningTime;

    private List<Highlighter> hgtlist = new List<Highlighter>();

    private bool m_onPlay;

    public enum CullState
    {
        None,
        Normal,
        Highlight,
        Disabled,
        Closed
    }

    public enum TargetType
    {
        Caster,
        BeHit,
        Itself
    }

    public enum RenderTarget
    {
        None,
        All,
        ModelSkinned,
        BindPoint
    }
}
