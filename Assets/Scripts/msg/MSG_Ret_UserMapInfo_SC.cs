using System;
using System.ComponentModel;
using ProtoBuf;

namespace msg
{
    [ProtoContract(Name = "MSG_Ret_UserMapInfo_SC")]
    [Serializable]
    public class MSG_Ret_UserMapInfo_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [DefaultValue(0L)]
        [ProtoMember(1, IsRequired = false, Name = "mapid", DataFormat = DataFormat.TwosComplement)]
        public uint mapid
        {
            get
            {
                return this._mapid;
            }
            set
            {
                this._mapid = value;
            }
        }

        [ProtoMember(2, IsRequired = false, Name = "mapname", DataFormat = DataFormat.Default)]
        [DefaultValue("")]
        public string mapname
        {
            get
            {
                return this._mapname;
            }
            set
            {
                this._mapname = value;
            }
        }

        [ProtoMember(3, IsRequired = false, Name = "filename", DataFormat = DataFormat.Default)]
        [DefaultValue("")]
        public string filename
        {
            get
            {
                return this._filename;
            }
            set
            {
                this._filename = value;
            }
        }

        [ProtoMember(4, IsRequired = false, Name = "pos", DataFormat = DataFormat.Default)]
        [DefaultValue(null)]
        public FloatMovePos pos
        {
            get
            {
                return this._pos;
            }
            set
            {
                this._pos = value;
            }
        }

        [ProtoMember(5, IsRequired = false, Name = "lineid", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint lineid
        {
            get
            {
                return this._lineid;
            }
            set
            {
                this._lineid = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(6, IsRequired = false, Name = "copymapidx", DataFormat = DataFormat.TwosComplement)]
        public uint copymapidx
        {
            get
            {
                return this._copymapidx;
            }
            set
            {
                this._copymapidx = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(7, IsRequired = false, Name = "subcopymapidx", DataFormat = DataFormat.TwosComplement)]
        public uint subcopymapidx
        {
            get
            {
                return this._subcopymapidx;
            }
            set
            {
                this._subcopymapidx = value;
            }
        }

        [DefaultValue(0f)]
        [ProtoMember(8, IsRequired = false, Name = "sceneid", DataFormat = DataFormat.TwosComplement)]
        public ulong sceneid
        {
            get
            {
                return this._sceneid;
            }
            set
            {
                this._sceneid = value;
            }
        }

        private uint _mapid;

        private string _mapname = string.Empty;

        private string _filename = string.Empty;

        private FloatMovePos _pos;

        private uint _lineid;

        private uint _copymapidx;

        private uint _subcopymapidx;

        private ulong _sceneid;

        private IExtension extensionObject;
    }
}
