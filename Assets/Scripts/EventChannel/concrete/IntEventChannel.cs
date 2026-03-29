using EventChannel.templates;
using UnityEngine;

namespace EventChannel.concrete
{
    [CreateAssetMenu(fileName = "IntEventChannel", menuName = "Events/IntEventChannel")]
    public class IntEventChannel : EventChannel<int> { }
}