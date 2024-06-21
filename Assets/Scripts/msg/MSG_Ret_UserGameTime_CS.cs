using System;
using System.ComponentModel;
using ProtoBuf;

namespace msg
{
    [ProtoContract(Name = "MSG_Ret_UserGameTime_CS")]
    [Serializable]
    public class MSG_Ret_UserGameTime_CS : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, IsRequired = false, Name = "usertempid", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint usertempid
        {
            get
            {
                return this._usertempid;
            }
            set
            {
                this._usertempid = value;
            }
        }

        [ProtoMember(2, IsRequired = false, Name = "gametime", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0f)]
        public ulong gametime
        {
            get
            {
                return this._gametime;
            }
            set
            {
                this._gametime = value;
            }
        }

        private uint _usertempid;

        private ulong _gametime;

        private IExtension extensionObject;
    }
}
