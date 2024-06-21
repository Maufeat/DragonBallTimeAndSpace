using System;
using System.ComponentModel;
using ProtoBuf;

namespace msg
{
    [ProtoContract(Name = "MSG_Create_Role_CS")]
    [Serializable]
    public class MSG_Create_Role_CS : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [DefaultValue("")]
        [ProtoMember(1, IsRequired = false, Name = "name", DataFormat = DataFormat.Default)]
        public string name
        {
            get
            {
                return this._name;
            }
            set
            {
                this._name = value;
            }
        }

        [ProtoMember(2, IsRequired = false, Name = "occupation", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint occupation
        {
            get
            {
                return this._occupation;
            }
            set
            {
                this._occupation = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(3, IsRequired = false, Name = "heroid", DataFormat = DataFormat.TwosComplement)]
        public uint heroid
        {
            get
            {
                return this._heroid;
            }
            set
            {
                this._heroid = value;
            }
        }

        [ProtoMember(4, IsRequired = true, Name = "sex", DataFormat = DataFormat.TwosComplement)]
        public SEX sex
        {
            get
            {
                return this._sex;
            }
            set
            {
                this._sex = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(5, IsRequired = false, Name = "facestyle", DataFormat = DataFormat.TwosComplement)]
        public uint facestyle
        {
            get
            {
                return this._facestyle;
            }
            set
            {
                this._facestyle = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(6, IsRequired = false, Name = "hairstyle", DataFormat = DataFormat.TwosComplement)]
        public uint hairstyle
        {
            get
            {
                return this._hairstyle;
            }
            set
            {
                this._hairstyle = value;
            }
        }

        [ProtoMember(7, IsRequired = false, Name = "haircolor", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint haircolor
        {
            get
            {
                return this._haircolor;
            }
            set
            {
                this._haircolor = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(8, IsRequired = false, Name = "antenna", DataFormat = DataFormat.TwosComplement)]
        public uint antenna
        {
            get
            {
                return this._antenna;
            }
            set
            {
                this._antenna = value;
            }
        }

        private string _name = string.Empty;

        private uint _occupation;

        private uint _heroid;

        private SEX _sex;

        private uint _facestyle;

        private uint _hairstyle;

        private uint _haircolor;

        private uint _antenna;

        private IExtension extensionObject;
    }
}
