using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Framework.Base;
using Framework.Managers;
using Game.Scene;
using ProtoBuf;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ResoureManager
{
    // Token: 0x020009AD RID: 2477
    public class ResourceManager : MonoBehaviour, IManager
    {
        // Token: 0x17000F6D RID: 3949
        // (get) Token: 0x0600566E RID: 22126 RVA: 0x00166930 File Offset: 0x00164B30
        string IManager.ManagerName
        {
            get
            {
                return "resource_manager";
            }
        }

        // Token: 0x0600566F RID: 22127 RVA: 0x00166938 File Offset: 0x00164B38
        private void Awake()
        {
            if (Const.DebugMode)
            {
            }
            FFAssetBundleRequest.MbHold = this;
        }

        // Token: 0x06005670 RID: 22128 RVA: 0x0016694C File Offset: 0x00164B4C
        public GameObject LoadResourceGameobject(string name, ResouresType type)
        {
            string resouresPath = CommonUtil.GetResouresPath(name, type);
            return Resources.Load<GameObject>(resouresPath);
        }

        // Token: 0x06005671 RID: 22129 RVA: 0x00166968 File Offset: 0x00164B68
        public GameObject LoadResourceObject(string path)
        {
            return Resources.Load(path) as GameObject;
        }

        // Token: 0x06005672 RID: 22130 RVA: 0x00166978 File Offset: 0x00164B78
        public T LoadResourceObject<T>(string path) where T : UnityEngine.Object
        {
            return (T)((object)Resources.Load<T>(path));
        }

        // Token: 0x06005673 RID: 22131 RVA: 0x00166994 File Offset: 0x00164B94
        public void LoadByteData(string strPath, Action<byte[]> callback)
        {
            try
            {
                string path = LoadHelper.GetPath(strPath, false);
                if (string.IsNullOrEmpty(path))
                {
                    FFDebug.LogWarning(this, "Not found Byte path:[" + strPath + "]");
                    callback(null);
                }
                else if (!File.Exists(path))
                {
                    callback(null);
                }
                else
                {
                    byte[] array;
                    using (FileStream fileStream = File.OpenRead(path))
                    {
                        array = new byte[fileStream.Length];
                        fileStream.Read(array, 0, array.Length);
                        fileStream.Close();
                    }
                    if (array == null)
                    {
                        FFDebug.LogWarning(this, "Byte data is null,path[" + strPath + string.Empty);
                        callback(null);
                    }
                    else
                    {
                        callback(array);
                        array = null;
                    }
                }
            }
            catch (Exception ex)
            {
                FFDebug.LogError(this, "byte fail,file path:[" + strPath + "]ExceptionMessage : " + ex.ToString());
                callback(null);
            }
        }

        // Token: 0x06005674 RID: 22132 RVA: 0x00166AC0 File Offset: 0x00164CC0
        public T LoadProtoBuffData<T>(string path) where T : IExtensible
        {
            T t = default(T);
            this.LoadByteData(path, delegate (byte[] bytes)
            {
                using (MemoryStream memoryStream = new MemoryStream(bytes))
                {
                    try
                    {
                        t = Serializer.Deserialize<T>(memoryStream);
                    }
                    catch (Exception ex)
                    {
                        FFDebug.LogError(this, "LoadProtoBuffData Exception :" + ex.Message);
                    }
                }
            });
            return t;
        }

        // Token: 0x06005675 RID: 22133 RVA: 0x00166B04 File Offset: 0x00164D04
        public AseetItem LoadAssetItem(string path, AssetbundleLoadComplete callback)
        {
            path = LoadHelper.GetPath(path, true);
            AseetItem aseetItem = new AseetItem(path);
            FFDebug.Log(this, FFLogType.AssetBundleLoad, string.Format("Try LoadBytes: [{0}]", path));
            AseetItem aseetItem2 = null;
            if (this.m_MapAssetItems.TryGetValue(path, out aseetItem2))
            {
                if (string.IsNullOrEmpty(aseetItem2.Req.error) && null != aseetItem2.Req.assetBundle)
                {
                    callback(aseetItem2);
                    return aseetItem2;
                }
            }
            else
            {
                this.m_MapAssetItems.Add(path, aseetItem);
            }
            base.StartCoroutine(this.loadAseetItem(aseetItem, callback));
            return aseetItem;
        }

        // Token: 0x06005676 RID: 22134 RVA: 0x00166BA4 File Offset: 0x00164DA4
        private IEnumerator loadAseetItem(AseetItem item, AssetbundleLoadComplete callback)
        {
            FFDebug.Log(this, FFLogType.AssetBundleLoad, string.Format("path:-----[{0}]", item.Path));
            yield return 0;
            item.Req = new WWW(item.Path);
            float lastTime = Time.realtimeSinceStartup;
            yield return item.Req;
            callback(item);
            yield break;
        }

        // Token: 0x06005677 RID: 22135 RVA: 0x00166BDC File Offset: 0x00164DDC
        public void LoadScene(string path, string name, AssetbundleLoadComplete callback)
        {
            AssetManager.LoadAssetBundle(path, delegate (string assetbundlename, bool success)
            {
                if (!success)
                {
                    AssetManager.LoadAssetBundle(path.Remove(path.Length - 2) + "_Lightmap.u", delegate (string lmbundlename, bool lmsucess)
                    {
                        this.StartCoroutine(this.enterScene(name, callback));
                    }, Bundle.BundleType.Default);
                }
                else
                {
                    this.StartCoroutine(this.enterScene(name, callback));
                }
            }, Bundle.BundleType.Default);
        }

        // Token: 0x17000F6E RID: 3950
        // (get) Token: 0x06005678 RID: 22136 RVA: 0x00166C24 File Offset: 0x00164E24
        public bool getUseNavMesh
        {
            get
            {
                return !string.IsNullOrEmpty(this.lastNavMeshAssetBundleName);
            }
        }

        // Token: 0x06005679 RID: 22137 RVA: 0x00166C34 File Offset: 0x00164E34
        public void LoadNavMeshScene(string path, string name, AssetbundleLoadComplete callback)
        {
            if (!string.IsNullOrEmpty(this.lastNavMeshAssetBundleName))
            {
                AssetManager.UnloadAssetBundle(this.lastNavMeshAssetBundleName, true);
            }
            AssetManager.LoadAssetBundle(path, delegate (string assetbundlename, bool success)
            {
                if (!success)
                {
                    this.lastNavMeshAssetBundleName = string.Empty;
                    callback(null);
                }
                else
                {
                    this.lastNavMeshAssetBundleName = assetbundlename;
                    this.StartCoroutine(this.AddScene(name, callback));
                }
            }, Bundle.BundleType.Default);
        }

        // Token: 0x0600567A RID: 22138 RVA: 0x00166C8C File Offset: 0x00164E8C
        private IEnumerator AddScene(string name, AssetbundleLoadComplete callback)
        {
            AsyncOperation asyCallback = null;
            if (Application.CanStreamedLevelBeLoaded(name))
            {
                asyCallback = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(name, LoadSceneMode.Additive);
            }
            if (asyCallback == null)
            {
                callback(null);
                yield break;
            }
            yield return asyCallback;
            callback(null);
            yield break;
        }

        // Token: 0x0600567B RID: 22139 RVA: 0x00166CBC File Offset: 0x00164EBC
        private IEnumerator enterScene(string name, AssetbundleLoadComplete callback)
        {
            AsyncOperation asyCallback;
            if (Application.CanStreamedLevelBeLoaded(name))
            {
                asyCallback = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(name);
                GameScene gsManager = ManagerCenter.Instance.GetManager<GameScene>();
                if (gsManager != null)
                {
                    gsManager.sceneData.useCustomScene = true;
                }
            }
            else if (Application.CanStreamedLevelBeLoaded(name + "_Lightmap"))
            {
                asyCallback = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(name + "_Lightmap");
            }
            else
            {
                asyCallback = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("TmpScene");
            }
            yield return asyCallback;
            callback(null);
            yield break;
        }

        // Token: 0x0600567C RID: 22140 RVA: 0x00166CEC File Offset: 0x00164EEC
        public void LoadAssetFromAssetbundle(AssetBundle ab, string name, Action<UnityEngine.Object> callback)
        {
            base.StartCoroutine(this.loadAssetFromAssetbundle(ab, name, callback));
        }

        // Token: 0x0600567D RID: 22141 RVA: 0x00166D00 File Offset: 0x00164F00
        public IEnumerator loadAssetFromAssetbundle(AssetBundle ab, string name, Action<UnityEngine.Object> callback)
        {
            AssetBundleRequest req = ab.LoadAssetAsync(name);
            yield return req;
            callback(req.asset);
            yield break;
        }

        // Token: 0x0600567E RID: 22142 RVA: 0x00166D40 File Offset: 0x00164F40
        public void LoadAllAssetFromAssetbundle(AssetBundle ab, Action<UnityEngine.Object[]> callback)
        {
            base.StartCoroutine(this.loadAllAssetFromAssetbundle(ab, callback));
        }

        // Token: 0x0600567F RID: 22143 RVA: 0x00166D54 File Offset: 0x00164F54
        public IEnumerator loadAllAssetFromAssetbundle(AssetBundle ab, Action<UnityEngine.Object[]> callback)
        {
            AssetBundleRequest req = ab.LoadAllAssetsAsync();
            yield return req;
            callback(req.allAssets);
            yield break;
        }

        // Token: 0x06005680 RID: 22144 RVA: 0x00166D84 File Offset: 0x00164F84
        public AssetBundle RequestLoadAssetbundle(object application, string path, AssetbundleLoadComplete assetbundleLoadComplete = null)
        {
            path = LoadHelper.GetPath(path, false);
            AssetBundle assetBundle = AssetBundle.LoadFromFile(path);
            if (null == assetBundle)
            {
                FFDebug.LogWarning(this, string.Concat(new string[]
                {
                    "Assetbundle Load Faild,Please check!path:[",
                    path,
                    "],error From:[",
                    application.ToString(),
                    "]"
                }));
            }
            else if (assetbundleLoadComplete != null)
            {
                assetbundleLoadComplete(null);
            }
            return assetBundle;
        }

        // Token: 0x06005681 RID: 22145 RVA: 0x00166DF8 File Offset: 0x00164FF8
        public void Unload(string path, bool force)
        {
            path = LoadHelper.GetPath(path, true);
            AseetItem aseetItem = null;
            if (this.m_MapAssetItems.TryGetValue(path, out aseetItem))
            {
                aseetItem.Req.assetBundle.Unload(force);
                this.m_MapAssetItems.Remove(path);
            }
            else
            {
                FFDebug.LogWarning(this, "Don't found loadasset task or assetbundle,name:[" + path + "]");
            }
        }

        // Token: 0x06005682 RID: 22146 RVA: 0x00166E5C File Offset: 0x0016505C
        public void OnUpdate()
        {
            FFAssetBundleRequest.UpdateALL();
            FFAssetBundle.UpdateALL();
        }

        // Token: 0x06005683 RID: 22147 RVA: 0x00166E68 File Offset: 0x00165068
        public void OnReSet()
        {
        }

        // Token: 0x04003797 RID: 14231
        private const int READ_MAX_BYTE = 1048576;

        // Token: 0x04003798 RID: 14232
        private ICompent m_Compent;

        // Token: 0x04003799 RID: 14233
        private string m_guiMessage = string.Empty;

        // Token: 0x0400379A RID: 14234
        private Dictionary<string, AseetItem> m_MapAssetItems = new Dictionary<string, AseetItem>();

        // Token: 0x0400379B RID: 14235
        public bool Ready;

        // Token: 0x0400379C RID: 14236
        private string lastNavMeshAssetBundleName;
    }
}
