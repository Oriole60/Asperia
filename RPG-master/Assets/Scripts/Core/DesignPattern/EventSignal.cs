using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Event Signal", menuName = "Game Event Signal", order = 52)]
public class EventSignal : ScriptableObject
{
    private List<EventListener> elisteners = new List<EventListener>();

    public void Register(EventListener listener)
    {
        elisteners.Add(listener);
    }

    public void Unregister(EventListener listener)
    {
        elisteners.Remove(listener);
    }

    public void Occurred(GameObject go)
    {
        for (int i = 0; i < elisteners.Count; i++)
        {
            elisteners[i].OnEventOccurs(go);
        }
    }
}
