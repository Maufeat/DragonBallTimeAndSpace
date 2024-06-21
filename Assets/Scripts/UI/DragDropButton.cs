using System;
using Framework.Managers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragDropButton : MonoBehaviour, IDragHandler, IEventSystemHandler, IBeginDragHandler, IPointerDownHandler, IPointerUpHandler
{
    public void SetMaskShow(bool show)
    {
        if (this.mMask)
        {
            this.mMask.gameObject.SetActive(show);
        }
    }

    public void Initilize(UIRootType _uiRootType, Vector2 _pos, string _imgPath = "", DragDropButtonDataBase _data = null)
    {
        this.mUIRootType = _uiRootType;
        this.mPos = _pos;
        this.mData = _data;
        Transform transform = (!string.IsNullOrEmpty(_imgPath)) ? base.transform.Find(_imgPath) : base.transform;
        this.mIcon = transform.GetComponent<Image>();
        if (this.mMask == null)
        {
            this.mMask = UnityEngine.Object.Instantiate<Image>(this.mIcon);
            this.mMask.transform.SetParent(this.mIcon.transform.parent);
            this.mMask.transform.SetAsLastSibling();
            this.mMask.sprite = null;
            this.mMask.overrideSprite = null;
            this.mMask.color = new Color(0f, 0f, 0f, 0.5f);
            this.mMask.transform.localScale = Vector3.one;
            this.mMask.transform.localPosition = Vector3.zero;
            (this.mMask.transform as RectTransform).sizeDelta = (this.mMask.transform.parent as RectTransform).sizeDelta;
            this.mMask.gameObject.SetActive(false);
            this.mMask.name = "DeadMask";
        }
    }

    public bool IsEmptyBtn()
    {
        return this.mData == null;
    }

    public void DataClear()
    {
        this.mData = null;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            DragDropManager.Instance.Use(this);
        }
        else if (DragDropManager.Instance.IsDraging())
        {
            if (eventData.pointerCurrentRaycast.gameObject != null)
            {
                if (this.mUIRootType == UIRootType.Bag && ControllerManager.Instance.GetController<ShopController>().CheckDragInSell(eventData.pointerCurrentRaycast.gameObject))
                {
                    ControllerManager.Instance.GetController<ShopController>().DragInSell(this.mData);
                    DragDropManager.Instance.SetDragStateUI(false);
                    return;
                }
                DragDropButton component = eventData.pointerCurrentRaycast.gameObject.GetComponent<DragDropButton>();
                DragDropManager.Instance.PutIn((!(this == null)) ? component : this);
                return;
            }
            else
            {
                DragDropManager.Instance.obj_mask_on_click(null);
            }
        }
        else
        {
            DragDropManager.Instance.LeftClick(this);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left && !this.IsEmptyBtn())
        {
            DragDropManager.Instance.DragUp(this);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
    }

    public UIRootType mUIRootType;

    public Vector2 mPos;

    public DragDropButtonDataBase mData;

    public Image mIcon;

    private Image mMask;

    private bool mIsPreDrag;

    private float mDownTime;
}
