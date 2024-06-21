using System;
using System.ComponentModel;
using ProtoBuf;

namespace quiz
{
    [ProtoContract(Name = "Day7ActivityInfo")]
    [Serializable]
    public class Day7ActivityInfo : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [DefaultValue(0L)]
        [ProtoMember(1, IsRequired = false, Name = "dayid", DataFormat = DataFormat.TwosComplement)]
        public uint dayid
        {
            get
            {
                return this._dayid;
            }
            set
            {
                this._dayid = value;
            }
        }

        [ProtoMember(2, IsRequired = false, Name = "state", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(ActivityState.ACTIVITY_STATE_UNOPEN)]
        public ActivityState state
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

        private uint _dayid;

        private ActivityState _state;

        private IExtension extensionObject;
    }
}
