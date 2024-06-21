using System;
using System.ComponentModel;
using ProtoBuf;

namespace npc
{
    [ProtoContract(Name = "MSG_Req_Holdon_CS")]
    [Serializable]
    public class MSG_Req_Holdon_CS : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, IsRequired = false, Name = "thisid", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0f)]
        public ulong thisid
        {
            get
            {
                return this._thisid;
            }
            set
            {
                this._thisid = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(2, IsRequired = false, Name = "npctype", DataFormat = DataFormat.TwosComplement)]
        public uint npctype
        {
            get
            {
                return this._npctype;
            }
            set
            {
                this._npctype = value;
            }
        }

        [ProtoMember(3, IsRequired = false, Name = "type", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint type
        {
            get
            {
                return this._type;
            }
            set
            {
                this._type = value;
            }
        }

        private ulong _thisid;

        private uint _npctype;

        private uint _type;

        private IExtension extensionObject;
    }
}
