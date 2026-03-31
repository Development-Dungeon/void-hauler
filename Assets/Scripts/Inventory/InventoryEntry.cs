using System;

namespace Inventory
{
    [Serializable]
    public class InventoryEntry
    {
        public Item item;
        public int count;
        public InventoryEntry(Item item, int count = 1)
        {
            this.item = item;
            this.count = count;
        }
    }
}