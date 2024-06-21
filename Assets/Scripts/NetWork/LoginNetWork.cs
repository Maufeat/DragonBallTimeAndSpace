using System;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using Framework.Managers;
using Game.Scene;
using LuaInterface;
using msg;
using Net;
using UnityEngine;

// Token: 0x02000726 RID: 1830
public class LoginNetWork : NetWorkBase
{
	// Token: 0x0600357B RID: 13691 RVA: 0x0002156A File Offset: 0x0001F76A
	public override void Initialize()
	{
		LoginNetWork.m_phoneinfo = new stPhoneInfo();
		this.RegisterMsg();
	}

	// Token: 0x0600357C RID: 13692 RVA: 0x000AD538 File Offset: 0x000AB738
	public void ParseFLEndpoint(string str)
	{
		if (string.IsNullOrEmpty(str))
		{
			return;
		}
		this.FLIPEndPoints.Clear();
		this.ConnectFlTimes = 0;
		string[] array = str.Split(new char[]
		{
			':'
		}, StringSplitOptions.RemoveEmptyEntries);
		string[] array2 = array[0].Split(new char[]
		{
			','
		}, StringSplitOptions.RemoveEmptyEntries);
		string[] array3 = array[1].Split(new char[]
		{
			','
		}, StringSplitOptions.RemoveEmptyEntries);
		for (int i = 0; i < array2.Length; i++)
		{
			IPAddress ipaddress = IPAddress.Parse(array2[i].Trim());
			int num = int.Parse(array3[i]);
			FFDebug.Log(this, FFLogType.Network, ipaddress + "\u3000\u3000\u3000" + num);
			IPEndPoint item = new IPEndPoint(ipaddress, num % 65536);
			this.FLIPEndPoints.Add(item);
		}
		IPEndPoint ipendPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 60222);
		this.FLIPEndPoints.Clear();
		this.FLIPEndPoints.Add(ipendPoint);
		this.CurrentIpEndPoint = ipendPoint;
	}

	// Token: 0x0600357D RID: 13693 RVA: 0x000AD63C File Offset: 0x000AB83C
	public void ConnectFirServer(IPEndPoint ipEndPoint, LoginType logintype, ushort zone, string pwd, ushort game, string uuid, ushort usertype, string account)
	{
		this.ConnectFirServer(ipEndPoint.Address.ToString(), ipEndPoint.Port, logintype, zone, pwd, game, uuid, usertype, account);
	}

	// Token: 0x0600357E RID: 13694 RVA: 0x000AD66C File Offset: 0x000AB86C
	public void ConnectFirServer(string firip, int firport, LoginType logintype, ushort zone, string pwd, ushort game, string uuid, ushort usertype, string account)
	{
		FFDebug.LogWarning(this, string.Concat(new object[]
		{
			"ConnectFirServer ",
			firip,
			",",
			firport,
			" ",
			DateTime.Now
		}));
		try
		{
			this.loginType = logintype;
			this.zone = zone;
			this.pwd = pwd;
			this.game = game;
			this.uuid = uuid;
			this.userType = usertype;
			this.account = account;
			LSingleton<NetWorkModule>.Instance.Connect(new IPEndPoint(IPAddress.Parse(firip), firport), USocketType.Fir, new OnSocketCallback(this.OnFirSocketConnect), new OnSocketCallback(this.OnFirSocketConnectFail));
		}
		catch (Exception ex)
		{
			FFDebug.LogError(this, "ConnectFirServer Exception " + ex.ToString());
		}
	}

	// Token: 0x0600357F RID: 13695 RVA: 0x0002157C File Offset: 0x0001F77C
	private void OnFirSocketConnect()
	{
		FFDebug.LogWarning(this, "OnFirSocketConnect");
		this.SendFirCheckVersion();
		this.SendRequestClientIP();
	}

	// Token: 0x06003580 RID: 13696 RVA: 0x000AD754 File Offset: 0x000AB954
	public void OnFirSocketConnectFail()
	{
		FFDebug.LogWarning(this, "OnFirSocketConnectFail");
		this.ConnectFlTimes++;
		if (this.ConnectFlTimes < this.FLIPEndPoints.Count)
		{
			this.CurrentIpEndPoint = this.FLIPEndPoints[this.ConnectFlTimes];
			LSingleton<NetWorkModule>.Instance.Reconnect();
		}
		else
		{
			FFDebug.LogWarning(this, "所有端口尝试失败，未完成连接");
			if (LSingleton<NetWorkModule>.Instance.IsReconnecting)
			{
				LSingleton<NetWorkModule>.Instance.IsReconnecting = false;
			}
			ControllerManager.Instance.GetController<LoadTipsController>().CloseReconnectTips();
			ControllerManager.Instance.GetController<MsgBoxController>().ShowMsgBox(MsgBoxController.MsgLevelSystemNotification, CommonUtil.GetText(dynamic_textid.ServerIDs.disconnect), MsgBoxController.MsgOptionConfirm, UIManager.ParentType.Loading, delegate ()
			{
			}, null);
			this.ConnectFlTimes = 0;
		}
	}

	// Token: 0x06003581 RID: 13697 RVA: 0x000AD830 File Offset: 0x000ABA30
	private void OnLoginFirError(stServerReturnLoginFailedCmd cmd)
	{
		string errorContent = this.GetErrorContent((uint)cmd.byReturnCode);
		LSingleton<NetWorkModule>.Instance.MainSocket.Dispose();
		ControllerManager.Instance.GetController<LoadTipsController>().CloseReconnectTips();
		ControllerManager.Instance.GetController<MsgBoxController>().ShowMsgBox(MsgBoxController.MsgLevelSystemNotification, errorContent, MsgBoxController.MsgOptionConfirm, UIManager.ParentType.Loading, delegate ()
		{
		}, null);
		FFDebug.LogWarning(this, "Login Fir Failed : " + cmd.data);
	}

	// Token: 0x06003582 RID: 13698 RVA: 0x00021595 File Offset: 0x0001F795
	private void onServerReturnClientIP(stServerReturnClientIPCmd data)
	{
		FFDebug.LogWarning(this, "onServerReturnClientIP " + data.pstrIP);
        Debug.Log("onServerReturnClientIP " + data.pstrIP);
        this.localWanIp = data.pstrIP;
		this.SendFirP2PLogin();
	}

	// Token: 0x06003583 RID: 13699 RVA: 0x000215BF File Offset: 0x0001F7BF
	private void onLoginFirSuccess(stServerReturnLoginSuccessCmd data)
	{
		this.ConnnectGateWay();
	}

	// Token: 0x06003584 RID: 13700 RVA: 0x000215C7 File Offset: 0x0001F7C7
	private void onLoginP2PFirSuccess(stServerReturnP2PLoginSuccessCmd data)
	{
		FFDebug.LogWarning(this, "onLoginP2PFirSuccess");
		this.gatewayData = data;
		this.ConnnectGateWay();
	}

	// Token: 0x06003585 RID: 13701 RVA: 0x000AD8B8 File Offset: 0x000ABAB8
	private void onP2PLoginFirFailed(stServerP2PReturnLoginFailedCmd data)
	{
		FFDebug.Log(this, FFLogType.Login, "onP2PLoginFirFailed" + data.returnCode.ToString());
		string errorContent = this.GetErrorContent((uint)data.returnCode);
		LSingleton<NetWorkModule>.Instance.MainSocket.Dispose();
		ControllerManager.Instance.GetController<MsgBoxController>().ShowMsgBox(MsgBoxController.MsgLevelSystemNotification, errorContent, MsgBoxController.MsgOptionRetry, MsgBoxController.MsgOptionLeave, UIManager.ParentType.Loading, delegate ()
		{
			FFDebug.LogWarning(this, "Re Reconnect " + DateTime.Now);
			LSingleton<NetWorkModule>.Instance.Reconnect();
		}, delegate ()
		{
			ManagerCenter.Instance.GetManager<GameMainManager>().QuitGame();
		}, null);
		FFDebug.LogWarning(this, "Login Fir Failed : " + data.error);
	}

	// Token: 0x06003586 RID: 13702 RVA: 0x000AD964 File Offset: 0x000ABB64
	private void SendFirCheckVersion()
	{
		stUserVerifyP2PVerCmd stUserVerifyP2PVerCmd = new stUserVerifyP2PVerCmd();
		stUserVerifyP2PVerCmd.version = 2000U;
		base.SendMsg(stUserVerifyP2PVerCmd, false);
		FFDebug.LogWarning(this, string.Concat(new object[]
		{
			"SendFirCheckVersion ",
			stUserVerifyP2PVerCmd.MsgCmd,
			" ",
			stUserVerifyP2PVerCmd.MsgParam,
			" ",
			stUserVerifyP2PVerCmd.Msgid
		}));
	}

	// Token: 0x06003587 RID: 13703 RVA: 0x000AD9E0 File Offset: 0x000ABBE0
	private void SendRequestClientIP()
	{
		stUserRequestIPCmd stUserRequestIPCmd = new stUserRequestIPCmd();
		base.SendMsg(stUserRequestIPCmd, false);
		FFDebug.LogWarning(this, string.Concat(new object[]
		{
			"SendRequestClientIP ",
			stUserRequestIPCmd.MsgCmd,
			" ",
			stUserRequestIPCmd.MsgParam,
			" ",
			stUserRequestIPCmd.Msgid
		}));
	}

	// Token: 0x06003588 RID: 13704 RVA: 0x000ADA50 File Offset: 0x000ABC50
	private void SendFirP2PLogin()
	{
		if (this.loginType == LoginType.UUID)
		{
			stRequestP2PLoginCmd stRequestP2PLoginCmd = new stRequestP2PLoginCmd();
			stRequestP2PLoginCmd.pstrName = this.account;
			byte[] bytes = Encoding.UTF8.GetBytes(this.localWanIp);
			byte[] destinationArray = new byte[18];
			Array.Copy(bytes, destinationArray, bytes.Length);
			stRequestP2PLoginCmd.pstrPassword = new byte[33];
			byte[] bytes2 = Encoding.UTF8.GetBytes(this.pwd);
			byte[] array = new byte[48];
			Array.Copy(bytes2, array, bytes2.Length);
			Array.Copy(array, stRequestP2PLoginCmd.pstrPassword, 33);
			stRequestP2PLoginCmd.uuid = new byte[25];
			string text = SystemInfo.deviceUniqueIdentifier;
			if (text.Length > 16)
			{
				text = text.Substring(0, 16);
			}
			byte[] bytes3 = Encoding.UTF8.GetBytes(text);
			byte[] array2 = new byte[32];
			Array.Copy(bytes3, array2, bytes3.Length);
			Array.Copy(array2, stRequestP2PLoginCmd.uuid, 25);
			stRequestP2PLoginCmd.game = 52;
			stRequestP2PLoginCmd.zone = this.zone;
			stRequestP2PLoginCmd.jpegPassport = string.Empty;
			stRequestP2PLoginCmd.maxAddr = this.getMacAddr();
			FFDebug.LogWarning("Send Login Cmd", SystemInfo.deviceUniqueIdentifier + "," + stRequestP2PLoginCmd.uuid);
			stRequestP2PLoginCmd.wdNetType = 0;
			stRequestP2PLoginCmd.passpodPwd = string.Empty;
			stRequestP2PLoginCmd.userType = 1;
			base.SendMsg(stRequestP2PLoginCmd, false);
		}
	}

	// Token: 0x06003589 RID: 13705 RVA: 0x000ADBA8 File Offset: 0x000ABDA8
	private string getMacAddr()
	{
		string text = string.Empty;
		NetworkInterface[] allNetworkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
		foreach (NetworkInterface networkInterface in allNetworkInterfaces)
		{
			text = networkInterface.GetPhysicalAddress().ToString();
			if (!string.IsNullOrEmpty(text))
			{
				return text;
			}
		}
		return string.Empty;
	}

	// Token: 0x0600358A RID: 13706 RVA: 0x000ADC00 File Offset: 0x000ABE00
	private void SendFirLogin()
	{
		if (this.loginType == LoginType.UUID)
		{
			base.SendMsg(new stIphoneUserRequestLoginCmd
			{
				userType = this.userType,
				account = this.account,
				game = 0,
				zone = this.zone,
				wdNetType = 0,
				uid = 0UL,
				token = string.Empty,
				phone_uuid = this.uuid
			}, false);
			FFDebug.Log(this, FFLogType.Login, "SendFirLogin");
		}
	}

	// Token: 0x0600358B RID: 13707 RVA: 0x000ADC8C File Offset: 0x000ABE8C
	public void ConnnectGateWay()
	{
		IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Parse(this.gatewayData.pstrIP), (int)this.gatewayData.wdPort);
		LSingleton<NetWorkModule>.Instance.Connect(ipEndPoint, USocketType.GateWay, new OnSocketCallback(this.OnGateWayConnect), new OnSocketCallback(this.OnGateWayConnectFail));
	}

	// Token: 0x0600358C RID: 13708 RVA: 0x000ADCE0 File Offset: 0x000ABEE0
	private void OnGateWayConnect()
	{
		FFDebug.LogWarning("OnGateWayConnect", string.Concat(new object[]
		{
			"连接网关",
			this.gatewayData.pstrIP,
			": ",
			this.gatewayData.wdPort,
			"成功"
		}));
		if (LSingleton<NetWorkModule>.Instance.IsReconnecting)
		{
			LSingleton<NetWorkModule>.Instance.IsReconnecting = false;
			ControllerManager.Instance.GetController<LoadTipsController>().CloseReconnectTips();
		}
		if (Host.WithoutPatch)
		{
			FFDebug.LogWarning("OnGateWayConnect WithoutPatch", "发送网关登录请求");
			this.SendGateWayCheckVersion();
			this.SendGateWayLogin();
			SingletonForMono<GameTime>.Instance.CheckPing = true;
		}
		else
		{
			PatchController controller = ControllerManager.Instance.GetController<PatchController>();
			if (controller != null)
			{
				controller.Send(delegate
				{
					this.SendGateWayCheckVersion();
					this.SendGateWayLogin();
					SingletonForMono<GameTime>.Instance.CheckPing = true;
				});
			}
			else
			{
				FFDebug.LogWarning(this, "patchController is null!");
			}
		}
	}

	// Token: 0x0600358D RID: 13709 RVA: 0x000ADDCC File Offset: 0x000ABFCC
	private void OnGateWayConnectFail()
	{
		FFDebug.LogWarning(this, string.Concat(new object[]
		{
			"连接网关",
			this.gatewayData.pstrIP,
			": ",
			this.gatewayData.wdPort,
			"失败"
		}));
		LSingleton<NetWorkModule>.Instance.IsReconnecting = false;
		LSingleton<NetWorkModule>.Instance.Reconnect();
	}

	// Token: 0x0600358E RID: 13710 RVA: 0x000ADE38 File Offset: 0x000AC038
	private void SendGateWayCheckVersion()
	{
		stUserVerifyVerCmd_CS stUserVerifyVerCmd_CS = new stUserVerifyVerCmd_CS();
		stUserVerifyVerCmd_CS.reserve = 0U;
		stUserVerifyVerCmd_CS.version = 1999U;
		base.SendMsg(stUserVerifyVerCmd_CS, false);
		FFDebug.Log(this, FFLogType.Login, "SendGateWayCheckVersion " + stUserVerifyVerCmd_CS.version);
	}

	// Token: 0x0600358F RID: 13711 RVA: 0x000ADE88 File Offset: 0x000AC088
	private void SendGateWayLogin()
	{
		base.SendMsg<MSG_Ret_UserMapInfo_SC>(CommandID.MSG_Ret_UserMapInfo_SC, new MSG_Ret_UserMapInfo_SC
		{
			mapid = 695U,
			filename = "toto",
			copymapidx = 123U,
			lineid = 64U,
			mapname = "toto",
			pos = new FloatMovePos(),
			sceneid = 864UL,
			subcopymapidx = 486U
		}, false);
		base.SendMsg(new stIphoneLoginUserCmd_CS
		{
			accid = this.gatewayData.dwUserID,
			user_type = this.userType,
			loginTempID = this.gatewayData.loginTempID,
			account = this.account,
			password = this.pwd,
			szMAC = this.getMacAddr(),
			szFlat = "iphone",
			phone = new stPhoneInfo()
		}, false);
		FFDebug.LogWarning("SendGateWayLogin", "发送登录请求到网关");
	}

	// Token: 0x06003590 RID: 13712 RVA: 0x000ADF7C File Offset: 0x000AC17C
	private void OnGateWayLoginFail(stServerReturnLoginFailedCmd_SC cmd)
	{
		string errorContent = this.GetErrorContent((uint)cmd.byReturnCode);
		ControllerManager.Instance.GetController<MsgBoxController>().ShowMsgBox(MsgBoxController.MsgLevelGetway, errorContent, MsgBoxController.MsgOptionConfirm, UIManager.ParentType.Loading, delegate ()
		{
			ManagerCenter.Instance.GetManager<GameMainManager>().QuitGame();
		}, null);
	}

	// Token: 0x17000C32 RID: 3122
	// (get) Token: 0x06003591 RID: 13713 RVA: 0x0001B65D File Offset: 0x0001985D
	private GameScene gameScene
	{
		get
		{
			return ManagerCenter.Instance.GetManager<GameScene>();
		}
	}

	// Token: 0x06003592 RID: 13714 RVA: 0x000ADFD0 File Offset: 0x000AC1D0
	private void OnUserMapInfo(MSG_Ret_UserMapInfo_SC mdata)
	{
		FFDebug.Log(this, FFLogType.Login, string.Concat(new object[]
		{
			"OnUserMapInfo------>",
			mdata.filename,
			" ",
			mdata.copymapidx
		}));
		UIManager.Instance.DeleteUI<UI_SelectRole>();
		UIManager.Instance.OnMapChangeCloseUI();
		PatchController controller = ControllerManager.Instance.GetController<PatchController>();
		if (controller != null)
		{
			controller.CloseVersionText();
		}
		SceneInfo sceneInfo = new SceneInfo();
		sceneInfo.mapid = mdata.mapid;
		sceneInfo.mapname = mdata.mapname;
		sceneInfo.lineid = mdata.lineid;
		sceneInfo.pos = mdata.pos;
		sceneInfo.sceneid = mdata.sceneid;
		sceneInfo.copymapid = mdata.subcopymapidx;
		if (MainPlayer.Self != null)
		{
			cs_MoveData cs_MoveData = new cs_MoveData();
			cs_MoveData.pos = new cs_FloatMovePos(mdata.pos.fx, mdata.pos.fy);
			cs_MoveData.dir = 0U;
			MainPlayer.Self.RecodeSeverMoveData(cs_MoveData);
		}
		LSingleton<NetWorkModule>.Instance.BeginCacheMsg();
		this.gameScene.CheckSameScene(sceneInfo);
		if (this.gameScene.bSameMapID)
		{
			LuaScriptMgr.Instance.CallLuaFunction("InstanceEffectCtrl.OpenUI", new object[]
			{
				Util.GetLuaTable("InstanceEffectCtrl")
			});
			Scheduler.Instance.AddTimer(1.6f, false, delegate
			{
				ManagerCenter.Instance.GetManager<EntitiesManager>().UnLoadCharactors();
				SingletonForMono<InputController>.Instance.Init();
				this.CopyMapidChange(mdata.copymapidx, mdata.subcopymapidx);
				ControllerManager.Instance.GetController<MainUIController>().LoadMainView(delegate
				{
					ControllerManager.Instance.GetController<UIMapController>().LoadView(delegate
					{
						LSingleton<NetWorkModule>.Instance.FinishCacheMsg();
						this.gameScene.OnSceneLoadNotifyServer();
						MSG_UserLoadingOk_CS t = new MSG_UserLoadingOk_CS();
						this.SendMsg<MSG_UserLoadingOk_CS>(CommandID.MSG_UserLoadingOk_CS, t, false);
						ShortcutsConfigController.isInGameSceneMap = true;
					});
				});
				if (MainPlayer.Self != null)
				{
					PlayerBufferControl component = MainPlayer.Self.GetComponent<PlayerBufferControl>();
					component.OnSceneChange();
				}
				if (sceneInfo.copymapid == 903U)
				{
					TipsWindow.ShowWindow(5007U);
				}
			});
		}
		else if (this.gameScene.bSameScene)
		{
			LuaScriptMgr.Instance.CallLuaFunction("InstanceEffectCtrl.OpenUI", new object[]
			{
				Util.GetLuaTable("InstanceEffectCtrl")
			});
			Scheduler.Instance.AddTimer(1.6f, false, delegate
			{
				this.gameScene.ChangeScene(sceneInfo, delegate
				{
					SingletonForMono<InputController>.Instance.Init();
					this.CopyMapidChange(mdata.copymapidx, mdata.subcopymapidx);
					ControllerManager.Instance.GetController<MainUIController>().LoadMainView(delegate
					{
						ControllerManager.Instance.GetController<UIMapController>().LoadView(delegate
						{
							LSingleton<NetWorkModule>.Instance.FinishCacheMsg();
							this.gameScene.OnSceneLoadNotifyServer();
							FFDebug.LogWarning("OnUserMapInfo bSameScene", "关闭创建角色UI");
							ControllerManager.Instance.GetLoginController().Close();
							MSG_UserLoadingOk_CS t = new MSG_UserLoadingOk_CS();
							this.SendMsg<MSG_UserLoadingOk_CS>(CommandID.MSG_UserLoadingOk_CS, t, false);
							ShortcutsConfigController.isInGameSceneMap = true;
						});
					});
					if (MainPlayer.Self != null)
					{
						PlayerBufferControl component = MainPlayer.Self.GetComponent<PlayerBufferControl>();
						component.OnSceneChange();
					}
					if (sceneInfo.copymapid == 903U)
					{
						TipsWindow.ShowWindow(5007U);
					}
				}, null);
			});
		}
		else
		{
			UI_Loading.StartLoading(sceneInfo);
			Scheduler.Instance.AddTimer(0.5f, false, delegate
			{
				ControllerManager.Instance.GetController<UIMapController>().onMapShow = null;
				ControllerManager.Instance.GetController<MainUIController>().onMainShow = null;
				this.gameScene.ChangeScene(sceneInfo, delegate
				{
					SingletonForMono<InputController>.Instance.Init();
					this.CopyMapidChange(mdata.copymapidx, mdata.subcopymapidx);
					ControllerManager.Instance.GetController<MainUIController>().LoadMainView(delegate
					{
						ControllerManager.Instance.GetController<UIMapController>().LoadView(delegate
						{
							LSingleton<NetWorkModule>.Instance.FinishCacheMsg();
							this.gameScene.OnSceneLoadNotifyServer();
							FFDebug.LogWarning("OnUserMapInfo", "关闭创建角色UI");
							ControllerManager.Instance.GetLoginController().Close();
							ShortcutsConfigController.isInGameSceneMap = true;
						});
					});
					if (MainPlayer.Self != null)
					{
						PlayerBufferControl component = MainPlayer.Self.GetComponent<PlayerBufferControl>();
						component.OnSceneChange();
					}
					if (sceneInfo.copymapid == 903U)
					{
						TipsWindow.ShowWindow(5007U);
					}
				}, new Action<float>(UI_Loading.SetLoadingProgress));
			});
		}
	}

	// Token: 0x06003593 RID: 13715 RVA: 0x000AE230 File Offset: 0x000AC430
	private void CopyMapidChange(uint copyid, uint copychildnode)
	{
		CopyManager manager = ManagerCenter.Instance.GetManager<CopyManager>();
		manager.SetCurrentCopy(copyid, copychildnode);
		LuaScriptMgr.Instance.CallLuaFunction("UIMapCtrl.SetLineActive", new object[]
		{
			Util.GetLuaTable("UIMapCtrl"),
			!manager.InCopy
		});
		ControllerManager.Instance.GetController<TaskUIController>().CkeckCopyTaskList();
		ControllerManager.Instance.GetController<MainUIController>().CheckOpenCompetition();
	}

	// Token: 0x06003594 RID: 13716 RVA: 0x000AE2A4 File Offset: 0x000AC4A4
	public void SendRegisterPlayer(string name, byte sex, uint professional)
	{
		base.SendMsg(new stCreateNewRoleUserCmd_CS
		{
			strRoleName = name,
			bySex = sex,
			flatid = 10U,
			phone = LoginNetWork.m_phoneinfo,
			heroid = professional
		}, false);
	}

	// Token: 0x06003595 RID: 13717 RVA: 0x000AE2E8 File Offset: 0x000AC4E8
	public void SendRegisterPlayer(string name, uint occupation)
	{
		FFDebug.Log(this, FFLogType.Network, string.Format("SendRegisterPlayer name :{0} occupation{1}", name, occupation));
		base.SendMsg<MSG_Create_Role_CS>(CommandID.MSG_Create_Role_CS, new MSG_Create_Role_CS
		{
			name = name,
			occupation = occupation
		}, false);
	}

	// Token: 0x06003596 RID: 13718 RVA: 0x000AE334 File Offset: 0x000AC534
	public void OnServerLoginFailed(MSG_Ret_ServerLoginFailed_SC data)
	{
		uint returncode = data.returncode;
		if (returncode == 6U)
		{
			LSingleton<NetWorkModule>.Instance.Reconnect();
		}
		string errorContent = this.GetErrorContent(data.returncode);
		TipsWindow.ShowWindow(errorContent);
		FFDebug.LogWarning(this, returncode + ">>>" + errorContent);
	}

	// Token: 0x06003597 RID: 13719 RVA: 0x000AE388 File Offset: 0x000AC588
	public void OnNotifyUserKickout(MSG_Ret_NotifyUserKickout_SC data)
	{
		this.Kickouted = true;
		TipsWindow.ShowWindow(CommonUtil.GetText(dynamic_textid.ServerIDs.kickout));
		ControllerManager.Instance.GetController<MsgBoxController>().ShowMsgBox(MsgBoxController.MsgLevelSystem, CommonUtil.GetText(dynamic_textid.ServerIDs.kickout), MsgBoxController.MsgOptionConfirm, UIManager.ParentType.Loading, delegate ()
		{
			Application.Quit();
		}, null);
	}

	// Token: 0x06003598 RID: 13720 RVA: 0x000AE3E8 File Offset: 0x000AC5E8
	public void OnNotifyAccountReuse(MSG_NotifyAccountReuse data)
	{
		ControllerManager.Instance.GetController<MsgBoxController>().ShowMsgBox(MsgBoxController.MsgLevelSystem, CommonUtil.GetText(dynamic_textid.ServerIDs.other_people_login), MsgBoxController.MsgOptionConfirm, UIManager.ParentType.Loading, delegate ()
		{
		}, null);
	}

	// Token: 0x06003599 RID: 13721 RVA: 0x000215E1 File Offset: 0x0001F7E1
	public void OnErrorOperationUserCmd(stRetErrorOperationUserCmd_SC data)
	{
		FFDebug.LogWarning(this, "Process cmd Error : " + data.cmd_id);
	}

	// Token: 0x0600359A RID: 13722 RVA: 0x000215FE File Offset: 0x0001F7FE
	public void GetServerTime(MSG_Ret_ServerTime_SC data)
	{
		SingletonForMono<GameTime>.Instance.SetServerTime(data.servertime);
	}

	// Token: 0x0600359B RID: 13723 RVA: 0x000AE434 File Offset: 0x000AC634
	public override void RegisterMsg()
	{
		UnRegisterMsg();
		LSingleton<NetWorkModule>.Instance.RegisterStructMsg(USocket.GetMsgId(104, 3), new StructMsgCallback<stServerReturnLoginFailedCmd>(OnLoginFirError));
		LSingleton<NetWorkModule>.Instance.RegisterStructMsg(USocket.GetMsgId(104, 4), new StructMsgCallback<stServerReturnP2PLoginSuccessCmd>(onLoginP2PFirSuccess));
		LSingleton<NetWorkModule>.Instance.RegisterStructMsg(USocket.GetMsgId(104, 16), new StructMsgCallback<stServerReturnClientIPCmd>(onServerReturnClientIP));
		LSingleton<NetWorkModule>.Instance.RegisterStructMsg(2003, new StructMsgCallback<stServerReturnLoginFailedCmd_SC>(OnGateWayLoginFail));
		LSingleton<NetWorkModule>.Instance.RegisterProtoMsg(2273, new ProtoMsgCallback<MSG_Ret_UserMapInfo_SC>(OnUserMapInfo));
		LSingleton<NetWorkModule>.Instance.RegisterProtoMsg(2271, new ProtoMsgCallback<MSG_Ret_NotifyUserKickout_SC>(OnNotifyUserKickout));
		LSingleton<NetWorkModule>.Instance.RegisterProtoMsg(2272, new ProtoMsgCallback<MSG_Ret_ServerLoginFailed_SC>(OnServerLoginFailed));
		LSingleton<NetWorkModule>.Instance.RegisterProtoMsg(2270, new ProtoMsgCallback<MSG_Ret_ServerTime_SC>(GetServerTime));
		LSingleton<NetWorkModule>.Instance.RegisterStructMsg(2072, new StructMsgCallback<stRetErrorOperationUserCmd_SC>(OnErrorOperationUserCmd));
		LSingleton<NetWorkModule>.Instance.RegisterProtoMsg(2538, new ProtoMsgCallback<MSG_NotifyAccountReuse>(OnNotifyAccountReuse));
	}

	// Token: 0x0600359C RID: 13724 RVA: 0x000AE560 File Offset: 0x000AC760
	public override void UnRegisterMsg()
	{
		LSingleton<NetWorkModule>.Instance.DeRegisterStrucMsg(USocket.GetMsgId(104, 3));
		LSingleton<NetWorkModule>.Instance.DeRegisterStrucMsg(USocket.GetMsgId(104, 4));
		LSingleton<NetWorkModule>.Instance.DeRegisterStrucMsg(USocket.GetMsgId(104, 16));
		LSingleton<NetWorkModule>.Instance.DeRegisterStrucMsg(2003);
		LSingleton<NetWorkModule>.Instance.DeRegisterMsg(2273);
		LSingleton<NetWorkModule>.Instance.DeRegisterMsg(2271);
		LSingleton<NetWorkModule>.Instance.DeRegisterMsg(2272);
		LSingleton<NetWorkModule>.Instance.DeRegisterMsg(2270);
		LSingleton<NetWorkModule>.Instance.DeRegisterStrucMsg(2072);
		LSingleton<NetWorkModule>.Instance.DeRegisterMsg(2538);
	}

	// Token: 0x0600359D RID: 13725 RVA: 0x000AE610 File Offset: 0x000AC810
	private string GetErrorContent(uint errorCode)
	{
		errorCode += 300U;
		string text = string.Empty;
		LuaTable configTable = LuaConfigManager.GetConfigTable("textconfig", (ulong)errorCode);
		if (configTable != null)
		{
			text = configTable.GetField_String("tips");
		}
		if (string.IsNullOrEmpty(text))
		{
			text = "unknow error, code = " + errorCode;
		}
		return text;
	}

	// Token: 0x04001F7D RID: 8061
	private const uint Duoqi_CopyMapId = 903U;

	// Token: 0x04001F7E RID: 8062
	private const uint Duoqi_TipsId = 5007U;

	// Token: 0x04001F7F RID: 8063
	private LoginType loginType;

	// Token: 0x04001F80 RID: 8064
	private string uuid;

	// Token: 0x04001F81 RID: 8065
	private string pwd;

	// Token: 0x04001F82 RID: 8066
	private ushort game;

	// Token: 0x04001F83 RID: 8067
	private ushort zone;

	// Token: 0x04001F84 RID: 8068
	private ushort userType;

	// Token: 0x04001F85 RID: 8069
	private string account;

	// Token: 0x04001F86 RID: 8070
	public static stPhoneInfo m_phoneinfo;

	// Token: 0x04001F87 RID: 8071
	public List<IPEndPoint> FLIPEndPoints = new List<IPEndPoint>();

	// Token: 0x04001F88 RID: 8072
	public IPEndPoint CurrentIpEndPoint;

	// Token: 0x04001F89 RID: 8073
	public int ConnectFlTimes;

	// Token: 0x04001F8A RID: 8074
	public bool Kickouted;

	// Token: 0x04001F8B RID: 8075
	public stServerReturnP2PLoginSuccessCmd gatewayData;

	// Token: 0x04001F8C RID: 8076
	public string localWanIp = string.Empty;
}
