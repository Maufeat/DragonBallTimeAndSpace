using System;
using System.Collections.Generic;
using quest;

public class NpcTask
{
    public TaskInfo GetFirstShowTask()
    {
        TaskInfo ShowTask = null;
        this.TaskInfoMap.BetterForeach(delegate (KeyValuePair<uint, TaskInfo> item)
        {
            TaskInfo value = item.Value;
            if (ShowTask == null)
            {
                ShowTask = value;
            }
            else if (ShowTask.ShowPriority < value.ShowPriority)
            {
                ShowTask = value;
            }
        });
        return ShowTask;
    }

    public void RefreshData(npcQuestList ServerData)
    {
        this.Eid = ServerData.npcid;
        for (int i = 0; i < ServerData.quests.Count; i++)
        {
            uint questid = ServerData.quests[i].questid;
            if (!this.TaskInfoMap.ContainsKey(questid))
            {
                this.TaskInfoMap[questid] = new TaskInfo();
            }
            TaskInfo taskInfo = this.TaskInfoMap[questid];
            taskInfo.RefreshData(ServerData.quests[i], (uint)ServerData.state, this.Eid);
        }
    }

    public void RefreshData(TaskInfoRefreshData ServerData, bool force = false)
    {
        if (!this.TaskInfoMap.ContainsKey(ServerData.questid) && force)
        {
            this.TaskInfoMap[ServerData.questid] = new TaskInfo();
        }
        if (this.TaskInfoMap.ContainsKey(ServerData.questid))
        {
            this.TaskInfoMap[ServerData.questid].RefreshData(ServerData);
        }
    }

    public uint Eid;

    public BetterDictionary<uint, TaskInfo> TaskInfoMap = new BetterDictionary<uint, TaskInfo>();
}
