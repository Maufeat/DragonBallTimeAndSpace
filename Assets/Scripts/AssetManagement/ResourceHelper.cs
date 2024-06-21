using System.IO;
using System.Text;
using UnityEngine;

public class ResourceHelper
{
    private static string tmpPath = string.Empty;
    private static StringBuilder getPathResult = new StringBuilder();
    public static CreateAssetBundleType createAssetBundleType = CreateAssetBundleType.AsyncCreateFromFile;
    public static readonly int MaxDownLoadAssetBundleCount = 10;

    public static string GetPath(string path)
    {
        if (FileLocalVersion.IsOpenLocalVersion())
            path = FileLocalVersion.GetLoaclFile(path);
        ResourceHelper.getPathResult.Length = 0;
        ResourceHelper.getPathResult.Append(Application.dataPath);
        ResourceHelper.getPathResult.Append("/StreamingAssets/");
        ResourceHelper.getPathResult.Append(path);
        ResourceHelper.tmpPath = ResourceHelper.getPathResult.ToString();
        return File.Exists(ResourceHelper.tmpPath) ? ResourceHelper.tmpPath : string.Empty;
    }
}
