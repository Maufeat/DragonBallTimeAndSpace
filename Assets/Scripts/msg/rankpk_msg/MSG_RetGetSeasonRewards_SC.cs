using System;
using System.ComponentModel;
using ProtoBuf;

namespace rankpk_msg
{
    [ProtoContract(Name = "MSG_RetGetSeasonRewards_SC")]
    [Serializable]
    public class MSG_RetGetSeasonRewards_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [DefaultValue(false)]
        [ProtoMember(1, IsRequired = false, Name = "season_rewards_received", DataFormat = DataFormat.Default)]
        public bool season_rewards_received
        {
            get
            {
                return this._season_rewards_received;
            }
            set
            {
                this._season_rewards_received = value;
            }
        }

        private bool _season_rewards_received;

        private IExtension extensionObject;
    }
}
