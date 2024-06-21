using System;
using System.ComponentModel;
using ProtoBuf;

namespace guild
{
    [ProtoContract(Name = "MSG_Ret_ChoosedCiTanEnemyGuild_SC")]
    [Serializable]
    public class MSG_Ret_ChoosedCiTanEnemyGuild_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, IsRequired = false, Name = "guild", DataFormat = DataFormat.Default)]
        [DefaultValue(null)]
        public CiTanEnemyGuildItem guild
        {
            get
            {
                return this._guild;
            }
            set
            {
                this._guild = value;
            }
        }

        private CiTanEnemyGuildItem _guild;

        private IExtension extensionObject;
    }
}
