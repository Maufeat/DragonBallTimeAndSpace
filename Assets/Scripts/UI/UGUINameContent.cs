using System;
using System.Collections.Generic;

public class UGUINameContent
{
    public static void RegisterUINameAndAssetbundleName()
    {
        if (UGUINameContent.UINamePairs.Count > 0)
        {
            return;
        }
        UGUINameContent.UINamePairs.Add("ui_revive", "ui_revive");
        UGUINameContent.UINamePairs.Add("ui_login", "ui_login");
        UGUINameContent.UINamePairs.Add("UI_P2PLogin", "ui_p2plogin");
        UGUINameContent.UINamePairs.Add("UI_Map", "ui_map");
        UGUINameContent.UINamePairs.Add("UI_Bag", "ui_bag");
        UGUINameContent.UINamePairs.Add("UI_Main", "ui_main");
        UGUINameContent.UINamePairs.Add("UI_Tips", "ui_tips");
        UGUINameContent.UINamePairs.Add("UI_Team", "ui_team");
        UGUINameContent.UINamePairs.Add("UI_FightMode", "ui_fightmode");
        UGUINameContent.UINamePairs.Add("UI_Guild", "ui_guild");
        UGUINameContent.UINamePairs.Add("UI_BuildGuild", "ui_buildguild");
        UGUINameContent.UINamePairs.Add("UI_GuildList", "ui_guildlist");
        UGUINameContent.UINamePairs.Add("UI_Family", "ui_family");
        UGUINameContent.UINamePairs.Add("UI_NPCtalk", "ui_npctalk");
        UGUINameContent.UINamePairs.Add("UI_NPCtalkAndTaskDlg", "ui_npctalkandtaskdlg");
        UGUINameContent.UINamePairs.Add("UI_HpSystem", "ui_hpsystem");
        UGUINameContent.UINamePairs.Add("UI_HpHit", "ui_hphit");
        UGUINameContent.UINamePairs.Add("UI_ProgressBar", "ui_progressbar");
        UGUINameContent.UINamePairs.Add("UI_Occupy", "ui_occupy");
        UGUINameContent.UINamePairs.Add("ui_loading", "ui_loading");
        UGUINameContent.UINamePairs.Add("UI_LoadTips", "ui_loadtips");
        UGUINameContent.UINamePairs.Add("UI_Confirm", "ui_confirm");
        UGUINameContent.UINamePairs.Add("UI_ChooseCamp", "ui_choosecamp");
        UGUINameContent.UINamePairs.Add("UI_TaskDialog", "ui_taskdialog");
        UGUINameContent.UINamePairs.Add("UI_Skill", "ui_skill");
        UGUINameContent.UINamePairs.Add("UI_Activity_Poke", "ui_activity_poke");
        UGUINameContent.UINamePairs.Add("UI_Alert", "ui_alert");
        UGUINameContent.UINamePairs.Add("UI_Chat", "ui_chat");
        UGUINameContent.UINamePairs.Add("UI_GmChat", "ui_gmchat");
        UGUINameContent.UINamePairs.Add("UI_CountDown", "ui_countdown");
        UGUINameContent.UINamePairs.Add("UI_PlayerOperate", "ui_playeroperate");
        UGUINameContent.UINamePairs.Add("UI_TaskList", "ui_tasklist");
        UGUINameContent.UINamePairs.Add("UI_ItemInfo", "ui_iteminfo");
        UGUINameContent.UINamePairs.Add("UI_Friend", "ui_friend");
        UGUINameContent.UINamePairs.Add("UI_FriendNew", "ui_friendnew");
        UGUINameContent.UINamePairs.Add("UI_FriendPrivateChat", "ui_friendprivatechat");
        UGUINameContent.UINamePairs.Add("UI_Instance", "ui_instance");
        UGUINameContent.UINamePairs.Add("UI_InstanceOver", "ui_instanceover");
        UGUINameContent.UINamePairs.Add("UI_Pet", "ui_pet");
        UGUINameContent.UINamePairs.Add("UI_ChooseRole", "ui_chooserole");
        UGUINameContent.UINamePairs.Add("UI_FightStrength", "ui_fightstrength");
        UGUINameContent.UINamePairs.Add("UI_ItemUse", "ui_itemuse");
        UGUINameContent.UINamePairs.Add("UI_Guide", "ui_guide");
        UGUINameContent.UINamePairs.Add("UI_Comic1", "ui_comic1");
        UGUINameContent.UINamePairs.Add("UI_GameSetting", "ui_gamesetting");
        UGUINameContent.UINamePairs.Add("UI_TeamAssign", "ui_teamassign");
        UGUINameContent.UINamePairs.Add("UI_Award", "ui_award");
        UGUINameContent.UINamePairs.Add("UI_ActivityGuide", "ui_activityguide");
        UGUINameContent.UINamePairs.Add("UI_Battlefield", "ui_battlefield");
        UGUINameContent.UINamePairs.Add("UI_Shop", "ui_shop");
        UGUINameContent.UINamePairs.Add("UI_NPCshop", "ui_npcshop");
        UGUINameContent.UINamePairs.Add("UI_Master", "ui_master");
        UGUINameContent.UINamePairs.Add("UI_Hero", "ui_hero");
        UGUINameContent.UINamePairs.Add("UI_Adventure", "ui_adventure");
        UGUINameContent.UINamePairs.Add("UI_PVPMatch", "ui_pvpmatch");
        UGUINameContent.UINamePairs.Add("UI_MatchComplete", "ui_matchcomplete");
        UGUINameContent.UINamePairs.Add("UI_Competition", "ui_competition");
        UGUINameContent.UINamePairs.Add("UI_Score", "ui_score");
        UGUINameContent.UINamePairs.Add("UI_CreateRole", "ui_createrole");
        UGUINameContent.UINamePairs.Add("UI_Rune", "ui_rune");
        UGUINameContent.UINamePairs.Add("UI_Gene", "ui_gene");
        UGUINameContent.UINamePairs.Add("UI_HeroInfo", "ui_heroinfo");
        UGUINameContent.UINamePairs.Add("UI_NumberInput", "ui_numberinput");
        UGUINameContent.UINamePairs.Add("UI_ItemSubmit", "ui_itemsubmit");
        UGUINameContent.UINamePairs.Add("UI_ChatConfig", "ui_chatconfig");
        UGUINameContent.UINamePairs.Add("UI_Rader", "ui_rader");
        UGUINameContent.UINamePairs.Add("UI_SDScore", "ui_sdscore");
        UGUINameContent.UINamePairs.Add("UI_SidouMatch", "ui_sidoumatch");
        UGUINameContent.UINamePairs.Add("UI_Manufacture", "ui_manufacture");
        UGUINameContent.UINamePairs.Add("UI_Escape", "ui_escape");
        UGUINameContent.UINamePairs.Add("UI_PropSubmit", "ui_propsubmit");
        UGUINameContent.UINamePairs.Add("UI_InstanceEffect", "ui_instanceeffect");
        UGUINameContent.UINamePairs.Add("UI_HeroStar", "ui_herostar");
        UGUINameContent.UINamePairs.Add("UI_SkillInfo", "ui_skillinfo");
        UGUINameContent.UINamePairs.Add("UI_HeroSimpleAttr", "ui_herosimpleattr");
        UGUINameContent.UINamePairs.Add("UI_ShortcutsConfig", "ui_shortcutsconfig");
        UGUINameContent.UINamePairs.Add("UI_SelectRole", "ui_selectrole");
        UGUINameContent.UINamePairs.Add("UI_3DIconModelPosCheck", "ui_3diconmodelposcheck");
        UGUINameContent.UINamePairs.Add("UI_GM", "ui_gm");
        UGUINameContent.UINamePairs.Add("UI_Exam", "ui_exam");
        UGUINameContent.UINamePairs.Add("UI_Exchange", "ui_exchange");
        UGUINameContent.UINamePairs.Add("UI_VipPrivilege", "ui_vipprivilege");
        UGUINameContent.UINamePairs.Add("UI_MessageBox", "ui_messagebox");
        UGUINameContent.UINamePairs.Add("UI_Character", "ui_character");
        UGUINameContent.UINamePairs.Add("UI_Queue", "ui_queue");
        UGUINameContent.UINamePairs.Add("UI_TeamSecondary", "ui_teamsecondary");
        UGUINameContent.UINamePairs.Add("UI_ItemTip", "ui_itemtip");
        UGUINameContent.UINamePairs.Add("UI_TextTip", "ui_texttip");
        UGUINameContent.UINamePairs.Add("UI_PickDrop", "ui_pickdrop");
        UGUINameContent.UINamePairs.Add("UI_MatchPassword", "ui_matchpassword");
        UGUINameContent.UINamePairs.Add("UI_QTE", "ui_qte");
        UGUINameContent.UINamePairs.Add("UI_QTE_jwq", "ui_qte_jwq");
        UGUINameContent.UINamePairs.Add("UI_QTE_yqd", "ui_qte_yqd");
        UGUINameContent.UINamePairs.Add("UI_Mail", "ui_mail");
        UGUINameContent.UINamePairs.Add("UI_UnLockSkills", "ui_unlockskills");
        UGUINameContent.UINamePairs.Add("UI_UnLockSkillsItemTips", "ui_unlockskillsitemtips");
        UGUINameContent.UINamePairs.Add("UI_CoolDown", "ui_cooldown");
        UGUINameContent.UINamePairs.Add("UI_HeroPokedex", "ui_heropokedex");
        UGUINameContent.UINamePairs.Add("UI_Business", "ui_business");
        UGUINameContent.UINamePairs.Add("UI_SecondPassword", "ui_secondpassword");
        UGUINameContent.UINamePairs.Add("UI_Transporter", "ui_transporter");
        UGUINameContent.UINamePairs.Add("UI_Depot", "ui_depot");
        UGUINameContent.UINamePairs.Add("UI_fcm", "ui_fcm");
        UGUINameContent.UINamePairs.Add("UI_SetPlayerDirTest", "ui_setplayerdirtest");
        UGUINameContent.UINamePairs.Add("UI_HollowMan", "ui_hollowman");
        UGUINameContent.UINamePairs.Add("UI_TaskTrackCheck", "ui_tasktrackcheck");
        UGUINameContent.UINamePairs.Add("UI_Effect", "ui_effect");
        UGUINameContent.UINamePairs.Add("UI_GetNewHero", "ui_getnewhero");
        UGUINameContent.UINamePairs.Add("UI_GuildWar", "ui_guildwar");
        UGUINameContent.UINamePairs.Add("UI_7days", "ui_7days");
        UGUINameContent.UINamePairs.Add("UI_AbattoirMatch", "ui_abattoirmatch");
        UGUINameContent.UINamePairs.Add("UI_AbattoirTips", "ui_abattoirtips");
        UGUINameContent.UINamePairs.Add("UI_AbattoirPray", "ui_abattoirpray");
        UGUINameContent.UINamePairs.Add("UI_AbattoirReward", "ui_abattoirreward");
        UGUINameContent.UINamePairs.Add("UI_AbattoirTransfer", "ui_abattoirtransfer");
        UGUINameContent.UINamePairs.Add("UI_Questionnaire", "ui_questionnaire");
    }

    public static void RegisterUINameAndAssetbundleName(string UIName, string AssetbundleName)
    {
        UGUINameContent.UINamePairs[UIName] = AssetbundleName.ToLower();
    }

    public const string UGUI_Bag = "UI_Bag";

    public const string UGUI_Tips = "UI_Tips";

    public const string UGUI_MainView = "UI_Main";

    public const string UGUI_Map = "UI_Map";

    public const string UGUI_Login = "ui_login";

    public const string UGUI_P2PLogin = "UI_P2PLogin";

    public const string UGUI_Revive = "ui_revive";

    public const string UGUI_Team = "UI_Team";

    public const string UGUI_FightMode = "UI_FightMode";

    public const string UGUI_GuildInfo = "UI_Guild";

    public const string UGUI_GuildList = "UI_GuildList";

    public const string UGUI_GuildBuild = "UI_BuildGuild";

    public const string UGUI_Family = "UI_Family";

    public const string UGUI_NpcDlg = "UI_NPCtalk";

    public const string UGUI_NpcAndTaskDlg = "UI_NPCtalkAndTaskDlg";

    public const string UGUI_ProgressBar = "UI_ProgressBar";

    public const string UGUI_Occupy = "UI_Occupy";

    public const string UGUI_Loading = "ui_loading";

    public const string UGUI_HpSystem = "UI_HpSystem";

    public const string UGUI_HpHit = "UI_HpHit";

    public const string UGUI_LoadTips = "UI_LoadTips";

    public const string UGUI_MsgBox = "UI_Confirm";

    public const string UGUI_JoinCamp = "UI_ChooseCamp";

    public const string UGUI_TaskDlg = "UI_TaskDialog";

    public const string UGUI_Skill = "UI_Skill";

    public const string UGUI_Pry = "UI_Activity_Poke";

    public const string UGUI_Alert = "UI_Alert";

    public const string UGUI_CountDown = "UI_CountDown";

    public const string UGUI_Chat = "UI_Chat";

    public const string UGUI_GmChat = "UI_GmChat";

    public const string UGUI_PlayerOperate = "UI_PlayerOperate";

    public const string UGUI_TaskList = "UI_TaskList";

    public const string UGUI_ItemInfo = "UI_ItemInfo";

    public const string UGUI_Friend = "UI_Friend";

    public const string UGUI_FriendNew = "UI_FriendNew";

    public const string UGUI_FriendPrivateChat = "UI_FriendPrivateChat";

    public const string UGUI_EnterCopy = "UI_Instance";

    public const string UGUI_CompleteCopy = "UI_InstanceOver";

    public const string UGUI_Pet = "UI_Pet";

    public const string UGUI_ChooseRole = "UI_ChooseRole";

    public const string UGUI_FightChange = "UI_FightStrength";

    public const string UGUI_ShortcutEquipUsed = "UI_ItemUse";

    public const string UGUI_Guide = "UI_Guide";

    public const string UGUI_Comic1 = "UI_Comic1";

    public const string UGUI_GameSetting = "UI_GameSetting";

    public const string UGUI_PublicDrop = "UI_TeamAssign";

    public const string UGUI_GiftBag = "UI_Award";

    public const string UGUI_ActivityGuide = "UI_ActivityGuide";

    public const string UGUI_DuoQi = "UI_Battlefield";

    public const string UGUI_MentoringSystem = "UI_Master";

    public const string UGUI_JieWangQuan = "UI_QTE_jwq";

    public const string UGUI_YuanQiDan = "UI_QTE_yqd";

    public const string UGUI_Shop = "UI_Shop";

    public const string UGUI_NPCshop = "UI_NPCshop";

    public const string UGUI_Hero = "UI_Hero";

    public const string UGUI_Adventure = "UI_Adventure";

    public const string UGUI_PVPMatch = "UI_PVPMatch";

    public const string UGUI_MatchComplete = "UI_MatchComplete";

    public const string UGUI_Competition = "UI_Competition";

    public const string UGUI_Score = "UI_Score";

    public const string UGUI_CreateRole = "UI_CreateRole";

    public const string UGUI_Rune = "UI_Rune";

    public const string UGUI_Gene = "UI_Gene";

    public const string UGUI_HeroInfo = "UI_HeroInfo";

    public const string UGUI_NumberInput = "UI_NumberInput";

    public const string UGUI_ItemSubmit = "UI_ItemSubmit";

    public const string UGUI_ChatConfig = "UI_ChatConfig";

    public const string UGUI_Rader = "UI_Rader";

    public const string UGUI_SDScore = "UI_SDScore";

    public const string UGUI_SidouMatch = "UI_SidouMatch";

    public const string UGUI_Manufacture = "UI_Manufacture";

    public const string UGUI_Escape = "UI_Escape";

    public const string UGUI_PropSubmit = "UI_PropSubmit";

    public const string UGUI_InstanceEffect = "UI_InstanceEffect";

    public const string UGUI_HeroStar = "UI_HeroStar";

    public const string UGUI_Exam = "UI_Exam";

    public const string UGUI_Exchange = "UI_Exchange";

    public const string UGUI_VipPrivilege = "UI_VipPrivilege";

    public const string UGUI_MessageBox = "UI_MessageBox";

    public const string UGUI_GMTool = "UI_GM";

    public const string UGUI_SkillInfo = "UI_SkillInfo";

    public const string UGUI_HeroSimpleAttr = "UI_HeroSimpleAttr";

    public const string UGUI_ShortcutsConfig = "UI_ShortcutsConfig";

    public const string UGUI_SelectRole = "UI_SelectRole";

    public const string UGUI_3DIconModelPosCheck = "UI_3DIconModelPosCheck";

    public const string UGUI_Character = "UI_Character";

    public const string UGUI_Queue = "UI_Queue";

    public const string UGUI_TeamSecondary = "UI_TeamSecondary";

    public const string UGUI_ItemTip = "UI_ItemTip";

    public const string UGUI_MatchPassword = "UI_MatchPassword";

    public const string UGUI_QTE = "UI_QTE";

    public const string UGUI_TextTip = "UI_TextTip";

    public const string UGUI_PickDrop = "UI_PickDrop";

    public const string UGUI_Mail = "UI_Mail";

    public const string UGUI_UnLockSkills = "UI_UnLockSkills";

    public const string UGUI_UnLockSkillsItemTips = "UI_UnLockSkillsItemTips";

    public const string UGUI_CoolDown = "UI_CoolDown";

    public const string UGUI_HeroHandbook = "UI_HeroPokedex";

    public const string UGUI_Business = "UI_Business";

    public const string UGUI_SecondPwd = "UI_SecondPassword";

    public const string UI_Transporter = "UI_Transporter";

    public const string UGUI_AntiAddiction = "UI_fcm";

    public const string UGUI_SetPlayerDirTest = "UI_SetPlayerDirTest";

    public const string UGUI_HollowMan = "UI_HollowMan";

    public const string UGUI_TaskTrackCheck = "UI_TaskTrackCheck";

    public const string UGUI_Depot = "UI_Depot";

    public const string UGUI_MailPwd = "UI_Mail";

    public const string UGUI_ExchangeGem = "UI_Exchange";

    public const string UGUI_Effect = "UI_Effect";

    public const string UGUI_GetNewHero = "UI_GetNewHero";

    public const string UGUI_GuildWar = "UI_GuildWar";

    public const string UGUI_SevenDays = "UI_7days";

    public const string UGUI_AbattoirMatch = "UI_AbattoirMatch";

    public const string UGUI_AbattoirTips = "UI_AbattoirTips";

    public const string UGUI_AbattoirPray = "UI_AbattoirPray";

    public const string UGUI_AbattoirReward = "UI_AbattoirReward";

    public const string UGUI_AbattoirTransfer = "UI_AbattoirTransfer";

    public const string UGUI_Questationnaire = "UI_Questionnaire";

    public static Dictionary<string, string> UINamePairs = new Dictionary<string, string>();
}
