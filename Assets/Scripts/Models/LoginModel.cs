using System;

namespace Models
{
    public class LoginModel : ModelBase
    {
        public override string ModelName
        {
            get
            {
                return "login_model";
            }
        }

        public string Account;
    }
}
