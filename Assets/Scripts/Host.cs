using System;
using System.Collections;
using System.IO;
using System.Text;
using Framework.Managers;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Host : MonoBehaviour
{
    private void Awake()
    {
        this.CleanLastVersionDataDone = new Action(this.CleanAfterInit);
        this.CleanLastVersionPersistentData();
    }

    private void CleanAfterInit()
    {
        this.InitFFDebug();
        this.InitBulgy();
        this.InitPatchController();
        if (Host.WithoutPatch)
        {
            LaunchHelp.StoragePath = Application.dataPath + "/StreamingAssets/";
            LaunchHelp.FLEndpoint = "192.168.183.117:1500";
            LaunchHelp.ServerList = new string[10];
            LaunchHelp.ServerList[0] = "1,1内网稳定服";
            LaunchHelp.ServerList[1] = "2,2内网测试服";
            LaunchHelp.ServerList[2] = "3,3内网测试服";
            LaunchHelp.ServerList[3] = "4,李建华测试服";
            LaunchHelp.ServerList[4] = "5,黄乔测试服";
            LaunchHelp.ServerList[5] = "6,张礼服测试服";
            LaunchHelp.ServerList[6] = "7,策划测试服";
            LaunchHelp.ServerList[7] = "8,陶应松测试服";
            LaunchHelp.ServerList[8] = "9,康中伟测试服";
            LaunchHelp.ServerList[9] = "10,RHEL7测试服";
            this.Init();
            base.Invoke("EnterGame", 0.2f);
        }
        else
        {
            new ConfigCopy(this).CopyConfig(delegate
            {
                NetworkReachability internetReachability = Application.internetReachability;
                this.Init();
                this._patchCtrl.SetVersionText(UserInfoStorage.StorageInfo.LastVersion);
                this.CheckNetworkConnect();
            }, false);
        }
    }

    private void CleanLastVersionPersistentData()
    {
        try
        {
            this.CleanLastVersionDataDone();
        }
        catch (Exception ex)
        {
            FFDebug.LogError(this, "CleanLastVersionPersistentData Error: " + ex.ToString());
        }
    }

    private IEnumerator LoadInitFile(string strUrl, Action<WWW> callback)
    {
        WWW www = new WWW(strUrl);
        yield return www;
        if (callback != null)
        {
            callback(www);
        }
        www.Dispose();
        www = null;
        yield break;
    }

    private void LoadStreamingAssetsInitFileDone(WWW www)
    {
        if (string.IsNullOrEmpty(www.error))
        {
            if (!string.IsNullOrEmpty(www.text))
            {
                string[] array = www.text.Split(new char[]
                {
                    ','
                });
                if (array == null || array.Length != 6)
                {
                    FFDebug.LogWarning(this, "StreamingAssets init.dat file content format error!");
                    this.CleanLastVersionDataDone();
                    return;
                }
                string[] array2 = array[4].Split(new char[]
                {
                    '.'
                }, StringSplitOptions.RemoveEmptyEntries);
                if (array2.Length != 4)
                {
                    FFDebug.LogWarning(this, "StreamingAssets init.dat file version format error!");
                    this.CleanLastVersionDataDone();
                    return;
                }
                string path = Application.persistentDataPath + "/Assets/" + this.m_strinitFileName;
                if (File.Exists(path))
                {
                    string[] array3;
                    using (StreamReader streamReader = new StreamReader(path))
                    {
                        array3 = streamReader.ReadLine().Split(new char[]
                        {
                            ','
                        });
                        streamReader.Close();
                    }
                    if (array3 == null || array3.Length != 6)
                    {
                        FFDebug.LogWarning(this, "Persistent init.dat file content format error!");
                        this.CleanLastVersionDataDone();
                        return;
                    }
                    string[] array4 = array3[4].Split(new char[]
                    {
                        '.'
                    }, StringSplitOptions.RemoveEmptyEntries);
                    if (array4.Length != 4)
                    {
                        FFDebug.LogWarning(this, "Persistent init.dat file version format error!");
                        this.CleanLastVersionDataDone();
                        return;
                    }
                    string value = array2[0].Trim();
                    string value2 = array2[1].Trim();
                    string value3 = array2[2].Trim();
                    string value4 = array4[0].Trim();
                    string value5 = array4[1].Trim();
                    string value6 = array4[2].Trim();
                    if (Convert.ToInt32(value) > Convert.ToInt32(value4) || (Convert.ToInt32(value) == Convert.ToInt32(value4) && Convert.ToInt32(value2) > Convert.ToInt32(value5)) || (Convert.ToInt32(value) == Convert.ToInt32(value4) && Convert.ToInt32(value2) == Convert.ToInt32(value5) && Convert.ToInt32(value3) > Convert.ToInt32(value6)))
                    {
                        DirectoryInfo directoryInfo = new DirectoryInfo(Application.persistentDataPath);
                        if (directoryInfo.Exists)
                        {
                            FileInfo[] files = directoryInfo.GetFiles();
                            DirectoryInfo[] directories = directoryInfo.GetDirectories();
                            if (files != null)
                            {
                                for (int i = 0; i < files.Length; i++)
                                {
                                    files[i].Delete();
                                }
                            }
                            if (directories != null)
                            {
                                for (int j = 0; j < directories.Length; j++)
                                {
                                    directories[j].Delete(true);
                                }
                            }
                        }
                    }
                }
                this.CleanLastVersionDataDone();
            }
            else
            {
                this.CleanLastVersionDataDone();
                FFDebug.LogWarning(this, "Init.dat WWW content is null!");
            }
        }
        else
        {
            this.CleanLastVersionDataDone();
            FFDebug.LogWarning(this, "Init.dat WWW is error!");
        }
    }

    public void InitLaunchHelp()
    {
        string url = LaunchHelp.ServerListPath + this.m_platform;
        base.StartCoroutine(this.LoadFrompersister(url, delegate (WWW www)
        {
            if (!string.IsNullOrEmpty(www.error))
            {
                this._patchCtrl.OnGetServerListFailed(new Action(this.ReInitLuanchHelp), new Action(this.QuitGame), new Action(this.QuitGame));
                FFDebug.LogError(this, www.error);
                return;
            }
            LaunchHelp.InitString = www.text.Split(new char[]
            {
                ';'
            }, StringSplitOptions.RemoveEmptyEntries);
            string[] array = LaunchHelp.InitString[1].Split(new char[]
            {
                '$'
            });
            LaunchHelp.FileVersionAssetPath = array[0];
            if (array.Length > 1)
            {
                LaunchHelp.FileVersionAssetPath1 = array[1];
            }
            else
            {
                LaunchHelp.FileVersionAssetPath1 = LaunchHelp.FileVersionAssetPath;
            }
            LaunchHelp.ServerList = LaunchHelp.InitString[9].Split(new char[]
            {
                '$'
            }, StringSplitOptions.RemoveEmptyEntries);
            LaunchHelp.FileVersionConfirmPath = LaunchHelp.InitString[2];
            LaunchHelp.DownloadPage = LaunchHelp.InitString[3];
            LaunchHelp.NoticePage = LaunchHelp.InitString[4];
            LaunchHelp.FLEndpoint = LaunchHelp.InitString[5] + ":" + LaunchHelp.InitString[6];
            int.TryParse(LaunchHelp.InitString[8], out LaunchHelp.RecommendServer);
            this.EnterGame();
        }));
    }

    private IEnumerator LoadFrompersister(string _url, Action<WWW> _callBack)
    {
        WWW www = new WWW(_url);
        yield return www;
        _callBack(www);
        www.Dispose();
        yield break;
    }

    private void EnterGame()
    {
        this.InitLocalVerion();
        this.m_AsyncOperationLoadGameScene = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("Game");
    }

    private string GetServerPath(string strPlatform)
    {
        string text = Resources.Load("LaunchConfig").ToString();
        text = text.Replace("\n", string.Empty);
        string[] array = text.Split(new char[]
        {
            ';'
        }, StringSplitOptions.RemoveEmptyEntries);
        string result = "Error: can not find url! Check LaunchConfig";
        for (int i = 0; i < array.Length; i++)
        {
            string[] array2 = array[i].Split(new char[]
            {
                ','
            }, StringSplitOptions.RemoveEmptyEntries);
            if (string.Compare(array2[0].Trim(), strPlatform.Trim()) == 0)
            {
                result = array2[1];
                break;
            }
        }
        return result;
    }

    private void Init()
    {
        StringBuilder stringBuilder = new StringBuilder();
        QualitySettings.vSyncCount = 0;
        this.m_platform = PlatformType.WINDOWS;
        stringBuilder.Length = 0;
        stringBuilder.Append(Application.dataPath);
        stringBuilder.Append("/");
        LaunchHelp.ServerListPath = this.GetServerPath("win");
        LaunchHelp.StoragePath = stringBuilder.ToString();
        Screen.sleepTimeout = -1;
        if (!Directory.Exists(LoadHelper.UpAssetsPath))
        {
            Directory.CreateDirectory(LoadHelper.UpAssetsPath);
        }
        if (!this.getUserInfoStorage())
        {
            FFDebug.LogError(this, "Get init.dat info error!InitPath：" + LaunchHelp.StoragePath + this.m_strinitFileName);
            return;
        }
        BuglyAgent.SetUserId(UserInfoStorage.StorageInfo.Uid);
        FFDebug.Log(this, FFLogType.Host, UserInfoStorage.StorageInfo.ToString());
        this._patchCtrl.SetVersionText(LaunchHelp.AppVersion);
    }

    private bool getUserInfoStorage()
    {
        string text = LaunchHelp.StoragePath + this.m_strinitFileName;
        UserInfoStorage.StorageInfo = new UserInfoStorage();
        UserInfoStorage.StorageInfo.SetHandle = new Action<bool>(Host.setUserInfoStorage);
        UserInfoStorage userInfoStorage = new UserInfoStorage();
        bool result;
        try
        {
            if (File.Exists(text))
            {
                UserInfoStorage.Read(UserInfoStorage.StorageInfo, LaunchHelp.GetString(text));
                LaunchHelp.Ready = true;
                LaunchHelp.AppVersion = UserInfoStorage.StorageInfo.LastVersion;
                FFDebug.Log(this, FFLogType.Host, "Get User floder init.data complete!" + UserInfoStorage.StorageInfo.ToString());
                if (!string.IsNullOrEmpty(userInfoStorage.LastVersion) && this.CompareVersion(userInfoStorage.LastVersion, UserInfoStorage.StorageInfo.LastVersion) > 0)
                {
                    FFDebug.Log(this, FFLogType.Host, string.Format("version form {0} to {1} update, clear tmp folder!", UserInfoStorage.StorageInfo.LastVersion, userInfoStorage.LastVersion));
                    this.clearPersistentDataPathFolder();
                    UserInfoStorage.StorageInfo.LastFileTick = userInfoStorage.LastFileTick;
                    UserInfoStorage.StorageInfo.LastVersion = userInfoStorage.LastVersion;
                    UserInfoStorage.StorageInfo.SetHandle(true);
                }
                LaunchHelp.AppVersion = UserInfoStorage.StorageInfo.LastVersion;
                LaunchHelp.Ready = true;
            }
            else
            {
                LaunchHelp.Ready = true;
                LaunchHelp.AppVersion = UserInfoStorage.StorageInfo.LastVersion;
                FFDebug.LogWarning(this, "Patch: Not found init.data!" + text);
            }
            result = true;
        }
        catch (Exception ex)
        {
            FFDebug.LogError(this, ex.ToString());
            result = false;
        }
        return result;
    }

    private bool ReadAppInitFile(string filePath, ref UserInfoStorage userInfoStorage)
    {
        bool result;
        try
        {
            if (File.Exists(filePath))
            {
                UserInfoStorage.Read(userInfoStorage, LaunchHelp.ReadTxt(filePath));
                FFDebug.Log(this, FFLogType.Host, "Get AppPackage init.dat sucess!" + userInfoStorage.ToString());
                result = true;
            }
            else
            {
                FFDebug.Log(this, FFLogType.Host, "Get AppPackage init.dat Error!Not found file:[" + filePath + "]");
                result = false;
            }
        }
        catch (Exception ex)
        {
            FFDebug.LogError(this, "Get AppPackage init.dat Error!" + ex.ToString());
            result = false;
        }
        return result;
    }

    private int CompareVersion(string v1, string v2)
    {
        if (v1 == v2)
        {
            return 0;
        }
        string[] array = v1.Split(new char[]
        {
            '.'
        });
        string[] array2 = v2.Split(new char[]
        {
            '.'
        });
        for (int i = 0; i < 4; i++)
        {
            int num = int.Parse(array[i].Trim()) - int.Parse(array2[i].Trim());
            if (num != 0)
            {
                return num;
            }
        }
        return 0;
    }

    private void clearPersistentDataPathFolder()
    {
        string storagePath = LaunchHelp.StoragePath;
        string[] directories = Directory.GetDirectories(storagePath);
        for (int i = 0; i < directories.Length; i++)
        {
            try
            {
                if (Directory.Exists(directories[i]))
                {
                    Directory.Delete(directories[i]);
                }
            }
            catch (Exception ex)
            {
                FFDebug.LogError(this, "delete file fail" + ex.Message.ToString());
            }
        }
    }

    public static void WriteUserInfoStorage(bool isBuglySet = true)
    {
        if (isBuglySet && UserInfoStorage.StorageInfo != null)
        {
            BuglyAgent.SetUserId(UserInfoStorage.StorageInfo.Uid);
        }
        Host.setUserInfoStorage(true);
    }

    private static void setUserInfoStorage(bool b)
    {
        try
        {
            using (FileStream fileStream = new FileStream(LaunchHelp.StoragePath + "init.dat", FileMode.Create))
            {
                using (StreamWriter streamWriter = new StreamWriter(fileStream, Encoding.UTF8))
                {
                    FFDebug.Log("Host", FFLogType.Host, UserInfoStorage.StorageInfo.ToString());
                    streamWriter.Write(UserInfoStorage.Write(UserInfoStorage.StorageInfo));
                    streamWriter.Close();
                }
            }
        }
        catch (Exception ex)
        {
            FFDebug.LogError("Host", ex.ToString());
        }
    }

    private void InitLocalVerion()
    {
        FileLocalVersion.CloseLocalVersion();
        if (FileLocalVersion.IsOpenLocalVersion())
        {
            FileLocalVersion.LoadFileLocalVersion();
        }
    }

    private void InitFFDebug()
    {
        FFDebug.TotalLogLevel = this.LogLevel;
        FFDebug.TotalLogLevel = FFDebug.LogLevel.Error;
        FFDebug.SetLogFile(Application.dataPath.Substring(0, Application.dataPath.LastIndexOf("/")));
        FFDebug.SetLogType(new object[]
        {
            FFLogType.Login,
            FFLogType.CutScene
        });
    }

    private void InitBulgy()
    {
        BuglyAgent.ConfigDebugMode(false);
        BuglyAgent.EnableExceptionHandler();
        BuglyAgent.ConfigAutoReportLogLevel(LogSeverity.LogError);
    }

    private static void FFDebugCallBackHandler(FFDebug.LogLevel logLevel, string name, string message, string tarceStack)
    {
    }

    private string GetInitPath
    {
        get
        {
            return LaunchHelp.GetStreamingAssetsPath + this.m_strinitFileName;
        }
    }

    private void InitPatchController()
    {
        this._patchCtrl = ControllerManager.Instance.AddcontrollerByType<PatchController>();
        this._patchCtrl.Awake();
        this._patchCtrl.InitPatchUI();
    }

    private void CheckNetworkConnect()
    {
        NetworkReachability internetReachability = Application.internetReachability;
        FFDebug.Log(this, FFLogType.Login, "CheckNetworkConnect: current NetworkReachability = " + internetReachability);
        if (internetReachability == NetworkReachability.NotReachable)
        {
            this._patchCtrl.OnBadNetWork(new Action(this.TryReconnect), new Action(this.QuitGame), new Action(this.QuitGame));
        }
        else
        {
            this.InitLaunchHelp();
        }
    }

    private void ReInitLuanchHelp()
    {
        base.Invoke("InitLaunchHelp", 1f);
    }

    private void TryReconnect()
    {
        FFDebug.Log(this, FFLogType.Login, "TryReconnect...");
        base.Invoke("CheckNetworkConnect", 1f);
    }

    private void QuitGame()
    {
        Application.Quit();
    }

    public static bool WithoutPatch = true;

    private PlatformType m_platform;

    private readonly string m_strinitFileName = "init.dat";

    private string message = string.Empty;

    public bool DisableAllLog;

    public FFDebug.LogLevel LogLevel = FFDebug.LogLevel.debug;

    private AsyncOperation m_AsyncOperationLoadGameScene;

    private Action CleanLastVersionDataDone;

    private PatchController _patchCtrl;
}
