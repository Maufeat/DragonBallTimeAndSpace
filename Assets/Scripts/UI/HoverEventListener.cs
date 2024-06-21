using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoverEventListener : MonoBehaviour, IEventSystemHandler, IPointerEnterHandler, IPointerExitHandler
{
    public static HoverEventListener Get(GameObject go)
    {
        HoverEventListener hoverEventListener = go.GetComponent<HoverEventListener>();
        if (hoverEventListener == null)
        {
            hoverEventListener = go.AddComponent<HoverEventListener>();
        }
        return hoverEventListener;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (this.onEnter != null)
        {
            this.onEnter(eventData);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (this.onExit != null)
        {
            this.onExit(eventData);
        }
    }

    public HoverEventListener.VoidDelegate onEnter;

    public HoverEventListener.VoidDelegate onExit;

    public delegate void VoidDelegate(PointerEventData eventData);
}
