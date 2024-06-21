using System;
using Framework.Managers;
using LuaInterface;
using UnityEngine;

public class Npc_TaskCollect : Npc
{
    public Npc_TaskCollect()
    {
        this.Init();
    }

    public override void Init()
    {
        base.Init();
        base.AddComponentImmediate(new NpcData());
        base.AddComponentImmediate(new AttackWarningEffect());
    }

    public override void Update()
    {
        base.Update();
    }

    public override void InitComponent()
    {
        base.InitComponent();
        this.ffbc = base.GetComponent<FFMaterialEffectControl>();
        FFMaterialAnimClip clip = ManagerCenter.Instance.GetManager<FFMaterialEffectManager>().GetClip("flash");
        if (clip != null)
        {
            this.eff = new FFMateffect(clip);
        }
    }

    public override void Die()
    {
        base.Die();
    }

    private EntitiesManager mEntitiesManager
    {
        get
        {
            return ManagerCenter.Instance.GetManager<EntitiesManager>();
        }
    }

    public bool CheckStateContainDoing()
    {
        bool flag = false;
        uint baseOrHeroId = base.NpcData.GetBaseOrHeroId();
        if (this.IsCanCollect == null)
        {
            this.IsCanCollect = LuaScriptMgr.Instance.GetLuaFunction("NpcTalkAndTaskDlgCtrl.IsNpcCanCollect");
        }
        if (this.TaskUICtrl == null)
        {
            this.TaskUICtrl = Util.GetLuaTable("NpcTalkAndTaskDlgCtrl");
        }
        if (this.IsCanCollect != null && this.TaskUICtrl != null)
        {
            object[] array = this.IsCanCollect.Call(new object[]
            {
                this.TaskUICtrl,
                baseOrHeroId
            });
            if (array != null && array.Length >= 0)
            {
                flag = bool.Parse(array[0].ToString());
            }
        }
        if (flag)
        {
            if (!this.isHaveEff && this.eff != null)
            {
                this.ffbc.AddEffect(this.eff);
                this.isHaveEff = true;
            }
        }
        else if (this.isHaveEff && this.eff != null)
        {
            this.isHaveEff = false;
            this.ffbc.RemoveEffect(this.eff);
            MeshRenderer component = base.ModelObj.GetComponent<MeshRenderer>();
            if (component != null && component.material != null)
            {
                component.material.SetFloat("_FrssnelValue", 0f);
            }
        }
        return flag;
    }

    private FFMaterialEffectControl ffbc;

    private FFMateffect eff;

    private bool isHaveEff;

    private LuaTable TaskUICtrl;

    private LuaFunction IsCanCollect;
}
