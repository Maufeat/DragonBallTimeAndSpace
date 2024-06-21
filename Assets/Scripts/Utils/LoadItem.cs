using System;
using UnityEngine;

public class LoadItem
{
    public LoadItem(string path)
    {
        this.Path = path;
    }

    public float Process
    {
        get
        {
            if (this.Req == null)
            {
                return 0f;
            }
            return this.Req.progress;
        }
    }

    public string Path;

    public WWW Req;
}
