using UnityEngine;
using UnityEngine.EventSystems;

public class UIEventListener : EventTrigger
{
    public UIEventListener.VoidDelegate onClick;
    public UIEventListener.VoidDelegate onDown;
    public UIEventListener.VoidDelegate onEnter;
    public UIEventListener.VoidDelegate onExit;
    public UIEventListener.VoidDelegate onUp;
    public UIEventListener.VoidDelegate onSelect;
    public UIEventListener.VoidDelegate onSubmit;
    public UIEventListener.VoidDelegate onUpdateSelect;
    public UIEventListener.VoidDelegate onBeginDrag;
    public UIEventListener.VoidDelegate onDrag;
    public UIEventListener.VoidDelegate onEndDrag;
    public UIEventListener.VoidDelegate onDrop;
    public UIEventListener.VoidDelegate onScroll;
    public UIEventListener.VoidDelegate onDestroy;
    public object parameter;

    public static UIEventListener Get(GameObject go)
    {
        UIEventListener uiEventListener = go.GetComponent<UIEventListener>();
        if ((Object)uiEventListener == (Object)null)
            uiEventListener = go.AddComponent<UIEventListener>();
        return uiEventListener;
    }

    public void OnDestroy()
    {
        if (this.onDestroy == null)
            return;
        this.onDestroy((PointerEventData)null);
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        if (this.onClick == null)
            return;
        this.onClick(eventData);
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (this.onDown == null)
            return;
        this.onDown(eventData);
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        if (this.onEnter == null)
            return;
        this.onEnter(eventData);
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        if (this.onExit == null)
            return;
        this.onExit(eventData);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        if (this.onUp == null)
            return;
        this.onUp(eventData);
    }

    public override void OnSelect(BaseEventData eventData)
    {
        if (this.onSelect == null)
            return;
        this.onSelect(eventData as PointerEventData);
    }

    public override void OnSubmit(BaseEventData eventData)
    {
        if (this.onSubmit == null)
            return;
        this.onSubmit(eventData as PointerEventData);
    }

    public override void OnUpdateSelected(BaseEventData eventData)
    {
        if (this.onUpdateSelect == null)
            return;
        this.onUpdateSelect(eventData as PointerEventData);
    }

    public override void OnBeginDrag(PointerEventData eventData)
    {
        if (this.onBeginDrag == null)
            return;
        this.onBeginDrag(eventData);
    }

    public override void OnDrag(PointerEventData eventData)
    {
        if (this.onDrag == null)
            return;
        this.onDrag(eventData);
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        if (this.onEndDrag == null)
            return;
        this.onEndDrag(eventData);
    }

    public override void OnDrop(PointerEventData eventData)
    {
        if (this.onDrop == null)
            return;
        this.onDrop(eventData);
    }

    public override void OnScroll(PointerEventData eventData)
    {
        if (this.onScroll == null)
            return;
        this.onScroll(eventData);
    }

    public delegate void VoidDelegate(PointerEventData eventData);
}
