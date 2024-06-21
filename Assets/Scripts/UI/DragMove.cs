using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragMove : MonoBehaviour, IDragHandler, IEventSystemHandler, IBeginDragHandler
{
    private void Start()
    {
        this.mCanvas = UIManager.FindInParents<Canvas>(base.gameObject);
    }

    private void OnEnable()
    {
        this.mMaxOffset = ((base.transform as RectTransform).sizeDelta.x - (base.transform.parent as RectTransform).sizeDelta.x) / 2f;
        this.mMaxOffset = Mathf.Abs(this.mMaxOffset);
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

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            this.mStartPos = this.GetMousePosition(eventData);
            this.mDragStartPos = base.transform.position;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            Vector3 position = base.transform.position;
            base.transform.position = this.mDragStartPos + this.GetMousePosition(eventData) - this.mStartPos;
            Vector2 anchoredPosition = (base.transform as RectTransform).anchoredPosition;
            if (Math.Abs(anchoredPosition.x) > this.mMaxOffset)
            {
                base.transform.position = new Vector3(position.x, base.transform.position.y, 0f);
            }
            if (Math.Abs(anchoredPosition.y) > this.mMaxOffset)
            {
                base.transform.position = new Vector3(base.transform.position.x, position.y, 0f);
            }
        }
    }

    private Canvas mCanvas;

    private Vector2 mStartPos;

    private Vector2 mDragStartPos;

    private float mMaxOffset;
}
