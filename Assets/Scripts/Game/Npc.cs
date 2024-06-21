using System;
using System.Collections.Generic;
using System.Xml;
using Framework.Managers;
using Game.Scene;
using LuaInterface;
using magic;
using UnityEngine;

public class Npc : CharactorBase
{
    public Npc()
    {
        this.Init();
    }

    public NpcData NpcData
    {
        get
        {
            return base.GetComponent<NpcData>();
        }
    }

    public override void Init()
    {
        base.Init();
        base.AddComponentImmediate(new NpcData());
        base.AddComponentImmediate(new AttackWarningEffect());
    }

    public LuaTable GetNpcConfig()
    {
        if (this.npcconfig == null)
        {
            cs_MapNpcData mapNpcData = this.NpcData.MapNpcData;
            this.npcconfig = LuaConfigManager.GetConfigTable("npc_data", (ulong)mapNpcData.baseid);
        }
        return this.npcconfig;
    }

    public void SetTargetPos(Vector2 serverpos)
    {
        if (!InputController.ShowTestBtn)
        {
            return;
        }
        if (this.Target == null)
        {
            this.Target = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        }
        Vector3 worldPosByServerPos = GraphUtils.GetWorldPosByServerPos(serverpos);
        worldPosByServerPos.y = base.ModelObj.transform.position.y;
        this.Target.transform.position = worldPosByServerPos;
    }

    public override void InitComponent()
    {
        base.AddComponent(new FFBehaviourControl());
        base.Fbc = base.GetComponent<FFBehaviourControl>();
        base.AddComponent(new FFBipBindMgr());
        base.AddComponent(new FFEffectControl());
        base.AddComponent(new FFMaterialEffectControl());
        base.AddComponent(new PlayerBufferControl());
        base.AddComponent(new SkillHolder());
        base.AddComponent(new FakeHitContorl());
        base.AddComponent(new FlyObjControl());
        base.AddComponent(new PlayerClientStateControl());
        base.AddComponent(new NpcPickCtrl());
        base.AddComponent(new EffectAppearance());
        this.ComponentMgr.InitOver();
        base.InitComponent();
    }

    public void FixMoveSpeed(Vector2 NewPos)
    {
        if (base.ModelObj == null)
        {
            return;
        }
        float num = (NewPos - base.NextPosition2D).magnitude * this.NpcData.MapNpcData.movespeed / 1000f;
        Vector3 worldPosByServerPos = GraphUtils.GetWorldPosByServerPos(NewPos);
        float num2 = Vector3.Distance(CommonTools.DismissYSize(base.ModelObj.transform.position), worldPosByServerPos);
        if (num > 0f)
        {
            base.moveSpeed = num2 / num;
        }
    }

    public virtual void CreatNpc()
    {
        cs_MapNpcData data = this.NpcData.MapNpcData;
        LuaTable configTable = LuaConfigManager.GetConfigTable("npc_data", (ulong)data.baseid);
        if (configTable == null)
        {
            FFDebug.LogWarning(this, "CreatNpc Error npcConfig == null! id = " + data.baseid);
            return;
        }
        data.ApparentBodySize = configTable.GetCacheField_Uint("appparentvolume");
        data.FHitPercent = Const.GetHitPercentage(configTable.GetCacheField_String("bodysize"));
        data.NpcType = (NpcType)configTable.GetField_Uint("kind");
        base.CharState = CharactorState.CreatModel;
        base.RecodeSeverMoveData(new cs_MoveData
        {
            pos = new cs_FloatMovePos(data.x, data.y),
            dir = data.dir
        });
        this.RetMoveDataQueue = new Queue<cs_MoveData>();
        base.BaseData.RefreshCharaBasePosition(data.x, data.y);
        base.NextPosition2D = base.CurrentPosition2D;
        uint appearanceid = this.NpcData.GetAppearanceid();
        uint[] featureIDs = new uint[]
        {
            data.showdata.haircolor,
            data.showdata.hairstyle,
            data.showdata.facestyle,
            data.showdata.antenna
        };
        this.lastCreateId += 1UL;
        FFCharacterModelHold.CreateModel(appearanceid, data.showdata.bodystyle, featureIDs, this.NpcData.GetACname(), delegate (PlayerCharactorCreateHelper modelHoldHelper)
        {
            if (modelHoldHelper != null && this.lastCreateId != modelHoldHelper.keyid)
            {
                modelHoldHelper.DisposeBonePObj();
                modelHoldHelper = null;
                return;
            }
            if (this.ModelHoldHelper != null)
            {
                this.DestroyModel();
            }
            this.ModelHoldHelper = modelHoldHelper;
            this.ModelObj = this.ModelHoldHelper.rootObj;
            Vector3 eulerAngles = this.ModelObj.transform.rotation.eulerAngles;
            eulerAngles.y = data.dir * 2U;
            this.ModelObj.transform.rotation = Quaternion.Euler(eulerAngles);
            this.CreateModelFinishCb();
        }, null, this.lastCreateId);
    }

    public virtual void CreateModelFinishCb()
    {
        base.ModelObj.name = string.Concat(new object[]
        {
            "NPC",
            this.NpcData.MapNpcData.baseid,
            this.NpcData.MapNpcData.name,
            this.NpcData.MapNpcData.tempid
        });
        this.animator = base.ModelObj.GetComponent<Animator>();
        this.SetModelSize();
        this.ResetModelRenderInfo();
        this.SetCollider();
        if (base.CharState == CharactorState.RemoveFromNineScreen)
        {
            this.TrueDestoryModel();
            return;
        }
        Scheduler.Instance.AddUpdator(new Scheduler.OnScheduler(this.Update));
        this.InitComponent();
        this.RefreshNPC();
        base.CharState = CharactorState.CreatComplete;
        this.SetHpdata();
        this.getNpcPosY();
        FFMaterialEffectControl component = base.GetComponent<FFMaterialEffectControl>();
        component.ResetRoleMaterial();
        component.SetRoleFloat("_RJ", 0f);
        base.SetPlayerPosition(new Vector2(this.NpcData.MapNpcData.x, this.NpcData.MapNpcData.y), this.NpcData.MapNpcData.dir);
        ManagerCenter.Instance.GetManager<EntitiesManager>().RunAllEentityActionCacheAndClear(this.EID);
    }

    public static XmlNodeList GetMapDataXmlNodeList(uint mapID)
    {
        if (Npc.m_CurrMapId != mapID || Npc.m_CurrMapList == null)
        {
            Npc.m_CurrMapId = mapID;
            LuaTable cacheField_Table = LuaConfigManager.GetXmlConfigTable("scenesinfo").GetCacheField_Table("mapinfo");
            mapinfo mapinfo = new mapinfo(cacheField_Table.GetCacheField_Table(mapID.ToString()));
            string str = mapinfo.xmlName();
            string url = Application.streamingAssetsPath + "/Scenes/MapData/" + str + ".xml";
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(XmlReader.Create(url, new XmlReaderSettings
            {
                IgnoreComments = true
            }));
            Npc.m_CurrMapList = xmlDocument.SelectSingleNode("map").ChildNodes;
        }
        return Npc.m_CurrMapList;
    }

    private void getNpcPosY()
    {
        uint mapID = ManagerCenter.Instance.GetManager<GameScene>().CurrentSceneData.mapID();
        XmlNodeList mapDataXmlNodeList = Npc.GetMapDataXmlNodeList(mapID);
        string b = this.NpcData.MapNpcData.baseid.ToString();
        foreach (object obj in mapDataXmlNodeList)
        {
            XmlElement xmlElement = (XmlElement)obj;
            if (xmlElement.Name == "npc" && xmlElement.GetAttribute("id") == b)
            {
                string attribute = xmlElement.GetAttribute("z");
                if (!string.IsNullOrEmpty(attribute))
                {
                    float num = float.Parse(attribute);
                    if (num > 0f)
                    {
                        this.CustomPosY = num;
                    }
                    break;
                }
            }
        }
    }

    public void RefreshNpcMapUserData()
    {
        this.RefreshModelAndAnimatorCnotroller();
    }

    private void RefreshModelFinishCb()
    {
        base.ModelObj.name = string.Concat(new object[]
        {
            "NPC",
            this.NpcData.MapNpcData.baseid,
            this.NpcData.MapNpcData.name,
            this.NpcData.MapNpcData.tempid
        });
        this.animator = base.ModelObj.GetComponent<Animator>();
        this.SetModelSize();
        this.SetCollider();
        base.ResetAllCompment();
        Transform bindPoint = base.GetComponent<FFBipBindMgr>().GetBindPoint("Top");
        if (this.hpdata != null)
        {
            this.hpdata.SetTarget(base.ModelObj, bindPoint);
        }
        if (this.IsSelect)
        {
            this.TargetSelect();
        }
        FFMaterialEffectControl component = base.GetComponent<FFMaterialEffectControl>();
        component.ResetRoleMaterial();
        component.SetRoleFloat("_RJ", 0f);
        base.SetPlayerPosition(new Vector2(this.NpcData.MapNpcData.x, this.NpcData.MapNpcData.y), this.NpcData.MapNpcData.dir);
        this.ResetModelRenderInfo();
        if (!base.IsLive)
        {
            base.PlayAniByState(UserState.USTATE_DEATH);
        }
        if (base.CharState == CharactorState.RemoveFromNineScreen)
        {
            this.TrueDestoryModel();
            return;
        }
    }

    public void RefreshModelAndAnimatorCnotroller()
    {
        if (base.CharState != CharactorState.CreatComplete)
        {
            return;
        }
        this.RefreshNpc();
    }

    private void RefreshNpc()
    {
        uint appearanceid = this.NpcData.GetAppearanceid();
        uint[] featureIDs = new uint[]
        {
            this.NpcData.MapNpcData.showdata.haircolor,
            this.NpcData.MapNpcData.showdata.hairstyle,
            this.NpcData.MapNpcData.showdata.facestyle,
            this.NpcData.MapNpcData.showdata.antenna
        };
        this.lastCreateId += 1UL;
        FFCharacterModelHold.CreateModel(appearanceid, this.NpcData.MapNpcData.showdata.bodystyle, featureIDs, this.NpcData.GetACname(), delegate (PlayerCharactorCreateHelper modelHoldHelper)
        {
            if (modelHoldHelper != null && this.lastCreateId != modelHoldHelper.keyid)
            {
                modelHoldHelper.DisposeBonePObj();
                modelHoldHelper = null;
                return;
            }
            if (this.ModelHoldHelper != null)
            {
                this.DestroyModel();
            }
            this.ModelHoldHelper = modelHoldHelper;
            base.ModelObj = this.ModelHoldHelper.rootObj;
            this.RefreshModelFinishCb();
        }, null, this.lastCreateId);
    }

    protected void SetModelSize()
    {
        if (base.ModelObj != null)
        {
            base.ModelObj.transform.localScale = base.BaseData.GetModelSize();
        }
    }

    private void SetCollider()
    {
        if (base.ModelObj != null)
        {
            CommonTools.SetGameObjectLayer(base.ModelObj, "Charactor", true);
            CapsuleCollider capsuleCollider = base.ModelObj.GetComponent<CapsuleCollider>();
            if (capsuleCollider == null)
            {
                capsuleCollider = base.ModelObj.AddComponent<CapsuleCollider>();
            }
            capsuleCollider.center = new Vector3(0f, 1f, 0f);
            capsuleCollider.radius = 0.5f;
            capsuleCollider.height = 2f;
            cs_MapNpcData mapNpcData = this.NpcData.MapNpcData;
            LuaTable configTable = LuaConfigManager.GetConfigTable("npc_data", (ulong)mapNpcData.baseid);
            if (configTable != null)
            {
                string cacheField_String = configTable.GetCacheField_String("CapsuleCollider");
                if (!string.IsNullOrEmpty(cacheField_String))
                {
                    string[] array = cacheField_String.Split(new char[]
                    {
                        '|'
                    });
                    if (array.Length >= 6)
                    {
                        capsuleCollider.center = new Vector3(float.Parse(array[0]), float.Parse(array[1]), float.Parse(array[2]));
                        capsuleCollider.radius = float.Parse(array[3]);
                        capsuleCollider.height = float.Parse(array[4]);
                        capsuleCollider.direction = int.Parse(array[5]);
                    }
                }
            }
        }
    }

    private void CheckHasStateBirth(Action callback)
    {
        cs_MapNpcData mapNpcData = this.NpcData.MapNpcData;
        if (mapNpcData.bBirth)
        {
            FFBehaviourState_Birth @object = ClassPool.GetObject<FFBehaviourState_Birth>();
            @object.OnBirthOver = delegate ()
            {
                if (callback != null)
                {
                    callback();
                }
            };
            base.Fbc.ChangeState(@object);
        }
        else
        {
            if (callback != null)
            {
                callback();
            }
            if (base.Fbc.CurrState == null)
            {
                base.Fbc.ChangeState(ClassPool.GetObject<FFBehaviourState_Idle>());
            }
        }
    }

    public void SetHpdata()
    {
        this.CheckHasStateBirth(delegate
        {
            Transform top = base.GetComponent<FFBipBindMgr>().GetBindPoint("Top");
            cs_MapNpcData data = this.NpcData.MapNpcData;
            LuaTable configTable = LuaConfigManager.GetConfigTable("npc_data", (ulong)data.baseid);
            if (this.hpdata == null && configTable.GetField_Uint("not_show_name") == 0U)
            {
                this.InitMosterHpDate(top, data, delegate
                {
                    this.hpdata.ResetData(data);
                    this.hpdata.RefreshModel();
                    this.hpdata.SetTarget(this.ModelObj, top);
                });
            }
            if (configTable.GetField_Uint("show_shadow") == 1U)
            {
                base.ModelObj.SetLayer(Const.Layer.NpcNoShadow, true);
            }
            else
            {
                base.ModelObj.SetLayer(Const.Layer.NpcShadow, true);
            }
        });
    }

    private void InitMosterHpDate(Transform tran, cs_MapNpcData data, Action callback)
    {
        ControllerManager.Instance.GetController<UIHpSystem>().CreateMosterHp(this, tran, data, delegate (HpStruct hpd)
        {
            this.hpdata = hpd;
            if (this is Npc_Assist)
            {
                this.hpdata.SetAssistTag(true);
            }
            else
            {
                this.hpdata.SetAssistTag(false);
            }
            if (callback != null)
            {
                callback();
            }
        });
    }

    private void ResetModelRenderInfo()
    {
        if (ManagerCenter.Instance.GetManager<GameScene>().LightInfo != null)
        {
            ManagerCenter.Instance.GetManager<GameScene>().SetMatLightInfo(base.ModelObj, false);
        }
        ShadowManager.ResetRenderQueue(base.ModelObj, Const.RenderQueue.SceneObjAfterCharactor);
        base.ReloadShader(base.ModelObj);
    }

    public virtual void Update()
    {
        base.Moving();
        base.Rotatint();
        if (this.hpdata != null)
        {
            this.hpdata.Refresh();
        }
        if (this.ComponentMgr != null)
        {
            this.ComponentMgr.Update();
        }
        this.dissolve();
    }

    private void dissolve()
    {
        if (this.bdissolve)
        {
            float num = Time.realtimeSinceStartup - this.lastTime;
            float num2 = num / this.dissolveTime * this._RJ;
            base.GetComponent<FFMaterialEffectControl>().SetRoleFloat("_RJ", num2);
            base.GetComponent<FFMaterialEffectControl>().SetWeaponFloat("_RJ", num2);
            if (num >= this.dissolveTime && this.dissloveCallback != null)
            {
                this.dissloveCallback();
            }
            if (base.ModelObj != null && base.ModelObj.layer != Const.Layer.NpcNoShadow)
            {
                base.ModelObj.SetLayer(Const.Layer.NpcNoShadow, true);
            }
        }
    }

    private void ResetNpcDissolve()
    {
        this.dissloveCallback = null;
        this.bdissolve = false;
        this.lastTime = 0f;
    }

    public override void Die()
    {
        if (this.hpdata != null)
        {
            this.hpdata.ResetHp(0f);
        }
        base.Die();
    }

    public override void DelayRelive()
    {
    }

    public void RefreshNPC()
    {
        cs_MapNpcData mapNpcData = this.NpcData.MapNpcData;
        cs_FloatMovePos pos = default(cs_FloatMovePos);
        pos.fx = mapNpcData.x;
        pos.fy = mapNpcData.y;
        if (base.IsBufferNoMove && this.HasBeControlledBy(BufferState.ControlType.ForceMove))
        {
            base.SetPlayerDirection(mapNpcData.dir, true);
            base.FastMoveTo(new Vector2(pos.fx, pos.fy), 0.2f, 0f, 0f);
        }
        else
        {
            base.SetPlayerPosition(pos, mapNpcData.dir);
        }
        base.moveSpeed = 1f / (mapNpcData.movespeed / 1000f);
        PlayerBufferControl component = base.GetComponent<PlayerBufferControl>();
        if (component != null)
        {
            component.AddStateByArray(this.NpcData.MapNpcData.lstState);
        }
        if (this.hpdata != null)
        {
            this.hpdata.ResetData(mapNpcData);
        }
    }

    public override void DestroyThisInNineScreen()
    {
        UIMapController controller = ControllerManager.Instance.GetController<UIMapController>();
        if (controller != null)
        {
            controller.DeletePanelNPCIcon(this);
        }
        LuaTable configTable = LuaConfigManager.GetConfigTable("npc_data", (ulong)this.NpcData.MapNpcData.baseid);
        this.CancelSelect();
        if (configTable == null)
        {
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
        if (this.Target != null)
        {
            UnityEngine.Object.Destroy(this.Target);
        }
    }

    private void TrueDestroy()
    {
        base.StopMoveImmediate(delegate
        {
            LuaTable configTable = LuaConfigManager.GetConfigTable("npc_data", (ulong)this.NpcData.MapNpcData.baseid);
            if (!base.IsLive && (configTable.GetField_Uint("kind") == 2U || configTable.GetField_Uint("kind") == 3U || configTable.GetField_Uint("kind") == 4U || configTable.GetField_Uint("kind") == 5U || configTable.GetField_Uint("kind") == 22U))
            {
                this.bdissolve = true;
                this.lastTime = Time.realtimeSinceStartup;
                this.dissloveCallback = delegate ()
                {
                    this.TrueDestoryModel();
                };
            }
            else
            {
                this.TrueDestoryModel();
            }
        });
    }

    private void TrueDestoryModel()
    {
        base.DestroyThisInNineScreen();
        if (this.hpdata != null)
        {
            this.hpdata.Distory();
            this.hpdata = null;
        }
        Scheduler.Instance.RemoveUpdator(new Scheduler.OnScheduler(this.Update));
        this.RetMoveDataQueue.Clear();
        this.DestroyModel();
        this.ResetNpcDissolve();
        if (MainPlayer.Self != null)
        {
            MainPlayerTargetSelectMgr component = MainPlayer.Self.GetComponent<MainPlayerTargetSelectMgr>();
            if (component != null && component.manualSelect.Target == this)
            {
                component.SetTargetNull();
            }
        }
    }

    public void DestroyModel()
    {
        if (this.Target != null)
        {
            UnityEngine.Object.Destroy(this.Target);
        }
        FFMaterialEffectControl component = base.GetComponent<FFMaterialEffectControl>();
        if (component != null)
        {
            component.ClearRoleMaterial();
        }
        if (this.ModelHoldHelper != null)
        {
            this.ModelHoldHelper.DisposeBonePObj();
            this.ModelHoldHelper = null;
        }
        base.ModelObj = null;
        this.animator = null;
    }

    public override void RevertHpMp(MSG_Ret_HpMpPop_SC mdata)
    {
        base.RevertHpMp(mdata);
        if (this.hpdata != null)
        {
            this.hpdata.RevertOrBleedChangeHp(mdata.hp, (float)mdata.hp_change, mdata.force);
        }
        this.NpcData.MapNpcData.hp = mdata.hp;
    }

    public override void CancelSelect()
    {
        if (!this.IsSelect)
        {
            return;
        }
        base.CancelSelect();
        if (this.hpdata != null)
        {
            this.hpdata.SetSelect(false);
        }
        if (MainPlayer.Self != null)
        {
            MainPlayer.Self.GetComponent<MainPlayerTargetSelectMgr>().CancelNpcSelect(this);
        }
    }

    public override void TargetSelect()
    {
        base.TargetSelect();
        if (this.hpdata != null)
        {
            this.hpdata.SetSelect(true);
        }
        MainUIController controller = ControllerManager.Instance.GetController<MainUIController>();
        if (controller != null)
        {
            controller.ShowTargetInfo(this.EID, false, null);
        }
    }

    public void ShowTopBtn()
    {
        if (this.hpdata == null)
        {
            return;
        }
        if (this._isOpenTopBtn)
        {
            return;
        }
        LuaTable configTable = LuaConfigManager.GetConfigTable("npc_data", (ulong)this.NpcData.MapNpcData.baseid);
        if (configTable == null)
        {
            return;
        }
        if (MainPlayer.Self.GetComponent<FFDetectionNpcControl>() == null)
        {
            return;
        }
        NpcType cacheField_Uint = (NpcType)configTable.GetCacheField_Uint("kind");
        if (cacheField_Uint == NpcType.NPC_TYPE_VISIT)
        {
            if (this._showPriority == -1 || this._showPriority == 100)
            {
                return;
            }
            this.TryUpdateMapIcon(false);
            string topIconNameByState = this.GetTopIconNameByState(configTable.GetField_String("visiticon"), this._showPriority);
            this.hpdata.ShowNPCTopButton(topIconNameByState);
            this._isOpenTopBtn = true;
        }
        else if (cacheField_Uint == NpcType.NPC_TYPE_HOLDON)
        {
            this.hpdata.ShowNPCTopButton(configTable.GetField_String("visiticon"));
            this._isOpenTopBtn = true;
        }
        else if (cacheField_Uint == NpcType.NPC_TYPE_GATHER)
        {
            this.AddCaijiEffect();
        }
        else if (cacheField_Uint == NpcType.NPC_TYPE_QUESTGATHER && this is Npc_TaskCollect)
        {
            bool flag = (this as Npc_TaskCollect).CheckStateContainDoing();
            if (flag)
            {
                this.AddCaijiEffect();
            }
        }
    }

    private void AddCaijiEffect()
    {
        string caijiEffectName = "lz_cjw_glow";
        Transform feet = base.GetComponent<FFBipBindMgr>().GetBindPoint("Feet");
        if (feet == null)
        {
            return;
        }
        ManagerCenter.Instance.GetManager<FFEffectManager>().LoadEffobj(caijiEffectName, delegate
        {
            if (feet && feet.Find(caijiEffectName) == null)
            {
                ObjectPool<EffectObjInPool> effobj = ManagerCenter.Instance.GetManager<FFEffectManager>().GetEffobj(caijiEffectName);
                if (effobj != null)
                {
                    effobj.GetItemFromPool(delegate (EffectObjInPool OIP)
                    {
                        ManagerCenter.Instance.GetManager<FFEffectManager>().SetObjectPoolUnit(0UL, caijiEffectName, OIP);
                        this.ClearCaijiEffect();
                        this._caijiEffect = OIP.ItemObj;
                        this._caijiEffect.name = caijiEffectName;
                        this._caijiEffect.SetLayer(Const.Layer.Effect, true);
                        this._caijiEffect.transform.SetParent(feet);
                        this._caijiEffect.transform.localPosition = Vector3.zero;
                    });
                }
            }
        });
    }

    private void ClearCaijiEffect()
    {
        if (this._caijiEffect != null)
        {
            UnityEngine.Object.Destroy(this._caijiEffect);
        }
    }

    private string GetTopIconNameByState(string defaultIcon, int showPriority)
    {
        if (showPriority == 4)
        {
            return "visit_chat";
        }
        if (showPriority == 3)
        {
            return "ic0145";
        }
        if (showPriority == 2)
        {
            return "ic0144";
        }
        if (showPriority == 1)
        {
            return "ic0143";
        }
        if (string.IsNullOrEmpty(defaultIcon))
        {
            defaultIcon = "visit_chat";
        }
        return defaultIcon;
    }

    public void UpdateTaskState(int priority)
    {
    }

    public void CloseTopBtn(bool isOutOfRange)
    {
        if (this.hpdata == null)
        {
            return;
        }
        if (!this._isOpenTopBtn)
        {
            return;
        }
        this._isOpenTopBtn = false;
        this.hpdata.CloseNPCTopButton();
    }

    public override void OnUpdateCharacterBuff(UserState newState)
    {
        if (newState == UserState.USTATE_NOSTATE)
        {
            this._showPriority = 100;
        }
        else if (newState == UserState.USTATE_TIPS)
        {
            PlayerBufferControl component = base.GetComponent<PlayerBufferControl>();
            if (component != null && component.HasNoTask())
            {
                this._showPriority = 0;
            }
        }
        else if (newState == UserState.USTATE_QUEST_START)
        {
            this._showPriority = 1;
        }
        else if (newState == UserState.USTATE_QUEST_DOING)
        {
            this._showPriority = 2;
        }
        else if (newState == UserState.USTATE_QUEST_FINISH)
        {
            this._showPriority = 3;
        }
        else
        {
            if (newState != UserState.USTATE_QUEST_TALK)
            {
                return;
            }
            this._showPriority = 4;
        }
        if (ControllerManager.Instance.GetController<GuideController>().CheckShowNpc(this.NpcData.MapNpcData.baseid))
        {
            this.EnableGuideEffect(true, this.EID.Id);
        }
        else
        {
            this.EnableGuideEffect(false, this.EID.Id);
        }
        this.TryUpdateMapIcon(true);
        if (this.hpdata != null)
        {
            this.hpdata.TryShowTopButton();
        }
        this.ClearCaijiEffect();
    }

    public void TryUpdateMapIcon(bool checkType)
    {
        if (checkType)
        {
            LuaTable configTable = LuaConfigManager.GetConfigTable("npc_data", (ulong)this.NpcData.MapNpcData.baseid);
            if (configTable == null)
            {
                return;
            }
            NpcType field_Uint = (NpcType)configTable.GetField_Uint("kind");
            if (field_Uint != NpcType.NPC_TYPE_VISIT)
            {
                return;
            }
        }
        UIMapController controller = ControllerManager.Instance.GetController<UIMapController>();
        controller.SetPanelNPCIcon(this, this._showPriority);
    }

    public override bool IsShowHitAnim(int hpChange)
    {
        bool result = false;
        if (hpChange >= 0)
        {
            return result;
        }
        cs_MapNpcData mapNpcData = this.NpcData.MapNpcData;
        if (mapNpcData == null)
        {
            FFDebug.LogWarning(this, string.Format("Npc.IsShowHitAnim NpcData.MapNpcData is null !!! npc Eid = {0}", this.EID.Id));
            return result;
        }
        if (mapNpcData.maxhp == 0U)
        {
            FFDebug.LogWarning(this, string.Format("Npc.IsShowHitAnim maxHp == 0 !!! npc id = {0}", mapNpcData.baseid));
            return result;
        }
        float num = Mathf.Abs((float)hpChange / mapNpcData.maxhp);
        if (num >= mapNpcData.FHitPercent)
        {
            result = true;
        }
        return result;
    }

    public bool IsMapShowNpc()
    {
        uint baseid = this.NpcData.MapNpcData.baseid;
        LuaTable configTable = LuaConfigManager.GetConfigTable("npc_data", (ulong)baseid);
        if (configTable != null)
        {
            NpcType field_Uint = (NpcType)configTable.GetField_Uint("kind");
            if (field_Uint == NpcType.NPC_TYPE_NOACTIVE || field_Uint == NpcType.NPC_TYPE_ACTIVE || field_Uint == NpcType.NPC_TYPE_MONSTER || field_Uint == NpcType.NPC_TYPE_BOSS)
            {
                return false;
            }
        }
        return true;
    }

    public override void OnRelationChange()
    {
        if (!this.IsSelect)
        {
            return;
        }
        MainUIController controller = ControllerManager.Instance.GetController<MainUIController>();
        RelationType type = ManagerCenter.Instance.GetManager<EntitiesManager>().CheckRelationBaseMainPlayer(this);
        controller.UpdateTargetRelation(type, false);
    }

    public override void HitOther(MSG_Ret_MagicAttack_SC mdata, CharactorBase[] BeNHits)
    {
        base.HitOther(mdata, BeNHits);
        LuaTable luaTable = ManagerCenter.Instance.GetManager<SkillManager>().Gett_skill_stage_config(mdata.skillstage);
        if (luaTable != null)
        {
            int selectindex = (!base.IsFly) ? 0 : 1;
            FFActionClip ffactionClip = ManagerCenter.Instance.GetManager<FFActionClipManager>().GetFFActionClip(base.animatorControllerName, luaTable.GetField_Uint("ActionID"), selectindex);
            if (null != ffactionClip)
            {
                string[] effectsByGroupID = ffactionClip.GetEffectsByGroupID(FFActionClip.EffectType.Type_Fly, 1U);
                if (effectsByGroupID.Length > 0)
                {
                    FlyObjControl component = base.GetComponent<FlyObjControl>();
                    if (component != null)
                    {
                        Vector3 pos = GraphUtils.GetWorldPosByServerPos(new Vector2(mdata.desx, mdata.desy));
                        pos = base.GetCharactorY(pos);
                        component.AddFlyObjArray(effectsByGroupID, pos, FlyObjConfig.LaunchType.ByHit);
                    }
                }
                string[] effectsByGroupID2 = ffactionClip.GetEffectsByGroupID(FFActionClip.EffectType.Type_Hit, 1U);
                if (effectsByGroupID2.Length > 0)
                {
                    FFEffectControl component2 = base.GetComponent<FFEffectControl>();
                    Vector3 vector = GraphUtils.GetWorldPosByServerPos(new Vector2(mdata.desx, mdata.desy));
                    vector = base.GetCharactorY(vector);
                    for (int i = 0; i < effectsByGroupID2.Length; i++)
                    {
                        EffectClip clip = ManagerCenter.Instance.GetManager<FFEffectManager>().GetClip(effectsByGroupID2[i]);
                        if (!(clip == null))
                        {
                            if (clip.IsPointPosition)
                            {
                                component2.AddEffect(effectsByGroupID2, vector);
                            }
                        }
                    }
                }
            }
        }
    }

    public override void HandleHit(MSG_Ret_MagicAttack_SC mdata)
    {
        FakeHitContorl component = base.GetComponent<FakeHitContorl>();
        if (component != null)
        {
            component.GetHit(mdata);
        }
    }

    public void InitShowState()
    {
        bool showState = this.CheckIsShowByTask(true);
        this.SetShowState(showState);
    }

    public bool CheckIsShowByTask(bool isForceReCheck = false)
    {
        PlayerClientStateControl component = base.GetComponent<PlayerClientStateControl>();
        if (component != null && !isForceReCheck && component.mPlayerState != PlayerShowState.none)
        {
            return component.mPlayerState == PlayerShowState.show;
        }
        if (this.NpcData == null)
        {
            FFDebug.LogError(this, "NpcData == null");
            return false;
        }
        bool result = false;
        uint baseOrHeroId = this.NpcData.GetBaseOrHeroId();
        if (this.IsThisNPCShow == null)
        {
            this.IsThisNPCShow = LuaScriptMgr.Instance.GetLuaFunction("NpcTalkAndTaskDlgCtrl.IsThisNPCShow");
        }
        if (this.TaskUICtrl == null)
        {
            this.TaskUICtrl = Util.GetLuaTable("NpcTalkAndTaskDlgCtrl");
        }
        if (this.IsThisNPCShow != null && this.TaskUICtrl != null)
        {
            object[] array = this.IsThisNPCShow.Call(new object[]
            {
                this.TaskUICtrl,
                baseOrHeroId
            });
            if (array != null && array.Length >= 0)
            {
                result = bool.Parse(array[0].ToString());
            }
        }
        return result;
    }

    public void SetShowState(bool isShow)
    {
        if (this.NpcData == null)
        {
            FFDebug.LogError(this, "NpcData == null");
            return;
        }
        PlayerClientStateControl component = base.GetComponent<PlayerClientStateControl>();
        if (component == null)
        {
            FFDebug.LogWarning(this, "This npc does not have PlayerClientStateControl !!! id = " + this.NpcData.GetBaseOrHeroId());
            return;
        }
        if (isShow)
        {
            component.SetPlayerState(PlayerShowState.show);
        }
        else
        {
            component.SetPlayerState(PlayerShowState.hide);
        }
    }

    public void onAddBehaviourControl()
    {
        base.Fbc.ResetComp();
    }

    public void EnableGuideEffect(bool active, ulong npcTempId)
    {
        if (active)
        {
            if (this.isShowGuideEffect)
            {
                return;
            }
            this.ShowGuideEffect();
        }
        else
        {
            this.isShowGuideEffect = false;
            this.HideGuideEffect();
        }
    }

    public void ShowGuideEffect()
    {
        if (this._guideEffect != null)
        {
            this._guideEffect.effobj_.SetActive(true);
        }
        else
        {
            this.isShowGuideEffect = true;
            Transform effectBindPoint = this.hpdata.GetEffectBindPoint();
            FFEffectControl component = base.GetComponent<FFEffectControl>();
            if (effectBindPoint)
            {
                this._guideEffect = component.AddEffect("gn_djyd_jiantou", effectBindPoint, null);
            }
        }
    }

    public void HideGuideEffect()
    {
        if (this._guideEffect != null)
        {
            this._guideEffect.SetEffectOver();
        }
    }

    public GameObject Target;

    private LuaTable npcconfig;

    private static XmlNodeList m_CurrMapList;

    private static uint m_CurrMapId;

    public PlayerCharactorCreateHelper ModelHoldHelper;

    private float dissolveTime = 1.2f;

    private float _RJ = 1.2f;

    private bool bdissolve;

    private Action dissloveCallback;

    private float lastTime;

    private bool _isOpenTopBtn;

    private GameObject _caijiEffect;

    private int _showPriority = -1;

    private UserState _laskTaskState;

    private LuaFunction IsThisNPCShow;

    private LuaTable TaskUICtrl;

    private bool isShowGuideEffect;

    private FFeffect _guideEffect;
}
