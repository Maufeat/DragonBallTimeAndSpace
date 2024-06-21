using System;
using System.Collections.Generic;
using Engine;
using Framework.Managers;
using Game.Scene;
using LuaInterface;
using Models;
using msg;
using UnityEngine;

internal class ChooseRoleController : ControllerBase
{
    public override string ControllerName
    {
        get
        {
            return base.GetType().ToString();
        }
        set
        {
        }
    }

    public UI_ChooseRole ChooseRoleUI
    {
        get
        {
            return UIManager.GetUIObject<UI_ChooseRole>();
        }
    }

    public void PreLoad(Action Callback)
    {
        FFDebug.Log(this, FFLogType.Default, "ChooseRoleController Init");
        SimpleTaskQueue simpleTaskQueue = new SimpleTaskQueue();
        simpleTaskQueue.OnStep = delegate (SimpleTaskQueue.Task st)
        {
            FFDebug.Log(this, FFLogType.Login, "ChooseRoleController--->" + st.WorkName);
        };
        simpleTaskQueue.Finish = delegate ()
        {
            this.LoadOver();
            Callback();
        };
        simpleTaskQueue.AddTask(new Action<Action>(this.LoadToChooseRoleScene), "加载场景", false);
        simpleTaskQueue.AddTask(new Action<Action>(this.LoadArtConfig), "加载美术配置", false);
        simpleTaskQueue.AddTask(new Action<Action>(this.LoadRole), "加载角色", false);
        simpleTaskQueue.AddTask(new Action<Action>(this.LoadUIView), "加载UI", false);
        simpleTaskQueue.Start();
    }

    private void LoadUIView(Action Callback)
    {
        UI_ChooseRole.LoadView(delegate
        {
            Callback();
        });
    }

    private void LoadToChooseRoleScene(Action Callback)
    {
        SceneInfo sceneInfo = new SceneInfo();
        sceneInfo.mapid = 1U;
        sceneInfo.mapname = "ChooseRole";
        sceneInfo.lineid = 1U;
        sceneInfo.pos = new FloatMovePos();
        ManagerCenter.Instance.GetManager<GameScene>().ChangeScene(sceneInfo, delegate
        {
            ManagerCenter.Instance.GetManager<GameScene>().OnSceneLoadNotifyServer();
            UI_ChooseRole.LoadView(delegate
            {
                Callback();
            });
        }, new Action<float>(this.LoadProgress));
    }

    private void LoadRole(Action Callback)
    {
        this.RoleList.Clear();
        SimpleTaskQueue simpleTaskQueue = new SimpleTaskQueue();
        simpleTaskQueue.OnStep = delegate (SimpleTaskQueue.Task st)
        {
        };
        simpleTaskQueue.Finish = delegate ()
        {
            Callback();
        };
        if (this.artConfig != null)
        {
            Camera.main.transform.position = this.artConfig.CameraPos;
            Camera.main.transform.rotation = this.artConfig.CameraRot;
            this.CameraAnimation = Camera.main.gameObject.AddComponent<Animation>();
            for (int i = 0; i < this.artConfig.SingleRoleConfigArray.Length; i++)
            {
                SingleRoleConfig singleRoleConfig = this.artConfig.SingleRoleConfigArray[i];
                this.ArtconfigMap[singleRoleConfig.RoleName] = singleRoleConfig;
                this.CameraAnimation.AddClip(singleRoleConfig.go, singleRoleConfig.go.name);
                this.CameraAnimation.AddClip(singleRoleConfig.back, singleRoleConfig.back.name);
                this.CameraAnimation.AddClip(singleRoleConfig.sideR, singleRoleConfig.sideR.name);
                this.CameraAnimation.AddClip(singleRoleConfig.sidlL, singleRoleConfig.sidlL.name);
            }
        }
        List<LuaTable> configTableList = LuaConfigManager.GetConfigTableList("newUser");
        for (int j = 0; j < configTableList.Count; j++)
        {
            LuaTable luaTable = configTableList[j];
            if (this.ArtconfigMap.ContainsKey(luaTable.GetField_String("modelname")))
            {
                SelectRole selectRole = new SelectRole();
                selectRole.occupation = luaTable.GetField_Uint("id");
                selectRole.config = luaTable;
                selectRole.artConfig = this.ArtconfigMap[luaTable.GetField_String("modelname")];
                this.RoleList.Add(selectRole);
                simpleTaskQueue.AddTask(new Action<Action>(selectRole.Load), luaTable.GetField_String("modelname"), false);
            }
            else
            {
                FFDebug.LogWarning(this, string.Format("Cont find SingleRoleConfig: {0}", luaTable.GetField_String("modelname")));
            }
        }
        simpleTaskQueue.Start();
        this.SetSelect(null);
    }

    private void LoadArtConfig(Action Callback)
    {
        FFAssetBundleRequest.CleverRequest(CharactorAndEffectBundleType.AnimatorController, "ChooseRoleArtConfig.u", delegate (FFAssetBundle ab)
        {
            if (ab != null)
            {
                this.configAb = ab;
                this.artConfig = this.configAb.GetMainAsset<ChooseRoleArtConfig>();
            }
            else
            {
                FFDebug.LogWarning(this, "LoadArtConfig  Error");
            }
            Callback();
        });
    }

    public void Show()
    {
        if (this.ChooseRoleUI != null)
        {
            this.ChooseRoleUI.input_name.text = ControllerManager.Instance.GetLoginController().loginModel.Account;
            this.ChooseRoleUI.Show();
        }
    }

    public void BackToRoleSelectView()
    {
        this.OnRoleSelect = true;
        this.MoveCameraBack();
        this.SetSelect(null);
    }

    private void LoadProgress(float progress)
    {
    }

    private void LoadOver()
    {
        SingletonForMono<InputController>.Instance.mScreenEventController.AddListener(new ScreenEventController.OnScreenEvent(this.OnScreenEvent));
        this.BackToRoleSelectView();
    }

    private void OnScreenEvent(ScreenEvent SE)
    {
        if (this.OnRoleSelect)
        {
            Ray ray = ResolutionManager.Instance.MainCameraScreenPointToRay(SE.InputPos);
            RaycastHit raycastHit;
            if (Physics.Raycast(ray, out raycastHit, 512f))
            {
                this.ClickSelectOnChooseJobView(raycastHit.collider.gameObject);
            }
        }
        else if (SE.mTpye == ScreenEvent.EventType.Slip)
        {
            float x = SE.SlipDis.x;
            if (this.rotTable != null)
            {
                this.rotTable.transform.Rotate(new Vector3(0f, -x, 0f));
            }
        }
    }

    public void SwitchSelectOnCreatRoleView(uint id)
    {
        if (this.CurrSelectRole == null)
        {
            return;
        }
        if (this.CurrSelectRole.config.GetField_Uint("id") == id)
        {
            return;
        }
        for (int i = 0; i < this.RoleList.Count; i++)
        {
            if (this.RoleList[i].config.GetField_Uint("id") == id)
            {
                this.MoveCameraSideL();
                this.SetSelect(this.RoleList[i]);
            }
        }
    }

    private void SetSelect(SelectRole Select)
    {
        if (Select == this.CurrSelectRole)
        {
            return;
        }
        for (int i = 0; i < this.RoleList.Count; i++)
        {
            this.RoleList[i].IsSelect(Select == this.RoleList[i]);
        }
        this.OutOfRottable();
        this.CurrSelectRole = Select;
        this.SetToRottable();
        if (this.CurrSelectRole != null)
        {
            if (this.ChooseRoleUI != null)
            {
                this.ChooseRoleUI.OpenCreatRoleView(this.CurrSelectRole);
                this.ChooseRoleUI.OpenCreatRoleView(this.CurrSelectRole);
            }
            this.OnRoleSelect = false;
        }
    }

    private void ClickSelectOnChooseJobView(GameObject obj)
    {
        if (!this.OnRoleSelect)
        {
            return;
        }
        for (int i = 0; i < this.RoleList.Count; i++)
        {
            if (this.RoleList[i].Modelobj == obj)
            {
                this.SetSelect(this.RoleList[i]);
            }
        }
        this.MoveCameraIn();
    }

    public void Register()
    {
        if (this.CurrSelectRole == null)
        {
            return;
        }
        ControllerManager.Instance.GetLoginController().RegisterPlayer(this.ChooseRoleUI.input_name.text, this.CurrSelectRole.occupation);
    }

    private void MoveCameraBack()
    {
        if (this.CurrSelectRole != null)
        {
            this.CameraAnimation.CrossFade(this.CurrSelectRole.artConfig.back.name, 0f);
        }
    }

    private void MoveCameraIn()
    {
        if (this.CurrSelectRole != null)
        {
            this.CameraAnimation.CrossFade(this.CurrSelectRole.artConfig.go.name, 0f);
        }
    }

    private void MoveCameraSideR()
    {
        if (this.CurrSelectRole != null)
        {
            this.CameraAnimation.CrossFade(this.CurrSelectRole.artConfig.sideR.name, 0f);
        }
    }

    private void MoveCameraSideL()
    {
        if (this.CurrSelectRole != null)
        {
            this.CameraAnimation.CrossFade(this.CurrSelectRole.artConfig.sidlL.name, 0f);
        }
    }

    public void Dispose()
    {
        if (this.rotTable != null)
        {
            UnityEngine.Object.Destroy(this.rotTable);
        }
        if (this.CameraAnimation != null)
        {
            UnityEngine.Object.Destroy(this.CameraAnimation);
        }
        ManagerCenter.Instance.GetManager<UIManager>().DeleteUI("UI_ChooseRole");
        for (int i = 0; i < this.RoleList.Count; i++)
        {
            this.RoleList[i].Unload();
        }
        this.RoleList.Clear();
        if (this.configAb != null)
        {
            this.configAb.Unload();
        }
        SingletonForMono<InputController>.Instance.mScreenEventController.RemoveListener(new ScreenEventController.OnScreenEvent(this.OnScreenEvent));
    }

    private void SetToRottable()
    {
        if (this.CurrSelectRole == null)
        {
            return;
        }
        if (this.CurrSelectRole.Modelobj == null)
        {
            return;
        }
        if (this.rotTable == null)
        {
            this.rotTable = new GameObject();
            this.rotTable.name = "rotTable";
            this.rotTable.transform.localScale = Vector3.one;
        }
        this.rotTable.transform.localEulerAngles = Vector3.zero;
        this.rotTable.transform.position = this.CurrSelectRole.Modelobj.transform.position;
        this.CurrSelectRole.Modelobj.transform.SetParent(this.rotTable.transform);
        this.CurrSelectRole.Modelobj.transform.localPosition = Vector3.zero;
    }

    private void OutOfRottable()
    {
        if (this.CurrSelectRole == null)
        {
            return;
        }
        if (this.CurrSelectRole.Modelobj == null)
        {
            return;
        }
        if (this.rotTable == null)
        {
            return;
        }
        if (this.CurrSelectRole.Modelobj.transform.parent == this.rotTable.transform)
        {
            this.rotTable.transform.localEulerAngles = Vector3.zero;
            this.CurrSelectRole.Modelobj.transform.SetParent(null);
            this.CurrSelectRole.Modelobj.transform.localPosition = this.rotTable.transform.position;
        }
    }

    private FFAssetBundle configAb;

    private ChooseRoleArtConfig artConfig;

    private Dictionary<string, SingleRoleConfig> ArtconfigMap = new Dictionary<string, SingleRoleConfig>();

    private Animation CameraAnimation;

    private List<SelectRole> RoleList = new List<SelectRole>();

    private SelectRole CurrSelectRole;

    private bool OnRoleSelect = true;

    private GameObject rotTable;
}
