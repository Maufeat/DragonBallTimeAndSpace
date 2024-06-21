using System;
using Models;

public class CountDownController : ControllerBase
{
    public override void Awake()
    {
    }

    private UI_CountDwon ui_countDown
    {
        get
        {
            return UIManager.GetUIObject<UI_CountDwon>();
        }
    }

    public override string ControllerName
    {
        get
        {
            return "countdown";
        }
    }

    public void ShowCountDownPanel(string message, float duation, Action onEnsure, Action onEnd = null, Action onClose = null, bool isShowCloseButton = false)
    {
        if (this.ui_countDown == null)
        {
            UIManager.Instance.ShowUI<UI_CountDwon>("UI_CountDown", delegate ()
            {
                this.ui_countDown.SetMessage(message, duation, onEnsure, onEnd, onClose, isShowCloseButton);
            }, UIManager.ParentType.CommonUI, false);
        }
        else
        {
            this.ui_countDown.SetMessage(message, duation, onEnsure, onEnd, onClose, isShowCloseButton);
        }
    }

    public void Dispose()
    {
    }
}
