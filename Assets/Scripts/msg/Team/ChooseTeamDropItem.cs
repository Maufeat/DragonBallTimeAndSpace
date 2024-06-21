using System;
using System.ComponentModel;
using ProtoBuf;

namespace Team
{
    [ProtoContract(Name = "ChooseTeamDropItem")]
    [Serializable]
    public class ChooseTeamDropItem : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [DefaultValue("")]
        [ProtoMember(1, IsRequired = false, Name = "thisid", DataFormat = DataFormat.Default)]
        public string thisid
        {
            get
            {
                return this._thisid;
            }
            set
            {
                this._thisid = value;
            }
        }

        [DefaultValue(ChooseType.ChooseType_Need)]
        [ProtoMember(2, IsRequired = false, Name = "choose", DataFormat = DataFormat.TwosComplement)]
        public ChooseType choose
        {
            get
            {
                return this._choose;
            }
            set
            {
                this._choose = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(3, IsRequired = false, Name = "errcode", DataFormat = DataFormat.TwosComplement)]
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

        private string _thisid = string.Empty;

        private ChooseType _choose = ChooseType.ChooseType_Need;

        private uint _errcode;

        private IExtension extensionObject;
    }
}
