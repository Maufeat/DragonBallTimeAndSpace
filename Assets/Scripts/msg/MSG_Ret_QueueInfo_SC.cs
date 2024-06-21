using System;
using System.ComponentModel;
using ProtoBuf;

namespace msg
{
    [ProtoContract(Name = "MSG_Ret_QueueInfo_SC")]
    [Serializable]
    public class MSG_Ret_QueueInfo_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [DefaultValue(0L)]
        [ProtoMember(1, IsRequired = false, Name = "queue_user_num", DataFormat = DataFormat.TwosComplement)]
        public uint queue_user_num
        {
            get
            {
                return this._queue_user_num;
            }
            set
            {
                this._queue_user_num = value;
            }
        }

        [ProtoMember(2, IsRequired = false, Name = "queue_wait_time", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint queue_wait_time
        {
            get
            {
                return this._queue_wait_time;
            }
            set
            {
                this._queue_wait_time = value;
            }
        }

        private uint _queue_user_num;

        private uint _queue_wait_time;

        private IExtension extensionObject;
    }
}
