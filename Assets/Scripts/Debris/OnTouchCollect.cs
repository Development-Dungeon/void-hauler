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

        private void OnTriggerEnter(Collider other)
        {
            if (inventoryEntries == null || inventoryEntries.Count == 0)
                return;

            var inventory = other.GetComponentInParent<Inventory.Inventory>();
            if (inventory == null)
                return;

            inventoryEntries.RemoveAll(entry =>
            {
                var added = inventory.AddItem(entry.item, entry.count);
                if (added)
                    WorldPopUpController.Instance.AddEvent(entry.item, transform.position);


                return added;
            });

            if (inventoryEntries.Count == 0 && destroyOnTouch)
                Destroy(gameObject);
        }
    }
}