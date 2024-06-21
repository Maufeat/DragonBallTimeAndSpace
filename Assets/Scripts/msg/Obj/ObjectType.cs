using System;
using ProtoBuf;

namespace Obj
{
    [ProtoContract(Name = "ObjectType")]
    public enum ObjectType
    {
        [ProtoEnum(Name = "OBJTYPE_WEAPON_NPC", Value = 0)]
        OBJTYPE_WEAPON_NPC,
        [ProtoEnum(Name = "OBJTYPE_WEAPON_MIN", Value = 1)]
        OBJTYPE_WEAPON_MIN,
        [ProtoEnum(Name = "OBJTYPE_SINGLE_SWORD", Value = 1)]
        OBJTYPE_SINGLE_SWORD = 1,
        [ProtoEnum(Name = "OBJTYPE_TWO_DAGGER", Value = 2)]
        OBJTYPE_TWO_DAGGER,
        [ProtoEnum(Name = "OBJTYPE_DOUBLE_GREATSWORD", Value = 3)]
        OBJTYPE_DOUBLE_GREATSWORD,
        [ProtoEnum(Name = "OBJTYPE_MAGIC_BALL", Value = 4)]
        OBJTYPE_MAGIC_BALL,
        [ProtoEnum(Name = "OBJTYPE_WEAPON_MAX", Value = 20)]
        OBJTYPE_WEAPON_MAX = 20,
        [ProtoEnum(Name = "OBJTYPE_EQUIP_MIN", Value = 21)]
        OBJTYPE_EQUIP_MIN,
        [ProtoEnum(Name = "OBJTYPE_EQUIP_HEAD", Value = 21)]
        OBJTYPE_EQUIP_HEAD = 21,
        [ProtoEnum(Name = "OBJTYPE_EQUIP_SHOULDER", Value = 22)]
        OBJTYPE_EQUIP_SHOULDER,
        [ProtoEnum(Name = "OBJTYPE_EQUIP_CHEST", Value = 23)]
        OBJTYPE_EQUIP_CHEST,
        [ProtoEnum(Name = "OBJTYPE_EQUIP_HAND", Value = 24)]
        OBJTYPE_EQUIP_HAND,
        [ProtoEnum(Name = "OBJTYPE_EQUIP_WAIST", Value = 25)]
        OBJTYPE_EQUIP_WAIST,
        [ProtoEnum(Name = "OBJTYPE_EQUIP_LEG", Value = 26)]
        OBJTYPE_EQUIP_LEG,
        [ProtoEnum(Name = "OBJTYPE_EQUIP_FOOT", Value = 27)]
        OBJTYPE_EQUIP_FOOT,
        [ProtoEnum(Name = "OBJTYPE_EQUIP_NECK", Value = 28)]
        OBJTYPE_EQUIP_NECK,
        [ProtoEnum(Name = "OBJTYPE_EQUIP_ACCESSORY", Value = 29)]
        OBJTYPE_EQUIP_ACCESSORY,
        [ProtoEnum(Name = "OBJTYPE_EQUIP_WEAPON", Value = 30)]
        OBJTYPE_EQUIP_WEAPON,
        [ProtoEnum(Name = "OBJTYPE_EQUIP_MAX", Value = 31)]
        OBJTYPE_EQUIP_MAX,
        [ProtoEnum(Name = "OBJTYPE_RES", Value = 41)]
        OBJTYPE_RES = 41,
        [ProtoEnum(Name = "OBJTYPE_SPET_EXP", Value = 42)]
        OBJTYPE_SPET_EXP,
        [ProtoEnum(Name = "OBJTYPE_FRIENDOATH", Value = 43)]
        OBJTYPE_FRIENDOATH,
        [ProtoEnum(Name = "OBJTYPE_QUEST", Value = 45)]
        OBJTYPE_QUEST = 45,
        [ProtoEnum(Name = "OBJTYPE_PET", Value = 46)]
        OBJTYPE_PET,
        [ProtoEnum(Name = "OBJTYPE_GIFT_BAG", Value = 47)]
        OBJTYPE_GIFT_BAG,
        [ProtoEnum(Name = "OBJTYPE_CAN_USE", Value = 48)]
        OBJTYPE_CAN_USE,
        [ProtoEnum(Name = "OBJTYPE_HERO", Value = 49)]
        OBJTYPE_HERO,
        [ProtoEnum(Name = "OBJTYPE_RUNE", Value = 50)]
        OBJTYPE_RUNE,
        [ProtoEnum(Name = "OBJTYPE_RING_QUEST", Value = 51)]
        OBJTYPE_RING_QUEST,
        [ProtoEnum(Name = "OBJTYPE_TREASURE_RADAR", Value = 52)]
        OBJTYPE_TREASURE_RADAR,
        [ProtoEnum(Name = "OBJTYPE_CARD", Value = 53)]
        OBJTYPE_CARD,
        [ProtoEnum(Name = "OBJTYPE_QUEST2", Value = 60)]
        OBJTYPE_QUEST2 = 60,
        [ProtoEnum(Name = "OBJTYPE_DNA", Value = 61)]
        OBJTYPE_DNA,
        [ProtoEnum(Name = "OBJTYPE_DNA2", Value = 62)]
        OBJTYPE_DNA2,
        [ProtoEnum(Name = "OBJTYPE_CLEAR_BATTLESTATE", Value = 63)]
        OBJTYPE_CLEAR_BATTLESTATE,
        [ProtoEnum(Name = "OBJTYPE_SKILL", Value = 64)]
        OBJTYPE_SKILL,
        [ProtoEnum(Name = "OBJTYPE_EVOLUTION", Value = 65)]
        OBJTYPE_EVOLUTION,
        [ProtoEnum(Name = "OBJTYPE_AVATAR", Value = 66)]
        OBJTYPE_AVATAR,
        [ProtoEnum(Name = "OBJTYPE_NORMAL_ITEM_QUEST", Value = 67)]
        OBJTYPE_NORMAL_ITEM_QUEST,
        [ProtoEnum(Name = "OBJTYPE_CAPSULE", Value = 68)]
        OBJTYPE_CAPSULE,
        [ProtoEnum(Name = "OBJTYPE_MAX", Value = 69)]
        OBJTYPE_MAX
    }
}
