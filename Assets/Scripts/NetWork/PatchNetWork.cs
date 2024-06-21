using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Cache;
using System.Threading;
using System.Xml;
using Engine;
using Framework.Managers;
using ICSharpCode.SharpZipLib.Zip;
using UnityEngine;

public class PatchNetWork
{
    public PatchNetWork()
    {
        this.patchController = ControllerManager.Instance.GetController<PatchController>();
    }

    public void CheckVersionXml()
    {
        string strUrl = LaunchHelp.FileVersionAssetPath + "Version/" + UserInfoStorage.StorageInfo.LastServer.ToString() + "/version.xml";
        byte[] array = this.HttpDownloadFile(strUrl);
        if (array == null)
        {
            return;
        }
        XmlDocument xmlDocument = new XmlDocument();
        xmlDocument.Load(new MemoryStream(array));
        XmlNode xmlNode = xmlDocument.SelectSingleNode("seal");
        this.lastVersion = xmlNode.Attributes["last_app_version"].InnerText;
        XmlNode xmlNode2 = xmlNode.SelectSingleNode("map").SelectSingleNode("patchs");
        int num = int.Parse(xmlNode2.Attributes["num"].InnerText);
        string innerText = xmlNode2.Attributes["ver"].InnerText;
        XmlNodeList patchs = xmlNode2.SelectSingleNode("map").SelectNodes("patch");
        if (this.CheckVersion(this.lastVersion, UserInfoStorage.StorageInfo.LastVersion))
        {
            this.patchController.VersionBack(this.lastVersion, delegate
            {
                this.VersionRollBack(patchs);
            });
            return;
        }
        long num2 = 0L;
        List<XmlNode> upList = new List<XmlNode>();
        for (int i = 0; i < patchs.Count; i++)
        {
            XmlNode xmlNode3 = patchs[i];
            if (this.CheckVersion(UserInfoStorage.StorageInfo.LastVersion, xmlNode3.Attributes["name"].InnerText))
            {
                num2 += long.Parse(xmlNode3.Attributes["size"].InnerText);
                upList.Add(xmlNode3);
            }
        }
        if (upList.Count > 0)
        {
            this.patchController.UpPatchInfo(this.lastVersion, upList.Count, num2, delegate
            {
                this.UpdatePatch(upList);
            });
        }
        else
        {
            Scheduler.Instance.AddQueue(this.patchController.OnCallBack);
        }
    }

    public void VersionRollBack(XmlNodeList patchs)
    {
        this.DeleteUpAssets(delegate
        {
            string url = "file://" + Application.streamingAssetsPath + "/init.dat";
            MonobehaviourManager.Instance.StartCoroutine(this.LoadFrompersister(url, delegate (WWW www)
            {
                string[] strs = www.text.Split(new char[]
                {
                    ','
                });
                ConfigCopy configCopy = new ConfigCopy(MonobehaviourManager.Instance);
                configCopy.CopyConfig(delegate
                {
                    UserInfoStorage.StorageInfo.LastVersion = strs[4];
                    List<XmlNode> list = new List<XmlNode>();
                    for (int i = 0; i < patchs.Count; i++)
                    {
                        XmlNode xmlNode = patchs[i];
                        if (this.CheckVersion(UserInfoStorage.StorageInfo.LastVersion, xmlNode.Attributes["name"].InnerText))
                        {
                            list.Add(xmlNode);
                        }
                    }
                    if (list.Count > 0)
                    {
                        this.UpdatePatch(list);
                    }
                    else
                    {
                        Scheduler.Instance.AddQueue(new Action(this.patchController.PatchFinish));
                    }
                }, true);
            }));
        });
    }

    public void DeleteUpAssets(Action callback)
    {
        string upAsstesPath = LoadHelper.UpAssetsPath;
        Thread thread = new Thread(delegate ()
        {
            Directory.Delete(upAsstesPath, true);
            Scheduler.Instance.AddQueue(callback);
        });
        thread.Start();
    }

    public void UpdatePatch(List<XmlNode> _upList)
    {
        string[] array = this.lastVersion.Split(new char[]
        {
            '.'
        });
        string url = string.Concat(new string[]
        {
            LaunchHelp.FileVersionAssetPath,
            "Patch/",
            array[0],
            ".",
            array[1],
            ".",
            array[2],
            "/"
        });
        if (!Directory.Exists(LoadHelper.UpPatchPath))
        {
            Directory.CreateDirectory(LoadHelper.UpPatchPath);
        }
        if (!Directory.Exists(LoadHelper.UpAssetsPath))
        {
            Directory.CreateDirectory(LoadHelper.UpAssetsPath);
        }
        int i = 0;
        this.OverrideAssets(url, LoadHelper.UpPatchPath, LoadHelper.UpAssetsPath, _upList, i);
    }

    public void OverrideAssets(string url, string upPatchPath, string upAssetsPath, List<XmlNode> _upList, int i)
    {
        this.patchController.OnPatchNum(i + 1, _upList.Count);
        this.versionPath = _upList[i].Attributes["name"].InnerText;
        string url2 = url + this.versionPath + ".zip";
        string writePath = upPatchPath + "/" + _upList[i].Attributes["name"].InnerText + ".zip";
        if (File.Exists(writePath))
        {
            this.ThreadUnZip(url, upPatchPath, upAssetsPath, _upList, i, writePath);
        }
        else
        {
            Scheduler.Instance.AddUpdator(new Scheduler.OnScheduler(this.OnInDownload));
            MonobehaviourManager.Instance.StartCoroutine(this.LoadFrompersister(url2, delegate (WWW www)
            {
                File.WriteAllBytes(writePath, www.bytes);
                Scheduler.Instance.RemoveUpdator(new Scheduler.OnScheduler(this.OnInDownload));
                this.ThreadUnZip(url, upPatchPath, upAssetsPath, _upList, i, writePath);
            }));
        }
    }

    public void ThreadUnZip(string url, string upPatchPath, string upAssetsPath, List<XmlNode> _upList, int i, string writePath)
    {
        Thread thread = new Thread(delegate ()
        {
            this.UnZipFiles(writePath, upAssetsPath + "/", null, delegate ()
            {
                UserInfoStorage.StorageInfo.LastVersion = this.versionPath;
                Host.WriteUserInfoStorage(false);
                Scheduler.Instance.AddQueue(delegate
                {
                    this.patchController.OnVersionChange(UserInfoStorage.StorageInfo.LastVersion);
                });
                Scheduler.Instance.RemoveUpdator(new Scheduler.OnScheduler(this.UzipProgress));
                i++;
                if (i < _upList.Count)
                {
                    Scheduler.Instance.AddQueue(delegate
                    {
                        this.OverrideAssets(url, upPatchPath, upAssetsPath, _upList, i);
                    });
                }
                else
                {
                    Scheduler.Instance.AddQueue(new Action(this.patchController.PatchFinish));
                }
            });
        });
        Scheduler.Instance.AddUpdator(new Scheduler.OnScheduler(this.UzipProgress));
        thread.Start();
    }

    public void UzipProgress()
    {
        this.patchController.OnInUzip((float)this.writeSize / (float)this.zipSize, this.versionPath);
    }

    public void OnInDownload()
    {
        if (this.www != null)
        {
            this.patchController.OnInDownload(0L, 0L, this.www.progress, this.versionPath);
        }
    }

    private IEnumerator LoadFrompersister(string _url, Action<WWW> _callBack)
    {
        this.www = new WWW(_url);
        yield return this.www;
        _callBack(this.www);
        this.www.Dispose();
        this.www = null;
        yield break;
    }

    public bool CheckVersion(string version1, string version2)
    {
        string[] array = version1.Split(new char[]
        {
            '.'
        });
        string[] array2 = version2.Split(new char[]
        {
            '.'
        });
        if (array.Length != 4 || array2.Length != 4)
        {
            FFDebug.LogWarning(this, "Version data's format is error! Ver1:" + version1 + " Ver2: " + version2);
            return false;
        }
        string s = string.Empty;
        string s2 = string.Empty;
        for (int i = 0; i < array2.Length; i++)
        {
            s = array[i].Trim();
            s2 = array2[i].Trim();
            if (int.Parse(s2) < int.Parse(s))
            {
                return false;
            }
            if (int.Parse(s2) > int.Parse(s))
            {
                return true;
            }
        }
        return false;
    }

    public bool CheckLargeVersion(string version1, string version2)
    {
        string[] array = version1.Split(new char[]
        {
            '.'
        });
        string[] array2 = version2.Split(new char[]
        {
            '.'
        });
        for (int i = 0; i < 3; i++)
        {
            if (int.Parse(array[i].Trim()) != int.Parse(array2[i].Trim()))
            {
                return false;
            }
        }
        return true;
    }

    public void UnZipFiles(string zipedFileName, string unZipDirectory, string password, Action callBack)
    {
        try
        {
            this.UnZipFiles(zipedFileName, unZipDirectory, password);
            callBack();
        }
        catch (Exception ex)
        {
            FFDebug.LogError(this, ex.ToString());
        }
    }

    public void UnZipFiles(string zipedFileName, string unZipDirectory, string password)
    {
        this.zipSize = 0L;
        using (ZipFile zipFile = new ZipFile(File.Open(zipedFileName, FileMode.OpenOrCreate)))
        {
            int num = 0;
            while ((long)num < zipFile.Count)
            {
                this.zipSize += zipFile[num].Size;
                num++;
            }
        }
        ZipExtraData zipExtraData = new ZipExtraData();
        this.writeSize = 0L;
        using (ZipInputStream zipInputStream = new ZipInputStream(File.Open(zipedFileName, FileMode.OpenOrCreate)))
        {
            if (!string.IsNullOrEmpty(password))
            {
                zipInputStream.Password = password;
            }
            ZipEntry nextEntry;
            while ((nextEntry = zipInputStream.GetNextEntry()) != null)
            {
                string text = Path.GetDirectoryName(unZipDirectory);
                string text2 = Path.GetDirectoryName(nextEntry.Name);
                string fileName = Path.GetFileName(nextEntry.Name);
                text2 = text2.Replace(".", "$");
                text = text + "/" + text2;
                if (!Directory.Exists(text))
                {
                    Directory.CreateDirectory(text);
                }
                if (!string.IsNullOrEmpty(fileName))
                {
                    FileStream fileStream = File.Create(Path.Combine(text, fileName));
                    byte[] array = new byte[2048];
                    for (; ; )
                    {
                        int num2 = zipInputStream.Read(array, 0, array.Length);
                        if (num2 <= 0)
                        {
                            break;
                        }
                        fileStream.Write(array, 0, num2);
                        this.writeSize += (long)num2;
                    }
                    fileStream.Close();
                }
            }
        }
    }

    public byte[] HttpDownloadFile(string strUrl)
    {
        byte[] result;
        try
        {
            if (string.IsNullOrEmpty(strUrl))
            {
                FFDebug.LogError(this, string.Concat(new string[]
                {
                    "URL is empty! " + strUrl
                }));
                result = null;
            }
            else
            {
                HttpWebRequest httpWebRequest = WebRequest.Create(strUrl) as HttpWebRequest;
                httpWebRequest.CachePolicy = new RequestCachePolicy(RequestCacheLevel.NoCacheNoStore);
                httpWebRequest.Timeout = 30000;
                HttpWebResponse httpWebResponse = httpWebRequest.GetResponse() as HttpWebResponse;
                Stream responseStream = httpWebResponse.GetResponseStream();
                int num = (int)httpWebResponse.ContentLength;
                if (0 >= num)
                {
                    FFDebug.LogError(this, "Http's content is empty!");
                    responseStream.Close();
                    httpWebResponse.Close();
                    httpWebRequest.Abort();
                    result = null;
                }
                else
                {
                    byte[] array = new byte[num];
                    responseStream.Read(array, 0, num);
                    responseStream.Close();
                    httpWebResponse.Close();
                    httpWebRequest.Abort();
                    result = array;
                }
            }
        }
        catch (Exception ex)
        {
            FFDebug.LogError(this, "HttpDownloadFile Faile! " + ex.ToString());
            result = null;
        }
        return result;
    }

    private long zipSize;

    private long writeSize;

    private WWW www;

    private PatchController patchController;

    private string versionPath = string.Empty;

    private string lastVersion = string.Empty;
}
