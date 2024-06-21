public class CacheBufferInfo
{
    public void Clear()
    {
        this.flag = UserState.USTATE_NOSTATE;
        this.buffAnim = 0U;
        this.revertAnim = 0U;
    }

    public UserState flag;

    public uint buffAnim;

    public uint revertAnim;
}
