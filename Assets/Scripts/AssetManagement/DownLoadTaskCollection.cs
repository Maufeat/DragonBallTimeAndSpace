using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class DownLoadTaskCollection
{
    public BetterDictionary<string, AssetBundleRequest> MapAssetBundleRequest = new BetterDictionary<string, AssetBundleRequest>();
    public DownloadTaskState DownloadTaskState = DownloadTaskState.Error;
    private List<AssetData> _lstTask;
    public bool NeedDelete;

    public string AssetBundlePath { get; set; }

    public string FilePath
    {
        get
        {
            return ResourceHelper.GetPath(this.AssetBundlePath);
        }
    }

    public AssetBundleCreateRequest AssetBundleCreateRequest { get; set; }

    public List<AssetData> DownLoadTaskList
    {
        get
        {
            if (this._lstTask == null)
                this._lstTask = new List<AssetData>();
            return this._lstTask;
        }
    }

    public override string ToString()
    {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append("AssetBundle path:[" + this.AssetBundlePath + "]");
        for (int index = 0; index < this.DownLoadTaskList.Count; ++index)
            stringBuilder.Append(this.DownLoadTaskList[index].Message);
        return stringBuilder.ToString();
    }

    public void Clear()
    {
        this.AssetBundlePath = string.Empty;
        if (this.AssetBundleCreateRequest != null)
            this.AssetBundleCreateRequest = (AssetBundleCreateRequest)null;
        this.MapAssetBundleRequest.Clear();
        this.DownloadTaskState = DownloadTaskState.Error;
        for (int index = 0; index < this._lstTask.Count; ++index)
        {
            if (this._lstTask[index] != null)
                this._lstTask[index].Rest();
        }
        this._lstTask.Clear();
        this.NeedDelete = false;
    }

    public void GoDelete()
    {
        this.NeedDelete = true;
    }
}
