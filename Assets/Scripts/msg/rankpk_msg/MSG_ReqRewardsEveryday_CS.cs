﻿using System;
using ProtoBuf;

namespace rankpk_msg
{
    [ProtoContract(Name = "MSG_ReqRewardsEveryday_CS")]
    [Serializable]
    public class MSG_ReqRewardsEveryday_CS : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        private IExtension extensionObject;
    }
}
