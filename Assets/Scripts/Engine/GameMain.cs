using System;
using System.IO;
using AudioStudio;
using Framework.Base;
using Framework.Managers;
using Game.Scene;
using Net;
using ResoureManager;
using TextModelpackage;
using UI.Exchange;
using UI.Login;
using UnityEngine;

namespace Engine
{
    public class GameMain : MonobehaviourManager
    {
        public new static ICompent Instance
        {
            get
            {
                throw new Exception("Please to use MonobehaviourManager.Instance instead!");
            }
        }

        private void Start()
        {
            SingletonForMono<PlayMovieManager>.Instance.Init();
            AssetManager.Init(this);
            this.InitManagers();
            this.InitModels();
            this.InitControllers();
            this.InitShader();
            this.InitObjectProto(delegate
            {
            });
            this.OnGameStart();
            AudioCtrl.Init();
        }

        private void LoadUIInformation()
        {
        }

        private void Update()
        {
            base.OnUpdate();
            AssetManager.Update();
            LuaScriptMgr.Instance.Update();
        }

        private void LateUpdate()
        {
            AssetManager.LateUpdate();
            LuaScriptMgr.Instance.LateUpate();
        }

        private void FixedUpdate()
        {
            LuaScriptMgr.Instance.FixedUpdate();
        }

        private void OnDestroy()
        {
        }

        private void OnApplicationQuit()
        {
            LuaManager.GetInstance().Destroy();
            LSingleton<NetWorkModule>.Instance.OnDestroy();
        }

        private void InitShader()
        {
            AssetManager.LoadAssetBundle("Shader/shader.u", delegate (string assetBundleName, bool succeed)
            {
                Bundle bundle = AssetManager.GetBundle(assetBundleName);
                if (bundle != null)
                {
                    this.allShader = bundle.Assetbundle.LoadAllAssets<Shader>();
                    foreach(Shader shader in this.allShader)
                    {
                    }
                }
                if (!succeed)
                {
                    FFDebug.LogWarning(this, "shader.u is not exist!");
                }
            }, Bundle.BundleType.EffectAndcharacter);
        }

        public Shader GetShader(string name)
        {
            if (this.allShader == null)
            {
                return null;
            }
            for (int i = 0; i < this.allShader.Length; i++)
            {
                if (this.allShader[i].name == name)
                {
                    return this.allShader[i];
                }
            }
            return null;
        }

        private void InitManagers()
        {
            ManagerCenter.Instance.AddManagerAsUnityComponent<ResourceManager>(base.gameObject);
            GameMainManager gameMainManager = ManagerCenter.Instance.AddManagerByType<GameMainManager>();
            ManagerCenter.Instance.AddManagerByType<EntitiesManager>();
            ManagerCenter.Instance.AddManagerByType<SkillManager>();
            ManagerCenter.Instance.AddManagerByType<SceneMusicMgr>();
            BiaoqingManager biaoqingManager = ManagerCenter.Instance.AddManagerByType<BiaoqingManager>();
            biaoqingManager.Awake();
            ManagerCenter.Instance.AddManagerByType<BipBindDataMgr>();
            ManagerCenter.Instance.AddManagerByType<FFEffectManager>();
            ManagerCenter.Instance.AddManagerByType<CharacterModelMgr>();
            ManagerCenter.Instance.AddManagerByType<FFMaterialEffectManager>();
            ManagerCenter.Instance.AddManagerByType<AnimatorControllerMgr>();
            ManagerCenter.Instance.AddManagerByType<WeaponResourcesMgr>();
            ManagerCenter.Instance.AddManagerByType<FFActionClipManager>();
            ManagerCenter.Instance.AddManagerByType<BufferStateManager>();
            ManagerCenter.Instance.AddManagerByType<CopyManager>();
            ManagerCenter.Instance.AddManagerByType<UIManager>();
            ManagerCenter.Instance.AddManagerByType<ObjectPoolManager>().Init();
            ManagerCenter.Instance.AddManagerByType<UITextureMgr>();
            ManagerCenter.Instance.AddManagerByType<RenderTextureMgr>();
            ManagerCenter.Instance.AddManagerByType<RawCharactorMgr>();
            ManagerCenter.Instance.AddManagerByType<FontMgr>();
            ManagerCenter.Instance.AddManagerByType<CutSceneManager>();
            ManagerCenter.Instance.AddManagerByType<CutSceneAssetManager>();
            gameMainManager.Awake();
            ManagerCenter.Instance.AddManagerByType<LuaNetWorkManager>();
            ManagerCenter.Instance.AddManagerByType<GameScene>().Awake();
            ManagerCenter.Instance.AddManagerByType<LuaManager>();
            ManagerCenter.Instance.AddManagerByType<NpctalkRawCharactorMgr>();
            ManagerCenter.Instance.AddManagerByType<ScreenShotManager>();
            ManagerCenter.Instance.AddManagerByType<EscManager>();
            AntiAddictionManager antiAddictionManager = ManagerCenter.Instance.AddManagerByType<AntiAddictionManager>();
            antiAddictionManager.Awake();
            ManagerCenter.Instance.AddManagerByType<LODManager>();
        }

        public void InitControllers()
        {
            this.CtrlMgr.AddcontrollerByType<PreLoading>();
            this.CtrlMgr.AddcontrollerByType<LoginP2PController>();
            this.CtrlMgr.AddcontrollerByType<ChooseRoleController>();
            this.CtrlMgr.AddcontrollerByType<TaskController>();
            this.CtrlMgr.AddcontrollerByType<GuildControllerNew>();
            this.CtrlMgr.AddcontrollerByType<TeamController>();
            this.CtrlMgr.AddcontrollerByType<FightModelController>();
            this.CtrlMgr.AddcontrollerByType<UIHpSystem>();
            this.CtrlMgr.AddcontrollerByType<OccupyController>();
            this.CtrlMgr.AddcontrollerByType<VisitPlayerController>();
            this.CtrlMgr.AddcontrollerByType<MainUIController>();
            this.CtrlMgr.AddcontrollerByType<LoadTipsController>();
            this.CtrlMgr.AddcontrollerByType<MsgBoxController>();
            this.CtrlMgr.AddcontrollerByType<TextModelController>();
            this.CtrlMgr.AddcontrollerByType<CampController>();
            this.CtrlMgr.AddcontrollerByType<CollectController>();
            this.CtrlMgr.AddcontrollerByType<TaskUIController>();
            this.CtrlMgr.AddcontrollerByType<SkillViewControll>();
            this.CtrlMgr.AddcontrollerByType<PryController>();
            this.CtrlMgr.AddcontrollerByType<AlertController>();
            this.CtrlMgr.AddcontrollerByType<UIMapController>();
            this.CtrlMgr.AddcontrollerByType<DuoQiController>();
            this.CtrlMgr.AddcontrollerByType<FriendControllerNew>();
            this.CtrlMgr.AddcontrollerByType<PetController>();
            this.CtrlMgr.AddcontrollerByType<ShortCutUseEquipController>();
            this.CtrlMgr.AddcontrollerByType<ProgressUIController>();
            this.CtrlMgr.AddcontrollerByType<UINpcDlgController>();
            this.CtrlMgr.AddcontrollerByType<GuideController>();
            this.CtrlMgr.AddcontrollerByType<CompleteCopyController>();
            this.CtrlMgr.AddcontrollerByType<CopyWayCheckContoller>();
            this.CtrlMgr.AddcontrollerByType<ComicController>();
            this.CtrlMgr.AddcontrollerByType<ImgMaskController>();
            this.CtrlMgr.AddcontrollerByType<UserSysSettingController>();
            this.CtrlMgr.AddcontrollerByType<GiftBagController>();
            this.CtrlMgr.AddcontrollerByType<ActivityController>();
            this.CtrlMgr.AddcontrollerByType<LocalGMController>();
            this.CtrlMgr.AddcontrollerByType<ShortcutsConfigController>();
            this.CtrlMgr.AddcontrollerByType<DepotController>();
            this.CtrlMgr.AddcontrollerByType<ChatControl>();
            this.CtrlMgr.AddcontrollerByType<GmChatController>();
            this.CtrlMgr.AddcontrollerByType<JieWangQuanController>();
            this.CtrlMgr.AddcontrollerByType<YuanQiDanController>();
            this.CtrlMgr.AddcontrollerByType<CardController>();
            this.CtrlMgr.AddcontrollerByType<QueueController>();
            this.CtrlMgr.AddcontrollerByType<ItemTipController>();
            this.CtrlMgr.AddcontrollerByType<GeneController>();
            this.CtrlMgr.AddcontrollerByType<PickDropController>();
            this.CtrlMgr.AddcontrollerByType<UnLockSkillsController>();
            this.CtrlMgr.AddcontrollerByType<CoolDownController>();
            this.CtrlMgr.AddcontrollerByType<HeroHandbookController>();
            this.CtrlMgr.AddcontrollerByType<PVPMatchController>();
            this.CtrlMgr.AddcontrollerByType<MatchPasswordController>();
            this.CtrlMgr.AddcontrollerByType<PVPCompetitionController>();
            this.CtrlMgr.AddcontrollerByType<QTEController>();
            this.CtrlMgr.AddcontrollerByType<ShopController>();
            this.CtrlMgr.AddcontrollerByType<NumberInputController>();
            this.CtrlMgr.AddcontrollerByType<SecondPwdControl>();
            this.CtrlMgr.AddcontrollerByType<TransporterController>();
            this.CtrlMgr.AddcontrollerByType<AntiAddictionController>();
            this.CtrlMgr.AddcontrollerByType<SystemSettingController>();
            this.CtrlMgr.AddcontrollerByType<NotifyController>();
            this.CtrlMgr.AddcontrollerByType<CountDownController>();
            this.CtrlMgr.AddcontrollerByType<ExchangeController>();
            this.CtrlMgr.AddcontrollerByType<VipPrivilegeController>();
            this.CtrlMgr.AddcontrollerByType<ExchangeGemController>();
            this.CtrlMgr.AddcontrollerByType<UIEffectController>();
            this.CtrlMgr.AddcontrollerByType<HeroAwakeController>();
            this.CtrlMgr.AddcontrollerByType<FashionController>();
            this.CtrlMgr.AddcontrollerByType<MailControl>();
            this.CtrlMgr.AddcontrollerByType<SevenDaysController>();
            this.CtrlMgr.AddcontrollerByType<AbattoirMatchController>();
            this.CtrlMgr.AddcontrollerByType<AbattoirPrayController>();
            this.CtrlMgr.AddcontrollerByType<QuestationnaireController>();
            this.CtrlMgr.AddcontrollerByType<AutoFightController>();
            global::Display.Instance.StartDisplay();
        }

        private void InitModels()
        {
        }

        public void InitObjectProto(Action callback)
        {
            string path = "uiInformation/TextModel.bytes";
            TextModelContentListProto textModelContentListProto = ManagerCenter.Instance.GetManager<ResourceManager>().LoadProtoBuffData<TextModelContentListProto>(path);
            ControllerManager.Instance.GetController<TextModelController>().textModelList = textModelContentListProto;
            for (int i = 0; i < textModelContentListProto.key.Count; i++)
            {
                if (!ControllerManager.Instance.GetController<TextModelController>().dicTrextModel.ContainsKey(textModelContentListProto.key[i]))
                {
                    ControllerManager.Instance.GetController<TextModelController>().dicTrextModel.Add(textModelContentListProto.key[i], textModelContentListProto.modelList[i]);
                }
            }
            ManagerCenter.Instance.GetManager<CutSceneManager>().LoadSubtitleProto();
            callback();
        }

        private void OnGameStart()
        {
            ManagerCenter.Instance.GetManager<GameMainManager>().FirstLoadIng();
            KeyWordFilter.InitFilter();
            LSingleton<NetWorkModule>.Instance.ResetScene = new Action(this.ReconnectResetScene);
            GameObject gameObject = GameObject.Find("UIRoot/LayerLoading/UI_Patch");
            gameObject.SetActive(false);
            string path = Path.Combine(Environment.CurrentDirectory, "wegame.txt");
            EntitiesManager manager = ManagerCenter.Instance.GetManager<EntitiesManager>();
            manager.wegame = File.Exists(path);
            if (manager.wegame)
            {
                string path2 = Path.Combine(Environment.CurrentDirectory, "wegametest.txt");
                manager.wegametest = File.Exists(path2);
            }
            ManagerCenter.Instance.GetManager<GameMainManager>().loadLoginScene(delegate
            {
                ControllerManager.Instance.GetController<LoginP2PController>().InitLoginView(null);
            });
        }

        private void ReconnectResetScene()
        {
            ManagerCenter.Instance.GetManager<EntitiesManager>().UnLoadCharactors();
            ControllerManager.Instance.GetController<OccupyController>().RemoveAllHoldTransform();
            ControllerManager.Instance.GetController<TeamController>().ClearTeamInfo();
            ControllerManager.Instance.GetController<MainUIController>().RefreshTeamInfo();
            LuaScriptMgr.Instance.CallLuaFunction("PVPMatchCtrl.BreakLineCancelMatch_SC", new object[]
            {
                Util.GetLuaTable("PVPMatchCtrl")
            });
        }

        private Shader[] allShader;
    }
}
