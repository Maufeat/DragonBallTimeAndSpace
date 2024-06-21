using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CInputMethodManager : MonoBehaviour
{
    public static CInputMethodManager Instance
    {
        get
        {
            return CInputMethodManager._instance;
        }
        set
        {
            CInputMethodManager._instance = value;
        }
    }

    private void Start()
    {
        CInputMethodManager.Instance = this;
        this.handle = CWin32Help.GetProcessWnd();
        this.imm = CWin32Help.GetIme(this.handle);
    }

    private void Update()
    {
        if (null != EventSystem.current.currentSelectedGameObject)
        {
            InputField component = EventSystem.current.currentSelectedGameObject.GetComponent<InputField>();
            if (component != null)
            {
                if (component.contentType != InputField.ContentType.IntegerNumber)
                {
                    if (!CWin32Help.GetImeStatus(this.imm))
                    {
                        CWin32Help.SetImeStatus(this.imm, true);
                    }
                }
                else if (CWin32Help.GetImeStatus(this.imm))
                {
                    CWin32Help.SetImeStatus(this.imm, false);
                }
            }
        }
        else if (CWin32Help.GetImeStatus(this.imm))
        {
            CWin32Help.SetImeStatus(this.imm, false);
        }
    }

    private IntPtr handle;

    private IntPtr imm;

    private static CInputMethodManager _instance;
}
