namespace GodotSpaceGameSketch.Core;

public class Singleton<T> where T : class
{
    protected static T instance;
    public static T Instance => instance;

    public Singleton()
    {
        instance = this as T;
    }
}