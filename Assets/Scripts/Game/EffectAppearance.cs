using System;
using System.Collections.Generic;
using Framework.Managers;
using LuaInterface;

public class EffectAppearance : IFFComponent
{
    public CompnentState State { get; set; }

    public void CompAwake(FFComponentMgr Mgr)
    {
        this.Owner = Mgr.Owner;
        this.SetMyEffectAppearance(false);
    }

    public void SetMyEffectAppearance(bool force = false)
    {
        LuaTable appearancenpcConfig = this.Owner.BaseData.GetAppearancenpcConfig();
        FFEffectControl component = this.Owner.GetComponent<FFEffectControl>();
        FFEffectManager manager = ManagerCenter.Instance.GetManager<FFEffectManager>();
        if (component != null && appearancenpcConfig != null)
        {
            string cacheField_String = appearancenpcConfig.GetCacheField_String("effect_appearance");
            if (this.LastEffectAppearance != cacheField_String || force)
            {
                this.ClearFFeffect();
                string[] group = manager.GetGroup(cacheField_String);
                for (int i = 0; i < group.Length; i++)
                {
                    FFeffect item = component.AddEffect(group[i], null, null);
                    this.CurrFFeffect.Add(item);
                }
                this.LastEffectAppearance = cacheField_String;
            }
        }
    }

    private void ClearFFeffect()
    {
        for (int i = 0; i < this.CurrFFeffect.Count; i++)
        {
            this.CurrFFeffect[i].SetEffectOver();
        }
        this.CurrFFeffect.Clear();
    }

    public void CompUpdate()
    {
    }

    public void CompDispose()
    {
        this.ClearFFeffect();
    }

    public void ResetComp()
    {
        this.SetMyEffectAppearance(true);
    }

    public CharactorBase Owner;

    private List<FFeffect> CurrFFeffect = new List<FFeffect>();

    private string LastEffectAppearance = string.Empty;
}
