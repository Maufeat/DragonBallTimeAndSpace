using System;
using System.IO;
using System.Text;
using UnityEngine;

public class LaunchHelp
{
    private static void createFolder(ref string tPath, string name)
    {
        tPath = CoreLoadHelp.StoragePath + name;
        if (!Directory.Exists(tPath))
        {
            Directory.CreateDirectory(tPath);
        }
    }

    public static string GetString(string path)
    {
        string result;
        using (FileStream fileStream = new FileStream(path, FileMode.Open))
        {
            using (StreamReader streamReader = new StreamReader(fileStream, Encoding.UTF8))
            {
                result = streamReader.ReadToEnd();
            }
        }
        return result;
    }

    public static string ReadTxt(string path)
    {
        string result;
        try
        {
            FileInfo fileInfo = new FileInfo(path);
            StreamReader streamReader = fileInfo.OpenText();
            result = streamReader.ReadToEnd();
        }
        catch (Exception ex)
        {
            FFDebug.LogError("LaunchHelp", string.Concat(new string[]
            {
                "Read txt file error:path[",
                path,
                "]Exception:[",
                ex.ToString(),
                "]"
            }));
            result = null;
        }
        return result;
    }

    public static void ServerInfoUpdateToLocal()
    {
        if (LaunchHelp.InitString != null)
        {
            UserInfoStorage.StorageInfo.LastVersion = LaunchHelp.InitString[0];
            UserInfoStorage.Write(UserInfoStorage.StorageInfo);
        }
    }

    public static string GetPlatformPath
    {
        get
        {
            return string.Concat(new string[]
            {
                Application.dataPath
            });
        }
    }

    public static string GetStreamingAssetsPath
    {
        get
        {
            return Application.dataPath;
        }
    }

    public static string StoragePath = string.Empty;

    public static string FileVersionConfirmPath = string.Empty;

    public static string FileVersionAssetPath = string.Empty;

    public static string FileVersionAssetPath1 = string.Empty;

    public static string NoticePage = string.Empty;

    public static string ServerListPath = string.Empty;

    public static string LogStr = string.Empty;

    public static bool Ready = false;

    public static string AppVersion = string.Empty;

    public static string OpenGLVersion = string.Empty;

    public static string NotificationID = string.Empty;

    public static string DownloadPage = string.Empty;

    public static string FLEndpoint = string.Empty;

    public static int RecommendServer = 0;

    public static string[] InitString = null;

    public static string[] ServerList = null;

    public static UserInfoStorage StorageInfo;
}
