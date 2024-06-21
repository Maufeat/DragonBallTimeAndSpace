using System;
using System.ComponentModel;
using ProtoBuf;

namespace rankpk_msg
{
    [ProtoContract(Name = "MSG_RetTeamLeftMemSize_SC")]
    [Serializable]
    public class MSG_RetTeamLeftMemSize_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [DefaultValue(0L)]
        [ProtoMember(1, IsRequired = false, Name = "team1id", DataFormat = DataFormat.TwosComplement)]
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

        [ProtoMember(2, IsRequired = false, Name = "team1left", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint team1left
        {
            get
            {
                return this._team1left;
            }
            set
            {
                this._team1left = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(3, IsRequired = false, Name = "team2id", DataFormat = DataFormat.TwosComplement)]
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
        [ProtoMember(4, IsRequired = false, Name = "team2left", DataFormat = DataFormat.TwosComplement)]
        public uint team2left
        {
            get
            {
                return this._team2left;
            }
            set
            {
                this._team2left = value;
            }
        }

        private uint _team1id;

        private uint _team1left;

        private uint _team2id;

        private uint _team2left;

        private IExtension extensionObject;
    }
}
