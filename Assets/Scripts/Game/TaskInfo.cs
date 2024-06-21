using System;
using quest;

public class TaskInfo
{
    public int ShowPriority
    {
        get
        {
            if (this.stateNum == 37U)
            {
                return 4;
            }
            if (this.stateNum == 9U)
            {
                return 3;
            }
            if (this.stateNum == 8U)
            {
                return 2;
            }
            if (this.stateNum == 7U)
            {
                return 1;
            }
            return 0;
        }
    }

    public string GetIconType()
    {
        int showPriority = this.ShowPriority;
        string result = string.Empty;
        switch (showPriority)
        {
            case 1:
                result = "icontask/img_no";
                break;
            case 2:
                result = "icontask/img_in";
                break;
            case 3:
                result = "icontask/img_ok";
                break;
            case 4:
                result = "icontask/img_dialog";
                break;
        }
        return result;
    }

    public void RefreshData(QuestStateInfo ServerData, uint showState, uint npcId)
    {
        this.questid = ServerData.questid;
        this.stateNum = showState;
    }

    public void RefreshData(TaskInfoRefreshData ServerData)
    {
        this.questid = ServerData.questid;
        this.stateNum = ServerData.state;
    }

    public uint questid;

    public uint stateNum;
}
