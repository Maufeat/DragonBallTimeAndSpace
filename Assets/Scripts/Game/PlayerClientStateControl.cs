using System;
using Framework.Managers;
using UnityEngine;

public class PlayerClientStateControl : IFFComponent
{
    public GameObject Root
    {
        get
        {
            return this.FFCompMgr.Owner.ModelObj;
        }
    }

    public CompnentState State { get; set; }

    public FFBipBindMgr mBipBindMgr
    {
        get
        {
            return this.FFCompMgr.GetComponent<FFBipBindMgr>();
        }
    }

    private EntitiesManager entitiesMgr
    {
        get
        {
            return ManagerCenter.Instance.GetManager<EntitiesManager>();
        }
    }

    public void CompAwake(FFComponentMgr Mgr)
    {
        this.FFCompMgr = Mgr;
    }

    public void CompUpdate()
    {
        if (this.FFCompMgr.Owner == null)
        {
            return;
        }
        if (this.FFCompMgr.Owner is MainPlayer)
        {
            return;
        }
        CopyManager manager = ManagerCenter.Instance.GetManager<CopyManager>();
        if (manager == null)
        {
            return;
        }
        if (!manager.InCompetitionCopy)
        {
            return;
        }
        if (this.entitiesMgr.PlayerStateInCompetition != this.mPlayerState)
        {
            this.SetPlayerState(this.entitiesMgr.PlayerStateInCompetition);
        }
    }

    private void InitRenderAndCollider()
    {
        if (this.Root == null)
        {
            return;
        }
        if (this._isInit)
        {
            return;
        }
        this._isInit = true;
        this.listNoteRender = this.Root.GetComponentsInChildren<Renderer>(true);
        this.listNoteCollider = this.Root.GetComponentsInChildren<Collider>(true);
    }

    private void noteAndDisactiveRender()
    {
        if (this.Root == null)
        {
            return;
        }
        this.InitRenderAndCollider();
        if (this.listNoteRender != null)
        {
            for (int i = 0; i < this.listNoteRender.Length; i++)
            {
                if (null != this.listNoteRender[i])
                {
                    this.listNoteRender[i].enabled = false;
                }
            }
        }
        if (this.listNoteCollider != null)
        {
            for (int j = 0; j < this.listNoteCollider.Length; j++)
            {
                if (null != this.listNoteCollider[j])
                {
                    this.listNoteCollider[j].enabled = false;
                }
            }
        }
    }

    private void activeRender()
    {
        if (this.Root == null)
        {
            return;
        }
        this.InitRenderAndCollider();
        if (this.listNoteRender != null)
        {
            for (int i = 0; i < this.listNoteRender.Length; i++)
            {
                if (this.listNoteRender[i] != null)
                {
                    this.listNoteRender[i].enabled = true;
                }
            }
        }
        if (this.listNoteCollider != null)
        {
            for (int j = 0; j < this.listNoteCollider.Length; j++)
            {
                this.listNoteCollider[j].enabled = true;
            }
        }
    }

    private void CheckSetTargetNull()
    {
        try
        {
            MainPlayerTargetSelectMgr component = MainPlayer.Self.GetComponent<MainPlayerTargetSelectMgr>();
            if (component != null && this.FFCompMgr != null && component.TargetCharactor == this.FFCompMgr.Owner)
            {
                component.SetTargetNull();
            }
        }
        catch (Exception)
        {
            throw;
        }
    }

    public void SetPlayerState(PlayerShowState state)
    {
        if (this.mPlayerState == state)
        {
            return;
        }
        this.mPlayerState = state;
        bool flag = GameSystemSettings.IsHideNpc();
        if (this.mPlayerState == PlayerShowState.show)
        {
            if (this.FFCompMgr.Owner is OtherPlayer)
            {
                this.activeRender();
                this.ShowPlayerEffect();
                this.ActiveShowPlayerUIInfo(true, false);
            }
            else if (this.FFCompMgr.Owner is Npc)
            {
                Npc npc = this.FFCompMgr.Owner as Npc;
                bool flag2 = npc.CheckIsShowByTask(false);
                if (flag2)
                {
                    if (!flag)
                    {
                        this.activeRender();
                        this.ShowPlayerEffect();
                        npc.onAddBehaviourControl();
                    }
                    else if (npc.NpcData.MapNpcData.NpcType == NpcType.NPC_TYPE_VISIT)
                    {
                        this.noteAndDisactiveRender();
                        this.HidePlayerEffect();
                    }
                    this.ActiveShowPlayerUIInfo(true, true);
                }
                else
                {
                    this.CheckSetTargetNull();
                }
            }
        }
        else if (this.FFCompMgr.Owner is OtherPlayer)
        {
            this.noteAndDisactiveRender();
            this.HidePlayerEffect();
            this.ActiveShowPlayerUIInfo(false, false);
        }
        else if (this.FFCompMgr.Owner is Npc)
        {
            this.noteAndDisactiveRender();
            this.HidePlayerEffect();
            Npc npc2 = this.FFCompMgr.Owner as Npc;
            bool flag3 = npc2.CheckIsShowByTask(false);
            if (flag3)
            {
                this.ActiveShowPlayerUIInfo(true, true);
            }
            else
            {
                this.ActiveShowPlayerUIInfo(false, true);
                this.CheckSetTargetNull();
            }
        }
    }

    public void ActiveShowPlayerUIInfo(bool bActive, bool isNpc)
    {
        if (this.FFCompMgr.Owner.hpdata == null)
        {
            return;
        }
        if (isNpc)
        {
            if (bActive)
            {
                this.FFCompMgr.Owner.hpdata.ActiveInfo();
            }
            else
            {
                this.FFCompMgr.Owner.hpdata.DisActiveInfo();
            }
        }
        else if (GameSystemSettings.IsMainPlayerInBattleState())
        {
            this.FFCompMgr.Owner.hpdata.ActiveInfo();
        }
        else if (bActive && !GameSystemSettings.IsHidePlayerName() && this.mPlayerState == PlayerShowState.show)
        {
            this.FFCompMgr.Owner.hpdata.ActiveInfo();
        }
        else
        {
            this.FFCompMgr.Owner.hpdata.DisActiveInfo();
        }
    }

    private void ShowPlayerEffect()
    {
        if (this.FFCompMgr.Owner.GetComponent<FFEffectControl>() == null)
        {
            this.FFCompMgr.Owner.AddComponent(new FFEffectControl());
        }
    }

    private void HidePlayerEffect()
    {
        if (this.FFCompMgr.Owner.GetComponent<FFEffectControl>() != null)
        {
            this.FFCompMgr.Owner.RemoveComponent(this.FFCompMgr.Owner.GetComponent<FFEffectControl>());
        }
    }

    public void CompDispose()
    {
        this.activeRender();
        this.ShowPlayerEffect();
        if (this.FFCompMgr.Owner is Npc)
        {
            this.ActiveShowPlayerUIInfo(true, true);
        }
        else
        {
            this.ActiveShowPlayerUIInfo(true, false);
        }
    }

    public void ResetComp()
    {
        this._isInit = false;
        this.activeRender();
        this.ShowPlayerEffect();
        if (this.FFCompMgr.Owner is Npc)
        {
            this.ActiveShowPlayerUIInfo(true, true);
            Npc npc = this.FFCompMgr.Owner as Npc;
            if (!npc.CheckIsShowByTask(false))
            {
                this.noteAndDisactiveRender();
                this.ActiveShowPlayerUIInfo(false, true);
            }
        }
        else
        {
            this.ActiveShowPlayerUIInfo(true, false);
        }
    }

    public FFComponentMgr FFCompMgr;

    public PlayerShowState mPlayerState;

    private Renderer[] listNoteRender;

    private Collider[] listNoteCollider;

    private bool _isInit;
}
