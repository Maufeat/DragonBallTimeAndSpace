public static class dynamic_textid
{
    public enum BaseIDs
    {
        system_prompt = 1,
        prompt = 2,
        system_notification = 3,
        server_getway = 4,
        confirm = 5,
        cancel = 6,
        reconnect = 7,
        create = 8,
        retry = 9,
        leave = 10, // 0x0000000A
        system_prompt_en = 11, // 0x0000000B
        prompt_en = 12, // 0x0000000C
        insist_rid = 13, // 0x0000000D
        reconsider = 14, // 0x0000000E
        yes = 15, // 0x0000000F
        no = 16, // 0x00000010
        contiune_use = 18, // 0x00000012
    }

    public enum ServerIDs
    {
        kickout = 50, // 0x00000032
        disconnect = 51, // 0x00000033
        other_people_login = 52, // 0x00000034
    }

    public enum IDs
    {
        lack_of_mp = 904, // 0x00000388
        friend_rid = 1000, // 0x000003E8
        sociaty_transmission = 1001, // 0x000003E9
        team_create = 1002, // 0x000003EA
        camp_join = 1003, // 0x000003EB
        sociaty_targettransmission = 1004, // 0x000003EC
        task_forgo = 1005, // 0x000003ED
        sociaty_changetransmission = 1006, // 0x000003EE
        copy_leave_single_nolimit = 1007, // 0x000003EF
        copy_leave_team_nolimit = 1008, // 0x000003F0
        copy_leave_single_limited = 1009, // 0x000003F1
        copy_leave_team_limited = 1010, // 0x000003F2
        drama_skip = 1011, // 0x000003F3
        friendisfullcannottakemaster = 1012, // 0x000003F4
        friendisfullcannottakeapprentice = 1013, // 0x000003F5
        unpublish_takemaster = 1014, // 0x000003F6
        unpublish_takeapprentice = 1015, // 0x000003F7
        apprentice_removetips = 1016, // 0x000003F8
        apprentice_removenow = 1017, // 0x000003F9
        apprentice_beremoved = 1018, // 0x000003FA
        apprentice_disengage = 1019, // 0x000003FB
        enter_adventure = 1020, // 0x000003FC
        rune_allunload = 1021, // 0x000003FD
        rune_activite = 1022, // 0x000003FE
        rune_selectactivite = 1024, // 0x00000400
        vit_max_limit = 1025, // 0x00000401
        source_hero_bind = 1026, // 0x00000402
        cost_hero_bind = 1027, // 0x00000403
        both_heros_bind = 1028, // 0x00000404
    }
}
