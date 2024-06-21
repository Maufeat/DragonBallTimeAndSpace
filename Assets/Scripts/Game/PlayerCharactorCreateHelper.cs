using System;
using System.Collections.Generic;
using Framework.Managers;
using LuaInterface;
using UnityEngine;

public class PlayerCharactorCreateHelper
{
    public uint[] useID
    {
        get
        {
            if (this.useID_ == null)
            {
                this.useID_ = new uint[5];
            }
            return this.useID_;
        }
    }

    public static void LoadBody(string respath, CharactorAndEffectBundleType btype, string bonename, string key, string body, string haircolor, string hairstyle, string facestyle, string antenna, Action<PlayerCharactorCreateHelper> back, Action<PlayerCharactorCreateHelper> beforeLoadFinish = null, bool shadow = false, ulong keyid = 0UL)
    {
        PlayerCharactorCreateHelper mhhTem = new PlayerCharactorCreateHelper();
        mhhTem.rootKey = key;
        mhhTem.resPath = respath;
        mhhTem.BundleTyp = btype;
        mhhTem.bodyName = body;
        mhhTem.headName = facestyle;
        mhhTem.hairName = hairstyle;
        mhhTem.hairColorName = haircolor;
        mhhTem.antennaName = antenna;
        mhhTem.isCasdShadow = shadow;
        mhhTem.boneName = ((!string.IsNullOrEmpty(bonename)) ? bonename : key);
        mhhTem.keyid = keyid;
        if (beforeLoadFinish != null)
        {
            beforeLoadFinish(mhhTem);
        }
        CharacterModelMgr cmm = ManagerCenter.Instance.GetManager<CharacterModelMgr>();
        if (cmm != null)
        {
            cmm.LoadAvatarPartObj(respath, btype, key, body, delegate (GameObjectInAB CharModel)
            {
                if (CharModel.Gobj != null)
                {
                    GameObject avatarPartObj = cmm.GetAvatarPartObj(key, body);
                    string text = key + "|" + body;
                    if (!PlayerCharactorCreateHelper.PoolManager.ObjectPools.ContainsKey(text))
                    {
                        GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(avatarPartObj);
                        gameObject.name = text;
                        PlayerCharactorCreateHelper.PoolManager.CreatPool<ModleObjInPool>(gameObject, null, null, true, bonename);
                    }
                    mhhTem.rootBone = PlayerCharactorCreateHelper.GenerateBoneUObj(mhhTem.rootBone, text);
                    mhhTem.rootObj = mhhTem.rootBone.ItemObj;
                    mhhTem.rootObj.transform.position = Vector3.up * 50f;
                    mhhTem.rootObj.transform.localScale = Vector3.one;
                    Scheduler.Instance.AddFrame(2U, false, delegate
                    {
                        if (mhhTem.rootBone != null && null != mhhTem.rootBone.ItemObj)
                        {
                            DynamicBone[] componentsInChildren = mhhTem.rootBone.ItemObj.GetComponentsInChildren<DynamicBone>();
                            if (componentsInChildren != null)
                            {
                                for (int i = 0; i < componentsInChildren.Length; i++)
                                {
                                    componentsInChildren[i].enabled = true;
                                }
                            }
                        }
                    });
                    mhhTem.LoadFace(mhhTem.headName, back, null, true);
                }
                else
                {
                    FFDebug.LogWarning("LoadRoot   key:", key + "   res:" + body);
                }
            });
        }
    }

    public void TrySetFaceColorRamp(uint charId, string faceID, string colorID)
    {
        string ramTexName = string.Empty;
        LuaTable configTable = LuaConfigManager.GetConfigTable("newUser", (ulong)charId);
        if (configTable != null)
        {
            string cacheField_String = configTable.GetCacheField_String("feature");
            string cacheField_String2 = configTable.GetCacheField_String("color");
            if (cacheField_String2.Contains(")"))
            {
                string[] array = cacheField_String.Split(new char[]
                {
                    '|'
                });
                int index = 0;
                for (int i = 0; i < array.Length; i++)
                {
                    if (array[i].Equals(faceID))
                    {
                        index = i;
                        break;
                    }
                }
                string[] array2 = cacheField_String2.Split(new string[]
                {
                    ")|"
                }, StringSplitOptions.RemoveEmptyEntries);
                int num = 0;
                for (int j = 0; j < array2.Length; j++)
                {
                    if (array2[j].Contains(colorID))
                    {
                        string[] array3 = array2[j].Split(new char[]
                        {
                            '|'
                        });
                        for (int k = 0; k < array3.Length; k++)
                        {
                            if (array3[k].Contains(colorID))
                            {
                                num = k;
                                break;
                            }
                        }
                        break;
                    }
                }
                string contentByMatch = UI_SelectRole.GetContentByMatch(cacheField_String2, "[(][0-9|\\|]{1,}[)]", index, new string[]
                {
                    "(",
                    ")"
                });
                string s = string.Empty;
                if (!string.IsNullOrEmpty(contentByMatch))
                {
                    string[] array4 = contentByMatch.Split(new char[]
                    {
                        '|'
                    });
                    for (int l = 0; l < array4.Length; l++)
                    {
                        if (l == num)
                        {
                            s = array4[l];
                            break;
                        }
                    }
                }
                ramTexName = LuaConfigManager.GetConfigTable("looksconfig", (ulong)uint.Parse(s)).GetCacheField_String("resource");
            }
        }
        this.SetFaceColor(ramTexName, null);
    }

    public void LoadFace(string headAbName, Action<PlayerCharactorCreateHelper> back = null, Action finishCall = null, bool isNeedLoadHair = true)
    {
        this.headName = headAbName;
        if (this.rootObj != null && !string.IsNullOrEmpty(headAbName))
        {
            CharacterModelMgr cmm = ManagerCenter.Instance.GetManager<CharacterModelMgr>();
            if (cmm != null)
            {
                cmm.LoadAvatarPartObj(this.resPath, this.BundleTyp, this.rootKey, headAbName.Trim(), delegate (GameObjectInAB CharModel)
                {
                    if (CharModel.Gobj != null)
                    {
                        GameObject avatarPartObj = cmm.GetAvatarPartObj(this.rootKey, headAbName.Trim());
                        string text = this.rootKey.ToLower() + "|" + headAbName.Trim().ToLower();
                        if (!PlayerCharactorCreateHelper.PoolManager.ObjectPools.ContainsKey(text))
                        {
                            GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(avatarPartObj);
                            gameObject.name = text;
                            PlayerCharactorCreateHelper.PoolManager.CreatPool<ModleObjInPool>(gameObject, null, null, true, string.Empty);
                        }
                        this.headBone = PlayerCharactorCreateHelper.GenerateBoneUObj(this.headBone, text);
                        this.headObj = this.headBone.ItemObj;
                    }
                    else
                    {
                        FFDebug.LogWarning("load res fail rootKey:", this.rootKey + "   headAbName:" + headAbName);
                    }
                    if (null != this.headObj)
                    {
                        this.HangToNodeByName(this.rootObj.transform, this.headObj.transform, "head");
                        this.headObj.transform.localScale = Vector3.one;
                        Scheduler.Instance.AddFrame(2U, false, delegate
                        {
                            if (null != this.headObj)
                            {
                                DynamicBone[] componentsInChildren = this.headObj.GetComponentsInChildren<DynamicBone>();
                                if (componentsInChildren != null)
                                {
                                    for (int i = 0; i < componentsInChildren.Length; i++)
                                    {
                                        componentsInChildren[i].enabled = true;
                                    }
                                }
                            }
                        });
                    }
                    if (isNeedLoadHair && (!string.IsNullOrEmpty(this.antennaName.Trim()) || !string.IsNullOrEmpty(this.hairName.Trim())))
                    {
                        if (!string.IsNullOrEmpty(this.antennaName.Trim()))
                        {
                            this.LoadHair(this.antennaName.Trim(), back, null);
                        }
                        else
                        {
                            this.LoadHair(this.hairName.Trim(), back, null);
                        }
                    }
                    else
                    {
                        this.LoadAniController(back);
                    }
                    if (finishCall != null)
                    {
                        finishCall();
                    }
                });
            }
        }
        else
        {
            this.LoadAniController(back);
            FFDebug.Log(this, FFLogType.Player, "CreatePlayer HangFace failed , headname : " + headAbName);
        }
    }

    public void SetFaceColor(string ramTexName = "", Action<PlayerCharactorCreateHelper> back = null)
    {
        if (this.rootObj != null && this.headObj != null && !string.IsNullOrEmpty(ramTexName))
        {
            this.headColorName = ramTexName;
            FFAssetBundleRequest.CleverRequest(CharactorAndEffectBundleType.CharactorTexture, this.rootKey + "/" + this.headColorName, delegate (FFAssetBundle item)
            {
                if (back != null)
                {
                    back(null);
                }
                if (item == null)
                {
                    return;
                }
                string text = string.Concat(new object[]
                {
                    "SkinnedTexute/characters/characterstexture/",
                    this.headColorName,
                    "/",
                    PlayerCharactorCreateHelper.IncreaseId++,
                    this.headColorName + ".u"
                });
                item.AddUse(text);
                this.loadedTexRes[text] = item;
                Texture2D mainAsset = item.GetMainAsset<Texture2D>();
                if (item != null && mainAsset != null && this.headObj)
                {
                    Renderer componentInChildren = this.headObj.GetComponentInChildren<Renderer>();
                    if (componentInChildren == null || componentInChildren.sharedMaterial == null)
                    {
                        FFDebug.LogWarning(this, "render.sharedMaterial.SetTexture(_Ramp, txture);  not set to an instance");
                        return;
                    }
                    componentInChildren.sharedMaterial.SetTexture("_Ramp", mainAsset);
                }
            });
        }
    }

    public void LoadHair(string hairAbName, Action<PlayerCharactorCreateHelper> back = null, Action finishCall = null)
    {
        this.hairName = hairAbName;
        if (this.rootObj != null && !string.IsNullOrEmpty(hairAbName))
        {
            CharacterModelMgr cmm = ManagerCenter.Instance.GetManager<CharacterModelMgr>();
            if (cmm != null)
            {
                cmm.LoadAvatarPartObj(this.resPath, this.BundleTyp, this.rootKey, hairAbName.Trim(), delegate (GameObjectInAB CharModel)
                {
                    if (CharModel.Gobj != null)
                    {
                        GameObject avatarPartObj = cmm.GetAvatarPartObj(this.rootKey, hairAbName);
                        string text = this.rootKey.ToLower() + "|" + hairAbName.Trim().ToString();
                        if (!PlayerCharactorCreateHelper.PoolManager.ObjectPools.ContainsKey(text))
                        {
                            GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(avatarPartObj);
                            gameObject.name = text;
                            PlayerCharactorCreateHelper.PoolManager.CreatPool<ModleObjInPool>(gameObject, null, null, true, string.Empty);
                        }
                        this.hairBone = PlayerCharactorCreateHelper.GenerateBoneUObj(this.hairBone, text);
                        this.hairObj = this.hairBone.ItemObj;
                        this.HangToNodeByName(this.rootObj.transform, this.hairObj.transform, "head");
                        this.hairObj.transform.localScale = Vector3.one;
                        Scheduler.Instance.AddFrame(2U, false, delegate
                        {
                            if (null != this.hairObj)
                            {
                                DynamicBone[] componentsInChildren = this.hairObj.GetComponentsInChildren<DynamicBone>();
                                if (componentsInChildren != null)
                                {
                                    for (int i = 0; i < componentsInChildren.Length; i++)
                                    {
                                        componentsInChildren[i].enabled = true;
                                    }
                                }
                            }
                        });
                    }
                    this.SetHairColor(this.hairColorName, back);
                    if (finishCall != null)
                    {
                        finishCall();
                    }
                });
            }
        }
        else
        {
            this.LoadAniController(back);
            FFDebug.LogWarning("LoadHair failed hairAbName = ", hairAbName);
        }
    }

    public void LoadAniController(Action<PlayerCharactorCreateHelper> back = null)
    {
        Animator animator = this.rootObj.GetComponent<Animator>();
        if (null != this.rootObj && null != animator)
        {
            AnimatorControllerMgr acmgr = ManagerCenter.Instance.GetManager<AnimatorControllerMgr>();
            acmgr.GetAnimatorController(this.boneName, delegate (RuntimeAnimatorController ac)
            {
                if (null != ac)
                {
                    BundleDetective bundleDetective = this.rootObj.GetComponent<BundleDetective>();
                    if (null == bundleDetective)
                    {
                        bundleDetective = this.rootObj.AddComponent<BundleDetective>();
                        bundleDetective.BundleOnAddCallBack = new Action<string>(acmgr.OnAnimatorAddUse);
                        bundleDetective.BundleOnDestryCallBack = new Action<string>(acmgr.OnAnimatorReduceUse);
                    }
                    bundleDetective.AniBundleName = ac.name.ToLower();
                    animator.runtimeAnimatorController = ac;
                }
                if (back != null)
                {
                    back(this);
                }
            });
        }
        else if (back != null)
        {
            back(this);
        }
    }

    public void SetHairColor(string cName, Action<PlayerCharactorCreateHelper> back = null)
    {
        if (!string.IsNullOrEmpty(cName))
        {
            this.hairColorName = cName;
            FFAssetBundleRequest.CleverRequest(CharactorAndEffectBundleType.CharactorTexture, this.rootKey + "/" + this.hairColorName, delegate (FFAssetBundle item)
            {
                this.LoadAniController(back);
                if (item == null)
                {
                    return;
                }
                string text = string.Concat(new object[]
                {
                    "SkinnedTexute/characters/characterstexture/",
                    this.hairColorName,
                    "/",
                    PlayerCharactorCreateHelper.IncreaseId++,
                    this.hairColorName + ".u"
                });
                item.AddUse(text);
                this.loadedTexRes[text] = item;
                Texture2D mainAsset = item.GetMainAsset<Texture2D>();
                if (item != null && mainAsset != null && this.hairObj)
                {
                    Renderer componentInChildren = this.hairObj.GetComponentInChildren<Renderer>();
                    componentInChildren.material.SetTexture("_Ramp", mainAsset);
                }
            });
        }
        else
        {
            this.LoadAniController(back);
        }
    }

    private static ObjectPoolManager PoolManager
    {
        get
        {
            return ManagerCenter.Instance.GetManager<ObjectPoolManager>();
        }
    }

    private static ModleObjInPool GenerateBoneUObj(ModleObjInPool objpool, string keyName)
    {
        ObjectPool<ModleObjInPool> objectPool = PlayerCharactorCreateHelper.PoolManager.GetObjectPool<ModleObjInPool>(keyName, true);
        if (objectPool != null)
        {
            PlayerCharactorCreateHelper.DisposeBonePObj(objpool);
            return objectPool.GetItemFromPool();
        }
        return null;
    }

    public static void DisposeBonePObj(ModleObjInPool objpool)
    {
        if (objpool != null)
        {
            objpool.DisableAndBackToPool(true);
        }
        objpool = null;
    }

    private void HangToNodeByName(Transform root, Transform child, string keyWords)
    {
        if (root && child && !string.IsNullOrEmpty(keyWords))
        {
            string value = keyWords.ToLower();
            Transform[] componentsInChildren = root.GetComponentsInChildren<Transform>();
            for (int i = 0; i < componentsInChildren.Length; i++)
            {
                string text = componentsInChildren[i].name.ToLower();
                if (text.Contains(value))
                {
                    child.SetParent(componentsInChildren[i], false);
                    child.localPosition = Vector3.zero;
                    child.localRotation = Quaternion.identity;
                    break;
                }
            }
        }
    }

    public void DestroyBonePObj()
    {
        if (this.rootBone != null)
        {
            this.rootBone.DestroyThis();
        }
        if (this.headBone != null)
        {
            this.headBone.DestroyThis();
        }
        if (this.hairBone != null)
        {
            this.hairBone.DestroyThis();
        }
        this.rootBone = null;
        this.headBone = null;
        this.hairBone = null;
        foreach (KeyValuePair<string, FFAssetBundle> keyValuePair in this.loadedTexRes)
        {
            if (keyValuePair.Value != null)
            {
                keyValuePair.Value.DeductUse(keyValuePair.Key);
                keyValuePair.Value.UnloadIfNoUse();
            }
        }
    }

    private void SetModelColor(GameObject target, Color c)
    {
        if (target == null)
        {
            return;
        }
        Renderer[] componentsInChildren = target.GetComponentsInChildren<Renderer>();
        if (componentsInChildren != null)
        {
            for (int i = 0; i < componentsInChildren.Length; i++)
            {
                componentsInChildren[i].material.SetColor("_Color", c);
            }
        }
    }

    public void DisposeBonePObj()
    {
        if (this.rootBone != null)
        {
            this.rootBone.DisableAndBackToPool(true);
        }
        if (this.headBone != null)
        {
            this.SetModelColor(this.headBone.ItemObj, Color.white);
            this.headBone.DisableAndBackToPool(true);
        }
        if (this.hairBone != null)
        {
            this.hairBone.DisableAndBackToPool(true);
        }
        this.rootBone = null;
        this.headBone = null;
        this.hairBone = null;
        foreach (KeyValuePair<string, FFAssetBundle> keyValuePair in this.loadedTexRes)
        {
            if (keyValuePair.Value != null)
            {
                keyValuePair.Value.DeductUse(keyValuePair.Key);
                keyValuePair.Value.UnloadIfNoUse();
            }
        }
    }

    public GameObject rootObj;

    public GameObject headObj;

    public GameObject hairObj;

    public string sceneNameKey;

    public string rootKey;

    public string bodyName;

    public string headName;

    public string boneName;

    public string hairName;

    public string hairColorName;

    public string headColorName;

    public string antennaName;

    public string resPath;

    private CharactorAndEffectBundleType BundleTyp;

    public uint[] useID_;

    public float bodyHeight;

    public ModleObjInPool rootBone;

    public ModleObjInPool headBone;

    public ModleObjInPool hairBone;

    public ulong keyid;

    private static int IncreaseId;

    private bool isSetHairColor;

    private bool isCasdShadow;

    private Dictionary<string, FFAssetBundle> loadedTexRes = new Dictionary<string, FFAssetBundle>();
}
