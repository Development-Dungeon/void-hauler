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
        public InventoryData playerInventory;
        
        // Upgrade UI
        [Header("Upgrade Controllers")]
        public List<UpgradeUIController> upgradeUIController = new ();

        // UI data
        public TMP_Text merchantName;
        public TMP_Text merchantDescription;
        public Image merchantImage;


        private void Start()
        {
            // populate the UI controllers with the upgrade information
            upgradeUIController[0].Init(GetUpgradeByNumber(upgradeScreenData, 1) );
            upgradeUIController[1].Init(GetUpgradeByNumber(upgradeScreenData, 2) );
            
            RefreshUI();
            
        }
        private void RefreshUI()
        {
            var anyAlreadyPurchased = upgradeUIController.Any(u => u.upgradeEntry.purchased);
            foreach (var uiController in upgradeUIController)
               uiController.RefreshUI(CanBuy(uiController.upgradeEntry), anyAlreadyPurchased); 
            merchantName.text = upgradeScreenData.merchantShopData.merchantName;
            merchantDescription.text = upgradeScreenData.merchantShopData.merchantDescription;
            merchantImage.sprite = upgradeScreenData.merchantShopData.merchantIcon;

        }

        private UpgradeEntry GetUpgradeByNumber(UpgradeScreenData screenData, int i)
        {
            var upgrades = screenData?.merchantShopData?.upgradeSoSo?.upgrades;

            return upgrades?.Count >= i ? upgrades[i-1] : null;
        }

        public bool CanBuy(UpgradeEntry upgrade)
        {
            return playerInventory.CanRemove(upgrade.itemCost, upgrade.price);
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
        
        public void PurchaseUpgrade(UpgradeUIController _upgradeUIController)
        {
            // get the upgrade and search my list for what upgrade is being purchased?
            var upgradeToPurchase = _upgradeUIController?.upgradeEntry;

            if (upgradeToPurchase == null) return;

            if (playerInventory == null) return;

            var price = upgradeToPurchase.price;
            
            if (!playerInventory.CanRemove(upgradeToPurchase.itemCost, price)) return;
            
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