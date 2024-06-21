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
    public static class Vector3Extensions
    {
        public static string MakeString(this Vector3 v)
        {
            return "<" + v.x + "," + v.y + "," + v.z + ">";
        }
        public static string MakeString(this Vector3 v, int decimals)
        {
            if (decimals <= 0) return "<" + Mathf.RoundToInt(v.x) + "," + Mathf.RoundToInt(v.y) + "," + Mathf.RoundToInt(v.z) + ">";
            string format = "{0:F" + decimals + "}";
            return "<" + string.Format(format, v.x) + "," + string.Format(format, v.y) + "," + string.Format(format, v.z) + ">";
        }
        public static Vector3 Product(this Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1.x * v2.x, v1.y * v2.y, v1.z * v2.z);
        }
        public static float Sum(this Vector3 v1)
        {
            return (v1.x + v1.y + v1.z);
        }
        public static float InProduct(this Vector3 v1, Vector3 v2)
        {
            return (v1.x * v2.x) + (v1.y * v2.y) + (v1.z * v2.z);
        }
        public static Vector3 Abs(this Vector3 v)
        {
            return new Vector3(Mathf.Abs(v.x), Mathf.Abs(v.y), Mathf.Abs(v.z));
        }
        public static Vector3 VectorMax(this Vector3 v1, Vector3 v2)
        {
            return new Vector3(Mathf.Max(v1.x, v2.x), Mathf.Max(v1.y, v2.y), Mathf.Max(v1.z, v2.z));
        }
        public static bool IsEqual(this Vector3 v1, Vector3 v2)
        { // this is faster than using ==
            return v1.x == v2.x && v1.y == v2.y && v1.z == v2.z;
        }
        public static bool DiffBetween(this Vector3 v1, Vector3 v2, float from, float to)
        {
            return v1.x - v2.x < to && v1.x - v2.x > from && v1.y - v2.y < to && v1.y - v2.y > from && v1.z - v2.z < to && v1.z - v2.z > from;
        }
        public static bool IsDiffSmallEnough(this Vector3 v1, Vector3 v2, float maxDiff)
        {
            return v1.x - v2.x < maxDiff && v2.x - v1.x < maxDiff && v1.y - v2.y < maxDiff && v2.y - v1.y < maxDiff && v1.z - v2.z < maxDiff && v2.z - v1.z < maxDiff;
        }
        public static bool IsAllSmaller(this Vector3 v1, Vector3 v2)
        {
            return v1.x < v2.x && v1.y < v2.y && v1.z < v2.z;
        }

        // p - point in triangle
        // a, b, c corners of triangle
        // return u, v where u = distance over c - a, v = distance over b - a
        public static Vector2 Barycentric(this Vector3 p, Vector3 a, Vector3 b, Vector3 c)
        {
            Vector3 v0 = c - a;
            Vector3 v1 = b - a;
            Vector3 v2 = p - a;

            // Compute dot products
            float dot00 = Vector3.Dot(v0, v0);
            float dot01 = Vector3.Dot(v0, v1);
            float dot02 = Vector3.Dot(v0, v2);
            float dot11 = Vector3.Dot(v1, v1);
            float dot12 = Vector3.Dot(v1, v2);

            // Compute barycentric coordinates
            float invDenom = 1f / (dot00 * dot11 - dot01 * dot01);
            float u = (dot11 * dot02 - dot01 * dot12) * invDenom;
            float v = (dot00 * dot12 - dot01 * dot02) * invDenom;
            return new Vector2(u, v);
        }
        public static Vector3 To180Angle(this Vector3 v)
        {
            v.x = v.x.To180Angle();
            v.y = v.y.To180Angle();
            v.z = v.z.To180Angle();
            return v;
        }

        public static Vector3 To360Angle(this Vector3 v)
        {
            v.x = v.x.To360Angle();
            v.y = v.y.To360Angle();
            v.z = v.z.To360Angle();
            return v;
        }
    }

    public static class Vector2Extensions
    {
        public static string MakeString(this Vector2 v)
        {
            return "<" + v.x + "," + v.y + ">";
        }
        public static string MakeString(this Vector2 v, int decimals)
        {
            if (decimals <= 0) return "<" + Mathf.RoundToInt(v.x) + "," + Mathf.RoundToInt(v.y) + ">";
            string format = "{0:F" + decimals + "}";
            return "<" + string.Format(format, v.x) + "," + string.Format(format, v.y) + ">";
        }
        public static Vector2 Product(this Vector2 v1, Vector2 v2)
        {
            return new Vector2(v1.x * v2.x, v1.y * v2.y);
        }
        public static float InProduct(this Vector2 v1, Vector2 v2)
        {
            return (v1.x * v2.x) + (v1.y * v2.y);
        }

        public static Vector2 Abs(this Vector2 v)
        {
            return new Vector2(Mathf.Abs(v.x), Mathf.Abs(v.y));
        }
        public static bool IsEqual(this Vector2 v1, Vector2 v2)
        { // this is faster than using ==
            return v1.x == v2.x && v1.y == v2.y;
        }
        public static bool DiffBetween(this Vector2 v1, Vector2 v2, float from, float to)
        { // this is faster than using ==
            return v1.x - v2.x < to && v1.x - v2.x > from && v1.y - v2.y < to && v1.y - v2.y > from;
        }
        public static bool IsDiffSmallEnough(this Vector2 v1, Vector2 v2, float maxDiff)
        {
            return v1.x - v2.x < maxDiff && v2.x - v1.x < maxDiff && v1.y - v2.y < maxDiff && v2.y - v1.y < maxDiff;
        }
        public static bool IsAllSmaller(this Vector2 v1, Vector2 v2)
        {
            return v1.x < v2.x && v1.y < v2.y;
        }
        public static bool IsBarycentricInTriangle(this Vector2 v)
        {
            return (v.x >= 0f) && (v.y >= 0f) && (v.x + v.y <= 1.01f);
        }
    }
}
