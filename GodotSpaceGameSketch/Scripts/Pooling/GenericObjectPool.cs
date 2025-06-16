using System;
using System.Collections.Generic;
using Godot;
using GodotSpaceGameSketch.Pooling.Flyweights.Base;

namespace GodotSpaceGameSketch.Pooling;

public class GenericObjectPool<T> where T: class, IFlyweightView
{
    private readonly Stack<T> stack = new();

    private readonly Node objectsParent;
    private readonly Func<T> createFunc;
    private readonly Action<T> actionOnGet;
    private readonly Action<T> actionOnRelease;
    private readonly Action<T> actionOnDestroy;

    public GenericObjectPool(
        Node objectsParent,
        Func<T> createFunc,
        Action<T> actionOnGet,
        Action<T> actionOnRelease,
        Action<T> actionOnDestroy)
    {
        this.objectsParent = objectsParent;
        this.createFunc = createFunc;
        this.actionOnGet = actionOnGet;
        this.actionOnRelease = actionOnRelease;
        this.actionOnDestroy = actionOnDestroy;
    }
    
    public T GetItem(Vector3 position, Quaternion rotation)
    {
        T obj = null;
        if (stack.Count == 0)
        {
            obj = CreateItem();
            objectsParent.AddChild(obj.Node);
        }
        else
        {
            obj = stack.Pop();
        }
        
        obj.Node.Position = position;
        obj.Node.Quaternion = rotation;
        actionOnGet?.Invoke(obj);
        obj.Show();
        obj.OnTakeFromPool();

        return obj;
    }

    public void Recycle(T obj)
    {
        obj.Hide();
        actionOnRelease?.Invoke(obj);
        stack.Push(obj);
    }
        
    private T CreateItem() => createFunc?.Invoke();
}