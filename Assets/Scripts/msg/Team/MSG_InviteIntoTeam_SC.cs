﻿using System;
using System.ComponentModel;
using ProtoBuf;

namespace Team
{
    [ProtoContract(Name = "MSG_InviteIntoTeam_SC")]
    [Serializable]
    public class MSG_InviteIntoTeam_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [DefaultValue(0L)]
        [ProtoMember(1, IsRequired = false, Name = "errcode", DataFormat = DataFormat.TwosComplement)]
        public uint errcode
        {
            get
            {
                return this._errcode;
            }
            set
            {
                this._errcode = value;
            }
        }

        private uint _errcode;

        private IExtension extensionObject;
    }
}
