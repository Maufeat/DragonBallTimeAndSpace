using System;
using Engine;
using Framework.Managers;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragableArea : MonoBehaviour, IDragHandler, IEventSystemHandler, IBeginDragHandler, IEndDragHandler
{
    private void Awake()
    {
    }

    private void SetInitPos()
    {
        if (this.layoutType == HorizontalLayoutType.Center)
        {
            return;
        }
        RectTransform rectTransform = this.mRootPanel.transform as RectTransform;
        rectTransform.anchorMin = new Vector2(0f, 0f);
        rectTransform.anchorMax = new Vector2(1f, 1f);
        rectTransform.pivot = new Vector2(0.5f, 0.5f);
        ResolutionInfo resolutionInfo = ResolutionManager.Instance.GetResolutionInfo();
        float num = float.Parse(resolutionInfo.Width);
        float x = this.checkRectTrTarget.sizeDelta.x;
        Vector3 zero = Vector3.zero;
        switch (this.layoutType)
        {
            case HorizontalLayoutType.Left:
                zero = new Vector3((x - num) / 2f + this.layoutOffset, 0f, 0f);
                break;
            case HorizontalLayoutType.Right:
                zero = new Vector3((num - x) / 2f + this.layoutOffset, 0f, 0f);
                break;
        }
        rectTransform.anchoredPosition3D = zero;
    }

    private void Start()
    {
        this.SetInitPos();
        this.mCanvas = UIManager.Instance.UIRoot.GetComponent<Canvas>();
    }

    private void obj_panel_on_pointerdown(PointerEventData eventdata)
    {
        this.mRootPanel.transform.SetAsLastSibling();
        ManagerCenter.Instance.GetManager<EscManager>().SetUIFront(this.mRootPanel.name);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        this.mStartPos = this.GetMousePosition(eventData);
        this.mDragStartPos = this.mRootPanel.transform.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        this.mRootPanel.transform.position = this.mDragStartPos + this.GetMousePosition(eventData) - this.mStartPos;
        if (this.checkRectTrTarget && this.mRootPanel && this.mCanvas)
        {
            this.mRootPanel.GetComponent<RectTransform>().anchoredPosition = this.CorrectPosToTargetRectRange(this.checkRectTrTarget, this.mRootPanel.GetComponent<RectTransform>(), this.mCanvas.GetComponent<RectTransform>());
        }
        this.curDraging = true;
        if (this.mRootPanel.GetComponent<CanvasGroup>())
        {
            this.mRootPanel.GetComponent<CanvasGroup>().alpha = 0.5f;
        }
    }

    private Vector2 CorrectPosToTargetRectRange(RectTransform checkSizeTarget, RectTransform rootControllTarget, RectTransform inRangeRect)
    {
        Vector2 one = Vector2.one;
        RectTransform rectTransform = checkSizeTarget;
        while (rectTransform && rectTransform != inRangeRect)
        {
            one.x *= rectTransform.localScale.x;
            one.y *= rectTransform.localScale.y;
            rectTransform = rectTransform.parent.GetComponent<RectTransform>();
        }
        Vector2 sizeDelta = inRangeRect.sizeDelta;
        Vector2 sizeDelta2 = checkSizeTarget.sizeDelta;
        sizeDelta2.x *= one.x;
        sizeDelta2.y *= one.y;
        Vector2 anchoredPosition = rootControllTarget.anchoredPosition;
        Vector2 pivot = checkSizeTarget.pivot;
        anchoredPosition.y = Mathf.Min(anchoredPosition.y, sizeDelta.y / 2f - sizeDelta2.y * (1f - pivot.y));
        anchoredPosition.y = Mathf.Max(anchoredPosition.y, -sizeDelta.y / 2f - sizeDelta2.y * pivot.y / 3f);
        anchoredPosition.x = Mathf.Max(anchoredPosition.x, -sizeDelta.x / 2f - sizeDelta2.x * pivot.x / 3f);
        anchoredPosition.x = Mathf.Min(anchoredPosition.x, sizeDelta.x / 2f + sizeDelta2.x * (1f - pivot.x) / 3f);
        return anchoredPosition + inRangeRect.anchoredPosition;
    }

    private Vector2 GetMousePosition(PointerEventData eventData)
    {
        Vector3 vector;
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(this.mCanvas.transform as RectTransform, eventData.position, eventData.pressEventCamera, out vector))
        {
            return new Vector2(vector.x, vector.y);
        }
        return Vector2.zero;
    }

    public static T FindInParents<T>(GameObject go) where T : Component
    {
        if (go == null)
        {
            return (T)((object)null);
        }
        T component = go.GetComponent<T>();
        if (component != null)
        {
            return component;
        }
        Transform parent = go.transform.parent;
        while (parent != null && component == null)
        {
            component = parent.gameObject.GetComponent<T>();
            parent = parent.parent;
        }
        return component;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        this.curDraging = false;
        if (this.mRootPanel.GetComponent<CanvasGroup>())
        {
            this.mRootPanel.GetComponent<CanvasGroup>().alpha = 1f;
        }
    }

    public GameObject mRootPanel;

    public RectTransform checkRectTrTarget;

    public HorizontalLayoutType layoutType = HorizontalLayoutType.Center;

    public float layoutOffset;

    private Canvas mCanvas;

    private Vector2 mStartPos;

    private Vector2 mDragStartPos;

    public bool curDraging;
}
