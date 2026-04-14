using System.Collections.Generic;
using UnityEditor.Timeline.Actions;
using UnityEngine;
using VContainer;

namespace Inventory
{
    public class Inventory : MonoBehaviour
    {
        public InventoryData inventoryDataTemplate;
        private InventoryData _inventoryData;

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
            var inventoryEntry = _inventoryData.items.Find(entry => Equals(entry.item, item));
            if (inventoryEntry != null)
            {
                inventoryEntry.count++;
                return true;
            }

            if (_inventoryData.items.Count >= _inventoryData.maxInventorySize)
                return false;

            _inventoryData.items.Add(new InventoryEntry(item));
            return true;
        }
    }
}