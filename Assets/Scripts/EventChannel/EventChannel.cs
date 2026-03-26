using System.Collections.Generic;
using UnityEngine;

namespace EventChannel
{
    public abstract class EventChannel<T> : ScriptableObject
    {
        private readonly HashSet<EventListener<T>> _observers = new();

        public void Invoke(T value)
        {
            foreach (var listener  in _observers)
            {
                listener.Raise(value); 
            }
        }

        public void Register(EventListener<T> listener) => _observers.Add(listener);
        public void Deregister(EventListener<T> listener) => _observers.Remove(listener);
    }


    public readonly struct Empty { }

    [CreateAssetMenu(fileName = "EventChannel", menuName = "Events/EventChannel")]
    public class EventChannel : EventChannel<Empty>
    {
    }
}