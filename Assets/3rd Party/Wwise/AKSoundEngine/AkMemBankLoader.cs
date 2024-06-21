// dnSpy decompiler from Assembly-CSharp.dll class: AkMemBankLoader
using System;
using System.Collections;
using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;

public class AkMemBankLoader : MonoBehaviour
{
	private void Start()
	{
		if (this.isLocalizedBank)
		{
			this.LoadLocalizedBank(this.bankName);
		}
		else
		{
			this.LoadNonLocalizedBank(this.bankName);
		}
	}

	public void LoadNonLocalizedBank(string in_bankFilename)
	{
		string in_bankPath = "file://" + Path.Combine(AkBasePathGetter.GetPlatformBasePath(), in_bankFilename);
		this.DoLoadBank(in_bankPath);
	}

	public void LoadLocalizedBank(string in_bankFilename)
	{
		string in_bankPath = "file://" + Path.Combine(Path.Combine(AkBasePathGetter.GetPlatformBasePath(), AkSoundEngine.GetCurrentLanguage()), in_bankFilename);
		this.DoLoadBank(in_bankPath);
	}

	private IEnumerator LoadFile()
	{
		this.ms_www = new WWW(this.m_bankPath);
		yield return this.ms_www;
		uint in_uInMemoryBankSize = 0u;
		try
		{
			this.ms_pinnedArray = GCHandle.Alloc(this.ms_www.bytes, GCHandleType.Pinned);
			this.ms_pInMemoryBankPtr = this.ms_pinnedArray.AddrOfPinnedObject();
			in_uInMemoryBankSize = (uint)this.ms_www.bytes.Length;
			if ((this.ms_pInMemoryBankPtr.ToInt64() & 15L) != 0L)
			{
				byte[] alignedBytes = new byte[(long)this.ms_www.bytes.Length + 16L];
				GCHandle new_pinnedArray = GCHandle.Alloc(alignedBytes, GCHandleType.Pinned);
				IntPtr new_pInMemoryBankPtr = new_pinnedArray.AddrOfPinnedObject();
				int alignedOffset = 0;
				if ((new_pInMemoryBankPtr.ToInt64() & 15L) != 0L)
				{
					long alignedPtr = new_pInMemoryBankPtr.ToInt64() + 15L & -16L;
					alignedOffset = (int)(alignedPtr - new_pInMemoryBankPtr.ToInt64());
					new_pInMemoryBankPtr = new IntPtr(alignedPtr);
				}
				Array.Copy(this.ms_www.bytes, 0, alignedBytes, alignedOffset, this.ms_www.bytes.Length);
				this.ms_pInMemoryBankPtr = new_pInMemoryBankPtr;
				this.ms_pinnedArray.Free();
				this.ms_pinnedArray = new_pinnedArray;
			}
		}
		catch
		{
			yield break;
		}
		AKRESULT result = AkSoundEngine.LoadBank(this.ms_pInMemoryBankPtr, in_uInMemoryBankSize, out this.ms_bankID);
		if (result != AKRESULT.AK_Success)
		{
			UnityEngine.Debug.LogError("WwiseUnity: AkMemBankLoader: bank loading failed with result " + result);
		}
		yield break;
	}

	private void DoLoadBank(string in_bankPath)
	{
		this.m_bankPath = in_bankPath;
		base.StartCoroutine(this.LoadFile());
	}

	private void OnDestroy()
	{
		if (this.ms_pInMemoryBankPtr != IntPtr.Zero)
		{
			AKRESULT akresult = AkSoundEngine.UnloadBank(this.ms_bankID, this.ms_pInMemoryBankPtr);
			if (akresult == AKRESULT.AK_Success)
			{
				this.ms_pinnedArray.Free();
			}
		}
	}

	private const int WaitMs = 50;

	private const long AK_BANK_PLATFORM_DATA_ALIGNMENT = 16L;

	private const long AK_BANK_PLATFORM_DATA_ALIGNMENT_MASK = 15L;

	public string bankName = string.Empty;

	public bool isLocalizedBank;

	private string m_bankPath;

	[HideInInspector]
	public uint ms_bankID;

	private IntPtr ms_pInMemoryBankPtr = IntPtr.Zero;

	private GCHandle ms_pinnedArray;

	private WWW ms_www;
}
