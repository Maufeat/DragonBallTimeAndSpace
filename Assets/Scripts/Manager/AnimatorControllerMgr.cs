using System;
using System.Collections.Generic;
using Framework.Base;
using UnityEngine;

public class AnimatorControllerMgr : IManager
{
    public void LoadAnimatorControllerInfo(Action callback)
    {
        FFAssetBundleRequest.Request("Config", "animatorcontrollerinfo", delegate (FFAssetBundle ab)
        {
            foreach (AnimatorControllerInfo animatorControllerInfo in ab.GetAllAsset<AnimatorControllerInfo>())
            {
                animatorControllerInfo.FillMap();
                this.AnimatorControllerInfoList[animatorControllerInfo.name] = animatorControllerInfo;
            }
            callback();
        }, true);
    }

    public void LoadFromProto(Action callback)
    {
        ScriptableToProto.Read<AnimatorControllerInfo>("config/animatorcontrollerinfo.bytes", delegate (AnimatorControllerInfo config)
        {
            if (config != null && config.ProtoList != null)
            {
                for (int i = 0; i < config.ProtoList.Length; i++)
                {
                    AnimatorControllerInfo animatorControllerInfo = config.ProtoList[i];
                    animatorControllerInfo.FillMap();
                    this.AnimatorControllerInfoList[animatorControllerInfo.name] = animatorControllerInfo;
                }
            }
            if (callback != null)
            {
                callback();
            }
        });
    }

    public float GetAnimationClipTime(string AcName, string clipname)
    {
        if (this.AnimatorControllerInfoList.ContainsKey(AcName))
        {
            AnimatorControllerInfo animatorControllerInfo = this.AnimatorControllerInfoList[AcName];
            if (animatorControllerInfo.ClipMap.ContainsKey(clipname))
            {
                return animatorControllerInfo.ClipMap[clipname].Length;
            }
        }
        return 1f;
    }

    public void LoadAnimatorController(string Key, Action callback)
    {
        if (this.RuntimeAnimatorControllerMap.ContainsKey(Key.ToLower()))
        {
            callback();
        }
        else
        {
            FFAssetBundleRequest.CleverRequest(CharactorAndEffectBundleType.AnimatorController, Key, delegate (FFAssetBundle ab)
            {
                if (ab != null)
                {
                    string text = Key.ToLower();
                    if (!this.RuntimeAnimatorControllerMap.ContainsKey(text))
                    {
                        AssetInAB<RuntimeAnimatorController> assetInAB = new AssetInAB<RuntimeAnimatorController>(ab);
                        if (null != assetInAB.mAsset)
                        {
                            this.RuntimeAnimatorControllerMap[assetInAB.mAsset.name.ToLower()] = assetInAB;
                            if (text != assetInAB.mAsset.name.ToLower())
                            {
                                FFDebug.LogWarning(this, string.Format("LoadAnimator make error key ={0}, target = {1}", Key, assetInAB.mAsset.name.ToLower()));
                            }
                        }
                    }
                }
                callback();
            });
        }
    }

    public void GetAnimatorController(string Key, Action<RuntimeAnimatorController> callBack)
    {
        if (!this.RuntimeAnimatorControllerMap.ContainsKey(Key.ToLower()))
        {
            this.LoadAnimatorController(Key, delegate
            {
                if (this.RuntimeAnimatorControllerMap.ContainsKey(Key.ToLower()))
                {
                    callBack(this.RuntimeAnimatorControllerMap[Key.ToLower()].mAsset);
                }
                else
                {
                    callBack(null);
                }
            });
        }
        else
        {
            callBack(this.RuntimeAnimatorControllerMap[Key.ToLower()].mAsset);
        }
    }

    public void OnAnimatorAddUse(string key)
    {
        if (string.IsNullOrEmpty(key))
        {
            return;
        }
        if (this.RuntimeAnimatorControllerMap.ContainsKey(key))
        {
            this.RuntimeAnimatorControllerMap[key].RefCount++;
        }
    }

    public void OnAnimatorReduceUse(string key)
    {
        if (string.IsNullOrEmpty(key))
        {
            return;
        }
        if (this.RuntimeAnimatorControllerMap.ContainsKey(key))
        {
            this.RuntimeAnimatorControllerMap[key].RefCount--;
            int refCount = this.RuntimeAnimatorControllerMap[key].RefCount;
            if (this.RuntimeAnimatorControllerMap[key].RefCount <= 0)
            {
                this.RuntimeAnimatorControllerMap[key].Unload();
                this.RuntimeAnimatorControllerMap.Remove(key);
            }
        }
    }

    public void AddUnloadbesideByMainplayer()
    {
        this.BesideTmp.Clear();
        if (MainPlayer.Self != null)
        {
            if (MainPlayer.Self.ModelHoldHelper != null)
            {
                Animator component = MainPlayer.Self.ModelHoldHelper.rootObj.GetComponent<Animator>();
                if (null != component && null != component.runtimeAnimatorController)
                {
                    this.BesideTmp.Add(component.runtimeAnimatorController.name.ToLower());
                }
            }
            for (int i = 0; i < GlobalRegister.rtObjRoot.transform.childCount; i++)
            {
                Animator component2 = GlobalRegister.rtObjRoot.transform.GetChild(i).gameObject.GetComponent<Animator>();
                if (null != component2 && null != component2.runtimeAnimatorController)
                {
                    this.BesideTmp.Add(component2.runtimeAnimatorController.name.ToLower());
                    FFDebug.LogWarning("AddUnloadbesideByMainplayer Model Exist name = ", component2.transform.name);
                }
            }
        }
    }

    public void RegUnloadAnimatorRes(string key)
    {
        if (this.BesideTmp.Contains(key))
        {
            this.BesideTmp.Add(key);
        }
    }

    public void UnloadBside()
    {
        this.RuntimeAnimatorControllerMap.BetterForeach(delegate (KeyValuePair<string, AssetInAB<RuntimeAnimatorController>> item)
        {
            if (!this.BesideTmp.Contains(item.Key))
            {
                item.Value.Unload();
                this.RuntimeAnimatorControllerMap.Remove(item.Key);
            }
        });
        this.BesideTmp.Clear();
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

    private BetterDictionary<string, AssetInAB<RuntimeAnimatorController>> RuntimeAnimatorControllerMap = new BetterDictionary<string, AssetInAB<RuntimeAnimatorController>>();

    private BetterDictionary<string, AnimatorControllerInfo> AnimatorControllerInfoList = new BetterDictionary<string, AnimatorControllerInfo>();

    private List<string> BesideTmp = new List<string>();
}
