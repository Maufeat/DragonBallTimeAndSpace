using System.IO;

public static class SystemHelper
{
    public static LuaStringBuffer ReadAllBytes(string filename)
    {
        return new LuaStringBuffer(File.ReadAllBytes(filename));
    }
}
