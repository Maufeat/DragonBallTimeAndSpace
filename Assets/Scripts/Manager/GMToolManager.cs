using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GMToolManager : MonoBehaviour
{
    public void Initilize()
    {
        if (Constant.CUR_VRESION == Constant.Version.Dist)
        {
            base.gameObject.SetActive(false);
            return;
        }
        base.gameObject.SetActive(false);
        Button component = base.GetComponent<Button>();
        component.onClick.RemoveAllListeners();
        component.onClick.AddListener(new UnityAction(this.btn_gmtool_on_click));
    }

    private void btn_gmtool_on_click()
    {
        if (UIManager.GetUIObject<UI_GM>())
        {
            UIManager.Instance.DeleteUI<UI_GM>();
        }
        else
        {
            UIManager.Instance.ShowUI<UI_GM>("UI_GM", delegate ()
            {
                UIManager.GetUIObject<UI_GM>().Initilize(this);
            }, UIManager.ParentType.CommonUI, false);
        }
        this.isGMToolOpen = !this.isGMToolOpen;
    }

    private void Update()
    {
    }

    public bool isGMToolOpen;
}
