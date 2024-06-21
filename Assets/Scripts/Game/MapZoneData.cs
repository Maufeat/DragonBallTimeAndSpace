using System;
using System.Collections.Generic;
using UnityEngine;

public class MapZoneData : ScriptableObject
{
    public int _nWidth;

    public int _nHeight;

    public int _nCellsX;

    public int _nCellsZ;

    public int _nCount;

    public List<ZoneData> _lstData;

    public List<LoadAreaData> _lstAreaData;
}
