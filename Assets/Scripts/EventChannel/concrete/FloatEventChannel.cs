using EventChannel.templates;
using UnityEngine;

namespace EventChannel.concrete
{
    [CreateAssetMenu(fileName = "FloatEventChannel", menuName = "Events/FloatEventChannel")]
    public class FloatEventChannel : EventChannel<float> { }
}