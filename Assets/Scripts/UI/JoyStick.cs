using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class JoyStick : MonoBehaviour
{
    private void Start()
    {
        this.realRectTransform = this.RealSprite.GetComponent<RectTransform>();
        this.fakeRectTransform = this.FakeSprite.GetComponent<RectTransform>();
        UIEventListener.Get(this.RealSprite).onEndDrag = new UIEventListener.VoidDelegate(this.OnEndDrag);
        UIEventListener.Get(this.RealSprite).onDown = new UIEventListener.VoidDelegate(this.OnDrag);
        UIEventListener.Get(this.RealSprite).onUp = new UIEventListener.VoidDelegate(this.OnEndDrag);
        UIEventListener.Get(this.RealSprite).onDrag = new UIEventListener.VoidDelegate(this.OnDrag);
        UIEventListener.Get(this.RealSprite).onBeginDrag = new UIEventListener.VoidDelegate(this.OnBeginDrag);
        this.OnJoyStart();
    }

    private void SetDragState(bool state)
    {
        this.isDraging = state;
        this.OnJoySetDragState(this.isDraging);
    }

    private void OnEndDrag(PointerEventData eventData)
    {
        if (eventData == null)
        {
            return;
        }
        eventData.pointerDrag = null;
        eventData.pointerPress = null;
        this.realRectTransform.localPosition = Vector3.zero;
        this.fakeRectTransform.localPosition = Vector3.zero;
        if (this.ProcessInput != null)
        {
            this.ProcessInput(Vector2.zero);
        }
        this.SetDragState(false);
    }

    private void OnBeginDrag(PointerEventData eventData)
    {
        this.dragObj = eventData;
        this.SetDragState(true);
    }

    private void OnDrag(PointerEventData eventData)
    {
        this.SetDragState(true);
        Vector3 position;
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(this.realRectTransform, eventData.position, eventData.pressEventCamera, out position))
        {
            this.realRectTransform.position = position;
        }
        float num = Vector3.Distance(this.RealSprite.transform.position, base.transform.position);
        Vector3 normalized = (this.RealSprite.transform.position - base.transform.position).normalized;
        if (num <= this.Range)
        {
            this.FakeSprite.transform.position = this.RealSprite.transform.position;
        }
        else
        {
            Vector3 b = normalized * this.Range;
            this.FakeSprite.transform.position = base.transform.position + b;
        }
    }

    public void OnVirtualDrag(Vector2 dir)
    {
        if (this.OnJoyVirtulDragReturn(dir))
        {
            this.realRectTransform.localPosition = Vector3.zero;
            this.fakeRectTransform.localPosition = Vector3.zero;
            return;
        }
        Vector3 b = dir.normalized * this.Range;
        this.fakeRectTransform.transform.position = base.transform.position + b;
        this.realRectTransform.position = this.fakeRectTransform.transform.position;
    }

    private void Update()
    {
        if (this.isDraging && this.ProcessInput != null && this.RealSprite != null)
        {
            Vector3 normalized = (this.RealSprite.transform.position - base.transform.position).normalized;
            Vector2 vector = new Vector2(normalized.x, normalized.y);
            this.ProcessInput(vector.normalized);
        }
    }

    public void ResetJoystick()
    {
        this.OnEndDrag(this.dragObj);
        EventSystem.current.SetSelectedGameObject(null, null);
        if (this.ProcessInput != null)
        {
            this.ProcessInput(Vector2.zero);
        }
    }

    public GameObject RealSprite;

    public GameObject FakeSprite;

    public float Range = 80f;

    private RectTransform realRectTransform;

    private RectTransform fakeRectTransform;

    public JoyStick.ProcessInputHandle ProcessInput;

    private PointerEventData dragObj;

    private bool isDraging;

    public Action OnJoyStart = delegate ()
    {
    };

    public Action<bool> OnJoySetDragState = delegate (bool state)
    {
    };

    public JoyStick.OnJoyVirtulDrag OnJoyVirtulDragReturn = (Vector2 dir) => false;

    public delegate void ProcessInputHandle(Vector2 dir);

    public delegate bool OnJoyVirtulDrag(Vector2 dir);
}
