using System;
using System.ComponentModel;
using ProtoBuf;

namespace Team
{
    [ProtoContract(Name = "MemberPos")]
    [Serializable]
    public class MemberPos : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [DefaultValue("")]
        [ProtoMember(1, IsRequired = false, Name = "memberid", DataFormat = DataFormat.Default)]
        public string memberid
        {
            get
            {
                return this._memberid;
            }
            set
            {
                this._memberid = value;
            }
        }

        [DefaultValue("")]
        [ProtoMember(2, IsRequired = false, Name = "sceneid", DataFormat = DataFormat.Default)]
        public string sceneid
        {
            get
            {
                return this._sceneid;
            }
            set
            {
                this._sceneid = value;
            }
        }

        [ProtoMember(3, IsRequired = false, Name = "x", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
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
        [ProtoMember(4, IsRequired = false, Name = "y", DataFormat = DataFormat.TwosComplement)]
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

        private string _memberid = string.Empty;

        private string _sceneid = string.Empty;

        private uint _x;

        private uint _y;

        private IExtension extensionObject;
    }
}
