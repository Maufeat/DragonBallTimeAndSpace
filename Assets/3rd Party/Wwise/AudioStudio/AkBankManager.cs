using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace AudioStudio
{
    public static class AkBankManager
    {
        public static void PerframeUpdate()
        {
            if (AkBankManager.BanksToUnload.Count > 0)
            {
                for (int i = AkBankManager.BanksToUnload.Count - 1; i >= 0; i--)
                {
                    if (Time.time - AkBankManager.BanksToUnload[i].BeginUnloadTime > AkBankManager.DelayUnloadSeconds)
                    {
                        AkBankManager.m_BankHandles.Remove(AkBankManager.BanksToUnload[i].bankName);
                        AkBankManager.BanksToUnload[i].UnloadBank();
                        AkBankManager.BanksToUnload.RemoveAt(i);
                    }
                }
            }
        }

        public static void Reset()
        {
            AkBankManager.m_BankHandles.Clear();
            AkBankManager.BanksToUnload.Clear();
        }

        public static void LoadBank(string name)
        {
            AkBankManager.Lock();
            AkBankManager.BankHandle bankHandle = null;
            if (!AkBankManager.m_BankHandles.TryGetValue(name, out bankHandle))
            {
                bankHandle = new AkBankManager.BankHandle(name);
                AkBankManager.m_BankHandles.Add(name, bankHandle);
                AkBankManager.Unlock();
                bankHandle.LoadBank();
            }
            else
            {
                bankHandle.IncRef();
                AkBankManager.RemoveFromUnload(bankHandle);
                AkBankManager.Unlock();
            }
        }

        public static void LoadBankAsync(string name, AkCallbackManager.BankCallback callback = null)
        {
            AkBankManager.Lock();
            AkBankManager.BankHandle bankHandle = null;
            if (!AkBankManager.m_BankHandles.TryGetValue(name, out bankHandle))
            {
                AkBankManager.AsyncBankHandle asyncBankHandle = new AkBankManager.AsyncBankHandle(name, callback);
                AkBankManager.m_BankHandles.Add(name, asyncBankHandle);
                AkBankManager.Unlock();
                asyncBankHandle.LoadBank();
            }
            else
            {
                bankHandle.IncRef();
                AkBankManager.RemoveFromUnload(bankHandle);
                AkBankManager.Unlock();
            }
        }

        public static void UnloadBank(string name)
        {
            AkBankManager.Lock();
            AkBankManager.BankHandle bankHandle = null;
            if (AkBankManager.m_BankHandles.TryGetValue(name, out bankHandle))
            {
                bankHandle.DecRef();
                if (bankHandle.RefCount == 0)
                {
                    bankHandle.BeginUnloadTime = Time.time;
                    AkBankManager.BanksToUnload.Add(bankHandle);
                }
            }
            AkBankManager.Unlock();
        }

        private static void RemoveFromUnload(AkBankManager.BankHandle handle)
        {
            if (AkBankManager.BanksToUnload.Contains(handle))
            {
                AkBankManager.BanksToUnload.Remove(handle);
            }
        }

        public static void ForceUnloadAll()
        {
        }

        private static void Lock()
        {
            if (AkBankManager.ThreadSafe)
            {
                AkBankManager.m_Mutex.WaitOne();
            }
        }

        private static void Unlock()
        {
            if (AkBankManager.ThreadSafe)
            {
                AkBankManager.m_Mutex.ReleaseMutex();
            }
        }

        private static Dictionary<string, AkBankManager.BankHandle> m_BankHandles = new Dictionary<string, AkBankManager.BankHandle>();

        private static List<AkBankManager.BankHandle> BanksToUnload = new List<AkBankManager.BankHandle>();

        private static Mutex m_Mutex = new Mutex();

        public static bool ThreadSafe = false;

        public static float LastUnloadTime = 0f;

        public static float DelayUnloadSeconds = 30f;

        private class BankHandle
        {
            public BankHandle(string name)
            {
                this.bankName = name;
            }

            public int RefCount
            {
                get
                {
                    return this.m_RefCount;
                }
            }

            public float BeginUnloadTime
            {
                get
                {
                    return this.m_beginUnloadTime;
                }
                set
                {
                    this.m_beginUnloadTime = value;
                }
            }

            public override bool Equals(object obj)
            {
                AkBankManager.BankHandle bankHandle = (AkBankManager.BankHandle)obj;
                return this.bankName.Equals(bankHandle.bankName);
            }

            public override int GetHashCode()
            {
                return this.bankName.GetHashCode();
            }

            public virtual AKRESULT DoLoadBank()
            {
                return AkSoundEngine.LoadBank(this.bankName, -1, out this.m_BankID);
            }

            public void LoadBank()
            {
                if (this.m_RefCount == 0)
                {
                    AKRESULT result = this.DoLoadBank();
                    this.LogLoadResult(result);
                }
                this.IncRef();
            }

            public virtual void UnloadBank()
            {
                AkSoundEngine.UnloadBank(this.m_BankID, IntPtr.Zero, null, null);
            }

            public void IncRef()
            {
                this.m_RefCount++;
            }

            public void DecRef()
            {
                this.m_RefCount--;
            }

            protected void LogLoadResult(AKRESULT result)
            {
                if (result != AKRESULT.AK_Success)
                {
                    AkSoundEngine.LogWarning(string.Concat(new string[]
                    {
                        "WwiseUnity: Bank ",
                        this.bankName,
                        " failed to load (",
                        result.ToString(),
                        ")"
                    }));
                }
            }

            protected int m_RefCount;

            protected uint m_BankID;

            protected float m_beginUnloadTime;

            public string bankName;
        }

        private class AsyncBankHandle : AkBankManager.BankHandle
        {
            public AsyncBankHandle(string name, AkCallbackManager.BankCallback callback) : base(name)
            {
                this.bankCallback = callback;
            }

            private static void GlobalBankCallback(uint in_bankID, IntPtr in_pInMemoryBankPtr, AKRESULT in_eLoadResult, uint in_memPoolId, object in_Cookie)
            {
                AkBankManager.Lock();
                AkBankManager.AsyncBankHandle asyncBankHandle = (AkBankManager.AsyncBankHandle)in_Cookie;
                AkCallbackManager.BankCallback bankCallback = asyncBankHandle.bankCallback;
                if (in_eLoadResult != AKRESULT.AK_Success)
                {
                    asyncBankHandle.LogLoadResult(in_eLoadResult);
                    AkBankManager.m_BankHandles.Remove(asyncBankHandle.bankName);
                }
                AkBankManager.Unlock();
                if (bankCallback != null)
                {
                    bankCallback(in_bankID, in_pInMemoryBankPtr, in_eLoadResult, in_memPoolId, null);
                }
            }

            public override AKRESULT DoLoadBank()
            {
                return AkSoundEngine.LoadBank(this.bankName, new AkCallbackManager.BankCallback(AkBankManager.AsyncBankHandle.GlobalBankCallback), this, -1, out this.m_BankID);
            }

            private AkCallbackManager.BankCallback bankCallback;
        }
    }
}
