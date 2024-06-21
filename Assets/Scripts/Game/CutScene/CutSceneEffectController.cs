using System;
using UnityEngine;

public class CutSceneEffectController
{
    public static CutSceneEffectController Instance
    {
        get
        {
            if (CutSceneEffectController.instance == null)
            {
                CutSceneEffectController.instance = new CutSceneEffectController();
            }
            return CutSceneEffectController.instance;
        }
    }

    public void LoadEffobj(string Key, Action callback)
    {
        if (this.EffectObjMap.ContainsKey(Key))
        {
            callback();
        }
        else
        {
            FFAssetBundleRequest.CleverRequest(CharactorAndEffectBundleType.Effect, Key.ToLower(), delegate (FFAssetBundle ab)
            {
                this.EffectObjMap[Key] = new GameObjectInAB(ab);
                callback();
            });
        }
    }

    public void PlayEffect(string effect, Transform parent)
    {
        if (this.EffectObjMap.ContainsKey(effect))
        {
            if (this.EffectGameObjMap.ContainsKey(effect))
            {
                UnityEngine.Object.Destroy(this.EffectGameObjMap[effect]);
                this.EffectGameObjMap.Remove(effect);
            }
            if (null == this.EffectObjMap[effect].Gobj)
            {
                FFDebug.LogError("PlayEffect Gobj is NULL ", effect);
                return;
            }
            GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.EffectObjMap[effect].Gobj);
            gameObject.transform.SetParent(parent);
            gameObject.transform.localPosition = new Vector3(0f, 0f, 0f);
            gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
            gameObject.transform.localRotation = new Quaternion(0f, 0f, 0f, 0f);
            gameObject.SetActive(true);
            this.EffectGameObjMap[effect] = gameObject;
        }
        else
        {
            this.LoadEffobj(effect, delegate
            {
                if (this.EffectGameObjMap.ContainsKey(effect))
                {
                    UnityEngine.Object.Destroy(this.EffectGameObjMap[effect]);
                    this.EffectGameObjMap.Remove(effect);
                }
                if (null == this.EffectObjMap[effect].Gobj)
                {
                    return;
                }
                GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(this.EffectObjMap[effect].Gobj);
                gameObject2.transform.SetParent(parent);
                gameObject2.transform.localPosition = new Vector3(0f, 0f, 0f);
                gameObject2.transform.localScale = new Vector3(1f, 1f, 1f);
                gameObject2.transform.localRotation = new Quaternion(0f, 0f, 0f, 0f);
                gameObject2.SetActive(true);
                this.EffectGameObjMap[effect] = gameObject2;
            });
        }
    }

    public void RemoveEffectObj(string key)
    {
        if (this.EffectGameObjMap.ContainsKey(key))
        {
            UnityEngine.Object.Destroy(this.EffectGameObjMap[key]);
            this.EffectGameObjMap.Remove(key);
        }
    }

    public void LoadAllEffect()
    {
    }

    private static CutSceneEffectController instance;

    private BetterDictionary<string, GameObjectInAB> EffectObjMap = new BetterDictionary<string, GameObjectInAB>();

    private BetterDictionary<string, GameObject> EffectGameObjMap = new BetterDictionary<string, GameObject>();
}
