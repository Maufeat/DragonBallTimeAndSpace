using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class LoadHelper
{
    private static StringBuilder getPathResult = new StringBuilder();
    private static Dictionary<string, string> m_PathByFalse = new Dictionary<string, string>();
    private static Dictionary<string, string> m_PathByTrue = new Dictionary<string, string>();


    public static string UpAssetsPath
    {
        get
        {
            return Application.streamingAssetsPath;
        }
    }

    public static string UpPatchPath
    {
        get
        {
            return Application.streamingAssetsPath + "/PatchStore";
        }
    }

    public static string GetPath(string path, bool isweburl = true)
    {
        if (FileLocalVersion.IsOpenLocalVersion())
            path = FileLocalVersion.GetLoaclFile(path);
        if (isweburl)
        {
            if (m_PathByTrue.ContainsKey(path))
                return m_PathByTrue[path];
        }
        else if (m_PathByFalse.ContainsKey(path))
            return m_PathByFalse[path];
        getPathResult.Length = 0;
        getPathResult.Append(Application.dataPath);
        getPathResult.Append("/StreamingAssets/");
        getPathResult.Append(path);
        getPathResult.ToString();
#if !UNITY_EDITOR
        if (isweburl)
            getPathResult.Insert(0, "file:///");
#endif
        string str = getPathResult.ToString();
        if (isweburl)
        {
            if (!m_PathByTrue.ContainsKey(path))
                m_PathByTrue.Add(path, str);
        }
        else if (!m_PathByFalse.ContainsKey(path))
            m_PathByFalse.Add(path, str);
        return str;
    }

}
