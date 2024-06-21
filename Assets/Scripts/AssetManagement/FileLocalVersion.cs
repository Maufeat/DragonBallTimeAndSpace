using System;
using System.IO;
using System.Text;
using UnityEngine;

public class FileLocalVersion
{
    public static bool LoadFileLocalVersion()
    {
        bool result;
        try
        {
            string localVersionFilePath = FileLocalVersion.GetLocalVersionFilePath();
            string text = string.Empty;
            if (File.Exists(localVersionFilePath))
            {
                text = LaunchHelp.ReadTxt(localVersionFilePath);
                if (string.IsNullOrEmpty(text))
                {
                    FFDebug.LogWarning("FileLocalVersion", "File content is null!  " + localVersionFilePath);
                    result = false;
                }
                else
                {
                    FileLocalVersion.DecryptFileVersionData(text);
                    result = true;
                }
            }
            else
            {
                FFDebug.LogWarning("FileLocalVersion", "File is not exist!  " + localVersionFilePath);
                result = false;
            }
        }
        catch (Exception ex)
        {
            FFDebug.LogError("FileLocalVersion", ex.ToString());
            result = false;
        }
        return result;
    }

    public static void OpenLocalVersion()
    {
        FileLocalVersion._bUseLocalVersion = true;
    }

    public static void CloseLocalVersion()
    {
        FileLocalVersion._bUseLocalVersion = false;
        FileLocalVersion._bdicFileVersion.Clear();
    }

    public static bool IsOpenLocalVersion()
    {
        return FileLocalVersion._bUseLocalVersion;
    }

    public static string GetLoaclFile(string strPath)
    {
        string empty = string.Empty;
        if (FileLocalVersion._bdicFileVersion.TryGetValue(strPath, out empty))
        {
            return empty;
        }
        FFDebug.LogWarning("FileLocalVersion", "Can not find local file data, and return original path!  " + strPath);
        return strPath;
    }

    private static void DecryptFileVersionData(string strData)
    {
        if (string.IsNullOrEmpty(strData))
        {
            FFDebug.LogWarning("FileLocalVersion", "File content is null! ");
            return;
        }
        string[] array = strData.Split(new char[]
        {
            '|'
        });
        FileLocalVersion._bdicFileVersion.Clear();
        if (array != null && 1 < array.Length)
        {
            FFDebug.Log("FileLocalVersion", FFLogType.Config, array[0]);
            int num = array[0].LastIndexOf(FileLocalVersion._strFileCount);
            string text = array[0].Substring(num + FileLocalVersion._strFileCount.Length);
            FFDebug.Log("FileLocalVersion", FFLogType.Config, "File Count: " + text);
            int num2 = array.Length - 1;
            if (num2 == Convert.ToInt32(text))
            {
                for (int i = 1; i <= num2; i++)
                {
                    string[] array2 = array[i].Split(new char[]
                    {
                        '^'
                    });
                    if (array2 == null || array2.Length != 2)
                    {
                        FileLocalVersion._bdicFileVersion.Clear();
                        FFDebug.LogWarning("FileLocalVersion", "Decrypt File KeyValue Error!");
                        return;
                    }
                    FileLocalVersion._bdicFileVersion.Add(array2[0], array2[1]);
                }
            }
            else
            {
                FFDebug.LogWarning("FileLocalVersion", "Decrypt File Count Error!");
            }
        }
        else
        {
            FFDebug.LogError("FileLocalVersion", "Decrypt File Error!");
        }
    }

    private static string GetLocalVersionFilePath()
    {
        StringBuilder stringBuilder = new StringBuilder();
        string text = string.Empty;
        stringBuilder.Append(Application.dataPath);
        stringBuilder.Append("/StreamingAssets/");
        stringBuilder.Append(FileLocalVersion._strLocalVersionFileName);
        text = stringBuilder.ToString();
        if (File.Exists(text))
        {
            return text;
        }
        return text;
    }

    private static BetterDictionary<string, string> _bdicFileVersion = new BetterDictionary<string, string>();

    private static string _strLocalVersionFileName = "dbfilelist.dat";

    private static string _strFileCount = "FileCount:";

    private static bool _bUseLocalVersion = false;
}
