using System;
using System.Collections.Generic;
using System.Net;
using Framework.Managers;
using ProtoBuf;

namespace Net
{
    public class NetWorkModule : LSingleton<NetWorkModule>
    {
        public void Connect(IPEndPoint ipEndPoint, USocketType type, OnSocketCallback onConnect, OnSocketCallback onConnectFail)
        {
            if (this.MainSocket != null)
            {
                this.MainSocket.Dispose();
                this.MainSocket = null;
            }
            this.MainSocket = new USocket(type);
            this.MainSocket.ResetScene = this.ResetScene;
            this.MainSocket.Connect(ipEndPoint, onConnect, onConnectFail, null, null);
        }

        public void RegisterProtoMsg<T>(CommandID cmdid, ProtoMsgCallback<T> callback) where T : IExtensible
        {
            this.RegisterProtoMsg<T>((ushort)cmdid, callback);
        }

        public void RegisterProtoMsg<T>(ushort cmdid, ProtoMsgCallback<T> callback) where T : IExtensible
        {
            this.ProtoParseCallbackDict[cmdid] = delegate (OctetsStream os)
            {
                T t = os.unmarshal_proto<T>();
                return new NullCmd
                {
                    Msgid = cmdid,
                    Data = t
                };
            };
            if (!this.ProtoCallbackDict.ContainsKey(cmdid))
            {
                this.ProtoCallbackDict[cmdid] = delegate (object obj)
                {
                    callback((T)((object)obj));
                };
            }
        }

        public void RegisterStructMsg<T>(ushort cmid, StructMsgCallback<T> callback) where T : StructCmd, new()
        {
            this.StructParseCallbackDict[cmid] = delegate (OctetsStream os)
            {
                T t = Activator.CreateInstance<T>();
                os.unmarshal_struct(t);
                return new NullCmd
                {
                    Msgid = cmid,
                    Data = t
                };
            };
            if (!this.StructCallbackDict.ContainsKey(cmid))
            {
                this.StructCallbackDict[cmid] = delegate (object obj)
                {
                    callback((T)((object)obj));
                };
            }
        }

        public void DeRegisterMsg(ushort cmdId)
        {
            if (this.ProtoCallbackDict.ContainsKey(cmdId))
            {
                this.ProtoCallbackDict.Remove(cmdId);
            }
        }

        public void DeRegisterStrucMsg(ushort cmdId)
        {
            if (this.StructCallbackDict.ContainsKey(cmdId))
            {
                this.StructCallbackDict.Remove(cmdId);
            }
        }

        public void RegisterBlockingMsg(ushort requestID, ushort responseID)
        {
            if (!this.BlockingMsgPair.ContainsKey(requestID))
            {
                this.BlockingMsgPair[requestID] = responseID;
            }
        }

        public void Send(StructCmd cmd, bool isToSelf = false)
        {
            if (this.MainSocket != null)
            {
                this.MainSocket.Send(cmd, isToSelf);
            }
        }

        public void Send<T>(byte cmd, byte param, T t, bool istoself = false) where T : IExtensible
        {
            if (this.MainSocket != null)
            {
                this.MainSocket.Send<T>(cmd, param, t, istoself);
            }
        }

        public void Send<T>(ushort cmdid, T t, bool istoself = false) where T : IExtensible
        {
            if (this.MainSocket != null)
            {
                this.MainSocket.Send<T>(cmdid, t, istoself);
            }
        }

        public void Send<T>(CommandID cmdid, T t, bool istoself = false) where T : IExtensible
        {
            if (this.MainSocket != null)
            {
                this.MainSocket.Send<T>((ushort)cmdid, t, istoself);
            }
        }

        public void Send(ushort cmdid, byte[] data)
        {
            if (this.MainSocket != null)
            {
                this.MainSocket.Send(cmdid, data);
            }
        }

        public void BeginCacheMsg()
        {
            this.MainSocket.BeginCacheMsg();
        }

        public void FinishCacheMsg()
        {
            this.MainSocket.FinishCacheMsg();
        }

        public bool isOnCacheMsg
        {
            get
            {
                return this.MainSocket.isOnCaching;
            }
        }

        public bool isNeedCloseLoading
        {
            get
            {
                bool afterCreatMainPlayerNeedCloseLoading = this.MainSocket.afterCreatMainPlayerNeedCloseLoading;
                if (afterCreatMainPlayerNeedCloseLoading)
                {
                    this.MainSocket.afterCreatMainPlayerNeedCloseLoading = false;
                }
                return afterCreatMainPlayerNeedCloseLoading;
            }
        }

        public void Reconnect()
        {
            ManagerCenter.Instance.GetManager<GameMainManager>().Logout(true);
        }

        public void Update()
        {
            if (this.MainSocket != null)
            {
                this.MainSocket.ReceiveBufferManager();
            }
        }

        public void OnDestroy()
        {
            if (this.MainSocket == null)
            {
                return;
            }
            this.MainSocket.Dispose();
        }

        public void Close()
        {
            if (this.MainSocket != null)
            {
                this.MainSocket.Dispose();
            }
        }

        public USocket MainSocket;

        public Dictionary<ushort, OnCmdParseCallback> ProtoParseCallbackDict = new Dictionary<ushort, OnCmdParseCallback>();

        public Dictionary<ushort, OnCmdParseCallback> StructParseCallbackDict = new Dictionary<ushort, OnCmdParseCallback>();

        public Dictionary<ushort, OnCmdCallback> ProtoCallbackDict = new Dictionary<ushort, OnCmdCallback>();

        public Dictionary<ushort, OnCmdCallback> StructCallbackDict = new Dictionary<ushort, OnCmdCallback>();

        public Dictionary<ushort, ushort> BlockingMsgPair = new Dictionary<ushort, ushort>();

        public Action ResetScene;

        public bool IsReconnecting;
    }
}
