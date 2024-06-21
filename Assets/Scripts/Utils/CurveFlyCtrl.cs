using System;
using System.Collections.Generic;
using UnityEngine;

internal class CurveFlyCtrl
{
    public void SetCurveSamlpe(CurveSamlpe[] Array)
    {
        this.SamlpeList.Clear();
        this.SamlpeList.AddRange(Array);
        this.TotalLength = this.SamlpeList[this.SamlpeList.Count - 1].Distance;
    }

    public Vector3 Apply(float rate)
    {
        float num = rate;
        if (num > 0.999f)
        {
            num = 0.999f;
        }
        if (num < 0f)
        {
            num = 0f;
        }
        Vector3 result = this.SamlpeList[0].pos;
        float currdis = num * this.TotalLength;
        if (this.CheckInterval(this.LastIndex, currdis))
        {
            result = this.GetPosBySamlpe(this.LastIndex, currdis);
        }
        else if (this.CheckInterval(this.LastIndex + 1, currdis))
        {
            result = this.GetPosBySamlpe(this.LastIndex + 1, currdis);
        }
        else if (this.CheckInterval(1, currdis))
        {
            result = this.GetPosBySamlpe(1, currdis);
        }
        else
        {
            for (int i = 1; i < this.SamlpeList.Count; i++)
            {
                if (this.CheckInterval(i, currdis))
                {
                    result = this.GetPosBySamlpe(i, currdis);
                    break;
                }
            }
        }
        return result;
    }

    public Vector3 TargetPos()
    {
        return this.SamlpeList[this.LastIndex].pos;
    }

    private bool CheckInterval(int Index, float currdis)
    {
        if (0 < Index && Index < this.SamlpeList.Count)
        {
            CurveSamlpe curveSamlpe = this.SamlpeList[Index];
            CurveSamlpe curveSamlpe2 = this.SamlpeList[Index - 1];
            return curveSamlpe2.Distance < currdis && currdis < curveSamlpe.Distance;
        }
        FFDebug.LogWarning(this, "CheckInterval  Our Range");
        return false;
    }

    private Vector3 GetPosBySamlpe(int Index, float currdis)
    {
        CurveSamlpe curveSamlpe = this.SamlpeList[Index];
        CurveSamlpe curveSamlpe2 = this.SamlpeList[Index - 1];
        float t = (currdis - curveSamlpe2.Distance) / (curveSamlpe.Distance - curveSamlpe2.Distance);
        this.LastIndex = Index;
        return Vector3.Lerp(curveSamlpe2.pos, curveSamlpe.pos, t);
    }

    public static Vector3 CRSpline(Vector3[] pts, float t)
    {
        int num = pts.Length - 3;
        int num2 = Mathf.Min(Mathf.FloorToInt(t * (float)num), num - 1);
        float num3 = t * (float)num - (float)num2;
        Vector3 a = pts[num2];
        Vector3 a2 = pts[num2 + 1];
        Vector3 vector = pts[num2 + 2];
        Vector3 b = pts[num2 + 3];
        return 0.5f * ((-a + 3f * a2 - 3f * vector + b) * (num3 * num3 * num3) + (2f * a - 5f * a2 + 4f * vector - b) * (num3 * num3) + (-a + vector) * num3 + 2f * a2);
    }

    public List<CurveSamlpe> SamlpeList = new List<CurveSamlpe>();

    private float TotalLength;

    private int LastIndex = 1;
}
