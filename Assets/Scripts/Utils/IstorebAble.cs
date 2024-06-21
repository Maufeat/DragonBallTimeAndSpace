using System;

public interface IstorebAble
{
    bool IsDirty { get; set; }

    void RestThisObject();

    void StoreToPool();
}
