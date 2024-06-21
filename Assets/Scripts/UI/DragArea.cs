using System;
using Framework.Managers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragArea : MonoBehaviour, IDragHandler, IEventSystemHandler, IBeginDragHandler, IEndDragHandler
{
    public void Initilize(DragAreaFinishCb _cb)
    {
        this.dragAreaFinishCb = _cb;
    }

    private void Start()
    {
        this.m_Canvas = UIManager.FindInParents<Canvas>(base.gameObject);
        this.m_DragPanel = base.transform.parent.Find("Panel").gameObject;
        this.cs = this.m_Canvas.GetComponent<CanvasScaler>();
        HoverEventListener.Get(base.gameObject).onEnter = delegate (PointerEventData pData)
        {
            MouseStateControoler.Instan.SetMoseState(MoseState.m_scale3);
        };
        HoverEventListener.Get(base.gameObject).onExit = delegate (PointerEventData pData)
        {
            if (!this.isDraging)
            {
                MouseStateControoler.Instan.SetMoseState(MoseState.m_default);
            }
        };
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        MouseStateControoler.Instan.SetMoseState(MoseState.m_scale3);
        this.canDragBg.SetActive(true);
        this.bg.SetActive(true);
        this.mStartPos = (base.transform as RectTransform).offsetMax;
        this.mDragStartPos = (this.m_DragPanel.transform as RectTransform).offsetMax;
        if (this.chatChontrol == null)
        {
            this.chatChontrol = ControllerManager.Instance.GetController<ChatControl>();
        }
        this.isDraging = true;
    }

    private void Update()
    {
        if (this.isDraging)
        {
            if (this.tmpDeltaTime > 0.3f)
            {
                this.tmpDeltaTime = 0f;
                if (this.dragAreaFinishCb != null)
                {
                    this.dragAreaFinishCb();
                }
            }
            else
            {
                this.tmpDeltaTime += Time.deltaTime;
            }
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 position;
        if (RectTransformUtility.RectangleContainsScreenPoint(this.canDragBg.transform as RectTransform, eventData.position, eventData.pressEventCamera) && RectTransformUtility.ScreenPointToWorldPointInRectangle(this.canDragBg.transform as RectTransform, eventData.position, eventData.pressEventCamera, out position))
        {
            Vector2 sizeDelta = base.transform.parent.GetComponent<RectTransform>().sizeDelta;
            Vector2 sizeDelta2 = this.m_Canvas.GetComponent<RectTransform>().sizeDelta;
            Vector2 vector = sizeDelta2 - sizeDelta;
            base.transform.position = position;
            float x = (base.transform as RectTransform).offsetMax.x - this.mStartPos.x;
            float y = (base.transform as RectTransform).offsetMax.y - this.mStartPos.y;
            Vector2 offsetMax = this.mDragStartPos + new Vector2(x, y);
            (this.m_DragPanel.transform as RectTransform).offsetMax = offsetMax;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        MouseStateControoler.Instan.SetMoseState(MoseState.m_default);
        this.canDragBg.SetActive(false);
        this.bg.SetActive(false);
        if (this.dragAreaFinishCb != null)
        {
            this.dragAreaFinishCb();
        }
        this.isDraging = false;
    }

    public GameObject bg;

    public GameObject canDragBg;

    private Canvas m_Canvas;

    private GameObject m_DragPanel;

    private CanvasScaler cs;

    private Vector2 mStartPos;

    private Vector2 mDragStartPos;

    public ChatControl chatChontrol;

    private DragAreaFinishCb dragAreaFinishCb;

    private bool isDraging;

    private float tmpDeltaTime;
}

public delegate void DragAreaFinishCb();
