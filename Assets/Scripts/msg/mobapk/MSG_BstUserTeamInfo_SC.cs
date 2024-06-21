using System;
using System.Collections.Generic;
using ProtoBuf;

namespace mobapk
{
    [ProtoContract(Name = "MSG_BstUserTeamInfo_SC")]
    [Serializable]
    public class MSG_BstUserTeamInfo_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, Name = "infos", DataFormat = DataFormat.Default)]
        public List<UserTeamInfo> infos
        {
            get
            {
                return this._infos;
            }
        }

        private readonly List<UserTeamInfo> _infos = new List<UserTeamInfo>();

        private IExtension extensionObject;
    }
}
