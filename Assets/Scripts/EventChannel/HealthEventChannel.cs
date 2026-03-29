using UnityEngine;

namespace EventChannel
{
    [CreateAssetMenu(fileName = "HealthEventChannel", menuName = "Events/HealthEventChannel")]
    public class HealthEventChannel : EventChannel<Health> { }
}