using System;
using UnityEngine;

public class ScreenEvent
{
    public ScreenEvent.EventType mTpye;

    public Vector3 InputPos;

    public Vector3 SlipDis;

    public Vector2 CursorPos;

    public float mfMouseScrollWheel;

    public bool IsDragGraphic;

    public bool IsMouseSlip;

    public Vector3 ClickDownPos;

    public Vector3 LastPos;

    public int mFingerId;

    public enum EventType
    {
        None,
        Began,
        Click,
        Slip,
        Zoom,
        Select,
        SlipEnd,
        QuickClick
    }
}
