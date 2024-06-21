using System;

public class ObjectInPoolUnit
{
    public ObjectInPoolUnit(ulong storeid, ulong tempid, string effname, ObjectInPoolBase objpool)
    {
        this.storeID = storeid;
        this.tempid = tempid;
        this.effName = effname;
        this.objInPool = objpool;
    }

    public ulong storeID;

    public ulong tempid;

    public string effName;

    public ObjectInPoolBase objInPool;
}
