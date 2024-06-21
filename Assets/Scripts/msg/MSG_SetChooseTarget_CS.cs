using System;
using System.ComponentModel;
using ProtoBuf;

namespace msg
{
    [ProtoContract(Name = "MSG_SetChooseTarget_CS")]
    [Serializable]
    public class MSG_SetChooseTarget_CS : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, IsRequired = false, Name = "charid", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0f)]
        public ulong charid
        {
            get
            {
                return this._charid;
            }
            set
            {
                this._charid = value;
            }
        }

        [DefaultValue(ChooseTargetType.CHOOSE_TARGE_TTYPE_SET)]
        [ProtoMember(2, IsRequired = false, Name = "choosetype", DataFormat = DataFormat.TwosComplement)]
        public ChooseTargetType choosetype
        {
            get
            {
                return this._choosetype;
            }
            set
            {
                this._choosetype = value;
            }
        }

        [ProtoMember(3, IsRequired = true, Name = "mapdatatype", DataFormat = DataFormat.TwosComplement)]
        public MapDataType mapdatatype
        {
            get
            {
                return this._mapdatatype;
            }
            set
            {
                this._mapdatatype = value;
            }
        }

        private ulong _charid;

        private ChooseTargetType _choosetype = ChooseTargetType.CHOOSE_TARGE_TTYPE_SET;

        private MapDataType _mapdatatype;

        private IExtension extensionObject;
    }
}
