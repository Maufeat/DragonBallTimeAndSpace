using System;

public class ShortcutDragDropButtonData : DragDropButtonDataBase
{
    public ShortcutDragDropButtonData(SaveDataItem _data)
    {
        this.mSaveDataItem = _data;
        this.mId = (uint)this.mSaveDataItem.id;
        this.thisid = this.mSaveDataItem.thisid;
    }

    public SaveDataItem mSaveDataItem;
}
