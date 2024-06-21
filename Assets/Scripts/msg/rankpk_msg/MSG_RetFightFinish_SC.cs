using System;
using System.Collections.Generic;
using System.ComponentModel;
using ProtoBuf;

namespace rankpk_msg
{
    [ProtoContract(Name = "MSG_RetFightFinish_SC")]
    [Serializable]
    public class MSG_RetFightFinish_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [DefaultValue(0L)]
        [ProtoMember(1, IsRequired = false, Name = "duration", DataFormat = DataFormat.TwosComplement)]
        public uint duration
        {
            get
            {
                return this._duration;
            }
            set
            {
                this._duration = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(2, IsRequired = false, Name = "winteamid", DataFormat = DataFormat.TwosComplement)]
        public uint winteamid
        {
            get
            {
                return this._winteamid;
            }
            set
            {
                this._winteamid = value;
            }
        }

        [ProtoMember(3, Name = "rewards", DataFormat = DataFormat.Default)]
        public List<RewardsNumber> rewards
        {
            get
            {
                return this._rewards;
            }
        }

        [ProtoMember(4, Name = "MeRankPKResult", DataFormat = DataFormat.Default)]
        public List<RankPKResult> MeRankPKResult
        {
            get
            {
                return this._MeRankPKResult;
            }
        }

        [ProtoMember(5, Name = "EnemyRankPKResult", DataFormat = DataFormat.Default)]
        public List<RankPKResult> EnemyRankPKResult
        {
            get
            {
                return this._EnemyRankPKResult;
            }
        }

        private uint _duration;

        private uint _winteamid;

        private readonly List<RewardsNumber> _rewards = new List<RewardsNumber>();

        private readonly List<RankPKResult> _MeRankPKResult = new List<RankPKResult>();

        private readonly List<RankPKResult> _EnemyRankPKResult = new List<RankPKResult>();

        private IExtension extensionObject;
    }
}
