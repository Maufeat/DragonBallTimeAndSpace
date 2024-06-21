using System;
using Framework.Base;
using UnityEngine;

public class RenderTextureMgr : IManager
{
    public string ManagerName
    {
        get
        {
            return base.GetType().Name;
        }
    }

    public void Init()
    {
        this.InitRenderTextureGameObject();
    }

    private void InitRenderTextureGameObject()
    {
        this.RTGameRoot = new GameObject();
        this.RTGameRoot.name = "RTRoot";
        this.RTGameRoot.AddComponent<DontDestroyOnLoad>();
        for (int i = 0; i < this.PreLoadRTNames.Length; i++)
        {
            string name = this.PreLoadRTNames[i];
            this.LoadObjectsAB("rt0001", delegate (GameObject o)
            {
                GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(o);
                gameObject.name = name;
                gameObject.transform.SetParent(this.RTGameRoot.transform);
                Transform transform = gameObject.transform.Find("cam");
                if (transform)
                {
                    transform.localPosition = new Vector3(0f, 0f, 9f);
                    Camera component = transform.GetComponent<Camera>();
                    if (component != null)
                    {
                        component.fieldOfView = 20f;
                    }
                }
                gameObject.SetActive(false);
                this.rtGameObject.Add(gameObject.name, gameObject);
            });
        }
    }

    private void LoadObjectsAB(string filename, Action<GameObject> callback)
    {
        FFAssetBundleRequest.Request("Objects", "objectsprefab/" + filename, delegate (FFAssetBundle ab)
        {
            callback(ab.GetMainAsset<GameObject>());
        }, true);
    }

    private GameObject GetRTGameObject(string name)
    {
        GameObject result = null;
        if (this.rtGameObject.TryGetValue(name, out result))
        {
            return result;
        }
        return null;
    }

    public GameObject UseRTGameObject(string name)
    {
        GameObject rtgameObject = this.GetRTGameObject(name);
        if (rtgameObject != null)
        {
            rtgameObject.SetActive(true);
        }
        return rtgameObject;
    }

    public void FreeRTGameObject(string name)
    {
        GameObject rtgameObject = this.GetRTGameObject(name);
        if (rtgameObject != null)
        {
            rtgameObject.SetActive(false);
        }
    }

    public void ResetRootPosition(string name, Vector3 v3Pos)
    {
        if (null != this.RTGameRoot)
        {
            GameObject rtgameObject = this.GetRTGameObject(name);
            rtgameObject.transform.position = v3Pos;
        }
    }

    public void OnUpdate()
    {
    }

    public void OnReSet()
    {
        if (this.RTGameRoot != null)
        {
            UnityEngine.Object.Destroy(this.RTGameRoot);
        }
        this.rtGameObject.Clear();
    }

    private GameObject RTGameRoot;

    private BetterDictionary<string, GameObject> rtGameObject = new BetterDictionary<string, GameObject>();

    private string[] PreLoadRTNames = new string[]
    {
        "HeroHandbook",
        "Character",
        "NpcTalk"
    };
}
