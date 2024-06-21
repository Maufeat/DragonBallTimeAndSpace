using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class LongPressOrClickEventTrigger : UIBehaviour, IPointerClickHandler, IEventSystemHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    public static LongPressOrClickEventTrigger Get(GameObject go)
    {
        LongPressOrClickEventTrigger longPressOrClickEventTrigger = go.GetComponent<LongPressOrClickEventTrigger>();
        if (longPressOrClickEventTrigger == null)
        {
            longPressOrClickEventTrigger = go.AddComponent<LongPressOrClickEventTrigger>();
        }
        return longPressOrClickEventTrigger;
    }

    private void Update()
    {
        if (this.isPointerDown && !this.longPressTriggered && Time.time - this.timePressStarted > this.durationThreshold)
        {
            this.longPressTriggered = true;
            if (this.onLongPress != null)
            {
                this.onLongPress();
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        this.timePressStarted = Time.time;
        this.isPointerDown = true;
        this.longPressTriggered = false;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        this.isPointerDown = false;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        this.isPointerDown = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!this.longPressTriggered && this.onClick != null)
        {
            this.onClick(eventData);
        }
    }

    [Tooltip("How long must pointer be down on this object to trigger a long press")]
    public float durationThreshold = 1f;

    public LongPressOrClickEventTrigger.VoidDelegate onClick;

    public LongPressOrClickEventTrigger.VoidDelegateVoid onLongPress;

    private bool isPointerDown;

    private bool longPressTriggered;

    private float timePressStarted;

    public delegate void VoidDelegate(PointerEventData eventData);

    public delegate void VoidDelegateVoid();
}
