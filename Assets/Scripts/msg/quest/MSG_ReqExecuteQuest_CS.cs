using System;
using System.ComponentModel;
using ProtoBuf;

namespace quest
{
    [ProtoContract(Name = "MSG_ReqExecuteQuest_CS")]
    [Serializable]
    public class MSG_ReqExecuteQuest_CS : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [DefaultValue(0L)]
        [ProtoMember(1, IsRequired = false, Name = "id", DataFormat = DataFormat.TwosComplement)]
        public uint id
        {
            get
            {
                return this._id;
            }
            set
            {
                this._id = value;
            }
        }

        [ProtoMember(2, IsRequired = false, Name = "target", DataFormat = DataFormat.Default)]
        [DefaultValue("")]
        public string target
        {
            get
            {
                return this._target;
            }
            set
            {
                this._target = value;
            }
        }

        [ProtoMember(3, IsRequired = false, Name = "offset", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint offset
        {
            get
            {
                return this._offset;
            }
            set
            {
                this._offset = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(4, IsRequired = false, Name = "questdesccrc", DataFormat = DataFormat.TwosComplement)]
        public uint questdesccrc
        {
            get
            {
                return this._questdesccrc;
            }
            set
            {
                this._questdesccrc = value;
            }
        }

        [DefaultValue(0f)]
        [ProtoMember(5, IsRequired = false, Name = "chartarget", DataFormat = DataFormat.TwosComplement)]
        public ulong chartarget
        {
            get
            {
                return this._chartarget;
            }
            set
            {
                this._chartarget = value;
            }
        }

        private uint _id;

        private string _target = string.Empty;

        private uint _offset;

        private uint _questdesccrc;

        private ulong _chartarget;

        private IExtension extensionObject;
    }
}
