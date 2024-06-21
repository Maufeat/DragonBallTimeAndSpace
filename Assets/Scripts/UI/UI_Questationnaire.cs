using System;
using Framework.Managers;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using ZenFulcrum.EmbeddedBrowser;

public class UI_Questationnaire : UIPanelBase
{
    private QuestationnaireController mController
    {
        get
        {
            return ControllerManager.Instance.GetController<QuestationnaireController>();
        }
    }

    public override void OnInit(Transform root)
    {
        this.sc = root.GetChild(0).GetComponent<SimpleController>();
        if (this.sc == null)
        {
            this.sc = root.GetChild(0).gameObject.AddComponent<SimpleController>();
        }
        root.GetChild(0).GetComponent<GUIBrowserUI>().raycaster = root.GetComponent<GraphicRaycaster>();
        this.sc.gameObject.SetActive(true);
        root.GetChild(1).GetComponent<Button>().onClick.RemoveAllListeners();
        root.GetChild(1).GetComponent<Button>().onClick.AddListener(new UnityAction(this.btn_ques_onclick));
        base.OnInit(root);
    }

    private void btn_ques_onclick()
    {
        this.mController.DeleteUI();
    }

    public void SetupPanel(string url)
    {
        this.sc.GoToURL(url);
    }

    public override void AfterInit()
    {
        base.AfterInit();
    }

    public override void OnDispose()
    {
        base.OnDispose();
    }

    private SimpleController sc;
}
