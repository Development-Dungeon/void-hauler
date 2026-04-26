using System;
using System.ComponentModel;
using Attributes;
using Inventory;
using TMPro;
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
        [Description ("cost per fuel")] // TODO confirm that this is working
        public float cost;
    }

    public class LaunchButtonController : MonoBehaviour
    {
        // [SerializeField] public LaunchCost launchCost;
        public UpgradeScreenData upgradeScreenData;
        public InventoryData playerInventory;
        public FuelData playerFuel;
        
        [Header("Launch UI")]
        [Get] public Button launchButton;
        public TMP_Text launchTextObject;
        public string launchTextTemplate;
        
        [Header("On Successful Payment")]
        public UnityEvent onLaunch;

        private float _fuelCost;

        private void Awake()
        {
            playerInventory.OnItemAdded += OnItemAdded;
            playerInventory.OnItemRemoved += OnItemRemoved;
            // set the text based on how much fuel the player needs?
            _fuelCost = CalculateLaunchCharge();
            launchButton.interactable = playerInventory.CanRemove(upgradeScreenData.launchCost.item, _fuelCost);
            launchTextObject.text = _fuelCost.ToString(launchTextTemplate);
        }

        private float CalculateLaunchCharge()
        {
            var chargePerFuel = upgradeScreenData.launchCost.cost;

            var fuelAmountToFull = playerFuel.GetFuelToFill();

            var totalCharge = chargePerFuel * fuelAmountToFull;

            return totalCharge;
        }

        public void Launch()
        {
            if (!playerInventory.CanRemove(upgradeScreenData.launchCost.item, _fuelCost)) return;
        
            playerInventory.Remove(upgradeScreenData.launchCost.item, _fuelCost);
            
            playerFuel.currentFuel = playerFuel.maxFuel;
            
            onLaunch.Invoke();
            
        }

        private void OnItemRemoved(ItemType arg1, float arg2)
        {
            launchButton.interactable = playerInventory.CanRemove(upgradeScreenData.launchCost.item, _fuelCost);
        }

        private void OnItemAdded(ItemType arg1, float arg2)
        {
            launchButton.interactable = playerInventory.CanRemove(upgradeScreenData.launchCost.item, _fuelCost);
        }

        private void OnDestroy()
        {
            playerInventory.OnItemAdded -= OnItemAdded;
            playerInventory.OnItemRemoved -= OnItemRemoved;
        }
    }
}