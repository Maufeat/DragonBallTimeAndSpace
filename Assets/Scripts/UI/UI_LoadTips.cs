using UnityEngine;
using UnityEngine.UI;

public class UI_LoadTips : UIPanelBase
{
    private Transform Root;
    private static Text txt_WarnText;

    public override void OnInit(Transform root)
    {
        base.OnInit(root);
        this.Root = root;
        this.Root.gameObject.SetActive(false);
        root.transform.Find("Offset_LoadTips/Panel").gameObject.SetActive(true);
        UI_LoadTips.txt_WarnText = root.transform.Find("Offset_LoadTips/Panel/Text").GetComponent<Text>();
        UI_LoadTips.txt_WarnText.gameObject.SetActive(false);
        root.transform.Find("Offset_LoadTips/Panel/img_loading").gameObject.SetActive(false);
        root.transform.Find("Offset_LoadTips/Panel/Dot1").gameObject.SetActive(false);
        root.transform.Find("Offset_LoadTips/Panel/Dot2").gameObject.SetActive(false);
        root.transform.Find("Offset_LoadTips/Panel/Dot3").gameObject.SetActive(false);
    }

    public void ShowThis(string content)
    {
        UI_LoadTips.txt_WarnText.text = content;
        this.Root.gameObject.SetActive(true);
    }

    public void CloseThis()
    {
        this.Root.gameObject.SetActive(false);
    }
}
