﻿using System;
using ProtoBuf;

namespace rankpk_msg
{
    [ProtoContract(Name = "MSG_Req_StartMatch_CS")]
    [Serializable]
    public class MSG_Req_StartMatch_CS : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        private IExtension extensionObject;
    }
}
