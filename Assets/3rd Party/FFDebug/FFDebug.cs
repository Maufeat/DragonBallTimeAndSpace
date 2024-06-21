using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class FFDebug
{
    public delegate void VoidDelegate();

    public delegate void LogCallBackDelegate(LogLevel level, string name, string info, string tarceStack);

    public enum LogLevel
    {
        debug = 1,
        Warning,
        Error,
        Exception,
        Disable
    }

    public static VoidDelegate OnOpenUICallBack;

    public static LogCallBackDelegate OnLogCallBack;

    public static bool AllDisable = false;

    public static bool ShowGUIlogButton = false;

    public static LogLevel TotalLogLevel = LogLevel.debug;

    private static bool _showGuilogWindow = false;

    private static float _heightAmount = 0.6f;

    private static int _guiMaxCount = 50;

    private static string _logContent = string.Empty;

    private static List<string> _guiLoglist = new List<string>();

    private static StringBuilder _logSB = new StringBuilder();

    private static StringBuilder _guiSB = new StringBuilder();

    public static int sbMaxLeng = 15000;

    private static Vector2 _srcollPos = Vector2.zero;

    private static Dictionary<int, bool> _logTypeDic = new Dictionary<int, bool>();

    private static float _runningTime = 0f;

    private static bool _openOtherUI = false;

    private static string _logPath = string.Empty;

    private static object m_lock = new object();

    public static Action<object, object> OnResourceError;

    private static string CurrentTime
    {
        get
        {
            if (AllDisable)
            {
                return string.Empty;
            }

            return DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
        }
    }

    public static void SetLogFile(string file)
    {
        _logPath = file + "/logs/";
    }

    public static void SetLogType(params object[] args)
    {
        _logTypeDic.Clear();
        if (args == null || args.Length < 1)
        {
            return;
        }

        for (int i = 0; i < args.Length; i++)
        {
            int key = (int)args[i];
            if (!_logTypeDic.ContainsKey(key))
            {
                _logTypeDic.Add(key, value: true);
            }
        }
    }

    public static bool CanLog(int type)
    {
        if (AllDisable)
        {
            return false;
        }

        if (IsAllLogDisable(LogLevel.debug))
        {
            return false;
        }

        return IsOpen(type);
    }

    public static void Log(object sourceObj, int type, object str)
    {
        if (str == null)
        {
            return;
        }

        try
        {
            if (CanLog(type))
            {
                _logSB.Length = 0;
                _logSB.Append(" Log ------ Soure:");
                _logSB.Append(GetSourceName(sourceObj));
                _logSB.Append("; Type:");
                _logSB.Append(type);
                _logSB.Append("; Info:");
                _logSB.Append(str.ToString());
                _logContent = _logSB.ToString();
                Debug.Log(_logContent);
                GuiLog(_logContent);
                SaveLog(CurrentTime.ToString() + _logContent);
                if (OnLogCallBack != null)
                {
                    OnLogCallBack(LogLevel.debug, "FFDebugLog", _logContent, string.Empty);
                }
            }
        }
        catch (Exception ex)
        {
            LogError("FFDebug", ex.ToString());
        }
    }

    public static void Log(object sourceObj, object type, object str)
    {
        if (str != null)
        {
            Log(sourceObj, (int)type, str);
        }
    }

    public static bool CanWarning()
    {
        if (AllDisable)
        {
            return false;
        }

        if (IsAllLogDisable(LogLevel.Warning))
        {
            return false;
        }

        return true;
    }

    public static void LogWarning(object sourceObj, object str)
    {
        if (!CanWarning() || str == null)
        {
            return;
        }

        try
        {
            _logSB.Length = 0;
            _logSB.Append(" Warn ------ Soure:");
            _logSB.Append(GetSourceName(sourceObj));
            _logSB.Append("; Info:");
            _logSB.Append(str.ToString());
            _logContent = _logSB.ToString();
            Debug.LogWarning(_logContent);
            GuiLog(_logContent);
            SaveLog(CurrentTime.ToString() + _logContent);
            if (OnLogCallBack != null)
            {
                OnLogCallBack(LogLevel.Warning, "FFDebugWarning", _logContent, string.Empty);
            }
        }
        catch (Exception ex)
        {
            LogError("FFDebug", ex.ToString());
        }
    }

    public static bool CanError()
    {
        if (AllDisable)
        {
            return false;
        }

        if (IsAllLogDisable(LogLevel.Error))
        {
            return false;
        }

        return true;
    }

    public static void LogError(object sourceObj, object str)
    {
        if (!CanError() || str == null)
        {
            return;
        }

        try
        {
            _logSB.Length = 0;
            _logSB.Append(" Error ------ Soure:");
            _logSB.Append(GetSourceName(sourceObj));
            _logSB.Append("; Info:");
            _logSB.Append(str.ToString());
            string text = _logSB.ToString();
            Debug.LogError(text);
            GuiLog(text);
            SaveLog(CurrentTime.ToString() + _logContent);
            if (OnLogCallBack != null)
            {
                OnLogCallBack(LogLevel.Error, "FFDebugError", text, string.Empty);
            }
        }
        catch (Exception ex)
        {
            LogError("FFDebug", ex.ToString());
        }
    }

    public static void LogResourceError(object sourceObj, object str)
    {
        if (Application.isEditor)
        {
            LogWarning(sourceObj, str);
        }

        try
        {
            if (OnResourceError != null)
            {
                OnResourceError(sourceObj, str);
            }
        }
        catch (Exception)
        {
        }
    }

    private static bool IsAllLogDisable(LogLevel Level)
    {
        return TotalLogLevel > Level;
    }

    public static void CatchExceptionHandler(string message, string traceStack, LogType type)
    {
        if (!AllDisable && type != LogType.Log && type != LogType.Warning)
        {
            _logSB.Length = 0;
            _logSB.AppendFormat(" {0}", type.ToString());
            _logSB.Append(" ------ Source:Unity; Info:");
            _logSB.Append(message);
            _logSB.Append(traceStack);
            _logContent = _logSB.ToString();
            if (TotalLogLevel <= LogLevel.Exception)
            {
                SaveLog(CurrentTime.ToString() + _logContent);
            }
        }
    }

    public static void DrawGuiConsole()
    {
        if (_showGuilogWindow)
        {
            GUI.Window(1, new Rect(0f, 0f, Screen.width, (float)Screen.height * _heightAmount), LogView, "");
        }

        if (_openOtherUI && GUI.Button(new Rect((float)Screen.width / 2f - 100f, (float)Screen.height - 100f, 200f, 50f), " 金手指 ") && OnOpenUICallBack != null)
        {
            OnOpenUICallBack();
            _openOtherUI = false;
        }

        if (ShowGUIlogButton && GUI.Button(new Rect((float)Screen.width / 2f - 350f, (float)Screen.height * _heightAmount - 50f, 200f, 50f), "显示GUI日志"))
        {
            _showGuilogWindow = true;
        }

        if (Input.touchCount >= 5 || Input.GetKey(KeyCode.L))
        {
            if (_runningTime < 3f)
            {
                _runningTime += Time.deltaTime;
                return;
            }

            _openOtherUI = true;
            _showGuilogWindow = true;
            _runningTime = 3f;
        }
        else
        {
            _runningTime = 0f;
        }
    }

    private static void GuiLog(string logstr)
    {
        _guiLoglist.Add(logstr);
        if (_guiLoglist.Count > _guiMaxCount)
        {
            _guiLoglist.RemoveAt(0);
        }
    }

    private static void LogView(int id)
    {
        GUI.contentColor = Color.red;
        float height = Mathf.Max(Screen.height, (float)(_guiLoglist.Count + 1) * 25f);
        _srcollPos = GUI.BeginScrollView(new Rect(0f, 0f, Screen.width, (float)Screen.height * _heightAmount - 50f), _srcollPos, new Rect(10f, 0f, (float)Screen.width * 0.9f, height));
        float height2 = Mathf.Max(Screen.height, (float)(_guiLoglist.Count + 1) * 25f);
        GUI.Label(new Rect(20f, 0f, (float)Screen.width * 0.9f, height2), GetLogString());
        GUI.EndScrollView();
        if (GUI.Button(new Rect((float)(Screen.width / 2) - 100f, (float)Screen.height * _heightAmount - 50f, 200f, 50f), "关闭GUI日志"))
        {
            _showGuilogWindow = false;
        }

        if (GUI.Button(new Rect((float)Screen.width / 2f + 150f, (float)Screen.height * _heightAmount - 50f, 200f, 50f), "清除GUI日志"))
        {
            _srcollPos = Vector2.zero;
            _guiLoglist.Clear();
        }
    }

    private static void SaveLog(string text)
    {
        if (string.IsNullOrEmpty(_logPath))
        {
            return;
        }

        lock (m_lock)
        {
            if (!Directory.Exists(_logPath))
            {
                Directory.CreateDirectory(_logPath);
            }

            string path = _logPath + "cfg.txt";
            string empty = string.Empty;
            if (File.Exists(path))
            {
                empty = File.ReadAllText(path);
                if (File.Exists(empty))
                {
                    if (new FileInfo(empty).Length >= 104857600)
                    {
                        empty = _logPath + DateTime.Now.ToString("yyyyMMddhhmmss") + ".txt";
                        File.WriteAllText(path, empty);
                    }
                }
                else
                {
                    empty = _logPath + DateTime.Now.ToString("yyyyMMddhhmmss") + ".txt";
                    File.WriteAllText(path, empty);
                }
            }
            else
            {
                empty = _logPath + DateTime.Now.ToString("yyyyMMddhhmmss") + ".txt";
                File.WriteAllText(path, empty);
            }

            using (FileStream stream = new FileStream(empty, FileMode.Append))
            {
                using (StreamWriter streamWriter = new StreamWriter(stream))
                {
                    streamWriter.AutoFlush = true;
                    streamWriter.WriteLine(text);
                }
            }
        }
    }

    private static string GetLogString()
    {
        if (_guiLoglist == null || _guiLoglist.Count < 1)
        {
            return string.Empty;
        }

        _guiSB.Length = 0;
        for (int i = 0; i < _guiLoglist.Count; i++)
        {
            _guiSB.AppendLine(_guiLoglist[i]);
        }

        if (_guiSB.Length >= sbMaxLeng)
        {
            _guiSB.Remove(0, _guiSB.Length - sbMaxLeng);
        }

        return _guiSB.ToString();
    }

    private static string GetSourceName(object sourceObj)
    {
        string empty = string.Empty;
        empty = ((sourceObj != null) ? sourceObj.ToString() : "null");
        if (string.IsNullOrEmpty(empty))
        {
            empty = "unknow";
        }

        return empty;
    }

    private static bool IsOpen(int type)
    {
        bool result = false;
        if (AllDisable)
        {
            return result;
        }

        if (_logTypeDic == null || _logTypeDic.Count < 1)
        {
            return result;
        }

        if (_logTypeDic.ContainsKey(type))
        {
            result = true;
        }

        return result;
    }
}

public enum FFLogType
{
    Default,
    AssetBundleLoad,
    Patch,
    UI,
    Table,
    Network,
    Lua,
    Battle,
    Effect,
    Skill,
    Camera,
    Avatar,
    Login,
    Player,
    Task,
    Config,
    Scene,
    HoldOn,
    FollowTarget,
    Pathfind,
    Npc,
    TargetSelect,
    Buff,
    HpSystem,
    Copy,
    Host,
    Equip,
    Tips,
    ResourceLoad,
    TopButton,
    PrivateChat,
    Bag,
    Storeage,
    Action,
    Team,
    CutScene,
    MapTour,
}
