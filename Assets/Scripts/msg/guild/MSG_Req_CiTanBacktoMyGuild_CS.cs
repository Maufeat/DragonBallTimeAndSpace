﻿using System;
using ProtoBuf;

namespace guild
{
    [ProtoContract(Name = "MSG_Req_CiTanBacktoMyGuild_CS")]
    [Serializable]
    public class MSG_Req_CiTanBacktoMyGuild_CS : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        private IExtension extensionObject;
    }
}
