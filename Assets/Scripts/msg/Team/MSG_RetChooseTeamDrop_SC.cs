﻿using System;
using System.Collections.Generic;
using ProtoBuf;

namespace Team
{
    [ProtoContract(Name = "MSG_RetChooseTeamDrop_SC")]
    [Serializable]
    public class MSG_RetChooseTeamDrop_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, Name = "item", DataFormat = DataFormat.Default)]
        public List<ChooseTeamDropItem> item
        {
            get
            {
                return this._item;
            }
        }

        private readonly List<ChooseTeamDropItem> _item = new List<ChooseTeamDropItem>();

        private IExtension extensionObject;
    }
}