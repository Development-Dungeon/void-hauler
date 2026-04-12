using System;

namespace Inventory
{
    [Serializable]
    public class InventoryEntry
    {
        public Item item;
        public float count;
        public InventoryEntry(Item item, float count = 1)
        {
            this.item = item;
            this.count = count;
        }
    }
}