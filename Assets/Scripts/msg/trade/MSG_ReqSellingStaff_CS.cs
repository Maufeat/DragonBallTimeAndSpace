﻿using System;
using ProtoBuf;

namespace trade
{
    [ProtoContract(Name = "MSG_ReqSellingStaff_CS")]
    [Serializable]
    public class MSG_ReqSellingStaff_CS : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        private IExtension extensionObject;
    }
}
