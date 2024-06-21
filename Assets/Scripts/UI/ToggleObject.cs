using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class ToggleObject : MonoBehaviour
{
    private void Start()
    {
        this.toggle = base.gameObject.GetComponent<Toggle>();
        if (this.toggle != null)
        {
            this.laststate = this.toggle.isOn;
            this.OnToggleValueChanged(this.laststate);
        }
    }

    private void Update()
    {
        if (this.laststate != this.toggle.isOn)
        {
            this.laststate = this.toggle.isOn;
            this.OnToggleValueChanged(this.laststate);
        }
    }

    private void OnToggleValueChanged(bool b)
    {
        if (this.Obj_ON != null)
        {
            this.Obj_ON.SetActive(b);
        }
        if (this.Obj_OFF)
        {
            this.Obj_OFF.SetActive(!b);
        }
    }

    public GameObject Obj_ON;

    public GameObject Obj_OFF;

    private Toggle toggle;

    private bool laststate;
}
