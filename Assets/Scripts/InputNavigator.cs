using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InputNavigator : MonoBehaviour, IEventSystemHandler, ISelectHandler, IDeselectHandler
{

    private EventSystem system;
    private bool _isSelect;

    private void Start()
    {
        system = EventSystem.current;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            Selectable selectable = null;
            Selectable selectable2 = null;
            if (this.system.currentSelectedGameObject != null && this.system.currentSelectedGameObject.activeInHierarchy)
            {
                selectable2 = this.system.currentSelectedGameObject.GetComponent<Selectable>();
            }
            if (selectable2 != null)
            {
                if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
                {
                    selectable = selectable2.FindSelectableOnUp();
                    if (selectable == null)
                    {
                        selectable = selectable2.FindSelectableOnLeft();
                    }
                }
                else
                {
                    selectable = selectable2.FindSelectableOnDown();
                    if (selectable == null)
                    {
                        selectable = selectable2.FindSelectableOnRight();
                    }
                }
            }
            else if (Selectable.allSelectables.Count > 0)
            {
                selectable = Selectable.allSelectables[0];
            }
            if (selectable != null)
            {
                selectable.Select();
            }
        }
    }

    public void OnSelect(BaseEventData eventData)
    {
        _isSelect = true;
    }

    public void OnDeselect(BaseEventData eventData)
    {
        _isSelect = false;
    }
}
