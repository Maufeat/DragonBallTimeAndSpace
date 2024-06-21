using System;
using LuaInterface;
using UnityEngine;

public class Const : ConstClient
{
    public static Color TextColorNormal
    {
        get
        {
            return new Color(0.721568644f, 0.5764706f, 0.07450981f, 1f);
        }
    }

    public static Color TextColorNormalInUI
    {
        get
        {
            return new Color(0.768627465f, 0.7490196f, 0.635294139f, 1f);
        }
    }

    public static Color TextColorFriend
    {
        get
        {
            return new Color(0.309803933f, 0.7019608f, 0.0431372561f, 1f);
        }
    }

    public static Color TextColorEnemy
    {
        get
        {
            return new Color(0.733333349f, 0.184313729f, 0.321568638f, 1f);
        }
    }

    public static Color TextColorTeammate
    {
        get
        {
            return new Color(0.309803933f, 0.7019608f, 0.0431372561f, 1f);
        }
    }

    public static Color TextColorTipsGreen
    {
        get
        {
            return new Color(0.309803933f, 0.7019608f, 0.0431372561f, 1f);
        }
    }

    public static Color TextColorTipsRed
    {
        get
        {
            return new Color(0.4745098f, 0.286274523f, 0.286274523f, 1f);
        }
    }

    public static Color SkillLocked
    {
        get
        {
            return new Color(0.392156869f, 0.392156869f, 0.392156869f, 1f);
        }
    }

    public static Color GetColorByName(string name)
    {
        LuaTable xmlConfigTable = LuaConfigManager.GetXmlConfigTable("color");
        if (xmlConfigTable != null)
        {
            LuaTable cacheField_Table = xmlConfigTable.GetCacheField_Table("ColorType");
            if (cacheField_Table != null)
            {
                LuaTable cacheField_Table2 = cacheField_Table.GetCacheField_Table(name);
                if (cacheField_Table2 != null)
                {
                    float num = (float)cacheField_Table2.GetCacheField_Double("r");
                    float num2 = (float)cacheField_Table2.GetCacheField_Double("g");
                    float num3 = (float)cacheField_Table2.GetCacheField_Double("b");
                    float num4 = (float)cacheField_Table2.GetCacheField_Double("a");
                    return new Color(num / 255f, num2 / 255f, num3 / 255f, num4 / 255f);
                }
            }
        }
        FFDebug.LogWarning("Const", " not  find this  color ! " + name);
        return Color.white;
    }

    public static Color ColorByRelation(RelationType type, bool isPlayer)
    {
        string name;
        switch (type)
        {
            case RelationType.None:
                name = "normalwhite";
                goto IL_9B;
            case RelationType.Self:
                name = "friendlyplayername_ol";
                goto IL_9B;
            case RelationType.Friend:
                if (isPlayer)
                {
                    name = "friendlyplayername_ol";
                }
                else
                {
                    name = "friendlynpcname";
                }
                goto IL_9B;
            case RelationType.Neutral:
                if (isPlayer)
                {
                    name = "friendlyplayername_ol";
                }
                else
                {
                    name = "neutralnpcname";
                }
                goto IL_9B;
            case RelationType.Enemy:
                name = "itemlimit";
                goto IL_9B;
        }
        name = "normalwhite";
    IL_9B:
        return Const.GetColorByName(name);
    }

    public static uint GetMainPackageSize()
    {
        return LuaConfigManager.GetXmlConfigTable("bag").GetCacheField_Table("mainpack").GetCacheField_Uint("size");
    }

    public static uint GetQusetPackageSize()
    {
        return LuaConfigManager.GetXmlConfigTable("bag").GetCacheField_Table("questpack").GetCacheField_Uint("size");
    }

    public static uint GetOnceExtendSize()
    {
        return LuaConfigManager.GetXmlConfigTable("bag").GetCacheField_Table("bagextend").GetCacheField_Uint("persize");
    }

    public static uint GetMaxExtendTimes()
    {
        return LuaConfigManager.GetXmlConfigTable("bag").GetCacheField_Table("bagextendlimit").GetCacheField_Uint("maxcount");
    }

    public static Texture2D GetBlankTex()
    {
        if (Const.t2dBlank == null)
        {
            Const.t2dBlank = new Texture2D(4, 4, TextureFormat.ARGB32, false);
            Color[] array = new Color[16];
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = new Color(0f, 0f, 0f, 0f);
            }
            Const.t2dBlank.SetPixels(array);
            Const.t2dBlank.Apply();
        }
        return Const.t2dBlank;
    }

    public static Sprite GetBlankSprite()
    {
        Texture2D blankTex = Const.GetBlankTex();
        return Sprite.Create(blankTex, new Rect(0f, 0f, (float)blankTex.width, (float)blankTex.height), Vector2.one * 0.5f, 100f, 0U, SpriteMeshType.Tight, new Vector4(1f, 1f, 1f, 1f));
    }

    public static float GetHitPercentage(string bodySize)
    {
        LuaTable cacheField_Table = LuaConfigManager.GetXmlConfigTable("massiveConfig").GetCacheField_Table("hitpercent");
        float result = 0f;
        if (cacheField_Table == null)
        {
            result = 0f;
        }
        LuaTable cacheField_Table2 = cacheField_Table.GetCacheField_Table(bodySize);
        if (cacheField_Table2 != null)
        {
            result = cacheField_Table2.GetCacheField_Float("value");
        }
        return result;
    }

    public static uint GetWeeklyOpenLevel()
    {
        return LuaConfigManager.GetXmlConfigTable("dailyguide").GetCacheField_Table("WeeklyOpenLevel").GetCacheField_Uint("value");
    }

    public static float DistNpcVisitResponse
    {
        get
        {
            if (Const.disNpcResponse < 0.1f)
            {
                Const.disNpcResponse = LuaConfigManager.GetXmlConfigTable("massiveConfig").GetCacheField_Table("npcVisiteResponseDis").GetCacheField_Float("value");
            }
            return Const.disNpcResponse;
        }
    }

    public static float DistMovingNpcVisitResponse
    {
        get
        {
            if (Const.DistMovingNpcVisitResponse_ < 0.1f)
            {
                Const.DistMovingNpcVisitResponse_ = LuaConfigManager.GetXmlConfigTable("massiveConfig").GetCacheField_Table("npcVisiteResponseDis").GetCacheField_Float("value_moving");
            }
            return Const.DistMovingNpcVisitResponse_;
        }
    }

    public const float NineScreenX = 15f;

    public const float NineScreenY = 15f;

    public const uint NoCareerLimit = 999U;

    public const uint CopyInfiniteNum = 9999U;

    public const uint TempBuildingNpcIDLimite = 19000U;

    public const string STR_DEFAULTPWD = "1";

    public const uint CopyBaseNum = 100U;

    public const string StrMoviePath = "Data/Raw/moviesamples/chuanshuo";

    public const float FMovieDuration = 60f;

    public const uint StoneID = 2U;

    public const uint MoneyID = 3U;

    public const uint WelPointID = 4U;

    public const uint GuildPointID = 11U;

    public const uint BluePointID = 13U;

    public const uint PurplePointID = 14U;

    public const uint EduPointID = 15U;

    public const uint HelpPointID = 16U;

    public const uint ActivePointID = 17U;

    public const uint DoublePointID = 18U;

    public const uint MaxSaveChatCount = 40U;

    public const string StrPrivateChatPath = "/privatechat.dat";

    public const uint TidyPackCD = 5U;

    public const string StrNPCVisitIcon = "visit_chat";

    public const string StrNPCAcceptTaskIcon = "ic0143";

    public const string StrNPCTaskDoingIcon = "ic0144";

    public const string StrNPCTaskFinishIcon = "ic0145";

    public const uint ShowMapFlag = 1U;

    public const uint NotShowMapFlag = 2U;

    public const float PathFinderSafetyRange = 12f;

    public const float PathFinderTaskNpcOffset = 2f;

    public const float PathFinderTaskNpcMinOffset = 1f;

    public const float PathFinderTaskNpcMaxOffsetAngle = 0.7853982f;

    public const float DramaTalkInterval = 1.5f;

    public const float DramaTipsInterval = 1.2f;

    public const float ComicInterval = 4f;

    public const uint VitalityRefreshTime = 6U;

    public const string OpenChatType = "DB_OPEN_CHAT_TYPE";

    public const float SelectTargetMaxDistance = 300f;

    public const float MsConvertSecondUnit = 1000f;

    public const float MapConversionUnit = 3f;

    public static bool DebugMode = true;

    public static bool EnableDebugLog;

    public static bool ShowDebugLogTime;

    public static bool DestoryEffect = true;

    public static bool ExportLog = true;

    public static bool DebugOutput = true;

    public static int ConstPanel = 1;

    public static int QueuePanel = 2;

    public static string UILayer = "UI";

    public static string DefaultLayer = "Default";

    public static string AppName = "FFXV";

    public static string ExtName = ".assetbundle";

    public static string AssetDir = "StreamingAssets";

    public static Shader charactorOriShader;

    public static CreateAssetBundleType createAssetBundleType;

    public static uint MaxLevel = 9999U;

    public static uint Camp1ID = 1U;

    public static uint Camp2ID = 2U;

    public static uint CampNeutralID = 6U;

    private static Texture2D t2dBlank;

    private static float disNpcResponse;

    private static float DistMovingNpcVisitResponse_;

    public struct Tags
    {
        public static string First = "FirstUI";

        public static string Second = "SecondUI";

        public static string Third = "ThirdUI";

        public static string Mask = "Mask";

        public static string Shadow = "Shadow";

        public static string Wall = "Wall";

        public static string Floor = "Floor";

        public static string DisAppear = "DisAppear";

        public static string Effect = "Effect";
    }

    public struct UICommonEvent
    {
        public static string Ui_Map_Open = "Ui_Map_Open";

        public static string Ui_Click_Cancel = "Ui_Click_Cancel";

        public static string Ui_Main_Open = "Ui_Main_Open";

        public static string Ui_Click_Default = "Ui_Click_Default";

        public static string Ui_Click_Confirm = "Ui_Click_Confirm";

        public static string Ui_SubMain_Close = "Ui_SubMain_Close";

        public static string Ui_Click_Select = "Ui_Click_Select";

        public static string Ui_SubMain_Open = "Ui_SubMain_Open";

        public static string Ui_Map_Close = "Ui_Map_Close";

        public static string Ui_LevelUp = "Ui_LevelUp";

        public static string Ui_Click_Section = "Ui_Click_Section";

        public static string Ui_Main_Close = "Ui_Main_Close";
    }

    public struct MusicConst
    {
        public static string MusicBankName = "Music";

        public static string MapTourBankName = "Music";

        public static string MapTourStart = "MapTourStart";

        public static string MapTourEnd = "MapTourEnd";

        public static string Music_Play = "Music_Play";

        public static string Music_Stop = "Music_Stop";
    }

    public struct Layer
    {
        public static int Default = LayerMask.NameToLayer("Default");

        public static int TransparentFX = LayerMask.NameToLayer("TransparentFX");

        public static int IgnoreRaycast = LayerMask.NameToLayer("Ignore Raycast");

        public static int Water = LayerMask.NameToLayer("Water");

        public static int UI = LayerMask.NameToLayer("UI");

        public static int Charactor = LayerMask.NameToLayer("Charactor");

        public static int Terrian = LayerMask.NameToLayer("Terrian");

        public static int RT = LayerMask.NameToLayer("RT");

        public static int NpcShadow = LayerMask.NameToLayer("NpcShadow");

        public static int NpcNoShadow = LayerMask.NameToLayer("NpcNoShadow");

        public static int MainPlayer = LayerMask.NameToLayer("MainPlayer");

        public static int OtherPlayer = LayerMask.NameToLayer("OtherPlayer");

        public static int Wall = LayerMask.NameToLayer("Wall");

        public static int Effect = LayerMask.NameToLayer("Effect");

        public static int Cutscene = LayerMask.NameToLayer("CutScene");

        public static int CutSceneUI = LayerMask.NameToLayer("CutSceneUI");

        public static int CameraEffect = LayerMask.NameToLayer("CameraEffect");
    }

    public struct LayerForMask
    {
        public static int Default = LayerMask.GetMask(new string[]
        {
            "Default"
        });

        public static int TransparentFX = LayerMask.GetMask(new string[]
        {
            "TransparentFX"
        });

        public static int IgnoreRaycast = LayerMask.GetMask(new string[]
        {
            "Ignore Raycast"
        });

        public static int Water = LayerMask.GetMask(new string[]
        {
            "Water"
        });

        public static int UI = LayerMask.GetMask(new string[]
        {
            "UI"
        });

        public static int Charactor = LayerMask.GetMask(new string[]
        {
            "Charactor"
        });

        public static int Terrian = LayerMask.GetMask(new string[]
        {
            "Terrian"
        });

        public static int RT = LayerMask.GetMask(new string[]
        {
            "RT"
        });

        public static int NpcShadow = LayerMask.GetMask(new string[]
        {
            "NpcShadow"
        });

        public static int NpcNoShadow = LayerMask.GetMask(new string[]
        {
            "NpcNoShadow"
        });

        public static int MainPlayer = LayerMask.GetMask(new string[]
        {
            "MainPlayer"
        });

        public static int OtherPlayer = LayerMask.GetMask(new string[]
        {
            "OtherPlayer"
        });

        public static int Cutscene = LayerMask.GetMask(new string[]
        {
            "CutScene"
        });

        public static int Wall = LayerMask.GetMask(new string[]
        {
            "Wall"
        });

        public static int Fence = LayerMask.GetMask(new string[]
        {
            "Fence"
        });

        public static int CameraEffect = LayerMask.NameToLayer("CameraEffect");

        public static int FenceValue = LayerMask.NameToLayer("Fence");

        public static int EveryLayer = Const.LayerForMask.Default + Const.LayerForMask.TransparentFX + Const.LayerForMask.IgnoreRaycast + Const.LayerForMask.Water + Const.LayerForMask.UI + Const.LayerForMask.Charactor + Const.LayerForMask.Terrian + Const.LayerForMask.RT + Const.LayerForMask.NpcShadow + Const.LayerForMask.NpcNoShadow + Const.LayerForMask.MainPlayer + Const.LayerForMask.OtherPlayer + Const.LayerForMask.CameraEffect;
    }

    public struct RenderQueue
    {
        public static int SceneObj = 2000;

        public static int Charactor = 1998;

        public static int SceneObjAfterCharactor = 2100;

        public static int Hole = 2150;

        public static int SceneObjAfterWithHole = 2200;

        public static int Plant = 2300;

        public static int Transparent = 3000;

        public static int SkillSelect = 2020;
    }
}
