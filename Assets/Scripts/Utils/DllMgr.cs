using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class DllMgr : SingletonModel<DllMgr>
{
    public Vector2 GetCursorPos()
    {
        DllRelateStruct.Point point;
        DllReference.GetCursorPos(out point);
        return new Vector2((float)point.x, (float)point.y);
    }

    public void SetCursorPos(Vector2 vet)
    {
        this.SetCursorPos((int)vet.x, (int)vet.y);
    }

    public void SetCursorPos(int x, int y)
    {
        DllReference.SetCursorPos(x, y);
    }
}

public class DllReference : SingletonModel<DllReference>
{
    [DllImport("user32.dll")]
    public static extern int SetCursorPos(int x, int y);

    [DllImport("user32.dll")]
    public static extern bool GetCursorPos(out DllRelateStruct.Point p);
}

public class DllRelateStruct
{
    public struct Point
    {
        public static implicit operator Vector2(DllRelateStruct.Point p)
        {
            return new Vector2((float)p.x, (float)p.y);
        }

        public int x;

        public int y;
    }
}