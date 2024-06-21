using System;
using System.ComponentModel;
using ProtoBuf;

namespace Subtitle
{
    [ProtoContract(Name = "SubtitleContent")]
    [Serializable]
    public class SubtitleContent : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [DefaultValue(0f)]
        [ProtoMember(1, IsRequired = false, Name = "showTime", DataFormat = DataFormat.FixedSize)]
        public float showTime
        {
            get
            {
                return this._showTime;
            }
            set
            {
                this._showTime = value;
            }
        }

        [DefaultValue("")]
        [ProtoMember(2, IsRequired = false, Name = "subtitle", DataFormat = DataFormat.Default)]
        public string subtitle
        {
            get
            {
                return this._subtitle;
            }
            set
            {
                this._subtitle = value;
            }
        }

        [ProtoMember(3, IsRequired = false, Name = "duration", DataFormat = DataFormat.FixedSize)]
        [DefaultValue(0f)]
        public float duration
        {
            get
            {
                return this._duration;
            }
            set
            {
                this._duration = value;
            }
        }

        private float _showTime;

        private string _subtitle = string.Empty;

        private float _duration;

        private IExtension extensionObject;
    }
}
