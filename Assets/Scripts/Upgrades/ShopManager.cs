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
        [Header("Shop Data")]
        public UpgradeScreenData upgradeScreenData;

        // Player Data
        [Header("Player Data")]
        public HealthData playerHealth;
        public PlanarForceMotorData planarForceMotor;
        public InventoryData playerInventory;

        // UI data
        [Header("UI Data")]
        public TMP_Text upgradeText1;
        public TMP_Text upgradeCostText1;
        public Button upgradeBuyButton1;
        
        public TMP_Text upgradeText2;
        public TMP_Text upgradeCostText2;
        public Button upgradeBuyButton2;

        private void Start()
        {
            RefreshUI();
        }
        private void RefreshUI()
        {
            upgradeBuyButton1.interactable = false;
            upgradeBuyButton2.interactable = false;

            PopulateUpgrade(upgradeText1, upgradeCostText1, upgradeBuyButton1, GetUpgradeByNumber(upgradeScreenData, 1));
            PopulateUpgrade(upgradeText2, upgradeCostText2, upgradeBuyButton2, GetUpgradeByNumber(upgradeScreenData, 2));
            
            // determine if one of the upgrades area already bought and then deactivate them
            DeactivateButton(upgradeBuyButton1, upgradeBuyButton2, GetUpgradeByNumber(upgradeScreenData, 1), GetUpgradeByNumber(upgradeScreenData, 2));
        }


        private void DeactivateButton(Button button1, Button button2, UpgradeEntry upgrade1, UpgradeEntry upgrade2)
        {
            if (upgrade1.purchased || upgrade2.purchased)
            {
                button1.interactable = false;
                button2.interactable = false;
            }
        }

        private UpgradeEntry GetUpgradeByNumber(UpgradeScreenData screenData, int i)
        {
            var upgrades = screenData?.merchantShopData?.upgradeSo?.upgrades;

            return upgrades?.Count >= i ? upgrades[i-1] : null;
        }

        private void PopulateUpgrade(TMP_Text upgradeText, TMP_Text upgradeCostText, Button upgradebutton, UpgradeEntry firstUpgrade)
        {

            if (firstUpgrade == null)
            {
                return;
            }

            if (upgradeText != null)
                upgradeText.text = firstUpgrade.name;
            if (upgradeCostText != null)
                upgradeCostText.text = firstUpgrade.price.ToString();
            if (upgradebutton != null)
            {
                var playerCanAffordUpgrade = playerInventory.CanRemove(firstUpgrade.itemCost, firstUpgrade.price);
                upgradebutton.interactable = playerCanAffordUpgrade;
            }
        }

        public void SellButtonJunk()
        {
            if (upgradeScreenData.merchantShopData.catalogByTier == null)
                return;
            
            foreach (var catalogEntry in upgradeScreenData.merchantShopData.catalogByTier)
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
        public void PurchaseUpgradeButton1()
        {
            if (upgradeScreenData.merchantShopData.upgradeSo == null || upgradeScreenData.merchantShopData.upgradeSo.upgrades == null || upgradeScreenData.merchantShopData.upgradeSo.upgrades.Count == 0) return;

            if (playerInventory == null) return;

            var upgradeToPurchase = GetUpgradeByNumber(upgradeScreenData, 1);
            var price = upgradeToPurchase.price;
            
            if (!playerInventory.CanRemove(upgradeToPurchase.itemCost, price)) return;
            
            planarForceMotor.boostUpgradeEnabled = true;
            upgradeToPurchase.purchased = true;
            playerInventory.Remove(upgradeToPurchase.itemCost, price);
            
            RefreshUI();

        }

        public void PurchaseUpgradeButton2()
        {
            if (upgradeScreenData.merchantShopData.upgradeSo == null || upgradeScreenData.merchantShopData.upgradeSo.upgrades == null || upgradeScreenData.merchantShopData.upgradeSo.upgrades.Count == 0) return;

            if (playerInventory == null) return;

            var upgradeToPurchase = GetUpgradeByNumber(upgradeScreenData, 2);
            var price = upgradeToPurchase.price;
            
            if (!playerInventory.CanRemove(upgradeToPurchase.itemCost, price)) return;
            
            planarForceMotor.boostUpgradeEnabled = true;
            upgradeToPurchase.purchased = true;
            playerInventory.Remove(upgradeToPurchase.itemCost, price);
            
            RefreshUI();

        }

        public bool CanSell(InventoryData inventoryData)
        {
            if (inventoryData == null) return false;

            return upgradeScreenData.merchantShopData.catalogByTier.Any(shopCatalogEntry => inventoryData.CanRemoveByTier(shopCatalogEntry.tierForSale));
        }
    }
}