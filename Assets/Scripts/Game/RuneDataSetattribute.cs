using System;
using System.Collections.Generic;

public class RuneDataSetattribute
{
    public void addBystring(string propertystr)
    {
        string[] array = propertystr.Split(new char[]
        {
            ';'
        }, StringSplitOptions.RemoveEmptyEntries);
        for (int i = 0; i < array.Length; i++)
        {
            string[] array2 = array[i].Split(new char[]
            {
                '-'
            }, StringSplitOptions.RemoveEmptyEntries);
            int num = int.Parse(array2[0]);
            uint num2 = uint.Parse(array2[1]);
            List<uint> list2;
            List<uint> list = list2 = this.list;
            int index2;
            int index = index2 = num;
            uint num3 = list2[index2];
            list[index] = num3 + num2;
        }
    }

    public void addByOtherRune(RuneDataSetattribute OtherRune)
    {
        for (int i = 0; i < OtherRune.list.Count; i++)
        {
            List<uint> list2;
            List<uint> list = list2 = this.list;
            int index2;
            int index = index2 = i;
            uint num = list2[index2];
            list[index] = num + OtherRune.list[i];
        }
    }

    public void clear()
    {
        this.list = new List<uint>(new uint[14]);
    }

    public List<uint> list = new List<uint>(new uint[14]);
}
