using System;
using hero;
using LuaInterface;

public class GeneDragDropData : DragDropButtonDataBase
{
    public GeneDragDropData(DnaItem di = null, SourceFrom _sf = SourceFrom.Bag, DNASlotType _dst = DNASlotType.ATT)
    {
        this.curDnaItem = di;
        this.sf = _sf;
        this.dst = _dst;
    }

    public GeneDragDropData(GeneDragDropData cloneObj, uint inPos, uint fromPos = 0U)
    {
        this.curDnaItem = cloneObj.curDnaItem;
        this.sf = cloneObj.sf;
        this.dst = cloneObj.dst;
        this.inPagePos = inPos;
        this.fromPagePos = fromPos;
    }

    public void ReInitDst()
    {
        if (this.curDnaItem != null)
        {
            uint num = this.curDnaItem.id + this.curDnaItem.level;
            LuaTable configTable = LuaConfigManager.GetConfigTable("dnachip_config", (ulong)num);
            if (configTable != null)
            {
                this.dst = (DNASlotType)configTable.GetCacheField_Int("type");
            }
        }
    }

    public override bool Equals(object obj)
    {
        if (obj == null)
        {
            return false;
        }
        GeneDragDropData geneDragDropData = obj as GeneDragDropData;
        return geneDragDropData != null && geneDragDropData.sf == this.sf && this.curDnaItem != null && geneDragDropData.curDnaItem != null && this.curDnaItem.id == geneDragDropData.curDnaItem.id && this.curDnaItem.level == geneDragDropData.curDnaItem.level;
    }

    public bool IsThisPosHaveCard()
    {
        return this.curDnaItem != null;
    }

    public SourceFrom sf;

    public DNASlotType dst;

    public DnaItem curDnaItem;

    public uint inPagePos;

    public uint fromPagePos;

    public bool isThisSlotOpen;
}
