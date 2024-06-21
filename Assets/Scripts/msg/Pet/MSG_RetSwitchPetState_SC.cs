using System;
using System.Collections.Generic;
using System.ComponentModel;
using ProtoBuf;

namespace Pet
{
    [ProtoContract(Name = "MSG_RetSwitchPetState_SC")]
    [Serializable]
    public class MSG_RetSwitchPetState_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, IsRequired = false, Name = "errcode", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint errcode
        {
            get
            {
                return this._errcode;
            }
            set
            {
                this._errcode = value;
            }
        }

        [ProtoMember(2, IsRequired = false, Name = "tempid", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint tempid
        {
            get
            {
                return this._tempid;
            }
            set
            {
                this._tempid = value;
            }
        }

        [ProtoMember(3, Name = "curstate", DataFormat = DataFormat.TwosComplement)]
        public List<PetState> curstate
        {
            get
            {
                return this._curstate;
            }
        }

        private uint _errcode;

        private uint _tempid;

        private readonly List<PetState> _curstate = new List<PetState>();

        private IExtension extensionObject;
    }
}
