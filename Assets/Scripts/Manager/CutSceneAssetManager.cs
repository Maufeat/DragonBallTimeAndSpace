using System;
using Framework.Base;
using UnityEngine;

public class CutSceneAssetManager : IManager
{
    public void LoadFromAB(string key, Action<CutSceneContent> callback)
    {
        string Key = key.ToLower();
        FFAssetBundleRequest.Request("cutscene", Key, delegate (FFAssetBundle ab)
        {
            this.CutScenemap[Key] = new GameObjectInAB(ab);
            GameObject gameObject = UnityEngine.Object.Instantiate(this.CutScenemap[Key].Gobj, Vector3.zero, Quaternion.identity) as GameObject;
            if (callback != null)
            {
                CutSceneContent component = gameObject.GetComponent<CutSceneContent>();
                gameObject.SetActive(false);
                callback(component);
            }
        }, true);
    }

    public void RemoveCutScene(string key)
    {
        if (this.CutScenemap.ContainsKey(key))
        {
            GameObjectInAB gameObjectInAB = this.CutScenemap[key];
            this.CutScenemap.Remove(key);
            gameObjectInAB.Unload();
        }
    }

    public void LoadSimpleCharacterObj(string key, Action callback)
    {
        string Key = key.ToLower();
        if (this.SimpleCharacterObjMap.ContainsKey(Key))
        {
            if (callback != null)
            {
                callback();
            }
        }
        else
        {
            FFAssetBundleRequest.CleverRequest(CharactorAndEffectBundleType.Charactor, Key, delegate (FFAssetBundle ab)
            {
                this.SimpleCharacterObjMap[Key] = new GameObjectInAB(ab);
                if (callback != null)
                {
                    callback();
                }
            });
        }
    }

    public GameObject GetSimpleCharacterObj(string Key)
    {
        string text = Key.ToLower();
        if (this.SimpleCharacterObjMap.ContainsKey(text))
        {
            return this.SimpleCharacterObjMap[text].Gobj;
        }
        FFDebug.LogWarning(this, "cant find  key : " + text + "  in  CombineCharacterbaseObjMap");
        return null;
    }

    public void RemoveSimpleCharacterObjMap(string key)
    {
        string key2 = key.ToLower();
        if (this.SimpleCharacterObjMap.ContainsKey(key2))
        {
            this.SimpleCharacterObjMap[key2].Unload();
            this.SimpleCharacterObjMap.Remove(key2);
        }
    }

    public string ManagerName
    {
        get
        {
            return base.GetType().ToString();
        }
    }

    public void OnUpdate()
    {
    }

    public void OnReSet()
    {
    }

    private BetterDictionary<string, GameObjectInAB> CutScenemap = new BetterDictionary<string, GameObjectInAB>();

    private BetterDictionary<string, GameObjectInAB> SimpleCharacterObjMap = new BetterDictionary<string, GameObjectInAB>();
}
