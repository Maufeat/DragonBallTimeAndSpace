using System;
using UnityEngine;
using UnityEngine.UI;

public class UIFllowTarget : MonoBehaviour
{
    public Camera maincaCamera
    {
        get
        {
            if (this.maincaCamera_ == null)
            {
                this.maincaCamera_ = Camera.main;
            }
            return this.maincaCamera_;
        }
        set
        {
            this.maincaCamera_ = null;
        }
    }

    private Graphic[] gs
    {
        get
        {
            if (this.gs_ == null)
            {
                this.gs_ = base.GetComponentsInChildren<Graphic>(true);
            }
            return this.gs_;
        }
    }

    private Transform checkTarget
    {
        get
        {
            if (this.maincaCamera == null)
            {
                return null;
            }
            return this.maincaCamera.transform;
        }
    }

    private void Start()
    {
        this.textSize = LuaConfigManager.GetXmlConfigTable("massiveConfig").GetCacheField_Table("TextSize").GetCacheField_Float("value");
        this.textSize = this.testTextSize;
        this.canvas = GameObject.Find("UIRoot").GetComponent<Canvas>();
        this.rect = (base.transform as RectTransform);
        this.render = base.gameObject.GetComponent<CanvasRenderer>();
        this.render.SetAlpha(0f);
    }

    private float GetModelPosY()
    {
        if (this.followTarget.transform.parent.Find("Bip001") == null)
        {
            return this.followTarget.transform.parent.GetChild(0).localPosition.y;
        }
        return this.followTarget.transform.parent.Find("Bip001").localPosition.y;
    }

    public void BeginJump(float time)
    {
        this.startY = this.GetModelPosY();
        Scheduler.Instance.RemoveTimer(new Scheduler.OnScheduler(this.SetJumpEnd));
        Scheduler.Instance.AddTimer(time, false, new Scheduler.OnScheduler(this.SetJumpEnd));
        this.isJumping = true;
    }

    private void SetJumpEnd()
    {
        this.isJumping = false;
    }

    private void LateUpdate()
    {
        if (this.checkTarget == null || this.followTarget == null)
        {
            return;
        }
        if (this.isJumping)
        {
            this.Offset = new Vector3(0f, this.GetModelPosY() - this.startY, 0f);
        }
        else
        {
            this.Offset = Vector3.zero;
        }
        this.rect.transform.position = this.followTarget.transform.position + this.Offset;
        base.transform.rotation = this.maincaCamera.transform.rotation;
        this.distance = Vector3.Distance(base.transform.position, this.checkTarget.position);
        if ((double)Mathf.Abs(this.lastDistance - this.distance) > 0.2)
        {
            this.lastDistance = this.distance;
            float num = this.distance * this.textSize;
            if (this.m_ucScaler == null || this.m_ucScaler.Length == 0)
            {
                this.m_ucScaler = base.transform.GetComponentsInChildren<UIChildScaler>();
                this.m_lyElement = new LayoutElement[this.m_ucScaler.Length];
                for (int i = 0; i < this.m_ucScaler.Length; i++)
                {
                    this.m_lyElement[i] = this.m_ucScaler[i].GetComponent<LayoutElement>();
                }
            }
            for (int j = 0; j < this.m_ucScaler.Length; j++)
            {
                if (this.m_ucScaler[j] != null && this.m_ucScaler[j].m_bScaler)
                {
                    if (this.m_ucScaler[j].isUseLimit)
                    {
                        this.m_ucScaler[j].transform.localScale = Vector3.one * Mathf.Clamp(num, 0.5f, 1.4f);
                    }
                    else
                    {
                        this.m_ucScaler[j].transform.localScale = Vector3.one * num;
                    }
                }
            }
            this.SetAlphaByDist();
            return;
        }
    }

    private void SetAlphaByDist()
    {
        if (this.gs == null)
        {
            return;
        }
        if (this.distance >= this.maxDistance)
        {
            this.alphaValue = 0f;
        }
        else if (this.distance <= this.minDistance)
        {
            this.alphaValue = 1f;
        }
        else
        {
            this.alphaValue = 1f - (this.distance - this.minDistance) / (this.maxDistance - this.minDistance);
        }
        this.alphaValue = Mathf.Clamp01(this.alphaValue);
        for (int i = 0; i < this.gs.Length; i++)
        {
            Color color = this.gs[i].color;
            color.a = this.alphaValue;
            this.gs[i].color = color;
        }
        if (this.testMode)
        {
            this.UpdateTestData();
        }
    }

    private TextMesh tm
    {
        get
        {
            if (this.tm_ == null)
            {
                GameObject gameObject = new GameObject("txt_mesh");
                gameObject.transform.SetParent(base.transform);
                this.tm_ = gameObject.AddComponent<TextMesh>();
                this.tm_.anchor = TextAnchor.UpperCenter;
            }
            return this.tm_;
        }
    }

    private void UpdateTestData()
    {
        if (this.checkTarget.gameObject)
        {
            float num = Vector3.Distance(base.transform.position, this.checkTarget.position);
            this.tm.transform.position = base.transform.position + (this.checkTarget.position - base.transform.position).normalized;
            this.tm.transform.rotation = base.transform.rotation;
            this.tm.text = num.ToString("f1") + "M";
        }
    }

    private Canvas canvas;

    private RectTransform rect;

    public GameObject followTarget;

    private float startY;

    private bool isJumping;

    public bool testMode;

    public Vector3 Offset;

    public float offsetY;

    private Camera maincaCamera_;

    private Graphic[] gs_;

    private CanvasRenderer render;

    public float textSize = 0.1f;

    private float distance;

    private float lastDistance;

    private float alphaValue;

    private float maxDistance = 50f;

    private float minDistance = 30f;

    private UIChildScaler[] m_ucScaler;

    private LayoutElement[] m_lyElement;

    public float testTextSize = 0.08f;

    private float m_TempHight;

    private TextMesh tm_;
}
