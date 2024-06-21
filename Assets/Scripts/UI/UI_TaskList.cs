using System;
using LuaInterface;
using UnityEngine;

public class UI_TaskList : UIPanelBase
{
    public override void OnInit(Transform root)
    {
    }

    public override void OnDispose()
    {
        base.OnDispose();
    }

    internal void OnGetActiveQuestData(LuaTable t)
    {
    }
}
