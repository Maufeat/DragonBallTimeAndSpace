using System;
using Framework.Base;
using Framework.Managers;
using Game.Scene;
using UnityEngine;
using UnityEngine.UI;

public class NpctalkRawCharactorMgr : IManager
{
    public string ManagerName
    {
        get
        {
            return base.GetType().Name;
        }
    }

    public void OnUpdate()
    {
    }

    public void OnReSet()
    {
    }

    public void Init()
    {
        this.InitRenderTextureGameObject();
    }

    public GameObject RTGameRoot { get; private set; }

    public void InitAssetsPosData(Action Callback)
    {
        FFAssetBundleRequest.CleverRequest(CharactorAndEffectBundleType.NPCTalk, "npctalktransformdata", delegate (FFAssetBundle ab)
        {
            if (ab == null)
            {
                Callback();
                return;
            }
            Callback();
        });
    }

    public void DisposeCharactor()
    {
        if (this.RTRawActor_L != null)
        {
            this.RTRawActor_L.DestroyThisInNineScreen();
            this.RTRawActor_L.TrueDestory();
            this.RTRawActor_L = null;
        }
        if (this.RTRawActor_M != null)
        {
            this.RTRawActor_M.DestroyThisInNineScreen();
            this.RTRawActor_M.TrueDestory();
            this.RTRawActor_M = null;
        }
        if (this.RTRawActor_R != null)
        {
            this.RTRawActor_R.DestroyThisInNineScreen();
            this.RTRawActor_R.TrueDestory();
            this.RTRawActor_R = null;
        }
        this.rtroot_L.gameObject.SetActive(false);
        this.rtroot_M.gameObject.SetActive(false);
        this.rtroot_R.gameObject.SetActive(false);
    }

    public void CreateModelByNpcId(uint npcId, Action<GameObject> back)
    {
        uint[] featureIDs = null;
        FFCharacterModelHold.CreateModel(npcId, 0U, featureIDs, string.Empty, delegate (PlayerCharactorCreateHelper CharModel)
        {
            if (CharModel != null && CharModel.rootObj != null)
            {
                back(CharModel.rootObj);
                if (ManagerCenter.Instance.GetManager<GameScene>().LightInfo != null)
                {
                    ManagerCenter.Instance.GetManager<GameScene>().SetMatLightInfo(CharModel.rootObj, false);
                }
            }
        }, null, 0UL);
    }

    public void GetRawCharactor(RawImage rw_role, uint npcid, uint type)
    {
        if (type == 1U)
        {
            this.rtroot_L.gameObject.SetActive(true);
            Camera component = this.rtroot_L.Find("cam").GetComponent<Camera>();
            rw_role.texture = new RenderTexture(1024, 1024, 24, RenderTextureFormat.ARGB32);
            component.targetTexture = (rw_role.texture as RenderTexture);
            if (this.RTRawActor_L == null)
            {
                this.InitRtRole(this.strRTName, npcid, 1U);
            }
            else
            {
                this.RTRawActor_L.RefreshModel(this.strRTName, npcid, Const.Layer.RT);
            }
        }
        else if (type == 2U)
        {
            this.rtroot_M.gameObject.SetActive(true);
            Camera component = this.rtroot_M.Find("cam").GetComponent<Camera>();
            component.targetTexture = (rw_role.texture as RenderTexture);
            if (this.RTRawActor_M == null)
            {
                this.InitRtRole(this.strRTName, npcid, 2U);
            }
            else
            {
                this.RTRawActor_M.RefreshModel(this.strRTName, npcid, Const.Layer.RT);
            }
        }
        else if (type == 3U)
        {
            this.rtroot_R.gameObject.SetActive(true);
            Camera component = this.rtroot_R.Find("cam").GetComponent<Camera>();
            rw_role.texture = new RenderTexture(1024, 1024, 24, RenderTextureFormat.ARGB32);
            component.targetTexture = (rw_role.texture as RenderTexture);
            if (this.RTRawActor_R == null)
            {
                this.InitRtRole(this.strRTName, npcid, 3U);
            }
            else
            {
                this.RTRawActor_R.RefreshModel(this.strRTName, npcid, Const.Layer.RT);
            }
        }
    }

    private void InitRenderTextureGameObject()
    {
        this.RTGameRoot = new GameObject();
        this.RTGameRoot.name = "NpcTalkRTRoot";
        this.RTGameRoot.AddComponent<DontDestroyOnLoad>();
        this.RTGameRoot.transform.localPosition = new Vector3(0f, 1000f, 0f);
        FFAssetBundleRequest.Request("Objects", "objectsprefab/rt0001", delegate (FFAssetBundle ab)
        {
            if (ab != null)
            {
                GameObject mainAsset = ab.GetMainAsset<GameObject>();
                if (mainAsset != null)
                {
                    this.rtroot_L = UnityEngine.Object.Instantiate<GameObject>(mainAsset).transform;
                    this.rtroot_L.name = "rtl";
                    this.rtroot_L.SetParent(this.RTGameRoot.transform);
                    this.rtroot_L.localPosition = new Vector3(50f, 0f, 0f);
                    this.rtroot_L.gameObject.SetActive(false);
                    this.rtroot_M = UnityEngine.Object.Instantiate<GameObject>(mainAsset).transform;
                    this.rtroot_M.name = "rtm";
                    this.rtroot_M.SetParent(this.RTGameRoot.transform);
                    this.rtroot_M.localPosition = new Vector3(100f, 0f, 0f);
                    this.rtroot_M.gameObject.SetActive(false);
                    this.rtroot_R = UnityEngine.Object.Instantiate<GameObject>(mainAsset).transform;
                    this.rtroot_R.name = "rtr";
                    this.rtroot_R.SetParent(this.RTGameRoot.transform);
                    this.rtroot_R.localPosition = new Vector3(150f, 0f, 0f);
                    this.rtroot_R.gameObject.SetActive(false);
                }
            }
        }, true);
    }

    private void InitRtRole(string strRTName, uint npcid, uint type)
    {
        if (type == 1U)
        {
            this.RTRawActor_L = new NpctalkRawCharactor();
            this.RTRawActor_L.CreateModel(strRTName, npcid, Const.Layer.RT, 1U);
        }
        else if (type == 2U)
        {
            this.RTRawActor_M = new NpctalkRawCharactor();
            this.RTRawActor_M.CreateModel(strRTName, npcid, Const.Layer.RT, 2U);
        }
        else if (type == 3U)
        {
            this.RTRawActor_R = new NpctalkRawCharactor();
            this.RTRawActor_R.CreateModel(strRTName, npcid, Const.Layer.RT, 3U);
        }
    }

    private string strRTName = "rt0001";

    public NpctalkRawCharactor RTRawActor_L;

    public NpctalkRawCharactor RTRawActor_M;

    public NpctalkRawCharactor RTRawActor_R;

    private Transform rtroot_L;

    private Transform rtroot_M;

    private Transform rtroot_R;
}
