using System;
using System.Collections.Generic;
using UnityEngine;

public class EditorSubWindow
{
    public static void OnGuiSubWin(ref Rect WinRect, string Title, Action Draw, bool CanDrag = true, bool CanHide = true)
    {
        EditorSubWindow.OnGuiSubWin(ref WinRect, Title, Draw, null, CanDrag, CanHide);
    }

    public static void OnGuiSubWin(ref Rect WinRect, string Title, Action Draw, Action Close, bool CanDrag = true, bool CanHide = true)
    {
        float width = WinRect.width;
        int hashCode = Title.GetHashCode();
        if (!EditorSubWindow.GuiSubWinIsShow.ContainsKey(hashCode))
        {
            EditorSubWindow.GuiSubWinIsShow[hashCode] = true;
        }
        Rect rect = WinRect;
        if (!EditorSubWindow.GuiSubWinIsShow[hashCode])
        {
            rect.height = 20f;
        }
        rect = GUI.Window(hashCode, rect, delegate (int id)
        {
            if (CanDrag)
            {
                GUI.DragWindow(new Rect(0f, 0f, width - 30f, 20f));
            }
            if (CanHide)
            {
                string text = (!EditorSubWindow.GuiSubWinIsShow[id]) ? "╉" : "▂";
                if (GUI.Button(new Rect(width - (float)((Close == null) ? 30 : 60), 0f, 30f, 20f), text))
                {
                    EditorSubWindow.GuiSubWinIsShow[id] = !EditorSubWindow.GuiSubWinIsShow[id];
                }
            }
            if (Close != null && GUI.Button(new Rect(width - 30f, 0f, 30f, 20f), "X") && Close != null)
            {
                Close();
            }
            Draw();
        }, Title);
        if (EditorSubWindow.GuiSubWinIsShow[hashCode])
        {
            WinRect = rect;
        }
        else
        {
            WinRect.x = rect.x;
            WinRect.y = rect.y;
        }
    }

    private static Dictionary<int, bool> GuiSubWinIsShow = new Dictionary<int, bool>();
}
