using System;
using System.ComponentModel;
using ProtoBuf;

namespace quest
{
    [ProtoContract(Name = "MSG_Ret_OutOfCircle_SC")]
    [Serializable]
    public class MSG_Ret_OutOfCircle_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [DefaultValue(0L)]
        [ProtoMember(1, IsRequired = false, Name = "npcid", DataFormat = DataFormat.TwosComplement)]
        public uint npcid
        {
            get
            {
                return this._npcid;
            }
            set
            {
                this._npcid = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(2, IsRequired = false, Name = "tipid", DataFormat = DataFormat.TwosComplement)]
        public uint tipid
        {
            get
            {
                return this._tipid;
            }
            set
            {
                this._tipid = value;
            }
        }

        [ProtoMember(3, IsRequired = false, Name = "state", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint state
        {
            get
            {
                return this._state;
            }
            set
            {
                this._state = value;
            }
        }

        private uint _npcid;

        private uint _tipid;

        private uint _state;

        private IExtension extensionObject;
    }
}
