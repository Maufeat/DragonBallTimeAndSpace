using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class ConfigCopy
{
    public ConfigCopy(MonoBehaviour _monoBehaviour)
    {
        this.m_monoBehaviour = _monoBehaviour;
    }

    public void CopyConfig(Action _callBack, bool isEnable = false)
    {
        _callBack();
    }

    public void CopyConfigImlement()
    {
        this.m_isDone = false;
        string url = Application.streamingAssetsPath + "/fileList.txt";
        List<string> abList = new List<string>();
        StringBuilder stringBuilder = new StringBuilder(Application.streamingAssetsPath);
        this.m_monoBehaviour.StartCoroutine(this.LoadFrompersister(url, delegate (WWW www)
        {
            if (string.IsNullOrEmpty(www.error))
            {
                string[] array = www.text.Split(new char[]
                {
                    ','
                });
                for (int i = 0; i < array.Length; i++)
                {
                    if (array[i] != string.Empty)
                    {
                        abList.Add(array[i]);
                    }
                }
                this.SerializePersistent(abList, 0);
            }
            else
            {
                FFDebug.LogWarning(this, "fileList Error:" + www.error);
            }
        }));
    }

    public void SerializePersistent(List<string> _abListUrl, int _i)
    {
        string directoryName = Path.GetDirectoryName(_abListUrl[_i]);
        string text = "/" + _abListUrl[_i];
        string strDirectory = Application.persistentDataPath + "/Assets/" + directoryName + "/";
        string des = Application.persistentDataPath + "/Assets" + text;
        string url = Application.streamingAssetsPath + text;
        this.m_monoBehaviour.StartCoroutine(this.LoadFrompersister(url, delegate (WWW www2)
        {
            try
            {
                if (string.IsNullOrEmpty(www2.error))
                {
                    if (File.Exists(des))
                    {
                        File.Delete(des);
                    }
                    Directory.CreateDirectory(strDirectory);
                    FileStream fileStream = File.Create(des);
                    fileStream.Write(www2.bytes, 0, www2.bytes.Length);
                    fileStream.Flush();
                    fileStream.Close();
                    fileStream.Dispose();
                }
                else
                {
                    FFDebug.LogWarning(this, www2.error);
                }
            }
            catch (Exception ex)
            {
                FFDebug.LogError(this, "File write:" + ex.ToString());
            }
            _i++;
            if (_i < _abListUrl.Count)
            {
                this.SerializePersistent(_abListUrl, _i);
            }
            else
            {
                this.Finish();
            }
        }));
    }

    public void Finish()
    {
        this.m_isDone = true;
        this.m_monoBehaviour.enabled = true;
        if (this.m_actionCallBack != null)
        {
            this.m_actionCallBack();
        }
    }

    private IEnumerator LoadFrompersister(string _url, Action<WWW> _callBack)
    {
        WWW www = new WWW(_url);
        yield return www;
        _callBack(www);
        www.Dispose();
        yield break;
    }

    private MonoBehaviour m_monoBehaviour;

    private Action m_actionCallBack;

    public bool m_isDone;
}
