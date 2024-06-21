using UnityEngine;

namespace ResoureManager
{
    public class AseetItem
    {
        public string Path;
        public WWW Req;

        public AseetItem(string path)
        {
            this.Path = path;
        }

        public float Process
        {
            get
            {
                return this.Req == null ? 0.0f : this.Req.progress;
            }
        }
    }
}
