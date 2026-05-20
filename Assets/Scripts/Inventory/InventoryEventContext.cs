using Inventory;
using UnityEngine;

namespace EventChannel.concrete
{
    public class InventoryEventContext
    {
        public Vector3 position;
        public ItemType itemType;
        public float quantity;

        public InventoryEventContext(Vector3 position, ItemType itemType, float quantity)
        {
            this.position = position;
            this.itemType = itemType;
            this.quantity = quantity;
        }
    }
}