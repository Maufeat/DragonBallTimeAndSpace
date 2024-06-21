using System;
using DG.Tweening;
using UnityEngine;

public class TweenScaleAlpha : MonoBehaviour
{
    private void Awake()
    {
        this.canvasGroup = base.GetComponent<CanvasGroup>();
        if (this.canvasGroup == null)
        {
            this.canvasGroup = base.gameObject.AddComponent<CanvasGroup>();
        }
        this.canvasGroup.alpha = 0f;
        base.transform.localScale = Vector3.zero;
    }

    private void Start()
    {
        base.transform.DOScale(Vector3.one, this.durtaion);
        this.canvasGroup.DOFade(1f, this.durtaion);
    }

    private void Update()
    {
    }

    public float durtaion = 1f;

    private CanvasGroup canvasGroup;
}
