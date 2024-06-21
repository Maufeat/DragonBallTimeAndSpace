using System;
using System.Collections.Generic;
using System.ComponentModel;
using ProtoBuf;

namespace career
{
    [ProtoContract(Name = "CareerSkillInfo")]
    [Serializable]
    public class CareerSkillInfo : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, Name = "unlockskills", DataFormat = DataFormat.Default)]
        public List<careerunlockItem> unlockskills
        {
            get
            {
                return this._unlockskills;
            }
        }

        [ProtoMember(2, Name = "curskills", DataFormat = DataFormat.Default)]
        public List<lineSkillItem> curskills
        {
            get
            {
                return this._curskills;
            }
        }

        [DefaultValue(0L)]
        [ProtoMember(3, IsRequired = false, Name = "curcareer", DataFormat = DataFormat.TwosComplement)]
        public uint curcareer
        {
            get
            {
                return this._curcareer;
            }
            set
            {
                this._curcareer = value;
            }
        }

        private readonly List<careerunlockItem> _unlockskills = new List<careerunlockItem>();

        private readonly List<lineSkillItem> _curskills = new List<lineSkillItem>();

        private uint _curcareer;

        private IExtension extensionObject;
    }
}
