using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Inventory
{
    [CreateAssetMenu(fileName = "InventoryData", menuName = "Inventory/InventoryData")]
    public class InventoryData : ScriptableObject
    {
        public List<InventoryEntry> items = new();
        public int maxInventorySize;
        public Action<ItemType, float> OnItemAdded;
        public Action<ItemType, float> OnItemRemoved;

        public bool Remove(InventoryEntry inventoryEntry)
        {
            return inventoryEntry != null && Remove(inventoryEntry.item.itemType, inventoryEntry.count);
        }

        public bool Remove(ItemType itemType, float quantity = 1)
        {
            if (!CanRemove(itemType, quantity))
                return false;
            
            var inventoryEntry = items.Find(entry => entry.item.itemType.Equals(itemType));

            if (inventoryEntry == null)
                return false;
           
            inventoryEntry.count -= quantity;

            if (inventoryEntry.count == 0)
                items.Remove(inventoryEntry);
            
            OnItemRemoved?.Invoke(itemType, quantity);

            return true;
        }

        public bool CanRemoveByTier(ItemType tierType, float quantity = 1)
        {
            var itemsOfTier = items.Where(entry => tierType.GetType().IsAssignableFrom(entry.item.itemType.GetType())).ToList();

            if (!itemsOfTier.Any()) return false;
            
            var totalItemsOfTier = itemsOfTier.Sum(entry => entry.count);

            return totalItemsOfTier >= quantity;
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
            
            OnItemAdded?.Invoke(itemType, count);
            
            return true;

        }

        public InventoryEntry FindByTier(ItemType tier)
        {
            return items.Find(entry => tier.GetType().IsAssignableFrom(entry.item.itemType.GetType()));
        }

        public List<InventoryEntry> FindAllByTier(ItemType tier)
        {
            return items.FindAll(entry => tier.GetType().IsAssignableFrom(entry.item.itemType.GetType()));
        }

        public List<InventoryEntry> FindAll(ItemType itemToFilterBy)
        {
            return items.FindAll(entry => entry.item.itemType.Equals(itemToFilterBy));
        }
    }
}