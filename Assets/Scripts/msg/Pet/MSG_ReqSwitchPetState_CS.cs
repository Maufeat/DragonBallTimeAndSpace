using System;
using System.ComponentModel;
using ProtoBuf;

namespace Pet
{
    [ProtoContract(Name = "MSG_ReqSwitchPetState_CS")]
    [Serializable]
    public class MSG_ReqSwitchPetState_CS : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, IsRequired = false, Name = "tempid", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0f)]
        public ulong tempid
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

        [ProtoMember(2, IsRequired = false, Name = "fromstate", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(PetState.PetState_None)]
        public PetState fromstate
        {
            get
            {
                return this._fromstate;
            }
            set
            {
                this._fromstate = value;
            }
        }

        [DefaultValue(PetState.PetState_None)]
        [ProtoMember(3, IsRequired = false, Name = "tostate", DataFormat = DataFormat.TwosComplement)]
        public PetState tostate
        {
            get
            {
                return this._tostate;
            }
            set
            {
                this._tostate = value;
            }
        }

        private ulong _tempid;

        private PetState _fromstate;

        private PetState _tostate;

        private IExtension extensionObject;
    }
}
