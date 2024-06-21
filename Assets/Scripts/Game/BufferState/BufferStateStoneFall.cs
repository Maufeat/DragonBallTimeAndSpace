using System;
using Framework.Managers;
using UnityEngine;

public class BufferStateStoneFall : BufferState
{
    public BufferStateStoneFall(UserState Flag) : base(Flag)
    {
    }

    public override void Enter(PlayerBufferControl PBControl)
    {
        base.Enter(PBControl);
        this.em = ManagerCenter.Instance.GetManager<EntitiesManager>();
        Scheduler.Instance.AddUpdator(new Scheduler.OnScheduler(this.Update));
    }

    public void Update()
    {
        this.npc = this.em.SearchNearNPCById(this.stoneId, -1f);
        if (this.npc != null)
        {
            float distance = this.em.GetDistance(this.em.MainPlayer, this.npc);
            if (distance <= this.minDistance)
            {
                this.MainPlayerMoveByStone(this.em.MainPlayer, this.npc);
            }
        }
    }

    public Vector2 GetBestPoint(MainPlayer mainPlayer, Npc npc)
    {
        Vector2 result = mainPlayer.CurrentPosition2D;
        Vector3 vector = GraphUtils.GetWorldPosByServerPos(mainPlayer.CurrentPosition2D);
        Vector3 worldPosByServerPos = GraphUtils.GetWorldPosByServerPos(npc.CurrentPosition2D);
        Vector3 worldPosByServerPos2 = GraphUtils.GetWorldPosByServerPos(npc.NextPosition2D);
        Vector3 vector2 = worldPosByServerPos2 - worldPosByServerPos;
        if (worldPosByServerPos.x != worldPosByServerPos2.x || worldPosByServerPos.z != worldPosByServerPos2.z)
        {
            Vector3 value = vector - worldPosByServerPos;
            if (Vector3.Dot(Vector3.Normalize(vector2), Vector3.Normalize(value)) >= 0f)
            {
                vector += vector2;
                result = GraphUtils.GetServerPosByWorldPos_new(vector);
            }
        }
        int inputDir = SingletonForMono<InputController>.Instance.InputDir;
        if (inputDir != -1)
        {
            Vector2 clientDirVector2ByServerDir = CommonTools.GetClientDirVector2ByServerDir(inputDir);
            Vector2 v = mainPlayer.CurrentPosition2D + clientDirVector2ByServerDir * mainPlayer.MinimunMoveUnit;
            Vector3 b = GraphUtils.GetWorldPosByServerPos(v) - GraphUtils.GetWorldPosByServerPos(mainPlayer.CurrentPosition2D);
            vector += b;
            if (Vector3.Distance(vector, worldPosByServerPos2) < this.minDistance)
            {
                Vector2 a = Vector3.Normalize(new Vector3(vector.x - worldPosByServerPos2.x, vector.y - worldPosByServerPos2.y));
                result = GraphUtils.GetServerPosByWorldPos_new(vector) + a * mainPlayer.MinimunMoveUnit;
            }
            else
            {
                result = GraphUtils.GetServerPosByWorldPos_new(vector);
            }
        }
        return result;
    }

    private bool CheckPosValid(Vector2 topos, Vector2 currentPos)
    {
        return true;
    }

    public void MainPlayerMoveByStone(MainPlayer mainPlayer, Npc npc)
    {
        Vector2 vector = this.GetBestPoint(mainPlayer, npc);
        int inputDir = SingletonForMono<InputController>.Instance.InputDir;
        if (vector.x < 0f || vector.y < 0f || vector.x >= (float)LSingleton<CurrentMapAccesser>.Instance.CellNumX || vector.y >= (float)LSingleton<CurrentMapAccesser>.Instance.CellNumY)
        {
            FFDebug.Log(this, FFLogType.Player, "已到达地图边界，无法移动");
            mainPlayer.ServerDir = (uint)inputDir;
            return;
        }
        if (GraphUtils.IsBlockPointForMove(vector.x, vector.y) || !this.CheckPosValid(vector, mainPlayer.CurrentPosition2D))
        {
            vector = mainPlayer.CurrentPosition2D;
        }
        Vector2 vector2 = vector;
        vector2.x = GraphUtils.Keep2DecimalPlaces(vector2.x);
        vector2.y = GraphUtils.Keep2DecimalPlaces(vector2.y);
        cs_MoveData cs_MoveData = new cs_MoveData();
        cs_MoveData.dir = (uint)inputDir;
        cs_MoveData.pos = default(cs_FloatMovePos);
        cs_MoveData.pos.fx = vector2.x;
        cs_MoveData.pos.fy = vector2.y;
        ManagerCenter.Instance.GetManager<EntitiesManager>().PNetWork.ReqMove(cs_MoveData, false, false);
        mainPlayer.MoveTo(cs_MoveData);
    }

    public override void Exit()
    {
        base.Exit();
        Scheduler.Instance.RemoveUpdator(new Scheduler.OnScheduler(this.Update));
        this.npc = null;
        this.em = null;
    }

    private float minDistance = 3f;

    private uint stoneId = 9094U;

    private Npc npc;

    private EntitiesManager em;
}
