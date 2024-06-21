// dnSpy decompiler from Assembly-CSharp.dll class: AK.Wwise.Bank
using System;

namespace AK.Wwise
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

		public void Load(bool decodeBank = false, bool saveDecodedBank = false)
		{
			if (this.IsValid())
			{
				AkBankManager.LoadBank(this.Name, decodeBank, saveDecodedBank);
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
