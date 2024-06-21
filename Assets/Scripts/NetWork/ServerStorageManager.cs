using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using apprentice;
using Framework.Managers;
using Game.Scene;
using Net;
using ProtoBuf;
using UnityEngine;

public class ServerStorageManager : NetWorkBase
{
    public static ServerStorageManager Instance
    {
        get
        {
            if (ServerStorageManager._instance == null)
            {
                ServerStorageManager._instance = new ServerStorageManager();
                ServerStorageManager._instance.OnInit();
            }
            return ServerStorageManager._instance;
        }
    }

    private string KEY_PRE
    {
        get
        {
            if (MainPlayer.Self == null)
            {
                return string.Empty;
            }
            GameScene manager = ManagerCenter.Instance.GetManager<GameScene>();
            ulong num = (!manager.isAbattoirScene) ? MainPlayer.Self.OtherPlayerData.BaseData.id : 0UL;
            return "storage_" + num + "_";
        }
    }

    private void OnInit()
    {
        LSingleton<NetWorkModule>.Instance.RegisterProtoMsg<MSG_Req_OperateClientDatas_CS>(CommandID.MSG_Req_OperateClientDatas_CS, new ProtoMsgCallback<MSG_Req_OperateClientDatas_CS>(this.MSG_Req_OperateClientDatas_CS_CB));
    }

    public void GetData(ServerStorageKey key, uint type = 0U)
    {
        this.GetData(key.ToString(), type);
    }

    public void GetData(string key, uint type = 0U)
    {
        if (type == 0U && MainPlayer.Self == null)
        {
            EntitiesManager manager = ManagerCenter.Instance.GetManager<EntitiesManager>();
            manager.onMainPlayer = (Action)Delegate.Combine(manager.onMainPlayer, new Action(delegate ()
            {
                this.GetData(key, type);
            }));
            return;
        }
        base.SendMsg<MSG_Req_OperateClientDatas_CS>(CommandID.MSG_Req_OperateClientDatas_CS, new MSG_Req_OperateClientDatas_CS
        {
            key = ((type != 0U) ? string.Empty : this.KEY_PRE) + key,
            op = 3U,
            type = type
        }, false);
    }

    public void AddUpdateData(ServerStorageKey key, string content, uint type = 0U)
    {
        this.AddUpdateData(key.ToString(), content, type);
    }

    public void AddUpdateData(string key, string content, uint type = 0U)
    {
        base.SendMsg<MSG_Req_OperateClientDatas_CS>(CommandID.MSG_Req_OperateClientDatas_CS, new MSG_Req_OperateClientDatas_CS
        {
            key = ((type != 0U) ? string.Empty : this.KEY_PRE) + key,
            value = content,
            op = 1U,
            type = type
        }, false);
    }

    public void RegReqCallBack(ServerStorageKey ssk, Action<MSG_Req_OperateClientDatas_CS> back)
    {
        this.backDic[ssk.ToString()] = back;
    }

    public void UnRegReqCallBack(string ssk)
    {
        if (this.backDic.ContainsKey(ssk))
        {
            this.backDic.Remove(ssk);
        }
    }

    private void MSG_Req_OperateClientDatas_CS_CB(MSG_Req_OperateClientDatas_CS msg)
    {
        string text = msg.key.Replace(this.KEY_PRE, string.Empty);
        if (text == ServerStorageKey.CharacterBottom.ToString())
        {
            if (this.DeserializeClass<CharacterBottomModel>(msg.value) == null)
            {
                CharacterBottomModel characterBottomModel = new CharacterBottomModel();
            }
        }
        else if (text == ServerStorageKey.ChatTab.ToString())
        {
            ControllerManager.Instance.GetController<ChatControl>().mUiChat.SetupTabPanelByStorageData(msg.value);
        }
        else if (text == ServerStorageKey.Shortcuts.ToString())
        {
            UIManager.GetUIObject<UI_MainView>().Root.GetComponent<UI_ShortcutControl>().ReqShortcutDataCb(msg, text);
            ControllerManager.Instance.GetController<AutoFightController>().InitPropCdData();
        }
        else if (text == ServerStorageKey.AbattoirShortcuts.ToString())
        {
            UIManager.GetUIObject<UI_MainView>().Root.GetComponent<UI_ShortcutControl>().ReqShortcutDataCb(msg, text);
        }
        else if (text == ServerStorageKey.SystemData.ToString())
        {
            ControllerManager.Instance.GetController<SystemSettingController>().StorageDataCallBack(text, msg.value);
        }
        else if (text.Contains(ServerStorageKey.SkillSlotSort.ToString()))
        {
            SkillViewControll controller = ControllerManager.Instance.GetController<SkillViewControll>();
            if (controller != null)
            {
                controller.ReqSkillUIIndexDataCb(msg.retcode, msg.value);
            }
            ControllerManager.Instance.GetController<AutoFightController>().InitSkillCdData();
        }
        else if (text == ServerStorageKey.FRIEND_IDS.ToString())
        {
            FriendControllerNew controller2 = ControllerManager.Instance.GetController<FriendControllerNew>();
            controller2.mOldFrinedIds = msg.value;
            ControllerManager.Instance.GetController<FriendControllerNew>().RefreshFriendTip();
        }
        else if (text.Contains(ServerStorageKey.AutoFight.ToString()))
        {
            AutoFightController controller3 = ControllerManager.Instance.GetController<AutoFightController>();
            controller3.OnServerSettingBack(msg.value);
        }
        if (this.backDic.ContainsKey(text))
        {
            if (this.backDic[text] != null)
            {
                this.backDic[text](msg);
            }
            else
            {
                this.backDic.Remove(text);
            }
        }
    }

    public T DeserializeClass<T>(string _msg)
    {
        T result;
        try
        {
            if (_msg == string.Empty)
            {
                result = default(T);
            }
            else
            {
                byte[] bytes = Encoding.Unicode.GetBytes(_msg);
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    memoryStream.Write(bytes, 0, bytes.Length);
                    memoryStream.Position = 0L;
                    result = Serializer.Deserialize<T>(memoryStream);
                }
            }
        }
        catch (Exception)
        {
            result = default(T);
        }
        return result;
    }

    public string SerializeClass<T>(T t)
    {
        byte[] array;
        using (MemoryStream memoryStream = new MemoryStream())
        {
            Serializer.Serialize<T>(memoryStream, t);
            array = new byte[memoryStream.Length];
            memoryStream.Position = 0L;
            memoryStream.Read(array, 0, array.Length);
        }
        return Encoding.Unicode.GetString(array);
    }

    public T DeserializeClassLocal<T>(string _msg)
    {
        T result;
        try
        {
            if (_msg == string.Empty)
            {
                result = default(T);
            }
            else
            {
                byte[] bytes = Encoding.UTF8.GetBytes(_msg);
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    memoryStream.Write(bytes, 0, bytes.Length);
                    memoryStream.Position = 0L;
                    result = Serializer.Deserialize<T>(memoryStream);
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("DeserializeClass err:" + ex.Message);
            result = default(T);
        }
        return result;
    }

    public string SerializeClassLocal<T>(T t)
    {
        byte[] array;
        using (MemoryStream memoryStream = new MemoryStream())
        {
            Serializer.Serialize<T>(memoryStream, t);
            array = new byte[memoryStream.Length];
            memoryStream.Position = 0L;
            memoryStream.Read(array, 0, array.Length);
        }
        return Encoding.UTF8.GetString(array);
    }

    private const string _PRE = "storage_";

    private static ServerStorageManager _instance;

    private Dictionary<string, Action<MSG_Req_OperateClientDatas_CS>> backDic = new Dictionary<string, Action<MSG_Req_OperateClientDatas_CS>>();

    private enum OpeType
    {
        DeleteAll,
        AddUpdate,
        Delete,
        Get
    }
}
