using System;
using System.Collections.Generic;
using AudioStudio;
using Framework.Managers;
using Game.Scene;
using LuaInterface;
using magic;
using Net;
using UnityEngine;

public class OtherPlayer : CharactorBase
{
    public OtherPlayer()
    {
        base.CharState = CharactorState.CreatEntity;
        this.Init();
    }

    public OtherPlayerData OtherPlayerData
    {
        get
        {
            return base.GetComponent<OtherPlayerData>();
        }
    }

    public bool InBattleState
    {
        get
        {
            PlayerBufferControl component = base.GetComponent<PlayerBufferControl>();
            return component != null && component.ContainsState(UserState.USTATE_BATTLE);
        }
    }

    public override void Init()
    {
        base.Init();
        base.AddComponentImmediate(new OtherPlayerData());
    }

    public ulong GetCharID()
    {
        return this.OtherPlayerData.MapUserData.charid;
    }

    public uint GetOccupation()
    {
        return this.OtherPlayerData.MapUserData.mapshow.occupation;
    }

    public uint GetCurLevel()
    {
        return this.OtherPlayerData.MapUserData.mapdata.level;
    }

    public string GetModePath(uint occupation)
    {
        return "Characters/play01";
    }

    public string GetAnimatorControllerPath(uint occupation)
    {
        return "Characters/AnimatorController/AC_Player1";
    }

    public void RefreshPlayerMapUserData()
    {
        base.moveSpeed = 1f / (this.OtherPlayerData.MapUserData.mapdata.movespeed / 1000f);
        if (this.hpdata != null)
        {
            this.hpdata.ResetData(this.OtherPlayerData.MapUserData);
        }
        if (ControllerManager.Instance.GetController<MainUIController>().mainView != null && this is MainPlayer)
        {
            ControllerManager.Instance.GetController<MainUIController>().ResetMainPlayerHp(this.OtherPlayerData.MapUserData.mapdata.hp, this.OtherPlayerData.MapUserData.mapdata.maxhp);
        }
        ControllerManager.Instance.GetController<UIHpSystem>().ResetPKModel();
    }

    public virtual void CreatPlayerModel()
    {
        cs_MapUserData data = this.OtherPlayerData.MapUserData;
        this.RetMoveDataQueue = new Queue<cs_MoveData>();
        this.AStarPathDataQueue = new Queue<cs_MoveData>();
        base.CharState = CharactorState.CreatModel;
        cs_MoveData cs_MoveData = new cs_MoveData();
        cs_MoveData.pos = data.mapdata.pos;
        cs_MoveData.dir = data.mapdata.dir;
        base.ServerDir = data.mapdata.dir;
        base.RecodeSeverMoveData(cs_MoveData);
        uint num = data.mapshow.bodystyle;
        uint[] array;
        if (data.mapshow.coat > 0U)
        {
            num = data.mapshow.coat;
            array = new uint[4];
        }
        else
        {
            array = new uint[]
            {
                data.mapshow.haircolor,
                data.mapshow.hairstyle,
                data.mapshow.facestyle,
                data.mapshow.antenna
            };
        }
        uint appearanceid = this.OtherPlayerData.GetAppearanceid();
        this.lastCreateId += 1UL;
        if (!FFCharacterModelHold.CreateModel(appearanceid, num, array, this.OtherPlayerData.GetACname(), delegate (PlayerCharactorCreateHelper modelHoldHelper)
        {
            if (modelHoldHelper != null && this.lastCreateId != modelHoldHelper.keyid)
            {
                modelHoldHelper.DisposeBonePObj();
                modelHoldHelper = null;
                return;
            }
            this.playingTime = 0f;
            if (null != this.animator)
            {
                this.playingTime = this.animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
            }
            if (this.ModelHoldHelper != null)
            {
                this.DestroyModel();
            }
            this.ModelHoldHelper = modelHoldHelper;
            this.ModelObj = this.ModelHoldHelper.rootObj;
            AnimationSound animationSound = this.ModelObj.GetComponent<AnimationSound>();
            if (animationSound != null)
            {
                animationSound.SetEffectData(this);
            }
            else
            {
                animationSound = this.ModelObj.AddComponent<AnimationSound>();
            }
            if (MainPlayer.Self != null && data.charid == MainPlayer.Self.OtherPlayerData.MapUserData.charid)
            {
                animationSound.PlayerControl = true;
            }
            this.CreateModelFinishCb();
        }, null, this.lastCreateId))
        {
            FFDebug.LogError(this, "FFCharacterModelHold.CreateModel Return false");
            FFDebug.LogError("CreateModel", string.Format("heroid:{0} bodyid:{1} featureIDs:{2} {3} {4} {5}", new object[]
            {
                appearanceid,
                num,
                array[0],
                array[1],
                array[2],
                array[3]
            }));
        }
    }

    protected override void createMirror(float mirrorTotalTime, float mirrorFadeSpeed)
    {
        if (null == this.mirror)
        {
            this.mirror = base.ModelObj.AddComponent<CharactorMirror>();
        }
        this.mirror.TotalTime = mirrorTotalTime;
        this.mirror.FadeSpeed = mirrorFadeSpeed;
        this.mirror.Begin();
    }

    private void CreateModelFinishCb()
    {
        if (!this.onModelCreateFinish())
        {
            return;
        }
        if (!this.isCommponentInited)
        {
            this.InitComponent();
        }
        Scheduler.Instance.AddUpdator(new Scheduler.OnScheduler(this.Update));
        this.InitHpData();
        base.SetPlayerDirection(this.OtherPlayerData.MapUserData.mapdata.dir, true);
        this.RefreshPlayerMapUserData();
    }

    private bool onModelCreateFinish()
    {
        if (this.onModelCreate != null)
        {
            this.onModelCreate();
            this.onModelCreate = null;
        }
        this.animator = base.ModelObj.GetComponent<Animator>();
        base.ResetAllCompment();
        if (this is MainPlayer && null != Camera.main)
        {
            FxPro component = Camera.main.gameObject.GetComponent<FxPro>();
            if (null != component)
            {
                component.DOFParams.Target = base.ModelObj.transform;
                component.DOFParams.EffectCamera = Camera.main;
            }
        }
        if (this is MainPlayer)
        {
            this.InitCamera();
        }
        if (this.OtherPlayerData != null)
        {
            base.ModelObj.name = this.OtherPlayerData.MapUserData.name + "-" + this.OtherPlayerData.MapUserData.charid;
        }
        if (base.CharState == CharactorState.RemoveFromNineScreen)
        {
            this.TrueDestroy();
            return false;
        }
        base.CharState = CharactorState.CreatComplete;
        this.SetModelSize();
        this.SetCollider();
        this.ResetModelRenderInfo();
        if (this.hpdata != null)
        {
            this.hpdata.RefreshModel();
        }
        FFBipBindMgr component2 = base.GetComponent<FFBipBindMgr>();
        if (component2 != null)
        {
            Transform bindPoint = component2.GetBindPoint("Top");
            if (this.hpdata != null)
            {
                this.hpdata.SetTarget(base.ModelObj, bindPoint);
            }
        }
        base.SetPlayerPosition(new cs_FloatMovePos
        {
            fx = base.CurrentPosition2D.x,
            fy = base.CurrentPosition2D.y
        });
        ManagerCenter.Instance.GetManager<EntitiesManager>().RunAllEentityActionCacheAndClear(this.EID);
        if (base.Fbc != null && !string.IsNullOrEmpty(base.Fbc.CurrClipName))
        {
            FFBehaviourState_Skill ffbehaviourState_Skill = base.Fbc.CurrStatebyType<FFBehaviourState_Skill>();
            if (ffbehaviourState_Skill != null)
            {
                SkillManager skillManager = null;
                if (ffbehaviourState_Skill.CurrSkillClip != null)
                {
                    if (ManagerCenter.Instance.GetManager<EntitiesManager>().MainPlayer == this)
                    {
                        skillManager = MainPlayerSkillHolder.Instance.Skillmgr;
                    }
                    else
                    {
                        skillManager = base.GetComponent<SkillHolder>().Skillmgr;
                    }
                    SkillClip ffskillAnimClip = skillManager.GetFFSkillAnimClip(this, ffbehaviourState_Skill.CurrSkillClip.SkillStageId, ffbehaviourState_Skill.CurrSkillClip.ServerData);
                    if (ffskillAnimClip != null)
                    {
                        base.Fbc.PlayAnim(ffskillAnimClip.AnimData.ClipName, 0f, this.playingTime);
                    }
                }
                ffbehaviourState_Skill.RefreshSkillClipQueue(this, skillManager);
            }
        }
        if (this is MainPlayer)
        {
            Camera main = Camera.main;
            if (null != main)
            {
                LimitPointLightCount component3 = main.gameObject.GetComponent<LimitPointLightCount>();
                if (component3 != null)
                {
                    component3.GetMainPlayer();
                }
            }
        }
        return true;
    }

    public void RefreshModelAndAnimatorCnotroller()
    {
        if (base.CharState != CharactorState.CreatComplete)
        {
            return;
        }
        this.RefreshAvatorModel();
    }

    private void RefreshAvatorModel()
    {
        if (this.ModelHoldHelper != null)
        {
            base.ModelObj = this.ModelHoldHelper.rootObj;
            this.RefreshModelFinishCb();
        }
        else
        {
            cs_MapUserData mapUserData = this.OtherPlayerData.MapUserData;
            uint bodyid = mapUserData.mapshow.bodystyle;
            uint[] featureIDs;
            if (mapUserData.mapshow.coat > 0U)
            {
                bodyid = mapUserData.mapshow.coat;
                featureIDs = new uint[4];
            }
            else
            {
                featureIDs = new uint[]
                {
                    mapUserData.mapshow.haircolor,
                    mapUserData.mapshow.hairstyle,
                    mapUserData.mapshow.facestyle,
                    mapUserData.mapshow.antenna
                };
            }
            uint appearanceid = this.OtherPlayerData.GetAppearanceid();
            this.lastCreateId += 1UL;
            FFCharacterModelHold.CreateModel(appearanceid, bodyid, featureIDs, this.OtherPlayerData.GetACname(), delegate (PlayerCharactorCreateHelper modelHoldHelper)
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
                AnimationSound component = base.ModelObj.GetComponent<AnimationSound>();
                if (component != null)
                {
                    component.SetEffectData(this);
                }
                this.RefreshModelFinishCb();
                this.ResetModelRenderInfo();
            }, null, this.lastCreateId);
        }
    }

    private void RefreshModelFinishCb()
    {
        if (!this.onModelCreateFinish())
        {
            return;
        }
        if (this.IsSelect)
        {
            this.TargetSelect();
        }
        if (!base.IsLive)
        {
            base.PlayAniByState(UserState.USTATE_DEATH);
        }
    }

    private void SetModelSize()
    {
        if (base.ModelObj != null && base.BaseData != null)
        {
            base.ModelObj.transform.localScale = base.BaseData.GetModelSize();
        }
    }

    public void SetCollider()
    {
        if (base.ModelObj != null)
        {
            CapsuleCollider capsuleCollider = base.ModelObj.GetComponent<CapsuleCollider>();
            if (capsuleCollider == null)
            {
                capsuleCollider = base.ModelObj.AddComponent<CapsuleCollider>();
            }
            capsuleCollider.center = new Vector3(0f, 1f, 0f);
            capsuleCollider.radius = 0.5f;
            capsuleCollider.height = 2f;
        }
    }

    private void ResetModelRenderInfo()
    {
        ShadowManager.ResetRenderQueue(base.ModelObj, Const.RenderQueue.SceneObjAfterCharactor);
        if (ManagerCenter.Instance.GetManager<EntitiesManager>().MainPlayer == this)
        {
            this.SetPlayerLayer(base.ModelObj, Const.Layer.MainPlayer);
        }
        else
        {
            this.SetPlayerLayer(base.ModelObj, Const.Layer.OtherPlayer);
        }
        if (ManagerCenter.Instance.GetManager<GameScene>().LightInfo != null)
        {
            ManagerCenter.Instance.GetManager<GameScene>().SetMatLightInfo(base.ModelObj, false);
        }
        base.ReloadShader(base.ModelObj);
    }

    public void SetPlayerLayer(GameObject value, int layer)
    {
        value.SetLayer(layer, false);
        SkinnedMeshRenderer[] componentsInChildren = value.GetComponentsInChildren<SkinnedMeshRenderer>();
        if (componentsInChildren != null)
        {
            for (int i = 0; i < componentsInChildren.Length; i++)
            {
                componentsInChildren[i].gameObject.SetLayer(layer, false);
            }
        }
    }

    public void InitCamera()
    {
        if (base.ModelObj == null)
        {
            return;
        }
        Camera camera = null;
        Camera[] array = UnityEngine.Object.FindObjectsOfType<Camera>();
        for (int i = 0; i < array.Length; i++)
        {
            if (array[i] != Camera.main && array[i] != null)
            {
                UnityEngine.Object.DestroyImmediate(array[i].GetComponent<CameraController>());
            }
            if (array[i].tag == "MainCamera")
            {
                camera = array[i];
            }
        }
        if (camera == null)
        {
            return;
        }
        if (CameraController.Self != null && !CameraController.Self.gameObject.Equals(camera.gameObject))
        {
            UnityEngine.Object.DestroyImmediate(CameraController.Self);
        }
        CameraController cameraController = camera.gameObject.GetComponent<CameraController>();
        if (cameraController == null)
        {
            cameraController = camera.gameObject.AddComponent<CameraController>();
        }
        BoxCollider boxCollider = camera.gameObject.GetComponent<BoxCollider>();
        if (null == boxCollider)
        {
            boxCollider = camera.gameObject.AddComponent<BoxCollider>();
            boxCollider.size = new Vector3(0.5f, 0.5f, 0.5f);
        }
        Rigidbody rigidbody = camera.gameObject.GetComponent<Rigidbody>();
        if (null == rigidbody)
        {
            rigidbody = camera.gameObject.AddComponent<Rigidbody>();
            rigidbody.useGravity = false;
            rigidbody.isKinematic = false;
        }
        cameraController.SetTarget(base.ModelObj.transform);
    }

    private void InitHpData()
    {
        cs_MapUserData mapUserData = this.OtherPlayerData.MapUserData;
        Transform bindPoint = base.GetComponent<FFBipBindMgr>().GetBindPoint("Top");
        if (mapUserData.charid != MainPlayer.Self.OtherPlayerData.MapUserData.charid)
        {
            if (this.hpdata == null)
            {
                ControllerManager.Instance.GetController<UIHpSystem>().CreatePlayerHp(this, bindPoint, mapUserData, delegate (HpStruct hpd)
                {
                    this.hpdata = hpd;
                });
            }
            else
            {
                this.hpdata.ResetData(mapUserData);
            }
        }
        else
        {
            if (this.hpdata == null)
            {
                ControllerManager.Instance.GetController<UIHpSystem>().CreatePlayerHp(this, bindPoint, mapUserData, delegate (HpStruct hpd)
                {
                    this.hpdata = hpd;
                });
            }
            else
            {
                this.hpdata.DisableHpBar();
            }
            bool isNeedCloseLoading = LSingleton<NetWorkModule>.Instance.isNeedCloseLoading;
            if (isNeedCloseLoading)
            {
                UI_Loading.EndLoading();
            }
        }
    }

    public override void InitComponent()
    {
        base.InitComponent();
        this.isCommponentInited = true;
        base.AddComponent(new FFBehaviourControl());
        base.Fbc = base.GetComponent<FFBehaviourControl>();
        base.AddComponent(new FFBipBindMgr());
        base.AddComponent(new FFEffectControl());
        base.AddComponent(new FFMaterialEffectControl());
        base.AddComponent(new PlayerBufferControl());
        base.AddComponent(new FFWeaponHold());
        base.AddComponent(new FakeHitContorl());
        base.AddComponent(new FlyObjControl());
        base.AddComponent(new AttackWarningEffect());
        base.AddComponent(new PlayerClientStateControl());
        if (ManagerCenter.Instance.GetManager<EntitiesManager>().MainPlayer == this)
        {
            base.AddComponent(MainPlayerSkillHolder.Instance);
        }
        else
        {
            base.AddComponent(new SkillHolder());
            base.AddComponent(new PlayerVisitCtrl());
        }
        this.ComponentMgr.InitOver();
        FFBehaviourControl component = base.GetComponent<FFBehaviourControl>();
        if (component.CurrState == null)
        {
            base.GetComponent<FFBehaviourControl>().ChangeState(ClassPool.GetObject<FFBehaviourState_Idle>());
        }
        base.GetComponent<PlayerClientStateControl>().SetPlayerState(PlayerShowState.show);
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
    }

    public override void Die()
    {
        base.Die();
        if (ManagerCenter.Instance.GetManager<EntitiesManager>().SelectTarget == this)
        {
            ManagerCenter.Instance.GetManager<EntitiesManager>().SelectTarget = null;
        }
        FFDebug.Log(this, FFLogType.Default, "Other Player Die!");
    }

    public override void DelayRelive()
    {
        base.DelayRelive();
    }

    public override void DestroyThisInNineScreen()
    {
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

    public void TrueDestroy()
    {
        base.StopMoveImmediate(delegate
        {
            if (this.hpdata != null)
            {
                this.hpdata.Distory();
                this.hpdata = null;
            }
            Scheduler.Instance.RemoveUpdator(new Scheduler.OnScheduler(this.Update));
            this.RetMoveDataQueue.Clear();
            if (ManagerCenter.Instance.GetManager<EntitiesManager>().SelectTarget == this)
            {
                ManagerCenter.Instance.GetManager<EntitiesManager>().SelectTarget = null;
            }
            ManagerCenter.Instance.GetManager<EntitiesManager>().RemoveCharacter(this);
            base.DestroyThisInNineScreen();
            this.DestroyModel();
        });
    }

    public void DestroyModel()
    {
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
    }

    public override void RevertHpMp(MSG_Ret_HpMpPop_SC mdata)
    {
        base.RevertHpMp(mdata);
        if (this.hpdata != null)
        {
            this.hpdata.RevertOrBleedChangeHp(mdata.hp, (float)mdata.hp_change, mdata.force);
        }
        this.OtherPlayerData.MapUserData.mapdata.hp = mdata.hp;
    }

    public override void HandleHit(MSG_Ret_MagicAttack_SC mdata)
    {
        FakeHitContorl component = base.GetComponent<FakeHitContorl>();
        if (component != null)
        {
            component.GetHit(mdata);
        }
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
            MainPlayer.Self.GetComponent<MainPlayerTargetSelectMgr>().SetTargetNull();
        }
    }

    public override void TargetSelect()
    {
        if (this.EID.Equals(MainPlayer.Self.EID))
        {
            return;
        }
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

    public override void OnRelationChange()
    {
        if (!this.IsSelect)
        {
            return;
        }
        MainUIController controller = ControllerManager.Instance.GetController<MainUIController>();
        RelationType type = ManagerCenter.Instance.GetManager<EntitiesManager>().CheckRelationBaseMainPlayer(this);
        controller.UpdateTargetRelation(type, true);
    }

    public override void HitOther(MSG_Ret_MagicAttack_SC mdata, CharactorBase[] BeNHits)
    {
        base.HitOther(mdata, BeNHits);
        if (mdata == null)
        {
            return;
        }
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
                    if (component2 != null)
                    {
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
    }

    private bool isCommponentInited;

    public PlayerCharactorCreateHelper ModelHoldHelper;

    private float playingTime;

    private CharactorMirror mirror;

    public Action onModelCreate;
}
