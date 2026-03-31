using Attributes;
using EventChannel.templates;
using UnityEngine;

namespace EventChannel.concrete
{
    [CreateAssetMenu(fileName = "HealthEventChannel", menuName = "Events/HealthEventChannel")]
    public class HealthEventChannel : EventChannel<Health> { }
}