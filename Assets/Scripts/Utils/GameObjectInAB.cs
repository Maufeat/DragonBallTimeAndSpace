using System;
using System.Collections.Generic;
using Framework.Managers;
using Game.Scene;
using UnityEngine;

public class GameObjectInAB
{
    public GameObjectInAB(FFAssetBundle ab)
    {
        this.AB = ab;
        if (this.AB != null)
        {
            this.Gobj = this.AB.GetMainAsset<GameObject>();
            this.RecordMaterials();
        }
        else
        {
            FFDebug.LogWarning(this, "GameObjectInAB Error : FFAssetBundle null");
        }
        if (this.Gobj == null)
        {
            FFDebug.LogWarning(this, "GameObjectInAB Error : Gobj null key: " + this.AB.Path);
        }
    }

    public void Unload()
    {
        this.RemoveMaterials();
        this.AB.Unload();
    }

    public static bool DestroyMaterial(Material mat)
    {
        if (mat == null)
        {
            return false;
        }
        if (!GameObjectInAB.allAssetsMaterials.Contains(mat.GetInstanceID()))
        {
            UnityEngine.Object.DestroyImmediate(mat, false);
            return true;
        }
        return false;
    }

    private void RecordMaterials()
    {
        if (this.AB == null || this.Gobj == null)
        {
            return;
        }
        foreach (Renderer renderer in this.Gobj.GetComponentsInChildren<Renderer>())
        {
            if (renderer != null && renderer.sharedMaterials != null)
            {
                for (int j = 0; j < renderer.sharedMaterials.Length; j++)
                {
                    Material material = renderer.sharedMaterials[j];
                    if (material != null)
                    {
                        int instanceID = material.GetInstanceID();
                        GameObjectInAB.allAssetsMaterials.Add(instanceID);
                        if (this.thisAssetsMaterials == null)
                        {
                            this.thisAssetsMaterials = new HashSet<int>();
                        }
                        this.thisAssetsMaterials.Add(instanceID);
                        if (material.shader.name.Equals("Dragon/Charactor/CharactorMultiUV"))
                        {
                            GameScene manager = ManagerCenter.Instance.GetManager<GameScene>();
                            manager.SetMatLightInfo(renderer.gameObject, false);
                        }
                    }
                }
            }
        }
    }

    private void RemoveMaterials()
    {
        if (this.thisAssetsMaterials != null)
        {
            GameObjectInAB.allAssetsMaterials.RemoveWhere(new Predicate<int>(this.PredicateMatset));
            this.thisAssetsMaterials.Clear();
            this.thisAssetsMaterials = null;
        }
    }

    private bool PredicateMatset(int Instanceid)
    {
        return this.thisAssetsMaterials != null && this.thisAssetsMaterials.Contains(Instanceid);
    }

    public GameObject Gobj;

    private FFAssetBundle AB;

    public bool RemoveByRef;

    public int RefCount;

    private static HashSet<int> allAssetsMaterials = new HashSet<int>();

    private HashSet<int> thisAssetsMaterials;
}
