using UnityEngine;
using UnityEngine.Events;

namespace EventChannel
{
    public abstract class EventListener<T> : MonoBehaviour
    {
        [SerializeField] private UnityEvent<T> unityEvent;
        [SerializeField] private EventChannel<T> channel;
    
        private void Awake() => channel?.Register(this);
        private void OnDestroy() => channel?.Deregister(this);
        public void Raise(T value) => unityEvent?.Invoke(value);
    }
}
