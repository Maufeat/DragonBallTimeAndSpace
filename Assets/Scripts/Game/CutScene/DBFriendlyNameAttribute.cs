using System;

namespace Game.CutScene
{
    public class DBFriendlyNameAttribute : Attribute
    {
        public DBFriendlyNameAttribute(string myFriendlyName)
        {
            this.friendlyName = myFriendlyName;
        }

        public string FriendlyName
        {
            get
            {
                return this.friendlyName;
            }
        }

        private string friendlyName;
    }
}
