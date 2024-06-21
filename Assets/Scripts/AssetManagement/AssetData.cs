using UnityEngine;

public class AssetData
{
    public AssetBundleCreateRequest AssetBundleCreateRequest;
    public AssetBundleRequest AssetBundleRequest;
    public OnAssetBundleLoadComplete OnAssetBundleLoadComplete;
    public OnAssetLoadComplete OnAssetLoadComplete;

    public AssetData(AssetLoadType type, string filename)
    {
        this.AssetLoadType = type;
        this.FileName = filename;
        this.DownloadTaskState = DownloadTaskState.Wait;
        this.FileFullPath = ResourceHelper.GetPath(filename);
        this.CreateAssetBundle_Type = ResourceHelper.createAssetBundleType;
    }

    public string FileName { get; private set; }
    public string FileFullPath { get; private set; }
    public float Process { get; set; }
    public string AssetName { get; set; }
    public System.Type AssetType { get; set; }
    public string Message { get; set; }
    public Bundle Bundle { get; set; }
    public AssetLoadType AssetLoadType { get; private set; }
    public CreateAssetBundleType CreateAssetBundle_Type { get; private set; }
    public DownloadTaskState DownloadTaskState { get; set; }

    public void Rest()
    {
        this.DownloadTaskState = DownloadTaskState.Error;
        this.AssetLoadType = AssetLoadType.LoadAsset;
        this.FileName = string.Empty;
        this.AssetName = string.Empty;
        this.AssetType = (System.Type)null;
        this.Message = string.Empty;
        this.Bundle = (Bundle)null;
        this.OnAssetBundleLoadComplete = (OnAssetBundleLoadComplete)null;
        this.OnAssetLoadComplete = (OnAssetLoadComplete)null;
    }

    public override string ToString()
    {
        if (this.AssetLoadType == AssetLoadType.LoadAsset)
            return string.Format("[AssetData]({0},{1},{2})", (object)this.AssetLoadType.ToString(), (object)this.FileName, (object)this.AssetName);
        return this.AssetLoadType == AssetLoadType.LoadAssetBundle ? string.Format("[AssetData]({0},{1})", (object)this.AssetLoadType.ToString(), (object)this.FileName) : string.Format("[AssetData]({0},{1})", (object)this.AssetLoadType.ToString(), (object)this.FileName);
    }

    public bool IsExistAsset
    {
        get
        {
            if ((UnityEngine.Object)null == (UnityEngine.Object)this.Bundle.Assetbundle)
                return false;
            UnityEngine.Object[] objectArray = this.Bundle.Assetbundle.LoadAllAssets();
            for (int index = 0; index < objectArray.Length; ++index)
            {
                FFDebug.Log((object)this, (object)FFLogType.ResourceLoad, (object)("type:[" + (object)objectArray[index].GetType() + "]name:[" + objectArray[index].name + "]"));
                if (string.Compare(objectArray[index].name, this.AssetName) == 0)
                    return true;
            }
            return false;
        }
    }
}

public delegate void OnAssetBundleLoadComplete(string assetBundleName, bool succeed);
public delegate void OnAssetLoadComplete(string assetBundleName, string assetName, UnityEngine.Object asset);

public enum AssetLoadType
{
    LoadAsset,
    LoadAssetBundle,
    LoadFile
}

public enum AssetState
{
    NonLoad,
    Loading,
    Loaded,
    UnLoaded,
}


public enum CreateAssetBundleType
{
    SyncCreate,
    AsyncCreateFromFile,
    AsyncCreateFromMemory,
    AsyncCreateFromWWW
}