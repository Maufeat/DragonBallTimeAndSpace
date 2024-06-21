using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class CornerDragablePanel : MonoBehaviour, IDragHandler, IEventSystemHandler, IBeginDragHandler, IEndDragHandler
{
    private void Start()
    {
        this.mCanvas = UIManager.FindInParents<Canvas>(base.gameObject);
        HoverEventListener.Get(base.gameObject).onEnter = delegate (PointerEventData pData)
        {
            MouseStateControoler.Instan.SetMoseState(MoseState.m_scale_topleft_bottomright);
        };
        HoverEventListener.Get(base.gameObject).onExit = delegate (PointerEventData pData)
        {
            if (!this.isDraging)
            {
                MouseStateControoler.Instan.SetMoseState(MoseState.m_default);
            }
        };
    }

    private Vector2 GetMousePointPos()
    {
        Vector3 v;
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(this.mCanvas.transform as RectTransform, Input.mousePosition, this.mCanvas.worldCamera, out v))
        {
            return v;
        }
        return Vector3.zero;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        this.mStartPos = this.GetMousePointPos();
        this.mStartWidthHeight = (this.mPanel.transform as RectTransform).sizeDelta;
        this.isDraging = true;
        MouseStateControoler.Instan.SetMoseState(MoseState.m_scale_topleft_bottomright);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 zero = Vector2.zero;
        Vector2 mousePointPos = this.GetMousePointPos();
        switch (this.mCorner)
        {
            case CornerDragablePanel.Corner.TopLeft:
                zero = new Vector2(this.mStartPos.x - mousePointPos.x, mousePointPos.y - this.mStartPos.y);
                break;
            case CornerDragablePanel.Corner.TopRight:
                zero = new Vector2(mousePointPos.x - this.mStartPos.x, mousePointPos.y - this.mStartPos.y);
                break;
            case CornerDragablePanel.Corner.BottomLeft:
                zero = new Vector2(this.mStartPos.x - mousePointPos.x, this.mStartPos.y - mousePointPos.y);
                break;
            case CornerDragablePanel.Corner.BottomRight:
                zero = new Vector2(mousePointPos.x - this.mStartPos.x, this.mStartPos.y - mousePointPos.y);
                break;
        }
        float num = 1f;
        RectTransform[] componentsInParent = this.mPanel.GetComponentsInParent<RectTransform>();
        for (int i = 0; i < componentsInParent.Length; i++)
        {
            num *= componentsInParent[i].localScale.x;
        }
        Vector2 sizeDelta = this.mStartWidthHeight + zero / num;
        if (sizeDelta.x < this.mWidthRange.x)
        {
            sizeDelta.x = this.mWidthRange.x;
        }
        else if (sizeDelta.x > this.mWidthRange.y)
        {
            sizeDelta.x = this.mWidthRange.y;
        }
        if (sizeDelta.y < this.mHeightRange.x)
        {
            sizeDelta.y = this.mHeightRange.x;
        }
        else if (sizeDelta.y > this.mHeightRange.y)
        {
            sizeDelta.y = this.mHeightRange.y;
        }
        (this.mPanel.transform as RectTransform).sizeDelta = sizeDelta;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        this.isDraging = false;
        MouseStateControoler.Instan.SetMoseState(MoseState.m_default);
    }

    public GameObject mPanel;

    public CornerDragablePanel.Corner mCorner;

    public Vector2 mWidthRange = new Vector2(100f, 200f);

    public Vector2 mHeightRange = new Vector2(100f, 200f);

    private Canvas mCanvas;

    private Vector2 mStartPos;

    private Vector2 mStartWidthHeight;

    private bool isDraging;

    public enum Corner
    {
        TopLeft,
        TopRight,
        BottomLeft,
        BottomRight
    }
}
