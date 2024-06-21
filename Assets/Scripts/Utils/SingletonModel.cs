using System;

public class SingletonModel<T> where T : new()
{
    public static T Instatnce
    {
        get
        {
            if (SingletonModel<T>._instance == null)
            {
                SingletonModel<T>._instance = ((default(T) == null) ? Activator.CreateInstance<T>() : default(T));
            }
            return SingletonModel<T>._instance;
        }
    }

    private static T _instance;
}
