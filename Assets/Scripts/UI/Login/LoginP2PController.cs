using System;
using Models;

namespace UI.Login
{
	// Token: 0x02000724 RID: 1828
	public class LoginP2PController : LoginControllerBase
	{
		// Token: 0x06003570 RID: 13680 RVA: 0x000AD374 File Offset: 0x000AB574
		public override void LoginAwake()
		{
			try
			{
				if (this.loginNetWork == null)
				{
					this.loginNetWork = new LoginNetWork();
				}
				this.loginNetWork.Initialize();
				this.loginNetWork.ParseFLEndpoint(LoginControllerBase.cacheflserver);
				this.loginModel = new LoginModel();
			}
			catch (Exception arg)
			{
				FFDebug.LogError(this, "Exception error: " + arg);
			}
		}

		// Token: 0x06003571 RID: 13681 RVA: 0x000AD3EC File Offset: 0x000AB5EC
		public override void InitLoginView(System.Action callback)
		{
			UI_Loading.LoadView();
			UI_P2PLogin.LoadView(delegate
			{
				if (this.onLogonShow != null)
				{
					this.onLogonShow();
					this.onLogonShow = null;
				}
				if (callback != null)
				{
					callback();
				}
			});
		}

		// Token: 0x06003572 RID: 13682 RVA: 0x00021531 File Offset: 0x0001F731
		public override void ParseNewFLAddress(string addr)
		{
			LoginControllerBase.cacheflserver = addr;
			this.loginNetWork.ParseFLEndpoint(addr);
		}

		// Token: 0x06003573 RID: 13683 RVA: 0x0000415C File Offset: 0x0000235C
		public override void OnUpdate()
		{
		}

		// Token: 0x17000C31 RID: 3121
		// (get) Token: 0x06003574 RID: 13684 RVA: 0x00021545 File Offset: 0x0001F745
		public override string ControllerName
		{
			get
			{
				return "loginp2p_controller";
			}
		}

		// Token: 0x06003575 RID: 13685 RVA: 0x000AD424 File Offset: 0x000AB624
		public override void Login(string account, ushort zone)
		{
			this.loginNetWork.ConnectFirServer(this.loginNetWork.CurrentIpEndPoint, LoginType.UUID, zone, "1", 52, "0", 0, account);
		}

		// Token: 0x06003576 RID: 13686 RVA: 0x000AD458 File Offset: 0x000AB658
		public override void Login()
		{
			string pwd = "1";
			string account = this.loginModel.Account;
			int lastServer = UserInfoStorage.StorageInfo.LastServer;
			string pwd2 = UserInfoStorage.StorageInfo.Pwd;
			if (!string.IsNullOrEmpty(pwd2))
			{
				pwd = pwd2;
			}
			string uid = UserInfoStorage.StorageInfo.Uid;
			if (!string.IsNullOrEmpty(uid))
			{
				account = uid;
			}
			int num = 1;
			int.TryParse(UserInfoStorage.StorageInfo.Zone, out num);
			this.loginNetWork.ConnectFirServer(this.loginNetWork.CurrentIpEndPoint, LoginType.UUID, (ushort)num, pwd, 52, "0", 0, account);
		}

		// Token: 0x04001F7A RID: 8058
		public System.Action onLogonShow;
	}
}
