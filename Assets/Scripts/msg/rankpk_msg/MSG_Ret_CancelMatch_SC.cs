﻿using System;
using System.ComponentModel;
using ProtoBuf;

namespace rankpk_msg
{
    [ProtoContract(Name = "MSG_Ret_CancelMatch_SC")]
    [Serializable]
    public class MSG_Ret_CancelMatch_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [DefaultValue(0L)]
        [ProtoMember(1, IsRequired = false, Name = "retcode", DataFormat = DataFormat.TwosComplement)]
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

        private uint _retcode;

        private IExtension extensionObject;
    }
}
