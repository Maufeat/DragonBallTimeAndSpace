using System;
using Framework.Managers;
using UnityEngine;

public class FollowTeamLeader : IFFComponent
{
    public bool FollowTeamLeaderOn
    {
        get
        {
            return this._FollowTeamLeaderOn;
        }
        set
        {
            if (this._FollowTeamLeaderOn != value)
            {
                this._FollowTeamLeaderOn = value;
                if (this._FollowTeamLeaderOn)
                {
                    this.GetTeamLeaderPos();
                    this.followStartTime = (ulong)SingletonForMono<GameTime>.Instance.GetCurrServerTimeBySecond();
                }
            }
        }
    }

    public void FindPathToleader(uint x, uint y)
    {
        float field_Float = LuaConfigManager.GetXmlConfigTable("team").GetField_Float("follow_leader_distance");
        float field_Float2 = LuaConfigManager.GetXmlConfigTable("team").GetField_Float("follow_leader_distance2");
        Vector2 a = new Vector2(x, y);
        Vector2 currentPosition2D = MainPlayer.Self.CurrentPosition2D;
        CopyManager manager = ManagerCenter.Instance.GetManager<CopyManager>();
        if (manager.InCopy)
        {
            if (field_Float > Vector2.Distance(a, currentPosition2D))
            {
                return;
            }
        }
        else if (field_Float2 > Vector2.Distance(a, currentPosition2D))
        {
            return;
        }
        if (MainPlayer.Self != null && MainPlayer.Self.Pfc != null)
        {
            this._farFromLeader = true;
            MainPlayer.Self.Pfc.PathFindOfDeviation(new Vector2(x, y), delegate
            {
                this._farFromLeader = false;
            });
        }
    }

    public CompnentState State { get; set; }

    public void CompAwake(FFComponentMgr Mgr)
    {
    }

    public void CompDispose()
    {
    }

    public void ResetComp()
    {
    }

    public void CompUpdate()
    {
        if (!this.FollowTeamLeaderOn)
        {
            return;
        }
        if ((ulong)SingletonForMono<GameTime>.Instance.GetCurrServerTimeBySecond() - this.followStartTime > this.followWaitTime)
        {
            this.GetTeamLeaderPos();
            this.followStartTime = (ulong)SingletonForMono<GameTime>.Instance.GetCurrServerTimeBySecond();
        }
    }

    public bool FarFromLeader
    {
        get
        {
            return this._farFromLeader;
        }
    }

    public void GetTeamLeaderPos()
    {
        TeamController controller = ControllerManager.Instance.GetController<TeamController>();
        if (controller.myTeamInfo != null && controller.myTeamInfo.id != 0U)
        {
            EntitiesManager manager = ManagerCenter.Instance.GetManager<EntitiesManager>();
            ulong charid = 0UL;
            ulong.TryParse(controller.myTeamInfo.leaderid, out charid);
            OtherPlayer playerByID = manager.GetPlayerByID(charid);
            if (playerByID != null)
            {
                if (MainPlayer.Self.GetComponent<FollowTeamLeader>() != null)
                {
                    MainPlayer.Self.GetComponent<FollowTeamLeader>().FindPathToleader((uint)playerByID.CurrentPosition2D.x, (uint)playerByID.CurrentPosition2D.y);
                }
                return;
            }
        }
        if (ControllerManager.Instance.GetController<TeamController>() != null)
        {
            ControllerManager.Instance.GetController<TeamController>().MSG_ReqLeaderMapPos_CS();
        }
    }

    private bool _FollowTeamLeaderOn;

    private float followWaitTime = 4f;

    private ulong followStartTime;

    private Vector2 oldTeamLeadetpos = Vector2.zero;

    private bool _farFromLeader;
}
