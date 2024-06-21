using System;
using System.ComponentModel;
using ProtoBuf;

namespace mobapk
{
    [ProtoContract(Name = "MSG_ServerTimer_SC")]
    [Serializable]
    public class MSG_ServerTimer_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, IsRequired = false, Name = "id", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(ServerTimer.MobaPk_Confirm_RestTime)]
        public ServerTimer id
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

        [DefaultValue("")]
        [ProtoMember(2, IsRequired = false, Name = "caption", DataFormat = DataFormat.Default)]
        public string caption
        {
            get
            {
                return this._caption;
            }
            set
            {
                this._caption = value;
            }
        }

        [ProtoMember(3, IsRequired = false, Name = "resttime", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint resttime
        {
            get
            {
                return this._resttime;
            }
            set
            {
                this._resttime = value;
            }
        }

        [ProtoMember(4, IsRequired = false, Name = "style", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint style
        {
            get
            {
                return this._style;
            }
            set
            {
                this._style = value;
            }
        }

        private ServerTimer _id = ServerTimer.MobaPk_Confirm_RestTime;

        private string _caption = string.Empty;

        private uint _resttime;

        private uint _style;

        private IExtension extensionObject;
    }
}
