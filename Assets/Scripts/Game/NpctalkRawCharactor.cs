using System;
using Framework.Managers;
using Game.Scene;
using LuaInterface;
using UnityEngine;

public class NpctalkRawCharactor : CharactorBase
{
    public NpctalkRawCharactor()
    {
        base.CharState = CharactorState.CreatEntity;
        this.Init();
    }

    public void CreateModel(string name, uint npcId, int layer, uint type)
    {
        this.Type = type;
        this.stName = name;
        this._npcId = npcId;
        LuaTable configTable = LuaConfigManager.GetConfigTable("npc_data", (ulong)npcId);
        this.stModelName = configTable.GetField_String("model");
        this.stAniCtrName = configTable.GetField_String("animatorcontroller");
        this.iLayerID = layer;
        Scheduler.Instance.AddUpdator(new Scheduler.OnScheduler(this.Update));
        base.CharState = CharactorState.CreatModel;
        this.LoadModel(new Action(this.OnModelLoadOver));
    }

    public void RefreshModel(string name, uint npcId, int layer)
    {
        this.stName = name;
        this._npcId = npcId;
        LuaTable configTable = LuaConfigManager.GetConfigTable("npc_data", (ulong)npcId);
        if (this.stAniCtrName == configTable.GetField_String("animatorcontroller") && this.stModelName == configTable.GetField_String("model"))
        {
            this.SetupIdleState();
            return;
        }
        if (this.stAniCtrName == configTable.GetField_String("animatorcontroller"))
        {
            this.stModelName = configTable.GetField_String("model");
            this.ChangeModel(configTable.GetField_String("model"), new Action(this.LoadModelCallback));
        }
        else
        {
            this.stModelName = configTable.GetField_String("model");
            this.stAniCtrName = configTable.GetField_String("animatorcontroller");
            this.LoadModel(new Action(this.LoadModelCallback));
        }
    }

    private void LoadModelCallback()
    {
        base.ResetAllCompment();
        this.SetupIdleState();
        if (base.ModelObj != null)
        {
            base.ModelObj.transform.eulerAngles = new Vector3(0f, 300f, 0f);
        }
    }

    public void ChangeModel(string modelname, Action callback)
    {
        CharacterModelMgr mCharacterModelMgr = ManagerCenter.Instance.GetManager<CharacterModelMgr>();
        mCharacterModelMgr.LoadSimpleCharacterObj(modelname, delegate
        {
            mCharacterModelMgr.SetMeshAndMaterialSmr(modelname, this.ModelObj.GetComponentInChildren<SkinnedMeshRenderer>());
            this.stModelName = modelname;
            if (callback != null)
            {
                callback();
            }
        });
    }

    public void SetupIdleState()
    {
        LuaTable configTable = LuaConfigManager.GetConfigTable("heros", (ulong)this._npcId);
        FFRawCharactorIdleState component = base.GetComponent<FFRawCharactorIdleState>();
        if (component != null && configTable != null)
        {
            component.Setup(configTable.GetField_String("modelact"));
        }
        if (base.ModelObj != null)
        {
            base.ModelObj.transform.eulerAngles = new Vector3(0f, 300f, 0f);
        }
    }

    public void ChangeWeapon(uint modelid)
    {
        base.GetComponent<FFWeaponHold>().ChangeWeapon(modelid);
        if (base.ModelObj != null)
        {
            base.ModelObj.SetLayer(this.iLayerID, true);
        }
    }

    private void LoadModel(Action callback)
    {
        uint[] featureIDs = null;
        FFCharacterModelHold.CreateModel(this._npcId, 0U, featureIDs, string.Empty, delegate (PlayerCharactorCreateHelper CharModel)
        {
            this.DestroyModel();
            this.ModelHold = CharModel;
            this.ModelObj = this.ModelHold.rootObj;
            if (this.ModelObj != null)
            {
                this.ModelObj.SetLayer(this.iLayerID, true);
                this.ModelObj.transform.Reset();
                this.ModelObj.name = this.stName;
                this.SetModelPosition();
                if (callback != null)
                {
                    callback();
                }
                this.ResetModelRenderInfo();
            }
        }, null, 0UL);
    }

    private void SetModelPosition()
    {
        if (this.Type == 1U)
        {
            base.SetWorldPosition(new Vector3(50f, 999.5f, 5.5f));
        }
        else if (this.Type == 2U)
        {
            base.SetWorldPosition(new Vector3(100f, 999.5f, 5.5f));
        }
        else if (this.Type == 3U)
        {
            base.SetWorldPosition(new Vector3(150f, 999.5f, 5.5f));
        }
    }

    public string GetModelName()
    {
        return this.stModelName;
    }

    private void OnModelLoadOver()
    {
        this.InitComponent();
        base.CharState = CharactorState.CreatComplete;
        this.SetupIdleState();
    }

    private void ResetModelRenderInfo()
    {
        if (ManagerCenter.Instance.GetManager<GameScene>().LightInfo != null)
        {
            ManagerCenter.Instance.GetManager<GameScene>().SetMatLightInfo(base.ModelObj, false);
        }
    }

    public override void InitComponent()
    {
        base.InitComponent();
        base.AddComponent(new FFRawCharactorIdleState());
        this.ComponentMgr.InitOver();
    }

    public virtual void Update()
    {
        if (this.ComponentMgr != null)
        {
            this.ComponentMgr.Update();
        }
    }

    public void DestroyModel()
    {
        if (this.ModelHold != null)
        {
            this.ModelHold.DisposeBonePObj();
        }
        base.ModelObj = null;
    }

    public void TrueDestory()
    {
        if (this.ModelHold != null)
        {
            this.ModelHold.DisposeBonePObj();
        }
        base.ModelObj = null;
        Scheduler.Instance.RemoveUpdator(new Scheduler.OnScheduler(this.Update));
    }

    private string stName = "rawactor";

    private string stModelName = string.Empty;

    private string stAniCtrName = string.Empty;

    private int iLayerID = Const.Layer.Charactor;

    private uint _npcId;

    private uint Type;

    public PlayerCharactorCreateHelper ModelHold;
}
