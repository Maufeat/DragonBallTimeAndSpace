using System;
using System.Collections.Generic;
using Framework.Managers;
using LuaInterface;
using map;
using UnityEngine;

public class Npc_Building : Npc
{
    public Npc_Building()
    {
        this.Init();
    }

    public override void Init()
    {
        base.Init();
    }

    public override void Update()
    {
        base.Update();
        this.onDieProgress();
    }

    public override void Die()
    {
        base.Die();
    }

    private EntitiesManager mEntitiesManager
    {
        get
        {
            return ManagerCenter.Instance.GetManager<EntitiesManager>();
        }
    }

    public override void CreatNpc()
    {
        cs_MapNpcData data = base.NpcData.MapNpcData;
        LuaTable npcConfig = base.GetNpcConfig();
        if (npcConfig == null)
        {
            FFDebug.Log(this, FFLogType.Npc, "CreatBuildingNpc Error npcConfig == null " + data.baseid);
            return;
        }
        FFDebug.Log(this, FFLogType.Npc, string.Concat(new object[]
        {
            "CreatBuildingNpc",
            "....",
            data.name,
            "...",
            data.tempid,
            " ...baseid:",
            data.baseid,
            "...kind:",
            npcConfig.GetField_Uint("kind")
        }));
        base.RecodeSeverMoveData(new cs_MoveData
        {
            pos = new cs_FloatMovePos(data.x, data.y),
            dir = data.dir
        });
        this.RetMoveDataQueue = new Queue<cs_MoveData>();
        string buildingname = npcConfig.GetField_String("model");
        string BoneName = npcConfig.GetField_String("animatorcontroller");
        MapLoader.ConbineMapData(npcConfig.GetField_Uint("id").ToString(), delegate (GameMapInfo buildingInfo)
        {
            PathFindComponent.RefreshMapData();
            MapLoader.LoadBuildingHightDataByName(buildingname + "_height", delegate
            {
                FFAssetBundleRequest.CleverRequest(CharactorAndEffectBundleType.NPCBulilding, buildingname, delegate (FFAssetBundle item)
                {
                    if (item == null)
                    {
                        return;
                    }
                    string user = string.Concat(new string[]
                    {
                        "Scenes/NPCBuilding/" + buildingname + ".u"
                    });
                    item.AddUse(user);
                    GameObject mainAsset = item.GetMainAsset<GameObject>();
                    GameObject gameObject = mainAsset;
                    if (this.CharState == CharactorState.RemoveFromNineScreen)
                    {
                        this.TrueDestroy();
                        return;
                    }
                    if (gameObject == null)
                    {
                        FFDebug.Log(this, FFLogType.Npc, "prefab  " + buildingname + " is null!!");
                        return;
                    }
                    this.ModelObj = UnityEngine.Object.Instantiate<GameObject>(gameObject);
                    Animator animator = this.ModelObj.GetComponent<Animator>();
                    if (animator != null)
                    {
                        ManagerCenter.Instance.GetManager<AnimatorControllerMgr>().GetAnimatorController(BoneName, delegate (RuntimeAnimatorController animatorController)
                        {
                            if (null != animatorController)
                            {
                                animator.cullingMode = AnimatorCullingMode.AlwaysAnimate;
                                animator.runtimeAnimatorController = animatorController;
                                animator.applyRootMotion = false;
                            }
                            else
                            {
                                FFDebug.LogWarning("Create npcbuilding GetAnimatorController", BoneName + " is null");
                            }
                            this.createFinishCallBack(buildingInfo, data);
                        });
                    }
                    else
                    {
                        this.createFinishCallBack(buildingInfo, data);
                    }
                });
            });
        });
    }

    private void createFinishCallBack(GameMapInfo buildingInfo, cs_MapNpcData data)
    {
        Scheduler.Instance.AddUpdator(new Scheduler.OnScheduler(this.Update));
        base.ModelObj.name = string.Format("{0}_{1}_{2}", data.name, data.baseid, this.EID.Id);
        this.animator = base.ModelObj.GetComponent<Animator>();
        base.SetModelSize();
        this.InitComponent();
        if (buildingInfo != null)
        {
            Vector3 worldPosByServerPos = GraphUtils.GetWorldPosByServerPos(buildingInfo.fBuildingPosX, buildingInfo.fBuildingPosY);
            worldPosByServerPos.y = buildingInfo.fBuildingHeight;
            base.ModelObj.transform.position = worldPosByServerPos;
            Vector3 euler = new Vector3(buildingInfo.fBuildingAngleX, buildingInfo.fBuildingAngleY, buildingInfo.fBuildingAngleZ);
            Quaternion rotation = base.ModelObj.transform.rotation;
            base.ModelObj.transform.rotation = Quaternion.Euler(euler);
            base.ServerDir = CommonTools.GetServerDirByClientDir(new Vector2(base.ModelObj.transform.forward.x, base.ModelObj.transform.forward.z));
        }
        else
        {
            FFDebug.LogWarning(this, "CreatNpc Airwall Exception buildingInfo is null");
        }
        base.CharState = CharactorState.CreatComplete;
        this.AddBuffCtrl();
        this.ComponentMgr.InitOver();
    }

    public override void InitComponent()
    {
        base.InitComponent();
        base.AddComponent(new FFBehaviourControl());
        base.Fbc = base.GetComponent<FFBehaviourControl>();
        base.Fbc.PlayNormalAction(1U, false, 0.1f);
    }

    public virtual void CombineBlockData()
    {
        if (base.GetNpcConfig() == null)
        {
            FFDebug.LogWarning(this, string.Format("CombineBlockData Error npcConfig is null id = {0} ", base.NpcData.MapNpcData.baseid));
            return;
        }
        MapLoader.ConbineMapData(base.NpcData.MapNpcData.baseid.ToString(), delegate (GameMapInfo buildingInfo)
        {
            PathFindComponent.RefreshMapData();
        });
    }

    public virtual void RemoveBlockData()
    {
        if (base.GetNpcConfig() == null)
        {
            FFDebug.LogWarning(this, string.Format("RemoveBlockData Error npcConfig is null id = {0} ", base.NpcData.MapNpcData.baseid));
            return;
        }
        MapLoader.SeperateMapData(base.NpcData.MapNpcData.baseid.ToString(), delegate (GameMapInfo buildingInfo)
        {
            PathFindComponent.RefreshMapData();
        });
    }

    public virtual void AddBuffCtrl()
    {
    }

    public new void DestroyModel()
    {
        Scheduler.Instance.RemoveUpdator(new Scheduler.OnScheduler(this.Update));
        GameObject modelObj = base.ModelObj;
        UnityEngine.Object.Destroy(modelObj);
        base.ModelObj = null;
    }

    private void TrueDestroy()
    {
        if (base.Fbc != null)
        {
            this.dieAnimTime = base.Fbc.PlayNormalAction(1000U, true, 0.1f);
        }
        if (this.dieAnimTime == -1f)
        {
            this.DestroyModel();
        }
        else
        {
            this.onDieAnim = true;
            this.lastTime = Time.realtimeSinceStartup;
            this.dieAnimCallback = delegate ()
            {
                this.DestroyModel();
            };
        }
        this.RemoveBlockData();
    }

    private void onDieProgress()
    {
        if (this.onDieAnim)
        {
            float num = Time.realtimeSinceStartup - this.lastTime;
            if (num >= this.dieAnimTime)
            {
                if (this.dieAnimCallback != null)
                {
                    this.dieAnimCallback();
                }
                this.onDieAnim = false;
            }
        }
    }

    public override void DestroyThisInNineScreen()
    {
        if (base.GetNpcConfig() == null)
        {
            FFDebug.Log(this, FFLogType.Npc, "DestroyThisInNineScreen Error  npcConfig == null");
            return;
        }
        if (base.CharState == CharactorState.CreatComplete)
        {
            base.CharState = CharactorState.RemoveFromNineScreen;
            this.TrueDestroy();
        }
        else
        {
            base.CharState = CharactorState.RemoveFromNineScreen;
        }
    }

    private bool onDieAnim;

    private Action dieAnimCallback;

    private float lastTime;

    private float dieAnimTime = 1.2f;
}
