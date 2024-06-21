using System;
using UnityEngine;

public class BundleDetective : MonoBehaviour
{
    public Action<string> BundleOnAddCallBack { get; set; }

    public Action<string> BundleOnDestryCallBack { get; set; }

    public string AniBundleName
    {
        get
        {
            return this._aniBundleName;
        }
        set
        {
            if (this._aniBundleName == value)
            {
                return;
            }
            if (this.BundleOnDestryCallBack != null)
            {
                this.BundleOnDestryCallBack(this._aniBundleName);
            }
            if (this.BundleOnAddCallBack != null)
            {
                this.BundleOnAddCallBack(value);
            }
            this._aniBundleName = value;
        }
    }

    private void OnDestroy()
    {
        if (this.BundleOnDestryCallBack != null)
        {
            this.BundleOnDestryCallBack(this._aniBundleName);
        }
    }

    private string _aniBundleName = string.Empty;
}
