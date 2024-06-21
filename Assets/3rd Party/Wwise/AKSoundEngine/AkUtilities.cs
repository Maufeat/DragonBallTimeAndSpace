// Decompiled with JetBrains decompiler
// Type: AkUtilities
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E932BD65-75F0-4D7B-B799-A44854E44E1C
// Assembly location: D:\DragonBallNew\DragonBallOL_Data\Managed\Assembly-CSharp.dll

using System.Text;

public class AkUtilities
{
    public class ShortIDGenerator
    {
        private const uint s_prime32 = 16777619;
        private const uint s_offsetBasis32 = 2166136261;
        private static byte s_hashSize;
        private static uint s_mask;

        static ShortIDGenerator()
        {
            AkUtilities.ShortIDGenerator.HashSize = (byte)32;
        }

        public static byte HashSize
        {
            get
            {
                return AkUtilities.ShortIDGenerator.s_hashSize;
            }
            set
            {
                AkUtilities.ShortIDGenerator.s_hashSize = value;
                AkUtilities.ShortIDGenerator.s_mask = (uint)((1 << (int)AkUtilities.ShortIDGenerator.s_hashSize) - 1);
            }
        }

        public static uint Compute(string in_name)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(in_name.ToLower());
            uint num = 2166136261;
            for (int index = 0; index < bytes.Length; ++index)
                num = num * 16777619U ^ (uint)bytes[index];
            if (AkUtilities.ShortIDGenerator.s_hashSize == (byte)32)
                return num;
            return num >> (int)AkUtilities.ShortIDGenerator.s_hashSize ^ num & AkUtilities.ShortIDGenerator.s_mask;
        }
    }
}
