using System;
using System.Collections.Generic;
using System.Linq;
using Attributes;
using Inventory;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Upgrades
{
    [Serializable]
    public class ShopCatalogEntry
    {
        public ItemType tierForSale;
        public float salePrice;
        public ItemType saleItemType;
    }

    public class ShopManager : MonoBehaviour
    {
        public List<ShopCatalogEntry> catalog = new();
        public Upgrades upgradeSo;

        // Player Data
        public HealthData playerHealth;
        public PlanarForceMotorData planarForceMotor;
        public InventoryData playerInventory;

        // UI data
        public Button sellJunkButton;
        public TMP_Text upgradeText;
        public TMP_Text upgradeCostText;
        public Button upgradeBuyButton;

        private void Start()
        {
            RefreshUI();
        }

        private void RefreshUI()
        {
            sellJunkButton.interactable = false;
            upgradeBuyButton.interactable = false;

            SetUpgrade();
        }

        private void SetUpgrade()
        {
            if (upgradeSo == null || upgradeSo.upgrades == null || upgradeSo.upgrades.Count <= 0)
                return;

            var firstUpgrade = upgradeSo.upgrades.First();

            if (upgradeText != null)
                upgradeText.text = firstUpgrade.name;
            if (upgradeCostText != null)
                upgradeCostText.text = firstUpgrade.price.ToString();
            if (upgradeBuyButton != null)
            {
                var playerCanAffordUpgrade = playerInventory.CanRemove(firstUpgrade.itemCost, firstUpgrade.price);
                upgradeBuyButton.interactable = playerCanAffordUpgrade && !firstUpgrade.purchased;
            }
        }

        public void SellButtonJunk()
        {
            Debug.Log("inside sell button");
            if (catalog == null)
                return;
            
            foreach (var catalogEntry in catalog)
            {
                // check if the player has the item
                if (!playerInventory.CanRemove(catalogEntry.tierForSale, 1)) continue;
                
                // if the player does have the item, check how many the player has and if we can remove them all
                var playerEntry = playerInventory.Find(catalogEntry.tierForSale);
                if(!playerInventory.CanRemove(catalogEntry.tierForSale, playerEntry.count)) continue;
                
                // check if i can add money equivalent to the number sold times the price
                var itemsToAdd = catalogEntry.salePrice * playerEntry.count;
                if (!playerInventory.CanAddItem(catalogEntry.saleItemType, itemsToAdd)) continue;
                
                // if everything is good then perform the add and remove
                if (playerInventory.Remove(catalogEntry.tierForSale, playerEntry.count))
                    playerInventory.Add(catalogEntry.saleItemType, itemsToAdd);

                RefreshUI();
                
            }
            
        }

        public void PurchaseUpgradeButton()
        {
            if (upgradeSo == null || upgradeSo.upgrades == null || upgradeSo.upgrades.Count == 0) return;

            if (playerInventory == null) return;

            var firstUpgrade = upgradeSo.upgrades.First();
            var price = firstUpgrade.price;
            
            if (!playerInventory.CanRemove(firstUpgrade.itemCost, price)) return;
            
            planarForceMotor.boostUpgradeEnabled = true;
            firstUpgrade.purchased = true;
            playerInventory.Remove(firstUpgrade.itemCost, price);
            
            RefreshUI();

        }
    }
}