using System;
using System.Collections.Generic;
using LuaInterface;

public class tGuideCompare : IComparer<LuaTable>
{
    public int Compare(LuaTable x, LuaTable y)
    {
        if (x.GetField_Uint("id") < y.GetField_Uint("id"))
        {
            return -1;
        }
        if (x.GetField_Uint("id") > y.GetField_Uint("id"))
        {
            return 1;
        }
        return 0;
    }
}
