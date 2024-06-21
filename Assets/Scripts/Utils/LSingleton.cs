using System;

public class LSingleton<T> where T : class, new()
{
    public static T Instance
    {
        get
        {
            if (LSingleton<T>.instance_ == null)
            {
                LSingleton<T>.instance_ = Activator.CreateInstance<T>();
            }
            return LSingleton<T>.instance_;
        }
    }

    public void DeleteInstance()
    {
        LSingleton<T>.instance_ = (T)((object)null);
    }

    private static T instance_;
}
