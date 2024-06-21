using UnityEngine;

public class ConstClient
{
    public const uint BuglyIOSAppID = 900049075;
    public const uint BuglyAndroidAppID = 900049926;

    public static int TargetFrameRate
    {
        get
        {
            return 60;
        }
    }

    public static Color MaxLevelColor
    {
        get
        {
            return new Color(0.2196078f, 1f, 0.2588235f, 1f);
        }
    }
}
