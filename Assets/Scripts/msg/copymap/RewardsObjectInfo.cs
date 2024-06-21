using System;
using System.Collections.Generic;
using System.ComponentModel;
using Obj;
using ProtoBuf;

namespace copymap
{
    [ProtoContract(Name = "RewardsObjectInfo")]
    [Serializable]
    public class RewardsObjectInfo : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, Name = "objs", DataFormat = DataFormat.Default)]
        public List<t_Object> objs
        {
            get
            {
                return this._objs;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(2, IsRequired = false, Name = "userid", DataFormat = DataFormat.TwosComplement)]
        public uint userid
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

        private readonly List<t_Object> _objs = new List<t_Object>();

        private uint _userid;

        private IExtension extensionObject;
    }
}
