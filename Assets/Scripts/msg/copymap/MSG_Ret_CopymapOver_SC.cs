using System;
using System.Collections.Generic;
using System.ComponentModel;
using massive;
using ProtoBuf;

namespace copymap
{
    [ProtoContract(Name = "MSG_Ret_CopymapOver_SC")]
    [Serializable]
    public class MSG_Ret_CopymapOver_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [DefaultValue(OverType.OVERMAP_ALLUSER_DEATH)]
        [ProtoMember(1, IsRequired = false, Name = "type", DataFormat = DataFormat.TwosComplement)]
        public OverType type
        {
            get
            {
                return this._type;
            }
            set
            {
                this._type = value;
            }
        }

        [ProtoMember(2, IsRequired = false, Name = "kickusertime", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint kickusertime
        {
            get
            {
                return this._kickusertime;
            }
            set
            {
                this._kickusertime = value;
            }
        }

        [ProtoMember(3, IsRequired = false, Name = "exp", DataFormat = DataFormat.TwosComplement)]
        [DefaultValue(0L)]
        public uint exp
        {
            get
            {
                return this._exp;
            }
            set
            {
                this._exp = value;
            }
        }

        [ProtoMember(4, Name = "money", DataFormat = DataFormat.Default)]
        public List<MSG_RetCurrencyChange_SC> money
        {
            get
            {
                return this._money;
            }
        }

        [DefaultValue(null)]
        [ProtoMember(5, IsRequired = false, Name = "items", DataFormat = DataFormat.Default)]
        public RewardsObjectInfo items
        {
            get
            {
                return this._items;
            }
            set
            {
                this._items = value;
            }
        }

        private OverType _type = OverType.OVERMAP_ALLUSER_DEATH;

        private uint _kickusertime;

        private uint _exp;

        private readonly List<MSG_RetCurrencyChange_SC> _money = new List<MSG_RetCurrencyChange_SC>();

        private RewardsObjectInfo _items;

        private IExtension extensionObject;
    }
}
