using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class Icon3DPosQuickSet : MonoBehaviour, IDragHandler, IEventSystemHandler, IPointerEnterHandler, IPointerExitHandler
{
    public void OnDrag(PointerEventData eventData)
    {
        IconRenderCtrl component = base.gameObject.GetComponent<IconRenderCtrl>();
        UI_3DIconModelPosCheck uiobject = UIManager.GetUIObject<UI_3DIconModelPosCheck>();
        if (uiobject != null && component != null)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                component.localPos += new Vector3(eventData.delta.x, eventData.delta.y, 0f) * Time.deltaTime * 0.2f;
                uiobject.SetPos(component.localPos);
            }
            if (eventData.button == PointerEventData.InputButton.Middle)
            {
                uiobject.SetRotY(-eventData.delta.x * 0.5f);
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        this.isEnter = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        this.isEnter = false;
    }

    private void Update()
    {
        if (this.isEnter)
        {
            float axis = Input.GetAxis("Mouse ScrollWheel");
            UI_3DIconModelPosCheck uiobject = UIManager.GetUIObject<UI_3DIconModelPosCheck>();
            if (uiobject != null)
            {
                uiobject.SetPosZ(axis * Time.deltaTime * 5f);
            }
        }
    }

    private bool isEnter;
}
