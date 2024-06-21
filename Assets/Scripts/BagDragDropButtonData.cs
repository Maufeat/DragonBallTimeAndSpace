using System;
using Obj;
using UnityEngine;

public class BagDragDropButtonData : DragDropButtonDataBase
{
    public BagDragDropButtonData(uint _id, string _thisid, PackType _packtype, uint _lock_end_time, Transform _root, Action<uint> _cb)
    {
        this.mId = _id;
        this.thisid = _thisid;
        this.packtype = _packtype;
        this.lock_end_time = _lock_end_time;
        this.root = _root;
        this.monclickitem = _cb;
    }

    public Transform root;

    public Action<uint> monclickitem;

    public PackType packtype;

    public uint lock_end_time;
}
