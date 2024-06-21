using System;
using Framework.Managers;
using LuaInterface;
using Net;
using quest;

public class TaskUINetWork : NetWorkBase
{
    public override void Initialize()
    {
        this.RegisterMsg();
    }

    private TaskUIController taskuiController
    {
        get
        {
            return ControllerManager.Instance.GetController<TaskUIController>();
        }
    }

    public override void RegisterMsg()
    {
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_RetChangeMapFindPath_SC>(2407, new ProtoMsgCallback<MSG_RetChangeMapFindPath_SC>(this.RetChangeMapFindPath));
    }

    public void ReqChangeMapFindPath(uint pathwayid, uint questid)
    {
        CutSceneManager manager = ManagerCenter.Instance.GetManager<CutSceneManager>();
        if (manager.IsOnLoadingCutScene() || manager.InPlayCutScene())
        {
            return;
        }
        LuaTable configTable = LuaConfigManager.GetConfigTable("pathway", (ulong)pathwayid);
        string field_String = configTable.GetField_String("coordinates");
        string[] array = field_String.Split(new char[]
        {
            ','
        });
        uint destx = 99999U;
        uint desty = 99999U;
        if (array == null || array.Length != 2)
        {
            FFDebug.LogError(this, string.Concat(new object[]
            {
                "ReqChangeMapFindPath pathfind config is wrong id = ",
                pathwayid,
                "coordinates = ",
                field_String
            }));
            return;
        }
        uint.TryParse(array[0], out destx);
        uint.TryParse(array[1], out desty);
        base.SendMsg<MSG_ReqChangeMapFindPath_CS>(CommandID.MSG_ReqChangeMapFindPath_CS, new MSG_ReqChangeMapFindPath_CS
        {
            pathwayid = pathwayid,
            questid = questid,
            destx = destx,
            desty = desty
        }, false);
    }

    public void RetChangeMapFindPath(MSG_RetChangeMapFindPath_SC msgInfo)
    {
        if (msgInfo.info.errcode == 1U)
        {
            FFDebug.LogWarning(this, "   FindPath   fail !");
        }
        else
        {
            this.taskuiController.FindPath(msgInfo.info.pathwayid, delegate
            {
                if (this.taskuiController.taskController.autoTask.CurPathwayid == msgInfo.info.pathwayid)
                {
                    this.taskuiController.taskController.autoTask.EnterState(msgInfo.info.pathwayid, TaskAuto.E_Type.Seek);
                }
            });
        }
    }
}
