using System;
using System.ComponentModel;
using ProtoBuf;

namespace mobapk
{
    [ProtoContract(Name = "MSG_MobaLevelUp_SC")]
    [Serializable]
    public class MSG_MobaLevelUp_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, IsRequired = false, Name = "oldlevel", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint oldlevel
        {
            get
            {
                return this._oldlevel;
            }
            set
            {
                this._oldlevel = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(2, IsRequired = false, Name = "newlevel", DataFormat = DataFormat.TwosComplement)]
        public uint newlevel
        {
            get
            {
                return this._newlevel;
            }
            set
            {
                this._newlevel = value;
            }
        }

        [ProtoMember(3, IsRequired = false, Name = "uid", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0f)]
        public ulong uid
        {
            get
            {
                return this._uid;
            }
            set
            {
                this._uid = value;
            }
        }

        [ProtoMember(4, IsRequired = false, Name = "type", DataFormat = DataFormat.TwosComplement)]
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

        private uint _oldlevel;

        private uint _newlevel;

        private ulong _uid;

        private uint _type;

        private IExtension extensionObject;
    }
}
