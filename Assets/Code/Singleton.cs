using System;

public abstract class Singleton<T> where T: class, new()
{
    private static T instance;
    public T Instance => instance ?? (instance = new T());
}
