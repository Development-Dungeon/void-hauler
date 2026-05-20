using System;
using EventChannel.concrete;
using UnityEngine;
using VContainer;

namespace Inventory
{
    public class Inventory : MonoBehaviour
    {
        public InventoryData inventoryDataTemplate;
        private InventoryData _inventoryData;
        public event Action<Inventory, ItemType, float> OnItemAdded;
        public event Action<Inventory> OnItemRemoved;
        public InventoryStateEventChannel OnInventoryAddItemChanged;
        
        [Inject]
        public void Construct(IObjectResolver resolver)
        {
            _inventoryData = resolver.ResolveOrDefault<InventoryData>();
        }
        
        private void Awake()
        {
            if(_inventoryData == null)
                _inventoryData = Instantiate(inventoryDataTemplate);
            _inventoryData.OnItemAdded += OnItemAddedCallback;
            _inventoryData.OnItemRemoved += OnItemRemovedCallback;
        }

        private void OnDestroy()
        {
            _inventoryData.OnItemAdded -= OnItemAddedCallback;
            _inventoryData.OnItemRemoved -= OnItemRemovedCallback;
        }

        private void OnItemRemovedCallback(ItemType arg1, float arg2)
        {
            OnItemRemoved?.Invoke(this);
        }

        private void OnItemAddedCallback(ItemType itemType, float quantity)
        {
            OnInventoryAddItemChanged?.Invoke(new InventoryEventContext(transform.position, itemType, quantity));
            // OnItemAdded?.Invoke(this, itemType, quantity);
        }

        public bool AddItem(Item item, float quantity = 1)
        {
            var inventoryEntry = _inventoryData.items.Find(entry => Equals(entry.item, item));
            if (inventoryEntry != null)
            {
                _inventoryData.Add(item.itemType, quantity);
                // inventoryEntry.count++;
                return true;
            }

            if (_inventoryData.items.Count >= _inventoryData.maxInventorySize)
                return false;

            _inventoryData.Add(item.itemType, quantity);
            // _inventoryData.items.Add(new InventoryEntry(item, quantity));
            return true;
        }
    }
}