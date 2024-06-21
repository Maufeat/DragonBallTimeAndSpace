using System;
using UnityEngine;

internal class WayPointCheck
{
    public WayPointCheck(GameObject g, bool isValid, uint index, float minCheckDist)
    {
        this.pointObj = g;
        this.isValid = isValid;
        this.index = index;
        this.minCheckDist = minCheckDist;
    }

    public bool isPointExsit
    {
        get
        {
            return this.pointObj != null;
        }
    }

    public bool IsInThisPointRange(Vector2 otherPoint)
    {
        Vector3 worldPosByServerPos = GraphUtils.GetWorldPosByServerPos(otherPoint);
        worldPosByServerPos.y = this.pointObj.transform.position.y;
        BoxCollider component = this.pointObj.GetComponent<BoxCollider>();
        return component.bounds.Contains(worldPosByServerPos);
    }

    public GameObject pointObj;

    public bool isValid;

    public uint index;

    private float minCheckDist;
}
