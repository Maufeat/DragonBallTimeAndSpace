using System;

public class ActionData
{
    public ActionData(ActionType _at, GeneDragDropData _gdddFrom = null, GeneDragDropData _gdddTo = null, uint _insertPos = 0U)
    {
        this.at = _at;
        this.gdddFrom = _gdddFrom;
        this.gdddTo = _gdddTo;
        this.insertPos = _insertPos;
    }

    public ActionType at;

    public GeneDragDropData gdddFrom;

    public GeneDragDropData gdddTo;

    public uint insertPos;
}
