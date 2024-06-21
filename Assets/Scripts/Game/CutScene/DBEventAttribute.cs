using System;

namespace Game.CutScene
{
    public class DBEventAttribute : Attribute
    {
        public DBEventAttribute(string myEventPath)
        {
            this.eventPath = myEventPath;
        }

        public string EventPath
        {
            get
            {
                return this.eventPath;
            }
        }

        private string eventPath;
    }
}
