using System;
using Framework.Managers;
using UnityEngine;

public class BoxEffectComponent : MonoBehaviour
{
    private AbattoirMatchController abattoirController
    {
        get
        {
            return ControllerManager.Instance.GetController<AbattoirMatchController>();
        }
    }

    public void OnEffectEnd()
    {
        if (this.showObj != null)
        {
            this.showObj.gameObject.SetActive(true);
        }
        base.gameObject.SetActive(false);
    }

    public Transform showObj;

    public int index;
}
