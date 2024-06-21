using System;
using System.Collections;
using System.IO;
using System.Text;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
using UnityEngine;

public class CoreLoadHelp
{
    public static LoadItem LoadBytes(MonoBehaviour mo, string path, Action<byte[]> callback)
    {
        LoadItem loadItem = new LoadItem(path);
        CoreLoadHelp.AddLog("LoadBytes:" + path);
        mo.StartCoroutine(CoreLoadHelp.loadBytes(loadItem, callback));
        return loadItem;
    }

    private static IEnumerator loadBytes(LoadItem item, Action<byte[]> callback)
    {
        yield return 0;
        item.Req = new WWW(item.Path);
        float lastProcess = 0f;
        float duration = 0f;
        float lastTime = Time.realtimeSinceStartup;
        while (!item.Req.isDone)
        {
            yield return new WaitForSeconds(0.01f);
            if (lastProcess == item.Req.progress)
            {
                duration += Time.realtimeSinceStartup - lastTime;
            }
            else
            {
                duration = 0f;
                lastProcess = item.Req.progress;
            }
            if (duration > 3f)
            {
                break;
            }
            lastTime = Time.realtimeSinceStartup;
        }
        yield return new WaitForEndOfFrame();
        if (duration < 3f && string.IsNullOrEmpty(item.Req.error))
        {
            callback(item.Req.bytes);
        }
        else
        {
            callback(null);
            CoreLoadHelp.AddLog("LoadBytes fail:" + item.Path + ", error:" + item.Req.error);
        }
        item.Req.Dispose();
        yield break;
    }

    public static void LoadText(MonoBehaviour mo, string path, Action<string> callback)
    {
        mo.StartCoroutine(CoreLoadHelp.loadText(path, callback));
    }

    private static IEnumerator loadText(string path, Action<string> callback)
    {
        yield return 0;
        WWW request = new WWW(path);
        float lastProcess = 0f;
        float duration = 0f;
        float lastTime = Time.realtimeSinceStartup;
        while (!request.isDone)
        {
            yield return new WaitForSeconds(0.2f);
            if (lastProcess == request.progress)
            {
                duration += Time.realtimeSinceStartup - lastTime;
            }
            else
            {
                duration = 0f;
                lastProcess = request.progress;
            }
            if (duration > 3f)
            {
                break;
            }
            lastTime = Time.realtimeSinceStartup;
        }
        yield return new WaitForEndOfFrame();
        if (duration < 3f && string.IsNullOrEmpty(request.error))
        {
            callback(request.text);
        }
        else
        {
            callback(string.Empty);
            CoreLoadHelp.AddLog(path + ":" + request.error);
        }
        request.Dispose();
        yield break;
    }

    public static void AddLog(string log)
    {
        CoreLoadHelp.logLineCount += 1;
        if (CoreLoadHelp.logLineCount > 30)
        {
            CoreLoadHelp.logLineCount = 0;
            CoreLoadHelp.logSb.Length = 0;
        }
        CoreLoadHelp.logSb.AppendLine(log);
        Debug.Log(log);
        CoreLoadHelp.LogStr = CoreLoadHelp.logSb.ToString();
    }

    public static void Init(MonoBehaviour _mono)
    {
        CoreLoadHelp.mono = _mono;
    }

    private static byte[] compress(byte[] data)
    {
        byte[] array = null;
        using (MemoryStream memoryStream = new MemoryStream(CoreLoadHelp.buffer))
        {
            using (DeflaterOutputStream deflaterOutputStream = new DeflaterOutputStream(memoryStream))
            {
                deflaterOutputStream.Write(data, 0, data.Length);
                deflaterOutputStream.Finish();
                CoreLoadHelp.length = (int)memoryStream.Position;
                array = new byte[CoreLoadHelp.length];
                for (int i = 0; i < CoreLoadHelp.length; i++)
                {
                    array[i] = CoreLoadHelp.buffer[i];
                }
            }
        }
        return array;
    }

    private static byte[] deCompress(byte[] data)
    {
        using (MemoryStream memoryStream = new MemoryStream(data, 0, data.Length))
        {
            memoryStream.Seek(0L, SeekOrigin.Begin);
            using (InflaterInputStream inflaterInputStream = new InflaterInputStream(memoryStream))
            {
                CoreLoadHelp.length = inflaterInputStream.Read(CoreLoadHelp.buffer, 0, CoreLoadHelp.buffer.Length);
            }
        }
        byte[] array = new byte[CoreLoadHelp.length];
        for (int i = 0; i < CoreLoadHelp.length; i++)
        {
            array[i] = CoreLoadHelp.buffer[i];
        }
        return array;
    }

    public static uint ReadUInt32(byte[] data)
    {
        return (uint)((int)data[0] + ((int)data[1] << 8) + ((int)data[2] << 16) + ((int)data[3] << 24));
    }

    public static string AppVersion = string.Empty;

    public static string OpenGLVersion = string.Empty;

    public static string NotificationID = string.Empty;

    public static string DownloadPage = string.Empty;

    public static string[] InitString = null;

    public static Action<float> ChangeProcess;

    public static Texture2D texLoginBack;

    public static GUIStyle styPlanBack;

    public static Texture2D texLogo;

    public static GUIStyle styCurPlan;

    public static GUIStyle styCurPlanPoint;

    public static GUIStyle styAskBack;

    public static GUIStyle btnAskCancel;

    public static GUIStyle btnAskSure;

    public static GUIStyle btnAskClose;

    public static GUIStyle styAskLoding;

    public static GUIStyle styAskUnderline;

    public static GUIStyle styAskInsideBack;

    public static GUIStyle styAskCheckBox;

    public static Texture texAskCheckBox;

    public static Action<byte> ExitHandle;

    public static bool Ready = false;

    private static byte logLineCount = 0;

    private static StringBuilder logSb = new StringBuilder();

    public static string StoragePath = string.Empty;

    public static string FileVersionConfirmPath = string.Empty;

    public static string FileVersionAssetPath = string.Empty;

    public static string FileVersionAssetPath1 = string.Empty;

    public static string NoticePage = string.Empty;

    public static string ServerListPath = string.Empty;

    public static string LogStr = string.Empty;

    private static MonoBehaviour mono;

    private static byte[] buffer = null;

    private static int length = 0;
}
