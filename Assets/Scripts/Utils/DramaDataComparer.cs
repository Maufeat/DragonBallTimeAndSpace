using System.Collections.Generic;
using LuaInterface;

public class DramaDataComparer : IComparer<LuaTable>
{
    public int Compare(LuaTable item1, LuaTable item2)
    {
        return item1.GetField_Uint("id").CompareTo(item2.GetField_Uint("id"));
    }
}
