using System;
using AK.Wwise;

namespace AudioStudio
{
    [Serializable]
    public class Bank : BaseType
    {
        public override WwiseObjectType WwiseObjectType
        {
            get
            {
                return WwiseObjectType.Soundbank;
            }
        }

        public void Load()
        {
            if (this.IsValid())
            {
                AkBankManager.LoadBank(this.Name);
            }
        }

        public void LoadAsync(AkCallbackManager.BankCallback callback = null)
        {
            if (this.IsValid())
            {
                AkBankManager.LoadBankAsync(this.Name, callback);
            }
        }

        public void Unload()
        {
            if (this.IsValid())
            {
                AkBankManager.UnloadBank(this.Name);
            }
        }
    }
}
