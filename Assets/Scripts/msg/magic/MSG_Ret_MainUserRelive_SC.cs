using System;
using System.ComponentModel;
using msg;
using ProtoBuf;

namespace magic
{
    [ProtoContract(Name = "MSG_Ret_MainUserRelive_SC")]
    [Serializable]
    public class MSG_Ret_MainUserRelive_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [DefaultValue(0f)]
        [ProtoMember(1, IsRequired = false, Name = "userid", DataFormat = DataFormat.TwosComplement)]
        public ulong userid
        {
            get
            {
                return this._userid;
            }
            set
            {
                this._userid = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(2, IsRequired = false, Name = "x", DataFormat = DataFormat.TwosComplement)]
        public uint x
        {
            get
            {
                return this._x;
            }
            set
            {
                this._x = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(3, IsRequired = false, Name = "y", DataFormat = DataFormat.TwosComplement)]
        public uint y
        {
            get
            {
                return this._y;
            }
            set
            {
                this._y = value;
            }
        }

        [ProtoMember(4, IsRequired = true, Name = "relivetype", DataFormat = DataFormat.TwosComplement)]
        public ReliveType relivetype
        {
            get
            {
                return this._relivetype;
            }
            set
            {
                this._relivetype = value;
            }
        }

        private ulong _userid;

        private uint _x;

        private uint _y;

        private ReliveType _relivetype;

        private IExtension extensionObject;
    }
}
