using System;
using Models;

namespace UI.Login
{
    public class LoginControllerBase : ControllerBase
    {
        public LoginControllerBase()
        {
            LoginControllerBase.Instance = this;
        }

        public UIPanelBase uiLogin
        {
            get
            {
                return UIManager.GetUIObject<UI_P2PLogin>();
            }
        }

        public void Close()
        {
            UI_P2PLogin uiobject = UIManager.GetUIObject<UI_P2PLogin>();
            if (null != uiobject)
            {
                uiobject.close();
            }
        }

        public void showStaticImg()
        {
            UI_P2PLogin uiobject = UIManager.GetUIObject<UI_P2PLogin>();
            if (null != uiobject)
            {
                uiobject.ShowStaticImg();
            }
        }

        public virtual void LoginAwake()
        {
        }

        public virtual void ParseNewFLAddress(string addr)
        {
        }

        public override void Awake()
        {
            this.LoginAwake();
        }

        public virtual void InitLoginView(Action callback)
        {
        }

        public override void OnUpdate()
        {
        }

        public override string ControllerName
        {
            get
            {
                return "login_controller";
            }
        }

        public virtual void Login(string account, ushort zone)
        {
        }

        public virtual void Login()
        {
        }

        public bool CheckName(string name)
        {
            if (name == string.Empty)
            {
                TipsWindow.ShowWindow(TipsType.NAME_CANNOT_NULL, null);
                return false;
            }
            if (this.getCharCount(name) > 14)
            {
                TipsWindow.ShowWindow(TipsType.NAME_LESS_THAN, null);
                return false;
            }
            for (int i = 0; i < this.strSenWord.Length; i++)
            {
                if (name.Contains(this.strSenWord[i]))
                {
                    TipsWindow.ShowWindow(TipsType.NAME_HAVE_ILLEGAL, null);
                    return false;
                }
            }
            KeyWordFilter.InitFilter();
            return true;
        }

        public void RegisterPlayer(string name, byte sex, uint pro)
        {
            if (!this.CheckName(name))
            {
                return;
            }
            this.loginNetWork.SendRegisterPlayer(name, sex, pro);
        }

        public void RegisterPlayer(string name, uint pro)
        {
            if (!this.CheckName(name))
            {
                return;
            }
            this.loginNetWork.SendRegisterPlayer(name, pro);
        }

        private int getCharCount(string str)
        {
            int num = 0;
            foreach (char c in str)
            {
                if (c >= 'a' && c <= 'z')
                {
                    num++;
                }
                else if (c >= 'A' && c <= 'Z')
                {
                    num++;
                }
                else if (c >= '0' && c <= '9')
                {
                    num++;
                }
                else
                {
                    num += 2;
                }
            }
            return num;
        }

        public LoginNetWork loginNetWork;

        protected static string cacheflserver = string.Empty;

        public static LoginControllerBase Instance;

        public LoginModel loginModel;

        private string[] strSenWord = new string[]
        {
            ",",
            "."
        };
    }
}
