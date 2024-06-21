using System;
using System.Collections.Generic;
using Engine;
using Framework.Base;
using Framework.Managers;
using Game.Scene;
using LuaInterface;
using msg;
using Net;
using ResoureManager;
using UI.Login;
using UnityEngine;

public class GameMainManager : IManager
{
    public string ManagerName
    {
        get
        {
            return "gamemain_manager";
        }
    }

    public void OnUpdate()
    {
    }

    public void Awake()
    {
    }

    public void FirstLoadIng()
    {
        SingletonForMono<GameTime>.Instance.Init();
        this.firstLoadingView = UI_firstLoading.LoadView();
        ScriptableToProto.SetInstanceFactory();
        ScriptableToProto.LoadByteDataHandle = new Action<string, Action<byte[]>>(ManagerCenter.Instance.GetManager<ResourceManager>().LoadByteData);
        SimpleTaskQueue STQueue = new SimpleTaskQueue();
        STQueue.OnStep = delegate (SimpleTaskQueue.Task st)
        {
            this.firstLoadingView.SetTrueProgress(STQueue.Progress);
        };
        ManagerCenter.Instance.GetManager<UIManager>().Init();
        ManagerCenter.Instance.GetManager<FontMgr>().Init();
        ManagerCenter.Instance.GetManager<RenderTextureMgr>().Init();
        ManagerCenter.Instance.GetManager<LuaManager>().Init();
        ManagerCenter.Instance.GetManager<RawCharactorMgr>().Init();
        ManagerCenter.Instance.GetManager<NpctalkRawCharactorMgr>().Init();
        ManagerCenter.Instance.GetManager<ScreenShotManager>().Init();
        ManagerCenter.Instance.GetManager<EscManager>().Init();
        STQueue.Finish = delegate ()
        {
            this.ResourcesLoadOver();
        };
        STQueue.AddTask(new Action<Action>(LuaConfigManager.StartLoadLuaConfig), "加载Lua配置", false);
        STQueue.AddTask(new Action<Action>(ManagerCenter.Instance.GetManager<BipBindDataMgr>().LoadFromProto), "加载捆绑点", true);
        STQueue.AddTask(new Action<Action>(ManagerCenter.Instance.GetManager<FFActionClipManager>().LoadFromProto), "加载动作配置", true);
        STQueue.AddTask(new Action<Action>(ManagerCenter.Instance.GetManager<FFEffectManager>().LoadFromProto), "加载特效", true);
        STQueue.AddTask(new Action<Action>(ManagerCenter.Instance.GetManager<FFMaterialEffectManager>().LoadFromProto), "加载材质特效", true);
        STQueue.AddTask(new Action<Action>(ManagerCenter.Instance.GetManager<AnimatorControllerMgr>().LoadFromProto), "加载AnimatorController", true);
        STQueue.AddTask(new Action<Action>(ManagerCenter.Instance.GetManager<CharacterModelMgr>().LoadFromProto), "加载Avatar配置", true);
        STQueue.AddTask(new Action<Action>(ManagerCenter.Instance.GetManager<NpctalkRawCharactorMgr>().InitAssetsPosData), "加载npctalkTransformdata", false);
        STQueue.AddTask(delegate (Action callback)
        {
            callback();
        }, "初始化系统设置", false);
        STQueue.Start();
    }

    public void loadLoginScene(Action callBack)
    {
        SceneInfo sceneInfo = new SceneInfo();
        sceneInfo.mapid = 685;
        sceneInfo.mapname = "Login";
        sceneInfo.lineid = 1;
        sceneInfo.pos = new FloatMovePos();
        GameScene gameScene = ManagerCenter.Instance.GetManager<GameScene>();
        gameScene.CheckSameScene(sceneInfo);
        F3DSun.SceneLoadFinished = false;
        gameScene.ChangeScene(sceneInfo, delegate
        {
            gameScene.OnSceneLoadNotifyServer();
            if (callBack != null)
            {
                callBack();
            }
        }, null);
    }

    public void ReLoadIng()
    {
        SimpleTaskQueue STQueue = new SimpleTaskQueue();
        GameMain gameMain = MonobehaviourManager.Instance as GameMain;
        gameMain.InitControllers();
        STQueue.OnStep = delegate (SimpleTaskQueue.Task st)
        {
            FFDebug.LogWarning(this, "loading--->" + st.WorkName);
            if (this.firstLoadingView != null)
            {
                this.firstLoadingView.SetTrueProgress(STQueue.Progress);
            }
        };
        ManagerCenter.Instance.GetManager<UIManager>().ReLoad();
        STQueue.Finish = delegate ()
        {
            this.ResourcesLoadOver();
            if (this.firstLoadingView != null)
            {
                this.firstLoadingView.Hide();
            }
        };
        STQueue.AddTask(new Action<Action>(gameMain.InitObjectProto));
        STQueue.Start();
    }

    private void ResourcesLoadOver()
    {
        try
        {
            ManagerCenter.Instance.GetManager<EntitiesManager>().Init();
            ManagerCenter.Instance.GetManager<SkillManager>().Init();
            ManagerCenter.Instance.GetManager<BufferStateManager>().Init();
            ManagerCenter.Instance.GetManager<CopyManager>().Init();
            ControllerManager.Instance.InitControllers();
            LuaScriptMgr.Instance.CallLuaFunction("ControllerManager.InitCtrls", Util.GetLuaTable("ControllerManager"));
            firstLoadingView.Hide();
        }
        catch (Exception ex)
        {
            FFDebug.LogError(this, ex.ToString());
        }
    }

    private void PreLoadAnimatorController(Action callback)
    {
        string[] AnimatorControllerList = new string[]
        {
            "AC_Player1",
            "AC_niu",
            "AC_lang",
            "ac_shuijing",
            "AC_zabing",
            "AC_kejigou"
        };
        if (AnimatorControllerList.Length == 0 && callback != null)
        {
            callback();
        }
        int index = 0;
        for (int i = 0; i < AnimatorControllerList.Length; i++)
        {
            ManagerCenter.Instance.GetManager<AnimatorControllerMgr>().LoadAnimatorController(AnimatorControllerList[i], delegate
            {
                index++;
                if (index == AnimatorControllerList.Length && callback != null)
                {
                    callback();
                }
            });
        }
    }

    public void PreLoadEffectBySkillId(ulong skillid, uint level)
    {
        ulong key = skillid * 10000UL + (ulong)(level * 10U) + 1UL;
        SkillManager skillmgr = MainPlayerSkillHolder.Instance.Skillmgr;
        LuaTable luaTable = skillmgr.Gett_skill_stage_config(key);
        if (luaTable == null)
        {
            return;
        }
        uint cacheField_Uint = luaTable.GetCacheField_Uint("ActionID");
        FFActionClip ffactionClip = ManagerCenter.Instance.GetManager<FFActionClipManager>().GetFFActionClip(MainPlayer.Self.animatorControllerName, cacheField_Uint, 0);
        if (ffactionClip != null)
        {
            for (int i = 0; i < ffactionClip.SkillEffectList.Count; i++)
            {
                this.LoadEffect(ffactionClip.SkillEffectList[i].effects);
            }
            for (int j = 0; j < ffactionClip.HitEffectList.Count; j++)
            {
                this.LoadEffect(ffactionClip.HitEffectList[j].effects);
            }
            for (int k = 0; k < ffactionClip.FlyEffectList.Count; k++)
            {
                this.LoadEffect(ffactionClip.FlyEffectList[k].effects);
            }
        }
    }

    private void LoadEffect(string[] effects)
    {
        FFEffectManager manager = ManagerCenter.Instance.GetManager<FFEffectManager>();
        if (effects == null)
        {
            return;
        }
        for (int i = 0; i < effects.Length; i++)
        {
            if (!string.IsNullOrEmpty(effects[i]))
            {
                EffectClip clip = manager.GetClip(effects[i]);
                if (!(null == clip))
                {
                    ManagerCenter.Instance.GetManager<FFEffectManager>().LoadEffobj(clip.EffectName, delegate
                    {
                    });
                }
            }
        }
    }

    private void RegistCsClassInLua()
    {
    }

    public void OnReSet()
    {
    }

    public void Logout(bool tologin)
    {
        if (tologin)
        {
            this.ResetAllManager();
            ManagerCenter.Instance.GetManager<GameMainManager>().loadLoginScene(delegate
            {
                ControllerManager.Instance.GetController<LoginP2PController>().InitLoginView(delegate ()
                {
                    UI_P2PLogin uiobject = UIManager.GetUIObject<UI_P2PLogin>();
                    if (null != uiobject)
                    {
                        uiobject.SetAnimVisible();
                    }
                    else
                    {
                        LoginP2PController controller = ControllerManager.Instance.GetController<LoginP2PController>();
                        controller.onLogonShow = (Action)Delegate.Combine(controller.onLogonShow, new Action(delegate ()
                        {
                            UIManager.GetUIObject<UI_P2PLogin>().SetAnimVisible();
                        }));
                    }
                });
            });
        }
        else
        {
            ManagerCenter.Instance.GetManager<EntitiesManager>().PNetWork.ReqBackToRoleList();
            this.OnBackToHeroList();
        }
    }

    public void RegOnBackToHeroListEvent(string eventName, Action callBack)
    {
        this.onBackHeroListBackDic[eventName] = callBack;
    }

    private void OnBackToHeroList()
    {
        try
        {
            foreach (KeyValuePair<string, Action> keyValuePair in this.onBackHeroListBackDic)
            {
                if (keyValuePair.Value != null)
                {
                    keyValuePair.Value();
                }
            }
        }
        catch (Exception message)
        {
            Debug.LogError(message);
        }
    }

    private void ResetAllManager()
    {
        LSingleton<NetWorkModule>.Instance.Close();
        if (UIManager.Instance != null)
        {
            UIManager.Instance.DeleteAllUI(true);
        }
        if (MainPlayer.Self != null)
        {
            MainPlayer.Self.TrueDestroy();
        }
        LuaProcess.UnAllRegisertFunction();
        ManagerCenter.Instance.ResetManager();
        if (CameraController.Self != null)
        {
            CameraController.Self.RemoveSelf();
        }
        ControllerManager.Instance.GetController<ChooseRoleController>().Dispose();
        ControllerManager.Instance.ReSet();
        SingletonForMono<InputController>.Instance.Reset();
        GC.Collect();
        Resources.UnloadUnusedAssets();
        this.firstLoadingView = UI_firstLoading.LoadView();
        Scheduler.Instance.AddFrame(2U, false, delegate
        {
            ManagerCenter.Instance.GetManager<GameMainManager>().ReLoadIng();
        });
        this.onBackHeroListBackDic.Clear();
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public PlayerInfo PlayerInfo;

    private UI_firstLoading firstLoadingView;

    private Dictionary<string, Action> onBackHeroListBackDic = new Dictionary<string, Action>();
}
