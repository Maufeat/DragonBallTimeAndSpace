using System;
using System.Collections;
using System.Collections.Generic;
using Framework.Managers;
using LuaInterface;
using UnityEngine;

public class FFBipBindMgr : IFFComponent
{
    public CompnentState State { get; set; }

    private void Init()
    {
        BipBindData clip = ManagerCenter.Instance.GetManager<BipBindDataMgr>().GetClip(this.boneName);
        if (clip != null)
        {
            this.MyBipBindData = UnityEngine.Object.Instantiate<BipBindData>(clip);
            for (int i = 0; i < this.MyBipBindData.BindPointList.Length; i++)
            {
                BindPoint bindPoint = this.MyBipBindData.BindPointList[i];
                this.BindPointMap[bindPoint.Name.ToLower()] = bindPoint;
                this.InitBindPoint(bindPoint);
            }
        }
        else
        {
            this.MyBipBindData = ScriptableObject.CreateInstance<BipBindData>();
            this.MyBipBindData.ModelName = this.boneName;
            this.MyBipBindData.BindPointList = new BindPoint[0];
        }
        this.ExcursionBip();
    }

    private void ExcursionBip()
    {
        LuaTable appearancenpcConfig = this.Owmner.BaseData.GetAppearancenpcConfig();
        if (appearancenpcConfig != null)
        {
            LuaTable cacheField_Table = appearancenpcConfig.GetCacheField_Table("PreWorkdata");
            if (cacheField_Table != null)
            {
                LuaTable cacheField_Table2 = cacheField_Table.GetCacheField_Table("bipbind_excursion");
                if (cacheField_Table2 != null)
                {
                    IEnumerator enumerator = cacheField_Table2.Values.GetEnumerator();
                    enumerator.Reset();
                    while (enumerator.MoveNext())
                    {
                        object obj = enumerator.Current;
                        LuaTable luaTable = obj as LuaTable;
                        if (luaTable != null)
                        {
                            string field_String = luaTable.GetField_String("name");
                            Transform bindPoint = this.GetBindPoint(field_String);
                            if (bindPoint != null)
                            {
                                Vector3 b = new Vector3(luaTable.GetCacheField_Float("x"), luaTable.GetCacheField_Float("y"), luaTable.GetCacheField_Float("z"));
                                bindPoint.transform.position += b;
                            }
                        }
                    }
                }
            }
        }
    }

    private void InitBindPoint(BindPoint BP)
    {
        GameObject gameObject = new GameObject();
        gameObject.name = BP.Name;
        if (BP.Path == string.Empty)
        {
            gameObject.transform.SetParent(this.Modle.transform);
        }
        else
        {
            Transform transform = this.Modle.transform.Find(BP.Path);
            if (transform != null)
            {
                gameObject.transform.SetParent(transform);
            }
            else
            {
                gameObject.transform.SetParent(this.Modle.transform);
            }
        }
        BipFollowTarget bipFollowTarget = gameObject.GetComponent<BipFollowTarget>();
        if (BP.FollowTarget == string.Empty)
        {
            if (null != bipFollowTarget)
            {
                bipFollowTarget.target = null;
            }
        }
        else
        {
            Transform transform2 = this.Modle.transform.Find(BP.FollowTarget);
            if (transform2 == null)
            {
                if (null != bipFollowTarget)
                {
                    bipFollowTarget.target = null;
                }
            }
            else
            {
                if (null == bipFollowTarget)
                {
                    bipFollowTarget = gameObject.AddComponent<BipFollowTarget>();
                }
                bipFollowTarget.target = transform2;
                bipFollowTarget.FixX = BP.FollowPosX;
                bipFollowTarget.FixY = BP.FollowPosY;
                bipFollowTarget.FixZ = BP.FollowPosZ;
            }
        }
        BP.Tran = gameObject.transform;
        BP.ReadPos();
    }

    public Transform GetBindPoint(string BindPointName)
    {
        string text = BindPointName.ToLower();
        if (this.BindPointMap.ContainsKey(text))
        {
            return this.BindPointMap[text].Tran;
        }
        FFDebug.Log(this, FFLogType.Default, "cant get BindPoint: " + text + " in Model " + this.boneName);
        return null;
    }

    private GameObject Modle
    {
        get
        {
            return this.Owmner.ModelObj;
        }
    }

    public void CompAwake(FFComponentMgr Mgr)
    {
        this.Owmner = Mgr.Owner;
        this.boneName = Mgr.Owner.BaseData.GetACname();
        if (this.boneName == "none")
        {
            this.boneName = Mgr.Owner.BaseData.GetModelName();
        }
        this.Init();
    }

    public void ResetComp()
    {
        this.CompDispose();
        this.boneName = this.Owmner.BaseData.GetACname();
        if (this.boneName == "none")
        {
            this.boneName = this.Owmner.BaseData.GetModelName();
        }
        this.Init();
    }

    public void CompUpdate()
    {
    }

    public void CompDispose()
    {
        this.BindPointMap.BetterForeach(delegate (KeyValuePair<string, BindPoint> item)
        {
            if (item.Value.Tran != null)
            {
                UnityEngine.Object.Destroy(item.Value.Tran.gameObject);
            }
        });
        this.BindPointMap.Clear();
    }

    private string boneName = string.Empty;

    private BipBindData MyBipBindData;

    private GameObject NoneUseBPs;

    private Transform BipRoot;

    public BetterDictionary<string, BindPoint> BindPointMap = new BetterDictionary<string, BindPoint>();

    public CharactorBase Owmner;
}
