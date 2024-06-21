using System;
using System.ComponentModel;
using ProtoBuf;

namespace msg
{
    [ProtoContract(Name = "CharacterMainData")]
    [Serializable]
    public class CharacterMainData : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [DefaultValue(null)]
        [ProtoMember(1, IsRequired = false, Name = "basedata", DataFormat = DataFormat.Default)]
        public CharacterBaseData basedata
        {
            get
            {
                return this._basedata;
            }
            set
            {
                this._basedata = value;
            }
        }

        [DefaultValue(null)]
        [ProtoMember(2, IsRequired = false, Name = "attridata", DataFormat = DataFormat.Default)]
        public AttributeData attridata
        {
            get
            {
                return this._attridata;
            }
            set
            {
                this._attridata = value;
            }
        }

        [ProtoMember(3, IsRequired = false, Name = "mapdata", DataFormat = DataFormat.Default)]
        [DefaultValue(null)]
        public MapUserData mapdata
        {
            get
            {
                return this._mapdata;
            }
            set
            {
                this._mapdata = value;
            }
        }

        [ProtoMember(4, IsRequired = false, Name = "fightdata", DataFormat = DataFormat.Default)]
        [DefaultValue(null)]
        public CharacterFightData fightdata
        {
            get
            {
                return this._fightdata;
            }
            set
            {
                this._fightdata = value;
            }
        }

        private CharacterBaseData _basedata;

        private AttributeData _attridata;

        private MapUserData _mapdata;

        private CharacterFightData _fightdata;

        private IExtension extensionObject;
    }
}
