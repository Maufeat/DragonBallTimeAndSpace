using System;
using System.Collections.Generic;
using Framework.Base;
using Framework.Managers;
using UnityEngine;

public class LODManager : IManager
{
    public static LODManager Instance
    {
        get
        {
            return ManagerCenter.Instance.GetManager<LODManager>();
        }
    }

    public string ManagerName
    {
        get
        {
            return base.GetType().Name;
        }
    }

    public void RegisterLod(LODSwitcher lod)
    {
        if (!this.LODList.Contains(lod))
        {
            this.LODList.Add(lod);
        }
    }

    public void SetLodCustomCamera(Camera camera)
    {
        for (int i = 0; i < this.LODList.Count; i++)
        {
            this.LODList[i].SetCustomCamera(camera);
        }
    }

    public void OnReSet()
    {
        this.LODList.Clear();
    }

    public void OnUpdate()
    {
    }

    private List<LODSwitcher> LODList = new List<LODSwitcher>();
}
