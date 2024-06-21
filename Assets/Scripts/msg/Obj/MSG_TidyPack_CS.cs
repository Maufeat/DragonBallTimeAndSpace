using System;
using System.Collections.Generic;
using ProtoBuf;

namespace Obj
{
    [ProtoContract(Name = "MSG_TidyPack_CS")]
    [Serializable]
    public class MSG_TidyPack_CS : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, IsRequired = true, Name = "packtype", DataFormat = DataFormat.TwosComplement)]
        public PackType packtype
        {
            get
            {
                return this._packtype;
            }
            set
            {
                this._packtype = value;
            }
        }

        [ProtoMember(2, Name = "infos", DataFormat = DataFormat.Default)]
        public List<t_TidyPackInfo> infos
        {
            get
            {
                return this._infos;
            }
        }

        private PackType _packtype;

        private readonly List<t_TidyPackInfo> _infos = new List<t_TidyPackInfo>();

        private IExtension extensionObject;
    }
}
