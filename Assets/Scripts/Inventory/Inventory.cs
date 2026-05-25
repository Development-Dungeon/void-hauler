using System;
using System.Collections.Generic;
using EventChannel.Audio_events;
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

        public bool AddItem(Item itemToAdd, Vector3? fromLocation, float quantity = 1)
        {
            var added = _inventoryData.Add(itemToAdd.itemType, quantity);
            
            if(added)
                OnItemAdded?.Invoke(new InventoryEventContext(itemToAdd.itemType, quantity, fromLocation));
            return added;
        }
    }
}