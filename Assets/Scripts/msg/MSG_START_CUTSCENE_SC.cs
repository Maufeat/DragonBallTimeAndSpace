using System;
using System.ComponentModel;
using ProtoBuf;

namespace msg
{
    [ProtoContract(Name = "MSG_START_CUTSCENE_SC")]
    [Serializable]
    public class MSG_START_CUTSCENE_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [DefaultValue(0L)]
        [ProtoMember(1, IsRequired = false, Name = "cutsceneid", DataFormat = DataFormat.TwosComplement)]
        public uint cutsceneid
        {
            get
            {
                return this._cutsceneid;
            }
            set
            {
                this._cutsceneid = value;
            }
        }

        [ProtoMember(2, IsRequired = false, Name = "onfinish", DataFormat = DataFormat.Default)]
        [DefaultValue("")]
        public string onfinish
        {
            get
            {
                return this._onfinish;
            }
            set
            {
                this._onfinish = value;
            }
        }

        private uint _cutsceneid;

        private string _onfinish = string.Empty;

        private IExtension extensionObject;
    }
}
