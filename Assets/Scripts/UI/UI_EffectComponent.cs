using System;
using UnityEngine;

public class UI_EffectComponent : MonoBehaviour
{
    public void InitEffect()
    {
        if (this._isInit)
        {
            return;
        }
        this._isInit = true;
        this.tweens = base.GetComponentsInChildren<UITweener>();
    }

    private void Awake()
    {
        this.InitEffect();
    }

    public void ShowEffect()
    {
        if (this.tweens == null || this.tweens.Length < 1)
        {
            return;
        }
        base.gameObject.SetActive(true);
        for (int i = 0; i < this.tweens.Length; i++)
        {
            UITweener uitweener = this.tweens[i];
            uitweener.Reset();
        }
        for (int j = 0; j < this.tweens.Length; j++)
        {
            UITweener uitweener2 = this.tweens[j];
            if (uitweener2.IsFirstOne)
            {
                uitweener2.Reset();
            }
            uitweener2.Play(true);
        }
    }

    private UITweener[] tweens;

    private bool _isInit;
}
