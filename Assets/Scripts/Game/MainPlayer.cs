using System;
using System.Collections;
using System.Collections.Generic;
using AudioStudio;
using Engine;
using Framework.Managers;
using Game.Scene;
using LuaInterface;
using magic;
using UnityEngine;

public class MainPlayer : OtherPlayer
{
    public MainPlayer()
    {
        SingletonForMono<InputController>.Instance.mScreenEventController.AddListener(new ScreenEventController.OnScreenEvent(this.OnClickEvent));
        base.CharState = CharactorState.CreatEntity;
        this.Init();
    }

    public AutoAttack autoAttack
    {
        get
        {
            return this._autoAttack;
        }
    }

    public static MainPlayer Self
    {
        get
        {
            if (ManagerCenter.Instance.GetManager<EntitiesManager>() == null)
            {
                return null;
            }
            return ManagerCenter.Instance.GetManager<EntitiesManager>().MainPlayer;
        }
    }

    public void OnNpcHatredListChange(CharactorBase cbase)
    {
        CharactorBase charactorBase = null;
        MainPlayerTargetSelectMgr component = base.GetComponent<MainPlayerTargetSelectMgr>();
        if (component != null)
        {
            charactorBase = component.TargetCharactor;
        }
        if (charactorBase == null)
        {
            return;
        }
        if (charactorBase != cbase)
        {
            return;
        }
    }

    public void BreakAutoAttack()
    {
        if (MainPlayerSkillHolder.Instance.skillAttackAutoAttack != null)
        {
            MainPlayerSkillHolder.Instance.skillAttackAutoAttack.ActiveSelf = false;
        }
        this.SwitchAutoAttack(false);
    }

    public void SwitchAutoAttack(bool autoattack)
    {
        if (this.autoAttack != null)
        {
            this.autoAttack.SwitchModle(autoattack);
        }
        if (MainPlayerSkillHolder.Instance != null && MainPlayerSkillHolder.Instance.skillAttackAutoAttack != null)
        {
            MainPlayerSkillHolder.Instance.skillAttackAutoAttack.AutoAttackState = autoattack;
        }
    }

    public MainPlayerData MainPlayeData
    {
        get
        {
            return base.GetComponent<MainPlayerData>();
        }
    }

    ~MainPlayer()
    {
        SingletonForMono<InputController>.Instance.mScreenEventController.RemoveListener(new ScreenEventController.OnScreenEvent(this.OnClickEvent));
    }

    public override void Init()
    {
        base.Init();
        base.AddComponentImmediate(new MainPlayerData());
        this.InitMainPlayer();
    }

    public uint CurMP
    {
        get
        {
            return this.MainPlayeData.AttributeData.mp;
        }
    }

    public void InitMainPlayer()
    {
        this.m_sceneManager = (MonobehaviourManager.Instance.MgrCenter.GetManagerByName("scene_manager") as GameScene);
    }

    public override void CreatPlayerModel()
    {
        base.CreatPlayerModel();
    }

    public override void InitComponent()
    {
        base.InitComponent();
        base.AddComponent(new MainPlayerTargetSelectMgr());
        base.AddComponent(new FFDetectionNpcControl());
        base.AddComponent(new SkillSelectRangeEffect());
        base.AddComponent(new AutoAttack());
        this._autoAttack = base.GetComponent<AutoAttack>();
        base.AddComponent(new AttactFollowTeamLeader());
        base.AddComponent(new FollowTeamLeader());
        base.AddComponent(new PathFindComponent());
        base.Pfc = base.GetComponent<PathFindComponent>();
        base.AddComponent(new PathFindFollowTarget());
        base.AddComponent(new BreakAutoattackUIMgr());
        base.AddComponent(new SkillPublicCDControl());
        base.AddComponent(new FindDirections());
        if (MainPlayer.Self != null && MainPlayer.Self.ModelObj != null)
        {
            if (!MainPlayer.Self.ModelObj.GetComponent<DefaultAudioListener>())
            {
                MainPlayer.Self.ModelObj.AddComponent<DefaultAudioListener>();
            }
            if (MainPlayer.Self.ModelObj.GetComponent<Rigidbody>() == null)
            {
                Rigidbody rigidbody = MainPlayer.Self.ModelObj.AddComponent<Rigidbody>();
                rigidbody.useGravity = false;
                rigidbody.isKinematic = true;
            }
        }
    }

    public void OnClickEvent(ScreenEvent SE)
    {
    }

    public void MoveToByDir(int dir, bool IsMoveInversion = false)
    {
        this.inputDir = dir;
        if (!base.IsCanMove())
        {
            return;
        }
        if (base.Pfc.isPathfinding)
        {
            base.Pfc.EndPathFind(PathFindState.Break, true);
        }
        int num = (!IsMoveInversion) ? dir : (dir - 90);
        Vector2 clientDirVector2ByServerDir = CommonTools.GetClientDirVector2ByServerDir(num);
        if (base.IsMoving)
        {
            return;
        }
        Vector2 vector = base.CurrentPosition2D + clientDirVector2ByServerDir * this.MinimunMoveUnit;
        if (vector.x < 0f || vector.y < 0f || vector.x >= (float)LSingleton<CurrentMapAccesser>.Instance.CellNumX || vector.y >= (float)LSingleton<CurrentMapAccesser>.Instance.CellNumY)
        {
            FFDebug.Log(this, FFLogType.Player, "已到达地图边界，无法移动");
            base.ServerDir = (uint)dir;
            return;
        }
        if (GraphUtils.IsBlockPointForMove(vector.x, vector.y) || !this.CheckPosValid(vector))
        {
            cs_MoveData bestPointNearbyBlockPoint = this.GetBestPointNearbyBlockPoint1(num);
            if (bestPointNearbyBlockPoint == null)
            {
                Vector2 bestPointNearbyBlockPoint2 = this.GetBestPointNearbyBlockPoint(clientDirVector2ByServerDir);
                if (Mathf.Abs(bestPointNearbyBlockPoint2.x - base.CurrentPosition2D.x) < 0.1f && Mathf.Abs(bestPointNearbyBlockPoint2.y - base.CurrentPosition2D.y) < 0.1f)
                {
                    FFDebug.Log(this, FFLogType.Player, "目标位置为阻挡点");
                    base.ServerDir = (uint)dir;
                    return;
                }
                vector = bestPointNearbyBlockPoint2;
            }
            else
            {
                dir = (int)bestPointNearbyBlockPoint.dir;
                vector.x = bestPointNearbyBlockPoint.pos.fx;
                vector.y = bestPointNearbyBlockPoint.pos.fy;
            }
        }
        Vector2 vector2 = vector;
        vector2.x = GraphUtils.Keep2DecimalPlaces(vector2.x);
        vector2.y = GraphUtils.Keep2DecimalPlaces(vector2.y);
        this._cs_MoveData.dir = (uint)dir;
        this._cs_FloatMovePos.fx = vector2.x;
        this._cs_FloatMovePos.fy = vector2.y;
        this._cs_MoveData.pos = this._cs_FloatMovePos;
        this._cs_MoveData.step = 0;
        ManagerCenter.Instance.GetManager<EntitiesManager>().PNetWork.ReqMove(this._cs_MoveData, false, false);
        base.MoveTo(this._cs_MoveData);
        base.GetComponent<MainPlayerTargetSelectMgr>().TabSelect.SetFirstTargetSearch();
    }

    public Vector2 GetBestPointNearbyBlockPoint(Vector2 movedir)
    {
        Vector2 result = base.CurrentPosition2D;
        int num = 5;
        float num2 = this.MinimunMoveUnit / (float)num;
        for (int i = 1; i <= num; i++)
        {
            Vector2 vector = base.CurrentPosition2D + num2 * (float)i * movedir;
            if (GraphUtils.IsBlockPointForMove(vector.x, vector.y))
            {
                break;
            }
            result = vector;
        }
        return result;
    }

    public cs_MoveData GetBestPointNearbyBlockPoint1(int inputdir)
    {
        bool flag = false;
        Vector2 topos = Vector2.zero;
        for (int i = 1; i <= 10; i++)
        {
            flag = !flag;
            int num = (i % 2 != 0) ? ((i + 1) / 2) : (i / 2);
            int num2 = inputdir + ((!flag) ? 1 : -1) * num * 5;
            if (num2 < 0)
            {
                num2 = 180 + num2;
            }
            Vector2 clientDirVector2ByServerDir = CommonTools.GetClientDirVector2ByServerDir(num2);
            topos = base.CurrentPosition2D + clientDirVector2ByServerDir * this.MinimunMoveUnit;
            if (!GraphUtils.IsBlockPointForMove(topos.x, topos.y) && this.CheckPosValid(topos))
            {
                cs_MoveData cs_MoveData = new cs_MoveData();
                cs_MoveData.dir = (uint)num2;
                cs_MoveData.pos = default(cs_FloatMovePos);
                cs_MoveData.pos.fx = GraphUtils.Keep2DecimalPlaces(topos.x);
                cs_MoveData.pos.fy = GraphUtils.Keep2DecimalPlaces(topos.y);
                return cs_MoveData;
            }
        }
        return null;
    }

    private bool CheckPosValid(Vector2 topos)
    {
        return true;
    }

    public void ForceSetPositionTo(Vector2 serverpos)
    {
        if (AreaMusicTool.Instance == null)
        {
            FFDebug.LogError(this, "加载场景完成后未初始化AreaMusicTool");
        }
        else
        {
            AreaMusicTool.Instance.UpdateMusic(serverpos);
        }
        base.BaseData.RefreshCharaBasePosition(serverpos);
        base.NextPosition2D = base.CurrentPosition2D;
        base.StopMoveImmediate(delegate
        {
            Vector3 worldPosByServerPos = GraphUtils.GetWorldPosByServerPos(serverpos);
            this.SetPhysicsPos(worldPosByServerPos);
            this.TargetPos = this.ModelObj.transform.position;
            if (this.Pfc.isPathfinding)
            {
                this.Pfc.EndPathFind(PathFindState.Break, true);
            }
        });
    }

    public Vector3 GetClickPoint(Vector3 ScreenPoint, float height)
    {
        Vector3 vector = Vector3.zero;
        Ray ray = ResolutionManager.Instance.MainCameraScreenPointToRay(ScreenPoint);
        RaycastHit raycastHit;
        if (Physics.Raycast(ray, out raycastHit, 3.40282347E+38f, 512))
        {
            vector = raycastHit.point;
            this.targetPosObj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            this.targetPosObj.transform.position = vector;
            this.targetPosObj.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
            this.targetPosObj.GetComponent<Renderer>().material.color = Color.red;
        }
        return vector;
    }

    public void TryReleaseSkill(uint skill)
    {
    }

    public override void Die()
    {
        base.Die();
        if (base.Pfc != null)
        {
            base.Pfc.EndPathFind(PathFindState.Break, true);
        }
        MainPlayerTargetSelectMgr component = base.GetComponent<MainPlayerTargetSelectMgr>();
        if (component != null)
        {
            base.GetComponent<MainPlayerTargetSelectMgr>().SetTargetNull();
        }
        UIManager.Instance.CloseOpenByNpcUI();
        this.SwitchAutoAttack(false);
    }

    public void OnMoveOneStep()
    {
        if ((int)this.lastPosition2D.x != (int)base.CurrentPosition2D.x || (int)this.lastPosition2D.y != (int)base.CurrentPosition2D.y)
        {
            this.m_sceneManager.CheckInDisArea((int)base.CurrentPosition2D.x, (int)base.CurrentPosition2D.y);
        }
        this.lastPosition2D = base.CurrentPosition2D;
        if (base.ModelObj != null)
        {
            this.m_sceneManager.GetTerrainIndex(base.ModelObj.transform.position, null);
            if (this.m_sceneManager.sls == SceneLoadState.Complete)
            {
                this.m_sceneManager.CheckLoadZoneData();
            }
            if (null != CameraController.Self && CameraController.Self.CurrState is CameraFollowTargetMonitor)
            {
                CameraFollowTargetMonitor cameraFollowTargetMonitor = CameraController.Self.CurrState as CameraFollowTargetMonitor;
                cameraFollowTargetMonitor.bNeedCheck = true;
            }
        }
        if (base.GetComponent<FFDetectionNpcControl>() != null)
        {
            base.GetComponent<FFDetectionNpcControl>().UpDateOnMoveOneStep();
        }
        if (base.CurrMoveData != null)
        {
            ControllerManager.Instance.GetController<UIMapController>().SetMyPos(base.CurrServerPos, base.ServerDir, false, true);
        }
        base.GetComponent<PathFindFollowTarget>().OnReachMoveingDestPos();
    }

    public override void DestroyThisInNineScreen()
    {
        FFDebug.Log(this, FFLogType.Player, "DestroyThisInNineScreen --------------> MainPlayer!!");
        ManagerCenter.Instance.GetManager<EntitiesManager>().SelectTarget = null;
    }

    public override void HandleHit(MSG_Ret_MagicAttack_SC mdata)
    {
        base.HandleHit(mdata);
        for (int i = 0; i < mdata.pklist.Count; i++)
        {
            PKResult pkresult = mdata.pklist[i];
            if (this.EID.Equals(pkresult.def))
            {
                ControllerManager.Instance.GetController<MainUIController>().ResetMainPlayerHp(pkresult.hp, MainPlayer.Self.MainPlayeData.AttributeData.maxhp);
            }
        }
        CharactorBase charactorByID = ManagerCenter.Instance.GetManager<EntitiesManager>().GetCharactorByID(mdata.att);
        if (charactorByID != null && charactorByID is Npc)
        {
            Npc npc = charactorByID as Npc;
            LuaTable configTable = LuaConfigManager.GetConfigTable("npc_data", (ulong)npc.NpcData.MapNpcData.baseid);
            if (configTable != null && configTable.GetField_Bool("not_beselect"))
            {
                return;
            }
        }
    }

    public override void HitOther(MSG_Ret_MagicAttack_SC mdata, CharactorBase[] BeNHits)
    {
        base.HitOther(mdata, BeNHits);
        if (BeNHits != null && BeNHits.Length > 0)
        {
            EntitiesManager manager = ManagerCenter.Instance.GetManager<EntitiesManager>();
            if (manager != null)
            {
                for (int i = 0; i < BeNHits.Length; i++)
                {
                    RelationType relationType = manager.CheckRelationBaseMainPlayer(BeNHits[i]);
                    if (relationType != RelationType.Self && relationType != RelationType.None && relationType != RelationType.Friend)
                    {
                        this.SwitchAutoAttack(true);
                        break;
                    }
                }
            }
        }
    }

    public override void RevertHpMp(MSG_Ret_HpMpPop_SC mdata)
    {
        base.RevertHpMp(mdata);
        this.MainPlayeData.RefreshMp(mdata.mp);
        this.MainPlayeData.RefreshHp(mdata.hp, 0U);
    }

    public void UpdateCurrency()
    {
        LuaScriptMgr.Instance.CallLuaFunction("BagCtrl.UpdateCurrency", new object[]
        {
            Util.GetLuaTable("BagCtrl")
        });
    }

    public void RefreshCurrency()
    {
        if (this.MainPlayeData == null)
        {
            FFDebug.LogWarning(this, "RefreshCurrency MainPlayerData is null");
            return;
        }
        if (this.MainPlayeData.CharacterBaseData == null)
        {
            FFDebug.LogWarning(this, "RefreshCurrency MainPlayeData.CharacterBaseData is null");
            return;
        }
        if (this.OnPlayerCuccencyUpdateEvent != null)
        {
            this.OnPlayerCuccencyUpdateEvent();
        }
    }

    public uint GetCurrencyByID(uint id)
    {
        if (this.MainPlayeData == null || this.MainPlayeData.CharacterBaseData == null)
        {
            FFDebug.LogWarning(this, "GetCurrencyByID MainPlayerData is null");
            return 0U;
        }
        uint result = 0U;
        switch (id)
        {
            case 2U:
                result = this.MainPlayeData.CharacterBaseData.Stone;
                break;
            case 3U:
                result = this.MainPlayeData.CharacterBaseData.Money;
                break;
            case 4U:
                result = this.MainPlayeData.CharacterBaseData.WelPoint;
                break;
            case 11U:
                result = this.MainPlayeData.CharacterBaseData.familyatt;
                break;
            case 13U:
                result = this.MainPlayeData.CharacterBaseData.bluecrystal;
                break;
            case 14U:
                result = this.MainPlayeData.CharacterBaseData.purplecrystal;
                break;
            case 15U:
                result = this.MainPlayeData.CharacterBaseData.EduPoint;
                break;
            case 16U:
                result = this.MainPlayeData.CharacterBaseData.cooppoint;
                break;
            case 17U:
                result = this.MainPlayeData.CharacterBaseData.vigourpoint;
                break;
            case 18U:
                result = this.MainPlayeData.CharacterBaseData.doublepoint;
                break;
        }
        return result;
    }

    public string GetMainPlayerName()
    {
        if (base.OtherPlayerData.MapUserData == null)
        {
            return string.Empty;
        }
        return base.OtherPlayerData.MapUserData.name;
    }

    public uint GetSelfDir()
    {
        return base.ServerDir;
    }

    public Vector2 GetSelfCurPos()
    {
        return base.CurrServerPos;
    }

    public uint GetMainPlayerFightValue()
    {
        if (this.MainPlayeData == null)
        {
            return 0U;
        }
        if (this.MainPlayeData.FightData == null)
        {
            return 0U;
        }
        return this.MainPlayeData.FightData.curfightvalue;
    }

    public List<PropsBase> MainPackageTableList()
    {
        List<PropsBase> list = new List<PropsBase>();
        object[] array = LuaScriptMgr.Instance.CallLuaFunction("BagCtrl.GetlisMainPackageTable", new object[]
        {
            Util.GetLuaTable("BagCtrl")
        });
        if (array.Length > 0)
        {
            LuaTable luaTable = array[0] as LuaTable;
            IEnumerator enumerator = luaTable.Values.GetEnumerator();
            enumerator.Reset();
            while (enumerator.MoveNext())
            {
                object obj = enumerator.Current;
                list.Add((PropsBase)obj);
            }
        }
        return list;
    }

    public override void TargetSelect()
    {
        MainUIController controller = ControllerManager.Instance.GetController<MainUIController>();
        if (this.hpdata != null)
        {
            this.hpdata.SetSelect(true);
        }
        if (controller != null)
        {
            controller.ShowTargetInfo(this.EID, false, null);
        }
    }

    public override void CancelSelect()
    {
        if (this.hpdata != null)
        {
            this.hpdata.SetSelect(false);
        }
        MainPlayerTargetSelectMgr component = base.GetComponent<MainPlayerTargetSelectMgr>();
        if (component != null)
        {
            component.SetTargetNull();
        }
    }

    public bool IsSendSingleMove;

    private AutoAttack _autoAttack;

    public Vector2 lastPosition2D;

    private GameScene m_sceneManager;

    private cs_MoveData _cs_MoveData = new cs_MoveData();

    private cs_FloatMovePos _cs_FloatMovePos = default(cs_FloatMovePos);

    private GameObject targetPosObj;

    private float lastvisttime;

    public MainPlayer.VoidDelegate OnPlayerCuccencyUpdateEvent;

    public delegate void VoidDelegate();
}
