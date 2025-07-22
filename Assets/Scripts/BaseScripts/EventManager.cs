using System;
using System.Collections.Generic;

public static class EventManager<T>
{
    private static Dictionary<string, Action<T>> eventTable = new Dictionary<string, Action<T>>();

    public static void Subscribe(string eventName, Action<T> callback)
    {
        if (!eventTable.ContainsKey(eventName))
        {
            eventTable[eventName] = callback;
            return;
        }
        eventTable[eventName] += callback;
    }

    public static void Unsubscribe(string eventName, Action<T> callback)
    {
        if (eventTable.ContainsKey(eventName))
            eventTable[eventName] -= callback;
    }

    public static void Trigger(string eventName, T data)
    {
        if (eventTable.ContainsKey(eventName))
            eventTable[eventName]?.Invoke(data);
    }
}
