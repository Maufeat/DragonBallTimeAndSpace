using System;
using System.Collections.Generic;
using Framework.Managers;
using UnityEngine;

public class robot
{
    private MainPlayer mine
    {
        get
        {
            return MainPlayer.Self;
        }
    }

    public void Init()
    {
        Scheduler.Instance.AddUpdator(new Scheduler.OnScheduler(this.Update));
    }

    public void SetTarget(string name)
    {
        this.NextPosition2DOfTargetFinder = Vector2.zero;
        ManagerCenter.Instance.GetManager<EntitiesManager>().CurrentNineScreenPlayers.BetterForeach(delegate (int index, KeyValuePair<ulong, OtherPlayer> item)
        {
            OtherPlayer value = item.Value;
            if (value.OtherPlayerData.MapUserData.name.Contains(name))
            {
                this.Target = value;
                this.targetName = value.OtherPlayerData.MapUserData.name;
                return false;
            }
            return true;
        });
        ManagerCenter.Instance.GetManager<EntitiesManager>().NpcList.BetterForeach(delegate (int index, KeyValuePair<ulong, Npc> item)
        {
            Npc value = item.Value;
            if (value.NpcData.MapNpcData.name.Contains(name))
            {
                this.Target = value;
                this.targetName = value.NpcData.MapNpcData.name;
                return false;
            }
            return true;
        });
        if (string.IsNullOrEmpty(this.targetName))
        {
            this.targetName = name;
        }
    }

    public void Update()
    {
        this.runningtime += Time.deltaTime;
        if (this.runningtime < this.NextthinkTime)
        {
            return;
        }
        this.runningtime = 0f;
        if (MainPlayer.Self == null)
        {
            return;
        }
        if (SingletonForMono<InputController>.Instance.InputDir != -1)
        {
            this.NextthinkTime = 0.8f;
            return;
        }
        if (MainPlayer.Self.GetComponent<PlayerBufferControl>().ContainsState(UserState.USTATE_DEATH))
        {
            ManagerCenter.Instance.GetManager<EntitiesManager>().PNetWork.ReqRelive(false);
        }
        if (this.Target != null)
        {
            if (this.Target.ModelObj == null)
            {
                this.SetTarget(this.targetName);
                return;
            }
            Vector3 vector = this.Target.ModelObj.transform.position - MainPlayer.Self.ModelObj.transform.position;
            float num = Vector3.Distance(this.Target.ModelObj.transform.position, MainPlayer.Self.ModelObj.transform.position);
            float num2 = Vector3.Dot(vector.normalized, MainPlayer.Self.ModelObj.transform.forward);
            if (num > 4f)
            {
                this.NextthinkTime = num / 4f;
                if (MainPlayer.Self.Pfc.CurrAutoMoveState != PathFindComponent.AutoMoveState.Off && Vector2.Distance(this.NextPosition2DOfTargetFinder, this.Target.NextPosition2D) < 12f)
                {
                    return;
                }
                MainPlayer.Self.Pfc.BeginFindPath(GraphUtils.GetServerPosByWorldPos(this.Target.ModelObj.transform.position - vector.normalized * 2f, true), PathFindComponent.AutoMoveState.MoveToAttackNpc, delegate ()
                {
                    this.NextPosition2DOfTargetFinder = Vector2.zero;
                }, null);
                this.NextPosition2DOfTargetFinder = this.Target.NextPosition2D;
            }
            else if (num2 < 0.6f)
            {
                if (num > 1f)
                {
                    this.NextthinkTime = 0.2f;
                    if (MainPlayer.Self.Pfc.CurrAutoMoveState != PathFindComponent.AutoMoveState.Off && Vector2.Distance(this.NextPosition2DOfTargetFinder, this.Target.NextPosition2D) < 12f)
                    {
                        return;
                    }
                    MainPlayer.Self.Pfc.BeginFindPath(GraphUtils.GetServerPosByWorldPos(MainPlayer.Self.ModelObj.transform.position + vector.normalized, true), PathFindComponent.AutoMoveState.MoveToAttackNpc, delegate ()
                    {
                        this.NextPosition2DOfTargetFinder = Vector2.zero;
                    }, null);
                    this.NextPosition2DOfTargetFinder = this.Target.NextPosition2D;
                }
                else
                {
                    this.NextthinkTime = 0.5f;
                    if (MainPlayer.Self.Pfc.CurrAutoMoveState != PathFindComponent.AutoMoveState.Off && Vector2.Distance(this.NextPosition2DOfTargetFinder, this.Target.NextPosition2D) < 12f)
                    {
                        return;
                    }
                    MainPlayer.Self.Pfc.BeginFindPath(GraphUtils.GetServerPosByWorldPos(MainPlayer.Self.ModelObj.transform.position + MainPlayer.Self.ModelObj.transform.forward * 3f, true), PathFindComponent.AutoMoveState.MoveToAttackNpc, delegate ()
                    {
                        this.NextPosition2DOfTargetFinder = Vector2.zero;
                    }, null);
                    this.NextPosition2DOfTargetFinder = this.Target.NextPosition2D;
                }
            }
            else
            {
                MainPlayerSkillHolder component = MainPlayer.Self.GetComponent<MainPlayerSkillHolder>();
                if (component != null)
                {
                    component.MainPlayerSkillList.BetterForeach(delegate (int index, KeyValuePair<uint, MainPlayerSkillBase> item)
                    {
                        if (index == this.skillindex)
                        {
                            this.NextthinkTime = 0.5f;
                            if (item.Value is MainPlayerNormalAttacklCombo)
                            {
                                MainPlayer.Self.GetComponent<MainPlayerSkillHolder>().ClickSkillEvent(item.Key);
                                MainPlayer.Self.GetComponent<MainPlayerSkillHolder>().ClickSkillEvent(item.Key);
                                MainPlayer.Self.GetComponent<MainPlayerSkillHolder>().ClickSkillEvent(item.Key);
                            }
                            else
                            {
                                MainPlayer.Self.GetComponent<MainPlayerSkillHolder>().ClickSkillEvent(item.Key);
                            }
                            this.skillindex++;
                            return false;
                        }
                        return true;
                    });
                }
                if (this.skillindex >= component.MainPlayerSkillList.Count || this.skillindex >= this.SkillCount)
                {
                    this.skillindex = 0;
                }
            }
        }
        else if (!string.IsNullOrEmpty(this.targetName))
        {
            this.SetTarget(this.targetName);
        }
    }

    public CharactorBase Target;

    private string targetName;

    private int SkillCount = 1;

    private bool OnGoto = true;

    private int skillindex;

    private float runningtime;

    private float NextthinkTime = 0.8f;

    private Vector2 NextPosition2DOfTargetFinder;
}
