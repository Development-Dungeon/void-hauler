using System.Collections.Generic;
using UnityEngine;

namespace Inventory
{
    [CreateAssetMenu(fileName = "InventoryData", menuName = "Inventory/InventoryData")]
    public class InventoryData : ScriptableObject
    {
        public List<InventoryEntry> items = new();
        public int maxInventorySize;

        public bool Remove(ItemType itemType, float quantity = 1)
        {
            if (!CanRemove(itemType, quantity))
                return false;
            
            var inventoryEntry = items.Find(entry => entry.item.itemType.Equals(itemType));

            if (inventoryEntry == null)
                return false;
           
            inventoryEntry.count -= quantity;
            
            if(inventoryEntry.count == 0)
                return items.Remove(inventoryEntry);

            return true;
        }

        public bool CanRemove(ItemType itemType, float quantity)
        {
            var inventoryEntry = items.Find(entry => entry.item.itemType.Equals(itemType));

            if (inventoryEntry == null)
                return false;

            if (inventoryEntry.count < quantity)
                return false;

            return true;
        }

        public bool CanAddItem(ItemType itemType, float quantity = 1)
        {
            var inventoryEntry = items.Find(entry => entry.item.itemType.Equals(itemType));

            if (inventoryEntry != null)
                return true;
            
            return items.Count < maxInventorySize;
        }

        public bool Add(ItemType itemType, float count)
        {
            if (!CanAddItem(itemType, count))
                return false;
            
            var inventoryEntry = items.Find(entry => entry.item.itemType.Equals(itemType));
            
            if(inventoryEntry == null) 
                items.Add(new InventoryEntry(new Item(itemType), count ));
            else
                inventoryEntry.count += count;
            
            return true;

        }
    }
}