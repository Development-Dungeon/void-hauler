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
        public List<ShopCatalogEntry> catalogByTier = new();
        public Upgrades upgradeSo;

        // Player Data
        public HealthData playerHealth;
        public PlanarForceMotorData planarForceMotor;
        public InventoryData playerInventory;

        // UI data
        public TMP_Text upgradeText;
        public TMP_Text upgradeCostText;
        public Button upgradeBuyButton;

        private void Start()
        {
            RefreshUI();
        }

        private void RefreshUI()
        {
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
            if (catalogByTier == null)
                return;
            
            foreach (var catalogEntry in catalogByTier)
            {
                // if the player does have the item, check how many the player has and if we can remove them all
                var inventoryEntries = playerInventory.FindAllByTier(catalogEntry.tierForSale);
                
                if(inventoryEntries.Count == 0) continue;
                
                foreach (var inventoryEntry in inventoryEntries)
                {
                   if(!playerInventory.CanRemove(inventoryEntry.item.itemType, inventoryEntry.count)) continue;
                   
                   var totalPrice = inventoryEntry.count * catalogEntry.salePrice;
                   
                   if(!playerInventory.CanAddItem(catalogEntry.saleItemType, totalPrice)) continue;
                   
                   playerInventory.Remove(inventoryEntry);
                   playerInventory.Add(catalogEntry.saleItemType, totalPrice);
                   
                }

            }
            RefreshUI();
            
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

        public bool CanSell(InventoryData inventoryData)
        {
            if (inventoryData == null) return false;

            return catalogByTier.Any(shopCatalogEntry => inventoryData.CanRemoveByTier(shopCatalogEntry.tierForSale));
        }
    }
}