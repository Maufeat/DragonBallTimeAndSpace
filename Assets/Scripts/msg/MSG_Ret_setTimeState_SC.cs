using System;
using System.ComponentModel;
using ProtoBuf;

namespace msg
{
    [ProtoContract(Name = "MSG_Ret_setTimeState_SC")]
    [Serializable]
    public class MSG_Ret_setTimeState_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [DefaultValue(null)]
        [ProtoMember(1, IsRequired = false, Name = "target", DataFormat = DataFormat.Default)]
        public EntryIDType target
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

        [ProtoMember(2, IsRequired = false, Name = "state", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint state
        {
            get
            {
                return this._state;
            }
            set
            {
                this._state = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(3, IsRequired = false, Name = "maxtime", DataFormat = DataFormat.TwosComplement)]
        public uint maxtime
        {
            get
            {
                return this._maxtime;
            }
            set
            {
                this._maxtime = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(4, IsRequired = false, Name = "lefttime", DataFormat = DataFormat.TwosComplement)]
        public uint lefttime
        {
            get
            {
                return this._lefttime;
            }
            set
            {
                this._lefttime = value;
            }
        }

        private EntryIDType _target;

        private uint _state;

        private uint _maxtime;

        private uint _lefttime;

        private IExtension extensionObject;
    }
}
