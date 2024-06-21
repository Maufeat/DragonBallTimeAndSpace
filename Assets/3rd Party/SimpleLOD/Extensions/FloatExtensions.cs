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
    public static class FloatExtensions
    {
        public static string MakeString(this float aFloat)
        {
            return "" + aFloat;
        }
        public static string MakeString(this float aFloat, int decimals)
        {
            if (decimals <= 0) return "" + Mathf.RoundToInt(aFloat);
            string format = "{0:F" + decimals + "}";
            return string.Format(format, aFloat);
        }

        public static float To180Angle(this float f)
        {
            while (f <= -180.0f) f += 360.0f;
            while (f > 180.0f) f -= 360.0f;
            return f;
        }
        public static float To360Angle(this float f)
        {
            while (f < 0.0f) f += 360.0f;
            while (f >= 360.0f) f -= 360.0f;
            return f;
        }
        public static float RadToCompassAngle(this float rad)
        {
            return DegreesToCompassAngle(rad * Mathf.Rad2Deg);
        }
        public static float DegreesToCompassAngle(this float angle)
        {
            angle = 90.0f - angle;
            return To360Angle(angle);
        }
        public static float Distance(this float f1, float f2)
        {
            return Mathf.Abs(f1 - f2);
        }
        public static float RelativePositionBetweenAngles(this float angle, float from, float to)
        {
            from = from.To360Angle();
            to = to.To360Angle();
            if ((from - to) > 180f) from = from - 360f;
            if ((to - from) > 180f) to = to - 360f;
            angle = angle.To360Angle();
            if (from < to)
            {
                if (angle >= from && angle < to) return (angle - from) / (to - from);
                if (angle - 360f >= from && angle - 360f < to) return (angle - 360f - from) / (to - from);
            }
            if (from > to)
            {
                if (angle < from && angle >= to) return (angle - to) / (from - to);
                if (angle - 360f < from && angle - 360f >= to) return (angle - 360f - to) / (from - to);
            }
            return -1f;
        }

        public static float Round(this float f, int decimals)
        {
            float multiplier = Mathf.Pow(10, decimals);
            f = Mathf.Round(f * multiplier);
            return f / multiplier;
        }
    }
}
