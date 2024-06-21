using System;
using Framework.Managers;
using UnityEngine;

public class BufferStateEvilToliet : BufferState
{
    public BufferStateEvilToliet(UserState Flag) : base(Flag)
    {
    }

    public override void Enter(PlayerBufferControl PBControl)
    {
        base.Enter(PBControl);
        this.ffbc = this.BufferControl.Owner.GetComponent<FFBehaviourControl>();
        this.needRefreshHeight = false;
        Vector3 position = this.BufferControl.Owner.ModelObj.transform.position;
        this.BufferControl.Owner.FallCheckingHeight = MapHightDataHolder.GetMapHeight(position.x, position.z);
        this.BufferControl.Owner.ModelObj.transform.position = new Vector3(position.x, this.BufferControl.Owner.FallCheckingHeight, position.z);
        if (this.BufferControl.Owner.FallCheckingHeight == 0f)
        {
            ManagerCenter.Instance.GetManager<EntitiesManager>().PNetWork.ReqFallEnd();
        }
        if (this.BufferControl.Owner is MainPlayer)
        {
            Scheduler.Instance.AddUpdator(new Scheduler.OnScheduler(this.Update));
        }
    }

    public void Update()
    {
        if (!this.BufferControl.Owner.inJumpState() && !this.BufferControl.Owner.isFalling)
        {
            Vector3 position = this.BufferControl.Owner.ModelObj.transform.position;
            float y = position.y;
            Vector3 vector = position + this.BufferControl.Owner.moveDir * this.BufferControl.Owner.moveSpeed * Time.deltaTime;
            float mapHeight = MapHightDataHolder.GetMapHeight(vector.x, vector.z);
            if (y - mapHeight >= 2f)
            {
                this.BufferControl.Owner.FallCheckingHeight = y;
            }
            else
            {
                this.BufferControl.Owner.FallCheckingHeight = mapHeight;
            }
        }
        if (this.BufferControl.Owner.isFalling && !this.needRefreshHeight && this.BufferControl.Owner.FallCheckingHeight < 0.0001f)
        {
            GlobalRegister.SetShortcutKeyEnableState(false);
            if (!this.BufferControl.Owner.inJumpState())
            {
                this.BufferControl.Owner.isFalling = false;
                this.needRefreshHeight = true;
                Scheduler.Instance.AddFrame(10U, false, delegate
                {
                    ManagerCenter.Instance.GetManager<EntitiesManager>().PNetWork.ReqFallEnd();
                });
            }
        }
        if (this.needRefreshHeight && !this.BufferControl.Owner.inJumpState())
        {
            Vector3 worldPosByServerPos = GraphUtils.GetWorldPosByServerPos(this.BufferControl.Owner.CurrentPosition2D);
            float mapHeight2 = MapHightDataHolder.GetMapHeight(worldPosByServerPos.x, worldPosByServerPos.z);
            if (mapHeight2 > 100f)
            {
                this.BufferControl.Owner.FallCheckingHeight = mapHeight2;
                this.BufferControl.Owner.ModelObj.transform.position = new Vector3(worldPosByServerPos.x, mapHeight2, worldPosByServerPos.z);
                this.needRefreshHeight = false;
                GlobalRegister.SetShortcutKeyEnableState(true);
            }
        }
    }

    public override void Exit()
    {
        base.Exit();
        if (this.BufferControl.Owner is MainPlayer)
        {
            Scheduler.Instance.RemoveUpdator(new Scheduler.OnScheduler(this.Update));
        }
        this.ffbc = null;
    }

    public const float heightDis = 2f;

    private bool needRefreshHeight;

    private FFBehaviourControl ffbc;
}
