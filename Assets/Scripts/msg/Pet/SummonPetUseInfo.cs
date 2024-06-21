using System;
using System.Collections.Generic;
using System.ComponentModel;
using ProtoBuf;

namespace Pet
{
    [ProtoContract(Name = "SummonPetUseInfo")]
    [Serializable]
    public class SummonPetUseInfo : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [DefaultValue(0L)]
        [ProtoMember(1, IsRequired = false, Name = "num", DataFormat = DataFormat.TwosComplement)]
        public uint num
        {
            get
            {
                return this._num;
            }
            set
            {
                this._num = value;
            }
        }

        [ProtoMember(2, IsRequired = false, Name = "curpet", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0f)]
        public ulong curpet
        {
            get
            {
                return this._curpet;
            }
            set
            {
                this._curpet = value;
            }
        }

        [ProtoMember(3, Name = "petlist", DataFormat = DataFormat.Default)]
        public List<PetBase> petlist
        {
            get
            {
                return this._petlist;
            }
        }

        [ProtoMember(4, IsRequired = false, Name = "unlockcount", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint unlockcount
        {
            get
            {
                return this._unlockcount;
            }
            set
            {
                this._unlockcount = value;
            }
        }

        [DefaultValue(0f)]
        [ProtoMember(5, IsRequired = false, Name = "curassist", DataFormat = DataFormat.TwosComplement)]
        public ulong curassist
        {
            get
            {
                return this._curassist;
            }
            set
            {
                this._curassist = value;
            }
        }

        private uint _num;

        private ulong _curpet;

        private readonly List<PetBase> _petlist = new List<PetBase>();

        private uint _unlockcount;

        private ulong _curassist;

        private IExtension extensionObject;
    }
}
