using System;
using UnityEngine;

public class TipPosSetManager
{
    public static TipPosSetManager Instance
    {
        get
        {
            if (TipPosSetManager._instance == null)
            {
                TipPosSetManager._instance = new TipPosSetManager();
            }
            return TipPosSetManager._instance;
        }
    }

    public void Initilize(GameObject btnObj, GameObject tipObj)
    {
        tipObj.transform.position = btnObj.transform.position;
        Vector3 vector = (tipObj.transform as RectTransform).anchoredPosition;
        Vector2 sizeDelta = (btnObj.transform as RectTransform).sizeDelta;
        Vector2 vector2 = Vector2.one;
        for (int i = 0; i < tipObj.transform.childCount; i++)
        {
            Transform child = tipObj.transform.GetChild(i);
            if (child.gameObject.activeSelf)
            {
                vector2 = (child as RectTransform).sizeDelta;
                break;
            }
        }
        Vector2 sizeDelta2 = (UIManager.Instance.UIRoot as RectTransform).sizeDelta;
        float y = sizeDelta2.y;
        float x = sizeDelta2.x;
        bool flag = vector.x > x / 2f;
        bool flag2 = vector.y < y / -2f;
        Vector2 zero = Vector2.zero;
        zero.x = vector.x + ((!flag) ? (sizeDelta.x / 2f + 10f) : (-sizeDelta.x / 2f - 10f - vector2.x));
        zero.y = vector.y - sizeDelta.y * 0.1f + ((!flag2) ? 0f : vector2.y);
        if (zero.y > 0f)
        {
            zero.y = 0f;
        }
        else if (zero.y - vector2.y < -y)
        {
            zero.y = -y + vector2.y;
        }
        (tipObj.transform as RectTransform).anchoredPosition = zero;
    }

    private static TipPosSetManager _instance;
}
