using System.Collections.Generic;
using UnityEngine;

namespace Inventory
{
    public class Inventory : MonoBehaviour
    {
        public List<InventoryEntry> items;
        public int maxInventorySize = 5;

        private void Awake()
        {
            items = new List<InventoryEntry>(maxInventorySize);
        }

        public bool AddItem(Item item)
        {
            var inventoryEntry = items.Find(entry => Equals(entry.item, item));
            if (inventoryEntry != null)
            {
                inventoryEntry.count++;
                return true;
            }

            if (items.Count >= maxInventorySize)
                return false;

            items.Add(new InventoryEntry(item));
            return true;
        }
    }
}