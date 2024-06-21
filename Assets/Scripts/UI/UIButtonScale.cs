using System;
using UnityEngine;
using UnityEngine.EventSystems;

[AddComponentMenu("NGUI/Interaction/Button Scale")]
public class UIButtonScale : MonoBehaviour, IEventSystemHandler, IPointerDownHandler, IPointerUpHandler
{
    private void Start()
    {
        if (!this.mStarted)
        {
            this.mStarted = true;
            if (this.tweenTarget == null)
            {
                this.tweenTarget = base.transform;
            }
            this.mScale = this.tweenTarget.localScale;
        }
    }

    private void OnEnable()
    {
        if (this.mStarted)
        {
            this.OnPointerUp(null);
        }
    }

    private void OnDisable()
    {
        if (this.mStarted && this.tweenTarget != null)
        {
            TweenScale component = this.tweenTarget.GetComponent<TweenScale>();
            if (component != null)
            {
                component.scale = this.mScale;
                component.enabled = false;
            }
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (base.enabled)
        {
            if (!this.mStarted)
            {
                this.Start();
            }
            TweenScale.Begin(this.tweenTarget.gameObject, this.duration, this.mScale).method = UITweener.Method.EaseInOutSin;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (base.enabled)
        {
            if (!this.mStarted)
            {
                this.Start();
            }
            TweenScale.Begin(this.tweenTarget.gameObject, this.duration, Vector3.Scale(this.mScale, this.pressed)).method = UITweener.Method.EaseInOutSin;
        }
    }

    public Transform tweenTarget;

    public Vector3 pressed = new Vector3(1.05f, 1.05f, 1.05f);

    public float duration = 0.2f;

    private Vector3 mScale;

    private bool mStarted;
}
