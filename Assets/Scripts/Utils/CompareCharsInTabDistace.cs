using System;
using System.Collections.Generic;
using UnityEngine;

public class CompareCharsInTabDistace : IComparer<CharactorBase>
{
    public int Compare(CharactorBase item1, CharactorBase item2)
    {
        float num = Vector3.Distance(CommonTools.DismissYSize(MainPlayer.Self.ModelObj.transform.position), CommonTools.DismissYSize(item1.ModelObj.transform.position));
        float value = Vector3.Distance(CommonTools.DismissYSize(MainPlayer.Self.ModelObj.transform.position), CommonTools.DismissYSize(item2.ModelObj.transform.position));
        return num.CompareTo(value);
    }
}
