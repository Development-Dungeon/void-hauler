using System.Collections.Generic;
using Inventory;
using UnityEngine;

namespace Debris
{
    public class OnTouchCollect : MonoBehaviour
    {
        public List<InventoryEntry> inventoryEntries;
        public bool destroyOnTouch;
        private void OnTriggerEnter(Collider other)
        {
            if ( inventoryEntries == null || inventoryEntries.Count == 0 )
                return;
            
            var inventory = other.GetComponentInParent<Inventory.Inventory>();
            if (inventory == null)
                return;

            inventoryEntries.RemoveAll(entry => inventory.AddItem(entry.item, entry.count));

            if (inventoryEntries.Count == 0 && destroyOnTouch)
                Destroy(gameObject);

        }
    }
}