/* SimpleLOD 1.5     */
/* By Orbcreation BV */
/* Richard Knol      */
/* March 4, 2015      */

/* Note: if you also use other packages by Orbcreation,  */
/* you may end up with multiple copies of this file.     */
/* In that case, better delete/merge those files into 1. */

using UnityEngine;
using System;
using System.Collections;

namespace OrbCreationExtensions
{
    public static class RectExtensions
    {
        public static bool MouseInRect(this Rect rect, Vector2 point)
        {
            return rect.Contains(point);
        }
        public static bool MouseInRect(this Rect rect)
        {
            Vector2 point = Input.mousePosition;
            point.y = (Screen.height - point.y);
            return rect.MouseInRect(point);
        }
        public static bool MouseInRect(this Rect rect, Rect parentRect, Vector2 point)
        {
            rect.x += parentRect.x;
            rect.y += parentRect.y;
            return rect.MouseInRect(point);
        }
        public static bool MouseInRect(this Rect rect, Rect parentRect)
        {
            Vector2 point = Input.mousePosition;
            point.y = (Screen.height - point.y);
            return rect.MouseInRect(parentRect, point);
        }
        public static bool MouseInRect(this Rect rect, Rect parentRect1, Rect parentRect2, Vector2 point)
        {
            rect.x += parentRect1.x;
            rect.y += parentRect1.y;
            rect.x += parentRect2.x;
            rect.y += parentRect2.y;
            return rect.MouseInRect(point);
        }
        public static bool MouseInRect(this Rect rect, Rect parentRect1, Rect parentRect2)
        {
            Vector2 point = Input.mousePosition;
            point.y = (Screen.height - point.y);
            return rect.MouseInRect(parentRect1, parentRect2, point);
        }
        public static Vector2 RelativeMousePosInRect(this Rect rect, Vector2 point)
        {
            Vector2 relativePoint = new Vector2(-1, -1);
            if (rect.Contains(point))
            {
                relativePoint.x = point.x - rect.x;
                if (rect.width > 0.0f) relativePoint.x = Mathf.Abs(relativePoint.x / rect.width);
                relativePoint.y = point.y - rect.y;
                if (rect.height > 0.0f) relativePoint.y = 1.0f - Mathf.Abs(relativePoint.y / rect.height);
            }
            return relativePoint;
        }
        public static Vector2 RelativeMousePosInRect(this Rect rect)
        {
            Vector2 point = Input.mousePosition;
            point.y = (Screen.height - point.y);
            return rect.RelativeMousePosInRect(point);
        }

        public static Rect RelativeRectInImage(this Rect r, Texture2D img)
        {
            return new Rect(r.x / img.width, 1.0f - ((r.y + r.height) / img.height), r.width / img.width, r.height / img.height);
        }

        public static float MaxExtents(this Bounds b)
        {
            return Mathf.Max(Mathf.Max(b.extents.x, b.extents.y), b.extents.z);
        }
        public static float MaxSize(this Bounds b)
        {
            return Mathf.Max(Mathf.Max(b.size.x, b.size.y), b.size.z);
        }
        public static float MinExtents(this Bounds b)
        {
            return Mathf.Min(Mathf.Min(b.extents.x, b.extents.y), b.extents.z);
        }
        public static float MinSize(this Bounds b)
        {
            return Mathf.Min(Mathf.Min(b.size.x, b.size.y), b.size.z);
        }
    }
}
