using System;
using System.ComponentModel;
using ProtoBuf;

namespace npc
{
    [ProtoContract(Name = "MSG_Ret_Holdon_SC")]
    [Serializable]
    public class MSG_Ret_Holdon_SC : IExtensible
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

        [ProtoMember(2, IsRequired = false, Name = "npctype", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
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

        [DefaultValue(0L)]
        [ProtoMember(3, IsRequired = false, Name = "needtime", DataFormat = DataFormat.TwosComplement)]
        public uint needtime
        {
            get
            {
                return this._needtime;
            }
            set
            {
                this._needtime = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(4, IsRequired = false, Name = "retcode", DataFormat = DataFormat.TwosComplement)]
        public uint retcode
        {
            get
            {
                return this._retcode;
            }
            set
            {
                this._retcode = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(5, IsRequired = false, Name = "type", DataFormat = DataFormat.TwosComplement)]
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

        private uint _needtime;

        private uint _retcode;

        private uint _type;

        private IExtension extensionObject;
    }
}
