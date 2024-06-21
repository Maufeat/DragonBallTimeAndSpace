// dnSpy decompiler from Assembly-CSharp.dll class: AkBankManager
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using UnityEngine;

public static class AkBankManager
{
	internal static void DoUnloadBanks()
	{
		int count = AkBankManager.BanksToUnload.Count;
		for (int i = 0; i < count; i++)
		{
			AkBankManager.BanksToUnload[i].UnloadBank();
		}
		AkBankManager.BanksToUnload.Clear();
	}

	internal static void Reset()
	{
		AkBankManager.m_BankHandles.Clear();
		AkBankManager.BanksToUnload.Clear();
	}

	public static void LoadBank(string name, bool decodeBank, bool saveDecodedBank)
	{
		AkBankManager.m_Mutex.WaitOne();
		AkBankManager.BankHandle bankHandle = null;
		if (!AkBankManager.m_BankHandles.TryGetValue(name, out bankHandle))
		{
			bankHandle = ((!decodeBank) ? new AkBankManager.BankHandle(name) : new AkBankManager.DecodableBankHandle(name, saveDecodedBank));
			AkBankManager.m_BankHandles.Add(name, bankHandle);
			AkBankManager.m_Mutex.ReleaseMutex();
			bankHandle.LoadBank();
		}
		else
		{
			bankHandle.IncRef();
			AkBankManager.m_Mutex.ReleaseMutex();
		}
	}

	public static void LoadBankAsync(string name, AkCallbackManager.BankCallback callback = null)
	{
		AkBankManager.m_Mutex.WaitOne();
		AkBankManager.BankHandle bankHandle = null;
		if (!AkBankManager.m_BankHandles.TryGetValue(name, out bankHandle))
		{
			AkBankManager.AsyncBankHandle asyncBankHandle = new AkBankManager.AsyncBankHandle(name, callback);
			AkBankManager.m_BankHandles.Add(name, asyncBankHandle);
			AkBankManager.m_Mutex.ReleaseMutex();
			asyncBankHandle.LoadBank();
		}
		else
		{
			bankHandle.IncRef();
			AkBankManager.m_Mutex.ReleaseMutex();
		}
	}

	public static void UnloadBank(string name)
	{
		AkBankManager.m_Mutex.WaitOne();
		AkBankManager.BankHandle bankHandle = null;
		if (AkBankManager.m_BankHandles.TryGetValue(name, out bankHandle))
		{
			bankHandle.DecRef();
			if (bankHandle.RefCount == 0)
			{
				AkBankManager.m_BankHandles.Remove(name);
			}
		}
		AkBankManager.m_Mutex.ReleaseMutex();
	}

	private static readonly Dictionary<string, AkBankManager.BankHandle> m_BankHandles = new Dictionary<string, AkBankManager.BankHandle>();

	private static readonly List<AkBankManager.BankHandle> BanksToUnload = new List<AkBankManager.BankHandle>();

	private static readonly Mutex m_Mutex = new Mutex();

	private class BankHandle
	{
		public BankHandle(string name)
		{
			this.bankName = name;
		}

		public int RefCount { get; private set; }

		public virtual AKRESULT DoLoadBank()
		{
			return AkSoundEngine.LoadBank(this.bankName, -1, out this.m_BankID);
		}

		public void LoadBank()
		{
			if (this.RefCount == 0)
			{
				if (AkBankManager.BanksToUnload.Contains(this))
				{
					AkBankManager.BanksToUnload.Remove(this);
				}
				else
				{
					AKRESULT result = this.DoLoadBank();
					this.LogLoadResult(result);
				}
			}
			this.IncRef();
		}

		public virtual void UnloadBank()
		{
			AkSoundEngine.UnloadBank(this.m_BankID, IntPtr.Zero, null, null);
		}

		public void IncRef()
		{
			this.RefCount++;
		}

		public void DecRef()
		{
			this.RefCount--;
			if (this.RefCount == 0)
			{
				AkBankManager.BanksToUnload.Add(this);
			}
		}

		protected void LogLoadResult(AKRESULT result)
		{
			if (result != AKRESULT.AK_Success && AkSoundEngine.IsInitialized())
			{
				UnityEngine.Debug.LogWarning(string.Concat(new object[]
				{
					"WwiseUnity: Bank ",
					this.bankName,
					" failed to load (",
					result,
					")"
				}));
			}
		}

		protected readonly string bankName;

		protected uint m_BankID;
	}

	private class AsyncBankHandle : AkBankManager.BankHandle
	{
		public AsyncBankHandle(string name, AkCallbackManager.BankCallback callback) : base(name)
		{
			this.bankCallback = callback;
		}

		private static void GlobalBankCallback(uint in_bankID, IntPtr in_pInMemoryBankPtr, AKRESULT in_eLoadResult, uint in_memPoolId, object in_Cookie)
		{
			AkBankManager.m_Mutex.WaitOne();
			AkBankManager.AsyncBankHandle asyncBankHandle = (AkBankManager.AsyncBankHandle)in_Cookie;
			AkCallbackManager.BankCallback bankCallback = asyncBankHandle.bankCallback;
			if (in_eLoadResult != AKRESULT.AK_Success)
			{
				asyncBankHandle.LogLoadResult(in_eLoadResult);
				AkBankManager.m_BankHandles.Remove(asyncBankHandle.bankName);
			}
			AkBankManager.m_Mutex.ReleaseMutex();
			if (bankCallback != null)
			{
				bankCallback(in_bankID, in_pInMemoryBankPtr, in_eLoadResult, in_memPoolId, null);
			}
		}

		public override AKRESULT DoLoadBank()
		{
			return AkSoundEngine.LoadBank(this.bankName, new AkCallbackManager.BankCallback(AkBankManager.AsyncBankHandle.GlobalBankCallback), this, -1, out this.m_BankID);
		}

		private readonly AkCallbackManager.BankCallback bankCallback;
	}

	private class DecodableBankHandle : AkBankManager.BankHandle
	{
		public DecodableBankHandle(string name, bool save) : base(name)
		{
			this.saveDecodedBank = save;
			string path = this.bankName + ".bnk";
			string currentLanguage = AkSoundEngine.GetCurrentLanguage();
			string decodedBankFullPath = AkSoundEngineController.GetDecodedBankFullPath();
			this.decodedBankPath = Path.Combine(decodedBankFullPath, currentLanguage);
			string path2 = Path.Combine(this.decodedBankPath, path);
			bool flag = File.Exists(path2);
			if (!flag)
			{
				this.decodedBankPath = decodedBankFullPath;
				path2 = Path.Combine(this.decodedBankPath, path);
				flag = File.Exists(path2);
			}
			if (flag)
			{
				try
				{
					DateTime lastWriteTime = File.GetLastWriteTime(path2);
					string soundbankBasePath = AkBasePathGetter.GetSoundbankBasePath();
					string path3 = Path.Combine(soundbankBasePath, path);
					DateTime lastWriteTime2 = File.GetLastWriteTime(path3);
					this.decodeBank = (lastWriteTime <= lastWriteTime2);
				}
				catch
				{
				}
			}
		}

		public override AKRESULT DoLoadBank()
		{
			if (this.decodeBank)
			{
				return AkSoundEngine.LoadAndDecodeBank(this.bankName, this.saveDecodedBank, out this.m_BankID);
			}
			AKRESULT akresult = AKRESULT.AK_Success;
			if (!string.IsNullOrEmpty(this.decodedBankPath))
			{
				akresult = AkSoundEngine.SetBasePath(this.decodedBankPath);
			}
			if (akresult == AKRESULT.AK_Success)
			{
				akresult = AkSoundEngine.LoadBank(this.bankName, -1, out this.m_BankID);
				if (!string.IsNullOrEmpty(this.decodedBankPath))
				{
					AkSoundEngine.SetBasePath(AkBasePathGetter.GetSoundbankBasePath());
				}
			}
			return akresult;
		}

		public override void UnloadBank()
		{
			if (this.decodeBank && !this.saveDecodedBank)
			{
				AkSoundEngine.PrepareBank(AkPreparationType.Preparation_Unload, this.m_BankID);
			}
			else
			{
				base.UnloadBank();
			}
		}

		private readonly bool decodeBank = true;

		private readonly string decodedBankPath;

		private readonly bool saveDecodedBank;
	}
}
