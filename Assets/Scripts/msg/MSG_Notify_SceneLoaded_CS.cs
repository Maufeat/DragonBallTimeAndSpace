using System;
using System.ComponentModel;
using ProtoBuf;

namespace msg
{
    [ProtoContract(Name = "MSG_Notify_SceneLoaded_CS")]
    [Serializable]
    public class MSG_Notify_SceneLoaded_CS : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [DefaultValue(0f)]
        [ProtoMember(1, IsRequired = false, Name = "sceneid", DataFormat = DataFormat.TwosComplement)]
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

        private ulong _sceneid;

        private IExtension extensionObject;
    }
}
