using System.Collections.Generic;
using msg;

public class ReqSubSellingList
{
    public ReqSubSellingList(SELLTYPE itemType, List<uint> idList, uint levelStar, bool checkShow)
    {
        this.ItemType = itemType;
        this.IdList = idList;
        this.LevelStar = levelStar;
        this.CheckShow = checkShow;
    }

    public SELLTYPE ItemType = SELLTYPE.OBJECT;

    public List<uint> IdList = new List<uint>();

    public uint LevelStar;

    public bool CheckShow;
}
