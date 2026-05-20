using System;
using System.Collections.Generic;
using UnityEditor.Timeline.Actions;
using UnityEngine;
using VContainer;

namespace Inventory
{
    public class InventoryEventContext
    {
        public ItemType item;
        public float quantity;
        public Vector3? position;

        public InventoryEventContext(ItemType item, float quantity, Vector3? position)
        {
            this.item = item;
            this.quantity = quantity;
            this.position = position;
        }
    }
    public class Inventory : MonoBehaviour
    {
        public InventoryData inventoryDataTemplate;
        private InventoryData _inventoryData;
        
        public event Action<InventoryEventContext> OnItemAdded;

        [Inject]
        public void Construct(IObjectResolver resolver)
        {
            _inventoryData = resolver.ResolveOrDefault<InventoryData>();
        }
        
        private void Awake()
        {
            if(_inventoryData == null)
                _inventoryData = Instantiate(inventoryDataTemplate);
        }

        public bool AddItem(Item item, float quantity = 1)
        {
            AudioEvents.RequestSound(
                AudioEvent.ItemPickup,
                transform.position);

            var added = _inventoryData.Add(item.itemType, quantity);
            
            if(added)
                OnItemAdded?.Invoke(new InventoryEventContext(item.itemType, quantity, null));
            return added;
        }

        public bool AddItem(Item entryItem, float entryCount, Vector3 fromLocation)
        {
            var added = AddItem(entryItem, entryCount);
            if(added)
                OnItemAdded?.Invoke(new InventoryEventContext(entryItem.itemType, entryCount, fromLocation));
            return added;
        }
    }
}