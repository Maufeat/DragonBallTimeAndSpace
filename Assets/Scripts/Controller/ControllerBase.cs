using Framework.Base.MVC;
using System;

namespace Models
{
    public class ControllerBase : IController
    {
        protected ControllerBase()
        {
        }

        public virtual void Awake()
        {
        }

        public virtual void OnUpdate()
        {
        }

        public virtual void OnDestroy()
        {
        }

        public virtual string ControllerName
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }
    }
}
