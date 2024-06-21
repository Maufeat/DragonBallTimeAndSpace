using Framework.Base;
using System;

namespace Framework.Managers
{
    public class ManagerBaseDefault : IManager
    {
        private ICompent _compent;
        private string _managerName;

        public string ManagerName
        {
            get
            {
                return this._managerName;
            }
            set
            {
                this._managerName = value;
            }
        }

        public ICompent Compent
        {
            set
            {
                this._compent = value;
                this.OnSetCompentComplete();
            }
            protected get
            {
                return this._compent;
            }
        }

        protected virtual void OnSetCompentComplete()
        {
        }

        public virtual void OnUpdate()
        {
            throw new NotImplementedException();
        }

        public void OnReSet()
        {
        }
    }
}
