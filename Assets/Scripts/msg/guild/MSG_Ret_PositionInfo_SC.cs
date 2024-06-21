using System;
using System.Collections.Generic;
using ProtoBuf;

namespace guild
{
    [ProtoContract(Name = "MSG_Ret_PositionInfo_SC")]
    [Serializable]
    public class MSG_Ret_PositionInfo_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, Name = "posinfos", DataFormat = DataFormat.Default)]
        public List<GuildPositionInfo> posinfos
        {
            get
            {
                return this._posinfos;
            }
        }

        private readonly List<GuildPositionInfo> _posinfos = new List<GuildPositionInfo>();

        private IExtension extensionObject;
    }
}
