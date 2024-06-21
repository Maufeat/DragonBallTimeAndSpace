﻿using System;
using System.ComponentModel;
using msg;
using ProtoBuf;

namespace Team
{
    [ProtoContract(Name = "MSG_RetLeaderAttackTarget_SC")]
    [Serializable]
    public class MSG_RetLeaderAttackTarget_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [DefaultValue(null)]
        [ProtoMember(1, IsRequired = false, Name = "target", DataFormat = DataFormat.Default)]
        public EntryIDType target
        {
            get
            {
                return this._target;
            }
            set
            {
                this._target = value;
            }
        }

        private EntryIDType _target;

        private IExtension extensionObject;
    }
}