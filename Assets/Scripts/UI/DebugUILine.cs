using System;
using UnityEngine;
using UnityEngine.UI;

public class DebugUILine : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        foreach (MaskableGraphic maskableGraphic in UnityEngine.Object.FindObjectsOfType<MaskableGraphic>())
        {
            if (maskableGraphic.raycastTarget)
            {
                RectTransform rectTransform = maskableGraphic.transform as RectTransform;
                rectTransform.GetWorldCorners(DebugUILine.fourCorners);
                Gizmos.color = Color.blue;
                for (int j = 0; j < 4; j++)
                {
                    Gizmos.DrawLine(DebugUILine.fourCorners[j], DebugUILine.fourCorners[(j + 1) % 4]);
                }
            }
        }
    }

    private static Vector3[] fourCorners = new Vector3[4];
}
