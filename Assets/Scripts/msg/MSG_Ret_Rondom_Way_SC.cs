using System;
using System.ComponentModel;
using ProtoBuf;

namespace msg
{
    [ProtoContract(Name = "MSG_Ret_Rondom_Way_SC")]
    [Serializable]
    public class MSG_Ret_Rondom_Way_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [DefaultValue(0L)]
        [ProtoMember(1, IsRequired = false, Name = "zonecenterx", DataFormat = DataFormat.TwosComplement)]
        public uint zonecenterx
        {
            get
            {
                return this._zonecenterx;
            }
            set
            {
                this._zonecenterx = value;
            }
        }

        [ProtoMember(2, IsRequired = false, Name = "zonecentery", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint zonecentery
        {
            get
            {
                return this._zonecentery;
            }
            set
            {
                this._zonecentery = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(3, IsRequired = false, Name = "zonewidth", DataFormat = DataFormat.TwosComplement)]
        public uint zonewidth
        {
            get
            {
                return this._zonewidth;
            }
            set
            {
                this._zonewidth = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(4, IsRequired = false, Name = "zoneheight", DataFormat = DataFormat.TwosComplement)]
        public uint zoneheight
        {
            get
            {
                return this._zoneheight;
            }
            set
            {
                this._zoneheight = value;
            }
        }

        [ProtoMember(5, IsRequired = false, Name = "gridwidth", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint gridwidth
        {
            get
            {
                return this._gridwidth;
            }
            set
            {
                this._gridwidth = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(6, IsRequired = false, Name = "gridheight", DataFormat = DataFormat.TwosComplement)]
        public uint gridheight
        {
            get
            {
                return this._gridheight;
            }
            set
            {
                this._gridheight = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(7, IsRequired = false, Name = "pathid", DataFormat = DataFormat.TwosComplement)]
        public uint pathid
        {
            get
            {
                return this._pathid;
            }
            set
            {
                this._pathid = value;
            }
        }

        [ProtoMember(8, IsRequired = false, Name = "pathinfo", DataFormat = DataFormat.Default)]
        [DefaultValue("")]
        public string pathinfo
        {
            get
            {
                return this._pathinfo;
            }
            set
            {
                this._pathinfo = value;
            }
        }

        private uint _zonecenterx;

        private uint _zonecentery;

        private uint _zonewidth;

        private uint _zoneheight;

        private uint _gridwidth;

        private uint _gridheight;

        private uint _pathid;

        private string _pathinfo = string.Empty;

        private IExtension extensionObject;
    }
}
