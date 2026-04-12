using EventChannel.templates;
using UnityEngine;

namespace EventChannel.concrete
{
    
    [CreateAssetMenu(fileName = "EventChannel", menuName = "Events/EventChannel")]
    public class EmptyEventChannel : EventChannel<Empty> { }
}