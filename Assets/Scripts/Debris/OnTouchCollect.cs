using System;
using System.Collections.Generic;
using Inventory;
using UnityEngine;

namespace Debris
{
    public class OnTouchCollect : MonoBehaviour
    {
        public List<InventoryEntry> inventoryEntries;
        public bool destroyOnTouch;

        public void SetQuantity(int quantity)
        {
            if (inventoryEntries == null) return;
            inventoryEntries.ForEach(entry => entry.count = quantity);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (inventoryEntries == null || inventoryEntries.Count == 0)
                return;

            var inventory = other.GetComponentInParent<Inventory.Inventory>();
            if (inventory == null)
                return;

            inventoryEntries.RemoveAll(entry => inventory.AddItem(entry.item, gameObject.transform.position, entry.count));

            if (inventoryEntries.Count == 0 && destroyOnTouch)
                Destroy(gameObject);
        }
    }
}