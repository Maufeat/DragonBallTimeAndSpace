using System;

public class Constant
{
    public static Constant.Version CUR_VRESION;

    public static bool MOVIE_PLAY = true;

    public static int SWITCH_HERO_PROGRESS_SECOND = 3;

    public enum Version
    {
        Debug,
        Release,
        Dist
    }
}
