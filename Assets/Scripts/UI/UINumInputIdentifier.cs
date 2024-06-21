using System;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class UINumInputIdentifier : UIIdentifier
{
    private void Awake()
    {
        this._type = UIIdentifier.IdentifierType.UINumberInput;
    }

    public InputField mInputField;

    public uint mIndex = 1U;
}
