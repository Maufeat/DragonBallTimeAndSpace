using System;
using System.ComponentModel;
using Obj;
using ProtoBuf;

namespace npc
{
    [ProtoContract(Name = "ObjItem")]
    [Serializable]
    public class ObjItem : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [DefaultValue(0L)]
        [ProtoMember(1, IsRequired = false, Name = "thisid", DataFormat = DataFormat.TwosComplement)]
        public uint thisid
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

        [ProtoMember(2, IsRequired = false, Name = "obj", DataFormat = DataFormat.Default)]
        [DefaultValue(null)]
        public t_Object obj
        {
            get
            {
                return this._obj;
            }
            set
            {
                this._obj = value;
            }
        }

        private uint _thisid;

        private t_Object _obj;

        private IExtension extensionObject;
    }
}
