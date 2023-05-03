using System;
using System.Collections.Generic;
using UnityEngine;

public interface ITypeEventSystem
{
    void Send<T>() where T : new();
    void Send<T>(T e);
    IUnRegister Register<T>(Action<T> onEvent);
    void UnRegister<T>(Action<T> onEvent);
}

public interface IUnRegister
{
    void UnRegister();
}

public class TypeEventSystemUnRegister<T> : IUnRegister
{
    public ITypeEventSystem typeEventSystem;
    public Action<T> onEvent;
    public void UnRegister()
    {
        typeEventSystem.UnRegister<T>(onEvent);

        typeEventSystem = null;

        onEvent = null;
    }
}

public class UnRegisterWhenDestroy : MonoBehaviour
{
    private HashSet<IUnRegister> UnRegisterList = new HashSet<IUnRegister>();

    public void AddUnRegister(IUnRegister unRegister)
    {
        UnRegisterList.Add(unRegister);
    }

    private void OnDestroy()
    {
        foreach (var unRegister in UnRegisterList)
        {
            unRegister.UnRegister();
        }
    }
}

public static class UnRegisterExtention
{
    public static void UnRegisterWhenGameObjectDestroy(this IUnRegister self, GameObject gameObject)
    {
        var trigger = gameObject.GetComponent<UnRegisterWhenDestroy>();

        if (!trigger)
        {
            trigger = gameObject.AddComponent<UnRegisterWhenDestroy>();
        }
        
        trigger.AddUnRegister(self);
        
        
    }
}

public class TypeEventSystem : ITypeEventSystem
{
    public interface IRegistrations
    {
        
    }
 
    public class Registrations<T> : IRegistrations
    {
        public Action<T> onEvent = e => { };
    }

    private Dictionary<Type, IRegistrations> mEventRegistrations = new Dictionary<Type, IRegistrations>();

    public void Send<T>() where T : new()
    {
        var e = new T();
        Send(e);
    }

    public void Send<T>(T e)
    {
        var type = typeof(T);
        IRegistrations registrations;
        if (mEventRegistrations.TryGetValue(type, out registrations))
        {
            (registrations as Registrations<T>).onEvent(e);
        }
    }

    public IUnRegister Register<T>(Action<T> onEvent)
    {
        var type = typeof(T);
        IRegistrations registrations;
        if (mEventRegistrations.TryGetValue(type, out registrations))
        {
            
        }
        else
        {
            registrations = new Registrations<T>();
            mEventRegistrations.Add(type, registrations);
        }

        (registrations as Registrations<T>).onEvent += onEvent;

        return new TypeEventSystemUnRegister<T>()
        {
            onEvent = onEvent,
            typeEventSystem = this
        };
    }

    public void UnRegister<T>(Action<T> onEvent)
    {
        var type = typeof(T);
        IRegistrations registrations;
        if (mEventRegistrations.TryGetValue(type, out registrations))
        {
            (registrations as Registrations<T>).onEvent -= onEvent;
        }
    }
}
