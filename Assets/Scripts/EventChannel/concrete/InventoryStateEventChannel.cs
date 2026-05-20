using EventChannel.templates;
using UnityEngine;

namespace EventChannel.concrete
{
    [CreateAssetMenu(fileName = "InventoryStateEventChannel", menuName = "Events/InventoryStateEventChannel")]
    public class InventoryStateEventChannel: EventChannel<InventoryEventContext> { }
}