using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIButtonState : MonoBehaviour
{
    private void Start()
    {
        UIEventListener uieventListener = UIEventListener.Get(base.gameObject);
        uieventListener.onEnter = (UIEventListener.VoidDelegate)Delegate.Combine(uieventListener.onEnter, new UIEventListener.VoidDelegate(this.OnPointerEnter));
        UIEventListener uieventListener2 = UIEventListener.Get(base.gameObject);
        uieventListener2.onExit = (UIEventListener.VoidDelegate)Delegate.Combine(uieventListener2.onExit, new UIEventListener.VoidDelegate(this.OnPointerExit));
        UIEventListener.Get(base.gameObject).onDown = new UIEventListener.VoidDelegate(this.OnPointerDown);
        UIEventListener.Get(base.gameObject).onUp = new UIEventListener.VoidDelegate(this.OnPointerUp);
        this.btn = base.gameObject.GetComponent<Button>();
        if (this.btn != null)
        {
            this.btnInterractState = this.btn.interactable;
            this.OnBtnInteratableStateChange(this.btnInterractState);
        }
    }

    private void Update()
    {
        if (this.btn != null && this.btnInterractState != this.btn.interactable)
        {
            this.btnInterractState = this.btn.interactable;
            this.OnBtnInteratableStateChange(this.btnInterractState);
        }
    }

    private void OnBtnInteratableStateChange(bool state)
    {
        if (this.btnInteracteOn != null && this.btnInteracteOn.Length > 0)
        {
            for (int i = 0; i < this.btnInteracteOn.Length; i++)
            {
                if (this.btnInteracteOn[i] != null)
                {
                    this.btnInteracteOn[i].SetActive(state);
                }
            }
        }
        if (this.btnInteracteOff != null && this.btnInteracteOff.Length > 0)
        {
            for (int j = 0; j < this.btnInteracteOff.Length; j++)
            {
                if (this.btnInteracteOff[j] != null)
                {
                    this.btnInteracteOff[j].SetActive(!state);
                }
            }
        }
    }

    private void HideAllState()
    {
        if (this.mNormal)
        {
            this.mNormal.SetActive(false);
        }
        if (this.mHover)
        {
            this.mHover.SetActive(false);
        }
        if (this.mPressed)
        {
            this.mPressed.SetActive(false);
        }
        if (this.mDisabled)
        {
            this.mDisabled.SetActive(false);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (this.btn != null && !this.btn.interactable)
        {
            return;
        }
        this.HideAllState();
        if (this.mHover)
        {
            this.mHover.SetActive(true);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (this.btn != null && !this.btn.interactable)
        {
            return;
        }
        this.HideAllState();
        if (this.mNormal)
        {
            this.mNormal.SetActive(true);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (this.btn != null && !this.btn.interactable)
        {
            return;
        }
        this.HideAllState();
        if (this.mPressed)
        {
            this.mPressed.SetActive(true);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (this.btn != null && !this.btn.interactable)
        {
            return;
        }
        this.HideAllState();
        if (this.mNormal)
        {
            this.mNormal.SetActive(true);
        }
    }

    public GameObject mNormal;

    public GameObject mHover;

    public GameObject mPressed;

    public GameObject mDisabled;

    [Tooltip("按钮为正常状态下控制开关的物体，可以控制多个")]
    public GameObject[] btnInteracteOn;

    [Tooltip("按钮为禁用状态下控制开关的物体，可以控制多个")]
    public GameObject[] btnInteracteOff;

    private bool btnInterractState = true;

    private Button btn;
}
