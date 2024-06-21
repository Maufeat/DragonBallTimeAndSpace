using System;
using System.ComponentModel;
using ProtoBuf;

namespace rankpk_msg
{
    [ProtoContract(Name = "MSG_Ret_MatchResult_SC")]
    [Serializable]
    public class MSG_Ret_MatchResult_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, IsRequired = false, Name = "retcode", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
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
        [ProtoMember(2, IsRequired = false, Name = "lefttime", DataFormat = DataFormat.TwosComplement)]
        public uint lefttime
        {
            get
            {
                return this._lefttime;
            }
            set
            {
                this._lefttime = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(3, IsRequired = false, Name = "totalnum", DataFormat = DataFormat.TwosComplement)]
        public uint totalnum
        {
            get
            {
                return this._totalnum;
            }
            set
            {
                this._totalnum = value;
            }
        }

        private uint _retcode;

        private uint _lefttime;

        private uint _totalnum;

        private IExtension extensionObject;
    }
}
