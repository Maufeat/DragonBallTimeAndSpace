using System;
using Framework.Base.MVC;

namespace Models
{
    public class ModelBase : IModel
    {
        protected ModelBase()
        {
        }

        public virtual string ModelName
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public virtual void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
