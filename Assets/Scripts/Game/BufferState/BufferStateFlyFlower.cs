using System;
using UnityEngine;

public class BufferStateFlyFlower : BufferState
{
    public BufferStateFlyFlower(UserState Flag) : base(Flag)
    {
        this.hangPoint = "top";
    }

    public override void Enter(PlayerBufferControl PBControl)
    {
        base.Enter(PBControl);
        this.InitConfigData();
    }

    private void InitConfigData()
    {
        if (base.CurrBuffConfig != null)
        {
            this.effectName = string.Empty;
            uint cacheField_Uint = base.CurrBuffConfig.GetCacheField_Uint("IconShowType");
            if (cacheField_Uint == 3U)
            {
                this.effectName = base.CurrBuffConfig.GetField_String("BuffEffect");
            }
            FFEffectControl component = this.BufferControl.Owner.GetComponent<FFEffectControl>();
            Transform parentTran = this.BufferControl.Owner.ModelObj.transform.Find(this.hangPoint);
            if (component != null && !string.IsNullOrEmpty(this.effectName))
            {
                this.ffectct = component.AddEffect(this.effectName, parentTran, null);
            }
        }
    }

    public override void Exit()
    {
        base.Exit();
        if (this.ffectct != null && this.ffectct.effobj != null)
        {
            UnityEngine.Object.Destroy(this.ffectct.effobj);
        }
    }

    public string effectName;

    public string hangPoint;

    public FFeffect ffectct;
}
