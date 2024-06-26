﻿using System;
using System.ComponentModel;
using ProtoBuf;

namespace massive
{
    [ProtoContract(Name = "MSG_Req_GuideOver_CS")]
    [Serializable]
    public class MSG_Req_GuideOver_CS : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [DefaultValue(0L)]
        [ProtoMember(1, IsRequired = false, Name = "guideid", DataFormat = DataFormat.TwosComplement)]
        public uint guideid
        {
            get
            {
                return this._guideid;
            }
            set
            {
                this._guideid = value;
            }
        }

        private uint _guideid;

        private IExtension extensionObject;
    }
}
