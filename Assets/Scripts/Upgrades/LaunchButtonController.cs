using System;
using Inventory;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Utility;

namespace Upgrades
{
    [Serializable]
    public class LaunchCost
    {
        public ItemType item;
        public float cost;
    }

    public class LaunchButtonController : MonoBehaviour
    {
        [SerializeField] public LaunchCost launchCost;
        public InventoryData playerInventory;
        [Get] public Button launchButton;
    
        [Header("On Successful Payment")]
        public UnityEvent onLaunch;

        private void Awake()
        {
            launchButton.interactable = playerInventory.CanRemove(launchCost.item, launchCost.cost);
            playerInventory.OnItemAdded += OnItemAdded;
            playerInventory.OnItemRemoved += OnItemRemoved;
        }
    
        public void Launch()
        {
            if (!playerInventory.CanRemove(launchCost.item, launchCost.cost)) return;
        
            playerInventory.Remove(launchCost.item, launchCost.cost);
        
            onLaunch.Invoke();
            
        }

        private void OnItemRemoved(ItemType arg1, float arg2)
        {
            launchButton.interactable = playerInventory.CanRemove(launchCost.item, launchCost.cost);
        }

        private void OnItemAdded(ItemType arg1, float arg2)
        {
            launchButton.interactable = playerInventory.CanRemove(launchCost.item, launchCost.cost);
        }

        private void OnDestroy()
        {
            playerInventory.OnItemAdded -= OnItemAdded;
            playerInventory.OnItemRemoved -= OnItemRemoved;
        }
    }
}