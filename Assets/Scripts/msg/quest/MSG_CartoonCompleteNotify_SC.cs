using System;
using System.ComponentModel;
using ProtoBuf;

namespace quest
{
    [ProtoContract(Name = "MSG_CartoonCompleteNotify_SC")]
    [Serializable]
    public class MSG_CartoonCompleteNotify_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [DefaultValue(0L)]
        [ProtoMember(1, IsRequired = false, Name = "groupid", DataFormat = DataFormat.TwosComplement)]
        public uint groupid
        {
            get
            {
                return this._groupid;
            }
            set
            {
                this._groupid = value;
            }
        }

        [DefaultValue("")]
        [ProtoMember(2, IsRequired = false, Name = "command", DataFormat = DataFormat.Default)]
        public string command
        {
            get
            {
                return this._command;
            }
            set
            {
                this._command = value;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(3, IsRequired = false, Name = "delay", DataFormat = DataFormat.TwosComplement)]
        public uint delay
        {
            get
            {
                return this._delay;
            }
            set
            {
                this._delay = value;
            }
        }

        private uint _groupid;

        private string _command = string.Empty;

        private uint _delay;

        private IExtension extensionObject;
    }
}
