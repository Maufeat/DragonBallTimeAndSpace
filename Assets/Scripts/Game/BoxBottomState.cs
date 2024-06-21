using System;

public class BoxBottomState
{
    public BoxBottomState.State state;

    public float m_ToLeft;

    public float m_ToRight;

    public float m_Dis;

    public enum State
    {
        none,
        left,
        stay,
        right
    }
}
