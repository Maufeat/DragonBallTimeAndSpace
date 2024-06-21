using System;
using Framework.Managers;
using magic;

public class Npc_Trap : Npc
{
    public override void Update()
    {
        base.Update();
        if (this.NeedDestroy)
        {
            FFBehaviourControl component = base.GetComponent<FFBehaviourControl>();
            if (component != null)
            {
                component.ChangeState(ClassPool.GetObject<FFBehaviourState_Skill>());
                if (!(component.CurrState is FFBehaviourState_Skill))
                {
                    this.TrueDestroy();
                }
                else if ((component.CurrState as FFBehaviourState_Skill).CurrSkillClip == null)
                {
                    this.TrueDestroy();
                }
            }
            else
            {
                this.TrueDestroy();
            }
        }
    }

    public override void Die()
    {
    }

    private EntitiesManager mEntitiesManager
    {
        get
        {
            return ManagerCenter.Instance.GetManager<EntitiesManager>();
        }
    }

    public override void HitOther(MSG_Ret_MagicAttack_SC mdata, CharactorBase[] BeNHits)
    {
        if (base.NpcData.MapNpcData == null || base.NpcData.MapNpcData.MasterData == null)
        {
            return;
        }
        if (this.mEntitiesManager.GetCharactorByID(base.NpcData.MapNpcData.MasterData.Eid) == null)
        {
            return;
        }
    }

    public override void DestroyThisInNineScreen()
    {
        FFBehaviourControl component = base.GetComponent<FFBehaviourControl>();
        if (component != null)
        {
            this.NeedDestroy = true;
        }
        else
        {
            this.TrueDestroy();
        }
    }

    private void TrueDestroy()
    {
        base.DestroyThisInNineScreen();
    }

    public override void CreateModelFinishCb()
    {
        base.CreateModelFinishCb();
        if (base.CharState == CharactorState.RemoveFromNineScreen)
        {
            return;
        }
        base.SetPlayerDirection(base.NpcData.MapNpcData.dir, true);
    }

    private bool NeedDestroy;
}
