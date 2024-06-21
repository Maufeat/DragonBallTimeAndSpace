using System;
using Framework.Managers;
using Models;

internal class SecondPwdControl : ControllerBase
{
    public UI_SecondPwd UISecondPwd
    {
        get
        {
            return UIManager.GetUIObject<UI_SecondPwd>();
        }
    }

    public bool CloseSecondPwd
    {
        get
        {
            return this._closeSecondPwd;
        }
        set
        {
            this._closeSecondPwd = value;
        }
    }

    public SecondPwdControl.Second_PWD_Show_Page ShowPage
    {
        get
        {
            return this._showPage;
        }
        set
        {
            this._showPage = value;
        }
    }

    public override void Awake()
    {
        this.secondPwdNetWork = new SecondPwdNetWork();
        this.secondPwdNetWork.Initialize();
    }

    public override string ControllerName
    {
        get
        {
            return "secondpwd_controller";
        }
    }

    public void ShowSecondPwd(SecondPwdControl.Second_PWD_Show_Page page, bool bylua = false)
    {
        this.ShowPage = page;
        ManagerCenter.Instance.GetManager<UIManager>().ShowUI<UI_SecondPwd>("UI_SecondPassword", null, UIManager.ParentType.CommonUI, bylua);
    }

    public void CloseUI()
    {
        if (null != this.UISecondPwd)
        {
            this.UISecondPwd.close();
        }
    }

    public string PlayerSecondPwd
    {
        get
        {
            return this._playerSecondPwd;
        }
        set
        {
            this._playerSecondPwd = value;
        }
    }

    public string PlayerInputSecondPwd
    {
        get
        {
            return this._playerInputSecondPwd;
        }
        set
        {
            this._playerInputSecondPwd = value;
        }
    }

    public void onInputPwdChanged(string pwd)
    {
        SecondPwdControl.PWD_INPUT_STATE pwd_INPUT_STATE = this.checkInput(pwd, null);
        SecondPwdControl.PWD_INPUT_STATE pwd_INPUT_STATE2 = pwd_INPUT_STATE;
        if (pwd_INPUT_STATE2 != SecondPwdControl.PWD_INPUT_STATE.INPUT_STATE_LENGTH_ERROR)
        {
            if (pwd_INPUT_STATE2 != SecondPwdControl.PWD_INPUT_STATE.INPUT_STATE_NUMBER_INVALID)
            {
                this.UISecondPwd.showFirstInputSuccessTips();
            }
            else
            {
                this.UISecondPwd.showFirstInputErrorTips(1);
            }
        }
        else
        {
            this.UISecondPwd.showFirstInputErrorTips(0);
        }
    }

    public bool CheckNeedInputSecondPwd()
    {
        if (string.IsNullOrEmpty(this.PlayerSecondPwd))
        {
            this.ShowSecondPwd(SecondPwdControl.Second_PWD_Show_Page.PAGE_SET_SECOND_PWD, false);
            return true;
        }
        if (!string.IsNullOrEmpty(this.PlayerInputSecondPwd))
        {
            string md5ByString = MD5Help.GetMD5ByString(this.PlayerInputSecondPwd);
            if (this.PlayerSecondPwd == md5ByString)
            {
                return false;
            }
        }
        this.ShowSecondPwd(SecondPwdControl.Second_PWD_Show_Page.PAGE_VERIFY_SECOND_PWD, false);
        return true;
    }

    public void onInputResetPwdChanged(string pwd)
    {
        SecondPwdControl.PWD_INPUT_STATE pwd_INPUT_STATE = this.checkInput(pwd, null);
        SecondPwdControl.PWD_INPUT_STATE pwd_INPUT_STATE2 = pwd_INPUT_STATE;
        if (pwd_INPUT_STATE2 != SecondPwdControl.PWD_INPUT_STATE.INPUT_STATE_LENGTH_ERROR)
        {
            if (pwd_INPUT_STATE2 != SecondPwdControl.PWD_INPUT_STATE.INPUT_STATE_NUMBER_INVALID)
            {
                this.UISecondPwd.showFirstInputSuccessTipsReset();
            }
            else
            {
                this.UISecondPwd.showFirstInputErrorTipsReset(1);
            }
        }
        else
        {
            this.UISecondPwd.showFirstInputErrorTipsReset(0);
        }
    }

    public void onConfirmPwdChanged(string pwd, string confirmPwd)
    {
        SecondPwdControl.PWD_INPUT_STATE pwd_INPUT_STATE = this.checkInput(pwd, confirmPwd);
        this.UISecondPwd.showConfirmInputTips(pwd_INPUT_STATE != SecondPwdControl.PWD_INPUT_STATE.INPUT_STATE_SUCCESS);
    }

    public void onConfirmPwdChangedReset(string pwd, string confirmPwd)
    {
        SecondPwdControl.PWD_INPUT_STATE pwd_INPUT_STATE = this.checkInput(pwd, confirmPwd);
        this.UISecondPwd.showConfirmInputTipsReset(pwd_INPUT_STATE != SecondPwdControl.PWD_INPUT_STATE.INPUT_STATE_SUCCESS);
    }

    public void ReqSetSecondPwd(string pwd)
    {
        SecondPwdControl.PWD_INPUT_STATE pwd_INPUT_STATE = this.checkInput(pwd, null);
        if (pwd_INPUT_STATE != SecondPwdControl.PWD_INPUT_STATE.INPUT_STATE_SUCCESS)
        {
            return;
        }
        this.secondPwdNetWork.ReqSetSecondPwd(pwd, string.Empty);
        this.PlayerInputSecondPwd = pwd;
    }

    public void ReqReSetSecondPwd(string pwd)
    {
        SecondPwdControl.PWD_INPUT_STATE pwd_INPUT_STATE = this.checkInput(pwd, null);
        if (pwd_INPUT_STATE != SecondPwdControl.PWD_INPUT_STATE.INPUT_STATE_SUCCESS)
        {
            return;
        }
        this.secondPwdNetWork.ReqSetSecondPwd(pwd, this.PlayerInputSecondPwd);
    }

    public void ReqVerifySecondPwd(string pwd)
    {
        string md5ByString = MD5Help.GetMD5ByString(pwd);
        bool flag = md5ByString == this._playerSecondPwd;
        if (flag)
        {
            this.PlayerInputSecondPwd = pwd;
            this.UISecondPwd.close();
        }
        else
        {
            this.UISecondPwd.OnVerifyError(1);
        }
    }

    public void ReqVerifySecondPwdReset(string pwd)
    {
        string md5ByString = MD5Help.GetMD5ByString(pwd);
        bool flag = md5ByString == this._playerSecondPwd;
        if (flag)
        {
            this.PlayerInputSecondPwd = pwd;
        }
        this.UISecondPwd.ShowVerifyReset(1, flag);
    }

    private SecondPwdControl.PWD_INPUT_STATE checkInput(string pwd, string confirmpwd)
    {
        if (string.IsNullOrEmpty(confirmpwd))
        {
            if (pwd.Length < 6)
            {
                return SecondPwdControl.PWD_INPUT_STATE.INPUT_STATE_LENGTH_ERROR;
            }
            int num = 0;
            if (!int.TryParse(pwd, out num))
            {
                return SecondPwdControl.PWD_INPUT_STATE.INPUT_STATE_NUMBER_INVALID;
            }
        }
        else if (pwd != confirmpwd)
        {
            return SecondPwdControl.PWD_INPUT_STATE.INPUT_STATE_CONFIM_ERROR;
        }
        return SecondPwdControl.PWD_INPUT_STATE.INPUT_STATE_SUCCESS;
    }

    private string _playerSecondPwd = string.Empty;

    private string _playerInputSecondPwd = string.Empty;

    private SecondPwdNetWork secondPwdNetWork;

    private bool _closeSecondPwd;

    private SecondPwdControl.Second_PWD_Show_Page _showPage;

    public enum Second_PWD_Show_Page
    {
        PAGE_SET_SECOND_PWD,
        PAGE_VERIFY_SECOND_PWD,
        PAGE_RESET_SECOND_PWD
    }

    private enum PWD_INPUT_STATE
    {
        INPUT_STATE_SUCCESS,
        INPUT_STATE_LENGTH_ERROR,
        INPUT_STATE_NUMBER_INVALID,
        INPUT_STATE_CONFIM_ERROR
    }
}
