using System;
using System.ComponentModel;
using ProtoBuf;

namespace quest
{
    [ProtoContract(Name = "MSG_ReqChangeMapFindPath_CS")]
    [Serializable]
    public class MSG_ReqChangeMapFindPath_CS : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, IsRequired = false, Name = "pathwayid", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint pathwayid
        {
            get
            {
                return this._pathwayid;
            }
            set
            {
                this._pathwayid = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(2, IsRequired = false, Name = "questid", DataFormat = DataFormat.TwosComplement)]
        public uint questid
        {
            get
            {
                return this._questid;
            }
            set
            {
                this._questid = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(3, IsRequired = false, Name = "destx", DataFormat = DataFormat.TwosComplement)]
        public uint destx
        {
            get
            {
                return this._destx;
            }
            set
            {
                this._destx = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(4, IsRequired = false, Name = "desty", DataFormat = DataFormat.TwosComplement)]
        public uint desty
        {
            get
            {
                return this._desty;
            }
            set
            {
                this._desty = value;
            }
        }

        private uint _pathwayid;

        private uint _questid;

        private uint _destx;

        private uint _desty;

        private IExtension extensionObject;
    }
}
