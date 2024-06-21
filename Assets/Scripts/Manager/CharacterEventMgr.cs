using System;
using System.Collections.Generic;

public class CharacterEventMgr : IstorebAble
{
    public void Init(CharactorBase Charb)
    {
        this.Owner = Charb;
    }

    public bool IsDirty { get; set; }

    public void AddListener(string Event, CharacterEventMgr.CharacterEventHandler Handler)
    {
        if (!this.AllCharacterEventHandlerMap.ContainsKey(Event))
        {
            this.AllCharacterEventHandlerMap.Add(Event, new List<CharacterEventMgr.CharacterEventHandler>());
        }
        List<CharacterEventMgr.CharacterEventHandler> list = this.AllCharacterEventHandlerMap[Event];
        if (!list.Contains(Handler))
        {
            list.Add(Handler);
        }
    }

    public void RemoveListener(string Event, CharacterEventMgr.CharacterEventHandler Handler)
    {
        if (this.AllCharacterEventHandlerMap.ContainsKey(Event))
        {
            List<CharacterEventMgr.CharacterEventHandler> list = this.AllCharacterEventHandlerMap[Event];
            list.Remove(Handler);
        }
    }

    public void SendEvent(string Event, params object[] args)
    {
        if (this.AllCharacterEventHandlerMap.ContainsKey(Event))
        {
            List<CharacterEventMgr.CharacterEventHandler> list = this.AllCharacterEventHandlerMap[Event];
            for (int i = 0; i < list.Count; i++)
            {
                try
                {
                    list[i](this.Owner, args);
                }
                catch (Exception ex)
                {
                    FFDebug.LogError(this, string.Concat(new object[]
                    {
                        "CallEvent: ",
                        Event,
                        " error",
                        ex
                    }));
                }
            }
        }
    }

    public void ClearEventHandler(string Event)
    {
        if (this.AllCharacterEventHandlerMap.ContainsKey(Event))
        {
            this.AllCharacterEventHandlerMap[Event].Clear();
        }
    }

    public void RestThisObject()
    {
        this.Owner = null;
        this.AllCharacterEventHandlerMap.Clear();
    }

    public void StoreToPool()
    {
        ClassPool.Store<CharacterEventMgr>(this, 60);
    }

    private CharactorBase Owner;

    private BetterDictionary<string, List<CharacterEventMgr.CharacterEventHandler>> AllCharacterEventHandlerMap = new BetterDictionary<string, List<CharacterEventMgr.CharacterEventHandler>>();

    public delegate void CharacterEventHandler(CharactorBase Owner, params object[] args);
}
