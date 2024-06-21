using System;
using System.ComponentModel;
using ProtoBuf;

namespace rankpk_msg
{
    [ProtoContract(Name = "MSG_RetTeamCurScore_SC")]
    [Serializable]
    public class MSG_RetTeamCurScore_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, IsRequired = false, Name = "team1id", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint team1id
        {
            get
            {
                return this._team1id;
            }
            set
            {
                this._team1id = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(2, IsRequired = false, Name = "team1score", DataFormat = DataFormat.TwosComplement)]
        public uint team1score
        {
            get
            {
                return this._team1score;
            }
            set
            {
                this._team1score = value;
            }
        }

        [ProtoMember(3, IsRequired = false, Name = "team2id", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint team2id
        {
            get
            {
                return this._team2id;
            }
            set
            {
                this._team2id = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(4, IsRequired = false, Name = "team2score", DataFormat = DataFormat.TwosComplement)]
        public uint team2score
        {
            get
            {
                return this._team2score;
            }
            set
            {
                this._team2score = value;
            }
        }

        private uint _team1id;

        private uint _team1score;

        private uint _team2id;

        private uint _team2score;

        private IExtension extensionObject;
    }
}
