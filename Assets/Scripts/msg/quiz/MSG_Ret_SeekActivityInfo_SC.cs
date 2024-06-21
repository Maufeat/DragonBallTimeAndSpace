using System;
using System.ComponentModel;
using ProtoBuf;

namespace quiz
{
    [ProtoContract(Name = "MSG_Ret_SeekActivityInfo_SC")]
    [Serializable]
    public class MSG_Ret_SeekActivityInfo_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, IsRequired = false, Name = "state", DataFormat = DataFormat.TwosComplement)]
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

        [DefaultValue(0L)]
        [ProtoMember(2, IsRequired = false, Name = "id", DataFormat = DataFormat.TwosComplement)]
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

        private ActivityState _state;

        private uint _id;

        private IExtension extensionObject;
    }
}
