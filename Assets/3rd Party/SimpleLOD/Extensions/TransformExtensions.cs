/* SimpleLOD 1.5     */
/* By Orbcreation BV */
/* Richard Knol      */
/* March 4, 2015      */

using UnityEngine;
using System;
using System.Collections;

namespace OrbCreationExtensions
{
    public static class TransformExtensions
    {
        public static bool IsPartOf(this Transform trans, Transform aTransform)
        {
            if (trans == aTransform) return true;
            if (trans.parent != null && trans.parent != trans) return IsPartOf(trans.parent, aTransform);
            return false;
        }

        public static Transform FindFirstChildWithName(this Transform trans, string childName)
        {
            if (trans.gameObject.name == childName) return trans;
            foreach (Transform child in trans)
            {
                Transform childWithName = FindFirstChildWithName(child, childName);
                if (childWithName != null) return childWithName;
            }
            return null;
        }

        public static Transform FindFirstChildWhereNameContains(this Transform trans, string childName)
        {
            if (trans.gameObject.name.IndexOf(childName) >= 0) return trans;
            foreach (Transform child in trans)
            {
                Transform childWithName = FindFirstChildWhereNameContains(child, childName);
                if (childWithName != null) return childWithName;
            }
            return null;
        }

        public static T GetFirstComponentInParents<T>(this Transform trans) where T : MonoBehaviour
        {
            T component = trans.gameObject.GetComponent<T>();
            if (component != null) return component;
            if (trans.parent != null && trans.parent != trans) return GetFirstComponentInParents<T>(trans.parent);
            return null;
        }

        public static Vector3 PointToWorldSpace(this Transform trans, Vector3 p)
        {
            return trans.TransformPoint(p);
        }
        public static Vector3 PointToLocalSpace(this Transform trans, Vector3 p)
        {
            return trans.InverseTransformPoint(p);
        }
    }
}
