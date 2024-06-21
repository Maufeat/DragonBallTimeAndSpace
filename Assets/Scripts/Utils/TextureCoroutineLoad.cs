using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureCoroutineLoad : MonoBehaviour
{
    public static TextureCoroutineLoad Current
    {
        get
        {
            TextureCoroutineLoad.Initialize();
            return TextureCoroutineLoad._current;
        }
    }

    private void Awake()
    {
        TextureCoroutineLoad._current = this;
        TextureCoroutineLoad.initialized = true;
    }

    private static void Initialize()
    {
        if (!TextureCoroutineLoad.initialized)
        {
            if (!Application.isPlaying)
            {
                return;
            }
            TextureCoroutineLoad.initialized = true;
            GameObject gameObject = new GameObject("Loom");
            TextureCoroutineLoad._current = gameObject.AddComponent<TextureCoroutineLoad>();
        }
    }

    public void RunAsync(Projector p, Texture2D t, float angle, Action act, float innerRadius = 3f)
    {
        base.StartCoroutine(this.GetTexture(p, t, angle, act, innerRadius));
    }

    private IEnumerator GetTexture(Projector projector, Texture2D sourceTex, float angle, Action callBack, float iRadius = 3f)
    {
        Texture2D newTex = new Texture2D(sourceTex.width, sourceTex.height, TextureFormat.ARGB32, false);
        newTex.name = sourceTex.name;
        newTex.wrapMode = TextureWrapMode.Clamp;
        iRadius = ((iRadius >= (float)(sourceTex.width / 2)) ? ((float)(sourceTex.width / 2 - 1)) : iRadius);
        float cosAngle = Mathf.Cos(0.0174532924f * angle / 2f);
        float sinAngle = Mathf.Sin(0.0174532924f * angle / 2f);
        int r = sourceTex.width / 2;
        Vector2 point0 = new Vector2((float)r, (float)r);
        bool isCircle = CommonTools.CheckFloatEqual(angle, 360f);
        Vector2 point = Vector2.zero;
        Color colorZero = new Color(0f, 0f, 0f, 0f);
        float rp = (float)(r * r);
        float ip = iRadius * iRadius;
        int part = 4;
        List<int> mParts = new List<int>();
        for (int i = 0; i < part; i++)
        {
            mParts.Add(sourceTex.width / part * (i + 1));
        }
        List<int> aready = new List<int>();
        for (int j = 0; j < sourceTex.width; j++)
        {
            for (int k = 0; k < sourceTex.height; k++)
            {
                if (mParts.Contains(j) && !aready.Contains(j))
                {
                    aready.Add(j);
                    yield return null;
                }
                if (j == 0 || k == 0 || j == sourceTex.width - 1 || k == sourceTex.height - 1)
                {
                    newTex.SetPixel(j, k, new Color(0f, 0f, 0f, 0f));
                }
                else
                {
                    Color c = sourceTex.GetPixel(j, k);
                    if (isCircle)
                    {
                        float d = (float)((j - r) * (j - r) + (k - r) * (k - r));
                        if (d >= ip && d <= rp)
                        {
                            newTex.SetPixel(j, k, c);
                        }
                        else
                        {
                            newTex.SetPixel(j, k, colorZero);
                        }
                    }
                    else if (TextureCoroutineLoad.IsPointInCirclularSector(point0.x, point0.y, iRadius, (float)r, cosAngle, (float)j, (float)k))
                    {
                        newTex.SetPixel(j, k, c);
                    }
                    else
                    {
                        newTex.SetPixel(j, k, colorZero);
                    }
                }
            }
        }
        newTex.Apply();
        projector.material.mainTexture = newTex;
        if (callBack != null)
        {
            callBack();
        }
        yield return null;
        yield break;
    }

    public static Texture2D GetTexture(Texture2D sourceTex, float angle, float iRadius = 6f)
    {
        Texture2D texture2D = new Texture2D(sourceTex.width, sourceTex.height, TextureFormat.ARGB32, false);
        texture2D.name = sourceTex.name;
        texture2D.wrapMode = TextureWrapMode.Clamp;
        iRadius = ((iRadius >= (float)(sourceTex.width / 2)) ? ((float)(sourceTex.width / 2 - 1)) : iRadius);
        float costheta = Mathf.Cos(0.0174532924f * angle / 2f);
        float num = Mathf.Sin(0.0174532924f * angle / 2f);
        int num2 = sourceTex.width / 2;
        Vector2 vector = new Vector2((float)num2, (float)num2);
        bool flag = CommonTools.CheckFloatEqual(angle, 360f);
        Vector2 zero = Vector2.zero;
        Color color = new Color(0f, 0f, 0f, 0f);
        float num3 = (float)(num2 * num2);
        float num4 = iRadius * iRadius;
        for (int i = 0; i < sourceTex.width; i++)
        {
            for (int j = 0; j < sourceTex.height; j++)
            {
                if (i == 0 || j == 0 || i == sourceTex.width - 1 || j == sourceTex.height - 1)
                {
                    texture2D.SetPixel(i, j, new Color(0f, 0f, 0f, 0f));
                }
                else
                {
                    Color pixel = sourceTex.GetPixel(i, j);
                    if (flag)
                    {
                        float num5 = (float)((i - num2) * (i - num2) + (j - num2) * (j - num2));
                        if (num5 >= num4 && num5 <= num3)
                        {
                            texture2D.SetPixel(i, j, pixel);
                        }
                        else
                        {
                            texture2D.SetPixel(i, j, color);
                        }
                    }
                    else if (TextureCoroutineLoad.IsPointInCirclularSector(vector.x, vector.y, iRadius, (float)num2, costheta, (float)i, (float)j))
                    {
                        texture2D.SetPixel(i, j, pixel);
                    }
                    else
                    {
                        texture2D.SetPixel(i, j, color);
                    }
                }
            }
        }
        texture2D.Apply();
        return texture2D;
    }

    public static bool IsPointInCirclularSector(float cx, float cy, float iRadius, float outRadius, float costheta, float px, float py)
    {
        float num = px - cx;
        float num2 = py - cy;
        float f = num * num + num2 * num2;
        float num3 = Mathf.Sqrt(f);
        return num3 >= iRadius && num3 <= outRadius && num2 > num3 * costheta;
    }

    private void OnDisable()
    {
        if (TextureCoroutineLoad._current == this)
        {
            TextureCoroutineLoad._current = null;
        }
    }

    private void OnDestroy()
    {
        base.StopAllCoroutines();
        TextureCoroutineLoad.initialized = false;
        TextureCoroutineLoad._current = null;
    }

    private static TextureCoroutineLoad _current;

    private static bool initialized;
}
