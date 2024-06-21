using System;
using System.Collections.Generic;
using ProtoBuf;

namespace Pet
{
    [ProtoContract(Name = "MSG_RetRefreshSummonPet_SC")]
    [Serializable]
    public class MSG_RetRefreshSummonPet_SC : IExtensible
    {
        IExtension IExtensible.GetExtensionObject(bool createIfMissing)
        {
            return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
        }

        [ProtoMember(1, Name = "pet", DataFormat = DataFormat.Default)]
        public List<PetBase> pet
        {
            get
            {
                return this._pet;
            }
        }

        private readonly List<PetBase> _pet = new List<PetBase>();

        private IExtension extensionObject;
    }
}
