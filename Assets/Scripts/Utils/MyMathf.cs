using UnityEngine;

public class MyMathf
{
    public static Vector3 GetRayPoint(Ray ray, float y, float maxL = 1000f)
    {
        Vector3 vector = Camera.main.transform.position;
        vector.y = y;
        vector -= Camera.main.transform.position;
        float num = Vector3.Angle(ray.direction, vector);
        num *= 0.0174532924f;
        float num2 = Mathf.Cos(num);
        float num3 = Camera.main.transform.position.y - y;
        float num4 = num3 / num2;
        if (num4 > maxL || num4 < 0f)
        {
            num4 = maxL;
        }
        return ray.GetPoint(num4);
    }

    public static Vector3 GetCircularPoint(Vector3 o, Vector3 point, float r)
    {
        Ray ray = new Ray(o, point);
        return ray.GetPoint(r);
    }
}
