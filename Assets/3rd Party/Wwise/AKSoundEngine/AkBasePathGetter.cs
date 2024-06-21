// dnSpy decompiler from Assembly-CSharp.dll class: AkBasePathGetter
using System;
using System.IO;
using UnityEngine;

public class AkBasePathGetter
{
	public static string GetPlatformName()
	{
		string empty = string.Empty;
		if (!string.IsNullOrEmpty(empty))
		{
			return empty;
		}
		return "Windows";
	}

	public static string GetPlatformBasePath()
	{
		string platformName = AkBasePathGetter.GetPlatformName();
		string result = Path.Combine(AkBasePathGetter.GetFullSoundBankPath(), platformName);
		AkBasePathGetter.FixSlashes(ref result);
		return result;
	}

	public static string GetFullSoundBankPath()
	{
		string text = string.Empty;
		if (string.IsNullOrEmpty(text))
		{
			text = AkWwiseInitializationSettings.ActivePlatformSettings.SoundbankPath;
		}
		text = Path.Combine(Application.streamingAssetsPath, text);
		AkBasePathGetter.FixSlashes(ref text);
		return text;
	}

	public static void FixSlashes(ref string path, char separatorChar, char badChar, bool addTrailingSlash)
	{
		if (string.IsNullOrEmpty(path))
		{
			return;
		}
		path = path.Trim().Replace(badChar, separatorChar).TrimStart(new char[]
		{
			'\\'
		});
		if (addTrailingSlash && !path.EndsWith(separatorChar.ToString()))
		{
			path += separatorChar;
		}
	}

	public static void FixSlashes(ref string path)
	{
		char directorySeparatorChar = Path.DirectorySeparatorChar;
		char badChar = (directorySeparatorChar != '\\') ? '\\' : '/';
		AkBasePathGetter.FixSlashes(ref path, directorySeparatorChar, badChar, true);
	}

	public static string GetSoundbankBasePath()
	{
		string platformBasePath = AkBasePathGetter.GetPlatformBasePath();
		bool flag = true;
		string path = Path.Combine(platformBasePath, "Init.bnk");
		if (!File.Exists(path))
		{
			flag = false;
		}
		if (platformBasePath == string.Empty || !flag)
		{
			UnityEngine.Debug.Log("WwiseUnity: Looking for SoundBanks in " + platformBasePath);
			UnityEngine.Debug.LogError("WwiseUnity: Could not locate the SoundBanks. Did you make sure to copy them to the StreamingAssets folder?");
		}
		return platformBasePath;
	}

	public static string DefaultBasePath = Path.Combine("Audio", "GeneratedSoundBanks");
}
