using System;
using System.Collections.Generic;
using Framework.Managers;
using Game.Scene;
using LuaInterface;
using map;
using UnityEngine;

public class Npc_BuildingAirWall : Npc_Building
{
    public Npc_BuildingAirWall()
    {
        base.Init();
    }

    public override void Die()
    {
        base.Die();
        this.PlayAnimation(Npc_BuildingAirWall.AnimState.Die);
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
            npcConfig.GetField_String("kind")
        }));
        base.RecodeSeverMoveData(new cs_MoveData
        {
            pos = new cs_FloatMovePos(data.x, data.y),
            dir = data.dir
        });
        this.RetMoveDataQueue = new Queue<cs_MoveData>();
        string modelname = base.GetNpcConfig().GetField_String("model");
        MapLoader.ConbineMapData(npcConfig.GetField_String("id"), delegate (GameMapInfo buildingInfo)
        {
            PathFindComponent.RefreshMapData();
            MapLoader.LoadBuildingHightDataByName(modelname + "_height", delegate
            {
                string strPath = "Scenes/NPCBuilding/" + modelname + ".u";
                ManagerCenter.Instance.GetManager<GameScene>().sceneData.LoadItemPrefab(strPath, modelname, delegate (UnityEngine.Object o)
                {
                    GameObject gameObject = o as GameObject;
                    if (this.CharState == CharactorState.RemoveFromNineScreen)
                    {
                        this.DestroyModel();
                        return;
                    }
                    if (gameObject == null)
                    {
                        FFDebug.Log(this, FFLogType.Npc, "prefab  " + modelname + " is null!!");
                        return;
                    }
                    this.ModelObj = UnityEngine.Object.Instantiate<GameObject>(gameObject);
                    this.ModelObj.name = string.Format("{0}_{1}_{2}", data.name, data.baseid, this.EID.Id);
                    if (buildingInfo != null)
                    {
                        Vector3 worldPosByServerPos = GraphUtils.GetWorldPosByServerPos(buildingInfo.fBuildingPosX, buildingInfo.fBuildingPosY);
                        worldPosByServerPos.y = buildingInfo.fBuildingHeight;
                        this.ModelObj.transform.position = worldPosByServerPos;
                        Vector3 euler = new Vector3(buildingInfo.fBuildingAngleX, buildingInfo.fBuildingAngleY, buildingInfo.fBuildingAngleZ);
                        this.ModelObj.transform.rotation = Quaternion.Euler(euler);
                        this.ModelObj.transform.SetParent(GameObject.Find("SceneRoot").transform);
                    }
                    else
                    {
                        FFDebug.LogWarning(this, "CreatNpc Airwall Exception buildingInfo is null");
                    }
                    this.CharState = CharactorState.CreatComplete;
                    this.OnLoadModelOver();
                });
            });
        });
    }

    private void OnLoadModelOver()
    {
        this.PlayAnimation(Npc_BuildingAirWall.AnimState.Born);
        Scheduler.Instance.AddTimer(1f, false, delegate
        {
            this.PlayAnimation(Npc_BuildingAirWall.AnimState.ActiveIdle);
        });
    }

    private void PlayAnimation(Npc_BuildingAirWall.AnimState state)
    {
        if (base.ModelObj == null)
        {
            return;
        }
        for (int i = 0; i < base.ModelObj.transform.childCount; i++)
        {
            Transform child = base.ModelObj.transform.GetChild(i);
            switch (state)
            {
                case Npc_BuildingAirWall.AnimState.DeactiveIdle:
                    child.Find("c_deactiveidle").gameObject.SetActive(true);
                    child.Find("c_born").gameObject.SetActive(false);
                    child.Find("c_activeidle").gameObject.SetActive(false);
                    child.Find("c_die").gameObject.SetActive(false);
                    break;
                case Npc_BuildingAirWall.AnimState.Born:
                    child.Find("c_deactiveidle").gameObject.SetActive(false);
                    child.Find("c_born").gameObject.SetActive(true);
                    child.Find("c_activeidle").gameObject.SetActive(false);
                    child.Find("c_die").gameObject.SetActive(false);
                    break;
                case Npc_BuildingAirWall.AnimState.ActiveIdle:
                    child.Find("c_deactiveidle").gameObject.SetActive(false);
                    child.Find("c_born").gameObject.SetActive(false);
                    child.Find("c_activeidle").gameObject.SetActive(true);
                    child.Find("c_die").gameObject.SetActive(false);
                    break;
                case Npc_BuildingAirWall.AnimState.Die:
                    child.Find("c_deactiveidle").gameObject.SetActive(false);
                    child.Find("c_born").gameObject.SetActive(false);
                    child.Find("c_activeidle").gameObject.SetActive(false);
                    child.Find("c_die").gameObject.SetActive(true);
                    break;
            }
        }
    }

    private enum AnimState
    {
        DeactiveIdle,
        Born,
        ActiveIdle,
        Die
    }
}
