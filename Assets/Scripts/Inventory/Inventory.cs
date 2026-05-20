using System;
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
        
        public event Action<GameObject> OnItemAdded;

        [Inject]
        public void Construct(IObjectResolver resolver)
        {
            _inventoryData = resolver.ResolveOrDefault<InventoryData>();
        }
        
        private void Awake()
        {
            if(_inventoryData == null)
                _inventoryData = Instantiate(inventoryDataTemplate);
            
            _inventoryData.OnItemAdded += (item, q) => OnItemAdded?.Invoke(gameObject);
        }

        private void OnDestroy()
        {
            _inventoryData.OnItemAdded -= (item, q) => OnItemAdded?.Invoke(gameObject);
        }

        public bool AddItem(Item item, float quantity = 1)
        {
            var added =_inventoryData.Add(item.itemType, quantity);
            
            if(added) 
                OnItemAdded?.Invoke(gameObject);
            
            return added;
            // var inventoryEntry = _inventoryData.items.Find(entry => Equals(entry.item, item));
            // if (inventoryEntry != null)
            // {
            //     inventoryEntry.count++;
            //     return true;
            // }
            //
            // if (_inventoryData.items.Count >= _inventoryData.maxInventorySize)
            //     return false;
            //
            // _inventoryData.items.Add(new InventoryEntry(item));
            // return true;
        }
    }
}