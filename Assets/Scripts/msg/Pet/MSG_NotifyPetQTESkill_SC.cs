using System;
using System.ComponentModel;
using ProtoBuf;

namespace Pet
{
    [ProtoContract(Name = "MSG_NotifyPetQTESkill_SC")]
    [Serializable]
    public class MSG_NotifyPetQTESkill_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, IsRequired = false, Name = "onoff", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint onoff
        {
            get
            {
                return this._onoff;
            }
            set
            {
                this._onoff = value;
            }
        }

        [DefaultValue(0f)]
        [ProtoMember(2, IsRequired = false, Name = "bosstempid", DataFormat = DataFormat.TwosComplement)]
        public ulong bosstempid
        {
            get
            {
                return this._bosstempid;
            }
            set
            {
                this._bosstempid = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(3, IsRequired = false, Name = "leftlasttime", DataFormat = DataFormat.TwosComplement)]
        public uint leftlasttime
        {
            get
            {
                return this._leftlasttime;
            }
            set
            {
                this._leftlasttime = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(4, IsRequired = false, Name = "totallasttime", DataFormat = DataFormat.TwosComplement)]
        public uint totallasttime
        {
            get
            {
                return this._totallasttime;
            }
            set
            {
                this._totallasttime = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(5, IsRequired = false, Name = "distancetomaster", DataFormat = DataFormat.TwosComplement)]
        public uint distancetomaster
        {
            get
            {
                return this._distancetomaster;
            }
            set
            {
                this._distancetomaster = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(6, IsRequired = false, Name = "distanceratio", DataFormat = DataFormat.TwosComplement)]
        public uint distanceratio
        {
            get
            {
                return this._distanceratio;
            }
            set
            {
                this._distanceratio = value;
            }
        }

        private uint _onoff;

        private ulong _bosstempid;

        private uint _leftlasttime;

        private uint _totallasttime;

        private uint _distancetomaster;

        private uint _distanceratio;

        private IExtension extensionObject;
    }
}
