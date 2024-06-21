using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Threading;
using Framework.Managers;
using magic;
using msg;
using ProtoBuf;
using UnityEngine;

namespace Net
{
	// Token: 0x02000CB2 RID: 3250
	public class USocket
	{
		// Token: 0x060060B9 RID: 24761 RVA: 0x0019A348 File Offset: 0x00198548
		public USocket(USocketType type)
		{
			this.socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			this.socket.NoDelay = true;
			this.socket.Bind(new IPEndPoint(IPAddress.Any, 0));
			this.type = type;
			this.receiveBuffer = new byte[65536];
			this.onConnect = null;
			this.onClose = null;
			this.onError = null;
			this.logMsg = true;
		}

		// Token: 0x17000FB6 RID: 4022
		// (get) Token: 0x060060BB RID: 24763 RVA: 0x0003B1A7 File Offset: 0x000393A7
		// (set) Token: 0x060060BC RID: 24764 RVA: 0x0003B1AF File Offset: 0x000393AF
		private int reconnecttimes
		{
			get
			{
				return this._reconnecttimes;
			}
			set
			{
				this._reconnecttimes = value;
			}
		}

		// Token: 0x060060BD RID: 24765 RVA: 0x0003B1B8 File Offset: 0x000393B8
		public void Connect(IPEndPoint ipEnd, OnSocketCallback onConnect = null, OnSocketCallback onConnectFail = null, OnSocketCallback onClose = null, OnSocketCallback onError = null)
		{
			this.ipEndPoint = ipEnd;
			this.onConnect = onConnect;
			this.onConnectFail = onConnectFail;
			this.onError = onError;
			this.onClose = onClose;
			this.ResetSocket();
			this.DoConncect();
		}

		// Token: 0x060060BE RID: 24766 RVA: 0x0019A42C File Offset: 0x0019862C
		private void DoConncect()
		{
			try
			{
				FFDebug.Log(this, FFLogType.Login, string.Concat(new object[]
				{
					"Do Connect ",
					this.ipEndPoint,
					"  ",
					DateTime.Now
				}));
				LSingleton<ThreadManager>.Instance.RunAsync(delegate
				{
					try
					{
						FFDebug.Log(this, FFLogType.Login, "Run Connect Wait on thread " + Thread.CurrentThread.GetHashCode());
						this.alldone.WaitOne(5000);
						this.connectCallBack();
					}
					catch (Exception ex)
					{
						Debug.LogError(string.Concat(new object[]
						{
							"ConnectWait Exception: ",
							ex.ToString(),
							" ",
							DateTime.Now
						}));
					}
				});
				LSingleton<ThreadManager>.Instance.RunAsync(delegate
				{
					try
					{
						FFDebug.Log(this, FFLogType.Login, string.Concat(new object[]
						{
							"Run Connect on thread ",
							Thread.CurrentThread.GetHashCode(),
							"   ",
							this.ipEndPoint,
							"  ",
							DateTime.Now
						}));
						this.alldone.Reset();
						this.socket.Connect(this.ipEndPoint);
					}
					catch (Exception ex)
					{
						Debug.LogError(string.Concat(new object[]
						{
							"Connect Exception: ",
							ex.ToString(),
							" ",
							DateTime.Now,
							"  ",
							this.socket.GetHashCode()
						}));
						this.CloseSocket();
					}
					finally
					{
						this.alldone.Set();
					}
				});
			}
			catch (Exception arg)
			{
				Debug.LogError("Connect Exception " + arg);
				if (this.onConnectFail != null)
				{
					this.onConnectFail();
				}
			}
		}

		// Token: 0x060060BF RID: 24767 RVA: 0x0019A4E0 File Offset: 0x001986E0
		private void connectCallBack()
		{
			FFDebug.Log(this, FFLogType.Login, string.Concat(new object[]
			{
				"connectCallBack ",
				DateTime.Now,
				"  thread: ",
				Thread.CurrentThread.GetHashCode()
			}));
			LSingleton<ThreadManager>.Instance.RunOnMainThread(delegate ()
			{
				try
				{
					if (this.socket == null)
					{
						Debug.LogWarning("socket == null " + DateTime.Now);
						if (this.onConnectFail != null)
						{
							this.onConnectFail();
						}
					}
					else
					{
						this.socket.Blocking = false;
						if (this.socket.Connected)
						{
							this.sendActions.Clear();
							this.SendBuffer.clear();
							this.SendBuffer.position(0);
							this.lastcmdlength = 0;
							this.sleepSwitch = false;
							this.MsgThread = new Thread(new ThreadStart(this.OnReceive));
							this.MsgThread.IsBackground = true;
							this.MsgThread.Start();
							FFDebug.Log(this, FFLogType.Login, "ConnectSuccess  " + DateTime.Now);
							if (this.onConnect != null)
							{
								this.onConnect();
							}
						}
						else
						{
							Debug.LogWarning("Connect Time Out!!! " + DateTime.Now);
							if (this.onConnectFail != null)
							{
								this.onConnectFail();
							}
						}
					}
				}
				catch (Exception ex)
				{
					Debug.LogError(ex.ToString());
					throw;
				}
			});
		}

		// Token: 0x060060C0 RID: 24768 RVA: 0x0019A54C File Offset: 0x0019874C
		public void ReConnectGateway()
		{
			if (LSingleton<NetWorkModule>.Instance.IsReconnecting)
			{
				FFDebug.Log(this, FFLogType.Login, "ReConnectGateway..." + LSingleton<NetWorkModule>.Instance.IsReconnecting.ToString());
				return;
			}
			LSingleton<NetWorkModule>.Instance.IsReconnecting = true;
			if (ControllerManager.Instance.GetController<LoadTipsController>() != null)
			{
				ControllerManager.Instance.GetController<LoadTipsController>().ShowReconnectTips("正在连接中");
			}
			if (this.reconnecttimes > 2)
			{
				this.Dispose();
				ControllerManager.Instance.GetLoginController().Login();
				return;
			}
			int reconnecttimes = this.reconnecttimes;
			this.reconnecttimes = reconnecttimes + 1;
			FFDebug.Log(this, FFLogType.Login, "ReConnectGateway..." + this.reconnecttimes);
			try
			{
				this.ResetSocket();
				if (this.ResetScene != null)
				{
					this.ResetScene();
				}
				this.socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
				FFDebug.Log(this, FFLogType.Login, "new Socket " + this.socket.GetHashCode());
				try
				{
					FFDebug.Log(this, FFLogType.Login, string.Concat(new object[]
					{
						"Do Reconnect GateWay Connect ",
						this.ipEndPoint,
						"  ",
						DateTime.Now,
						"  ",
						Thread.CurrentThread.GetHashCode()
					}));
					this.DoConncect();
				}
				catch (Exception arg)
				{
					Debug.LogError("Connect Exception " + arg);
					if (this.onConnectFail != null)
					{
						this.onConnectFail();
					}
				}
			}
			catch (Exception ex)
			{
				Debug.LogError("Reconnect Exception : " + ex.Message);
				if (this.onConnectFail != null)
				{
					this.onConnectFail();
				}
			}
		}

		// Token: 0x060060C1 RID: 24769 RVA: 0x0019A728 File Offset: 0x00198928
		private bool send()
		{
			if (this.blockingState == BlockingState.Close)
			{
				return false;
			}
			bool result = false;
			try
			{
				int num = MessageEncrypt.FillZero(0, ref this.SendBuffer);
				int index = this.SendBuffer.size() - this.lastcmdlength - num - 4;
				if (num != 0)
				{
					this.SendBuffer.write_ushort(index, (ushort)(num + this.lastcmdlength));
				}
				int num2 = this.socket.Send(this.SendBuffer.buffer(), this.SendBuffer.size(), SocketFlags.None);
				if (num2 > 0)
				{
					this.SendBuffer.erase(0, num2);
				}
				result = true;
				this.lastcmdlength = 0;
			}
			catch (Exception ex)
			{
				Debug.LogError("send Exception    " + ex.Message);
				result = false;
				this.SendBuffer.clear();
				this.lastcmdlength = 0;
			}
			return result;
		}

		// Token: 0x060060C2 RID: 24770 RVA: 0x0019A800 File Offset: 0x00198A00
		private bool IsSendBlock(ushort cmdid)
		{
			if (LSingleton<NetWorkModule>.Instance.BlockingMsgPair.ContainsKey(cmdid))
			{
				if (this.blockingState == BlockingState.Waiting)
				{
					Debug.LogWarning("Blocking Waiting,Cant Send Msg,Last Msg ID: " + this.lastBlockingRequestMsgID);
					return true;
				}
				this.lastSendTick = DateTime.Now.Ticks;
				this.blockingResponseMsgID = (int)LSingleton<NetWorkModule>.Instance.BlockingMsgPair[cmdid];
				this.blockingState = BlockingState.Waiting;
				this.lastBlockingRequestMsgID = (int)cmdid;
			}
			return false;
		}

		// Token: 0x060060C3 RID: 24771 RVA: 0x0019A880 File Offset: 0x00198A80
		public void Send<T>(ushort cmdid, T t, bool istoself) where T : IExtensible
		{
			if (istoself)
			{
				if (LSingleton<NetWorkModule>.Instance.ProtoCallbackDict.ContainsKey(cmdid))
				{
					LSingleton<NetWorkModule>.Instance.ProtoCallbackDict[cmdid](t);
					this.DumpSendPacket(cmdid, t);
				}
				return;
			}
			if (this.IsSendBlock(cmdid))
			{
				return;
			}
			if (this.logMsg && cmdid != 2269 && cmdid != 2583 && cmdid != 2296 && cmdid != 2279)
			{
				this.DumpSendPacket(cmdid, t);
				Debug.Log(string.Concat(new object[]
				{
					"SendMsg: cmdid->",
					cmdid,
					" CommandID->",
					(CommandID)cmdid
				}));
			}
			object obj = USocket.sendlock;
			lock (obj)
			{
				this.sendActions.Enqueue(delegate
				{
					int num = this.SendBuffer.size();
					try
					{
						this.SendBuffer.marshal_uint(0U);
						this.SendBuffer.marshal_ushort(cmdid);
						uint intervalMsecond = SingletonForMono<GameTime>.Instance.GetIntervalMsecond();
						this.SendBuffer.marshal_uint(intervalMsecond);
						this.SendBuffer.marshal_proto<T>(t);
						int num2 = this.SendBuffer.size() - num - 4;
						if (NullCmd.IS_COMPRESS && this.SendBuffer.size() - num - 4 > 32 && MessageCompress.MsgCompress(num + 4, num2, ref this.SendBuffer))
						{
							this.SendBuffer.write_byte(num + 3, 64);
							num2 = (int)((ushort)(this.SendBuffer.size() - num - 4));
						}
						this.SendBuffer.write_ushort(num, (ushort)num2);
						this.lastcmdlength = num2;
					}
					catch (Exception ex)
					{
						Debug.LogError("Send Exception:" + ex.Message);
						this.SendBuffer.erase(num, this.SendBuffer.size());
					}
				});
			}
		}

		// Token: 0x060060C4 RID: 24772 RVA: 0x0019A9DC File Offset: 0x00198BDC
		public void Send<T>(byte cmd, byte param, T t, bool istoself) where T : IExtensible
		{
			if (istoself)
			{
				if (LSingleton<NetWorkModule>.Instance.ProtoCallbackDict.ContainsKey(USocket.GetMsgId(cmd, param)))
				{
					LSingleton<NetWorkModule>.Instance.ProtoCallbackDict[USocket.GetMsgId(cmd, param)](t);
					this.DumpSendPacket((ushort)cmd, t);
				}
				return;
			}
			if (this.IsSendBlock(USocket.GetMsgId(cmd, param)))
			{
				return;
			}
			if (this.logMsg)
			{
				this.DumpSendPacket((ushort)cmd, t);
				Debug.Log(string.Concat(new object[]
				{
					"SendMsg: cmdid->",
					USocket.GetMsgId(cmd, param),
					" CommandID->",
					(CommandID)USocket.GetMsgId(cmd, param)
				}));
			}
			object obj = USocket.sendlock;
			lock (obj)
			{
				this.sendActions.Enqueue(delegate
				{
					int num = this.SendBuffer.size();
					try
					{
						this.SendBuffer.marshal_uint(0U);
						this.SendBuffer.marshal_byte(cmd);
						this.SendBuffer.marshal_byte(param);
						uint intervalMsecond = SingletonForMono<GameTime>.Instance.GetIntervalMsecond();
						this.SendBuffer.marshal_uint(intervalMsecond);
						this.SendBuffer.marshal_proto<T>(t);
						int num2 = this.SendBuffer.size() - num - 4;
						if (NullCmd.IS_COMPRESS && this.SendBuffer.size() - num - 4 > 32 && MessageCompress.MsgCompress(num + 4, num2, ref this.SendBuffer))
						{
							this.SendBuffer.write_byte(num + 3, 64);
							num2 = (int)((ushort)(this.SendBuffer.size() - num - 4));
						}
						this.SendBuffer.write_ushort(num, (ushort)num2);
						this.lastcmdlength = num2;
					}
					catch (Exception ex)
					{
						Debug.LogError("Send Exception:" + ex.Message);
						this.SendBuffer.erase(num, this.SendBuffer.size());
					}
				});
			}
		}

		// Token: 0x060060C5 RID: 24773 RVA: 0x0019AB40 File Offset: 0x00198D40
		public void Send(StructCmd cmd, bool istoself)
		{
			ushort num;
			if (this.type == USocketType.Fir)
			{
				num = USocket.GetMsgId(cmd.MsgCmd, cmd.MsgParam);
				if (this.logMsg)
				{
					this.DumpSendPacket((ushort)cmd.MsgCmd, cmd);
					Debug.Log("SendMsg: cmd.MsgCmd-> " + cmd.MsgCmd + " cmd.MsgParam-> " + cmd.MsgParam + " num: " + num);
				}
			}
			else
			{
				num = cmd.Msgid;
				if (this.logMsg && num != 2583)
				{
					this.DumpSendPacket((ushort)cmd.MsgCmd, cmd);
					this.DumpPacket(num, cmd.Data);
					Debug.Log(string.Concat(new object[]
					{
						"SendMsg: cmdid->",
						num,
						" CommandID->",
						(CommandID)num
					}));
				}
			}
			if (istoself)
			{
				if (LSingleton<NetWorkModule>.Instance.StructCallbackDict.ContainsKey(num))
				{
					LSingleton<NetWorkModule>.Instance.StructCallbackDict[num](cmd);
					this.DumpSendPacket(num, cmd);
				}
				return;
			}
			if (this.IsSendBlock(num))
			{
				return;
			}
			if (this.logMsg)
			{
				this.DumpSendPacket(num, cmd);
				Debug.Log(string.Concat(new object[]
				{
					"SendMsg: cmdid->",
					num,
					" CommandID->",
					(CommandID)num
				}));
			}
			object obj = USocket.sendlock;
			lock (obj)
			{
				this.sendActions.Enqueue(delegate
				{
					int num2 = this.SendBuffer.size();
					try
					{
						this.SendBuffer.marshal_uint(0U);
						if (this.type == USocketType.Fir)
						{
							this.SendBuffer.marshal_byte(cmd.MsgCmd);
							this.SendBuffer.marshal_byte(cmd.MsgParam);
						}
						else
						{
							this.SendBuffer.marshal_ushort(cmd.Msgid);
						}
						uint intervalMsecond = SingletonForMono<GameTime>.Instance.GetIntervalMsecond();
						this.SendBuffer.marshal_uint(intervalMsecond);
						this.SendBuffer.marshal_struct(cmd);
						ushort num3 = (ushort)(this.SendBuffer.size() - num2 - 4);
						if (NullCmd.IS_COMPRESS && this.SendBuffer.size() - num2 - 4 > 32 && MessageCompress.MsgCompress(num2 + 4, (int)num3, ref this.SendBuffer))
						{
							this.SendBuffer.write_byte(num2 + 3, 64);
							num3 = (ushort)(this.SendBuffer.size() - num2 - 4);
						}
						this.SendBuffer.write_ushort(num2, num3);
						this.lastcmdlength = (int)num3;
					}
					catch (Exception ex)
					{
						Debug.LogError("Send Exception:" + ex.Message);
						this.SendBuffer.erase(num2, this.SendBuffer.size());
					}
				});
			}
		}

		// Token: 0x060060C6 RID: 24774 RVA: 0x0019AD34 File Offset: 0x00198F34
		public void Send(ushort cmdid, byte[] data)
		{
			if (this.IsSendBlock(cmdid))
			{
				return;
			}
			if (this.logMsg)
			{
				Debug.Log(string.Concat(new object[]
				{
					"SendMsg: cmdid->",
					cmdid,
					" CommandID->",
					(CommandID)cmdid
				}));
			}
			object obj = USocket.sendlock;
			lock (obj)
			{
				this.sendActions.Enqueue(delegate
				{
					int num = this.SendBuffer.size();
					try
					{
						this.SendBuffer.marshal_uint(0U);
						this.SendBuffer.marshal_ushort(cmdid);
						uint intervalMsecond = SingletonForMono<GameTime>.Instance.GetIntervalMsecond();
						this.SendBuffer.marshal_uint(intervalMsecond);
						this.SendBuffer.marshal_ushort((ushort)data.Length);
						this.SendBuffer.push_bytes(data);
						ushort num2 = (ushort)(this.SendBuffer.size() - num - 4);
						if (NullCmd.IS_COMPRESS && this.SendBuffer.size() - num - 4 > 32 && MessageCompress.MsgCompress(num + 4, (int)num2, ref this.SendBuffer))
						{
							this.SendBuffer.write_byte(num + 3, 64);
							num2 = (ushort)(this.SendBuffer.size() - num - 4);
						}
						this.SendBuffer.write_ushort(num, num2);
						this.lastcmdlength = (int)num2;
					}
					catch (Exception ex)
					{
						Debug.LogError("Send Exception:" + ex.Message);
						this.SendBuffer.erase(num, this.SendBuffer.size());
					}
				});
			}
		}

		// Token: 0x060060C7 RID: 24775 RVA: 0x0019ADEC File Offset: 0x00198FEC
		public static void PrintByteArr(byte[] byteArr, int len)
		{
			string text = string.Empty;
			for (int i = 0; i < len; i++)
			{
				text = text + byteArr[i] + "==";
			}
			FFDebug.Log("USocket", FFLogType.Network, text);
		}

		private void OnReceive()
		{
			if (blockingState == BlockingState.Close)
			{
				return;
			}
			Debug.Log("OnReceive thread " + Thread.CurrentThread.GetHashCode());
			while (!sleepSwitch)
			{
				Thread.Sleep(10);
				try
				{
					if (socket.Connected && socket.Available > 0)
                    {
                        int size = socket.Receive(receiveBuffer);
						int num = ReceiveBuffer.size() - encryptremainsize;
                        ReceiveBuffer.insert(ReceiveBuffer.size(), receiveBuffer, 0, size);
						encryptremainsize = 0;
						if (ReceiveBuffer.size() - num > 8)
						{
							try
                            {
								MessageEncrypt.DESDencrypt(ReceiveBuffer.buffer(), num, ReceiveBuffer.size() - encryptremainsize - num, false);
							}
							catch (Exception ex)
							{
								Debug.LogError(ex.Message + "\n也许proto有问题");
								break;
							}
						}
						ProcessReceiveBuffer();
					}
					ProcessSend();
				}
				catch (Exception ex2)
				{
					Debug.LogError(ex2.Message + " \n可能proto不一致");
					break;
				}
			}
			try
			{
				Thread.Sleep(-1);
			}
			catch (ThreadInterruptedException ex3)
			{
				Debug.LogError(ex3);
			}
		}

		private void ProcessReceiveBuffer()
        {
            bool flag = false;
			for (; ; )
			{
				flag = false;
				if (ReceiveBuffer.size() - encryptremainsize >= 4 && cmdHeadInfo == null)
				{
					try
					{
						cmdHeadInfo = NullCmd.unmarshal_head(ref ReceiveBuffer);
					}
					catch (Exception ex)
					{
						Debug.LogError("unmarshal_head Exception: " + ex.Message);
					}
				}
				if (cmdHeadInfo == null)
				{
					break;
				}
				if (ReceiveBuffer.size() - encryptremainsize >= cmdHeadInfo.MsgLength + 4)
				{
					try
					{
						DecompressBuffer.clear();
						DecompressBuffer.position(0);
						DecompressBuffer.insert(0, ReceiveBuffer, 4, cmdHeadInfo.MsgLength);
						ReceiveBuffer.erase(0, cmdHeadInfo.MsgLength + 4);
						ReceiveBuffer.position(0);
					}
					catch (Exception ex2)
					{
						Debug.LogError("ProcessReceiveBuffer Exception: " + ex2.Message);
					}
					if (cmdHeadInfo.IsCompress)
					{
						MessageCompress.DeCompress(0, ref DecompressBuffer);
					}
					byte @byte = DecompressBuffer.getByte(0);
					byte byte2 = DecompressBuffer.getByte(1);
					ushort num = DecompressBuffer.unmarshal_ushort();
					DecompressBuffer.unmarshal_uint();
					ushort num2 = 0;
					if (type == USocketType.Fir)
					{
						num2 = GetMsgId(@byte, byte2);
					}
					else
					{
						num2 = num;
					}
					object obj = receivelock;
                    lock (obj)
					{
						try
						{
							if (ManagerCenter.Instance.GetManager<LuaNetWorkManager>().IsRegisterInLua(num2))
							{
								int pos = DecompressBuffer.position();
								byte[] bufferData = DecompressBuffer.unmarshal_bytes();
								DecompressBuffer.position(pos);
								NullCmd nullCmd = new NullCmd();
								nullCmd.Msgid = num2;
								nullCmd.BufferData = bufferData;
                                if (LSingleton<NetWorkModule>.Instance.ProtoParseCallbackDict.ContainsKey(num2))
								{
									nullCmd.Data = LSingleton<NetWorkModule>.Instance.ProtoParseCallbackDict[num2](DecompressBuffer).Data;
                                }
                                if (logMsg)
								{
									Debug.LogWarning("Receive msgid: " + num2 + " " + (CommandID)num2);
								}
								this.receivedQueue.Enqueue(nullCmd);
							}
							else if (LSingleton<NetWorkModule>.Instance.ProtoParseCallbackDict.ContainsKey(num2))
                            {
                                NullCmd nullCmd2 = LSingleton<NetWorkModule>.Instance.ProtoParseCallbackDict[num2](DecompressBuffer);
								if (nullCmd2.Msgid == 2267)
								{
									SingletonForMono<GameTime>.Instance.OnServerTimeInit(nullCmd2.Data as MSG_Ret_GameTime_SC);
								}
								else if (nullCmd2.Msgid == 2268)
								{
									SingletonForMono<GameTime>.Instance.OnServerTimeReq(nullCmd2.Data as MSG_Req_UserGameTime_SC);
								}
								else if (nullCmd2.Msgid == 2296)
								{
									SingletonForMono<GameTime>.Instance.OnRetPing(nullCmd2.Data as MSG_Req_Ping_CS);
								}
								else
								{
									if (logMsg && num2 != 2291 && num2 != 2582 && num2 != 2583 && num2 != 2280)
									{
										Debug.LogWarning("Receive msgid:" + num2 + " " + (CommandID)num2);
									}
									receivedQueue.Enqueue(nullCmd2);
								}
							}
							else if (LSingleton<NetWorkModule>.Instance.StructParseCallbackDict.ContainsKey(num2))
                            {
                                NullCmd nullCmd3 = LSingleton<NetWorkModule>.Instance.StructParseCallbackDict[num2](DecompressBuffer);
                                Debug.Log("x3: " + num2);
                                if (logMsg)
								{
									try
									{
										Debug.LogWarning("Receive msgid: "+num2+" "+(CommandID)num2+" pktsize: "+nullCmd3.BufferData.Length.ToString());
									}
									catch
									{
										Debug.LogWarning("Receive msgid:"+num2+" "+(CommandID)num2);
									}
								}
								this.receivedQueue.Enqueue(nullCmd3);
							}
							else if (!ManagerCenter.Instance.GetManager<LuaNetWorkManager>().IsRegisterInLua(num2))
                            {
                                if (this.logMsg)
								{
									Debug.LogWarning(string.Concat(new object[]
									{
										"Receive msgid:",
										num2,
										" ",
										(CommandID)num2
									}));
								}
								Debug.LogWarning( "MessageID : " + num2 + " have not been register");
							}
						}
						catch (Exception ex3)
						{
							Debug.LogError(string.Concat(new object[]
							{
								"msgid proto解析失败:",
								num2,
								" ",
								(CommandID)num2,
								" ProcessReceiveBuffer Exception: ",
								ex3.Message
							}));
						}
					}
					this.DecompressBuffer.clear();
					this.DecompressBuffer.position(0);
					this.cmdHeadInfo = null;
					flag = true;
				}
				if (!flag)
				{
					return;
				}
			}
		}

		// Token: 0x060060CA RID: 24778 RVA: 0x0019B564 File Offset: 0x00199764
		private void ProcessSend()
		{
			if (this.blockingState == BlockingState.Close)
			{
				return;
			}
			while (this.sendActions.Count > 0)
			{
				object obj = USocket.sendlock;
				lock (obj)
				{
					this.sendActions.Dequeue()();
				}
			}
			if (this.SendBuffer.size() > 0)
			{
				if (this.send())
				{
					this.reconnecttimes = 0;
					return;
				}
				if (this.blockingState != BlockingState.Close)
				{
					Debug.LogWarning("ProcessSend Exception Need Reconnect!");
					this.NeedReconnect = true;
				}
			}
		}

		// Token: 0x060060CB RID: 24779 RVA: 0x0003B1EB File Offset: 0x000393EB
		public string ClearAndGetMsgLog()
		{
			string msgLog = this.MsgLog;
			this.MsgLog = "msg-->";
			return msgLog;
		}

		// Token: 0x060060CC RID: 24780 RVA: 0x0003B171 File Offset: 0x00039371
		public static ushort GetMsgId(byte cmdId, byte cmdParam)
		{
			return (ushort)(cmdId * 10000 + cmdParam);
		}

		// Token: 0x060060CD RID: 24781 RVA: 0x0003B1FE File Offset: 0x000393FE
		public void CloseSocket()
		{
			if (this.socket != null)
			{
				this.socket.Close();
				this.socket = null;
			}
		}

		// Token: 0x060060CE RID: 24782 RVA: 0x0019B5F8 File Offset: 0x001997F8
		public void ResetSocket()
		{
			try
			{
				ManagerCenter.Instance.GetManager<LuaNetWorkManager>().ResetCache();
				this.receivedQueue.Clear();
				this.sendActions.Clear();
				this.SendBuffer.clear();
				this.SendBuffer.position(0);
				this.ReceiveBuffer.clear();
				this.ReceiveBuffer.position(0);
				this.encryptremainsize = 0;
				this.lastcmdlength = 0;
				this.blockingResponseMsgID = 0;
				this.lastBlockingRequestMsgID = 0;
				this.DecompressBuffer.clear();
				this.sleepSwitch = true;
				this.whetherCacheMsg = false;
				this.finishCatch = false;
				this.afterCreatMainPlayerNeedCloseLoading = false;
				this.NeedReconnect = false;
				this.cmdHeadInfo = null;
				if (this.MsgThread != null)
				{
					this.MsgThread.Interrupt();
					this.MsgThread.Join();
				}
				if (this.socket != null)
				{
					if (this.socket.Connected)
					{
						this.socket.Shutdown(SocketShutdown.Both);
						this.socket.Disconnect(true);
					}
					this.sleepSwitch = true;
				}
			}
			catch (Exception ex)
			{
				Debug.LogError(string.Concat(new object[]
				{
					"连接断开异常 Thread ：",
					Thread.CurrentThread.GetHashCode(),
					"   ",
					ex.ToString()
				}));
			}
		}

		// Token: 0x060060CF RID: 24783 RVA: 0x0019B754 File Offset: 0x00199954
		public void Dispose()
		{
			Debug.LogWarning("Dispose USocket");
			this.sleepSwitch = true;
			this.ResetSocket();
			this.onClose = null;
			this.onError = null;
			this.onConnect = null;
			this.onConnectFail = null;
			this.receiveBuffer = null;
			this.NeedReconnect = false;
			this.ReceiveCmdQueue.Clear();
			this.blockingState = BlockingState.Close;
		}

		// Token: 0x060060D0 RID: 24784 RVA: 0x0019B7B8 File Offset: 0x001999B8
		private void BlockingDelay()
		{
			if (this.lastSendTick != 0L)
			{
				long num = DateTime.Now.Ticks - this.lastSendTick;
				if (num > 70000000L)
				{
					this.blockingState = BlockingState.Exception;
					this.lastSendTick = 0L;
					return;
				}
				if (num > 20000000L)
				{
					this.blockingState = BlockingState.TimeOut;
				}
			}
		}

		// Token: 0x060060D1 RID: 24785 RVA: 0x0019B80C File Offset: 0x00199A0C
		public void ReceiveBufferManager()
		{
			if (this.whetherCacheMsg && !this.finishCatch)
			{
				return;
			}
			bool flag = false;
			while (this.receivedQueue.Count > 0)
			{
				if (this.whetherCacheMsg && !this.finishCatch)
				{
					flag = true;
					break;
				}
				object obj;
				if (this.finishCatch)
				{
					obj = USocket.receivelock;
					lock (obj)
					{
						this.catchedList.Clear();
						this.catchedList.AddRange(this.receivedQueue);
						this.receivedQueue.Clear();
						int num = -1;
						for (int i = 0; i < this.catchedList.Count; i++)
						{
							if (this.catchedList[i].Msgid == 2261)
							{
								num = i;
							}
						}
						for (int j = 0; j < this.catchedList.Count; j++)
						{
							if (num == j)
							{
								this.afterCreatMainPlayerNeedCloseLoading = true;
							}
							NullCmd vcmd = this.catchedList[j];
							this.DoMSG(vcmd);
						}
						if (num == -1)
						{
							UI_Loading.EndLoading();
						}
						this.catchedList.Clear();
						break;
					}
				}
				obj = USocket.receivelock;
				lock (obj)
				{
					NullCmd vcmd2 = this.receivedQueue.Dequeue();
					this.DoMSG(vcmd2);
				}
			}
			if (!flag && this.finishCatch)
			{
				this.whetherCacheMsg = false;
				this.finishCatch = false;
			}
			if (this.NeedReconnect)
			{
				this.NeedReconnect = false;
				LSingleton<NetWorkModule>.Instance.Reconnect();
			}
			this.BlockingDelay();
		}

		// Token: 0x060060D2 RID: 24786 RVA: 0x0019B9A4 File Offset: 0x00199BA4
		private void DoMSG(NullCmd vcmd)
		{
			if ((int)vcmd.Msgid == this.blockingResponseMsgID)
			{
				this.blockingState = BlockingState.Sendable;
				this.blockingResponseMsgID = 0;
				this.lastSendTick = 0L;
			}
			if (ManagerCenter.Instance.GetManager<LuaNetWorkManager>().IsRegisterInLua(vcmd.Msgid))
			{
				try
				{
					this.DumpPacket(vcmd.Msgid, vcmd.Data);
				}
				catch
				{
				}
				ManagerCenter.Instance.GetManager<LuaNetWorkManager>().OnMessage(vcmd);
			}
			if (LSingleton<NetWorkModule>.Instance.ProtoCallbackDict.ContainsKey(vcmd.Msgid))
			{
				try
				{
					this.DumpPacket(vcmd.Msgid, vcmd.Data);
				}
				catch
				{
				}
				LSingleton<NetWorkModule>.Instance.ProtoCallbackDict[vcmd.Msgid](vcmd.Data);
			}
			if (LSingleton<NetWorkModule>.Instance.StructCallbackDict.ContainsKey(vcmd.Msgid))
			{
				try
				{
					this.DumpPacket(vcmd.Msgid, vcmd.Data);
				}
				catch
				{
				}
				LSingleton<NetWorkModule>.Instance.StructCallbackDict[vcmd.Msgid](vcmd.Data);
			}
		}

		// Token: 0x060060D3 RID: 24787 RVA: 0x0003B21A File Offset: 0x0003941A
		public bool GetSocketState()
		{
			return this.socket != null && this.socket.Connected;
		}

		// Token: 0x060060D4 RID: 24788 RVA: 0x0003B231 File Offset: 0x00039431
		public void BeginCacheMsg()
		{
			this.whetherCacheMsg = true;
			this.finishCatch = false;
			this.afterCreatMainPlayerNeedCloseLoading = false;
		}

		// Token: 0x17000FB7 RID: 4023
		// (get) Token: 0x060060D5 RID: 24789 RVA: 0x0003B248 File Offset: 0x00039448
		public bool isOnCaching
		{
			get
			{
				return this.whetherCacheMsg;
			}
		}

		// Token: 0x060060D6 RID: 24790 RVA: 0x0003B250 File Offset: 0x00039450
		public void FinishCacheMsg()
		{
			this.finishCatch = true;
		}

		// Token: 0x060060D7 RID: 24791 RVA: 0x0019BAD8 File Offset: 0x00199CD8
		public void DumpPacket(ushort num2, object data)
		{
			if (data == null)
			{
				return;
			}
			string text = "";
			text += this.ProcessDumpPacket(data);
			File.WriteAllText(string.Concat(new object[]
			{
				"./dump/",
				(CommandID)num2,
				"_",
				Convert.ToString((int)DateTime.Now.Subtract(new DateTime(1970, 1, 1)).TotalSeconds)
			}), text);
		}

		// Token: 0x060060DB RID: 24795 RVA: 0x0019BE4C File Offset: 0x0019A04C
		public string ProcessDumpPacket(object obj)
		{
			if (obj == null)
			{
				return Environment.NewLine;
			}
			string text = "";
			Type type = obj.GetType();
			foreach (PropertyInfo propertyInfo in type.GetProperties())
			{
				if (type == typeof(string))
				{
					return Environment.NewLine;
				}
				object obj2;
				if (propertyInfo.PropertyType.IsArray)
				{
					obj2 = (Array)propertyInfo.GetValue(obj, null);
				}
				else
				{
					obj2 = propertyInfo.GetValue(obj, null);
				}
				IList list = obj2 as IList;
				if (list != null)
				{
					text += propertyInfo.Name;
					text = text + " IList of " + obj2.GetType().Name + Environment.NewLine;
					for (int j = 0; j < list.Count; j++)
					{
						text = string.Concat(new object[]
						{
							text,
							Environment.NewLine,
							propertyInfo.Name,
							"[",
							j.ToString(),
							"]: ",
							list[j],
							Environment.NewLine
						});
						if (type != typeof(string))
						{
							text += Environment.NewLine;
							text = text + Environment.NewLine + this.ProcessDumpPacket(list[j]) + Environment.NewLine;
						}
					}
				}
				else if (propertyInfo.PropertyType.Assembly == type.Assembly)
				{
					text = text + Environment.NewLine + propertyInfo.Name;
					this.ProcessDumpPacket(obj2);
				}
				else
				{
					text = string.Concat(new object[]
					{
						text,
						Environment.NewLine,
						propertyInfo.Name,
						":",
						obj2
					});
				}
			}
			return text;
		}

		// Token: 0x060060DC RID: 24796 RVA: 0x0019C014 File Offset: 0x0019A214
		public void DumpSendPacket(ushort num2, object data)
		{
			if (data == null)
			{
				return;
			}
			string text = "";
			text += this.ProcessDumpPacket(data);
			File.WriteAllText(string.Concat(new object[]
			{
				"./dump/",
				"send_",
				(CommandID)num2,
				"_",
				Convert.ToString((int)DateTime.Now.Subtract(new DateTime(1970, 1, 1)).TotalSeconds)
			}), text);
		}

		// Token: 0x04003FBD RID: 16317
		public const int BUFFER_LENGTH = 65536;

		// Token: 0x04003FBE RID: 16318
		public const int DEFAULT_IO_BUFFERLENGTH = 32768;

		// Token: 0x04003FBF RID: 16319
		public const int SOCKET_THREAD_SLEEP_INTERVAL = 10;

		// Token: 0x04003FC0 RID: 16320
		public USocketType type;

		// Token: 0x04003FC1 RID: 16321
		private Socket socket;

		// Token: 0x04003FC2 RID: 16322
		private byte[] receiveBuffer;

		// Token: 0x04003FC3 RID: 16323
		private OnSocketCallback onConnect;

		// Token: 0x04003FC4 RID: 16324
		private OnSocketCallback onConnectFail;

		// Token: 0x04003FC5 RID: 16325
		private OnSocketCallback onClose;

		// Token: 0x04003FC6 RID: 16326
		private OnSocketCallback onError;

		// Token: 0x04003FC7 RID: 16327
		private Queue<NullCmd> ReceiveCmdQueue = new Queue<NullCmd>();

		// Token: 0x04003FC8 RID: 16328
		public OctetsStream SendBuffer = new OctetsStream(65536);

		// Token: 0x04003FC9 RID: 16329
		public OctetsStream ReceiveBuffer = new OctetsStream(65536);

		// Token: 0x04003FCA RID: 16330
		public OctetsStream DecompressBuffer = new OctetsStream();

		// Token: 0x04003FCB RID: 16331
		private int blockingResponseMsgID;

		// Token: 0x04003FCC RID: 16332
		private int lastBlockingRequestMsgID;

		// Token: 0x04003FCD RID: 16333
		private BlockingState blockingState;

		// Token: 0x04003FCE RID: 16334
		private IPEndPoint ipEndPoint;

		// Token: 0x04003FCF RID: 16335
		private int _reconnecttimes;

		// Token: 0x04003FD0 RID: 16336
		private long lastSendTick;

		// Token: 0x04003FD1 RID: 16337
		public bool afterCreatMainPlayerNeedCloseLoading;

		// Token: 0x04003FD2 RID: 16338
		private bool whetherCacheMsg;

		// Token: 0x04003FD3 RID: 16339
		private bool finishCatch;

		// Token: 0x04003FD4 RID: 16340
		public System.Action ResetScene;

		// Token: 0x04003FD5 RID: 16341
		private Thread MsgThread;

		// Token: 0x04003FD6 RID: 16342
		private bool NeedReconnect;

		// Token: 0x04003FD7 RID: 16343
		private static readonly object sendlock = new object();

		// Token: 0x04003FD8 RID: 16344
		private static readonly object receivelock = new object();

		// Token: 0x04003FD9 RID: 16345
		private ManualResetEvent alldone = new ManualResetEvent(false);

		// Token: 0x04003FDA RID: 16346
		private Queue<NullCmd> receivedQueue = new Queue<NullCmd>();

		// Token: 0x04003FDB RID: 16347
		private List<NullCmd> catchedList = new List<NullCmd>();

		// Token: 0x04003FDC RID: 16348
		private bool logMsg;

		// Token: 0x04003FDD RID: 16349
		private int lastcmdlength;

		// Token: 0x04003FDE RID: 16350
		private Queue<System.Action> sendActions = new Queue<System.Action>();

		// Token: 0x04003FDF RID: 16351
		private bool sleepSwitch;

		// Token: 0x04003FE0 RID: 16352
		private int encryptremainsize;

		// Token: 0x04003FE1 RID: 16353
		private CmdHead cmdHeadInfo;

		// Token: 0x04003FE2 RID: 16354
		private string MsgLog = "msg-->";
	}
}
