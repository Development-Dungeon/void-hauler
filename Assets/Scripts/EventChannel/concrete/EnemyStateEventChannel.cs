using Enemy;
using EventChannel.templates;
using UnityEngine;

namespace EventChannel.concrete
{
    [CreateAssetMenu(fileName = "EnemyStateEventChannel", menuName = "Events/EnemyStateEventChannel")]
    public class EnemyStateEventChannel: EventChannel<EnemyStateContext> { }
}
