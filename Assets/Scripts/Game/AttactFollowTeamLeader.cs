using System;
using Framework.Managers;
using UnityEngine;

public class AttactFollowTeamLeader : AutoAttack
{
    public override bool AutoAttackOn
    {
        get
        {
            return this._AutoAttackOn;
        }
        set
        {
            if (this._AutoAttackOn != value)
            {
                this._AutoAttackOn = value;
                if (MainPlayer.Self.GetComponent<FollowTeamLeader>() != null)
                {
                    MainPlayer.Self.GetComponent<FollowTeamLeader>().FollowTeamLeaderOn = value;
                }
                if (this._AutoAttackOn)
                {
                    CopyManager manager = ManagerCenter.Instance.GetManager<CopyManager>();
                    if (!manager.InCopy)
                    {
                        return;
                    }
                    if (MainPlayer.Self.GetComponent<MainPlayerTargetSelectMgr>() != null)
                    {
                        MainPlayer.Self.GetComponent<MainPlayerTargetSelectMgr>().attackFollowTargetSelect.ReqTarget();
                    }
                }
                else
                {
                    MainPlayer.Self.StopMoveImmediateWithOutSetPos(null);
                }
            }
        }
    }

    public override void SwitchModle(bool isAutoBattle)
    {
        this.AutoAttackOn = isAutoBattle;
        base.SetUI();
    }

    private bool IsFarFromLeader()
    {
        return MainPlayer.Self.GetComponent<FollowTeamLeader>().FarFromLeader;
    }

    public override void MakeDecide()
    {
        if (base.IsOnPlayerControl())
        {
            return;
        }
        if (this.IsFarFromLeader())
        {
            return;
        }
        CopyManager manager = ManagerCenter.Instance.GetManager<CopyManager>();
        if (!manager.InCopy)
        {
            return;
        }
        if (!MainPlayer.Self.GetComponent<MainPlayerTargetSelectMgr>().attackFollowTargetSelect.CheckLegal(base.Target, false))
        {
            MainPlayer.Self.GetComponent<MainPlayerTargetSelectMgr>().attackFollowTargetSelect.ReqTarget();
            this.NextPosition2DOfTargetFinder = Vector2.zero;
            this.NextthinkTime = 3f;
            return;
        }
        TeamController controller = ControllerManager.Instance.GetController<TeamController>();
        if (controller.myTeamInfo != null && controller.myTeamInfo.id != 0U)
        {
            bool flag = controller.myTeamInfo.leaderid.Equals(ManagerCenter.Instance.GetManager<EntitiesManager>().MainPlayer.GetCharID().ToString());
        }
        base.RefreshDisToTarget();
        base.RefreshCurSkill();
        if (base.CheckMoveToTarget())
        {
            return;
        }
        base.CheckMakeAttack();
    }

    private bool _AutoAttackOn;
}
