using System;
using System.ComponentModel;
using ProtoBuf;

namespace quest
{
    [ProtoContract(Name = "UnorderQuestBranchInfo")]
    [Serializable]
    public class UnorderQuestBranchInfo : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, IsRequired = false, Name = "degreevar", DataFormat = DataFormat.Default)]
        [DefaultValue("")]
        public string degreevar
        {
            get
            {
                return this._degreevar;
            }
            set
            {
                this._degreevar = value;
            }
        }

        [ProtoMember(2, IsRequired = false, Name = "curvalue", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint curvalue
        {
            get
            {
                return this._curvalue;
            }
            set
            {
                this._curvalue = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(3, IsRequired = false, Name = "maxvalue", DataFormat = DataFormat.TwosComplement)]
        public uint maxvalue
        {
            get
            {
                return this._maxvalue;
            }
            set
            {
                this._maxvalue = value;
            }
        }

        private string _degreevar = string.Empty;

        private uint _curvalue;

        private uint _maxvalue;

        private IExtension extensionObject;
    }
}
